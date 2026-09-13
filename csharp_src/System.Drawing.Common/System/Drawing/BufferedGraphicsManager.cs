namespace System.Drawing;

public static class BufferedGraphicsManager
{
	public static BufferedGraphicsContext Current
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}
}
