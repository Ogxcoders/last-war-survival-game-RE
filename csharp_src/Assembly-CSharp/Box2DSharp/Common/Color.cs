using System;

namespace Box2DSharp.Common;

public struct Color
{
	private const int ARGBAlphaShift = 24;

	private const int ARGBRedShift = 16;

	private const int ARGBGreenShift = 8;

	private const int ARGBBlueShift = 0;

	public byte R => (byte)(Value >> 16);

	public byte G => (byte)(Value >> 8);

	public byte B => (byte)Value;

	public byte A => (byte)(Value >> 24);

	public long Value { get; }

	public static Color Blue { get; } = new Color(4278190335L);

	public static Color Green { get; } = new Color(4278255360L);

	public static Color Red { get; } = new Color(4294901760L);

	public static Color Yellow { get; } = new Color(4294967040L);

	private Color(long value)
	{
		Value = value;
	}

	private static void CheckByte(int value, string name)
	{
		if ((uint)value > 255u)
		{
			throw new ArgumentException($"Value of '{value}' is not valid for '{name}'. '{name}' should be greater than or equal to {(byte)0} and less than or equal to {byte.MaxValue}.");
		}
	}

	public static Color FromArgb(int alpha, int red, int green, int blue)
	{
		CheckByte(alpha, "alpha");
		CheckByte(red, "red");
		CheckByte(green, "green");
		CheckByte(blue, "blue");
		return FromArgb((uint)((alpha << 24) | (red << 16) | (green << 8) | blue));
	}

	public static Color FromArgb(int r, int g, int b)
	{
		return FromArgb(255, r, g, b);
	}

	public static Color FromArgb(float a, float r, float g, float b)
	{
		return FromArgb((int)(a * 255f), (int)(r * 255f), (int)(g * 255f), (int)(b * 255f));
	}

	public static Color FromArgb(float r, float g, float b)
	{
		return FromArgb(255, (int)(r * 255f), (int)(g * 255f), (int)(b * 255f));
	}

	public static Color FromArgb(int argb)
	{
		return FromArgb((uint)argb);
	}

	private static Color FromArgb(uint argb)
	{
		return new Color(argb);
	}
}
