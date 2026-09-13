namespace System.Drawing;

public sealed class BufferedGraphics : IDisposable
{
	public Graphics Graphics
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal BufferedGraphics()
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	~BufferedGraphics()
	{
		throw new PlatformNotSupportedException();
	}

	public void Render()
	{
		throw new PlatformNotSupportedException();
	}

	public void Render(Graphics target)
	{
		throw new PlatformNotSupportedException();
	}

	public void Render(IntPtr targetDC)
	{
		throw new PlatformNotSupportedException();
	}
}
