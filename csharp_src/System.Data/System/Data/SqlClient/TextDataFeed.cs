using System.IO;

namespace System.Data.SqlClient;

internal class TextDataFeed : DataFeed
{
	internal TextReader _source;

	internal TextDataFeed(TextReader source)
	{
		_source = source;
	}
}
