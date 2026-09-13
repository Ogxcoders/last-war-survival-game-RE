namespace System.Net.NetworkInformation;

public sealed class NetworkChange
{
	private static INetworkChange networkChange;

	public static event NetworkAddressChangedEventHandler NetworkAddressChanged
	{
		add
		{
			lock (typeof(INetworkChange))
			{
				MaybeCreate();
				if (networkChange != null)
				{
					networkChange.NetworkAddressChanged += value;
				}
			}
		}
		remove
		{
			lock (typeof(INetworkChange))
			{
				if (networkChange != null)
				{
					networkChange.NetworkAddressChanged -= value;
					MaybeDispose();
				}
			}
		}
	}

	public static event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
	{
		add
		{
			lock (typeof(INetworkChange))
			{
				MaybeCreate();
				if (networkChange != null)
				{
					networkChange.NetworkAvailabilityChanged += value;
				}
			}
		}
		remove
		{
			lock (typeof(INetworkChange))
			{
				if (networkChange != null)
				{
					networkChange.NetworkAvailabilityChanged -= value;
					MaybeDispose();
				}
			}
		}
	}

	private static void MaybeCreate()
	{
		if (networkChange != null)
		{
			return;
		}
		try
		{
			networkChange = new MacNetworkChange();
		}
		catch
		{
			networkChange = new LinuxNetworkChange();
		}
	}

	private static void MaybeDispose()
	{
		if (networkChange != null && networkChange.HasRegisteredEvents)
		{
			networkChange.Dispose();
			networkChange = null;
		}
	}
}
