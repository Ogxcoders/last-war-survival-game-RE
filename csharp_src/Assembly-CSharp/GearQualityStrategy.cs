using System;
using System.Collections.Generic;

public class GearQualityStrategy : ILODStrategy
{
	private static int _curGearQuality = -1;

	private static bool _isSubscribed = false;

	private static Action<object> _eventHandler;

	public GearQualityStrategy()
	{
		if (!_isSubscribed && GameEntry.Event != null)
		{
			if (_eventHandler == null)
			{
				_eventHandler = OnQualityLevelChanged;
			}
			GameEntry.Event.Subscribe(EventId.GameQualityLevelChanged, _eventHandler);
			_isSubscribed = true;
		}
		RefreshGearQuality();
	}

	private static void OnQualityLevelChanged(object userData)
	{
		RefreshGearQuality();
	}

	private static void RefreshGearQuality()
	{
		if (GameEntry.Lua != null && GameEntry.Lua.HasGameStart)
		{
			_curGearQuality = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetGearQuality");
		}
	}

	public int CalculateLODLevel(int currentLevel, List<ISceneLODNode> nodes)
	{
		if (_curGearQuality == -1)
		{
			RefreshGearQuality();
		}
		return _curGearQuality;
	}

	public static void Unsubscribe()
	{
		if (_isSubscribed && GameEntry.Event != null && _eventHandler != null)
		{
			GameEntry.Event.Unsubscribe(EventId.GameQualityLevelChanged, _eventHandler);
			_isSubscribed = false;
		}
	}
}
