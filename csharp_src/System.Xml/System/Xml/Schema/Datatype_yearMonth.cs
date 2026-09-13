namespace System.Xml.Schema;

internal class Datatype_yearMonth : Datatype_dateTimeBase
{
	public override XmlTypeCode TypeCode => XmlTypeCode.GYearMonth;

	internal Datatype_yearMonth()
		: base(XsdDateTimeFlags.GYearMonth)
	{
	}
}
