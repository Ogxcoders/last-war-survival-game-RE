using System.Collections;

namespace System.Web.Services.Description;

public abstract class ServiceDescriptionBaseCollection : CollectionBase
{
	private Hashtable table = new Hashtable();

	private object parent;

	protected virtual IDictionary Table => table;

	internal ServiceDescriptionBaseCollection(object parent)
	{
		this.parent = parent;
	}

	protected virtual string GetKey(object value)
	{
		return null;
	}

	protected override void OnClear()
	{
		Table.Clear();
	}

	protected override void OnInsertComplete(int index, object value)
	{
		if (GetKey(value) != null)
		{
			Table[GetKey(value)] = value;
		}
		SetParent(value, parent);
	}

	protected override void OnRemove(int index, object value)
	{
		if (GetKey(value) != null)
		{
			Table.Remove(GetKey(value));
		}
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (GetKey(oldValue) != null)
		{
			Table.Remove(GetKey(oldValue));
		}
		if (GetKey(newValue) != null)
		{
			Table[GetKey(newValue)] = newValue;
		}
		SetParent(newValue, parent);
	}

	protected virtual void SetParent(object value, object parent)
	{
	}
}
