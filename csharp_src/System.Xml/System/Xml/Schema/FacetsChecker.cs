using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Xml.Schema;

internal abstract class FacetsChecker
{
	private struct FacetsCompiler
	{
		private struct Map
		{
			internal char match;

			internal string replacement;

			internal Map(char m, string r)
			{
				match = m;
				replacement = r;
			}
		}

		private DatatypeImplementation datatype;

		private RestrictionFacets derivedRestriction;

		private RestrictionFlags baseFlags;

		private RestrictionFlags baseFixedFlags;

		private RestrictionFlags validRestrictionFlags;

		private XmlSchemaDatatype nonNegativeInt;

		private XmlSchemaDatatype builtInType;

		private XmlTypeCode builtInEnum;

		private bool firstPattern;

		private StringBuilder regStr;

		private XmlSchemaPatternFacet pattern_facet;

		private static readonly Map[] c_map = new Map[8]
		{
			new Map('c', "\\p{_xmlC}"),
			new Map('C', "\\P{_xmlC}"),
			new Map('d', "\\p{_xmlD}"),
			new Map('D', "\\P{_xmlD}"),
			new Map('i', "\\p{_xmlI}"),
			new Map('I', "\\P{_xmlI}"),
			new Map('w', "\\p{_xmlW}"),
			new Map('W', "\\P{_xmlW}")
		};

		public FacetsCompiler(DatatypeImplementation baseDatatype, RestrictionFacets restriction)
		{
			firstPattern = true;
			regStr = null;
			pattern_facet = null;
			datatype = baseDatatype;
			derivedRestriction = restriction;
			baseFlags = ((datatype.Restriction != null) ? datatype.Restriction.Flags : ((RestrictionFlags)0));
			baseFixedFlags = ((datatype.Restriction != null) ? datatype.Restriction.FixedFlags : ((RestrictionFlags)0));
			validRestrictionFlags = datatype.ValidRestrictionFlags;
			nonNegativeInt = DatatypeImplementation.GetSimpleTypeFromTypeCode(XmlTypeCode.NonNegativeInteger).Datatype;
			builtInEnum = ((!(datatype is Datatype_union) && !(datatype is Datatype_List)) ? datatype.TypeCode : XmlTypeCode.None);
			builtInType = ((builtInEnum > XmlTypeCode.None) ? DatatypeImplementation.GetSimpleTypeFromTypeCode(builtInEnum).Datatype : datatype);
		}

		internal void CompileLengthFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.Length, "The length constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.Length, "This is a duplicate Length constraining facet.");
			derivedRestriction.Length = XmlBaseConverter.DecimalToInt32((decimal)ParseFacetValue(nonNegativeInt, facet, "The Length constraining facet is invalid - {0}", null, null));
			if ((baseFixedFlags & RestrictionFlags.Length) != 0 && !datatype.IsEqual(datatype.Restriction.Length, derivedRestriction.Length))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			if ((baseFlags & RestrictionFlags.Length) != 0 && datatype.Restriction.Length < derivedRestriction.Length)
			{
				throw new XmlSchemaException("It is an error if 'length' is among the members of {facets} of {base type definition} and {value} is greater than the {value} of the parent 'length'.", facet);
			}
			if ((baseFlags & RestrictionFlags.MinLength) != 0 && datatype.Restriction.MinLength > derivedRestriction.Length)
			{
				throw new XmlSchemaException("It is an error for both 'length' and either 'minLength' or 'maxLength' to be members of {facets}, unless they are specified in different derivation steps. In which case the following must be true: the {value} of 'minLength' <= the {value} of 'length' <= the {value} of 'maxLength'.", facet);
			}
			if ((baseFlags & RestrictionFlags.MaxLength) != 0 && datatype.Restriction.MaxLength < derivedRestriction.Length)
			{
				throw new XmlSchemaException("It is an error for both 'length' and either 'minLength' or 'maxLength' to be members of {facets}, unless they are specified in different derivation steps. In which case the following must be true: the {value} of 'minLength' <= the {value} of 'length' <= the {value} of 'maxLength'.", facet);
			}
			SetFlag(facet, RestrictionFlags.Length);
		}

