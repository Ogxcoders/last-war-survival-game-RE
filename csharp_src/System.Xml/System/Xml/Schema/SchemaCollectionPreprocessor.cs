using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System.Xml.Schema;

internal sealed class SchemaCollectionPreprocessor : BaseProcessor
{
	private enum Compositor
	{
		Root,
		Include,
		Import
	}

	private XmlSchema schema;

	private string targetNamespace;

	private bool buildinIncluded;

	private XmlSchemaForm elementFormDefault;

	private XmlSchemaForm attributeFormDefault;

	private XmlSchemaDerivationMethod blockDefault;

	private XmlSchemaDerivationMethod finalDefault;

	private Hashtable schemaLocations;

	private Hashtable referenceNamespaces;

	private string Xmlns;

	private const XmlSchemaDerivationMethod schemaBlockDefaultAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod schemaFinalDefaultAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

	private const XmlSchemaDerivationMethod elementBlockAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod elementFinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;

	private const XmlSchemaDerivationMethod simpleTypeFinalAllowed = XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;

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

	public SchemaCollectionPreprocessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
		: base(nameTable, schemaNames, eventHandler)
	{
	}

	public bool Execute(XmlSchema schema, string targetNamespace, bool loadExternals, XmlSchemaCollection xsc)
	{
		this.schema = schema;
		Xmlns = base.NameTable.Add("xmlns");
		Cleanup(schema);
		if (loadExternals && xmlResolver != null)
		{
			schemaLocations = new Hashtable();
			if (schema.BaseUri != null)
			{
				schemaLocations.Add(schema.BaseUri, schema.BaseUri);
			}
			LoadExternals(schema, xsc);
		}
		ValidateIdAttribute(schema);
		Preprocess(schema, targetNamespace, Compositor.Root);
		if (!base.HasErrors)
		{
			schema.IsPreprocessed = true;
			for (int i = 0; i < schema.Includes.Count; i++)
			{
				XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
				if (xmlSchemaExternal.Schema != null)
				{
					xmlSchemaExternal.Schema.IsPreprocessed = true;
				}
			}
		}
		return !base.HasErrors;
	}

