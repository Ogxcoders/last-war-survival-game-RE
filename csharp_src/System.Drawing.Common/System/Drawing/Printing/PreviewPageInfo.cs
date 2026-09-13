namespace System.Drawing.Printing;

public sealed class PreviewPageInfo
{
	public Image Image
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Size PhysicalSize
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PreviewPageInfo(Image image, Size physicalSize)
	{
		throw new PlatformNotSupportedException();
	}
}
