using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema;

public sealed class XmlSchemaValidator
{
	private XmlSchemaSet schemaSet;

	private XmlSchemaValidationFlags validationFlags;

	private int startIDConstraint = -1;

	private const int STACK_INCREMENT = 10;

	private bool isRoot;

	private bool rootHasSchema;

	private bool attrValid;

	private bool checkEntity;

	private SchemaInfo compiledSchemaInfo;

	private IDtdInfo dtdSchemaInfo;

	private Hashtable validatedNamespaces;

	private HWStack validationStack;

	private ValidationState context;

	private ValidatorState currentState;

	private Hashtable attPresence;

	private SchemaAttDef wildID;

	private Hashtable IDs;

	private IdRefNode idRefListHead;

	private XmlQualifiedName contextQName;

	private string NsXs;

	private string NsXsi;

	private string NsXmlNs;

	private string NsXml;

	private XmlSchemaObject partialValidationType;

	private StringBuilder textValue;

	private ValidationEventHandler eventHandler;

	private object validationEventSender;

	private XmlNameTable nameTable;

	private IXmlLineInfo positionInfo;

	private IXmlLineInfo dummyPositionInfo;

	private XmlResolver xmlResolver;

	private Uri sourceUri;

	private string sourceUriString;

	private IXmlNamespaceResolver nsResolver;

	private XmlSchemaContentProcessing processContents = XmlSchemaContentProcessing.Strict;

	private static XmlSchemaAttribute xsiTypeSO;

	private static XmlSchemaAttribute xsiNilSO;

	private static XmlSchemaAttribute xsiSLSO;

	private static XmlSchemaAttribute xsiNoNsSLSO;

	private string xsiTypeString;

	private string xsiNilString;

	private string xsiSchemaLocationString;

	private string xsiNoNamespaceSchemaLocationString;

	private static readonly XmlSchemaDatatype dtQName = XmlSchemaDatatype.FromXmlTokenizedTypeXsd(XmlTokenizedType.QName);

	private static readonly XmlSchemaDatatype dtCDATA = XmlSchemaDatatype.FromXmlTokenizedType(XmlTokenizedType.CDATA);

	private static readonly XmlSchemaDatatype dtStringArray = dtCDATA.DeriveByList(null);

	private const string Quote = "'";

	private static XmlSchemaParticle[] EmptyParticleArray = new XmlSchemaParticle[0];

	private static XmlSchemaAttribute[] EmptyAttributeArray = new XmlSchemaAttribute[0];

	private XmlCharType xmlCharType = XmlCharType.Instance;

	internal static bool[,] ValidStates = new bool[12, 12]
	{
		{
			true, true, false, false, false, false, false, false, false, false,
			false, false
		},
		{
			false, true, true, true, true, false, false, false, false, false,
			false, true
		},
		{
			false, false, false, false, false, false, false, false, false, false,
			false, true
		},
		{
			false, false, false, true, true, false, false, false, false, false,
			false, true
		},
		{
			false, false, false, true, false, true, true, false, false, true,
			true, false
		},
		{
			false, false, false, false, false, true, true, false, false, true,
			true, false
		},
		{
			false, false, false, false, true, false, false, true, true, true,
			true, false
		},
		{
			false, false, false, false, true, false, false, true, true, true,
			true, false
		},
		{
			false, false, false, false, true, false, false, true, true, true,
			true, false
		},
		{
			false, false, false, true, true, false, false, true, true, true,
			true, true
		},
		{
			false, false, false, true, true, false, false, true, true, true,
			true, true
		},
		{
			false, true, false, false, false, false, false, false, false, false,
			false, false
		}
	};

	private static string[] MethodNames = new string[12]
	{
		"None", "Initialize", "top-level ValidateAttribute", "top-level ValidateText or ValidateWhitespace", "ValidateElement", "ValidateAttribute", "ValidateEndOfAttributes", "ValidateText", "ValidateWhitespace", "ValidateEndElement",
		"SkipToEndElement", "EndValidation"
	};

	public XmlResolver XmlResolver
	{
		set
		{
			xmlResolver = value;
		}
	}

	public IXmlLineInfo LineInfoProvider
	{
		get
		{
			return positionInfo;
		}
		set
		{
			if (value == null)
			{
				positionInfo = dummyPositionInfo;
			}
			else
			{
				positionInfo = value;
			}
		}
	}

	public Uri SourceUri
	{
		get
		{
			return sourceUri;
		}
		set
		{
			sourceUri = value;
			sourceUriString = sourceUri.ToString();
		}
	}

	public object ValidationEventSender
	{
		get
		{
			return validationEventSender;
		}
		set
		{
			validationEventSender = value;
		}
	}

	internal XmlSchemaSet SchemaSet => schemaSet;

	internal XmlSchemaValidationFlags ValidationFlags => validationFlags;

	internal XmlSchemaContentType CurrentContentType
	{
		get
		{
			if (context.ElementDecl == null)
			{
				return XmlSchemaContentType.Empty;
			}
			return context.ElementDecl.ContentValidator.ContentType;
		}
	}

	internal XmlSchemaContentProcessing CurrentProcessContents => processContents;

	private bool StrictlyAssessed
	{
		get
		{
			if ((processContents == XmlSchemaContentProcessing.Strict || processContents == XmlSchemaContentProcessing.Lax) && context.ElementDecl != null)
			{
				return !context.ValidationSkipped;
			}
			return false;
		}
	}

	private bool HasSchema
	{
		get
		{
			if (isRoot)
			{
				isRoot = false;
				if (!compiledSchemaInfo.Contains(context.Namespace))
				{
					rootHasSchema = false;
				}
			}
			return rootHasSchema;
		}
	}

	private bool HasIdentityConstraints
	{
		get
		{
			if (ProcessIdentityConstraints)
			{
				return startIDConstraint != -1;
			}
			return false;
		}
	}

	internal bool ProcessIdentityConstraints => (validationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) != 0;

	internal bool ReportValidationWarnings => (validationFlags & XmlSchemaValidationFlags.ReportValidationWarnings) != 0;

