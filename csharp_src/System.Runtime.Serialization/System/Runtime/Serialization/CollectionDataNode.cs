using System.Collections.Generic;
using System.Globalization;

namespace System.Runtime.Serialization;

internal class CollectionDataNode : DataNode<Array>
{
	private IList<IDataNode> items;

	private string itemName;

	private string itemNamespace;

	private int size = -1;

	internal IList<IDataNode> Items
	{
		get
		{
			return items;
		}
		set
		{
			items = value;
		}
	}

	internal string ItemName
	{
		get
		{
			return itemName;
		}
		set
		{
			itemName = value;
		}
	}

	internal string ItemNamespace
	{
		get
		{
			return itemNamespace;
		}
		set
		{
			itemNamespace = value;
		}
	}

	internal int Size
	{
		get
		{
			return size;
		}
		set
		{
			size = value;
		}
	}

	internal CollectionDataNode()
	{
		dataType = Globals.TypeOfCollectionDataNode;
	}

	public override void GetData(ElementData element)
	{
		base.GetData(element);
		element.AddAttribute("z", "http://schemas.microsoft.com/2003/10/Serialization/", "Size", Size.ToString(NumberFormatInfo.InvariantInfo));
	}

	public override void Clear()
	{
		base.Clear();
		items = null;
		size = -1;
	}
}
