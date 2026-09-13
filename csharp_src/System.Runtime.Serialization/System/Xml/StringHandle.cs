namespace System.Xml;

internal class StringHandle
{
	private enum StringHandleType
	{
		Dictionary,
		UTF8,
		EscapedUTF8,
		ConstString
	}

	private XmlBufferReader bufferReader;

	private StringHandleType type;

	private int key;

	private int offset;

	private int length;

	private static string[] constStrings = new string[3] { "type", "root", "item" };

	public bool IsEmpty
	{
		get
		{
			if (type == StringHandleType.UTF8)
			{
				return length == 0;
			}
			return Equals2(string.Empty);
		}
	}

	public bool IsXmlns
	{
		get
		{
			if (type == StringHandleType.UTF8)
			{
				if (length != 5)
				{
					return false;
				}
				byte[] buffer = bufferReader.Buffer;
				int num = offset;
				if (buffer[num] == 120 && buffer[num + 1] == 109 && buffer[num + 2] == 108 && buffer[num + 3] == 110)
				{
					return buffer[num + 4] == 115;
				}
				return false;
			}
			return Equals2("xmlns");
		}
	}

	public StringHandle(XmlBufferReader bufferReader)
	{
		this.bufferReader = bufferReader;
		SetValue(0, 0);
	}

	public void SetValue(int offset, int length)
	{
		type = StringHandleType.UTF8;
		this.offset = offset;
		this.length = length;
	}

	public void SetConstantValue(StringHandleConstStringType constStringType)
	{
		type = StringHandleType.ConstString;
		key = (int)constStringType;
	}

	public void SetValue(int offset, int length, bool escaped)
	{
		type = ((!escaped) ? StringHandleType.UTF8 : StringHandleType.EscapedUTF8);
		this.offset = offset;
		this.length = length;
	}

	public void SetValue(int key)
	{
		type = StringHandleType.Dictionary;
		this.key = key;
	}

	public void SetValue(StringHandle value)
	{
		type = value.type;
		key = value.key;
		offset = value.offset;
		length = value.length;
	}

	public void ToPrefixHandle(PrefixHandle prefix)
	{
		prefix.SetValue(offset, length);
	}

	public string GetString(XmlNameTable nameTable)
	{
		return type switch
		{
			StringHandleType.UTF8 => bufferReader.GetString(offset, length, nameTable), 
			StringHandleType.Dictionary => nameTable.Add(bufferReader.GetDictionaryString(key).Value), 
			StringHandleType.ConstString => nameTable.Add(constStrings[key]), 
			_ => bufferReader.GetEscapedString(offset, length, nameTable), 
		};
	}

	public string GetString()
	{
		return type switch
		{
			StringHandleType.UTF8 => bufferReader.GetString(offset, length), 
			StringHandleType.Dictionary => bufferReader.GetDictionaryString(key).Value, 
			StringHandleType.ConstString => constStrings[key], 
			_ => bufferReader.GetEscapedString(offset, length), 
		};
	}

	public byte[] GetString(out int offset, out int length)
	{
		switch (type)
		{
		case StringHandleType.UTF8:
			offset = this.offset;
			length = this.length;
			return bufferReader.Buffer;
		case StringHandleType.Dictionary:
		{
			byte[] array3 = bufferReader.GetDictionaryString(key).ToUTF8();
			offset = 0;
			length = array3.Length;
			return array3;
		}
		case StringHandleType.ConstString:
		{
			byte[] array2 = XmlConverter.ToBytes(constStrings[key]);
			offset = 0;
			length = array2.Length;
			return array2;
		}
		default:
		{
			byte[] array = XmlConverter.ToBytes(bufferReader.GetEscapedString(this.offset, this.length));
			offset = 0;
			length = array.Length;
			return array;
		}
		}
	}

	public bool TryGetDictionaryString(out XmlDictionaryString value)
	{
		if (type == StringHandleType.Dictionary)
		{
			value = bufferReader.GetDictionaryString(key);
			return true;
		}
		if (IsEmpty)
		{
			value = XmlDictionaryString.Empty;
			return true;
		}
		value = null;
		return false;
	}

	public override string ToString()
	{
		return GetString();
	}

	private bool Equals2(int key2, XmlBufferReader bufferReader2)
	{
		return type switch
		{
			StringHandleType.Dictionary => bufferReader.Equals2(key, key2, bufferReader2), 
			StringHandleType.UTF8 => bufferReader.Equals2(offset, length, bufferReader2.GetDictionaryString(key2).Value), 
			_ => GetString() == bufferReader.GetDictionaryString(key2).Value, 
		};
	}

	private bool Equals2(XmlDictionaryString xmlString2)
	{
		return type switch
		{
			StringHandleType.Dictionary => bufferReader.Equals2(key, xmlString2), 
			StringHandleType.UTF8 => bufferReader.Equals2(offset, length, xmlString2.ToUTF8()), 
			_ => GetString() == xmlString2.Value, 
		};
	}

	private bool Equals2(string s2)
	{
		return type switch
		{
			StringHandleType.Dictionary => bufferReader.GetDictionaryString(key).Value == s2, 
			StringHandleType.UTF8 => bufferReader.Equals2(offset, length, s2), 
			_ => GetString() == s2, 
		};
	}

	private bool Equals2(int offset2, int length2, XmlBufferReader bufferReader2)
	{
		return type switch
		{
			StringHandleType.Dictionary => bufferReader2.Equals2(offset2, length2, bufferReader.GetDictionaryString(key).Value), 
			StringHandleType.UTF8 => bufferReader.Equals2(offset, length, bufferReader2, offset2, length2), 
			_ => GetString() == bufferReader.GetString(offset2, length2), 
		};
	}

	private bool Equals2(StringHandle s2)
	{
		return s2.type switch
		{
			StringHandleType.Dictionary => Equals2(s2.key, s2.bufferReader), 
			StringHandleType.UTF8 => Equals2(s2.offset, s2.length, s2.bufferReader), 
			_ => Equals2(s2.GetString()), 
		};
	}

	public static bool operator ==(StringHandle s1, XmlDictionaryString xmlString2)
	{
		return s1.Equals2(xmlString2);
	}

	public static bool operator !=(StringHandle s1, XmlDictionaryString xmlString2)
	{
		return !s1.Equals2(xmlString2);
	}

	public static bool operator ==(StringHandle s1, string s2)
	{
		return s1.Equals2(s2);
	}

	public static bool operator !=(StringHandle s1, string s2)
	{
		return !s1.Equals2(s2);
	}

	public static bool operator ==(StringHandle s1, StringHandle s2)
	{
		return s1.Equals2(s2);
	}

	public static bool operator !=(StringHandle s1, StringHandle s2)
	{
		return !s1.Equals2(s2);
	}

	public int CompareTo(StringHandle that)
	{
		if (type == StringHandleType.UTF8 && that.type == StringHandleType.UTF8)
		{
			return bufferReader.Compare(offset, length, that.offset, that.length);
		}
		return string.Compare(GetString(), that.GetString(), StringComparison.Ordinal);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StringHandle stringHandle))
		{
			return false;
		}
		return this == stringHandle;
	}

	public override int GetHashCode()
	{
		return GetString().GetHashCode();
	}
}
