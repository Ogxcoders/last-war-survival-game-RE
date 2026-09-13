namespace System.Diagnostics;

public class DataReceivedEventArgs : EventArgs
{
	private string data;

	public string Data => data;

	internal DataReceivedEventArgs(string data)
	{
		this.data = data;
	}
}
