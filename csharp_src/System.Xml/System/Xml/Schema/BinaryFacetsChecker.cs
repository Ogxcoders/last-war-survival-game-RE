using System.Collections;

namespace System.Xml.Schema;

internal class BinaryFacetsChecker : FacetsChecker
{
	internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
	{
		byte[] value2 = (byte[])value;
		return CheckValueFacets(value2, datatype);
	}

	internal override Exception CheckValueFacets(byte[] value, XmlSchemaDatatype datatype)
	{
		RestrictionFacets restriction = datatype.Restriction;
		int num = value.Length;
		RestrictionFlags restrictionFlags = restriction?.Flags ?? ((RestrictionFlags)0);
		if (restrictionFlags != 0)
		{
			if ((restrictionFlags & RestrictionFlags.Length) != 0 && restriction.Length != num)
			{
				return new XmlSchemaException("The actual length is not equal to the specified length.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinLength) != 0 && num < restriction.MinLength)
			{
				return new XmlSchemaException("The actual length is less than the MinLength value.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MaxLength) != 0 && restriction.MaxLength < num)
			{
				return new XmlSchemaException("The actual length is greater than the MaxLength value.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.Enumeration) != 0 && !MatchEnumeration(value, restriction.Enumeration, datatype))
			{
				return new XmlSchemaException("The Enumeration constraint failed.", string.Empty);
			}
		}
		return null;
	}

	internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		return MatchEnumeration((byte[])value, enumeration, datatype);
	}

	private bool MatchEnumeration(byte[] value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		for (int i = 0; i < enumeration.Count; i++)
		{
			if (datatype.Compare(value, (byte[])enumeration[i]) == 0)
			{
				return true;
			}
		}
		return false;
	}
}
