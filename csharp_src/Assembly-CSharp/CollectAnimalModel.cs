using UnityEngine;

public class CollectAnimalModel : MonoBehaviour
{
	public class Param
	{
		public int pointIndex;

		public int targetPointId;

		public long uuid;

		public InstanceRequest request;

		public CollectAnimalModel collectAnimalModel;

		public int buildId;
	}

	private enum AnimalAnimationState
	{
		None,
		FlayTarget,
		Work,
		FlaySelf,
		FlayEnd,
		Full
	}

	[SerializeField]
	private Transform resRoot;

	[SerializeField]
	private SimpleAnimation _anim;

	[SerializeField]
	private GameObject _saomiaoObj;

	private InstanceRequest resReq;

	private SimpleAnimation resAnim;

	public static float MoveSpeed = 3f;

	public static float TargetDistance = 2.6f;

	public static float SelfDistance = 0.4f;

	public static float RotateNeedTime = 0.5f;

	public static float ShowResourceTime = 2f;

	public static float EndFullSelfDistance = 0.8f;

	public static Quaternion FaceToUp = Quaternion.Euler(0f, 180f, 0f);

	public Param _param;

	private Vector3 _targetPoint;

	private Vector3 _selfPoint;

	private AnimalAnimationState _curState;

	private float _needTime;

	private float _curTime;

	private float _distance;

	private Quaternion _goQuaternion;

	private Quaternion _backQuaternion;

	private Quaternion _goQuaternion2;

	public void Init(Param param)
	{
		_param = param;
		_targetPoint = SceneManager.World.TileIndexToWorld(param.targetPointId);
		_selfPoint = SceneManager.World.TileIndexToWorld(param.pointIndex);
		_targetPoint += Vector3.Normalize(_selfPoint - _targetPoint) * TargetDistance;
		_selfPoint += Vector3.Normalize(_targetPoint - _selfPoint) * SelfDistance;
		_distance = Vector3.Distance(_targetPoint, _selfPoint);
		Vector3 to = Vector3.Normalize(_targetPoint - _selfPoint);
		float num = Vector3.Angle(Vector3.forward, to);
		if (_targetPoint.x < _selfPoint.x)
		{
			num = 360f - num;
		}
		_goQuaternion = Quaternion.Euler(0f, num, 0f);
		_backQuaternion = Quaternion.Euler(0f, num - 180f, 0f);
		_goQuaternion2 = Quaternion.Euler(0f, num - 360f, 0f);
		resReq = GameEntry.Resource.InstantiateAsync(GetResPath(param.buildId));
		resReq.completed += delegate(InstanceRequest req)
		{
			if (req.gameObject == null || resRoot == null)
			{
				req.Destroy();
			}
			else
			{
				GameObject gameObject = req.gameObject;
				gameObject.transform.SetParent(resRoot);
				gameObject.transform.localPosition = Vector3.zero;
				resAnim = gameObject.GetComponentInChildren<SimpleAnimation>();
				_curState = AnimalAnimationState.FlayEnd;
				RefreshState();
			}
		};
	}

	public void UnInit()
	{
		if (resReq != null)
		{
			resReq.Destroy();
			resReq = null;
		}
	}

	private string GetResPath(int buildId)
	{
		return buildId switch
		{
			736000 => "Assets/_Art/Models/Soldier/WuRenJi/prefab/A_soldie_wood.prefab", 
			737000 => "Assets/_Art/Models/Soldier/WuRenJi/prefab/A_soldie_stone.prefab", 
			_ => "Assets/_Art/Models/Soldier/WuRenJi/prefab/A_soldie_crystal.prefab", 
		};
	}

