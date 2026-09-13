namespace System.Drawing.Drawing2D;

public sealed class Matrix : MarshalByRefObject, IDisposable
{
	public float[] Elements
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool IsIdentity
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public bool IsInvertible
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public float OffsetX
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public float OffsetY
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Matrix()
	{
		throw new PlatformNotSupportedException();
	}

	public Matrix(Rectangle rect, Point[] plgpts)
	{
		throw new PlatformNotSupportedException();
	}

	public Matrix(RectangleF rect, PointF[] plgpts)
	{
		throw new PlatformNotSupportedException();
	}

	public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
	{
		throw new PlatformNotSupportedException();
	}

	public Matrix Clone()
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	public override bool Equals(object obj)
	{
		throw new PlatformNotSupportedException();
	}

	~Matrix()
	{
		throw new PlatformNotSupportedException();
	}

	public override int GetHashCode()
	{
		throw new PlatformNotSupportedException();
	}

	public void Invert()
	{
		throw new PlatformNotSupportedException();
	}

	public void Multiply(Matrix matrix)
	{
		throw new PlatformNotSupportedException();
	}

	public void Multiply(Matrix matrix, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void Reset()
	{
		throw new PlatformNotSupportedException();
	}

	public void Rotate(float angle)
	{
		throw new PlatformNotSupportedException();
	}

	public void Rotate(float angle, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void RotateAt(float angle, PointF point)
	{
		throw new PlatformNotSupportedException();
	}

	public void RotateAt(float angle, PointF point, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void Scale(float scaleX, float scaleY)
	{
		throw new PlatformNotSupportedException();
	}

	public void Scale(float scaleX, float scaleY, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void Shear(float shearX, float shearY)
	{
		throw new PlatformNotSupportedException();
	}

	public void Shear(float shearX, float shearY, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void TransformPoints(PointF[] pts)
	{
		throw new PlatformNotSupportedException();
	}

	public void TransformPoints(Point[] pts)
	{
		throw new PlatformNotSupportedException();
	}

	public void TransformVectors(PointF[] pts)
	{
		throw new PlatformNotSupportedException();
	}

	public void TransformVectors(Point[] pts)
	{
		throw new PlatformNotSupportedException();
	}

	public void Translate(float offsetX, float offsetY)
	{
		throw new PlatformNotSupportedException();
	}

	public void Translate(float offsetX, float offsetY, MatrixOrder order)
	{
		throw new PlatformNotSupportedException();
	}

	public void VectorTransformPoints(Point[] pts)
	{
		throw new PlatformNotSupportedException();
	}
}
