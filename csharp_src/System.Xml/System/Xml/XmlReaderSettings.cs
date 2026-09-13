using System.ComponentModel;
using System.IO;
using System.Xml.Schema;
using System.Xml.XmlConfiguration;

namespace System.Xml;

public sealed class XmlReaderSettings
{
	private bool useAsync;

	private XmlNameTable nameTable;

	private XmlResolver xmlResolver;

	private int lineNumberOffset;

	private int linePositionOffset;

	private ConformanceLevel conformanceLevel;

	private bool checkCharacters;

	private long maxCharactersInDocument;

	private long maxCharactersFromEntities;

	private bool ignoreWhitespace;

	private bool ignorePIs;

	private bool ignoreComments;

	private DtdProcessing dtdProcessing;

	private ValidationType validationType;

	private XmlSchemaValidationFlags validationFlags;

	private XmlSchemaSet schemas;

	private ValidationEventHandler valEventHandler;

	private bool closeInput;

	private bool isReadOnly;

	private static bool? s_enableLegacyXmlSettings;

	public bool Async
	{
		get
		{
			return useAsync;
		}
		set
		{
			CheckReadOnly("Async");
			useAsync = value;
		}
	}

	public XmlNameTable NameTable
	{
		get
		{
			return nameTable;
		}
		set
		{
			CheckReadOnly("NameTable");
			nameTable = value;
		}
	}

	internal bool IsXmlResolverSet { get; set; }

	public XmlResolver XmlResolver
	{
		set
		{
			CheckReadOnly("XmlResolver");
			xmlResolver = value;
			IsXmlResolverSet = true;
		}
	}

	public int LineNumberOffset
	{
		get
		{
			return lineNumberOffset;
		}
		set
		{
			CheckReadOnly("LineNumberOffset");
			lineNumberOffset = value;
		}
	}

	public int LinePositionOffset
	{
		get
		{
			return linePositionOffset;
		}
		set
		{
			CheckReadOnly("LinePositionOffset");
			linePositionOffset = value;
		}
	}

