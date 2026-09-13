namespace System.Drawing.Imaging;

public sealed class MetafileHeader
{
	public Rectangle Bounds
	{
		get
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

	public int EmfPlusHeaderSize
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int LogicalDpiX
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int LogicalDpiY
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int MetafileSize
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public MetafileType Type
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int Version
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public MetaHeader WmfHeader
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal MetafileHeader()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsDisplay()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsEmf()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsEmfOrEmfPlus()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsEmfPlus()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsEmfPlusDual()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsEmfPlusOnly()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsWmf()
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsWmfPlaceable()
	{
		throw new PlatformNotSupportedException();
	}
}
