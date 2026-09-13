using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.SqlServer.Server;

internal sealed class SqlRecordBuffer
{
	internal enum StorageType
	{
		Boolean,
		Byte,
		ByteArray,
		CharArray,
		DateTime,
		DateTimeOffset,
		Double,
		Guid,
		Int16,
		Int32,
		Int64,
		Single,
		String,
		SqlDecimal,
		TimeSpan
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct Storage
	{
		[FieldOffset(0)]
		internal bool _boolean;

		[FieldOffset(0)]
		internal byte _byte;

		[FieldOffset(0)]
		internal DateTime _dateTime;

		[FieldOffset(0)]
		internal DateTimeOffset _dateTimeOffset;

		[FieldOffset(0)]
		internal double _double;

		[FieldOffset(0)]
		internal Guid _guid;

		[FieldOffset(0)]
		internal short _int16;

		[FieldOffset(0)]
		internal int _int32;

		[FieldOffset(0)]
		internal long _int64;

		[FieldOffset(0)]
		internal float _single;

		[FieldOffset(0)]
		internal TimeSpan _timeSpan;
	}

	private bool _isNull;

	private StorageType _type;

	private Storage _value;

	private object _object;

	private SmiMetaData _metadata;

	private bool _isMetaSet;

	internal bool IsNull => _isNull;

	internal bool Boolean
	{
		get
		{
			return _value._boolean;
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
			return _value._byte;
		}
		set
		{
			_value._byte = value;
			_type = StorageType.Byte;
			_isNull = false;
		}
	}

	internal DateTime DateTime
	{
		get
		{
			return _value._dateTime;
		}
		set
		{
			_value._dateTime = value;
			_type = StorageType.DateTime;
			_isNull = false;
			if (_isMetaSet)
			{
				_isMetaSet = false;
			}
			else
			{
				_metadata = null;
			}
		}
	}

	internal DateTimeOffset DateTimeOffset
	{
		get
		{
			return _value._dateTimeOffset;
		}
		set
		{
			_value._dateTimeOffset = value;
			_type = StorageType.DateTimeOffset;
			_isNull = false;
		}
	}

	internal double Double
	{
		get
		{
			return _value._double;
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
			return _value._guid;
		}
		set
		{
			_value._guid = value;
			_type = StorageType.Guid;
			_isNull = false;
		}
	}

	internal short Int16
	{
		get
		{
			return _value._int16;
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
			return _value._int32;
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
			return _value._int64;
		}
		set
		{
			_value._int64 = value;
			_type = StorageType.Int64;
			_isNull = false;
			if (_isMetaSet)
			{
				_isMetaSet = false;
			}
			else
			{
				_metadata = null;
			}
		}
	}

	internal float Single
	{
		get
		{
			return _value._single;
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
			if (StorageType.String == _type)
			{
				return (string)_object;
			}
			if (StorageType.CharArray == _type)
			{
				return new string((char[])_object, 0, (int)CharsLength);
			}
			return new SqlXml(new MemoryStream((byte[])_object, writable: false)).Value;
		}
		set
		{
			_object = value;
			_value._int64 = value.Length;
			_type = StorageType.String;
			_isNull = false;
			if (_isMetaSet)
			{
				_isMetaSet = false;
			}
			else
			{
				_metadata = null;
			}
		}
	}

	internal SqlDecimal SqlDecimal
	{
		get
		{
			return (SqlDecimal)_object;
		}
		set
		{
			_object = value;
			_type = StorageType.SqlDecimal;
			_isNull = false;
		}
	}

	internal TimeSpan TimeSpan
	{
		get
		{
			return _value._timeSpan;
		}
		set
		{
			_value._timeSpan = value;
			_type = StorageType.TimeSpan;
			_isNull = false;
		}
	}

	internal long BytesLength
	{
		get
		{
			if (StorageType.String == _type)
			{
				ConvertXmlStringToByteArray();
			}
			return _value._int64;
		}
		set
		{
			if (value == 0L)
			{
				_value._int64 = value;
				_object = Array.Empty<byte>();
				_type = StorageType.ByteArray;
				_isNull = false;
			}
			else
			{
				_value._int64 = value;
			}
		}
	}

	internal long CharsLength
	{
		get
		{
			return _value._int64;
		}
		set
		{
			if (value == 0L)
			{
				_value._int64 = value;
				_object = Array.Empty<char>();
				_type = StorageType.CharArray;
				_isNull = false;
			}
			else
			{
				_value._int64 = value;
			}
		}
	}

