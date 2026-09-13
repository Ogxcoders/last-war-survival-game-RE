namespace System.Xml.Schema;

internal class Datatype_month : Datatype_dateTimeBase
{
	public override XmlTypeCode TypeCode => XmlTypeCode.GMonth;

	internal Datatype_month()
		: base(XsdDateTimeFlags.GMonth)
	{
	}
}
