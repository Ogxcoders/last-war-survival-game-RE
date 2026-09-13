using System.Globalization;

namespace System.Runtime.Serialization;

internal class DataNode<T> : IDataNode
{
	protected Type dataType;

	private T value;

	private string dataContractName;

	private string dataContractNamespace;

	private string clrTypeName;

	private string clrAssemblyName;

	private string id = Globals.NewObjectId;

	private bool isFinalValue;

	public Type DataType => dataType;

	public object Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = (T)value;
		}
	}

	bool IDataNode.IsFinalValue
	{
		get
		{
			return isFinalValue;
		}
		set
		{
			isFinalValue = value;
		}
	}

	public string DataContractName
	{
		get
		{
			return dataContractName;
		}
		set
		{
			dataContractName = value;
		}
	}

	public string DataContractNamespace
	{
		get
		{
			return dataContractNamespace;
		}
		set
		{
			dataContractNamespace = value;
		}
	}

	public string ClrTypeName
	{
		get
		{
			return clrTypeName;
		}
		set
		{
			clrTypeName = value;
		}
	}

	public string ClrAssemblyName
	{
		get
		{
			return clrAssemblyName;
		}
		set
		{
			clrAssemblyName = value;
		}
	}

	public bool PreservesReferences => Id != Globals.NewObjectId;

	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	internal DataNode()
	{
		dataType = typeof(T);
		isFinalValue = true;
	}

	internal DataNode(T value)
		: this()
	{
		this.value = value;
	}

	public T GetValue()
	{
		return value;
	}

	public virtual void GetData(ElementData element)
	{
		element.dataNode = this;
		element.attributeCount = 0;
		element.childElementIndex = 0;
		if (DataContractName != null)
		{
			AddQualifiedNameAttribute(element, "i", "type", "http://www.w3.org/2001/XMLSchema-instance", DataContractName, DataContractNamespace);
		}
		if (ClrTypeName != null)
		{
			element.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Type", ClrTypeName);
		}
		if (ClrAssemblyName != null)
		{
			element.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Assembly", ClrAssemblyName);
		}
	}

	public virtual void Clear()
	{
		clrTypeName = (clrAssemblyName = null);
	}

	internal void AddQualifiedNameAttribute(ElementData element, string elementPrefix, string elementName, string elementNs, string valueName, string valueNs)
	{
		string prefix = ExtensionDataReader.GetPrefix(valueNs);
		element.AddAttribute(elementPrefix, elementNs, elementName, string.Format(CultureInfo.InvariantCulture, "{0}:{1}", prefix, valueName));
		bool flag = false;
		if (element.attributes != null)
		{
			for (int i = 0; i < element.attributes.Length; i++)
			{
				AttributeData attributeData = element.attributes[i];
				if (attributeData != null && attributeData.prefix == "xmlns" && attributeData.localName == prefix)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			element.AddAttribute("xmlns", "http://www.w3.org/2000/xmlns/", prefix, valueNs);
		}
	}
}
