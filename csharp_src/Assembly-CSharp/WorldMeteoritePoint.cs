using System;
using System.Text;
using Protobuf;
using UnityEngine;

public class WorldMeteoritePoint : PointInfo
{
	public enum MeteoritePointState
	{
		Default = -1,
		Prepare,
		Drop,
		Collecting,
		Idle
	}

	public int fromPoint = -1;

	public int buildId;

	public int openTime;

	public int collectEndTime;

	public int expireTime;

	public int remainTime;

	public long gatherUUID;

	public string gatherUid;

	public string gatherAllianceId;

	public string targetPic;

	public int targetPicVer;

	public int targetHeadId;

	public long targetHeadET;

	public int maxGatherTime;

	public int lastPoint;

	public int pointPerSec;

	public int VirtualRemainTimeSec
	{
		get
		{
			if (collectEndTime <= 0)
			{
				return remainTime;
			}
			int num = collectEndTime - remainTime;
			int num2 = GameEntry.Timer.GetServerTimeSeconds() - num;
			if (num2 >= 0)
			{
				int num3 = remainTime - num2;
				if (num3 < 0)
				{
					return 0;
				}
				return num3;
			}
			return remainTime;
		}
	}

	public int VirtualRemainTimeMs
	{
		get
		{
			if (collectEndTime <= 0)
			{
				return remainTime * 1000;
			}
			int num = collectEndTime - remainTime;
			int num2 = (int)(GameEntry.Timer.GetServerTime() - (long)num * 1000L);
			if (num2 >= 0)
			{
				int num3 = remainTime * 1000 - num2;
				if (num3 < 0)
				{
					return 0;
				}
				return num3;
			}
			return remainTime * 1000;
		}
	}

	public int VirtualRemainCount => Mathf.Max(VirtualRemainTimeSec, 0) * pointPerSec + lastPoint;

	public int VirtualCollectCount
	{
		get
		{
			if (collectEndTime <= 0)
			{
				return 0;
			}
			int num = collectEndTime - remainTime;
			int num2 = GameEntry.Timer.GetServerTimeSeconds() - num;
			if (num2 <= 0)
			{
				return 0;
			}
			return num2 * pointPerSec;
		}
	}

	public int OpenTimeCountdownSec
	{
		get
		{
			int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
			int num = openTime - serverTimeSeconds;
			if (num < 0)
			{
				return 0;
			}
			return num;
		}
	}

