using System.ComponentModel;

namespace System.Drawing.Printing;

public class PrintEventArgs : CancelEventArgs
{
	public PrintAction PrintAction
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PrintEventArgs()
	{
		throw new PlatformNotSupportedException();
	}
}
