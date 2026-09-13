namespace System.Drawing.Printing;

public class StandardPrintController : PrintController
{
	public StandardPrintController()
	{
		throw new PlatformNotSupportedException();
	}

	public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}
}
