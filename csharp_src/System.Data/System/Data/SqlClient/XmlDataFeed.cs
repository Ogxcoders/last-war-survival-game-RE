using System.Xml;

namespace System.Data.SqlClient;

internal class XmlDataFeed : DataFeed
{
	internal XmlReader _source;

	internal XmlDataFeed(XmlReader source)
	{
		_source = source;
	}
}