	public int OpenTimeCountdownMs
	{
		get
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			int num = (int)((long)openTime * 1000L - serverTime);
			if (num < 0)
			{
				return 0;
			}
			return num;
		}
	}

	public MeteoritePointState State
	{
		get
		{
			if (gatherUUID > 0)
			{
				return MeteoritePointState.Collecting;
			}
			if (GameEntry.Timer.GetServerTimeSeconds() >= openTime)
			{
				return MeteoritePointState.Idle;
			}
			if (fromPoint > 0)
			{
				return MeteoritePointState.Drop;
			}
			return MeteoritePointState.Prepare;
		}
	}

	public bool IsFull => remainTime >= maxGatherTime;

	public bool IsCollecting => gatherUUID > 0;

	public long ExpiredTime
	{
		get
		{
			if (expireTime > 0)
			{
				return 1000L * (long)expireTime;
			}
			if (collectEndTime > 0)
			{
				return 1000L * (long)collectEndTime;
			}
			return -1L;
		}
	}

	public bool CanCollect
	{
		get
		{
			if (openTime <= 0)
			{
				return false;
			}
			return GameEntry.Timer.GetServerTimeSeconds() >= openTime;
		}
	}

	public WorldMeteoritePoint()
	{
	}

	public WorldMeteoritePoint(WorldPointInfo pi)
		: base(pi)
	{
		MeteoritePoint meteoritePoint = pi.MeteoritePoint;
		if (meteoritePoint != null)
		{
			fromPoint = meteoritePoint.FromPoint;
			buildId = meteoritePoint.BuildId;
			openTime = meteoritePoint.OpenTime;
			collectEndTime = meteoritePoint.CollectEndTime;
			expireTime = meteoritePoint.ExpireTime;
			remainTime = meteoritePoint.RemainTime;
			gatherUUID = meteoritePoint.GatherUUID;
			gatherUid = meteoritePoint.GatherUid;
			gatherAllianceId = meteoritePoint.GatherAllianceId;
			targetPic = meteoritePoint.TargetPic;
			targetPicVer = meteoritePoint.TargetPicVer;
			targetHeadId = meteoritePoint.TargetHeadId;
			targetHeadET = meteoritePoint.TargetHeadET;
			int.TryParse(GameEntry.ConfigCache.GetTemplateData("yuntie_battle_entity", buildId, "occupy_time"), out maxGatherTime);
			int.TryParse(GameEntry.ConfigCache.GetTemplateData("yuntie_battle_entity", buildId, "point_last"), out lastPoint);
			int.TryParse(GameEntry.ConfigCache.GetTemplateData("yuntie_battle_entity", buildId, "point_produce_per_second"), out pointPerSec);
		}
	}

	public override PointInfo Clone()
	{
		WorldMeteoritePoint worldMeteoritePoint = new WorldMeteoritePoint();
		BaseClone(worldMeteoritePoint);
		worldMeteoritePoint.fromPoint = fromPoint;
		worldMeteoritePoint.buildId = buildId;
		worldMeteoritePoint.openTime = openTime;
		worldMeteoritePoint.collectEndTime = collectEndTime;
		worldMeteoritePoint.expireTime = expireTime;
		worldMeteoritePoint.remainTime = remainTime;
		worldMeteoritePoint.gatherUUID = gatherUUID;
		worldMeteoritePoint.gatherUid = gatherUid;
		worldMeteoritePoint.gatherAllianceId = gatherAllianceId;
		worldMeteoritePoint.targetPic = targetPic;
		worldMeteoritePoint.targetPicVer = targetPicVer;
		worldMeteoritePoint.targetHeadId = targetHeadId;
		worldMeteoritePoint.targetHeadET = targetHeadET;
		worldMeteoritePoint.maxGatherTime = maxGatherTime;
		return worldMeteoritePoint;
	}

	private string FormatTime(int time)
	{
		return DateTimeOffset.FromUnixTimeSeconds(time).UtcDateTime.ToString("yyyy-M-d hh:mm:ss");
	}

	public override void OnDescription(StringBuilder sb)
	{
		int time = GameEntry.Timer?.GetServerTimeSeconds() ?? 0;
		sb.AppendLine("--MeteoritePointDetail--");
		sb.AppendLine($"id:{buildId}");
		sb.AppendLine($"fromPoint:{fromPoint}");
		sb.AppendLine("openTime:" + FormatTime(openTime));
		sb.AppendLine("collectEndTime:" + FormatTime(collectEndTime));
		sb.AppendLine("expireTime:" + FormatTime(expireTime));
		sb.AppendLine($"remainTime:{remainTime}");
		sb.AppendLine("当前时间:" + FormatTime(time));
		sb.AppendLine($"gatherUUID:{gatherUUID}");
		sb.AppendLine("gatherUid:" + gatherUid);
		sb.AppendLine("gatherAllianceId:" + gatherAllianceId);
		sb.AppendLine("gatherPic:" + targetPic);
		sb.AppendLine($"gatherPicVer:{targetPicVer}");
		sb.AppendLine($"maxGatherTime:{maxGatherTime}");
		sb.AppendLine($"lastPoint:{lastPoint}");
		sb.AppendLine($"pointPerSec:{pointPerSec}");
		sb.AppendLine($"VirtualRemainTime:{VirtualRemainTimeSec}");
		sb.AppendLine($"VirtualRemainCount:{VirtualRemainCount}");
		sb.AppendLine($"VirtualCollectCount:{VirtualCollectCount}");
	}
}
