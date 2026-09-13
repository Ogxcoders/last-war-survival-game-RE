namespace System.Drawing.Printing;

public class QueryPageSettingsEventArgs : PrintEventArgs
{
	public PageSettings PageSettings
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

	public QueryPageSettingsEventArgs(PageSettings pageSettings)
	{
		throw new PlatformNotSupportedException();
	}
}
