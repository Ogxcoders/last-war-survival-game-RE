using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public struct ComponentActivitySnapshot : TEcsPoolDelegate<ComponentActivitySnapshot>, IEcsPoolDelegate, IEcsAutoReset<ComponentActivitySnapshot>, IEcsAutoCopy<ComponentActivitySnapshot>, IEcsAutoSnapshot<ComponentActivitySnapshot>
{
	public bool IsActive;

	public EcsEntitySnapshot Snapshot;

	public List<Type> Includes;

	public List<Type> Excludes;

	public Type DelegateType => typeof(ComponentActivitySnapshot);

	public void AutoReset(ref ComponentActivitySnapshot c, EcsWorld world, int entity)
	{
		c.IsActive = true;
		c.Snapshot = null;
		c.Includes = null;
		c.Excludes = null;
	}

	public void AutoCopy(ref ComponentActivitySnapshot src, ref ComponentActivitySnapshot dst)
	{
		throw new NotSupportedException();
	}

	public object TakeSnapshot(ref ComponentActivitySnapshot c, EcsWorld world, int entity, object env)
	{
		return c;
	}

	public void RestoreSnapshot(ref ComponentActivitySnapshot c, EcsWorld world, int entity, object data, object env)
	{
		ComponentActivitySnapshot componentActivitySnapshot = (ComponentActivitySnapshot)data;
		c.IsActive = componentActivitySnapshot.IsActive;
		c.Snapshot = componentActivitySnapshot.Snapshot;
		c.Includes = componentActivitySnapshot.Includes;
		c.Excludes = componentActivitySnapshot.Excludes;
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return true;
	}
}
