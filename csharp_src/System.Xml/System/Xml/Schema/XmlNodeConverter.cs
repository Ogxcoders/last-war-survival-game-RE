using System.Xml.XPath;

namespace System.Xml.Schema;

internal class XmlNodeConverter : XmlBaseConverter
{
	public static readonly XmlValueConverter Node = new XmlNodeConverter();

	protected XmlNodeConverter()
		: base(XmlTypeCode.Node)
	{
	}

	public override object ChangeType(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (destinationType == null)
		{
			throw new ArgumentNullException("destinationType");
		}
		Type type = value.GetType();
		if (destinationType == XmlBaseConverter.ObjectType)
		{
			destinationType = base.DefaultClrType;
		}
		if (destinationType == XmlBaseConverter.XPathNavigatorType && XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XPathNavigatorType))
		{
			return (XPathNavigator)value;
		}
		if (destinationType == XmlBaseConverter.XPathItemType && XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XPathNavigatorType))
		{
			return (XPathItem)value;
		}
		return ChangeListType(value, destinationType, nsResolver);
	}
}
