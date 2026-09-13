using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes;

[Serializable]
[XmlSchemaProvider("GetXsdType")]
public sealed class SqlBytes : INullable, IXmlSerializable, ISerializable
{
	internal byte[] _rgbBuf;

	private long _lCurLen;

	internal Stream _stream;

	private SqlBytesCharsState _state;

	private byte[] _rgbWorkBuf;

	private const long x_lMaxLen = 2147483647L;

	private const long x_lNull = -1L;

	public bool IsNull => _state == SqlBytesCharsState.Null;

	public byte[] Buffer
	{
		get
		{
			if (FStream())
			{
				CopyStreamToBuffer();
			}
			return _rgbBuf;
		}
	}

	public long Length => _state switch
	{
		SqlBytesCharsState.Null => throw new SqlNullValueException(), 
		SqlBytesCharsState.Stream => _stream.Length, 
		_ => _lCurLen, 
	};

	public long MaxLength
	{
		get
		{
			if (_state == SqlBytesCharsState.Stream)
			{
				return -1L;
			}
			if (_rgbBuf != null)
			{
				return _rgbBuf.Length;
			}
			return -1L;
		}
	}

	public byte[] Value
	{
		get
		{
			byte[] array;
			switch (_state)
			{
			case SqlBytesCharsState.Null:
				throw new SqlNullValueException();
			case SqlBytesCharsState.Stream:
				if (_stream.Length > int.MaxValue)
				{
					throw new SqlTypeException("The buffer is insufficient. Read or write operation failed.");
				}
				array = new byte[_stream.Length];
				if (_stream.Position != 0L)
				{
					_stream.Seek(0L, SeekOrigin.Begin);
				}
				_stream.Read(array, 0, checked((int)_stream.Length));
				break;
			default:
				array = new byte[_lCurLen];
				Array.Copy(_rgbBuf, 0, array, 0, (int)_lCurLen);
				break;
			}
			return array;
		}
	}

	public byte this[long offset]
	{
		get
		{
			if (offset < 0 || offset >= Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (_rgbWorkBuf == null)
			{
				_rgbWorkBuf = new byte[1];
			}
			Read(offset, _rgbWorkBuf, 0, 1);
			return _rgbWorkBuf[0];
		}
		set
		{
			if (_rgbWorkBuf == null)
			{
				_rgbWorkBuf = new byte[1];
			}
			_rgbWorkBuf[0] = value;
			Write(offset, _rgbWorkBuf, 0, 1);
		}
	}

	public StorageState Storage => _state switch
	{
		SqlBytesCharsState.Null => throw new SqlNullValueException(), 
		SqlBytesCharsState.Stream => StorageState.Stream, 
		SqlBytesCharsState.Buffer => StorageState.Buffer, 
		_ => StorageState.UnmanagedBuffer, 
	};

	public Stream Stream
	{
		get
		{
			if (!FStream())
			{
				return new StreamOnSqlBytes(this);
			}
			return _stream;
		}
		set
		{
			_lCurLen = -1L;
			_stream = value;
			_state = ((value != null) ? SqlBytesCharsState.Stream : SqlBytesCharsState.Null);
		}
	}

	public static SqlBytes Null => new SqlBytes((byte[])null);

	public SqlBytes()
	{
		SetNull();
	}

	public SqlBytes(byte[] buffer)
	{
		_rgbBuf = buffer;
		_stream = null;
		if (_rgbBuf == null)
		{
			_state = SqlBytesCharsState.Null;
			_lCurLen = -1L;
		}
		else
		{
			_state = SqlBytesCharsState.Buffer;
			_lCurLen = _rgbBuf.Length;
		}
		_rgbWorkBuf = null;
	}

	public SqlBytes(SqlBinary value)
		: this(value.IsNull ? null : value.Value)
	{
	}

	public SqlBytes(Stream s)
	{
		_rgbBuf = null;
		_lCurLen = -1L;
		_stream = s;
		_state = ((s != null) ? SqlBytesCharsState.Stream : SqlBytesCharsState.Null);
		_rgbWorkBuf = null;
	}

	public void SetNull()
	{
		_lCurLen = -1L;
		_stream = null;
		_state = SqlBytesCharsState.Null;
	}

	public void SetLength(long value)
	{
		if (value < 0)
		{
			throw new ArgumentOutOfRangeException("value");
		}
		if (FStream())
		{
			_stream.SetLength(value);
			return;
		}
		if (_rgbBuf == null)
		{
			throw new SqlTypeException("There is no buffer. Read or write operation failed.");
		}
		if (value > _rgbBuf.Length)
		{
			throw new ArgumentOutOfRangeException("value");
		}
		if (IsNull)
		{
			_state = SqlBytesCharsState.Buffer;
		}
		_lCurLen = value;
	}

	public long Read(long offset, byte[] buffer, int offsetInBuffer, int count)
	{
		if (IsNull)
		{
			throw new SqlNullValueException();
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset > Length || offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offsetInBuffer > buffer.Length || offsetInBuffer < 0)
		{
			throw new ArgumentOutOfRangeException("offsetInBuffer");
		}
		if (count < 0 || count > buffer.Length - offsetInBuffer)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > Length - offset)
		{
			count = (int)(Length - offset);
		}
		if (count != 0)
		{
			if (_state == SqlBytesCharsState.Stream)
			{
				if (_stream.Position != offset)
				{
					_stream.Seek(offset, SeekOrigin.Begin);
				}
				_stream.Read(buffer, offsetInBuffer, count);
			}
			else
			{
				Array.Copy(_rgbBuf, offset, buffer, offsetInBuffer, count);
			}
		}
		return count;
	}

