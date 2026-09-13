namespace System.Drawing.Text;

public sealed class PrivateFontCollection : FontCollection
{
	public PrivateFontCollection()
	{
		throw new PlatformNotSupportedException();
	}

	public void AddFontFile(string filename)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddMemoryFont(IntPtr memory, int length)
	{
		throw new PlatformNotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		throw new PlatformNotSupportedException();
	}
}
