namespace System.Xml.Schema;

internal class Datatype_ENTITY : Datatype_NCName
{
	public override XmlTypeCode TypeCode => XmlTypeCode.Entity;

	public override XmlTokenizedType TokenizedType => XmlTokenizedType.ENTITY;
}
