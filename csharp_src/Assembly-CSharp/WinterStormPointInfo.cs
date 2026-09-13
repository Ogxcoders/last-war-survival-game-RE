using System.Collections.Generic;
using System.Linq;
using Protobuf;

public class WinterStormPointInfo : PointInfo
{
	public WinterEntityPointInfo detail;

	public List<long> targetEnemyUUID;

	public int assistanceCount;

	public int maxAssistanceCount;

	public WinterStormPointInfo()
	{
	}

	public WinterStormPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		detail = WinterEntityPointInfo.Parser.ParseFrom(extraInfo);
		targetEnemyUUID = detail.TargetEnemyUUID.ToArray().ToList();
		tileSize = SceneManager.World.GetDragonBuildSizeByItemId(detail.BuildId);
		UpdateAssistanceCount();
	}

	public override PointInfo Clone()
	{
		WinterStormPointInfo winterStormPointInfo = new WinterStormPointInfo();
		BaseClone(winterStormPointInfo);
		winterStormPointInfo.detail = detail;
		winterStormPointInfo.targetEnemyUUID = detail.TargetEnemyUUID.ToArray().ToList();
		winterStormPointInfo.assistanceCount = assistanceCount;
		winterStormPointInfo.maxAssistanceCount = maxAssistanceCount;
		return winterStormPointInfo;
	}

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public override PlayerType GetPlayerType()
	{
		if (detail != null && !detail.OwnerUid.IsNullOrEmpty())
		{
			if (!GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", detail.OwnerUid, 2))
			{
				return PlayerType.PlayerAlliance;
			}
			return PlayerType.PlayerOther;
		}
		return PlayerType.PlayerNone;
	}
}
