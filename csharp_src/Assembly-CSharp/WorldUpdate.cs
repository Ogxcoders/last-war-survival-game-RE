using System;
using System.Collections.Generic;
using Protobuf;
using Sfs2X.Entities.Data;

public class WorldUpdate
{
	private QueuedThread workerThread;

	private Queue<ParsePointsTask> taskList = new Queue<ParsePointsTask>();

	private List<int> keyToRemove = new List<int>();

	private WorldSortHelper sortHelper = new WorldSortHelper(20);

	public void Init()
	{
		workerThread = new QueuedThread("WorldUpdate");
		workerThread.Start();
	}

	public void UnInit()
	{
		Clear();
		workerThread.Stop();
	}

	public void Clear()
	{
		keyToRemove.Clear();
		taskList.Clear();
	}

	public void Update()
	{
		while (taskList.Count > 0)
		{
			ParsePointsTask parsePointsTask = taskList.Peek();
			if (parsePointsTask.isDone)
			{
				taskList.Dequeue();
				parsePointsTask.InvokeCallback();
				continue;
			}
			break;
		}
	}

	public void ProcessPointsMessage(ISFSObject message, Action<List<PointInfo>> callback, Action<List<LandPointInfo>> cbLandPoints, Action<List<WorldDesertInfo>> cbDesertInfos, Action<List<PointInfo>> cbAlInfos, Action<SFSObject> cbWarFlagInfos, Action<SFSObject> cbWarEffectInfos, Action<SFSObject, int, int> cbHeatSourceInfos, Action<SFSObject> cbTriggerInfos, Action<List<WorldAreaGreenInfo>> cbAreaGreens, Action<WorldAllCityGreenInfo> cbCityGreens)
	{
		ParsePointsTask parsePointsTask = new ParsePointsTask
		{
			taskType = ParsePointsTask.TaskType.Points,
			message = message,
			cbPoints = callback,
			cbLandPoints = cbLandPoints,
			cbDesertInfos = cbDesertInfos,
			cbAlInfos = cbAlInfos,
			cbWarFlagInfos = cbWarFlagInfos,
			cbWarEffectInfos = cbWarEffectInfos,
			cbHeatSourceInfos = cbHeatSourceInfos,
			cbTriggerInfos = cbTriggerInfos,
			cbAreaGreens = cbAreaGreens,
			cbCityGreens = cbCityGreens
		};
		ISFSArray sFSArray = message.GetSFSArray("points");
		if (sFSArray != null)
		{
			for (int i = 0; i < sFSArray.Count; i++)
			{
				WorldPointInfo.Parser.ParseFrom(sFSArray.GetByteArray(i).Bytes);
			}
		}
		taskList.Enqueue(parsePointsTask);
		workerThread.AddTask(parsePointsTask);
	}

	public void ProcessCreatePointMessage(ISFSObject message, Action<List<PointInfo>> callback)
	{
		string utfString = message.GetUtfString("type");
		ParsePointsTask parsePointsTask = new ParsePointsTask
		{
			taskType = ParsePointsTask.TaskType.Create,
			message = message,
			cbCreates = callback,
			isCreate = (utfString == "create")
		};
		taskList.Enqueue(parsePointsTask);
		workerThread.AddTask(parsePointsTask);
	}

	public void ProcessUpdateTileMessage(ISFSObject message, Action<List<WorldDesertInfo>> callback)
	{
		ParsePointsTask parsePointsTask = new ParsePointsTask
		{
			taskType = ParsePointsTask.TaskType.UpdateTiles,
			message = message,
			cbDesertInfos = callback
		};
		taskList.Enqueue(parsePointsTask);
		workerThread.AddTask(parsePointsTask);
	}

	public void ProcessRemovePointMessage(ISFSObject message, Action<List<int>> callback)
	{
		ParsePointsTask parsePointsTask = new ParsePointsTask
		{
			taskType = ParsePointsTask.TaskType.Remove,
			message = message,
			cbRemoves = callback
		};
		taskList.Enqueue(parsePointsTask);
		workerThread.AddTask(parsePointsTask);
	}
}
