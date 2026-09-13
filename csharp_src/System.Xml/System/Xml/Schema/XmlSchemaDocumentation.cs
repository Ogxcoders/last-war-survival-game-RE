using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaDocumentation : XmlSchemaObject
{
	private string source;

	private string language;

	private XmlNode[] markup;

	private static XmlSchemaSimpleType languageType = DatatypeImplementation.GetSimpleTypeFromXsdType(new XmlQualifiedName("language", "http://www.w3.org/2001/XMLSchema"));

	[XmlAttribute("source", DataType = "anyURI")]
	public string Source
	{
		get
		{
			return source;
		}
		set
		{
			source = value;
		}
	}

	[XmlAttribute("xml:lang")]
	public string Language
	{
		get
		{
			return language;
		}
		set
		{
			language = (string)languageType.Datatype.ParseValue(value, null, null);
		}
	}

	[XmlText]
	[XmlAnyElement]
	public XmlNode[] Markup
	{
		get
		{
			return markup;
		}
		set
		{
			markup = value;
		}
	}
}
