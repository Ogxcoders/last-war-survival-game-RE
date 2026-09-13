using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient;

internal sealed class SqlBuffer
{
	internal enum StorageType
	{
		Empty,
		Boolean,
		Byte,
		DateTime,
		Decimal,
		Double,
		Int16,
		Int32,
		Int64,
		Money,
		Single,
		String,
		SqlBinary,
		SqlCachedBuffer,
		SqlGuid,
		SqlXml,
		Date,
		DateTime2,
		DateTimeOffset,
		Time
	}

	internal struct DateTimeInfo
	{
		internal int daypart;

		internal int timepart;
	}

	internal struct NumericInfo
	{
		internal int data1;

		internal int data2;

		internal int data3;

		internal int data4;

		internal byte precision;

		internal byte scale;

		internal bool positive;
	}

	internal struct TimeInfo
	{
		internal long ticks;

		internal byte scale;
	}

	internal struct DateTime2Info
	{
		internal int date;

		internal TimeInfo timeInfo;
	}

	internal struct DateTimeOffsetInfo
	{
		internal DateTime2Info dateTime2Info;

		internal short offset;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct Storage
	{
		[FieldOffset(0)]
		internal bool _boolean;

		[FieldOffset(0)]
		internal byte _byte;

		[FieldOffset(0)]
		internal DateTimeInfo _dateTimeInfo;

		[FieldOffset(0)]
		internal double _double;

		[FieldOffset(0)]
		internal NumericInfo _numericInfo;

		[FieldOffset(0)]
		internal short _int16;

		[FieldOffset(0)]
		internal int _int32;

		[FieldOffset(0)]
		internal long _int64;

		[FieldOffset(0)]
		internal float _single;

		[FieldOffset(0)]
		internal TimeInfo _timeInfo;

		[FieldOffset(0)]
		internal DateTime2Info _dateTime2Info;

		[FieldOffset(0)]
		internal DateTimeOffsetInfo _dateTimeOffsetInfo;
	}

	private bool _isNull;

	private StorageType _type;

	private Storage _value;

	private object _object;

	private static string[] s_katmaiDateTimeOffsetFormatByScale = new string[8] { "yyyy-MM-dd HH:mm:ss zzz", "yyyy-MM-dd HH:mm:ss.f zzz", "yyyy-MM-dd HH:mm:ss.ff zzz", "yyyy-MM-dd HH:mm:ss.fff zzz", "yyyy-MM-dd HH:mm:ss.ffff zzz", "yyyy-MM-dd HH:mm:ss.fffff zzz", "yyyy-MM-dd HH:mm:ss.ffffff zzz", "yyyy-MM-dd HH:mm:ss.fffffff zzz" };

	private static string[] s_katmaiDateTime2FormatByScale = new string[8] { "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss.f", "yyyy-MM-dd HH:mm:ss.ff", "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss.ffff", "yyyy-MM-dd HH:mm:ss.fffff", "yyyy-MM-dd HH:mm:ss.ffffff", "yyyy-MM-dd HH:mm:ss.fffffff" };

	private static string[] s_katmaiTimeFormatByScale = new string[8] { "HH:mm:ss", "HH:mm:ss.f", "HH:mm:ss.ff", "HH:mm:ss.fff", "HH:mm:ss.ffff", "HH:mm:ss.fffff", "HH:mm:ss.ffffff", "HH:mm:ss.fffffff" };

	internal bool IsEmpty => _type == StorageType.Empty;

	internal bool IsNull => _isNull;

	internal StorageType VariantInternalStorageType => _type;

