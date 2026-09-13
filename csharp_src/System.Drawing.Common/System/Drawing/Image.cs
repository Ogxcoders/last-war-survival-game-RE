using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;

namespace System.Drawing;

[ImmutableObject(true)]
public abstract class Image : MarshalByRefObject, ICloneable, IDisposable, ISerializable
{
	public delegate bool GetThumbnailImageAbort();

	[Browsable(false)]
	public int Flags
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public Guid[] FrameDimensionsList
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Height
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public float HorizontalResolution
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public ColorPalette Palette
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

	public SizeF PhysicalDimension
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public PixelFormat PixelFormat
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public int[] PropertyIdList
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public PropertyItem[] PropertyItems
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public ImageFormat RawFormat
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

	[DefaultValue(null)]
	[Localizable(false)]
	public object Tag
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

	public float VerticalResolution
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int Width
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal Image()
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

	~Image()
	{
		throw new PlatformNotSupportedException();
	}

	public static Image FromFile(string filename)
	{
		throw new PlatformNotSupportedException();
	}

	public static Image FromFile(string filename, bool useEmbeddedColorManagement)
	{
		throw new PlatformNotSupportedException();
	}

	public static Bitmap FromHbitmap(IntPtr hbitmap)
	{
		throw new PlatformNotSupportedException();
	}

	public static Bitmap FromHbitmap(IntPtr hbitmap, IntPtr hpalette)
	{
		throw new PlatformNotSupportedException();
	}

	public static Image FromStream(Stream stream)
	{
		throw new PlatformNotSupportedException();
	}

	public static Image FromStream(Stream stream, bool useEmbeddedColorManagement)
	{
		throw new PlatformNotSupportedException();
	}

	public static Image FromStream(Stream stream, bool useEmbeddedColorManagement, bool validateImageData)
	{
		throw new PlatformNotSupportedException();
	}

	public RectangleF GetBounds(ref GraphicsUnit pageUnit)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameters GetEncoderParameterList(Guid encoder)
	{
		throw new PlatformNotSupportedException();
	}

	public int GetFrameCount(FrameDimension dimension)
	{
		throw new PlatformNotSupportedException();
	}

	public static int GetPixelFormatSize(PixelFormat pixfmt)
	{
		throw new PlatformNotSupportedException();
	}

	public PropertyItem GetPropertyItem(int propid)
	{
		throw new PlatformNotSupportedException();
	}

	public Image GetThumbnailImage(int thumbWidth, int thumbHeight, GetThumbnailImageAbort callback, IntPtr callbackData)
	{
		throw new PlatformNotSupportedException();
	}

	public static bool IsAlphaPixelFormat(PixelFormat pixfmt)
	{
		throw new PlatformNotSupportedException();
	}

	public static bool IsCanonicalPixelFormat(PixelFormat pixfmt)
	{
		throw new PlatformNotSupportedException();
	}

	public static bool IsExtendedPixelFormat(PixelFormat pixfmt)
	{
		throw new PlatformNotSupportedException();
	}

	public void RemovePropertyItem(int propid)
	{
		throw new PlatformNotSupportedException();
	}

	public void RotateFlip(RotateFlipType rotateFlipType)
	{
		throw new PlatformNotSupportedException();
	}

	public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
	{
		throw new PlatformNotSupportedException();
	}

	public void Save(Stream stream, ImageFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void Save(string filename)
	{
		throw new PlatformNotSupportedException();
	}

	public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
	{
		throw new PlatformNotSupportedException();
	}

	public void Save(string filename, ImageFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public void SaveAdd(Image image, EncoderParameters encoderParams)
	{
		throw new PlatformNotSupportedException();
	}

	public void SaveAdd(EncoderParameters encoderParams)
	{
		throw new PlatformNotSupportedException();
	}

	public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetPropertyItem(PropertyItem propitem)
	{
		throw new PlatformNotSupportedException();
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new PlatformNotSupportedException();
	}
}
