using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace System.Xml;

internal sealed class XmlValidatingReaderImpl : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
{
	private enum ParsingFunction
	{
		Read,
		Init,
		ParseDtdFromContext,
		ResolveEntityInternally,
		InReadBinaryContent,
		ReaderClosed,
		Error,
		None
	}

	internal class ValidationEventHandling : IValidationEventHandling
	{
		private XmlValidatingReaderImpl reader;

		private ValidationEventHandler eventHandler;

		object IValidationEventHandling.EventHandler => eventHandler;

		internal ValidationEventHandling(XmlValidatingReaderImpl reader)
		{
			this.reader = reader;
		}

		void IValidationEventHandling.SendEvent(Exception exception, XmlSeverityType severity)
		{
			if (eventHandler != null)
			{
				eventHandler(reader, new ValidationEventArgs((XmlSchemaException)exception, severity));
			}
			else if (reader.ValidationType != ValidationType.None && severity == XmlSeverityType.Error)
			{
				throw exception;
			}
		}

		internal void AddHandler(ValidationEventHandler handler)
		{
			eventHandler = (ValidationEventHandler)Delegate.Combine(eventHandler, handler);
		}

		internal void RemoveHandler(ValidationEventHandler handler)
		{
			eventHandler = (ValidationEventHandler)Delegate.Remove(eventHandler, handler);
		}
	}

	private XmlReader coreReader;

	private XmlTextReaderImpl coreReaderImpl;

	private IXmlNamespaceResolver coreReaderNSResolver;

	private ValidationType validationType;

	private BaseValidator validator;

	private XmlSchemaCollection schemaCollection;

	private bool processIdentityConstraints;

	private ParsingFunction parsingFunction = ParsingFunction.Init;

	private ValidationEventHandling eventHandling;

	private XmlParserContext parserContext;

	private ReadContentAsBinaryHelper readBinaryHelper;

	private XmlReader outerReader;

	private static XmlResolver s_tempResolver;

	public override XmlReaderSettings Settings
	{
		get
		{
			XmlReaderSettings xmlReaderSettings = ((!coreReaderImpl.V1Compat) ? coreReader.Settings : null);
			xmlReaderSettings = ((xmlReaderSettings == null) ? new XmlReaderSettings() : xmlReaderSettings.Clone());
			xmlReaderSettings.ValidationType = ValidationType.DTD;
			if (!processIdentityConstraints)
			{
				xmlReaderSettings.ValidationFlags &= ~XmlSchemaValidationFlags.ProcessIdentityConstraints;
			}
			xmlReaderSettings.ReadOnly = true;
			return xmlReaderSettings;
		}
	}

	public override XmlNodeType NodeType => coreReader.NodeType;

	public override string Name => coreReader.Name;

	public override string LocalName => coreReader.LocalName;

	public override string NamespaceURI => coreReader.NamespaceURI;

	public override string Prefix => coreReader.Prefix;

	public override bool HasValue => coreReader.HasValue;

	public override string Value => coreReader.Value;

	public override int Depth => coreReader.Depth;

	public override string BaseURI => coreReader.BaseURI;

	public override bool IsEmptyElement => coreReader.IsEmptyElement;

	public override bool IsDefault => coreReader.IsDefault;

	public override char QuoteChar => coreReader.QuoteChar;

	public override XmlSpace XmlSpace => coreReader.XmlSpace;

	public override string XmlLang => coreReader.XmlLang;

	public override ReadState ReadState
	{
		get
		{
			if (parsingFunction != ParsingFunction.Init)
			{
				return coreReader.ReadState;
			}
			return ReadState.Initial;
		}
	}

	public override bool EOF => coreReader.EOF;

	public override XmlNameTable NameTable => coreReader.NameTable;

	internal Encoding Encoding => coreReaderImpl.Encoding;

	public override int AttributeCount => coreReader.AttributeCount;

	public override bool CanReadBinaryContent => true;

	public override bool CanResolveEntity => true;

	internal XmlReader OuterReader
	{
		get
		{
			return outerReader;
		}
		set
		{
			outerReader = value;
		}
	}

