using System.Collections;
using System.Text;

namespace System.Xml.Serialization;

public class XmlElementAttributes : CollectionBase
{
	public XmlElementAttribute this[int index]
	{
		get
		{
			return (XmlElementAttribute)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	internal int Order
	{
		get
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					XmlElementAttribute xmlElementAttribute = (XmlElementAttribute)enumerator.Current;
					if (xmlElementAttribute.Order >= 0)
					{
						return xmlElementAttribute.Order;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return -1;
		}
	}

	public int Add(XmlElementAttribute attribute)
	{
		return base.List.Add(attribute);
	}

	public bool Contains(XmlElementAttribute attribute)
	{
		return base.List.Contains(attribute);
	}

	public int IndexOf(XmlElementAttribute attribute)
	{
		return base.List.IndexOf(attribute);
	}

	public void Insert(int index, XmlElementAttribute attribute)
	{
		base.List.Insert(index, attribute);
	}

	public void Remove(XmlElementAttribute attribute)
	{
		base.List.Remove(attribute);
	}

	public void CopyTo(XmlElementAttribute[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void AddKeyHash(StringBuilder sb)
	{
		if (base.Count != 0)
		{
			sb.Append("XEAS ");
			for (int i = 0; i < base.Count; i++)
			{
				this[i].AddKeyHash(sb);
			}
			sb.Append('|');
		}
	}
}
