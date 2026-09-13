namespace System.Web.Services.Description;

public sealed class MessageCollection : ServiceDescriptionBaseCollection
{
	public Message this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (Message)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public Message this[string name]
	{
		get
		{
			int num = IndexOf((Message)Table[name]);
			if (num >= 0)
			{
				return this[num];
			}
			return null;
		}
	}

	internal MessageCollection(ServiceDescription serviceDescription)
		: base(serviceDescription)
	{
	}

	public int Add(Message message)
	{
		Insert(base.Count, message);
		return base.Count - 1;
	}

	public bool Contains(Message message)
	{
		return base.List.Contains(message);
	}

	public void CopyTo(Message[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is Message))
		{
			throw new InvalidCastException();
		}
		return ((Message)value).Name;
	}

	public int IndexOf(Message message)
	{
		return base.List.IndexOf(message);
	}

	public void Insert(int index, Message message)
	{
		base.List.Insert(index, message);
	}

	public void Remove(Message message)
	{
		base.List.Remove(message);
	}

	protected override void SetParent(object value, object parent)
	{
		((Message)value).SetParent((ServiceDescription)parent);
	}
}
