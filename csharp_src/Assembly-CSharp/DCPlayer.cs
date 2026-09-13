using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using XLua;

public class DCPlayer : BaseDataContainer
{
	public string Uid;

	private int serverId;

	private long mainUuid;

	private string allianceId;

	private string allianceLeaderID;

	private string fightAllianceId = "";

	private HashSet<int> serverDic = new HashSet<int>();

	private Dictionary<string, long> attackerDic = new Dictionary<string, long>();

	private Dictionary<string, int> allianceServerCamp = new Dictionary<string, int>();

	private int curWorldId;

	private int curWorldType;

	private int crossServerId = -1;

	private int crossFightSrcServerId = -1;

	private HashSet<long> _alreadyReceiveBerserkBossRewardDic = new HashSet<long>();

	private Dictionary<string, bool> dataConfigSwitchStates = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

	private long? immediateToggle;

	private bool? isPlayerInSeasonOrHalt;

	private Dictionary<string, string> dataCache = new Dictionary<string, string>();

	public bool IsBattleFieldWatching { get; set; }

	public int PlayerWorldPointId { get; private set; } = -1;

	public string GetUid()
	{
		return Uid;
	}

	public bool IsInSourceServer()
	{
		return GetCurServerId() == GetSourceServerId();
	}

	public int GetSourceServerId()
	{
		if (crossFightSrcServerId == -1)
		{
			return serverId;
		}
		return crossFightSrcServerId;
	}

	public void SetCrossFightSrcServerId(int serverId)
	{
		crossFightSrcServerId = serverId;
	}

	public int GetCrossFightSrcServerId()
	{
		if (crossFightSrcServerId == -1)
		{
			return GetCurServerId();
		}
		return crossFightSrcServerId;
	}

	public int GetSelfServerId()
	{
		return serverId;
	}

	public int GetCrossServerId()
	{
		return crossServerId;
	}

	public long GetMainUuid()
	{
		if (mainUuid == 0L)
		{
			mainUuid = GameEntry.Lua.CallWithReturn<long>("CSharpCallLuaInterface.GetMainUuid");
		}
		return mainUuid;
	}

	public int GetCurServerId()
	{
		if (crossServerId >= 0)
		{
			return crossServerId;
		}
		return serverId;
	}

	public void SetWorldId(int worldId)
	{
		curWorldId = worldId;
	}

	public int GetWorldId()
	{
		return curWorldId;
	}

	public void SetWorldType(int worldType)
	{
		curWorldType = worldType;
	}

	public int GetWorldType()
	{
		return curWorldType;
	}

	public BattleFieldType GetBattleFieldType()
	{
		return (BattleFieldType)curWorldType;
	}

	public void UpdatePlayerWorldPointId(int playerPointId)
	{
		PlayerWorldPointId = playerPointId;
	}

	public int GetSrcWorldId()
	{
		return 0;
	}

	public bool IsInBattleField(int wType = -1)
	{
		bool flag = curWorldId > 0;
		if (wType == -1)
		{
			if (flag)
			{
				return curWorldType != 0;
			}
			return false;
		}
		if (flag)
		{
			return curWorldType == wType;
		}
		return false;
	}

	public void OnCrossServerId(int targetServerId)
	{
		crossServerId = targetServerId;
	}

	public string GetName()
	{
		return GameEntry.Lua.GetValue_String("LuaEntry.Player", "name");
	}

	public bool IsInSelfServer()
	{
		if (crossServerId >= 0 && crossServerId != serverId)
		{
			return false;
		}
		return true;
	}

	public string GetAllianceId()
	{
		return allianceId ?? (allianceId = GameEntry.Lua.CallWithReturn<string>("LuaEntry.Player:GetAllianceUid"));
	}

	public void SetAllianceId(string allianceId)
	{
		this.allianceId = allianceId;
	}

