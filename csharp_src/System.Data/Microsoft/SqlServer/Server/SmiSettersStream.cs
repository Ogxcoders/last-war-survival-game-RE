using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace Microsoft.SqlServer.Server;

internal class SmiSettersStream : Stream
{
	private SmiEventSink_Default _sink;

	private ITypedSettersV3 _setters;

	private int _ordinal;

	private long _lengthWritten;

	private SmiMetaData _metaData;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length => _lengthWritten;

	public override long Position
	{
		get
		{
			return _lengthWritten;
		}
		set
		{
			throw SQL.StreamSeekNotSupported();
		}
	}

	internal SmiSettersStream(SmiEventSink_Default sink, ITypedSettersV3 setters, int ordinal, SmiMetaData metaData)
	{
		_sink = sink;
		_setters = setters;
		_ordinal = ordinal;
		_lengthWritten = 0L;
		_metaData = metaData;
	}

	public override void Flush()
	{
		_lengthWritten = ValueUtilsSmi.SetBytesLength(_sink, _setters, _ordinal, _metaData, _lengthWritten);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw SQL.StreamSeekNotSupported();
	}

	public override void SetLength(long value)
	{
		if (value < 0)
		{
			throw ADP.ArgumentOutOfRange("value");
		}
		ValueUtilsSmi.SetBytesLength(_sink, _setters, _ordinal, _metaData, value);
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw SQL.StreamReadNotSupported();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_lengthWritten += ValueUtilsSmi.SetBytes(_sink, _setters, _ordinal, _metaData, _lengthWritten, buffer, offset, count);
	}
}
