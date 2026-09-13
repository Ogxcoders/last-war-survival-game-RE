using System;
using Sfs2X.Entities.Data;

public class InvasionBossInfo : IDisposable
{
	public int pointId;

	public int curHp;

	public int maxHp;

	public long battleStartTime;

	public long battleEndTime;

	public int invasionId;

	public void Update(ISFSObject q)
	{
		pointId = q.TryGetInt("pointId");
		curHp = q.TryGetInt("curHp");
		maxHp = q.TryGetInt("maxHp");
		battleStartTime = q.TryGetLong("battleStartTime");
		battleEndTime = q.TryGetLong("battleEndTime");
		invasionId = q.TryGetInt("invasionId");
	}

	public void Dispose()
	{
	}
}