	internal bool ProcessInlineSchema => (validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != 0;

	internal bool ProcessSchemaLocation => (validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != 0;

	internal bool ProcessSchemaHints
	{
		get
		{
			if ((validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) == 0)
			{
				return (validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != 0;
			}
			return true;
		}
	}

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
			eventHandler = (ValidationEventHandler)Delegate.Combine(eventHandler, value);
		}
		remove
		{
			eventHandler = (ValidationEventHandler)Delegate.Remove(eventHandler, value);
		}
	}

	public XmlSchemaValidator(XmlNameTable nameTable, XmlSchemaSet schemas, IXmlNamespaceResolver namespaceResolver, XmlSchemaValidationFlags validationFlags)
	{
		if (nameTable == null)
		{
			throw new ArgumentNullException("nameTable");
		}
		if (schemas == null)
		{
			throw new ArgumentNullException("schemas");
		}
		if (namespaceResolver == null)
		{
			throw new ArgumentNullException("namespaceResolver");
		}
		this.nameTable = nameTable;
		nsResolver = namespaceResolver;
		this.validationFlags = validationFlags;
		if ((validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != XmlSchemaValidationFlags.None || (validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None)
		{
			schemaSet = new XmlSchemaSet(nameTable);
			schemaSet.ValidationEventHandler += schemas.GetEventHandler();
			schemaSet.CompilationSettings = schemas.CompilationSettings;
			schemaSet.XmlResolver = schemas.GetResolver();
			schemaSet.Add(schemas);
			validatedNamespaces = new Hashtable();
		}
		else
		{
			schemaSet = schemas;
		}
		Init();
	}

	private void Init()
	{
		validationStack = new HWStack(10);
		attPresence = new Hashtable();
		Push(XmlQualifiedName.Empty);
		dummyPositionInfo = new PositionInfo();
		positionInfo = dummyPositionInfo;
		validationEventSender = this;
		currentState = ValidatorState.None;
		textValue = new StringBuilder(100);
		xmlResolver = XmlReaderSection.CreateDefaultResolver();
		contextQName = new XmlQualifiedName();
		Reset();
		RecompileSchemaSet();
		NsXs = nameTable.Add("http://www.w3.org/2001/XMLSchema");
		NsXsi = nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
		NsXmlNs = nameTable.Add("http://www.w3.org/2000/xmlns/");
		NsXml = nameTable.Add("http://www.w3.org/XML/1998/namespace");
		xsiTypeString = nameTable.Add("type");
		xsiNilString = nameTable.Add("nil");
		xsiSchemaLocationString = nameTable.Add("schemaLocation");
		xsiNoNamespaceSchemaLocationString = nameTable.Add("noNamespaceSchemaLocation");
	}

	private void Reset()
	{
		isRoot = true;
		rootHasSchema = true;
		while (validationStack.Length > 1)
		{
			validationStack.Pop();
		}
		startIDConstraint = -1;
		partialValidationType = null;
		if (IDs != null)
		{
			IDs.Clear();
		}
		if (ProcessSchemaHints)
		{
			validatedNamespaces.Clear();
		}
	}

	public void AddSchema(XmlSchema schema)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if ((validationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) == 0)
		{
			return;
		}
		string text = schema.TargetNamespace;
		if (text == null)
		{
			text = string.Empty;
		}
		Hashtable schemaLocations = schemaSet.SchemaLocations;
		DictionaryEntry[] array = new DictionaryEntry[schemaLocations.Count];
		schemaLocations.CopyTo(array, 0);
		if (validatedNamespaces[text] != null && schemaSet.FindSchemaByNSAndUrl(schema.BaseUri, text, array) == null)
		{
			SendValidationEvent("An element or attribute information item has already been validated from the '{0}' namespace. It is an error if 'xsi:schemaLocation', 'xsi:noNamespaceSchemaLocation', or an inline schema occurs for that namespace.", text, XmlSeverityType.Error);
		}
		if (schema.ErrorCount != 0)
		{
			return;
		}
		try
		{
			schemaSet.Add(schema);
			RecompileSchemaSet();
		}
		catch (XmlSchemaException ex)
		{
			SendValidationEvent("Cannot load the schema for the namespace '{0}' - {1}", new string[2]
			{
				schema.BaseUri.ToString(),
				ex.Message
			}, ex);
		}
		for (int i = 0; i < schema.ImportedSchemas.Count; i++)
		{
			XmlSchema xmlSchema = (XmlSchema)schema.ImportedSchemas[i];
			text = xmlSchema.TargetNamespace;
			if (text == null)
			{
				text = string.Empty;
			}
			if (validatedNamespaces[text] != null && schemaSet.FindSchemaByNSAndUrl(xmlSchema.BaseUri, text, array) == null)
			{
				SendValidationEvent("An element or attribute information item has already been validated from the '{0}' namespace. It is an error if 'xsi:schemaLocation', 'xsi:noNamespaceSchemaLocation', or an inline schema occurs for that namespace.", text, XmlSeverityType.Error);
				schemaSet.RemoveRecursive(schema);
				break;
			}
		}
	}

	public void Initialize()
	{
		if (currentState != ValidatorState.None && currentState != ValidatorState.Finish)
		{
			object[] args = new string[2]
			{
				MethodNames[(int)currentState],
				MethodNames[1]
			};
			throw new InvalidOperationException(Res.GetString("The transition from the '{0}' method to the '{1}' method is not allowed.", args));
		}
		currentState = ValidatorState.Start;
		Reset();
	}

	public void Initialize(XmlSchemaObject partialValidationType)
	{
		if (currentState != ValidatorState.None && currentState != ValidatorState.Finish)
		{
			object[] args = new string[2]
			{
				MethodNames[(int)currentState],
				MethodNames[1]
			};
			throw new InvalidOperationException(Res.GetString("The transition from the '{0}' method to the '{1}' method is not allowed.", args));
		}
		if (partialValidationType == null)
		{
			throw new ArgumentNullException("partialValidationType");
		}
		if (!(partialValidationType is XmlSchemaElement) && !(partialValidationType is XmlSchemaAttribute) && !(partialValidationType is XmlSchemaType))
		{
			throw new ArgumentException(Res.GetString("The partial validation type has to be 'XmlSchemaElement', 'XmlSchemaAttribute', or 'XmlSchemaType'."));
		}
		currentState = ValidatorState.Start;
		Reset();
		this.partialValidationType = partialValidationType;
	}

	public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo schemaInfo)
	{
		ValidateElement(localName, namespaceUri, schemaInfo, null, null, null, null);
	}

	public void ValidateElement(string localName, string namespaceUri, XmlSchemaInfo schemaInfo, string xsiType, string xsiNil, string xsiSchemaLocation, string xsiNoNamespaceSchemaLocation)
	{
		if (localName == null)
		{
			throw new ArgumentNullException("localName");
		}
		if (namespaceUri == null)
		{
			throw new ArgumentNullException("namespaceUri");
		}
		CheckStateTransition(ValidatorState.Element, MethodNames[4]);
		ClearPSVI();
		contextQName.Init(localName, namespaceUri);
		XmlQualifiedName xmlQualifiedName = contextQName;
		bool invalidElementInContext;
		object particle = ValidateElementContext(xmlQualifiedName, out invalidElementInContext);
		SchemaElementDecl schemaElementDecl = FastGetElementDecl(xmlQualifiedName, particle);
		Push(xmlQualifiedName);
		if (invalidElementInContext)
		{
			context.Validity = XmlSchemaValidity.Invalid;
		}
		if ((validationFlags & XmlSchemaValidationFlags.ProcessSchemaLocation) != XmlSchemaValidationFlags.None && xmlResolver != null)
		{
			ProcessSchemaLocations(xsiSchemaLocation, xsiNoNamespaceSchemaLocation);
		}
		if (processContents != XmlSchemaContentProcessing.Skip)
		{
			if (schemaElementDecl == null && partialValidationType == null)
			{
				schemaElementDecl = compiledSchemaInfo.GetElementDecl(xmlQualifiedName);
			}
			bool declFound = schemaElementDecl != null;
			if (xsiType != null || xsiNil != null)
			{
				schemaElementDecl = CheckXsiTypeAndNil(schemaElementDecl, xsiType, xsiNil, ref declFound);
			}
			if (schemaElementDecl == null)
			{
				ThrowDeclNotFoundWarningOrError(declFound);
			}
		}
		context.ElementDecl = schemaElementDecl;
		XmlSchemaElement schemaElement = null;
		XmlSchemaType schemaType = null;
		if (schemaElementDecl != null)
		{
			CheckElementProperties();
			attPresence.Clear();
			context.NeedValidateChildren = processContents != XmlSchemaContentProcessing.Skip;
			ValidateStartElementIdentityConstraints();
			schemaElementDecl.ContentValidator.InitValidation(context);
			schemaType = schemaElementDecl.SchemaType;
			schemaElement = GetSchemaElement();
		}
		if (schemaInfo != null)
		{
			schemaInfo.SchemaType = schemaType;
			schemaInfo.SchemaElement = schemaElement;
			schemaInfo.IsNil = context.IsNill;
			schemaInfo.Validity = context.Validity;
		}
		if (ProcessSchemaHints && validatedNamespaces[namespaceUri] == null)
		{
			validatedNamespaces.Add(namespaceUri, namespaceUri);
		}
		if (isRoot)
		{
			isRoot = false;
		}
	}

	public object ValidateAttribute(string localName, string namespaceUri, string attributeValue, XmlSchemaInfo schemaInfo)
	{
		if (attributeValue == null)
		{
			throw new ArgumentNullException("attributeValue");
		}
		return ValidateAttribute(localName, namespaceUri, null, attributeValue, schemaInfo);
	}

	public object ValidateAttribute(string localName, string namespaceUri, XmlValueGetter attributeValue, XmlSchemaInfo schemaInfo)
	{
		if (attributeValue == null)
		{
			throw new ArgumentNullException("attributeValue");
		}
		return ValidateAttribute(localName, namespaceUri, attributeValue, null, schemaInfo);
	}

	private object ValidateAttribute(string lName, string ns, XmlValueGetter attributeValueGetter, string attributeStringValue, XmlSchemaInfo schemaInfo)
	{
		if (lName == null)
		{
			throw new ArgumentNullException("localName");
		}
		if (ns == null)
		{
			throw new ArgumentNullException("namespaceUri");
		}
		ValidatorState validatorState = ((validationStack.Length > 1) ? ValidatorState.Attribute : ValidatorState.TopLevelAttribute);
		CheckStateTransition(validatorState, MethodNames[(int)validatorState]);
		object obj = null;
		attrValid = true;
		XmlSchemaValidity validity = XmlSchemaValidity.NotKnown;
		XmlSchemaAttribute xmlSchemaAttribute = null;
		XmlSchemaSimpleType memberType = null;
		ns = nameTable.Add(ns);
		if (Ref.Equal(ns, NsXmlNs))
		{
			return null;
		}
		SchemaAttDef schemaAttDef = null;
		SchemaElementDecl elementDecl = context.ElementDecl;
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(lName, ns);
		if (attPresence[xmlQualifiedName] != null)
		{
			SendValidationEvent("The '{0}' attribute has already been validated and is a duplicate attribute.", xmlQualifiedName.ToString());
			schemaInfo?.Clear();
			return null;
		}
		if (!Ref.Equal(ns, NsXsi))
		{
			XmlSchemaObject xmlSchemaObject = ((currentState == ValidatorState.TopLevelAttribute) ? partialValidationType : null);
			schemaAttDef = compiledSchemaInfo.GetAttributeXsd(elementDecl, xmlQualifiedName, xmlSchemaObject, out var attributeMatchState);
			switch (attributeMatchState)
			{
			case AttributeMatchState.UndeclaredElementAndAttribute:
				if ((schemaAttDef = CheckIsXmlAttribute(xmlQualifiedName)) == null)
				{
					if (elementDecl == null && processContents == XmlSchemaContentProcessing.Strict && xmlQualifiedName.Namespace.Length != 0 && compiledSchemaInfo.Contains(xmlQualifiedName.Namespace))
					{
						attrValid = false;
						SendValidationEvent("The '{0}' attribute is not declared.", xmlQualifiedName.ToString());
					}
					else if (processContents != XmlSchemaContentProcessing.Skip)
					{
						SendValidationEvent("Could not find schema information for the attribute '{0}'.", xmlQualifiedName.ToString(), XmlSeverityType.Warning);
					}
					break;
				}
				goto case AttributeMatchState.AttributeFound;
			case AttributeMatchState.UndeclaredAttribute:
				if ((schemaAttDef = CheckIsXmlAttribute(xmlQualifiedName)) == null)
				{
					attrValid = false;
					SendValidationEvent("The '{0}' attribute is not declared.", xmlQualifiedName.ToString());
					break;
				}
				goto case AttributeMatchState.AttributeFound;
			case AttributeMatchState.ProhibitedAnyAttribute:
				if ((schemaAttDef = CheckIsXmlAttribute(xmlQualifiedName)) == null)
				{
					attrValid = false;
					SendValidationEvent("The '{0}' attribute is not allowed.", xmlQualifiedName.ToString());
					break;
				}
				goto case AttributeMatchState.AttributeFound;
			case AttributeMatchState.ProhibitedAttribute:
				attrValid = false;
				SendValidationEvent("The '{0}' attribute is not allowed.", xmlQualifiedName.ToString());
				break;
			case AttributeMatchState.AttributeNameMismatch:
				attrValid = false;
				SendValidationEvent("The attribute name '{0}' does not match the name '{1}' of the 'XmlSchemaAttribute' set as a partial validation type.", new string[2]
				{
					xmlQualifiedName.ToString(),
					((XmlSchemaAttribute)xmlSchemaObject).QualifiedName.ToString()
				});
				break;
			case AttributeMatchState.ValidateAttributeInvalidCall:
				currentState = ValidatorState.Start;
				attrValid = false;
				SendValidationEvent("If the partial validation type is 'XmlSchemaElement' or 'XmlSchemaType', the 'ValidateAttribute' method cannot be called.", string.Empty);
				break;
			case AttributeMatchState.AnyIdAttributeFound:
				if (wildID == null)
				{
					wildID = schemaAttDef;
					if ((elementDecl.SchemaType as XmlSchemaComplexType).ContainsIdAttribute(findAll: false))
					{
						SendValidationEvent("It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.", string.Empty);
						break;
					}
					goto case AttributeMatchState.AttributeFound;
				}
				SendValidationEvent("It is an error if more than one attribute whose type is xs:ID or is derived from xs:ID, matches an attribute wildcard on an element.", string.Empty);
				break;
			case AttributeMatchState.AttributeFound:
			{
				xmlSchemaAttribute = schemaAttDef.SchemaAttribute;
				if (elementDecl != null)
				{
					attPresence.Add(xmlQualifiedName, schemaAttDef);
				}
				object obj2 = ((attributeValueGetter == null) ? attributeStringValue : attributeValueGetter());
				obj = CheckAttributeValue(obj2, schemaAttDef);
				XmlSchemaDatatype datatype = schemaAttDef.Datatype;
				if (datatype.Variety == XmlSchemaDatatypeVariety.Union && obj != null)
				{
					XsdSimpleValue obj3 = obj as XsdSimpleValue;
					memberType = obj3.XmlType;
					datatype = obj3.XmlType.Datatype;
					obj = obj3.TypedValue;
				}
				CheckTokenizedTypes(datatype, obj, attrValue: true);
				if (HasIdentityConstraints)
				{
					AttributeIdentityConstraints(xmlQualifiedName.Name, xmlQualifiedName.Namespace, obj, obj2.ToString(), datatype);
				}
				break;
			}
			case AttributeMatchState.AnyAttributeLax:
				SendValidationEvent("Could not find schema information for the attribute '{0}'.", xmlQualifiedName.ToString(), XmlSeverityType.Warning);
				break;
			}
		}
		else
		{
			lName = nameTable.Add(lName);
			if (Ref.Equal(lName, xsiTypeString) || Ref.Equal(lName, xsiNilString) || Ref.Equal(lName, xsiSchemaLocationString) || Ref.Equal(lName, xsiNoNamespaceSchemaLocationString))
			{
				attPresence.Add(xmlQualifiedName, SchemaAttDef.Empty);
			}
			else
			{
				attrValid = false;
				SendValidationEvent("The attribute '{0}' does not match one of the four allowed attributes in the 'xsi' namespace.", xmlQualifiedName.ToString());
			}
		}
		if (!attrValid)
		{
			validity = XmlSchemaValidity.Invalid;
		}
		else if (schemaAttDef != null)
		{
			validity = XmlSchemaValidity.Valid;
		}
		if (schemaInfo != null)
		{
			schemaInfo.SchemaAttribute = xmlSchemaAttribute;
			schemaInfo.SchemaType = xmlSchemaAttribute?.AttributeSchemaType;
			schemaInfo.MemberType = memberType;
			schemaInfo.IsDefault = false;
			schemaInfo.Validity = validity;
		}
		if (ProcessSchemaHints && validatedNamespaces[ns] == null)
		{
			validatedNamespaces.Add(ns, ns);
		}
		return obj;
	}

	public void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributes)
	{
		if (defaultAttributes == null)
		{
			throw new ArgumentNullException("defaultAttributes");
		}
		CheckStateTransition(ValidatorState.Attribute, "GetUnspecifiedDefaultAttributes");
		GetUnspecifiedDefaultAttributes(defaultAttributes, createNodeData: false);
	}

