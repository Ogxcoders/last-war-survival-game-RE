using System.Collections.Generic;
using System.Text;
using GameFramework;
using Sfs2X.Entities.Data;
using Unity.Mathematics;
using UnityEngine;

public class HSRMarch
{
	private class Station
	{
		public int serverId;

		public Vector3 position;

		public HSRDirection direction;

		public int stationId;

		public Station(int serverId, Vector3 position, HSRDirection direction, int stationId)
		{
			this.serverId = serverId;
			this.position = position;
			this.direction = direction;
			this.stationId = stationId;
		}
	}

	private long createTime;

	private long startTime;

	private long endTime;

	private int carriageCount;

	private float durationBetweenStations;

	private float frequencyBetweenStations;

	private long tailDelay;

	private List<Station> stations = new List<Station>();

	public void Init(ISFSObject message)
	{
		stations.Clear();
		durationBetweenStations = 1000f / HSRMarchManager.GetInstance().GetSpeed() * 1000f;
		frequencyBetweenStations = 1f / durationBetweenStations;
		createTime = message.TryGetLong("createTrainTime");
		if (createTime <= 0)
		{
			Log.Error($"HSRMarch Init 错误，createTrainTime 为0：{message}");
		}
		startTime = message.TryGetLong("sendTime");
		if (startTime <= 0)
		{
			startTime = createTime + HSRMarchManager.GetInstance().GetWaitTime();
		}
		ISFSObject iSFSObject = message.TryGetObj("marchInfo");
		int num = 0;
		long num2 = 0L;
		if (iSFSObject != null)
		{
			num2 = iSFSObject.TryGetLong("startTime");
			num = message.TryGetInt("lastCity");
		}
		ISFSArray iSFSArray = message.TryGetArray("buyMaxNumList");
		SetCarriageCount(10);
		Vector3 vector = Vector3.zero;
		int serverId = 0;
		int stationId = 0;
		int num3 = 0;
		int num4 = -1;
		foreach (object item in iSFSArray)
		{
			if (item is ISFSObject obj)
			{
				int num5 = obj.TryGetInt("station");
				if (num2 > 0 && num4 < 0 && num5 == num)
				{
					float num6 = durationBetweenStations * (float)num3;
					startTime = num2 - (long)num6;
					num4 = num3;
				}
				int num7 = obj.TryGetInt("serverId");
				Vector3 stationPosition = HSRMarchManager.GetInstance().GetStationPosition(num7);
				if (num3 > 0)
				{
					Vector3 direction = stationPosition - vector;
					HSRDirection direction2 = Vector3ToDirection(direction);
					stations.Add(new Station(serverId, vector, direction2, stationId));
				}
				vector = stationPosition;
				serverId = num7;
				stationId = num5;
				num3++;
			}
		}
		stations.Add(stations[0]);
		float num8 = durationBetweenStations * (float)(stations.Count - 1);
		endTime = startTime + (long)num8;
		LogHSRTimeList(num2, num4, message);
	}

	private string LogTime(long time)
	{
		return GameEntry.Timer.TimeStampToHMS(time) ?? "";
	}

