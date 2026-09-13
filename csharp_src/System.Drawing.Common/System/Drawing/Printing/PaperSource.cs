namespace System.Drawing.Printing;

public class PaperSource
{
	public PaperSourceKind Kind
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int RawKind
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

	public string SourceName
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

	public PaperSource()
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
