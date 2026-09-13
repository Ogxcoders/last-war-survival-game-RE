using System.Collections.Generic;
using UnityEngine;

public class BasementGetResourceWater : CityBuilding
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

	[SerializeField]
	private Renderer _renderer;

	[SerializeField]
	private GameObject _proModel;

	private MaterialPropertyBlock _propBlock;

	private PercentState _statePro;

	private bool _isAnim;

	private PercentState _lastState;

	private float _time;

	public static float AnimUseTime = 1.5f;

	private int _buildId;

	private float _lastHeight;

	private float _willHeight;

	private ITimer _timer;

	private int _realPercentFewValue = 5;

	public static Dictionary<PercentState, float> WaterRealHeight = new Dictionary<PercentState, float>
	{
		{
			PercentState.None,
			1f
		},
		{
			PercentState.Zero,
			1f
		},
		{
			PercentState.Few,
			1.15f
		},
		{
			PercentState.Middle,
			1.5f
		},
		{
			PercentState.Max,
			3f
		}
	};

	public static Dictionary<PercentState, float> GasRealHeight = new Dictionary<PercentState, float>
	{
		{
			PercentState.None,
			1f
		},
		{
			PercentState.Zero,
			0.3f
		},
		{
			PercentState.Few,
			0.8f
		},
		{
			PercentState.Middle,
			1.2f
		},
		{
			PercentState.Max,
			3f
		}
	};

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		_propBlock = new MaterialPropertyBlock();
		_statePro = PercentState.None;
		RemoveTimer();
		_isAnim = false;
		GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
		Refresh();
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		_isAnim = false;
		RemoveTimer();
		_propBlock = null;
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
	}

	private void UpdateBuildDataSignal(object userData)
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
		if (buildingDataByUuid.buildUpdateTime > 0)
		{
			_proModel.gameObject.SetActive(value: false);
			return;
		}
		_proModel.gameObject.SetActive(value: true);
		float num = GameEntry.Lua.CallWithReturn<float, int, int>("CSharpCallLuaInterface.GetShowBubblePercent", _buildId, buildingDataByUuid.level);
		_realPercentFewValue = (int)num * 100;
		int state = (int)(GameEntry.Lua.CallWithReturn<float, long>("CSharpCallLuaInterface.GetBuildDataResourcePercent", base.Uuid) * 100f);
		PercentState percentStateByTrueState = GetPercentStateByTrueState(state);
		if (_statePro != percentStateByTrueState)
		{
			_lastState = _statePro;
			_statePro = percentStateByTrueState;
			_time = 0f;
			_lastHeight = GetRealHeightByBuildId(_buildId, _lastState);
			_willHeight = GetRealHeightByBuildId(_buildId, _statePro);
			_isAnim = true;
		}
		AddTimer();
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

	private void Update()
	{
		if (_isAnim)
		{
			_time += Time.deltaTime;
			if (_time > AnimUseTime)
			{
				_isAnim = false;
				_time = 0f;
				SetHeightPro(GetRealHeightByBuildId(_buildId, _statePro));
			}
			else
			{
				float heightPro = _time / AnimUseTime * (_willHeight - _lastHeight) + _lastHeight;
				SetHeightPro(heightPro);
			}
		}
	}

	private float GetRealHeightByBuildId(int buildId, PercentState state)
	{
		return buildId switch
		{
			432000 => WaterRealHeight[state], 
			413000 => GasRealHeight[state], 
			_ => 0f, 
		};
	}

	private void SetHeightPro(float value)
	{
		_propBlock.SetFloat("_ManualControl", value);
		_renderer.SetPropertyBlock(_propBlock);
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

	private void RemoveTimer()
	{
		if (_timer != null)
		{
			GameEntry.Timer.CancelTimer(_timer);
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