	private void Cleanup(XmlSchema schema)
	{
		if (schema.IsProcessing)
		{
			return;
		}
		schema.IsProcessing = true;
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			if (xmlSchemaExternal.Schema != null)
			{
				Cleanup(xmlSchemaExternal.Schema);
			}
			if (xmlSchemaExternal is XmlSchemaRedefine)
			{
				XmlSchemaRedefine obj = xmlSchemaExternal as XmlSchemaRedefine;
				obj.AttributeGroups.Clear();
				obj.Groups.Clear();
				obj.SchemaTypes.Clear();
			}
		}
		schema.Attributes.Clear();
		schema.AttributeGroups.Clear();
		schema.SchemaTypes.Clear();
		schema.Elements.Clear();
		schema.Groups.Clear();
		schema.Notations.Clear();
		schema.Ids.Clear();
		schema.IdentityConstraints.Clear();
		schema.IsProcessing = false;
	}

	private void LoadExternals(XmlSchema schema, XmlSchemaCollection xsc)
	{
		if (schema.IsProcessing)
		{
			return;
		}
		schema.IsProcessing = true;
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			Uri uri = null;
			if (xmlSchemaExternal.Schema != null)
			{
				if (xmlSchemaExternal is XmlSchemaImport && ((XmlSchemaImport)xmlSchemaExternal).Namespace == "http://www.w3.org/XML/1998/namespace")
				{
					buildinIncluded = true;
					continue;
				}
				uri = xmlSchemaExternal.BaseUri;
				if (uri != null && schemaLocations[uri] == null)
				{
					schemaLocations.Add(uri, uri);
				}
				LoadExternals(xmlSchemaExternal.Schema, xsc);
				continue;
			}
			if (xsc != null && xmlSchemaExternal is XmlSchemaImport)
			{
				XmlSchemaImport xmlSchemaImport = (XmlSchemaImport)xmlSchemaExternal;
				string ns = ((xmlSchemaImport.Namespace != null) ? xmlSchemaImport.Namespace : string.Empty);
				xmlSchemaExternal.Schema = xsc[ns];
				if (xmlSchemaExternal.Schema != null)
				{
					xmlSchemaExternal.Schema = xmlSchemaExternal.Schema.Clone();
					if (xmlSchemaExternal.Schema.BaseUri != null && schemaLocations[xmlSchemaExternal.Schema.BaseUri] == null)
					{
						schemaLocations.Add(xmlSchemaExternal.Schema.BaseUri, xmlSchemaExternal.Schema.BaseUri);
					}
					Uri uri2 = null;
					for (int j = 0; j < xmlSchemaExternal.Schema.Includes.Count; j++)
					{
						XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)xmlSchemaExternal.Schema.Includes[j];
						if (!(xmlSchemaExternal2 is XmlSchemaImport))
						{
							continue;
						}
						XmlSchemaImport xmlSchemaImport2 = (XmlSchemaImport)xmlSchemaExternal2;
						uri2 = ((xmlSchemaImport2.BaseUri != null) ? xmlSchemaImport2.BaseUri : ((xmlSchemaImport2.Schema != null && xmlSchemaImport2.Schema.BaseUri != null) ? xmlSchemaImport2.Schema.BaseUri : null));
						if (uri2 != null)
						{
							if (schemaLocations[uri2] != null)
							{
								xmlSchemaImport2.Schema = null;
							}
							else
							{
								schemaLocations.Add(uri2, uri2);
							}
						}
					}
					continue;
				}
			}
			if (xmlSchemaExternal is XmlSchemaImport && ((XmlSchemaImport)xmlSchemaExternal).Namespace == "http://www.w3.org/XML/1998/namespace")
			{
				if (!buildinIncluded)
				{
					buildinIncluded = true;
					xmlSchemaExternal.Schema = Preprocessor.GetBuildInSchema();
				}
				continue;
			}
			string schemaLocation = xmlSchemaExternal.SchemaLocation;
			if (schemaLocation == null)
			{
				continue;
			}
			Uri uri3 = ResolveSchemaLocationUri(schema, schemaLocation);
			if (!(uri3 != null) || schemaLocations[uri3] != null)
			{
				continue;
			}
			Stream schemaEntity = GetSchemaEntity(uri3);
			if (schemaEntity != null)
			{
				xmlSchemaExternal.BaseUri = uri3;
				schemaLocations.Add(uri3, uri3);
				XmlTextReader xmlTextReader = new XmlTextReader(uri3.ToString(), schemaEntity, base.NameTable);
				xmlTextReader.XmlResolver = xmlResolver;
				try
				{
					Parser parser = new Parser(SchemaType.XSD, base.NameTable, base.SchemaNames, base.EventHandler);
					parser.Parse(xmlTextReader, null);
					while (xmlTextReader.Read())
					{
					}
					xmlSchemaExternal.Schema = parser.XmlSchema;
					LoadExternals(xmlSchemaExternal.Schema, xsc);
				}
				catch (XmlSchemaException ex)
				{
					SendValidationEventNoThrow(new XmlSchemaException("Cannot load the schema for the namespace '{0}' - {1}", new string[2] { schemaLocation, ex.Message }, ex.SourceUri, ex.LineNumber, ex.LinePosition), XmlSeverityType.Error);
				}
				catch (Exception)
				{
					SendValidationEvent("Cannot resolve the 'schemaLocation' attribute.", xmlSchemaExternal, XmlSeverityType.Warning);
				}
				finally
				{
					xmlTextReader.Close();
				}
			}
			else
			{
				SendValidationEvent("Cannot resolve the 'schemaLocation' attribute.", xmlSchemaExternal, XmlSeverityType.Warning);
			}
		}
		schema.IsProcessing = false;
	}

	private void BuildRefNamespaces(XmlSchema schema)
	{
		referenceNamespaces = new Hashtable();
		referenceNamespaces.Add("http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
		referenceNamespaces.Add(string.Empty, string.Empty);
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			if (schema.Includes[i] is XmlSchemaImport { Namespace: { } text } && referenceNamespaces[text] == null)
			{
				referenceNamespaces.Add(text, text);
			}
		}
		if (schema.TargetNamespace != null && referenceNamespaces[schema.TargetNamespace] == null)
		{
			referenceNamespaces.Add(schema.TargetNamespace, schema.TargetNamespace);
		}
	}

	private void Preprocess(XmlSchema schema, string targetNamespace, Compositor compositor)
	{
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
				try
				{
					XmlConvert.ToUri(text);
				}
				catch
				{
					SendValidationEvent("The Namespace '{0}' is an invalid URI.", schema.TargetNamespace, schema);
				}
			}
		}
		if (schema.Version != null)
		{
			try
			{
				XmlConvert.VerifyTOKEN(schema.Version);
			}
			catch (Exception)
			{
				SendValidationEvent("The '{0}' attribute has an invalid value according to its data type.", "version", schema);
			}
		}
		switch (compositor)
		{
		case Compositor.Root:
			if (targetNamespace == null && schema.TargetNamespace != null)
			{
				targetNamespace = schema.TargetNamespace;
			}
			else if (schema.TargetNamespace == null && targetNamespace != null && targetNamespace.Length == 0)
			{
				targetNamespace = null;
			}
			if (targetNamespace != schema.TargetNamespace)
			{
				SendValidationEvent("The targetNamespace parameter '{0}' should be the same value as the targetNamespace '{1}' of the schema.", targetNamespace, schema.TargetNamespace, schema);
			}
			break;
		case Compositor.Import:
			if (targetNamespace != schema.TargetNamespace)
			{
				SendValidationEvent("The namespace attribute '{0}' of an import should be the same value as the targetNamespace '{1}' of the imported schema.", targetNamespace, schema.TargetNamespace, schema);
			}
			break;
		case Compositor.Include:
			if (schema.TargetNamespace != null && targetNamespace != schema.TargetNamespace)
			{
				SendValidationEvent("The targetNamespace '{0}' of included/redefined schema should be the same as the targetNamespace '{1}' of the including schema.", targetNamespace, schema.TargetNamespace, schema);
			}
			break;
		}
		for (int i = 0; i < schema.Includes.Count; i++)
		{
			XmlSchemaExternal xmlSchemaExternal = (XmlSchemaExternal)schema.Includes[i];
			SetParent(xmlSchemaExternal, schema);
			PreprocessAnnotation(xmlSchemaExternal);
			string schemaLocation = xmlSchemaExternal.SchemaLocation;
			if (schemaLocation != null)
			{
				try
				{
					XmlConvert.ToUri(schemaLocation);
				}
				catch
				{
					SendValidationEvent("The SchemaLocation '{0}' is an invalid URI.", schemaLocation, xmlSchemaExternal);
				}
			}
			else if ((xmlSchemaExternal is XmlSchemaRedefine || xmlSchemaExternal is XmlSchemaInclude) && xmlSchemaExternal.Schema == null)
			{
				SendValidationEvent("The required attribute '{0}' is missing.", "schemaLocation", xmlSchemaExternal);
			}
			if (xmlSchemaExternal.Schema != null)
			{
				if (xmlSchemaExternal is XmlSchemaRedefine)
				{
					Preprocess(xmlSchemaExternal.Schema, schema.TargetNamespace, Compositor.Include);
				}
				else if (xmlSchemaExternal is XmlSchemaImport)
				{
					if (((XmlSchemaImport)xmlSchemaExternal).Namespace == null && schema.TargetNamespace == null)
					{
						SendValidationEvent("The enclosing <schema> must have a targetNamespace, if the Namespace attribute is absent on the import element.", xmlSchemaExternal);
					}
					else if (((XmlSchemaImport)xmlSchemaExternal).Namespace == schema.TargetNamespace)
					{
						SendValidationEvent("Namespace attribute of an import must not match the real value of the enclosing targetNamespace of the <schema>.", xmlSchemaExternal);
					}
					Preprocess(xmlSchemaExternal.Schema, ((XmlSchemaImport)xmlSchemaExternal).Namespace, Compositor.Import);
				}
				else
				{
					Preprocess(xmlSchemaExternal.Schema, schema.TargetNamespace, Compositor.Include);
				}
			}
			else
			{
				if (!(xmlSchemaExternal is XmlSchemaImport))
				{
					continue;
				}
				string text3 = ((XmlSchemaImport)xmlSchemaExternal).Namespace;
				if (text3 == null)
				{
					continue;
				}
				if (text3.Length == 0)
				{
					SendValidationEvent("The namespace attribute cannot have empty string as its value.", text3, xmlSchemaExternal);
					continue;
				}
				try
				{
					XmlConvert.ToUri(text3);
				}
				catch (FormatException)
				{
					SendValidationEvent("The Namespace '{0}' is an invalid URI.", text3, xmlSchemaExternal);
				}
			}
		}
		BuildRefNamespaces(schema);
		this.targetNamespace = ((targetNamespace == null) ? string.Empty : targetNamespace);
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
		for (int j = 0; j < schema.Includes.Count; j++)
		{
			XmlSchemaExternal xmlSchemaExternal2 = (XmlSchemaExternal)schema.Includes[j];
			if (xmlSchemaExternal2 is XmlSchemaRedefine)
			{
				XmlSchemaRedefine xmlSchemaRedefine = (XmlSchemaRedefine)xmlSchemaExternal2;
				if (xmlSchemaExternal2.Schema != null)
				{
					PreprocessRedefine(xmlSchemaRedefine);
				}
				else
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
			XmlSchema xmlSchema = xmlSchemaExternal2.Schema;
			if (xmlSchema != null)
			{
				foreach (XmlSchemaElement value in xmlSchema.Elements.Values)
				{
					AddToTable(schema.Elements, value.QualifiedName, value);
				}
				foreach (XmlSchemaAttribute value2 in xmlSchema.Attributes.Values)
				{
					AddToTable(schema.Attributes, value2.QualifiedName, value2);
				}
				foreach (XmlSchemaGroup value3 in xmlSchema.Groups.Values)
				{
					AddToTable(schema.Groups, value3.QualifiedName, value3);
				}
				foreach (XmlSchemaAttributeGroup value4 in xmlSchema.AttributeGroups.Values)
				{
					AddToTable(schema.AttributeGroups, value4.QualifiedName, value4);
				}
				foreach (XmlSchemaType value5 in xmlSchema.SchemaTypes.Values)
				{
					AddToTable(schema.SchemaTypes, value5.QualifiedName, value5);
				}
				foreach (XmlSchemaNotation value6 in xmlSchema.Notations.Values)
				{
					AddToTable(schema.Notations, value6.QualifiedName, value6);
				}
			}
			ValidateIdAttribute(xmlSchemaExternal2);
		}
		List<XmlSchemaObject> list = new List<XmlSchemaObject>();
		for (int l = 0; l < schema.Items.Count; l++)
		{
			SetParent(schema.Items[l], schema);
			if (schema.Items[l] is XmlSchemaAttribute xmlSchemaAttribute2)
			{
				PreprocessAttribute(xmlSchemaAttribute2);
				AddToTable(schema.Attributes, xmlSchemaAttribute2.QualifiedName, xmlSchemaAttribute2);
			}
			else if (schema.Items[l] is XmlSchemaAttributeGroup)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = (XmlSchemaAttributeGroup)schema.Items[l];
				PreprocessAttributeGroup(xmlSchemaAttributeGroup2);
				AddToTable(schema.AttributeGroups, xmlSchemaAttributeGroup2.QualifiedName, xmlSchemaAttributeGroup2);
			}
			else if (schema.Items[l] is XmlSchemaComplexType)
			{
				XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)schema.Items[l];
				PreprocessComplexType(xmlSchemaComplexType, local: false);
				AddToTable(schema.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
			}
			else if (schema.Items[l] is XmlSchemaSimpleType)
			{
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)schema.Items[l];
				PreprocessSimpleType(xmlSchemaSimpleType, local: false);
				AddToTable(schema.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
			}
			else if (schema.Items[l] is XmlSchemaElement)
			{
				XmlSchemaElement xmlSchemaElement2 = (XmlSchemaElement)schema.Items[l];
				PreprocessElement(xmlSchemaElement2);
				AddToTable(schema.Elements, xmlSchemaElement2.QualifiedName, xmlSchemaElement2);
			}
			else if (schema.Items[l] is XmlSchemaGroup)
			{
				XmlSchemaGroup xmlSchemaGroup2 = (XmlSchemaGroup)schema.Items[l];
				PreprocessGroup(xmlSchemaGroup2);
				AddToTable(schema.Groups, xmlSchemaGroup2.QualifiedName, xmlSchemaGroup2);
			}
			else if (schema.Items[l] is XmlSchemaNotation)
			{
				XmlSchemaNotation xmlSchemaNotation2 = (XmlSchemaNotation)schema.Items[l];
				PreprocessNotation(xmlSchemaNotation2);
				AddToTable(schema.Notations, xmlSchemaNotation2.QualifiedName, xmlSchemaNotation2);
			}
			else if (!(schema.Items[l] is XmlSchemaAnnotation))
			{
				SendValidationEvent("The schema items collection cannot contain an object of type 'XmlSchemaInclude', 'XmlSchemaImport', or 'XmlSchemaRedefine'.", schema.Items[l]);
				list.Add(schema.Items[l]);
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			schema.Items.Remove(list[m]);
		}
		schema.IsProcessing = false;
	}

	private void PreprocessRedefine(XmlSchemaRedefine redefine)
	{
		for (int i = 0; i < redefine.Items.Count; i++)
		{
			SetParent(redefine.Items[i], redefine);
			if (redefine.Items[i] is XmlSchemaGroup xmlSchemaGroup)
			{
				PreprocessGroup(xmlSchemaGroup);
				if (redefine.Groups[xmlSchemaGroup.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for group.", xmlSchemaGroup);
					continue;
				}
				AddToTable(redefine.Groups, xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
				xmlSchemaGroup.Redefined = (XmlSchemaGroup)redefine.Schema.Groups[xmlSchemaGroup.QualifiedName];
				if (xmlSchemaGroup.Redefined != null)
				{
					CheckRefinedGroup(xmlSchemaGroup);
				}
				else
				{
					SendValidationEvent("No group to redefine.", xmlSchemaGroup);
				}
			}
			else if (redefine.Items[i] is XmlSchemaAttributeGroup)
			{
				XmlSchemaAttributeGroup xmlSchemaAttributeGroup = (XmlSchemaAttributeGroup)redefine.Items[i];
				PreprocessAttributeGroup(xmlSchemaAttributeGroup);
				if (redefine.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for attribute group.", xmlSchemaAttributeGroup);
					continue;
				}
				AddToTable(redefine.AttributeGroups, xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
				xmlSchemaAttributeGroup.Redefined = (XmlSchemaAttributeGroup)redefine.Schema.AttributeGroups[xmlSchemaAttributeGroup.QualifiedName];
				if (xmlSchemaAttributeGroup.Redefined != null)
				{
					CheckRefinedAttributeGroup(xmlSchemaAttributeGroup);
				}
				else
				{
					SendValidationEvent("No attribute group to redefine.", xmlSchemaAttributeGroup);
				}
			}
			else if (redefine.Items[i] is XmlSchemaComplexType)
			{
				XmlSchemaComplexType xmlSchemaComplexType = (XmlSchemaComplexType)redefine.Items[i];
				PreprocessComplexType(xmlSchemaComplexType, local: false);
				if (redefine.SchemaTypes[xmlSchemaComplexType.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for complex type.", xmlSchemaComplexType);
					continue;
				}
				AddToTable(redefine.SchemaTypes, xmlSchemaComplexType.QualifiedName, xmlSchemaComplexType);
				XmlSchemaType xmlSchemaType = (XmlSchemaType)redefine.Schema.SchemaTypes[xmlSchemaComplexType.QualifiedName];
				if (xmlSchemaType != null)
				{
					if (xmlSchemaType is XmlSchemaComplexType)
					{
						xmlSchemaComplexType.Redefined = xmlSchemaType;
						CheckRefinedComplexType(xmlSchemaComplexType);
					}
					else
					{
						SendValidationEvent("Cannot redefine a simple type as complex type.", xmlSchemaComplexType);
					}
				}
				else
				{
					SendValidationEvent("No complex type to redefine.", xmlSchemaComplexType);
				}
			}
			else
			{
				if (!(redefine.Items[i] is XmlSchemaSimpleType))
				{
					continue;
				}
				XmlSchemaSimpleType xmlSchemaSimpleType = (XmlSchemaSimpleType)redefine.Items[i];
				PreprocessSimpleType(xmlSchemaSimpleType, local: false);
				if (redefine.SchemaTypes[xmlSchemaSimpleType.QualifiedName] != null)
				{
					SendValidationEvent("Double redefine for simple type.", xmlSchemaSimpleType);
					continue;
				}
				AddToTable(redefine.SchemaTypes, xmlSchemaSimpleType.QualifiedName, xmlSchemaSimpleType);
				XmlSchemaType xmlSchemaType2 = (XmlSchemaType)redefine.Schema.SchemaTypes[xmlSchemaSimpleType.QualifiedName];
				if (xmlSchemaType2 != null)
				{
					if (xmlSchemaType2 is XmlSchemaSimpleType)
					{
						xmlSchemaSimpleType.Redefined = xmlSchemaType2;
						CheckRefinedSimpleType(xmlSchemaSimpleType);
					}
					else
					{
						SendValidationEvent("Cannot redefine a complex type as simple type.", xmlSchemaSimpleType);
					}
				}
				else
				{
					SendValidationEvent("No simple type to redefine.", xmlSchemaSimpleType);
				}
			}
		}
		foreach (DictionaryEntry group in redefine.Groups)
		{
			redefine.Schema.Groups.Insert((XmlQualifiedName)group.Key, (XmlSchemaObject)group.Value);
		}
		foreach (DictionaryEntry attributeGroup in redefine.AttributeGroups)
		{
			redefine.Schema.AttributeGroups.Insert((XmlQualifiedName)attributeGroup.Key, (XmlSchemaObject)attributeGroup.Value);
		}
		foreach (DictionaryEntry schemaType in redefine.SchemaTypes)
		{
			redefine.Schema.SchemaTypes.Insert((XmlQualifiedName)schemaType.Key, (XmlSchemaObject)schemaType.Value);
		}
	}

	private int CountGroupSelfReference(XmlSchemaObjectCollection items, XmlQualifiedName name)
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i] is XmlSchemaGroupRef xmlSchemaGroupRef)
			{
				if (xmlSchemaGroupRef.RefName == name)
				{
					if (xmlSchemaGroupRef.MinOccurs != 1m || xmlSchemaGroupRef.MaxOccurs != 1m)
					{
						SendValidationEvent("When group is redefined, the real value of both minOccurs and maxOccurs attribute must be 1 (or absent).", xmlSchemaGroupRef);
					}
					num++;
				}
			}
			else if (items[i] is XmlSchemaGroupBase)
			{
				num += CountGroupSelfReference(((XmlSchemaGroupBase)items[i]).Items, name);
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
			num = CountGroupSelfReference(group.Particle.Items, group.QualifiedName);
		}
		if (num > 1)
		{
			SendValidationEvent("Multiple self-reference within a group is redefined.", group);
		}
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
		if (schema.TargetNamespace == "http://www.w3.org/2001/XMLSchema-instance")
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
			if (!element.SchemaTypeName.IsEmpty || element.IsAbstract || element.Block != XmlSchemaDerivationMethod.None || element.SchemaType != null || element.HasConstraints || element.DefaultValue != null || element.Form != XmlSchemaForm.None || element.FixedValue != null || element.HasNillableAttribute)
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
		if (element.IsAbstract)
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
			SetParent(element.Constraints[i], element);
			PreprocessIdentityConstraint((XmlSchemaIdentityConstraint)element.Constraints[i]);
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
		if (schema.IdentityConstraints[constraint.QualifiedName] != null)
		{
			SendValidationEvent("The identity constraint '{0}' has already been declared.", constraint.QualifiedName.ToString(), constraint);
			flag = false;
		}
		else
		{
			schema.IdentityConstraints.Add(constraint.QualifiedName, constraint);
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
					simpleType.SetFinalResolved(finalDefault & (XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
				}
			}
			else
			{
				if ((simpleType.Final & ~(XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union)) != XmlSchemaDerivationMethod.Empty)
				{
					SendValidationEvent("The values 'substitution' and 'extension' are invalid for the final attribute on simpleType.", simpleType);
				}
				simpleType.SetFinalResolved(simpleType.Final & (XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union));
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
				for (int j = 0; j < xmlSchemaSimpleTypeUnion.MemberTypes.Length; j++)
				{
					ValidateQNameAttribute(xmlSchemaSimpleTypeUnion, "memberTypes", xmlSchemaSimpleTypeUnion.MemberTypes[j]);
				}
			}
			if (num == 0)
			{
				SendValidationEvent("Either the memberTypes attribute must be non-empty or there must be at least one simpleType child.", xmlSchemaSimpleTypeUnion);
			}
			for (int k = 0; k < xmlSchemaSimpleTypeUnion.BaseTypes.Count; k++)
			{
				SetParent(xmlSchemaSimpleTypeUnion.BaseTypes[k], xmlSchemaSimpleTypeUnion);
				PreprocessSimpleType((XmlSchemaSimpleType)xmlSchemaSimpleTypeUnion.BaseTypes[k], local: true);
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
		if (notation.Public != null)
		{
			try
			{
				XmlConvert.ToUri(notation.Public);
			}
			catch
			{
				SendValidationEvent("Public attribute '{0}' is an invalid URI.", notation.Public, notation);
			}
		}
		else
		{
			SendValidationEvent("The required attribute '{0}' is missing.", "public", notation);
		}
		if (notation.System != null)
		{
			try
			{
				XmlConvert.ToUri(notation.System);
			}
			catch
			{
				SendValidationEvent("System attribute '{0}' is an invalid URI.", notation.System, notation);
			}
		}
		PreprocessAnnotation(notation);
		ValidateIdAttribute(notation);
	}

	private void PreprocessParticle(XmlSchemaParticle particle)
	{
		if (particle is XmlSchemaAll xmlSchemaAll)
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
			for (int i = 0; i < xmlSchemaAll.Items.Count; i++)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)xmlSchemaAll.Items[i];
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
			if (particle is XmlSchemaChoice { Items: var items })
			{
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
				XmlSchemaObjectCollection items2 = ((XmlSchemaSequence)particle).Items;
				for (int k = 0; k < items2.Count; k++)
				{
					SetParent(items2[k], particle);
					if (items2[k] is XmlSchemaElement element2)
					{
						PreprocessLocalElement(element2);
					}
					else
					{
						PreprocessParticle((XmlSchemaParticle)items2[k]);
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
					((XmlSchemaAny)particle).BuildNamespaceListV1Compat(targetNamespace);
				}
				catch
				{
					SendValidationEvent("Invalid namespace in 'any'.", particle);
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
				anyAttribute.BuildNamespaceListV1Compat(targetNamespace);
			}
			catch
			{
				SendValidationEvent("Invalid namespace in 'anyAttribute'.", anyAttribute);
			}
			ValidateIdAttribute(anyAttribute);
		}
	}

	private void ValidateIdAttribute(XmlSchemaObject xso)
	{
		if (xso.IdAttribute == null)
		{
			return;
		}
		try
		{
			xso.IdAttribute = base.NameTable.Add(XmlConvert.VerifyNCName(xso.IdAttribute));
			if (schema.Ids[xso.IdAttribute] != null)
			{
				SendValidationEvent("Duplicate ID attribute.", xso);
			}
			else
			{
				schema.Ids.Add(xso.IdAttribute, xso);
			}
		}
		catch (Exception ex)
		{
			SendValidationEvent("Invalid 'id' attribute value: {0}", ex.Message, xso);
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
			if (referenceNamespaces[value.Namespace] == null)
			{
				SendValidationEvent("Namespace '{0}' is not available to be referenced in this schema.", value.Namespace, xso, XmlSeverityType.Warning);
			}
		}
		catch (Exception ex)
		{
			SendValidationEvent("Invalid '{0}' attribute: '{1}'.", attributeName, ex.Message, xso);
		}
	}

	private void SetParent(XmlSchemaObject child, XmlSchemaObject parent)
	{
		child.Parent = parent;
	}

	private void PreprocessAnnotation(XmlSchemaObject schemaObject)
	{
		if (schemaObject is XmlSchemaAnnotated { Annotation: not null } xmlSchemaAnnotated)
		{
			xmlSchemaAnnotated.Annotation.Parent = schemaObject;
			for (int i = 0; i < xmlSchemaAnnotated.Annotation.Items.Count; i++)
			{
				xmlSchemaAnnotated.Annotation.Items[i].Parent = xmlSchemaAnnotated.Annotation;
			}
		}
	}

	private Uri ResolveSchemaLocationUri(XmlSchema enclosingSchema, string location)
	{
		try
		{
			return xmlResolver.ResolveUri(enclosingSchema.BaseUri, location);
		}
		catch
		{
			return null;
		}
	}

	private Stream GetSchemaEntity(Uri ruri)
	{
		try
		{
			return (Stream)xmlResolver.GetEntity(ruri, null, null);
		}
		catch
		{
			return null;
		}
	}
}
