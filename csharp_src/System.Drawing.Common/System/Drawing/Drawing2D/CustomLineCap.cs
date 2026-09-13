namespace System.Drawing.Drawing2D;

public class CustomLineCap : MarshalByRefObject, ICloneable, IDisposable
{
	public LineCap BaseCap
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

	public float BaseInset
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

	public LineJoin StrokeJoin
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

	public float WidthScale
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

	public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath)
	{
		throw new PlatformNotSupportedException();
	}

	public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap)
	{
		throw new PlatformNotSupportedException();
	}

	public CustomLineCap(GraphicsPath fillPath, GraphicsPath strokePath, LineCap baseCap, float baseInset)
	{
		throw new PlatformNotSupportedException();
	}

	public object Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	protected virtual void Dispose(bool disposing)
	{
		throw new PlatformNotSupportedException();
	}

	~CustomLineCap()
	{
		throw new PlatformNotSupportedException();
	}

	public void GetStrokeCaps(out LineCap startCap, out LineCap endCap)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetStrokeCaps(LineCap startCap, LineCap endCap)
	{
		throw new PlatformNotSupportedException();
	}
}