	public void ValidateEndOfAttributes(XmlSchemaInfo schemaInfo)
	{
		CheckStateTransition(ValidatorState.EndOfAttributes, MethodNames[6]);
		SchemaElementDecl elementDecl = context.ElementDecl;
		if (elementDecl != null && elementDecl.HasRequiredAttribute)
		{
			context.CheckRequiredAttribute = false;
			CheckRequiredAttributes(elementDecl);
		}
		if (schemaInfo != null)
		{
			schemaInfo.Validity = context.Validity;
		}
	}

	public void ValidateText(string elementValue)
	{
		if (elementValue == null)
		{
			throw new ArgumentNullException("elementValue");
		}
		ValidateText(elementValue, null);
	}

	public void ValidateText(XmlValueGetter elementValue)
	{
		if (elementValue == null)
		{
			throw new ArgumentNullException("elementValue");
		}
		ValidateText(null, elementValue);
	}

	private void ValidateText(string elementStringValue, XmlValueGetter elementValueGetter)
	{
		ValidatorState validatorState = ((validationStack.Length > 1) ? ValidatorState.Text : ValidatorState.TopLevelTextOrWS);
		CheckStateTransition(validatorState, MethodNames[(int)validatorState]);
		if (!context.NeedValidateChildren)
		{
			return;
		}
		if (context.IsNill)
		{
			SendValidationEvent("Element '{0}' must have no character or element children.", QNameString(context.LocalName, context.Namespace));
			return;
		}
		switch (context.ElementDecl.ContentValidator.ContentType)
		{
		case XmlSchemaContentType.Empty:
			SendValidationEvent("The element cannot contain text. Content model is empty.", string.Empty);
			break;
		case XmlSchemaContentType.TextOnly:
			if (elementValueGetter != null)
			{
				SaveTextValue(elementValueGetter());
			}
			else
			{
				SaveTextValue(elementStringValue);
			}
			break;
		case XmlSchemaContentType.ElementOnly:
		{
			string str = ((elementValueGetter != null) ? elementValueGetter().ToString() : elementStringValue);
			if (!xmlCharType.IsOnlyWhitespace(str))
			{
				ArrayList arrayList = context.ElementDecl.ContentValidator.ExpectedParticles(context, isRequiredOnly: false, schemaSet);
				if (arrayList == null || arrayList.Count == 0)
				{
					SendValidationEvent("The element {0} cannot contain text.", BuildElementName(context.LocalName, context.Namespace));
					break;
				}
				SendValidationEvent("The element {0} cannot contain text. List of possible elements expected: {1}.", new string[2]
				{
					BuildElementName(context.LocalName, context.Namespace),
					PrintExpectedElements(arrayList, getParticles: true)
				});
			}
			break;
		}
		case XmlSchemaContentType.Mixed:
			if (context.ElementDecl.DefaultValueTyped != null)
			{
				if (elementValueGetter != null)
				{
					SaveTextValue(elementValueGetter());
				}
				else
				{
					SaveTextValue(elementStringValue);
				}
			}
			break;
		}
	}

	public void ValidateWhitespace(string elementValue)
	{
		if (elementValue == null)
		{
			throw new ArgumentNullException("elementValue");
		}
		ValidateWhitespace(elementValue, null);
	}

	public void ValidateWhitespace(XmlValueGetter elementValue)
	{
		if (elementValue == null)
		{
			throw new ArgumentNullException("elementValue");
		}
		ValidateWhitespace(null, elementValue);
	}

	private void ValidateWhitespace(string elementStringValue, XmlValueGetter elementValueGetter)
	{
		ValidatorState validatorState = ((validationStack.Length > 1) ? ValidatorState.Whitespace : ValidatorState.TopLevelTextOrWS);
		CheckStateTransition(validatorState, MethodNames[(int)validatorState]);
		if (!context.NeedValidateChildren)
		{
			return;
		}
		if (context.IsNill)
		{
			SendValidationEvent("Element '{0}' must have no character or element children.", QNameString(context.LocalName, context.Namespace));
		}
		switch (context.ElementDecl.ContentValidator.ContentType)
		{
		case XmlSchemaContentType.Empty:
			SendValidationEvent("The element cannot contain white space. Content model is empty.", string.Empty);
			break;
		case XmlSchemaContentType.TextOnly:
			if (elementValueGetter != null)
			{
				SaveTextValue(elementValueGetter());
			}
			else
			{
				SaveTextValue(elementStringValue);
			}
			break;
		case XmlSchemaContentType.Mixed:
			if (context.ElementDecl.DefaultValueTyped != null)
			{
				if (elementValueGetter != null)
				{
					SaveTextValue(elementValueGetter());
				}
				else
				{
					SaveTextValue(elementStringValue);
				}
			}
			break;
		case XmlSchemaContentType.ElementOnly:
			break;
		}
	}

	public object ValidateEndElement(XmlSchemaInfo schemaInfo)
	{
		return InternalValidateEndElement(schemaInfo, null);
	}

	public object ValidateEndElement(XmlSchemaInfo schemaInfo, object typedValue)
	{
		if (typedValue == null)
		{
			throw new ArgumentNullException("typedValue");
		}
		if (textValue.Length > 0)
		{
			throw new InvalidOperationException(Res.GetString("It is invalid to call the 'ValidateEndElement' overload that takes in a 'typedValue' after 'ValidateText' or 'ValidateWhitespace' methods have been called."));
		}
		return InternalValidateEndElement(schemaInfo, typedValue);
	}

	public void SkipToEndElement(XmlSchemaInfo schemaInfo)
	{
		if (validationStack.Length <= 1)
		{
			throw new InvalidOperationException(Res.GetString("The call to the '{0}' method does not match a corresponding call to 'ValidateElement' method.", MethodNames[10]));
		}
		CheckStateTransition(ValidatorState.SkipToEndElement, MethodNames[10]);
		if (schemaInfo != null)
		{
			SchemaElementDecl elementDecl = context.ElementDecl;
			if (elementDecl != null)
			{
				schemaInfo.SchemaType = elementDecl.SchemaType;
				schemaInfo.SchemaElement = GetSchemaElement();
			}
			else
			{
				schemaInfo.SchemaType = null;
				schemaInfo.SchemaElement = null;
			}
			schemaInfo.MemberType = null;
			schemaInfo.IsNil = context.IsNill;
			schemaInfo.IsDefault = context.IsDefault;
			schemaInfo.Validity = context.Validity;
		}
		context.ValidationSkipped = true;
		currentState = ValidatorState.SkipToEndElement;
		Pop();
	}

	public void EndValidation()
	{
		if (validationStack.Length > 1)
		{
			throw new InvalidOperationException(Res.GetString("The 'EndValidation' method cannot not be called when all the elements have not been validated. 'ValidateEndElement' calls corresponding to 'ValidateElement' calls might be missing."));
		}
		CheckStateTransition(ValidatorState.Finish, MethodNames[11]);
		CheckForwardRefs();
	}

	public XmlSchemaParticle[] GetExpectedParticles()
	{
		if (currentState == ValidatorState.Start || currentState == ValidatorState.TopLevelTextOrWS)
		{
			if (partialValidationType != null)
			{
				if (partialValidationType is XmlSchemaElement xmlSchemaElement)
				{
					return new XmlSchemaParticle[1] { xmlSchemaElement };
				}
				return EmptyParticleArray;
			}
			ICollection values = schemaSet.GlobalElements.Values;
			ArrayList arrayList = new ArrayList(values.Count);
			foreach (XmlSchemaElement item in values)
			{
				ContentValidator.AddParticleToExpected(item, schemaSet, arrayList, global: true);
			}
			return arrayList.ToArray(typeof(XmlSchemaParticle)) as XmlSchemaParticle[];
		}
		if (context.ElementDecl != null)
		{
			ArrayList arrayList2 = context.ElementDecl.ContentValidator.ExpectedParticles(context, isRequiredOnly: false, schemaSet);
			if (arrayList2 != null)
			{
				return arrayList2.ToArray(typeof(XmlSchemaParticle)) as XmlSchemaParticle[];
			}
		}
		return EmptyParticleArray;
	}

	public XmlSchemaAttribute[] GetExpectedAttributes()
	{
		if (currentState == ValidatorState.Element || currentState == ValidatorState.Attribute)
		{
			SchemaElementDecl elementDecl = context.ElementDecl;
			ArrayList arrayList = new ArrayList();
			if (elementDecl != null)
			{
				foreach (SchemaAttDef value in elementDecl.AttDefs.Values)
				{
					if (attPresence[value.Name] == null)
					{
						arrayList.Add(value.SchemaAttribute);
					}
				}
			}
			if (nsResolver.LookupPrefix(NsXsi) != null)
			{
				AddXsiAttributes(arrayList);
			}
			return arrayList.ToArray(typeof(XmlSchemaAttribute)) as XmlSchemaAttribute[];
		}
		if (currentState == ValidatorState.Start && partialValidationType != null && partialValidationType is XmlSchemaAttribute xmlSchemaAttribute)
		{
			return new XmlSchemaAttribute[1] { xmlSchemaAttribute };
		}
		return EmptyAttributeArray;
	}

