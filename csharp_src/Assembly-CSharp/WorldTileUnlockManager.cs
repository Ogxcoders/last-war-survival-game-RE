using System.Collections.Generic;

public class WorldTileUnlockManager : WorldManagerBase
{
	private Dictionary<int, WorldTileUnlockObject> _allTiles = new Dictionary<int, WorldTileUnlockObject>();

	public WorldTileUnlockManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		base.Init();
	}

	public override void UnInit()
	{
		base.UnInit();
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}

	private void OnTileUnlock(object userData)
	{
		int key = (int)(long)userData;
		if (!_allTiles.TryGetValue(key, out var value))
		{
			return;
		}
		value.OnUnlocked();
		foreach (int next in value.NextList)
		{
			if (_allTiles.TryGetValue(next, out var value2))
			{
				value2.RefreshState();
			}
		}
	}
}
