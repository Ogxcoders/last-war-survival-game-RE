using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml;

internal class XsdValidatingReader : XmlReader, IXmlSchemaInfo, IXmlLineInfo, IXmlNamespaceResolver
{
	private enum ValidatingReaderState
	{
		None = 0,
		Init = 1,
		Read = 2,
		OnDefaultAttribute = -1,
		OnReadAttributeValue = -2,
		OnAttribute = 3,
		ClearAttributes = 4,
		ParseInlineSchema = 5,
		ReadAhead = 6,
		OnReadBinaryContent = 7,
		ReaderClosed = 8,
		EOF = 9,
		Error = 10
	}

	private XmlReader coreReader;

	private IXmlNamespaceResolver coreReaderNSResolver;

	private IXmlNamespaceResolver thisNSResolver;

	private XmlSchemaValidator validator;

	private XmlResolver xmlResolver;

	private ValidationEventHandler validationEvent;

	private ValidatingReaderState validationState;

	private XmlValueGetter valueGetter;

	private XmlNamespaceManager nsManager;

	private bool manageNamespaces;

	private bool processInlineSchema;

	private bool replayCache;

	private ValidatingReaderNodeData cachedNode;

	private AttributePSVIInfo attributePSVI;

	private int attributeCount;

	private int coreReaderAttributeCount;

	private int currentAttrIndex;

	private AttributePSVIInfo[] attributePSVINodes;

	private ArrayList defaultAttributes;

	private Parser inlineSchemaParser;

	private object atomicValue;

	private XmlSchemaInfo xmlSchemaInfo;

	private string originalAtomicValueString;

	private XmlNameTable coreReaderNameTable;

	private XsdCachingReader cachingReader;

	private ValidatingReaderNodeData textNode;

	private string NsXmlNs;

	private string NsXs;

	private string NsXsi;

	private string XsiType;

	private string XsiNil;

	private string XsdSchema;

	private string XsiSchemaLocation;

	private string XsiNoNamespaceSchemaLocation;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	private IXmlLineInfo lineInfo;

	private ReadContentAsBinaryHelper readBinaryHelper;

	private ValidatingReaderState savedState;

	private const int InitialAttributeCount = 8;

	private static volatile Type TypeOfString;

