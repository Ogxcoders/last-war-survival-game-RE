using System.Collections.Generic;
using XLua;

public class WorldMarchBattleSound
{
	private readonly HashSet<int> _cityIdSet = new HashSet<int>();

	private readonly Dictionary<int, int> _posToCityId = new Dictionary<int, int>();

	private readonly HashSet<long> _battleMarchSet = new HashSet<long>();

	private int _allSoundCount;

	private int _attackerCount;

	private int _attackerCheckCount;

	private int _battleMarchCount;

	private bool _playSound;

	public void Init()
	{
		UnInit();
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetBattleSoundConfig");
		if (luaTable != null)
		{
			_allSoundCount = luaTable.Get<int>("allSoundCount");
			_attackerCount = luaTable.Get<int>("attackerCount");
			_attackerCheckCount = luaTable.Get<int>("attackerCheckCount");
			luaTable.Get<LuaTable>("cityIdMap")?.ForEach(delegate(int id, bool b)
			{
				_cityIdSet.Add(id);
			});
		}
		GameEntry.Event.Subscribe(EventId.ShowCrossServerTip, OnCrossServer);
		GameEntry.Event.Subscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
		GameEntry.Event.Subscribe(EventId.StopWarBgm, OnStopWarBgm);
		GameEntry.Event.Subscribe(EventId.RefreshWarBgm, OnRefreshWarBgm);
	}

	public void UnInit()
	{
		_battleMarchSet.Clear();
		_cityIdSet.Clear();
		_posToCityId.Clear();
		_allSoundCount = 0;
		_attackerCount = 0;
		_attackerCheckCount = 0;
		_battleMarchCount = 0;
		_playSound = false;
		GameEntry.Event.Unsubscribe(EventId.ShowCrossServerTip, OnCrossServer);
		GameEntry.Event.Unsubscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
		GameEntry.Event.Unsubscribe(EventId.StopWarBgm, OnStopWarBgm);
		GameEntry.Event.Unsubscribe(EventId.RefreshWarBgm, OnRefreshWarBgm);
	}

	public void AddMarch(long marchUuid, int matchTargetPos)
	{
		if (_allSoundCount > 0 && _battleMarchCount < _attackerCheckCount && GameEntry.Data.Player.IsInSelfServer() && marchUuid != 0L && matchTargetPos != 0)
		{
			bool flag = false;
			if (!_posToCityId.TryGetValue(matchTargetPos, out var value))
			{
				value = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetCityIdByPointIndex", matchTargetPos);
				_posToCityId.Add(matchTargetPos, value);
			}
			if (_cityIdSet.Contains(value) && _battleMarchSet.Add(marchUuid))
			{
				flag = true;
				_battleMarchCount++;
			}
			if (flag)
			{
				RefreshSoundPlay();
			}
		}
	}

	public void RemoveMarch(long marchUuid)
	{
		if (_allSoundCount > 0 && _battleMarchCount > 0 && GameEntry.Data.Player.IsInSelfServer() && _battleMarchSet.Remove(marchUuid))
		{
			_battleMarchCount--;
			RefreshSoundPlay();
		}
	}

	public void RefreshSoundPlay()
	{
		bool flag = _battleMarchCount != 0 && _battleMarchCount >= _attackerCount;
		if (flag != _playSound)
		{
			_playSound = flag;
			GameEntry.Lua.Call("CSharpCallLuaInterface.SetBattleSoundPlay", flag);
		}
	}

	private void OnCrossServer(object obj)
	{
		if (!GameEntry.Data.Player.IsInSelfServer() && _playSound)
		{
			_playSound = false;
			GameEntry.Lua.Call("CSharpCallLuaInterface.SetBattleSoundPlay", param1: false);
		}
	}

	private void OnQuitCrossServer(object obj)
	{
		RefreshSoundPlay();
	}

	private void OnStopWarBgm(object obj)
	{
		_playSound = false;
		GameEntry.Lua.Call("CSharpCallLuaInterface.SetBattleSoundPlay", param1: false);
	}

	private void OnRefreshWarBgm(object obj)
	{
		RefreshSoundPlay();
	}
}
