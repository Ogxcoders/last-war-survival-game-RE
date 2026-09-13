using System;
using System.Globalization;

namespace UnityEngine.UIElements;

public struct Length : IEquatable<Length>
{
	private float m_Value;

	private LengthUnit m_Unit;

	public float value
	{
		get
		{
			return m_Value;
		}
		set
		{
			m_Value = value;
		}
	}

	public LengthUnit unit
	{
		get
		{
			return m_Unit;
		}
		set
		{
			m_Unit = value;
		}
	}

	public static Length Percent(float value)
	{
		return new Length(value, LengthUnit.Percent);
	}

	public Length(float value)
		: this(value, LengthUnit.Pixel)
	{
	}

	public Length(float value, LengthUnit unit)
	{
		m_Value = value;
		m_Unit = unit;
	}

	public static implicit operator Length(float value)
	{
		return new Length(value, LengthUnit.Pixel);
	}

	public static bool operator ==(Length lhs, Length rhs)
	{
		return lhs.m_Value == rhs.m_Value && lhs.m_Unit == rhs.m_Unit;
	}

	public static bool operator !=(Length lhs, Length rhs)
	{
		return !(lhs == rhs);
	}

	public bool Equals(Length other)
	{
		return other == this;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Length length))
		{
			return false;
		}
		return length == this;
	}

	public override int GetHashCode()
	{
		int num = 851985039;
		num = num * -1521134295 + m_Value.GetHashCode();
		return num * -1521134295 + m_Unit.GetHashCode();
	}

	public override string ToString()
	{
		string text = string.Empty;
		switch (unit)
		{
		case LengthUnit.Pixel:
			if (!Mathf.Approximately(0f, value))
			{
				text = "px";
			}
			break;
		case LengthUnit.Percent:
			text = "%";
			break;
		}
		return value.ToString(CultureInfo.InvariantCulture.NumberFormat) + text;
	}
}
