using System.Collections;
using System.Text;

namespace System.Xml.Schema;

internal sealed class DtdValidator : BaseValidator
{
	private class NamespaceManager : XmlNamespaceManager
	{
		public override string LookupNamespace(string prefix)
		{
			return prefix;
		}
	}

	private static NamespaceManager namespaceManager = new NamespaceManager();

	private const int STACK_INCREMENT = 10;

	private HWStack validationStack;

	private Hashtable attPresence;

	private XmlQualifiedName name = XmlQualifiedName.Empty;

	private Hashtable IDs;

	private IdRefNode idRefListHead;

	private bool processIdentityConstraints;

	public override bool PreserveWhitespace
	{
		get
		{
			if (context.ElementDecl == null)
			{
				return false;
			}
			return context.ElementDecl.ContentValidator.PreserveWhitespace;
		}
	}

	internal DtdValidator(XmlValidatingReaderImpl reader, IValidationEventHandling eventHandling, bool processIdentityConstraints)
		: base(reader, null, eventHandling)
	{
		this.processIdentityConstraints = processIdentityConstraints;
		Init();
	}

	private void Init()
	{
		validationStack = new HWStack(10);
		textValue = new StringBuilder();
		name = XmlQualifiedName.Empty;
		attPresence = new Hashtable();
		schemaInfo = new SchemaInfo();
		checkDatatype = false;
		Push(name);
	}

