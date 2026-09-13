namespace System.Net.Sockets;

public class MulticastOption
{
	private IPAddress group;

	private IPAddress localAddress;

	private int ifIndex;

	public IPAddress Group
	{
		get
		{
			return group;
		}
		set
		{
			group = value;
		}
	}

	public IPAddress LocalAddress
	{
		get
		{
			return localAddress;
		}
		set
		{
			ifIndex = 0;
			localAddress = value;
		}
	}

	public int InterfaceIndex
	{
		get
		{
			return ifIndex;
		}
		set
		{
			if (value < 0 || value > 16777215)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			localAddress = null;
			ifIndex = value;
		}
	}

	public MulticastOption(IPAddress group, IPAddress mcint)
	{
		if (group == null)
		{
			throw new ArgumentNullException("group");
		}
		if (mcint == null)
		{
			throw new ArgumentNullException("mcint");
		}
		Group = group;
		LocalAddress = mcint;
	}

	public MulticastOption(IPAddress group, int interfaceIndex)
	{
		if (group == null)
		{
			throw new ArgumentNullException("group");
		}
		if (interfaceIndex < 0 || interfaceIndex > 16777215)
		{
			throw new ArgumentOutOfRangeException("interfaceIndex");
		}
		Group = group;
		ifIndex = interfaceIndex;
	}

	public MulticastOption(IPAddress group)
	{
		if (group == null)
		{
			throw new ArgumentNullException("group");
		}
		Group = group;
		LocalAddress = IPAddress.Any;
	}
}
