using System.Drawing.Drawing2D;

namespace System.Drawing;

public sealed class Region : MarshalByRefObject, IDisposable
{
	public Region()
	{
		throw new PlatformNotSupportedException();
	}

	public Region(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public Region(RegionData rgnData)
	{
		throw new PlatformNotSupportedException();
	}

	public Region(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public Region(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public Region Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public void Complement(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void Complement(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Complement(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Complement(Region region)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	public bool Equals(Region region, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public void Exclude(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void Exclude(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Exclude(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Exclude(Region region)
	{
		throw new PlatformNotSupportedException();
	}

	~Region()
	{
		throw new PlatformNotSupportedException();
	}

	public static Region FromHrgn(IntPtr hrgn)
	{
		throw new PlatformNotSupportedException();
	}

	public RectangleF GetBounds(Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public IntPtr GetHrgn(Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public RegionData GetRegionData()
	{
		throw new PlatformNotSupportedException();
	}

	public RectangleF[] GetRegionScans(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Intersect(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void Intersect(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Intersect(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Intersect(Region region)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsEmpty(Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsInfinite(Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Point point)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Point point, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(PointF point)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(PointF point, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(Rectangle rect, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(RectangleF rect, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(int x, int y, int width, int height, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y, float width, float height)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsVisible(float x, float y, float width, float height, Graphics g)
	{
		throw new PlatformNotSupportedException();
	}

	public void MakeEmpty()
	{
		throw new PlatformNotSupportedException();
	}

	public void MakeInfinite()
	{
		throw new PlatformNotSupportedException();
	}

	public void ReleaseHrgn(IntPtr regionHandle)
	{
		throw new PlatformNotSupportedException();
	}

	public void Transform(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Translate(int dx, int dy)
	{
		throw new PlatformNotSupportedException();
	}

	public void Translate(float dx, float dy)
	{
		throw new PlatformNotSupportedException();
	}

	public void Union(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void Union(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Union(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Union(Region region)
	{
		throw new PlatformNotSupportedException();
	}

	public void Xor(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public void Xor(Rectangle rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Xor(RectangleF rect)
	{
		throw new PlatformNotSupportedException();
	}

	public void Xor(Region region)
	{
		throw new PlatformNotSupportedException();
	}
}
