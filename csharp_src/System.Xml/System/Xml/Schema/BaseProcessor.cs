namespace System.Xml.Schema;

internal class BaseProcessor
{
	private XmlNameTable nameTable;

	private SchemaNames schemaNames;

	private ValidationEventHandler eventHandler;

	private XmlSchemaCompilationSettings compilationSettings;

	private int errorCount;

	private string NsXml;

	protected XmlNameTable NameTable => nameTable;

	protected SchemaNames SchemaNames
	{
		get
		{
			if (schemaNames == null)
			{
				schemaNames = new SchemaNames(nameTable);
			}
			return schemaNames;
		}
	}

	protected ValidationEventHandler EventHandler => eventHandler;

	protected XmlSchemaCompilationSettings CompilationSettings => compilationSettings;

	protected bool HasErrors => errorCount != 0;

	public BaseProcessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
		: this(nameTable, schemaNames, eventHandler, new XmlSchemaCompilationSettings())
	{
	}

	public BaseProcessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler, XmlSchemaCompilationSettings compilationSettings)
	{
		this.nameTable = nameTable;
		this.schemaNames = schemaNames;
		this.eventHandler = eventHandler;
		this.compilationSettings = compilationSettings;
		NsXml = nameTable.Add("http://www.w3.org/XML/1998/namespace");
	}

	protected void AddToTable(XmlSchemaObjectTable table, XmlQualifiedName qname, XmlSchemaObject item)
	{
		if (qname.Name.Length == 0)
		{
			return;
		}
		XmlSchemaObject xmlSchemaObject = table[qname];
		if (xmlSchemaObject != null)
		{
			if (xmlSchemaObject == item)
			{
				return;
			}
			string code = "The global element '{0}' has already been declared.";
			if (item is XmlSchemaAttributeGroup)
			{
				if (Ref.Equal(nameTable.Add(qname.Namespace), NsXml))
				{
					XmlSchemaObject xmlSchemaObject2 = Preprocessor.GetBuildInSchema().AttributeGroups[qname];
					if (xmlSchemaObject == xmlSchemaObject2)
					{
						table.Insert(qname, item);
						return;
					}
					if (item == xmlSchemaObject2)
					{
						return;
					}
				}
				else if (IsValidAttributeGroupRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				code = "The attributeGroup '{0}' has already been declared.";
			}
			else if (item is XmlSchemaAttribute)
			{
				if (Ref.Equal(nameTable.Add(qname.Namespace), NsXml))
				{
					XmlSchemaObject xmlSchemaObject3 = Preprocessor.GetBuildInSchema().Attributes[qname];
					if (xmlSchemaObject == xmlSchemaObject3)
					{
						table.Insert(qname, item);
						return;
					}
					if (item == xmlSchemaObject3)
					{
						return;
					}
				}
				code = "The global attribute '{0}' has already been declared.";
			}
			else if (item is XmlSchemaSimpleType)
			{
				if (IsValidTypeRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				code = "The simpleType '{0}' has already been declared.";
			}
			else if (item is XmlSchemaComplexType)
			{
				if (IsValidTypeRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				code = "The complexType '{0}' has already been declared.";
			}
			else if (item is XmlSchemaGroup)
			{
				if (IsValidGroupRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				code = "The group '{0}' has already been declared.";
			}
			else if (item is XmlSchemaNotation)
			{
				code = "The notation '{0}' has already been declared.";
			}
			else if (item is XmlSchemaIdentityConstraint)
			{
				code = "The identity constraint '{0}' has already been declared.";
			}
			SendValidationEvent(code, qname.ToString(), item);
		}
		else
		{
			table.Add(qname, item);
		}
	}

	private bool IsValidAttributeGroupRedefine(XmlSchemaObject existingObject, XmlSchemaObject item, XmlSchemaObjectTable table)
	{
		XmlSchemaAttributeGroup xmlSchemaAttributeGroup = item as XmlSchemaAttributeGroup;
		XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = existingObject as XmlSchemaAttributeGroup;
		if (xmlSchemaAttributeGroup2 == xmlSchemaAttributeGroup.Redefined)
		{
			if (xmlSchemaAttributeGroup2.AttributeUses.Count == 0)
			{
				table.Insert(xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
				return true;
			}
		}
		else if (xmlSchemaAttributeGroup2.Redefined == xmlSchemaAttributeGroup)
		{
			return true;
		}
		return false;
	}

	private bool IsValidGroupRedefine(XmlSchemaObject existingObject, XmlSchemaObject item, XmlSchemaObjectTable table)
	{
		XmlSchemaGroup xmlSchemaGroup = item as XmlSchemaGroup;
		XmlSchemaGroup xmlSchemaGroup2 = existingObject as XmlSchemaGroup;
		if (xmlSchemaGroup2 == xmlSchemaGroup.Redefined)
		{
			if (xmlSchemaGroup2.CanonicalParticle == null)
			{
				table.Insert(xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
				return true;
			}
		}
		else if (xmlSchemaGroup2.Redefined == xmlSchemaGroup)
		{
			return true;
		}
		return false;
	}

	private bool IsValidTypeRedefine(XmlSchemaObject existingObject, XmlSchemaObject item, XmlSchemaObjectTable table)
	{
		XmlSchemaType xmlSchemaType = item as XmlSchemaType;
		XmlSchemaType xmlSchemaType2 = existingObject as XmlSchemaType;
		if (xmlSchemaType2 == xmlSchemaType.Redefined)
		{
			if (xmlSchemaType2.ElementDecl == null)
			{
				table.Insert(xmlSchemaType.QualifiedName, xmlSchemaType);
				return true;
			}
		}
		else if (xmlSchemaType2.Redefined == xmlSchemaType)
		{
			return true;
		}
		return false;
	}

	protected void SendValidationEvent(string code, XmlSchemaObject source)
	{
		SendValidationEvent(new XmlSchemaException(code, source), XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, string msg, XmlSchemaObject source)
	{
		SendValidationEvent(new XmlSchemaException(code, msg, source), XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, string msg1, string msg2, XmlSchemaObject source)
	{
		SendValidationEvent(new XmlSchemaException(code, new string[2] { msg1, msg2 }, source), XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, string[] args, Exception innerException, XmlSchemaObject source)
	{
		SendValidationEvent(new XmlSchemaException(code, args, innerException, source.SourceUri, source.LineNumber, source.LinePosition, source), XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, string msg1, string msg2, string sourceUri, int lineNumber, int linePosition)
	{
		SendValidationEvent(new XmlSchemaException(code, new string[2] { msg1, msg2 }, sourceUri, lineNumber, linePosition), XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, XmlSchemaObject source, XmlSeverityType severity)
	{
		SendValidationEvent(new XmlSchemaException(code, source), severity);
	}

	protected void SendValidationEvent(XmlSchemaException e)
	{
		SendValidationEvent(e, XmlSeverityType.Error);
	}

	protected void SendValidationEvent(string code, string msg, XmlSchemaObject source, XmlSeverityType severity)
	{
		SendValidationEvent(new XmlSchemaException(code, msg, source), severity);
	}

	protected void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
	{
		if (severity == XmlSeverityType.Error)
		{
			errorCount++;
		}
		if (eventHandler != null)
		{
			eventHandler(null, new ValidationEventArgs(e, severity));
		}
		else if (severity == XmlSeverityType.Error)
		{
			throw e;
		}
	}

	protected void SendValidationEventNoThrow(XmlSchemaException e, XmlSeverityType severity)
	{
		if (severity == XmlSeverityType.Error)
		{
			errorCount++;
		}
		if (eventHandler != null)
		{
			eventHandler(null, new ValidationEventArgs(e, severity));
		}
	}
}
