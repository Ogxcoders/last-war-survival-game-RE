using System;
using Sfs2X.Entities.Data;

public class CityBattleS1MonsterInfo : IDisposable
{
	public int effectId;

	public float effectValue;

	public int soldierType;

	public void Update(ISFSObject q)
	{
		effectId = q.TryGetInt("mFeatureEffId");
		effectValue = q.TryGetFloat("mFeatureEffValue");
		soldierType = q.TryGetInt("soldierType");
	}

	public void Dispose()
	{
	}
}
