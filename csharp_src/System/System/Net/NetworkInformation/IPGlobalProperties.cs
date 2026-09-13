using System.Security.Permissions;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation;

public abstract class IPGlobalProperties
{
	public abstract string DhcpScopeName { get; }

	public abstract string DomainName { get; }

	public abstract string HostName { get; }

	public abstract bool IsWinsProxy { get; }

	public abstract NetBiosNodeType NodeType { get; }

	public static IPGlobalProperties GetIPGlobalProperties()
	{
		return IPGlobalPropertiesFactoryPal.Create();
	}

	internal static IPGlobalProperties InternalGetIPGlobalProperties()
	{
		return GetIPGlobalProperties();
	}

	public abstract IPEndPoint[] GetActiveUdpListeners();

	public abstract IPEndPoint[] GetActiveTcpListeners();

	public abstract TcpConnectionInformation[] GetActiveTcpConnections();

	public abstract TcpStatistics GetTcpIPv4Statistics();

	public abstract TcpStatistics GetTcpIPv6Statistics();

	public abstract UdpStatistics GetUdpIPv4Statistics();

	public abstract UdpStatistics GetUdpIPv6Statistics();

	public abstract IcmpV4Statistics GetIcmpV4Statistics();

	public abstract IcmpV6Statistics GetIcmpV6Statistics();

	public abstract IPGlobalStatistics GetIPv4GlobalStatistics();

	public abstract IPGlobalStatistics GetIPv6GlobalStatistics();

	public virtual UnicastIPAddressInformationCollection GetUnicastAddresses()
	{
		throw ExceptionHelper.MethodNotImplementedException;
	}

	public virtual IAsyncResult BeginGetUnicastAddresses(AsyncCallback callback, object state)
	{
		throw ExceptionHelper.MethodNotImplementedException;
	}

	public virtual UnicastIPAddressInformationCollection EndGetUnicastAddresses(IAsyncResult asyncResult)
	{
		throw ExceptionHelper.MethodNotImplementedException;
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public virtual Task<UnicastIPAddressInformationCollection> GetUnicastAddressesAsync()
	{
		return Task<UnicastIPAddressInformationCollection>.Factory.FromAsync(BeginGetUnicastAddresses, EndGetUnicastAddresses, null);
	}
}
