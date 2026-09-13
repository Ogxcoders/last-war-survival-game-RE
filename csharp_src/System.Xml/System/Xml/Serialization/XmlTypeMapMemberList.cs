namespace System.Xml.Serialization;

internal class XmlTypeMapMemberList : XmlTypeMapMemberElement
{
	public XmlTypeMapping ListTypeMapping => ((XmlTypeMapElementInfo)base.ElementInfo[0]).MappedType;

	public string ElementName => ((XmlTypeMapElementInfo)base.ElementInfo[0]).ElementName;

	public string Namespace => ((XmlTypeMapElementInfo)base.ElementInfo[0]).Namespace;
}
