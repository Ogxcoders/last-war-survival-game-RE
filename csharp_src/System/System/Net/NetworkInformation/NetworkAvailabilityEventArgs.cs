namespace System.Net.NetworkInformation;

public class NetworkAvailabilityEventArgs : EventArgs
{
	private bool isAvailable;

	public bool IsAvailable => isAvailable;

	internal NetworkAvailabilityEventArgs(bool isAvailable)
	{
		this.isAvailable = isAvailable;
	}
}
