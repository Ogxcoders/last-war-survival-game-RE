using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml;

internal class DtdParser : IDtdParser
{
	private enum Token
	{
		CDATA,
		ID,
		IDREF,
		IDREFS,
		ENTITY,
		ENTITIES,
		NMTOKEN,
		NMTOKENS,
		NOTATION,
		None,
		PERef,
		AttlistDecl,
		ElementDecl,
		EntityDecl,
		NotationDecl,
		Comment,
		PI,
		CondSectionStart,
		CondSectionEnd,
		Eof,
		REQUIRED,
		IMPLIED,
		FIXED,
		QName,
		Name,
		Nmtoken,
		Quote,
		LeftParen,
		RightParen,
		GreaterThan,
		Or,
		LeftBracket,
		RightBracket,
		PUBLIC,
		SYSTEM,
		Literal,
		DOCTYPE,
		NData,
		Percent,
		Star,
		QMark,
		Plus,
		PCDATA,
		Comma,
		ANY,
		EMPTY,
		IGNORE,
		INCLUDE
	}

	private enum ScanningFunction
	{
		SubsetContent,
		Name,
		QName,
		Nmtoken,
		Doctype1,
		Doctype2,
		Element1,
		Element2,
		Element3,
		Element4,
		Element5,
		Element6,
		Element7,
		Attlist1,
		Attlist2,
		Attlist3,
		Attlist4,
		Attlist5,
		Attlist6,
		Attlist7,
		Entity1,
		Entity2,
		Entity3,
		Notation1,
		CondSection1,
		CondSection2,
		CondSection3,
		Literal,
		SystemId,
		PublicId1,
		PublicId2,
		ClosingTag,
		ParamEntitySpace,
		None
	}

	private enum LiteralType
	{
		AttributeValue,
		EntityReplText,
		SystemOrPublicID
	}

	private class UndeclaredNotation
	{
		internal string name;

		internal int lineNo;

		internal int linePos;

		internal UndeclaredNotation next;

		internal UndeclaredNotation(string name, int lineNo, int linePos)
		{
			this.name = name;
			this.lineNo = lineNo;
			this.linePos = linePos;
			next = null;
		}
	}

	private class ParseElementOnlyContent_LocalFrame
	{
		public int startParenEntityId;

		public Token parsingSchema;

		public ParseElementOnlyContent_LocalFrame(int startParentEntityIdParam)
		{
			startParenEntityId = startParentEntityIdParam;
			parsingSchema = Token.None;
		}
	}

	private IDtdParserAdapter readerAdapter;

	private IDtdParserAdapterWithValidation readerAdapterWithValidation;

	private XmlNameTable nameTable;

	private SchemaInfo schemaInfo;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	private string systemId = string.Empty;

	private string publicId = string.Empty;

	private bool normalize = true;

	private bool validate;

	private bool supportNamespaces = true;

	private bool v1Compat;

	private char[] chars;

	private int charsUsed;

	private int curPos;

	private ScanningFunction scanningFunction;

	private ScanningFunction nextScaningFunction;

	private ScanningFunction savedScanningFunction;

	private bool whitespaceSeen;

	private int tokenStartPos;

	private int colonPos;

	private StringBuilder internalSubsetValueSb;

	private int externalEntitiesDepth;

	private int currentEntityId;

	private bool freeFloatingDtd;

	private bool hasFreeFloatingInternalSubset;

	private StringBuilder stringBuilder;

	private int condSectionDepth;

	private LineInfo literalLineInfo = new LineInfo(0, 0);

	private char literalQuoteChar = '"';

	private string documentBaseUri = string.Empty;

	private string externalDtdBaseUri = string.Empty;

	private Dictionary<string, UndeclaredNotation> undeclaredNotations;

	private int[] condSectionEntityIds;

	private const int CondSectionEntityIdsInitialSize = 2;

	private bool ParsingInternalSubset => externalEntitiesDepth == 0;

	private bool IgnoreEntityReferences => scanningFunction == ScanningFunction.CondSection3;

	private bool SaveInternalSubsetValue
	{
		get
		{
			if (readerAdapter.EntityStackLength == 0)
			{
				return internalSubsetValueSb != null;
			}
			return false;
		}
	}

	private bool ParsingTopLevelMarkup
	{
		get
		{
			if (scanningFunction != ScanningFunction.SubsetContent)
			{
				if (scanningFunction == ScanningFunction.ParamEntitySpace)
				{
					return savedScanningFunction == ScanningFunction.SubsetContent;
				}
				return false;
			}
			return true;
		}
	}

	private bool SupportNamespaces => supportNamespaces;

	private bool Normalize => normalize;

	private int LineNo => readerAdapter.LineNo;

	private int LinePos => curPos - readerAdapter.LineStartPosition;

	private string BaseUriStr
	{
		get
		{
			Uri baseUri = readerAdapter.BaseUri;
			if (!(baseUri != null))
			{
				return string.Empty;
			}
			return baseUri.ToString();
		}
	}

	static DtdParser()
	{
	}

	private DtdParser()
	{
	}

	internal static IDtdParser Create()
	{
		return new DtdParser();
	}

	private void Initialize(IDtdParserAdapter readerAdapter)
	{
		this.readerAdapter = readerAdapter;
		readerAdapterWithValidation = readerAdapter as IDtdParserAdapterWithValidation;
		nameTable = readerAdapter.NameTable;
		if (readerAdapter is IDtdParserAdapterWithValidation dtdParserAdapterWithValidation)
		{
			validate = dtdParserAdapterWithValidation.DtdValidation;
		}
		if (readerAdapter is IDtdParserAdapterV1 dtdParserAdapterV)
		{
			v1Compat = dtdParserAdapterV.V1CompatibilityMode;
			normalize = dtdParserAdapterV.Normalization;
			supportNamespaces = dtdParserAdapterV.Namespaces;
		}
		schemaInfo = new SchemaInfo();
		schemaInfo.SchemaType = SchemaType.DTD;
		stringBuilder = new StringBuilder();
		Uri baseUri = readerAdapter.BaseUri;
		if (baseUri != null)
		{
			documentBaseUri = baseUri.ToString();
		}
		freeFloatingDtd = false;
	}

	private void InitializeFreeFloatingDtd(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter adapter)
	{
		Initialize(adapter);
		if (docTypeName == null || docTypeName.Length == 0)
		{
			throw XmlConvert.CreateInvalidNameArgumentException(docTypeName, "docTypeName");
		}
		XmlConvert.VerifyName(docTypeName);
		int num = docTypeName.IndexOf(':');
		if (num == -1)
		{
			schemaInfo.DocTypeName = new XmlQualifiedName(nameTable.Add(docTypeName));
		}
		else
		{
			schemaInfo.DocTypeName = new XmlQualifiedName(nameTable.Add(docTypeName.Substring(0, num)), nameTable.Add(docTypeName.Substring(num + 1)));
		}
		if (systemId != null && systemId.Length > 0)
		{
			int invCharPos;
			if ((invCharPos = xmlCharType.IsOnlyCharData(systemId)) >= 0)
			{
				ThrowInvalidChar(curPos, systemId, invCharPos);
			}
			this.systemId = systemId;
		}
		if (publicId != null && publicId.Length > 0)
		{
			int invCharPos;
			if ((invCharPos = xmlCharType.IsPublicId(publicId)) >= 0)
			{
				ThrowInvalidChar(curPos, publicId, invCharPos);
			}
			this.publicId = publicId;
		}
		if (internalSubset != null && internalSubset.Length > 0)
		{
			readerAdapter.PushInternalDtd(baseUri, internalSubset);
			hasFreeFloatingInternalSubset = true;
		}
		Uri baseUri2 = readerAdapter.BaseUri;
		if (baseUri2 != null)
		{
			documentBaseUri = baseUri2.ToString();
		}
		freeFloatingDtd = true;
	}

	IDtdInfo IDtdParser.ParseInternalDtd(IDtdParserAdapter adapter, bool saveInternalSubset)
	{
		Initialize(adapter);
		Parse(saveInternalSubset);
		return schemaInfo;
	}

	IDtdInfo IDtdParser.ParseFreeFloatingDtd(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter adapter)
	{
		InitializeFreeFloatingDtd(baseUri, docTypeName, publicId, systemId, internalSubset, adapter);
		Parse(saveInternalSubset: false);
		return schemaInfo;
	}

