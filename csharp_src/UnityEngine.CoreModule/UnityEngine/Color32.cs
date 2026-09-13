using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Explicit)]
[UsedByNativeCode]
public struct Color32
{
	[FieldOffset(0)]
	[Ignore(DoesNotContributeToSize = true)]
	private int rgba;

	[FieldOffset(0)]
	public byte r;

	[FieldOffset(1)]
	public byte g;

	[FieldOffset(2)]
	public byte b;

	[FieldOffset(3)]
	public byte a;

	public byte this[int index]
	{
		get
		{
			return index switch
			{
				0 => r, 
				1 => g, 
				2 => b, 
				3 => a, 
				_ => throw new IndexOutOfRangeException("Invalid Color32 index(" + index + ")!"), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				r = value;
				break;
			case 1:
				g = value;
				break;
			case 2:
				b = value;
				break;
			case 3:
				a = value;
				break;
			default:
				throw new IndexOutOfRangeException("Invalid Color32 index(" + index + ")!");
			}
		}
	}

	public Color32(byte r, byte g, byte b, byte a)
	{
		rgba = 0;
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public static implicit operator Color32(Color c)
	{
		return new Color32((byte)Mathf.Round(Mathf.Clamp01(c.r) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.g) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.b) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.a) * 255f));
	}

	public static implicit operator Color(Color32 c)
	{
		return new Color((float)(int)c.r / 255f, (float)(int)c.g / 255f, (float)(int)c.b / 255f, (float)(int)c.a / 255f);
	}

	public static Color32 Lerp(Color32 a, Color32 b, float t)
	{
		t = Mathf.Clamp01(t);
		return new Color32((byte)((float)(int)a.r + (float)(b.r - a.r) * t), (byte)((float)(int)a.g + (float)(b.g - a.g) * t), (byte)((float)(int)a.b + (float)(b.b - a.b) * t), (byte)((float)(int)a.a + (float)(b.a - a.a) * t));
	}

	public static Color32 LerpUnclamped(Color32 a, Color32 b, float t)
	{
		return new Color32((byte)((float)(int)a.r + (float)(b.r - a.r) * t), (byte)((float)(int)a.g + (float)(b.g - a.g) * t), (byte)((float)(int)a.b + (float)(b.b - a.b) * t), (byte)((float)(int)a.a + (float)(b.a - a.a) * t));
	}

	[VisibleToOtherModules]
	internal bool InternalEquals(Color32 other)
	{
		return rgba == other.rgba;
	}

	public override string ToString()
	{
		return UnityString.Format("RGBA({0}, {1}, {2}, {3})", r, g, b, a);
	}

	public string ToString(string format)
	{
		return UnityString.Format("RGBA({0}, {1}, {2}, {3})", r.ToString(format), g.ToString(format), b.ToString(format), a.ToString(format));
	}
}
