using System.Collections.Generic;

namespace FibMatrix.Rendering;

public abstract class RenderStateRegister
{
	public HashSet<object> objects = new HashSet<object>();

	private static Dictionary<int, string> CountStringPool = new Dictionary<int, string>(16);

	public virtual bool Valid => objects.Count > 0;

	public virtual int Register(object obj, bool require)
	{
		if (require)
		{
			return Register(obj);
		}
		return Deregister(obj);
	}

	public virtual int Register(object obj)
	{
		objects.Add(obj);
		if (objects.Count == 1)
		{
			OnEnable();
		}
		return objects.Count;
	}

	public virtual int Deregister(object obj)
	{
		objects.Remove(obj);
		if (objects.Count == 0)
		{
			OnDisable();
		}
		return objects.Count;
	}

	public abstract void OnEnable();

	public abstract void OnDisable();

	public override string ToString()
	{
		if (!CountStringPool.TryGetValue(objects.Count, out var value) && CountStringPool.Count <= 256)
		{
			value = objects.Count.ToString();
			CountStringPool[objects.Count] = value;
		}
		else
		{
			value = base.ToString();
		}
		return value;
	}
}
