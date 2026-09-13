namespace System.Web.Services.Description;

public sealed class MessagePartCollection : ServiceDescriptionBaseCollection
{
	public MessagePart this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (MessagePart)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public MessagePart this[string name] => this[IndexOf((MessagePart)Table[name])];

	internal MessagePartCollection(Message message)
		: base(message)
	{
	}

	public int Add(MessagePart messagePart)
	{
		Insert(base.Count, messagePart);
		return base.Count - 1;
	}

	public bool Contains(MessagePart messagePart)
	{
		return base.List.Contains(messagePart);
	}

	public void CopyTo(MessagePart[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is MessagePart))
		{
			throw new InvalidCastException();
		}
		return ((MessagePart)value).Name;
	}

	public int IndexOf(MessagePart messagePart)
	{
		return base.List.IndexOf(messagePart);
	}

	public void Insert(int index, MessagePart messagePart)
	{
		base.List.Insert(index, messagePart);
	}

	public void Remove(MessagePart messagePart)
	{
		base.List.Remove(messagePart);
	}

	protected override void SetParent(object value, object parent)
	{
		((MessagePart)value).SetParent((Message)parent);
	}
}
