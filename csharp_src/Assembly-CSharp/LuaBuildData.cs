public class LuaBuildData
{
	public long uuid;

	public int pointId;

	public int state;

	public int buildId;

	public int level;

	public long buildUpdateTime;

	public LuaBuildData(long uid, long updateTime, int point, int tempState, int itemId, int lv)
	{
		uuid = uid;
		buildUpdateTime = updateTime;
		pointId = point;
		state = tempState;
		buildId = itemId;
		level = lv;
	}

	public LuaBuildData()
	{
	}

	public bool IsActive()
	{
		if (state != 2)
		{
			return level > 0;
		}
		return false;
	}
}
