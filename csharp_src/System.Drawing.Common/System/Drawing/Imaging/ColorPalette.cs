namespace System.Drawing.Imaging;

public sealed class ColorPalette
{
	public Color[] Entries
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int Flags
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal ColorPalette()
	{
		throw new PlatformNotSupportedException();
	}
}
