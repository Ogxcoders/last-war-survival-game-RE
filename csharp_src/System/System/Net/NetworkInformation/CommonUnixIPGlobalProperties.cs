using System.Runtime.InteropServices;
using System.Text;

namespace System.Net.NetworkInformation;

internal abstract class CommonUnixIPGlobalProperties : IPGlobalProperties
{
	public override string DhcpScopeName => string.Empty;

	public override string DomainName
	{
		get
		{
			byte[] array = new byte[256];
			if (getdomainname(array, 256) != 0)
			{
				throw new NetworkInformationException();
			}
			int num = Array.IndexOf(array, (byte)0);
			return Encoding.ASCII.GetString(array, 0, (num < 0) ? 256 : num);
		}
	}

	public override string HostName
	{
		get
		{
			byte[] array = new byte[256];
			if (gethostname(array, 256) != 0)
			{
				throw new NetworkInformationException();
			}
			int num = Array.IndexOf(array, (byte)0);
			return Encoding.ASCII.GetString(array, 0, (num < 0) ? 256 : num);
		}
	}

	public override bool IsWinsProxy => false;

	public override NetBiosNodeType NodeType => NetBiosNodeType.Unknown;

	[DllImport("libc")]
	private static extern int gethostname([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] name, int len);

	[DllImport("libc")]
	private static extern int getdomainname([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] name, int len);
}