	private void Parse(bool saveInternalSubset)
	{
		if (freeFloatingDtd)
		{
			ParseFreeFloatingDtd();
		}
		else
		{
			ParseInDocumentDtd(saveInternalSubset);
		}
		schemaInfo.Finish();
		if (!validate || undeclaredNotations == null)
		{
			return;
		}
		foreach (UndeclaredNotation value in undeclaredNotations.Values)
		{
			for (UndeclaredNotation undeclaredNotation = value; undeclaredNotation != null; undeclaredNotation = undeclaredNotation.next)
			{
				SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("The '{0}' notation is not declared.", value.name, BaseUriStr, value.lineNo, value.linePos));
			}
		}
	}

	private void ParseInDocumentDtd(bool saveInternalSubset)
	{
		LoadParsingBuffer();
		scanningFunction = ScanningFunction.QName;
		nextScaningFunction = ScanningFunction.Doctype1;
		if (GetToken(needWhiteSpace: false) != Token.QName)
		{
			OnUnexpectedError();
		}
		schemaInfo.DocTypeName = GetNameQualified(canHavePrefix: true);
		Token token = GetToken(needWhiteSpace: false);
		if (token == Token.SYSTEM || token == Token.PUBLIC)
		{
			ParseExternalId(token, Token.DOCTYPE, out publicId, out systemId);
			token = GetToken(needWhiteSpace: false);
		}
		switch (token)
		{
		case Token.LeftBracket:
			if (saveInternalSubset)
			{
				SaveParsingBuffer();
				internalSubsetValueSb = new StringBuilder();
			}
			ParseInternalSubset();
			break;
		default:
			OnUnexpectedError();
			break;
		case Token.GreaterThan:
			break;
		}
		SaveParsingBuffer();
		if (systemId != null && systemId.Length > 0)
		{
			ParseExternalSubset();
		}
	}

	private void ParseFreeFloatingDtd()
	{
		if (hasFreeFloatingInternalSubset)
		{
			LoadParsingBuffer();
			ParseInternalSubset();
			SaveParsingBuffer();
		}
		if (systemId != null && systemId.Length > 0)
		{
			ParseExternalSubset();
		}
	}

	private void ParseInternalSubset()
	{
		ParseSubset();
	}

	private void ParseExternalSubset()
	{
		if (readerAdapter.PushExternalSubset(systemId, publicId))
		{
			Uri baseUri = readerAdapter.BaseUri;
			if (baseUri != null)
			{
				externalDtdBaseUri = baseUri.ToString();
			}
			externalEntitiesDepth++;
			LoadParsingBuffer();
			ParseSubset();
		}
	}

	private void ParseSubset()
	{
		while (true)
		{
			Token token = GetToken(needWhiteSpace: false);
			int num = currentEntityId;
			switch (token)
			{
			case Token.AttlistDecl:
				ParseAttlistDecl();
				break;
			case Token.ElementDecl:
				ParseElementDecl();
				break;
			case Token.EntityDecl:
				ParseEntityDecl();
				break;
			case Token.NotationDecl:
				ParseNotationDecl();
				break;
			case Token.Comment:
				ParseComment();
				break;
			case Token.PI:
				ParsePI();
				break;
			case Token.CondSectionStart:
				if (ParsingInternalSubset)
				{
					Throw(curPos - 3, "A conditional section is not allowed in an internal subset.");
				}
				ParseCondSection();
				num = currentEntityId;
				break;
			case Token.CondSectionEnd:
				if (condSectionDepth > 0)
				{
					condSectionDepth--;
					if (validate && currentEntityId != condSectionEntityIds[condSectionDepth])
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
				}
				else
				{
					Throw(curPos - 3, "']]>' is not expected.");
				}
				break;
			case Token.RightBracket:
				if (ParsingInternalSubset)
				{
					if (condSectionDepth != 0)
					{
						Throw(curPos, "There is an unclosed conditional section.");
					}
					if (internalSubsetValueSb != null)
					{
						SaveParsingBuffer(curPos - 1);
						schemaInfo.InternalDtdSubset = internalSubsetValueSb.ToString();
						internalSubsetValueSb = null;
					}
					if (GetToken(needWhiteSpace: false) != Token.GreaterThan)
					{
						ThrowUnexpectedToken(curPos, ">");
					}
				}
				else
				{
					Throw(curPos, "Expected DTD markup was not found.");
				}
				return;
			case Token.Eof:
				if (ParsingInternalSubset && !freeFloatingDtd)
				{
					Throw(curPos, "Incomplete DTD content.");
				}
				if (condSectionDepth != 0)
				{
					Throw(curPos, "There is an unclosed conditional section.");
				}
				return;
			}
			if (currentEntityId != num)
			{
				if (validate)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				else if (!v1Compat)
				{
					Throw(curPos, "The parameter entity replacement text must nest properly within markup declarations.");
				}
			}
		}
	}

	private void ParseAttlistDecl()
	{
		if (GetToken(needWhiteSpace: true) == Token.QName)
		{
			XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: true);
			if (!schemaInfo.ElementDecls.TryGetValue(nameQualified, out var value) && !schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out value))
			{
				value = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
				schemaInfo.UndeclaredElementDecls.Add(nameQualified, value);
			}
			SchemaAttDef schemaAttDef = null;
			while (true)
			{
				switch (GetToken(needWhiteSpace: false))
				{
				case Token.QName:
				{
					XmlQualifiedName nameQualified2 = GetNameQualified(canHavePrefix: true);
					schemaAttDef = new SchemaAttDef(nameQualified2, nameQualified2.Namespace);
					schemaAttDef.IsDeclaredInExternal = !ParsingInternalSubset;
					schemaAttDef.LineNumber = LineNo;
					schemaAttDef.LinePosition = LinePos - (curPos - tokenStartPos);
					bool flag = value.GetAttDef(schemaAttDef.Name) != null;
					ParseAttlistType(schemaAttDef, value, flag);
					ParseAttlistDefault(schemaAttDef, flag);
					if (schemaAttDef.Prefix.Length > 0 && schemaAttDef.Prefix.Equals("xml"))
					{
						if (schemaAttDef.Name.Name == "space")
						{
							if (v1Compat)
							{
								string text = schemaAttDef.DefaultValueExpanded.Trim();
								if (text.Equals("preserve") || text.Equals("default"))
								{
									schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								}
							}
							else
							{
								schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								if (schemaAttDef.TokenizedType != XmlTokenizedType.ENUMERATION)
								{
									Throw("Enumeration data type required.", string.Empty, schemaAttDef.LineNumber, schemaAttDef.LinePosition);
								}
								if (validate)
								{
									schemaAttDef.CheckXmlSpace(readerAdapterWithValidation.ValidationEventHandling);
								}
							}
						}
						else if (schemaAttDef.Name.Name == "lang")
						{
							schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlLang;
						}
					}
					if (!flag)
					{
						value.AddAttDef(schemaAttDef);
					}
					continue;
				}
				case Token.GreaterThan:
					if (v1Compat && schemaAttDef != null && schemaAttDef.Prefix.Length > 0 && schemaAttDef.Prefix.Equals("xml") && schemaAttDef.Name.Name == "space")
					{
						schemaAttDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
						if (schemaAttDef.Datatype.TokenizedType != XmlTokenizedType.ENUMERATION)
						{
							Throw("Enumeration data type required.", string.Empty, schemaAttDef.LineNumber, schemaAttDef.LinePosition);
						}
						if (validate)
						{
							schemaAttDef.CheckXmlSpace(readerAdapterWithValidation.ValidationEventHandling);
						}
					}
					return;
				}
				break;
			}
		}
		OnUnexpectedError();
	}

	private void ParseAttlistType(SchemaAttDef attrDef, SchemaElementDecl elementDecl, bool ignoreErrors)
	{
		Token token = GetToken(needWhiteSpace: true);
		if (token != Token.CDATA)
		{
			elementDecl.HasNonCDataAttribute = true;
		}
		if (IsAttributeValueType(token))
		{
			attrDef.TokenizedType = (XmlTokenizedType)token;
			attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
			switch (token)
			{
			default:
				return;
			case Token.ID:
				if (validate && elementDecl.IsIdDeclared)
				{
					SchemaAttDef attDef = elementDecl.GetAttDef(attrDef.Name);
					if ((attDef == null || attDef.Datatype.TokenizedType != XmlTokenizedType.ID) && !ignoreErrors)
					{
						SendValidationEvent(XmlSeverityType.Error, "The attribute of type ID is already declared on the '{0}' element.", elementDecl.Name.ToString());
					}
				}
				elementDecl.IsIdDeclared = true;
				return;
			case Token.NOTATION:
				break;
			}
			if (validate)
			{
				if (elementDecl.IsNotationDeclared && !ignoreErrors)
				{
					SendValidationEvent(curPos - 8, XmlSeverityType.Error, "No element type can have more than one NOTATION attribute specified.", elementDecl.Name.ToString());
				}
				else
				{
					if (elementDecl.ContentValidator != null && elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Empty && !ignoreErrors)
					{
						SendValidationEvent(curPos - 8, XmlSeverityType.Error, "An attribute of type NOTATION must not be declared on an element declared EMPTY.", elementDecl.Name.ToString());
					}
					elementDecl.IsNotationDeclared = true;
				}
			}
			if (GetToken(needWhiteSpace: true) == Token.LeftParen && GetToken(needWhiteSpace: false) == Token.Name)
			{
				do
				{
					string nameString = GetNameString();
					if (!schemaInfo.Notations.ContainsKey(nameString))
					{
						AddUndeclaredNotation(nameString);
					}
					if (validate && !v1Compat && attrDef.Values != null && attrDef.Values.Contains(nameString) && !ignoreErrors)
					{
						SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate notation value.", nameString, BaseUriStr, LineNo, LinePos));
					}
					attrDef.AddValue(nameString);
					switch (GetToken(needWhiteSpace: false))
					{
					case Token.Or:
						continue;
					case Token.RightParen:
						return;
					}
					break;
				}
				while (GetToken(needWhiteSpace: false) == Token.Name);
			}
		}
		else if (token == Token.LeftParen)
		{
			attrDef.TokenizedType = XmlTokenizedType.ENUMERATION;
			attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
			if (GetToken(needWhiteSpace: false) == Token.Nmtoken)
			{
				attrDef.AddValue(GetNameString());
				while (true)
				{
					string nmtokenString;
					switch (GetToken(needWhiteSpace: false))
					{
					case Token.Or:
						if (GetToken(needWhiteSpace: false) == Token.Nmtoken)
						{
							nmtokenString = GetNmtokenString();
							if (validate && !v1Compat && attrDef.Values != null && attrDef.Values.Contains(nmtokenString) && !ignoreErrors)
							{
								SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate enumeration value.", nmtokenString, BaseUriStr, LineNo, LinePos));
							}
							goto IL_0275;
						}
						break;
					case Token.RightParen:
						return;
					}
					break;
					IL_0275:
					attrDef.AddValue(nmtokenString);
				}
			}
		}
		OnUnexpectedError();
	}

	private void ParseAttlistDefault(SchemaAttDef attrDef, bool ignoreErrors)
	{
		switch (GetToken(needWhiteSpace: true))
		{
		case Token.REQUIRED:
			attrDef.Presence = SchemaDeclBase.Use.Required;
			return;
		case Token.IMPLIED:
			attrDef.Presence = SchemaDeclBase.Use.Implied;
			return;
		case Token.FIXED:
			attrDef.Presence = SchemaDeclBase.Use.Fixed;
			if (GetToken(needWhiteSpace: true) != Token.Literal)
			{
				break;
			}
			goto case Token.Literal;
		case Token.Literal:
			if (validate && attrDef.Datatype.TokenizedType == XmlTokenizedType.ID && !ignoreErrors)
			{
				SendValidationEvent(curPos, XmlSeverityType.Error, "An attribute of type ID must have a declared default of either #IMPLIED or #REQUIRED.", string.Empty);
			}
			if (attrDef.TokenizedType != XmlTokenizedType.CDATA)
			{
				attrDef.DefaultValueExpanded = GetValueWithStrippedSpaces();
			}
			else
			{
				attrDef.DefaultValueExpanded = GetValue();
			}
			attrDef.ValueLineNumber = literalLineInfo.lineNo;
			attrDef.ValueLinePosition = literalLineInfo.linePos + 1;
			DtdValidator.SetDefaultTypedValue(attrDef, readerAdapter);
			return;
		}
		OnUnexpectedError();
	}

	private void ParseElementDecl()
	{
		if (GetToken(needWhiteSpace: true) == Token.QName)
		{
			SchemaElementDecl value = null;
			XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: true);
			if (schemaInfo.ElementDecls.TryGetValue(nameQualified, out value))
			{
				if (validate)
				{
					SendValidationEvent(curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The '{0}' element has already been declared.", GetNameString());
				}
			}
			else
			{
				if (schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out value))
				{
					schemaInfo.UndeclaredElementDecls.Remove(nameQualified);
				}
				else
				{
					value = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
				}
				schemaInfo.ElementDecls.Add(nameQualified, value);
			}
			value.IsDeclaredInExternal = !ParsingInternalSubset;
			Token token = GetToken(needWhiteSpace: true);
			if (token != Token.LeftParen)
			{
				if (token != Token.ANY)
				{
					if (token != Token.EMPTY)
					{
						goto IL_0181;
					}
					value.ContentValidator = ContentValidator.Empty;
				}
				else
				{
					value.ContentValidator = ContentValidator.Any;
				}
			}
			else
			{
				int startParenEntityId = currentEntityId;
				Token token2 = GetToken(needWhiteSpace: false);
				if (token2 != Token.None)
				{
					if (token2 != Token.PCDATA)
					{
						goto IL_0181;
					}
					ParticleContentValidator particleContentValidator = new ParticleContentValidator(XmlSchemaContentType.Mixed);
					particleContentValidator.Start();
					particleContentValidator.OpenGroup();
					ParseElementMixedContent(particleContentValidator, startParenEntityId);
					value.ContentValidator = particleContentValidator.Finish(useDFA: true);
				}
				else
				{
					ParticleContentValidator particleContentValidator2 = null;
					particleContentValidator2 = new ParticleContentValidator(XmlSchemaContentType.ElementOnly);
					particleContentValidator2.Start();
					particleContentValidator2.OpenGroup();
					ParseElementOnlyContent(particleContentValidator2, startParenEntityId);
					value.ContentValidator = particleContentValidator2.Finish(useDFA: true);
				}
			}
			if (GetToken(needWhiteSpace: false) != Token.GreaterThan)
			{
				ThrowUnexpectedToken(curPos, ">");
			}
			return;
		}
		goto IL_0181;
		IL_0181:
		OnUnexpectedError();
	}

	private void ParseElementOnlyContent(ParticleContentValidator pcv, int startParenEntityId)
	{
		Stack<ParseElementOnlyContent_LocalFrame> stack = new Stack<ParseElementOnlyContent_LocalFrame>();
		ParseElementOnlyContent_LocalFrame parseElementOnlyContent_LocalFrame = new ParseElementOnlyContent_LocalFrame(startParenEntityId);
		stack.Push(parseElementOnlyContent_LocalFrame);
		while (true)
		{
			Token token = GetToken(needWhiteSpace: false);
			if (token != Token.QName)
			{
				if (token != Token.LeftParen)
				{
					if (token != Token.GreaterThan)
					{
						goto IL_0148;
					}
					Throw(curPos, "Invalid content model.");
					goto IL_014e;
				}
				pcv.OpenGroup();
				parseElementOnlyContent_LocalFrame = new ParseElementOnlyContent_LocalFrame(currentEntityId);
				stack.Push(parseElementOnlyContent_LocalFrame);
				continue;
			}
			pcv.AddName(GetNameQualified(canHavePrefix: true), null);
			ParseHowMany(pcv);
			goto IL_0078;
			IL_0148:
			OnUnexpectedError();
			goto IL_014e;
			IL_00f9:
			pcv.CloseGroup();
			if (validate && currentEntityId != parseElementOnlyContent_LocalFrame.startParenEntityId)
			{
				SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
			}
			ParseHowMany(pcv);
			goto IL_014e;
			IL_00cb:
			if (parseElementOnlyContent_LocalFrame.parsingSchema == Token.Comma)
			{
				Throw(curPos, "Invalid content model.");
			}
			pcv.AddChoice();
			parseElementOnlyContent_LocalFrame.parsingSchema = Token.Or;
			continue;
			IL_014e:
			stack.Pop();
			if (stack.Count > 0)
			{
				parseElementOnlyContent_LocalFrame = stack.Peek();
				goto IL_0078;
			}
			break;
			IL_0135:
			Throw(curPos, "Invalid content model.");
			goto IL_014e;
			IL_0078:
			switch (GetToken(needWhiteSpace: false))
			{
			case Token.Comma:
				break;
			case Token.Or:
				goto IL_00cb;
			case Token.RightParen:
				goto IL_00f9;
			case Token.GreaterThan:
				goto IL_0135;
			default:
				goto IL_0148;
			}
			if (parseElementOnlyContent_LocalFrame.parsingSchema == Token.Or)
			{
				Throw(curPos, "Invalid content model.");
			}
			pcv.AddSequence();
			parseElementOnlyContent_LocalFrame.parsingSchema = Token.Comma;
		}
	}

	private void ParseHowMany(ParticleContentValidator pcv)
	{
		switch (GetToken(needWhiteSpace: false))
		{
		case Token.Star:
			pcv.AddStar();
			break;
		case Token.QMark:
			pcv.AddQMark();
			break;
		case Token.Plus:
			pcv.AddPlus();
			break;
		}
	}

	private void ParseElementMixedContent(ParticleContentValidator pcv, int startParenEntityId)
	{
		bool flag = false;
		int num = -1;
		int num2 = currentEntityId;
		while (true)
		{
			switch (GetToken(needWhiteSpace: false))
			{
			case Token.RightParen:
				pcv.CloseGroup();
				if (validate && currentEntityId != startParenEntityId)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (GetToken(needWhiteSpace: false) == Token.Star && flag)
				{
					pcv.AddStar();
				}
				else if (flag)
				{
					ThrowUnexpectedToken(curPos, "*");
				}
				return;
			case Token.Or:
			{
				if (!flag)
				{
					flag = true;
				}
				else
				{
					pcv.AddChoice();
				}
				if (validate)
				{
					num = currentEntityId;
					if (num2 < num)
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
				}
				if (GetToken(needWhiteSpace: false) != Token.QName)
				{
					break;
				}
				XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: true);
				if (pcv.Exists(nameQualified) && validate)
				{
					SendValidationEvent(XmlSeverityType.Error, "The '{0}' element already exists in the content model.", nameQualified.ToString());
				}
				pcv.AddName(nameQualified, null);
				if (validate)
				{
					num2 = currentEntityId;
					if (num2 < num)
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
				}
				continue;
			}
			}
			OnUnexpectedError();
		}
	}

	private void ParseEntityDecl()
	{
		bool flag = false;
		SchemaEntity schemaEntity = null;
		Token token = GetToken(needWhiteSpace: true);
		if (token == Token.Name)
		{
			goto IL_002c;
		}
		if (token == Token.Percent)
		{
			flag = true;
			if (GetToken(needWhiteSpace: true) == Token.Name)
			{
				goto IL_002c;
			}
		}
		goto IL_01d6;
		IL_002c:
		XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: false);
		schemaEntity = new SchemaEntity(nameQualified, flag);
		schemaEntity.BaseURI = BaseUriStr;
		schemaEntity.DeclaredURI = ((externalDtdBaseUri.Length == 0) ? documentBaseUri : externalDtdBaseUri);
		if (flag)
		{
			if (!schemaInfo.ParameterEntities.ContainsKey(nameQualified))
			{
				schemaInfo.ParameterEntities.Add(nameQualified, schemaEntity);
			}
		}
		else if (!schemaInfo.GeneralEntities.ContainsKey(nameQualified))
		{
			schemaInfo.GeneralEntities.Add(nameQualified, schemaEntity);
		}
		schemaEntity.DeclaredInExternal = !ParsingInternalSubset;
		schemaEntity.ParsingInProgress = true;
		Token token2 = GetToken(needWhiteSpace: true);
		if ((uint)(token2 - 33) > 1u)
		{
			if (token2 != Token.Literal)
			{
				goto IL_01d6;
			}
			schemaEntity.Text = GetValue();
			schemaEntity.Line = literalLineInfo.lineNo;
			schemaEntity.Pos = literalLineInfo.linePos;
		}
		else
		{
			ParseExternalId(token2, Token.EntityDecl, out var pubid, out var url);
			schemaEntity.IsExternal = true;
			schemaEntity.Url = url;
			schemaEntity.Pubid = pubid;
			if (GetToken(needWhiteSpace: false) == Token.NData)
			{
				if (flag)
				{
					ThrowUnexpectedToken(curPos - 5, ">");
				}
				if (!whitespaceSeen)
				{
					Throw(curPos - 5, "'{0}' is an unexpected token. Expecting white space.", "NDATA");
				}
				if (GetToken(needWhiteSpace: true) != Token.Name)
				{
					goto IL_01d6;
				}
				schemaEntity.NData = GetNameQualified(canHavePrefix: false);
				string name = schemaEntity.NData.Name;
				if (!schemaInfo.Notations.ContainsKey(name))
				{
					AddUndeclaredNotation(name);
				}
			}
		}
		if (GetToken(needWhiteSpace: false) == Token.GreaterThan)
		{
			schemaEntity.ParsingInProgress = false;
			return;
		}
		goto IL_01d6;
		IL_01d6:
		OnUnexpectedError();
	}

	private void ParseNotationDecl()
	{
		if (GetToken(needWhiteSpace: true) != Token.Name)
		{
			OnUnexpectedError();
		}
		XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: false);
		SchemaNotation schemaNotation = null;
		if (!schemaInfo.Notations.ContainsKey(nameQualified.Name))
		{
			if (undeclaredNotations != null)
			{
				undeclaredNotations.Remove(nameQualified.Name);
			}
			schemaNotation = new SchemaNotation(nameQualified);
			schemaInfo.Notations.Add(schemaNotation.Name.Name, schemaNotation);
		}
		else if (validate)
		{
			SendValidationEvent(curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The notation '{0}' has already been declared.", nameQualified.Name);
		}
		Token token = GetToken(needWhiteSpace: true);
		if (token == Token.SYSTEM || token == Token.PUBLIC)
		{
			ParseExternalId(token, Token.NOTATION, out var pubid, out var systemLiteral);
			if (schemaNotation != null)
			{
				schemaNotation.SystemLiteral = systemLiteral;
				schemaNotation.Pubid = pubid;
			}
		}
		else
		{
			OnUnexpectedError();
		}
		if (GetToken(needWhiteSpace: false) != Token.GreaterThan)
		{
			OnUnexpectedError();
		}
	}

	private void AddUndeclaredNotation(string notationName)
	{
		if (undeclaredNotations == null)
		{
			undeclaredNotations = new Dictionary<string, UndeclaredNotation>();
		}
		UndeclaredNotation undeclaredNotation = new UndeclaredNotation(notationName, LineNo, LinePos - notationName.Length);
		if (undeclaredNotations.TryGetValue(notationName, out var value))
		{
			undeclaredNotation.next = value.next;
			value.next = undeclaredNotation;
		}
		else
		{
			undeclaredNotations.Add(notationName, undeclaredNotation);
		}
	}

	private void ParseComment()
	{
		SaveParsingBuffer();
		try
		{
			if (SaveInternalSubsetValue)
			{
				readerAdapter.ParseComment(internalSubsetValueSb);
				internalSubsetValueSb.Append("-->");
			}
			else
			{
				readerAdapter.ParseComment(null);
			}
		}
		catch (XmlException ex)
		{
			if (!(ex.ResString == "Unexpected end of file while parsing {0} has occurred.") || currentEntityId == 0)
			{
				throw;
			}
			SendValidationEvent(XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", null);
		}
		LoadParsingBuffer();
	}

	private void ParsePI()
	{
		SaveParsingBuffer();
		if (SaveInternalSubsetValue)
		{
			readerAdapter.ParsePI(internalSubsetValueSb);
			internalSubsetValueSb.Append("?>");
		}
		else
		{
			readerAdapter.ParsePI(null);
		}
		LoadParsingBuffer();
	}

	private void ParseCondSection()
	{
		int num = currentEntityId;
		switch (GetToken(needWhiteSpace: false))
		{
		case Token.INCLUDE:
			if (GetToken(needWhiteSpace: false) == Token.LeftBracket)
			{
				if (validate && num != currentEntityId)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (validate)
				{
					if (condSectionEntityIds == null)
					{
						condSectionEntityIds = new int[2];
					}
					else if (condSectionEntityIds.Length == condSectionDepth)
					{
						int[] destinationArray = new int[condSectionEntityIds.Length * 2];
						Array.Copy(condSectionEntityIds, 0, destinationArray, 0, condSectionEntityIds.Length);
						condSectionEntityIds = destinationArray;
					}
					condSectionEntityIds[condSectionDepth] = num;
				}
				condSectionDepth++;
				break;
			}
			goto default;
		case Token.IGNORE:
			if (GetToken(needWhiteSpace: false) == Token.LeftBracket)
			{
				if (validate && num != currentEntityId)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (GetToken(needWhiteSpace: false) == Token.CondSectionEnd)
				{
					if (validate && num != currentEntityId)
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					break;
				}
			}
			goto default;
		default:
			OnUnexpectedError();
			break;
		}
	}

	private void ParseExternalId(Token idTokenType, Token declType, out string publicId, out string systemId)
	{
		LineInfo keywordLineInfo = new LineInfo(LineNo, LinePos - 6);
		publicId = null;
		systemId = null;
		if (GetToken(needWhiteSpace: true) != Token.Literal)
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
		if (idTokenType == Token.SYSTEM)
		{
			systemId = GetValue();
			if (systemId.IndexOf('#') >= 0)
			{
				Throw(curPos - systemId.Length - 1, "Fragment identifier '{0}' cannot be part of the system identifier '{1}'.", new string[2]
				{
					systemId.Substring(systemId.IndexOf('#')),
					systemId
				});
			}
			if (declType == Token.DOCTYPE && !freeFloatingDtd)
			{
				literalLineInfo.linePos++;
				readerAdapter.OnSystemId(systemId, keywordLineInfo, literalLineInfo);
			}
			return;
		}
		publicId = GetValue();
		int num;
		if ((num = xmlCharType.IsPublicId(publicId)) >= 0)
		{
			ThrowInvalidChar(curPos - 1 - publicId.Length + num, publicId, num);
		}
		if (declType == Token.DOCTYPE && !freeFloatingDtd)
		{
			literalLineInfo.linePos++;
			readerAdapter.OnPublicId(publicId, keywordLineInfo, literalLineInfo);
			if (GetToken(needWhiteSpace: false) == Token.Literal)
			{
				if (!whitespaceSeen)
				{
					Throw("'{0}' is an unexpected token. Expecting white space.", new string(literalQuoteChar, 1), literalLineInfo.lineNo, literalLineInfo.linePos);
				}
				systemId = GetValue();
				literalLineInfo.linePos++;
				readerAdapter.OnSystemId(systemId, keywordLineInfo, literalLineInfo);
			}
			else
			{
				ThrowUnexpectedToken(curPos, "\"", "'");
			}
		}
		else if (GetToken(needWhiteSpace: false) == Token.Literal)
		{
			if (!whitespaceSeen)
			{
				Throw("'{0}' is an unexpected token. Expecting white space.", new string(literalQuoteChar, 1), literalLineInfo.lineNo, literalLineInfo.linePos);
			}
			systemId = GetValue();
		}
		else if (declType != Token.NOTATION)
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
	}

	private Token GetToken(bool needWhiteSpace)
	{
		whitespaceSeen = false;
		while (true)
		{
			switch (chars[curPos])
			{
			case '\0':
				if (curPos != charsUsed)
				{
					ThrowInvalidChar(chars, charsUsed, curPos);
				}
				break;
			case '\n':
				whitespaceSeen = true;
				curPos++;
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\r':
				whitespaceSeen = true;
				if (chars[curPos + 1] == '\n')
				{
					if (Normalize)
					{
						SaveParsingBuffer();
						readerAdapter.CurrentPosition++;
					}
					curPos += 2;
				}
				else
				{
					if (curPos + 1 >= charsUsed && !readerAdapter.IsEof)
					{
						break;
					}
					chars[curPos] = '\n';
					curPos++;
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\t':
			case ' ':
				whitespaceSeen = true;
				curPos++;
				continue;
			case '%':
				if (charsUsed - curPos < 2)
				{
					break;
				}
				if (!xmlCharType.IsWhiteSpace(chars[curPos + 1]))
				{
					if (IgnoreEntityReferences)
					{
						curPos++;
					}
					else
					{
						HandleEntityReference(paramEntity: true, inLiteral: false, inAttribute: false);
					}
					continue;
				}
				goto default;
			default:
				if (needWhiteSpace && !whitespaceSeen && scanningFunction != ScanningFunction.ParamEntitySpace)
				{
					Throw(curPos, "'{0}' is an unexpected token. Expecting white space.", ParseUnexpectedToken(curPos));
				}
				tokenStartPos = curPos;
				while (true)
				{
					switch (scanningFunction)
					{
					case ScanningFunction.Name:
						return ScanNameExpected();
					case ScanningFunction.QName:
						return ScanQNameExpected();
					case ScanningFunction.Nmtoken:
						return ScanNmtokenExpected();
					case ScanningFunction.SubsetContent:
						return ScanSubsetContent();
					case ScanningFunction.Doctype1:
						return ScanDoctype1();
					case ScanningFunction.Doctype2:
						return ScanDoctype2();
					case ScanningFunction.Element1:
						return ScanElement1();
					case ScanningFunction.Element2:
						return ScanElement2();
					case ScanningFunction.Element3:
						return ScanElement3();
					case ScanningFunction.Element4:
						return ScanElement4();
					case ScanningFunction.Element5:
						return ScanElement5();
					case ScanningFunction.Element6:
						return ScanElement6();
					case ScanningFunction.Element7:
						return ScanElement7();
					case ScanningFunction.Attlist1:
						return ScanAttlist1();
					case ScanningFunction.Attlist2:
						return ScanAttlist2();
					case ScanningFunction.Attlist3:
						return ScanAttlist3();
					case ScanningFunction.Attlist4:
						return ScanAttlist4();
					case ScanningFunction.Attlist5:
						return ScanAttlist5();
					case ScanningFunction.Attlist6:
						return ScanAttlist6();
					case ScanningFunction.Attlist7:
						return ScanAttlist7();
					case ScanningFunction.Notation1:
						return ScanNotation1();
					case ScanningFunction.SystemId:
						return ScanSystemId();
					case ScanningFunction.PublicId1:
						return ScanPublicId1();
					case ScanningFunction.PublicId2:
						return ScanPublicId2();
					case ScanningFunction.Entity1:
						return ScanEntity1();
					case ScanningFunction.Entity2:
						return ScanEntity2();
					case ScanningFunction.Entity3:
						return ScanEntity3();
					case ScanningFunction.CondSection1:
						return ScanCondSection1();
					case ScanningFunction.CondSection2:
						return ScanCondSection2();
					case ScanningFunction.CondSection3:
						return ScanCondSection3();
					case ScanningFunction.ClosingTag:
						return ScanClosingTag();
					case ScanningFunction.ParamEntitySpace:
						break;
					default:
						return Token.None;
					}
					whitespaceSeen = true;
					scanningFunction = savedScanningFunction;
				}
			}
			if ((readerAdapter.IsEof || ReadData() == 0) && !HandleEntityEnd(inLiteral: false))
			{
				if (scanningFunction == ScanningFunction.SubsetContent)
				{
					break;
				}
				Throw(curPos, "Incomplete DTD content.");
			}
		}
		return Token.Eof;
	}

	private Token ScanSubsetContent()
	{
		while (true)
		{
			char c = chars[curPos];
			if (c != '<')
			{
				if (c != ']')
				{
					goto IL_04f3;
				}
				if (charsUsed - curPos >= 2 || readerAdapter.IsEof)
				{
					if (chars[curPos + 1] != ']')
					{
						curPos++;
						scanningFunction = ScanningFunction.ClosingTag;
						return Token.RightBracket;
					}
					if (charsUsed - curPos >= 3 || readerAdapter.IsEof)
					{
						if (chars[curPos + 1] == ']' && chars[curPos + 2] == '>')
						{
							break;
						}
						goto IL_04f3;
					}
				}
			}
			else
			{
				switch (chars[curPos + 1])
				{
				case '!':
					switch (chars[curPos + 2])
					{
					case 'E':
						if (chars[curPos + 3] == 'L')
						{
							if (charsUsed - curPos >= 9)
							{
								if (chars[curPos + 4] != 'E' || chars[curPos + 5] != 'M' || chars[curPos + 6] != 'E' || chars[curPos + 7] != 'N' || chars[curPos + 8] != 'T')
								{
									Throw(curPos, "Expected DTD markup was not found.");
								}
								curPos += 9;
								scanningFunction = ScanningFunction.QName;
								nextScaningFunction = ScanningFunction.Element1;
								return Token.ElementDecl;
							}
						}
						else if (chars[curPos + 3] == 'N')
						{
							if (charsUsed - curPos >= 8)
							{
								if (chars[curPos + 4] != 'T' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'T' || chars[curPos + 7] != 'Y')
								{
									Throw(curPos, "Expected DTD markup was not found.");
								}
								curPos += 8;
								scanningFunction = ScanningFunction.Entity1;
								return Token.EntityDecl;
							}
						}
						else if (charsUsed - curPos >= 4)
						{
							Throw(curPos, "Expected DTD markup was not found.");
							return Token.None;
						}
						break;
					case 'A':
						if (charsUsed - curPos >= 9)
						{
							if (chars[curPos + 3] != 'T' || chars[curPos + 4] != 'T' || chars[curPos + 5] != 'L' || chars[curPos + 6] != 'I' || chars[curPos + 7] != 'S' || chars[curPos + 8] != 'T')
							{
								Throw(curPos, "Expected DTD markup was not found.");
							}
							curPos += 9;
							scanningFunction = ScanningFunction.QName;
							nextScaningFunction = ScanningFunction.Attlist1;
							return Token.AttlistDecl;
						}
						break;
					case 'N':
						if (charsUsed - curPos >= 10)
						{
							if (chars[curPos + 3] != 'O' || chars[curPos + 4] != 'T' || chars[curPos + 5] != 'A' || chars[curPos + 6] != 'T' || chars[curPos + 7] != 'I' || chars[curPos + 8] != 'O' || chars[curPos + 9] != 'N')
							{
								Throw(curPos, "Expected DTD markup was not found.");
							}
							curPos += 10;
							scanningFunction = ScanningFunction.Name;
							nextScaningFunction = ScanningFunction.Notation1;
							return Token.NotationDecl;
						}
						break;
					case '[':
						curPos += 3;
						scanningFunction = ScanningFunction.CondSection1;
						return Token.CondSectionStart;
					case '-':
						if (chars[curPos + 3] == '-')
						{
							curPos += 4;
							return Token.Comment;
						}
						if (charsUsed - curPos >= 4)
						{
							Throw(curPos, "Expected DTD markup was not found.");
						}
						break;
					default:
						if (charsUsed - curPos >= 3)
						{
							Throw(curPos + 2, "Expected DTD markup was not found.");
						}
						break;
					}
					break;
				case '?':
					curPos += 2;
					return Token.PI;
				default:
					if (charsUsed - curPos >= 2)
					{
						Throw(curPos, "Expected DTD markup was not found.");
						return Token.None;
					}
					break;
				}
			}
			goto IL_0513;
			IL_0513:
			if (ReadData() == 0)
			{
				Throw(charsUsed, "Incomplete DTD content.");
			}
			continue;
			IL_04f3:
			if (charsUsed - curPos != 0)
			{
				Throw(curPos, "Expected DTD markup was not found.");
			}
			goto IL_0513;
		}
		curPos += 3;
		return Token.CondSectionEnd;
	}

	private Token ScanNameExpected()
	{
		ScanName();
		scanningFunction = nextScaningFunction;
		return Token.Name;
	}

	private Token ScanQNameExpected()
	{
		ScanQName();
		scanningFunction = nextScaningFunction;
		return Token.QName;
	}

	private Token ScanNmtokenExpected()
	{
		ScanNmtoken();
		scanningFunction = nextScaningFunction;
		return Token.Nmtoken;
	}

	private Token ScanDoctype1()
	{
		switch (chars[curPos])
		{
		case 'P':
			if (!EatPublicKeyword())
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Doctype2;
			scanningFunction = ScanningFunction.PublicId1;
			return Token.PUBLIC;
		case 'S':
			if (!EatSystemKeyword())
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Doctype2;
			scanningFunction = ScanningFunction.SystemId;
			return Token.SYSTEM;
		case '[':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.LeftBracket;
		case '>':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		default:
			Throw(curPos, "Expecting external ID, '[' or '>'.");
			return Token.None;
		}
	}

	private Token ScanDoctype2()
	{
		switch (chars[curPos])
		{
		case '[':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.LeftBracket;
		case '>':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		default:
			Throw(curPos, "Expecting an internal subset or the end of the DOCTYPE declaration.");
			return Token.None;
		}
	}

	private Token ScanClosingTag()
	{
		if (chars[curPos] != '>')
		{
			ThrowUnexpectedToken(curPos, ">");
		}
		curPos++;
		scanningFunction = ScanningFunction.SubsetContent;
		return Token.GreaterThan;
	}

	private Token ScanElement1()
	{
		while (true)
		{
			char c = chars[curPos];
			if (c != '(')
			{
				if (c != 'A')
				{
					if (c == 'E')
					{
						if (charsUsed - curPos < 5)
						{
							goto IL_011b;
						}
						if (chars[curPos + 1] == 'M' && chars[curPos + 2] == 'P' && chars[curPos + 3] == 'T' && chars[curPos + 4] == 'Y')
						{
							curPos += 5;
							scanningFunction = ScanningFunction.ClosingTag;
							return Token.EMPTY;
						}
					}
				}
				else
				{
					if (charsUsed - curPos < 3)
					{
						goto IL_011b;
					}
					if (chars[curPos + 1] == 'N' && chars[curPos + 2] == 'Y')
					{
						break;
					}
				}
				Throw(curPos, "Invalid content model.");
				goto IL_011b;
			}
			scanningFunction = ScanningFunction.Element2;
			curPos++;
			return Token.LeftParen;
			IL_011b:
			if (ReadData() == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
		curPos += 3;
		scanningFunction = ScanningFunction.ClosingTag;
		return Token.ANY;
	}

	private Token ScanElement2()
	{
		if (chars[curPos] == '#')
		{
			while (charsUsed - curPos < 7)
			{
				if (ReadData() == 0)
				{
					Throw(curPos, "Incomplete DTD content.");
				}
			}
			if (chars[curPos + 1] == 'P' && chars[curPos + 2] == 'C' && chars[curPos + 3] == 'D' && chars[curPos + 4] == 'A' && chars[curPos + 5] == 'T' && chars[curPos + 6] == 'A')
			{
				curPos += 7;
				scanningFunction = ScanningFunction.Element6;
				return Token.PCDATA;
			}
			Throw(curPos + 1, "Expecting 'PCDATA'.");
		}
		scanningFunction = ScanningFunction.Element3;
		return Token.None;
	}

	private Token ScanElement3()
	{
		switch (chars[curPos])
		{
		case '(':
			curPos++;
			return Token.LeftParen;
		case '>':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		default:
			ScanQName();
			scanningFunction = ScanningFunction.Element4;
			return Token.QName;
		}
	}

	private Token ScanElement4()
	{
		scanningFunction = ScanningFunction.Element5;
		Token result;
		switch (chars[curPos])
		{
		case '*':
			result = Token.Star;
			break;
		case '?':
			result = Token.QMark;
			break;
		case '+':
			result = Token.Plus;
			break;
		default:
			return Token.None;
		}
		if (whitespaceSeen)
		{
			Throw(curPos, "White space not allowed before '?', '*', or '+'.");
		}
		curPos++;
		return result;
	}

	private Token ScanElement5()
	{
		switch (chars[curPos])
		{
		case ',':
			curPos++;
			scanningFunction = ScanningFunction.Element3;
			return Token.Comma;
		case '|':
			curPos++;
			scanningFunction = ScanningFunction.Element3;
			return Token.Or;
		case ')':
			curPos++;
			scanningFunction = ScanningFunction.Element4;
			return Token.RightParen;
		case '>':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		default:
			Throw(curPos, "Expecting '?', '*', or '+'.");
			return Token.None;
		}
	}

	private Token ScanElement6()
	{
		switch (chars[curPos])
		{
		case ')':
			curPos++;
			scanningFunction = ScanningFunction.Element7;
			return Token.RightParen;
		case '|':
			curPos++;
			nextScaningFunction = ScanningFunction.Element6;
			scanningFunction = ScanningFunction.QName;
			return Token.Or;
		default:
			ThrowUnexpectedToken(curPos, ")", "|");
			return Token.None;
		}
	}

	private Token ScanElement7()
	{
		scanningFunction = ScanningFunction.ClosingTag;
		if (chars[curPos] == '*' && !whitespaceSeen)
		{
			curPos++;
			return Token.Star;
		}
		return Token.None;
	}

	private Token ScanAttlist1()
	{
		if (chars[curPos] == '>')
		{
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		}
		if (!whitespaceSeen)
		{
			Throw(curPos, "'{0}' is an unexpected token. Expecting white space.", ParseUnexpectedToken(curPos));
		}
		ScanQName();
		scanningFunction = ScanningFunction.Attlist2;
		return Token.QName;
	}

	private Token ScanAttlist2()
	{
		while (true)
		{
			switch (chars[curPos])
			{
			case '(':
				curPos++;
				scanningFunction = ScanningFunction.Nmtoken;
				nextScaningFunction = ScanningFunction.Attlist5;
				return Token.LeftParen;
			case 'C':
				if (charsUsed - curPos >= 5)
				{
					if (chars[curPos + 1] != 'D' || chars[curPos + 2] != 'A' || chars[curPos + 3] != 'T' || chars[curPos + 4] != 'A')
					{
						Throw(curPos, "Invalid attribute type.");
					}
					curPos += 5;
					scanningFunction = ScanningFunction.Attlist6;
					return Token.CDATA;
				}
				break;
			case 'E':
				if (charsUsed - curPos < 9)
				{
					break;
				}
				scanningFunction = ScanningFunction.Attlist6;
				if (chars[curPos + 1] != 'N' || chars[curPos + 2] != 'T' || chars[curPos + 3] != 'I' || chars[curPos + 4] != 'T')
				{
					Throw(curPos, "'{0}' is an invalid attribute type.");
				}
				switch (chars[curPos + 5])
				{
				case 'I':
					if (chars[curPos + 6] != 'E' || chars[curPos + 7] != 'S')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					curPos += 8;
					return Token.ENTITIES;
				case 'Y':
					curPos += 6;
					return Token.ENTITY;
				}
				Throw(curPos, "'{0}' is an invalid attribute type.");
				break;
			case 'I':
				if (charsUsed - curPos >= 6)
				{
					scanningFunction = ScanningFunction.Attlist6;
					if (chars[curPos + 1] != 'D')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					if (chars[curPos + 2] != 'R')
					{
						curPos += 2;
						return Token.ID;
					}
					if (chars[curPos + 3] != 'E' || chars[curPos + 4] != 'F')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					if (chars[curPos + 5] != 'S')
					{
						curPos += 5;
						return Token.IDREF;
					}
					curPos += 6;
					return Token.IDREFS;
				}
				break;
			case 'N':
				if (charsUsed - curPos < 8 && !readerAdapter.IsEof)
				{
					break;
				}
				switch (chars[curPos + 1])
				{
				case 'O':
					if (chars[curPos + 2] != 'T' || chars[curPos + 3] != 'A' || chars[curPos + 4] != 'T' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'O' || chars[curPos + 7] != 'N')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					curPos += 8;
					scanningFunction = ScanningFunction.Attlist3;
					return Token.NOTATION;
				case 'M':
					if (chars[curPos + 2] != 'T' || chars[curPos + 3] != 'O' || chars[curPos + 4] != 'K' || chars[curPos + 5] != 'E' || chars[curPos + 6] != 'N')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					scanningFunction = ScanningFunction.Attlist6;
					if (chars[curPos + 7] == 'S')
					{
						curPos += 8;
						return Token.NMTOKENS;
					}
					curPos += 7;
					return Token.NMTOKEN;
				}
				Throw(curPos, "'{0}' is an invalid attribute type.");
				break;
			default:
				Throw(curPos, "'{0}' is an invalid attribute type.");
				break;
			}
			if (ReadData() == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
	}

	private Token ScanAttlist3()
	{
		if (chars[curPos] == '(')
		{
			curPos++;
			scanningFunction = ScanningFunction.Name;
			nextScaningFunction = ScanningFunction.Attlist4;
			return Token.LeftParen;
		}
		ThrowUnexpectedToken(curPos, "(");
		return Token.None;
	}

	private Token ScanAttlist4()
	{
		switch (chars[curPos])
		{
		case ')':
			curPos++;
			scanningFunction = ScanningFunction.Attlist6;
			return Token.RightParen;
		case '|':
			curPos++;
			scanningFunction = ScanningFunction.Name;
			nextScaningFunction = ScanningFunction.Attlist4;
			return Token.Or;
		default:
			ThrowUnexpectedToken(curPos, ")", "|");
			return Token.None;
		}
	}

	private Token ScanAttlist5()
	{
		switch (chars[curPos])
		{
		case ')':
			curPos++;
			scanningFunction = ScanningFunction.Attlist6;
			return Token.RightParen;
		case '|':
			curPos++;
			scanningFunction = ScanningFunction.Nmtoken;
			nextScaningFunction = ScanningFunction.Attlist5;
			return Token.Or;
		default:
			ThrowUnexpectedToken(curPos, ")", "|");
			return Token.None;
		}
	}

	private Token ScanAttlist6()
	{
		while (true)
		{
			switch (chars[curPos])
			{
			case '"':
			case '\'':
				ScanLiteral(LiteralType.AttributeValue);
				scanningFunction = ScanningFunction.Attlist1;
				return Token.Literal;
			case '#':
				if (charsUsed - curPos < 6)
				{
					break;
				}
				switch (chars[curPos + 1])
				{
				case 'R':
					if (charsUsed - curPos >= 9)
					{
						if (chars[curPos + 2] != 'E' || chars[curPos + 3] != 'Q' || chars[curPos + 4] != 'U' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'R' || chars[curPos + 7] != 'E' || chars[curPos + 8] != 'D')
						{
							Throw(curPos, "Expecting an attribute type.");
						}
						curPos += 9;
						scanningFunction = ScanningFunction.Attlist1;
						return Token.REQUIRED;
					}
					break;
				case 'I':
					if (charsUsed - curPos >= 8)
					{
						if (chars[curPos + 2] != 'M' || chars[curPos + 3] != 'P' || chars[curPos + 4] != 'L' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'E' || chars[curPos + 7] != 'D')
						{
							Throw(curPos, "Expecting an attribute type.");
						}
						curPos += 8;
						scanningFunction = ScanningFunction.Attlist1;
						return Token.IMPLIED;
					}
					break;
				case 'F':
					if (chars[curPos + 2] != 'I' || chars[curPos + 3] != 'X' || chars[curPos + 4] != 'E' || chars[curPos + 5] != 'D')
					{
						Throw(curPos, "Expecting an attribute type.");
					}
					curPos += 6;
					scanningFunction = ScanningFunction.Attlist7;
					return Token.FIXED;
				default:
					Throw(curPos, "Expecting an attribute type.");
					break;
				}
				break;
			default:
				Throw(curPos, "Expecting an attribute type.");
				break;
			}
			if (ReadData() == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
	}

	private Token ScanAttlist7()
	{
		char c = chars[curPos];
		if (c == '"' || c == '\'')
		{
			ScanLiteral(LiteralType.AttributeValue);
			scanningFunction = ScanningFunction.Attlist1;
			return Token.Literal;
		}
		ThrowUnexpectedToken(curPos, "\"", "'");
		return Token.None;
	}

	private Token ScanLiteral(LiteralType literalType)
	{
		char c = chars[curPos];
		char value = ((literalType == LiteralType.AttributeValue) ? ' ' : '\n');
		int num = currentEntityId;
		literalLineInfo.Set(LineNo, LinePos);
		curPos++;
		tokenStartPos = curPos;
		stringBuilder.Length = 0;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 0x80) != 0 && chars[curPos] != '%')
			{
				curPos++;
				continue;
			}
			if (chars[curPos] == c && currentEntityId == num)
			{
				break;
			}
			int num2 = curPos - tokenStartPos;
			if (num2 > 0)
			{
				stringBuilder.Append(chars, tokenStartPos, num2);
				tokenStartPos = curPos;
			}
			switch (chars[curPos])
			{
			case '"':
			case '\'':
			case '>':
				curPos++;
				continue;
			case '\n':
				curPos++;
				if (Normalize)
				{
					stringBuilder.Append(value);
					tokenStartPos = curPos;
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\r':
				if (chars[curPos + 1] == '\n')
				{
					if (Normalize)
					{
						if (literalType == LiteralType.AttributeValue)
						{
							stringBuilder.Append(readerAdapter.IsEntityEolNormalized ? "  " : " ");
						}
						else
						{
							stringBuilder.Append(readerAdapter.IsEntityEolNormalized ? "\r\n" : "\n");
						}
						tokenStartPos = curPos + 2;
						SaveParsingBuffer();
						readerAdapter.CurrentPosition++;
					}
					curPos += 2;
				}
				else
				{
					if (curPos + 1 == charsUsed)
					{
						break;
					}
					curPos++;
					if (Normalize)
					{
						stringBuilder.Append(value);
						tokenStartPos = curPos;
					}
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\t':
				if (literalType == LiteralType.AttributeValue && Normalize)
				{
					stringBuilder.Append(' ');
					tokenStartPos++;
				}
				curPos++;
				continue;
			case '<':
				if (literalType == LiteralType.AttributeValue)
				{
					Throw(curPos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
				}
				curPos++;
				continue;
			case '%':
				if (literalType != LiteralType.EntityReplText)
				{
					curPos++;
					continue;
				}
				HandleEntityReference(paramEntity: true, inLiteral: true, literalType == LiteralType.AttributeValue);
				tokenStartPos = curPos;
				continue;
			case '&':
			{
				if (literalType == LiteralType.SystemOrPublicID)
				{
					curPos++;
					continue;
				}
				if (curPos + 1 == charsUsed)
				{
					break;
				}
				if (chars[curPos + 1] == '#')
				{
					SaveParsingBuffer();
					int num3 = readerAdapter.ParseNumericCharRef(SaveInternalSubsetValue ? internalSubsetValueSb : null);
					LoadParsingBuffer();
					stringBuilder.Append(chars, curPos, num3 - curPos);
					readerAdapter.CurrentPosition = num3;
					tokenStartPos = num3;
					curPos = num3;
					continue;
				}
				SaveParsingBuffer();
				if (literalType == LiteralType.AttributeValue)
				{
					int num4 = readerAdapter.ParseNamedCharRef(expand: true, SaveInternalSubsetValue ? internalSubsetValueSb : null);
					LoadParsingBuffer();
					if (num4 >= 0)
					{
						stringBuilder.Append(chars, curPos, num4 - curPos);
						readerAdapter.CurrentPosition = num4;
						tokenStartPos = num4;
						curPos = num4;
					}
					else
					{
						HandleEntityReference(paramEntity: false, inLiteral: true, inAttribute: true);
						tokenStartPos = curPos;
					}
					continue;
				}
				int num5 = readerAdapter.ParseNamedCharRef(expand: false, null);
				LoadParsingBuffer();
				if (num5 >= 0)
				{
					tokenStartPos = curPos;
					curPos = num5;
					continue;
				}
				stringBuilder.Append('&');
				curPos++;
				tokenStartPos = curPos;
				XmlQualifiedName entityName = ScanEntityName();
				VerifyEntityReference(entityName, paramEntity: false, mustBeDeclared: false, inAttribute: false);
				continue;
			}
			default:
				if (curPos == charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[curPos]))
				{
					if (curPos + 1 == charsUsed)
					{
						break;
					}
					curPos++;
					if (XmlCharType.IsLowSurrogate(chars[curPos]))
					{
						curPos++;
						continue;
					}
				}
				ThrowInvalidChar(chars, charsUsed, curPos);
				return Token.None;
			}
			if ((readerAdapter.IsEof || ReadData() == 0) && (literalType == LiteralType.SystemOrPublicID || !HandleEntityEnd(inLiteral: true)))
			{
				Throw(curPos, "There is an unclosed literal string.");
			}
			tokenStartPos = curPos;
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Append(chars, tokenStartPos, curPos - tokenStartPos);
		}
		curPos++;
		literalQuoteChar = c;
		return Token.Literal;
	}

	private XmlQualifiedName ScanEntityName()
	{
		try
		{
			ScanName();
		}
		catch (XmlException ex)
		{
			Throw("An error occurred while parsing EntityName.", string.Empty, ex.LineNumber, ex.LinePosition);
		}
		if (chars[curPos] != ';')
		{
			ThrowUnexpectedToken(curPos, ";");
		}
		XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: false);
		curPos++;
		return nameQualified;
	}

	private Token ScanNotation1()
	{
		switch (chars[curPos])
		{
		case 'P':
			if (!EatPublicKeyword())
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.ClosingTag;
			scanningFunction = ScanningFunction.PublicId1;
			return Token.PUBLIC;
		case 'S':
			if (!EatSystemKeyword())
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.ClosingTag;
			scanningFunction = ScanningFunction.SystemId;
			return Token.SYSTEM;
		default:
			Throw(curPos, "Expecting a system identifier or a public identifier.");
			return Token.None;
		}
	}

	private Token ScanSystemId()
	{
		if (chars[curPos] != '"' && chars[curPos] != '\'')
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
		ScanLiteral(LiteralType.SystemOrPublicID);
		scanningFunction = nextScaningFunction;
		return Token.Literal;
	}

	private Token ScanEntity1()
	{
		if (chars[curPos] == '%')
		{
			curPos++;
			nextScaningFunction = ScanningFunction.Entity2;
			scanningFunction = ScanningFunction.Name;
			return Token.Percent;
		}
		ScanName();
		scanningFunction = ScanningFunction.Entity2;
		return Token.Name;
	}

	private Token ScanEntity2()
	{
		switch (chars[curPos])
		{
		case 'P':
			if (!EatPublicKeyword())
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Entity3;
			scanningFunction = ScanningFunction.PublicId1;
			return Token.PUBLIC;
		case 'S':
			if (!EatSystemKeyword())
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Entity3;
			scanningFunction = ScanningFunction.SystemId;
			return Token.SYSTEM;
		case '"':
		case '\'':
			ScanLiteral(LiteralType.EntityReplText);
			scanningFunction = ScanningFunction.ClosingTag;
			return Token.Literal;
		default:
			Throw(curPos, "Expecting an external identifier or an entity value.");
			return Token.None;
		}
	}

	private Token ScanEntity3()
	{
		if (chars[curPos] == 'N')
		{
			do
			{
				if (charsUsed - curPos >= 5)
				{
					if (chars[curPos + 1] != 'D' || chars[curPos + 2] != 'A' || chars[curPos + 3] != 'T' || chars[curPos + 4] != 'A')
					{
						break;
					}
					curPos += 5;
					scanningFunction = ScanningFunction.Name;
					nextScaningFunction = ScanningFunction.ClosingTag;
					return Token.NData;
				}
			}
			while (ReadData() != 0);
		}
		scanningFunction = ScanningFunction.ClosingTag;
		return Token.None;
	}

	private Token ScanPublicId1()
	{
		if (chars[curPos] != '"' && chars[curPos] != '\'')
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
		ScanLiteral(LiteralType.SystemOrPublicID);
		scanningFunction = ScanningFunction.PublicId2;
		return Token.Literal;
	}

	private Token ScanPublicId2()
	{
		if (chars[curPos] != '"' && chars[curPos] != '\'')
		{
			scanningFunction = nextScaningFunction;
			return Token.None;
		}
		ScanLiteral(LiteralType.SystemOrPublicID);
		scanningFunction = nextScaningFunction;
		return Token.Literal;
	}

	private Token ScanCondSection1()
	{
		if (chars[curPos] != 'I')
		{
			Throw(curPos, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
		}
		curPos++;
		while (true)
		{
			if (charsUsed - curPos >= 5)
			{
				char c = chars[curPos];
				if (c == 'G')
				{
					if (chars[curPos + 1] != 'N' || chars[curPos + 2] != 'O' || chars[curPos + 3] != 'R' || chars[curPos + 4] != 'E' || xmlCharType.IsNameSingleChar(chars[curPos + 5]))
					{
						break;
					}
					nextScaningFunction = ScanningFunction.CondSection3;
					scanningFunction = ScanningFunction.CondSection2;
					curPos += 5;
					return Token.IGNORE;
				}
				if (c != 'N')
				{
					break;
				}
				if (charsUsed - curPos >= 6)
				{
					if (chars[curPos + 1] != 'C' || chars[curPos + 2] != 'L' || chars[curPos + 3] != 'U' || chars[curPos + 4] != 'D' || chars[curPos + 5] != 'E' || xmlCharType.IsNameSingleChar(chars[curPos + 6]))
					{
						break;
					}
					nextScaningFunction = ScanningFunction.SubsetContent;
					scanningFunction = ScanningFunction.CondSection2;
					curPos += 6;
					return Token.INCLUDE;
				}
			}
			if (ReadData() == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
		Throw(curPos - 1, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
		return Token.None;
	}

	private Token ScanCondSection2()
	{
		if (chars[curPos] != '[')
		{
			ThrowUnexpectedToken(curPos, "[");
		}
		curPos++;
		scanningFunction = nextScaningFunction;
		return Token.LeftBracket;
	}

	private Token ScanCondSection3()
	{
		int num = 0;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 0x40) != 0 && chars[curPos] != ']')
			{
				curPos++;
				continue;
			}
			switch (chars[curPos])
			{
			case '\t':
			case '"':
			case '&':
			case '\'':
				curPos++;
				continue;
			case '\n':
				curPos++;
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\r':
				if (chars[curPos + 1] == '\n')
				{
					curPos += 2;
				}
				else
				{
					if (curPos + 1 >= charsUsed && !readerAdapter.IsEof)
					{
						break;
					}
					curPos++;
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '<':
				if (charsUsed - curPos >= 3)
				{
					if (chars[curPos + 1] != '!' || chars[curPos + 2] != '[')
					{
						curPos++;
						continue;
					}
					num++;
					curPos += 3;
					continue;
				}
				break;
			case ']':
				if (charsUsed - curPos < 3)
				{
					break;
				}
				if (chars[curPos + 1] != ']' || chars[curPos + 2] != '>')
				{
					curPos++;
					continue;
				}
				if (num > 0)
				{
					num--;
					curPos += 3;
					continue;
				}
				curPos += 3;
				scanningFunction = ScanningFunction.SubsetContent;
				return Token.CondSectionEnd;
			default:
				if (curPos == charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[curPos]))
				{
					if (curPos + 1 == charsUsed)
					{
						break;
					}
					curPos++;
					if (XmlCharType.IsLowSurrogate(chars[curPos]))
					{
						curPos++;
						continue;
					}
				}
				ThrowInvalidChar(chars, charsUsed, curPos);
				return Token.None;
			}
			if (readerAdapter.IsEof || ReadData() == 0)
			{
				if (HandleEntityEnd(inLiteral: false))
				{
					continue;
				}
				Throw(curPos, "There is an unclosed conditional section.");
			}
			tokenStartPos = curPos;
		}
	}

	private void ScanName()
	{
		ScanQName(isQName: false);
	}

	private void ScanQName()
	{
		ScanQName(SupportNamespaces);
	}

	private void ScanQName(bool isQName)
	{
		tokenStartPos = curPos;
		int num = -1;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 4) != 0 || chars[curPos] == ':')
			{
				curPos++;
			}
			else if (curPos + 1 >= charsUsed)
			{
				if (ReadDataInName())
				{
					continue;
				}
				Throw(curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
			}
			else
			{
				Throw(curPos, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(chars, charsUsed, curPos));
			}
			while (true)
			{
				if ((xmlCharType.charProperties[(uint)chars[curPos]] & 8) != 0)
				{
					curPos++;
					continue;
				}
				if (chars[curPos] == ':')
				{
					if (isQName)
					{
						break;
					}
					curPos++;
					continue;
				}
				if (curPos == charsUsed)
				{
					if (ReadDataInName())
					{
						continue;
					}
					if (tokenStartPos == curPos)
					{
						Throw(curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
					}
				}
				colonPos = ((num == -1) ? (-1) : (tokenStartPos + num));
				return;
			}
			if (num != -1)
			{
				Throw(curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
			}
			num = curPos - tokenStartPos;
			curPos++;
		}
	}

	private bool ReadDataInName()
	{
		int num = curPos - tokenStartPos;
		curPos = tokenStartPos;
		bool result = ReadData() != 0;
		tokenStartPos = curPos;
		curPos += num;
		return result;
	}

	private void ScanNmtoken()
	{
		tokenStartPos = curPos;
		int num;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 8) != 0 || chars[curPos] == ':')
			{
				curPos++;
				continue;
			}
			if (curPos < charsUsed)
			{
				if (curPos - tokenStartPos == 0)
				{
					Throw(curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(chars, charsUsed, curPos));
				}
				return;
			}
			num = curPos - tokenStartPos;
			curPos = tokenStartPos;
			if (ReadData() == 0)
			{
				if (num > 0)
				{
					break;
				}
				Throw(curPos, "Unexpected end of file while parsing {0} has occurred.", "NmToken");
			}
			tokenStartPos = curPos;
			curPos += num;
		}
		tokenStartPos = curPos;
		curPos += num;
	}

	private bool EatPublicKeyword()
	{
		while (charsUsed - curPos < 6)
		{
			if (ReadData() == 0)
			{
				return false;
			}
		}
		if (chars[curPos + 1] != 'U' || chars[curPos + 2] != 'B' || chars[curPos + 3] != 'L' || chars[curPos + 4] != 'I' || chars[curPos + 5] != 'C')
		{
			return false;
		}
		curPos += 6;
		return true;
	}

	private bool EatSystemKeyword()
	{
		while (charsUsed - curPos < 6)
		{
			if (ReadData() == 0)
			{
				return false;
			}
		}
		if (chars[curPos + 1] != 'Y' || chars[curPos + 2] != 'S' || chars[curPos + 3] != 'T' || chars[curPos + 4] != 'E' || chars[curPos + 5] != 'M')
		{
			return false;
		}
		curPos += 6;
		return true;
	}

	private XmlQualifiedName GetNameQualified(bool canHavePrefix)
	{
		if (colonPos == -1)
		{
			return new XmlQualifiedName(nameTable.Add(chars, tokenStartPos, curPos - tokenStartPos));
		}
		if (canHavePrefix)
		{
			return new XmlQualifiedName(nameTable.Add(chars, colonPos + 1, curPos - colonPos - 1), nameTable.Add(chars, tokenStartPos, colonPos - tokenStartPos));
		}
		Throw(tokenStartPos, "'{0}' is an unqualified name and cannot contain the character ':'.", GetNameString());
		return null;
	}

	private string GetNameString()
	{
		return new string(chars, tokenStartPos, curPos - tokenStartPos);
	}

	private string GetNmtokenString()
	{
		return GetNameString();
	}

	private string GetValue()
	{
		if (stringBuilder.Length == 0)
		{
			return new string(chars, tokenStartPos, curPos - tokenStartPos - 1);
		}
		return stringBuilder.ToString();
	}

	private string GetValueWithStrippedSpaces()
	{
		return StripSpaces((stringBuilder.Length == 0) ? new string(chars, tokenStartPos, curPos - tokenStartPos - 1) : stringBuilder.ToString());
	}

	private int ReadData()
	{
		SaveParsingBuffer();
		int result = readerAdapter.ReadData();
		LoadParsingBuffer();
		return result;
	}

	private void LoadParsingBuffer()
	{
		chars = readerAdapter.ParsingBuffer;
		charsUsed = readerAdapter.ParsingBufferLength;
		curPos = readerAdapter.CurrentPosition;
	}

	private void SaveParsingBuffer()
	{
		SaveParsingBuffer(curPos);
	}

	private void SaveParsingBuffer(int internalSubsetValueEndPos)
	{
		if (SaveInternalSubsetValue)
		{
			int currentPosition = readerAdapter.CurrentPosition;
			if (internalSubsetValueEndPos - currentPosition > 0)
			{
				internalSubsetValueSb.Append(chars, currentPosition, internalSubsetValueEndPos - currentPosition);
			}
		}
		readerAdapter.CurrentPosition = curPos;
	}

	private bool HandleEntityReference(bool paramEntity, bool inLiteral, bool inAttribute)
	{
		curPos++;
		return HandleEntityReference(ScanEntityName(), paramEntity, inLiteral, inAttribute);
	}

	private bool HandleEntityReference(XmlQualifiedName entityName, bool paramEntity, bool inLiteral, bool inAttribute)
	{
		SaveParsingBuffer();
		if (paramEntity && ParsingInternalSubset && !ParsingTopLevelMarkup)
		{
			Throw(curPos - entityName.Name.Length - 1, "A parameter entity reference is not allowed in internal markup.");
		}
		SchemaEntity schemaEntity = VerifyEntityReference(entityName, paramEntity, mustBeDeclared: true, inAttribute);
		if (schemaEntity == null)
		{
			return false;
		}
		if (schemaEntity.ParsingInProgress)
		{
			Throw(curPos - entityName.Name.Length - 1, paramEntity ? "Parameter entity '{0}' references itself." : "General entity '{0}' references itself.", entityName.Name);
		}
		int entityId;
		if (schemaEntity.IsExternal)
		{
			if (!readerAdapter.PushEntity(schemaEntity, out entityId))
			{
				return false;
			}
			externalEntitiesDepth++;
		}
		else
		{
			if (schemaEntity.Text.Length == 0)
			{
				return false;
			}
			if (!readerAdapter.PushEntity(schemaEntity, out entityId))
			{
				return false;
			}
		}
		currentEntityId = entityId;
		if (paramEntity && !inLiteral && scanningFunction != ScanningFunction.ParamEntitySpace)
		{
			savedScanningFunction = scanningFunction;
			scanningFunction = ScanningFunction.ParamEntitySpace;
		}
		LoadParsingBuffer();
		return true;
	}

	private bool HandleEntityEnd(bool inLiteral)
	{
		SaveParsingBuffer();
		if (!readerAdapter.PopEntity(out var oldEntity, out currentEntityId))
		{
			return false;
		}
		LoadParsingBuffer();
		if (oldEntity == null)
		{
			if (scanningFunction == ScanningFunction.ParamEntitySpace)
			{
				scanningFunction = savedScanningFunction;
			}
			return false;
		}
		if (oldEntity.IsExternal)
		{
			externalEntitiesDepth--;
		}
		if (!inLiteral && scanningFunction != ScanningFunction.ParamEntitySpace)
		{
			savedScanningFunction = scanningFunction;
			scanningFunction = ScanningFunction.ParamEntitySpace;
		}
		return true;
	}

	private SchemaEntity VerifyEntityReference(XmlQualifiedName entityName, bool paramEntity, bool mustBeDeclared, bool inAttribute)
	{
		SchemaEntity value;
		if (paramEntity)
		{
			schemaInfo.ParameterEntities.TryGetValue(entityName, out value);
		}
		else
		{
			schemaInfo.GeneralEntities.TryGetValue(entityName, out value);
		}
		if (value == null)
		{
			if (paramEntity)
			{
				if (validate)
				{
					SendValidationEvent(curPos - entityName.Name.Length - 1, XmlSeverityType.Error, "Reference to undeclared parameter entity '{0}'.", entityName.Name);
				}
			}
			else if (mustBeDeclared)
			{
				if (!ParsingInternalSubset)
				{
					if (validate)
					{
						SendValidationEvent(curPos - entityName.Name.Length - 1, XmlSeverityType.Error, "Reference to undeclared entity '{0}'.", entityName.Name);
					}
				}
				else
				{
					Throw(curPos - entityName.Name.Length - 1, "Reference to undeclared entity '{0}'.", entityName.Name);
				}
			}
			return null;
		}
		if (!value.NData.IsEmpty)
		{
			Throw(curPos - entityName.Name.Length - 1, "Reference to unparsed entity '{0}'.", entityName.Name);
		}
		if (inAttribute && value.IsExternal)
		{
			Throw(curPos - entityName.Name.Length - 1, "External entity '{0}' reference cannot appear in the attribute value.", entityName.Name);
		}
		return value;
	}

	private void SendValidationEvent(int pos, XmlSeverityType severity, string code, string arg)
	{
		SendValidationEvent(severity, new XmlSchemaException(code, arg, BaseUriStr, LineNo, LinePos + (pos - curPos)));
	}

	private void SendValidationEvent(XmlSeverityType severity, string code, string arg)
	{
		SendValidationEvent(severity, new XmlSchemaException(code, arg, BaseUriStr, LineNo, LinePos));
	}

	private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException e)
	{
		readerAdapterWithValidation.ValidationEventHandling?.SendEvent(e, severity);
	}

	private bool IsAttributeValueType(Token token)
	{
		if (token >= Token.CDATA)
		{
			return token <= Token.NOTATION;
		}
		return false;
	}

	private void OnUnexpectedError()
	{
		Throw(curPos, "An internal error has occurred.");
	}

	private void Throw(int curPos, string res)
	{
		Throw(curPos, res, string.Empty);
	}

	private void Throw(int curPos, string res, string arg)
	{
		this.curPos = curPos;
		Uri baseUri = readerAdapter.BaseUri;
		readerAdapter.Throw(new XmlException(res, arg, LineNo, LinePos, (baseUri == null) ? null : baseUri.ToString()));
	}

	private void Throw(int curPos, string res, string[] args)
	{
		this.curPos = curPos;
		Uri baseUri = readerAdapter.BaseUri;
		readerAdapter.Throw(new XmlException(res, args, LineNo, LinePos, (baseUri == null) ? null : baseUri.ToString()));
	}

	private void Throw(string res, string arg, int lineNo, int linePos)
	{
		Uri baseUri = readerAdapter.BaseUri;
		readerAdapter.Throw(new XmlException(res, arg, lineNo, linePos, (baseUri == null) ? null : baseUri.ToString()));
	}

	private void ThrowInvalidChar(int pos, string data, int invCharPos)
	{
		Throw(pos, "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, invCharPos));
	}

	private void ThrowInvalidChar(char[] data, int length, int invCharPos)
	{
		Throw(invCharPos, "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, length, invCharPos));
	}

	private void ThrowUnexpectedToken(int pos, string expectedToken)
	{
		ThrowUnexpectedToken(pos, expectedToken, null);
	}

	private void ThrowUnexpectedToken(int pos, string expectedToken1, string expectedToken2)
	{
		string text = ParseUnexpectedToken(pos);
		if (expectedToken2 != null)
		{
			Throw(curPos, "'{0}' is an unexpected token. The expected token is '{1}' or '{2}'.", new string[3] { text, expectedToken1, expectedToken2 });
		}
		else
		{
			Throw(curPos, "'{0}' is an unexpected token. The expected token is '{1}'.", new string[2] { text, expectedToken1 });
		}
	}

	private string ParseUnexpectedToken(int startPos)
	{
		if (xmlCharType.IsNCNameSingleChar(chars[startPos]))
		{
			int i;
			for (i = startPos; xmlCharType.IsNCNameSingleChar(chars[i]); i++)
			{
			}
			int num = i - startPos;
			return new string(chars, startPos, (num <= 0) ? 1 : num);
		}
		return new string(chars, startPos, 1);
	}

	internal static string StripSpaces(string value)
	{
		int length = value.Length;
		if (length <= 0)
		{
			return string.Empty;
		}
		int num = 0;
		StringBuilder stringBuilder = null;
		while (value[num] == ' ')
		{
			num++;
			if (num == length)
			{
				return " ";
			}
		}
		int i;
		for (i = num; i < length; i++)
		{
			if (value[i] != ' ')
			{
				continue;
			}
			int j;
			for (j = i + 1; j < length && value[j] == ' '; j++)
			{
			}
			if (j == length)
			{
				if (stringBuilder == null)
				{
					return value.Substring(num, i - num);
				}
				stringBuilder.Append(value, num, i - num);
				return stringBuilder.ToString();
			}
			if (j > i + 1)
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length);
				}
				stringBuilder.Append(value, num, i - num + 1);
				num = j;
				i = j - 1;
			}
		}
		if (stringBuilder == null)
		{
			if (num != 0)
			{
				return value.Substring(num, length - num);
			}
			return value;
		}
		if (i > num)
		{
			stringBuilder.Append(value, num, i - num);
		}
		return stringBuilder.ToString();
	}

	async Task<IDtdInfo> IDtdParser.ParseInternalDtdAsync(IDtdParserAdapter adapter, bool saveInternalSubset)
	{
		Initialize(adapter);
		await ParseAsync(saveInternalSubset).ConfigureAwait(continueOnCapturedContext: false);
		return schemaInfo;
	}

	async Task<IDtdInfo> IDtdParser.ParseFreeFloatingDtdAsync(string baseUri, string docTypeName, string publicId, string systemId, string internalSubset, IDtdParserAdapter adapter)
	{
		InitializeFreeFloatingDtd(baseUri, docTypeName, publicId, systemId, internalSubset, adapter);
		await ParseAsync(saveInternalSubset: false).ConfigureAwait(continueOnCapturedContext: false);
		return schemaInfo;
	}

	private async Task ParseAsync(bool saveInternalSubset)
	{
		if (!freeFloatingDtd)
		{
			await ParseInDocumentDtdAsync(saveInternalSubset).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			await ParseFreeFloatingDtdAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		schemaInfo.Finish();
		if (!validate || undeclaredNotations == null)
		{
			return;
		}
		foreach (UndeclaredNotation value in undeclaredNotations.Values)
		{
			for (UndeclaredNotation undeclaredNotation = value; undeclaredNotation != null; undeclaredNotation = undeclaredNotation.next)
			{
				SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("The '{0}' notation is not declared.", value.name, BaseUriStr, value.lineNo, value.linePos));
			}
		}
	}

	private async Task ParseInDocumentDtdAsync(bool saveInternalSubset)
	{
		LoadParsingBuffer();
		scanningFunction = ScanningFunction.QName;
		nextScaningFunction = ScanningFunction.Doctype1;
		if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) != Token.QName)
		{
			OnUnexpectedError();
		}
		schemaInfo.DocTypeName = GetNameQualified(canHavePrefix: true);
		Token token = await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false);
		if (token == Token.SYSTEM || token == Token.PUBLIC)
		{
			Tuple<string, string> tuple = await ParseExternalIdAsync(token, Token.DOCTYPE).ConfigureAwait(continueOnCapturedContext: false);
			publicId = tuple.Item1;
			systemId = tuple.Item2;
			token = await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false);
		}
		switch (token)
		{
		case Token.LeftBracket:
			if (saveInternalSubset)
			{
				SaveParsingBuffer();
				internalSubsetValueSb = new StringBuilder();
			}
			await ParseInternalSubsetAsync().ConfigureAwait(continueOnCapturedContext: false);
			break;
		default:
			OnUnexpectedError();
			break;
		case Token.GreaterThan:
			break;
		}
		SaveParsingBuffer();
		if (systemId != null && systemId.Length > 0)
		{
			await ParseExternalSubsetAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task ParseFreeFloatingDtdAsync()
	{
		if (hasFreeFloatingInternalSubset)
		{
			LoadParsingBuffer();
			await ParseInternalSubsetAsync().ConfigureAwait(continueOnCapturedContext: false);
			SaveParsingBuffer();
		}
		if (systemId != null && systemId.Length > 0)
		{
			await ParseExternalSubsetAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private Task ParseInternalSubsetAsync()
	{
		return ParseSubsetAsync();
	}

	private async Task ParseExternalSubsetAsync()
	{
		if (await readerAdapter.PushExternalSubsetAsync(systemId, publicId).ConfigureAwait(continueOnCapturedContext: false))
		{
			Uri baseUri = readerAdapter.BaseUri;
			if (baseUri != null)
			{
				externalDtdBaseUri = baseUri.ToString();
			}
			externalEntitiesDepth++;
			LoadParsingBuffer();
			await ParseSubsetAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task ParseSubsetAsync()
	{
		while (true)
		{
			Token token = await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false);
			int startTagEntityId = currentEntityId;
			switch (token)
			{
			case Token.AttlistDecl:
				await ParseAttlistDeclAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case Token.ElementDecl:
				await ParseElementDeclAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case Token.EntityDecl:
				await ParseEntityDeclAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case Token.NotationDecl:
				await ParseNotationDeclAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case Token.Comment:
				await ParseCommentAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case Token.PI:
				await ParsePIAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case Token.CondSectionStart:
				if (ParsingInternalSubset)
				{
					Throw(curPos - 3, "A conditional section is not allowed in an internal subset.");
				}
				await ParseCondSectionAsync().ConfigureAwait(continueOnCapturedContext: false);
				startTagEntityId = currentEntityId;
				break;
			case Token.CondSectionEnd:
				if (condSectionDepth > 0)
				{
					condSectionDepth--;
					if (validate && currentEntityId != condSectionEntityIds[condSectionDepth])
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
				}
				else
				{
					Throw(curPos - 3, "']]>' is not expected.");
				}
				break;
			case Token.RightBracket:
				if (ParsingInternalSubset)
				{
					if (condSectionDepth != 0)
					{
						Throw(curPos, "There is an unclosed conditional section.");
					}
					if (internalSubsetValueSb != null)
					{
						SaveParsingBuffer(curPos - 1);
						schemaInfo.InternalDtdSubset = internalSubsetValueSb.ToString();
						internalSubsetValueSb = null;
					}
					if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) != Token.GreaterThan)
					{
						ThrowUnexpectedToken(curPos, ">");
					}
				}
				else
				{
					Throw(curPos, "Expected DTD markup was not found.");
				}
				return;
			case Token.Eof:
				if (ParsingInternalSubset && !freeFloatingDtd)
				{
					Throw(curPos, "Incomplete DTD content.");
				}
				if (condSectionDepth != 0)
				{
					Throw(curPos, "There is an unclosed conditional section.");
				}
				return;
			}
			if (currentEntityId != startTagEntityId)
			{
				if (validate)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				else if (!v1Compat)
				{
					Throw(curPos, "The parameter entity replacement text must nest properly within markup declarations.");
				}
			}
		}
	}

	private async Task ParseAttlistDeclAsync()
	{
		if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) == Token.QName)
		{
			XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: true);
			if (!schemaInfo.ElementDecls.TryGetValue(nameQualified, out var elementDecl) && !schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out elementDecl))
			{
				elementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
				schemaInfo.UndeclaredElementDecls.Add(nameQualified, elementDecl);
			}
			SchemaAttDef attrDef = null;
			while (true)
			{
				switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
				{
				case Token.QName:
				{
					XmlQualifiedName nameQualified2 = GetNameQualified(canHavePrefix: true);
					attrDef = new SchemaAttDef(nameQualified2, nameQualified2.Namespace)
					{
						IsDeclaredInExternal = !ParsingInternalSubset,
						LineNumber = LineNo,
						LinePosition = LinePos - (curPos - tokenStartPos)
					};
					bool attrDefAlreadyExists = elementDecl.GetAttDef(attrDef.Name) != null;
					await ParseAttlistTypeAsync(attrDef, elementDecl, attrDefAlreadyExists).ConfigureAwait(continueOnCapturedContext: false);
					await ParseAttlistDefaultAsync(attrDef, attrDefAlreadyExists).ConfigureAwait(continueOnCapturedContext: false);
					if (attrDef.Prefix.Length > 0 && attrDef.Prefix.Equals("xml"))
					{
						if (attrDef.Name.Name == "space")
						{
							if (v1Compat)
							{
								string text = attrDef.DefaultValueExpanded.Trim();
								if (text.Equals("preserve") || text.Equals("default"))
								{
									attrDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								}
							}
							else
							{
								attrDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
								if (attrDef.TokenizedType != XmlTokenizedType.ENUMERATION)
								{
									Throw("Enumeration data type required.", string.Empty, attrDef.LineNumber, attrDef.LinePosition);
								}
								if (validate)
								{
									attrDef.CheckXmlSpace(readerAdapterWithValidation.ValidationEventHandling);
								}
							}
						}
						else if (attrDef.Name.Name == "lang")
						{
							attrDef.Reserved = SchemaAttDef.Reserve.XmlLang;
						}
					}
					if (!attrDefAlreadyExists)
					{
						elementDecl.AddAttDef(attrDef);
					}
					continue;
				}
				case Token.GreaterThan:
					if (v1Compat && attrDef != null && attrDef.Prefix.Length > 0 && attrDef.Prefix.Equals("xml") && attrDef.Name.Name == "space")
					{
						attrDef.Reserved = SchemaAttDef.Reserve.XmlSpace;
						if (attrDef.Datatype.TokenizedType != XmlTokenizedType.ENUMERATION)
						{
							Throw("Enumeration data type required.", string.Empty, attrDef.LineNumber, attrDef.LinePosition);
						}
						if (validate)
						{
							attrDef.CheckXmlSpace(readerAdapterWithValidation.ValidationEventHandling);
						}
					}
					return;
				}
				break;
			}
		}
		OnUnexpectedError();
	}

	private async Task ParseAttlistTypeAsync(SchemaAttDef attrDef, SchemaElementDecl elementDecl, bool ignoreErrors)
	{
		Token token = await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false);
		if (token != Token.CDATA)
		{
			elementDecl.HasNonCDataAttribute = true;
		}
		if (IsAttributeValueType(token))
		{
			attrDef.TokenizedType = (XmlTokenizedType)token;
			attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
			switch (token)
			{
			default:
				return;
			case Token.ID:
				if (validate && elementDecl.IsIdDeclared)
				{
					SchemaAttDef attDef = elementDecl.GetAttDef(attrDef.Name);
					if ((attDef == null || attDef.Datatype.TokenizedType != XmlTokenizedType.ID) && !ignoreErrors)
					{
						SendValidationEvent(XmlSeverityType.Error, "The attribute of type ID is already declared on the '{0}' element.", elementDecl.Name.ToString());
					}
				}
				elementDecl.IsIdDeclared = true;
				return;
			case Token.NOTATION:
				break;
			}
			if (validate)
			{
				if (elementDecl.IsNotationDeclared && !ignoreErrors)
				{
					SendValidationEvent(curPos - 8, XmlSeverityType.Error, "No element type can have more than one NOTATION attribute specified.", elementDecl.Name.ToString());
				}
				else
				{
					if (elementDecl.ContentValidator != null && elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Empty && !ignoreErrors)
					{
						SendValidationEvent(curPos - 8, XmlSeverityType.Error, "An attribute of type NOTATION must not be declared on an element declared EMPTY.", elementDecl.Name.ToString());
					}
					elementDecl.IsNotationDeclared = true;
				}
			}
			if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) == Token.LeftParen && await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Name)
			{
				do
				{
					string nameString = GetNameString();
					if (!schemaInfo.Notations.ContainsKey(nameString))
					{
						AddUndeclaredNotation(nameString);
					}
					if (validate && !v1Compat && attrDef.Values != null && attrDef.Values.Contains(nameString) && !ignoreErrors)
					{
						SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate notation value.", nameString, BaseUriStr, LineNo, LinePos));
					}
					attrDef.AddValue(nameString);
					switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
					{
					case Token.Or:
						continue;
					case Token.RightParen:
						return;
					}
					break;
				}
				while (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Name);
			}
		}
		else if (token == Token.LeftParen)
		{
			attrDef.TokenizedType = XmlTokenizedType.ENUMERATION;
			attrDef.SchemaType = XmlSchemaType.GetBuiltInSimpleType(attrDef.Datatype.TypeCode);
			if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Nmtoken)
			{
				attrDef.AddValue(GetNameString());
				while (true)
				{
					string nmtokenString;
					switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
					{
					case Token.Or:
						if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Nmtoken)
						{
							nmtokenString = GetNmtokenString();
							if (validate && !v1Compat && attrDef.Values != null && attrDef.Values.Contains(nmtokenString) && !ignoreErrors)
							{
								SendValidationEvent(XmlSeverityType.Error, new XmlSchemaException("'{0}' is a duplicate enumeration value.", nmtokenString, BaseUriStr, LineNo, LinePos));
							}
							goto IL_0653;
						}
						break;
					case Token.RightParen:
						return;
					}
					break;
					IL_0653:
					attrDef.AddValue(nmtokenString);
				}
			}
		}
		OnUnexpectedError();
	}

	private async Task ParseAttlistDefaultAsync(SchemaAttDef attrDef, bool ignoreErrors)
	{
		switch (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false))
		{
		case Token.REQUIRED:
			attrDef.Presence = SchemaDeclBase.Use.Required;
			return;
		case Token.IMPLIED:
			attrDef.Presence = SchemaDeclBase.Use.Implied;
			return;
		case Token.FIXED:
			attrDef.Presence = SchemaDeclBase.Use.Fixed;
			if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) != Token.Literal)
			{
				break;
			}
			goto case Token.Literal;
		case Token.Literal:
			if (validate && attrDef.Datatype.TokenizedType == XmlTokenizedType.ID && !ignoreErrors)
			{
				SendValidationEvent(curPos, XmlSeverityType.Error, "An attribute of type ID must have a declared default of either #IMPLIED or #REQUIRED.", string.Empty);
			}
			if (attrDef.TokenizedType != XmlTokenizedType.CDATA)
			{
				attrDef.DefaultValueExpanded = GetValueWithStrippedSpaces();
			}
			else
			{
				attrDef.DefaultValueExpanded = GetValue();
			}
			attrDef.ValueLineNumber = literalLineInfo.lineNo;
			attrDef.ValueLinePosition = literalLineInfo.linePos + 1;
			DtdValidator.SetDefaultTypedValue(attrDef, readerAdapter);
			return;
		}
		OnUnexpectedError();
	}

	private async Task ParseElementDeclAsync()
	{
		if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) == Token.QName)
		{
			SchemaElementDecl elementDecl = null;
			XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: true);
			if (schemaInfo.ElementDecls.TryGetValue(nameQualified, out elementDecl))
			{
				if (validate)
				{
					SendValidationEvent(curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The '{0}' element has already been declared.", GetNameString());
				}
			}
			else
			{
				if (schemaInfo.UndeclaredElementDecls.TryGetValue(nameQualified, out elementDecl))
				{
					schemaInfo.UndeclaredElementDecls.Remove(nameQualified);
				}
				else
				{
					elementDecl = new SchemaElementDecl(nameQualified, nameQualified.Namespace);
				}
				schemaInfo.ElementDecls.Add(nameQualified, elementDecl);
			}
			elementDecl.IsDeclaredInExternal = !ParsingInternalSubset;
			Token token = await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false);
			if (token != Token.LeftParen)
			{
				if (token != Token.ANY)
				{
					if (token != Token.EMPTY)
					{
						goto IL_0466;
					}
					elementDecl.ContentValidator = ContentValidator.Empty;
				}
				else
				{
					elementDecl.ContentValidator = ContentValidator.Any;
				}
			}
			else
			{
				int startParenEntityId = currentEntityId;
				Token token2 = await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false);
				if (token2 != Token.None)
				{
					if (token2 != Token.PCDATA)
					{
						goto IL_0466;
					}
					ParticleContentValidator pcv = new ParticleContentValidator(XmlSchemaContentType.Mixed);
					pcv.Start();
					pcv.OpenGroup();
					await ParseElementMixedContentAsync(pcv, startParenEntityId).ConfigureAwait(continueOnCapturedContext: false);
					elementDecl.ContentValidator = pcv.Finish(useDFA: true);
				}
				else
				{
					ParticleContentValidator pcv = new ParticleContentValidator(XmlSchemaContentType.ElementOnly);
					pcv.Start();
					pcv.OpenGroup();
					await ParseElementOnlyContentAsync(pcv, startParenEntityId).ConfigureAwait(continueOnCapturedContext: false);
					elementDecl.ContentValidator = pcv.Finish(useDFA: true);
				}
			}
			if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) != Token.GreaterThan)
			{
				ThrowUnexpectedToken(curPos, ">");
			}
			return;
		}
		goto IL_0466;
		IL_0466:
		OnUnexpectedError();
	}

	private async Task ParseElementOnlyContentAsync(ParticleContentValidator pcv, int startParenEntityId)
	{
		Stack<ParseElementOnlyContent_LocalFrame> localFrames = new Stack<ParseElementOnlyContent_LocalFrame>();
		ParseElementOnlyContent_LocalFrame currentFrame = new ParseElementOnlyContent_LocalFrame(startParenEntityId);
		localFrames.Push(currentFrame);
		while (true)
		{
			Token token = await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false);
			if (token != Token.QName)
			{
				if (token != Token.LeftParen)
				{
					if (token != Token.GreaterThan)
					{
						goto IL_035b;
					}
					Throw(curPos, "Invalid content model.");
					goto IL_0361;
				}
				pcv.OpenGroup();
				currentFrame = new ParseElementOnlyContent_LocalFrame(currentEntityId);
				localFrames.Push(currentFrame);
				continue;
			}
			pcv.AddName(GetNameQualified(canHavePrefix: true), null);
			await ParseHowManyAsync(pcv).ConfigureAwait(continueOnCapturedContext: false);
			goto IL_019d;
			IL_035b:
			OnUnexpectedError();
			goto IL_0361;
			IL_029b:
			pcv.CloseGroup();
			if (validate && currentEntityId != currentFrame.startParenEntityId)
			{
				SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
			}
			await ParseHowManyAsync(pcv).ConfigureAwait(continueOnCapturedContext: false);
			goto IL_0361;
			IL_025e:
			if (currentFrame.parsingSchema == Token.Comma)
			{
				Throw(curPos, "Invalid content model.");
			}
			pcv.AddChoice();
			currentFrame.parsingSchema = Token.Or;
			continue;
			IL_0361:
			localFrames.Pop();
			if (localFrames.Count > 0)
			{
				currentFrame = localFrames.Peek();
				goto IL_019d;
			}
			break;
			IL_0348:
			Throw(curPos, "Invalid content model.");
			goto IL_0361;
			IL_019d:
			switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
			{
			case Token.Comma:
				break;
			case Token.Or:
				goto IL_025e;
			case Token.RightParen:
				goto IL_029b;
			case Token.GreaterThan:
				goto IL_0348;
			default:
				goto IL_035b;
			}
			if (currentFrame.parsingSchema == Token.Or)
			{
				Throw(curPos, "Invalid content model.");
			}
			pcv.AddSequence();
			currentFrame.parsingSchema = Token.Comma;
		}
	}

	private async Task ParseHowManyAsync(ParticleContentValidator pcv)
	{
		switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
		{
		case Token.Star:
			pcv.AddStar();
			break;
		case Token.QMark:
			pcv.AddQMark();
			break;
		case Token.Plus:
			pcv.AddPlus();
			break;
		}
	}

	private async Task ParseElementMixedContentAsync(ParticleContentValidator pcv, int startParenEntityId)
	{
		bool hasNames = false;
		int connectorEntityId = -1;
		int contentEntityId = currentEntityId;
		while (true)
		{
			switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
			{
			case Token.RightParen:
				pcv.CloseGroup();
				if (validate && currentEntityId != startParenEntityId)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Star && hasNames)
				{
					pcv.AddStar();
				}
				else if (hasNames)
				{
					ThrowUnexpectedToken(curPos, "*");
				}
				return;
			case Token.Or:
			{
				if (!hasNames)
				{
					hasNames = true;
				}
				else
				{
					pcv.AddChoice();
				}
				if (validate)
				{
					connectorEntityId = currentEntityId;
					if (contentEntityId < connectorEntityId)
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
				}
				if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) != Token.QName)
				{
					break;
				}
				XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: true);
				if (pcv.Exists(nameQualified) && validate)
				{
					SendValidationEvent(XmlSeverityType.Error, "The '{0}' element already exists in the content model.", nameQualified.ToString());
				}
				pcv.AddName(nameQualified, null);
				if (validate)
				{
					contentEntityId = currentEntityId;
					if (contentEntityId < connectorEntityId)
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
				}
				continue;
			}
			}
			OnUnexpectedError();
		}
	}

	private async Task ParseEntityDeclAsync()
	{
		bool isParamEntity = false;
		Token token = await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false);
		if (token == Token.Name)
		{
			goto IL_0132;
		}
		if (token == Token.Percent)
		{
			isParamEntity = true;
			if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) == Token.Name)
			{
				goto IL_0132;
			}
		}
		goto IL_0531;
		IL_0132:
		XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: false);
		SchemaEntity entity = new SchemaEntity(nameQualified, isParamEntity)
		{
			BaseURI = BaseUriStr,
			DeclaredURI = ((externalDtdBaseUri.Length == 0) ? documentBaseUri : externalDtdBaseUri)
		};
		if (isParamEntity)
		{
			if (!schemaInfo.ParameterEntities.ContainsKey(nameQualified))
			{
				schemaInfo.ParameterEntities.Add(nameQualified, entity);
			}
		}
		else if (!schemaInfo.GeneralEntities.ContainsKey(nameQualified))
		{
			schemaInfo.GeneralEntities.Add(nameQualified, entity);
		}
		entity.DeclaredInExternal = !ParsingInternalSubset;
		entity.ParsingInProgress = true;
		Token token2 = await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false);
		if ((uint)(token2 - 33) > 1u)
		{
			if (token2 != Token.Literal)
			{
				goto IL_0531;
			}
			entity.Text = GetValue();
			entity.Line = literalLineInfo.lineNo;
			entity.Pos = literalLineInfo.linePos;
		}
		else
		{
			Tuple<string, string> obj = await ParseExternalIdAsync(token2, Token.EntityDecl).ConfigureAwait(continueOnCapturedContext: false);
			string item = obj.Item1;
			string item2 = obj.Item2;
			entity.IsExternal = true;
			entity.Url = item2;
			entity.Pubid = item;
			if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.NData)
			{
				if (isParamEntity)
				{
					ThrowUnexpectedToken(curPos - 5, ">");
				}
				if (!whitespaceSeen)
				{
					Throw(curPos - 5, "'{0}' is an unexpected token. Expecting white space.", "NDATA");
				}
				if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) != Token.Name)
				{
					goto IL_0531;
				}
				entity.NData = GetNameQualified(canHavePrefix: false);
				string name = entity.NData.Name;
				if (!schemaInfo.Notations.ContainsKey(name))
				{
					AddUndeclaredNotation(name);
				}
			}
		}
		if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.GreaterThan)
		{
			entity.ParsingInProgress = false;
			return;
		}
		goto IL_0531;
		IL_0531:
		OnUnexpectedError();
	}

	private async Task ParseNotationDeclAsync()
	{
		if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) != Token.Name)
		{
			OnUnexpectedError();
		}
		XmlQualifiedName nameQualified = GetNameQualified(canHavePrefix: false);
		SchemaNotation notation = null;
		if (!schemaInfo.Notations.ContainsKey(nameQualified.Name))
		{
			if (undeclaredNotations != null)
			{
				undeclaredNotations.Remove(nameQualified.Name);
			}
			notation = new SchemaNotation(nameQualified);
			schemaInfo.Notations.Add(notation.Name.Name, notation);
		}
		else if (validate)
		{
			SendValidationEvent(curPos - nameQualified.Name.Length, XmlSeverityType.Error, "The notation '{0}' has already been declared.", nameQualified.Name);
		}
		Token token = await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false);
		if (token == Token.SYSTEM || token == Token.PUBLIC)
		{
			Tuple<string, string> obj = await ParseExternalIdAsync(token, Token.NOTATION).ConfigureAwait(continueOnCapturedContext: false);
			string item = obj.Item1;
			string item2 = obj.Item2;
			if (notation != null)
			{
				notation.SystemLiteral = item2;
				notation.Pubid = item;
			}
		}
		else
		{
			OnUnexpectedError();
		}
		if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) != Token.GreaterThan)
		{
			OnUnexpectedError();
		}
	}

	private async Task ParseCommentAsync()
	{
		SaveParsingBuffer();
		try
		{
			if (!SaveInternalSubsetValue)
			{
				await readerAdapter.ParseCommentAsync(null).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await readerAdapter.ParseCommentAsync(internalSubsetValueSb).ConfigureAwait(continueOnCapturedContext: false);
				internalSubsetValueSb.Append("-->");
			}
		}
		catch (XmlException ex)
		{
			if (!(ex.ResString == "Unexpected end of file while parsing {0} has occurred.") || currentEntityId == 0)
			{
				throw;
			}
			SendValidationEvent(XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", null);
		}
		LoadParsingBuffer();
	}

	private async Task ParsePIAsync()
	{
		SaveParsingBuffer();
		if (!SaveInternalSubsetValue)
		{
			await readerAdapter.ParsePIAsync(null).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			await readerAdapter.ParsePIAsync(internalSubsetValueSb).ConfigureAwait(continueOnCapturedContext: false);
			internalSubsetValueSb.Append("?>");
		}
		LoadParsingBuffer();
	}

	private async Task ParseCondSectionAsync()
	{
		int csEntityId = currentEntityId;
		switch (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false))
		{
		case Token.INCLUDE:
			if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.LeftBracket)
			{
				if (validate && csEntityId != currentEntityId)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (validate)
				{
					if (condSectionEntityIds == null)
					{
						condSectionEntityIds = new int[2];
					}
					else if (condSectionEntityIds.Length == condSectionDepth)
					{
						int[] destinationArray = new int[condSectionEntityIds.Length * 2];
						Array.Copy(condSectionEntityIds, 0, destinationArray, 0, condSectionEntityIds.Length);
						condSectionEntityIds = destinationArray;
					}
					condSectionEntityIds[condSectionDepth] = csEntityId;
				}
				condSectionDepth++;
				break;
			}
			goto default;
		case Token.IGNORE:
			if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.LeftBracket)
			{
				if (validate && csEntityId != currentEntityId)
				{
					SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
				}
				if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.CondSectionEnd)
				{
					if (validate && csEntityId != currentEntityId)
					{
						SendValidationEvent(curPos, XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", string.Empty);
					}
					break;
				}
			}
			goto default;
		default:
			OnUnexpectedError();
			break;
		}
	}

	private async Task<Tuple<string, string>> ParseExternalIdAsync(Token idTokenType, Token declType)
	{
		LineInfo keywordLineInfo = new LineInfo(LineNo, LinePos - 6);
		string publicId = null;
		string systemId = null;
		if (await GetTokenAsync(needWhiteSpace: true).ConfigureAwait(continueOnCapturedContext: false) != Token.Literal)
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
		if (idTokenType == Token.SYSTEM)
		{
			systemId = GetValue();
			if (systemId.IndexOf('#') >= 0)
			{
				Throw(curPos - systemId.Length - 1, "Fragment identifier '{0}' cannot be part of the system identifier '{1}'.", new string[2]
				{
					systemId.Substring(systemId.IndexOf('#')),
					systemId
				});
			}
			if (declType == Token.DOCTYPE && !freeFloatingDtd)
			{
				literalLineInfo.linePos++;
				readerAdapter.OnSystemId(systemId, keywordLineInfo, literalLineInfo);
			}
		}
		else
		{
			publicId = GetValue();
			int num;
			if ((num = xmlCharType.IsPublicId(publicId)) >= 0)
			{
				ThrowInvalidChar(curPos - 1 - publicId.Length + num, publicId, num);
			}
			if (declType == Token.DOCTYPE && !freeFloatingDtd)
			{
				literalLineInfo.linePos++;
				readerAdapter.OnPublicId(publicId, keywordLineInfo, literalLineInfo);
				if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Literal)
				{
					if (!whitespaceSeen)
					{
						Throw("'{0}' is an unexpected token. Expecting white space.", new string(literalQuoteChar, 1), literalLineInfo.lineNo, literalLineInfo.linePos);
					}
					systemId = GetValue();
					literalLineInfo.linePos++;
					readerAdapter.OnSystemId(systemId, keywordLineInfo, literalLineInfo);
				}
				else
				{
					ThrowUnexpectedToken(curPos, "\"", "'");
				}
			}
			else if (await GetTokenAsync(needWhiteSpace: false).ConfigureAwait(continueOnCapturedContext: false) == Token.Literal)
			{
				if (!whitespaceSeen)
				{
					Throw("'{0}' is an unexpected token. Expecting white space.", new string(literalQuoteChar, 1), literalLineInfo.lineNo, literalLineInfo.linePos);
				}
				systemId = GetValue();
			}
			else if (declType != Token.NOTATION)
			{
				ThrowUnexpectedToken(curPos, "\"", "'");
			}
		}
		return new Tuple<string, string>(publicId, systemId);
	}

	private async Task<Token> GetTokenAsync(bool needWhiteSpace)
	{
		whitespaceSeen = false;
		while (true)
		{
			switch (chars[curPos])
			{
			case '\0':
				if (curPos != charsUsed)
				{
					ThrowInvalidChar(chars, charsUsed, curPos);
				}
				break;
			case '\n':
				whitespaceSeen = true;
				curPos++;
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\r':
				whitespaceSeen = true;
				if (chars[curPos + 1] == '\n')
				{
					if (Normalize)
					{
						SaveParsingBuffer();
						readerAdapter.CurrentPosition++;
					}
					curPos += 2;
				}
				else
				{
					if (curPos + 1 >= charsUsed && !readerAdapter.IsEof)
					{
						break;
					}
					chars[curPos] = '\n';
					curPos++;
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\t':
			case ' ':
				whitespaceSeen = true;
				curPos++;
				continue;
			case '%':
				if (charsUsed - curPos < 2)
				{
					break;
				}
				if (!xmlCharType.IsWhiteSpace(chars[curPos + 1]))
				{
					if (IgnoreEntityReferences)
					{
						curPos++;
					}
					else
					{
						await HandleEntityReferenceAsync(paramEntity: true, inLiteral: false, inAttribute: false).ConfigureAwait(continueOnCapturedContext: false);
					}
					continue;
				}
				goto default;
			default:
				if (needWhiteSpace && !whitespaceSeen && scanningFunction != ScanningFunction.ParamEntitySpace)
				{
					Throw(curPos, "'{0}' is an unexpected token. Expecting white space.", ParseUnexpectedToken(curPos));
				}
				tokenStartPos = curPos;
				while (true)
				{
					switch (scanningFunction)
					{
					case ScanningFunction.Name:
						return await ScanNameExpectedAsync().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.QName:
						return await ScanQNameExpectedAsync().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Nmtoken:
						return await ScanNmtokenExpectedAsync().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.SubsetContent:
						return await ScanSubsetContentAsync().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Doctype1:
						return await ScanDoctype1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Doctype2:
						return ScanDoctype2();
					case ScanningFunction.Element1:
						return await ScanElement1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Element2:
						return await ScanElement2Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Element3:
						return await ScanElement3Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Element4:
						return ScanElement4();
					case ScanningFunction.Element5:
						return ScanElement5();
					case ScanningFunction.Element6:
						return ScanElement6();
					case ScanningFunction.Element7:
						return ScanElement7();
					case ScanningFunction.Attlist1:
						return await ScanAttlist1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Attlist2:
						return await ScanAttlist2Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Attlist3:
						return ScanAttlist3();
					case ScanningFunction.Attlist4:
						return ScanAttlist4();
					case ScanningFunction.Attlist5:
						return ScanAttlist5();
					case ScanningFunction.Attlist6:
						return await ScanAttlist6Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Attlist7:
						return ScanAttlist7();
					case ScanningFunction.Notation1:
						return await ScanNotation1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.SystemId:
						return await ScanSystemIdAsync().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.PublicId1:
						return await ScanPublicId1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.PublicId2:
						return await ScanPublicId2Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Entity1:
						return await ScanEntity1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Entity2:
						return await ScanEntity2Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.Entity3:
						return await ScanEntity3Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.CondSection1:
						return await ScanCondSection1Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.CondSection2:
						return ScanCondSection2();
					case ScanningFunction.CondSection3:
						return await ScanCondSection3Async().ConfigureAwait(continueOnCapturedContext: false);
					case ScanningFunction.ClosingTag:
						return ScanClosingTag();
					case ScanningFunction.ParamEntitySpace:
						break;
					default:
						return Token.None;
					}
					whitespaceSeen = true;
					scanningFunction = savedScanningFunction;
				}
			}
			bool flag = readerAdapter.IsEof;
			if (!flag)
			{
				flag = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0;
			}
			if (flag && !HandleEntityEnd(inLiteral: false))
			{
				if (scanningFunction == ScanningFunction.SubsetContent)
				{
					break;
				}
				Throw(curPos, "Incomplete DTD content.");
			}
		}
		return Token.Eof;
	}

	private async Task<Token> ScanSubsetContentAsync()
	{
		while (true)
		{
			char c = chars[curPos];
			if (c != '<')
			{
				if (c != ']')
				{
					goto IL_0548;
				}
				if (charsUsed - curPos >= 2 || readerAdapter.IsEof)
				{
					if (chars[curPos + 1] != ']')
					{
						curPos++;
						scanningFunction = ScanningFunction.ClosingTag;
						return Token.RightBracket;
					}
					if (charsUsed - curPos >= 3 || readerAdapter.IsEof)
					{
						if (chars[curPos + 1] == ']' && chars[curPos + 2] == '>')
						{
							break;
						}
						goto IL_0548;
					}
				}
			}
			else
			{
				switch (chars[curPos + 1])
				{
				case '!':
					switch (chars[curPos + 2])
					{
					case 'E':
						if (chars[curPos + 3] == 'L')
						{
							if (charsUsed - curPos >= 9)
							{
								if (chars[curPos + 4] != 'E' || chars[curPos + 5] != 'M' || chars[curPos + 6] != 'E' || chars[curPos + 7] != 'N' || chars[curPos + 8] != 'T')
								{
									Throw(curPos, "Expected DTD markup was not found.");
								}
								curPos += 9;
								scanningFunction = ScanningFunction.QName;
								nextScaningFunction = ScanningFunction.Element1;
								return Token.ElementDecl;
							}
						}
						else if (chars[curPos + 3] == 'N')
						{
							if (charsUsed - curPos >= 8)
							{
								if (chars[curPos + 4] != 'T' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'T' || chars[curPos + 7] != 'Y')
								{
									Throw(curPos, "Expected DTD markup was not found.");
								}
								curPos += 8;
								scanningFunction = ScanningFunction.Entity1;
								return Token.EntityDecl;
							}
						}
						else if (charsUsed - curPos >= 4)
						{
							Throw(curPos, "Expected DTD markup was not found.");
							return Token.None;
						}
						break;
					case 'A':
						if (charsUsed - curPos >= 9)
						{
							if (chars[curPos + 3] != 'T' || chars[curPos + 4] != 'T' || chars[curPos + 5] != 'L' || chars[curPos + 6] != 'I' || chars[curPos + 7] != 'S' || chars[curPos + 8] != 'T')
							{
								Throw(curPos, "Expected DTD markup was not found.");
							}
							curPos += 9;
							scanningFunction = ScanningFunction.QName;
							nextScaningFunction = ScanningFunction.Attlist1;
							return Token.AttlistDecl;
						}
						break;
					case 'N':
						if (charsUsed - curPos >= 10)
						{
							if (chars[curPos + 3] != 'O' || chars[curPos + 4] != 'T' || chars[curPos + 5] != 'A' || chars[curPos + 6] != 'T' || chars[curPos + 7] != 'I' || chars[curPos + 8] != 'O' || chars[curPos + 9] != 'N')
							{
								Throw(curPos, "Expected DTD markup was not found.");
							}
							curPos += 10;
							scanningFunction = ScanningFunction.Name;
							nextScaningFunction = ScanningFunction.Notation1;
							return Token.NotationDecl;
						}
						break;
					case '[':
						curPos += 3;
						scanningFunction = ScanningFunction.CondSection1;
						return Token.CondSectionStart;
					case '-':
						if (chars[curPos + 3] == '-')
						{
							curPos += 4;
							return Token.Comment;
						}
						if (charsUsed - curPos >= 4)
						{
							Throw(curPos, "Expected DTD markup was not found.");
						}
						break;
					default:
						if (charsUsed - curPos >= 3)
						{
							Throw(curPos + 2, "Expected DTD markup was not found.");
						}
						break;
					}
					break;
				case '?':
					curPos += 2;
					return Token.PI;
				default:
					if (charsUsed - curPos >= 2)
					{
						Throw(curPos, "Expected DTD markup was not found.");
						return Token.None;
					}
					break;
				}
			}
			goto IL_0568;
			IL_0568:
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw(charsUsed, "Incomplete DTD content.");
			}
			continue;
			IL_0548:
			if (charsUsed - curPos != 0)
			{
				Throw(curPos, "Expected DTD markup was not found.");
			}
			goto IL_0568;
		}
		curPos += 3;
		return Token.CondSectionEnd;
	}

	private async Task<Token> ScanNameExpectedAsync()
	{
		await ScanNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = nextScaningFunction;
		return Token.Name;
	}

	private async Task<Token> ScanQNameExpectedAsync()
	{
		await ScanQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = nextScaningFunction;
		return Token.QName;
	}

	private async Task<Token> ScanNmtokenExpectedAsync()
	{
		await ScanNmtokenAsync().ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = nextScaningFunction;
		return Token.Nmtoken;
	}

	private async Task<Token> ScanDoctype1Async()
	{
		switch (chars[curPos])
		{
		case 'P':
			if (!(await EatPublicKeywordAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Doctype2;
			scanningFunction = ScanningFunction.PublicId1;
			return Token.PUBLIC;
		case 'S':
			if (!(await EatSystemKeywordAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Doctype2;
			scanningFunction = ScanningFunction.SystemId;
			return Token.SYSTEM;
		case '[':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.LeftBracket;
		case '>':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		default:
			Throw(curPos, "Expecting external ID, '[' or '>'.");
			return Token.None;
		}
	}

	private async Task<Token> ScanElement1Async()
	{
		while (true)
		{
			char c = chars[curPos];
			if (c != '(')
			{
				if (c != 'A')
				{
					if (c == 'E')
					{
						if (charsUsed - curPos < 5)
						{
							goto IL_0141;
						}
						if (chars[curPos + 1] == 'M' && chars[curPos + 2] == 'P' && chars[curPos + 3] == 'T' && chars[curPos + 4] == 'Y')
						{
							curPos += 5;
							scanningFunction = ScanningFunction.ClosingTag;
							return Token.EMPTY;
						}
					}
				}
				else
				{
					if (charsUsed - curPos < 3)
					{
						goto IL_0141;
					}
					if (chars[curPos + 1] == 'N' && chars[curPos + 2] == 'Y')
					{
						break;
					}
				}
				Throw(curPos, "Invalid content model.");
				goto IL_0141;
			}
			scanningFunction = ScanningFunction.Element2;
			curPos++;
			return Token.LeftParen;
			IL_0141:
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
		curPos += 3;
		scanningFunction = ScanningFunction.ClosingTag;
		return Token.ANY;
	}

	private async Task<Token> ScanElement2Async()
	{
		if (chars[curPos] == '#')
		{
			while (charsUsed - curPos < 7)
			{
				if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
				{
					Throw(curPos, "Incomplete DTD content.");
				}
			}
			if (chars[curPos + 1] == 'P' && chars[curPos + 2] == 'C' && chars[curPos + 3] == 'D' && chars[curPos + 4] == 'A' && chars[curPos + 5] == 'T' && chars[curPos + 6] == 'A')
			{
				curPos += 7;
				scanningFunction = ScanningFunction.Element6;
				return Token.PCDATA;
			}
			Throw(curPos + 1, "Expecting 'PCDATA'.");
		}
		scanningFunction = ScanningFunction.Element3;
		return Token.None;
	}

	private async Task<Token> ScanElement3Async()
	{
		switch (chars[curPos])
		{
		case '(':
			curPos++;
			return Token.LeftParen;
		case '>':
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		default:
			await ScanQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
			scanningFunction = ScanningFunction.Element4;
			return Token.QName;
		}
	}

	private async Task<Token> ScanAttlist1Async()
	{
		if (chars[curPos] == '>')
		{
			curPos++;
			scanningFunction = ScanningFunction.SubsetContent;
			return Token.GreaterThan;
		}
		if (!whitespaceSeen)
		{
			Throw(curPos, "'{0}' is an unexpected token. Expecting white space.", ParseUnexpectedToken(curPos));
		}
		await ScanQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = ScanningFunction.Attlist2;
		return Token.QName;
	}

	private async Task<Token> ScanAttlist2Async()
	{
		while (true)
		{
			switch (chars[curPos])
			{
			case '(':
				curPos++;
				scanningFunction = ScanningFunction.Nmtoken;
				nextScaningFunction = ScanningFunction.Attlist5;
				return Token.LeftParen;
			case 'C':
				if (charsUsed - curPos >= 5)
				{
					if (chars[curPos + 1] != 'D' || chars[curPos + 2] != 'A' || chars[curPos + 3] != 'T' || chars[curPos + 4] != 'A')
					{
						Throw(curPos, "Invalid attribute type.");
					}
					curPos += 5;
					scanningFunction = ScanningFunction.Attlist6;
					return Token.CDATA;
				}
				break;
			case 'E':
				if (charsUsed - curPos < 9)
				{
					break;
				}
				scanningFunction = ScanningFunction.Attlist6;
				if (chars[curPos + 1] != 'N' || chars[curPos + 2] != 'T' || chars[curPos + 3] != 'I' || chars[curPos + 4] != 'T')
				{
					Throw(curPos, "'{0}' is an invalid attribute type.");
				}
				switch (chars[curPos + 5])
				{
				case 'I':
					if (chars[curPos + 6] != 'E' || chars[curPos + 7] != 'S')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					curPos += 8;
					return Token.ENTITIES;
				case 'Y':
					curPos += 6;
					return Token.ENTITY;
				}
				Throw(curPos, "'{0}' is an invalid attribute type.");
				break;
			case 'I':
				if (charsUsed - curPos >= 6)
				{
					scanningFunction = ScanningFunction.Attlist6;
					if (chars[curPos + 1] != 'D')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					if (chars[curPos + 2] != 'R')
					{
						curPos += 2;
						return Token.ID;
					}
					if (chars[curPos + 3] != 'E' || chars[curPos + 4] != 'F')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					if (chars[curPos + 5] != 'S')
					{
						curPos += 5;
						return Token.IDREF;
					}
					curPos += 6;
					return Token.IDREFS;
				}
				break;
			case 'N':
				if (charsUsed - curPos < 8 && !readerAdapter.IsEof)
				{
					break;
				}
				switch (chars[curPos + 1])
				{
				case 'O':
					if (chars[curPos + 2] != 'T' || chars[curPos + 3] != 'A' || chars[curPos + 4] != 'T' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'O' || chars[curPos + 7] != 'N')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					curPos += 8;
					scanningFunction = ScanningFunction.Attlist3;
					return Token.NOTATION;
				case 'M':
					if (chars[curPos + 2] != 'T' || chars[curPos + 3] != 'O' || chars[curPos + 4] != 'K' || chars[curPos + 5] != 'E' || chars[curPos + 6] != 'N')
					{
						Throw(curPos, "'{0}' is an invalid attribute type.");
					}
					scanningFunction = ScanningFunction.Attlist6;
					if (chars[curPos + 7] == 'S')
					{
						curPos += 8;
						return Token.NMTOKENS;
					}
					curPos += 7;
					return Token.NMTOKEN;
				}
				Throw(curPos, "'{0}' is an invalid attribute type.");
				break;
			default:
				Throw(curPos, "'{0}' is an invalid attribute type.");
				break;
			}
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
	}

	private async Task<Token> ScanAttlist6Async()
	{
		while (true)
		{
			switch (chars[curPos])
			{
			case '"':
			case '\'':
				await ScanLiteralAsync(LiteralType.AttributeValue).ConfigureAwait(continueOnCapturedContext: false);
				scanningFunction = ScanningFunction.Attlist1;
				return Token.Literal;
			case '#':
				if (charsUsed - curPos < 6)
				{
					break;
				}
				switch (chars[curPos + 1])
				{
				case 'R':
					if (charsUsed - curPos >= 9)
					{
						if (chars[curPos + 2] != 'E' || chars[curPos + 3] != 'Q' || chars[curPos + 4] != 'U' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'R' || chars[curPos + 7] != 'E' || chars[curPos + 8] != 'D')
						{
							Throw(curPos, "Expecting an attribute type.");
						}
						curPos += 9;
						scanningFunction = ScanningFunction.Attlist1;
						return Token.REQUIRED;
					}
					break;
				case 'I':
					if (charsUsed - curPos >= 8)
					{
						if (chars[curPos + 2] != 'M' || chars[curPos + 3] != 'P' || chars[curPos + 4] != 'L' || chars[curPos + 5] != 'I' || chars[curPos + 6] != 'E' || chars[curPos + 7] != 'D')
						{
							Throw(curPos, "Expecting an attribute type.");
						}
						curPos += 8;
						scanningFunction = ScanningFunction.Attlist1;
						return Token.IMPLIED;
					}
					break;
				case 'F':
					if (chars[curPos + 2] != 'I' || chars[curPos + 3] != 'X' || chars[curPos + 4] != 'E' || chars[curPos + 5] != 'D')
					{
						Throw(curPos, "Expecting an attribute type.");
					}
					curPos += 6;
					scanningFunction = ScanningFunction.Attlist7;
					return Token.FIXED;
				default:
					Throw(curPos, "Expecting an attribute type.");
					break;
				}
				break;
			default:
				Throw(curPos, "Expecting an attribute type.");
				break;
			}
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
	}

	private async Task<Token> ScanLiteralAsync(LiteralType literalType)
	{
		char quoteChar = chars[curPos];
		char replaceChar = ((literalType == LiteralType.AttributeValue) ? ' ' : '\n');
		int startQuoteEntityId = currentEntityId;
		literalLineInfo.Set(LineNo, LinePos);
		curPos++;
		tokenStartPos = curPos;
		stringBuilder.Length = 0;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 0x80) != 0 && chars[curPos] != '%')
			{
				curPos++;
				continue;
			}
			if (chars[curPos] == quoteChar && currentEntityId == startQuoteEntityId)
			{
				break;
			}
			int num = curPos - tokenStartPos;
			if (num > 0)
			{
				stringBuilder.Append(chars, tokenStartPos, num);
				tokenStartPos = curPos;
			}
			switch (chars[curPos])
			{
			case '"':
			case '\'':
			case '>':
				curPos++;
				continue;
			case '\n':
				curPos++;
				if (Normalize)
				{
					stringBuilder.Append(replaceChar);
					tokenStartPos = curPos;
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\r':
				if (chars[curPos + 1] == '\n')
				{
					if (Normalize)
					{
						if (literalType == LiteralType.AttributeValue)
						{
							stringBuilder.Append(readerAdapter.IsEntityEolNormalized ? "  " : " ");
						}
						else
						{
							stringBuilder.Append(readerAdapter.IsEntityEolNormalized ? "\r\n" : "\n");
						}
						tokenStartPos = curPos + 2;
						SaveParsingBuffer();
						readerAdapter.CurrentPosition++;
					}
					curPos += 2;
				}
				else
				{
					if (curPos + 1 == charsUsed)
					{
						break;
					}
					curPos++;
					if (Normalize)
					{
						stringBuilder.Append(replaceChar);
						tokenStartPos = curPos;
					}
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\t':
				if (literalType == LiteralType.AttributeValue && Normalize)
				{
					stringBuilder.Append(' ');
					tokenStartPos++;
				}
				curPos++;
				continue;
			case '<':
				if (literalType == LiteralType.AttributeValue)
				{
					Throw(curPos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
				}
				curPos++;
				continue;
			case '%':
				if (literalType != LiteralType.EntityReplText)
				{
					curPos++;
					continue;
				}
				await HandleEntityReferenceAsync(paramEntity: true, inLiteral: true, literalType == LiteralType.AttributeValue).ConfigureAwait(continueOnCapturedContext: false);
				tokenStartPos = curPos;
				continue;
			case '&':
			{
				if (literalType == LiteralType.SystemOrPublicID)
				{
					curPos++;
					continue;
				}
				if (curPos + 1 == charsUsed)
				{
					break;
				}
				if (chars[curPos + 1] == '#')
				{
					SaveParsingBuffer();
					int num2 = await readerAdapter.ParseNumericCharRefAsync(SaveInternalSubsetValue ? internalSubsetValueSb : null).ConfigureAwait(continueOnCapturedContext: false);
					LoadParsingBuffer();
					stringBuilder.Append(chars, curPos, num2 - curPos);
					readerAdapter.CurrentPosition = num2;
					tokenStartPos = num2;
					curPos = num2;
					continue;
				}
				SaveParsingBuffer();
				if (literalType == LiteralType.AttributeValue)
				{
					int num3 = await readerAdapter.ParseNamedCharRefAsync(expand: true, SaveInternalSubsetValue ? internalSubsetValueSb : null).ConfigureAwait(continueOnCapturedContext: false);
					LoadParsingBuffer();
					if (num3 >= 0)
					{
						stringBuilder.Append(chars, curPos, num3 - curPos);
						readerAdapter.CurrentPosition = num3;
						tokenStartPos = num3;
						curPos = num3;
					}
					else
					{
						await HandleEntityReferenceAsync(paramEntity: false, inLiteral: true, inAttribute: true).ConfigureAwait(continueOnCapturedContext: false);
						tokenStartPos = curPos;
					}
					continue;
				}
				int num4 = await readerAdapter.ParseNamedCharRefAsync(expand: false, null).ConfigureAwait(continueOnCapturedContext: false);
				LoadParsingBuffer();
				if (num4 >= 0)
				{
					tokenStartPos = curPos;
					curPos = num4;
					continue;
				}
				stringBuilder.Append('&');
				curPos++;
				tokenStartPos = curPos;
				XmlQualifiedName entityName = ScanEntityName();
				VerifyEntityReference(entityName, paramEntity: false, mustBeDeclared: false, inAttribute: false);
				continue;
			}
			default:
				if (curPos == charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[curPos]))
				{
					if (curPos + 1 == charsUsed)
					{
						break;
					}
					curPos++;
					if (XmlCharType.IsLowSurrogate(chars[curPos]))
					{
						curPos++;
						continue;
					}
				}
				ThrowInvalidChar(chars, charsUsed, curPos);
				return Token.None;
			}
			bool flag = readerAdapter.IsEof;
			if (!flag)
			{
				flag = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0;
			}
			if (flag && (literalType == LiteralType.SystemOrPublicID || !HandleEntityEnd(inLiteral: true)))
			{
				Throw(curPos, "There is an unclosed literal string.");
			}
			tokenStartPos = curPos;
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Append(chars, tokenStartPos, curPos - tokenStartPos);
		}
		curPos++;
		literalQuoteChar = quoteChar;
		return Token.Literal;
	}

	private async Task<Token> ScanNotation1Async()
	{
		switch (chars[curPos])
		{
		case 'P':
			if (!(await EatPublicKeywordAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.ClosingTag;
			scanningFunction = ScanningFunction.PublicId1;
			return Token.PUBLIC;
		case 'S':
			if (!(await EatSystemKeywordAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.ClosingTag;
			scanningFunction = ScanningFunction.SystemId;
			return Token.SYSTEM;
		default:
			Throw(curPos, "Expecting a system identifier or a public identifier.");
			return Token.None;
		}
	}

	private async Task<Token> ScanSystemIdAsync()
	{
		if (chars[curPos] != '"' && chars[curPos] != '\'')
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
		await ScanLiteralAsync(LiteralType.SystemOrPublicID).ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = nextScaningFunction;
		return Token.Literal;
	}

	private async Task<Token> ScanEntity1Async()
	{
		if (chars[curPos] == '%')
		{
			curPos++;
			nextScaningFunction = ScanningFunction.Entity2;
			scanningFunction = ScanningFunction.Name;
			return Token.Percent;
		}
		await ScanNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = ScanningFunction.Entity2;
		return Token.Name;
	}

	private async Task<Token> ScanEntity2Async()
	{
		switch (chars[curPos])
		{
		case 'P':
			if (!(await EatPublicKeywordAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Entity3;
			scanningFunction = ScanningFunction.PublicId1;
			return Token.PUBLIC;
		case 'S':
			if (!(await EatSystemKeywordAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				Throw(curPos, "Expecting external ID, '[' or '>'.");
			}
			nextScaningFunction = ScanningFunction.Entity3;
			scanningFunction = ScanningFunction.SystemId;
			return Token.SYSTEM;
		case '"':
		case '\'':
			await ScanLiteralAsync(LiteralType.EntityReplText).ConfigureAwait(continueOnCapturedContext: false);
			scanningFunction = ScanningFunction.ClosingTag;
			return Token.Literal;
		default:
			Throw(curPos, "Expecting an external identifier or an entity value.");
			return Token.None;
		}
	}

	private async Task<Token> ScanEntity3Async()
	{
		if (chars[curPos] == 'N')
		{
			do
			{
				if (charsUsed - curPos >= 5)
				{
					if (chars[curPos + 1] != 'D' || chars[curPos + 2] != 'A' || chars[curPos + 3] != 'T' || chars[curPos + 4] != 'A')
					{
						break;
					}
					curPos += 5;
					scanningFunction = ScanningFunction.Name;
					nextScaningFunction = ScanningFunction.ClosingTag;
					return Token.NData;
				}
			}
			while (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0);
		}
		scanningFunction = ScanningFunction.ClosingTag;
		return Token.None;
	}

	private async Task<Token> ScanPublicId1Async()
	{
		if (chars[curPos] != '"' && chars[curPos] != '\'')
		{
			ThrowUnexpectedToken(curPos, "\"", "'");
		}
		await ScanLiteralAsync(LiteralType.SystemOrPublicID).ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = ScanningFunction.PublicId2;
		return Token.Literal;
	}

	private async Task<Token> ScanPublicId2Async()
	{
		if (chars[curPos] != '"' && chars[curPos] != '\'')
		{
			scanningFunction = nextScaningFunction;
			return Token.None;
		}
		await ScanLiteralAsync(LiteralType.SystemOrPublicID).ConfigureAwait(continueOnCapturedContext: false);
		scanningFunction = nextScaningFunction;
		return Token.Literal;
	}

	private async Task<Token> ScanCondSection1Async()
	{
		if (chars[curPos] != 'I')
		{
			Throw(curPos, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
		}
		curPos++;
		while (true)
		{
			if (charsUsed - curPos >= 5)
			{
				char c = chars[curPos];
				if (c == 'G')
				{
					if (chars[curPos + 1] != 'N' || chars[curPos + 2] != 'O' || chars[curPos + 3] != 'R' || chars[curPos + 4] != 'E' || xmlCharType.IsNameSingleChar(chars[curPos + 5]))
					{
						break;
					}
					nextScaningFunction = ScanningFunction.CondSection3;
					scanningFunction = ScanningFunction.CondSection2;
					curPos += 5;
					return Token.IGNORE;
				}
				if (c != 'N')
				{
					break;
				}
				if (charsUsed - curPos >= 6)
				{
					if (chars[curPos + 1] != 'C' || chars[curPos + 2] != 'L' || chars[curPos + 3] != 'U' || chars[curPos + 4] != 'D' || chars[curPos + 5] != 'E' || xmlCharType.IsNameSingleChar(chars[curPos + 6]))
					{
						break;
					}
					nextScaningFunction = ScanningFunction.SubsetContent;
					scanningFunction = ScanningFunction.CondSection2;
					curPos += 6;
					return Token.INCLUDE;
				}
			}
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw(curPos, "Incomplete DTD content.");
			}
		}
		Throw(curPos - 1, "Conditional sections must specify the keyword 'IGNORE' or 'INCLUDE'.");
		return Token.None;
	}

	private async Task<Token> ScanCondSection3Async()
	{
		int ignoreSectionDepth = 0;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 0x40) != 0 && chars[curPos] != ']')
			{
				curPos++;
				continue;
			}
			switch (chars[curPos])
			{
			case '\t':
			case '"':
			case '&':
			case '\'':
				curPos++;
				continue;
			case '\n':
				curPos++;
				readerAdapter.OnNewLine(curPos);
				continue;
			case '\r':
				if (chars[curPos + 1] == '\n')
				{
					curPos += 2;
				}
				else
				{
					if (curPos + 1 >= charsUsed && !readerAdapter.IsEof)
					{
						break;
					}
					curPos++;
				}
				readerAdapter.OnNewLine(curPos);
				continue;
			case '<':
				if (charsUsed - curPos >= 3)
				{
					if (chars[curPos + 1] != '!' || chars[curPos + 2] != '[')
					{
						curPos++;
						continue;
					}
					ignoreSectionDepth++;
					curPos += 3;
					continue;
				}
				break;
			case ']':
				if (charsUsed - curPos < 3)
				{
					break;
				}
				if (chars[curPos + 1] != ']' || chars[curPos + 2] != '>')
				{
					curPos++;
					continue;
				}
				if (ignoreSectionDepth > 0)
				{
					ignoreSectionDepth--;
					curPos += 3;
					continue;
				}
				curPos += 3;
				scanningFunction = ScanningFunction.SubsetContent;
				return Token.CondSectionEnd;
			default:
				if (curPos == charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[curPos]))
				{
					if (curPos + 1 == charsUsed)
					{
						break;
					}
					curPos++;
					if (XmlCharType.IsLowSurrogate(chars[curPos]))
					{
						curPos++;
						continue;
					}
				}
				ThrowInvalidChar(chars, charsUsed, curPos);
				return Token.None;
			}
			bool flag = readerAdapter.IsEof;
			if (!flag)
			{
				flag = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0;
			}
			if (flag)
			{
				if (HandleEntityEnd(inLiteral: false))
				{
					continue;
				}
				Throw(curPos, "There is an unclosed conditional section.");
			}
			tokenStartPos = curPos;
		}
	}

	private Task ScanNameAsync()
	{
		return ScanQNameAsync(isQName: false);
	}

	private Task ScanQNameAsync()
	{
		return ScanQNameAsync(SupportNamespaces);
	}

	private async Task ScanQNameAsync(bool isQName)
	{
		tokenStartPos = curPos;
		int colonOffset = -1;
		while (true)
		{
			bool flag = false;
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 4) != 0 || chars[curPos] == ':')
			{
				curPos++;
			}
			else if (curPos + 1 >= charsUsed)
			{
				flag = true;
			}
			else
			{
				Throw(curPos, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(chars, charsUsed, curPos));
			}
			if (flag)
			{
				if (await ReadDataInNameAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					continue;
				}
				Throw(curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
			}
			while (true)
			{
				if ((xmlCharType.charProperties[(uint)chars[curPos]] & 8) != 0)
				{
					curPos++;
					continue;
				}
				if (chars[curPos] == ':')
				{
					if (isQName)
					{
						break;
					}
					curPos++;
					continue;
				}
				if (curPos == charsUsed)
				{
					if (await ReadDataInNameAsync().ConfigureAwait(continueOnCapturedContext: false))
					{
						continue;
					}
					if (tokenStartPos == curPos)
					{
						Throw(curPos, "Unexpected end of file while parsing {0} has occurred.", "Name");
					}
				}
				colonPos = ((colonOffset == -1) ? (-1) : (tokenStartPos + colonOffset));
				return;
			}
			if (colonOffset != -1)
			{
				Throw(curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
			}
			colonOffset = curPos - tokenStartPos;
			curPos++;
		}
	}

	private async Task<bool> ReadDataInNameAsync()
	{
		int offset = curPos - tokenStartPos;
		curPos = tokenStartPos;
		bool result = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0;
		tokenStartPos = curPos;
		curPos += offset;
		return result;
	}

	private async Task ScanNmtokenAsync()
	{
		tokenStartPos = curPos;
		int len;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[curPos]] & 8) != 0 || chars[curPos] == ':')
			{
				curPos++;
				continue;
			}
			if (curPos < charsUsed)
			{
				if (curPos - tokenStartPos == 0)
				{
					Throw(curPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(chars, charsUsed, curPos));
				}
				return;
			}
			len = curPos - tokenStartPos;
			curPos = tokenStartPos;
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				if (len > 0)
				{
					break;
				}
				Throw(curPos, "Unexpected end of file while parsing {0} has occurred.", "NmToken");
			}
			tokenStartPos = curPos;
			curPos += len;
		}
		tokenStartPos = curPos;
		curPos += len;
	}

	private async Task<bool> EatPublicKeywordAsync()
	{
		while (charsUsed - curPos < 6)
		{
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				return false;
			}
		}
		if (chars[curPos + 1] != 'U' || chars[curPos + 2] != 'B' || chars[curPos + 3] != 'L' || chars[curPos + 4] != 'I' || chars[curPos + 5] != 'C')
		{
			return false;
		}
		curPos += 6;
		return true;
	}

	private async Task<bool> EatSystemKeywordAsync()
	{
		while (charsUsed - curPos < 6)
		{
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				return false;
			}
		}
		if (chars[curPos + 1] != 'Y' || chars[curPos + 2] != 'S' || chars[curPos + 3] != 'T' || chars[curPos + 4] != 'E' || chars[curPos + 5] != 'M')
		{
			return false;
		}
		curPos += 6;
		return true;
	}

	private async Task<int> ReadDataAsync()
	{
		SaveParsingBuffer();
		int result = await readerAdapter.ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false);
		LoadParsingBuffer();
		return result;
	}

	private Task<bool> HandleEntityReferenceAsync(bool paramEntity, bool inLiteral, bool inAttribute)
	{
		curPos++;
		return HandleEntityReferenceAsync(ScanEntityName(), paramEntity, inLiteral, inAttribute);
	}

	private async Task<bool> HandleEntityReferenceAsync(XmlQualifiedName entityName, bool paramEntity, bool inLiteral, bool inAttribute)
	{
		SaveParsingBuffer();
		if (paramEntity && ParsingInternalSubset && !ParsingTopLevelMarkup)
		{
			Throw(curPos - entityName.Name.Length - 1, "A parameter entity reference is not allowed in internal markup.");
		}
		SchemaEntity schemaEntity = VerifyEntityReference(entityName, paramEntity, mustBeDeclared: true, inAttribute);
		if (schemaEntity == null)
		{
			return false;
		}
		if (schemaEntity.ParsingInProgress)
		{
			Throw(curPos - entityName.Name.Length - 1, paramEntity ? "Parameter entity '{0}' references itself." : "General entity '{0}' references itself.", entityName.Name);
		}
		int item;
		if (schemaEntity.IsExternal)
		{
			Tuple<int, bool> obj = await readerAdapter.PushEntityAsync(schemaEntity).ConfigureAwait(continueOnCapturedContext: false);
			item = obj.Item1;
			if (!obj.Item2)
			{
				return false;
			}
			externalEntitiesDepth++;
		}
		else
		{
			if (schemaEntity.Text.Length == 0)
			{
				return false;
			}
			Tuple<int, bool> obj2 = await readerAdapter.PushEntityAsync(schemaEntity).ConfigureAwait(continueOnCapturedContext: false);
			item = obj2.Item1;
			if (!obj2.Item2)
			{
				return false;
			}
		}
		currentEntityId = item;
		if (paramEntity && !inLiteral && scanningFunction != ScanningFunction.ParamEntitySpace)
		{
			savedScanningFunction = scanningFunction;
			scanningFunction = ScanningFunction.ParamEntitySpace;
		}
		LoadParsingBuffer();
		return true;
	}
}
