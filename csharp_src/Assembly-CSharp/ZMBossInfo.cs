using System;
using Sfs2X.Entities.Data;

public class ZMBossInfo : IDisposable
{
	public int zMBossId;

	public int stage;

	public long nextStageTime;

	public long transferEndTime;

	public int bossSrcServer;

	public long shieldHp;

	public long shieldMaxHp;

	public long shieldEndTime;

	public long frenzyEndTime;

	public int frenzyDuration;

	public void Update(ISFSObject q)
	{
		zMBossId = q.TryGetInt("zMBossId");
		stage = q.TryGetInt("stage");
		nextStageTime = q.TryGetLong("nextStageTime");
		transferEndTime = q.TryGetLong("transferEndTime");
		bossSrcServer = q.TryGetInt("bossSrcServer");
		shieldHp = q.TryGetLong("shieldHp");
		shieldMaxHp = q.TryGetLong("shieldMaxHp");
		shieldEndTime = q.TryGetLong("shieldEndTime");
		frenzyEndTime = q.TryGetLong("frenzyEndTime");
		frenzyDuration = q.TryGetInt("frenzyDuration");
	}

	public void Dispose()
	{
	}
}
