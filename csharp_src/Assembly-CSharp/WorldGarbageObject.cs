using System.Collections.Generic;

public class WorldGarbageObject : WorldDetectEventItemObject
{
	public WorldGarbageObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	protected override bool NeedShowDetectEventIcon()
	{
		return false;
	}

	protected override bool NeedShowTime()
	{
		if (!(world.GetPointInfo(pointIndex) is GarbagePointInfo garbagePointInfo))
		{
			return false;
		}
		bool result = false;
		pickupStartTime = 0L;
		pickupEndTime = 0L;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		int count = ownerMarches.Count;
		for (int i = 0; i < count; i++)
		{
			WorldMarch worldMarch = ownerMarches[i];
			if (worldMarch != null && worldMarch.targetUuid == garbagePointInfo.uuid && (worldMarch.status == MarchStatus.PICKING || worldMarch.status == MarchStatus.SAMPLING))
			{
				result = true;
				pickupStartTime = worldMarch.startTime;
				pickupEndTime = worldMarch.endTime;
				break;
			}
		}
		foreach (KeyValuePair<long, WorldMarch> allSampleFakeDatum in world.GetAllSampleFakeData())
		{
			if (allSampleFakeDatum.Value != null && allSampleFakeDatum.Value.targetUuid == garbagePointInfo.uuid && allSampleFakeDatum.Value.status == MarchStatus.SAMPLING)
			{
				result = true;
				pickupStartTime = allSampleFakeDatum.Value.startTime;
				pickupEndTime = allSampleFakeDatum.Value.endTime;
				break;
			}
		}
		return result;
	}

	protected override long GetPointUid()
	{
		if (world.GetPointInfo(pointIndex) is GarbagePointInfo garbagePointInfo)
		{
			return garbagePointInfo.uuid;
		}
		return 0L;
	}

	protected override string GetEventId()
	{
		if (world.GetPointInfo(pointIndex) is GarbagePointInfo garbagePointInfo)
		{
			return garbagePointInfo.eventId;
		}
		return "";
	}

	protected override string GetModePath()
	{
		string text = "";
		int key = eventId.ToInt();
		int key2 = 10;
		if (WorldScene.ModelPathDic.ContainsKey(key))
		{
			Dictionary<int, string> dictionary = WorldScene.ModelPathDic[key];
			if (dictionary.ContainsKey(key2))
			{
				text = dictionary[key2];
			}
		}
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.ConfigCache.GetTemplateData("detect_event", key, "image");
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		return $"Assets/Main/Prefabs/Garbage/{text}.prefab";
	}
}
