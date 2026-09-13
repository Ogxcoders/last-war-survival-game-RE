using System;
using System.Globalization;

namespace UnityEngine.UIElements.StyleSheets;

[Serializable]
internal struct Dimension : IEquatable<Dimension>
{
	public enum Unit
	{
		Unitless,
		Pixel,
		Percent
	}

	public Unit unit;

	public float value;

	public Dimension(float value, Unit unit)
	{
		this.unit = unit;
		this.value = value;
	}

	public Length ToLength()
	{
		LengthUnit lengthUnit = ((unit == Unit.Percent) ? LengthUnit.Percent : LengthUnit.Pixel);
		return new Length(value, lengthUnit);
	}

	public static bool operator ==(Dimension lhs, Dimension rhs)
	{
		return lhs.value == rhs.value && lhs.unit == rhs.unit;
	}

	public static bool operator !=(Dimension lhs, Dimension rhs)
	{
		return !(lhs == rhs);
	}

	public bool Equals(Dimension other)
	{
		return other == this;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Dimension dimension))
		{
			return false;
		}
		return dimension == this;
	}

	public override int GetHashCode()
	{
		int num = -799583767;
		num = num * -1521134295 + unit.GetHashCode();
		return num * -1521134295 + value.GetHashCode();
	}

	public override string ToString()
	{
		string text = string.Empty;
		switch (unit)
		{
		case Unit.Pixel:
			text = "px";
			break;
		case Unit.Percent:
			text = "%";
			break;
		}
		return value.ToString(CultureInfo.InvariantCulture.NumberFormat) + text;
	}
}
