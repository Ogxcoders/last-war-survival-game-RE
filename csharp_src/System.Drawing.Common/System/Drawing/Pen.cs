using System.Drawing.Drawing2D;

namespace System.Drawing;

public sealed class Pen : MarshalByRefObject, ICloneable, IDisposable
{
	public PenAlignment Alignment
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

	public Brush Brush
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

	public Color Color
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

	public float[] CompoundArray
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

	public CustomLineCap CustomEndCap
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

	public CustomLineCap CustomStartCap
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

	public DashCap DashCap
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

	public float DashOffset
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

	public float[] DashPattern
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

	public DashStyle DashStyle
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

	public LineCap EndCap
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

	public LineJoin LineJoin
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

	public float MiterLimit
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

	public PenType PenType
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public LineCap StartCap
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

	public float Width
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

	public Pen(Brush brush)
	{
		throw new PlatformNotSupportedException();
	}

	public Pen(Brush brush, float width)
	{
		throw new PlatformNotSupportedException();
	}

	public Pen(Color color)
	{
		throw new PlatformNotSupportedException();
	}

	public Pen(Color color, float width)
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

	~Pen()
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

	public void SetLineCap(LineCap startCap, LineCap endCap, DashCap dashCap)
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