	public ConformanceLevel ConformanceLevel
	{
		get
		{
			return conformanceLevel;
		}
		set
		{
			CheckReadOnly("ConformanceLevel");
			if ((uint)value > 2u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			conformanceLevel = value;
		}
	}

	public bool CheckCharacters
	{
		get
		{
			return checkCharacters;
		}
		set
		{
			CheckReadOnly("CheckCharacters");
			checkCharacters = value;
		}
	}

	public long MaxCharactersInDocument
	{
		get
		{
			return maxCharactersInDocument;
		}
		set
		{
			CheckReadOnly("MaxCharactersInDocument");
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			maxCharactersInDocument = value;
		}
	}

	public long MaxCharactersFromEntities
	{
		get
		{
			return maxCharactersFromEntities;
		}
		set
		{
			CheckReadOnly("MaxCharactersFromEntities");
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			maxCharactersFromEntities = value;
		}
	}

	public bool IgnoreWhitespace
	{
		get
		{
			return ignoreWhitespace;
		}
		set
		{
			CheckReadOnly("IgnoreWhitespace");
			ignoreWhitespace = value;
		}
	}

	public bool IgnoreProcessingInstructions
	{
		get
		{
			return ignorePIs;
		}
		set
		{
			CheckReadOnly("IgnoreProcessingInstructions");
			ignorePIs = value;
		}
	}

	public bool IgnoreComments
	{
		get
		{
			return ignoreComments;
		}
		set
		{
			CheckReadOnly("IgnoreComments");
			ignoreComments = value;
		}
	}

	[Obsolete("Use XmlReaderSettings.DtdProcessing property instead.")]
	public bool ProhibitDtd
	{
		get
		{
			return dtdProcessing == DtdProcessing.Prohibit;
		}
		set
		{
			CheckReadOnly("ProhibitDtd");
			dtdProcessing = ((!value) ? DtdProcessing.Parse : DtdProcessing.Prohibit);
		}
	}

	public DtdProcessing DtdProcessing
	{
		get
		{
			return dtdProcessing;
		}
		set
		{
			CheckReadOnly("DtdProcessing");
			if ((uint)value > 2u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			dtdProcessing = value;
		}
	}

	public bool CloseInput
	{
		get
		{
			return closeInput;
		}
		set
		{
			CheckReadOnly("CloseInput");
			closeInput = value;
		}
	}

	public ValidationType ValidationType
	{
		get
		{
			return validationType;
		}
		set
		{
			CheckReadOnly("ValidationType");
			if ((uint)value > 4u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			validationType = value;
		}
	}

	public XmlSchemaValidationFlags ValidationFlags
	{
		get
		{
			return validationFlags;
		}
		set
		{
			CheckReadOnly("ValidationFlags");
			if ((uint)value > 31u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			validationFlags = value;
		}
	}

	public XmlSchemaSet Schemas
	{
		get
		{
			if (schemas == null)
			{
				schemas = new XmlSchemaSet();
			}
			return schemas;
		}
		set
		{
			CheckReadOnly("Schemas");
			schemas = value;
		}
	}

	internal bool ReadOnly
	{
		get
		{
			return isReadOnly;
		}
		set
		{
			isReadOnly = value;
		}
	}

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
			CheckReadOnly("ValidationEventHandler");
			valEventHandler = (ValidationEventHandler)Delegate.Combine(valEventHandler, value);
		}
		remove
		{
			CheckReadOnly("ValidationEventHandler");
			valEventHandler = (ValidationEventHandler)Delegate.Remove(valEventHandler, value);
		}
	}

	public XmlReaderSettings()
	{
		Initialize();
	}

	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public XmlReaderSettings(XmlResolver resolver)
	{
		Initialize(resolver);
	}

	internal XmlResolver GetXmlResolver()
	{
		return xmlResolver;
	}

	internal XmlResolver GetXmlResolver_CheckConfig()
	{
		if (XmlReaderSection.ProhibitDefaultUrlResolver && !IsXmlResolverSet)
		{
			return null;
		}
		return xmlResolver;
	}

	public void Reset()
	{
		CheckReadOnly("Reset");
		Initialize();
	}

	public XmlReaderSettings Clone()
	{
		XmlReaderSettings obj = MemberwiseClone() as XmlReaderSettings;
		obj.ReadOnly = false;
		return obj;
	}

	internal ValidationEventHandler GetEventHandler()
	{
		return valEventHandler;
	}

	internal XmlReader CreateReader(string inputUri, XmlParserContext inputContext)
	{
		if (inputUri == null)
		{
			throw new ArgumentNullException("inputUri");
		}
		if (inputUri.Length == 0)
		{
			throw new ArgumentException(Res.GetString("The string was not recognized as a valid Uri."), "inputUri");
		}
		XmlResolver xmlResolver = GetXmlResolver();
		if (xmlResolver == null)
		{
			xmlResolver = CreateDefaultResolver();
		}
		XmlReader xmlReader = new XmlTextReaderImpl(inputUri, this, inputContext, xmlResolver);
		if (ValidationType != ValidationType.None)
		{
			xmlReader = AddValidation(xmlReader);
		}
		if (useAsync)
		{
			xmlReader = XmlAsyncCheckReader.CreateAsyncCheckWrapper(xmlReader);
		}
		return xmlReader;
	}

	internal XmlReader CreateReader(Stream input, Uri baseUri, string baseUriString, XmlParserContext inputContext)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		if (baseUriString == null)
		{
			baseUriString = ((!(baseUri == null)) ? baseUri.ToString() : string.Empty);
		}
		XmlReader xmlReader = new XmlTextReaderImpl(input, null, 0, this, baseUri, baseUriString, inputContext, closeInput);
		if (ValidationType != ValidationType.None)
		{
			xmlReader = AddValidation(xmlReader);
		}
		if (useAsync)
		{
			xmlReader = XmlAsyncCheckReader.CreateAsyncCheckWrapper(xmlReader);
		}
		return xmlReader;
	}

	internal XmlReader CreateReader(TextReader input, string baseUriString, XmlParserContext inputContext)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		if (baseUriString == null)
		{
			baseUriString = string.Empty;
		}
		XmlReader xmlReader = new XmlTextReaderImpl(input, this, baseUriString, inputContext);
		if (ValidationType != ValidationType.None)
		{
			xmlReader = AddValidation(xmlReader);
		}
		if (useAsync)
		{
			xmlReader = XmlAsyncCheckReader.CreateAsyncCheckWrapper(xmlReader);
		}
		return xmlReader;
	}

	internal XmlReader CreateReader(XmlReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		return AddValidationAndConformanceWrapper(reader);
	}

	private void CheckReadOnly(string propertyName)
	{
		if (isReadOnly)
		{
			throw new XmlException("The '{0}' property is read only and cannot be set.", GetType().Name + "." + propertyName);
		}
	}

	private void Initialize()
	{
		Initialize(null);
	}

	private void Initialize(XmlResolver resolver)
	{
		nameTable = null;
		xmlResolver = ((resolver == null) ? CreateDefaultResolver() : resolver);
		maxCharactersFromEntities = 0L;
		lineNumberOffset = 0;
		linePositionOffset = 0;
		checkCharacters = true;
		conformanceLevel = ConformanceLevel.Document;
		ignoreWhitespace = false;
		ignorePIs = false;
		ignoreComments = false;
		dtdProcessing = DtdProcessing.Prohibit;
		closeInput = false;
		maxCharactersInDocument = 0L;
		schemas = null;
		validationType = ValidationType.None;
		validationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints;
		validationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;
		useAsync = false;
		isReadOnly = false;
		IsXmlResolverSet = false;
	}

	private static XmlResolver CreateDefaultResolver()
	{
		return new XmlUrlResolver();
	}

	internal XmlReader AddValidation(XmlReader reader)
	{
		if (validationType == ValidationType.Schema)
		{
			XmlResolver xmlResolver = GetXmlResolver_CheckConfig();
			if (xmlResolver == null && !IsXmlResolverSet && !EnableLegacyXmlSettings())
			{
				xmlResolver = new XmlUrlResolver();
			}
			reader = new XsdValidatingReader(reader, xmlResolver, this);
		}
		else if (validationType == ValidationType.DTD)
		{
			reader = CreateDtdValidatingReader(reader);
		}
		return reader;
	}

