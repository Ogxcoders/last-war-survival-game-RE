using System;

namespace Leopotam.EcsLite;

public class TEcsPoolDelegateIgnore<T> : IEcsPoolDelegate, IEcsAutoReset<T>, IEcsAutoCopy<T>, IEcsAutoSnapshot<T> where T : struct
{
	public Type DelegateType => typeof(T);

	public void AutoReset(ref T c, EcsWorld world, int entity)
	{
	}

	public void AutoCopy(ref T src, ref T dst)
	{
	}

	public object TakeSnapshot(ref T c, EcsWorld world, int entity, object env)
	{
		throw new NotSupportedException();
	}

	public void RestoreSnapshot(ref T c, EcsWorld world, int entity, object data, object env)
	{
	}

	public bool IsSnapshotEqual(object a, object b, EcsWorld world)
	{
		return false;
	}
}
