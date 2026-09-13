using System.Collections.Generic;
using System.Linq;
using Protobuf;

public class GhostreconPointInfo : PointInfo
{
	public int cfgId;

	public long completionTime;

	public List<string> stealList;

	public List<GhostReconMemberData> memberList;

	public int ownerServer;

	public long taskExpireTime;

	public string allianceId;

	public int size;

	public long actEndTime;

	public long teamStartTime;

	public GhostreconPointInfo()
	{
	}

	public GhostreconPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		GhostReconPointInfo ghostReconPointInfo = pi.GhostReconPointInfo;
		ownerUid = ghostReconPointInfo.OwnerUid;
		cfgId = ghostReconPointInfo.CfgId;
		completionTime = ghostReconPointInfo.CompletionTime;
		stealList = ghostReconPointInfo.StealList.ToArray().ToList();
		memberList = ghostReconPointInfo.MemberList.ToArray().ToList();
		ownerServer = ghostReconPointInfo.OwnerServer;
		taskExpireTime = ghostReconPointInfo.TaskExpireTime;
		allianceId = ghostReconPointInfo.AllianceId;
		size = ghostReconPointInfo.Size;
		actEndTime = ghostReconPointInfo.ActEndTime;
		teamStartTime = ghostReconPointInfo.TeamStartTime;
		tileSize = size;
	}

	public override PointInfo Clone()
	{
		GhostreconPointInfo ghostreconPointInfo = new GhostreconPointInfo();
		BaseClone(ghostreconPointInfo);
		ghostreconPointInfo.cfgId = cfgId;
		ghostreconPointInfo.completionTime = completionTime;
		ghostreconPointInfo.stealList = stealList;
		ghostreconPointInfo.memberList = memberList;
		ghostreconPointInfo.ownerServer = ownerServer;
		ghostreconPointInfo.taskExpireTime = taskExpireTime;
		ghostreconPointInfo.allianceId = allianceId;
		ghostreconPointInfo.size = size;
		ghostreconPointInfo.actEndTime = actEndTime;
		ghostreconPointInfo.teamStartTime = teamStartTime;
		ghostreconPointInfo.tileSize = tileSize;
		return ghostreconPointInfo;
	}

	public bool OwnHaveReward()
	{
		bool result = false;
		for (int i = 0; i < memberList.Count; i++)
		{
			if (memberList[i].MemberUid == GameEntry.Data.Player.Uid && memberList[i].Rewarded == 0)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool OwnIsJoin()
	{
		bool result = false;
		for (int i = 0; i < memberList.Count; i++)
		{
			if (memberList[i].MemberUid == GameEntry.Data.Player.Uid)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool OwnIsAlly()
	{
		if (!string.IsNullOrEmpty(allianceId))
		{
			return allianceId == GameEntry.Data.Player.GetAllianceId();
		}
		return false;
	}

	public bool OwnIsLeader()
	{
		return ownerUid == GameEntry.Data.Player.Uid;
	}

	public bool OwnCanReward()
	{
		bool result = false;
		for (int i = 0; i < memberList.Count; i++)
		{
			if (memberList[i].MemberUid == GameEntry.Data.Player.Uid && memberList[i].CanReward == 1)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool GetRewardedByUid(long uid)
	{
		bool result = false;
		for (int i = 0; i < memberList.Count; i++)
		{
			if (memberList[i].MemberUid == uid.ToString())
			{
				result = memberList[i].Rewarded == 1;
				break;
			}
		}
		return result;
	}
}
