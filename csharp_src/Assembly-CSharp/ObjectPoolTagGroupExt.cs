using System;
using System.Collections.Generic;

public static class ObjectPoolTagGroupExt
{
	private static readonly Dictionary<ObjectPoolTagGroup, ObjectPoolTag[]> _groupTags = new Dictionary<ObjectPoolTagGroup, ObjectPoolTag[]>
	{
		{
			ObjectPoolTagGroup.Normal,
			new ObjectPoolTag[2]
			{
				ObjectPoolTag.Normal,
				ObjectPoolTag.City
			}
		},
		{
			ObjectPoolTagGroup.Battle,
			new ObjectPoolTag[3]
			{
				ObjectPoolTag.Battle,
				ObjectPoolTag.BattleScene,
				ObjectPoolTag.BattleBullet
			}
		}
	};

	public static ObjectPoolTag[] GetTags(this ObjectPoolTagGroup group)
	{
		if (_groupTags.TryGetValue(group, out var value))
		{
			return value;
		}
		return Array.Empty<ObjectPoolTag>();
	}
}
