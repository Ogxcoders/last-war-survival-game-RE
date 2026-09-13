using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace System.Xml.Schema;

internal class StringFacetsChecker : FacetsChecker
{
	private static Regex languagePattern;

	private static Regex LanguagePattern
	{
		get
		{
			if (languagePattern == null)
			{
				Regex value = new Regex("^([a-zA-Z]{1,8})(-[a-zA-Z0-9]{1,8})*$", RegexOptions.None);
				Interlocked.CompareExchange(ref languagePattern, value, null);
			}
			return languagePattern;
		}
	}

	internal override Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
	{
		string value2 = datatype.ValueConverter.ToString(value);
		return CheckValueFacets(value2, datatype, verifyUri: true);
	}

	internal override Exception CheckValueFacets(string value, XmlSchemaDatatype datatype)
	{
		return CheckValueFacets(value, datatype, verifyUri: true);
	}

	internal Exception CheckValueFacets(string value, XmlSchemaDatatype datatype, bool verifyUri)
	{
		int length = value.Length;
		RestrictionFacets restriction = datatype.Restriction;
		RestrictionFlags restrictionFlags = restriction?.Flags ?? ((RestrictionFlags)0);
		Exception ex = CheckBuiltInFacets(value, datatype.TypeCode, verifyUri);
		if (ex != null)
		{
			return ex;
		}
		if (restrictionFlags != 0)
		{
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
			if ((restrictionFlags & RestrictionFlags.Enumeration) != 0 && !MatchEnumeration(value, restriction.Enumeration, datatype))
			{
				return new XmlSchemaException("The Enumeration constraint failed.", string.Empty);
			}
		}
		return null;
	}

	internal override bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		return MatchEnumeration(datatype.ValueConverter.ToString(value), enumeration, datatype);
	}

	private bool MatchEnumeration(string value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		if (datatype.TypeCode == XmlTypeCode.AnyUri)
		{
			for (int i = 0; i < enumeration.Count; i++)
			{
				if (value.Equals(((Uri)enumeration[i]).OriginalString))
				{
					return true;
				}
			}
		}
		else
		{
			for (int j = 0; j < enumeration.Count; j++)
			{
				if (value.Equals((string)enumeration[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	private Exception CheckBuiltInFacets(string s, XmlTypeCode typeCode, bool verifyUri)
	{
		Exception result = null;
		switch (typeCode)
		{
		case XmlTypeCode.AnyUri:
			if (verifyUri)
			{
				result = XmlConvert.TryToUri(s, out var _);
			}
			break;
		case XmlTypeCode.NormalizedString:
			result = XmlConvert.TryVerifyNormalizedString(s);
			break;
		case XmlTypeCode.Token:
			result = XmlConvert.TryVerifyTOKEN(s);
			break;
		case XmlTypeCode.Language:
			if (s == null || s.Length == 0)
			{
				return new XmlSchemaException("The attribute value cannot be empty.", string.Empty);
			}
			if (!LanguagePattern.IsMatch(s))
			{
				return new XmlSchemaException("'{0}' is an invalid language identifier.", string.Empty);
			}
			break;
		case XmlTypeCode.NmToken:
			result = XmlConvert.TryVerifyNMTOKEN(s);
			break;
		case XmlTypeCode.Name:
			result = XmlConvert.TryVerifyName(s);
			break;
		case XmlTypeCode.NCName:
		case XmlTypeCode.Id:
		case XmlTypeCode.Idref:
		case XmlTypeCode.Entity:
			result = XmlConvert.TryVerifyNCName(s);
			break;
		}
		return result;
	}
}
