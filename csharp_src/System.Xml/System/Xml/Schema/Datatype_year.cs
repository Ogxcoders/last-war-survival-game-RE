namespace System.Xml.Schema;

internal class Datatype_year : Datatype_dateTimeBase
{
	public override XmlTypeCode TypeCode => XmlTypeCode.GYear;

	internal Datatype_year()
		: base(XsdDateTimeFlags.GYear)
	{
	}
}