	internal bool Boolean
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Boolean == _type)
			{
				return _value._boolean;
			}
			return (bool)Value;
		}
		set
		{
			_value._boolean = value;
			_type = StorageType.Boolean;
			_isNull = false;
		}
	}

	internal byte Byte
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Byte == _type)
			{
				return _value._byte;
			}
			return (byte)Value;
		}
		set
		{
			_value._byte = value;
			_type = StorageType.Byte;
			_isNull = false;
		}
	}

	internal byte[] ByteArray
	{
		get
		{
			ThrowIfNull();
			return SqlBinary.Value;
		}
	}

	internal DateTime DateTime
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Date == _type)
			{
				return DateTime.MinValue.AddDays(_value._int32);
			}
			if (StorageType.DateTime2 == _type)
			{
				return new DateTime(GetTicksFromDateTime2Info(_value._dateTime2Info));
			}
			if (StorageType.DateTime == _type)
			{
				return SqlTypeWorkarounds.SqlDateTimeToDateTime(_value._dateTimeInfo.daypart, _value._dateTimeInfo.timepart);
			}
			return (DateTime)Value;
		}
	}

	internal decimal Decimal
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Decimal == _type)
			{
				if (_value._numericInfo.data4 != 0 || _value._numericInfo.scale > 28)
				{
					throw new OverflowException(SQLResource.ConversionOverflowMessage);
				}
				return new decimal(_value._numericInfo.data1, _value._numericInfo.data2, _value._numericInfo.data3, !_value._numericInfo.positive, _value._numericInfo.scale);
			}
			if (StorageType.Money == _type)
			{
				long num = _value._int64;
				bool isNegative = false;
				if (num < 0)
				{
					isNegative = true;
					num = -num;
				}
				return new decimal((int)(num & 0xFFFFFFFFu), (int)(num >> 32), 0, isNegative, 4);
			}
			return (decimal)Value;
		}
	}

	internal double Double
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Double == _type)
			{
				return _value._double;
			}
			return (double)Value;
		}
		set
		{
			_value._double = value;
			_type = StorageType.Double;
			_isNull = false;
		}
	}

	internal Guid Guid
	{
		get
		{
			ThrowIfNull();
			return SqlGuid.Value;
		}
	}

	internal short Int16
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Int16 == _type)
			{
				return _value._int16;
			}
			return (short)Value;
		}
		set
		{
			_value._int16 = value;
			_type = StorageType.Int16;
			_isNull = false;
		}
	}

	internal int Int32
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Int32 == _type)
			{
				return _value._int32;
			}
			return (int)Value;
		}
		set
		{
			_value._int32 = value;
			_type = StorageType.Int32;
			_isNull = false;
		}
	}

	internal long Int64
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Int64 == _type)
			{
				return _value._int64;
			}
			return (long)Value;
		}
		set
		{
			_value._int64 = value;
			_type = StorageType.Int64;
			_isNull = false;
		}
	}

	internal float Single
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Single == _type)
			{
				return _value._single;
			}
			return (float)Value;
		}
		set
		{
			_value._single = value;
			_type = StorageType.Single;
			_isNull = false;
		}
	}

	internal string String
	{
		get
		{
			ThrowIfNull();
			if (StorageType.String == _type)
			{
				return (string)_object;
			}
			if (StorageType.SqlCachedBuffer == _type)
			{
				return ((SqlCachedBuffer)_object).ToString();
			}
			return (string)Value;
		}
	}

	internal string KatmaiDateTimeString
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Date == _type)
			{
				return DateTime.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
			}
			if (StorageType.Time == _type)
			{
				byte scale = _value._timeInfo.scale;
				return new DateTime(_value._timeInfo.ticks).ToString(s_katmaiTimeFormatByScale[scale], DateTimeFormatInfo.InvariantInfo);
			}
			if (StorageType.DateTime2 == _type)
			{
				byte scale2 = _value._dateTime2Info.timeInfo.scale;
				return DateTime.ToString(s_katmaiDateTime2FormatByScale[scale2], DateTimeFormatInfo.InvariantInfo);
			}
			if (StorageType.DateTimeOffset == _type)
			{
				DateTimeOffset dateTimeOffset = DateTimeOffset;
				byte scale3 = _value._dateTimeOffsetInfo.dateTime2Info.timeInfo.scale;
				return dateTimeOffset.ToString(s_katmaiDateTimeOffsetFormatByScale[scale3], DateTimeFormatInfo.InvariantInfo);
			}
			return (string)Value;
		}
	}

	internal SqlString KatmaiDateTimeSqlString
	{
		get
		{
			if (StorageType.Date == _type || StorageType.Time == _type || StorageType.DateTime2 == _type || StorageType.DateTimeOffset == _type)
			{
				if (IsNull)
				{
					return SqlString.Null;
				}
				return new SqlString(KatmaiDateTimeString);
			}
			return (SqlString)SqlValue;
		}
	}

	internal TimeSpan Time
	{
		get
		{
			ThrowIfNull();
			if (StorageType.Time == _type)
			{
				return new TimeSpan(_value._timeInfo.ticks);
			}
			return (TimeSpan)Value;
		}
	}

	internal DateTimeOffset DateTimeOffset
	{
		get
		{
			ThrowIfNull();
			if (StorageType.DateTimeOffset == _type)
			{
				TimeSpan offset = new TimeSpan(0, _value._dateTimeOffsetInfo.offset, 0);
				return new DateTimeOffset(GetTicksFromDateTime2Info(_value._dateTimeOffsetInfo.dateTime2Info) + offset.Ticks, offset);
			}
			return (DateTimeOffset)Value;
		}
	}

	internal SqlBinary SqlBinary
	{
		get
		{
			if (StorageType.SqlBinary == _type)
			{
				return (SqlBinary)_object;
			}
			return (SqlBinary)SqlValue;
		}
		set
		{
			_object = value;
			_type = StorageType.SqlBinary;
			_isNull = value.IsNull;
		}
	}

	internal SqlBoolean SqlBoolean
	{
		get
		{
			if (StorageType.Boolean == _type)
			{
				if (IsNull)
				{
					return SqlBoolean.Null;
				}
				return new SqlBoolean(_value._boolean);
			}
			return (SqlBoolean)SqlValue;
		}
	}

	internal SqlByte SqlByte
	{
		get
		{
			if (StorageType.Byte == _type)
			{
				if (IsNull)
				{
					return SqlByte.Null;
				}
				return new SqlByte(_value._byte);
			}
			return (SqlByte)SqlValue;
		}
	}

	internal SqlCachedBuffer SqlCachedBuffer
	{
		get
		{
			if (StorageType.SqlCachedBuffer == _type)
			{
				if (IsNull)
				{
					return SqlCachedBuffer.Null;
				}
				return (SqlCachedBuffer)_object;
			}
			return (SqlCachedBuffer)SqlValue;
		}
		set
		{
			_object = value;
			_type = StorageType.SqlCachedBuffer;
			_isNull = value.IsNull;
		}
	}

	internal SqlXml SqlXml
	{
		get
		{
			if (StorageType.SqlXml == _type)
			{
				if (IsNull)
				{
					return SqlXml.Null;
				}
				return (SqlXml)_object;
			}
			return (SqlXml)SqlValue;
		}
		set
		{
			_object = value;
			_type = StorageType.SqlXml;
			_isNull = value.IsNull;
		}
	}

	internal SqlDateTime SqlDateTime
	{
		get
		{
			if (StorageType.DateTime == _type)
			{
				if (IsNull)
				{
					return SqlDateTime.Null;
				}
				return new SqlDateTime(_value._dateTimeInfo.daypart, _value._dateTimeInfo.timepart);
			}
			return (SqlDateTime)SqlValue;
		}
	}

	internal SqlDecimal SqlDecimal
	{
		get
		{
			if (StorageType.Decimal == _type)
			{
				if (IsNull)
				{
					return SqlDecimal.Null;
				}
				return new SqlDecimal(_value._numericInfo.precision, _value._numericInfo.scale, _value._numericInfo.positive, _value._numericInfo.data1, _value._numericInfo.data2, _value._numericInfo.data3, _value._numericInfo.data4);
			}
			return (SqlDecimal)SqlValue;
		}
	}

	internal SqlDouble SqlDouble
	{
		get
		{
			if (StorageType.Double == _type)
			{
				if (IsNull)
				{
					return SqlDouble.Null;
				}
				return new SqlDouble(_value._double);
			}
			return (SqlDouble)SqlValue;
		}
	}

	internal SqlGuid SqlGuid
	{
		get
		{
			if (StorageType.SqlGuid == _type)
			{
				return (SqlGuid)_object;
			}
			return (SqlGuid)SqlValue;
		}
		set
		{
			_object = value;
			_type = StorageType.SqlGuid;
			_isNull = value.IsNull;
		}
	}

	internal SqlInt16 SqlInt16
	{
		get
		{
			if (StorageType.Int16 == _type)
			{
				if (IsNull)
				{
					return SqlInt16.Null;
				}
				return new SqlInt16(_value._int16);
			}
			return (SqlInt16)SqlValue;
		}
	}

	internal SqlInt32 SqlInt32
	{
		get
		{
			if (StorageType.Int32 == _type)
			{
				if (IsNull)
				{
					return SqlInt32.Null;
				}
				return new SqlInt32(_value._int32);
			}
			return (SqlInt32)SqlValue;
		}
	}

	internal SqlInt64 SqlInt64
	{
		get
		{
			if (StorageType.Int64 == _type)
			{
				if (IsNull)
				{
					return SqlInt64.Null;
				}
				return new SqlInt64(_value._int64);
			}
			return (SqlInt64)SqlValue;
		}
	}

	internal SqlMoney SqlMoney
	{
		get
		{
			if (StorageType.Money == _type)
			{
				if (IsNull)
				{
					return SqlMoney.Null;
				}
				return SqlTypeWorkarounds.SqlMoneyCtor(_value._int64, 1);
			}
			return (SqlMoney)SqlValue;
		}
	}

	internal SqlSingle SqlSingle
	{
		get
		{
			if (StorageType.Single == _type)
			{
				if (IsNull)
				{
					return SqlSingle.Null;
				}
				return new SqlSingle(_value._single);
			}
			return (SqlSingle)SqlValue;
		}
	}

	internal SqlString SqlString
	{
		get
		{
			if (StorageType.String == _type)
			{
				if (IsNull)
				{
					return SqlString.Null;
				}
				return new SqlString((string)_object);
			}
			if (StorageType.SqlCachedBuffer == _type)
			{
				SqlCachedBuffer sqlCachedBuffer = (SqlCachedBuffer)_object;
				if (sqlCachedBuffer.IsNull)
				{
					return SqlString.Null;
				}
				return sqlCachedBuffer.ToSqlString();
			}
			return (SqlString)SqlValue;
		}
	}

	internal object SqlValue
	{
		get
		{
			switch (_type)
			{
			case StorageType.Empty:
				return DBNull.Value;
			case StorageType.Boolean:
				return SqlBoolean;
			case StorageType.Byte:
				return SqlByte;
			case StorageType.DateTime:
				return SqlDateTime;
			case StorageType.Decimal:
				return SqlDecimal;
			case StorageType.Double:
				return SqlDouble;
			case StorageType.Int16:
				return SqlInt16;
			case StorageType.Int32:
				return SqlInt32;
			case StorageType.Int64:
				return SqlInt64;
			case StorageType.Money:
				return SqlMoney;
			case StorageType.Single:
				return SqlSingle;
			case StorageType.String:
				return SqlString;
			case StorageType.SqlCachedBuffer:
			{
				SqlCachedBuffer sqlCachedBuffer = (SqlCachedBuffer)_object;
				if (sqlCachedBuffer.IsNull)
				{
					return SqlXml.Null;
				}
				return sqlCachedBuffer.ToSqlXml();
			}
			case StorageType.SqlBinary:
			case StorageType.SqlGuid:
				return _object;
			case StorageType.SqlXml:
				if (_isNull)
				{
					return SqlXml.Null;
				}
				return (SqlXml)_object;
			case StorageType.Date:
			case StorageType.DateTime2:
				if (_isNull)
				{
					return DBNull.Value;
				}
				return DateTime;
			case StorageType.DateTimeOffset:
				if (_isNull)
				{
					return DBNull.Value;
				}
				return DateTimeOffset;
			case StorageType.Time:
				if (_isNull)
				{
					return DBNull.Value;
				}
				return Time;
			default:
				return null;
			}
		}
	}

	internal object Value
	{
		get
		{
			if (IsNull)
			{
				return DBNull.Value;
			}
			return _type switch
			{
				StorageType.Empty => DBNull.Value, 
				StorageType.Boolean => Boolean, 
				StorageType.Byte => Byte, 
				StorageType.DateTime => DateTime, 
				StorageType.Decimal => Decimal, 
				StorageType.Double => Double, 
				StorageType.Int16 => Int16, 
				StorageType.Int32 => Int32, 
				StorageType.Int64 => Int64, 
				StorageType.Money => Decimal, 
				StorageType.Single => Single, 
				StorageType.String => String, 
				StorageType.SqlBinary => ByteArray, 
				StorageType.SqlCachedBuffer => ((SqlCachedBuffer)_object).ToString(), 
				StorageType.SqlGuid => Guid, 
				StorageType.SqlXml => ((SqlXml)_object).Value, 
				StorageType.Date => DateTime, 
				StorageType.DateTime2 => DateTime, 
				StorageType.DateTimeOffset => DateTimeOffset, 
				StorageType.Time => Time, 
				_ => null, 
			};
		}
	}

	internal SqlBuffer()
	{
	}

	private SqlBuffer(SqlBuffer value)
	{
		_isNull = value._isNull;
		_type = value._type;
		_value = value._value;
		_object = value._object;
	}

	private static long GetTicksFromDateTime2Info(DateTime2Info dateTime2Info)
	{
		return dateTime2Info.date * 864000000000L + dateTime2Info.timeInfo.ticks;
	}

	internal Type GetTypeFromStorageType(bool isSqlType)
	{
		if (isSqlType)
		{
			switch (_type)
			{
			case StorageType.Empty:
				return null;
			case StorageType.Boolean:
				return typeof(SqlBoolean);
			case StorageType.Byte:
				return typeof(SqlByte);
			case StorageType.DateTime:
				return typeof(SqlDateTime);
			case StorageType.Decimal:
				return typeof(SqlDecimal);
			case StorageType.Double:
				return typeof(SqlDouble);
			case StorageType.Int16:
				return typeof(SqlInt16);
			case StorageType.Int32:
				return typeof(SqlInt32);
			case StorageType.Int64:
				return typeof(SqlInt64);
			case StorageType.Money:
				return typeof(SqlMoney);
			case StorageType.Single:
				return typeof(SqlSingle);
			case StorageType.String:
				return typeof(SqlString);
			case StorageType.SqlCachedBuffer:
				return typeof(SqlString);
			case StorageType.SqlBinary:
				return typeof(object);
			case StorageType.SqlGuid:
				return typeof(object);
			case StorageType.SqlXml:
				return typeof(SqlXml);
			}
		}
		else
		{
			switch (_type)
			{
			case StorageType.Empty:
				return null;
			case StorageType.Boolean:
				return typeof(bool);
			case StorageType.Byte:
				return typeof(byte);
			case StorageType.DateTime:
				return typeof(DateTime);
			case StorageType.Decimal:
				return typeof(decimal);
			case StorageType.Double:
				return typeof(double);
			case StorageType.Int16:
				return typeof(short);
			case StorageType.Int32:
				return typeof(int);
			case StorageType.Int64:
				return typeof(long);
			case StorageType.Money:
				return typeof(decimal);
			case StorageType.Single:
				return typeof(float);
			case StorageType.String:
				return typeof(string);
			case StorageType.SqlBinary:
				return typeof(byte[]);
			case StorageType.SqlCachedBuffer:
				return typeof(string);
			case StorageType.SqlGuid:
				return typeof(Guid);
			case StorageType.SqlXml:
				return typeof(string);
			}
		}
		return null;
	}

	internal static SqlBuffer[] CreateBufferArray(int length)
	{
		SqlBuffer[] array = new SqlBuffer[length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new SqlBuffer();
		}
		return array;
	}

	internal static SqlBuffer[] CloneBufferArray(SqlBuffer[] values)
	{
		SqlBuffer[] array = new SqlBuffer[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = new SqlBuffer(values[i]);
		}
		return array;
	}

	internal static void Clear(SqlBuffer[] values)
	{
		if (values != null)
		{
			for (int i = 0; i < values.Length; i++)
			{
				values[i].Clear();
			}
		}
	}

	internal void Clear()
	{
		_isNull = false;
		_type = StorageType.Empty;
		_object = null;
	}

	internal void SetToDateTime(int daypart, int timepart)
	{
		_value._dateTimeInfo.daypart = daypart;
		_value._dateTimeInfo.timepart = timepart;
		_type = StorageType.DateTime;
		_isNull = false;
	}

	internal void SetToDecimal(byte precision, byte scale, bool positive, int[] bits)
	{
		_value._numericInfo.precision = precision;
		_value._numericInfo.scale = scale;
		_value._numericInfo.positive = positive;
		_value._numericInfo.data1 = bits[0];
		_value._numericInfo.data2 = bits[1];
		_value._numericInfo.data3 = bits[2];
		_value._numericInfo.data4 = bits[3];
		_type = StorageType.Decimal;
		_isNull = false;
	}

	internal void SetToMoney(long value)
	{
		_value._int64 = value;
		_type = StorageType.Money;
		_isNull = false;
	}

	internal void SetToNullOfType(StorageType storageType)
	{
		_type = storageType;
		_isNull = true;
		_object = null;
	}

	internal void SetToString(string value)
	{
		_object = value;
		_type = StorageType.String;
		_isNull = false;
	}

	internal void SetToDate(byte[] bytes)
	{
		_type = StorageType.Date;
		_value._int32 = GetDateFromByteArray(bytes, 0);
		_isNull = false;
	}

	internal void SetToDate(DateTime date)
	{
		_type = StorageType.Date;
		_value._int32 = date.Subtract(DateTime.MinValue).Days;
		_isNull = false;
	}

	internal void SetToTime(byte[] bytes, int length, byte scale)
	{
		_type = StorageType.Time;
		FillInTimeInfo(ref _value._timeInfo, bytes, length, scale);
		_isNull = false;
	}

	internal void SetToTime(TimeSpan timeSpan, byte scale)
	{
		_type = StorageType.Time;
		_value._timeInfo.ticks = timeSpan.Ticks;
		_value._timeInfo.scale = scale;
		_isNull = false;
	}

	internal void SetToDateTime2(byte[] bytes, int length, byte scale)
	{
		_type = StorageType.DateTime2;
		FillInTimeInfo(ref _value._dateTime2Info.timeInfo, bytes, length - 3, scale);
		_value._dateTime2Info.date = GetDateFromByteArray(bytes, length - 3);
		_isNull = false;
	}

	internal void SetToDateTime2(DateTime dateTime, byte scale)
	{
		_type = StorageType.DateTime2;
		_value._dateTime2Info.timeInfo.ticks = dateTime.TimeOfDay.Ticks;
		_value._dateTime2Info.timeInfo.scale = scale;
		_value._dateTime2Info.date = dateTime.Subtract(DateTime.MinValue).Days;
		_isNull = false;
	}

	internal void SetToDateTimeOffset(byte[] bytes, int length, byte scale)
	{
		_type = StorageType.DateTimeOffset;
		FillInTimeInfo(ref _value._dateTimeOffsetInfo.dateTime2Info.timeInfo, bytes, length - 5, scale);
		_value._dateTimeOffsetInfo.dateTime2Info.date = GetDateFromByteArray(bytes, length - 5);
		_value._dateTimeOffsetInfo.offset = (short)(bytes[length - 2] + (bytes[length - 1] << 8));
		_isNull = false;
	}

	internal void SetToDateTimeOffset(DateTimeOffset dateTimeOffset, byte scale)
	{
		_type = StorageType.DateTimeOffset;
		DateTime utcDateTime = dateTimeOffset.UtcDateTime;
		_value._dateTimeOffsetInfo.dateTime2Info.timeInfo.ticks = utcDateTime.TimeOfDay.Ticks;
		_value._dateTimeOffsetInfo.dateTime2Info.timeInfo.scale = scale;
		_value._dateTimeOffsetInfo.dateTime2Info.date = utcDateTime.Subtract(DateTime.MinValue).Days;
		_value._dateTimeOffsetInfo.offset = (short)dateTimeOffset.Offset.TotalMinutes;
		_isNull = false;
	}

	private static void FillInTimeInfo(ref TimeInfo timeInfo, byte[] timeBytes, int length, byte scale)
	{
		long num = (long)(timeBytes[0] + ((ulong)timeBytes[1] << 8) + ((ulong)timeBytes[2] << 16));
		if (length > 3)
		{
			num += (long)((ulong)timeBytes[3] << 24);
		}
		if (length > 4)
		{
			num += (long)((ulong)timeBytes[4] << 32);
		}
		timeInfo.ticks = num * TdsEnums.TICKS_FROM_SCALE[scale];
		timeInfo.scale = scale;
	}

	private static int GetDateFromByteArray(byte[] buf, int offset)
	{
		return buf[offset] + (buf[offset + 1] << 8) + (buf[offset + 2] << 16);
	}

	private void ThrowIfNull()
	{
		if (IsNull)
		{
			throw new SqlNullValueException();
		}
	}
}
