using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Xml.XmlConfiguration;

namespace System.Xml.Schema;

[XmlRoot("schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
public class XmlSchema : XmlSchemaObject
{
	public const string Namespace = "http://www.w3.org/2001/XMLSchema";

	public const string InstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

	private XmlSchemaForm attributeFormDefault;

	private XmlSchemaForm elementFormDefault;

	private XmlSchemaDerivationMethod blockDefault = XmlSchemaDerivationMethod.None;

	private XmlSchemaDerivationMethod finalDefault = XmlSchemaDerivationMethod.None;

	private string targetNs;

	private string version;

	private XmlSchemaObjectCollection includes = new XmlSchemaObjectCollection();

	private XmlSchemaObjectCollection items = new XmlSchemaObjectCollection();

	private string id;

	private XmlAttribute[] moreAttributes;

	private bool isCompiled;

	private bool isCompiledBySet;

	private bool isPreprocessed;

	private bool isRedefined;

	private int errorCount;

	private XmlSchemaObjectTable attributes;

	private XmlSchemaObjectTable attributeGroups = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable elements = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable types = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable groups = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable notations = new XmlSchemaObjectTable();

	private XmlSchemaObjectTable identityConstraints = new XmlSchemaObjectTable();

	private static int globalIdCounter = -1;

	private ArrayList importedSchemas;

	private ArrayList importedNamespaces;

	private int schemaId = -1;

	private Uri baseUri;

	private bool isChameleon;

	private Hashtable ids = new Hashtable();

	private XmlDocument document;

	private XmlNameTable nameTable;

	[XmlAttribute("attributeFormDefault")]
	[DefaultValue(XmlSchemaForm.None)]
	public XmlSchemaForm AttributeFormDefault
	{
		get
		{
			return attributeFormDefault;
		}
		set
		{
			attributeFormDefault = value;
		}
	}

	[XmlAttribute("blockDefault")]
	[DefaultValue(XmlSchemaDerivationMethod.None)]
	public XmlSchemaDerivationMethod BlockDefault
	{
		get
		{
			return blockDefault;
		}
		set
		{
			blockDefault = value;
		}
	}

	[XmlAttribute("finalDefault")]
	[DefaultValue(XmlSchemaDerivationMethod.None)]
	public XmlSchemaDerivationMethod FinalDefault
	{
		get
		{
			return finalDefault;
		}
		set
		{
			finalDefault = value;
		}
	}

	[XmlAttribute("elementFormDefault")]
	[DefaultValue(XmlSchemaForm.None)]
	public XmlSchemaForm ElementFormDefault
	{
		get
		{
			return elementFormDefault;
		}
		set
		{
			elementFormDefault = value;
		}
	}

	[XmlAttribute("targetNamespace", DataType = "anyURI")]
	public string TargetNamespace
	{
		get
		{
			return targetNs;
		}
		set
		{
			targetNs = value;
		}
	}

	[XmlAttribute("version", DataType = "token")]
	public string Version
	{
		get
		{
			return version;
		}
		set
		{
			version = value;
		}
	}

	[XmlElement("include", typeof(XmlSchemaInclude))]
	[XmlElement("import", typeof(XmlSchemaImport))]
	[XmlElement("redefine", typeof(XmlSchemaRedefine))]
	public XmlSchemaObjectCollection Includes => includes;

	[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
	[XmlElement("attribute", typeof(XmlSchemaAttribute))]
	[XmlElement("attributeGroup", typeof(XmlSchemaAttributeGroup))]
	[XmlElement("complexType", typeof(XmlSchemaComplexType))]
	[XmlElement("simpleType", typeof(XmlSchemaSimpleType))]
	[XmlElement("element", typeof(XmlSchemaElement))]
	[XmlElement("group", typeof(XmlSchemaGroup))]
	[XmlElement("notation", typeof(XmlSchemaNotation))]
	public XmlSchemaObjectCollection Items => items;

	[XmlIgnore]
	public bool IsCompiled
	{
		get
		{
			if (!isCompiled)
			{
				return isCompiledBySet;
			}
			return true;
		}
	}

	[XmlIgnore]
	internal bool IsCompiledBySet
	{
		get
		{
			return isCompiledBySet;
		}
		set
		{
			isCompiledBySet = value;
		}
	}

	[XmlIgnore]
	internal bool IsPreprocessed
	{
		get
		{
			return isPreprocessed;
		}
		set
		{
			isPreprocessed = value;
		}
	}

	[XmlIgnore]
	internal bool IsRedefined
	{
		get
		{
			return isRedefined;
		}
		set
		{
			isRedefined = value;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable Attributes
	{
		get
		{
			if (attributes == null)
			{
				attributes = new XmlSchemaObjectTable();
			}
			return attributes;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable AttributeGroups
	{
		get
		{
			if (attributeGroups == null)
			{
				attributeGroups = new XmlSchemaObjectTable();
			}
			return attributeGroups;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable SchemaTypes
	{
		get
		{
			if (types == null)
			{
				types = new XmlSchemaObjectTable();
			}
			return types;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable Elements
	{
		get
		{
			if (elements == null)
			{
				elements = new XmlSchemaObjectTable();
			}
			return elements;
		}
	}

	[XmlAttribute("id", DataType = "ID")]
	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	[XmlAnyAttribute]
	public XmlAttribute[] UnhandledAttributes
	{
		get
		{
			return moreAttributes;
		}
		set
		{
			moreAttributes = value;
		}
	}

	[XmlIgnore]
	public XmlSchemaObjectTable Groups => groups;

	[XmlIgnore]
	public XmlSchemaObjectTable Notations => notations;

	[XmlIgnore]
	internal XmlSchemaObjectTable IdentityConstraints => identityConstraints;

	[XmlIgnore]
	internal Uri BaseUri
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

	[XmlIgnore]
	internal int SchemaId
	{
		get
		{
			if (schemaId == -1)
			{
				schemaId = Interlocked.Increment(ref globalIdCounter);
			}
			return schemaId;
		}
	}

	[XmlIgnore]
	internal bool IsChameleon
	{
		get
		{
			return isChameleon;
		}
		set
		{
			isChameleon = value;
		}
	}

	[XmlIgnore]
	internal Hashtable Ids => ids;

	[XmlIgnore]
	internal XmlDocument Document
	{
		get
		{
			if (document == null)
			{
				document = new XmlDocument();
			}
			return document;
		}
	}

	[XmlIgnore]
	internal int ErrorCount
	{
		get
		{
			return errorCount;
		}
		set
		{
			errorCount = value;
		}
	}

	[XmlIgnore]
	internal override string IdAttribute
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	internal XmlNameTable NameTable
	{
		get
		{
			if (nameTable == null)
			{
				nameTable = new NameTable();
			}
			return nameTable;
		}
	}

	internal ArrayList ImportedSchemas
	{
		get
		{
			if (importedSchemas == null)
			{
				importedSchemas = new ArrayList();
			}
			return importedSchemas;
		}
	}

	internal ArrayList ImportedNamespaces
	{
		get
		{
			if (importedNamespaces == null)
			{
				importedNamespaces = new ArrayList();
			}
			return importedNamespaces;
		}
	}

	public static XmlSchema Read(TextReader reader, ValidationEventHandler validationEventHandler)
	{
		return Read(new XmlTextReader(reader), validationEventHandler);
	}

	public static XmlSchema Read(Stream stream, ValidationEventHandler validationEventHandler)
	{
		return Read(new XmlTextReader(stream), validationEventHandler);
	}

	public static XmlSchema Read(XmlReader reader, ValidationEventHandler validationEventHandler)
	{
		XmlNameTable xmlNameTable = reader.NameTable;
		Parser parser = new Parser(SchemaType.XSD, xmlNameTable, new SchemaNames(xmlNameTable), validationEventHandler);
		try
		{
			parser.Parse(reader, null);
		}
		catch (XmlSchemaException ex)
		{
			if (validationEventHandler != null)
			{
				validationEventHandler(null, new ValidationEventArgs(ex));
				return null;
			}
			throw ex;
		}
		return parser.XmlSchema;
	}

	public void Write(Stream stream)
	{
		Write(stream, null);
	}

	public void Write(Stream stream, XmlNamespaceManager namespaceManager)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, null);
		xmlTextWriter.Formatting = Formatting.Indented;
		Write(xmlTextWriter, namespaceManager);
	}

	public void Write(TextWriter writer)
	{
		Write(writer, null);
	}

	public void Write(TextWriter writer, XmlNamespaceManager namespaceManager)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(writer);
		xmlTextWriter.Formatting = Formatting.Indented;
		Write(xmlTextWriter, namespaceManager);
	}

	public void Write(XmlWriter writer)
	{
		Write(writer, null);
	}

	public void Write(XmlWriter writer, XmlNamespaceManager namespaceManager)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlSchema));
		XmlSerializerNamespaces xmlSerializerNamespaces;
		if (namespaceManager != null)
		{
			xmlSerializerNamespaces = new XmlSerializerNamespaces();
			bool flag = false;
			if (base.Namespaces != null)
			{
				flag = base.Namespaces.Namespaces["xs"] != null || base.Namespaces.Namespaces.ContainsValue("http://www.w3.org/2001/XMLSchema");
			}
			if (!flag && namespaceManager.LookupPrefix("http://www.w3.org/2001/XMLSchema") == null && namespaceManager.LookupNamespace("xs") == null)
			{
				xmlSerializerNamespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
			}
			foreach (string item in namespaceManager)
			{
				if (item != "xml" && item != "xmlns")
				{
					xmlSerializerNamespaces.Add(item, namespaceManager.LookupNamespace(item));
				}
			}
		}
		else if (base.Namespaces != null && base.Namespaces.Count > 0)
		{
			Hashtable hashtable = base.Namespaces.Namespaces;
			if (hashtable["xs"] == null && !hashtable.ContainsValue("http://www.w3.org/2001/XMLSchema"))
			{
				hashtable.Add("xs", "http://www.w3.org/2001/XMLSchema");
			}
			xmlSerializerNamespaces = base.Namespaces;
		}
		else
		{
			xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("xs", "http://www.w3.org/2001/XMLSchema");
			if (targetNs != null && targetNs.Length != 0)
			{
				xmlSerializerNamespaces.Add("tns", targetNs);
			}
		}
		xmlSerializer.Serialize(writer, this, xmlSerializerNamespaces);
	}

	[Obsolete("Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation. http://go.microsoft.com/fwlink/?linkid=14202")]
	public void Compile(ValidationEventHandler validationEventHandler)
	{
		SchemaInfo schemaInfo = new SchemaInfo();
		schemaInfo.SchemaType = SchemaType.XSD;
		CompileSchema(null, XmlReaderSection.CreateDefaultResolver(), schemaInfo, null, validationEventHandler, NameTable, CompileContentModel: false);
	}

	[Obsolete("Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation. http://go.microsoft.com/fwlink/?linkid=14202")]
	public void Compile(ValidationEventHandler validationEventHandler, XmlResolver resolver)
	{
		SchemaInfo schemaInfo = new SchemaInfo();
		schemaInfo.SchemaType = SchemaType.XSD;
		CompileSchema(null, resolver, schemaInfo, null, validationEventHandler, NameTable, CompileContentModel: false);
	}

	internal bool CompileSchema(XmlSchemaCollection xsc, XmlResolver resolver, SchemaInfo schemaInfo, string ns, ValidationEventHandler validationEventHandler, XmlNameTable nameTable, bool CompileContentModel)
	{
		lock (this)
		{
			if (!new SchemaCollectionPreprocessor(nameTable, null, validationEventHandler)
			{
				XmlResolver = resolver
			}.Execute(this, ns, loadExternals: true, xsc))
			{
				return false;
			}
			SchemaCollectionCompiler schemaCollectionCompiler = new SchemaCollectionCompiler(nameTable, validationEventHandler);
			isCompiled = schemaCollectionCompiler.Execute(this, schemaInfo, CompileContentModel);
			SetIsCompiled(isCompiled);
			return isCompiled;
		}
	}

	internal void CompileSchemaInSet(XmlNameTable nameTable, ValidationEventHandler eventHandler, XmlSchemaCompilationSettings compilationSettings)
	{
		Compiler compiler = new Compiler(nameTable, eventHandler, null, compilationSettings);
		compiler.Prepare(this, cleanup: true);
		isCompiledBySet = compiler.Compile();
	}

	internal new XmlSchema Clone()
	{
		XmlSchema obj = new XmlSchema
		{
			attributeFormDefault = attributeFormDefault,
			elementFormDefault = elementFormDefault,
			blockDefault = blockDefault,
			finalDefault = finalDefault,
			targetNs = targetNs,
			version = version,
			includes = includes,
			Namespaces = base.Namespaces,
			items = items,
			BaseUri = BaseUri
		};
		SchemaCollectionCompiler.Cleanup(obj);
		return obj;
	}

	internal XmlSchema DeepClone()
	{
		XmlSchema xmlSchema = new XmlSchema();
		xmlSchema.attributeFormDefault = attributeFormDefault;
		xmlSchema.elementFormDefault = elementFormDefault;
		xmlSchema.blockDefault = blockDefault;
		xmlSchema.finalDefault = finalDefault;
		xmlSchema.targetNs = targetNs;
		xmlSchema.version = version;
		xmlSchema.isPreprocessed = isPreprocessed;
		for (int i = 0; i < items.Count; i++)
		{
			XmlSchemaObject item = ((items[i] is XmlSchemaComplexType xmlSchemaComplexType) ? xmlSchemaComplexType.Clone(this) : ((items[i] is XmlSchemaElement xmlSchemaElement) ? xmlSchemaElement.Clone(this) : ((!(items[i] is XmlSchemaGroup xmlSchemaGroup)) ? items[i].Clone() : xmlSchemaGroup.Clone(this))));
			xmlSchema.Items.Add(item);
		}
		for (int j = 0; j < includes.Count; j++)
		{
			XmlSchemaExternal item2 = (XmlSchemaExternal)includes[j].Clone();
			xmlSchema.Includes.Add(item2);
		}
		xmlSchema.Namespaces = base.Namespaces;
		xmlSchema.BaseUri = BaseUri;
		return xmlSchema;
	}

	internal void SetIsCompiled(bool isCompiled)
	{
		this.isCompiled = isCompiled;
	}

	internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
	{
		this.moreAttributes = moreAttributes;
	}

	internal override void AddAnnotation(XmlSchemaAnnotation annotation)
	{
		items.Add(annotation);
	}

	internal void GetExternalSchemasList(IList extList, XmlSchema schema)
	{
		if (extList.Contains(schema))
		{
			return;
		}
		extList.Add(schema);
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			if (xmlSchemaExternal.Schema != null)
			{
				GetExternalSchemasList(extList, xmlSchemaExternal.Schema);
			}
		}
	}
}
