using System.Collections.Generic;
using System.Text;
using Sfs2X.Entities.Data;

public class WorldALPointsInfos
{
	private long leaderPosition;

	private List<long> memberPositions;

	private long Encode(int serverId, int positionIndex)
	{
		return ((long)serverId << 32) | (uint)positionIndex;
	}

	private void Decode(long val, out int serverId, out int positionIndex)
	{
		serverId = (int)(val >> 32);
		positionIndex = (int)(val & 0xFFFFFFFFu);
	}

	private void Parse(ISFSObject singleServerInfo)
	{
		if (singleServerInfo == null)
		{
			return;
		}
		if (memberPositions == null)
		{
			memberPositions = new List<long>(120);
		}
		int num = singleServerInfo.TryGetInt("serverId");
		if (num <= 0)
		{
			return;
		}
		int num2 = singleServerInfo.TryGetInt("leaderPoint");
		if (num2 > 0)
		{
			leaderPosition = Encode(num, num2);
		}
		int[] array = singleServerInfo.TryGetIntArray("memberPoints");
		if (array != null && array.Length != 0)
		{
			int i = 0;
			for (int num3 = array.Length; i < num3; i++)
			{
				memberPositions.Add(Encode(num, array[i]));
			}
		}
	}

	public void UpdateFromMsg(ISFSObject message)
	{
		Clear();
		ISFSArray sFSArray = message.GetSFSArray("alMemberPointsArr");
		if (sFSArray != null && sFSArray.Count > 0)
		{
			int i = 0;
			for (int count = sFSArray.Count; i < count; i++)
			{
				Parse(sFSArray.GetSFSObject(i));
			}
		}
		GameEntry.Event.Fire(EventId.OnWorldAlliancePointsRefresh);
	}

	public string Description()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (leaderPosition != 0L)
		{
			Decode(leaderPosition, out var serverId, out var positionIndex);
			stringBuilder.AppendFormat($"盟主:{positionIndex}[{serverId}]\n");
		}
		if (memberPositions != null)
		{
			stringBuilder.AppendLine($"盟友数量:{memberPositions.Count}");
			foreach (long memberPosition in memberPositions)
			{
				Decode(memberPosition, out var serverId2, out var positionIndex2);
				stringBuilder.AppendFormat($"{positionIndex2}[{serverId2}]\n");
			}
		}
		return stringBuilder.ToString();
	}

	public void Clear()
	{
		memberPositions?.Clear();
		leaderPosition = 0L;
	}

	public void GetALMemberPoints(out long leaderPosition, out List<long> memberPositions)
	{
		leaderPosition = this.leaderPosition;
		memberPositions = this.memberPositions;
	}
}
