using System.Collections;

namespace System.Xml.Schema;

internal class UnionFacetsChecker : FacetsChecker
{
	internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
	{
		RestrictionFacets restriction = datatype.Restriction;
		if (((restriction?.Flags ?? ((RestrictionFlags)0)) & RestrictionFlags.Enumeration) != 0 && !MatchEnumeration(value, restriction.Enumeration, datatype))
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
