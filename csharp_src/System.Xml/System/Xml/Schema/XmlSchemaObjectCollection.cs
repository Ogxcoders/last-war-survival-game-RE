using System.Collections;

namespace System.Xml.Schema;

public class XmlSchemaObjectCollection : CollectionBase
{
	private XmlSchemaObject parent;

	public virtual XmlSchemaObject this[int index]
	{
		get
		{
			return (XmlSchemaObject)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public XmlSchemaObjectCollection()
	{
	}

	public XmlSchemaObjectCollection(XmlSchemaObject parent)
	{
		this.parent = parent;
	}

	public new XmlSchemaObjectEnumerator GetEnumerator()
	{
		return new XmlSchemaObjectEnumerator(base.InnerList.GetEnumerator());
	}

	public int Add(XmlSchemaObject item)
	{
		return base.List.Add(item);
	}

	public void Insert(int index, XmlSchemaObject item)
	{
		base.List.Insert(index, item);
	}

	public int IndexOf(XmlSchemaObject item)
	{
		return base.List.IndexOf(item);
	}

	public bool Contains(XmlSchemaObject item)
	{
		return base.List.Contains(item);
	}

	public void Remove(XmlSchemaObject item)
	{
		base.List.Remove(item);
	}

	public void CopyTo(XmlSchemaObject[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	protected override void OnInsert(int index, object item)
	{
		if (parent != null)
		{
			parent.OnAdd(this, item);
		}
	}

	protected override void OnSet(int index, object oldValue, object newValue)
	{
		if (parent != null)
		{
			parent.OnRemove(this, oldValue);
			parent.OnAdd(this, newValue);
		}
	}

	protected override void OnClear()
	{
		if (parent != null)
		{
			parent.OnClear(this);
		}
	}

	protected override void OnRemove(int index, object item)
	{
		if (parent != null)
		{
			parent.OnRemove(this, item);
		}
	}

	internal XmlSchemaObjectCollection Clone()
	{
		return new XmlSchemaObjectCollection { this };
	}

	private void Add(XmlSchemaObjectCollection collToAdd)
	{
		base.InnerList.InsertRange(0, collToAdd);
	}
}
