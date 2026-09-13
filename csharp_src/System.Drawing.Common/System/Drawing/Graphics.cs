using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace System.Drawing;

public sealed class Graphics : MarshalByRefObject, IDeviceContext, IDisposable
{
	public delegate bool DrawImageAbort(IntPtr callbackdata);

	public delegate bool EnumerateMetafileProc(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData);

	public Region Clip
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

	public RectangleF ClipBounds
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public CompositingMode CompositingMode
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

	public CompositingQuality CompositingQuality
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

	public float DpiX
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public float DpiY
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public InterpolationMode InterpolationMode
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

	public bool IsClipEmpty
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool IsVisibleClipEmpty
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public float PageScale
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

	public GraphicsUnit PageUnit
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

	public PixelOffsetMode PixelOffsetMode
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

	public Point RenderingOrigin
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

	public SmoothingMode SmoothingMode
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

	public int TextContrast
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

	public TextRenderingHint TextRenderingHint
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

	public RectangleF VisibleClipBounds
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal Graphics()
	{
		throw new PlatformNotSupportedException();
	}

	public void AddMetafileComment(byte[] data)
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsContainer BeginContainer()
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
	{
		throw new PlatformNotSupportedException();
	}

	public void Clear(Color color)
	{
		throw new PlatformNotSupportedException();
	}

	public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
	{
		throw new PlatformNotSupportedException();
	}

	public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
	{
		throw new PlatformNotSupportedException();
	}

	public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
	{
		throw new PlatformNotSupportedException();
	}

	public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawBeziers(Pen pen, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawBeziers(Pen pen, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawClosedCurve(Pen pen, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawClosedCurve(Pen pen, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, PointF[] points, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawCurve(Pen pen, Point[] points, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawEllipse(Pen pen, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawEllipse(Pen pen, RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawEllipse(Pen pen, int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawEllipse(Pen pen, float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawIcon(Icon icon, Rectangle targetRect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawIcon(Icon icon, int x, int y)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Point point)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, PointF point)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, PointF[] destPoints)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Point[] destPoints)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, int x, int y)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, float x, float y)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImage(Image image, float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImageUnscaled(Image image, Point point)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImageUnscaled(Image image, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImageUnscaled(Image image, int x, int y)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawLine(Pen pen, Point pt1, Point pt2)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawLine(Pen pen, PointF pt1, PointF pt2)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawLines(Pen pen, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawLines(Pen pen, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPath(Pen pen, GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPolygon(Pen pen, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawPolygon(Pen pen, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawRectangle(Pen pen, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawRectangle(Pen pen, int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawRectangle(Pen pen, float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawRectangles(Pen pen, RectangleF[] rects)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawRectangles(Pen pen, Rectangle[] rects)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawString(string s, Font font, Brush brush, PointF point)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawString(string s, Font font, Brush brush, float x, float y)
	{
		throw new PlatformNotSupportedException();
	}

	public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void EndContainer(GraphicsContainer container)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
	{
		throw new PlatformNotSupportedException();
	}

	public void ExcludeClip(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void ExcludeClip(Region region)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillClosedCurve(Brush brush, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillClosedCurve(Brush brush, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillEllipse(Brush brush, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillEllipse(Brush brush, RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillEllipse(Brush brush, int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillEllipse(Brush brush, float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPath(Brush brush, GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPolygon(Brush brush, PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPolygon(Brush brush, Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRectangle(Brush brush, Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRectangle(Brush brush, RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRectangle(Brush brush, int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRectangle(Brush brush, float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRectangles(Brush brush, RectangleF[] rects)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRectangles(Brush brush, Rectangle[] rects)
	{
		throw new PlatformNotSupportedException();
	}

	public void FillRegion(Brush brush, Region region)
	{
		throw new PlatformNotSupportedException();
	}

	~Graphics()
	{
		throw new PlatformNotSupportedException();
	}

	public void Flush()
	{
		throw new PlatformNotSupportedException();
	}

	public void Flush(FlushIntention intention)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Graphics FromHdc(IntPtr hdc)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Graphics FromHdc(IntPtr hdc, IntPtr hdevice)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Graphics FromHdcInternal(IntPtr hdc)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Graphics FromHwnd(IntPtr hwnd)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static Graphics FromHwndInternal(IntPtr hwnd)
	{
		throw new PlatformNotSupportedException();
	}

	public static Graphics FromImage(Image image)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public object GetContextInfo()
	{
		throw new PlatformNotSupportedException();
	}

	public static IntPtr GetHalftonePalette()
	{
		throw new PlatformNotSupportedException();
	}

	public IntPtr GetHdc()
	{
		throw new PlatformNotSupportedException();
	}

	public Color GetNearestColor(Color color)
	{
		throw new PlatformNotSupportedException();
	}

	public void IntersectClip(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void IntersectClip(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void IntersectClip(Region region)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Point point)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(PointF point)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font, SizeF layoutArea)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font, int width)
	{
		throw new PlatformNotSupportedException();
	}

	public SizeF MeasureString(string text, Font font, int width, StringFormat format)
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

	public void ReleaseHdc()
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public void ReleaseHdc(IntPtr hdc)
	{
		throw new PlatformNotSupportedException();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ReleaseHdcInternal(IntPtr hdc)
	{
		throw new PlatformNotSupportedException();
	}

	public void ResetClip()
	{
		throw new PlatformNotSupportedException();
	}

	public void ResetTransform()
	{
		throw new PlatformNotSupportedException();
	}

	public void Restore(GraphicsState gstate)
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

	public GraphicsState Save()
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

	public void SetClip(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(GraphicsPath path, CombineMode combineMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(Graphics g, CombineMode combineMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(Rectangle rect, CombineMode combineMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(RectangleF rect, CombineMode combineMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetClip(Region region, CombineMode combineMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
	{
		throw new PlatformNotSupportedException();
	}

	public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
	{
		throw new PlatformNotSupportedException();
	}

	public void TranslateClip(int dx, int dy)
	{
		throw new PlatformNotSupportedException();
	}

	public void TranslateClip(float dx, float dy)
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
