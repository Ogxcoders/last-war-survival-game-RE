using System.Text;
using Protobuf;
using Sfs2X.Util;

public class WorldAoiAssistanceInfos
{
	private NestedDictionary<int, int, int> pointId2Number = new NestedDictionary<int, int, int>();

	private NestedDictionary<int, int, int> pointId2MaxNumber = new NestedDictionary<int, int, int>();

	private NestedDictionary<int, int, bool> dirtyPoints = new NestedDictionary<int, int, bool>();

	public bool IsDirty => dirtyPoints.Count != 0;

	public NestedDictionary<int, int, bool> GetDirtyPoints(bool cloneIt)
	{
		if (cloneIt)
		{
			NestedDictionary<int, int, bool> nestedDictionary = new NestedDictionary<int, int, bool>();
			dirtyPoints.CloneTo(nestedDictionary);
			return nestedDictionary;
		}
		return dirtyPoints;
	}

	public void UpdateFromBlock(ByteArray message, int serverId, int worldId)
	{
		WorldAoiAssistanceInfoMsg worldAoiAssistanceInfoMsg = WorldAoiAssistanceInfoMsg.Parser.ParseFrom(message.Bytes);
		if (worldAoiAssistanceInfoMsg == null || !(worldAoiAssistanceInfoMsg.List?.Count > 0))
		{
			return;
		}
		int i = 0;
		for (int count = worldAoiAssistanceInfoMsg.List.Count; i < count; i++)
		{
			AllianceAssistanceInfo allianceAssistanceInfo = worldAoiAssistanceInfoMsg.List[i];
			int pointId = allianceAssistanceInfo.PointId;
			int number = allianceAssistanceInfo.Number;
			if (pointId2Number.TryGetValue(serverId, pointId, out var value))
			{
				if (value != number)
				{
					dirtyPoints[serverId, pointId] = true;
					pointId2Number[serverId, pointId] = number;
					pointId2MaxNumber[serverId, pointId] = allianceAssistanceInfo.Max;
				}
			}
			else
			{
				pointId2Number[serverId, pointId] = number;
				pointId2MaxNumber[serverId, pointId] = allianceAssistanceInfo.Max;
				dirtyPoints[serverId, pointId] = true;
			}
		}
	}

	public void UpdateSingleInfo(ByteArray message, int serverId, int worldId)
	{
		AllianceAssistanceInfo allianceAssistanceInfo = AllianceAssistanceInfo.Parser.ParseFrom(message.Bytes);
		if (allianceAssistanceInfo == null)
		{
			return;
		}
		int pointId = allianceAssistanceInfo.PointId;
		int number = allianceAssistanceInfo.Number;
		if (pointId2Number.TryGetValue(serverId, pointId, out var value))
		{
			if (value != number)
			{
				dirtyPoints[serverId, pointId] = true;
				pointId2Number[serverId, pointId] = number;
				pointId2MaxNumber[serverId, pointId] = allianceAssistanceInfo.Max;
			}
		}
		else
		{
			pointId2Number[serverId, pointId] = number;
			pointId2MaxNumber[serverId, pointId] = allianceAssistanceInfo.Max;
			dirtyPoints[serverId, pointId] = true;
		}
		GameEntry.Event.Fire(EventId.UIRefreshAssistanceDetailInfo, pointId);
	}

	public bool TryGetAssistanceCount(int serverId, int pointId, out int count, out int max)
	{
		count = 0;
		max = 0;
		if (pointId2Number.TryGetValue(serverId, pointId, out count))
		{
			pointId2MaxNumber.TryGetValue(serverId, pointId, out max);
			return true;
		}
		return false;
	}

	public int FillDirtyPoints(NestedDictionary<int, int, bool> outList)
	{
		if (outList == null)
		{
			dirtyPoints.Clear();
			return 0;
		}
		int result = dirtyPoints.CloneTo(outList);
		dirtyPoints.Clear();
		return result;
	}

	public void ClearDirtyPoints()
	{
		dirtyPoints.Clear();
	}

	public void Clear()
	{
		pointId2Number.Clear();
		pointId2MaxNumber.Clear();
		dirtyPoints.Clear();
	}

	public void RemoveByPointIndex(int serverId, int pointIndex)
	{
		pointId2Number.Remove(serverId, pointIndex);
		pointId2MaxNumber.Remove(serverId, pointIndex);
	}

	public string Description()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("--大世界驻防信息--");
		sb.AppendLine($"保存驻防数量:{pointId2Number.Count}[{pointId2MaxNumber.Count}]");
		if (pointId2Number.Count > 0)
		{
			pointId2Number.ForEach(delegate(int serverId, int pointIndex, int count)
			{
				sb.AppendLine($"{serverId}:{pointIndex}:{count}");
			});
		}
		sb.AppendLine($"IsDirty:{IsDirty}, dirtyCount:{dirtyPoints.Count}");
		return sb.ToString();
	}
}
