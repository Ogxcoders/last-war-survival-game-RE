namespace System.Net.Sockets;

public class IPv6MulticastOption
{
	private IPAddress m_Group;

	private long m_Interface;

	public IPAddress Group
	{
		get
		{
			return m_Group;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_Group = value;
		}
	}

	public long InterfaceIndex
	{
		get
		{
			return m_Interface;
		}
		set
		{
			if (value < 0 || value > uint.MaxValue)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			m_Interface = value;
		}
	}

	public IPv6MulticastOption(IPAddress group, long ifindex)
	{
		if (group == null)
		{
			throw new ArgumentNullException("group");
		}
		if (ifindex < 0 || ifindex > uint.MaxValue)
		{
			throw new ArgumentOutOfRangeException("ifindex");
		}
		Group = group;
		InterfaceIndex = ifindex;
	}

	public IPv6MulticastOption(IPAddress group)
	{
		if (group == null)
		{
			throw new ArgumentNullException("group");
		}
		Group = group;
		InterfaceIndex = 0L;
	}
}