	private void LogHSRTimeList(long lastStationTime, int lastStationIndex, ISFSObject message)
	{
		if (!CommonUtils.IsDebug())
		{
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format("高铁时刻表{0}", message.TryGetLong("uuid")));
		stringBuilder.AppendLine("createTime=" + LogTime(createTime));
		long num = message.TryGetLong("sendTime");
		if (num <= 0)
		{
			long time = createTime + HSRMarchManager.GetInstance().GetWaitTime();
			stringBuilder.AppendLine("没有sendTime，使用createTime+waitTime估算的发车时间startTime=" + LogTime(time));
		}
		else
		{
			stringBuilder.AppendLine("发车时间startTime=sendTime=:" + LogTime(num));
		}
		stringBuilder.AppendLine("现在时间now=" + LogTime(serverTime));
		GetPosition(serverTime, out var position, out var _, out var lastStationIndex2);
		float num2 = (float)(serverTime - startTime) / durationBetweenStations;
		if (lastStationTime > 0)
		{
			stringBuilder.AppendLine(string.Format("存在marchInfo，进行时间校正：lastCity{0}，上一站是第{1}站，上一站到站时间marchInfo.startTime:{2}", message.TryGetInt("lastCity"), lastStationIndex, LogTime(lastStationTime)));
			stringBuilder.AppendLine($"现在时间 与 上一站到站时间 相差多少秒:{(serverTime - lastStationTime) / 1000}");
			stringBuilder.AppendLine($"相邻两站时间间隔={durationBetweenStations / 1000f}");
			float num3 = (float)(serverTime - lastStationTime) / durationBetweenStations;
			stringBuilder.AppendLine($"上一站到站后，已经经过了多少站={num3:F2}={(double)(serverTime - lastStationTime) * 0.001}/{(double)durationBetweenStations * 0.001}");
			int num4 = Mathf.Min((int)num3 + lastStationIndex, stations.Count - 1);
			int num5 = Mathf.Min(num4 + 1, stations.Count - 1);
			stringBuilder.AppendLine($"上一站是第{num4}站，下一站是第{num5}站");
			Vector3 position2 = stations[num4].position;
			Vector3 position3 = stations[num5].position;
			stringBuilder.AppendLine($"上一站位置{position2}，下一站位置{position3}，差值{num3 - (float)(int)num3:F2}");
			Vector3 vector = Vector3.Lerp(position2, position3, num3 - (float)(int)num3);
			stringBuilder.AppendLine($"Lerp得到当前位置：{vector}");
			Vector2Int vector2Int = TileCoord.WorldToTile(vector);
			stringBuilder.AppendLine($"前端认为：tile:{vector2Int.x:D3},{vector2Int.y:D3},curServer:{(((double)(num3 - (float)(int)num3) < 0.5) ? stations[num4].serverId : stations[num5].serverId)},curPos:{vector},");
			ISFSObject iSFSObject = message.TryGetObj("marchInfo");
			if (iSFSObject != null)
			{
				int num6 = iSFSObject.TryGetInt("curServer");
				int num7 = iSFSObject.TryGetInt("curX");
				int num8 = iSFSObject.TryGetInt("curY");
				Vector3 vector2 = TileCoord.TileToWorld(num7, num8, num6);
				stringBuilder.AppendLine($"后端认为：tile:{num7:D3},{num8:D3},curServer:{num6},curPos:{vector2},");
			}
			if (lastStationIndex2 != num4)
			{
				Log.Error($"高铁前端错误，lastStationIndex2!=lastId：{lastStationIndex2}！={num4}");
			}
			if ((double)Vector3.Distance(vector, position) > 0.01)
			{
				Log.Error($"高铁前端错误，Vector3.Distance(clientP,pos)>0.01 ：{Vector3.Distance(vector, position)}");
			}
		}
		else
		{
			stringBuilder.AppendLine("没有marchInfo,无需校正发车时间");
		}
		stringBuilder.AppendLine($"progress = (now - startTime)/durationBetweenStations  :  {num2:F2}={(double)(serverTime - startTime) * 0.001}/{(double)durationBetweenStations * 0.001}");
		stringBuilder.AppendLine("endTime:" + LogTime(endTime));
		for (int i = 0; i < stations.Count; i++)
		{
			int num9 = (int)(Mathf.Clamp(stations[i].position.x / 2f, 0f, 2999f) / 1000f);
			int num10 = (int)(Mathf.Clamp(stations[i].position.z / 2f, 0f, 2999f) / 1000f);
			int num11 = 3 * num10 + num9 + 1;
			string text = Position2String(stations[i].position);
			long time2 = startTime + i * (long)durationBetweenStations;
			stringBuilder.AppendLine($"第{i:D2}站：九宫位置：{num11}/{text}，station:{stations[i].stationId:D4}，server:{stations[i].serverId}，time:{LogTime(time2)}");
		}
		Log.Warning(stringBuilder.ToString());
	}

	private string Position2String(Vector3 pos)
	{
		string text = ((pos.x < 1000f) ? "左" : ((!(pos.x > 4000f)) ? "中" : "右"));
		string text2 = ((pos.z < 1000f) ? "下" : ((!(pos.z > 4000f)) ? "中" : "上"));
		return text + text2;
	}

