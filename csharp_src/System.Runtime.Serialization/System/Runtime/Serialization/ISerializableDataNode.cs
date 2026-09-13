using System.Collections.Generic;

namespace System.Runtime.Serialization;

internal class ISerializableDataNode : DataNode<object>
{
	private string factoryTypeName;

	private string factoryTypeNamespace;

	private IList<ISerializableDataMember> members;

	internal string FactoryTypeName
	{
		get
		{
			return factoryTypeName;
		}
		set
		{
			factoryTypeName = value;
		}
	}

	internal string FactoryTypeNamespace
	{
		get
		{
			return factoryTypeNamespace;
		}
		set
		{
			factoryTypeNamespace = value;
		}
	}

	internal IList<ISerializableDataMember> Members
	{
		get
		{
			return members;
		}
		set
		{
			members = value;
		}
	}

	internal ISerializableDataNode()
	{
		dataType = Globals.TypeOfISerializableDataNode;
	}

	public override void GetData(ElementData element)
	{
		base.GetData(element);
		if (FactoryTypeName != null)
		{
			AddQualifiedNameAttribute(element, "z", "FactoryType", "http://schemas.microsoft.com/2003/10/Serialization/", FactoryTypeName, FactoryTypeNamespace);
		}
	}

	public override void Clear()
	{
		base.Clear();
		members = null;
		factoryTypeName = (factoryTypeNamespace = null);
	}
}
