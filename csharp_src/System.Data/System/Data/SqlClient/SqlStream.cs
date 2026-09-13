using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace System.Data.SqlClient;

internal sealed class SqlStream : Stream
{
	private SqlDataReader _reader;

	private int _columnOrdinal;

	private long _bytesCol;

	private int _bom;

	private byte[] _bufferedData;

	private bool _processAllRows;

	private bool _advanceReader;

	private bool _readFirstRow;

	private bool _endOfColumn;

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw ADP.NotSupported();
		}
	}

	public override long Position
	{
		get
		{
			throw ADP.NotSupported();
		}
		set
		{
			throw ADP.NotSupported();
		}
	}

	internal SqlStream(SqlDataReader reader, bool addByteOrderMark, bool processAllRows)
		: this(0, reader, addByteOrderMark, processAllRows, advanceReader: true)
	{
	}

	internal SqlStream(int columnOrdinal, SqlDataReader reader, bool addByteOrderMark, bool processAllRows, bool advanceReader)
	{
		_columnOrdinal = columnOrdinal;
		_reader = reader;
		_bom = (addByteOrderMark ? 65279 : 0);
		_processAllRows = processAllRows;
		_advanceReader = advanceReader;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && _advanceReader && _reader != null && !_reader.IsClosed)
			{
				_reader.Close();
			}
			_reader = null;
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override void Flush()
	{
		throw ADP.NotSupported();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = 0;
		int num2 = 0;
		if (_reader == null)
		{
			throw ADP.StreamClosed("Read");
		}
		if (buffer == null)
		{
			throw ADP.ArgumentNull("buffer");
		}
		if (offset < 0 || count < 0)
		{
			throw ADP.ArgumentOutOfRange(string.Empty, (offset < 0) ? "offset" : "count");
		}
		if (buffer.Length - offset < count)
		{
			throw ADP.ArgumentOutOfRange("count");
		}
		if (_bom > 0)
		{
			_bufferedData = new byte[2];
			num2 = ReadBytes(_bufferedData, 0, 2);
			if (num2 < 2 || (_bufferedData[0] == 223 && _bufferedData[1] == byte.MaxValue))
			{
				_bom = 0;
			}
			while (count > 0 && _bom > 0)
			{
				buffer[offset] = (byte)_bom;
				_bom >>= 8;
				offset++;
				count--;
				num++;
			}
		}
		if (num2 > 0)
		{
			while (count > 0)
			{
				buffer[offset++] = _bufferedData[0];
				num++;
				count--;
				if (num2 > 1 && count > 0)
				{
					buffer[offset++] = _bufferedData[1];
					num++;
					count--;
					break;
				}
			}
			_bufferedData = null;
		}
		return num + ReadBytes(buffer, offset, count);
	}

	private static bool AdvanceToNextRow(SqlDataReader reader)
	{
		do
		{
			if (reader.Read())
			{
				return true;
			}
		}
		while (reader.NextResult());
		return false;
	}

	private int ReadBytes(byte[] buffer, int offset, int count)
	{
		bool flag = true;
		int num = 0;
		int num2 = 0;
		if (_reader.IsClosed || _endOfColumn)
		{
			return 0;
		}
		try
		{
			while (count > 0)
			{
				if (_advanceReader && _bytesCol == 0L)
				{
					flag = false;
					if ((!_readFirstRow || _processAllRows) && AdvanceToNextRow(_reader))
					{
						_readFirstRow = true;
						if (_reader.IsDBNull(_columnOrdinal))
						{
							continue;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
				num2 = (int)_reader.GetBytesInternal(_columnOrdinal, _bytesCol, buffer, offset, count);
				if (num2 < count)
				{
					_bytesCol = 0L;
					flag = false;
					if (!_advanceReader)
					{
						_endOfColumn = true;
					}
				}
				else
				{
					_bytesCol += num2;
				}
				count -= num2;
				offset += num2;
				num += num2;
			}
			if (!flag && _advanceReader)
			{
				_reader.Close();
			}
		}
		catch (Exception e)
		{
			if (_advanceReader && ADP.IsCatchableExceptionType(e))
			{
				_reader.Close();
			}
			throw;
		}
		return num;
	}

	internal XmlReader ToXmlReader(bool async = false)
	{
		return SqlTypeWorkarounds.SqlXmlCreateSqlXmlReader(this, closeInput: true, async);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw ADP.NotSupported();
	}

	public override void SetLength(long value)
	{
		throw ADP.NotSupported();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw ADP.NotSupported();
	}
}
