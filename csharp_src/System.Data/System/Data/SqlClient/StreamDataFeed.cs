using System.IO;

namespace System.Data.SqlClient;

internal class StreamDataFeed : DataFeed
{
	internal Stream _source;

	internal StreamDataFeed(Stream source)
	{
		_source = source;
	}
}
