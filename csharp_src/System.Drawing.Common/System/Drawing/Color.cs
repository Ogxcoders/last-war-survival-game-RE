using System.ComponentModel;
using System.Diagnostics;
using System.Numerics.Hashing;
using System.Runtime.CompilerServices;

namespace System.Drawing;

[Serializable]
[DebuggerDisplay("{NameAndARGBValue}")]
[TypeForwardedFrom("Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065")]
[TypeConverter(typeof(ColorConverter))]
public readonly struct Color : IEquatable<Color>
{
	public static readonly Color Empty;

	private const short StateKnownColorValid = 1;

	private const short StateARGBValueValid = 2;

	private const short StateValueMask = 2;

	private const short StateNameValid = 8;

	private const long NotDefinedValue = 0L;

	private const int ARGBAlphaShift = 24;

	private const int ARGBRedShift = 16;

	private const int ARGBGreenShift = 8;

	private const int ARGBBlueShift = 0;

	private readonly string name;

	private readonly long value;

	private readonly short knownColor;

	private readonly short state;

	public static Color Transparent => new Color(KnownColor.Transparent);

	public static Color AliceBlue => new Color(KnownColor.AliceBlue);

	public static Color AntiqueWhite => new Color(KnownColor.AntiqueWhite);

	public static Color Aqua => new Color(KnownColor.Aqua);

	public static Color Aquamarine => new Color(KnownColor.Aquamarine);

	public static Color Azure => new Color(KnownColor.Azure);

	public static Color Beige => new Color(KnownColor.Beige);

	public static Color Bisque => new Color(KnownColor.Bisque);

	public static Color Black => new Color(KnownColor.Black);

	public static Color BlanchedAlmond => new Color(KnownColor.BlanchedAlmond);

	public static Color Blue => new Color(KnownColor.Blue);

	public static Color BlueViolet => new Color(KnownColor.BlueViolet);

	public static Color Brown => new Color(KnownColor.Brown);

	public static Color BurlyWood => new Color(KnownColor.BurlyWood);

	public static Color CadetBlue => new Color(KnownColor.CadetBlue);

	public static Color Chartreuse => new Color(KnownColor.Chartreuse);

	public static Color Chocolate => new Color(KnownColor.Chocolate);

	public static Color Coral => new Color(KnownColor.Coral);

	public static Color CornflowerBlue => new Color(KnownColor.CornflowerBlue);

	public static Color Cornsilk => new Color(KnownColor.Cornsilk);

	public static Color Crimson => new Color(KnownColor.Crimson);

	public static Color Cyan => new Color(KnownColor.Cyan);

	public static Color DarkBlue => new Color(KnownColor.DarkBlue);

	public static Color DarkCyan => new Color(KnownColor.DarkCyan);

	public static Color DarkGoldenrod => new Color(KnownColor.DarkGoldenrod);

	public static Color DarkGray => new Color(KnownColor.DarkGray);

	public static Color DarkGreen => new Color(KnownColor.DarkGreen);

	public static Color DarkKhaki => new Color(KnownColor.DarkKhaki);

	public static Color DarkMagenta => new Color(KnownColor.DarkMagenta);

	public static Color DarkOliveGreen => new Color(KnownColor.DarkOliveGreen);

	public static Color DarkOrange => new Color(KnownColor.DarkOrange);

	public static Color DarkOrchid => new Color(KnownColor.DarkOrchid);

	public static Color DarkRed => new Color(KnownColor.DarkRed);

	public static Color DarkSalmon => new Color(KnownColor.DarkSalmon);

	public static Color DarkSeaGreen => new Color(KnownColor.DarkSeaGreen);

	public static Color DarkSlateBlue => new Color(KnownColor.DarkSlateBlue);

	public static Color DarkSlateGray => new Color(KnownColor.DarkSlateGray);

	public static Color DarkTurquoise => new Color(KnownColor.DarkTurquoise);

	public static Color DarkViolet => new Color(KnownColor.DarkViolet);

	public static Color DeepPink => new Color(KnownColor.DeepPink);

	public static Color DeepSkyBlue => new Color(KnownColor.DeepSkyBlue);

	public static Color DimGray => new Color(KnownColor.DimGray);

	public static Color DodgerBlue => new Color(KnownColor.DodgerBlue);

	public static Color Firebrick => new Color(KnownColor.Firebrick);

	public static Color FloralWhite => new Color(KnownColor.FloralWhite);

	public static Color ForestGreen => new Color(KnownColor.ForestGreen);

	public static Color Fuchsia => new Color(KnownColor.Fuchsia);

	public static Color Gainsboro => new Color(KnownColor.Gainsboro);

	public static Color GhostWhite => new Color(KnownColor.GhostWhite);

	public static Color Gold => new Color(KnownColor.Gold);

	public static Color Goldenrod => new Color(KnownColor.Goldenrod);

	public static Color Gray => new Color(KnownColor.Gray);

	public static Color Green => new Color(KnownColor.Green);

	public static Color GreenYellow => new Color(KnownColor.GreenYellow);

	public static Color Honeydew => new Color(KnownColor.Honeydew);

	public static Color HotPink => new Color(KnownColor.HotPink);

	public static Color IndianRed => new Color(KnownColor.IndianRed);

	public static Color Indigo => new Color(KnownColor.Indigo);

	public static Color Ivory => new Color(KnownColor.Ivory);

	public static Color Khaki => new Color(KnownColor.Khaki);

	public static Color Lavender => new Color(KnownColor.Lavender);

	public static Color LavenderBlush => new Color(KnownColor.LavenderBlush);

	public static Color LawnGreen => new Color(KnownColor.LawnGreen);

	public static Color LemonChiffon => new Color(KnownColor.LemonChiffon);

	public static Color LightBlue => new Color(KnownColor.LightBlue);

	public static Color LightCoral => new Color(KnownColor.LightCoral);

	public static Color LightCyan => new Color(KnownColor.LightCyan);

	public static Color LightGoldenrodYellow => new Color(KnownColor.LightGoldenrodYellow);

	public static Color LightGreen => new Color(KnownColor.LightGreen);

	public static Color LightGray => new Color(KnownColor.LightGray);

	public static Color LightPink => new Color(KnownColor.LightPink);

	public static Color LightSalmon => new Color(KnownColor.LightSalmon);

	public static Color LightSeaGreen => new Color(KnownColor.LightSeaGreen);

	public static Color LightSkyBlue => new Color(KnownColor.LightSkyBlue);

	public static Color LightSlateGray => new Color(KnownColor.LightSlateGray);

	public static Color LightSteelBlue => new Color(KnownColor.LightSteelBlue);

	public static Color LightYellow => new Color(KnownColor.LightYellow);

	public static Color Lime => new Color(KnownColor.Lime);

	public static Color LimeGreen => new Color(KnownColor.LimeGreen);

	public static Color Linen => new Color(KnownColor.Linen);

	public static Color Magenta => new Color(KnownColor.Magenta);

	public static Color Maroon => new Color(KnownColor.Maroon);

	public static Color MediumAquamarine => new Color(KnownColor.MediumAquamarine);

	public static Color MediumBlue => new Color(KnownColor.MediumBlue);

	public static Color MediumOrchid => new Color(KnownColor.MediumOrchid);

	public static Color MediumPurple => new Color(KnownColor.MediumPurple);

	public static Color MediumSeaGreen => new Color(KnownColor.MediumSeaGreen);

	public static Color MediumSlateBlue => new Color(KnownColor.MediumSlateBlue);

	public static Color MediumSpringGreen => new Color(KnownColor.MediumSpringGreen);

	public static Color MediumTurquoise => new Color(KnownColor.MediumTurquoise);

	public static Color MediumVioletRed => new Color(KnownColor.MediumVioletRed);

	public static Color MidnightBlue => new Color(KnownColor.MidnightBlue);

	public static Color MintCream => new Color(KnownColor.MintCream);

	public static Color MistyRose => new Color(KnownColor.MistyRose);

	public static Color Moccasin => new Color(KnownColor.Moccasin);

	public static Color NavajoWhite => new Color(KnownColor.NavajoWhite);

	public static Color Navy => new Color(KnownColor.Navy);

	public static Color OldLace => new Color(KnownColor.OldLace);

	public static Color Olive => new Color(KnownColor.Olive);

	public static Color OliveDrab => new Color(KnownColor.OliveDrab);

	public static Color Orange => new Color(KnownColor.Orange);

	public static Color OrangeRed => new Color(KnownColor.OrangeRed);

	public static Color Orchid => new Color(KnownColor.Orchid);

	public static Color PaleGoldenrod => new Color(KnownColor.PaleGoldenrod);

	public static Color PaleGreen => new Color(KnownColor.PaleGreen);

	public static Color PaleTurquoise => new Color(KnownColor.PaleTurquoise);

	public static Color PaleVioletRed => new Color(KnownColor.PaleVioletRed);

	public static Color PapayaWhip => new Color(KnownColor.PapayaWhip);

	public static Color PeachPuff => new Color(KnownColor.PeachPuff);

	public static Color Peru => new Color(KnownColor.Peru);

	public static Color Pink => new Color(KnownColor.Pink);

	public static Color Plum => new Color(KnownColor.Plum);

	public static Color PowderBlue => new Color(KnownColor.PowderBlue);

	public static Color Purple => new Color(KnownColor.Purple);

	public static Color Red => new Color(KnownColor.Red);

	public static Color RosyBrown => new Color(KnownColor.RosyBrown);

	public static Color RoyalBlue => new Color(KnownColor.RoyalBlue);

	public static Color SaddleBrown => new Color(KnownColor.SaddleBrown);

	public static Color Salmon => new Color(KnownColor.Salmon);

	public static Color SandyBrown => new Color(KnownColor.SandyBrown);

	public static Color SeaGreen => new Color(KnownColor.SeaGreen);

	public static Color SeaShell => new Color(KnownColor.SeaShell);

	public static Color Sienna => new Color(KnownColor.Sienna);

	public static Color Silver => new Color(KnownColor.Silver);

	public static Color SkyBlue => new Color(KnownColor.SkyBlue);

	public static Color SlateBlue => new Color(KnownColor.SlateBlue);

	public static Color SlateGray => new Color(KnownColor.SlateGray);

	public static Color Snow => new Color(KnownColor.Snow);

	public static Color SpringGreen => new Color(KnownColor.SpringGreen);

	public static Color SteelBlue => new Color(KnownColor.SteelBlue);

	public static Color Tan => new Color(KnownColor.Tan);

	public static Color Teal => new Color(KnownColor.Teal);

	public static Color Thistle => new Color(KnownColor.Thistle);

	public static Color Tomato => new Color(KnownColor.Tomato);

	public static Color Turquoise => new Color(KnownColor.Turquoise);

	public static Color Violet => new Color(KnownColor.Violet);

	public static Color Wheat => new Color(KnownColor.Wheat);

	public static Color White => new Color(KnownColor.White);

	public static Color WhiteSmoke => new Color(KnownColor.WhiteSmoke);

	public static Color Yellow => new Color(KnownColor.Yellow);

	public static Color YellowGreen => new Color(KnownColor.YellowGreen);

	public byte R => (byte)((Value >> 16) & 0xFF);

	public byte G => (byte)((Value >> 8) & 0xFF);

	public byte B => (byte)(Value & 0xFF);

	public byte A => (byte)((Value >> 24) & 0xFF);

	public bool IsKnownColor => (state & 1) != 0;

	public bool IsEmpty => state == 0;

	public bool IsNamedColor
	{
		get
		{
			if ((state & 8) == 0)
			{
				return IsKnownColor;
			}
			return true;
		}
	}

	public bool IsSystemColor
	{
		get
		{
			if (IsKnownColor)
			{
				if (knownColor > 26)
				{
					return knownColor > 167;
				}
				return true;
			}
			return false;
		}
	}

	private string NameAndARGBValue => $"{{Name={Name}, ARGB=({A}, {R}, {G}, {B})}}";

	public string Name
	{
		get
		{
			if ((state & 8) != 0)
			{
				return name;
			}
			if (IsKnownColor)
			{
				return KnownColorTable.KnownColorToName((KnownColor)knownColor);
			}
			return Convert.ToString(value, 16);
		}
	}

	private long Value
	{
		get
		{
			if ((state & 2) != 0)
			{
				return value;
			}
			if (IsKnownColor)
			{
				return KnownColorTable.KnownColorToArgb((KnownColor)knownColor);
			}
			return 0L;
		}
	}

	internal Color(KnownColor knownColor)
	{
		value = 0L;
		state = 1;
		name = null;
		this.knownColor = (short)knownColor;
	}

	private Color(long value, short state, string name, KnownColor knownColor)
	{
		this.value = value;
		this.state = state;
		this.name = name;
		this.knownColor = (short)knownColor;
	}

	private static void CheckByte(int value, string name)
	{
		if (value < 0 || value > 255)
		{
			throw new ArgumentException(global::SR.Format("Value of '{1}' is not valid for '{0}'. '{0}' should be greater than or equal to {2} and less than or equal to {3}.", name, value, 0, 255));
		}
	}

	private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
	{
		return (long)(uint)((red << 16) | (green << 8) | blue | (alpha << 24)) & 0xFFFFFFFFL;
	}

	public static Color FromArgb(int argb)
	{
		return new Color(argb & 0xFFFFFFFFu, 2, null, (KnownColor)0);
	}

	public static Color FromArgb(int alpha, int red, int green, int blue)
	{
		CheckByte(alpha, "alpha");
		CheckByte(red, "red");
		CheckByte(green, "green");
		CheckByte(blue, "blue");
		return new Color(MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue), 2, null, (KnownColor)0);
	}

	public static Color FromArgb(int alpha, Color baseColor)
	{
		CheckByte(alpha, "alpha");
		return new Color(MakeArgb((byte)alpha, baseColor.R, baseColor.G, baseColor.B), 2, null, (KnownColor)0);
	}

	public static Color FromArgb(int red, int green, int blue)
	{
		return FromArgb(255, red, green, blue);
	}

	public static Color FromKnownColor(KnownColor color)
	{
		if (color > (KnownColor)0 && color <= KnownColor.MenuHighlight)
		{
			return new Color(color);
		}
		return FromName(color.ToString());
	}

	public static Color FromName(string name)
	{
		if (ColorTable.TryGetNamedColor(name, out var result))
		{
			return result;
		}
		return new Color(0L, 8, name, (KnownColor)0);
	}

	public float GetBrightness()
	{
		float num = (float)(int)R / 255f;
		float num2 = (float)(int)G / 255f;
		float num3 = (float)(int)B / 255f;
		float num4 = num;
		float num5 = num;
		if (num2 > num4)
		{
			num4 = num2;
		}
		else if (num2 < num5)
		{
			num5 = num2;
		}
		if (num3 > num4)
		{
			num4 = num3;
		}
		else if (num3 < num5)
		{
			num5 = num3;
		}
		return (num4 + num5) / 2f;
	}

	public float GetHue()
	{
		if (R == G && G == B)
		{
			return 0f;
		}
		float num = (float)(int)R / 255f;
		float num2 = (float)(int)G / 255f;
		float num3 = (float)(int)B / 255f;
		float num4 = num;
		float num5 = num;
		if (num2 > num4)
		{
			num4 = num2;
		}
		else if (num2 < num5)
		{
			num5 = num2;
		}
		if (num3 > num4)
		{
			num4 = num3;
		}
		else if (num3 < num5)
		{
			num5 = num3;
		}
		float num6 = num4 - num5;
		float num7 = ((num == num4) ? ((num2 - num3) / num6) : ((num2 != num4) ? (4f + (num - num2) / num6) : (2f + (num3 - num) / num6)));
		num7 *= 60f;
		if (num7 < 0f)
		{
			num7 += 360f;
		}
		return num7;
	}

	public float GetSaturation()
	{
		float num = (float)(int)R / 255f;
		float num2 = (float)(int)G / 255f;
		float num3 = (float)(int)B / 255f;
		float result = 0f;
		float num4 = num;
		float num5 = num;
		if (num2 > num4)
		{
			num4 = num2;
		}
		else if (num2 < num5)
		{
			num5 = num2;
		}
		if (num3 > num4)
		{
			num4 = num3;
		}
		else if (num3 < num5)
		{
			num5 = num3;
		}
		if (num4 != num5)
		{
			result = ((!((double)((num4 + num5) / 2f) <= 0.5)) ? ((num4 - num5) / (2f - num4 - num5)) : ((num4 - num5) / (num4 + num5)));
		}
		return result;
	}

	public int ToArgb()
	{
		return (int)Value;
	}

	public KnownColor ToKnownColor()
	{
		return (KnownColor)knownColor;
	}

	public override string ToString()
	{
		if ((state & 8) != 0 || (state & 1) != 0)
		{
			return "Color [" + Name + "]";
		}
		if ((state & 2) != 0)
		{
			return "Color [A=" + A + ", R=" + R + ", G=" + G + ", B=" + B + "]";
		}
		return "Color [Empty]";
	}

	public static bool operator ==(Color left, Color right)
	{
		if (left.value == right.value && left.state == right.state && left.knownColor == right.knownColor)
		{
			return left.name == right.name;
		}
		return false;
	}

	public static bool operator !=(Color left, Color right)
	{
		return !(left == right);
	}

	public override bool Equals(object obj)
	{
		if (obj is Color)
		{
			return Equals((Color)obj);
		}
		return false;
	}

	public bool Equals(Color other)
	{
		return this == other;
	}

	public override int GetHashCode()
	{
		if ((name != null) & !IsKnownColor)
		{
			return name.GetHashCode();
		}
		return System.Numerics.Hashing.HashHelpers.Combine(System.Numerics.Hashing.HashHelpers.Combine(value.GetHashCode(), state.GetHashCode()), knownColor.GetHashCode());
	}
}
