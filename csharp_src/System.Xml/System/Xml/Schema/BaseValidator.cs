using System.Collections;
using System.Text;

namespace System.Xml.Schema;

internal class BaseValidator
{
	private XmlSchemaCollection schemaCollection;

	private IValidationEventHandling eventHandling;

	private XmlNameTable nameTable;

	private SchemaNames schemaNames;

	private PositionInfo positionInfo;

	private XmlResolver xmlResolver;

	private Uri baseUri;

	protected SchemaInfo schemaInfo;

	protected XmlValidatingReaderImpl reader;

	protected XmlQualifiedName elementName;

	protected ValidationState context;

	protected StringBuilder textValue;

	protected string textString;

	protected bool hasSibling;

	protected bool checkDatatype;

	public XmlValidatingReaderImpl Reader => reader;

	public XmlSchemaCollection SchemaCollection => schemaCollection;

	public XmlNameTable NameTable => nameTable;

	public SchemaNames SchemaNames
	{
		get
		{
			if (schemaNames != null)
			{
				return schemaNames;
			}
			if (schemaCollection != null)
			{
				schemaNames = schemaCollection.GetSchemaNames(nameTable);
			}
			else
			{
				schemaNames = new SchemaNames(nameTable);
			}
			return schemaNames;
		}
	}

	public PositionInfo PositionInfo => positionInfo;

	public XmlResolver XmlResolver
	{
		get
		{
			return xmlResolver;
		}
		set
		{
			xmlResolver = value;
		}
	}

	public Uri BaseUri
	{
		get
		{
			return baseUri;
		}
		set
		{
			baseUri = value;
		}
	}

	public ValidationEventHandler EventHandler => (ValidationEventHandler)eventHandling.EventHandler;

	public SchemaInfo SchemaInfo
	{
		get
		{
			return schemaInfo;
		}
		set
		{
			schemaInfo = value;
		}
	}

	public IDtdInfo DtdInfo
	{
		get
		{
			return schemaInfo;
		}
		set
		{
			if (!(value is SchemaInfo schemaInfo))
			{
				throw new XmlException("An internal error has occurred.", string.Empty);
			}
			this.schemaInfo = schemaInfo;
		}
	}

	public virtual bool PreserveWhitespace => false;

	public BaseValidator(BaseValidator other)
	{
		reader = other.reader;
		schemaCollection = other.schemaCollection;
		eventHandling = other.eventHandling;
		nameTable = other.nameTable;
		schemaNames = other.schemaNames;
		positionInfo = other.positionInfo;
		xmlResolver = other.xmlResolver;
		baseUri = other.baseUri;
		elementName = other.elementName;
	}

	public BaseValidator(XmlValidatingReaderImpl reader, XmlSchemaCollection schemaCollection, IValidationEventHandling eventHandling)
	{
		this.reader = reader;
		this.schemaCollection = schemaCollection;
		this.eventHandling = eventHandling;
		nameTable = reader.NameTable;
		positionInfo = PositionInfo.GetPositionInfo(reader);
		elementName = new XmlQualifiedName();
	}

	public virtual void Validate()
	{
	}

	public virtual void CompleteValidation()
	{
	}

	public virtual object FindId(string name)
	{
		return null;
	}