	internal void GetUnspecifiedDefaultAttributes(ArrayList defaultAttributes, bool createNodeData)
	{
		currentState = ValidatorState.Attribute;
		SchemaElementDecl elementDecl = context.ElementDecl;
		if (elementDecl == null || !elementDecl.HasDefaultAttribute)
		{
			return;
		}
		for (int i = 0; i < elementDecl.DefaultAttDefs.Count; i++)
		{
			SchemaAttDef schemaAttDef = (SchemaAttDef)elementDecl.DefaultAttDefs[i];
			if (attPresence.Contains(schemaAttDef.Name) || schemaAttDef.DefaultValueTyped == null)
			{
				continue;
			}
			string text = nameTable.Add(schemaAttDef.Name.Namespace);
			string text2 = string.Empty;
			if (text.Length > 0)
			{
				text2 = GetDefaultAttributePrefix(text);
				if (text2 == null || text2.Length == 0)
				{
					SendValidationEvent("Default attribute '{0}' for element '{1}' could not be applied as the attribute namespace is not mapped to a prefix in the instance document.", new string[2]
					{
						schemaAttDef.Name.ToString(),
						QNameString(context.LocalName, context.Namespace)
					});
					continue;
				}
			}
			XmlSchemaDatatype datatype = schemaAttDef.Datatype;
			if (createNodeData)
			{
				ValidatingReaderNodeData validatingReaderNodeData = new ValidatingReaderNodeData();
				validatingReaderNodeData.LocalName = nameTable.Add(schemaAttDef.Name.Name);
				validatingReaderNodeData.Namespace = text;
				validatingReaderNodeData.Prefix = nameTable.Add(text2);
				validatingReaderNodeData.NodeType = XmlNodeType.Attribute;
				AttributePSVIInfo attributePSVIInfo = new AttributePSVIInfo();
				XmlSchemaInfo attributeSchemaInfo = attributePSVIInfo.attributeSchemaInfo;
				if (schemaAttDef.Datatype.Variety == XmlSchemaDatatypeVariety.Union)
				{
					XsdSimpleValue xsdSimpleValue = schemaAttDef.DefaultValueTyped as XsdSimpleValue;
					attributeSchemaInfo.MemberType = xsdSimpleValue.XmlType;
					datatype = xsdSimpleValue.XmlType.Datatype;
					attributePSVIInfo.typedAttributeValue = xsdSimpleValue.TypedValue;
				}
				else
				{
					attributePSVIInfo.typedAttributeValue = schemaAttDef.DefaultValueTyped;
				}
				attributeSchemaInfo.IsDefault = true;
				attributeSchemaInfo.Validity = XmlSchemaValidity.Valid;
				attributeSchemaInfo.SchemaType = schemaAttDef.SchemaType;
				attributeSchemaInfo.SchemaAttribute = schemaAttDef.SchemaAttribute;
				validatingReaderNodeData.RawValue = attributeSchemaInfo.XmlType.ValueConverter.ToString(attributePSVIInfo.typedAttributeValue);
				validatingReaderNodeData.AttInfo = attributePSVIInfo;
				defaultAttributes.Add(validatingReaderNodeData);
			}
			else
			{
				defaultAttributes.Add(schemaAttDef.SchemaAttribute);
			}
			CheckTokenizedTypes(datatype, schemaAttDef.DefaultValueTyped, attrValue: true);
			if (HasIdentityConstraints)
			{
				AttributeIdentityConstraints(schemaAttDef.Name.Name, schemaAttDef.Name.Namespace, schemaAttDef.DefaultValueTyped, schemaAttDef.DefaultValueRaw, datatype);
			}
		}
	}

	internal void SetDtdSchemaInfo(IDtdInfo dtdSchemaInfo)
	{
		this.dtdSchemaInfo = dtdSchemaInfo;
		checkEntity = true;
	}

	internal string GetConcatenatedValue()
	{
		return textValue.ToString();
	}

	private object InternalValidateEndElement(XmlSchemaInfo schemaInfo, object typedValue)
	{
		if (validationStack.Length <= 1)
		{
			throw new InvalidOperationException(Res.GetString("The call to the '{0}' method does not match a corresponding call to 'ValidateElement' method.", MethodNames[9]));
		}
		CheckStateTransition(ValidatorState.EndElement, MethodNames[9]);
		SchemaElementDecl elementDecl = context.ElementDecl;
		XmlSchemaSimpleType memberType = null;
		XmlSchemaType schemaType = null;
		XmlSchemaElement schemaElement = null;
		string text = string.Empty;
		if (elementDecl != null)
		{
			if (context.CheckRequiredAttribute && elementDecl.HasRequiredAttribute)
			{
				CheckRequiredAttributes(elementDecl);
			}
			if (!context.IsNill && context.NeedValidateChildren)
			{
				switch (elementDecl.ContentValidator.ContentType)
				{
				case XmlSchemaContentType.TextOnly:
					if (typedValue == null)
					{
						text = textValue.ToString();
						typedValue = ValidateAtomicValue(text, out memberType);
					}
					else
					{
						typedValue = ValidateAtomicValue(typedValue, out memberType);
					}
					break;
				case XmlSchemaContentType.Mixed:
					if (elementDecl.DefaultValueTyped != null && typedValue == null)
					{
						text = textValue.ToString();
						typedValue = CheckMixedValueConstraint(text);
					}
					break;
				case XmlSchemaContentType.ElementOnly:
					if (typedValue != null)
					{
						throw new InvalidOperationException(Res.GetString("It is invalid to call the 'ValidateEndElement' overload that takes in a 'typedValue' for elements with complex content."));
					}
					break;
				}
				if (!elementDecl.ContentValidator.CompleteValidation(context))
				{
					CompleteValidationError(context, eventHandler, nsResolver, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition, schemaSet);
					context.Validity = XmlSchemaValidity.Invalid;
				}
			}
			if (HasIdentityConstraints)
			{
				XmlSchemaType xmlSchemaType = ((memberType == null) ? elementDecl.SchemaType : memberType);
				EndElementIdentityConstraints(typedValue, text, xmlSchemaType.Datatype);
			}
			schemaType = elementDecl.SchemaType;
			schemaElement = GetSchemaElement();
		}
		if (schemaInfo != null)
		{
			schemaInfo.SchemaType = schemaType;
			schemaInfo.SchemaElement = schemaElement;
			schemaInfo.MemberType = memberType;
			schemaInfo.IsNil = context.IsNill;
			schemaInfo.IsDefault = context.IsDefault;
			if (context.Validity == XmlSchemaValidity.NotKnown && StrictlyAssessed)
			{
				context.Validity = XmlSchemaValidity.Valid;
			}
			schemaInfo.Validity = context.Validity;
		}
		Pop();
		return typedValue;
	}

	private void ProcessSchemaLocations(string xsiSchemaLocation, string xsiNoNamespaceSchemaLocation)
	{
		bool flag = false;
		if (xsiNoNamespaceSchemaLocation != null)
		{
			flag = true;
			LoadSchema(string.Empty, xsiNoNamespaceSchemaLocation);
		}
		if (xsiSchemaLocation != null)
		{
			object typedValue;
			Exception ex = dtStringArray.TryParseValue(xsiSchemaLocation, nameTable, nsResolver, out typedValue);
			if (ex != null)
			{
				SendValidationEvent("The attribute '{0}' has an invalid value '{1}' according to its schema type '{2}' - {3}", new string[4] { "schemaLocation", xsiSchemaLocation, dtStringArray.TypeCodeString, ex.Message }, ex);
				return;
			}
			string[] array = (string[])typedValue;
			flag = true;
			try
			{
				for (int i = 0; i < array.Length - 1; i += 2)
				{
					LoadSchema(array[i], array[i + 1]);
				}
			}
			catch (XmlSchemaException e)
			{
				SendValidationEvent(e);
			}
		}
		if (flag)
		{
			RecompileSchemaSet();
		}
	}

	private object ValidateElementContext(XmlQualifiedName elementName, out bool invalidElementInContext)
	{
		object obj = null;
		int errorCode = 0;
		XmlSchemaElement xmlSchemaElement = null;
		invalidElementInContext = false;
		if (context.NeedValidateChildren)
		{
			if (context.IsNill)
			{
				SendValidationEvent("Element '{0}' must have no character or element children.", QNameString(context.LocalName, context.Namespace));
				return null;
			}
			if (context.ElementDecl.ContentValidator.ContentType == XmlSchemaContentType.Mixed && context.ElementDecl.Presence == SchemaDeclBase.Use.Fixed)
			{
				SendValidationEvent("Although the '{0}' element's content type is mixed, it cannot have element children, because it has a fixed value constraint in the schema.", QNameString(context.LocalName, context.Namespace));
				return null;
			}
			XmlQualifiedName xmlQualifiedName = elementName;
			bool flag = false;
			while (true)
			{
				obj = context.ElementDecl.ContentValidator.ValidateElement(xmlQualifiedName, context, out errorCode);
				if (obj != null)
				{
					break;
				}
				if (errorCode == -2)
				{
					SendValidationEvent("Element '{0}' cannot appear more than once if content model type is \"all\".", elementName.ToString());
					invalidElementInContext = true;
					processContents = (context.ProcessContents = XmlSchemaContentProcessing.Skip);
					return null;
				}
				flag = true;
				xmlSchemaElement = GetSubstitutionGroupHead(xmlQualifiedName);
				if (xmlSchemaElement == null)
				{
					break;
				}
				xmlQualifiedName = xmlSchemaElement.QualifiedName;
			}
			if (flag)
			{
				if (!(obj is XmlSchemaElement xmlSchemaElement2))
				{
					obj = null;
				}
				else if (xmlSchemaElement2.RefName.IsEmpty)
				{
					SendValidationEvent("The element {0} cannot substitute for a local element {1} expected in that position.", BuildElementName(elementName), BuildElementName(xmlSchemaElement2.QualifiedName));
					invalidElementInContext = true;
					processContents = (context.ProcessContents = XmlSchemaContentProcessing.Skip);
				}
				else
				{
					obj = compiledSchemaInfo.GetElement(elementName);
					context.NeedValidateChildren = true;
				}
			}
			if (obj == null)
			{
				ElementValidationError(elementName, context, eventHandler, nsResolver, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition, schemaSet);
				invalidElementInContext = true;
				processContents = (context.ProcessContents = XmlSchemaContentProcessing.Skip);
			}
		}
		return obj;
	}

