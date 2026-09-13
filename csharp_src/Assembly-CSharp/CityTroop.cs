using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XLua;

public class CityTroop : MonoBehaviour, ITouchPickable, ITouchObjectBeginLongTabHandler, ITouchObject, ITouchObjectEndLongTabHandler, ITouchObjectBeginDragHandler, ITouchObjectEndDragHandler, ITouchObjectDragHandler
{
	private struct WorkManMoveTempStruct
	{
		public Vector3 pos;

		public Quaternion quar;
	}

	[SerializeField]
	private Transform _modelGo;

	private const float EdgeRateX = 0.08f;

	private const float EdgeRateY = 0.1f;

	private const float ShowArrow = 1f;

	private const float NoShowArrow = 0f;

	private const float DefaultRadius = 1.3f;

	private const float RadiusAdd = 0.3f;

	private const float PickGarbageMoveDistance = 6f;

	private const float MoveGatherTime = 1f;

	private const float SpreadTime = 1f;

	private const float PickGarbageRadius = 2f;

	private const float OpenFogMoveDistance = 6.3f;

	private InstanceRequest _moveTroopLineInst;

	private WorldTroopLine _moveTroopLine;

	private InstanceRequest _moveEndPosArrowInst;

	private WorldTroopDestinationSignal _moveEndPosArrow;

	private InstanceRequest _dragTroopLineInst;

	private WorldTroopLine _dragTroopLine;

	private InstanceRequest _troopDestinationInst;

	private WorldTroopDestinationSignal _troopDestination;

	private CityTroopMachine _cityTroopMachine;

	private List<CityWorkMan> _allWorkMan;

	private List<InstanceRequest> _allWorkManReq;

	private const string modelPath = "Model";

	private InstanceRequest requestInst;

	private Transform truckTransform;

	private Vector3 StartPos;

	private bool _isFollow;

	private bool _isDrag;

	private SimpleAnimation[] anims;

	private GPUSkinningAnimator[] gpuAnims;

	private bool isTruck;

	private bool needMoveAfterCreate;

	private int targetPos;

	public const string City_Troop_Anim_Idle = "idle";

	public const string City_Troop_Anim_Run = "run";

	private List<CityTroopUnit> troopUnits = new List<CityTroopUnit>();

	private static readonly Vector3[] CityGarbageBirthPos = new Vector3[5]
	{
		new Vector3(0f, 0f, 3f),
		new Vector3(2.1f, 0f, 0.4f),
		new Vector3(1.5f, 0f, 1.65f),
		new Vector3(-1.5f, 0f, 1.65f),
		new Vector3(-2.1f, 0f, 0.4f)
	};

	private static readonly Vector3[] CityWorkManGuideBirthPos = new Vector3[4]
	{
		new Vector3(95.51f, 0f, 96.16f),
		new Vector3(95.21f, 0f, 94.31f),
		new Vector3(93.42f, 0f, 92.98f),
		new Vector3(91.52f, 0f, 94.2f)
	};

	private static readonly Quaternion[] CityWorkManGuideBirthRotation = new Quaternion[4]
	{
		Quaternion.Euler(new Vector3(0f, 246f, 0f)),
		Quaternion.Euler(new Vector3(0f, 320f, 0f)),
		Quaternion.Euler(new Vector3(0f, 2.917f, 0f)),
		Quaternion.Euler(new Vector3(0f, 52.6f, 0f))
	};

	public WorldPreviewType PreviewType => WorldPreviewType.Default;

	public Vector3 EndPos { get; set; }

	public Vector3 TmpEndPos { get; set; }

	public int OpenFogPointIndex { get; set; }

	public float Priority { get; }

	public Vector2Int TilePos => SceneManager.World.WorldToTile(base.transform.position);

	public void Init()
	{
		GameEntry.Event.Subscribe(EventId.CityTroopMove, CityTroopMoveSignal);
		GameEntry.Event.Subscribe(EventId.CameraFollowCityTroop, CameraFollowCityTroopSignal);
		GameEntry.Event.Subscribe(EventId.OnWorldInputPointDown, OnWorldInputPointDownSignal);
		GameEntry.Event.Subscribe(EventId.RefreshGuide, RefreshGuideSignal);
		GameEntry.Event.Subscribe(EventId.RefreshCityTroopPeopleNum, RefreshCityTroopPeopleNumSignal);
		if (GameEntry.Data.Building.GetBuildingDataByBuildId(427000) != null)
		{
			isTruck = true;
		}
		else
		{
			isTruck = false;
		}
		if (!isTruck)
		{
			_allWorkManReq = new List<InstanceRequest>();
			_allWorkMan = new List<CityWorkMan>();
			LoadWorkMan();
		}
		else
		{
			LoadTruck();
		}
		TmpEndPos = new Vector3(-1f, -1f, -1f);
		_cityTroopMachine = new CityTroopMachine(this);
		_isFollow = InitFollowCamera();
	}