	public override void Validate()
	{
		if (schemaInfo.SchemaType == SchemaType.DTD)
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Element:
				ValidateElement();
				if (reader.IsEmptyElement)
				{
					goto case XmlNodeType.EndElement;
				}
				break;
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if (MeetsStandAloneConstraint())
				{
					ValidateWhitespace();
				}
				break;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
				ValidatePIComment();
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				ValidateText();
				break;
			case XmlNodeType.EntityReference:
				if (!GenEntity(new XmlQualifiedName(reader.LocalName, reader.Prefix)))
				{
					ValidateText();
				}
				break;
			case XmlNodeType.EndElement:
				ValidateEndElement();
				break;
			case XmlNodeType.Attribute:
			case XmlNodeType.Entity:
			case XmlNodeType.Document:
			case XmlNodeType.DocumentType:
			case XmlNodeType.DocumentFragment:
			case XmlNodeType.Notation:
				break;
			}
		}
		else if (reader.Depth == 0 && reader.NodeType == XmlNodeType.Element)
		{
			SendValidationEvent("No DTD found.", name.ToString(), XmlSeverityType.Warning);
		}
	}

	private bool MeetsStandAloneConstraint()
	{
		if (reader.StandAlone && context.ElementDecl != null && context.ElementDecl.IsDeclaredInExternal && context.ElementDecl.ContentValidator.ContentType == XmlSchemaContentType.ElementOnly)
		{
			SendValidationEvent("The standalone document declaration must have a value of 'no'.");
			return false;
		}
		return true;
	}

	private void ValidatePIComment()
	{
		if (context.NeedValidateChildren && context.ElementDecl.ContentValidator == ContentValidator.Empty)
		{
			SendValidationEvent("The element cannot contain comment or processing instruction. Content model is empty.");
		}
	}

	private void ValidateElement()
	{
		elementName.Init(reader.LocalName, reader.Prefix);
		if (reader.Depth == 0 && !schemaInfo.DocTypeName.IsEmpty && !schemaInfo.DocTypeName.Equals(elementName))
		{
			SendValidationEvent("Root element name must match the DocType name.");
		}
		else
		{
			ValidateChildElement();
		}
		ProcessElement();
	}

	private void ValidateChildElement()
	{
		if (context.NeedValidateChildren)
		{
			int errorCode = 0;
			context.ElementDecl.ContentValidator.ValidateElement(elementName, context, out errorCode);
			if (errorCode < 0)
			{
				XmlSchemaValidator.ElementValidationError(elementName, context, base.EventHandler, reader, reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition, null);
			}
		}
	}

	private void ValidateStartElement()
	{
		if (context.ElementDecl != null)
		{
			base.Reader.SchemaTypeObject = context.ElementDecl.SchemaType;
			if (base.Reader.IsEmptyElement && context.ElementDecl.DefaultValueTyped != null)
			{
				base.Reader.TypedValueObject = context.ElementDecl.DefaultValueTyped;
				context.IsNill = true;
			}
			if (context.ElementDecl.HasRequiredAttribute)
			{
				attPresence.Clear();
			}
		}
		if (!base.Reader.MoveToFirstAttribute())
		{
			return;
		}
		do
		{
			try
			{
				reader.SchemaTypeObject = null;
				SchemaAttDef attDef = context.ElementDecl.GetAttDef(new XmlQualifiedName(reader.LocalName, reader.Prefix));
				if (attDef != null)
				{
					if (context.ElementDecl != null && context.ElementDecl.HasRequiredAttribute)
					{
						attPresence.Add(attDef.Name, attDef);
					}
					base.Reader.SchemaTypeObject = attDef.SchemaType;
					if (attDef.Datatype != null && !reader.IsDefault)
					{
						CheckValue(base.Reader.Value, attDef);
					}
				}
				else
				{
					SendValidationEvent("The '{0}' attribute is not declared.", reader.Name);
				}
			}
			catch (XmlSchemaException ex)
			{
				ex.SetSource(base.Reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
				SendValidationEvent(ex);
			}
		}
		while (base.Reader.MoveToNextAttribute());
		base.Reader.MoveToElement();
	}

	private void ValidateEndStartElement()
	{
		if (context.ElementDecl.HasRequiredAttribute)
		{
			try
			{
				context.ElementDecl.CheckAttributes(attPresence, base.Reader.StandAlone);
			}
			catch (XmlSchemaException ex)
			{
				ex.SetSource(base.Reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
				SendValidationEvent(ex);
			}
		}
		if (context.ElementDecl.Datatype != null)
		{
			checkDatatype = true;
			hasSibling = false;
			textString = string.Empty;
			textValue.Length = 0;
		}
	}

	private void ProcessElement()
	{
		SchemaElementDecl elementDecl = schemaInfo.GetElementDecl(elementName);
		Push(elementName);
		if (elementDecl != null)
		{
			context.ElementDecl = elementDecl;
			ValidateStartElement();
			ValidateEndStartElement();
			context.NeedValidateChildren = true;
			elementDecl.ContentValidator.InitValidation(context);
		}
		else
		{
			SendValidationEvent("The '{0}' element is not declared.", XmlSchemaValidator.QNameString(context.LocalName, context.Namespace));
			context.ElementDecl = null;
		}
	}

	public override void CompleteValidation()
	{
		if (schemaInfo.SchemaType == SchemaType.DTD)
		{
			do
			{
				ValidateEndElement();
			}
			while (Pop());
			CheckForwardRefs();
		}
	}

	private void ValidateEndElement()
	{
		if (context.ElementDecl != null)
		{
			if (context.NeedValidateChildren && !context.ElementDecl.ContentValidator.CompleteValidation(context))
			{
				XmlSchemaValidator.CompleteValidationError(context, base.EventHandler, reader, reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition, null);
			}
			if (checkDatatype)
			{
				string value = ((!hasSibling) ? textString : textValue.ToString());
				CheckValue(value, null);
				checkDatatype = false;
				textValue.Length = 0;
				textString = string.Empty;
			}
		}
		Pop();
	}

	private void ProcessTokenizedType(XmlTokenizedType ttype, string name)
	{
		switch (ttype)
		{
		case XmlTokenizedType.ID:
			if (processIdentityConstraints)
			{
				if (FindId(name) != null)
				{
					SendValidationEvent("'{0}' is already used as an ID.", name);
				}
				else
				{
					AddID(name, context.LocalName);
				}
			}
			break;
		case XmlTokenizedType.IDREF:
			if (processIdentityConstraints && FindId(name) == null)
			{
				idRefListHead = new IdRefNode(idRefListHead, name, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
			}
			break;
		case XmlTokenizedType.ENTITY:
			BaseValidator.ProcessEntity(schemaInfo, name, this, base.EventHandler, base.Reader.BaseURI, base.PositionInfo.LineNumber, base.PositionInfo.LinePosition);
			break;
		case XmlTokenizedType.IDREFS:
			break;
		}
	}

	private void CheckValue(string value, SchemaAttDef attdef)
	{
		try
		{
			reader.TypedValueObject = null;
			bool flag = attdef != null;
			XmlSchemaDatatype xmlSchemaDatatype = (flag ? attdef.Datatype : context.ElementDecl.Datatype);
			if (xmlSchemaDatatype == null)
			{
				return;
			}
			if (xmlSchemaDatatype.TokenizedType != XmlTokenizedType.CDATA)
			{
				value = value.Trim();
			}
			object obj = xmlSchemaDatatype.ParseValue(value, base.NameTable, namespaceManager);
			reader.TypedValueObject = obj;
			XmlTokenizedType tokenizedType = xmlSchemaDatatype.TokenizedType;
			if (tokenizedType == XmlTokenizedType.ENTITY || tokenizedType == XmlTokenizedType.ID || tokenizedType == XmlTokenizedType.IDREF)
			{
				if (xmlSchemaDatatype.Variety == XmlSchemaDatatypeVariety.List)
				{
					string[] array = (string[])obj;
					for (int i = 0; i < array.Length; i++)
					{
						ProcessTokenizedType(xmlSchemaDatatype.TokenizedType, array[i]);
					}
				}
				else
				{
					ProcessTokenizedType(xmlSchemaDatatype.TokenizedType, (string)obj);
				}
			}
			SchemaDeclBase schemaDeclBase = (flag ? ((SchemaDeclBase)attdef) : ((SchemaDeclBase)context.ElementDecl));
			if (schemaDeclBase.Values != null && !schemaDeclBase.CheckEnumeration(obj))
			{
				if (xmlSchemaDatatype.TokenizedType == XmlTokenizedType.NOTATION)
				{
					SendValidationEvent("'{0}' is not in the notation list.", obj.ToString());
				}
				else
				{
					SendValidationEvent("'{0}' is not in the enumeration list.", obj.ToString());
				}
			}
			if (!schemaDeclBase.CheckValue(obj))
			{
				if (flag)
				{
					SendValidationEvent("The value of the '{0}' attribute does not equal its fixed value.", attdef.Name.ToString());
				}
				else
				{
					SendValidationEvent("The value of the '{0}' element does not equal its fixed value.", XmlSchemaValidator.QNameString(context.LocalName, context.Namespace));
				}
			}
		}
		catch (XmlSchemaException)
		{
			if (attdef != null)
			{
				SendValidationEvent("The '{0}' attribute has an invalid value according to its data type.", attdef.Name.ToString());
			}
			else
			{
				SendValidationEvent("The '{0}' element has an invalid value according to its data type.", XmlSchemaValidator.QNameString(context.LocalName, context.Namespace));
			}
		}
	}

	internal void AddID(string name, object node)
	{
		if (IDs == null)
		{
			IDs = new Hashtable();
		}
		IDs.Add(name, node);
	}

	public override object FindId(string name)
	{
		if (IDs != null)
		{
			return IDs[name];
		}
		return null;
	}

	private bool GenEntity(XmlQualifiedName qname)
	{
		string text = qname.Name;
		if (text[0] == '#')
		{
			return false;
		}
		if (SchemaEntity.IsPredefinedEntity(text))
		{
			return false;
		}
		SchemaEntity entity = GetEntity(qname, fParameterEntity: false);
		if (entity == null)
		{
			throw new XmlException("Reference to undeclared entity '{0}'.", text);
		}
		if (!entity.NData.IsEmpty)
		{
			throw new XmlException("Reference to unparsed entity '{0}'.", text);
		}
		if (reader.StandAlone && entity.DeclaredInExternal)
		{
			SendValidationEvent("The standalone document declaration must have a value of 'no'.");
		}
		return true;
	}

	private SchemaEntity GetEntity(XmlQualifiedName qname, bool fParameterEntity)
	{
		SchemaEntity value;
		if (fParameterEntity)
		{
			if (schemaInfo.ParameterEntities.TryGetValue(qname, out value))
			{
				return value;
			}
		}
		else if (schemaInfo.GeneralEntities.TryGetValue(qname, out value))
		{
			return value;
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
				SendValidationEvent(new XmlSchemaException("Reference to undeclared ID is '{0}'.", idRefNode.Id, reader.BaseURI, idRefNode.LineNo, idRefNode.LinePos));
			}
			IdRefNode next = idRefNode.Next;
			idRefNode.Next = null;
			idRefNode = next;
		}
		idRefListHead = null;
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
		context.NeedValidateChildren = false;
	}

	private bool Pop()
	{
		if (validationStack.Length > 1)
		{
			validationStack.Pop();
			context = (ValidationState)validationStack.Peek();
			return true;
		}
		return false;
	}

	public static void SetDefaultTypedValue(SchemaAttDef attdef, IDtdParserAdapter readerAdapter)
	{
		try
		{
			string text = attdef.DefaultValueExpanded;
			XmlSchemaDatatype datatype = attdef.Datatype;
			if (datatype != null)
			{
				if (datatype.TokenizedType != XmlTokenizedType.CDATA)
				{
					text = text.Trim();
				}
				attdef.DefaultValueTyped = datatype.ParseValue(text, readerAdapter.NameTable, readerAdapter.NamespaceResolver);
			}
		}
		catch (Exception)
		{
			IValidationEventHandling validationEventHandling = ((IDtdParserAdapterWithValidation)readerAdapter).ValidationEventHandling;
			if (validationEventHandling != null)
			{
				XmlSchemaException exception = new XmlSchemaException("The default value of '{0}' attribute is invalid according to its datatype.", attdef.Name.ToString());
				validationEventHandling.SendEvent(exception, XmlSeverityType.Error);
			}
		}
	}

	public static void CheckDefaultValue(SchemaAttDef attdef, SchemaInfo sinfo, IValidationEventHandling eventHandling, string baseUriStr)
	{
		try
		{
			if (baseUriStr == null)
			{
				baseUriStr = string.Empty;
			}
			XmlSchemaDatatype datatype = attdef.Datatype;
			if (datatype == null)
			{
				return;
			}
			object defaultValueTyped = attdef.DefaultValueTyped;
			switch (datatype.TokenizedType)
			{
			case XmlTokenizedType.ENTITY:
				if (datatype.Variety == XmlSchemaDatatypeVariety.List)
				{
					string[] array = (string[])defaultValueTyped;
					for (int i = 0; i < array.Length; i++)
					{
						BaseValidator.ProcessEntity(sinfo, array[i], eventHandling, baseUriStr, attdef.ValueLineNumber, attdef.ValueLinePosition);
					}
				}
				else
				{
					BaseValidator.ProcessEntity(sinfo, (string)defaultValueTyped, eventHandling, baseUriStr, attdef.ValueLineNumber, attdef.ValueLinePosition);
				}
				break;
			case XmlTokenizedType.ENUMERATION:
				if (!attdef.CheckEnumeration(defaultValueTyped) && eventHandling != null)
				{
					XmlSchemaException exception = new XmlSchemaException("'{0}' is not in the enumeration list.", defaultValueTyped.ToString(), baseUriStr, attdef.ValueLineNumber, attdef.ValueLinePosition);
					eventHandling.SendEvent(exception, XmlSeverityType.Error);
				}
				break;
			}
		}
		catch (Exception)
		{
			if (eventHandling != null)
			{
				XmlSchemaException exception2 = new XmlSchemaException("The default value of '{0}' attribute is invalid according to its datatype.", attdef.Name.ToString());
				eventHandling.SendEvent(exception2, XmlSeverityType.Error);
			}
		}
	}
}
