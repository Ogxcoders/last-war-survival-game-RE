using System.Drawing.Text;

namespace System.Drawing;

public sealed class StringFormat : MarshalByRefObject, ICloneable, IDisposable
{
	public StringAlignment Alignment
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

	public int DigitSubstitutionLanguage
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public StringDigitSubstitute DigitSubstitutionMethod
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public StringFormatFlags FormatFlags
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

	public static StringFormat GenericDefault
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public static StringFormat GenericTypographic
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public HotkeyPrefix HotkeyPrefix
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

	public StringAlignment LineAlignment
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

	public StringTrimming Trimming
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

	public StringFormat()
	{
		throw new PlatformNotSupportedException();
	}

	public StringFormat(StringFormat format)
	{
		throw new PlatformNotSupportedException();
	}

	public StringFormat(StringFormatFlags options)
	{
		throw new PlatformNotSupportedException();
	}

	public StringFormat(StringFormatFlags options, int language)
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

	~StringFormat()
	{
		throw new PlatformNotSupportedException();
	}

	public float[] GetTabStops(out float firstTabOffset)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetDigitSubstitution(int language, StringDigitSubstitute substitute)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetMeasurableCharacterRanges(CharacterRange[] ranges)
	{
		throw new PlatformNotSupportedException();
	}

	public void SetTabStops(float firstTabOffset, float[] tabStops)
	{
		throw new PlatformNotSupportedException();
	}

	public override string ToString()
	{
		throw new PlatformNotSupportedException();
	}
}