	public void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.CityTroopMove, CityTroopMoveSignal);
		GameEntry.Event.Unsubscribe(EventId.CameraFollowCityTroop, CameraFollowCityTroopSignal);
		GameEntry.Event.Unsubscribe(EventId.OnWorldInputPointDown, OnWorldInputPointDownSignal);
		GameEntry.Event.Unsubscribe(EventId.RefreshGuide, RefreshGuideSignal);
		GameEntry.Event.Unsubscribe(EventId.RefreshCityTroopPeopleNum, RefreshCityTroopPeopleNumSignal);
		_cityTroopMachine.UnInit();
		DestroyTruck();
		DestroyWorkMan();
		DestroyDragLine();
		DestroyMoveLine();
		DestroyMoveEndArrow();
		DestroyTroopDestination();
		GameEntry.Event.Fire(EventId.HideCityTroopHead);
	}

	public PointInfo GetPointInfo()
	{
		return null;
	}

	public bool OnDrag(Vector3 dragStartPos, Vector3 dragCurrPos)
	{
		if (!_isDrag)
		{
			_isDrag = true;
			GetCurState()?.OnBeginDrag();
		}
		return true;
	}

	public bool OnBeginDrag(Vector3 dragStartPos)
	{
		if (!_isDrag)
		{
			_isDrag = true;
			GetCurState()?.OnBeginDrag();
		}
		return true;
	}

	public void OnCreateDragLine()
	{
		CreateDragLine();
		CreateTroopDestination();
	}

	public void OnDragLineUpdate(Vector3 pos)
	{
		EdgeDragUpdate(pos);
		RefreshLineLength(pos);
		RefreshLineDestination(pos);
	}

	public bool DealGarbageQueueDataWhenDragEnd(Vector3 pos)
	{
		int index = SceneManager.World.WorldToTileIndex(EndPos);
		int pointType = SceneManager.World.GetPointType(index);
		int num = SceneManager.World.WorldToTileIndex(pos);
		int pointType2 = SceneManager.World.GetPointType(num);
		if (pointType2 == 1)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.AddGuideGarbageCollectToQueue", num);
		}
		else
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveFromGarbageQueue", -1);
		}
		if (pointType == 1)
		{
			return pointType2 == 1;
		}
		return false;
	}

	public void OnDragLineStop()
	{
		DestroyDragLine();
		DestroyTroopDestination();
	}

	public void OnCreateMoveLine()
	{
		CreateMoveLine();
		CreateMoveEndArrow();
		Vector3 realEndPos = GetRealEndPos();
		RefreshMoveArrowPosition(realEndPos);
	}

	public bool DoWhenUseTmpEndPos()
	{
		if (TmpEndPos.x >= 0f)
		{
			EndPos = TmpEndPos;
			TmpEndPos = new Vector3(-1f, -1f, -1f);
			return true;
		}
		return false;
	}

	private void OnCreateTmpMoveLine()
	{
		if (!(TmpEndPos.x < 0f))
		{
			CreateMoveLine(useTmpPoint: true);
			CreateMoveEndArrow(useTmpPoint: true);
		}
	}

	public void DoWhenSetTmpEndPos()
	{
		OnCreateTmpMoveLine();
	}

	public void OnMoveLineUpdate()
	{
		Vector3 realEndPos = GetRealEndPos();
		RefreshMoveLineLength(realEndPos);
	}

	public void OnMoveLineStop()
	{
		DestroyMoveLine();
		DestroyMoveEndArrow();
	}

	public void EnterWorkIdle()
	{
		if (!isTruck)
		{
			for (int i = 0; i < _allWorkMan.Count; i++)
			{
				_allWorkMan[i].PlayAnim("idle");
			}
		}
	}

	public void PlayAnim(string animName)
	{
		if (anims != null)
		{
			if (anims.Length > 1)
			{
				for (int i = 0; i < anims.Length; i++)
				{
					SimpleAnimation simpleAnimation = anims[i];
					SimpleAnimation.State state = simpleAnimation.GetState(animName);
					if (state != null)
					{
						simpleAnimation[animName].time = UnityEngine.Random.Range(0f, state.length);
						simpleAnimation.Play(animName);
					}
				}
			}
			else if (anims.Length == 1 && anims[0].GetState(animName) != null)
			{
				anims[0].Play(animName);
			}
		}
		if (gpuAnims != null)
		{
			for (int j = 0; j < gpuAnims.Length; j++)
			{
				gpuAnims[j].Play(animName, UnityEngine.Random.Range(0f, 1f));
			}
		}
	}

	private float GetRadius()
	{
		int num = SceneManager.World.WorldToTileIndex(EndPos);
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", num);
		if (luaTable != null)
		{
			if (luaTable.Get<int>("type") == 1)
			{
				string str = luaTable.Get<string>("itemId");
				string templateData = GameEntry.ConfigCache.GetTemplateData("aps_singlemap_junk", str.ToInt(), "Radius");
				if (!string.IsNullOrEmpty(templateData))
				{
					return templateData.ToFloat();
				}
				return 1.3f;
			}
			int pointSize = SceneManager.World.GetPointSize(num);
			if (pointSize <= 1)
			{
				return 1.3f;
			}
			return 0.3f + (float)pointSize;
		}
		return 1.3f;
	}

	private Quaternion GetWorkManQuaternion(Vector3 end, Vector3 start)
	{
		if ((end - start).normalized + Vector3.forward == Vector3.zero)
		{
			return Quaternion.Euler(new Vector3(0f, 180f, 0f));
		}
		return Quaternion.FromToRotation(Vector3.forward, (end - start).normalized);
	}

	private float GetMoveSpeed(float dis)
	{
		string[] array = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "prologue_vic_people", "k1").Split(new char[1] { '|' });
		int num = -1;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array3 = text.Split(new char[1] { ';' });
				int num2 = int.Parse(array3[0]);
				num = int.Parse(array3[1]);
				if (dis <= (float)num2)
				{
					break;
				}
			}
		}
		if (num < 0)
		{
			num = 5;
		}
		return num;
	}

	public float EnterWorkMove(float moveSpeed)
	{
		float num = 0f;
		Vector3 position = base.transform.position;
		Vector3 realEndPos = GetRealEndPos();
		moveSpeed = GetMoveSpeed(Vector3.Distance(realEndPos, position));
		if (isTruck)
		{
			Quaternion workManQuaternion = GetWorkManQuaternion(realEndPos, position);
			truckTransform.rotation = workManQuaternion;
			num = Vector3.Distance(realEndPos, position) / moveSpeed;
		}
		else
		{
			num = Vector3.Distance(realEndPos, position) / moveSpeed;
			List<WorkManMoveTempStruct> list = new List<WorkManMoveTempStruct>();
			int count = _allWorkMan.Count;
			for (int i = 0; i < count; i++)
			{
				_allWorkMan[i].PlayAnim("xiaoren_run");
				float radius = GetRadius();
				list.Add(GetWorkManPosByIndex(i, count, radius, realEndPos));
			}
			Quaternion workManQuaternion2 = GetWorkManQuaternion(realEndPos, position);
			for (int j = 0; j < count; j++)
			{
				CityWorkMan.WorkManMoveStruct moveStruct = _allWorkMan[j].MoveStruct;
				moveStruct.endPos = list[j].pos;
				moveStruct.endQuat = list[j].quar;
				moveStruct.startPos = _allWorkMan[j].GetPosition();
				moveStruct.startQuat = _allWorkMan[j].GetRotation();
				moveStruct.moveQuat = workManQuaternion2;
				moveStruct.time = num;
				if (moveStruct.time > 2f)
				{
					moveStruct.gatheredTime = 1f;
					Vector3 centerPos = Vector3.Lerp(base.transform.position, realEndPos, 1f / moveStruct.time);
					moveStruct.gatheredPos = GetWorkManPosByIndex(j, count, 1.3f, centerPos).pos;
					moveStruct.gatheredQuat = GetWorkManQuaternion(moveStruct.gatheredPos, moveStruct.startPos);
				}
				else
				{
					moveStruct.gatheredTime = 0f;
				}
				moveStruct.spreadTime = ((moveStruct.time < 1f) ? moveStruct.time : 1f);
				Vector3 centerPos2 = Vector3.Lerp(base.transform.position, realEndPos, (moveStruct.time - 1f) / moveStruct.time);
				moveStruct.spreadPos = GetWorkManPosByIndex(j, count, 1.3f, centerPos2).pos;
				moveStruct.spreadQuat = GetWorkManQuaternion(moveStruct.endPos, moveStruct.spreadPos);
				_allWorkMan[j].MoveStruct = moveStruct;
			}
		}
		int num2 = SceneManager.World.WorldToTileIndex(EndPos);
		if (SceneManager.World.GetPointType(num2) == 1)
		{
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", num2);
			if (luaTable != null)
			{
				string str = luaTable.Get<string>("itemId");
				string templateData = GameEntry.ConfigCache.GetTemplateData("aps_singlemap_junk", str.ToInt(), "Time");
				if (!templateData.IsNullOrEmpty())
				{
					int num3 = 0;
					float num4 = (float)num3 + num + templateData.ToFloat() + 1f;
					GameEntry.Lua.Call("CSharpCallLuaInterface.SetGuideGarbageCollectTime", (long)num3, (long)num4);
				}
			}
		}
		return num;
	}

	public void MoveStartRotation(float time)
	{
		if (!isTruck)
		{
			int count = _allWorkMan.Count;
			for (int i = 0; i < count; i++)
			{
				_allWorkMan[i].ChangeRotation(Quaternion.Slerp(_allWorkMan[i].MoveStruct.startQuat, _allWorkMan[i].MoveStruct.moveQuat, time));
			}
		}
	}

	public void MoveEndRotation(float time)
	{
		if (!isTruck)
		{
			int count = _allWorkMan.Count;
			for (int i = 0; i < count; i++)
			{
				_allWorkMan[i].ChangeRotation(Quaternion.Slerp(_allWorkMan[i].MoveStruct.moveQuat, _allWorkMan[i].MoveStruct.endQuat, time));
			}
		}
	}

	public void Move(float time)
	{
		if (isTruck)
		{
			return;
		}
		int count = _allWorkMan.Count;
		for (int i = 0; i < count; i++)
		{
			if (time <= _allWorkMan[i].MoveStruct.gatheredTime)
			{
				_allWorkMan[i].ChangeRotation(_allWorkMan[i].MoveStruct.gatheredQuat);
				_allWorkMan[i].ChangePosition(Vector3.Lerp(_allWorkMan[i].MoveStruct.startPos, _allWorkMan[i].MoveStruct.gatheredPos, time / 1f));
				continue;
			}
			if (time >= _allWorkMan[i].MoveStruct.time - _allWorkMan[i].MoveStruct.spreadTime)
			{
				float num = time - (_allWorkMan[i].MoveStruct.time - 1f);
				_allWorkMan[i].ChangeRotation(_allWorkMan[i].MoveStruct.spreadQuat);
				_allWorkMan[i].ChangePosition(Vector3.Lerp(_allWorkMan[i].MoveStruct.spreadPos, _allWorkMan[i].MoveStruct.endPos, num / 1f));
				continue;
			}
			float num2 = time - _allWorkMan[i].MoveStruct.gatheredTime;
			float num3 = _allWorkMan[i].MoveStruct.time - _allWorkMan[i].MoveStruct.gatheredTime - _allWorkMan[i].MoveStruct.spreadTime;
			if (_allWorkMan[i].MoveStruct.gatheredTime > 0f)
			{
				_allWorkMan[i].ChangeRotation(_allWorkMan[i].MoveStruct.moveQuat);
				_allWorkMan[i].ChangePosition(Vector3.Lerp(_allWorkMan[i].MoveStruct.gatheredPos, _allWorkMan[i].MoveStruct.spreadPos, num2 / num3));
			}
			else
			{
				_allWorkMan[i].ChangeRotation(_allWorkMan[i].MoveStruct.moveQuat);
				_allWorkMan[i].ChangePosition(Vector3.Lerp(_allWorkMan[i].MoveStruct.startPos, _allWorkMan[i].MoveStruct.spreadPos, num2 / num3));
			}
		}
	}

	public void EnterWorkGarbage()
	{
		if (!isTruck)
		{
			for (int i = 0; i < _allWorkMan.Count; i++)
			{
				_allWorkMan[i].PlayAnim("xiaoren_work");
			}
		}
	}

	public void EnterWorkAttack()
	{
		if (!isTruck)
		{
			for (int i = 0; i < _allWorkMan.Count; i++)
			{
				_allWorkMan[i].PlayAnim("xiaoren_work");
			}
		}
	}

	public Vector3 GetRealEndPos()
	{
		int index = SceneManager.World.WorldToTileIndex(EndPos);
		if (SceneManager.World.GetPointType(index) == 1)
		{
			return SceneManager.World.TileIndexToWorld(index);
		}
		return EndPos;
	}

	public void EnterWorkOpenFog()
	{
		if (isTruck)
		{
			return;
		}
		int num = 0;
		Vector3 realEndPos = GetRealEndPos();
		float num2 = float.MaxValue;
		for (int i = 0; i < _allWorkMan.Count; i++)
		{
			float num3 = Vector3.Distance(realEndPos, _allWorkMan[i].GetPosition());
			if (num2 > num3)
			{
				num2 = num3;
				num = i;
			}
		}
		int count = _allWorkMan.Count;
		for (int j = 0; j < count; j++)
		{
			if (j != num)
			{
				_allWorkMan[j].PlayAnim("idle");
			}
		}
		if (count > num)
		{
			_allWorkMan[num].PlayAnim("xiaoren_scanning");
		}
	}

	public void PlayGarbageSuccess()
	{
		if (!isTruck)
		{
			for (int i = 0; i < _allWorkMan.Count; i++)
			{
				_allWorkMan[i].PlayAnim("xiaoren_show");
			}
			return;
		}
		foreach (CityTroopUnit troopUnit in troopUnits)
		{
			troopUnit.PickGarbageSuccess();
		}
	}

	public void PlayGarbageFail()
	{
		if (!isTruck)
		{
			for (int i = 0; i < _allWorkMan.Count; i++)
			{
				_allWorkMan[i].PlayAnim("xiaoren_fail");
			}
			return;
		}
		foreach (CityTroopUnit troopUnit in troopUnits)
		{
			troopUnit.PickGarbageFail();
		}
	}

	private void Update()
	{
		if (_isDrag)
		{
			if (SceneManager.World.CanMoving)
			{
				_isDrag = false;
				GetCurState().OnEndDrag();
			}
			else
			{
				GetCurState().OnDrag();
			}
		}
		GetCurState().OnUpdate(Time.deltaTime);
		if (_isFollow)
		{
			SceneManager.World.Lookat(base.transform.position);
		}
		UpdateTroopUnits();
	}

	private void UpdateTroopUnits()
	{
		if (troopUnits.Count <= 0)
		{
			return;
		}
		foreach (CityTroopUnit troopUnit in troopUnits)
		{
			troopUnit.Update();
		}
		if (troopUnits.All((CityTroopUnit i) => i.IsBackFinish()))
		{
			ClearTroopUnits();
		}
	}

	public void ClearTroopUnits()
	{
		troopUnits.ForEach(delegate(CityTroopUnit i)
		{
			i.Destroy();
		});
		troopUnits.Clear();
	}

	private void CityTroopMoveSignal(object userData)
	{
		int index = userData.ToInt();
		Vector3 pos = SceneManager.World.TileIndexToWorld(index);
		GetCurState()?.CityTroopMoveSignal(pos);
	}

	public void MoveAfterCreate(int targetPosition)
	{
		targetPos = targetPosition;
		needMoveAfterCreate = true;
	}

	private void CameraFollowCityTroopSignal(object userData)
	{
		if ((bool)userData)
		{
			SceneManager.World.AutoLookat(base.transform.position, -1f, 0.4f, delegate
			{
				_isFollow = true;
			});
		}
		else
		{
			_isFollow = false;
		}
	}

	private void OnWorldInputPointDownSignal(object userData)
	{
		_isFollow = IsNeedFollowCamera();
	}

	private void RefreshGuideSignal(object userData)
	{
	}

	private BaseCityTroopState GetCurState()
	{
		if (_cityTroopMachine != null)
		{
			return _cityTroopMachine.GetCurState();
		}
		return null;
	}

	private void RefreshLineLength(Vector3 pos)
	{
		if (_dragTroopLine != null)
		{
			_dragTroopLine.SetDragPath(base.transform.position, pos);
		}
	}

	private void RefreshLineDestination(Vector3 pos)
	{
		int tileSize = 1;
		int num = SceneManager.World.WorldToTileIndex(pos);
		MarchTargetType targetType = SceneManager.World.GetTargetType(0L, num);
		EnumDestinationSignalType destinationType = SceneManager.World.GetDestinationType(0L, 0L, num, targetType, isFormation: false, ref pos, ref tileSize);
		if (_troopDestination != null)
		{
			_troopDestination.SetDestination(pos, destinationType, targetType, tileSize, 0f);
		}
	}

	private void CreateTroopDestination()
	{
		if (_troopDestinationInst == null)
		{
			_troopDestinationInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
			_troopDestinationInst.completed += delegate
			{
				_troopDestinationInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				_troopDestination = _troopDestinationInst.gameObject.GetComponent<WorldTroopDestinationSignal>();
			};
		}
	}

	private void DestroyTroopDestination()
	{
		if (_troopDestinationInst != null)
		{
			_troopDestinationInst.Destroy();
			_troopDestinationInst = null;
			_troopDestination = null;
		}
	}

	private void CreateDragLine()
	{
		if (_dragTroopLineInst == null)
		{
			_dragTroopLineInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopLineDrag.prefab");
			_dragTroopLineInst.completed += delegate
			{
				_dragTroopLineInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				_dragTroopLine = _dragTroopLineInst.gameObject.GetComponent<WorldTroopLine>();
			};
		}
	}

	private void DestroyDragLine()
	{
		if (_dragTroopLineInst != null)
		{
			_dragTroopLineInst.Destroy();
			_dragTroopLineInst = null;
			_dragTroopLine = null;
		}
	}

	private void CreateMoveLine(bool useTmpPoint = false)
	{
		if (_moveTroopLineInst != null)
		{
			return;
		}
		_moveTroopLineInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopLine.prefab");
		_moveTroopLineInst.completed += delegate
		{
			_moveTroopLineInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			_moveTroopLine = _moveTroopLineInst.gameObject.GetComponent<WorldTroopLine>();
			if (TmpEndPos.x >= 0f && useTmpPoint)
			{
				RefreshMoveLineLength(TmpEndPos);
			}
		};
	}

	private void DestroyMoveLine()
	{
		if (_moveTroopLineInst != null)
		{
			RefreshMoveLineLength(base.transform.position);
			_moveTroopLineInst.Destroy();
			_moveTroopLineInst = null;
			_moveTroopLine = null;
		}
	}

	private void RefreshMoveLineLength(Vector3 pos)
	{
		if (_moveTroopLine != null)
		{
			_moveTroopLine.SetDragPath(base.transform.position, pos);
		}
	}

	private void CreateMoveEndArrow(bool useTmpPoint = false)
	{
		if (_moveEndPosArrowInst != null)
		{
			return;
		}
		_moveEndPosArrowInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
		_moveEndPosArrowInst.completed += delegate
		{
			_moveEndPosArrowInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			_moveEndPosArrow = _moveEndPosArrowInst.gameObject.GetComponent<WorldTroopDestinationSignal>();
			if (TmpEndPos.x >= 0f && useTmpPoint)
			{
				RefreshMoveLineLength(TmpEndPos);
			}
			else
			{
				RefreshMoveArrowPosition(EndPos);
			}
		};
	}

	private void DestroyMoveEndArrow()
	{
		if (_moveEndPosArrowInst != null)
		{
			_moveEndPosArrowInst.Destroy();
			_moveEndPosArrowInst = null;
			_moveEndPosArrow = null;
		}
	}

	private void RefreshMoveArrowPosition(Vector3 pos)
	{
		int tileSize = 1;
		int num = SceneManager.World.WorldToTileIndex(pos);
		MarchTargetType targetType = SceneManager.World.GetTargetType(0L, num);
		EnumDestinationSignalType destinationType = SceneManager.World.GetDestinationType(0L, 0L, num, targetType, isFormation: false, ref pos, ref tileSize);
		if (_moveEndPosArrow != null)
		{
			if (destinationType == EnumDestinationSignalType.EmptyGround)
			{
				_moveEndPosArrow.SetDestination(pos, destinationType, targetType, tileSize, 1f);
				_moveEndPosArrow.SetDestinationOver();
			}
			else
			{
				_moveEndPosArrow.SetDestinationForMarch(pos, destinationType, tileSize);
			}
		}
	}

	private void EdgeDragUpdate(Vector3 dragPosCurrent)
	{
		float num = dragPosCurrent.x / (float)Screen.width;
		float num2 = dragPosCurrent.y / (float)Screen.height;
		if (num < 0.08f || num > 0.92f || num2 < 0.1f || num2 > 0.9f)
		{
			Vector3 touchPoint = SceneManager.World.GetTouchPoint(dragPosCurrent);
			Vector3 curTarget = SceneManager.World.CurTarget;
			Vector3 vector = touchPoint - curTarget;
			float magnitude = vector.magnitude;
			if (magnitude > 0.1f)
			{
				Vector3 vector2 = vector / magnitude;
				float num3 = 1f * Time.deltaTime * SceneManager.World.GetLodDistance();
				Vector3 lookWorldPosition = curTarget + vector2 * num3;
				SceneManager.World.CanMoving = true;
				SceneManager.World.Lookat(lookWorldPosition);
				SceneManager.World.CanMoving = false;
			}
		}
	}

	private void LoadTruck()
	{
		requestInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/WorldTroop.prefab");
		requestInst.completed += delegate
		{
			requestInst.gameObject.transform.SetParent(_modelGo);
			truckTransform = requestInst.gameObject.transform;
			BoxCollider component = truckTransform.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.enabled = false;
			}
			truckTransform.localPosition = Vector3.zero;
			if (needMoveAfterCreate)
			{
				CityTroopMoveSignal(targetPos);
				needMoveAfterCreate = false;
				targetPos = 0;
			}
			GameObject gameObject = base.transform.Find("Model").gameObject;
			anims = gameObject.GetComponentsInChildren<SimpleAnimation>();
			gpuAnims = gameObject.GetComponentsInChildren<GPUSkinningAnimator>();
		};
	}

	public bool IsTruck()
	{
		return isTruck;
	}

	private void DestroyTruck()
	{
		if (requestInst != null)
		{
			requestInst.Destroy();
		}
		anims = null;
		gpuAnims = null;
		requestInst = null;
		truckTransform = null;
	}

	private void LoadWorkMan()
	{
		int allCount = GameEntry.Lua.CallWithReturn<int>("DataCenter.GuideManager:GetCityTroopPeopleNum");
		int count = _allWorkManReq.Count;
		Vector3 endPos = base.transform.position;
		if (allCount <= count)
		{
			return;
		}
		for (int i = 0; i < allCount; i++)
		{
			int index = i;
			if (i >= count)
			{
				InstanceRequest req = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/CityScene/CityWorkMan.prefab");
				req.completed += delegate
				{
					req.gameObject.transform.SetParent(_modelGo);
					CityWorkMan component = req.gameObject.GetComponent<CityWorkMan>();
					if (component != null)
					{
						_allWorkMan.Add(component);
						component.Index = index;
						WorkManMoveTempStruct workManPosByIndex2 = GetWorkManPosByIndex(index, allCount, 1.3f, endPos);
						component.ChangePosition(workManPosByIndex2.pos);
						component.ChangeRotation(workManPosByIndex2.quar);
						component.PlayAnim("idle");
						if (needMoveAfterCreate && index == allCount - 1)
						{
							CityTroopMoveSignal(targetPos);
							needMoveAfterCreate = false;
							targetPos = 0;
							SceneManager.World.TrackMarch(1L);
						}
					}
				};
				_allWorkManReq.Add(req);
			}
			else if (_allWorkMan[index] != null)
			{
				WorkManMoveTempStruct workManPosByIndex = GetWorkManPosByIndex(index, allCount, 1.3f, endPos);
				_allWorkMan[index].ChangePosition(workManPosByIndex.pos);
				_allWorkMan[index].ChangeRotation(workManPosByIndex.quar);
			}
		}
	}

	private void DestroyWorkMan()
	{
		if (_allWorkManReq != null)
		{
			for (int i = 0; i < _allWorkManReq.Count; i++)
			{
				_allWorkManReq[i].Destroy();
			}
		}
		_allWorkManReq = null;
		_allWorkMan = null;
	}

	private WorkManMoveTempStruct GetWorkManPosByIndex(int index, int allCount, float radius, Vector3 centerPos)
	{
		float num = (float)(index + 1) / (float)allCount * 360f;
		Vector3 zero = Vector3.zero;
		zero.x = centerPos.x + radius * Mathf.Cos(num * (MathF.PI / 180f));
		zero.z = centerPos.z + radius * Mathf.Sin(num * (MathF.PI / 180f));
		zero.y = centerPos.y;
		WorkManMoveTempStruct result = new WorkManMoveTempStruct
		{
			pos = zero
		};
		Vector3 normalized = (centerPos - zero).normalized;
		if (normalized + Vector3.forward == Vector3.zero)
		{
			result.quar = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		}
		else
		{
			result.quar = Quaternion.FromToRotation(Vector3.forward, normalized);
		}
		return result;
	}

	private void SortWorkManByDistance(Vector3 pos)
	{
		_allWorkMan.Sort(delegate(CityWorkMan a, CityWorkMan b)
		{
			float num = Vector3.Distance(a.GetPosition(), pos);
			float num2 = Vector3.Distance(b.GetPosition(), pos);
			if (num.Equals(num2))
			{
				return 0;
			}
			return (num > num2) ? 1 : (-1);
		});
	}

	private bool IsNeedFollowCamera()
	{
		return false;
	}

	private bool InitFollowCamera()
	{
		return false;
	}

	private void RefreshCityTroopPeopleNumSignal(object userData)
	{
		if (!isTruck)
		{
			LoadWorkMan();
		}
	}

	public bool CanLongTap()
	{
		return false;
	}

	public Transform GetTransform()
	{
		if (base.transform != null)
		{
			return base.transform;
		}
		return null;
	}

	public T GetPickComponent<T>() where T : MonoBehaviour
	{
		if (base.transform != null)
		{
			return base.transform.GetComponent<T>();
		}
		return null;
	}

	public bool PointInPick()
	{
		if (SceneManager.World.GetTouchTilePos() == TilePos)
		{
			return true;
		}
		return false;
	}

	public void Drag(Vector3 pos)
	{
	}

	public bool Select()
	{
		return false;
	}

	public void Click()
	{
	}

	public void ShowHeadUI()
	{
		GameEntry.Event.Fire(EventId.ShowCityTroopHead);
	}

	public bool IsOutRange(Vector3 pos)
	{
		return false;
	}

	public void ChangeTouchPos(int index)
	{
	}

	public Vector3 GetClosestPoint(Vector3 pos)
	{
		return Vector3.zero;
	}

	public bool OnEndDrag(Vector3 dragStopPos)
	{
		return true;
	}

	public bool OnClick()
	{
		return true;
	}

	public bool OnEndLongTap()
	{
		return true;
	}

	public bool OnBeginLongTap()
	{
		return true;
	}

	public bool IsTruckPickGarbageTroop()
	{
		int pointId = SceneManager.World.WorldToTileIndex(EndPos);
		MarchTargetType targetType = SceneManager.World.GetTargetType(0L, pointId);
		if (isTruck)
		{
			return targetType == MarchTargetType.PICK_GARBAGE;
		}
		return false;
	}

	public bool IsWorkmanPickGarbageTroop()
	{
		int pointId = SceneManager.World.WorldToTileIndex(EndPos);
		MarchTargetType targetType = SceneManager.World.GetTargetType(0L, pointId);
		if (!isTruck && targetType == MarchTargetType.PICK_GARBAGE)
		{
			return GameEntry.Data.Fog.IsUnlock(pointId);
		}
		return false;
	}

	public Vector3 GetTrunkPosition()
	{
		if (isTruck && truckTransform != null)
		{
			return truckTransform.position;
		}
		return new Vector3(0f, 0f, 0f);
	}

	public float GetPickGarbageMoveDistance()
	{
		return 6f;
	}

	public float GetOpenFogMoveDistance()
	{
		return 6.3f;
	}

	private List<Vector3> GetPickGarbagePositions()
	{
		List<Vector3> list = new List<Vector3>();
		float num = 2f;
		int i = 0;
		int num2 = 5;
		float num3 = 360 / num2;
		Vector3 realEndPos = GetRealEndPos();
		double x = base.transform.position.x - realEndPos.x;
		double num4 = Math.Atan2(base.transform.position.z - realEndPos.z, x) * 180.0 / Math.PI;
		for (; i < num2; i++)
		{
			double num5 = (double)(num3 * (float)i) + num4;
			double num6 = (double)num * Math.Cos(num5 * Math.PI / 180.0);
			double num7 = (double)num * Math.Sin(num5 * Math.PI / 180.0);
			Vector3 item = realEndPos + new Vector3((float)num6, 0f, (float)num7);
			list.Add(item);
		}
		return list;
	}

	public void TroopUnitsBirthThenPickGarbage(bool appear = true)
	{
		if (troopUnits.Count > 0 || !IsTruckPickGarbageTroop() || !isTruck)
		{
			return;
		}
		List<Vector3> pickGarbagePositions = GetPickGarbagePositions();
		int num = CityGarbageBirthPos.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = CityGarbageBirthPos[i];
			Vector3 birthDest = GetTrunkPosition() + truckTransform.right * vector.x + truckTransform.forward * vector.z;
			Vector3 pickDest = GetTrunkPosition();
			if (pickGarbagePositions.Count > 0)
			{
				if (pickGarbagePositions.Count > i)
				{
					pickDest = pickGarbagePositions[i];
				}
				else
				{
					pickDest = pickGarbagePositions[pickGarbagePositions.Count - 1];
				}
			}
			CityPickGarbageTroopUnit entity = new CityPickGarbageTroopUnit(CityTroopUnit.UnitType.Junkman, this);
			entity.CreateInstance(delegate
			{
				entity.SetPosition(GetTrunkPosition() + new Vector3(0f, 1.6f, 0f));
				Vector3 realEndPos = GetRealEndPos();
				if (appear)
				{
					entity.BirthThenMoveToGarbage(birthDest, pickDest, realEndPos);
				}
				else
				{
					entity.BirthThenPickGarbage(birthDest, pickDest, realEndPos);
				}
			}, truckTransform);
			troopUnits.Add(entity);
		}
	}

	public void TroopUnitPickSuccess()
	{
		foreach (CityTroopUnit troopUnit in troopUnits)
		{
			troopUnit.PickGarbageSuccess();
		}
	}
}
