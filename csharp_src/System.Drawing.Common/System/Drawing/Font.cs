using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Drawing;

public sealed class Font : MarshalByRefObject, ICloneable, IDisposable, ISerializable
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Bold
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public FontFamily FontFamily
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public byte GdiCharSet
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool GdiVerticalFont
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

	[Browsable(false)]
	public bool IsSystemFont
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Italic
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string Name
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public string OriginalFontName
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public float Size
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public float SizeInPoints
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Strikeout
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public FontStyle Style
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[Browsable(false)]
	public string SystemFontName
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Underline
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public GraphicsUnit Unit
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public Font(Font prototype, FontStyle newStyle)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(FontFamily family, float emSize)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(FontFamily family, float emSize, FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(FontFamily family, float emSize, GraphicsUnit unit)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(string familyName, float emSize)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(string familyName, float emSize, FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
	{
		throw new PlatformNotSupportedException();
	}

	public Font(string familyName, float emSize, GraphicsUnit unit)
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

	public override bool Equals(object obj)
	{
		throw new PlatformNotSupportedException();
	}

	~Font()
	{
		throw new PlatformNotSupportedException();
	}

	public static Font FromHdc(IntPtr hdc)
	{
		throw new PlatformNotSupportedException();
	}

	public static Font FromHfont(IntPtr hfont)
	{
		throw new PlatformNotSupportedException();
	}

	public static Font FromLogFont(object lf)
	{
		throw new PlatformNotSupportedException();
	}

	public static Font FromLogFont(object lf, IntPtr hdc)
	{
		throw new PlatformNotSupportedException();
	}

	public override int GetHashCode()
	{
		throw new PlatformNotSupportedException();
	}

	public float GetHeight()
	{
		throw new PlatformNotSupportedException();
	}

	public float GetHeight(Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public float GetHeight(float dpi)
	{
		throw new PlatformNotSupportedException();
	}

	void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
	{
		throw new PlatformNotSupportedException();
	}

	public IntPtr ToHfont()
	{
		throw new PlatformNotSupportedException();
	}

	public void ToLogFont(object logFont)
	{
		throw new PlatformNotSupportedException();
	}

	public void ToLogFont(object logFont, Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