	public void ValidateText()
	{
		if (!context.NeedValidateChildren)
		{
			return;
		}
		if (context.IsNill)
		{
			SendValidationEvent("Element '{0}' must have no character or element children.", XmlSchemaValidator.QNameString(context.LocalName, context.Namespace));
			return;
		}
		ContentValidator contentValidator = context.ElementDecl.ContentValidator;
		switch (contentValidator.ContentType)
		{
		case XmlSchemaContentType.ElementOnly:
		{
			ArrayList arrayList = contentValidator.ExpectedElements(context, isRequiredOnly: false);
			if (arrayList == null)
			{
				SendValidationEvent("The element {0} cannot contain text.", XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace));
				break;
			}
			SendValidationEvent("The element {0} cannot contain text. List of possible elements expected: {1}.", new string[2]
			{
				XmlSchemaValidator.BuildElementName(context.LocalName, context.Namespace),
				XmlSchemaValidator.PrintExpectedElements(arrayList, getParticles: false)
			});
			break;
		}
		case XmlSchemaContentType.Empty:
			SendValidationEvent("The element cannot contain text. Content model is empty.", string.Empty);
			break;
		}
		if (checkDatatype)
		{
			SaveTextValue(reader.Value);
		}
	}

	public void ValidateWhitespace()
	{
		if (context.NeedValidateChildren)
		{
			XmlSchemaContentType contentType = context.ElementDecl.ContentValidator.ContentType;
			if (context.IsNill)
			{
				SendValidationEvent("Element '{0}' must have no character or element children.", XmlSchemaValidator.QNameString(context.LocalName, context.Namespace));
			}
			if (contentType == XmlSchemaContentType.Empty)
			{
				SendValidationEvent("The element cannot contain white space. Content model is empty.", string.Empty);
			}
			if (checkDatatype)
			{
				SaveTextValue(reader.Value);
			}
		}
	}

	private void SaveTextValue(string value)
	{
		if (textString.Length == 0)
		{
			textString = value;
			return;
		}
		if (!hasSibling)
		{
			textValue.Append(textString);
			hasSibling = true;
		}
		textValue.Append(value);
	}

	protected void SendValidationEvent(string code)
	{
		SendValidationEvent(code, string.Empty);
	}

	protected void SendValidationEvent(string code, string[] args)
	{
		SendValidationEvent(new XmlSchemaException(code, args, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	protected void SendValidationEvent(string code, string arg)
	{
		SendValidationEvent(new XmlSchemaException(code, arg, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	protected void SendValidationEvent(string code, string arg1, string arg2)
	{
		SendValidationEvent(new XmlSchemaException(code, new string[2] { arg1, arg2 }, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition));
	}

	protected void SendValidationEvent(XmlSchemaException e)
	{
		SendValidationEvent(e, XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, string msg, XmlSeverityType severity)
	{
		SendValidationEvent(new XmlSchemaException(code, msg, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition), severity);
	}

	protected void SendValidationEvent(string code, string[] args, XmlSeverityType severity)
	{
		SendValidationEvent(new XmlSchemaException(code, args, reader.BaseURI, positionInfo.LineNumber, positionInfo.LinePosition), severity);
	}

	protected void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
	{
		if (eventHandling != null)
		{
			eventHandling.SendEvent(e, severity);
		}
		else if (severity == XmlSeverityType.Error)
		{
			throw e;
		}
	}

	protected static void ProcessEntity(SchemaInfo sinfo, string name, object sender, ValidationEventHandler eventhandler, string baseUri, int lineNumber, int linePosition)
	{
		XmlSchemaException ex = null;
		if (!sinfo.GeneralEntities.TryGetValue(new XmlQualifiedName(name), out var value))
		{
			ex = new XmlSchemaException("Reference to an undeclared entity, '{0}'.", name, baseUri, lineNumber, linePosition);
		}
		else if (value.NData.IsEmpty)
		{
			ex = new XmlSchemaException("Reference to an unparsed entity, '{0}'.", name, baseUri, lineNumber, linePosition);
		}
		if (ex != null)
		{
			if (eventhandler == null)
			{
				throw ex;
			}
			eventhandler(sender, new ValidationEventArgs(ex));
		}
	}

	protected static void ProcessEntity(SchemaInfo sinfo, string name, IValidationEventHandling eventHandling, string baseUriStr, int lineNumber, int linePosition)
	{
		string text = null;
		if (!sinfo.GeneralEntities.TryGetValue(new XmlQualifiedName(name), out var value))
		{
			text = "Reference to an undeclared entity, '{0}'.";
		}
		else if (value.NData.IsEmpty)
		{
			text = "Reference to an unparsed entity, '{0}'.";
		}
		if (text != null)
		{
			XmlSchemaException ex = new XmlSchemaException(text, name, baseUriStr, lineNumber, linePosition);
			if (eventHandling == null)
			{
				throw ex;
			}
			eventHandling.SendEvent(ex, XmlSeverityType.Error);
		}
	}

	public static BaseValidator CreateInstance(ValidationType valType, XmlValidatingReaderImpl reader, XmlSchemaCollection schemaCollection, IValidationEventHandling eventHandling, bool processIdentityConstraints)
	{
		return valType switch
		{
			ValidationType.XDR => new XdrValidator(reader, schemaCollection, eventHandling), 
			ValidationType.Schema => new XsdValidator(reader, schemaCollection, eventHandling), 
			ValidationType.DTD => new DtdValidator(reader, eventHandling, processIdentityConstraints), 
			ValidationType.Auto => new AutoValidator(reader, schemaCollection, eventHandling), 
			ValidationType.None => new BaseValidator(reader, schemaCollection, eventHandling), 
			_ => null, 
		};
	}
}
