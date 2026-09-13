namespace System.Drawing;

public sealed class SolidBrush : Brush
{
	public Color Color
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

	public SolidBrush(Color color)
	{
		throw new PlatformNotSupportedException();
	}

	public override object Clone()
	{
		throw new PlatformNotSupportedException();
	}

	protected override void Dispose(bool disposing)
	{
		throw new PlatformNotSupportedException();
	}
}
