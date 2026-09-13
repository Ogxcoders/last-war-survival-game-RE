using System.Data.SqlClient;
using System.IO;

namespace Microsoft.SqlServer.Server;

internal class SmiGettersStream : Stream
{
	private SmiEventSink_Default _sink;

	private ITypedGettersV3 _getters;

	private int _ordinal;

	private long _readPosition;

	private SmiMetaData _metaData;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length => ValueUtilsSmi.GetBytesInternal(_sink, _getters, _ordinal, _metaData, 0L, null, 0, 0, throwOnNull: false);

	public override long Position
	{
		get
		{
			return _readPosition;
		}
		set
		{
			throw SQL.StreamSeekNotSupported();
		}
	}

	internal SmiGettersStream(SmiEventSink_Default sink, ITypedGettersV3 getters, int ordinal, SmiMetaData metaData)
	{
		_sink = sink;
		_getters = getters;
		_ordinal = ordinal;
		_readPosition = 0L;
		_metaData = metaData;
	}

	public override void Flush()
	{
		throw SQL.StreamWriteNotSupported();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw SQL.StreamSeekNotSupported();
	}

	public override void SetLength(long value)
	{
		throw SQL.StreamWriteNotSupported();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		long bytesInternal = ValueUtilsSmi.GetBytesInternal(_sink, _getters, _ordinal, _metaData, _readPosition, buffer, offset, count, throwOnNull: false);
		_readPosition += bytesInternal;
		return checked((int)bytesInternal);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw SQL.StreamWriteNotSupported();
	}
}
