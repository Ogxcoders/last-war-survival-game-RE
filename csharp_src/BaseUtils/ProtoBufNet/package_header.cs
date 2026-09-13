using System.Text;

namespace ProtoBufNet;

public class package_header
{
	private int expectedLength = -1;

	private bool binary = true;

	private bool compressed;

	private bool encrypted;

	private bool blueBoxed;

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

	public bool BlueBoxed
	{
		get
		{
			return blueBoxed;
		}
		set
		{
			blueBoxed = value;
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

	public package_header(bool encrypted, bool compressed, bool blueBoxed, bool bigSized, bool forward)
	{
		this.compressed = compressed;
		this.encrypted = encrypted;
		this.blueBoxed = blueBoxed;
		this.bigSized = bigSized;
		this.forward = forward;
	}

	public static package_header FromBinary(int headerByte)
	{
		return new package_header((headerByte & 0x40) > 0, (headerByte & 0x20) > 0, (headerByte & 0x10) > 0, (headerByte & 8) > 0, (headerByte & 4) > 0);
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
		if (blueBoxed)
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
		stringBuilder.Append("BlueBoxed:\t" + blueBoxed + "\n");
		stringBuilder.Append("BigSized:\t" + bigSized + "\n");
		stringBuilder.Append("Forward:\t" + forward + "\n");
		stringBuilder.Append("---------------------------------------------\n");
		return stringBuilder.ToString();
	}
}
