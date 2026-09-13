using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace System.Drawing;

public sealed class TextureBrush : Brush
{
	public Image Image
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Matrix Transform
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

	public WrapMode WrapMode
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

	public TextureBrush(Image bitmap)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, WrapMode wrapMode)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, WrapMode wrapMode, Rectangle dstRect)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, WrapMode wrapMode, RectangleF dstRect)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, Rectangle dstRect)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, Rectangle dstRect, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, RectangleF dstRect)
	{
		throw new PlatformNotSupportedException();
	}

	public TextureBrush(Image image, RectangleF dstRect, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public override object Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public void MultiplyTransform(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void MultiplyTransform(Matrix matrix, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void ResetTransform()
	{
		throw new PlatformNotSupportedException();
	}

	public void RotateTransform(float angle)
	{
		throw new PlatformNotSupportedException();
	}

	public void RotateTransform(float angle, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void ScaleTransform(float sx, float sy)
	{
		throw new PlatformNotSupportedException();
	}

	public void ScaleTransform(float sx, float sy, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void TranslateTransform(float dx, float dy)
	{
		throw new PlatformNotSupportedException();
	}

	public void TranslateTransform(float dx, float dy, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}
}
