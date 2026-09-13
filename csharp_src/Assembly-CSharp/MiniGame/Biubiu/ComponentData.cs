using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentData : TEcsPoolDelegate<ComponentData>, IEcsPoolDelegate, IEcsAutoReset<ComponentData>, IEcsAutoCopy<ComponentData>, IEcsAutoSnapshot<ComponentData>
{
	public Property[] KeyValues;

	public Type DelegateType => typeof(ComponentData);

	public void AutoReset(ref ComponentData c, EcsWorld world, int entity)
	{
		c.KeyValues = null;
	}

	public void AutoCopy(ref ComponentData src, ref ComponentData dst)
	{
		throw new NotSupportedException("ComponentData不支持拷贝");
	}

	public object TakeSnapshot(ref ComponentData c, EcsWorld world, int entity, object env)
	{
		ComponentDataSnapshotData componentDataSnapshotData = new ComponentDataSnapshotData();
		List<Property> list = new List<Property>();
		if (c.KeyValues != null)
		{
			Property[] keyValues = c.KeyValues;
			for (int i = 0; i < keyValues.Length; i++)
			{
				Property property = keyValues[i];
				if (property.Value != FP.Zero)
				{
					list.Add(new Property
					{
						PropertyID = property.PropertyID,
						Value = property.Value
					});
				}
			}
		}
		componentDataSnapshotData.KeyValues = list.ToArray();
		return componentDataSnapshotData;
	}

	public void RestoreSnapshot(ref ComponentData c, EcsWorld world, int entity, object data, object env)
	{
		ComponentDataSnapshotData obj = (ComponentDataSnapshotData)data;
		if (c.KeyValues == null)
		{
			c.KeyValues = new Property[100];
		}
		Property[] keyValues = obj.KeyValues;
		for (int i = 0; i < keyValues.Length; i++)
		{
			Property property = keyValues[i];
			c.KeyValues[property.PropertyID] = new Property
			{
				PropertyID = property.PropertyID,
				Value = property.Value
			};
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		ComponentDataSnapshotData componentDataSnapshotData = (ComponentDataSnapshotData)a;
		ComponentDataSnapshotData componentDataSnapshotData2 = (ComponentDataSnapshotData)b;
		if (componentDataSnapshotData.KeyValues.Length != componentDataSnapshotData2.KeyValues.Length)
		{
			return false;
		}
		for (int i = 0; i < componentDataSnapshotData.KeyValues.Length; i++)
		{
			if (componentDataSnapshotData.KeyValues[i].PropertyID != componentDataSnapshotData2.KeyValues[i].PropertyID)
			{
				return false;
			}
			if (componentDataSnapshotData.KeyValues[i].Value != componentDataSnapshotData2.KeyValues[i].Value)
			{
				return false;
			}
		}
		return true;
	}
}
