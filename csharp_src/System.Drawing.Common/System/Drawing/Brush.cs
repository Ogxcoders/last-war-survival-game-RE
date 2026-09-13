namespace System.Drawing;

public abstract class Brush : MarshalByRefObject, ICloneable, IDisposable
{
	protected Brush()
	{
		throw new PlatformNotSupportedException();
	}

	public abstract object Clone();

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void Dispose(bool disposing)
	{
		throw new PlatformNotSupportedException();
	}

	~Brush()
	{
		throw new PlatformNotSupportedException();
	}

	protected internal void SetNativeBrush(IntPtr brush)
	{
		throw new PlatformNotSupportedException();
	}
}
