using System.ComponentModel;

namespace System.Drawing.Printing;

public class PrintDocument : Component
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PageSettings DefaultPageSettings
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

	[DefaultValue("document")]
	public string DocumentName
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

	[DefaultValue(false)]
	public bool OriginAtMargins
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PrintController PrintController
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PrinterSettings PrinterSettings
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

	public event PrintEventHandler BeginPrint
	{
		add
		{
			throw new PlatformNotSupportedException();
		}
		remove
		{
			throw new PlatformNotSupportedException();
		}
	}

	public event PrintEventHandler EndPrint
	{
		add
		{
			throw new PlatformNotSupportedException();
		}
		remove
		{
			throw new PlatformNotSupportedException();
		}
	}

	public event PrintPageEventHandler PrintPage
	{
		add
		{
			throw new PlatformNotSupportedException();
		}
		remove
		{
			throw new PlatformNotSupportedException();
		}
	}

	public event QueryPageSettingsEventHandler QueryPageSettings
	{
		add
		{
			throw new PlatformNotSupportedException();
		}
		remove
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PrintDocument()
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void OnBeginPrint(PrintEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void OnEndPrint(PrintEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void OnPrintPage(PrintPageEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void OnQueryPageSettings(QueryPageSettingsEventArgs e)
	{
		throw new PlatformNotSupportedException();
	}

	public void Print()
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
