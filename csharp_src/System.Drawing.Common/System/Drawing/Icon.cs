using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace System.Drawing;

public sealed class Icon : MarshalByRefObject, ICloneable, IDisposable, ISerializable
{
	[Browsable(false)]
	public IntPtr Handle
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public int Height
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Size Size
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public int Width
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Icon(Icon original, Size size)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(Icon original, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(Stream stream)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(Stream stream, Size size)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(Stream stream, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(string fileName)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(string fileName, Size size)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(string fileName, int width, int height)
	{
		throw new PlatformNotSupportedException();
	}

	public Icon(Type type, string resource)
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

	public static Icon ExtractAssociatedIcon(string filePath)
	{
		throw new PlatformNotSupportedException();
	}

	~Icon()
	{
		throw new PlatformNotSupportedException();
	}

	public static Icon FromHandle(IntPtr handle)
	{
		throw new PlatformNotSupportedException();
	}

	public void Save(Stream outputStream)
	{
		throw new PlatformNotSupportedException();
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new PlatformNotSupportedException();
	}

	public Bitmap ToBitmap()
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
