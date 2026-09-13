using System;
using Sfs2X.Entities.Data;

public class AllianceChallengeInfo : IDisposable
{
	public long bossUuid;

	public int configId;

	public long endTime;

	public long statusStartTime;

	public long statusEndTime;

	public int curStatus;

	public int count;

	public bool isEnd;

	public long allianceDamage;

	public string abbr = "";

	public string name = "";

	public bool allianceDamageMax;

	public ISFSObject marsInfo;

	public void Update(ISFSObject q)
	{
		bossUuid = q.TryGetLong("bossUuid");
		configId = q.TryGetInt("configId");
		endTime = q.TryGetLong("endTime");
		statusStartTime = q.TryGetLong("statusStartTime");
		statusEndTime = q.TryGetLong("statusEndTime");
		curStatus = q.TryGetInt("curStatus");
		count = q.TryGetInt("count");
		isEnd = q.TryGetBool("isEnd");
		allianceDamage = q.TryGetLong("allianceDamage");
		marsInfo = q.TryGetObj("marsInfo");
		if (marsInfo != null)
		{
			abbr = marsInfo.TryGetString("abbr");
			name = marsInfo.TryGetString("name");
		}
		allianceDamageMax = q.TryGetBool("allianceDamageMax");
	}

	public void Dispose()
	{
	}
}
