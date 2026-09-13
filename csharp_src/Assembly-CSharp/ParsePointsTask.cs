using System;
using System.Collections.Generic;
using GameFramework;
using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

internal class ParsePointsTask : IQueuedThreadTask
{
	public enum TaskType
	{
		Create,
		Remove,
		Points,
		UpdateTiles
	}

	public TaskType taskType;

	public ISFSObject message;

	public List<PointInfo> pointInfos = new List<PointInfo>();

	public List<PointInfo> allianceInfos = new List<PointInfo>();

	public SFSObject warFlagInfo;

	public SFSObject warEffectInfo;

	public SFSObject hotSpotInfo;

	private int lb;

	private int rt;

	public SFSObject triggerInfo;

	public List<int> removedPointId = new List<int>();

	public List<LandPointInfo> landPointInfos = new List<LandPointInfo>();

	public List<WorldDesertInfo> desertInfos = new List<WorldDesertInfo>();

	public List<WorldAreaGreenInfo> areaGreens;

	public WorldAllCityGreenInfo cityGreens;

	public Action<List<PointInfo>> cbPoints;

	public Action<List<PointInfo>> cbCreates;

	public Action<List<int>> cbRemoves;

	public Action<List<LandPointInfo>> cbLandPoints;

	public Action<List<WorldDesertInfo>> cbDesertInfos;

	public Action<List<PointInfo>> cbAlInfos;

	public Action<SFSObject> cbWarFlagInfos;

	public Action<SFSObject> cbWarEffectInfos;

	public Action<SFSObject, int, int> cbHeatSourceInfos;

	public Action<SFSObject> cbTriggerInfos;

	public Action<List<WorldAreaGreenInfo>> cbAreaGreens;

	public Action<WorldAllCityGreenInfo> cbCityGreens;

	public volatile bool isDone;

	public bool isCreate;

	public void Process()
	{
		try
		{
			if (taskType == TaskType.Points)
			{
				ISFSArray sFSArray = message.GetSFSArray("points");
				if (sFSArray != null)
				{
					for (int i = 0; i < sFSArray.Count; i++)
					{
						WorldPointInfo p = WorldPointInfo.Parser.ParseFrom(sFSArray.GetByteArray(i).Bytes);
						AddPointInfo(NewPointInfo(p));
					}
				}
				ISFSArray sFSArray2 = message.GetSFSArray("lands");
				if (sFSArray2 != null)
				{
					for (int j = 0; j < sFSArray2.Count; j++)
					{
						LandPointInfo item = LandPointInfo.Parser.ParseFrom(sFSArray2.GetByteArray(j).Bytes);
						landPointInfos.Add(item);
					}
				}
				ISFSArray sFSArray3 = message.GetSFSArray("deserts");
				if (sFSArray3 != null)
				{
					for (int k = 0; k < sFSArray3.Count; k++)
					{
						DesertInfo di = DesertInfo.Parser.ParseFrom(sFSArray3.GetByteArray(k).Bytes);
						desertInfos.Add(new WorldDesertInfo(di));
					}
				}
				ISFSArray sFSArray4 = message.GetSFSArray("greenPoints");
				if (sFSArray4 != null && sFSArray4.Count > 0)
				{
					if (areaGreens == null)
					{
						areaGreens = new List<WorldAreaGreenInfo>();
					}
					for (int l = 0; l < sFSArray4.Count; l++)
					{
						GreenPoints di2 = GreenPoints.Parser.ParseFrom(sFSArray4.GetByteArray(l).Bytes);
						areaGreens.Add(new WorldAreaGreenInfo(di2));
					}
				}
				ByteArray byteArray = message.GetByteArray("cityGreen");
				if (byteArray != null)
				{
					cityGreens = WorldAllCityGreenInfo.Parser.ParseFrom(byteArray.Bytes);
				}
				ISFSArray sFSArray5 = message.GetSFSArray("alInfos");
				if (sFSArray5 != null)
				{
					for (int m = 0; m < sFSArray5.Count; m++)
					{
						WorldPointInfo p2 = WorldPointInfo.Parser.ParseFrom(sFSArray5.GetByteArray(m).Bytes);
						allianceInfos.Add(NewPointInfo(p2));
						AddPointInfo(NewPointInfo(p2));
					}
				}
				ISFSObject sFSObject = message.GetSFSObject("worldFlags");
				if (sFSObject != null && sFSObject is SFSObject sFSObject2)
				{
					warFlagInfo = sFSObject2;
				}
				ISFSObject sFSObject3 = message.GetSFSObject("effectObj");
				if (sFSObject3 != null && sFSObject3 is SFSObject sFSObject4)
				{
					warEffectInfo = sFSObject4;
				}
				ISFSObject sFSObject5 = message.GetSFSObject("hotSpots");
				if (sFSObject5 != null && sFSObject5 is SFSObject sFSObject6)
				{
					hotSpotInfo = sFSObject6;
					lb = message.GetInt("leftBottom");
					rt = message.GetInt("rightTop");
				}
				ISFSObject sFSObject7 = message.GetSFSObject("triggers");
				if (sFSObject7 != null && sFSObject7 is SFSObject sFSObject8)
				{
					triggerInfo = sFSObject8;
				}
			}
			else if (taskType == TaskType.Create)
			{
				ISFSArray sFSArray6 = message.GetSFSArray("points");
				for (int n = 0; n < sFSArray6.Count; n++)
				{
					WorldPointInfo p3 = WorldPointInfo.Parser.ParseFrom(sFSArray6.GetByteArray(n).Bytes);
					AddPointInfo(NewPointInfo(p3));
				}
			}
			else if (taskType == TaskType.UpdateTiles)
			{
				ISFSArray sFSArray7 = message.GetSFSArray("deserts");
				if (sFSArray7 != null)
				{
					for (int num = 0; num < sFSArray7.Count; num++)
					{
						DesertInfo di3 = DesertInfo.Parser.ParseFrom(sFSArray7.GetByteArray(num).Bytes);
						desertInfos.Add(new WorldDesertInfo(di3));
					}
				}
			}
			else if (taskType == TaskType.Remove)
			{
				ISFSArray sFSArray8 = message.GetSFSArray("pointIds");
				for (int num2 = 0; num2 < sFSArray8.Count; num2++)
				{
					removedPointId.Add(sFSArray8.GetInt(num2));
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
		finally
		{
			isDone = true;
		}
	}

	public void InvokeCallback()
	{
		if (taskType == TaskType.Points)
		{
			cbPoints(pointInfos);
			cbLandPoints(landPointInfos);
			cbDesertInfos(desertInfos);
			cbAlInfos(allianceInfos);
			cbWarFlagInfos(warFlagInfo);
			cbWarEffectInfos(warEffectInfo);
			cbHeatSourceInfos(hotSpotInfo, lb, rt);
			cbTriggerInfos(triggerInfo);
			if (areaGreens != null)
			{
				cbAreaGreens(areaGreens);
			}
			if (cityGreens != null)
			{
				cbCityGreens(cityGreens);
			}
		}
		else if (taskType == TaskType.Create)
		{
			cbCreates(pointInfos);
		}
		else if (taskType == TaskType.UpdateTiles)
		{
			cbDesertInfos(desertInfos);
		}
		else if (taskType == TaskType.Remove)
		{
			cbRemoves(removedPointId);
		}
	}

	private PointInfo NewPointInfo(WorldPointInfo p)
	{
		return WorldPointManager.NewPointInfo(p, isCreate);
	}

	private void AddPointInfo(PointInfo pi)
	{
		if (pi != null)
		{
			pointInfos.Add(pi);
		}
	}
}
