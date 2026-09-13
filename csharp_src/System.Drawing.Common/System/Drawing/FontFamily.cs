using System.Drawing.Text;

namespace System.Drawing;

public sealed class FontFamily : MarshalByRefObject, IDisposable
{
	public static FontFamily[] Families
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static FontFamily GenericMonospace
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static FontFamily GenericSansSerif
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static FontFamily GenericSerif
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public string Name
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public FontFamily(GenericFontFamilies genericFamily)
	{
		throw new PlatformNotSupportedException();
	}

	public FontFamily(string name)
	{
		throw new PlatformNotSupportedException();
	}

	public FontFamily(string name, FontCollection fontCollection)
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

	~FontFamily()
	{
		throw new PlatformNotSupportedException();
	}

	public int GetCellAscent(FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	public int GetCellDescent(FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	public int GetEmHeight(FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	[Obsolete("Do not use method GetFamilies, use property Families instead")]
	public static FontFamily[] GetFamilies(Graphics graphics)
	{
		throw new PlatformNotSupportedException();
	}

	public override int GetHashCode()
	{
		throw new PlatformNotSupportedException();
	}

	public int GetLineSpacing(FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	public string GetName(int language)
	{
		throw new PlatformNotSupportedException();
	}

	public bool IsStyleAvailable(FontStyle style)
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
