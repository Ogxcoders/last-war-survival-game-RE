using System.Collections.Generic;
using UnityEngine;

public class BasementGetResourceMetal : CityBuilding
{
	public enum PercentState
	{
		None = -1,
		Zero = 0,
		Few = 25,
		Middle = 50,
		Max = 100
	}

	private enum RealPercent
	{
		Zero = 0,
		Few = 5,
		Middle = 25,
		Max = 100
	}

	public static Dictionary<PercentState, string> MetalRealModelPath = new Dictionary<PercentState, string>
	{
		{
			PercentState.Zero,
			null
		},
		{
			PercentState.Few,
			"Assets/Main/Prefabs/Building/BuildMetalFew.prefab"
		},
		{
			PercentState.Middle,
			"Assets/Main/Prefabs/Building/BuildMetalMiddle.prefab"
		},
		{
			PercentState.Max,
			"Assets/Main/Prefabs/Building/BuildMetalMax.prefab"
		}
	};

	public static Dictionary<PercentState, string> WoodRealModelPath = new Dictionary<PercentState, string>
	{
		{
			PercentState.Zero,
			null
		},
		{
			PercentState.Few,
			"Assets/Main/Prefabs/Building/BuildWoodFew.prefab"
		},
		{
			PercentState.Middle,
			"Assets/Main/Prefabs/Building/BuildWoodMiddle.prefab"
		},
		{
			PercentState.Max,
			"Assets/Main/Prefabs/Building/BuildWoodMax.prefab"
		}
	};

	private PercentState _statePro;

	private int _buildId;

	private InstanceRequest _instanceRequest;

	private int _realPercentFewValue = 5;

	private ITimer _timer;

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		_statePro = PercentState.None;
		RemoveTimer();
		GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
		GameEntry.Event.Subscribe(EventId.CollectAnimEnd, CollectAnimEndSignal);
		Refresh();
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		RemoveTimer();
		DeleteModel();
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
		GameEntry.Event.Unsubscribe(EventId.CollectAnimEnd, CollectAnimEndSignal);
	}

	private void UpdateBuildDataSignal(object userData)
	{
		if ((long)userData == base.Uuid)
		{
			Refresh();
		}
	}

	private void CollectAnimEndSignal(object userData)
	{
		if ((long)userData == base.Uuid)
		{
			Refresh();
		}
	}

	private void Refresh()
	{
		LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(base.Uuid);
		if (buildingDataByUuid == null)
		{
			return;
		}
		_buildId = buildingDataByUuid.buildId;
		if (buildingDataByUuid.buildUpdateTime > 0)
		{
			if (_instanceRequest != null && _instanceRequest.gameObject != null)
			{
				_instanceRequest.gameObject.SetActive(value: false);
			}
			return;
		}
		if (_instanceRequest != null && _instanceRequest.gameObject != null)
		{
			_instanceRequest.gameObject.SetActive(value: true);
		}
		float num = GameEntry.Lua.CallWithReturn<float, int, int>("CSharpCallLuaInterface.GetShowBubblePercent", _buildId, buildingDataByUuid.level);
		_realPercentFewValue = (int)num * 100;
		int state = (state = (int)(GameEntry.Lua.CallWithReturn<float, long>("CSharpCallLuaInterface.GetBuildDataResourcePercent", base.Uuid) * 100f));
		PercentState percentStateByTrueState = GetPercentStateByTrueState(state);
		if (_statePro != percentStateByTrueState)
		{
			_statePro = percentStateByTrueState;
			LoadModel(GetRealModelByBuildId(_buildId, percentStateByTrueState));
		}
	}

	private PercentState GetPercentStateByTrueState(int state)
	{
		if (state < _realPercentFewValue)
		{
			return PercentState.Zero;
		}
		if (state < 25)
		{
			return PercentState.Few;
		}
		if (state < 100)
		{
			return PercentState.Middle;
		}
		return PercentState.Max;
	}

	private string GetRealModelByBuildId(int buildId, PercentState state)
	{
		return buildId switch
		{
			737000 => MetalRealModelPath[state], 
			412000 => MetalRealModelPath[state], 
			736000 => WoodRealModelPath[state], 
			_ => null, 
		};
	}

	private void LoadModel(string modelName)
	{
		DeleteModel();
		if (!string.IsNullOrEmpty(modelName))
		{
			_instanceRequest = GameEntry.Resource.InstantiateAsync(modelName);
			_instanceRequest.completed += delegate
			{
				GameObject obj = _instanceRequest.gameObject;
				obj.transform.SetParent(buildModel.transform);
				obj.transform.localPosition = Vector3.zero;
			};
		}
	}

	private void DeleteModel()
	{
		if (_instanceRequest != null)
		{
			_instanceRequest.Destroy();
			_instanceRequest = null;
		}
	}

	private void RemoveTimer()
	{
		if (_timer != null)
		{
			GameEntry.Timer.CancelTimer(_timer);
		}
	}

	private void AddTimer()
	{
		RemoveTimer();
		RealPercent realPercent = RealPercent.Zero;
		switch (_statePro)
		{
		case PercentState.Zero:
			realPercent = RealPercent.Few;
			break;
		case PercentState.Few:
			realPercent = RealPercent.Middle;
			break;
		case PercentState.Middle:
			realPercent = RealPercent.Max;
			break;
		}
		if (realPercent == RealPercent.Zero)
		{
			return;
		}
		long num = GameEntry.Lua.CallWithReturn<long, int, int>("CSharpCallLuaInterface.GetNextChangeTimeByResourceUuid", _buildId, (int)realPercent);
		if (num > 0)
		{
			_timer = GameEntry.Timer.RegisterTimer(num, delegate
			{
				RemoveTimer();
				Refresh();
			});
		}
	}

	public override void refeshDate()
	{
		base.refeshDate();
		if (_buildId != 0)
		{
			Refresh();
		}
	}
}
