using System;
using Sfs2X.Entities.Data;

public class AllianceS3NewBossInfo : IDisposable
{
	public int stage;

	public int dflyLevel;

	public long totalHp;

	public long curHp;

	public int dizzinessState;

	public long dizzinessTime;

	public string dizzinessUid;

	public AllianceS3NewBossDizzinessUserInfo dizzinessUser;

	public void Update(ISFSObject q)
	{
		if (q != null)
		{
			stage = q.TryGetInt("stage");
			dflyLevel = q.TryGetInt("dflyLevel");
			totalHp = q.TryGetLong("totalHp");
			curHp = q.TryGetLong("curHp");
			dizzinessState = q.TryGetInt("dizzinessState");
			dizzinessTime = q.TryGetLong("dizzinessTime");
			dizzinessUid = q.TryGetString("dizzinessUid");
			ISFSObject iSFSObject = q.TryGetObj("dizzinessUser");
			if (iSFSObject != null)
			{
				dizzinessUser = new AllianceS3NewBossDizzinessUserInfo();
				dizzinessUser.Update(iSFSObject);
			}
		}
	}

	public void Dispose()
	{
	}
}
