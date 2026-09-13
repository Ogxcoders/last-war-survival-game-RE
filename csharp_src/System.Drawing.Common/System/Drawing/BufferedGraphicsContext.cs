namespace System.Drawing;

public sealed class BufferedGraphicsContext : IDisposable
{
	public Size MaximumBuffer
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

	public BufferedGraphicsContext()
	{
		throw new PlatformNotSupportedException();
	}

	public BufferedGraphics Allocate(Graphics targetGraphics, Rectangle targetRectangle)
	{
		throw new PlatformNotSupportedException();
	}

	public BufferedGraphics Allocate(IntPtr targetDC, Rectangle targetRectangle)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	~BufferedGraphicsContext()
	{
		throw new PlatformNotSupportedException();
	}

	public void Invalidate()
	{
		throw new PlatformNotSupportedException();
	}
}
