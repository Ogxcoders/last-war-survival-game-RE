namespace System.Drawing.Printing;

public class PrintPageEventArgs : EventArgs
{
	public bool Cancel
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Graphics Graphics
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool HasMorePages
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Rectangle MarginBounds
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Rectangle PageBounds
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PageSettings PageSettings
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PrintPageEventArgs(Graphics graphics, Rectangle marginBounds, Rectangle pageBounds, PageSettings pageSettings)
	{
		throw new PlatformNotSupportedException();
	}
}