	private XmlSchemaElement GetSubstitutionGroupHead(XmlQualifiedName member)
	{
		XmlSchemaElement element = compiledSchemaInfo.GetElement(member);
		if (element != null)
		{
			XmlQualifiedName substitutionGroup = element.SubstitutionGroup;
			if (!substitutionGroup.IsEmpty)
			{
				XmlSchemaElement element2 = compiledSchemaInfo.GetElement(substitutionGroup);
				if (element2 != null)
				{
					if ((element2.BlockResolved & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
					{
						SendValidationEvent("Element '{0}' cannot substitute in place of head element '{1}' because it has block='substitution'.", new string[2]
						{
							member.ToString(),
							substitutionGroup.ToString()
						});
						return null;
					}
					if (!XmlSchemaType.IsDerivedFrom(element.ElementSchemaType, element2.ElementSchemaType, element2.BlockResolved))
					{
						SendValidationEvent("Member element {0}'s type cannot be derived by restriction or extension from head element {1}'s type, because it has block='restriction' or 'extension'.", new string[2]
						{
							member.ToString(),
							substitutionGroup.ToString()
						});
						return null;
					}
					return element2;
				}
			}
		}
		return null;
	}

	private object ValidateAtomicValue(string stringValue, out XmlSchemaSimpleType memberType)
	{
		object typedValue = null;
		memberType = null;
		SchemaElementDecl elementDecl = context.ElementDecl;
		if (!context.IsNill)
		{
			if (stringValue.Length == 0 && elementDecl.DefaultValueTyped != null)
			{
				SchemaElementDecl elementDeclBeforeXsi = context.ElementDeclBeforeXsi;
				if (elementDeclBeforeXsi != null && elementDeclBeforeXsi != elementDecl)
				{
					if (elementDecl.Datatype.TryParseValue(elementDecl.DefaultValueRaw, nameTable, nsResolver, out typedValue) != null)
					{
						SendValidationEvent("The default value '{0}' of element '{1}' is invalid according to the type specified by xsi:type.", new string[2]
						{
							elementDecl.DefaultValueRaw,
							QNameString(context.LocalName, context.Namespace)
						});
					}
					else
					{
						context.IsDefault = true;
					}
				}
				else
				{
					context.IsDefault = true;
					typedValue = elementDecl.DefaultValueTyped;
				}
			}
			else
			{
				typedValue = CheckElementValue(stringValue);
			}
			XsdSimpleValue xsdSimpleValue = typedValue as XsdSimpleValue;
			XmlSchemaDatatype datatype = elementDecl.Datatype;
			if (xsdSimpleValue != null)
			{
				memberType = xsdSimpleValue.XmlType;
				typedValue = xsdSimpleValue.TypedValue;
				datatype = memberType.Datatype;
			}
			CheckTokenizedTypes(datatype, typedValue, attrValue: false);
		}
		return typedValue;
	}

	private object ValidateAtomicValue(object parsedValue, out XmlSchemaSimpleType memberType)
	{
		memberType = null;
		SchemaElementDecl elementDecl = context.ElementDecl;
		object typedValue = null;
		if (!context.IsNill)
		{
			SchemaDeclBase schemaDeclBase = elementDecl;
			XmlSchemaDatatype datatype = elementDecl.Datatype;
			Exception ex = datatype.TryParseValue(parsedValue, nameTable, nsResolver, out typedValue);
			if (ex != null)
			{
				string text = parsedValue as string;
				if (text == null)
				{
					text = XmlSchemaDatatype.ConcatenatedToString(parsedValue);
				}
				SendValidationEvent("The '{0}' element is invalid - The value '{1}' is invalid according to its datatype '{2}' - {3}", new string[4]
				{
					QNameString(context.LocalName, context.Namespace),
					text,
					GetTypeName(schemaDeclBase),
					ex.Message
				}, ex);
				return null;
			}
			if (!schemaDeclBase.CheckValue(typedValue))
			{
				SendValidationEvent("The value of the '{0}' element does not equal its fixed value.", QNameString(context.LocalName, context.Namespace));
			}
			if (datatype.Variety == XmlSchemaDatatypeVariety.Union)
			{
				XsdSimpleValue xsdSimpleValue = typedValue as XsdSimpleValue;
				memberType = xsdSimpleValue.XmlType;
				typedValue = xsdSimpleValue.TypedValue;
				datatype = memberType.Datatype;
			}
			CheckTokenizedTypes(datatype, typedValue, attrValue: false);
		}
		return typedValue;
	}

	private string GetTypeName(SchemaDeclBase decl)
	{
		string text = decl.SchemaType.QualifiedName.ToString();
		if (text.Length == 0)
		{
			text = decl.Datatype.TypeCodeString;
		}
		return text;
	}

	private void SaveTextValue(object value)
	{
		string value2 = value.ToString();
		textValue.Append(value2);
	}

	private void Push(XmlQualifiedName elementName)
	{
		context = (ValidationState)validationStack.Push();
		if (context == null)
		{
			context = new ValidationState();
			validationStack.AddToTop(context);
		}
		context.LocalName = elementName.Name;
		context.Namespace = elementName.Namespace;
		context.HasMatched = false;
		context.IsNill = false;
		context.IsDefault = false;
		context.CheckRequiredAttribute = true;
		context.ValidationSkipped = false;
		context.Validity = XmlSchemaValidity.NotKnown;
		context.NeedValidateChildren = false;
		context.ProcessContents = processContents;
		context.ElementDeclBeforeXsi = null;
		context.Constr = null;
	}

	private void Pop()
	{
		ValidationState obj = (ValidationState)validationStack.Pop();
		if (startIDConstraint == validationStack.Length)
		{
			startIDConstraint = -1;
		}
		context = (ValidationState)validationStack.Peek();
		if (obj.Validity == XmlSchemaValidity.Invalid)
		{
			context.Validity = XmlSchemaValidity.Invalid;
		}
		if (obj.ValidationSkipped)
		{
			context.ValidationSkipped = true;
		}
		processContents = context.ProcessContents;
	}

	private void AddXsiAttributes(ArrayList attList)
	{
		BuildXsiAttributes();
		if (attPresence[xsiTypeSO.QualifiedName] == null)
		{
			attList.Add(xsiTypeSO);
		}
		if (attPresence[xsiNilSO.QualifiedName] == null)
		{
			attList.Add(xsiNilSO);
		}
		if (attPresence[xsiSLSO.QualifiedName] == null)
		{
			attList.Add(xsiSLSO);
		}
		if (attPresence[xsiNoNsSLSO.QualifiedName] == null)
		{
			attList.Add(xsiNoNsSLSO);
		}
	}

	private SchemaElementDecl FastGetElementDecl(XmlQualifiedName elementName, object particle)
	{
		SchemaElementDecl schemaElementDecl = null;
		if (particle != null)
		{
			if (particle is XmlSchemaElement xmlSchemaElement)
			{
				schemaElementDecl = xmlSchemaElement.ElementDecl;
			}
			else
			{
				XmlSchemaAny xmlSchemaAny = (XmlSchemaAny)particle;
				processContents = xmlSchemaAny.ProcessContentsCorrect;
			}
		}
		if (schemaElementDecl == null && processContents != XmlSchemaContentProcessing.Skip)
		{
			if (isRoot && partialValidationType != null)
			{
				if (partialValidationType is XmlSchemaElement)
				{
					XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)partialValidationType;
					if (elementName.Equals(xmlSchemaElement2.QualifiedName))
					{
						schemaElementDecl = xmlSchemaElement2.ElementDecl;
					}
					else
					{
						SendValidationEvent("The element name '{0}' does not match the name '{1}' of the 'XmlSchemaElement' set as a partial validation type.", elementName.ToString(), xmlSchemaElement2.QualifiedName.ToString());
					}
				}
				else if (partialValidationType is XmlSchemaType)
				{
					schemaElementDecl = ((XmlSchemaType)partialValidationType).ElementDecl;
				}
				else
				{
					SendValidationEvent("If the partial validation type is 'XmlSchemaAttribute', the 'ValidateElement' method cannot be called.", string.Empty);
				}
			}
			else
			{
				schemaElementDecl = compiledSchemaInfo.GetElementDecl(elementName);
			}
		}
		return schemaElementDecl;
	}

	private SchemaElementDecl CheckXsiTypeAndNil(SchemaElementDecl elementDecl, string xsiType, string xsiNil, ref bool declFound)
	{
		XmlQualifiedName xmlQualifiedName = XmlQualifiedName.Empty;
		if (xsiType != null)
		{
			object typedValue = null;
			Exception ex = dtQName.TryParseValue(xsiType, nameTable, nsResolver, out typedValue);
			if (ex != null)
			{
				SendValidationEvent("The attribute '{0}' has an invalid value '{1}' according to its schema type '{2}' - {3}", new string[4] { "type", xsiType, dtQName.TypeCodeString, ex.Message }, ex);
			}
			else
			{
				xmlQualifiedName = typedValue as XmlQualifiedName;
			}
		}
		if (elementDecl != null)
		{
			if (elementDecl.IsNillable)
			{
				if (xsiNil != null)
				{
					context.IsNill = XmlConvert.ToBoolean(xsiNil);
					if (context.IsNill && elementDecl.Presence == SchemaDeclBase.Use.Fixed)
					{
						SendValidationEvent("There must be no fixed value when an attribute is 'xsi:nil' and has a value of 'true'.");
					}
				}
			}
			else if (xsiNil != null)
			{
				SendValidationEvent("If the 'nillable' attribute is false in the schema, the 'xsi:nil' attribute must not be present in the instance.");
			}
		}
		if (xmlQualifiedName.IsEmpty)
		{
			if (elementDecl != null && elementDecl.IsAbstract)
			{
				SendValidationEvent("The element '{0}' is abstract or its type is abstract.", QNameString(context.LocalName, context.Namespace));
				elementDecl = null;
			}
		}
		else
		{
			SchemaElementDecl schemaElementDecl = compiledSchemaInfo.GetTypeDecl(xmlQualifiedName);
			XmlSeverityType severity = XmlSeverityType.Warning;
			if (HasSchema && processContents == XmlSchemaContentProcessing.Strict)
			{
				severity = XmlSeverityType.Error;
			}
			if (schemaElementDecl == null && xmlQualifiedName.Namespace == NsXs)
			{
				XmlSchemaType xmlSchemaType = DatatypeImplementation.GetSimpleTypeFromXsdType(xmlQualifiedName);
				if (xmlSchemaType == null)
				{
					xmlSchemaType = XmlSchemaType.GetBuiltInComplexType(xmlQualifiedName);
				}
				if (xmlSchemaType != null)
				{
					schemaElementDecl = xmlSchemaType.ElementDecl;
				}
			}
			if (schemaElementDecl == null)
			{
				SendValidationEvent("This is an invalid xsi:type '{0}'.", xmlQualifiedName.ToString(), severity);
				elementDecl = null;
			}
			else
			{
				declFound = true;
				if (schemaElementDecl.IsAbstract)
				{
					SendValidationEvent("The xsi:type '{0}' cannot be abstract.", xmlQualifiedName.ToString(), severity);
					elementDecl = null;
				}
				else if (elementDecl != null && !XmlSchemaType.IsDerivedFrom(schemaElementDecl.SchemaType, elementDecl.SchemaType, elementDecl.Block))
				{
					SendValidationEvent("The xsi:type attribute value '{0}' is not valid for the element '{1}', either because it is not a type validly derived from the type in the schema, or because it has xsi:type derivation blocked.", new string[2]
					{
						xmlQualifiedName.ToString(),
						QNameString(context.LocalName, context.Namespace)
					});
					elementDecl = null;
				}
				else
				{
					if (elementDecl != null)
					{
						schemaElementDecl = schemaElementDecl.Clone();
						schemaElementDecl.Constraints = elementDecl.Constraints;
						schemaElementDecl.DefaultValueRaw = elementDecl.DefaultValueRaw;
						schemaElementDecl.DefaultValueTyped = elementDecl.DefaultValueTyped;
						schemaElementDecl.Block = elementDecl.Block;
					}
					context.ElementDeclBeforeXsi = elementDecl;
					elementDecl = schemaElementDecl;
				}
			}
		}
		return elementDecl;
	}

	private void ThrowDeclNotFoundWarningOrError(bool declFound)
	{
		if (declFound)
		{
			processContents = (context.ProcessContents = XmlSchemaContentProcessing.Skip);
			context.NeedValidateChildren = false;
		}
		else if (HasSchema && processContents == XmlSchemaContentProcessing.Strict)
		{
			processContents = (context.ProcessContents = XmlSchemaContentProcessing.Skip);
			context.NeedValidateChildren = false;
			SendValidationEvent("The '{0}' element is not declared.", QNameString(context.LocalName, context.Namespace));
		}
		else
		{
			SendValidationEvent("Could not find schema information for the element '{0}'.", QNameString(context.LocalName, context.Namespace), XmlSeverityType.Warning);
		}
	}

	private void CheckElementProperties()
	{
		if (context.ElementDecl.IsAbstract)
		{
			SendValidationEvent("The element '{0}' is abstract or its type is abstract.", QNameString(context.LocalName, context.Namespace));
		}
	}

	private void ValidateStartElementIdentityConstraints()
	{
		if (ProcessIdentityConstraints && context.ElementDecl.Constraints != null)
		{
			AddIdentityConstraints();
		}
		if (HasIdentityConstraints)
		{
			ElementIdentityConstraints();
		}
	}

	private SchemaAttDef CheckIsXmlAttribute(XmlQualifiedName attQName)
	{
		SchemaAttDef value = null;
		if (Ref.Equal(attQName.Namespace, NsXml) && (validationFlags & XmlSchemaValidationFlags.AllowXmlAttributes) != XmlSchemaValidationFlags.None)
		{
			if (!compiledSchemaInfo.Contains(NsXml))
			{
				AddXmlNamespaceSchema();
			}
			compiledSchemaInfo.AttributeDecls.TryGetValue(attQName, out value);
		}
		return value;
	}

	private void AddXmlNamespaceSchema()
	{
		XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
		xmlSchemaSet.Add(Preprocessor.GetBuildInSchema());
		xmlSchemaSet.Compile();
		schemaSet.Add(xmlSchemaSet);
		RecompileSchemaSet();
	}

	internal object CheckMixedValueConstraint(string elementValue)
	{
		SchemaElementDecl elementDecl = context.ElementDecl;
		if (context.IsNill)
		{
			return null;
		}
		if (elementValue.Length == 0)
		{
			context.IsDefault = true;
			return elementDecl.DefaultValueTyped;
		}
		if (elementDecl.Presence == SchemaDeclBase.Use.Fixed && !elementValue.Equals(elementDecl.DefaultValueRaw))
		{
			SendValidationEvent("The value of the '{0}' element does not equal its fixed value.", elementDecl.Name.ToString());
		}
		return elementValue;
	}

	private void LoadSchema(string uri, string url)
	{
		XmlReader xmlReader = null;
		try
		{
			Uri uri2 = xmlResolver.ResolveUri(sourceUri, url);
			Stream input = (Stream)xmlResolver.GetEntity(uri2, null, null);
			XmlReaderSettings readerSettings = schemaSet.ReaderSettings;
			readerSettings.CloseInput = true;
			readerSettings.XmlResolver = xmlResolver;
			xmlReader = XmlReader.Create(input, readerSettings, uri2.ToString());
			schemaSet.Add(uri, xmlReader, validatedNamespaces);
			while (xmlReader.Read())
			{
			}
		}
		catch (XmlSchemaException ex)
		{
			SendValidationEvent("Cannot load the schema for the namespace '{0}' - {1}", new string[2] { uri, ex.Message }, ex);
		}
		catch (Exception ex2)
		{
			SendValidationEvent("Cannot load the schema for the namespace '{0}' - {1}", new string[2] { uri, ex2.Message }, ex2, XmlSeverityType.Warning);
		}
		finally
		{
			xmlReader?.Close();
		}
	}

	internal void RecompileSchemaSet()
	{
		if (!schemaSet.IsCompiled)
		{
			try
			{
				schemaSet.Compile();
			}
			catch (XmlSchemaException e)
			{
				SendValidationEvent(e);
			}
		}
		compiledSchemaInfo = schemaSet.CompiledInfo;
	}

	private void ProcessTokenizedType(XmlTokenizedType ttype, string name, bool attrValue)
	{
		switch (ttype)
		{
		case XmlTokenizedType.ID:
			if (!ProcessIdentityConstraints)
			{
				break;
			}
			if (FindId(name) != null)
			{
				if (attrValue)
				{
					attrValid = false;
				}
				SendValidationEvent("'{0}' is already used as an ID.", name);
			}
			else
			{
				if (IDs == null)
				{
					IDs = new Hashtable();
				}
				IDs.Add(name, context.LocalName);
			}
			break;
		case XmlTokenizedType.IDREF:
			if (ProcessIdentityConstraints && FindId(name) == null)
			{
				idRefListHead = new IdRefNode(idRefListHead, name, positionInfo.LineNumber, positionInfo.LinePosition);
			}
			break;
		case XmlTokenizedType.ENTITY:
			ProcessEntity(name);
			break;
		case XmlTokenizedType.IDREFS:
			break;
		}
	}

	private object CheckAttributeValue(object value, SchemaAttDef attdef)
	{
		object typedValue = null;
		XmlSchemaDatatype datatype = attdef.Datatype;
		string text = value as string;
		Exception ex = null;
		if (text != null)
		{
			ex = datatype.TryParseValue(text, nameTable, nsResolver, out typedValue);
			if (ex == null)
			{
				goto IL_0050;
			}
		}
		else
		{
			ex = datatype.TryParseValue(value, nameTable, nsResolver, out typedValue);
			if (ex == null)
			{
				goto IL_0050;
			}
		}
		attrValid = false;
		if (text == null)
		{
			text = XmlSchemaDatatype.ConcatenatedToString(value);
		}
		SendValidationEvent("The '{0}' attribute is invalid - The value '{1}' is invalid according to its datatype '{2}' - {3}", new string[4]
		{
			attdef.Name.ToString(),
			text,
			GetTypeName(attdef),
			ex.Message
		}, ex);
		return null;
		IL_0050:
		if (!attdef.CheckValue(typedValue))
		{
			attrValid = false;
			SendValidationEvent("The value of the '{0}' attribute does not equal its fixed value.", attdef.Name.ToString());
		}
		return typedValue;
	}

	private object CheckElementValue(string stringValue)
	{
		object typedValue = null;
		SchemaDeclBase elementDecl = context.ElementDecl;
		Exception ex = elementDecl.Datatype.TryParseValue(stringValue, nameTable, nsResolver, out typedValue);
		if (ex != null)
		{
			SendValidationEvent("The '{0}' element is invalid - The value '{1}' is invalid according to its datatype '{2}' - {3}", new string[4]
			{
				QNameString(context.LocalName, context.Namespace),
				stringValue,
				GetTypeName(elementDecl),
				ex.Message
			}, ex);
			return null;
		}
		if (!elementDecl.CheckValue(typedValue))
		{
			SendValidationEvent("The value of the '{0}' element does not equal its fixed value.", QNameString(context.LocalName, context.Namespace));
		}
		return typedValue;
	}

	private void CheckTokenizedTypes(XmlSchemaDatatype dtype, object typedValue, bool attrValue)
	{
		if (typedValue == null)
		{
			return;
		}
		XmlTokenizedType tokenizedType = dtype.TokenizedType;
		if (tokenizedType != XmlTokenizedType.ENTITY && tokenizedType != XmlTokenizedType.ID && tokenizedType != XmlTokenizedType.IDREF)
		{
			return;
		}
		if (dtype.Variety == XmlSchemaDatatypeVariety.List)
		{
			string[] array = (string[])typedValue;
			for (int i = 0; i < array.Length; i++)
			{
				ProcessTokenizedType(dtype.TokenizedType, array[i], attrValue);
			}
		}
		else
		{
			ProcessTokenizedType(dtype.TokenizedType, (string)typedValue, attrValue);
		}
	}

	private object FindId(string name)
	{
		if (IDs != null)
		{
			return IDs[name];
		}
		return null;
	}

	private void CheckForwardRefs()
	{
		IdRefNode idRefNode = idRefListHead;
		while (idRefNode != null)
		{
			if (FindId(idRefNode.Id) == null)
			{
				SendValidationEvent(new XmlSchemaValidationException("Reference to undeclared ID is '{0}'.", idRefNode.Id, sourceUriString, idRefNode.LineNo, idRefNode.LinePos), XmlSeverityType.Error);
			}
			IdRefNode next = idRefNode.Next;
			idRefNode.Next = null;
			idRefNode = next;
		}
		idRefListHead = null;
	}

	private void CheckStateTransition(ValidatorState toState, string methodName)
	{
		if (!ValidStates[(int)currentState, (int)toState])
		{
			object[] args;
			if (currentState == ValidatorState.None)
			{
				args = new string[2]
				{
					methodName,
					MethodNames[1]
				};
				throw new InvalidOperationException(Res.GetString("It is invalid to call the '{0}' method in the current state of the validator. The '{1}' method must be called before proceeding with validation.", args));
			}
			args = new string[2]
			{
				MethodNames[(int)currentState],
				methodName
			};
			throw new InvalidOperationException(Res.GetString("The transition from the '{0}' method to the '{1}' method is not allowed.", args));
		}
		currentState = toState;
	}

	private void ClearPSVI()
	{
		if (textValue != null)
		{
			textValue.Length = 0;
		}
		attPresence.Clear();
		wildID = null;
	}

	private void CheckRequiredAttributes(SchemaElementDecl currentElementDecl)
	{
		foreach (SchemaAttDef value in currentElementDecl.AttDefs.Values)
		{
			if (attPresence[value.Name] == null && (value.Presence == SchemaDeclBase.Use.Required || value.Presence == SchemaDeclBase.Use.RequiredFixed))
			{
				SendValidationEvent("The required attribute '{0}' is missing.", value.Name.ToString());
			}
		}
	}

	private XmlSchemaElement GetSchemaElement()
	{
		SchemaElementDecl elementDeclBeforeXsi = context.ElementDeclBeforeXsi;
		SchemaElementDecl elementDecl = context.ElementDecl;
		if (elementDeclBeforeXsi != null && elementDeclBeforeXsi.SchemaElement != null)
		{
			XmlSchemaElement obj = (XmlSchemaElement)elementDeclBeforeXsi.SchemaElement.Clone(null);
			obj.SchemaTypeName = XmlQualifiedName.Empty;
			obj.SchemaType = elementDecl.SchemaType;
			obj.SetElementType(elementDecl.SchemaType);
			obj.ElementDecl = elementDecl;
			return obj;
		}
		return elementDecl.SchemaElement;
	}

	internal string GetDefaultAttributePrefix(string attributeNS)
	{
		IDictionary<string, string> namespacesInScope = nsResolver.GetNamespacesInScope(XmlNamespaceScope.All);
		string text = null;
		foreach (KeyValuePair<string, string> item in namespacesInScope)
		{
			if (Ref.Equal(nameTable.Add(item.Value), attributeNS))
			{
				text = item.Key;
				if (text.Length != 0)
				{
					return text;
				}
			}
		}
		return text;
	}

	private void AddIdentityConstraints()
	{
		SchemaElementDecl elementDecl = context.ElementDecl;
		context.Constr = new ConstraintStruct[elementDecl.Constraints.Length];
		int num = 0;
		for (int i = 0; i < elementDecl.Constraints.Length; i++)
		{
			context.Constr[num++] = new ConstraintStruct(elementDecl.Constraints[i]);
		}
		for (int j = 0; j < context.Constr.Length; j++)
		{
			if (context.Constr[j].constraint.Role != CompiledIdentityConstraint.ConstraintRole.Keyref)
			{
				continue;
			}
			bool flag = false;
			for (int num2 = validationStack.Length - 1; num2 >= ((startIDConstraint >= 0) ? startIDConstraint : (validationStack.Length - 1)); num2--)
			{
				if (((ValidationState)validationStack[num2]).Constr != null)
				{
					ConstraintStruct[] constr = ((ValidationState)validationStack[num2]).Constr;
					for (int k = 0; k < constr.Length; k++)
					{
						if (constr[k].constraint.name == context.Constr[j].constraint.refer)
						{
							flag = true;
							if (constr[k].keyrefTable == null)
							{
								constr[k].keyrefTable = new Hashtable();
							}
							context.Constr[j].qualifiedTable = constr[k].keyrefTable;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (!flag)
			{
				SendValidationEvent("The Keyref '{0}' cannot find the referred key or unique in scope.", QNameString(context.LocalName, context.Namespace));
			}
		}
		if (startIDConstraint == -1)
		{
			startIDConstraint = validationStack.Length - 1;
		}
	}

	private void ElementIdentityConstraints()
	{
		SchemaElementDecl elementDecl = context.ElementDecl;
		string localName = context.LocalName;
		string uRN = context.Namespace;
		for (int i = startIDConstraint; i < validationStack.Length; i++)
		{
			if (((ValidationState)validationStack[i]).Constr == null)
			{
				continue;
			}
			ConstraintStruct[] constr = ((ValidationState)validationStack[i]).Constr;
			for (int j = 0; j < constr.Length; j++)
			{
				if (constr[j].axisSelector.MoveToStartElement(localName, uRN))
				{
					constr[j].axisSelector.PushKS(positionInfo.LineNumber, positionInfo.LinePosition);
				}
				for (int k = 0; k < constr[j].axisFields.Count; k++)
				{
					LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)constr[j].axisFields[k];
					if (locatedActiveAxis.MoveToStartElement(localName, uRN) && elementDecl != null)
					{
						if (elementDecl.Datatype == null || elementDecl.ContentValidator.ContentType == XmlSchemaContentType.Mixed)
						{
							SendValidationEvent("The field '{0}' is expecting an element or attribute with simple type or simple content.", localName);
						}
						else
						{
							locatedActiveAxis.isMatched = true;
						}
					}
				}
			}
		}
	}

	private void AttributeIdentityConstraints(string name, string ns, object obj, string sobj, XmlSchemaDatatype datatype)
	{
		for (int i = startIDConstraint; i < validationStack.Length; i++)
		{
			if (((ValidationState)validationStack[i]).Constr == null)
			{
				continue;
			}
			ConstraintStruct[] constr = ((ValidationState)validationStack[i]).Constr;
			for (int j = 0; j < constr.Length; j++)
			{
				for (int k = 0; k < constr[j].axisFields.Count; k++)
				{
					LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)constr[j].axisFields[k];
					if (locatedActiveAxis.MoveToAttribute(name, ns))
					{
						if (locatedActiveAxis.Ks[locatedActiveAxis.Column] != null)
						{
							SendValidationEvent("The field '{0}' is expecting at the most one value.", name);
						}
						else
						{
							locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(obj, sobj, datatype);
						}
					}
				}
			}
		}
	}

	private void EndElementIdentityConstraints(object typedValue, string stringValue, XmlSchemaDatatype datatype)
	{
		string localName = context.LocalName;
		string uRN = context.Namespace;
		for (int num = validationStack.Length - 1; num >= startIDConstraint; num--)
		{
			if (((ValidationState)validationStack[num]).Constr != null)
			{
				ConstraintStruct[] constr = ((ValidationState)validationStack[num]).Constr;
				for (int i = 0; i < constr.Length; i++)
				{
					for (int j = 0; j < constr[i].axisFields.Count; j++)
					{
						LocatedActiveAxis locatedActiveAxis = (LocatedActiveAxis)constr[i].axisFields[j];
						if (locatedActiveAxis.isMatched)
						{
							locatedActiveAxis.isMatched = false;
							if (locatedActiveAxis.Ks[locatedActiveAxis.Column] != null)
							{
								SendValidationEvent("The field '{0}' is expecting at the most one value.", localName);
							}
							else if (System.LocalAppContextSwitches.IgnoreEmptyKeySequences)
							{
								if (typedValue != null && stringValue.Length != 0)
								{
									locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(typedValue, stringValue, datatype);
								}
							}
							else if (typedValue != null)
							{
								locatedActiveAxis.Ks[locatedActiveAxis.Column] = new TypedObject(typedValue, stringValue, datatype);
							}
						}
						locatedActiveAxis.EndElement(localName, uRN);
					}
					if (!constr[i].axisSelector.EndElement(localName, uRN))
					{
						continue;
					}
					KeySequence keySequence = constr[i].axisSelector.PopKS();
					switch (constr[i].constraint.Role)
					{
					case CompiledIdentityConstraint.ConstraintRole.Key:
						if (!keySequence.IsQualified())
						{
							SendValidationEvent(new XmlSchemaValidationException("The identity constraint '{0}' validation has failed. Either a key is missing or the existing key has an empty node.", constr[i].constraint.name.ToString(), sourceUriString, keySequence.PosLine, keySequence.PosCol));
						}
						else if (constr[i].qualifiedTable.Contains(keySequence))
						{
							SendValidationEvent(new XmlSchemaValidationException("There is a duplicate key sequence '{0}' for the '{1}' key or unique identity constraint.", new string[2]
							{
								keySequence.ToString(),
								constr[i].constraint.name.ToString()
							}, sourceUriString, keySequence.PosLine, keySequence.PosCol));
						}
						else
						{
							constr[i].qualifiedTable.Add(keySequence, keySequence);
						}
						break;
					case CompiledIdentityConstraint.ConstraintRole.Unique:
						if (keySequence.IsQualified())
						{
							if (constr[i].qualifiedTable.Contains(keySequence))
							{
								SendValidationEvent(new XmlSchemaValidationException("There is a duplicate key sequence '{0}' for the '{1}' key or unique identity constraint.", new string[2]
								{
									keySequence.ToString(),
									constr[i].constraint.name.ToString()
								}, sourceUriString, keySequence.PosLine, keySequence.PosCol));
							}
							else
							{
								constr[i].qualifiedTable.Add(keySequence, keySequence);
							}
						}
						break;
					case CompiledIdentityConstraint.ConstraintRole.Keyref:
						if (constr[i].qualifiedTable != null && keySequence.IsQualified() && !constr[i].qualifiedTable.Contains(keySequence))
						{
							constr[i].qualifiedTable.Add(keySequence, keySequence);
						}
						break;
					}
				}
			}
		}
		ConstraintStruct[] constr2 = ((ValidationState)validationStack[validationStack.Length - 1]).Constr;
		if (constr2 == null)
		{
			return;
		}
		for (int k = 0; k < constr2.Length; k++)
		{
			if (constr2[k].constraint.Role == CompiledIdentityConstraint.ConstraintRole.Keyref || constr2[k].keyrefTable == null)
			{
				continue;
			}
			foreach (KeySequence key in constr2[k].keyrefTable.Keys)
			{
				if (!constr2[k].qualifiedTable.Contains(key))
				{
					SendValidationEvent(new XmlSchemaValidationException("The key sequence '{0}' in '{1}' Keyref fails to refer to some key.", new string[2]
					{
						key.ToString(),
						constr2[k].constraint.name.ToString()
					}, sourceUriString, key.PosLine, key.PosCol));
				}
			}
		}
	}

	private static void BuildXsiAttributes()
	{
		if (xsiTypeSO == null)
		{
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "type";
			xmlSchemaAttribute.SetQualifiedName(new XmlQualifiedName("type", "http://www.w3.org/2001/XMLSchema-instance"));
			xmlSchemaAttribute.SetAttributeType(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.QName));
			Interlocked.CompareExchange(ref xsiTypeSO, xmlSchemaAttribute, null);
		}
		if (xsiNilSO == null)
		{
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "nil";
			xmlSchemaAttribute2.SetQualifiedName(new XmlQualifiedName("nil", "http://www.w3.org/2001/XMLSchema-instance"));
			xmlSchemaAttribute2.SetAttributeType(XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.Boolean));
			Interlocked.CompareExchange(ref xsiNilSO, xmlSchemaAttribute2, null);
		}
		if (xsiSLSO == null)
		{
			XmlSchemaSimpleType builtInSimpleType = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String);
			XmlSchemaAttribute xmlSchemaAttribute3 = new XmlSchemaAttribute();
			xmlSchemaAttribute3.Name = "schemaLocation";
			xmlSchemaAttribute3.SetQualifiedName(new XmlQualifiedName("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
			xmlSchemaAttribute3.SetAttributeType(builtInSimpleType);
			Interlocked.CompareExchange(ref xsiSLSO, xmlSchemaAttribute3, null);
		}
		if (xsiNoNsSLSO == null)
		{
			XmlSchemaSimpleType builtInSimpleType2 = XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String);
			XmlSchemaAttribute xmlSchemaAttribute4 = new XmlSchemaAttribute();
			xmlSchemaAttribute4.Name = "noNamespaceSchemaLocation";
			xmlSchemaAttribute4.SetQualifiedName(new XmlQualifiedName("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance"));
			xmlSchemaAttribute4.SetAttributeType(builtInSimpleType2);
			Interlocked.CompareExchange(ref xsiNoNsSLSO, xmlSchemaAttribute4, null);
		}
	}

	internal static void ElementValidationError(XmlQualifiedName name, ValidationState context, ValidationEventHandler eventHandler, object sender, string sourceUri, int lineNo, int linePos, XmlSchemaSet schemaSet)
	{
		ArrayList arrayList = null;
		if (context.ElementDecl == null)
		{
			return;
		}
		ContentValidator contentValidator = context.ElementDecl.ContentValidator;
		XmlSchemaContentType contentType = contentValidator.ContentType;
		if (contentType == XmlSchemaContentType.ElementOnly || (contentType == XmlSchemaContentType.Mixed && contentValidator != ContentValidator.Mixed && contentValidator != ContentValidator.Any))
		{
			bool flag = schemaSet != null;
			arrayList = ((!flag) ? contentValidator.ExpectedElements(context, isRequiredOnly: false) : contentValidator.ExpectedParticles(context, isRequiredOnly: false, schemaSet));
			if (arrayList == null || arrayList.Count == 0)
			{
				if (context.TooComplex)
				{
					SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has invalid child element {1} - {2}", new string[3]
					{
						BuildElementName(context.LocalName, context.Namespace),
						BuildElementName(name),
						Res.GetString("Content model validation resulted in a large number of states, possibly due to large occurrence ranges. Therefore, content model may not be validated accurately.")
					}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
				}
				else
				{
					SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has invalid child element {1}.", new string[2]
					{
						BuildElementName(context.LocalName, context.Namespace),
						BuildElementName(name)
					}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
				}
			}
			else if (context.TooComplex)
			{
				SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has invalid child element {1}. List of possible elements expected: {2}. {3}", new string[4]
				{
					BuildElementName(context.LocalName, context.Namespace),
					BuildElementName(name),
					PrintExpectedElements(arrayList, flag),
					Res.GetString("Content model validation resulted in a large number of states, possibly due to large occurrence ranges. Therefore, content model may not be validated accurately.")
				}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
			}
			else
			{
				SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has invalid child element {1}. List of possible elements expected: {2}.", new string[3]
				{
					BuildElementName(context.LocalName, context.Namespace),
					BuildElementName(name),
					PrintExpectedElements(arrayList, flag)
				}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
			}
		}
		else if (contentType == XmlSchemaContentType.Empty)
		{
			SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element '{0}' cannot contain child element '{1}' because the parent element's content model is empty.", new string[2]
			{
				QNameString(context.LocalName, context.Namespace),
				name.ToString()
			}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
		}
		else if (!contentValidator.IsOpen)
		{
			SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element '{0}' cannot contain child element '{1}' because the parent element's content model is text only.", new string[2]
			{
				QNameString(context.LocalName, context.Namespace),
				name.ToString()
			}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
		}
	}

	internal static void CompleteValidationError(ValidationState context, ValidationEventHandler eventHandler, object sender, string sourceUri, int lineNo, int linePos, XmlSchemaSet schemaSet)
	{
		ArrayList arrayList = null;
		bool flag = schemaSet != null;
		if (context.ElementDecl != null)
		{
			arrayList = ((!flag) ? context.ElementDecl.ContentValidator.ExpectedElements(context, isRequiredOnly: true) : context.ElementDecl.ContentValidator.ExpectedParticles(context, isRequiredOnly: true, schemaSet));
		}
		if (arrayList == null || arrayList.Count == 0)
		{
			if (context.TooComplex)
			{
				SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has incomplete content - {2}", new string[2]
				{
					BuildElementName(context.LocalName, context.Namespace),
					Res.GetString("Content model validation resulted in a large number of states, possibly due to large occurrence ranges. Therefore, content model may not be validated accurately.")
				}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
			}
			SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has incomplete content.", BuildElementName(context.LocalName, context.Namespace), sourceUri, lineNo, linePos), XmlSeverityType.Error);
		}
		else if (context.TooComplex)
		{
			SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has incomplete content. List of possible elements expected: {1}. {2}", new string[3]
			{
				BuildElementName(context.LocalName, context.Namespace),
				PrintExpectedElements(arrayList, flag),
				Res.GetString("Content model validation resulted in a large number of states, possibly due to large occurrence ranges. Therefore, content model may not be validated accurately.")
			}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
		}
		else
		{
			SendValidationEvent(eventHandler, sender, new XmlSchemaValidationException("The element {0} has incomplete content. List of possible elements expected: {1}.", new string[2]
			{
				BuildElementName(context.LocalName, context.Namespace),
				PrintExpectedElements(arrayList, flag)
			}, sourceUri, lineNo, linePos), XmlSeverityType.Error);
		}
	}

	internal static string PrintExpectedElements(ArrayList expected, bool getParticles)
	{
		if (getParticles)
		{
			object[] args = new string[1] { " " };
			string value = Res.GetString("{0}as well as", args);
			XmlSchemaParticle xmlSchemaParticle = null;
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = new StringBuilder();
			if (expected.Count == 1)
			{
				xmlSchemaParticle = expected[0] as XmlSchemaParticle;
			}
			else
			{
				for (int i = 1; i < expected.Count; i++)
				{
					XmlSchemaParticle obj = expected[i - 1] as XmlSchemaParticle;
					xmlSchemaParticle = expected[i] as XmlSchemaParticle;
					XmlQualifiedName qualifiedName = obj.GetQualifiedName();
					if (qualifiedName.Namespace != xmlSchemaParticle.GetQualifiedName().Namespace)
					{
						arrayList.Add(qualifiedName);
						PrintNamesWithNS(arrayList, stringBuilder);
						arrayList.Clear();
						stringBuilder.Append(value);
					}
					else
					{
						arrayList.Add(qualifiedName);
					}
				}
			}
			arrayList.Add(xmlSchemaParticle.GetQualifiedName());
			PrintNamesWithNS(arrayList, stringBuilder);
			return stringBuilder.ToString();
		}
		return PrintNames(expected);
	}

	private static string PrintNames(ArrayList expected)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("'");
		stringBuilder.Append(expected[0].ToString());
		for (int i = 1; i < expected.Count; i++)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(expected[i].ToString());
		}
		stringBuilder.Append("'");
		return stringBuilder.ToString();
	}

	private static void PrintNamesWithNS(ArrayList expected, StringBuilder builder)
	{
		XmlQualifiedName xmlQualifiedName = null;
		xmlQualifiedName = expected[0] as XmlQualifiedName;
		if (expected.Count == 1)
		{
			if (xmlQualifiedName.Name == "*")
			{
				EnumerateAny(builder, xmlQualifiedName.Namespace);
			}
			else if (xmlQualifiedName.Namespace.Length != 0)
			{
				builder.Append(Res.GetString("'{0}' in namespace '{1}'", xmlQualifiedName.Name, xmlQualifiedName.Namespace));
			}
			else
			{
				builder.Append(Res.GetString("'{0}'", xmlQualifiedName.Name));
			}
			return;
		}
		bool flag = false;
		bool flag2 = true;
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < expected.Count; i++)
		{
			xmlQualifiedName = expected[i] as XmlQualifiedName;
			if (xmlQualifiedName.Name == "*")
			{
				flag = true;
				continue;
			}
			if (flag2)
			{
				flag2 = false;
			}
			else
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(xmlQualifiedName.Name);
		}
		if (flag)
		{
			stringBuilder.Append(", ");
			stringBuilder.Append(Res.GetString("any element"));
		}
		else if (xmlQualifiedName.Namespace.Length != 0)
		{
			builder.Append(Res.GetString("'{0}' in namespace '{1}'", stringBuilder.ToString(), xmlQualifiedName.Namespace));
		}
		else
		{
			builder.Append(Res.GetString("'{0}'", stringBuilder.ToString()));
		}
	}

	private static void EnumerateAny(StringBuilder builder, string namespaces)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (namespaces == "##any" || namespaces == "##other")
		{
			stringBuilder.Append(namespaces);
		}
		else
		{
			string[] array = XmlConvert.SplitString(namespaces);
			stringBuilder.Append(array[0]);
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.Append(", ");
				stringBuilder.Append(array[i]);
			}
		}
		builder.Append(Res.GetString("any element in namespace '{0}'", stringBuilder.ToString()));
	}

	internal static string QNameString(string localName, string ns)
	{
		if (ns.Length == 0)
		{
			return localName;
		}
		return ns + ":" + localName;
	}

	internal static string BuildElementName(XmlQualifiedName qname)
	{
		return BuildElementName(qname.Name, qname.Namespace);
	}

	internal static string BuildElementName(string localName, string ns)
	{
		if (ns.Length != 0)
		{
			return Res.GetString("'{0}' in namespace '{1}'", localName, ns);
		}
		return Res.GetString("'{0}'", localName);
	}

	private void ProcessEntity(string name)
	{
		if (checkEntity)
		{
			IDtdEntityInfo dtdEntityInfo = null;
			if (dtdSchemaInfo != null)
			{
				dtdEntityInfo = dtdSchemaInfo.LookupEntity(name);
			}
			if (dtdEntityInfo == null)
			{
				SendValidationEvent("Reference to an undeclared entity, '{0}'.", name);
			}
			else if (dtdEntityInfo.IsUnparsedEntity)
			{
				SendValidationEvent("Reference to an unparsed entity, '{0}'.", name);
			}
		}
	}

	private void SendValidationEvent(string code)
	{
		SendValidationEvent(code, string.Empty);
	}

	private void SendValidationEvent(string code, string[] args)
	{
		SendValidationEvent(new XmlSchemaValidationException(code, args, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	private void SendValidationEvent(string code, string arg)
	{
		SendValidationEvent(new XmlSchemaValidationException(code, arg, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	private void SendValidationEvent(string code, string arg1, string arg2)
	{
		SendValidationEvent(new XmlSchemaValidationException(code, new string[2] { arg1, arg2 }, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	private void SendValidationEvent(string code, string[] args, Exception innerException, XmlSeverityType severity)
	{
		if (severity != XmlSeverityType.Warning || ReportValidationWarnings)
		{
			SendValidationEvent(new XmlSchemaValidationException(code, args, innerException, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition), severity);
		}
	}

	private void SendValidationEvent(string code, string[] args, Exception innerException)
	{
		SendValidationEvent(new XmlSchemaValidationException(code, args, innerException, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition), XmlSeverityType.Error);
	}

	private void SendValidationEvent(XmlSchemaValidationException e)
	{
		SendValidationEvent(e, XmlSeverityType.Error);
	}

	private void SendValidationEvent(XmlSchemaException e)
	{
		SendValidationEvent(new XmlSchemaValidationException(e.GetRes, e.Args, e.SourceUri, e.LineNumber, e.LinePosition), XmlSeverityType.Error);
	}

	private void SendValidationEvent(string code, string msg, XmlSeverityType severity)
	{
		if (severity != XmlSeverityType.Warning || ReportValidationWarnings)
		{
			SendValidationEvent(new XmlSchemaValidationException(code, msg, sourceUriString, positionInfo.LineNumber, positionInfo.LinePosition), severity);
		}
	}

	private void SendValidationEvent(XmlSchemaValidationException e, XmlSeverityType severity)
	{
		bool flag = false;
		if (severity == XmlSeverityType.Error)
		{
			flag = true;
			context.Validity = XmlSchemaValidity.Invalid;
		}
		if (flag)
		{
			if (eventHandler == null)
			{
				throw e;
			}
			eventHandler(validationEventSender, new ValidationEventArgs(e, severity));
		}
		else if (ReportValidationWarnings && eventHandler != null)
		{
			eventHandler(validationEventSender, new ValidationEventArgs(e, severity));
		}
	}

	internal static void SendValidationEvent(ValidationEventHandler eventHandler, object sender, XmlSchemaValidationException e, XmlSeverityType severity)
	{
		if (eventHandler != null)
		{
			eventHandler(sender, new ValidationEventArgs(e, severity));
		}
		else if (severity == XmlSeverityType.Error)
		{
			throw e;
		}
	}
}
