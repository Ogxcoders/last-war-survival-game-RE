using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Xml;

[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
public sealed class XmlWriterSettings
{
	private bool useAsync;

	private Encoding encoding;

	private bool omitXmlDecl;

	private NewLineHandling newLineHandling;

	private string newLineChars;

	private TriState indent;

	private string indentChars;

	private bool newLineOnAttributes;

	private bool closeOutput;

	private NamespaceHandling namespaceHandling;

	private ConformanceLevel conformanceLevel;

	private bool checkCharacters;

	private bool writeEndDocumentOnClose;

	private XmlOutputMethod outputMethod;

	private List<XmlQualifiedName> cdataSections = new List<XmlQualifiedName>();

	private bool doNotEscapeUriAttributes;

	private bool mergeCDataSections;

	private string mediaType;

	private string docTypeSystem;

	private string docTypePublic;

	private XmlStandalone standalone;

	private bool autoXmlDecl;

	private bool isReadOnly;

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

	public Encoding Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			CheckReadOnly("Encoding");
			encoding = value;
		}
	}

	public bool OmitXmlDeclaration
	{
		get
		{
			return omitXmlDecl;
		}
		set
		{
			CheckReadOnly("OmitXmlDeclaration");
			omitXmlDecl = value;
		}
	}

	public NewLineHandling NewLineHandling
	{
		get
		{
			return newLineHandling;
		}
		set
		{
			CheckReadOnly("NewLineHandling");
			if ((uint)value > 2u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			newLineHandling = value;
		}
	}

	public string NewLineChars
	{
		get
		{
			return newLineChars;
		}
		set
		{
			CheckReadOnly("NewLineChars");
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			newLineChars = value;
		}
	}

	public bool Indent
	{
		get
		{
			return indent == TriState.True;
		}
		set
		{
			CheckReadOnly("Indent");
			indent = (value ? TriState.True : TriState.False);
		}
	}

	public string IndentChars
	{
		get
		{
			return indentChars;
		}
		set
		{
			CheckReadOnly("IndentChars");
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			indentChars = value;
		}
	}

	public bool NewLineOnAttributes
	{
		get
		{
			return newLineOnAttributes;
		}
		set
		{
			CheckReadOnly("NewLineOnAttributes");
			newLineOnAttributes = value;
		}
	}

	public bool CloseOutput
	{
		get
		{
			return closeOutput;
		}
		set
		{
			CheckReadOnly("CloseOutput");
			closeOutput = value;
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

	public NamespaceHandling NamespaceHandling
	{
		get
		{
			return namespaceHandling;
		}
		set
		{
			CheckReadOnly("NamespaceHandling");
			if ((uint)value > 1u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			namespaceHandling = value;
		}
	}

	public bool WriteEndDocumentOnClose
	{
		get
		{
			return writeEndDocumentOnClose;
		}
		set
		{
			CheckReadOnly("WriteEndDocumentOnClose");
			writeEndDocumentOnClose = value;
		}
	}

	public XmlOutputMethod OutputMethod
	{
		get
		{
			return outputMethod;
		}
		internal set
		{
			outputMethod = value;
		}
	}

	internal List<XmlQualifiedName> CDataSectionElements => cdataSections;

	public bool DoNotEscapeUriAttributes
	{
		get
		{
			return doNotEscapeUriAttributes;
		}
		set
		{
			CheckReadOnly("DoNotEscapeUriAttributes");
			doNotEscapeUriAttributes = value;
		}
	}

	internal bool MergeCDataSections
	{
		get
		{
			return mergeCDataSections;
		}
		set
		{
			CheckReadOnly("MergeCDataSections");
			mergeCDataSections = value;
		}
	}

	internal string MediaType
	{
		get
		{
			return mediaType;
		}
		set
		{
			CheckReadOnly("MediaType");
			mediaType = value;
		}
	}

	internal string DocTypeSystem
	{
		get
		{
			return docTypeSystem;
		}
		set
		{
			CheckReadOnly("DocTypeSystem");
			docTypeSystem = value;
		}
	}

	internal string DocTypePublic
	{
		get
		{
			return docTypePublic;
		}
		set
		{
			CheckReadOnly("DocTypePublic");
			docTypePublic = value;
		}
	}

	internal XmlStandalone Standalone
	{
		get
		{
			return standalone;
		}
		set
		{
			CheckReadOnly("Standalone");
			standalone = value;
		}
	}

	internal bool AutoXmlDeclaration
	{
		get
		{
			return autoXmlDecl;
		}
		set
		{
			CheckReadOnly("AutoXmlDeclaration");
			autoXmlDecl = value;
		}
	}

	internal TriState IndentInternal
	{
		get
		{
			return indent;
		}
		set
		{
			indent = value;
		}
	}

	internal bool IsQuerySpecific
	{
		get
		{
			if (cdataSections.Count == 0 && docTypePublic == null && docTypeSystem == null)
			{
				return standalone == XmlStandalone.Yes;
			}
			return true;
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

	public XmlWriterSettings()
	{
		Initialize();
	}

	public void Reset()
	{
		CheckReadOnly("Reset");
		Initialize();
	}

	public XmlWriterSettings Clone()
	{
		XmlWriterSettings obj = MemberwiseClone() as XmlWriterSettings;
		obj.cdataSections = new List<XmlQualifiedName>(cdataSections);
		obj.isReadOnly = false;
		return obj;
	}

	internal XmlWriter CreateWriter(string outputFileName)
	{
		if (outputFileName == null)
		{
			throw new ArgumentNullException("outputFileName");
		}
		XmlWriterSettings xmlWriterSettings = this;
		if (!xmlWriterSettings.CloseOutput)
		{
			xmlWriterSettings = xmlWriterSettings.Clone();
			xmlWriterSettings.CloseOutput = true;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.Read, 4096, useAsync);
			return xmlWriterSettings.CreateWriter(fileStream);
		}
		catch
		{
			fileStream?.Close();
			throw;
		}
	}

	internal XmlWriter CreateWriter(Stream output)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		XmlWriter xmlWriter;
		if (Encoding.WebName == "utf-8")
		{
			switch (OutputMethod)
			{
			case XmlOutputMethod.Xml:
				xmlWriter = ((!Indent) ? new XmlUtf8RawTextWriter(output, this) : new XmlUtf8RawTextWriterIndent(output, this));
				break;
			case XmlOutputMethod.Html:
				xmlWriter = ((!Indent) ? new HtmlUtf8RawTextWriter(output, this) : new HtmlUtf8RawTextWriterIndent(output, this));
				break;
			case XmlOutputMethod.Text:
				xmlWriter = new TextUtf8RawTextWriter(output, this);
				break;
			case XmlOutputMethod.AutoDetect:
				xmlWriter = new XmlAutoDetectWriter(output, this);
				break;
			default:
				return null;
			}
		}
		else
		{
			switch (OutputMethod)
			{
			case XmlOutputMethod.Xml:
				xmlWriter = ((!Indent) ? new XmlEncodedRawTextWriter(output, this) : new XmlEncodedRawTextWriterIndent(output, this));
				break;
			case XmlOutputMethod.Html:
				xmlWriter = ((!Indent) ? new HtmlEncodedRawTextWriter(output, this) : new HtmlEncodedRawTextWriterIndent(output, this));
				break;
			case XmlOutputMethod.Text:
				xmlWriter = new TextEncodedRawTextWriter(output, this);
				break;
			case XmlOutputMethod.AutoDetect:
				xmlWriter = new XmlAutoDetectWriter(output, this);
				break;
			default:
				return null;
			}
		}
		if (OutputMethod != XmlOutputMethod.AutoDetect && IsQuerySpecific)
		{
			xmlWriter = new QueryOutputWriter((XmlRawWriter)xmlWriter, this);
		}
		xmlWriter = new XmlWellFormedWriter(xmlWriter, this);
		if (useAsync)
		{
			xmlWriter = new XmlAsyncCheckWriter(xmlWriter);
		}
		return xmlWriter;
	}

	internal XmlWriter CreateWriter(TextWriter output)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		XmlWriter xmlWriter;
		switch (OutputMethod)
		{
		case XmlOutputMethod.Xml:
			xmlWriter = ((!Indent) ? new XmlEncodedRawTextWriter(output, this) : new XmlEncodedRawTextWriterIndent(output, this));
			break;
		case XmlOutputMethod.Html:
			xmlWriter = ((!Indent) ? new HtmlEncodedRawTextWriter(output, this) : new HtmlEncodedRawTextWriterIndent(output, this));
			break;
		case XmlOutputMethod.Text:
			xmlWriter = new TextEncodedRawTextWriter(output, this);
			break;
		case XmlOutputMethod.AutoDetect:
			xmlWriter = new XmlAutoDetectWriter(output, this);
			break;
		default:
			return null;
		}
		if (OutputMethod != XmlOutputMethod.AutoDetect && IsQuerySpecific)
		{
			xmlWriter = new QueryOutputWriter((XmlRawWriter)xmlWriter, this);
		}
		xmlWriter = new XmlWellFormedWriter(xmlWriter, this);
		if (useAsync)
		{
			xmlWriter = new XmlAsyncCheckWriter(xmlWriter);
		}
		return xmlWriter;
	}

	internal XmlWriter CreateWriter(XmlWriter output)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		return AddConformanceWrapper(output);
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
		encoding = Encoding.UTF8;
		omitXmlDecl = false;
		newLineHandling = NewLineHandling.Replace;
		newLineChars = Environment.NewLine;
		indent = TriState.Unknown;
		indentChars = "  ";
		newLineOnAttributes = false;
		closeOutput = false;
		namespaceHandling = NamespaceHandling.Default;
		conformanceLevel = ConformanceLevel.Document;
		checkCharacters = true;
		writeEndDocumentOnClose = true;
		outputMethod = XmlOutputMethod.Xml;
		cdataSections.Clear();
		mergeCDataSections = false;
		mediaType = null;
		docTypeSystem = null;
		docTypePublic = null;
		standalone = XmlStandalone.Omit;
		doNotEscapeUriAttributes = false;
		useAsync = false;
		isReadOnly = false;
	}

	private XmlWriter AddConformanceWrapper(XmlWriter baseWriter)
	{
		ConformanceLevel conformanceLevel = ConformanceLevel.Auto;
		XmlWriterSettings settings = baseWriter.Settings;
		bool flag = false;
		bool checkNames = false;
		bool flag2 = false;
		bool flag3 = false;
		if (settings == null)
		{
			if (newLineHandling == NewLineHandling.Replace)
			{
				flag2 = true;
				flag3 = true;
			}
			if (checkCharacters)
			{
				flag = true;
				flag3 = true;
			}
		}
		else
		{
			if (this.conformanceLevel != settings.ConformanceLevel)
			{
				conformanceLevel = ConformanceLevel;
				flag3 = true;
			}
			if (checkCharacters && !settings.CheckCharacters)
			{
				flag = true;
				checkNames = conformanceLevel == ConformanceLevel.Auto;
				flag3 = true;
			}
			if (newLineHandling == NewLineHandling.Replace && settings.NewLineHandling == NewLineHandling.None)
			{
				flag2 = true;
				flag3 = true;
			}
		}
		XmlWriter xmlWriter = baseWriter;
		if (flag3)
		{
			if (conformanceLevel != ConformanceLevel.Auto)
			{
				xmlWriter = new XmlWellFormedWriter(xmlWriter, this);
			}
			if (flag || flag2)
			{
				xmlWriter = new XmlCharCheckingWriter(xmlWriter, flag, checkNames, flag2, NewLineChars);
			}
		}
		if (IsQuerySpecific && (settings == null || !settings.IsQuerySpecific))
		{
			xmlWriter = new QueryOutputWriterV1(xmlWriter, this);
		}
		return xmlWriter;
	}
}
