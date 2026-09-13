namespace System.Web.Services.Description;

public sealed class ServiceCollection : ServiceDescriptionBaseCollection
{
	public Service this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (Service)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public Service this[string name]
	{
		get
		{
			int num = IndexOf((Service)Table[name]);
			if (num >= 0)
			{
				return this[num];
			}
			return null;
		}
	}

	internal ServiceCollection(ServiceDescription serviceDescription)
		: base(serviceDescription)
	{
	}

	public int Add(Service service)
	{
		Insert(base.Count, service);
		return base.Count - 1;
	}

	public bool Contains(Service service)
	{
		return base.List.Contains(service);
	}

	public void CopyTo(Service[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is Service))
		{
			throw new InvalidCastException();
		}
		return ((Service)value).Name;
	}

	public int IndexOf(Service service)
	{
		return base.List.IndexOf(service);
	}

	public void Insert(int index, Service service)
	{
		base.List.Insert(index, service);
	}

	public void Remove(Service service)
	{
		base.List.Remove(service);
	}

	protected override void SetParent(object value, object parent)
	{
		((Service)value).SetParent((ServiceDescription)parent);
	}
}
