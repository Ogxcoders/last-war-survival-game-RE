namespace System.Xml.Schema;

internal class Datatype_time : Datatype_dateTimeBase
{
	public override XmlTypeCode TypeCode => XmlTypeCode.Time;

	internal Datatype_time()
		: base(XsdDateTimeFlags.Time)
	{
	}
}
