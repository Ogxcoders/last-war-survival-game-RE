namespace System.Drawing.Drawing2D;

public sealed class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
{
	public FillMode FillMode
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

	public PathData PathData
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PointF[] PathPoints
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public byte[] PathTypes
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int PointCount
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public GraphicsPath()
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsPath(FillMode fillMode)
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsPath(PointF[] pts, byte[] types)
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsPath(PointF[] pts, byte[] types, FillMode fillMode)
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsPath(Point[] pts, byte[] types)
	{
		throw new PlatformNotSupportedException();
	}

	public GraphicsPath(Point[] pts, byte[] types, FillMode fillMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddArc(Rectangle rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddArc(RectangleF rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddBeziers(PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddBeziers(params Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddClosedCurve(PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddClosedCurve(PointF[] points, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddClosedCurve(Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddClosedCurve(Point[] points, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddCurve(PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddCurve(PointF[] points, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddCurve(Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddCurve(Point[] points, float tension)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddEllipse(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddEllipse(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddEllipse(int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddEllipse(float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddLine(Point pt1, Point pt2)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddLine(PointF pt1, PointF pt2)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddLine(int x1, int y1, int x2, int y2)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddLine(float x1, float y1, float x2, float y2)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddLines(PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddLines(Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddPath(GraphicsPath addingPath, bool connect)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddPolygon(PointF[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddPolygon(Point[] points)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddRectangle(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddRectangle(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddRectangles(RectangleF[] rects)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddRectangles(Rectangle[] rects)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void ClearMarkers()
	{
		throw new PlatformNotSupportedException();
	}

	public object Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public void CloseAllFigures()
	{
		throw new PlatformNotSupportedException();
	}

	public void CloseFigure()
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	~GraphicsPath()
	{
		throw new PlatformNotSupportedException();
	}

	public void Flatten()
	{
		throw new PlatformNotSupportedException();
	}

	public void Flatten(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Flatten(Matrix matrix, float flatness)
	{
		throw new PlatformNotSupportedException();
	}

	public RectangleF GetBounds()
	{
		throw new PlatformNotSupportedException();
	}

	public RectangleF GetBounds(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public RectangleF GetBounds(Matrix matrix, Pen pen)
	{
		throw new PlatformNotSupportedException();
	}

	public PointF GetLastPoint()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(Point point, Pen pen)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(Point pt, Pen pen, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(PointF point, Pen pen)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(PointF pt, Pen pen, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(int x, int y, Pen pen)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(int x, int y, Pen pen, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(float x, float y, Pen pen)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsOutlineVisible(float x, float y, Pen pen, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Point point)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Point pt, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(PointF point)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(PointF pt, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public void Reset()
	{
		throw new PlatformNotSupportedException();
	}

	public void Reverse()
	{
		throw new PlatformNotSupportedException();
	}

	public void SetMarkers()
	{
		throw new PlatformNotSupportedException();
	}

	public void StartFigure()
	{
		throw new PlatformNotSupportedException();
	}

	public void Transform(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Warp(PointF[] destPoints, RectangleF srcRect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode)
	{
		throw new PlatformNotSupportedException();
	}

	public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode, float flatness)
	{
		throw new PlatformNotSupportedException();
	}

	public void Widen(Pen pen)
	{
		throw new PlatformNotSupportedException();
	}

	public void Widen(Pen pen, Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Widen(Pen pen, Matrix matrix, float flatness)
	{
		throw new PlatformNotSupportedException();
	}
}
