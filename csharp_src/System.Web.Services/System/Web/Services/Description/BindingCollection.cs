namespace System.Web.Services.Description;

public sealed class BindingCollection : ServiceDescriptionBaseCollection
{
	private ServiceDescription serviceDescription;

	public Binding this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (Binding)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public Binding this[string name] => (Binding)Table[name];

	internal BindingCollection(ServiceDescription serviceDescription)
		: base(serviceDescription)
	{
	}

	public int Add(Binding binding)
	{
		Insert(base.Count, binding);
		return base.Count - 1;
	}

	public bool Contains(Binding binding)
	{
		return base.List.Contains(binding);
	}

	public void CopyTo(Binding[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override string GetKey(object value)
	{
		if (!(value is Binding))
		{
			throw new InvalidCastException();
		}
		return ((Binding)value).Name;
	}

	public int IndexOf(Binding binding)
	{
		return base.List.IndexOf(binding);
	}

	public void Insert(int index, Binding binding)
	{
		base.List.Insert(index, binding);
	}

	public void Remove(Binding binding)
	{
		base.List.Remove(binding);
	}

	protected override void SetParent(object value, object parent)
	{
		((Binding)value).SetParent((ServiceDescription)parent);
	}
}
