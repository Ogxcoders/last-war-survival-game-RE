using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaAll : XmlSchemaGroupBase
{
	private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

	[XmlElement("element", typeof(XmlSchemaElement))]
	public override XmlSchemaObjectCollection Items => items;

	internal override bool IsEmpty
	{
		get
		{
			if (!base.IsEmpty)
			{
				return items.Count == 0;
			}
			return true;
		}
	}

	internal override void SetItems(XmlSchemaObjectCollection newItems)
	{
		items = newItems;
	}
}