	public int LineNumber => ((IXmlLineInfo)coreReader).LineNumber;

	public int LinePosition => ((IXmlLineInfo)coreReader).LinePosition;

	internal object SchemaType
	{
		get
		{
			if (validationType != ValidationType.None)
			{
				if (coreReaderImpl.InternalSchemaType is XmlSchemaType xmlSchemaType && xmlSchemaType.QualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema")
				{
					return xmlSchemaType.Datatype;
				}
				return coreReaderImpl.InternalSchemaType;
			}
			return null;
		}
	}

	internal XmlReader Reader => coreReader;

	internal XmlTextReaderImpl ReaderImpl => coreReaderImpl;

	internal ValidationType ValidationType
	{
		get
		{
			return validationType;
		}
		set
		{
			if (ReadState != ReadState.Initial)
			{
				throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
			}
			validationType = value;
			SetupValidation(value);
		}
	}

	internal XmlSchemaCollection Schemas => schemaCollection;

	internal EntityHandling EntityHandling
	{
		get
		{
			return coreReaderImpl.EntityHandling;
		}
		set
		{
			coreReaderImpl.EntityHandling = value;
		}
	}

	internal XmlResolver XmlResolver
	{
		set
		{
			coreReaderImpl.XmlResolver = value;
			validator.XmlResolver = value;
			schemaCollection.XmlResolver = value;
		}
	}

	internal bool Namespaces
	{
		get
		{
			return coreReaderImpl.Namespaces;
		}
		set
		{
			coreReaderImpl.Namespaces = value;
		}
	}

	internal BaseValidator Validator
	{
		get
		{
			return validator;
		}
		set
		{
			validator = value;
		}
	}

	internal override XmlNamespaceManager NamespaceManager => coreReaderImpl.NamespaceManager;

	internal bool StandAlone => coreReaderImpl.StandAlone;

	internal object SchemaTypeObject
	{
		set
		{
			coreReaderImpl.InternalSchemaType = value;
		}
	}

	internal object TypedValueObject
	{
		get
		{
			return coreReaderImpl.InternalTypedValue;
		}
		set
		{
			coreReaderImpl.InternalTypedValue = value;
		}
	}

	internal bool Normalization => coreReaderImpl.Normalization;

	internal override IDtdInfo DtdInfo => coreReaderImpl.DtdInfo;

	internal event ValidationEventHandler ValidationEventHandler
	{
		add
		{
			eventHandling.AddHandler(value);
		}
		remove
		{
			eventHandling.RemoveHandler(value);
		}
	}

	internal XmlValidatingReaderImpl(XmlReader reader)
	{
		if (reader is XmlAsyncCheckReader xmlAsyncCheckReader)
		{
			reader = xmlAsyncCheckReader.CoreReader;
		}
		outerReader = this;
		coreReader = reader;
		coreReaderNSResolver = reader as IXmlNamespaceResolver;
		coreReaderImpl = reader as XmlTextReaderImpl;
		if (coreReaderImpl == null && reader is XmlTextReader xmlTextReader)
		{
			coreReaderImpl = xmlTextReader.Impl;
		}
		if (coreReaderImpl == null)
		{
			throw new ArgumentException(Res.GetString("The XmlReader passed in to construct this XmlValidatingReaderImpl must be an instance of a System.Xml.XmlTextReader."), "reader");
		}
		coreReaderImpl.EntityHandling = EntityHandling.ExpandEntities;
		coreReaderImpl.XmlValidatingReaderCompatibilityMode = true;
		processIdentityConstraints = true;
		schemaCollection = new XmlSchemaCollection(coreReader.NameTable);
		schemaCollection.XmlResolver = GetResolver();
		eventHandling = new ValidationEventHandling(this);
		coreReaderImpl.ValidationEventHandling = eventHandling;
		coreReaderImpl.OnDefaultAttributeUse = ValidateDefaultAttributeOnUse;
		validationType = ValidationType.Auto;
		SetupValidation(ValidationType.Auto);
	}

