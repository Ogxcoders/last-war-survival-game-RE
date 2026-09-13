namespace System.Drawing.Printing;

public class PreviewPrintController : PrintController
{
	public override bool IsPreview
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public virtual bool UseAntiAlias
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

	public PreviewPrintController()
	{
		throw new PlatformNotSupportedException();
	}

	public PreviewPageInfo[] GetPreviewPageInfo()
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
