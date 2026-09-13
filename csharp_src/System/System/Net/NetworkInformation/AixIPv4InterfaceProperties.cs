namespace System.Net.NetworkInformation;

internal sealed class AixIPv4InterfaceProperties : UnixIPv4InterfaceProperties
{
	private int _mtu;

	public override bool IsForwardingEnabled => false;

	public override int Mtu => _mtu;

	public AixIPv4InterfaceProperties(AixNetworkInterface iface, int mtu)
		: base(iface)
	{
		_mtu = mtu;
	}
}
