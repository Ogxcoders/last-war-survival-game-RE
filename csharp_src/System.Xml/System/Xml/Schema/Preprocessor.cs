using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace System.Xml.Schema;

internal sealed class Preprocessor : BaseProcessor
{
	private string Xmlns;

	private string NsXsi;

	private string targetNamespace;

	private XmlSchema rootSchema;

	private XmlSchema currentSchema;

	private XmlSchemaForm elementFormDefault;

	private XmlSchemaForm attributeFormDefault;

	private XmlSchemaDerivationMethod blockDefault;

	private XmlSchemaDerivationMethod finalDefault;

	private Hashtable schemaLocations;

	private Hashtable chameleonSchemas;

	private Hashtable referenceNamespaces;

	private Hashtable processedExternals;

	private SortedList lockList;

	private XmlReaderSettings readerSettings;

	private XmlSchema rootSchemaForRedefine;

	private ArrayList redefinedList;

	private static XmlSchema builtInSchemaForXmlNS;

	private const XmlSchemaDerivationMethod schemaBlockDefaultAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod schemaFinalDefaultAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

	private const XmlSchemaDerivationMethod elementBlockAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod elementFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod simpleTypeFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

	private const XmlSchemaDerivationMethod complexTypeBlockAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod complexTypeFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private XmlResolver xmlResolver;

	internal XmlResolver XmlResolver
	{
		set
		{
			xmlResolver = value;
		}
	}

	internal XmlReaderSettings ReaderSettings
	{
		get
		{
			if (readerSettings == null)
			{
				readerSettings = new XmlReaderSettings();
				readerSettings.DtdProcessing = DtdProcessing.Prohibit;
			}
			return readerSettings;
		}
		set
		{
			readerSettings = value;
		}
	}

	internal Hashtable SchemaLocations
	{
		set
		{
			schemaLocations = value;
		}
	}

	internal Hashtable ChameleonSchemas
	{
		set
		{
			chameleonSchemas = value;
		}
	}

	internal XmlSchema RootSchema => rootSchema;

	public Preprocessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
		: this(nameTable, schemaNames, eventHandler, new XmlSchemaCompilationSettings())
	{
	}