	private void ChangeAnimalState()
	{
		switch (_curState)
		{
		case AnimalAnimationState.None:
			base.transform.position = _selfPoint + Vector3.Normalize(_targetPoint - _selfPoint) * EndFullSelfDistance;
			base.transform.rotation = FaceToUp;
			_saomiaoObj.SetActive(value: false);
			if (resAnim != null)
			{
				resAnim.gameObject.SetActive(value: false);
			}
			break;
		case AnimalAnimationState.FlayTarget:
			_saomiaoObj.SetActive(value: false);
			if (resAnim != null)
			{
				resAnim.gameObject.SetActive(value: false);
			}
			_curTime = 0f;
			_needTime = _distance / MoveSpeed;
			_anim.Play("shxr_run_01");
			break;
		case AnimalAnimationState.Work:
			_saomiaoObj.SetActive(value: true);
			if (resAnim != null)
			{
				resAnim.gameObject.SetActive(value: false);
			}
			_curTime = 0f;
			_anim.Play("shxr_work");
			_needTime = _anim.GetClipLength("shxr_work");
			break;
		case AnimalAnimationState.FlaySelf:
			_saomiaoObj.SetActive(value: false);
			if (resAnim != null)
			{
				resAnim.gameObject.SetActive(value: true);
				resAnim.Play("crystal_run");
			}
			_curTime = 0f;
			_needTime = _distance / MoveSpeed;
			_anim.Play("shxr_run_02");
			break;
		case AnimalAnimationState.FlayEnd:
			_saomiaoObj.SetActive(value: false);
			_needTime = 1f;
			if (resAnim != null)
			{
				resAnim.gameObject.SetActive(value: true);
				resAnim.Play("crystal_end");
				_needTime = resAnim.GetClipLength("crystal_end");
			}
			_curTime = 0f;
			_anim.Play("shxr_end");
			break;
		case AnimalAnimationState.Full:
			base.transform.position = _selfPoint + Vector3.Normalize(_targetPoint - _selfPoint) * EndFullSelfDistance;
			base.transform.rotation = FaceToUp;
			_saomiaoObj.SetActive(value: false);
			if (resAnim != null)
			{
				resAnim.gameObject.SetActive(value: false);
			}
			_anim.Play("shxr_idle_02");
			break;
		}
	}

	private void Update()
	{
		_curTime += Time.deltaTime;
		switch (_curState)
		{
		case AnimalAnimationState.FlayTarget:
		{
			float num3 = _curTime / _needTime;
			base.transform.position = Vector3.Lerp(_selfPoint, _targetPoint, num3);
			float num4 = _curTime / RotateNeedTime;
			if (num4 <= 1f)
			{
				base.transform.rotation = Quaternion.Slerp(_backQuaternion, _goQuaternion2, num4);
			}
			if (num3 >= 1f)
			{
				base.transform.rotation = _goQuaternion;
				_curState = AnimalAnimationState.Work;
				ChangeAnimalState();
			}
			break;
		}
		case AnimalAnimationState.Work:
			if (_curTime >= ShowResourceTime)
			{
				if (resAnim != null)
				{
					resAnim.gameObject.SetActive(value: true);
				}
				_saomiaoObj.SetActive(value: false);
			}
			if (_curTime >= _needTime)
			{
				_curState = AnimalAnimationState.FlaySelf;
				ChangeAnimalState();
			}
			break;
		case AnimalAnimationState.FlaySelf:
		{
			float num = _curTime / _needTime;
			base.transform.position = Vector3.Lerp(_targetPoint, _selfPoint, num);
			float num2 = _curTime / RotateNeedTime;
			if (num2 <= 1f)
			{
				base.transform.rotation = Quaternion.Slerp(_goQuaternion, _backQuaternion, num2);
			}
			if (num >= 1f)
			{
				base.transform.rotation = _backQuaternion;
				_curState = AnimalAnimationState.FlayEnd;
				ChangeAnimalState();
			}
			break;
		}
		case AnimalAnimationState.FlayEnd:
			if (_curTime / _needTime >= 1f)
			{
				if (resAnim != null)
				{
					resAnim.gameObject.SetActive(value: false);
				}
				RefreshState();
				GameEntry.Event.Fire(EventId.CollectAnimEnd, _param.uuid);
			}
			break;
		}
	}

	public void RefreshState()
	{
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.uuid);
		if (buildingDataByUuid == null)
		{
			_curState = AnimalAnimationState.None;
			ChangeAnimalState();
		}
		else
		{
			if ((_curState != AnimalAnimationState.None && _curState != AnimalAnimationState.FlayEnd && _curState != AnimalAnimationState.Full) || buildingDataByUuid == null)
			{
				return;
			}
			if (GameEntry.Lua.CallWithReturn<float, long>("CSharpCallLuaInterface.GetBuildDataResourcePercent", _param.uuid) >= 1f)
			{
				if (_curState == AnimalAnimationState.FlayEnd)
				{
					_curState = AnimalAnimationState.Full;
					ChangeAnimalState();
				}
			}
			else
			{
				_curState = AnimalAnimationState.FlayTarget;
				ChangeAnimalState();
			}
		}
	}
}
