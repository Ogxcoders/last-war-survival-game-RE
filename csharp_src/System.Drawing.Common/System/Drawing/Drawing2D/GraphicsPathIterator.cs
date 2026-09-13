namespace System.Drawing.Drawing2D;

public sealed class GraphicsPathIterator : MarshalByRefObject, IDisposable
{
	public int Count
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int SubpathCount
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public GraphicsPathIterator(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public int CopyData(ref PointF[] points, ref byte[] types, int startIndex, int endIndex)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	public int Enumerate(ref PointF[] points, ref byte[] types)
	{
		throw new PlatformNotSupportedException();
	}

	~GraphicsPathIterator()
	{
		throw new PlatformNotSupportedException();
	}

	public bool HasCurve()
	{
		throw new PlatformNotSupportedException();
	}

	public int NextMarker(GraphicsPath path)
	{
		throw new PlatformNotSupportedException();
	}

	public int NextMarker(out int startIndex, out int endIndex)
	{
		throw new PlatformNotSupportedException();
	}

	public int NextPathType(out byte pathType, out int startIndex, out int endIndex)
	{
		throw new PlatformNotSupportedException();
	}

	public int NextSubpath(GraphicsPath path, out bool isClosed)
	{
		throw new PlatformNotSupportedException();
	}

	public int NextSubpath(out int startIndex, out int endIndex, out bool isClosed)
	{
		throw new PlatformNotSupportedException();
	}

	public void Rewind()
	{
		throw new PlatformNotSupportedException();
	}
}
