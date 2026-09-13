using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient;

internal class TdsValueSetter
{
	private TdsParserStateObject _stateObj;

	private SmiMetaData _metaData;

	private bool _isPlp;

	private bool _plpUnknownSent;

	private Encoder _encoder;

	private SmiMetaData _variantType;

	internal TdsValueSetter(TdsParserStateObject stateObj, SmiMetaData md)
	{
		_stateObj = stateObj;
		_metaData = md;
		_isPlp = MetaDataUtilsSmi.IsPlpFormat(md);
		_plpUnknownSent = false;
		_encoder = null;
	}

	internal void SetDBNull()
	{
		if (_isPlp)
		{
			_stateObj.Parser.WriteUnsignedLong(ulong.MaxValue, _stateObj);
			return;
		}
		switch (_metaData.SqlDbType)
		{
		case SqlDbType.BigInt:
		case SqlDbType.Bit:
		case SqlDbType.DateTime:
		case SqlDbType.Decimal:
		case SqlDbType.Float:
		case SqlDbType.Int:
		case SqlDbType.Money:
		case SqlDbType.Real:
		case SqlDbType.UniqueIdentifier:
		case SqlDbType.SmallDateTime:
		case SqlDbType.SmallInt:
		case SqlDbType.SmallMoney:
		case SqlDbType.TinyInt:
		case SqlDbType.Date:
		case SqlDbType.Time:
		case SqlDbType.DateTime2:
		case SqlDbType.DateTimeOffset:
			_stateObj.WriteByte(0);
			break;
		case SqlDbType.Binary:
		case SqlDbType.Char:
		case SqlDbType.Image:
		case SqlDbType.NChar:
		case SqlDbType.NText:
		case SqlDbType.NVarChar:
		case SqlDbType.Text:
		case SqlDbType.Timestamp:
		case SqlDbType.VarBinary:
		case SqlDbType.VarChar:
			_stateObj.Parser.WriteShort(65535, _stateObj);
			break;
		case SqlDbType.Variant:
			_stateObj.Parser.WriteInt(0, _stateObj);
			break;
		case (SqlDbType)24:
		case SqlDbType.Xml:
		case (SqlDbType)26:
		case (SqlDbType)27:
		case (SqlDbType)28:
		case SqlDbType.Udt:
		case SqlDbType.Structured:
			break;
		}
	}

