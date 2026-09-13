namespace System.Net.Sockets;

public struct IPPacketInformation
{
	private IPAddress address;

	private int networkInterface;

	public IPAddress Address => address;

	public int Interface => networkInterface;

	internal IPPacketInformation(IPAddress address, int networkInterface)
	{
		this.address = address;
		this.networkInterface = networkInterface;
	}

	public static bool operator ==(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
	{
		return packetInformation1.Equals(packetInformation2);
	}

	public static bool operator !=(IPPacketInformation packetInformation1, IPPacketInformation packetInformation2)
	{
		return !packetInformation1.Equals(packetInformation2);
	}

	public override bool Equals(object comparand)
	{
		if (comparand == null)
		{
			return false;
		}
		if (!(comparand is IPPacketInformation iPPacketInformation))
		{
			return false;
		}
		if (address.Equals(iPPacketInformation.address) && networkInterface == iPPacketInformation.networkInterface)
		{
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return address.GetHashCode() + networkInterface.GetHashCode();
	}
}