	internal XmlValidatingReaderImpl(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
		: this(new XmlTextReader(xmlFragment, fragType, context))
	{
		if (coreReader.BaseURI.Length > 0)
		{
			validator.BaseUri = GetResolver().ResolveUri(null, coreReader.BaseURI);
		}
		if (context != null)
		{
			parsingFunction = ParsingFunction.ParseDtdFromContext;
			parserContext = context;
		}
	}

	internal XmlValidatingReaderImpl(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
		: this(new XmlTextReader(xmlFragment, fragType, context))
	{
		if (coreReader.BaseURI.Length > 0)
		{
			validator.BaseUri = GetResolver().ResolveUri(null, coreReader.BaseURI);
		}
		if (context != null)
		{
			parsingFunction = ParsingFunction.ParseDtdFromContext;
			parserContext = context;
		}
	}

	internal XmlValidatingReaderImpl(XmlReader reader, ValidationEventHandler settingsEventHandler, bool processIdentityConstraints)
	{
		if (reader is XmlAsyncCheckReader xmlAsyncCheckReader)
		{
			reader = xmlAsyncCheckReader.CoreReader;
		}
		outerReader = this;
		coreReader = reader;
		coreReaderImpl = reader as XmlTextReaderImpl;
		if (coreReaderImpl == null && reader is XmlTextReader xmlTextReader)
		{
			coreReaderImpl = xmlTextReader.Impl;
		}
		if (coreReaderImpl == null)
		{
			throw new ArgumentException(Res.GetString("The XmlReader passed in to construct this XmlValidatingReaderImpl must be an instance of a System.Xml.XmlTextReader."), "reader");
		}
		coreReaderImpl.XmlValidatingReaderCompatibilityMode = true;
		coreReaderNSResolver = reader as IXmlNamespaceResolver;
		this.processIdentityConstraints = processIdentityConstraints;
		schemaCollection = new XmlSchemaCollection(coreReader.NameTable);
		schemaCollection.XmlResolver = GetResolver();
		eventHandling = new ValidationEventHandling(this);
		if (settingsEventHandler != null)
		{
			eventHandling.AddHandler(settingsEventHandler);
		}
		coreReaderImpl.ValidationEventHandling = eventHandling;
		coreReaderImpl.OnDefaultAttributeUse = ValidateDefaultAttributeOnUse;
		validationType = ValidationType.DTD;
		SetupValidation(ValidationType.DTD);
	}

	public override string GetAttribute(string name)
	{
		return coreReader.GetAttribute(name);
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		return coreReader.GetAttribute(localName, namespaceURI);
	}

	public override string GetAttribute(int i)
	{
		return coreReader.GetAttribute(i);
	}

	public override bool MoveToAttribute(string name)
	{
		if (!coreReader.MoveToAttribute(name))
		{
			return false;
		}
		parsingFunction = ParsingFunction.Read;
		return true;
	}

	public override bool MoveToAttribute(string localName, string namespaceURI)
	{
		if (!coreReader.MoveToAttribute(localName, namespaceURI))
		{
			return false;
		}
		parsingFunction = ParsingFunction.Read;
		return true;
	}

	public override void MoveToAttribute(int i)
	{
		coreReader.MoveToAttribute(i);
		parsingFunction = ParsingFunction.Read;
	}

	public override bool MoveToFirstAttribute()
	{
		if (!coreReader.MoveToFirstAttribute())
		{
			return false;
		}
		parsingFunction = ParsingFunction.Read;
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (!coreReader.MoveToNextAttribute())
		{
			return false;
		}
		parsingFunction = ParsingFunction.Read;
		return true;
	}

	public override bool MoveToElement()
	{
		if (!coreReader.MoveToElement())
		{
			return false;
		}
		parsingFunction = ParsingFunction.Read;
		return true;
	}

	public override bool Read()
	{
		switch (parsingFunction)
		{
		case ParsingFunction.Read:
			if (coreReader.Read())
			{
				ProcessCoreReaderEvent();
				return true;
			}
			validator.CompleteValidation();
			return false;
		case ParsingFunction.ParseDtdFromContext:
			parsingFunction = ParsingFunction.Read;
			ParseDtdFromParserContext();
			goto case ParsingFunction.Read;
		case ParsingFunction.ReaderClosed:
		case ParsingFunction.Error:
			return false;
		case ParsingFunction.Init:
			parsingFunction = ParsingFunction.Read;
			if (coreReader.ReadState == ReadState.Interactive)
			{
				ProcessCoreReaderEvent();
				return true;
			}
			goto case ParsingFunction.Read;
		case ParsingFunction.ResolveEntityInternally:
			parsingFunction = ParsingFunction.Read;
			ResolveEntityInternally();
			goto case ParsingFunction.Read;
		case ParsingFunction.InReadBinaryContent:
			parsingFunction = ParsingFunction.Read;
			readBinaryHelper.Finish();
			goto case ParsingFunction.Read;
		default:
			return false;
		}
	}

	public override void Close()
	{
		coreReader.Close();
		parsingFunction = ParsingFunction.ReaderClosed;
	}

	public override string LookupNamespace(string prefix)
	{
		return coreReaderImpl.LookupNamespace(prefix);
	}

	public override bool ReadAttributeValue()
	{
		if (parsingFunction == ParsingFunction.InReadBinaryContent)
		{
			parsingFunction = ParsingFunction.Read;
			readBinaryHelper.Finish();
		}
		if (!coreReader.ReadAttributeValue())
		{
			return false;
		}
		parsingFunction = ParsingFunction.Read;
		return true;
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = readBinaryHelper.ReadContentAsBase64(buffer, index, count);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = readBinaryHelper.ReadContentAsBinHex(buffer, index, count);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = readBinaryHelper.ReadElementContentAsBase64(buffer, index, count);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = readBinaryHelper.ReadElementContentAsBinHex(buffer, index, count);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override void ResolveEntity()
	{
		if (parsingFunction == ParsingFunction.ResolveEntityInternally)
		{
			parsingFunction = ParsingFunction.Read;
		}
		coreReader.ResolveEntity();
	}

	internal void MoveOffEntityReference()
	{
		if (outerReader.NodeType == XmlNodeType.EntityReference && parsingFunction != ParsingFunction.ResolveEntityInternally && !outerReader.Read())
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	public override string ReadString()
	{
		MoveOffEntityReference();
		return base.ReadString();
	}

	public bool HasLineInfo()
	{
		return true;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		return LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return LookupPrefix(namespaceName);
	}

	internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return coreReaderNSResolver.GetNamespacesInScope(scope);
	}

	internal string LookupPrefix(string namespaceName)
	{
		return coreReaderNSResolver.LookupPrefix(namespaceName);
	}

	public object ReadTypedValue()
	{
		if (validationType == ValidationType.None)
		{
			return null;
		}
		switch (outerReader.NodeType)
		{
		case XmlNodeType.Attribute:
			return coreReaderImpl.InternalTypedValue;
		case XmlNodeType.Element:
			if (SchemaType == null)
			{
				return null;
			}
			if (((SchemaType is XmlSchemaDatatype) ? ((XmlSchemaDatatype)SchemaType) : ((XmlSchemaType)SchemaType).Datatype) != null)
			{
				if (!outerReader.IsEmptyElement)
				{
					XmlNodeType nodeType;
					do
					{
						if (!outerReader.Read())
						{
							throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
						}
						nodeType = outerReader.NodeType;
					}
					while (nodeType == XmlNodeType.CDATA || nodeType == XmlNodeType.Text || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace || nodeType == XmlNodeType.Comment || nodeType == XmlNodeType.ProcessingInstruction);
					if (outerReader.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("'{0}' is an invalid XmlNodeType.", outerReader.NodeType.ToString());
					}
				}
				return coreReaderImpl.InternalTypedValue;
			}
			return null;
		case XmlNodeType.EndElement:
			return null;
		default:
			if (coreReaderImpl.V1Compat)
			{
				return null;
			}
			return Value;
		}
	}

	private void ParseDtdFromParserContext()
	{
		if (parserContext.DocTypeName != null && parserContext.DocTypeName.Length != 0)
		{
			IDtdInfo dtdInfo = DtdParser.Create().ParseFreeFloatingDtd(adapter: new XmlTextReaderImpl.DtdParserProxy(coreReaderImpl), baseUri: parserContext.BaseURI, docTypeName: parserContext.DocTypeName, publicId: parserContext.PublicId, systemId: parserContext.SystemId, internalSubset: parserContext.InternalSubset);
			coreReaderImpl.SetDtdInfo(dtdInfo);
			ValidateDtd();
		}
	}

	private void ValidateDtd()
	{
		IDtdInfo dtdInfo = coreReaderImpl.DtdInfo;
		if (dtdInfo != null)
		{
			switch (validationType)
			{
			default:
				return;
			case ValidationType.Auto:
				SetupValidation(ValidationType.DTD);
				break;
			case ValidationType.None:
			case ValidationType.DTD:
				break;
			}
			validator.DtdInfo = dtdInfo;
		}
	}

	private void ResolveEntityInternally()
	{
		int depth = coreReader.Depth;
		outerReader.ResolveEntity();
		while (outerReader.Read() && coreReader.Depth > depth)
		{
		}
	}

	private void SetupValidation(ValidationType valType)
	{
		validator = BaseValidator.CreateInstance(valType, this, schemaCollection, eventHandling, processIdentityConstraints);
		XmlResolver resolver = GetResolver();
		validator.XmlResolver = resolver;
		if (outerReader.BaseURI.Length > 0)
		{
			validator.BaseUri = ((resolver == null) ? new Uri(outerReader.BaseURI, UriKind.RelativeOrAbsolute) : resolver.ResolveUri(null, outerReader.BaseURI));
		}
		coreReaderImpl.ValidationEventHandling = ((validationType == ValidationType.None) ? null : eventHandling);
	}

	private XmlResolver GetResolver()
	{
		XmlResolver resolver = coreReaderImpl.GetResolver();
		if (resolver == null && !coreReaderImpl.IsResolverSet && !XmlReaderSettings.EnableLegacyXmlSettings())
		{
			if (s_tempResolver == null)
			{
				s_tempResolver = new XmlUrlResolver();
			}
			return s_tempResolver;
		}
		return resolver;
	}

	private void ProcessCoreReaderEvent()
	{
		switch (coreReader.NodeType)
		{
		case XmlNodeType.Whitespace:
			if ((coreReader.Depth > 0 || coreReaderImpl.FragmentType != XmlNodeType.Document) && validator.PreserveWhitespace)
			{
				coreReaderImpl.ChangeCurrentNodeType(XmlNodeType.SignificantWhitespace);
			}
			break;
		case XmlNodeType.DocumentType:
			ValidateDtd();
			return;
		case XmlNodeType.EntityReference:
			parsingFunction = ParsingFunction.ResolveEntityInternally;
			break;
		}
		coreReaderImpl.InternalSchemaType = null;
		coreReaderImpl.InternalTypedValue = null;
		validator.Validate();
	}

	internal void Close(bool closeStream)
	{
		coreReaderImpl.Close(closeStream);
		parsingFunction = ParsingFunction.ReaderClosed;
	}

	internal bool AddDefaultAttribute(SchemaAttDef attdef)
	{
		return coreReaderImpl.AddDefaultAttributeNonDtd(attdef);
	}

	internal void ValidateDefaultAttributeOnUse(IDtdDefaultAttributeInfo defaultAttribute, XmlTextReaderImpl coreReader)
	{
		if (defaultAttribute is SchemaAttDef { DefaultValueChecked: false } schemaAttDef && coreReader.DtdInfo is SchemaInfo sinfo)
		{
			DtdValidator.CheckDefaultValue(schemaAttDef, sinfo, eventHandling, coreReader.BaseURI);
		}
	}

	public override Task<string> GetValueAsync()
	{
		return coreReader.GetValueAsync();
	}

	public override async Task<bool> ReadAsync()
	{
		switch (parsingFunction)
		{
		case ParsingFunction.Read:
			if (await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				ProcessCoreReaderEvent();
				return true;
			}
			validator.CompleteValidation();
			return false;
		case ParsingFunction.ParseDtdFromContext:
			parsingFunction = ParsingFunction.Read;
			await ParseDtdFromParserContextAsync().ConfigureAwait(continueOnCapturedContext: false);
			goto case ParsingFunction.Read;
		case ParsingFunction.ReaderClosed:
		case ParsingFunction.Error:
			return false;
		case ParsingFunction.Init:
			parsingFunction = ParsingFunction.Read;
			if (coreReader.ReadState == ReadState.Interactive)
			{
				ProcessCoreReaderEvent();
				return true;
			}
			goto case ParsingFunction.Read;
		case ParsingFunction.ResolveEntityInternally:
			parsingFunction = ParsingFunction.Read;
			await ResolveEntityInternallyAsync().ConfigureAwait(continueOnCapturedContext: false);
			goto case ParsingFunction.Read;
		case ParsingFunction.InReadBinaryContent:
			parsingFunction = ParsingFunction.Read;
			await readBinaryHelper.FinishAsync().ConfigureAwait(continueOnCapturedContext: false);
			goto case ParsingFunction.Read;
		default:
			return false;
		}
	}

	public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = await readBinaryHelper.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = await readBinaryHelper.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = await readBinaryHelper.ReadElementContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		if (ReadState != ReadState.Interactive)
		{
			return 0;
		}
		if (parsingFunction != ParsingFunction.InReadBinaryContent)
		{
			readBinaryHelper = ReadContentAsBinaryHelper.CreateOrReset(readBinaryHelper, outerReader);
		}
		parsingFunction = ParsingFunction.Read;
		int result = await readBinaryHelper.ReadElementContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		parsingFunction = ParsingFunction.InReadBinaryContent;
		return result;
	}

	internal async Task MoveOffEntityReferenceAsync()
	{
		if (outerReader.NodeType == XmlNodeType.EntityReference && parsingFunction != ParsingFunction.ResolveEntityInternally && !(await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false)))
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	public async Task<object> ReadTypedValueAsync()
	{
		if (validationType == ValidationType.None)
		{
			return null;
		}
		switch (outerReader.NodeType)
		{
		case XmlNodeType.Attribute:
			return coreReaderImpl.InternalTypedValue;
		case XmlNodeType.Element:
			if (SchemaType == null)
			{
				return null;
			}
			if (((SchemaType is XmlSchemaDatatype) ? ((XmlSchemaDatatype)SchemaType) : ((XmlSchemaType)SchemaType).Datatype) != null)
			{
				if (!outerReader.IsEmptyElement)
				{
					XmlNodeType nodeType;
					do
					{
						if (!(await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false)))
						{
							throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
						}
						nodeType = outerReader.NodeType;
					}
					while (nodeType == XmlNodeType.CDATA || nodeType == XmlNodeType.Text || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace || nodeType == XmlNodeType.Comment || nodeType == XmlNodeType.ProcessingInstruction);
					if (outerReader.NodeType != XmlNodeType.EndElement)
					{
						throw new XmlException("'{0}' is an invalid XmlNodeType.", outerReader.NodeType.ToString());
					}
				}
				return coreReaderImpl.InternalTypedValue;
			}
			return null;
		case XmlNodeType.EndElement:
			return null;
		default:
			if (coreReaderImpl.V1Compat)
			{
				return null;
			}
			return await GetValueAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task ParseDtdFromParserContextAsync()
	{
		if (parserContext.DocTypeName != null && parserContext.DocTypeName.Length != 0)
		{
			IDtdInfo dtdInfo = await DtdParser.Create().ParseFreeFloatingDtdAsync(adapter: new XmlTextReaderImpl.DtdParserProxy(coreReaderImpl), baseUri: parserContext.BaseURI, docTypeName: parserContext.DocTypeName, publicId: parserContext.PublicId, systemId: parserContext.SystemId, internalSubset: parserContext.InternalSubset).ConfigureAwait(continueOnCapturedContext: false);
			coreReaderImpl.SetDtdInfo(dtdInfo);
			ValidateDtd();
		}
	}

	private async Task ResolveEntityInternallyAsync()
	{
		int initialDepth = coreReader.Depth;
		outerReader.ResolveEntity();
		while (await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false) && coreReader.Depth > initialDepth)
		{
		}
	}
}