	internal void SetBoolean(bool value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(3, 50, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		if (value)
		{
			_stateObj.WriteByte(1);
		}
		else
		{
			_stateObj.WriteByte(0);
		}
	}

	internal void SetByte(byte value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(3, 48, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		_stateObj.WriteByte(value);
	}

	internal int SetBytes(long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		SetBytesNoOffsetHandling(fieldOffset, buffer, bufferOffset, length);
		return length;
	}

	private void SetBytesNoOffsetHandling(long fieldOffset, byte[] buffer, int bufferOffset, int length)
	{
		if (_isPlp)
		{
			if (!_plpUnknownSent)
			{
				_stateObj.Parser.WriteUnsignedLong(18446744073709551614uL, _stateObj);
				_plpUnknownSent = true;
			}
			_stateObj.Parser.WriteInt(length, _stateObj);
			_stateObj.WriteByteArray(buffer, length, bufferOffset);
		}
		else
		{
			if (SqlDbType.Variant == _metaData.SqlDbType)
			{
				_stateObj.Parser.WriteSqlVariantHeader(4 + length, 165, 2, _stateObj);
			}
			_stateObj.Parser.WriteShort(length, _stateObj);
			_stateObj.WriteByteArray(buffer, length, bufferOffset);
		}
	}

	internal void SetBytesLength(long length)
	{
		if (length == 0L)
		{
			if (_isPlp)
			{
				_stateObj.Parser.WriteLong(0L, _stateObj);
				_plpUnknownSent = true;
			}
			else
			{
				if (SqlDbType.Variant == _metaData.SqlDbType)
				{
					_stateObj.Parser.WriteSqlVariantHeader(4, 165, 2, _stateObj);
				}
				_stateObj.Parser.WriteShort(0, _stateObj);
			}
		}
		if (_plpUnknownSent)
		{
			_stateObj.Parser.WriteInt(0, _stateObj);
			_plpUnknownSent = false;
		}
	}

	internal int SetChars(long fieldOffset, char[] buffer, int bufferOffset, int length)
	{
		if (MetaDataUtilsSmi.IsAnsiType(_metaData.SqlDbType))
		{
			if (_encoder == null)
			{
				_encoder = _stateObj.Parser._defaultEncoding.GetEncoder();
			}
			byte[] array = new byte[_encoder.GetByteCount(buffer, bufferOffset, length, flush: false)];
			_encoder.GetBytes(buffer, bufferOffset, length, array, 0, flush: false);
			SetBytesNoOffsetHandling(fieldOffset, array, 0, array.Length);
		}
		else if (_isPlp)
		{
			if (!_plpUnknownSent)
			{
				_stateObj.Parser.WriteUnsignedLong(18446744073709551614uL, _stateObj);
				_plpUnknownSent = true;
			}
			_stateObj.Parser.WriteInt(length * 2, _stateObj);
			_stateObj.Parser.WriteCharArray(buffer, length, bufferOffset, _stateObj);
		}
		else if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantValue(new string(buffer, bufferOffset, length), length, 0, _stateObj);
		}
		else
		{
			_stateObj.Parser.WriteShort(length * 2, _stateObj);
			_stateObj.Parser.WriteCharArray(buffer, length, bufferOffset, _stateObj);
		}
		return length;
	}

	internal void SetCharsLength(long length)
	{
		if (length == 0L)
		{
			if (_isPlp)
			{
				_stateObj.Parser.WriteLong(0L, _stateObj);
				_plpUnknownSent = true;
			}
			else
			{
				_stateObj.Parser.WriteShort(0, _stateObj);
			}
		}
		if (_plpUnknownSent)
		{
			_stateObj.Parser.WriteInt(0, _stateObj);
			_plpUnknownSent = false;
		}
		_encoder = null;
	}

	internal void SetString(string value, int offset, int length)
	{
		if (MetaDataUtilsSmi.IsAnsiType(_metaData.SqlDbType))
		{
			byte[] bytes;
			if (offset == 0 && value.Length <= length)
			{
				bytes = _stateObj.Parser._defaultEncoding.GetBytes(value);
			}
			else
			{
				char[] chars = value.ToCharArray(offset, length);
				bytes = _stateObj.Parser._defaultEncoding.GetBytes(chars);
			}
			SetBytes(0L, bytes, 0, bytes.Length);
			SetBytesLength(bytes.Length);
		}
		else if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			SqlCollation sqlCollation = new SqlCollation();
			sqlCollation.LCID = checked((int)_variantType.LocaleId);
			sqlCollation.SqlCompareOptions = _variantType.CompareOptions;
			if (length * 2 > 8000)
			{
				byte[] array = ((offset != 0 || value.Length > length) ? _stateObj.Parser._defaultEncoding.GetBytes(value.ToCharArray(offset, length)) : _stateObj.Parser._defaultEncoding.GetBytes(value));
				_stateObj.Parser.WriteSqlVariantHeader(9 + array.Length, 167, 7, _stateObj);
				_stateObj.Parser.WriteUnsignedInt(sqlCollation.info, _stateObj);
				_stateObj.WriteByte(sqlCollation.sortId);
				_stateObj.Parser.WriteShort(array.Length, _stateObj);
				_stateObj.WriteByteArray(array, array.Length, 0);
			}
			else
			{
				_stateObj.Parser.WriteSqlVariantHeader(9 + length * 2, 231, 7, _stateObj);
				_stateObj.Parser.WriteUnsignedInt(sqlCollation.info, _stateObj);
				_stateObj.WriteByte(sqlCollation.sortId);
				_stateObj.Parser.WriteShort(length * 2, _stateObj);
				_stateObj.Parser.WriteString(value, length, offset, _stateObj);
			}
			_variantType = null;
		}
		else if (_isPlp)
		{
			_stateObj.Parser.WriteLong(length * 2, _stateObj);
			_stateObj.Parser.WriteInt(length * 2, _stateObj);
			_stateObj.Parser.WriteString(value, length, offset, _stateObj);
			if (length != 0)
			{
				_stateObj.Parser.WriteInt(0, _stateObj);
			}
		}
		else
		{
			_stateObj.Parser.WriteShort(length * 2, _stateObj);
			_stateObj.Parser.WriteString(value, length, offset, _stateObj);
		}
	}

