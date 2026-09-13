using System.Collections;
using System.Globalization;

namespace System.Xml.Schema;

internal class Numeric10FacetsChecker : FacetsChecker
{
	private static readonly char[] signs = new char[2] { '+', '-' };

	private decimal maxValue;

	private decimal minValue;

	internal Numeric10FacetsChecker(decimal minVal, decimal maxVal)
	{
		minValue = minVal;
		maxValue = maxVal;
	}

	internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
	{
		decimal value2 = datatype.ValueConverter.ToDecimal(value);
		return CheckValueFacets(value2, datatype);
	}

	internal override Exception CheckValueFacets(decimal value, XmlSchemaDatatype datatype)
	{
		RestrictionFacets restriction = datatype.Restriction;
		RestrictionFlags restrictionFlags = restriction?.Flags ?? ((RestrictionFlags)0);
		XmlValueConverter valueConverter = datatype.ValueConverter;
		if (value > maxValue || value < minValue)
		{
			return new OverflowException(Res.GetString("Value '{0}' was either too large or too small for {1}.", value.ToString(CultureInfo.InvariantCulture), datatype.TypeCodeString));
		}
		if (restrictionFlags != 0)
		{
			if ((restrictionFlags & RestrictionFlags.MaxInclusive) != 0 && value > valueConverter.ToDecimal(restriction.MaxInclusive))
			{
				return new XmlSchemaException("The MaxInclusive constraint failed.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MaxExclusive) != 0 && value >= valueConverter.ToDecimal(restriction.MaxExclusive))
			{
				return new XmlSchemaException("The MaxExclusive constraint failed.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinInclusive) != 0 && value < valueConverter.ToDecimal(restriction.MinInclusive))
			{
				return new XmlSchemaException("The MinInclusive constraint failed.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinExclusive) != 0 && value <= valueConverter.ToDecimal(restriction.MinExclusive))
			{
				return new XmlSchemaException("The MinExclusive constraint failed.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.Enumeration) != 0 && !MatchEnumeration(value, restriction.Enumeration, valueConverter))
			{
				return new XmlSchemaException("The Enumeration constraint failed.", string.Empty);
			}
			return CheckTotalAndFractionDigits(value, restriction.TotalDigits, restriction.FractionDigits, (restrictionFlags & RestrictionFlags.TotalDigits) != 0, (restrictionFlags & RestrictionFlags.FractionDigits) != 0);
		}
		return null;
	}

	internal override Exception CheckValueFacets(long value, XmlSchemaDatatype datatype)
	{
		decimal value2 = value;
		return CheckValueFacets(value2, datatype);
	}

	internal override Exception CheckValueFacets(int value, XmlSchemaDatatype datatype)
	{
		decimal value2 = value;
		return CheckValueFacets(value2, datatype);
	}

	internal override Exception CheckValueFacets(short value, XmlSchemaDatatype datatype)
	{
		decimal value2 = value;
		return CheckValueFacets(value2, datatype);
	}

	internal override Exception CheckValueFacets(byte value, XmlSchemaDatatype datatype)
	{
		decimal value2 = value;
		return CheckValueFacets(value2, datatype);
	}

	internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		return MatchEnumeration(datatype.ValueConverter.ToDecimal(value), enumeration, datatype.ValueConverter);
	}

	internal bool MatchEnumeration(decimal value, ArrayList enumeration, XmlValueConverter valueConverter)
	{
		for (int i = 0; i < enumeration.Count; i++)
		{
			if (value == valueConverter.ToDecimal(enumeration[i]))
			{
				return true;
			}
		}
		return false;
	}

	internal Exception CheckTotalAndFractionDigits(decimal value, int totalDigits, int fractionDigits, bool checkTotal, bool checkFraction)
	{
		decimal num = FacetsChecker.Power(10, totalDigits) - 1m;
		int num2 = 0;
		if (value < 0m)
		{
			value = decimal.Negate(value);
		}
		while (decimal.Truncate(value) != value)
		{
			value *= 10m;
			num2++;
		}
		if (checkTotal && (value > num || num2 > totalDigits))
		{
			return new XmlSchemaException("The TotalDigits constraint failed.", string.Empty);
		}
		if (checkFraction && num2 > fractionDigits)
		{
			return new XmlSchemaException("The FractionDigits constraint failed.", string.Empty);
		}
		return null;
	}
}
