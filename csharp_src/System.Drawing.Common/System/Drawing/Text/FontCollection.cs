namespace System.Drawing.Text;

public abstract class FontCollection : IDisposable
{
	public FontFamily[] Families
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal FontCollection()
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void Dispose(bool disposing)
	{
		throw new PlatformNotSupportedException();
	}

	~FontCollection()
	{
		throw new PlatformNotSupportedException();
	}
}
