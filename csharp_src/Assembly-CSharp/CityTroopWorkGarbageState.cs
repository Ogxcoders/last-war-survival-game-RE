using UnityEngine;
using XLua;

public class CityTroopWorkGarbageState : BaseCityTroopState
{
	private Vector3 _pos;

	private float _curTime;

	private float _allTime;

	private const float DefaultTime = 5f;

	private const float DisappearTime = 0.5f;

	private InstanceRequest _pickGarbageInst;

	private InstanceRequest _colloctEffectObject;

	private SuperTextMesh _timeText;

	private long _showTime;

	private bool _isHiding;

	private const string ShowAnimationName = "Default";

	private const string HideAnimationName = "CollectGarbageUI_hide";

	public CityTroopWorkGarbageState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		_pos = Vector3.zero;
		_curTime = 0f;
		_allTime = 5.5f;
		_isHiding = false;
		_showTime = 0L;
		int param = SceneManager.World.WorldToTileIndex(cityTroop.EndPos);
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", param);
		if (luaTable != null)
		{
			string str = luaTable.Get<string>("itemId");
			string templateData = GameEntry.ConfigCache.GetTemplateData("aps_singlemap_junk", str.ToInt(), "Time");
			if (!templateData.IsNullOrEmpty())
			{
				_allTime = templateData.ToFloat() + 0.5f;
			}
		}
		cityTroop.EnterWorkGarbage();
		cityTroop.PlayAnim("idle");
		ShowCollectParticle();
	}

	public override void OnUpdate(float deltaTime)
	{
		_curTime += deltaTime;
		if (_curTime >= _allTime)
		{
			machine.ChangeState(TroopState.GarbageResult);
			int param = SceneManager.World.WorldToTileIndex(cityTroop.EndPos);
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", param);
			if (luaTable != null)
			{
				long param2 = luaTable.Get<long>("uuid");
				GameEntry.Lua.Call("DataCenter.GuideCityManager:SendCityPickGarbageFinish", param2);
			}
		}
		else
		{
			long num = (int)((_allTime - 0.5f - _curTime) * 1000f);
			if (_showTime != num)
			{
				RefreshTime(num);
			}
		}
	}

	public override void OnLeave()
	{
		DestroyTimeObj();
	}

	public override void OnBeginDrag()
	{
		SceneManager.World.CanMoving = false;
		cityTroop.OnCreateDragLine();
	}

	public override void OnDrag()
	{
		SceneManager.World.CanMoving = false;
		Vector3 touchPoint = SceneManager.World.GetTouchPoint();
		if (_pos != touchPoint)
		{
			_pos = touchPoint;
			cityTroop.OnDragLineUpdate(touchPoint);
		}
	}

	public override void OnEndDrag()
	{
		SceneManager.World.CanMoving = true;
		cityTroop.OnDragLineStop();
		Vector3 vector = CheckCanMovePosByPos(SceneManager.World.GetTouchPoint());
		if (vector != Vector3.zero)
		{
			cityTroop.TmpEndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			cityTroop.DoWhenSetTmpEndPos();
		}
	}

	public override void CityTroopMoveSignal(Vector3 pos1)
	{
		Vector3 vector = CheckCanMovePosByPos(pos1);
		if (vector != Vector3.zero)
		{
			cityTroop.TmpEndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			cityTroop.DoWhenSetTmpEndPos();
		}
	}

	public void RefreshTime(long leftTime)
	{
		if (leftTime <= 0)
		{
			HideTimeObject();
		}
		else if (_timeText != null && leftTime >= 0)
		{
			_timeText.text = GameEntry.Timer.MilliSecondToFmtString(leftTime);
		}
	}

	private void HideTimeObject()
	{
		if (_pickGarbageInst != null && !_isHiding)
		{
			SimpleAnimation componentInChildren = _pickGarbageInst.gameObject.GetComponentInChildren<SimpleAnimation>();
			_isHiding = true;
			componentInChildren.Play("CollectGarbageUI_hide");
		}
	}

	private void DestroyTimeObj()
	{
		if (_pickGarbageInst != null)
		{
			_pickGarbageInst.Destroy();
			_pickGarbageInst = null;
			_timeText = null;
		}
		HideCollectParticle();
	}

	private void CreateTimeObj()
	{
		long time = GameEntry.Timer.GetServerTime();
		if (_pickGarbageInst == null)
		{
			_pickGarbageInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/CollectGarbageUI.prefab");
			_pickGarbageInst.completed += delegate
			{
				_pickGarbageInst.gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				_pickGarbageInst.gameObject.transform.position = cityTroop.GetRealEndPos();
				_timeText = _pickGarbageInst.gameObject.transform.Find("PosGo/TimeText").GetComponent<SuperTextMesh>();
				_pickGarbageInst.gameObject.GetComponent<ChangeSceneCircleSlider>().Init(time, (long)((_allTime - 0.5f) * 1000f) + time);
				_pickGarbageInst.gameObject.GetComponentInChildren<SimpleAnimation>().Play("Default");
			};
		}
	}

	private void ShowCollectParticle()
	{
		if (_colloctEffectObject != null)
		{
			_colloctEffectObject.gameObject.SetActive(value: true);
			return;
		}
		_colloctEffectObject = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/Common/VFX_jianlaji.prefab");
		_colloctEffectObject.completed += delegate
		{
			GameObject gameObject = _colloctEffectObject.gameObject;
			gameObject.SetActive(value: true);
			Transform dynamicObjNode = SceneManager.World.DynamicObjNode;
			if (dynamicObjNode != null)
			{
				gameObject.transform.SetParent(dynamicObjNode);
				gameObject.transform.position = cityTroop.GetRealEndPos();
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
			}
		};
	}

	private void HideCollectParticle()
	{
		if (_colloctEffectObject != null)
		{
			_colloctEffectObject.Destroy();
			_colloctEffectObject = null;
		}
	}
}
