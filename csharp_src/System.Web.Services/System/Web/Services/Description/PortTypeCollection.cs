namespace System.Web.Services.Description;

public sealed class PortTypeCollection : ServiceDescriptionBaseCollection
{
	public PortType this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (PortType)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public PortType this[string name]
	{
		get
		{
			int num = IndexOf((PortType)Table[name]);
			if (num >= 0)
			{
				return this[num];
			}
			return null;
		}
	}

	internal PortTypeCollection(ServiceDescription serviceDescription)
		: base(serviceDescription)
	{
	}

	public int Add(PortType portType)
	{
		Insert(base.Count, portType);
		return base.Count - 1;
	}

	public bool Contains(PortType portType)
	{
		return base.List.Contains(portType);
	}

	public void CopyTo(PortType[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is PortType))
		{
			throw new InvalidCastException();
		}
		return ((PortType)value).Name;
	}

	public int IndexOf(PortType portType)
	{
		return base.List.IndexOf(portType);
	}

	public void Insert(int index, PortType portType)
	{
		base.List.Insert(index, portType);
	}

	public void Remove(PortType portType)
	{
		base.List.Remove(portType);
	}

	protected override void SetParent(object value, object parent)
	{
		((PortType)value).SetParent((ServiceDescription)parent);
	}
}