	internal SmiMetaData VariantType
	{
		get
		{
			return _type switch
			{
				StorageType.Boolean => SmiMetaData.DefaultBit, 
				StorageType.Byte => SmiMetaData.DefaultTinyInt, 
				StorageType.ByteArray => SmiMetaData.DefaultVarBinary, 
				StorageType.CharArray => SmiMetaData.DefaultNVarChar, 
				StorageType.DateTime => _metadata ?? SmiMetaData.DefaultDateTime, 
				StorageType.DateTimeOffset => SmiMetaData.DefaultDateTimeOffset, 
				StorageType.Double => SmiMetaData.DefaultFloat, 
				StorageType.Guid => SmiMetaData.DefaultUniqueIdentifier, 
				StorageType.Int16 => SmiMetaData.DefaultSmallInt, 
				StorageType.Int32 => SmiMetaData.DefaultInt, 
				StorageType.Int64 => _metadata ?? SmiMetaData.DefaultBigInt, 
				StorageType.Single => SmiMetaData.DefaultReal, 
				StorageType.String => _metadata ?? SmiMetaData.DefaultNVarChar, 
				StorageType.SqlDecimal => new SmiMetaData(SqlDbType.Decimal, 17L, ((SqlDecimal)_object).Precision, ((SqlDecimal)_object).Scale, 0L, SqlCompareOptions.None, null), 
				StorageType.TimeSpan => SmiMetaData.DefaultTime, 
				_ => null, 
			};
		}
		set
		{
			_metadata = value;
			_isMetaSet = true;
		}
	}

	internal SqlRecordBuffer(SmiMetaData metaData)
	{
		_isNull = true;
	}

	internal int GetBytes(long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		int srcOffset = (int)fieldOffset;
		if (StorageType.String == _type)
		{
			ConvertXmlStringToByteArray();
		}
		Buffer.BlockCopy((byte[])_object, srcOffset, buffer, bufferOffset, length);
		return length;
	}

	internal int GetChars(long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		int sourceIndex = (int)fieldOffset;
		if (StorageType.CharArray == _type)
		{
			Array.Copy((char[])_object, sourceIndex, buffer, bufferOffset, length);
		}
		else
		{
			((string)_object).CopyTo(sourceIndex, buffer, bufferOffset, length);
		}
		return length;
	}

	internal int SetBytes(long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		int num = (int)fieldOffset;
		if (IsNull || StorageType.ByteArray != _type)
		{
			if (num != 0)
			{
				throw ADP.ArgumentOutOfRange("fieldOffset");
			}
			_object = new byte[length];
			_type = StorageType.ByteArray;
			_isNull = false;
			BytesLength = length;
		}
		else
		{
			if (num > BytesLength)
			{
				throw ADP.ArgumentOutOfRange("fieldOffset");
			}
			if (num + length > BytesLength)
			{
				int num2 = ((byte[])_object).Length;
				if (num + length > num2)
				{
					byte[] array = new byte[Math.Max(num + length, 2 * num2)];
					Buffer.BlockCopy((byte[])_object, 0, array, 0, (int)BytesLength);
					_object = array;
				}
				BytesLength = num + length;
			}
		}
		Buffer.BlockCopy(buffer, bufferOffset, (byte[])_object, num, length);
		return length;
	}

	internal int SetChars(long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		int num = (int)fieldOffset;
		if (IsNull || (StorageType.CharArray != _type && StorageType.String != _type))
		{
			if (num != 0)
			{
				throw ADP.ArgumentOutOfRange("fieldOffset");
			}
			_object = new char[length];
			_type = StorageType.CharArray;
			_isNull = false;
			CharsLength = length;
		}
		else
		{
			if (num > CharsLength)
			{
				throw ADP.ArgumentOutOfRange("fieldOffset");
			}
			if (StorageType.String == _type)
			{
				_object = ((string)_object).ToCharArray();
				_type = StorageType.CharArray;
			}
			if (num + length > CharsLength)
			{
				int num2 = ((char[])_object).Length;
				if (num + length > num2)
				{
					char[] array = new char[Math.Max(num + length, 2 * num2)];
					Array.Copy((char[])_object, 0, array, 0, (int)CharsLength);
					_object = array;
				}
				CharsLength = num + length;
			}
		}
		Array.Copy(buffer, bufferOffset, (char[])_object, num, length);
		return length;
	}

	internal void SetNull()
	{
		_isNull = true;
	}

	private void ConvertXmlStringToByteArray()
	{
		string text = (string)_object;
		byte[] array = new byte[2 + Encoding.Unicode.GetByteCount(text)];
		array[0] = byte.MaxValue;
		array[1] = 254;
		Encoding.Unicode.GetBytes(text, 0, text.Length, array, 2);
		_object = array;
		_value._int64 = array.Length;
		_type = StorageType.ByteArray;
	}
}