		internal void CompileMinLengthFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.MinLength, "The MinLength constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.MinLength, "This is a duplicate MinLength constraining facet.");
			derivedRestriction.MinLength = XmlBaseConverter.DecimalToInt32((decimal)ParseFacetValue(nonNegativeInt, facet, "The MinLength constraining facet is invalid - {0}", null, null));
			if ((baseFixedFlags & RestrictionFlags.MinLength) != 0 && !datatype.IsEqual(datatype.Restriction.MinLength, derivedRestriction.MinLength))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			if ((baseFlags & RestrictionFlags.MinLength) != 0 && datatype.Restriction.MinLength > derivedRestriction.MinLength)
			{
				throw new XmlSchemaException("It is an error if 'minLength' is among the members of {facets} of {base type definition} and {value} is less than the {value} of the parent 'minLength'.", facet);
			}
			if ((baseFlags & RestrictionFlags.Length) != 0 && datatype.Restriction.Length < derivedRestriction.MinLength)
			{
				throw new XmlSchemaException("It is an error for both 'length' and either 'minLength' or 'maxLength' to be members of {facets}, unless they are specified in different derivation steps. In which case the following must be true: the {value} of 'minLength' <= the {value} of 'length' <= the {value} of 'maxLength'.", facet);
			}
			SetFlag(facet, RestrictionFlags.MinLength);
		}

		internal void CompileMaxLengthFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.MaxLength, "The MaxLength constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.MaxLength, "This is a duplicate MaxLength constraining facet.");
			derivedRestriction.MaxLength = XmlBaseConverter.DecimalToInt32((decimal)ParseFacetValue(nonNegativeInt, facet, "The MaxLength constraining facet is invalid - {0}", null, null));
			if ((baseFixedFlags & RestrictionFlags.MaxLength) != 0 && !datatype.IsEqual(datatype.Restriction.MaxLength, derivedRestriction.MaxLength))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			if ((baseFlags & RestrictionFlags.MaxLength) != 0 && datatype.Restriction.MaxLength < derivedRestriction.MaxLength)
			{
				throw new XmlSchemaException("It is an error if 'maxLength' is among the members of {facets} of {base type definition} and {value} is greater than the {value} of the parent 'maxLength'.", facet);
			}
			if ((baseFlags & RestrictionFlags.Length) != 0 && datatype.Restriction.Length > derivedRestriction.MaxLength)
			{
				throw new XmlSchemaException("It is an error for both 'length' and either 'minLength' or 'maxLength' to be members of {facets}, unless they are specified in different derivation steps. In which case the following must be true: the {value} of 'minLength' <= the {value} of 'length' <= the {value} of 'maxLength'.", facet);
			}
			SetFlag(facet, RestrictionFlags.MaxLength);
		}

		internal void CompilePatternFacet(XmlSchemaPatternFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.Pattern, "The Pattern constraining facet is prohibited for '{0}'.");
			if (firstPattern)
			{
				regStr = new StringBuilder();
				regStr.Append("(");
				regStr.Append(facet.Value);
				pattern_facet = facet;
				firstPattern = false;
			}
			else
			{
				regStr.Append(")|(");
				regStr.Append(facet.Value);
			}
			SetFlag(facet, RestrictionFlags.Pattern);
		}

		internal void CompileEnumerationFacet(XmlSchemaFacet facet, IXmlNamespaceResolver nsmgr, XmlNameTable nameTable)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.Enumeration, "The Enumeration constraining facet is prohibited for '{0}'.");
			if (derivedRestriction.Enumeration == null)
			{
				derivedRestriction.Enumeration = new ArrayList();
			}
			derivedRestriction.Enumeration.Add(ParseFacetValue(datatype, facet, "The Enumeration constraining facet is invalid - {0}", nsmgr, nameTable));
			SetFlag(facet, RestrictionFlags.Enumeration);
		}

		internal void CompileWhitespaceFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.WhiteSpace, "The WhiteSpace constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.WhiteSpace, "This is a duplicate WhiteSpace constraining facet.");
			if (facet.Value == "preserve")
			{
				derivedRestriction.WhiteSpace = XmlSchemaWhiteSpace.Preserve;
			}
			else if (facet.Value == "replace")
			{
				derivedRestriction.WhiteSpace = XmlSchemaWhiteSpace.Replace;
			}
			else
			{
				if (!(facet.Value == "collapse"))
				{
					throw new XmlSchemaException("The white space character, '{0}', is invalid.", facet.Value, facet);
				}
				derivedRestriction.WhiteSpace = XmlSchemaWhiteSpace.Collapse;
			}
			if ((baseFixedFlags & RestrictionFlags.WhiteSpace) != 0 && !datatype.IsEqual(datatype.Restriction.WhiteSpace, derivedRestriction.WhiteSpace))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			XmlSchemaWhiteSpace xmlSchemaWhiteSpace = (((baseFlags & RestrictionFlags.WhiteSpace) == 0) ? datatype.BuiltInWhitespaceFacet : datatype.Restriction.WhiteSpace);
			if (xmlSchemaWhiteSpace == XmlSchemaWhiteSpace.Collapse && (derivedRestriction.WhiteSpace == XmlSchemaWhiteSpace.Replace || derivedRestriction.WhiteSpace == XmlSchemaWhiteSpace.Preserve))
			{
				throw new XmlSchemaException("It is an error if 'whiteSpace' is among the members of {facets} of {base type definition}, {value} is 'replace' or 'preserve', and the {value} of the parent 'whiteSpace' is 'collapse'.", facet);
			}
			if (xmlSchemaWhiteSpace == XmlSchemaWhiteSpace.Replace && derivedRestriction.WhiteSpace == XmlSchemaWhiteSpace.Preserve)
			{
				throw new XmlSchemaException("It is an error if 'whiteSpace' is among the members of {facets} of {base type definition}, {value} is 'preserve', and the {value} of the parent 'whiteSpace' is 'replace'.", facet);
			}
			SetFlag(facet, RestrictionFlags.WhiteSpace);
		}

		internal void CompileMaxInclusiveFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.MaxInclusive, "The MaxInclusive constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.MaxInclusive, "This is a duplicate MaxInclusive constraining facet.");
			derivedRestriction.MaxInclusive = ParseFacetValue(builtInType, facet, "The MaxInclusive constraining facet is invalid - {0}", null, null);
			if ((baseFixedFlags & RestrictionFlags.MaxInclusive) != 0 && !datatype.IsEqual(datatype.Restriction.MaxInclusive, derivedRestriction.MaxInclusive))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			CheckValue(derivedRestriction.MaxInclusive, facet);
			SetFlag(facet, RestrictionFlags.MaxInclusive);
		}

		internal void CompileMaxExclusiveFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.MaxExclusive, "The MaxExclusive constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.MaxExclusive, "This is a duplicate MaxExclusive constraining facet.");
			derivedRestriction.MaxExclusive = ParseFacetValue(builtInType, facet, "The MaxExclusive constraining facet is invalid - {0}", null, null);
			if ((baseFixedFlags & RestrictionFlags.MaxExclusive) != 0 && !datatype.IsEqual(datatype.Restriction.MaxExclusive, derivedRestriction.MaxExclusive))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			CheckValue(derivedRestriction.MaxExclusive, facet);
			SetFlag(facet, RestrictionFlags.MaxExclusive);
		}

		internal void CompileMinInclusiveFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.MinInclusive, "The MinInclusive constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.MinInclusive, "This is a duplicate MinInclusive constraining facet.");
			derivedRestriction.MinInclusive = ParseFacetValue(builtInType, facet, "The MinInclusive constraining facet is invalid - {0}", null, null);
			if ((baseFixedFlags & RestrictionFlags.MinInclusive) != 0 && !datatype.IsEqual(datatype.Restriction.MinInclusive, derivedRestriction.MinInclusive))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			CheckValue(derivedRestriction.MinInclusive, facet);
			SetFlag(facet, RestrictionFlags.MinInclusive);
		}

		internal void CompileMinExclusiveFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.MinExclusive, "The MinExclusive constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.MinExclusive, "This is a duplicate MinExclusive constraining facet.");
			derivedRestriction.MinExclusive = ParseFacetValue(builtInType, facet, "The MinExclusive constraining facet is invalid - {0}", null, null);
			if ((baseFixedFlags & RestrictionFlags.MinExclusive) != 0 && !datatype.IsEqual(datatype.Restriction.MinExclusive, derivedRestriction.MinExclusive))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			CheckValue(derivedRestriction.MinExclusive, facet);
			SetFlag(facet, RestrictionFlags.MinExclusive);
		}

		internal void CompileTotalDigitsFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.TotalDigits, "The TotalDigits constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.TotalDigits, "This is a duplicate TotalDigits constraining facet.");
			XmlSchemaDatatype xmlSchemaDatatype = DatatypeImplementation.GetSimpleTypeFromTypeCode(XmlTypeCode.PositiveInteger).Datatype;
			derivedRestriction.TotalDigits = XmlBaseConverter.DecimalToInt32((decimal)ParseFacetValue(xmlSchemaDatatype, facet, "The TotalDigits constraining facet is invalid - {0}", null, null));
			if ((baseFixedFlags & RestrictionFlags.TotalDigits) != 0 && !datatype.IsEqual(datatype.Restriction.TotalDigits, derivedRestriction.TotalDigits))
			{
				throw new XmlSchemaException("Values that are declared as {fixed} in a base type can not be changed in a derived type.", facet);
			}
			if ((baseFlags & RestrictionFlags.TotalDigits) != 0 && derivedRestriction.TotalDigits > datatype.Restriction.TotalDigits)
			{
				throw new XmlSchemaException("It is an error if the derived 'totalDigits' facet value is greater than the parent 'totalDigits' facet value.", string.Empty);
			}
			SetFlag(facet, RestrictionFlags.TotalDigits);
		}

		internal void CompileFractionDigitsFacet(XmlSchemaFacet facet)
		{
			CheckProhibitedFlag(facet, RestrictionFlags.FractionDigits, "The FractionDigits constraining facet is prohibited for '{0}'.");
			CheckDupFlag(facet, RestrictionFlags.FractionDigits, "This is a duplicate FractionDigits constraining facet.");
			derivedRestriction.FractionDigits = XmlBaseConverter.DecimalToInt32((decimal)ParseFacetValue(nonNegativeInt, facet, "The FractionDigits constraining facet is invalid - {0}", null, null));
			if (derivedRestriction.FractionDigits != 0 && datatype.TypeCode != XmlTypeCode.Decimal)
			{
				throw new XmlSchemaException("The FractionDigits constraining facet is invalid - {0}", Res.GetString("FractionDigits should be equal to 0 on types other then decimal."), facet);
			}
			if ((baseFlags & RestrictionFlags.FractionDigits) != 0 && derivedRestriction.FractionDigits > datatype.Restriction.FractionDigits)
			{
				throw new XmlSchemaException("It is an error if the derived 'totalDigits' facet value is greater than the parent 'totalDigits' facet value.", string.Empty);
			}
			SetFlag(facet, RestrictionFlags.FractionDigits);
		}

		internal void FinishFacetCompile()
		{
			if (firstPattern)
			{
				return;
			}
			if (derivedRestriction.Patterns == null)
			{
				derivedRestriction.Patterns = new ArrayList();
			}
			try
			{
				regStr.Append(")");
				if (regStr.ToString().IndexOf('|') != -1)
				{
					regStr.Insert(0, "(");
					regStr.Append(")");
				}
				derivedRestriction.Patterns.Add(new Regex(Preprocess(regStr.ToString()), RegexOptions.None));
			}
			catch (Exception ex)
			{
				throw new XmlSchemaException("The Pattern constraining facet is invalid - {0}", new string[1] { ex.Message }, ex, pattern_facet.SourceUri, pattern_facet.LineNumber, pattern_facet.LinePosition, pattern_facet);
			}
		}

		private void CheckValue(object value, XmlSchemaFacet facet)
		{
			RestrictionFacets restriction = datatype.Restriction;
			switch (facet.FacetType)
			{
			case FacetType.MaxInclusive:
				if ((baseFlags & RestrictionFlags.MaxInclusive) != 0 && datatype.Compare(value, restriction.MaxInclusive) > 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'maxInclusive' facet value is greater than the parent 'maxInclusive' facet value.", string.Empty);
				}
				if ((baseFlags & RestrictionFlags.MaxExclusive) != 0 && datatype.Compare(value, restriction.MaxExclusive) >= 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'maxInclusive' facet value is greater than or equal to the parent 'maxExclusive' facet value.", string.Empty);
				}
				break;
			case FacetType.MaxExclusive:
				if ((baseFlags & RestrictionFlags.MaxExclusive) != 0 && datatype.Compare(value, restriction.MaxExclusive) > 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'maxExclusive' facet value is greater than the parent 'maxExclusive' facet value.", string.Empty);
				}
				if ((baseFlags & RestrictionFlags.MaxInclusive) != 0 && datatype.Compare(value, restriction.MaxInclusive) > 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'maxExclusive' facet value is greater than or equal to the parent 'maxInclusive' facet value.", string.Empty);
				}
				break;
			case FacetType.MinInclusive:
				if ((baseFlags & RestrictionFlags.MinInclusive) != 0 && datatype.Compare(value, restriction.MinInclusive) < 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'minInclusive' facet value is less than the parent 'minInclusive' facet value.", string.Empty);
				}
				if ((baseFlags & RestrictionFlags.MinExclusive) != 0 && datatype.Compare(value, restriction.MinExclusive) < 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'minInclusive' facet value is less than or equal to the parent 'minExclusive' facet value.", string.Empty);
				}
				if ((baseFlags & RestrictionFlags.MaxExclusive) != 0 && datatype.Compare(value, restriction.MaxExclusive) >= 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'minInclusive' facet value is greater than or equal to the parent 'maxExclusive' facet value.", string.Empty);
				}
				break;
			case FacetType.MinExclusive:
				if ((baseFlags & RestrictionFlags.MinExclusive) != 0 && datatype.Compare(value, restriction.MinExclusive) < 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'minExclusive' facet value is less than the parent 'minExclusive' facet value.", string.Empty);
				}
				if ((baseFlags & RestrictionFlags.MinInclusive) != 0 && datatype.Compare(value, restriction.MinInclusive) < 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'minExclusive' facet value is less than or equal to the parent 'minInclusive' facet value.", string.Empty);
				}
				if ((baseFlags & RestrictionFlags.MaxExclusive) != 0 && datatype.Compare(value, restriction.MaxExclusive) >= 0)
				{
					throw new XmlSchemaException("It is an error if the derived 'minExclusive' facet value is greater than or equal to the parent 'maxExclusive' facet value.", string.Empty);
				}
				break;
			}
		}

		internal void CompileFacetCombinations()
		{
			_ = datatype.Restriction;
			if ((derivedRestriction.Flags & RestrictionFlags.MaxInclusive) != 0 && (derivedRestriction.Flags & RestrictionFlags.MaxExclusive) != 0)
			{
				throw new XmlSchemaException("'maxInclusive' and 'maxExclusive' cannot both be specified for the same data type.", string.Empty);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinInclusive) != 0 && (derivedRestriction.Flags & RestrictionFlags.MinExclusive) != 0)
			{
				throw new XmlSchemaException("'minInclusive' and 'minExclusive' cannot both be specified for the same data type.", string.Empty);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.Length) != 0 && (derivedRestriction.Flags & (RestrictionFlags.MinLength | RestrictionFlags.MaxLength)) != 0)
			{
				throw new XmlSchemaException("It is an error for both length and minLength or maxLength to be present.", string.Empty);
			}
			CopyFacetsFromBaseType();
			if ((derivedRestriction.Flags & RestrictionFlags.MinLength) != 0 && (derivedRestriction.Flags & RestrictionFlags.MaxLength) != 0 && derivedRestriction.MinLength > derivedRestriction.MaxLength)
			{
				throw new XmlSchemaException("MinLength is greater than MaxLength.", string.Empty);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinInclusive) != 0 && (derivedRestriction.Flags & RestrictionFlags.MaxInclusive) != 0 && datatype.Compare(derivedRestriction.MinInclusive, derivedRestriction.MaxInclusive) > 0)
			{
				throw new XmlSchemaException("The value specified for 'minInclusive' cannot be greater than the value specified for 'maxInclusive' for the same data type.", string.Empty);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinInclusive) != 0 && (derivedRestriction.Flags & RestrictionFlags.MaxExclusive) != 0 && datatype.Compare(derivedRestriction.MinInclusive, derivedRestriction.MaxExclusive) > 0)
			{
				throw new XmlSchemaException("The value specified for 'minInclusive' cannot be greater than the value specified for 'maxExclusive' for the same data type.", string.Empty);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinExclusive) != 0 && (derivedRestriction.Flags & RestrictionFlags.MaxExclusive) != 0 && datatype.Compare(derivedRestriction.MinExclusive, derivedRestriction.MaxExclusive) > 0)
			{
				throw new XmlSchemaException("The value specified for 'minExclusive' cannot be greater than the value specified for 'maxExclusive' for the same data type.", string.Empty);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinExclusive) != 0 && (derivedRestriction.Flags & RestrictionFlags.MaxInclusive) != 0 && datatype.Compare(derivedRestriction.MinExclusive, derivedRestriction.MaxInclusive) > 0)
			{
				throw new XmlSchemaException("The value specified for 'minExclusive' cannot be greater than the value specified for 'maxInclusive' for the same data type.", string.Empty);
			}
			if ((derivedRestriction.Flags & (RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits)) == (RestrictionFlags.TotalDigits | RestrictionFlags.FractionDigits) && derivedRestriction.FractionDigits > derivedRestriction.TotalDigits)
			{
				throw new XmlSchemaException("FractionDigits is greater than TotalDigits.", string.Empty);
			}
		}

		private void CopyFacetsFromBaseType()
		{
			RestrictionFacets restriction = datatype.Restriction;
			if ((derivedRestriction.Flags & RestrictionFlags.Length) == 0 && (baseFlags & RestrictionFlags.Length) != 0)
			{
				derivedRestriction.Length = restriction.Length;
				SetFlag(RestrictionFlags.Length);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinLength) == 0 && (baseFlags & RestrictionFlags.MinLength) != 0)
			{
				derivedRestriction.MinLength = restriction.MinLength;
				SetFlag(RestrictionFlags.MinLength);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MaxLength) == 0 && (baseFlags & RestrictionFlags.MaxLength) != 0)
			{
				derivedRestriction.MaxLength = restriction.MaxLength;
				SetFlag(RestrictionFlags.MaxLength);
			}
			if ((baseFlags & RestrictionFlags.Pattern) != 0)
			{
				if (derivedRestriction.Patterns == null)
				{
					derivedRestriction.Patterns = restriction.Patterns;
				}
				else
				{
					derivedRestriction.Patterns.AddRange(restriction.Patterns);
				}
				SetFlag(RestrictionFlags.Pattern);
			}
			if ((baseFlags & RestrictionFlags.Enumeration) != 0)
			{
				if (derivedRestriction.Enumeration == null)
				{
					derivedRestriction.Enumeration = restriction.Enumeration;
				}
				SetFlag(RestrictionFlags.Enumeration);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.WhiteSpace) == 0 && (baseFlags & RestrictionFlags.WhiteSpace) != 0)
			{
				derivedRestriction.WhiteSpace = restriction.WhiteSpace;
				SetFlag(RestrictionFlags.WhiteSpace);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MaxInclusive) == 0 && (baseFlags & RestrictionFlags.MaxInclusive) != 0)
			{
				derivedRestriction.MaxInclusive = restriction.MaxInclusive;
				SetFlag(RestrictionFlags.MaxInclusive);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MaxExclusive) == 0 && (baseFlags & RestrictionFlags.MaxExclusive) != 0)
			{
				derivedRestriction.MaxExclusive = restriction.MaxExclusive;
				SetFlag(RestrictionFlags.MaxExclusive);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinInclusive) == 0 && (baseFlags & RestrictionFlags.MinInclusive) != 0)
			{
				derivedRestriction.MinInclusive = restriction.MinInclusive;
				SetFlag(RestrictionFlags.MinInclusive);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.MinExclusive) == 0 && (baseFlags & RestrictionFlags.MinExclusive) != 0)
			{
				derivedRestriction.MinExclusive = restriction.MinExclusive;
				SetFlag(RestrictionFlags.MinExclusive);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.TotalDigits) == 0 && (baseFlags & RestrictionFlags.TotalDigits) != 0)
			{
				derivedRestriction.TotalDigits = restriction.TotalDigits;
				SetFlag(RestrictionFlags.TotalDigits);
			}
			if ((derivedRestriction.Flags & RestrictionFlags.FractionDigits) == 0 && (baseFlags & RestrictionFlags.FractionDigits) != 0)
			{
				derivedRestriction.FractionDigits = restriction.FractionDigits;
				SetFlag(RestrictionFlags.FractionDigits);
			}
		}

		private object ParseFacetValue(XmlSchemaDatatype datatype, XmlSchemaFacet facet, string code, IXmlNamespaceResolver nsmgr, XmlNameTable nameTable)
		{
			object typedValue;
			Exception ex = datatype.TryParseValue(facet.Value, nameTable, nsmgr, out typedValue);
			if (ex == null)
			{
				return typedValue;
			}
			throw new XmlSchemaException(code, new string[1] { ex.Message }, ex, facet.SourceUri, facet.LineNumber, facet.LinePosition, facet);
		}

		private static string Preprocess(string pattern)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("^");
			char[] array = pattern.ToCharArray();
			int length = pattern.Length;
			int num = 0;
			for (int i = 0; i < length - 2; i++)
			{
				if (array[i] != '\\')
				{
					continue;
				}
				if (array[i + 1] == '\\')
				{
					i++;
					continue;
				}
				char c = array[i + 1];
				for (int j = 0; j < c_map.Length; j++)
				{
					if (c_map[j].match == c)
					{
						if (num < i)
						{
							stringBuilder.Append(array, num, i - num);
						}
						stringBuilder.Append(c_map[j].replacement);
						i++;
						num = i + 1;
						break;
					}
				}
			}
			if (num < length)
			{
				stringBuilder.Append(array, num, length - num);
			}
			stringBuilder.Append("$");
			return stringBuilder.ToString();
		}

		private void CheckProhibitedFlag(XmlSchemaFacet facet, RestrictionFlags flag, string errorCode)
		{
			if ((validRestrictionFlags & flag) == 0)
			{
				throw new XmlSchemaException(errorCode, datatype.TypeCodeString, facet);
			}
		}

		private void CheckDupFlag(XmlSchemaFacet facet, RestrictionFlags flag, string errorCode)
		{
			if ((derivedRestriction.Flags & flag) != 0)
			{
				throw new XmlSchemaException(errorCode, facet);
			}
		}

		private void SetFlag(XmlSchemaFacet facet, RestrictionFlags flag)
		{
			derivedRestriction.Flags |= flag;
			if (facet.IsFixed)
			{
				derivedRestriction.FixedFlags |= flag;
			}
		}

		private void SetFlag(RestrictionFlags flag)
		{
			derivedRestriction.Flags |= flag;
			if ((baseFixedFlags & flag) != 0)
			{
				derivedRestriction.FixedFlags |= flag;
			}
		}
	}

	internal virtual Exception CheckLexicalFacets(ref string parseString, XmlSchemaDatatype datatype)
	{
		CheckWhitespaceFacets(ref parseString, datatype);
		return CheckPatternFacets(datatype.Restriction, parseString);
	}

	internal virtual Exception CheckValueFacets(object value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(decimal value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(long value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(int value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(short value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(byte value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(DateTime value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(double value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(float value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(string value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(byte[] value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(TimeSpan value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal virtual Exception CheckValueFacets(XmlQualifiedName value, XmlSchemaDatatype datatype)
	{
		return null;
	}

	internal void CheckWhitespaceFacets(ref string s, XmlSchemaDatatype datatype)
	{
		RestrictionFacets restriction = datatype.Restriction;
		switch (datatype.Variety)
		{
		case XmlSchemaDatatypeVariety.List:
			s = s.Trim();
			break;
		case XmlSchemaDatatypeVariety.Atomic:
			if (datatype.BuiltInWhitespaceFacet == XmlSchemaWhiteSpace.Collapse)
			{
				s = XmlComplianceUtil.NonCDataNormalize(s);
			}
			else if (datatype.BuiltInWhitespaceFacet == XmlSchemaWhiteSpace.Replace)
			{
				s = XmlComplianceUtil.CDataNormalize(s);
			}
			else if (restriction != null && (restriction.Flags & RestrictionFlags.WhiteSpace) != 0)
			{
				if (restriction.WhiteSpace == XmlSchemaWhiteSpace.Replace)
				{
					s = XmlComplianceUtil.CDataNormalize(s);
				}
				else if (restriction.WhiteSpace == XmlSchemaWhiteSpace.Collapse)
				{
					s = XmlComplianceUtil.NonCDataNormalize(s);
				}
			}
			break;
		}
	}

	internal Exception CheckPatternFacets(RestrictionFacets restriction, string value)
	{
		if (restriction != null && (restriction.Flags & RestrictionFlags.Pattern) != 0)
		{
			for (int i = 0; i < restriction.Patterns.Count; i++)
			{
				if (!((Regex)restriction.Patterns[i]).IsMatch(value))
				{
					return new XmlSchemaException("The Pattern constraint failed.", string.Empty);
				}
			}
		}
		return null;
	}

	internal virtual bool MatchEnumeration(object value, ArrayList enumeration, XmlSchemaDatatype datatype)
	{
		return false;
	}

	internal virtual RestrictionFacets ConstructRestriction(DatatypeImplementation datatype, XmlSchemaObjectCollection facets, XmlNameTable nameTable)
	{
		RestrictionFacets restrictionFacets = new RestrictionFacets();
		FacetsCompiler facetsCompiler = new FacetsCompiler(datatype, restrictionFacets);
		for (int i = 0; i < facets.Count; i++)
		{
			XmlSchemaFacet xmlSchemaFacet = (XmlSchemaFacet)facets[i];
			if (xmlSchemaFacet.Value == null)
			{
				throw new XmlSchemaException("The 'value' attribute must be present in facet.", xmlSchemaFacet);
			}
			IXmlNamespaceResolver nsmgr = new SchemaNamespaceManager(xmlSchemaFacet);
			switch (xmlSchemaFacet.FacetType)
			{
			case FacetType.Length:
				facetsCompiler.CompileLengthFacet(xmlSchemaFacet);
				break;
			case FacetType.MinLength:
				facetsCompiler.CompileMinLengthFacet(xmlSchemaFacet);
				break;
			case FacetType.MaxLength:
				facetsCompiler.CompileMaxLengthFacet(xmlSchemaFacet);
				break;
			case FacetType.Pattern:
				facetsCompiler.CompilePatternFacet(xmlSchemaFacet as XmlSchemaPatternFacet);
				break;
			case FacetType.Enumeration:
				facetsCompiler.CompileEnumerationFacet(xmlSchemaFacet, nsmgr, nameTable);
				break;
			case FacetType.Whitespace:
				facetsCompiler.CompileWhitespaceFacet(xmlSchemaFacet);
				break;
			case FacetType.MinInclusive:
				facetsCompiler.CompileMinInclusiveFacet(xmlSchemaFacet);
				break;
			case FacetType.MinExclusive:
				facetsCompiler.CompileMinExclusiveFacet(xmlSchemaFacet);
				break;
			case FacetType.MaxInclusive:
				facetsCompiler.CompileMaxInclusiveFacet(xmlSchemaFacet);
				break;
			case FacetType.MaxExclusive:
				facetsCompiler.CompileMaxExclusiveFacet(xmlSchemaFacet);
				break;
			case FacetType.TotalDigits:
				facetsCompiler.CompileTotalDigitsFacet(xmlSchemaFacet);
				break;
			case FacetType.FractionDigits:
				facetsCompiler.CompileFractionDigitsFacet(xmlSchemaFacet);
				break;
			default:
				throw new XmlSchemaException("This is an unknown facet.", xmlSchemaFacet);
			}
		}
		facetsCompiler.FinishFacetCompile();
		facetsCompiler.CompileFacetCombinations();
		return restrictionFacets;
	}

	internal static decimal Power(int x, int y)
	{
		decimal result = 1m;
		decimal num = x;
		if (y > 28)
		{
			return decimal.MaxValue;
		}
		for (int i = 0; i < y; i++)
		{
			result *= num;
		}
		return result;
	}
}
