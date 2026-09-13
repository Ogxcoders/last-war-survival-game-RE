using System.Collections.Generic;
using System.Linq;
using BaseUtils;
using GameFramework;
using Protobuf;

public class TreasurePointInfo : PointInfo
{
	public string eventId;

	public long startTime;

	public long completionTime;

	public string allianceId;

	public string allianceAbbr;

	public List<string> rewardUserList;

	public List<UserInfo> diggingUserList;

	public bool complete;

	public float speed;

	public string ownerName;

	public long expireTime;

	public long createTime;

	public HashSet<string> rewardUserHashSet;

	public HashSet<string> diggingUserHashSet;

	public WorldTreasureType type = WorldTreasureType.RadarTreasure;

	public int fromPoint;

	public int multiple;

	public string killerId;

	public string customInfoStr;

	public TreasurePointInfo()
	{
	}

	public TreasurePointInfo(WorldPointInfo pi)
		: base(pi)
	{
		Protobuf.TreasurePointInfo treasurePointInfo = pi.TreasurePointInfo;
		eventId = treasurePointInfo.EventId;
		startTime = treasurePointInfo.StartTime;
		completionTime = treasurePointInfo.CompletionTime;
		allianceId = treasurePointInfo.AllianceId;
		allianceAbbr = treasurePointInfo.AllianceAbbr;
		rewardUserList = treasurePointInfo.RewardUserList.ToArray().ToList();
		diggingUserList = treasurePointInfo.DiggingUserList.ToArray().ToList();
		complete = treasurePointInfo.Complete;
		speed = treasurePointInfo.Speed;
		ownerName = treasurePointInfo.OwnerName;
		expireTime = treasurePointInfo.ExpireTime;
		createTime = treasurePointInfo.CreateTime;
		ownerUid = treasurePointInfo.OwnerUid;
		fromPoint = treasurePointInfo.FromPoint;
		type = (WorldTreasureType)treasurePointInfo.Type;
		multiple = treasurePointInfo.Multiple;
		killerId = treasurePointInfo.KillerId;
		customInfoStr = treasurePointInfo.CustomInfo;
		if (killerId == GameEntry.Data.Player.Uid)
		{
			Log.Info($"Recv with killerId {uuid} , {mainIndex} , {pointIndex}");
		}
		if (type == WorldTreasureType.RadarTreasure || type == WorldTreasureType.ActivityRadarTreasure || type == WorldTreasureType.WolfShadow || type == WorldTreasureType.OffSeasonDetect)
		{
			tileSize = 3;
		}
		else
		{
			tileSize = SceneManager.World.GetTreasureSizeByItemId(int.Parse(eventId));
		}
		rewardUserHashSet = new HashSet<string>();
		for (int i = 0; i < rewardUserList.Count; i++)
		{
			rewardUserHashSet.Add(rewardUserList[i]);
		}
		diggingUserHashSet = new HashSet<string>();
		for (int j = 0; j < diggingUserList.Count; j++)
		{
			diggingUserHashSet.Add(diggingUserList[j].Uid);
		}
		if (treasurePointInfo.ThermalConductor != null)
		{
			thermalConductor = new ThermalConductor(treasurePointInfo.ThermalConductor);
		}
	}

	public int GetWorldTreasureType()
	{
		return (int)type;
	}

	public override PointInfo Clone()
	{
		TreasurePointInfo treasurePointInfo = new TreasurePointInfo();
		BaseClone(treasurePointInfo);
		treasurePointInfo.eventId = eventId;
		treasurePointInfo.startTime = startTime;
		treasurePointInfo.completionTime = completionTime;
		treasurePointInfo.allianceId = allianceId;
		treasurePointInfo.type = type;
		treasurePointInfo.allianceAbbr = allianceAbbr;
		treasurePointInfo.rewardUserList = rewardUserList;
		treasurePointInfo.diggingUserList = diggingUserList;
		treasurePointInfo.complete = complete;
		treasurePointInfo.speed = speed;
		treasurePointInfo.ownerName = ownerName;
		treasurePointInfo.expireTime = expireTime;
		treasurePointInfo.createTime = createTime;
		treasurePointInfo.rewardUserHashSet = rewardUserHashSet;
		treasurePointInfo.diggingUserHashSet = diggingUserHashSet;
		treasurePointInfo.ownerUid = ownerUid;
		treasurePointInfo.fromPoint = fromPoint;
		treasurePointInfo.multiple = multiple;
		treasurePointInfo.killerId = killerId;
		return treasurePointInfo;
	}

	public bool IsHaveGetReward(string playerUid)
	{
		if (rewardUserHashSet != null && rewardUserHashSet.Contains(playerUid))
		{
			return true;
		}
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.CheckTreasureReachDailyLimit", eventId.ToInt());
	}

	public bool IsHaveWorking(string playerUid)
	{
		if (diggingUserHashSet == null)
		{
			return false;
		}
		return diggingUserHashSet.Contains(playerUid);
	}

	public int GetRewardMaxNum()
	{
		int result = 1;
		string[] array = BaseUtils.StringUtils.SplitString(GameEntry.ConfigCache.GetTemplateData("detect_event", eventId.ToInt(), "para2"), ';');
		if (array.Length >= 2)
		{
			result = array[1].ToInt();
		}
		return result;
	}

	public bool IsReceiveAllReward()
	{
		int num = multiple;
		if (num <= 0)
		{
			num = 1;
		}
		int num2 = GetRewardMaxNum() * num;
		return rewardUserList.Count >= num2;
	}

	public bool IsComplete()
	{
		if (!complete)
		{
			if (completionTime > 0)
			{
				return completionTime <= GameEntry.Timer.GetServerTime();
			}
			return false;
		}
		return true;
	}
}
