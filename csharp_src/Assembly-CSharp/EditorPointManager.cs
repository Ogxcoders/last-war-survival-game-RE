using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class EditorPointManager : WorldPointManager
{
	public EditorPointManager(EditorSceneBuilding scene)
		: base(scene)
	{
	}

	public override void OnUpdate(float deltaTime)
	{
		WorldPointManager.time = Time.realtimeSinceStartup + 0.015f;
		int lodLevel = world.GetLodLevel();
		svLod = GetServerLod(lodLevel);
		ViewCulling();
		AsyncCreate();
		ObjectsOnUpdate(deltaTime);
		if (_isPointUpdate)
		{
			_isPointUpdate = false;
			GameEntry.Event.Fire(EventId.UPDATE_POINTS_DATA);
		}
		timeCount += deltaTime;
		if (timeCount > 1f)
		{
			timeCount -= 1f;
			Update1000MS();
		}
	}

	public void AddPlayerBuilding(BuildPointInfo building)
	{
		building.tileSize = GetBuildTileByItemId(building.itemId);
		_pointInfos.Add(building);
		AddPointInfo(building);
		AddToBuildList(building.pointIndex);
		isRecvViewPoints = true;
		CheckNeedRefreshRoad();
		MarkPointUpdate();
	}

	public void ClearPlayerBuildings()
	{
		List<int> list = new List<int>();
		foreach (PointInfo pointInfo in _pointInfos)
		{
			if (pointInfo.pointType == WorldPointType.PlayerBuilding)
			{
				list.Add(pointInfo.pointIndex);
			}
		}
		ISFSObject message = EditorMockData.MockWorldPointRemove(list);
		ParseWorldPointRemove(message);
	}
}
