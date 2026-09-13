using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protobuf;

public class BattlefieldBuildPointInfo : PointInfo
{
	public QuarantinePointInfo detail;

	public List<long> targetEnemyUUID;

	public int assistanceCount;

	public int maxAssistanceCount;

	public BattlefieldBuildPointInfo()
	{
	}

	public BattlefieldBuildPointInfo(WorldPointInfo pi)
		: base(pi)
	{
		detail = pi.QuarantinePoint;
		tileSize = SceneManager.World.GetDragonBuildSizeByItemId(detail.BuildId);
		targetEnemyUUID = detail.TargetUUID.ToArray().ToList();
		UpdateAssistanceCount();
	}

	public override PointInfo Clone()
	{
		BattlefieldBuildPointInfo battlefieldBuildPointInfo = new BattlefieldBuildPointInfo();
		BaseClone(battlefieldBuildPointInfo);
		battlefieldBuildPointInfo.detail = detail;
		battlefieldBuildPointInfo.targetEnemyUUID = detail.TargetUUID.ToArray().ToList();
		return battlefieldBuildPointInfo;
	}

	public override PlayerType GetPlayerType()
	{
		if (detail != null && detail.Role != 0)
		{
			if (!GameEntry.Lua.CallWithReturn<bool, int, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", detail.Role, GameEntry.Data?.Player?.GetWorldType() ?? 0))
			{
				return PlayerType.PlayerAlliance;
			}
			return PlayerType.PlayerOther;
		}
		return PlayerType.PlayerNone;
	}

	public void UpdateAssistanceCount()
	{
		base.PointManager?.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out assistanceCount, out maxAssistanceCount);
	}

	public override void OnDescription(StringBuilder sb)
	{
		base.OnDescription(sb);
		sb.AppendLine($"Role = {detail.Role}");
		sb.AppendLine($"OpenTime = {detail.OpenTime}");
		sb.AppendLine($"State = {detail.State}");
		sb.AppendLine($"BuildId = {detail.BuildId}");
		sb.AppendLine($"EndTime = {detail.BuffEndTime}");
		sb.AppendLine("MarchUid = " + detail.MarchUid);
		int? num = GameEntry.Timer?.GetServerTimeSeconds();
		sb.AppendLine($"CurTime = {num}");
		sb.AppendLine($"TimeGap = {detail.BuffEndTime - num}");
	}
}
