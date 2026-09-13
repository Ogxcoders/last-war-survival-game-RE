using Protobuf;

public class WorldDesertInfo
{
	public int pointIndex;

	public int desertId;

	public int oriDesertId;

	public long uuid;

	public int serverId;

	public int srcServerId;

	public string ownerUid;

	public string allianceId;

	public long protectEndTime;

	public bool hasAssistance;

	public int mineId;

	public UserInfo lastAssistanceUser;

	public long fireEndTime;

	public long giveUpTime;

	public WorldDesertInfo()
	{
	}

	public WorldDesertInfo(DesertInfo di)
	{
		pointIndex = di.Id;
		uuid = di.Uuid;
		desertId = di.DesertId;
		serverId = di.ServerId;
		srcServerId = di.OwnerServer;
		allianceId = di.AllianceId;
		protectEndTime = di.ProtectEndTime / 1000;
		ownerUid = di.Uid;
		hasAssistance = di.HasAssistance;
		mineId = di.MineId;
		oriDesertId = di.OriDesertId;
		lastAssistanceUser = di.UserInfo;
		fireEndTime = di.FireEndTime;
		giveUpTime = di.GiveUpTime / 1000;
	}

	public PlayerType GetPlayerType()
	{
		if (ownerUid.IsNullOrEmpty() && allianceId.IsNullOrEmpty())
		{
			return PlayerType.PlayerNone;
		}
		if (GameEntry.Data.Player.GetUid() == ownerUid)
		{
			return PlayerType.PlayerSelf;
		}
		string text = GameEntry.Data.Player.GetAllianceId();
		if (string.IsNullOrEmpty(text))
		{
			return PlayerType.PlayerOther;
		}
		if (text == allianceId)
		{
			return PlayerType.PlayerAlliance;
		}
		return PlayerType.PlayerOther;
	}

	public bool IsRed()
	{
		return false;
	}

	public bool IsYellow()
	{
		string fightAllianceId = GameEntry.Data.Player.GetFightAllianceId();
		if (!fightAllianceId.IsNullOrEmpty() && fightAllianceId == allianceId)
		{
			return true;
		}
		if (GameEntry.Data.Player.GetIsInFightServerList(srcServerId))
		{
			return true;
		}
		if (GameEntry.Data.Player.GetIsInAttackDic(ownerUid))
		{
			return true;
		}
		if (GameEntry.GlobalData.serverType == 9)
		{
			if (GameEntry.Data.Player.IsAllianceSelfCamp(allianceId))
			{
				return false;
			}
			return true;
		}
		return false;
	}
}
