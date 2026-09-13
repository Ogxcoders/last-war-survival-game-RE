namespace System.Net.NetworkInformation;

internal sealed class AndroidIPGlobalProperties : UnixIPGlobalProperties
{
	public override string DomainName => string.Empty;
}
