namespace System.Web.Services.Description;

public sealed class FaultBindingCollection : ServiceDescriptionBaseCollection
{
	public FaultBinding this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (FaultBinding)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public FaultBinding this[string name] => this[IndexOf((FaultBinding)Table[name])];

	internal FaultBindingCollection(OperationBinding operationBinding)
		: base(operationBinding)
	{
	}

	public int Add(FaultBinding bindingOperationFault)
	{
		Insert(base.Count, bindingOperationFault);
		return base.Count - 1;
	}

	public bool Contains(FaultBinding bindingOperationFault)
	{
		return base.List.Contains(bindingOperationFault);
	}

	public void CopyTo(FaultBinding[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is FaultBinding))
		{
			throw new InvalidCastException();
		}
		return ((FaultBinding)value).Name;
	}

	public int IndexOf(FaultBinding bindingOperationFault)
	{
		return base.List.IndexOf(bindingOperationFault);
	}

	public void Insert(int index, FaultBinding bindingOperationFault)
	{
		base.List.Insert(index, bindingOperationFault);
	}

	public void Remove(FaultBinding bindingOperationFault)
	{
		base.List.Remove(bindingOperationFault);
	}

	protected override void SetParent(object value, object parent)
	{
		((FaultBinding)value).SetParent((OperationBinding)parent);
	}
}
