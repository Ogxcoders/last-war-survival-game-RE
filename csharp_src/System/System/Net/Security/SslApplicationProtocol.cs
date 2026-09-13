using System.Text;

namespace System.Net.Security;

public readonly struct SslApplicationProtocol : IEquatable<SslApplicationProtocol>
{
	private readonly ReadOnlyMemory<byte> _readOnlyProtocol;

	private static readonly Encoding s_utf8 = Encoding.GetEncoding(Encoding.UTF8.CodePage, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);

	public static readonly SslApplicationProtocol Http2 = new SslApplicationProtocol(new byte[2] { 104, 50 }, copy: false);

	public static readonly SslApplicationProtocol Http11 = new SslApplicationProtocol(new byte[8] { 104, 116, 116, 112, 47, 49, 46, 49 }, copy: false);

	public ReadOnlyMemory<byte> Protocol => _readOnlyProtocol;

	internal SslApplicationProtocol(byte[] protocol, bool copy)
	{
		if (protocol == null)
		{
			throw new ArgumentNullException("protocol");
		}
		if (protocol.Length == 0 || protocol.Length > 255)
		{
			throw new ArgumentException("The application protocol value is invalid.", "protocol");
		}
		if (copy)
		{
			byte[] array = new byte[protocol.Length];
			Array.Copy(protocol, 0, array, 0, protocol.Length);
			_readOnlyProtocol = new ReadOnlyMemory<byte>(array);
		}
		else
		{
			_readOnlyProtocol = new ReadOnlyMemory<byte>(protocol);
		}
	}

	public SslApplicationProtocol(byte[] protocol)
		: this(protocol, copy: true)
	{
	}

	public SslApplicationProtocol(string protocol)
		: this(s_utf8.GetBytes(protocol), copy: false)
	{
	}

	public bool Equals(SslApplicationProtocol other)
	{
		if (_readOnlyProtocol.Length != other._readOnlyProtocol.Length)
		{
			return false;
		}
		if (!_readOnlyProtocol.IsEmpty || !other._readOnlyProtocol.IsEmpty)
		{
			return _readOnlyProtocol.Span.SequenceEqual(other._readOnlyProtocol.Span);
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj is SslApplicationProtocol other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (_readOnlyProtocol.Length == 0)
		{
			return 0;
		}
		int num = 0;
		ReadOnlySpan<byte> span = _readOnlyProtocol.Span;
		for (int i = 0; i < _readOnlyProtocol.Length; i++)
		{
			num = ((num << 5) + num) ^ span[i];
		}
		return num;
	}

	public override string ToString()
	{
		try
		{
			if (_readOnlyProtocol.Length == 0)
			{
				return null;
			}
			return s_utf8.GetString(_readOnlyProtocol.Span);
		}
		catch
		{
			int num = _readOnlyProtocol.Length * 5;
			char[] array = new char[num];
			int num2 = 0;
			ReadOnlySpan<byte> span = _readOnlyProtocol.Span;
			for (int i = 0; i < num; i += 5)
			{
				byte a = span[num2++];
				array[i] = '0';
				array[i + 1] = 'x';
				array[i + 2] = GetHexValue(Math.DivRem(a, 16, out var result));
				array[i + 3] = GetHexValue(result);
				array[i + 4] = ' ';
			}
			return new string(array, 0, num - 1);
		}
	}

	private static char GetHexValue(int i)
	{
		if (i < 10)
		{
			return (char)(i + 48);
		}
		return (char)(i - 10 + 97);
	}

	public static bool operator ==(SslApplicationProtocol left, SslApplicationProtocol right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(SslApplicationProtocol left, SslApplicationProtocol right)
	{
		return !(left == right);
	}
}
