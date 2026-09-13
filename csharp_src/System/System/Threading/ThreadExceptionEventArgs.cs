namespace System.Threading;

public class ThreadExceptionEventArgs : EventArgs
{
	private Exception exception;

	public Exception Exception => exception;

	public ThreadExceptionEventArgs(Exception t)
	{
		exception = t;
	}
}
