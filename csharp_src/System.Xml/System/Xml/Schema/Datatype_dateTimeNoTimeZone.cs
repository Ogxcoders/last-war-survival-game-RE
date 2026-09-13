namespace System.Xml.Schema;

internal class Datatype_dateTimeNoTimeZone : Datatype_dateTimeBase
{
	internal Datatype_dateTimeNoTimeZone()
		: base(XsdDateTimeFlags.XdrDateTimeNoTz)
	{
	}
}