	internal void SetInt16(short value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(4, 52, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		_stateObj.Parser.WriteShort(value, _stateObj);
	}

	internal void SetInt32(int value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(6, 56, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		_stateObj.Parser.WriteInt(value, _stateObj);
	}

	internal void SetInt64(long value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			if (_variantType == null)
			{
				_stateObj.Parser.WriteSqlVariantHeader(10, 127, 0, _stateObj);
				_stateObj.Parser.WriteLong(value, _stateObj);
				return;
			}
			_stateObj.Parser.WriteSqlVariantHeader(10, 60, 0, _stateObj);
			_stateObj.Parser.WriteInt((int)(value >> 32), _stateObj);
			_stateObj.Parser.WriteInt((int)value, _stateObj);
			_variantType = null;
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
			if (SqlDbType.SmallMoney == _metaData.SqlDbType)
			{
				_stateObj.Parser.WriteInt((int)value, _stateObj);
			}
			else if (SqlDbType.Money == _metaData.SqlDbType)
			{
				_stateObj.Parser.WriteInt((int)(value >> 32), _stateObj);
				_stateObj.Parser.WriteInt((int)value, _stateObj);
			}
			else
			{
				_stateObj.Parser.WriteLong(value, _stateObj);
			}
		}
	}

	internal void SetSingle(float value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(6, 59, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		_stateObj.Parser.WriteFloat(value, _stateObj);
	}

	internal void SetDouble(double value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(10, 62, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		_stateObj.Parser.WriteDouble(value, _stateObj);
	}

	internal void SetSqlDecimal(SqlDecimal value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(21, 108, 2, _stateObj);
			_stateObj.WriteByte(value.Precision);
			_stateObj.WriteByte(value.Scale);
			_stateObj.Parser.WriteSqlDecimal(value, _stateObj);
		}
		else
		{
			_stateObj.WriteByte(checked((byte)MetaType.MetaDecimal.FixedLength));
			_stateObj.Parser.WriteSqlDecimal(SqlDecimal.ConvertToPrecScale(value, _metaData.Precision, _metaData.Scale), _stateObj);
		}
	}

	internal void SetDateTime(DateTime value)
	{
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			if (_variantType != null && _variantType.SqlDbType == SqlDbType.DateTime2)
			{
				_stateObj.Parser.WriteSqlVariantDateTime2(value, _stateObj);
			}
			else if (_variantType != null && _variantType.SqlDbType == SqlDbType.Date)
			{
				_stateObj.Parser.WriteSqlVariantDate(value, _stateObj);
			}
			else
			{
				TdsDateTime tdsDateTime = MetaType.FromDateTime(value, 8);
				_stateObj.Parser.WriteSqlVariantHeader(10, 61, 0, _stateObj);
				_stateObj.Parser.WriteInt(tdsDateTime.days, _stateObj);
				_stateObj.Parser.WriteInt(tdsDateTime.time, _stateObj);
			}
			_variantType = null;
			return;
		}
		_stateObj.WriteByte((byte)_metaData.MaxLength);
		if (SqlDbType.SmallDateTime == _metaData.SqlDbType)
		{
			TdsDateTime tdsDateTime2 = MetaType.FromDateTime(value, (byte)_metaData.MaxLength);
			_stateObj.Parser.WriteShort(tdsDateTime2.days, _stateObj);
			_stateObj.Parser.WriteShort(tdsDateTime2.time, _stateObj);
			return;
		}
		if (SqlDbType.DateTime == _metaData.SqlDbType)
		{
			TdsDateTime tdsDateTime3 = MetaType.FromDateTime(value, (byte)_metaData.MaxLength);
			_stateObj.Parser.WriteInt(tdsDateTime3.days, _stateObj);
			_stateObj.Parser.WriteInt(tdsDateTime3.time, _stateObj);
			return;
		}
		int days = value.Subtract(DateTime.MinValue).Days;
		if (SqlDbType.DateTime2 == _metaData.SqlDbType)
		{
			long value2 = value.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[_metaData.Scale];
			_stateObj.WriteByteArray(BitConverter.GetBytes(value2), (int)_metaData.MaxLength - 3, 0);
		}
		_stateObj.WriteByteArray(BitConverter.GetBytes(days), 3, 0);
	}

	internal void SetGuid(Guid value)
	{
		byte[] array = value.ToByteArray();
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			_stateObj.Parser.WriteSqlVariantHeader(18, 36, 0, _stateObj);
		}
		else
		{
			_stateObj.WriteByte((byte)_metaData.MaxLength);
		}
		_stateObj.WriteByteArray(array, array.Length, 0);
	}

	internal void SetTimeSpan(TimeSpan value)
	{
		byte scale;
		byte b;
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			scale = SmiMetaData.DefaultTime.Scale;
			b = (byte)SmiMetaData.DefaultTime.MaxLength;
			_stateObj.Parser.WriteSqlVariantHeader(8, 41, 1, _stateObj);
			_stateObj.WriteByte(scale);
		}
		else
		{
			scale = _metaData.Scale;
			b = (byte)_metaData.MaxLength;
			_stateObj.WriteByte(b);
		}
		long value2 = value.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		_stateObj.WriteByteArray(BitConverter.GetBytes(value2), b, 0);
	}

	internal void SetDateTimeOffset(DateTimeOffset value)
	{
		byte scale;
		byte b;
		if (SqlDbType.Variant == _metaData.SqlDbType)
		{
			SmiMetaData defaultDateTimeOffset = SmiMetaData.DefaultDateTimeOffset;
			scale = MetaType.MetaDateTimeOffset.Scale;
			b = (byte)defaultDateTimeOffset.MaxLength;
			_stateObj.Parser.WriteSqlVariantHeader(13, 43, 1, _stateObj);
			_stateObj.WriteByte(scale);
		}
		else
		{
			scale = _metaData.Scale;
			b = (byte)_metaData.MaxLength;
			_stateObj.WriteByte(b);
		}
		DateTime utcDateTime = value.UtcDateTime;
		long value2 = utcDateTime.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		int days = utcDateTime.Subtract(DateTime.MinValue).Days;
		short num = (short)value.Offset.TotalMinutes;
		_stateObj.WriteByteArray(BitConverter.GetBytes(value2), b - 5, 0);
		_stateObj.WriteByteArray(BitConverter.GetBytes(days), 3, 0);
		_stateObj.WriteByte((byte)(num & 0xFF));
		_stateObj.WriteByte((byte)((num >> 8) & 0xFF));
	}

	internal void SetVariantType(SmiMetaData value)
	{
		_variantType = value;
	}

	[Conditional("DEBUG")]
	private void CheckSettingOffset(long offset)
	{
	}
}
