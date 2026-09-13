using System.Text;

namespace System.Net.Sockets;

public sealed class UnixDomainSocketEndPoint : EndPoint
{
	private static readonly int s_nativePathOffset;

	private static readonly int s_nativePathLength;

	private static readonly int s_nativeAddressSize;

	private const AddressFamily EndPointAddressFamily = AddressFamily.Unix;

	private static readonly Encoding s_pathEncoding;

	private static readonly Lazy<bool> s_udsSupported;

	private readonly string _path;

	private readonly byte[] _encodedPath;

	public override AddressFamily AddressFamily => AddressFamily.Unix;

	static UnixDomainSocketEndPoint()
	{
		s_pathEncoding = Encoding.UTF8;
		s_udsSupported = new Lazy<bool>(delegate
		{
			try
			{
				new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP).Dispose();
				return true;
			}
			catch
			{
				return false;
			}
		});
		global::Interop.Sys.GetDomainSocketSizes(out s_nativePathOffset, out s_nativePathLength, out s_nativeAddressSize);
	}

	private SocketAddress CreateSocketAddressForSerialize()
	{
		return new SocketAddress(AddressFamily.Unix, s_nativePathOffset + _encodedPath.Length);
	}

	public UnixDomainSocketEndPoint(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		bool num = IsAbstract(path);
		int num2 = s_pathEncoding.GetByteCount(path);
		if (!num)
		{
			num2++;
		}
		if (path.Length == 0 || num2 > s_nativePathLength)
		{
			throw new ArgumentOutOfRangeException("path", path, SR.Format("The path '{0}' is of an invalid length for use with domain sockets on this platform.  The length must be between 1 and {1} characters, inclusive.", path, s_nativePathLength));
		}
		_path = path;
		_encodedPath = new byte[num2];
		s_pathEncoding.GetBytes(path, 0, path.Length, _encodedPath, 0);
		if (!s_udsSupported.Value)
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal UnixDomainSocketEndPoint(SocketAddress socketAddress)
	{
		if (socketAddress == null)
		{
			throw new ArgumentNullException("socketAddress");
		}
		if (socketAddress.Family != AddressFamily.Unix || socketAddress.Size > s_nativeAddressSize)
		{
			throw new ArgumentOutOfRangeException("socketAddress");
		}
		if (socketAddress.Size > s_nativePathOffset)
		{
			_encodedPath = new byte[socketAddress.Size - s_nativePathOffset];
			for (int i = 0; i < _encodedPath.Length; i++)
			{
				_encodedPath[i] = socketAddress[s_nativePathOffset + i];
			}
			int num = _encodedPath.Length;
			if (!IsAbstract(_encodedPath))
			{
				while (_encodedPath[num - 1] == 0)
				{
					num--;
				}
			}
			_path = s_pathEncoding.GetString(_encodedPath, 0, num);
		}
		else
		{
			_encodedPath = Array.Empty<byte>();
			_path = string.Empty;
		}
	}

	public override SocketAddress Serialize()
	{
		SocketAddress socketAddress = CreateSocketAddressForSerialize();
		for (int i = 0; i < _encodedPath.Length; i++)
		{
			socketAddress[s_nativePathOffset + i] = _encodedPath[i];
		}
		return socketAddress;
	}

	public override EndPoint Create(SocketAddress socketAddress)
	{
		return new UnixDomainSocketEndPoint(socketAddress);
	}

	public override string ToString()
	{
		if (IsAbstract(_path))
		{
			return "@" + _path.Substring(1);
		}
		return _path;
	}

	private static bool IsAbstract(string path)
	{
		if (path.Length > 0)
		{
			return path[0] == '\0';
		}
		return false;
	}

	private static bool IsAbstract(byte[] encodedPath)
	{
		if (encodedPath.Length != 0)
		{
			return encodedPath[0] == 0;
		}
		return false;
	}
}
