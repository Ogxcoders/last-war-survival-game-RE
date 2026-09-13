namespace System.Data.SqlClient;

internal static class SNINativeMethodWrapper
{
	internal enum SniSpecialErrors : uint
	{
		LocalDBErrorCode = 50u,
		MultiSubnetFailoverWithMoreThan64IPs = 47u,
		MultiSubnetFailoverWithInstanceSpecified = 48u,
		MultiSubnetFailoverWithNonTcpProtocol = 49u,
		MaxErrorValue = 50157u
	}
}