	private XmlReader AddValidationAndConformanceWrapper(XmlReader reader)
	{
		if (validationType == ValidationType.DTD)
		{
			reader = CreateDtdValidatingReader(reader);
		}
		reader = AddConformanceWrapper(reader);
		if (validationType == ValidationType.Schema)
		{
			reader = new XsdValidatingReader(reader, GetXmlResolver_CheckConfig(), this);
		}
		return reader;
	}

	private XmlValidatingReaderImpl CreateDtdValidatingReader(XmlReader baseReader)
	{
		return new XmlValidatingReaderImpl(baseReader, GetEventHandler(), (ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) != 0);
	}

	internal XmlReader AddConformanceWrapper(XmlReader baseReader)
	{
		XmlReaderSettings settings = baseReader.Settings;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool ignorePis = false;
		DtdProcessing dtdProcessing = (DtdProcessing)(-1);
		bool flag4 = false;
		if (settings == null)
		{
			if (conformanceLevel != ConformanceLevel.Auto && conformanceLevel != XmlReader.GetV1ConformanceLevel(baseReader))
			{
				throw new InvalidOperationException(Res.GetString("Cannot change conformance checking to {0}. Make sure the ConformanceLevel in XmlReaderSettings is set to Auto for wrapping scenarios.", conformanceLevel.ToString()));
			}
			XmlTextReader xmlTextReader = baseReader as XmlTextReader;
			if (xmlTextReader == null && baseReader is XmlValidatingReader xmlValidatingReader)
			{
				xmlTextReader = (XmlTextReader)xmlValidatingReader.Reader;
			}
			if (ignoreWhitespace)
			{
				WhitespaceHandling whitespaceHandling = WhitespaceHandling.All;
				if (xmlTextReader != null)
				{
					whitespaceHandling = xmlTextReader.WhitespaceHandling;
				}
				if (whitespaceHandling == WhitespaceHandling.All)
				{
					flag2 = true;
					flag4 = true;
				}
			}
			if (ignoreComments)
			{
				flag3 = true;
				flag4 = true;
			}
			if (ignorePIs)
			{
				ignorePis = true;
				flag4 = true;
			}
			DtdProcessing dtdProcessing2 = DtdProcessing.Parse;
			if (xmlTextReader != null)
			{
				dtdProcessing2 = xmlTextReader.DtdProcessing;
			}
			if ((this.dtdProcessing == DtdProcessing.Prohibit && dtdProcessing2 != DtdProcessing.Prohibit) || (this.dtdProcessing == DtdProcessing.Ignore && dtdProcessing2 == DtdProcessing.Parse))
			{
				dtdProcessing = this.dtdProcessing;
				flag4 = true;
			}
		}
		else
		{
			if (conformanceLevel != settings.ConformanceLevel && conformanceLevel != ConformanceLevel.Auto)
			{
				throw new InvalidOperationException(Res.GetString("Cannot change conformance checking to {0}. Make sure the ConformanceLevel in XmlReaderSettings is set to Auto for wrapping scenarios.", conformanceLevel.ToString()));
			}
			if (checkCharacters && !settings.CheckCharacters)
			{
				flag = true;
				flag4 = true;
			}
			if (ignoreWhitespace && !settings.IgnoreWhitespace)
			{
				flag2 = true;
				flag4 = true;
			}
			if (ignoreComments && !settings.IgnoreComments)
			{
				flag3 = true;
				flag4 = true;
			}
			if (ignorePIs && !settings.IgnoreProcessingInstructions)
			{
				ignorePis = true;
				flag4 = true;
			}
			if ((this.dtdProcessing == DtdProcessing.Prohibit && settings.DtdProcessing != DtdProcessing.Prohibit) || (this.dtdProcessing == DtdProcessing.Ignore && settings.DtdProcessing == DtdProcessing.Parse))
			{
				dtdProcessing = this.dtdProcessing;
				flag4 = true;
			}
		}
		if (flag4)
		{
			if (baseReader is IXmlNamespaceResolver readerAsNSResolver)
			{
				return new XmlCharCheckingReaderWithNS(baseReader, readerAsNSResolver, flag, flag2, flag3, ignorePis, dtdProcessing);
			}
			return new XmlCharCheckingReader(baseReader, flag, flag2, flag3, ignorePis, dtdProcessing);
		}
		return baseReader;
	}

	internal static bool EnableLegacyXmlSettings()
	{
		if (s_enableLegacyXmlSettings.HasValue)
		{
			return s_enableLegacyXmlSettings.Value;
		}
		if (!BinaryCompatibility.TargetsAtLeast_Desktop_V4_5_2)
		{
			s_enableLegacyXmlSettings = true;
			return s_enableLegacyXmlSettings.Value;
		}
		s_enableLegacyXmlSettings = false;
		return s_enableLegacyXmlSettings.Value;
	}
}
