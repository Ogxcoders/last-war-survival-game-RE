namespace System.Web.Services.Description;

public sealed class OperationFaultCollection : ServiceDescriptionBaseCollection
{
	public OperationFault this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (OperationFault)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public OperationFault this[string name] => this[IndexOf((OperationFault)Table[name])];

	internal OperationFaultCollection(Operation operation)
		: base(operation)
	{
	}

	public int Add(OperationFault operationFaultMessage)
	{
		Insert(base.Count, operationFaultMessage);
		return base.Count - 1;
	}

	public bool Contains(OperationFault operationFaultMessage)
	{
		return base.List.Contains(operationFaultMessage);
	}

	public void CopyTo(OperationFault[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is OperationFault))
		{
			throw new InvalidCastException();
		}
		return ((OperationFault)value).Name;
	}

	public int IndexOf(OperationFault operationFaultMessage)
	{
		return base.List.IndexOf(operationFaultMessage);
	}

	public void Insert(int index, OperationFault operationFaultMessage)
	{
		base.List.Insert(index, operationFaultMessage);
	}

	public void Remove(OperationFault operationFaultMessage)
	{
		base.List.Remove(operationFaultMessage);
	}

	protected override void SetParent(object value, object parent)
	{
		((OperationFault)value).SetParent((Operation)parent);
	}
}
