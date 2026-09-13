using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class ValueHandle
{
	private XmlBufferReader bufferReader;

	private ValueHandleType type;

	private int offset;

	private int length;

	private static Base64Encoding base64Encoding;

	private static string[] constStrings = new string[6] { "string", "number", "array", "object", "boolean", "null" };

	private static Base64Encoding Base64Encoding
	{
		get
		{
			if (base64Encoding == null)
			{
				base64Encoding = new Base64Encoding();
			}
			return base64Encoding;
		}
	}

	public ValueHandle(XmlBufferReader bufferReader)
	{
		this.bufferReader = bufferReader;
		type = ValueHandleType.Empty;
	}

	public void SetConstantValue(ValueHandleConstStringType constStringType)
	{
		type = ValueHandleType.ConstString;
		offset = (int)constStringType;
	}

	public void SetValue(ValueHandleType type)
	{
		this.type = type;
	}

	public void SetDictionaryValue(int key)
	{
		SetValue(ValueHandleType.Dictionary, key, 0);
	}

	public void SetCharValue(int ch)
	{
		SetValue(ValueHandleType.Char, ch, 0);
	}

	public void SetQNameValue(int prefix, int key)
	{
		SetValue(ValueHandleType.QName, key, prefix);
	}

	public void SetValue(ValueHandleType type, int offset, int length)
	{
		this.type = type;
		this.offset = offset;
		this.length = length;
	}

	public bool IsWhitespace()
	{
		switch (type)
		{
		case ValueHandleType.UTF8:
			return bufferReader.IsWhitespaceUTF8(offset, length);
		case ValueHandleType.Dictionary:
			return bufferReader.IsWhitespaceKey(offset);
		case ValueHandleType.Char:
		{
			int num = GetChar();
			if (num > 65535)
			{
				return false;
			}
			return XmlConverter.IsWhitespace((char)num);
		}
		case ValueHandleType.EscapedUTF8:
			return bufferReader.IsWhitespaceUTF8(offset, length);
		case ValueHandleType.Unicode:
			return bufferReader.IsWhitespaceUnicode(offset, length);
		case ValueHandleType.True:
		case ValueHandleType.False:
		case ValueHandleType.Zero:
		case ValueHandleType.One:
			return false;
		case ValueHandleType.ConstString:
			return constStrings[offset].Length == 0;
		default:
			return length == 0;
		}
	}

	public Type ToType()
	{
		switch (type)
		{
		case ValueHandleType.True:
		case ValueHandleType.False:
			return typeof(bool);
		case ValueHandleType.Zero:
		case ValueHandleType.One:
		case ValueHandleType.Int8:
		case ValueHandleType.Int16:
		case ValueHandleType.Int32:
			return typeof(int);
		case ValueHandleType.Int64:
			return typeof(long);
		case ValueHandleType.UInt64:
			return typeof(ulong);
		case ValueHandleType.Single:
			return typeof(float);
		case ValueHandleType.Double:
			return typeof(double);
		case ValueHandleType.Decimal:
			return typeof(decimal);
		case ValueHandleType.DateTime:
			return typeof(DateTime);
		case ValueHandleType.Empty:
		case ValueHandleType.UTF8:
		case ValueHandleType.EscapedUTF8:
		case ValueHandleType.Dictionary:
		case ValueHandleType.Char:
		case ValueHandleType.Unicode:
		case ValueHandleType.QName:
		case ValueHandleType.ConstString:
			return typeof(string);
		case ValueHandleType.Base64:
			return typeof(byte[]);
		case ValueHandleType.List:
			return typeof(object[]);
		case ValueHandleType.UniqueId:
			return typeof(UniqueId);
		case ValueHandleType.Guid:
			return typeof(Guid);
		case ValueHandleType.TimeSpan:
			return typeof(TimeSpan);
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}
	}

	public bool ToBoolean()
	{
		switch (type)
		{
		case ValueHandleType.False:
			return false;
		case ValueHandleType.True:
			return true;
		case ValueHandleType.UTF8:
			return XmlConverter.ToBoolean(bufferReader.Buffer, offset, length);
		case ValueHandleType.Int8:
			switch (GetInt8())
			{
			case 0:
				return false;
			case 1:
				return true;
			}
			break;
		}
		return XmlConverter.ToBoolean(GetString());
	}

	public int ToInt()
	{
		ValueHandleType valueHandleType = type;
		switch (valueHandleType)
		{
		case ValueHandleType.Zero:
			return 0;
		case ValueHandleType.One:
			return 1;
		case ValueHandleType.Int8:
			return GetInt8();
		case ValueHandleType.Int16:
			return GetInt16();
		case ValueHandleType.Int32:
			return GetInt32();
		case ValueHandleType.Int64:
		{
			long @int = GetInt64();
			if (@int >= int.MinValue && @int <= int.MaxValue)
			{
				return (int)@int;
			}
			break;
		}
		}
		if (valueHandleType == ValueHandleType.UInt64)
		{
			ulong uInt = GetUInt64();
			if (uInt <= int.MaxValue)
			{
				return (int)uInt;
			}
		}
		if (valueHandleType == ValueHandleType.UTF8)
		{
			return XmlConverter.ToInt32(bufferReader.Buffer, offset, length);
		}
		return XmlConverter.ToInt32(GetString());
	}

	public long ToLong()
	{
		ValueHandleType valueHandleType = type;
		switch (valueHandleType)
		{
		case ValueHandleType.Zero:
			return 0L;
		case ValueHandleType.One:
			return 1L;
		case ValueHandleType.Int8:
			return GetInt8();
		case ValueHandleType.Int16:
			return GetInt16();
		case ValueHandleType.Int32:
			return GetInt32();
		case ValueHandleType.Int64:
			return GetInt64();
		case ValueHandleType.UInt64:
		{
			ulong uInt = GetUInt64();
			if (uInt <= long.MaxValue)
			{
				return (long)uInt;
			}
			break;
		}
		}
		if (valueHandleType == ValueHandleType.UTF8)
		{
			return XmlConverter.ToInt64(bufferReader.Buffer, offset, length);
		}
		return XmlConverter.ToInt64(GetString());
	}

	public ulong ToULong()
	{
		ValueHandleType valueHandleType = type;
		switch (valueHandleType)
		{
		case ValueHandleType.Zero:
			return 0uL;
		case ValueHandleType.One:
			return 1uL;
		case ValueHandleType.Int8:
		case ValueHandleType.Int16:
		case ValueHandleType.Int32:
		case ValueHandleType.Int64:
		{
			long num = ToLong();
			if (num >= 0)
			{
				return (ulong)num;
			}
			break;
		}
		}
		return valueHandleType switch
		{
			ValueHandleType.UInt64 => GetUInt64(), 
			ValueHandleType.UTF8 => XmlConverter.ToUInt64(bufferReader.Buffer, offset, length), 
			_ => XmlConverter.ToUInt64(GetString()), 
		};
	}

	public float ToSingle()
	{
		ValueHandleType valueHandleType = type;
		switch (valueHandleType)
		{
		case ValueHandleType.Single:
			return GetSingle();
		case ValueHandleType.Double:
		{
			double num = GetDouble();
			if ((num >= -3.4028234663852886E+38 && num <= 3.4028234663852886E+38) || double.IsInfinity(num) || double.IsNaN(num))
			{
				return (float)num;
			}
			break;
		}
		}
		return valueHandleType switch
		{
			ValueHandleType.Zero => 0f, 
			ValueHandleType.One => 1f, 
			ValueHandleType.Int8 => GetInt8(), 
			ValueHandleType.Int16 => GetInt16(), 
			ValueHandleType.UTF8 => XmlConverter.ToSingle(bufferReader.Buffer, offset, length), 
			_ => XmlConverter.ToSingle(GetString()), 
		};
	}

	public double ToDouble()
	{
		return type switch
		{
			ValueHandleType.Double => GetDouble(), 
			ValueHandleType.Single => GetSingle(), 
			ValueHandleType.Zero => 0.0, 
			ValueHandleType.One => 1.0, 
			ValueHandleType.Int8 => GetInt8(), 
			ValueHandleType.Int16 => GetInt16(), 
			ValueHandleType.Int32 => GetInt32(), 
			ValueHandleType.UTF8 => XmlConverter.ToDouble(bufferReader.Buffer, offset, length), 
			_ => XmlConverter.ToDouble(GetString()), 
		};
	}

	public decimal ToDecimal()
	{
		ValueHandleType valueHandleType = type;
		switch (valueHandleType)
		{
		case ValueHandleType.Decimal:
			return GetDecimal();
		case ValueHandleType.Zero:
			return 0m;
		case ValueHandleType.One:
			return 1m;
		case ValueHandleType.Int8:
		case ValueHandleType.Int16:
		case ValueHandleType.Int32:
		case ValueHandleType.Int64:
			return ToLong();
		default:
			return valueHandleType switch
			{
				ValueHandleType.UInt64 => GetUInt64(), 
				ValueHandleType.UTF8 => XmlConverter.ToDecimal(bufferReader.Buffer, offset, length), 
				_ => XmlConverter.ToDecimal(GetString()), 
			};
		}
	}

	public DateTime ToDateTime()
	{
		if (type == ValueHandleType.DateTime)
		{
			return XmlConverter.ToDateTime(GetInt64());
		}
		if (type == ValueHandleType.UTF8)
		{
			return XmlConverter.ToDateTime(bufferReader.Buffer, offset, length);
		}
		return XmlConverter.ToDateTime(GetString());
	}

	public UniqueId ToUniqueId()
	{
		if (type == ValueHandleType.UniqueId)
		{
			return GetUniqueId();
		}
		if (type == ValueHandleType.UTF8)
		{
			return XmlConverter.ToUniqueId(bufferReader.Buffer, offset, length);
		}
		return XmlConverter.ToUniqueId(GetString());
	}

	public TimeSpan ToTimeSpan()
	{
		if (type == ValueHandleType.TimeSpan)
		{
			return new TimeSpan(GetInt64());
		}
		if (type == ValueHandleType.UTF8)
		{
			return XmlConverter.ToTimeSpan(bufferReader.Buffer, offset, length);
		}
		return XmlConverter.ToTimeSpan(GetString());
	}

	public Guid ToGuid()
	{
		if (type == ValueHandleType.Guid)
		{
			return GetGuid();
		}
		if (type == ValueHandleType.UTF8)
		{
			return XmlConverter.ToGuid(bufferReader.Buffer, offset, length);
		}
		return XmlConverter.ToGuid(GetString());
	}

	public override string ToString()
	{
		return GetString();
	}

	public byte[] ToByteArray()
	{
		if (type == ValueHandleType.Base64)
		{
			byte[] array = new byte[length];
			GetBase64(array, 0, length);
			return array;
		}
		if (type == ValueHandleType.UTF8 && length % 4 == 0)
		{
			try
			{
				int num = length / 4 * 3;
				if (length > 0 && bufferReader.Buffer[offset + length - 1] == 61)
				{
					num--;
					if (bufferReader.Buffer[offset + length - 2] == 61)
					{
						num--;
					}
				}
				byte[] array2 = new byte[num];
				int bytes = Base64Encoding.GetBytes(bufferReader.Buffer, offset, length, array2, 0);
				if (bytes != array2.Length)
				{
					byte[] array3 = new byte[bytes];
					Buffer.BlockCopy(array2, 0, array3, 0, bytes);
					array2 = array3;
				}
				return array2;
			}
			catch (FormatException)
			{
			}
		}
		try
		{
			return Base64Encoding.GetBytes(XmlConverter.StripWhitespace(GetString()));
		}
		catch (FormatException ex2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(ex2.Message, ex2.InnerException));
		}
	}

	public string GetString()
	{
		ValueHandleType valueHandleType = type;
		if (valueHandleType == ValueHandleType.UTF8)
		{
			return GetCharsText();
		}
		switch (valueHandleType)
		{
		case ValueHandleType.False:
			return "false";
		case ValueHandleType.True:
			return "true";
		case ValueHandleType.Zero:
			return "0";
		case ValueHandleType.One:
			return "1";
		case ValueHandleType.Int8:
		case ValueHandleType.Int16:
		case ValueHandleType.Int32:
			return XmlConverter.ToString(ToInt());
		case ValueHandleType.Int64:
			return XmlConverter.ToString(GetInt64());
		case ValueHandleType.UInt64:
			return XmlConverter.ToString(GetUInt64());
		case ValueHandleType.Single:
			return XmlConverter.ToString(GetSingle());
		case ValueHandleType.Double:
			return XmlConverter.ToString(GetDouble());
		case ValueHandleType.Decimal:
			return XmlConverter.ToString(GetDecimal());
		case ValueHandleType.DateTime:
			return XmlConverter.ToString(ToDateTime());
		case ValueHandleType.Empty:
			return string.Empty;
		case ValueHandleType.UTF8:
			return GetCharsText();
		case ValueHandleType.Unicode:
			return GetUnicodeCharsText();
		case ValueHandleType.EscapedUTF8:
			return GetEscapedCharsText();
		case ValueHandleType.Char:
			return GetCharText();
		case ValueHandleType.Dictionary:
			return GetDictionaryString().Value;
		case ValueHandleType.Base64:
			return Base64Encoding.GetString(ToByteArray());
		case ValueHandleType.List:
			return XmlConverter.ToString(ToList());
		case ValueHandleType.UniqueId:
			return XmlConverter.ToString(ToUniqueId());
		case ValueHandleType.Guid:
			return XmlConverter.ToString(ToGuid());
		case ValueHandleType.TimeSpan:
			return XmlConverter.ToString(ToTimeSpan());
		case ValueHandleType.QName:
			return GetQNameDictionaryText();
		case ValueHandleType.ConstString:
			return constStrings[offset];
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}
	}

	public bool Equals2(string str, bool checkLower)
	{
		if (type != ValueHandleType.UTF8)
		{
			return GetString() == str;
		}
		if (length != str.Length)
		{
			return false;
		}
		byte[] buffer = bufferReader.Buffer;
		for (int i = 0; i < length; i++)
		{
			byte b = buffer[i + offset];
			if (b != str[i] && (!checkLower || char.ToLowerInvariant((char)b) != str[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void Sign(XmlSigningNodeWriter writer)
	{
		switch (type)
		{
		case ValueHandleType.Int8:
		case ValueHandleType.Int16:
		case ValueHandleType.Int32:
			writer.WriteInt32Text(ToInt());
			break;
		case ValueHandleType.Int64:
			writer.WriteInt64Text(GetInt64());
			break;
		case ValueHandleType.UInt64:
			writer.WriteUInt64Text(GetUInt64());
			break;
		case ValueHandleType.Single:
			writer.WriteFloatText(GetSingle());
			break;
		case ValueHandleType.Double:
			writer.WriteDoubleText(GetDouble());
			break;
		case ValueHandleType.Decimal:
			writer.WriteDecimalText(GetDecimal());
			break;
		case ValueHandleType.DateTime:
			writer.WriteDateTimeText(ToDateTime());
			break;
		case ValueHandleType.UTF8:
			writer.WriteEscapedText(bufferReader.Buffer, offset, length);
			break;
		case ValueHandleType.Base64:
			writer.WriteBase64Text(bufferReader.Buffer, 0, bufferReader.Buffer, offset, length);
			break;
		case ValueHandleType.UniqueId:
			writer.WriteUniqueIdText(ToUniqueId());
			break;
		case ValueHandleType.Guid:
			writer.WriteGuidText(ToGuid());
			break;
		case ValueHandleType.TimeSpan:
			writer.WriteTimeSpanText(ToTimeSpan());
			break;
		default:
			writer.WriteEscapedText(GetString());
			break;
		case ValueHandleType.Empty:
			break;
		}
	}

	public object[] ToList()
	{
		return bufferReader.GetList(offset, length);
	}

	public object ToObject()
	{
		switch (type)
		{
		case ValueHandleType.True:
		case ValueHandleType.False:
			return ToBoolean();
		case ValueHandleType.Zero:
		case ValueHandleType.One:
		case ValueHandleType.Int8:
		case ValueHandleType.Int16:
		case ValueHandleType.Int32:
			return ToInt();
		case ValueHandleType.Int64:
			return ToLong();
		case ValueHandleType.UInt64:
			return GetUInt64();
		case ValueHandleType.Single:
			return ToSingle();
		case ValueHandleType.Double:
			return ToDouble();
		case ValueHandleType.Decimal:
			return ToDecimal();
		case ValueHandleType.DateTime:
			return ToDateTime();
		case ValueHandleType.Empty:
		case ValueHandleType.UTF8:
		case ValueHandleType.EscapedUTF8:
		case ValueHandleType.Dictionary:
		case ValueHandleType.Char:
		case ValueHandleType.Unicode:
		case ValueHandleType.ConstString:
			return ToString();
		case ValueHandleType.Base64:
			return ToByteArray();
		case ValueHandleType.List:
			return ToList();
		case ValueHandleType.UniqueId:
			return ToUniqueId();
		case ValueHandleType.Guid:
			return ToGuid();
		case ValueHandleType.TimeSpan:
			return ToTimeSpan();
		default:
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}
	}

	public bool TryReadBase64(byte[] buffer, int offset, int count, out int actual)
	{
		if (type == ValueHandleType.Base64)
		{
			actual = Math.Min(length, count);
			GetBase64(buffer, offset, actual);
			this.offset += actual;
			length -= actual;
			return true;
		}
		if (type == ValueHandleType.UTF8 && count >= 3 && length % 4 == 0)
		{
			try
			{
				int num = Math.Min(count / 3 * 4, length);
				actual = Base64Encoding.GetBytes(bufferReader.Buffer, this.offset, num, buffer, offset);
				this.offset += num;
				length -= num;
				return true;
			}
			catch (FormatException)
			{
			}
		}
		actual = 0;
		return false;
	}

	public bool TryReadChars(char[] chars, int offset, int count, out int actual)
	{
		if (type == ValueHandleType.Unicode)
		{
			return TryReadUnicodeChars(chars, offset, count, out actual);
		}
		if (type != ValueHandleType.UTF8)
		{
			actual = 0;
			return false;
		}
		int num = offset;
		int num2 = count;
		byte[] buffer = bufferReader.Buffer;
		int num3 = this.offset;
		int num4 = length;
		bool flag = false;
		while (true)
		{
			if (num2 > 0 && num4 > 0)
			{
				byte b = buffer[num3];
				if (b < 128)
				{
					chars[num] = (char)b;
					num3++;
					num4--;
					num++;
					num2--;
					continue;
				}
			}
			if (num2 == 0 || num4 == 0 || flag)
			{
				break;
			}
			UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
			int chars2;
			int num5;
			try
			{
				if (num2 >= uTF8Encoding.GetMaxCharCount(num4) || num2 >= uTF8Encoding.GetCharCount(buffer, num3, num4))
				{
					chars2 = uTF8Encoding.GetChars(buffer, num3, num4, chars, num);
					num5 = num4;
				}
				else
				{
					Decoder decoder = uTF8Encoding.GetDecoder();
					num5 = Math.Min(num2, num4);
					chars2 = decoder.GetChars(buffer, num3, num5, chars, num);
					while (chars2 == 0)
					{
						if (num5 >= 3 && num2 < 2)
						{
							flag = true;
							break;
						}
						chars2 = decoder.GetChars(buffer, num3 + num5, 1, chars, num);
						num5++;
					}
					num5 = uTF8Encoding.GetByteCount(chars, num, chars2);
				}
			}
			catch (FormatException exception)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlExceptionHelper.CreateEncodingException(buffer, num3, num4, exception));
			}
			num3 += num5;
			num4 -= num5;
			num += chars2;
			num2 -= chars2;
		}
		this.offset = num3;
		length = num4;
		actual = count - num2;
		return true;
	}

	private bool TryReadUnicodeChars(char[] chars, int offset, int count, out int actual)
	{
		int num = Math.Min(count, length / 2);
		for (int i = 0; i < num; i++)
		{
			chars[offset + i] = (char)bufferReader.GetInt16(this.offset + i * 2);
		}
		this.offset += num * 2;
		length -= num * 2;
		actual = num;
		return true;
	}

	public bool TryGetDictionaryString(out XmlDictionaryString value)
	{
		if (type == ValueHandleType.Dictionary)
		{
			value = GetDictionaryString();
			return true;
		}
		value = null;
		return false;
	}

	public bool TryGetByteArrayLength(out int length)
	{
		if (type == ValueHandleType.Base64)
		{
			length = this.length;
			return true;
		}
		length = 0;
		return false;
	}

	private string GetCharsText()
	{
		if (length == 1 && bufferReader.GetByte(offset) == 49)
		{
			return "1";
		}
		return bufferReader.GetString(offset, length);
	}

	private string GetUnicodeCharsText()
	{
		return bufferReader.GetUnicodeString(offset, length);
	}

	private string GetEscapedCharsText()
	{
		return bufferReader.GetEscapedString(offset, length);
	}

	private string GetCharText()
	{
		int num = GetChar();
		if (num > 65535)
		{
			SurrogateChar surrogateChar = new SurrogateChar(num);
			return new string(new char[2] { surrogateChar.HighChar, surrogateChar.LowChar }, 0, 2);
		}
		return ((char)num).ToString();
	}

	private int GetChar()
	{
		return offset;
	}

	private int GetInt8()
	{
		return bufferReader.GetInt8(offset);
	}

	private int GetInt16()
	{
		return bufferReader.GetInt16(offset);
	}

	private int GetInt32()
	{
		return bufferReader.GetInt32(offset);
	}

	private long GetInt64()
	{
		return bufferReader.GetInt64(offset);
	}

	private ulong GetUInt64()
	{
		return bufferReader.GetUInt64(offset);
	}

	private float GetSingle()
	{
		return bufferReader.GetSingle(offset);
	}

	private double GetDouble()
	{
		return bufferReader.GetDouble(offset);
	}

	private decimal GetDecimal()
	{
		return bufferReader.GetDecimal(offset);
	}

	private UniqueId GetUniqueId()
	{
		return bufferReader.GetUniqueId(offset);
	}

	private Guid GetGuid()
	{
		return bufferReader.GetGuid(offset);
	}

	private void GetBase64(byte[] buffer, int offset, int count)
	{
		bufferReader.GetBase64(this.offset, buffer, offset, count);
	}

	private XmlDictionaryString GetDictionaryString()
	{
		return bufferReader.GetDictionaryString(offset);
	}

	private string GetQNameDictionaryText()
	{
		return PrefixHandle.GetString(PrefixHandle.GetAlphaPrefix(length)) + ":" + bufferReader.GetDictionaryString(offset);
	}
}
