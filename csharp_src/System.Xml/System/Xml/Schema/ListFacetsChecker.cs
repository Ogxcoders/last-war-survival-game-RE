using System.Collections;

namespace System.Xml.Schema;

internal class ListFacetsChecker : FacetsChecker
{
	internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
	{
		Array array = value as Array;
		RestrictionFacets restriction = datatype.Restriction;
		RestrictionFlags restrictionFlags = restriction?.Flags ?? ((RestrictionFlags)0);
		if ((restrictionFlags & (RestrictionFlags.Length | RestrictionFlags.MinLength | RestrictionFlags.MaxLength)) != 0)
		{
			int length = array.Length;
			if ((restrictionFlags & RestrictionFlags.Length) != 0 && restriction.Length != length)
			{
				return new XmlSchemaException("The actual length is not equal to the specified length.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MinLength) != 0 && length < restriction.MinLength)
			{
				return new XmlSchemaException("The actual length is less than the MinLength value.", string.Empty);
			}
			if ((restrictionFlags & RestrictionFlags.MaxLength) != 0 && restriction.MaxLength < length)
			{
				return new XmlSchemaException("The actual length is greater than the MaxLength value.", string.Empty);
			}
		}
		if ((restrictionFlags & RestrictionFlags.Enumeration) != 0 && !MatchEnumeration(value, restriction.Enumeration, datatype))
		{
			return new XmlSchemaException("The Enumeration constraint failed.", string.Empty);
		}
		return null;
	}

	internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		for (int i = 0; i < enumeration.Count; i++)
		{
			if (datatype.Compare(value, enumeration[i]) == 0)
			{
				return true;
			}
		}
		return false;
	}
}
