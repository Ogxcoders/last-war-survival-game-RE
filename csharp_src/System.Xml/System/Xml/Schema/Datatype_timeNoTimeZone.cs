namespace System.Xml.Schema;

internal class Datatype_timeNoTimeZone : Datatype_dateTimeBase
{
	internal Datatype_timeNoTimeZone()
		: base(XsdDateTimeFlags.XdrTimeNoTz)
	{
	}
}
