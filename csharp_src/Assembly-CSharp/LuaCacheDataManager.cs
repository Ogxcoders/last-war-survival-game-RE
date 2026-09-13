using System.Collections.Generic;

public class LuaCacheDataManager : WorldManagerBase
{
	private Dictionary<int, Dictionary<string, PlayerType>> playerTypeCache = new Dictionary<int, Dictionary<string, PlayerType>>();

	public LuaCacheDataManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
	}

	public override void UnInit()
	{
		CleanAllianceCacheData();
	}

	public void CleanAllianceCacheData()
	{
		foreach (Dictionary<string, PlayerType> value in playerTypeCache.Values)
		{
			value.Clear();
		}
		playerTypeCache.Clear();
	}

	public void OnWorldColorDirty(string allianceId)
	{
		foreach (KeyValuePair<int, Dictionary<string, PlayerType>> item in playerTypeCache)
		{
			if (item.Value.ContainsKey(allianceId))
			{
				item.Value.Remove(allianceId);
			}
		}
	}

	public PlayerType IsMyEnemy(int serverId, string allianceId)
	{
		if (serverId <= 0 && allianceId.IsNullOrEmpty())
		{
			return PlayerType.PlayerNone;
		}
		if (!playerTypeCache.TryGetValue(serverId, out var value))
		{
			value = new Dictionary<string, PlayerType>();
			playerTypeCache[serverId] = value;
		}
		if (!value.TryGetValue(allianceId, out var value2))
		{
			value2 = (value[allianceId] = GameEntry.Lua.CallWithReturn<PlayerType, int, string, string>("CSharpCallLuaInterface.CheckPlayerType", serverId, allianceId, null));
		}
		return value2;
	}
}