	public override XmlReaderSettings Settings
	{
		get
		{
			XmlReaderSettings xmlReaderSettings = coreReader.Settings;
			if (xmlReaderSettings != null)
			{
				xmlReaderSettings = xmlReaderSettings.Clone();
			}
			if (xmlReaderSettings == null)
			{
				xmlReaderSettings = new XmlReaderSettings();
			}
			xmlReaderSettings.Schemas = validator.SchemaSet;
			xmlReaderSettings.ValidationType = ValidationType.Schema;
			xmlReaderSettings.ValidationFlags = validator.ValidationFlags;
			xmlReaderSettings.ReadOnly = true;
			return xmlReaderSettings;
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return cachedNode.NodeType;
			}
			XmlNodeType nodeType = coreReader.NodeType;
			if (nodeType == XmlNodeType.Whitespace && (validator.CurrentContentType == XmlSchemaContentType.TextOnly || validator.CurrentContentType == XmlSchemaContentType.Mixed))
			{
				return XmlNodeType.SignificantWhitespace;
			}
			return nodeType;
		}
	}

	public override string Name
	{
		get
		{
			if (validationState == ValidatingReaderState.OnDefaultAttribute)
			{
				string defaultAttributePrefix = validator.GetDefaultAttributePrefix(cachedNode.Namespace);
				if (defaultAttributePrefix != null && defaultAttributePrefix.Length != 0)
				{
					return string.Concat(defaultAttributePrefix + ":" + cachedNode.LocalName);
				}
				return cachedNode.LocalName;
			}
			return coreReader.Name;
		}
	}

	public override string LocalName
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return cachedNode.LocalName;
			}
			return coreReader.LocalName;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return cachedNode.Namespace;
			}
			return coreReader.NamespaceURI;
		}
	}

	public override string Prefix
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return cachedNode.Prefix;
			}
			return coreReader.Prefix;
		}
	}

	public override bool HasValue
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return true;
			}
			return coreReader.HasValue;
		}
	}

	public override string Value
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return cachedNode.RawValue;
			}
			return coreReader.Value;
		}
	}

	public override int Depth
	{
		get
		{
			if (validationState < ValidatingReaderState.None)
			{
				return cachedNode.Depth;
			}
			return coreReader.Depth;
		}
	}

	public override string BaseURI => coreReader.BaseURI;

	public override bool IsEmptyElement => coreReader.IsEmptyElement;

	public override bool IsDefault
	{
		get
		{
			if (validationState == ValidatingReaderState.OnDefaultAttribute)
			{
				return true;
			}
			return coreReader.IsDefault;
		}
	}

	public override char QuoteChar => coreReader.QuoteChar;

	public override XmlSpace XmlSpace => coreReader.XmlSpace;

	public override string XmlLang => coreReader.XmlLang;

	public override IXmlSchemaInfo SchemaInfo => this;

	public override Type ValueType
	{
		get
		{
			switch (NodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.EndElement:
				if (xmlSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
				{
					return xmlSchemaInfo.SchemaType.Datatype.ValueType;
				}
				break;
			case XmlNodeType.Attribute:
				if (attributePSVI != null && AttributeSchemaInfo.ContentType == XmlSchemaContentType.TextOnly)
				{
					return AttributeSchemaInfo.SchemaType.Datatype.ValueType;
				}
				break;
			}
			return TypeOfString;
		}
	}

	public override int AttributeCount => attributeCount;

	public override bool EOF => coreReader.EOF;

	public override ReadState ReadState
	{
		get
		{
			if (validationState != ValidatingReaderState.Init)
			{
				return coreReader.ReadState;
			}
			return ReadState.Initial;
		}
	}

	public override XmlNameTable NameTable => coreReaderNameTable;

	public override bool CanReadBinaryContent => true;

	bool IXmlSchemaInfo.IsDefault
	{
		get
		{
			switch (NodeType)
			{
			case XmlNodeType.Element:
				if (!coreReader.IsEmptyElement)
				{
					GetIsDefault();
				}
				return xmlSchemaInfo.IsDefault;
			case XmlNodeType.EndElement:
				return xmlSchemaInfo.IsDefault;
			case XmlNodeType.Attribute:
				if (attributePSVI != null)
				{
					return AttributeSchemaInfo.IsDefault;
				}
				break;
			}
			return false;
		}
	}

	bool IXmlSchemaInfo.IsNil
	{
		get
		{
			XmlNodeType nodeType = NodeType;
			if (nodeType == XmlNodeType.Element || nodeType == XmlNodeType.EndElement)
			{
				return xmlSchemaInfo.IsNil;
			}
			return false;
		}
	}

	XmlSchemaValidity IXmlSchemaInfo.Validity
	{
		get
		{
			switch (NodeType)
			{
			case XmlNodeType.Element:
				if (coreReader.IsEmptyElement)
				{
					return xmlSchemaInfo.Validity;
				}
				if (xmlSchemaInfo.Validity == XmlSchemaValidity.Valid)
				{
					return XmlSchemaValidity.NotKnown;
				}
				return xmlSchemaInfo.Validity;
			case XmlNodeType.EndElement:
				return xmlSchemaInfo.Validity;
			case XmlNodeType.Attribute:
				if (attributePSVI != null)
				{
					return AttributeSchemaInfo.Validity;
				}
				break;
			}
			return XmlSchemaValidity.NotKnown;
		}
	}

	XmlSchemaSimpleType IXmlSchemaInfo.MemberType
	{
		get
		{
			switch (NodeType)
			{
			case XmlNodeType.Element:
				if (!coreReader.IsEmptyElement)
				{
					GetMemberType();
				}
				return xmlSchemaInfo.MemberType;
			case XmlNodeType.EndElement:
				return xmlSchemaInfo.MemberType;
			case XmlNodeType.Attribute:
				if (attributePSVI != null)
				{
					return AttributeSchemaInfo.MemberType;
				}
				return null;
			default:
				return null;
			}
		}
	}

	XmlSchemaType IXmlSchemaInfo.SchemaType
	{
		get
		{
			switch (NodeType)
			{
			case XmlNodeType.Element:
			case XmlNodeType.EndElement:
				return xmlSchemaInfo.SchemaType;
			case XmlNodeType.Attribute:
				if (attributePSVI != null)
				{
					return AttributeSchemaInfo.SchemaType;
				}
				return null;
			default:
				return null;
			}
		}
	}

	XmlSchemaElement IXmlSchemaInfo.SchemaElement
	{
		get
		{
			if (NodeType == XmlNodeType.Element || NodeType == XmlNodeType.EndElement)
			{
				return xmlSchemaInfo.SchemaElement;
			}
			return null;
		}
	}

	XmlSchemaAttribute IXmlSchemaInfo.SchemaAttribute
	{
		get
		{
			if (NodeType == XmlNodeType.Attribute && attributePSVI != null)
			{
				return AttributeSchemaInfo.SchemaAttribute;
			}
			return null;
		}
	}

	public int LineNumber
	{
		get
		{
			if (lineInfo != null)
			{
				return lineInfo.LineNumber;
			}
			return 0;
		}
	}

	public int LinePosition
	{
		get
		{
			if (lineInfo != null)
			{
				return lineInfo.LinePosition;
			}
			return 0;
		}
	}

	private XmlSchemaType ElementXmlType => xmlSchemaInfo.XmlType;

	private XmlSchemaType AttributeXmlType
	{
		get
		{
			if (attributePSVI != null)
			{
				return AttributeSchemaInfo.XmlType;
			}
			return null;
		}
	}

	private XmlSchemaInfo AttributeSchemaInfo => attributePSVI.attributeSchemaInfo;

	internal XsdValidatingReader(XmlReader reader, XmlResolver xmlResolver, XmlReaderSettings readerSettings, XmlSchemaObject partialValidationType)
	{
		coreReader = reader;
		coreReaderNSResolver = reader as IXmlNamespaceResolver;
		lineInfo = reader as IXmlLineInfo;
		coreReaderNameTable = coreReader.NameTable;
		if (coreReaderNSResolver == null)
		{
			nsManager = new XmlNamespaceManager(coreReaderNameTable);
			manageNamespaces = true;
		}
		thisNSResolver = this;
		this.xmlResolver = xmlResolver;
		processInlineSchema = (readerSettings.ValidationFlags & XmlSchemaValidationFlags.ProcessInlineSchema) != 0;
		Init();
		SetupValidator(readerSettings, reader, partialValidationType);
		validationEvent = readerSettings.GetEventHandler();
	}

	internal XsdValidatingReader(XmlReader reader, XmlResolver xmlResolver, XmlReaderSettings readerSettings)
		: this(reader, xmlResolver, readerSettings, null)
	{
	}

	private void Init()
	{
		validationState = ValidatingReaderState.Init;
		defaultAttributes = new ArrayList();
		currentAttrIndex = -1;
		attributePSVINodes = new AttributePSVIInfo[8];
		valueGetter = GetStringValue;
		TypeOfString = typeof(string);
		xmlSchemaInfo = new XmlSchemaInfo();
		NsXmlNs = coreReaderNameTable.Add("http://www.w3.org/2000/xmlns/");
		NsXs = coreReaderNameTable.Add("http://www.w3.org/2001/XMLSchema");
		NsXsi = coreReaderNameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
		XsiType = coreReaderNameTable.Add("type");
		XsiNil = coreReaderNameTable.Add("nil");
		XsiSchemaLocation = coreReaderNameTable.Add("schemaLocation");
		XsiNoNamespaceSchemaLocation = coreReaderNameTable.Add("noNamespaceSchemaLocation");
		XsdSchema = coreReaderNameTable.Add("schema");
	}

	private void SetupValidator(XmlReaderSettings readerSettings, XmlReader reader, XmlSchemaObject partialValidationType)
	{
		validator = new XmlSchemaValidator(coreReaderNameTable, readerSettings.Schemas, thisNSResolver, readerSettings.ValidationFlags);
		validator.XmlResolver = xmlResolver;
		validator.SourceUri = XmlConvert.ToUri(reader.BaseURI);
		validator.ValidationEventSender = this;
		validator.ValidationEventHandler += readerSettings.GetEventHandler();
		validator.LineInfoProvider = lineInfo;
		if (validator.ProcessSchemaHints)
		{
			validator.SchemaSet.ReaderSettings.DtdProcessing = readerSettings.DtdProcessing;
		}
		validator.SetDtdSchemaInfo(reader.DtdInfo);
		if (partialValidationType != null)
		{
			validator.Initialize(partialValidationType);
		}
		else
		{
			validator.Initialize();
		}
	}

	public override object ReadContentAsObject()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsObject");
		}
		return InternalReadContentAsObject(unwrapTypedValue: true);
	}

	public override bool ReadContentAsBoolean()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsBoolean");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToBoolean(value) ?? XmlUntypedConverter.Untyped.ToBoolean(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException3, this);
		}
	}

	public override DateTime ReadContentAsDateTime()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsDateTime");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToDateTime(value) ?? XmlUntypedConverter.Untyped.ToDateTime(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException3, this);
		}
	}

	public override double ReadContentAsDouble()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsDouble");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToDouble(value) ?? XmlUntypedConverter.Untyped.ToDouble(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException3, this);
		}
	}

	public override float ReadContentAsFloat()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsFloat");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToSingle(value) ?? XmlUntypedConverter.Untyped.ToSingle(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException3, this);
		}
	}

	public override decimal ReadContentAsDecimal()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsDecimal");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToDecimal(value) ?? XmlUntypedConverter.Untyped.ToDecimal(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException3, this);
		}
	}

	public override int ReadContentAsInt()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsInt");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToInt32(value) ?? XmlUntypedConverter.Untyped.ToInt32(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Int", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Int", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Int", innerException3, this);
		}
	}

	public override long ReadContentAsLong()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsLong");
		}
		object value = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			return xmlSchemaType?.ValueConverter.ToInt64(value) ?? XmlUntypedConverter.Untyped.ToInt64(value);
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Long", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Long", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Long", innerException3, this);
		}
	}

	public override string ReadContentAsString()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsString");
		}
		object obj = InternalReadContentAsObject();
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			if (xmlSchemaType != null)
			{
				return xmlSchemaType.ValueConverter.ToString(obj);
			}
			return obj as string;
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException3, this);
		}
	}

	public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAs");
		}
		string originalStringValue;
		object value = InternalReadContentAsObject(unwrapTypedValue: false, out originalStringValue);
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			if (xmlSchemaType != null)
			{
				if (returnType == typeof(DateTimeOffset) && xmlSchemaType.Datatype is Datatype_dateTimeBase)
				{
					value = originalStringValue;
				}
				return xmlSchemaType.ValueConverter.ChangeType(value, returnType);
			}
			return XmlUntypedConverter.Untyped.ChangeType(value, returnType, namespaceResolver);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException3, this);
		}
	}

	public override object ReadElementContentAsObject()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsObject");
		}
		XmlSchemaType xmlType;
		return InternalReadElementContentAsObject(out xmlType, unwrapTypedValue: true);
	}

	public override bool ReadElementContentAsBoolean()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsBoolean");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToBoolean(value) ?? XmlUntypedConverter.Untyped.ToBoolean(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Boolean", innerException3, this);
		}
	}

	public override DateTime ReadElementContentAsDateTime()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsDateTime");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToDateTime(value) ?? XmlUntypedConverter.Untyped.ToDateTime(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "DateTime", innerException3, this);
		}
	}

	public override double ReadElementContentAsDouble()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsDouble");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToDouble(value) ?? XmlUntypedConverter.Untyped.ToDouble(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Double", innerException3, this);
		}
	}

	public override float ReadElementContentAsFloat()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsFloat");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToSingle(value) ?? XmlUntypedConverter.Untyped.ToSingle(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Float", innerException3, this);
		}
	}

	public override decimal ReadElementContentAsDecimal()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsDecimal");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToDecimal(value) ?? XmlUntypedConverter.Untyped.ToDecimal(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Decimal", innerException3, this);
		}
	}

	public override int ReadElementContentAsInt()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsInt");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToInt32(value) ?? XmlUntypedConverter.Untyped.ToInt32(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Int", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Int", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Int", innerException3, this);
		}
	}

	public override long ReadElementContentAsLong()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsLong");
		}
		XmlSchemaType xmlType;
		object value = InternalReadElementContentAsObject(out xmlType);
		try
		{
			return xmlType?.ValueConverter.ToInt64(value) ?? XmlUntypedConverter.Untyped.ToInt64(value);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Long", innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Long", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "Long", innerException3, this);
		}
	}

	public override string ReadElementContentAsString()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsString");
		}
		XmlSchemaType xmlType;
		object obj = InternalReadElementContentAsObject(out xmlType);
		try
		{
			if (xmlType != null)
			{
				return xmlType.ValueConverter.ToString(obj);
			}
			return obj as string;
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException3, this);
		}
	}

	public override object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAs");
		}
		XmlSchemaType xmlType;
		string originalString;
		object value = InternalReadElementContentAsObject(out xmlType, unwrapTypedValue: false, out originalString);
		try
		{
			if (xmlType != null)
			{
				if (returnType == typeof(DateTimeOffset) && xmlType.Datatype is Datatype_dateTimeBase)
				{
					value = originalString;
				}
				return xmlType.ValueConverter.ChangeType(value, returnType, namespaceResolver);
			}
			return XmlUntypedConverter.Untyped.ChangeType(value, returnType, namespaceResolver);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException3, this);
		}
	}

	public override string GetAttribute(string name)
	{
		string text = coreReader.GetAttribute(name);
		if (text == null && attributeCount > 0)
		{
			ValidatingReaderNodeData defaultAttribute = GetDefaultAttribute(name, updatePosition: false);
			if (defaultAttribute != null)
			{
				text = defaultAttribute.RawValue;
			}
		}
		return text;
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		string attribute = coreReader.GetAttribute(name, namespaceURI);
		if (attribute == null && attributeCount > 0)
		{
			namespaceURI = ((namespaceURI == null) ? string.Empty : coreReaderNameTable.Get(namespaceURI));
			name = coreReaderNameTable.Get(name);
			if (name == null || namespaceURI == null)
			{
				return null;
			}
			ValidatingReaderNodeData defaultAttribute = GetDefaultAttribute(name, namespaceURI, updatePosition: false);
			if (defaultAttribute != null)
			{
				return defaultAttribute.RawValue;
			}
		}
		return attribute;
	}

	public override string GetAttribute(int i)
	{
		if (attributeCount == 0)
		{
			return null;
		}
		if (i < coreReaderAttributeCount)
		{
			return coreReader.GetAttribute(i);
		}
		int index = i - coreReaderAttributeCount;
		return ((ValidatingReaderNodeData)defaultAttributes[index]).RawValue;
	}

	public override bool MoveToAttribute(string name)
	{
		if (coreReader.MoveToAttribute(name))
		{
			validationState = ValidatingReaderState.OnAttribute;
			attributePSVI = GetAttributePSVI(name);
			goto IL_0057;
		}
		if (attributeCount > 0)
		{
			ValidatingReaderNodeData defaultAttribute = GetDefaultAttribute(name, updatePosition: true);
			if (defaultAttribute != null)
			{
				validationState = ValidatingReaderState.OnDefaultAttribute;
				attributePSVI = defaultAttribute.AttInfo;
				cachedNode = defaultAttribute;
				goto IL_0057;
			}
		}
		return false;
		IL_0057:
		if (validationState == ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper.Finish();
			validationState = savedState;
		}
		return true;
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		name = coreReaderNameTable.Get(name);
		ns = ((ns != null) ? coreReaderNameTable.Get(ns) : string.Empty);
		if (name == null || ns == null)
		{
			return false;
		}
		if (coreReader.MoveToAttribute(name, ns))
		{
			validationState = ValidatingReaderState.OnAttribute;
			if (inlineSchemaParser == null)
			{
				attributePSVI = GetAttributePSVI(name, ns);
			}
			else
			{
				attributePSVI = null;
			}
		}
		else
		{
			ValidatingReaderNodeData defaultAttribute = GetDefaultAttribute(name, ns, updatePosition: true);
			if (defaultAttribute == null)
			{
				return false;
			}
			attributePSVI = defaultAttribute.AttInfo;
			cachedNode = defaultAttribute;
			validationState = ValidatingReaderState.OnDefaultAttribute;
		}
		if (validationState == ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper.Finish();
			validationState = savedState;
		}
		return true;
	}

	public override void MoveToAttribute(int i)
	{
		if (i < 0 || i >= attributeCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		currentAttrIndex = i;
		if (i < coreReaderAttributeCount)
		{
			coreReader.MoveToAttribute(i);
			if (inlineSchemaParser == null)
			{
				attributePSVI = attributePSVINodes[i];
			}
			else
			{
				attributePSVI = null;
			}
			validationState = ValidatingReaderState.OnAttribute;
		}
		else
		{
			int index = i - coreReaderAttributeCount;
			cachedNode = (ValidatingReaderNodeData)defaultAttributes[index];
			attributePSVI = cachedNode.AttInfo;
			validationState = ValidatingReaderState.OnDefaultAttribute;
		}
		if (validationState == ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper.Finish();
			validationState = savedState;
		}
	}

	public override bool MoveToFirstAttribute()
	{
		if (coreReader.MoveToFirstAttribute())
		{
			currentAttrIndex = 0;
			if (inlineSchemaParser == null)
			{
				attributePSVI = attributePSVINodes[0];
			}
			else
			{
				attributePSVI = null;
			}
			validationState = ValidatingReaderState.OnAttribute;
		}
		else
		{
			if (defaultAttributes.Count <= 0)
			{
				return false;
			}
			cachedNode = (ValidatingReaderNodeData)defaultAttributes[0];
			attributePSVI = cachedNode.AttInfo;
			currentAttrIndex = 0;
			validationState = ValidatingReaderState.OnDefaultAttribute;
		}
		if (validationState == ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper.Finish();
			validationState = savedState;
		}
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (currentAttrIndex + 1 < coreReaderAttributeCount)
		{
			coreReader.MoveToNextAttribute();
			currentAttrIndex++;
			if (inlineSchemaParser == null)
			{
				attributePSVI = attributePSVINodes[currentAttrIndex];
			}
			else
			{
				attributePSVI = null;
			}
			validationState = ValidatingReaderState.OnAttribute;
		}
		else
		{
			if (currentAttrIndex + 1 >= attributeCount)
			{
				return false;
			}
			int index = ++currentAttrIndex - coreReaderAttributeCount;
			cachedNode = (ValidatingReaderNodeData)defaultAttributes[index];
			attributePSVI = cachedNode.AttInfo;
			validationState = ValidatingReaderState.OnDefaultAttribute;
		}
		if (validationState == ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper.Finish();
			validationState = savedState;
		}
		return true;
	}

	public override bool MoveToElement()
	{
		if (coreReader.MoveToElement() || validationState < ValidatingReaderState.None)
		{
			currentAttrIndex = -1;
			validationState = ValidatingReaderState.ClearAttributes;
			return true;
		}
		return false;
	}

	public override bool Read()
	{
		switch (validationState)
		{
		case ValidatingReaderState.Read:
			if (coreReader.Read())
			{
				ProcessReaderEvent();
				return true;
			}
			validator.EndValidation();
			if (coreReader.EOF)
			{
				validationState = ValidatingReaderState.EOF;
			}
			return false;
		case ValidatingReaderState.ParseInlineSchema:
			ProcessInlineSchema();
			return true;
		case ValidatingReaderState.OnReadAttributeValue:
		case ValidatingReaderState.OnDefaultAttribute:
		case ValidatingReaderState.OnAttribute:
		case ValidatingReaderState.ClearAttributes:
			ClearAttributesInfo();
			if (inlineSchemaParser != null)
			{
				validationState = ValidatingReaderState.ParseInlineSchema;
				goto case ValidatingReaderState.ParseInlineSchema;
			}
			validationState = ValidatingReaderState.Read;
			goto case ValidatingReaderState.Read;
		case ValidatingReaderState.ReadAhead:
			ClearAttributesInfo();
			ProcessReaderEvent();
			validationState = ValidatingReaderState.Read;
			return true;
		case ValidatingReaderState.OnReadBinaryContent:
			validationState = savedState;
			readBinaryHelper.Finish();
			return Read();
		case ValidatingReaderState.Init:
			validationState = ValidatingReaderState.Read;
			if (coreReader.ReadState == ReadState.Interactive)
			{
				ProcessReaderEvent();
				return true;
			}
			goto case ValidatingReaderState.Read;
		case ValidatingReaderState.ReaderClosed:
		case ValidatingReaderState.EOF:
			return false;
		default:
			return false;
		}
	}

	public override void Close()
	{
		coreReader.Close();
		validationState = ValidatingReaderState.ReaderClosed;
	}

	public override void Skip()
	{
		_ = Depth;
		XmlNodeType nodeType = NodeType;
		if (nodeType != XmlNodeType.Element)
		{
			if (nodeType != XmlNodeType.Attribute)
			{
				goto IL_0081;
			}
			MoveToElement();
		}
		if (!coreReader.IsEmptyElement)
		{
			bool flag = true;
			if ((xmlSchemaInfo.IsUnionType || xmlSchemaInfo.IsDefault) && coreReader is XsdCachingReader)
			{
				flag = false;
			}
			coreReader.Skip();
			validationState = ValidatingReaderState.ReadAhead;
			if (flag)
			{
				validator.SkipToEndElement(xmlSchemaInfo);
			}
		}
		goto IL_0081;
		IL_0081:
		Read();
	}

	public override string LookupNamespace(string prefix)
	{
		return thisNSResolver.LookupNamespace(prefix);
	}

	public override void ResolveEntity()
	{
		throw new InvalidOperationException();
	}

	public override bool ReadAttributeValue()
	{
		if (validationState == ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper.Finish();
			validationState = savedState;
		}
		if (NodeType == XmlNodeType.Attribute)
		{
			if (validationState == ValidatingReaderState.OnDefaultAttribute)
			{
				cachedNode = CreateDummyTextNode(cachedNode.RawValue, cachedNode.Depth + 1);
				validationState = ValidatingReaderState.OnReadAttributeValue;
				return true;
			}
			return coreReader.ReadAttributeValue();
		}
		return false;
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = readBinaryHelper.ReadContentAsBase64(buffer, index, count);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public bool HasLineInfo()
	{
		return true;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		if (coreReaderNSResolver != null)
		{
			return coreReaderNSResolver.GetNamespacesInScope(scope);
		}
		return nsManager.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		if (coreReaderNSResolver != null)
		{
			return coreReaderNSResolver.LookupNamespace(prefix);
		}
		return nsManager.LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		if (coreReaderNSResolver != null)
		{
			return coreReaderNSResolver.LookupPrefix(namespaceName);
		}
		return nsManager.LookupPrefix(namespaceName);
	}

	private object GetStringValue()
	{
		return coreReader.Value;
	}

	private void ProcessReaderEvent()
	{
		if (!replayCache)
		{
			switch (coreReader.NodeType)
			{
			case XmlNodeType.Element:
				ProcessElementEvent();
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				validator.ValidateWhitespace(GetStringValue);
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				validator.ValidateText(GetStringValue);
				break;
			case XmlNodeType.EndElement:
				ProcessEndElementEvent();
				break;
			case XmlNodeType.EntityReference:
				throw new InvalidOperationException();
			case XmlNodeType.DocumentType:
				validator.SetDtdSchemaInfo(coreReader.DtdInfo);
				break;
			case XmlNodeType.Attribute:
			case XmlNodeType.Entity:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				break;
			}
		}
	}

	private void ProcessElementEvent()
	{
		if (processInlineSchema && IsXSDRoot(coreReader.LocalName, coreReader.NamespaceURI) && coreReader.Depth > 0)
		{
			xmlSchemaInfo.Clear();
			attributeCount = (coreReaderAttributeCount = coreReader.AttributeCount);
			if (!coreReader.IsEmptyElement)
			{
				inlineSchemaParser = new Parser(SchemaType.XSD, coreReaderNameTable, validator.SchemaSet.GetSchemaNames(coreReaderNameTable), validationEvent);
				inlineSchemaParser.StartParsing(coreReader, null);
				inlineSchemaParser.ParseReaderNode();
				validationState = ValidatingReaderState.ParseInlineSchema;
			}
			else
			{
				validationState = ValidatingReaderState.ClearAttributes;
			}
			return;
		}
		atomicValue = null;
		originalAtomicValueString = null;
		xmlSchemaInfo.Clear();
		if (manageNamespaces)
		{
			nsManager.PushScope();
		}
		string xsiSchemaLocation = null;
		string xsiNoNamespaceSchemaLocation = null;
		string xsiNil = null;
		string xsiType = null;
		if (coreReader.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = coreReader.NamespaceURI;
				string localName = coreReader.LocalName;
				if (Ref.Equal(namespaceURI, NsXsi))
				{
					if (Ref.Equal(localName, XsiSchemaLocation))
					{
						xsiSchemaLocation = coreReader.Value;
					}
					else if (Ref.Equal(localName, XsiNoNamespaceSchemaLocation))
					{
						xsiNoNamespaceSchemaLocation = coreReader.Value;
					}
					else if (Ref.Equal(localName, XsiType))
					{
						xsiType = coreReader.Value;
					}
					else if (Ref.Equal(localName, XsiNil))
					{
						xsiNil = coreReader.Value;
					}
				}
				if (manageNamespaces && Ref.Equal(coreReader.NamespaceURI, NsXmlNs))
				{
					nsManager.AddNamespace((coreReader.Prefix.Length == 0) ? string.Empty : coreReader.LocalName, coreReader.Value);
				}
			}
			while (coreReader.MoveToNextAttribute());
			coreReader.MoveToElement();
		}
		validator.ValidateElement(coreReader.LocalName, coreReader.NamespaceURI, xmlSchemaInfo, xsiType, xsiNil, xsiSchemaLocation, xsiNoNamespaceSchemaLocation);
		ValidateAttributes();
		validator.ValidateEndOfAttributes(xmlSchemaInfo);
		if (coreReader.IsEmptyElement)
		{
			ProcessEndElementEvent();
		}
		validationState = ValidatingReaderState.ClearAttributes;
	}

	private void ProcessEndElementEvent()
	{
		atomicValue = validator.ValidateEndElement(xmlSchemaInfo);
		originalAtomicValueString = GetOriginalAtomicValueStringOfElement();
		if (xmlSchemaInfo.IsDefault)
		{
			int depth = coreReader.Depth;
			coreReader = GetCachingReader();
			cachingReader.RecordTextNode(xmlSchemaInfo.XmlType.ValueConverter.ToString(atomicValue), originalAtomicValueString, depth + 1, 0, 0);
			cachingReader.RecordEndElementNode();
			cachingReader.SetToReplayMode();
			replayCache = true;
		}
		else if (manageNamespaces)
		{
			nsManager.PopScope();
		}
	}

	private void ValidateAttributes()
	{
		attributeCount = (coreReaderAttributeCount = coreReader.AttributeCount);
		int num = 0;
		bool flag = false;
		if (coreReader.MoveToFirstAttribute())
		{
			do
			{
				string localName = coreReader.LocalName;
				string namespaceURI = coreReader.NamespaceURI;
				AttributePSVIInfo attributePSVIInfo = AddAttributePSVI(num);
				attributePSVIInfo.localName = localName;
				attributePSVIInfo.namespaceUri = namespaceURI;
				if ((object)namespaceURI == NsXmlNs)
				{
					num++;
					continue;
				}
				attributePSVIInfo.typedAttributeValue = validator.ValidateAttribute(localName, namespaceURI, valueGetter, attributePSVIInfo.attributeSchemaInfo);
				if (!flag)
				{
					flag = attributePSVIInfo.attributeSchemaInfo.Validity == XmlSchemaValidity.Invalid;
				}
				num++;
			}
			while (coreReader.MoveToNextAttribute());
		}
		coreReader.MoveToElement();
		if (flag)
		{
			xmlSchemaInfo.Validity = XmlSchemaValidity.Invalid;
		}
		validator.GetUnspecifiedDefaultAttributes(defaultAttributes, createNodeData: true);
		attributeCount += defaultAttributes.Count;
	}

	private void ClearAttributesInfo()
	{
		attributeCount = 0;
		coreReaderAttributeCount = 0;
		currentAttrIndex = -1;
		defaultAttributes.Clear();
		attributePSVI = null;
	}

	private AttributePSVIInfo GetAttributePSVI(string name)
	{
		if (inlineSchemaParser != null)
		{
			return null;
		}
		ValidateNames.SplitQName(name, out var prefix, out var lname);
		prefix = coreReaderNameTable.Add(prefix);
		lname = coreReaderNameTable.Add(lname);
		string ns = ((prefix.Length != 0) ? thisNSResolver.LookupNamespace(prefix) : string.Empty);
		return GetAttributePSVI(lname, ns);
	}

	private AttributePSVIInfo GetAttributePSVI(string localName, string ns)
	{
		AttributePSVIInfo attributePSVIInfo = null;
		for (int i = 0; i < coreReaderAttributeCount; i++)
		{
			attributePSVIInfo = attributePSVINodes[i];
			if (attributePSVIInfo != null && Ref.Equal(localName, attributePSVIInfo.localName) && Ref.Equal(ns, attributePSVIInfo.namespaceUri))
			{
				currentAttrIndex = i;
				return attributePSVIInfo;
			}
		}
		return null;
	}

	private ValidatingReaderNodeData GetDefaultAttribute(string name, bool updatePosition)
	{
		ValidateNames.SplitQName(name, out var prefix, out var lname);
		prefix = coreReaderNameTable.Add(prefix);
		lname = coreReaderNameTable.Add(lname);
		string ns = ((prefix.Length != 0) ? thisNSResolver.LookupNamespace(prefix) : string.Empty);
		return GetDefaultAttribute(lname, ns, updatePosition);
	}

	private ValidatingReaderNodeData GetDefaultAttribute(string attrLocalName, string ns, bool updatePosition)
	{
		ValidatingReaderNodeData validatingReaderNodeData = null;
		for (int i = 0; i < defaultAttributes.Count; i++)
		{
			validatingReaderNodeData = (ValidatingReaderNodeData)defaultAttributes[i];
			if (Ref.Equal(validatingReaderNodeData.LocalName, attrLocalName) && Ref.Equal(validatingReaderNodeData.Namespace, ns))
			{
				if (updatePosition)
				{
					currentAttrIndex = coreReader.AttributeCount + i;
				}
				return validatingReaderNodeData;
			}
		}
		return null;
	}

	private AttributePSVIInfo AddAttributePSVI(int attIndex)
	{
		AttributePSVIInfo attributePSVIInfo = attributePSVINodes[attIndex];
		if (attributePSVIInfo != null)
		{
			attributePSVIInfo.Reset();
			return attributePSVIInfo;
		}
		if (attIndex >= attributePSVINodes.Length - 1)
		{
			AttributePSVIInfo[] destinationArray = new AttributePSVIInfo[attributePSVINodes.Length * 2];
			Array.Copy(attributePSVINodes, 0, destinationArray, 0, attributePSVINodes.Length);
			attributePSVINodes = destinationArray;
		}
		attributePSVIInfo = attributePSVINodes[attIndex];
		if (attributePSVIInfo == null)
		{
			attributePSVIInfo = new AttributePSVIInfo();
			attributePSVINodes[attIndex] = attributePSVIInfo;
		}
		return attributePSVIInfo;
	}

	private bool IsXSDRoot(string localName, string ns)
	{
		if (Ref.Equal(ns, NsXs))
		{
			return Ref.Equal(localName, XsdSchema);
		}
		return false;
	}

	private void ProcessInlineSchema()
	{
		if (coreReader.Read())
		{
			if (coreReader.NodeType == XmlNodeType.Element)
			{
				attributeCount = (coreReaderAttributeCount = coreReader.AttributeCount);
			}
			else
			{
				ClearAttributesInfo();
			}
			if (!inlineSchemaParser.ParseReaderNode())
			{
				inlineSchemaParser.FinishParsing();
				XmlSchema xmlSchema = inlineSchemaParser.XmlSchema;
				validator.AddSchema(xmlSchema);
				inlineSchemaParser = null;
				validationState = ValidatingReaderState.Read;
			}
		}
	}

	private object InternalReadContentAsObject()
	{
		return InternalReadContentAsObject(unwrapTypedValue: false);
	}

	private object InternalReadContentAsObject(bool unwrapTypedValue)
	{
		string originalStringValue;
		return InternalReadContentAsObject(unwrapTypedValue, out originalStringValue);
	}

	private object InternalReadContentAsObject(bool unwrapTypedValue, out string originalStringValue)
	{
		switch (NodeType)
		{
		case XmlNodeType.Attribute:
			originalStringValue = Value;
			if (attributePSVI != null && attributePSVI.typedAttributeValue != null)
			{
				if (validationState == ValidatingReaderState.OnDefaultAttribute)
				{
					XmlSchemaAttribute schemaAttribute = attributePSVI.attributeSchemaInfo.SchemaAttribute;
					originalStringValue = ((schemaAttribute.DefaultValue != null) ? schemaAttribute.DefaultValue : schemaAttribute.FixedValue);
				}
				return ReturnBoxedValue(attributePSVI.typedAttributeValue, AttributeSchemaInfo.XmlType, unwrapTypedValue);
			}
			return Value;
		case XmlNodeType.EndElement:
			if (atomicValue != null)
			{
				originalStringValue = originalAtomicValueString;
				return atomicValue;
			}
			originalStringValue = string.Empty;
			return string.Empty;
		default:
			if (validator.CurrentContentType == XmlSchemaContentType.TextOnly)
			{
				object result = ReturnBoxedValue(ReadTillEndElement(), xmlSchemaInfo.XmlType, unwrapTypedValue);
				originalStringValue = originalAtomicValueString;
				return result;
			}
			if (coreReader is XsdCachingReader xsdCachingReader)
			{
				originalStringValue = xsdCachingReader.ReadOriginalContentAsString();
			}
			else
			{
				originalStringValue = InternalReadContentAsString();
			}
			return originalStringValue;
		}
	}

	private object InternalReadElementContentAsObject(out XmlSchemaType xmlType)
	{
		return InternalReadElementContentAsObject(out xmlType, unwrapTypedValue: false);
	}

	private object InternalReadElementContentAsObject(out XmlSchemaType xmlType, bool unwrapTypedValue)
	{
		string originalString;
		return InternalReadElementContentAsObject(out xmlType, unwrapTypedValue, out originalString);
	}

	private object InternalReadElementContentAsObject(out XmlSchemaType xmlType, bool unwrapTypedValue, out string originalString)
	{
		object obj = null;
		xmlType = null;
		if (IsEmptyElement)
		{
			obj = ((xmlSchemaInfo.ContentType != XmlSchemaContentType.TextOnly) ? atomicValue : ReturnBoxedValue(atomicValue, xmlSchemaInfo.XmlType, unwrapTypedValue));
			originalString = originalAtomicValueString;
			xmlType = ElementXmlType;
			Read();
			return obj;
		}
		Read();
		if (NodeType == XmlNodeType.EndElement)
		{
			if (xmlSchemaInfo.IsDefault)
			{
				obj = ((xmlSchemaInfo.ContentType != XmlSchemaContentType.TextOnly) ? atomicValue : ReturnBoxedValue(atomicValue, xmlSchemaInfo.XmlType, unwrapTypedValue));
				originalString = originalAtomicValueString;
			}
			else
			{
				obj = string.Empty;
				originalString = string.Empty;
			}
		}
		else
		{
			if (NodeType == XmlNodeType.Element)
			{
				throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
			}
			obj = InternalReadContentAsObject(unwrapTypedValue, out originalString);
			if (NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
			}
		}
		xmlType = ElementXmlType;
		Read();
		return obj;
	}

	private object ReadTillEndElement()
	{
		if (atomicValue == null)
		{
			while (coreReader.Read())
			{
				if (replayCache)
				{
					continue;
				}
				switch (coreReader.NodeType)
				{
				case XmlNodeType.Element:
					ProcessReaderEvent();
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					validator.ValidateText(GetStringValue);
					continue;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					validator.ValidateWhitespace(GetStringValue);
					continue;
				case XmlNodeType.EndElement:
					atomicValue = validator.ValidateEndElement(xmlSchemaInfo);
					originalAtomicValueString = GetOriginalAtomicValueStringOfElement();
					if (manageNamespaces)
					{
						nsManager.PopScope();
					}
					break;
				default:
					continue;
				}
				break;
			}
		}
		else
		{
			if (atomicValue == this)
			{
				atomicValue = null;
			}
			SwitchReader();
		}
		return atomicValue;
	}

	private void SwitchReader()
	{
		if (coreReader is XsdCachingReader xsdCachingReader)
		{
			coreReader = xsdCachingReader.GetCoreReader();
		}
		replayCache = false;
	}

	private void ReadAheadForMemberType()
	{
		while (coreReader.Read())
		{
			switch (coreReader.NodeType)
			{
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				validator.ValidateText(GetStringValue);
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				validator.ValidateWhitespace(GetStringValue);
				break;
			case XmlNodeType.EndElement:
				atomicValue = validator.ValidateEndElement(xmlSchemaInfo);
				originalAtomicValueString = GetOriginalAtomicValueStringOfElement();
				if (atomicValue == null)
				{
					atomicValue = this;
				}
				else if (xmlSchemaInfo.IsDefault)
				{
					cachingReader.SwitchTextNodeAndEndElement(xmlSchemaInfo.XmlType.ValueConverter.ToString(atomicValue), originalAtomicValueString);
				}
				return;
			}
		}
	}

	private void GetIsDefault()
	{
		if (coreReader is XsdCachingReader || !xmlSchemaInfo.HasDefaultValue)
		{
			return;
		}
		coreReader = GetCachingReader();
		if (xmlSchemaInfo.IsUnionType && !xmlSchemaInfo.IsNil)
		{
			ReadAheadForMemberType();
		}
		else if (coreReader.Read())
		{
			switch (coreReader.NodeType)
			{
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				validator.ValidateText(GetStringValue);
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				validator.ValidateWhitespace(GetStringValue);
				break;
			case XmlNodeType.EndElement:
				atomicValue = validator.ValidateEndElement(xmlSchemaInfo);
				originalAtomicValueString = GetOriginalAtomicValueStringOfElement();
				if (xmlSchemaInfo.IsDefault)
				{
					cachingReader.SwitchTextNodeAndEndElement(xmlSchemaInfo.XmlType.ValueConverter.ToString(atomicValue), originalAtomicValueString);
				}
				break;
			}
		}
		cachingReader.SetToReplayMode();
		replayCache = true;
	}

	private void GetMemberType()
	{
		if (xmlSchemaInfo.MemberType == null && atomicValue != this && !(coreReader is XsdCachingReader) && xmlSchemaInfo.IsUnionType && !xmlSchemaInfo.IsNil)
		{
			coreReader = GetCachingReader();
			ReadAheadForMemberType();
			cachingReader.SetToReplayMode();
			replayCache = true;
		}
	}

	private object ReturnBoxedValue(object typedValue, XmlSchemaType xmlType, bool unWrap)
	{
		if (typedValue != null)
		{
			if (unWrap && xmlType.Datatype.Variety == XmlSchemaDatatypeVariety.List && (xmlType.Datatype as Datatype_List).ItemType.Variety == XmlSchemaDatatypeVariety.Union)
			{
				typedValue = xmlType.ValueConverter.ChangeType(typedValue, xmlType.Datatype.ValueType, thisNSResolver);
			}
			return typedValue;
		}
		typedValue = validator.GetConcatenatedValue();
		return typedValue;
	}

	private XsdCachingReader GetCachingReader()
	{
		if (cachingReader == null)
		{
			cachingReader = new XsdCachingReader(coreReader, lineInfo, CachingCallBack);
		}
		else
		{
			cachingReader.Reset(coreReader);
		}
		lineInfo = cachingReader;
		return cachingReader;
	}

	internal ValidatingReaderNodeData CreateDummyTextNode(string attributeValue, int depth)
	{
		if (textNode == null)
		{
			textNode = new ValidatingReaderNodeData(XmlNodeType.Text);
		}
		textNode.Depth = depth;
		textNode.RawValue = attributeValue;
		return textNode;
	}

	internal void CachingCallBack(XsdCachingReader cachingReader)
	{
		coreReader = cachingReader.GetCoreReader();
		lineInfo = cachingReader.GetLineInfo();
		replayCache = false;
	}

	private string GetOriginalAtomicValueStringOfElement()
	{
		if (xmlSchemaInfo.IsDefault)
		{
			XmlSchemaElement schemaElement = xmlSchemaInfo.SchemaElement;
			if (schemaElement != null)
			{
				if (schemaElement.DefaultValue == null)
				{
					return schemaElement.FixedValue;
				}
				return schemaElement.DefaultValue;
			}
			return string.Empty;
		}
		return validator.GetConcatenatedValue();
	}

	public override Task<string> GetValueAsync()
	{
		if (validationState < ValidatingReaderState.None)
		{
			return Task.FromResult(cachedNode.RawValue);
		}
		return coreReader.GetValueAsync();
	}

	public override Task<object> ReadContentAsObjectAsync()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsObject");
		}
		return InternalReadContentAsObjectAsync(unwrapTypedValue: true);
	}

	public override async Task<string> ReadContentAsStringAsync()
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAsString");
		}
		object obj = await InternalReadContentAsObjectAsync().ConfigureAwait(continueOnCapturedContext: false);
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			if (xmlSchemaType != null)
			{
				return xmlSchemaType.ValueConverter.ToString(obj);
			}
			return obj as string;
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException3, this);
		}
	}

	public override async Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		if (!XmlReader.CanReadContentAs(NodeType))
		{
			throw CreateReadContentAsException("ReadContentAs");
		}
		Tuple<string, object> obj = await InternalReadContentAsObjectTupleAsync(unwrapTypedValue: false).ConfigureAwait(continueOnCapturedContext: false);
		string item = obj.Item1;
		object value = obj.Item2;
		XmlSchemaType xmlSchemaType = ((NodeType == XmlNodeType.Attribute) ? AttributeXmlType : ElementXmlType);
		try
		{
			if (xmlSchemaType != null)
			{
				if (returnType == typeof(DateTimeOffset) && xmlSchemaType.Datatype is Datatype_dateTimeBase)
				{
					value = item;
				}
				return xmlSchemaType.ValueConverter.ChangeType(value, returnType);
			}
			return XmlUntypedConverter.Untyped.ChangeType(value, returnType, namespaceResolver);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException3, this);
		}
	}

	public override async Task<object> ReadElementContentAsObjectAsync()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsObject");
		}
		return (await InternalReadElementContentAsObjectAsync(unwrapTypedValue: true).ConfigureAwait(continueOnCapturedContext: false)).Item2;
	}

	public override async Task<string> ReadElementContentAsStringAsync()
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAsString");
		}
		Tuple<XmlSchemaType, object> obj = await InternalReadElementContentAsObjectAsync().ConfigureAwait(continueOnCapturedContext: false);
		XmlSchemaType item = obj.Item1;
		object item2 = obj.Item2;
		try
		{
			if (item != null)
			{
				return item.ValueConverter.ToString(item2);
			}
			return item2 as string;
		}
		catch (InvalidCastException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException, this);
		}
		catch (FormatException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", "String", innerException3, this);
		}
	}

	public override async Task<object> ReadElementContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw CreateReadElementContentAsException("ReadElementContentAs");
		}
		Tuple<XmlSchemaType, string, object> obj = await InternalReadElementContentAsObjectTupleAsync(unwrapTypedValue: false).ConfigureAwait(continueOnCapturedContext: false);
		XmlSchemaType item = obj.Item1;
		string item2 = obj.Item2;
		object value = obj.Item3;
		try
		{
			if (item != null)
			{
				if (returnType == typeof(DateTimeOffset) && item.Datatype is Datatype_dateTimeBase)
				{
					value = item2;
				}
				return item.ValueConverter.ChangeType(value, returnType, namespaceResolver);
			}
			return XmlUntypedConverter.Untyped.ChangeType(value, returnType, namespaceResolver);
		}
		catch (FormatException innerException)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException, this);
		}
		catch (InvalidCastException innerException2)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException2, this);
		}
		catch (OverflowException innerException3)
		{
			throw new XmlException("Content cannot be converted to the type {0}.", returnType.ToString(), innerException3, this);
		}
	}

	private Task<bool> ReadAsync_Read(Task<bool> task)
	{
		if (task.IsSuccess())
		{
			if (task.Result)
			{
				return ProcessReaderEventAsync().ReturnTaskBoolWhenFinish(ret: true);
			}
			validator.EndValidation();
			if (coreReader.EOF)
			{
				validationState = ValidatingReaderState.EOF;
			}
			return AsyncHelper.DoneTaskFalse;
		}
		return _ReadAsync_Read(task);
	}

	private async Task<bool> _ReadAsync_Read(Task<bool> task)
	{
		if (await task.ConfigureAwait(continueOnCapturedContext: false))
		{
			await ProcessReaderEventAsync().ConfigureAwait(continueOnCapturedContext: false);
			return true;
		}
		validator.EndValidation();
		if (coreReader.EOF)
		{
			validationState = ValidatingReaderState.EOF;
		}
		return false;
	}

	private Task<bool> ReadAsync_ReadAhead(Task task)
	{
		if (task.IsSuccess())
		{
			validationState = ValidatingReaderState.Read;
			return AsyncHelper.DoneTaskTrue;
		}
		return _ReadAsync_ReadAhead(task);
	}

	private async Task<bool> _ReadAsync_ReadAhead(Task task)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		validationState = ValidatingReaderState.Read;
		return true;
	}

	public override Task<bool> ReadAsync()
	{
		switch (validationState)
		{
		case ValidatingReaderState.Read:
		{
			Task<bool> task = coreReader.ReadAsync();
			return ReadAsync_Read(task);
		}
		case ValidatingReaderState.ParseInlineSchema:
			return ProcessInlineSchemaAsync().ReturnTaskBoolWhenFinish(ret: true);
		case ValidatingReaderState.OnReadAttributeValue:
		case ValidatingReaderState.OnDefaultAttribute:
		case ValidatingReaderState.OnAttribute:
		case ValidatingReaderState.ClearAttributes:
			ClearAttributesInfo();
			if (inlineSchemaParser != null)
			{
				validationState = ValidatingReaderState.ParseInlineSchema;
				goto case ValidatingReaderState.ParseInlineSchema;
			}
			validationState = ValidatingReaderState.Read;
			goto case ValidatingReaderState.Read;
		case ValidatingReaderState.ReadAhead:
		{
			ClearAttributesInfo();
			Task task2 = ProcessReaderEventAsync();
			return ReadAsync_ReadAhead(task2);
		}
		case ValidatingReaderState.OnReadBinaryContent:
			validationState = savedState;
			return readBinaryHelper.FinishAsync().CallBoolTaskFuncWhenFinish(ReadAsync);
		case ValidatingReaderState.Init:
			validationState = ValidatingReaderState.Read;
			if (coreReader.ReadState == ReadState.Interactive)
			{
				return ProcessReaderEventAsync().ReturnTaskBoolWhenFinish(ret: true);
			}
			goto case ValidatingReaderState.Read;
		case ValidatingReaderState.ReaderClosed:
		case ValidatingReaderState.EOF:
			return AsyncHelper.DoneTaskFalse;
		default:
			return AsyncHelper.DoneTaskFalse;
		}
	}

	public override async Task SkipAsync()
	{
		_ = Depth;
		XmlNodeType nodeType = NodeType;
		if (nodeType != XmlNodeType.Element)
		{
			if (nodeType != XmlNodeType.Attribute)
			{
				goto IL_0116;
			}
			MoveToElement();
		}
		if (!coreReader.IsEmptyElement)
		{
			bool callSkipToEndElem = true;
			if ((xmlSchemaInfo.IsUnionType || xmlSchemaInfo.IsDefault) && coreReader is XsdCachingReader)
			{
				callSkipToEndElem = false;
			}
			await coreReader.SkipAsync().ConfigureAwait(continueOnCapturedContext: false);
			validationState = ValidatingReaderState.ReadAhead;
			if (callSkipToEndElem)
			{
				validator.SkipToEndElement(xmlSchemaInfo);
			}
		}
		goto IL_0116;
		IL_0116:
		await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = await readBinaryHelper.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = await readBinaryHelper.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = await readBinaryHelper.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (validationState != ValidatingReaderState.OnReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, this);
			savedState = validationState;
		}
		validationState = savedState;
		int result = await readBinaryHelper.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		savedState = validationState;
		validationState = ValidatingReaderState.OnReadBinaryContent;
		return result;
	}

	private Task ProcessReaderEventAsync()
	{
		if (replayCache)
		{
			return AsyncHelper.DoneTask;
		}
		switch (coreReader.NodeType)
		{
		case XmlNodeType.Element:
			return ProcessElementEventAsync();
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			validator.ValidateWhitespace(GetStringValue);
			break;
		case XmlNodeType.Text:
		case XmlNodeType.CDATA:
			validator.ValidateText(GetStringValue);
			break;
		case XmlNodeType.EndElement:
			return ProcessEndElementEventAsync();
		case XmlNodeType.EntityReference:
			throw new InvalidOperationException();
		case XmlNodeType.DocumentType:
			validator.SetDtdSchemaInfo(coreReader.DtdInfo);
			break;
		}
		return AsyncHelper.DoneTask;
	}

	private async Task ProcessElementEventAsync()
	{
		if (processInlineSchema && IsXSDRoot(coreReader.LocalName, coreReader.NamespaceURI) && coreReader.Depth > 0)
		{
			xmlSchemaInfo.Clear();
			attributeCount = (coreReaderAttributeCount = coreReader.AttributeCount);
			if (!coreReader.IsEmptyElement)
			{
				inlineSchemaParser = new Parser(SchemaType.XSD, coreReaderNameTable, validator.SchemaSet.GetSchemaNames(coreReaderNameTable), validationEvent);
				await inlineSchemaParser.StartParsingAsync(coreReader, null).ConfigureAwait(continueOnCapturedContext: false);
				inlineSchemaParser.ParseReaderNode();
				validationState = ValidatingReaderState.ParseInlineSchema;
			}
			else
			{
				validationState = ValidatingReaderState.ClearAttributes;
			}
			return;
		}
		atomicValue = null;
		originalAtomicValueString = null;
		xmlSchemaInfo.Clear();
		if (manageNamespaces)
		{
			nsManager.PushScope();
		}
		string xsiSchemaLocation = null;
		string xsiNoNamespaceSchemaLocation = null;
		string xsiNil = null;
		string xsiType = null;
		if (coreReader.MoveToFirstAttribute())
		{
			do
			{
				string namespaceURI = coreReader.NamespaceURI;
				string localName = coreReader.LocalName;
				if (Ref.Equal(namespaceURI, NsXsi))
				{
					if (Ref.Equal(localName, XsiSchemaLocation))
					{
						xsiSchemaLocation = coreReader.Value;
					}
					else if (Ref.Equal(localName, XsiNoNamespaceSchemaLocation))
					{
						xsiNoNamespaceSchemaLocation = coreReader.Value;
					}
					else if (Ref.Equal(localName, XsiType))
					{
						xsiType = coreReader.Value;
					}
					else if (Ref.Equal(localName, XsiNil))
					{
						xsiNil = coreReader.Value;
					}
				}
				if (manageNamespaces && Ref.Equal(coreReader.NamespaceURI, NsXmlNs))
				{
					nsManager.AddNamespace((coreReader.Prefix.Length == 0) ? string.Empty : coreReader.LocalName, coreReader.Value);
				}
			}
			while (coreReader.MoveToNextAttribute());
			coreReader.MoveToElement();
		}
		validator.ValidateElement(coreReader.LocalName, coreReader.NamespaceURI, xmlSchemaInfo, xsiType, xsiNil, xsiSchemaLocation, xsiNoNamespaceSchemaLocation);
		ValidateAttributes();
		validator.ValidateEndOfAttributes(xmlSchemaInfo);
		if (coreReader.IsEmptyElement)
		{
			await ProcessEndElementEventAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		validationState = ValidatingReaderState.ClearAttributes;
	}

	private async Task ProcessEndElementEventAsync()
	{
		atomicValue = validator.ValidateEndElement(xmlSchemaInfo);
		originalAtomicValueString = GetOriginalAtomicValueStringOfElement();
		if (xmlSchemaInfo.IsDefault)
		{
			int depth = coreReader.Depth;
			coreReader = GetCachingReader();
			cachingReader.RecordTextNode(xmlSchemaInfo.XmlType.ValueConverter.ToString(atomicValue), originalAtomicValueString, depth + 1, 0, 0);
			cachingReader.RecordEndElementNode();
			await cachingReader.SetToReplayModeAsync().ConfigureAwait(continueOnCapturedContext: false);
			replayCache = true;
		}
		else if (manageNamespaces)
		{
			nsManager.PopScope();
		}
	}

	private async Task ProcessInlineSchemaAsync()
	{
		if (await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
		{
			if (coreReader.NodeType == XmlNodeType.Element)
			{
				attributeCount = (coreReaderAttributeCount = coreReader.AttributeCount);
			}
			else
			{
				ClearAttributesInfo();
			}
			if (!inlineSchemaParser.ParseReaderNode())
			{
				inlineSchemaParser.FinishParsing();
				XmlSchema xmlSchema = inlineSchemaParser.XmlSchema;
				validator.AddSchema(xmlSchema);
				inlineSchemaParser = null;
				validationState = ValidatingReaderState.Read;
			}
		}
	}

	private Task<object> InternalReadContentAsObjectAsync()
	{
		return InternalReadContentAsObjectAsync(unwrapTypedValue: false);
	}

	private async Task<object> InternalReadContentAsObjectAsync(bool unwrapTypedValue)
	{
		return (await InternalReadContentAsObjectTupleAsync(unwrapTypedValue).ConfigureAwait(continueOnCapturedContext: false)).Item2;
	}

	private async Task<Tuple<string, object>> InternalReadContentAsObjectTupleAsync(bool unwrapTypedValue)
	{
		switch (NodeType)
		{
		case XmlNodeType.Attribute:
		{
			string item2 = Value;
			if (attributePSVI != null && attributePSVI.typedAttributeValue != null)
			{
				if (validationState == ValidatingReaderState.OnDefaultAttribute)
				{
					XmlSchemaAttribute schemaAttribute = attributePSVI.attributeSchemaInfo.SchemaAttribute;
					item2 = ((schemaAttribute.DefaultValue != null) ? schemaAttribute.DefaultValue : schemaAttribute.FixedValue);
				}
				return new Tuple<string, object>(item2, ReturnBoxedValue(attributePSVI.typedAttributeValue, AttributeSchemaInfo.XmlType, unwrapTypedValue));
			}
			return new Tuple<string, object>(item2, Value);
		}
		case XmlNodeType.EndElement:
		{
			string item2;
			if (atomicValue != null)
			{
				item2 = originalAtomicValueString;
				return new Tuple<string, object>(item2, atomicValue);
			}
			item2 = string.Empty;
			return new Tuple<string, object>(item2, string.Empty);
		}
		default:
		{
			string item2;
			if (validator.CurrentContentType == XmlSchemaContentType.TextOnly)
			{
				object item = ReturnBoxedValue(await ReadTillEndElementAsync().ConfigureAwait(continueOnCapturedContext: false), xmlSchemaInfo.XmlType, unwrapTypedValue);
				item2 = originalAtomicValueString;
				return new Tuple<string, object>(item2, item);
			}
			item2 = ((!(coreReader is XsdCachingReader xsdCachingReader)) ? (await InternalReadContentAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : xsdCachingReader.ReadOriginalContentAsString());
			return new Tuple<string, object>(item2, item2);
		}
		}
	}

	private Task<Tuple<XmlSchemaType, object>> InternalReadElementContentAsObjectAsync()
	{
		return InternalReadElementContentAsObjectAsync(unwrapTypedValue: false);
	}

	private async Task<Tuple<XmlSchemaType, object>> InternalReadElementContentAsObjectAsync(bool unwrapTypedValue)
	{
		Tuple<XmlSchemaType, string, object> tuple = await InternalReadElementContentAsObjectTupleAsync(unwrapTypedValue).ConfigureAwait(continueOnCapturedContext: false);
		return new Tuple<XmlSchemaType, object>(tuple.Item1, tuple.Item3);
	}

	private async Task<Tuple<XmlSchemaType, string, object>> InternalReadElementContentAsObjectTupleAsync(bool unwrapTypedValue)
	{
		object typedValue;
		string originalString;
		XmlSchemaType xmlType;
		if (IsEmptyElement)
		{
			typedValue = ((xmlSchemaInfo.ContentType != XmlSchemaContentType.TextOnly) ? atomicValue : ReturnBoxedValue(atomicValue, xmlSchemaInfo.XmlType, unwrapTypedValue));
			originalString = originalAtomicValueString;
			xmlType = ElementXmlType;
			await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			return new Tuple<XmlSchemaType, string, object>(xmlType, originalString, typedValue);
		}
		await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (NodeType == XmlNodeType.EndElement)
		{
			if (xmlSchemaInfo.IsDefault)
			{
				typedValue = ((xmlSchemaInfo.ContentType != XmlSchemaContentType.TextOnly) ? atomicValue : ReturnBoxedValue(atomicValue, xmlSchemaInfo.XmlType, unwrapTypedValue));
				originalString = originalAtomicValueString;
			}
			else
			{
				typedValue = string.Empty;
				originalString = string.Empty;
			}
		}
		else
		{
			if (NodeType == XmlNodeType.Element)
			{
				throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
			}
			Tuple<string, object> tuple = await InternalReadContentAsObjectTupleAsync(unwrapTypedValue).ConfigureAwait(continueOnCapturedContext: false);
			originalString = tuple.Item1;
			typedValue = tuple.Item2;
			if (NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("ReadElementContentAs() methods cannot be called on an element that has child elements.", string.Empty, this);
			}
		}
		xmlType = ElementXmlType;
		await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		return new Tuple<XmlSchemaType, string, object>(xmlType, originalString, typedValue);
	}

	private async Task<object> ReadTillEndElementAsync()
	{
		if (atomicValue == null)
		{
			while (await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				if (replayCache)
				{
					continue;
				}
				switch (coreReader.NodeType)
				{
				case XmlNodeType.Element:
					await ProcessReaderEventAsync().ConfigureAwait(continueOnCapturedContext: false);
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					validator.ValidateText(GetStringValue);
					continue;
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					validator.ValidateWhitespace(GetStringValue);
					continue;
				case XmlNodeType.EndElement:
					atomicValue = validator.ValidateEndElement(xmlSchemaInfo);
					originalAtomicValueString = GetOriginalAtomicValueStringOfElement();
					if (manageNamespaces)
					{
						nsManager.PopScope();
					}
					break;
				default:
					continue;
				}
				break;
			}
		}
		else
		{
			if (atomicValue == this)
			{
				atomicValue = null;
			}
			SwitchReader();
		}
		return atomicValue;
	}
}
