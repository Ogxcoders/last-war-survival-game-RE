using System;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server;

internal sealed class MemoryRecordBuffer : SmiRecordBuffer
{
	private SqlRecordBuffer[] _buffer;

	internal MemoryRecordBuffer(SmiMetaData[] metaData)
	{
		_buffer = new SqlRecordBuffer[metaData.Length];
		for (int i = 0; i < _buffer.Length; i++)
		{
			_buffer[i] = new SqlRecordBuffer(metaData[i]);
		}
	}

	public override bool IsDBNull(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].IsNull;
	}

	public override SmiMetaData GetVariantType(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].VariantType;
	}

	public override bool GetBoolean(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Boolean;
	}

	public override byte GetByte(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Byte;
	}

	public override long GetBytesLength(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].BytesLength;
	}

	public override int GetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		return _buffer[ordinal].GetBytes(fieldOffset, buffer, bufferOffset, length);
	}

	public override long GetCharsLength(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].CharsLength;
	}

	public override int GetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		return _buffer[ordinal].GetChars(fieldOffset, buffer, bufferOffset, length);
	}

	public override string GetString(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].String;
	}

	public override short GetInt16(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Int16;
	}

	public override int GetInt32(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Int32;
	}

	public override long GetInt64(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Int64;
	}

	public override float GetSingle(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Single;
	}

	public override double GetDouble(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Double;
	}

	public override SqlDecimal GetSqlDecimal(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].SqlDecimal;
	}

	public override DateTime GetDateTime(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].DateTime;
	}

	public override Guid GetGuid(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].Guid;
	}

	public override TimeSpan GetTimeSpan(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].TimeSpan;
	}

	public override DateTimeOffset GetDateTimeOffset(SmiEventSink sink, int ordinal)
	{
		return _buffer[ordinal].DateTimeOffset;
	}

	public override void SetDBNull(SmiEventSink sink, int ordinal)
	{
		_buffer[ordinal].SetNull();
	}

	public override void SetBoolean(SmiEventSink sink, int ordinal, bool value)
	{
		_buffer[ordinal].Boolean = value;
	}

	public override void SetByte(SmiEventSink sink, int ordinal, byte value)
	{
		_buffer[ordinal].Byte = value;
	}

	public override int SetBytes(SmiEventSink sink, int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		return _buffer[ordinal].SetBytes(fieldOffset, buffer, bufferOffset, length);
	}

	public override void SetBytesLength(SmiEventSink sink, int ordinal, long length)
	{
		_buffer[ordinal].BytesLength = length;
	}

	public override int SetChars(SmiEventSink sink, int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		return _buffer[ordinal].SetChars(fieldOffset, buffer, bufferOffset, length);
	}

	public override void SetCharsLength(SmiEventSink sink, int ordinal, long length)
	{
		_buffer[ordinal].CharsLength = length;
	}

	public override void SetString(SmiEventSink sink, int ordinal, string value, int offset, int length)
	{
		_buffer[ordinal].String = value.Substring(offset, length);
	}

	public override void SetInt16(SmiEventSink sink, int ordinal, short value)
	{
		_buffer[ordinal].Int16 = value;
	}

	public override void SetInt32(SmiEventSink sink, int ordinal, int value)
	{
		_buffer[ordinal].Int32 = value;
	}

	public override void SetInt64(SmiEventSink sink, int ordinal, long value)
	{
		_buffer[ordinal].Int64 = value;
	}

	public override void SetSingle(SmiEventSink sink, int ordinal, float value)
	{
		_buffer[ordinal].Single = value;
	}

	public override void SetDouble(SmiEventSink sink, int ordinal, double value)
	{
		_buffer[ordinal].Double = value;
	}

	public override void SetSqlDecimal(SmiEventSink sink, int ordinal, SqlDecimal value)
	{
		_buffer[ordinal].SqlDecimal = value;
	}

	public override void SetDateTime(SmiEventSink sink, int ordinal, DateTime value)
	{
		_buffer[ordinal].DateTime = value;
	}

	public override void SetGuid(SmiEventSink sink, int ordinal, Guid value)
	{
		_buffer[ordinal].Guid = value;
	}

	public override void SetTimeSpan(SmiEventSink sink, int ordinal, TimeSpan value)
	{
		_buffer[ordinal].TimeSpan = value;
	}

	public override void SetDateTimeOffset(SmiEventSink sink, int ordinal, DateTimeOffset value)
	{
		_buffer[ordinal].DateTimeOffset = value;
	}

	public override void SetVariantMetaData(SmiEventSink sink, int ordinal, SmiMetaData metaData)
	{
		_buffer[ordinal].VariantType = metaData;
	}
}