	public void Write(long offset, byte[] buffer, int offsetInBuffer, int count)
	{
		if (FStream())
		{
			if (_stream.Position != offset)
			{
				_stream.Seek(offset, SeekOrigin.Begin);
			}
			_stream.Write(buffer, offsetInBuffer, count);
			return;
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (_rgbBuf == null)
		{
			throw new SqlTypeException("There is no buffer. Read or write operation failed.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (offset > _rgbBuf.Length)
		{
			throw new SqlTypeException("The buffer is insufficient. Read or write operation failed.");
		}
		if (offsetInBuffer < 0 || offsetInBuffer > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offsetInBuffer");
		}
		if (count < 0 || count > buffer.Length - offsetInBuffer)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > _rgbBuf.Length - offset)
		{
			throw new SqlTypeException("The buffer is insufficient. Read or write operation failed.");
		}
		if (IsNull)
		{
			if (offset != 0L)
			{
				throw new SqlTypeException("Cannot write to non-zero offset, because current value is Null.");
			}
			_lCurLen = 0L;
			_state = SqlBytesCharsState.Buffer;
		}
		else if (offset > _lCurLen)
		{
			throw new SqlTypeException("Cannot write from an offset that is larger than current length. It would leave uninitialized data in the buffer.");
		}
		if (count != 0)
		{
			Array.Copy(buffer, offsetInBuffer, _rgbBuf, offset, count);
			if (_lCurLen < offset + count)
			{
				_lCurLen = offset + count;
			}
		}
	}

	public SqlBinary ToSqlBinary()
	{
		if (!IsNull)
		{
			return new SqlBinary(Value);
		}
		return SqlBinary.Null;
	}

	public static explicit operator SqlBinary(SqlBytes value)
	{
		return value.ToSqlBinary();
	}

	public static explicit operator SqlBytes(SqlBinary value)
	{
		return new SqlBytes(value);
	}

	[Conditional("DEBUG")]
	private void AssertValid()
	{
		_ = IsNull;
	}

	private void CopyStreamToBuffer()
	{
		long length = _stream.Length;
		if (length >= int.MaxValue)
		{
			throw new SqlTypeException("Cannot write from an offset that is larger than current length. It would leave uninitialized data in the buffer.");
		}
		if (_rgbBuf == null || _rgbBuf.Length < length)
		{
			_rgbBuf = new byte[length];
		}
		if (_stream.Position != 0L)
		{
			_stream.Seek(0L, SeekOrigin.Begin);
		}
		_stream.Read(_rgbBuf, 0, (int)length);
		_stream = null;
		_lCurLen = length;
		_state = SqlBytesCharsState.Buffer;
	}

	internal bool FStream()
	{
		return _state == SqlBytesCharsState.Stream;
	}

	private void SetBuffer(byte[] buffer)
	{
		_rgbBuf = buffer;
		_lCurLen = ((_rgbBuf == null) ? (-1) : _rgbBuf.Length);
		_stream = null;
		_state = ((_rgbBuf != null) ? SqlBytesCharsState.Buffer : SqlBytesCharsState.Null);
	}

	XmlSchema IXmlSerializable.GetSchema()
	{
		return null;
	}

	void IXmlSerializable.ReadXml(XmlReader r)
	{
		byte[] buffer = null;
		string attribute = r.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
		if (attribute != null && XmlConvert.ToBoolean(attribute))
		{
			r.ReadElementString();
			SetNull();
		}
		else
		{
			string text = r.ReadElementString();
			if (text == null)
			{
				buffer = Array.Empty<byte>();
			}
			else
			{
				text = text.Trim();
				buffer = ((text.Length != 0) ? Convert.FromBase64String(text) : Array.Empty<byte>());
			}
		}
		SetBuffer(buffer);
	}

	void IXmlSerializable.WriteXml(XmlWriter writer)
	{
		if (IsNull)
		{
			writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			return;
		}
		byte[] buffer = Buffer;
		writer.WriteString(Convert.ToBase64String(buffer, 0, (int)Length));
	}

	public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
	{
		return new XmlQualifiedName("base64Binary", "http://www.w3.org/2001/XMLSchema");
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new PlatformNotSupportedException();
	}
}
