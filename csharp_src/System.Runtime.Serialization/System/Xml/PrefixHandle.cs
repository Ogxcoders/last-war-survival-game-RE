namespace System.Xml;

internal class PrefixHandle
{
	private XmlBufferReader bufferReader;

	private PrefixHandleType type;

	private int offset;

	private int length;

	private static string[] prefixStrings = new string[27]
	{
		"", "a", "b", "c", "d", "e", "f", "g", "h", "i",
		"j", "k", "l", "m", "n", "o", "p", "q", "r", "s",
		"t", "u", "v", "w", "x", "y", "z"
	};

	private static byte[] prefixBuffer = new byte[26]
	{
		97, 98, 99, 100, 101, 102, 103, 104, 105, 106,
		107, 108, 109, 110, 111, 112, 113, 114, 115, 116,
		117, 118, 119, 120, 121, 122
	};

	public bool IsEmpty => type == PrefixHandleType.Empty;

	public bool IsXmlns
	{
		get
		{
			if (type != PrefixHandleType.Buffer)
			{
				return false;
			}
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
	}

	public bool IsXml
	{
		get
		{
			if (type != PrefixHandleType.Buffer)
			{
				return false;
			}
			if (length != 3)
			{
				return false;
			}
			byte[] buffer = bufferReader.Buffer;
			int num = offset;
			if (buffer[num] == 120 && buffer[num + 1] == 109)
			{
				return buffer[num + 2] == 108;
			}
			return false;
		}
	}

	public PrefixHandle(XmlBufferReader bufferReader)
	{
		this.bufferReader = bufferReader;
	}

	public void SetValue(PrefixHandleType type)
	{
		this.type = type;
	}

	public void SetValue(PrefixHandle prefix)
	{
		type = prefix.type;
		offset = prefix.offset;
		length = prefix.length;
	}

	public void SetValue(int offset, int length)
	{
		switch (length)
		{
		case 0:
			SetValue(PrefixHandleType.Empty);
			return;
		case 1:
		{
			byte b = bufferReader.GetByte(offset);
			if (b >= 97 && b <= 122)
			{
				SetValue(GetAlphaPrefix(b - 97));
				return;
			}
			break;
		}
		}
		type = PrefixHandleType.Buffer;
		this.offset = offset;
		this.length = length;
	}

	public bool TryGetShortPrefix(out PrefixHandleType type)
	{
		type = this.type;
		return type != PrefixHandleType.Buffer;
	}

	public static string GetString(PrefixHandleType type)
	{
		return prefixStrings[(int)type];
	}

	public static PrefixHandleType GetAlphaPrefix(int index)
	{
		return (PrefixHandleType)(1 + index);
	}

	public static byte[] GetString(PrefixHandleType type, out int offset, out int length)
	{
		if (type == PrefixHandleType.Empty)
		{
			offset = 0;
			length = 0;
		}
		else
		{
			length = 1;
			offset = (int)(type - 1);
		}
		return prefixBuffer;
	}

	public string GetString(XmlNameTable nameTable)
	{
		PrefixHandleType prefixHandleType = type;
		if (prefixHandleType != PrefixHandleType.Buffer)
		{
			return GetString(prefixHandleType);
		}
		return bufferReader.GetString(offset, length, nameTable);
	}

	public string GetString()
	{
		PrefixHandleType prefixHandleType = type;
		if (prefixHandleType != PrefixHandleType.Buffer)
		{
			return GetString(prefixHandleType);
		}
		return bufferReader.GetString(offset, length);
	}

	public byte[] GetString(out int offset, out int length)
	{
		PrefixHandleType prefixHandleType = type;
		if (prefixHandleType != PrefixHandleType.Buffer)
		{
			return GetString(prefixHandleType, out offset, out length);
		}
		offset = this.offset;
		length = this.length;
		return bufferReader.Buffer;
	}

	public int CompareTo(PrefixHandle that)
	{
		return GetString().CompareTo(that.GetString());
	}

	private bool Equals2(PrefixHandle prefix2)
	{
		PrefixHandleType prefixHandleType = type;
		PrefixHandleType prefixHandleType2 = prefix2.type;
		if (prefixHandleType != prefixHandleType2)
		{
			return false;
		}
		if (prefixHandleType != PrefixHandleType.Buffer)
		{
			return true;
		}
		if (bufferReader == prefix2.bufferReader)
		{
			return bufferReader.Equals2(offset, length, prefix2.offset, prefix2.length);
		}
		return bufferReader.Equals2(offset, length, prefix2.bufferReader, prefix2.offset, prefix2.length);
	}

	private bool Equals2(string prefix2)
	{
		PrefixHandleType prefixHandleType = type;
		if (prefixHandleType != PrefixHandleType.Buffer)
		{
			return GetString(prefixHandleType) == prefix2;
		}
		return bufferReader.Equals2(offset, length, prefix2);
	}

	private bool Equals2(XmlDictionaryString prefix2)
	{
		return Equals2(prefix2.Value);
	}

	public static bool operator ==(PrefixHandle prefix1, string prefix2)
	{
		return prefix1.Equals2(prefix2);
	}

	public static bool operator !=(PrefixHandle prefix1, string prefix2)
	{
		return !prefix1.Equals2(prefix2);
	}

	public static bool operator ==(PrefixHandle prefix1, XmlDictionaryString prefix2)
	{
		return prefix1.Equals2(prefix2);
	}

	public static bool operator !=(PrefixHandle prefix1, XmlDictionaryString prefix2)
	{
		return !prefix1.Equals2(prefix2);
	}

	public static bool operator ==(PrefixHandle prefix1, PrefixHandle prefix2)
	{
		return prefix1.Equals2(prefix2);
	}

	public static bool operator !=(PrefixHandle prefix1, PrefixHandle prefix2)
	{
		return !prefix1.Equals2(prefix2);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is PrefixHandle prefixHandle))
		{
			return false;
		}
		return this == prefixHandle;
	}

	public override string ToString()
	{
		return GetString();
	}

	public override int GetHashCode()
	{
		return GetString().GetHashCode();
	}
}
