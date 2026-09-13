namespace System.Transactions;

public static class TransactionManager
{
	private static TimeSpan defaultTimeout;

	private static TimeSpan maxTimeout;

	public static TimeSpan DefaultTimeout => defaultTimeout;

	[System.MonoTODO("Not implemented")]
	public static HostCurrentTransactionCallback HostCurrentCallback
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public static TimeSpan MaximumTimeout => maxTimeout;

	public static event TransactionStartedEventHandler DistributedTransactionStarted;

	static TransactionManager()
	{
		defaultTimeout = new TimeSpan(0, 1, 0);
		maxTimeout = new TimeSpan(0, 10, 0);
	}

	[System.MonoTODO("Not implemented")]
	public static void RecoveryComplete(Guid resourceManagerIdentifier)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO("Not implemented")]
	public static Enlistment Reenlist(Guid resourceManagerIdentifier, byte[] recoveryInformation, IEnlistmentNotification enlistmentNotification)
	{
		throw new NotImplementedException();
	}
}
