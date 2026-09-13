namespace System.Timers;

public class ElapsedEventArgs : EventArgs
{
	private DateTime time;

	public DateTime SignalTime => time;

	internal ElapsedEventArgs(DateTime time)
	{
		this.time = time;
	}
}
