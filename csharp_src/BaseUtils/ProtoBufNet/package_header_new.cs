using System.Text;

namespace ProtoBufNet;

public class package_header_new
{
	private int expectedLength = -1;

	private int uncompressLength = -1;

	private bool binary = true;

	private bool compressed;

	private bool encrypted;

	private bool useLZ4;

	private bool bigSized;

	private bool forward;

	public int ExpectedLength
	{
		get
		{
			return expectedLength;
		}
		set
		{
			expectedLength = value;
		}
	}

	public int UnCompressLength
	{
		get
		{
			return uncompressLength;
		}
		set
		{
			uncompressLength = value;
		}
	}

	public bool Encrypted
	{
		get
		{
			return encrypted;
		}
		set
		{
			encrypted = value;
		}
	}

	public bool Compressed
	{
		get
		{
			return compressed;
		}
		set
		{
			compressed = value;
		}
	}

	public bool UseLZ4
	{
		get
		{
			return useLZ4;
		}
		set
		{
			useLZ4 = value;
		}
	}

	public bool Binary
	{
		get
		{
			return binary;
		}
		set
		{
			binary = value;
		}
	}

	public bool BigSized
	{
		get
		{
			return bigSized;
		}
		set
		{
			bigSized = value;
		}
	}

	public bool Forward
	{
		get
		{
			return forward;
		}
		set
		{
			forward = value;
		}
	}

	public package_header_new(bool encrypted, bool compressed, bool useLZ4, bool bigSized, bool forward)
	{
		this.compressed = compressed;
		this.encrypted = encrypted;
		this.useLZ4 = useLZ4;
		this.bigSized = bigSized;
		this.forward = forward;
	}

	public static package_header_new FromBinary(int headerByte)
	{
		return new package_header_new((headerByte & 0x40) > 0, (headerByte & 0x20) > 0, (headerByte & 0x10) > 0, (headerByte & 8) > 0, (headerByte & 4) > 0);
	}

	public byte Encode()
	{
		byte b = 0;
		if (binary)
		{
			b |= 0x80;
		}
		if (Encrypted)
		{
			b |= 0x40;
		}
		if (Compressed)
		{
			b |= 0x20;
		}
		if (useLZ4)
		{
			b |= 0x10;
		}
		if (bigSized)
		{
			b |= 8;
		}
		if (forward)
		{
			b |= 4;
		}
		return b;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("---------------------------------------------\n");
		stringBuilder.Append("Binary:  \t" + binary + "\n");
		stringBuilder.Append("Compressed:\t" + compressed + "\n");
		stringBuilder.Append("Encrypted:\t" + encrypted + "\n");
		stringBuilder.Append("UseLZ4:\t" + useLZ4 + "\n");
		stringBuilder.Append("BigSized:\t" + bigSized + "\n");
		stringBuilder.Append("Forward:\t" + forward + "\n");
		stringBuilder.Append("---------------------------------------------\n");
		return stringBuilder.ToString();
	}
}
