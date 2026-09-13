namespace System.Web.Services.Description;

public sealed class ImportCollection : ServiceDescriptionBaseCollection
{
	public Import this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (Import)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal ImportCollection(ServiceDescription serviceDescription)
		: base(serviceDescription)
	{
	}

	public int Add(Import import)
	{
		Insert(base.Count, import);
		return base.Count - 1;
	}

	public bool Contains(Import import)
	{
		return base.List.Contains(import);
	}

	public void CopyTo(Import[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	public int IndexOf(Import import)
	{
		return base.List.IndexOf(import);
	}

	public void Insert(int index, Import import)
	{
		base.List.Insert(index, import);
	}

	public void Remove(Import import)
	{
		base.List.Remove(import);
	}

	protected override void SetParent(object value, object parent)
	{
		((Import)value).SetParent((ServiceDescription)parent);
	}
}
