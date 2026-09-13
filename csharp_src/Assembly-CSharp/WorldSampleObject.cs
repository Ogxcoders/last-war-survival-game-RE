using System.Collections.Generic;

public class WorldSampleObject : WorldDetectEventItemObject
{
	public WorldSampleObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	protected override bool NeedShowDetectEventIcon()
	{
		return true;
	}

	public override bool DoDisappear()
	{
		return false;
	}

	protected override long GetPointUid()
	{
		if (world.GetPointInfo(pointIndex) is SamplePointInfo samplePointInfo)
		{
			return samplePointInfo.uuid;
		}
		return 0L;
	}

	protected override bool NeedShowTime()
	{
		if (!(world.GetPointInfo(pointIndex) is SamplePointInfo samplePointInfo))
		{
			return false;
		}
		bool flag = false;
		pickupStartTime = 0L;
		pickupEndTime = 0L;
		List<WorldMarch> ownerMarches = world.GetOwnerMarches(GameEntry.Data.Player.Uid);
		int count = ownerMarches.Count;
		for (int i = 0; i < count; i++)
		{
			WorldMarch worldMarch = ownerMarches[i];
			if (worldMarch != null && worldMarch.targetUuid == samplePointInfo.uuid && (worldMarch.status == MarchStatus.PICKING || worldMarch.status == MarchStatus.SAMPLING))
			{
				flag = true;
				pickupStartTime = worldMarch.startTime;
				pickupEndTime = worldMarch.endTime;
				break;
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<long, WorldMarch> allSampleFakeDatum in world.GetAllSampleFakeData())
			{
				if (allSampleFakeDatum.Value != null && allSampleFakeDatum.Value.targetUuid == samplePointInfo.uuid && allSampleFakeDatum.Value.status == MarchStatus.SAMPLING)
				{
					flag = true;
					pickupStartTime = allSampleFakeDatum.Value.startTime;
					pickupEndTime = allSampleFakeDatum.Value.endTime;
					break;
				}
			}
		}
		return flag;
	}

	protected override string GetEventId()
	{
		if (world.GetPointInfo(pointIndex) is SamplePointInfo samplePointInfo)
		{
			return samplePointInfo.eventId;
		}
		return "";
	}

	protected override string GetModePath()
	{
		string text = "";
		int key = eventId.ToInt();
		int key2 = 9;
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
