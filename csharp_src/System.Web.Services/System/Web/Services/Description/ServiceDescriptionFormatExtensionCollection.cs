using System.Collections;
using System.Xml;

namespace System.Web.Services.Description;

public sealed class ServiceDescriptionFormatExtensionCollection : ServiceDescriptionBaseCollection
{
	public object this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public ServiceDescriptionFormatExtensionCollection(object parent)
		: base(parent)
	{
	}

	public int Add(object extension)
	{
		Insert(base.Count, extension);
		return base.Count - 1;
	}

	public bool Contains(object extension)
	{
		return base.List.Contains(extension);
	}

	public void CopyTo(object[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	public object Find(Type type)
	{
		foreach (object item in base.List)
		{
			if (type.IsInstanceOfType(item))
			{
				return item;
			}
		}
		return null;
	}

	public XmlElement Find(string name, string ns)
	{
		foreach (object item in base.List)
		{
			if (item is XmlElement)
			{
				XmlElement xmlElement = item as XmlElement;
				if (xmlElement.Name == name && xmlElement.NamespaceURI == ns)
				{
					return xmlElement;
				}
			}
		}
		return null;
	}

	public object[] FindAll(Type type)
	{
		ArrayList arrayList = new ArrayList();
		foreach (object item in base.List)
		{
			if (type.IsInstanceOfType(item))
			{
				arrayList.Add(item);
			}
		}
		object[] array = new object[arrayList.Count];
		if (arrayList.Count > 0)
		{
			arrayList.CopyTo(array);
		}
		return array;
	}

	public XmlElement[] FindAll(string name, string ns)
	{
		ArrayList arrayList = new ArrayList();
		foreach (object item in base.List)
		{
			if (item is XmlElement)
			{
				XmlElement xmlElement = item as XmlElement;
				if (xmlElement.Name == name && xmlElement.NamespaceURI == ns)
				{
					arrayList.Add(xmlElement);
				}
			}
		}
		XmlElement[] array = new XmlElement[arrayList.Count];
		if (arrayList.Count > 0)
		{
			arrayList.CopyTo(array);
		}
		return array;
	}

	public int IndexOf(object extension)
	{
		return base.List.IndexOf(extension);
	}

	public void Insert(int index, object extension)
	{
		base.List.Insert(index, extension);
	}

	[System.MonoTODO]
	public bool IsHandled(object item)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public bool IsRequired(object item)
	{
		throw new NotImplementedException();
	}

	protected override void OnValidate(object value)
	{
		if (value == null)
		{
			throw new ArgumentNullException();
		}
		if (!(value is XmlElement) && !(value is ServiceDescriptionFormatExtension))
		{
			throw new ArgumentException();
		}
	}

	public void Remove(object extension)
	{
		base.List.Remove(extension);
	}

	protected override void SetParent(object value, object parent)
	{
		if (value is ServiceDescriptionFormatExtension serviceDescriptionFormatExtension)
		{
			serviceDescriptionFormatExtension.SetParent(parent);
		}
	}
}
