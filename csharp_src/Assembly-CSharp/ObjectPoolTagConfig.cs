using System.Collections.Generic;

public class ObjectPoolTagConfig
{
	public float cleanPoolTime;

	public int maxPooledObjectCount;

	private static readonly Dictionary<ObjectPoolTag, ObjectPoolTagConfig> _objectPoolTagConfigDic = new Dictionary<ObjectPoolTag, ObjectPoolTagConfig>
	{
		{
			ObjectPoolTag.Normal,
			new ObjectPoolTagConfig(30f, -1)
		},
		{
			ObjectPoolTag.Battle,
			new ObjectPoolTagConfig(5f, 15)
		},
		{
			ObjectPoolTag.BattleScene,
			new ObjectPoolTagConfig(40f, 15)
		},
		{
			ObjectPoolTag.BattleBullet,
			new ObjectPoolTagConfig(5f, 200)
		},
		{
			ObjectPoolTag.City,
			new ObjectPoolTagConfig(120f, -1)
		}
	};

	public ObjectPoolTagConfig(float cleanPoolTime, int maxPooledObjectCount)
	{
		this.cleanPoolTime = cleanPoolTime;
		this.maxPooledObjectCount = maxPooledObjectCount;
	}

	public static ObjectPoolTagConfig GetConfig(ObjectPoolTag tag)
	{
		if (_objectPoolTagConfigDic.TryGetValue(tag, out var value))
		{
			return value;
		}
		return null;
	}
}