	public string GetAllianceLeaderID()
	{
		return allianceLeaderID ?? (allianceLeaderID = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetAllianceLeaderUid"));
	}

	public void SetAllianceLeaderID(string leaderID)
	{
		allianceLeaderID = leaderID;
	}

	public T GetValue<T>(string strKey)
	{
		return GameEntry.Lua.GetValue<T>("LuaEntry.Player", strKey);
	}

	public void SetValue<T>(string strKey, T value)
	{
		GameEntry.Lua.SetValue("LuaEntry.Player", strKey, value);
	}

	public override void CSInit(ISFSObject obj)
	{
		if (obj.ContainsKey("user"))
		{
			UpdateUser(obj.GetSFSObject("user"));
		}
		if (obj.ContainsKey("crossWormObj"))
		{
			ISFSObject sFSObject = obj.GetSFSObject("crossWormObj");
			if (sFSObject.ContainsKey("worldId"))
			{
				int worldId = sFSObject.TryGetInt("worldId");
				SetWorldId(worldId);
				GameEntry.GlobalData.serverType = sFSObject.TryGetInt("serverType");
			}
		}
		else
		{
			if (!obj.ContainsKey("dragonObj"))
			{
				return;
			}
			ISFSObject sFSObject2 = obj.GetSFSObject("dragonObj");
			if (sFSObject2.ContainsKey("worldId"))
			{
				int num = sFSObject2.TryGetInt("worldId");
				SetWorldId(num);
				if (num > 0)
				{
					GameEntry.GlobalData.serverType = 8;
				}
			}
		}
	}

	public void UpdateUser(ISFSObject user)
	{
		Uid = user.GetUtfString("uid");
		serverId = user.GetInt("serverId");
		GameEntry.GlobalData.serverType = user.GetInt("serverType");
		GameEntry.GlobalData.serverMax = user.GetInt("serverMax");
		if (user.ContainsKey("deviceBindTimes"))
		{
			GameEntry.GlobalData.nowGameCnt = user.TryGetInt("deviceBindTimes");
		}
	}

	public void SetFightAllianceId(string allianceId)
	{
		fightAllianceId = allianceId;
	}

	public string GetFightAllianceId()
	{
		return fightAllianceId;
	}

	public void SetFightServerList(LuaTable serverList)
	{
		int mySourceServerId = GameEntry.Data.Player.GetCrossFightSrcServerId();
		serverDic.Clear();
		if (serverList.Length <= 0)
		{
			return;
		}
		serverList.ForEach(delegate(int sId, int data)
		{
			if (sId > 0 && sId != mySourceServerId)
			{
				serverDic.Add(sId);
			}
		});
	}

	public bool GetIsInFightServerList(int sId)
	{
		return serverDic.Contains(sId);
	}

	public void SetAttackInfoList(LuaTable attackerList)
	{
		attackerDic.Clear();
		if (attackerList.Length <= 0)
		{
			return;
		}
		attackerList.ForEach(delegate(int _, LuaTable data)
		{
			string text = data.Get<string>("uid");
			long num = data.Get<long>("endTime");
			if (!text.IsNullOrEmpty() && num > 0)
			{
				attackerDic.Add(text, num);
			}
		});
	}

	public bool GetIsInAttackDic(string uid)
	{
		if (attackerDic.ContainsKey(uid))
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			if (attackerDic[uid] > serverTime)
			{
				return true;
			}
		}
		return false;
	}

	public void SetAllianceServerCamp(LuaTable campList)
	{
		allianceServerCamp.Clear();
		if (campList.Length <= 0)
		{
			return;
		}
		campList.ForEach(delegate(int _, LuaTable data)
		{
			string text = data.Get<string>("allianceId");
			int num = data.Get<int>("camp");
			if (!text.IsNullOrEmpty() && num > 0)
			{
				allianceServerCamp.Add(text, num);
			}
		});
	}

	public bool IsAllianceSelfCamp(string allianceId)
	{
		string text = GetAllianceId();
		if (!text.IsNullOrEmpty() && !allianceId.IsNullOrEmpty() && allianceServerCamp.ContainsKey(text) && allianceServerCamp.ContainsKey(allianceId) && allianceServerCamp[text] == allianceServerCamp[allianceId])
		{
			return true;
		}
		return false;
	}

	public int GetAllianceCampByAllianceId(string allianceId)
	{
		if (allianceId.IsNullOrEmpty())
		{
			return -1;
		}
		if (allianceServerCamp.ContainsKey(allianceId))
		{
			return allianceServerCamp[allianceId];
		}
		return -1;
	}

	public string GetData(string key)
	{
		if (dataCache.ContainsKey(key))
		{
			return dataCache[key];
		}
		return null;
	}

	public void SetData(string key, string value)
	{
		dataCache[key] = value;
	}

	public void DeleteData(string key)
	{
		dataCache.Remove(key);
	}

	public Dictionary<string, string> GetAllData()
	{
		return dataCache;
	}

	public void SetReceiveBerserkBossRewardDic(LuaTable receiveBossUuidList)
	{
		_alreadyReceiveBerserkBossRewardDic.Clear();
		if (receiveBossUuidList.Length <= 0)
		{
			return;
		}
		receiveBossUuidList.ForEach(delegate(int _, long bossUuid)
		{
			if (bossUuid > 0)
			{
				_alreadyReceiveBerserkBossRewardDic.Add(bossUuid);
			}
		});
	}

	public bool GetIsAlreadyBerserkBossReward(long bossUuid)
	{
		return _alreadyReceiveBerserkBossRewardDic.Contains(bossUuid);
	}

	public bool CheckSwitch(string key, bool defaultVal)
	{
		if (dataConfigSwitchStates.TryGetValue(key, out var value))
		{
			return value;
		}
		int num = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.CheckSwitchSafe", key);
		switch (num)
		{
		case 0:
		case 1:
			value = num > 0;
			dataConfigSwitchStates.Add(key, value);
			break;
		case -1:
			return defaultVal;
		}
		return value;
	}

	public bool CheckImmediateSwitch(int key, bool defaultVal)
	{
		if (!immediateToggle.HasValue)
		{
			return defaultVal;
		}
		return (immediateToggle & (1L << key)) != 0;
	}

	public void SyncImmediateSwitch(long? immediateToggle)
	{
		this.immediateToggle = immediateToggle;
	}

	public bool IsPlayerInSeasonOrHalt()
	{
		if (!isPlayerInSeasonOrHalt.HasValue)
		{
			isPlayerInSeasonOrHalt = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsPlayerInSeasonOrHalt");
		}
		return isPlayerInSeasonOrHalt.Value;
	}

	public string GetRegisterCountry()
	{
		return GameEntry.Lua.GetValue_String("LuaEntry.Player", "regCountry");
	}

	public void ClearAllCacheSwitch()
	{
		dataConfigSwitchStates?.Clear();
	}
}
