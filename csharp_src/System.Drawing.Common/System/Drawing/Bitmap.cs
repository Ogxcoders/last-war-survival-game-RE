using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace System.Drawing;

public sealed class Bitmap : Image
{
	public Bitmap(Image original)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(Image original, Size newSize)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(Image original, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(int width, int height, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(int width, int height, PixelFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(int width, int height, int stride, PixelFormat format, IntPtr scan0)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(Stream stream)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(Stream stream, bool useIcm)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(string filename)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(string filename, bool useIcm)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap(Type type, string resource)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap Clone(Rectangle rect, PixelFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap Clone(RectangleF rect, PixelFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public static Bitmap FromHicon(IntPtr hicon)
	{
		throw new PlatformNotSupportedException();
	}

	public static Bitmap FromResource(IntPtr hinstance, string bitmapName)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr GetHbitmap()
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr GetHbitmap(Color background)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public IntPtr GetHicon()
	{
		throw new PlatformNotSupportedException();
	}

	public Color GetPixel(int x, int y)
	{
		throw new PlatformNotSupportedException();
	}

	public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format, BitmapData bitmapData)
	{
		throw new PlatformNotSupportedException();
	}

	public void MakeTransparent()
	{
		throw new PlatformNotSupportedException();
	}

	public void MakeTransparent(Color transparentColor)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetPixel(int x, int y, Color color)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetResolution(float xDpi, float yDpi)
	{
		throw new PlatformNotSupportedException();
	}

	public void UnlockBits(BitmapData bitmapdata)
	{
		throw new PlatformNotSupportedException();
	}
}
