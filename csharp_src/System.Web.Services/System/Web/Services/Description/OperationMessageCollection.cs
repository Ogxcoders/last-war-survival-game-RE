namespace System.Web.Services.Description;

public sealed class OperationMessageCollection : ServiceDescriptionBaseCollection
{
	public OperationFlow Flow
	{
		get
		{
			switch (base.Count)
			{
			case 1:
				if (this[0] is OperationInput)
				{
					return OperationFlow.OneWay;
				}
				return OperationFlow.Notification;
			case 2:
				if (this[0] is OperationInput)
				{
					return OperationFlow.RequestResponse;
				}
				return OperationFlow.SolicitResponse;
			default:
				return OperationFlow.None;
			}
		}
	}

	public OperationInput Input
	{
		get
		{
			foreach (object item in base.List)
			{
				if (item is OperationInput)
				{
					return (OperationInput)item;
				}
			}
			return null;
		}
	}

	public OperationMessage this[int index]
	{
		get
		{
			return (OperationMessage)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public OperationOutput Output
	{
		get
		{
			foreach (object item in base.List)
			{
				if (item is OperationOutput)
				{
					return (OperationOutput)item;
				}
			}
			return null;
		}
	}

	internal OperationFault Fault
	{
		get
		{
			foreach (object item in base.List)
			{
				if (item is OperationFault)
				{
					return (OperationFault)item;
				}
			}
			return null;
		}
	}

	internal OperationMessageCollection(Operation operation)
		: base(operation)
	{
	}

	public int Add(OperationMessage operationMessage)
	{
		Insert(base.Count, operationMessage);
		return base.Count - 1;
	}

	public bool Contains(OperationMessage operationMessage)
	{
		return base.List.Contains(operationMessage);
	}

	public void CopyTo(OperationMessage[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal OperationMessage Find(string name)
	{
		foreach (OperationMessage item in base.List)
		{
			if (item.Name == name)
			{
				return item;
			}
		}
		return null;
	}

	public int IndexOf(OperationMessage operationMessage)
	{
		return base.List.IndexOf(operationMessage);
	}

	public void Insert(int index, OperationMessage operationMessage)
	{
		base.List.Insert(index, operationMessage);
	}

	protected override void OnInsert(int index, object value)
	{
		if (base.Count == 0 || (base.Count == 1 && value.GetType() != this[0].GetType()))
		{
			return;
		}
		throw new InvalidOperationException("The operation object can only contain one input and one output message.");
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (oldValue.GetType() != newValue.GetType())
		{
			throw new InvalidOperationException("The message types of the old and new value are not the same.");
		}
		base.OnSet(index, oldValue, newValue);
	}

	protected override void OnValidate(object value)
	{
		if (value == null)
		{
			throw new ArgumentException("The message object is a null reference.");
		}
		if (!(value is OperationInput) && !(value is OperationOutput))
		{
			throw new ArgumentException("The message object is not an input or an output message.");
		}
	}

	public void Remove(OperationMessage operationMessage)
	{
		base.List.Remove(operationMessage);
	}

	protected override void SetParent(object value, object parent)
	{
		((OperationMessage)value).SetParent((Operation)parent);
	}
}
