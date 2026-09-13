namespace System.Net.NetworkInformation;

internal interface INetworkChange : IDisposable
{
	bool HasRegisteredEvents { get; }

	event NetworkAddressChangedEventHandler NetworkAddressChanged;

	event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged;
}
