using System;
using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public struct ComponentComposeEntityRoot : TEcsPoolDelegate<ComponentComposeEntityRoot>, IEcsPoolDelegate, IEcsAutoReset<ComponentComposeEntityRoot>, IEcsAutoCopy<ComponentComposeEntityRoot>, IEcsAutoSnapshot<ComponentComposeEntityRoot>
{
	[Serializable]
	public struct Item
	{
		public int ID;

		public string Template;

		public FVector2 Position;

		public FP Angle;
	}

	public Item[] Entities;

	public Type DelegateType => typeof(ComponentComposeEntityRoot);

	public void AutoReset(ref ComponentComposeEntityRoot c, EcsWorld world, int entity)
	{
	}

	public void AutoCopy(ref ComponentComposeEntityRoot src, ref ComponentComposeEntityRoot dst)
	{
		throw new NotSupportedException();
	}

	public object TakeSnapshot(ref ComponentComposeEntityRoot c, EcsWorld world, int entity, object env)
	{
		ComponentComposeEntityRoot componentComposeEntityRoot = new ComponentComposeEntityRoot
		{
			Entities = new Item[c.Entities.Length]
		};
		for (int i = 0; i < c.Entities.Length; i++)
		{
			componentComposeEntityRoot.Entities[i] = c.Entities[i];
		}
		return componentComposeEntityRoot;
	}

	public void RestoreSnapshot(ref ComponentComposeEntityRoot c, EcsWorld world, int entity, object data, object env)
	{
		ComponentComposeEntityRoot componentComposeEntityRoot = (ComponentComposeEntityRoot)data;
		c.Entities = new Item[componentComposeEntityRoot.Entities.Length];
		for (int i = 0; i < componentComposeEntityRoot.Entities.Length; i++)
		{
			c.Entities[i] = componentComposeEntityRoot.Entities[i];
		}
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return false;
	}
}
