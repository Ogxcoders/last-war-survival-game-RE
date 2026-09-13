using Protobuf;
using Sfs2X.Entities.Data;

public class MonsterChallengeNewDonate
{
	public int configId;

	public int count;

	public long endTimeStamp;

	public MonsterChallengeNewDonate()
	{
	}

	public MonsterChallengeNewDonate(Protobuf.MonsterChallengeNewDonate msg)
	{
		ParseData(msg);
	}

	public virtual void ParseData(ISFSObject msg)
	{
		configId = msg.TryGetInt("configId");
		count = msg.TryGetInt("count");
		endTimeStamp = msg.TryGetLong("endTimeStamp");
	}

	protected virtual void ParseData(Protobuf.MonsterChallengeNewDonate msg)
	{
		configId = msg.ConfigId;
		count = msg.Count;
		endTimeStamp = msg.EndTimeStamp;
	}
}
