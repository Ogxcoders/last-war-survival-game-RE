namespace System.Net.Sockets;

public struct UdpReceiveResult : IEquatable<UdpReceiveResult>
{
	private byte[] m_buffer;

	private IPEndPoint m_remoteEndPoint;

	public byte[] Buffer => m_buffer;

	public IPEndPoint RemoteEndPoint => m_remoteEndPoint;

	public UdpReceiveResult(byte[] buffer, IPEndPoint remoteEndPoint)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (remoteEndPoint == null)
		{
			throw new ArgumentNullException("remoteEndPoint");
		}
		m_buffer = buffer;
		m_remoteEndPoint = remoteEndPoint;
	}

	public override int GetHashCode()
	{
		if (m_buffer == null)
		{
			return 0;
		}
		return m_buffer.GetHashCode() ^ m_remoteEndPoint.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UdpReceiveResult))
		{
			return false;
		}
		return Equals((UdpReceiveResult)obj);
	}

	public bool Equals(UdpReceiveResult other)
	{
		if (object.Equals(m_buffer, other.m_buffer))
		{
			return object.Equals(m_remoteEndPoint, other.m_remoteEndPoint);
		}
		return false;
	}

	public static bool operator ==(UdpReceiveResult left, UdpReceiveResult right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(UdpReceiveResult left, UdpReceiveResult right)
	{
		return !left.Equals(right);
	}
}
