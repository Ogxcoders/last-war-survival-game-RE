using System.Net.Sockets;

namespace System.Net;

public class DnsEndPoint : EndPoint
{
	private string m_Host;

	private int m_Port;

	private AddressFamily m_Family;

	public string Host => m_Host;

	public override AddressFamily AddressFamily => m_Family;

	public int Port => m_Port;

	public DnsEndPoint(string host, int port)
		: this(host, port, AddressFamily.Unspecified)
	{
	}

	public DnsEndPoint(string host, int port, AddressFamily addressFamily)
	{
		if (host == null)
		{
			throw new ArgumentNullException("host");
		}
		if (string.IsNullOrEmpty(host))
		{
			throw new ArgumentException(global::SR.GetString("The parameter '{0}' cannot be an empty string.", "host"));
		}
		if (port < 0 || port > 65535)
		{
			throw new ArgumentOutOfRangeException("port");
		}
		if (addressFamily != AddressFamily.InterNetwork && addressFamily != AddressFamily.InterNetworkV6 && addressFamily != AddressFamily.Unspecified)
		{
			throw new ArgumentException(global::SR.GetString("The specified value is not valid."), "addressFamily");
		}
		m_Host = host;
		m_Port = port;
		m_Family = addressFamily;
	}

	public override bool Equals(object comparand)
	{
		if (!(comparand is DnsEndPoint dnsEndPoint))
		{
			return false;
		}
		if (m_Family == dnsEndPoint.m_Family && m_Port == dnsEndPoint.m_Port)
		{
			return m_Host == dnsEndPoint.m_Host;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return StringComparer.InvariantCultureIgnoreCase.GetHashCode(ToString());
	}

	public override string ToString()
	{
		return m_Family.ToString() + "/" + m_Host + ":" + m_Port;
	}
}