	public int GetCarriageCount()
	{
		return carriageCount;
	}

	public void SetCarriageCount(int count)
	{
		carriageCount = count;
		float num = HSRMarchManager.GetInstance().GetCarriageLength() * (float)count;
		tailDelay = (long)(1000f * num / HSRMarchManager.GetInstance().GetSpeed());
	}

	public void GetPosition(long timeStamp, out Vector3 position, out HSRDirection direction, out int lastStationIndex)
	{
		if (timeStamp < startTime)
		{
			position = stations[0].position;
			direction = stations[0].direction;
			lastStationIndex = 0;
			return;
		}
		if (timeStamp >= endTime)
		{
			position = stations[stations.Count - 1].position;
			direction = stations[stations.Count - 1].direction;
			lastStationIndex = stations.Count - 1;
			return;
		}
		float num = (float)(timeStamp - startTime) * frequencyBetweenStations;
		int num2 = (int)num;
		if (num2 >= stations.Count - 1)
		{
			position = stations[stations.Count - 1].position;
			direction = stations[stations.Count - 1].direction;
			lastStationIndex = stations.Count - 1;
			return;
		}
		float t = num - (float)num2;
		Vector3 position2 = stations[num2].position;
		Vector3 position3 = stations[num2 + 1].position;
		position = Vector3.Lerp(position2, position3, t);
		direction = stations[num2].direction;
		lastStationIndex = num2;
	}

	private HSRDirection Vector3ToDirection(Vector3 direction)
	{
		if (math.abs(direction.z) < 1f)
		{
			if (direction.x > 0f)
			{
				return HSRDirection.East;
			}
			if (direction.x < 0f)
			{
				return HSRDirection.West;
			}
		}
		else if (math.abs(direction.x) < 1f)
		{
			if (direction.z > 0f)
			{
				return HSRDirection.North;
			}
			if (direction.z < 0f)
			{
				return HSRDirection.South;
			}
		}
		Log.Error("HSRMarch Vector3ToDirection error:" + direction.ToString());
		return HSRDirection.None;
	}

	public Vector3 GetTailPosition()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		GetPosition(serverTime - tailDelay, out var position, out var _, out var _);
		return position;
	}

	public bool IsInView(Rect viewRect)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (serverTime < startTime || serverTime >= endTime)
		{
			return false;
		}
		GetPosition(serverTime, out var position, out var direction, out var lastStationIndex);
		GetPosition(serverTime - tailDelay, out var position2, out var direction2, out var _);
		if (direction == direction2)
		{
			return AxisAlignRectIntersectAxisAlignSegment(viewRect, position.x, position.z, position2.x, position2.z);
		}
		Vector3 position3 = stations[lastStationIndex].position;
		if (!AxisAlignRectIntersectAxisAlignSegment(viewRect, position.x, position.z, position3.x, position3.z))
		{
			return AxisAlignRectIntersectAxisAlignSegment(viewRect, position3.x, position3.z, position2.x, position2.z);
		}
		return true;
	}

	public static bool AxisAlignRectIntersectAxisAlignSegment(Rect rect, float p1X, float p1Y, float p2X, float p2Y)
	{
		if (Mathf.Abs(p2X - p1X) < 0.01f)
		{
			if (p1X < rect.xMin || p1X > rect.xMax)
			{
				return false;
			}
			double num = Mathf.Min(p1Y, p2Y);
			if ((double)Mathf.Max(p1Y, p2Y) >= (double)rect.yMin)
			{
				return num <= (double)rect.yMax;
			}
			return false;
		}
		if (Mathf.Abs(p1Y - p2Y) < 0.01f)
		{
			if (p1Y < rect.yMin || p1Y > rect.yMax)
			{
				return false;
			}
			double num2 = Mathf.Min(p1X, p2X);
			if ((double)Mathf.Max(p1X, p2X) >= (double)rect.xMin)
			{
				return num2 <= (double)rect.xMax;
			}
			return false;
		}
		Log.Error($"IsAxisAlignedSegmentIntersect 线段没有轴对齐，请使用IntersectsSegment方法 {p1X}, {p2X}, {p1Y}, {p2Y}");
		return false;
	}
}
