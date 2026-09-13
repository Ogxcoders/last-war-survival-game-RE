using System.Collections.Generic;
using FibMatrix;
using Sfs2X.Entities.Data;

public class MultiKillDataManager : WorldManagerBase
{
	private ObjectPool<MultiKillBubbleData> pool;

	private Dictionary<int, MultiKillConfig> multiKillConfigs;

	public MultiKillDataManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		pool = new ObjectPool<MultiKillBubbleData>();
		multiKillConfigs = new Dictionary<int, MultiKillConfig>(16);
	}

	public override void UnInit()
	{
		multiKillConfigs.Clear();
		pool.Dispose();
		pool = null;
	}

	public void HandlePushMultiKillUpdate(ISFSObject msg)
	{
		ISFSObject sFSObject = msg.GetSFSObject("worldKillSkinInfo");
		MultiKillBubbleData multiKillBubbleData = pool.Allocate();
		multiKillBubbleData.ParseData(sFSObject);
		world.MultiKillPointAddTask(multiKillBubbleData);
	}

	public void CreateFakeData()
	{
	}

	private void CreateFakeDataByIndex(int playerIndex)
	{
	}

	public void Recycle(MultiKillBubbleData data)
	{
		if (pool == null)
		{
			data.Dispose();
		}
		else
		{
			pool.Recycle(data);
		}
	}
}
