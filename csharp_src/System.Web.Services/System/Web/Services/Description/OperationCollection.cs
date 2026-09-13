namespace System.Web.Services.Description;

public sealed class OperationCollection : ServiceDescriptionBaseCollection
{
	public Operation this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (Operation)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal OperationCollection(PortType portType)
		: base(portType)
	{
	}

	public int Add(Operation operation)
	{
		Insert(base.Count, operation);
		return base.Count - 1;
	}

	public bool Contains(Operation operation)
	{
		return base.List.Contains(operation);
	}

	public void CopyTo(Operation[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal Operation Find(string name)
	{
		foreach (Operation item in base.List)
		{
			if (item.Name == name)
			{
				return item;
			}
		}
		return null;
	}

	public int IndexOf(Operation operation)
	{
		return base.List.IndexOf(operation);
	}

	public void Insert(int index, Operation operation)
	{
		base.List.Insert(index, operation);
	}

	public void Remove(Operation operation)
	{
		base.List.Remove(operation);
	}

	protected override void SetParent(object value, object parent)
	{
		((Operation)value).SetParent((PortType)parent);
	}
}