	public Preprocessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler, XmlSchemaCompilationSettings compilationSettings)
		: base(nameTable, schemaNames, eventHandler, compilationSettings)
	{
		referenceNamespaces = new Hashtable();
		processedExternals = new Hashtable();
		lockList = new SortedList();
	}

	public bool Execute(XmlSchema schema, string targetNamespace, bool loadExternals)
	{
		rootSchema = schema;
		Xmlns = base.NameTable.Add("xmlns");
		NsXsi = base.NameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
		rootSchema.ImportedSchemas.Clear();
		rootSchema.ImportedNamespaces.Clear();
		if (rootSchema.BaseUri != null && schemaLocations[rootSchema.BaseUri] == null)
		{
			schemaLocations.Add(rootSchema.BaseUri, rootSchema);
		}
		if (rootSchema.TargetNamespace != null)
		{
			if (targetNamespace == null)
			{
				targetNamespace = rootSchema.TargetNamespace;
			}
			else if (targetNamespace != rootSchema.TargetNamespace)
			{
				SendValidationEvent("The targetNamespace parameter '{0}' should be the same value as the targetNamespace '{1}' of the schema.", targetNamespace, rootSchema.TargetNamespace, rootSchema);
			}
		}
		else if (targetNamespace != null && targetNamespace.Length != 0)
		{
			rootSchema = GetChameleonSchema(targetNamespace, rootSchema);
		}
		if (loadExternals && xmlResolver != null)
		{
			LoadExternals(rootSchema);
		}
		BuildSchemaList(rootSchema);
		int i = 0;
		try
		{
			for (i = 0; i < lockList.Count; i++)
			{
				XmlSchema xmlSchema = (XmlSchema)lockList.GetByIndex(i);
				Monitor.Enter(xmlSchema);
				xmlSchema.IsProcessing = false;
			}
			rootSchemaForRedefine = rootSchema;
			Preprocess(rootSchema, targetNamespace, rootSchema.ImportedSchemas);
			if (redefinedList != null)
			{
				for (int j = 0; j < redefinedList.Count; j++)
				{
					PreprocessRedefine((RedefineEntry)redefinedList[j]);
				}
			}
		}
		finally
		{
			if (i == lockList.Count)
			{
				i--;
			}
			while (i >= 0)
			{
				XmlSchema xmlSchema = (XmlSchema)lockList.GetByIndex(i);
				xmlSchema.IsProcessing = false;
				if (xmlSchema == GetBuildInSchema())
				{
					Monitor.Exit(xmlSchema);
				}
				else
				{
					xmlSchema.IsCompiledBySet = false;
					xmlSchema.IsPreprocessed = !base.HasErrors;
					Monitor.Exit(xmlSchema);
				}
				i--;
			}
		}
		rootSchema.IsPreprocessed = !base.HasErrors;
		return !base.HasErrors;
	}

	private void Cleanup(XmlSchema schema)
	{
		if (schema != GetBuildInSchema())
		{
			schema.Attributes.Clear();
			schema.AttributeGroups.Clear();
			schema.SchemaTypes.Clear();
			schema.Elements.Clear();
			schema.Groups.Clear();
			schema.Notations.Clear();
			schema.Ids.Clear();
			schema.IdentityConstraints.Clear();
			schema.IsRedefined = false;
			schema.IsCompiledBySet = false;
		}
	}

	private void CleanupRedefine(XmlSchemaExternal include)
	{
		XmlSchemaRedefine obj = include as XmlSchemaRedefine;
		obj.AttributeGroups.Clear();
		obj.Groups.Clear();
		obj.SchemaTypes.Clear();
	}

	private void BuildSchemaList(XmlSchema schema)
	{
		if (lockList.Contains(schema.SchemaId))
		{
			return;
		}
		lockList.Add(schema.SchemaId, schema);
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			if (xmlSchemaExternal.Schema != null)
			{
				BuildSchemaList(xmlSchemaExternal.Schema);
			}
		}
	}

	private void LoadExternals(XmlSchema schema)
	{
		if (schema.IsProcessing)
		{
			return;
		}
		schema.IsProcessing = true;
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			Uri uri = null;
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			XmlSchema schema2 = xmlSchemaExternal.Schema;
			if (schema2 != null)
			{
				uri = schema2.BaseUri;
				if (uri != null && schemaLocations[uri] == null)
				{
					schemaLocations.Add(uri, schema2);
				}
				LoadExternals(schema2);
				continue;
			}
			string schemaLocation = xmlSchemaExternal.SchemaLocation;
			Uri uri2 = null;
			Exception innerException = null;
			if (schemaLocation != null)
			{
				try
				{
					uri2 = ResolveSchemaLocationUri(schema, schemaLocation);
				}
				catch (Exception ex)
				{
					uri2 = null;
					innerException = ex;
				}
			}
			if (xmlSchemaExternal.Compositor == Compositor.Import)
			{
				XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
				string text = ((xmlSchemaImport.Namespace != null) ? xmlSchemaImport.Namespace : string.Empty);
				if (!schema.ImportedNamespaces.Contains(text))
				{
					schema.ImportedNamespaces.Add(text);
				}
				if (text == "http://www.w3.org/XML/1998/namespace" && uri2 == null)
				{
					xmlSchemaExternal.Schema = GetBuildInSchema();
					continue;
				}
			}
			if (uri2 == null)
			{
				if (schemaLocation != null)
				{
					SendValidationEvent(new XmlSchemaException("Cannot resolve the 'schemaLocation' attribute.", null, innerException, xmlSchemaExternal.SourceUri, xmlSchemaExternal.LineNumber, xmlSchemaExternal.LinePosition, xmlSchemaExternal), XmlSeverityType.Warning);
				}
			}
			else if (schemaLocations[uri2] == null)
			{
				object obj = null;
				try
				{
					obj = GetSchemaEntity(uri2);
				}
				catch (Exception ex2)
				{
					innerException = ex2;
					obj = null;
				}
				if (obj != null)
				{
					xmlSchemaExternal.BaseUri = uri2;
					Type type = obj.GetType();
					if (typeof(XmlSchema).IsAssignableFrom(type))
					{
						xmlSchemaExternal.Schema = (XmlSchema)obj;
						schemaLocations.Add(uri2, xmlSchemaExternal.Schema);
						LoadExternals(xmlSchemaExternal.Schema);
						continue;
					}
					XmlReader xmlReader = null;
					if (type.IsSubclassOf(typeof(Stream)))
					{
						readerSettings.CloseInput = true;
						readerSettings.XmlResolver = xmlResolver;
						xmlReader = XmlReader.Create((Stream)obj, readerSettings, uri2.ToString());
					}
					else if (type.IsSubclassOf(typeof(XmlReader)))
					{
						xmlReader = (XmlReader)obj;
					}
					else if (type.IsSubclassOf(typeof(TextReader)))
					{
						readerSettings.CloseInput = true;
						readerSettings.XmlResolver = xmlResolver;
						xmlReader = XmlReader.Create((TextReader)obj, readerSettings, uri2.ToString());
					}
					if (xmlReader == null)
					{
						SendValidationEvent("Cannot resolve the 'schemaLocation' attribute.", xmlSchemaExternal, XmlSeverityType.Warning);
						continue;
					}
					try
					{
						Parser parser = new Parser(SchemaType.XSD, base.NameTable, base.SchemaNames, base.EventHandler);
						parser.Parse(xmlReader, null);
						while (xmlReader.Read())
						{
						}
						schema2 = (xmlSchemaExternal.Schema = parser.XmlSchema);
						schemaLocations.Add(uri2, schema2);
						LoadExternals(schema2);
					}
					catch (XmlSchemaException ex3)
					{
						SendValidationEvent("Cannot load the schema from the location '{0}' - {1}", schemaLocation, ex3.Message, ex3.SourceUri, ex3.LineNumber, ex3.LinePosition);
					}
					catch (Exception innerException2)
					{
						SendValidationEvent(new XmlSchemaException("Cannot resolve the 'schemaLocation' attribute.", null, innerException2, xmlSchemaExternal.SourceUri, xmlSchemaExternal.LineNumber, xmlSchemaExternal.LinePosition, xmlSchemaExternal), XmlSeverityType.Warning);
					}
					finally
					{
						xmlReader.Close();
					}
				}
				else
				{
					SendValidationEvent(new XmlSchemaException("Cannot resolve the 'schemaLocation' attribute.", null, innerException, xmlSchemaExternal.SourceUri, xmlSchemaExternal.LineNumber, xmlSchemaExternal.LinePosition, xmlSchemaExternal), XmlSeverityType.Warning);
				}
			}
			else
			{
				xmlSchemaExternal.Schema = (XmlSchema)schemaLocations[uri2];
			}
		}
	}

	internal static XmlSchema GetBuildInSchema()
	{
		if (builtInSchemaForXmlNS == null)
		{
			XmlSchema xmlSchema = new XmlSchema();
			xmlSchema.TargetNamespace = "http://www.w3.org/XML/1998/namespace";
			xmlSchema.Namespaces.Add("xml", "http://www.w3.org/XML/1998/namespace");
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "lang";
			xmlSchemaAttribute.SchemaTypeName = new XmlQualifiedName("language", "http://www.w3.org/2001/XMLSchema");
			xmlSchema.Items.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "base";
			xmlSchemaAttribute2.SchemaTypeName = new XmlQualifiedName("anyURI", "http://www.w3.org/2001/XMLSchema");
			xmlSchema.Items.Add(xmlSchemaAttribute2);
			XmlSchemaAttribute xmlSchemaAttribute3 = new XmlSchemaAttribute();
			xmlSchemaAttribute3.Name = "space";
			XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
			xmlSchemaSimpleTypeRestriction.BaseTypeName = new XmlQualifiedName("NCName", "http://www.w3.org/2001/XMLSchema");
			XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = new XmlSchemaEnumerationFacet();
			xmlSchemaEnumerationFacet.Value = "default";
			xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaEnumerationFacet);
			XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet2 = new XmlSchemaEnumerationFacet();
			xmlSchemaEnumerationFacet2.Value = "preserve";
			xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaEnumerationFacet2);
			xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeRestriction;
			xmlSchemaAttribute3.SchemaType = xmlSchemaSimpleType;
			xmlSchemaAttribute3.DefaultValue = "preserve";
			xmlSchema.Items.Add(xmlSchemaAttribute3);
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup = new XmlSchemaAttributeGroup();
			xmlSchemaAttributeGroup.Name = "specialAttrs";
			XmlSchemaAttribute xmlSchemaAttribute4 = new XmlSchemaAttribute();
			xmlSchemaAttribute4.RefName = new XmlQualifiedName("lang", "http://www.w3.org/XML/1998/namespace");
			xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute4);
			XmlSchemaAttribute xmlSchemaAttribute5 = new XmlSchemaAttribute();
			xmlSchemaAttribute5.RefName = new XmlQualifiedName("space", "http://www.w3.org/XML/1998/namespace");
			xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute5);
			XmlSchemaAttribute xmlSchemaAttribute6 = new XmlSchemaAttribute();
			xmlSchemaAttribute6.RefName = new XmlQualifiedName("base", "http://www.w3.org/XML/1998/namespace");
			xmlSchemaAttributeGroup.Attributes.Add(xmlSchemaAttribute6);
			xmlSchema.Items.Add(xmlSchemaAttributeGroup);
			xmlSchema.IsPreprocessed = true;
			xmlSchema.CompileSchemaInSet(new NameTable(), null, null);
			Interlocked.CompareExchange(ref builtInSchemaForXmlNS, xmlSchema, null);
		}
		return builtInSchemaForXmlNS;
	}

	private void BuildRefNamespaces(XmlSchema schema)
	{
		referenceNamespaces.Clear();
		referenceNamespaces.Add("http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			if (xmlSchemaExternal is XmlSchemaImport)
			{
				string text = (xmlSchemaExternal as XmlSchemaImport).Namespace;
				if (text == null)
				{
					text = string.Empty;
				}
				if (referenceNamespaces[text] == null)
				{
					referenceNamespaces.Add(text, text);
				}
			}
		}
		string empty = schema.TargetNamespace;
		if (empty == null)
		{
			empty = string.Empty;
		}
		if (referenceNamespaces[empty] == null)
		{
			referenceNamespaces.Add(empty, empty);
		}
	}

	private void ParseUri(string uri, string code, XmlSchemaObject sourceSchemaObject)
	{
		try
		{
			XmlConvert.ToUri(uri);
		}
		catch (FormatException innerException)
		{
			SendValidationEvent(code, new string[1] { uri }, innerException, sourceSchemaObject);
		}
	}

	private void Preprocess(XmlSchema schema, string targetNamespace, ArrayList imports)
	{
		XmlSchema xmlSchema = null;
		if (schema.IsProcessing)
		{
			return;
		}
		schema.IsProcessing = true;
		string text = schema.TargetNamespace;
		if (text != null)
		{
			text = (schema.TargetNamespace = base.NameTable.Add(text));
			if (text.Length == 0)
			{
				SendValidationEvent("The targetNamespace attribute cannot have empty string as its value.", schema);
			}
			else
			{
				ParseUri(text, "The Namespace '{0}' is an invalid URI.", schema);
			}
		}
		if (schema.Version != null)
		{
			XmlSchemaDatatype datatype = DatatypeImplementation.GetSimpleTypeFromTypeCode(XmlTypeCode.Token).Datatype;
			object typedValue;
			Exception ex = datatype.TryParseValue(schema.Version, null, null, out typedValue);
			if (ex != null)
			{
				SendValidationEvent("The '{0}' attribute is invalid - The value '{1}' is invalid according to its datatype '{2}' - {3}", new string[4] { "version", schema.Version, datatype.TypeCodeString, ex.Message }, ex, schema);
			}
			else
			{
				schema.Version = (string)typedValue;
			}
		}
		Cleanup(schema);
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			XmlSchema xmlSchema2 = xmlSchemaExternal.Schema;
			SetParent(xmlSchemaExternal, schema);
			PreprocessAnnotation(xmlSchemaExternal);
			string schemaLocation = xmlSchemaExternal.SchemaLocation;
			if (schemaLocation != null)
			{
				ParseUri(schemaLocation, "The SchemaLocation '{0}' is an invalid URI.", xmlSchemaExternal);
			}
			else if ((xmlSchemaExternal.Compositor == Compositor.Include || xmlSchemaExternal.Compositor == Compositor.Redefine) && xmlSchema2 == null)
			{
				SendValidationEvent("The required attribute '{0}' is missing.", "schemaLocation", xmlSchemaExternal);
			}
			switch (xmlSchemaExternal.Compositor)
			{
			case Compositor.Import:
			{
				XmlSchemaImport xmlSchemaImport = xmlSchemaExternal as XmlSchemaImport;
				string text3 = xmlSchemaImport.Namespace;
				if (text3 == schema.TargetNamespace)
				{
					SendValidationEvent("Namespace attribute of an import must not match the real value of the enclosing targetNamespace of the <schema>.", xmlSchemaExternal);
				}
				if (xmlSchema2 != null)
				{
					if (text3 != xmlSchema2.TargetNamespace)
					{
						SendValidationEvent("The namespace attribute '{0}' of an import should be the same value as the targetNamespace '{1}' of the imported schema.", text3, xmlSchema2.TargetNamespace, xmlSchemaImport);
					}
					xmlSchema = rootSchemaForRedefine;
					rootSchemaForRedefine = xmlSchema2;
					Preprocess(xmlSchema2, text3, imports);
					rootSchemaForRedefine = xmlSchema;
				}
				else if (text3 != null)
				{
					if (text3.Length == 0)
					{
						SendValidationEvent("The namespace attribute cannot have empty string as its value.", text3, xmlSchemaExternal);
					}
					else
					{
						ParseUri(text3, "The Namespace '{0}' is an invalid URI.", xmlSchemaExternal);
					}
				}
				continue;
			}
			case Compositor.Include:
				if (xmlSchemaExternal.Schema == null)
				{
					continue;
				}
				break;
			case Compositor.Redefine:
				if (xmlSchema2 == null)
				{
					continue;
				}
				CleanupRedefine(xmlSchemaExternal);
				break;
			}
			if (xmlSchema2.TargetNamespace != null)
			{
				if (schema.TargetNamespace != xmlSchema2.TargetNamespace)
				{
					SendValidationEvent("The targetNamespace '{0}' of included/redefined schema should be the same as the targetNamespace '{1}' of the including schema.", xmlSchema2.TargetNamespace, schema.TargetNamespace, xmlSchemaExternal);
				}
			}
			else if (targetNamespace != null && targetNamespace.Length != 0)
			{
				xmlSchema2 = (xmlSchemaExternal.Schema = GetChameleonSchema(targetNamespace, xmlSchema2));
			}
			Preprocess(xmlSchema2, schema.TargetNamespace, imports);
		}
		currentSchema = schema;
		BuildRefNamespaces(schema);
		ValidateIdAttribute(schema);
		this.targetNamespace = ((targetNamespace == null) ? string.Empty : targetNamespace);
		SetSchemaDefaults(schema);
		processedExternals.Clear();
		for (int j = 0; j < schema.Includes.Count; j++)
		{
			XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)schema.Includes[j];
			XmlSchema schema2 = xmlSchemaExternal2.Schema;
			if (schema2 != null)
			{
				switch (xmlSchemaExternal2.Compositor)
				{
				case Compositor.Include:
					if (processedExternals[schema2] != null)
					{
						continue;
					}
					processedExternals.Add(schema2, xmlSchemaExternal2);
					CopyIncludedComponents(schema2, schema);
					break;
				case Compositor.Redefine:
					if (redefinedList == null)
					{
						redefinedList = new ArrayList();
					}
					redefinedList.Add(new RedefineEntry(xmlSchemaExternal2 as XmlSchemaRedefine, rootSchemaForRedefine));
					if (processedExternals[schema2] != null)
					{
						continue;
					}
					processedExternals.Add(schema2, xmlSchemaExternal2);
					CopyIncludedComponents(schema2, schema);
					break;
				case Compositor.Import:
					if (schema2 != rootSchema)
					{
						XmlSchemaImport xmlSchemaImport2 = xmlSchemaExternal2 as XmlSchemaImport;
						string text4 = ((xmlSchemaImport2.Namespace != null) ? xmlSchemaImport2.Namespace : string.Empty);
						if (!imports.Contains(schema2))
						{
							imports.Add(schema2);
						}
						if (!rootSchema.ImportedNamespaces.Contains(text4))
						{
							rootSchema.ImportedNamespaces.Add(text4);
						}
					}
					break;
				}
			}
			else if (xmlSchemaExternal2.Compositor == Compositor.Redefine)
			{
				XmlSchemaRedefine xmlSchemaRedefine = xmlSchemaExternal2 as XmlSchemaRedefine;
				if (xmlSchemaRedefine.BaseUri == null)
				{
					for (int k = 0; k < xmlSchemaRedefine.Items.Count; k++)
					{
						if (!(xmlSchemaRedefine.Items[k] is XmlSchemaAnnotation))
						{
							SendValidationEvent("'SchemaLocation' must successfully resolve if <redefine> contains any child other than <annotation>.", xmlSchemaRedefine);
							break;
						}
					}
				}
			}
			ValidateIdAttribute(xmlSchemaExternal2);
		}
		List<XmlSchemaObject> list = new List<XmlSchemaObject>();
		XmlSchemaObjectCollection items = schema.Items;
		for (int l = 0; l < items.Count; l++)
		{
			SetParent(items[l], schema);
			if (items[l] is XmlSchemaAttribute xmlSchemaAttribute)
			{
				PreprocessAttribute(xmlSchemaAttribute);
				AddToTable(schema.Attributes, xmlSchemaAttribute.QualifiedName, xmlSchemaAttribute);
			}
			else if (items[l] is XmlSchemaAttributeGroup)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)items[l];
				PreprocessAttributeGroup(xmlSchemaAttributeGroup);
				AddToTable(schema.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
			}
			else if (items[l] is XmlSchemaComplexType)
			{
				XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)items[l];
				PreprocessComplexType(xmlSchemaComplexType, local: false);
				AddToTable(schema.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
			}
			else if (items[l] is XmlSchemaSimpleType)
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)items[l];
				PreprocessSimpleType(xmlSchemaSimpleType, local: false);
				AddToTable(schema.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
			}
			else if (items[l] is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)items[l];
				PreprocessElement(xmlSchemaElement);
				AddToTable(schema.Elements, xmlSchemaElement.QualifiedName, xmlSchemaElement);
			}
			else if (items[l] is XmlSchemaGroup)
			{
				XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup)items[l];
				PreprocessGroup(xmlSchemaGroup);
				AddToTable(schema.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
			}
			else if (items[l] is XmlSchemaNotation)
			{
				XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation)items[l];
				PreprocessNotation(xmlSchemaNotation);
				AddToTable(schema.Notations, xmlSchemaNotation.QualifiedName, xmlSchemaNotation);
			}
			else if (items[l] is XmlSchemaAnnotation)
			{
				PreprocessAnnotation(items[l] as XmlSchemaAnnotation);
			}
			else
			{
				SendValidationEvent("The schema items collection cannot contain an object of type 'XmlSchemaInclude', 'XmlSchemaImport', or 'XmlSchemaRedefine'.", items[l]);
				list.Add(items[l]);
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			schema.Items.Remove(list[m]);
		}
	}

	private void CopyIncludedComponents(XmlSchema includedSchema, XmlSchema schema)
	{
		foreach (XmlSchemaElement value in includedSchema.Elements.Values)
		{
			AddToTable(schema.Elements, value.QualifiedName, value);
		}
		foreach (XmlSchemaAttribute value2 in includedSchema.Attributes.Values)
		{
			AddToTable(schema.Attributes, value2.QualifiedName, value2);
		}
		foreach (XmlSchemaGroup value3 in includedSchema.Groups.Values)
		{
			AddToTable(schema.Groups, value3.QualifiedName, value3);
		}
		foreach (XmlSchemaAttributeGroup value4 in includedSchema.AttributeGroups.Values)
		{
			AddToTable(schema.AttributeGroups, value4.QualifiedName, value4);
		}
		foreach (XmlSchemaType value5 in includedSchema.SchemaTypes.Values)
		{
			AddToTable(schema.SchemaTypes, value5.QualifiedName, value5);
		}
		foreach (XmlSchemaNotation value6 in includedSchema.Notations.Values)
		{
			AddToTable(schema.Notations, value6.QualifiedName, value6);
		}
	}

	private void PreprocessRedefine(RedefineEntry redefineEntry)
	{
		XmlSchemaRedefine redefine = redefineEntry.redefine;
		XmlSchema schema = redefine.Schema;
		currentSchema = GetParentSchema(redefine);
		SetSchemaDefaults(currentSchema);
		if (schema.IsRedefined)
		{
			SendValidationEvent("Multiple redefines of the same schema will be ignored.", redefine, XmlSeverityType.Warning);
			return;
		}
		schema.IsRedefined = true;
		XmlSchema schemaToUpdate = redefineEntry.schemaToUpdate;
		ArrayList arrayList = new ArrayList();
		GetIncludedSet(schema, arrayList);
		string text = ((schemaToUpdate.TargetNamespace == null) ? string.Empty : schemaToUpdate.TargetNamespace);
		XmlSchemaObjectCollection items = redefine.Items;
		for (int i = 0; i < items.Count; i++)
		{
			SetParent(items[i], redefine);
			if (items[i] is XmlSchemaGroup xmlSchemaGroup)
			{
				PreprocessGroup(xmlSchemaGroup);
				xmlSchemaGroup.QualifiedName.SetNamespace(text);
				if (redefine.Groups[xmlSchemaGroup.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for group.", xmlSchemaGroup);
					continue;
				}
				AddToTable(redefine.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
				XmlSchemaGroup xmlSchemaGroup2 = (XmlSchemaGroup)schemaToUpdate.Groups[xmlSchemaGroup.QualifiedName];
				XmlSchema parentSchema = GetParentSchema(xmlSchemaGroup2);
				if (xmlSchemaGroup2 == null || (parentSchema != schema && !arrayList.Contains(parentSchema)))
				{
					SendValidationEvent("Cannot find a {0} with name '{1}' to redefine.", "<group>", xmlSchemaGroup.QualifiedName.ToString(), xmlSchemaGroup);
					continue;
				}
				xmlSchemaGroup.Redefined = xmlSchemaGroup2;
				schemaToUpdate.Groups.Insert(xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
				CheckRefinedGroup(xmlSchemaGroup);
			}
			else if (items[i] is XmlSchemaAttributeGroup)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)items[i];
				PreprocessAttributeGroup(xmlSchemaAttributeGroup);
				xmlSchemaAttributeGroup.QualifiedName.SetNamespace(text);
				if (redefine.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for attribute group.", xmlSchemaAttributeGroup);
					continue;
				}
				AddToTable(redefine.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = (XmlSchemaAttributeGroup)schemaToUpdate.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName];
				XmlSchema parentSchema2 = GetParentSchema(xmlSchemaAttributeGroup2);
				if (xmlSchemaAttributeGroup2 == null || (parentSchema2 != schema && !arrayList.Contains(parentSchema2)))
				{
					SendValidationEvent("Cannot find a {0} with name '{1}' to redefine.", "<attributeGroup>", xmlSchemaAttributeGroup.QualifiedName.ToString(), xmlSchemaAttributeGroup);
					continue;
				}
				xmlSchemaAttributeGroup.Redefined = xmlSchemaAttributeGroup2;
				schemaToUpdate.AttributeGroups.Insert(xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
				CheckRefinedAttributeGroup(xmlSchemaAttributeGroup);
			}
			else if (items[i] is XmlSchemaComplexType)
			{
				XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)items[i];
				PreprocessComplexType(xmlSchemaComplexType, local: false);
				xmlSchemaComplexType.QualifiedName.SetNamespace(text);
				if (redefine.SchemaTypes[xmlSchemaComplexType.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for complex type.", xmlSchemaComplexType);
					continue;
				}
				AddToTable(redefine.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
				XmlSchemaType xmlSchemaType = (XmlSchemaType)schemaToUpdate.SchemaTypes[xmlSchemaComplexType.QualifiedName];
				XmlSchema parentSchema3 = GetParentSchema(xmlSchemaType);
				if (xmlSchemaType == null || (parentSchema3 != schema && !arrayList.Contains(parentSchema3)))
				{
					SendValidationEvent("Cannot find a {0} with name '{1}' to redefine.", "<complexType>", xmlSchemaComplexType.QualifiedName.ToString(), xmlSchemaComplexType);
				}
				else if (xmlSchemaType is XmlSchemaComplexType)
				{
					xmlSchemaComplexType.Redefined = xmlSchemaType;
					schemaToUpdate.SchemaTypes.Insert(xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
					CheckRefinedComplexType(xmlSchemaComplexType);
				}
				else
				{
					SendValidationEvent("Cannot redefine a simple type as complex type.", xmlSchemaComplexType);
				}
			}
			else
			{
				if (!(items[i] is XmlSchemaSimpleType))
				{
					continue;
				}
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)items[i];
				PreprocessSimpleType(xmlSchemaSimpleType, local: false);
				xmlSchemaSimpleType.QualifiedName.SetNamespace(text);
				if (redefine.SchemaTypes[xmlSchemaSimpleType.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for simple type.", xmlSchemaSimpleType);
					continue;
				}
				AddToTable(redefine.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
				XmlSchemaType xmlSchemaType2 = (XmlSchemaType)schemaToUpdate.SchemaTypes[xmlSchemaSimpleType.QualifiedName];
				XmlSchema parentSchema4 = GetParentSchema(xmlSchemaType2);
				if (xmlSchemaType2 == null || (parentSchema4 != schema && !arrayList.Contains(parentSchema4)))
				{
					SendValidationEvent("Cannot find a {0} with name '{1}' to redefine.", "<simpleType>", xmlSchemaSimpleType.QualifiedName.ToString(), xmlSchemaSimpleType);
				}
				else if (xmlSchemaType2 is XmlSchemaSimpleType)
				{
					xmlSchemaSimpleType.Redefined = xmlSchemaType2;
					schemaToUpdate.SchemaTypes.Insert(xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
					CheckRefinedSimpleType(xmlSchemaSimpleType);
				}
				else
				{
					SendValidationEvent("Cannot redefine a complex type as simple type.", xmlSchemaSimpleType);
				}
			}
		}
	}

	private void GetIncludedSet(XmlSchema schema, ArrayList includesList)
	{
		if (includesList.Contains(schema))
		{
			return;
		}
		includesList.Add(schema);
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			if ((xmlSchemaExternal.Compositor == Compositor.Include || xmlSchemaExternal.Compositor == Compositor.Redefine) && xmlSchemaExternal.Schema != null)
			{
				GetIncludedSet(xmlSchemaExternal.Schema, includesList);
			}
		}
	}

	internal static XmlSchema GetParentSchema(XmlSchemaObject currentSchemaObject)
	{
		XmlSchema xmlSchema = null;
		while (xmlSchema == null && currentSchemaObject != null)
		{
			currentSchemaObject = currentSchemaObject.Parent;
			xmlSchema = currentSchemaObject as XmlSchema;
		}
		return xmlSchema;
	}

	private void SetSchemaDefaults(XmlSchema schema)
	{
		if (schema.BlockDefault == XmlSchemaDerivationMethod.All)
		{
			blockDefault = XmlSchemaDerivationMethod.All;
		}
		else if (schema.BlockDefault == XmlSchemaDerivationMethod.None)
		{
			blockDefault = XmlSchemaDerivationMethod.Empty;
		}
		else
		{
			if ((schema.BlockDefault & ~(XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
			{
				SendValidationEvent("The values 'list' and 'union' are invalid for the blockDefault attribute.", schema);
			}
			blockDefault = schema.BlockDefault & (XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction);
		}
		if (schema.FinalDefault == XmlSchemaDerivationMethod.All)
		{
			finalDefault = XmlSchemaDerivationMethod.All;
		}
		else if (schema.FinalDefault == XmlSchemaDerivationMethod.None)
		{
			finalDefault = XmlSchemaDerivationMethod.Empty;
		}
		else
		{
			if ((schema.FinalDefault & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)) != XmlSchemaDerivationMethod.Empty)
			{
				SendValidationEvent("The value 'substitution' is invalid for the finalDefault attribute.", schema);
			}
			finalDefault = schema.FinalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union);
		}
		elementFormDefault = schema.ElementFormDefault;
		if (elementFormDefault == XmlSchemaForm.None)
		{
			elementFormDefault = XmlSchemaForm.Unqualified;
		}
		attributeFormDefault = schema.AttributeFormDefault;
		if (attributeFormDefault == XmlSchemaForm.None)
		{
			attributeFormDefault = XmlSchemaForm.Unqualified;
		}
	}

	private int CountGroupSelfReference(XmlSchemaObjectCollection items, XmlQualifiedName name, XmlSchemaGroup redefined)
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i] is XmlSchemaGroupRef xmlSchemaGroupRef)
			{
				if (xmlSchemaGroupRef.RefName == name)
				{
					xmlSchemaGroupRef.Redefined = redefined;
					if (xmlSchemaGroupRef.MinOccurs != 1m || xmlSchemaGroupRef.MaxOccurs != 1m)
					{
						SendValidationEvent("When group is redefined, the real value of both minOccurs and maxOccurs attribute must be 1 (or absent).", xmlSchemaGroupRef);
					}
					num++;
				}
			}
			else if (items[i] is XmlSchemaGroupBase)
			{
				num += CountGroupSelfReference(((XmlSchemaGroupBase)items[i]).Items, name, redefined);
			}
			if (num > 1)
			{
				break;
			}
		}
		return num;
	}

	private void CheckRefinedGroup(XmlSchemaGroup group)
	{
		int num = 0;
		if (group.Particle != null)
		{
			num = CountGroupSelfReference(group.Particle.Items, group.QualifiedName, group.Redefined);
		}
		if (num > 1)
		{
			SendValidationEvent("Multiple self-reference within a group is redefined.", group);
		}
		group.SelfReferenceCount = num;
	}

	private void CheckRefinedAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
	{
		int num = 0;
		for (int i = 0; i < attributeGroup.Attributes.Count; i++)
		{
			if (attributeGroup.Attributes[i] is XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef && xmlSchemaAttributeGroupRef.RefName == attributeGroup.QualifiedName)
			{
				num++;
			}
		}
		if (num > 1)
		{
			SendValidationEvent("Multiple self-reference within an attribute group is redefined.", attributeGroup);
		}
		attributeGroup.SelfReferenceCount = num;
	}

	private void CheckRefinedSimpleType(XmlSchemaSimpleType stype)
	{
		if (stype.Content == null || !(stype.Content is XmlSchemaSimpleTypeRestriction) || !(((XmlSchemaSimpleTypeRestriction)stype.Content).BaseTypeName == stype.QualifiedName))
		{
			SendValidationEvent("If type is being redefined, the base type has to be self-referenced.", stype);
		}
	}

	private void CheckRefinedComplexType(XmlSchemaComplexType ctype)
	{
		if (ctype.ContentModel != null)
		{
			XmlQualifiedName xmlQualifiedName;
			if (ctype.ContentModel is XmlSchemaComplexContent)
			{
				XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)ctype.ContentModel;
				xmlQualifiedName = ((!(xmlSchemaComplexContent.Content is XmlSchemaComplexContentRestriction)) ? ((XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content).BaseTypeName : ((XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content).BaseTypeName);
			}
			else
			{
				XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)ctype.ContentModel;
				xmlQualifiedName = ((!(xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentRestriction)) ? ((XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content).BaseTypeName : ((XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content).BaseTypeName);
			}
			if (xmlQualifiedName == ctype.QualifiedName)
			{
				return;
			}
		}
		SendValidationEvent("If type is being redefined, the base type has to be self-referenced.", ctype);
	}

	private void PreprocessAttribute(XmlSchemaAttribute attribute)
	{
		if (attribute.Name != null)
		{
			ValidateNameAttribute(attribute);
			attribute.SetQualifiedName(new XmlQualifiedName(attribute.Name, targetNamespace));
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "name", attribute);
		}
		if (attribute.Use != XmlSchemaUse.None)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "use", attribute);
		}
		if (attribute.Form != XmlSchemaForm.None)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "form", attribute);
		}
		PreprocessAttributeContent(attribute);
		ValidateIdAttribute(attribute);
	}

	private void PreprocessLocalAttribute(XmlSchemaAttribute attribute)
	{
		if (attribute.Name != null)
		{
			ValidateNameAttribute(attribute);
			PreprocessAttributeContent(attribute);
			attribute.SetQualifiedName(new XmlQualifiedName(attribute.Name, (attribute.Form == XmlSchemaForm.Qualified || (attribute.Form == XmlSchemaForm.None && attributeFormDefault == XmlSchemaForm.Qualified)) ? targetNamespace : null));
		}
		else
		{
			PreprocessAnnotation(attribute);
			if (attribute.RefName.IsEmpty)
			{
				SendValidationEvent("For attribute '{0}', either the name or the ref attribute must be present, but not both.", "???", attribute);
			}
			else
			{
				ValidateQNameAttribute(attribute, "ref", attribute.RefName);
			}
			if (!attribute.SchemaTypeName.IsEmpty || attribute.SchemaType != null || attribute.Form != XmlSchemaForm.None)
			{
				SendValidationEvent("If ref is present, all of 'simpleType', 'form', 'type', and 'use' must be absent.", attribute);
			}
			attribute.SetQualifiedName(attribute.RefName);
		}
		ValidateIdAttribute(attribute);
	}

	private void PreprocessAttributeContent(XmlSchemaAttribute attribute)
	{
		PreprocessAnnotation(attribute);
		if (Ref.Equal(currentSchema.TargetNamespace, NsXsi))
		{
			SendValidationEvent("The target namespace of an attribute declaration, whether local or global, must not match http://www.w3.org/2001/XMLSchema-instance.", attribute);
		}
		if (!attribute.RefName.IsEmpty)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "ref", attribute);
		}
		if (attribute.DefaultValue != null && attribute.FixedValue != null)
		{
			SendValidationEvent("The fixed and default attributes cannot both be present.", attribute);
		}
		if (attribute.DefaultValue != null && attribute.Use != XmlSchemaUse.Optional && attribute.Use != XmlSchemaUse.None)
		{
			SendValidationEvent("The 'use' attribute must be optional (or absent) if the default attribute is present.", attribute);
		}
		if (attribute.Name == Xmlns)
		{
			SendValidationEvent("The value 'xmlns' cannot be used as the name of an attribute declaration.", attribute);
		}
		if (attribute.SchemaType != null)
		{
			SetParent(attribute.SchemaType, attribute);
			if (!attribute.SchemaTypeName.IsEmpty)
			{
				SendValidationEvent("The type attribute cannot be present with either simpleType or complexType.", attribute);
			}
			PreprocessSimpleType(attribute.SchemaType, local: true);
		}
		if (!attribute.SchemaTypeName.IsEmpty)
		{
			ValidateQNameAttribute(attribute, "type", attribute.SchemaTypeName);
		}
	}

	private void PreprocessAttributeGroup(XmlSchemaAttributeGroup attributeGroup)
	{
		if (attributeGroup.Name != null)
		{
			ValidateNameAttribute(attributeGroup);
			attributeGroup.SetQualifiedName(new XmlQualifiedName(attributeGroup.Name, targetNamespace));
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "name", attributeGroup);
		}
		PreprocessAttributes(attributeGroup.Attributes, attributeGroup.AnyAttribute, attributeGroup);
		PreprocessAnnotation(attributeGroup);
		ValidateIdAttribute(attributeGroup);
	}

	private void PreprocessElement(XmlSchemaElement element)
	{
		if (element.Name != null)
		{
			ValidateNameAttribute(element);
			element.SetQualifiedName(new XmlQualifiedName(element.Name, targetNamespace));
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "name", element);
		}
		PreprocessElementContent(element);
		if (element.Final == XmlSchemaDerivationMethod.All)
		{
			element.SetFinalResolved(XmlSchemaDerivationMethod.All);
		}
		else if (element.Final == XmlSchemaDerivationMethod.None)
		{
			if (finalDefault == XmlSchemaDerivationMethod.All)
			{
				element.SetFinalResolved(XmlSchemaDerivationMethod.All);
			}
			else
			{
				element.SetFinalResolved(finalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
		}
		else
		{
			if ((element.Final & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
			{
				SendValidationEvent("The values 'substitution', 'list', and 'union' are invalid for the final attribute on element.", element);
			}
			element.SetFinalResolved(element.Final & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
		}
		if (element.Form != XmlSchemaForm.None)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "form", element);
		}
		if (element.MinOccursString != null)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "minOccurs", element);
		}
		if (element.MaxOccursString != null)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "maxOccurs", element);
		}
		if (!element.SubstitutionGroup.IsEmpty)
		{
			ValidateQNameAttribute(element, "type", element.SubstitutionGroup);
		}
		ValidateIdAttribute(element);
	}

	private void PreprocessLocalElement(XmlSchemaElement element)
	{
		if (element.Name != null)
		{
			ValidateNameAttribute(element);
			PreprocessElementContent(element);
			element.SetQualifiedName(new XmlQualifiedName(element.Name, (element.Form == XmlSchemaForm.Qualified || (element.Form == XmlSchemaForm.None && elementFormDefault == XmlSchemaForm.Qualified)) ? targetNamespace : null));
		}
		else
		{
			PreprocessAnnotation(element);
			if (element.RefName.IsEmpty)
			{
				SendValidationEvent("For element declaration, either the name or the ref attribute must be present.", element);
			}
			else
			{
				ValidateQNameAttribute(element, "ref", element.RefName);
			}
			if (!element.SchemaTypeName.IsEmpty || element.HasAbstractAttribute || element.Block != XmlSchemaDerivationMethod.None || element.SchemaType != null || element.HasConstraints || element.DefaultValue != null || element.Form != XmlSchemaForm.None || element.FixedValue != null || element.HasNillableAttribute)
			{
				SendValidationEvent("If ref is present, all of <complexType>, <simpleType>, <key>, <keyref>, <unique>, nillable, default, fixed, form, block, and type must be absent.", element);
			}
			if (element.DefaultValue != null && element.FixedValue != null)
			{
				SendValidationEvent("The fixed and default attributes cannot both be present.", element);
			}
			element.SetQualifiedName(element.RefName);
		}
		if (element.MinOccurs > element.MaxOccurs)
		{
			element.MinOccurs = 0m;
			SendValidationEvent("minOccurs value cannot be greater than maxOccurs value.", element);
		}
		if (element.HasAbstractAttribute)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "abstract", element);
		}
		if (element.Final != XmlSchemaDerivationMethod.None)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "final", element);
		}
		if (!element.SubstitutionGroup.IsEmpty)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "substitutionGroup", element);
		}
		ValidateIdAttribute(element);
	}

	private void PreprocessElementContent(XmlSchemaElement element)
	{
		PreprocessAnnotation(element);
		if (!element.RefName.IsEmpty)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "ref", element);
		}
		if (element.Block == XmlSchemaDerivationMethod.All)
		{
			element.SetBlockResolved(XmlSchemaDerivationMethod.All);
		}
		else if (element.Block == XmlSchemaDerivationMethod.None)
		{
			if (blockDefault == XmlSchemaDerivationMethod.All)
			{
				element.SetBlockResolved(XmlSchemaDerivationMethod.All);
			}
			else
			{
				element.SetBlockResolved(blockDefault & (XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
		}
		else
		{
			if ((element.Block & ~(XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
			{
				SendValidationEvent("The values 'list' and 'union' are invalid for the block attribute on element.", element);
			}
			element.SetBlockResolved(element.Block & (XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
		}
		if (element.SchemaType != null)
		{
			SetParent(element.SchemaType, element);
			if (!element.SchemaTypeName.IsEmpty)
			{
				SendValidationEvent("The type attribute cannot be present with either simpleType or complexType.", element);
			}
			if (element.SchemaType is XmlSchemaComplexType)
			{
				PreprocessComplexType((XmlSchemaComplexType)element.SchemaType, local: true);
			}
			else
			{
				PreprocessSimpleType((XmlSchemaSimpleType)element.SchemaType, local: true);
			}
		}
		if (!element.SchemaTypeName.IsEmpty)
		{
			ValidateQNameAttribute(element, "type", element.SchemaTypeName);
		}
		if (element.DefaultValue != null && element.FixedValue != null)
		{
			SendValidationEvent("The fixed and default attributes cannot both be present.", element);
		}
		for (int i = 0; i < element.Constraints.Count; i++)
		{
			XmlSchemaIdentityConstraint xmlSchemaIdentityConstraint = (XmlSchemaIdentityConstraint)element.Constraints[i];
			SetParent(xmlSchemaIdentityConstraint, element);
			PreprocessIdentityConstraint(xmlSchemaIdentityConstraint);
		}
	}

	private void PreprocessIdentityConstraint(XmlSchemaIdentityConstraint constraint)
	{
		bool flag = true;
		PreprocessAnnotation(constraint);
		if (constraint.Name != null)
		{
			ValidateNameAttribute(constraint);
			constraint.SetQualifiedName(new XmlQualifiedName(constraint.Name, targetNamespace));
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "name", constraint);
			flag = false;
		}
		if (rootSchema.IdentityConstraints[constraint.QualifiedName] != null)
		{
			SendValidationEvent("The identity constraint '{0}' has already been declared.", constraint.QualifiedName.ToString(), constraint);
			flag = false;
		}
		else
		{
			rootSchema.IdentityConstraints.Add(constraint.QualifiedName, constraint);
		}
		if (constraint.Selector == null)
		{
			SendValidationEvent("Selector must be present.", constraint);
			flag = false;
		}
		if (constraint.Fields.Count == 0)
		{
			SendValidationEvent("At least one field must be present.", constraint);
			flag = false;
		}
		if (constraint is XmlSchemaKeyref)
		{
			XmlSchemaKeyref xmlSchemaKeyref = (XmlSchemaKeyref)constraint;
			if (xmlSchemaKeyref.Refer.IsEmpty)
			{
				SendValidationEvent("The referring attribute must be present.", constraint);
				flag = false;
			}
			else
			{
				ValidateQNameAttribute(xmlSchemaKeyref, "refer", xmlSchemaKeyref.Refer);
			}
		}
		if (flag)
		{
			ValidateIdAttribute(constraint);
			ValidateIdAttribute(constraint.Selector);
			SetParent(constraint.Selector, constraint);
			for (int i = 0; i < constraint.Fields.Count; i++)
			{
				SetParent(constraint.Fields[i], constraint);
				ValidateIdAttribute(constraint.Fields[i]);
			}
		}
	}

	private void PreprocessSimpleType(XmlSchemaSimpleType simpleType, bool local)
	{
		if (local)
		{
			if (simpleType.Name != null)
			{
				SendValidationEvent("The '{0}' attribute cannot be present.", "name", simpleType);
			}
		}
		else
		{
			if (simpleType.Name != null)
			{
				ValidateNameAttribute(simpleType);
				simpleType.SetQualifiedName(new XmlQualifiedName(simpleType.Name, targetNamespace));
			}
			else
			{
				SendValidationEvent("The required attribute '{0}' is missing.", "name", simpleType);
			}
			if (simpleType.Final == XmlSchemaDerivationMethod.All)
			{
				simpleType.SetFinalResolved(XmlSchemaDerivationMethod.All);
			}
			else if (simpleType.Final == XmlSchemaDerivationMethod.None)
			{
				if (finalDefault == XmlSchemaDerivationMethod.All)
				{
					simpleType.SetFinalResolved(XmlSchemaDerivationMethod.All);
				}
				else
				{
					simpleType.SetFinalResolved(finalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
				}
			}
			else
			{
				if ((simpleType.Final & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)) != XmlSchemaDerivationMethod.Empty)
				{
					SendValidationEvent("The values 'substitution' and 'extension' are invalid for the final attribute on simpleType.", simpleType);
				}
				simpleType.SetFinalResolved(simpleType.Final & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
			}
		}
		if (simpleType.Content == null)
		{
			SendValidationEvent("SimpleType content is missing.", simpleType);
		}
		else if (simpleType.Content is XmlSchemaSimpleTypeRestriction)
		{
			XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = (XmlSchemaSimpleTypeRestriction)simpleType.Content;
			SetParent(xmlSchemaSimpleTypeRestriction, simpleType);
			for (int i = 0; i < xmlSchemaSimpleTypeRestriction.Facets.Count; i++)
			{
				SetParent(xmlSchemaSimpleTypeRestriction.Facets[i], xmlSchemaSimpleTypeRestriction);
			}
			if (xmlSchemaSimpleTypeRestriction.BaseType != null)
			{
				if (!xmlSchemaSimpleTypeRestriction.BaseTypeName.IsEmpty)
				{
					SendValidationEvent("SimpleType restriction should have either the base attribute or a simpleType child, but not both.", xmlSchemaSimpleTypeRestriction);
				}
				PreprocessSimpleType(xmlSchemaSimpleTypeRestriction.BaseType, local: true);
			}
			else if (xmlSchemaSimpleTypeRestriction.BaseTypeName.IsEmpty)
			{
				SendValidationEvent("SimpleType restriction should have either the base attribute or a simpleType child to indicate the base type for the derivation.", xmlSchemaSimpleTypeRestriction);
			}
			else
			{
				ValidateQNameAttribute(xmlSchemaSimpleTypeRestriction, "base", xmlSchemaSimpleTypeRestriction.BaseTypeName);
			}
			PreprocessAnnotation(xmlSchemaSimpleTypeRestriction);
			ValidateIdAttribute(xmlSchemaSimpleTypeRestriction);
		}
		else if (simpleType.Content is XmlSchemaSimpleTypeList)
		{
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = (XmlSchemaSimpleTypeList)simpleType.Content;
			SetParent(xmlSchemaSimpleTypeList, simpleType);
			if (xmlSchemaSimpleTypeList.ItemType != null)
			{
				if (!xmlSchemaSimpleTypeList.ItemTypeName.IsEmpty)
				{
					SendValidationEvent("SimpleType list should have either the itemType attribute or a simpleType child, but not both.", xmlSchemaSimpleTypeList);
				}
				SetParent(xmlSchemaSimpleTypeList.ItemType, xmlSchemaSimpleTypeList);
				PreprocessSimpleType(xmlSchemaSimpleTypeList.ItemType, local: true);
			}
			else if (xmlSchemaSimpleTypeList.ItemTypeName.IsEmpty)
			{
				SendValidationEvent("SimpleType list should have either the itemType attribute or a simpleType child to indicate the itemType of the list.", xmlSchemaSimpleTypeList);
			}
			else
			{
				ValidateQNameAttribute(xmlSchemaSimpleTypeList, "itemType", xmlSchemaSimpleTypeList.ItemTypeName);
			}
			PreprocessAnnotation(xmlSchemaSimpleTypeList);
			ValidateIdAttribute(xmlSchemaSimpleTypeList);
		}
		else
		{
			XmlSchemaSimpleTypeUnion xmlSchemaSimpleTypeUnion = (XmlSchemaSimpleTypeUnion)simpleType.Content;
			SetParent(xmlSchemaSimpleTypeUnion, simpleType);
			int num = xmlSchemaSimpleTypeUnion.BaseTypes.Count;
			if (xmlSchemaSimpleTypeUnion.MemberTypes != null)
			{
				num += xmlSchemaSimpleTypeUnion.MemberTypes.Length;
				XmlQualifiedName[] memberTypes = xmlSchemaSimpleTypeUnion.MemberTypes;
				for (int j = 0; j < memberTypes.Length; j++)
				{
					ValidateQNameAttribute(xmlSchemaSimpleTypeUnion, "memberTypes", memberTypes[j]);
				}
			}
			if (num == 0)
			{
				SendValidationEvent("Either the memberTypes attribute must be non-empty or there must be at least one simpleType child.", xmlSchemaSimpleTypeUnion);
			}
			for (int k = 0; k < xmlSchemaSimpleTypeUnion.BaseTypes.Count; k++)
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)xmlSchemaSimpleTypeUnion.BaseTypes[k];
				SetParent(xmlSchemaSimpleType, xmlSchemaSimpleTypeUnion);
				PreprocessSimpleType(xmlSchemaSimpleType, local: true);
			}
			PreprocessAnnotation(xmlSchemaSimpleTypeUnion);
			ValidateIdAttribute(xmlSchemaSimpleTypeUnion);
		}
		ValidateIdAttribute(simpleType);
	}

	private void PreprocessComplexType(XmlSchemaComplexType complexType, bool local)
	{
		if (local)
		{
			if (complexType.Name != null)
			{
				SendValidationEvent("The '{0}' attribute cannot be present.", "name", complexType);
			}
		}
		else
		{
			if (complexType.Name != null)
			{
				ValidateNameAttribute(complexType);
				complexType.SetQualifiedName(new XmlQualifiedName(complexType.Name, targetNamespace));
			}
			else
			{
				SendValidationEvent("The required attribute '{0}' is missing.", "name", complexType);
			}
			if (complexType.Block == XmlSchemaDerivationMethod.All)
			{
				complexType.SetBlockResolved(XmlSchemaDerivationMethod.All);
			}
			else if (complexType.Block == XmlSchemaDerivationMethod.None)
			{
				complexType.SetBlockResolved(blockDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
			else
			{
				if ((complexType.Block & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
				{
					SendValidationEvent("The values 'substitution', 'list', and 'union' are invalid for the block attribute on complexType.", complexType);
				}
				complexType.SetBlockResolved(complexType.Block & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
			if (complexType.Final == XmlSchemaDerivationMethod.All)
			{
				complexType.SetFinalResolved(XmlSchemaDerivationMethod.All);
			}
			else if (complexType.Final == XmlSchemaDerivationMethod.None)
			{
				if (finalDefault == XmlSchemaDerivationMethod.All)
				{
					complexType.SetFinalResolved(XmlSchemaDerivationMethod.All);
				}
				else
				{
					complexType.SetFinalResolved(finalDefault & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
				}
			}
			else
			{
				if ((complexType.Final & ~(XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction)) != XmlSchemaDerivationMethod.Empty)
				{
					SendValidationEvent("The values 'substitution', 'list', and 'union' are invalid for the final attribute on complexType.", complexType);
				}
				complexType.SetFinalResolved(complexType.Final & (XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction));
			}
		}
		if (complexType.ContentModel != null)
		{
			SetParent(complexType.ContentModel, complexType);
			PreprocessAnnotation(complexType.ContentModel);
			if (complexType.Particle == null)
			{
				_ = complexType.Attributes;
			}
			if (complexType.ContentModel is XmlSchemaSimpleContent)
			{
				XmlSchemaSimpleContent xmlSchemaSimpleContent = (XmlSchemaSimpleContent)complexType.ContentModel;
				if (xmlSchemaSimpleContent.Content == null)
				{
					if (complexType.QualifiedName == XmlQualifiedName.Empty)
					{
						SendValidationEvent("'restriction' or 'extension' child is required for complexType with simpleContent or complexContent child.", complexType);
					}
					else
					{
						SendValidationEvent("'restriction' or 'extension' child is required for complexType '{0}' in namespace '{1}', because it has a simpleContent or complexContent child.", complexType.QualifiedName.Name, complexType.QualifiedName.Namespace, complexType);
					}
				}
				else
				{
					SetParent(xmlSchemaSimpleContent.Content, xmlSchemaSimpleContent);
					PreprocessAnnotation(xmlSchemaSimpleContent.Content);
					if (xmlSchemaSimpleContent.Content is XmlSchemaSimpleContentExtension)
					{
						XmlSchemaSimpleContentExtension xmlSchemaSimpleContentExtension = (XmlSchemaSimpleContentExtension)xmlSchemaSimpleContent.Content;
						if (xmlSchemaSimpleContentExtension.BaseTypeName.IsEmpty)
						{
							SendValidationEvent("The '{0}' attribute is either invalid or missing.", "base", xmlSchemaSimpleContentExtension);
						}
						else
						{
							ValidateQNameAttribute(xmlSchemaSimpleContentExtension, "base", xmlSchemaSimpleContentExtension.BaseTypeName);
						}
						PreprocessAttributes(xmlSchemaSimpleContentExtension.Attributes, xmlSchemaSimpleContentExtension.AnyAttribute, xmlSchemaSimpleContentExtension);
						ValidateIdAttribute(xmlSchemaSimpleContentExtension);
					}
					else
					{
						XmlSchemaSimpleContentRestriction xmlSchemaSimpleContentRestriction = (XmlSchemaSimpleContentRestriction)xmlSchemaSimpleContent.Content;
						if (xmlSchemaSimpleContentRestriction.BaseTypeName.IsEmpty)
						{
							SendValidationEvent("The '{0}' attribute is either invalid or missing.", "base", xmlSchemaSimpleContentRestriction);
						}
						else
						{
							ValidateQNameAttribute(xmlSchemaSimpleContentRestriction, "base", xmlSchemaSimpleContentRestriction.BaseTypeName);
						}
						if (xmlSchemaSimpleContentRestriction.BaseType != null)
						{
							SetParent(xmlSchemaSimpleContentRestriction.BaseType, xmlSchemaSimpleContentRestriction);
							PreprocessSimpleType(xmlSchemaSimpleContentRestriction.BaseType, local: true);
						}
						PreprocessAttributes(xmlSchemaSimpleContentRestriction.Attributes, xmlSchemaSimpleContentRestriction.AnyAttribute, xmlSchemaSimpleContentRestriction);
						ValidateIdAttribute(xmlSchemaSimpleContentRestriction);
					}
				}
				ValidateIdAttribute(xmlSchemaSimpleContent);
			}
			else
			{
				XmlSchemaComplexContent xmlSchemaComplexContent = (XmlSchemaComplexContent)complexType.ContentModel;
				if (xmlSchemaComplexContent.Content == null)
				{
					if (complexType.QualifiedName == XmlQualifiedName.Empty)
					{
						SendValidationEvent("'restriction' or 'extension' child is required for complexType with simpleContent or complexContent child.", complexType);
					}
					else
					{
						SendValidationEvent("'restriction' or 'extension' child is required for complexType '{0}' in namespace '{1}', because it has a simpleContent or complexContent child.", complexType.QualifiedName.Name, complexType.QualifiedName.Namespace, complexType);
					}
				}
				else
				{
					if (!xmlSchemaComplexContent.HasMixedAttribute && complexType.IsMixed)
					{
						xmlSchemaComplexContent.IsMixed = true;
					}
					SetParent(xmlSchemaComplexContent.Content, xmlSchemaComplexContent);
					PreprocessAnnotation(xmlSchemaComplexContent.Content);
					if (xmlSchemaComplexContent.Content is XmlSchemaComplexContentExtension)
					{
						XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = (XmlSchemaComplexContentExtension)xmlSchemaComplexContent.Content;
						if (xmlSchemaComplexContentExtension.BaseTypeName.IsEmpty)
						{
							SendValidationEvent("The '{0}' attribute is either invalid or missing.", "base", xmlSchemaComplexContentExtension);
						}
						else
						{
							ValidateQNameAttribute(xmlSchemaComplexContentExtension, "base", xmlSchemaComplexContentExtension.BaseTypeName);
						}
						if (xmlSchemaComplexContentExtension.Particle != null)
						{
							SetParent(xmlSchemaComplexContentExtension.Particle, xmlSchemaComplexContentExtension);
							PreprocessParticle(xmlSchemaComplexContentExtension.Particle);
						}
						PreprocessAttributes(xmlSchemaComplexContentExtension.Attributes, xmlSchemaComplexContentExtension.AnyAttribute, xmlSchemaComplexContentExtension);
						ValidateIdAttribute(xmlSchemaComplexContentExtension);
					}
					else
					{
						XmlSchemaComplexContentRestriction xmlSchemaComplexContentRestriction = (XmlSchemaComplexContentRestriction)xmlSchemaComplexContent.Content;
						if (xmlSchemaComplexContentRestriction.BaseTypeName.IsEmpty)
						{
							SendValidationEvent("The '{0}' attribute is either invalid or missing.", "base", xmlSchemaComplexContentRestriction);
						}
						else
						{
							ValidateQNameAttribute(xmlSchemaComplexContentRestriction, "base", xmlSchemaComplexContentRestriction.BaseTypeName);
						}
						if (xmlSchemaComplexContentRestriction.Particle != null)
						{
							SetParent(xmlSchemaComplexContentRestriction.Particle, xmlSchemaComplexContentRestriction);
							PreprocessParticle(xmlSchemaComplexContentRestriction.Particle);
						}
						PreprocessAttributes(xmlSchemaComplexContentRestriction.Attributes, xmlSchemaComplexContentRestriction.AnyAttribute, xmlSchemaComplexContentRestriction);
						ValidateIdAttribute(xmlSchemaComplexContentRestriction);
					}
					ValidateIdAttribute(xmlSchemaComplexContent);
				}
			}
		}
		else
		{
			if (complexType.Particle != null)
			{
				SetParent(complexType.Particle, complexType);
				PreprocessParticle(complexType.Particle);
			}
			PreprocessAttributes(complexType.Attributes, complexType.AnyAttribute, complexType);
		}
		ValidateIdAttribute(complexType);
	}

	private void PreprocessGroup(XmlSchemaGroup group)
	{
		if (group.Name != null)
		{
			ValidateNameAttribute(group);
			group.SetQualifiedName(new XmlQualifiedName(group.Name, targetNamespace));
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "name", group);
		}
		if (group.Particle == null)
		{
			SendValidationEvent("'sequence', 'choice', or 'all' child is required.", group);
			return;
		}
		if (group.Particle.MinOccursString != null)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "minOccurs", group.Particle);
		}
		if (group.Particle.MaxOccursString != null)
		{
			SendValidationEvent("The '{0}' attribute cannot be present.", "maxOccurs", group.Particle);
		}
		PreprocessParticle(group.Particle);
		PreprocessAnnotation(group);
		ValidateIdAttribute(group);
	}

	private void PreprocessNotation(XmlSchemaNotation notation)
	{
		if (notation.Name != null)
		{
			ValidateNameAttribute(notation);
			notation.QualifiedName = new XmlQualifiedName(notation.Name, targetNamespace);
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "name", notation);
		}
		if (notation.Public == null && notation.System == null)
		{
			SendValidationEvent("NOTATION must have either the Public or System attribute present.", notation);
		}
		else
		{
			if (notation.Public != null)
			{
				try
				{
					XmlConvert.VerifyTOKEN(notation.Public);
				}
				catch (XmlException innerException)
				{
					SendValidationEvent("Public attribute '{0}' is an invalid URI.", new string[1] { notation.Public }, innerException, notation);
				}
			}
			if (notation.System != null)
			{
				ParseUri(notation.System, "System attribute '{0}' is an invalid URI.", notation);
			}
		}
		PreprocessAnnotation(notation);
		ValidateIdAttribute(notation);
	}

	private void PreprocessParticle(XmlSchemaParticle particle)
	{
		if (particle is XmlSchemaAll)
		{
			if (particle.MinOccurs != 0m && particle.MinOccurs != 1m)
			{
				particle.MinOccurs = 1m;
				SendValidationEvent("'all' must have 'minOccurs' value of 0 or 1.", particle);
			}
			if (particle.MaxOccurs != 1m)
			{
				particle.MaxOccurs = 1m;
				SendValidationEvent("'all' must have {max occurs}=1.", particle);
			}
			XmlSchemaObjectCollection items = ((XmlSchemaAll)particle).Items;
			for (int i = 0; i < items.Count; i++)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)items[i];
				if (xmlSchemaElement.MaxOccurs != 0m && xmlSchemaElement.MaxOccurs != 1m)
				{
					xmlSchemaElement.MaxOccurs = 1m;
					SendValidationEvent("The {max occurs} of all the particles in the {particles} of an all group must be 0 or 1.", xmlSchemaElement);
				}
				SetParent(xmlSchemaElement, particle);
				PreprocessLocalElement(xmlSchemaElement);
			}
		}
		else
		{
			if (particle.MinOccurs > particle.MaxOccurs)
			{
				particle.MinOccurs = particle.MaxOccurs;
				SendValidationEvent("minOccurs value cannot be greater than maxOccurs value.", particle);
			}
			if (particle is XmlSchemaChoice)
			{
				XmlSchemaObjectCollection items = ((XmlSchemaChoice)particle).Items;
				for (int j = 0; j < items.Count; j++)
				{
					SetParent(items[j], particle);
					if (items[j] is XmlSchemaElement element)
					{
						PreprocessLocalElement(element);
					}
					else
					{
						PreprocessParticle((XmlSchemaParticle)items[j]);
					}
				}
			}
			else if (particle is XmlSchemaSequence)
			{
				XmlSchemaObjectCollection items = ((XmlSchemaSequence)particle).Items;
				for (int k = 0; k < items.Count; k++)
				{
					SetParent(items[k], particle);
					if (items[k] is XmlSchemaElement element2)
					{
						PreprocessLocalElement(element2);
					}
					else
					{
						PreprocessParticle((XmlSchemaParticle)items[k]);
					}
				}
			}
			else if (particle is XmlSchemaGroupRef)
			{
				XmlSchemaGroupRef xmlSchemaGroupRef = (XmlSchemaGroupRef)particle;
				if (xmlSchemaGroupRef.RefName.IsEmpty)
				{
					SendValidationEvent("The '{0}' attribute is either invalid or missing.", "ref", xmlSchemaGroupRef);
				}
				else
				{
					ValidateQNameAttribute(xmlSchemaGroupRef, "ref", xmlSchemaGroupRef.RefName);
				}
			}
			else if (particle is XmlSchemaAny)
			{
				try
				{
					((XmlSchemaAny)particle).BuildNamespaceList(targetNamespace);
				}
				catch (FormatException ex)
				{
					SendValidationEvent("The value of the namespace attribute of the element or attribute wildcard is invalid - {0}", new string[1] { ex.Message }, ex, particle);
				}
			}
		}
		PreprocessAnnotation(particle);
		ValidateIdAttribute(particle);
	}

	private void PreprocessAttributes(XmlSchemaObjectCollection attributes, XmlSchemaAnyAttribute anyAttribute, XmlSchemaObject parent)
	{
		for (int i = 0; i < attributes.Count; i++)
		{
			SetParent(attributes[i], parent);
			if (attributes[i] is XmlSchemaAttribute attribute)
			{
				PreprocessLocalAttribute(attribute);
				continue;
			}
			XmlSchemaAttributeGroupRef xmlSchemaAttributeGroupRef = (XmlSchemaAttributeGroupRef)attributes[i];
			if (xmlSchemaAttributeGroupRef.RefName.IsEmpty)
			{
				SendValidationEvent("The '{0}' attribute is either invalid or missing.", "ref", xmlSchemaAttributeGroupRef);
			}
			else
			{
				ValidateQNameAttribute(xmlSchemaAttributeGroupRef, "ref", xmlSchemaAttributeGroupRef.RefName);
			}
			PreprocessAnnotation(attributes[i]);
			ValidateIdAttribute(attributes[i]);
		}
		if (anyAttribute != null)
		{
			try
			{
				SetParent(anyAttribute, parent);
				PreprocessAnnotation(anyAttribute);
				anyAttribute.BuildNamespaceList(targetNamespace);
			}
			catch (FormatException ex)
			{
				SendValidationEvent("The value of the namespace attribute of the element or attribute wildcard is invalid - {0}", new string[1] { ex.Message }, ex, anyAttribute);
			}
			ValidateIdAttribute(anyAttribute);
		}
	}

	private void ValidateIdAttribute(XmlSchemaObject xso)
	{
		if (xso.IdAttribute != null)
		{
			try
			{
				xso.IdAttribute = base.NameTable.Add(XmlConvert.VerifyNCName(xso.IdAttribute));
			}
			catch (XmlException ex)
			{
				SendValidationEvent("Invalid 'id' attribute value: {0}", new string[1] { ex.Message }, ex, xso);
				return;
			}
			catch (ArgumentNullException)
			{
				SendValidationEvent("Invalid 'id' attribute value: {0}", Res.GetString("Value cannot be null."), xso);
				return;
			}
			try
			{
				currentSchema.Ids.Add(xso.IdAttribute, xso);
			}
			catch (ArgumentException)
			{
				SendValidationEvent("Duplicate ID attribute.", xso);
			}
		}
	}

	private void ValidateNameAttribute(XmlSchemaObject xso)
	{
		string nameAttribute = xso.NameAttribute;
		if (nameAttribute == null || nameAttribute.Length == 0)
		{
			SendValidationEvent("Invalid 'name' attribute value '{0}': '{1}'.", null, Res.GetString("Value cannot be null."), xso);
		}
		nameAttribute = XmlComplianceUtil.NonCDataNormalize(nameAttribute);
		int num = ValidateNames.ParseNCName(nameAttribute, 0);
		if (num != nameAttribute.Length)
		{
			string[] array = XmlException.BuildCharExceptionArgs(nameAttribute, num);
			string msg = Res.GetString("The '{0}' character, hexadecimal value {1}, at position {2} within the name, cannot be included in a name.", array[0], array[1], num);
			SendValidationEvent("Invalid 'name' attribute value '{0}': '{1}'.", nameAttribute, msg, xso);
		}
		else
		{
			xso.NameAttribute = base.NameTable.Add(nameAttribute);
		}
	}

	private void ValidateQNameAttribute(XmlSchemaObject xso, string attributeName, XmlQualifiedName value)
	{
		try
		{
			value.Verify();
			value.Atomize(base.NameTable);
			if (currentSchema.IsChameleon && value.Namespace.Length == 0)
			{
				value.SetNamespace(currentSchema.TargetNamespace);
			}
			if (referenceNamespaces[value.Namespace] == null)
			{
				SendValidationEvent("Namespace '{0}' is not available to be referenced in this schema.", value.Namespace, xso, XmlSeverityType.Warning);
			}
		}
		catch (FormatException ex)
		{
			SendValidationEvent("Invalid '{0}' attribute: '{1}'.", new string[2] { attributeName, ex.Message }, ex, xso);
		}
		catch (XmlException ex2)
		{
			SendValidationEvent("Invalid '{0}' attribute: '{1}'.", new string[2] { attributeName, ex2.Message }, ex2, xso);
		}
	}

	private Uri ResolveSchemaLocationUri(XmlSchema enclosingSchema, string location)
	{
		if (location.Length == 0)
		{
			return null;
		}
		return xmlResolver.ResolveUri(enclosingSchema.BaseUri, location);
	}

	private object GetSchemaEntity(Uri ruri)
	{
		return xmlResolver.GetEntity(ruri, null, null);
	}

	private XmlSchema GetChameleonSchema(string targetNamespace, XmlSchema schema)
	{
		ChameleonKey key = new ChameleonKey(targetNamespace, schema);
		XmlSchema xmlSchema = (XmlSchema)chameleonSchemas[key];
		if (xmlSchema == null)
		{
			xmlSchema = schema.DeepClone();
			xmlSchema.IsChameleon = true;
			xmlSchema.TargetNamespace = targetNamespace;
			chameleonSchemas.Add(key, xmlSchema);
			xmlSchema.SourceUri = schema.SourceUri;
			schema.IsProcessing = false;
		}
		return xmlSchema;
	}

	private void SetParent(XmlSchemaObject child, XmlSchemaObject parent)
	{
		child.Parent = parent;
	}

	private void PreprocessAnnotation(XmlSchemaObject schemaObject)
	{
		if (schemaObject is XmlSchemaAnnotated)
		{
			XmlSchemaAnnotation annotation = (schemaObject as XmlSchemaAnnotated).Annotation;
			if (annotation != null)
			{
				PreprocessAnnotation(annotation);
				annotation.Parent = schemaObject;
			}
		}
	}

	private void PreprocessAnnotation(XmlSchemaAnnotation annotation)
	{
		ValidateIdAttribute(annotation);
		for (int i = 0; i < annotation.Items.Count; i++)
		{
			annotation.Items[i].Parent = annotation;
		}
	}
}
