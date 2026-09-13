namespace System.Drawing.Drawing2D;

public sealed class HatchBrush : Brush
{
	public Color BackgroundColor
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Color ForegroundColor
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public HatchStyle HatchStyle
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public HatchBrush(HatchStyle hatchstyle, Color foreColor)
	{
		throw new PlatformNotSupportedException();
	}

	public HatchBrush(HatchStyle hatchstyle, Color foreColor, Color backColor)
	{
		throw new PlatformNotSupportedException();
	}

	public override object Clone()
	{
		throw new PlatformNotSupportedException();
	}
}
