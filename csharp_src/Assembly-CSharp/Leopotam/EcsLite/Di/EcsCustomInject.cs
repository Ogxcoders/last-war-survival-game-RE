using System;

namespace Leopotam.EcsLite.Di;

public struct EcsCustomInject<T> : IEcsCustomDataInject where T : class
{
	public T Value;

	void IEcsCustomDataInject.Fill(object[] injects)
	{
		if (injects.Length == 0)
		{
			return;
		}
		Type typeFromHandle = typeof(T);
		foreach (object obj in injects)
		{
			if (typeFromHandle.IsInstanceOfType(obj))
			{
				Value = (T)obj;
				break;
			}
		}
	}
}
