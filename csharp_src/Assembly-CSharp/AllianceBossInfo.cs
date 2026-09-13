using System;
using Sfs2X.Entities.Data;

public class AllianceBossInfo : IDisposable
{
	public long readyTime;

	public long battleStartTime;

	public long battleEndTime;

	public long actEndTime;

	public long damage;

	public long maxDamage;

	public int lv;

	public int bossType;

	public AllianceS3NewBossInfo dataS3;

	public void Update(ISFSObject q)
	{
		readyTime = q.TryGetLong("readyTime");
		battleStartTime = q.TryGetLong("battleStartTime");
		battleEndTime = q.TryGetLong("battleEndTime");
		actEndTime = q.TryGetLong("actEndTime");
		damage = q.TryGetLong("damage");
		maxDamage = q.TryGetLong("maxDamage");
		lv = q.TryGetInt("lv");
		bossType = q.TryGetInt("bossType");
		ISFSObject iSFSObject = q.TryGetObj("dataS3");
		if (iSFSObject != null)
		{
			dataS3 = new AllianceS3NewBossInfo();
			dataS3.Update(iSFSObject);
		}
	}

	public void Dispose()
	{
	}
}
