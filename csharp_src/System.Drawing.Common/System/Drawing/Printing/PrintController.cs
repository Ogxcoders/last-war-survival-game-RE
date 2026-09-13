namespace System.Drawing.Printing;

public abstract class PrintController
{
	public virtual bool IsPreview
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	protected PrintController()
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void OnEndPage(PrintDocument document, PrintPageEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void OnEndPrint(PrintDocument document, PrintEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public virtual void OnStartPrint(PrintDocument document, PrintEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}
}
