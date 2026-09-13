using System.Collections.Generic;

namespace System.Xml.Schema;

internal class SchemaInfo : IDtdInfo
{
	private Dictionary<XmlQualifiedName, SchemaElementDecl> elementDecls = new Dictionary<XmlQualifiedName, SchemaElementDecl>();

	private Dictionary<XmlQualifiedName, SchemaElementDecl> undeclaredElementDecls = new Dictionary<XmlQualifiedName, SchemaElementDecl>();

	private Dictionary<XmlQualifiedName, SchemaEntity> generalEntities;

	private Dictionary<XmlQualifiedName, SchemaEntity> parameterEntities;

	private XmlQualifiedName docTypeName = XmlQualifiedName.Empty;

	private string internalDtdSubset = string.Empty;

	private bool hasNonCDataAttributes;

	private bool hasDefaultAttributes;

	private Dictionary<string, bool> targetNamespaces = new Dictionary<string, bool>();

	private Dictionary<XmlQualifiedName, SchemaAttDef> attributeDecls = new Dictionary<XmlQualifiedName, SchemaAttDef>();

	private int errorCount;

	private SchemaType schemaType;

	private Dictionary<XmlQualifiedName, SchemaElementDecl> elementDeclsByType = new Dictionary<XmlQualifiedName, SchemaElementDecl>();

	private Dictionary<string, SchemaNotation> notations;

	public XmlQualifiedName DocTypeName
	{
		get
		{
			return docTypeName;
		}
		set
		{
			docTypeName = value;
		}
	}

	internal string InternalDtdSubset
	{
		get
		{
			return internalDtdSubset;
		}
		set
		{
			internalDtdSubset = value;
		}
	}

	internal Dictionary<XmlQualifiedName, SchemaElementDecl> ElementDecls => elementDecls;

	internal Dictionary<XmlQualifiedName, SchemaElementDecl> UndeclaredElementDecls => undeclaredElementDecls;

	internal Dictionary<XmlQualifiedName, SchemaEntity> GeneralEntities
	{
		get
		{
			if (generalEntities == null)
			{
				generalEntities = new Dictionary<XmlQualifiedName, SchemaEntity>();
			}
			return generalEntities;
		}
	}

	internal Dictionary<XmlQualifiedName, SchemaEntity> ParameterEntities
	{
		get
		{
			if (parameterEntities == null)
			{
				parameterEntities = new Dictionary<XmlQualifiedName, SchemaEntity>();
			}
			return parameterEntities;
		}
	}

	internal SchemaType SchemaType
	{
		get
		{
			return schemaType;
		}
		set
		{
			schemaType = value;
		}
	}

	internal Dictionary<string, bool> TargetNamespaces => targetNamespaces;

	internal Dictionary<XmlQualifiedName, SchemaElementDecl> ElementDeclsByType => elementDeclsByType;

	internal Dictionary<XmlQualifiedName, SchemaAttDef> AttributeDecls => attributeDecls;

	internal Dictionary<string, SchemaNotation> Notations
	{
		get
		{
			if (notations == null)
			{
				notations = new Dictionary<string, SchemaNotation>();
			}
			return notations;
		}
	}

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

	bool IDtdInfo.HasDefaultAttributes => hasDefaultAttributes;

	bool IDtdInfo.HasNonCDataAttributes => hasNonCDataAttributes;

	XmlQualifiedName IDtdInfo.Name => docTypeName;

	string IDtdInfo.InternalDtdSubset => internalDtdSubset;

	internal SchemaInfo()
	{
		schemaType = SchemaType.None;
	}

	internal SchemaElementDecl GetElementDecl(XmlQualifiedName qname)
	{
		if (elementDecls.TryGetValue(qname, out var value))
		{
			return value;
		}
		return null;
	}

	internal SchemaElementDecl GetTypeDecl(XmlQualifiedName qname)
	{
		if (elementDeclsByType.TryGetValue(qname, out var value))
		{
			return value;
		}
		return null;
	}

	internal XmlSchemaElement GetElement(XmlQualifiedName qname)
	{
		return GetElementDecl(qname)?.SchemaElement;
	}

	internal XmlSchemaAttribute GetAttribute(XmlQualifiedName qname)
	{
		return attributeDecls[qname]?.SchemaAttribute;
	}

	internal XmlSchemaElement GetType(XmlQualifiedName qname)
	{
		return GetElementDecl(qname)?.SchemaElement;
	}

	internal bool HasSchema(string ns)
	{
		return targetNamespaces.ContainsKey(ns);
	}

	internal bool Contains(string ns)
	{
		return targetNamespaces.ContainsKey(ns);
	}

	internal SchemaAttDef GetAttributeXdr(SchemaElementDecl ed, XmlQualifiedName qname)
	{
		SchemaAttDef value = null;
		if (ed != null)
		{
			value = ed.GetAttDef(qname);
			if (value == null)
			{
				if (!ed.ContentValidator.IsOpen || qname.Namespace.Length == 0)
				{
					throw new XmlSchemaException("The '{0}' attribute is not declared.", qname.ToString());
				}
				if (!attributeDecls.TryGetValue(qname, out value) && targetNamespaces.ContainsKey(qname.Namespace))
				{
					throw new XmlSchemaException("The '{0}' attribute is not declared.", qname.ToString());
				}
			}
		}
		return value;
	}

	internal SchemaAttDef GetAttributeXsd(SchemaElementDecl ed, XmlQualifiedName qname, XmlSchemaObject partialValidationType, out AttributeMatchState attributeMatchState)
	{
		SchemaAttDef value = null;
		attributeMatchState = AttributeMatchState.UndeclaredAttribute;
		if (ed != null)
		{
			value = ed.GetAttDef(qname);
			if (value != null)
			{
				attributeMatchState = AttributeMatchState.AttributeFound;
				return value;
			}
			XmlSchemaAnyAttribute anyAttribute = ed.AnyAttribute;
			if (anyAttribute != null)
			{
				if (!anyAttribute.NamespaceList.Allows(qname))
				{
					attributeMatchState = AttributeMatchState.ProhibitedAnyAttribute;
				}
				else if (anyAttribute.ProcessContentsCorrect != XmlSchemaContentProcessing.Skip)
				{
					if (attributeDecls.TryGetValue(qname, out value))
					{
						if (value.Datatype.TypeCode == XmlTypeCode.Id)
						{
							attributeMatchState = AttributeMatchState.AnyIdAttributeFound;
						}
						else
						{
							attributeMatchState = AttributeMatchState.AttributeFound;
						}
					}
					else if (anyAttribute.ProcessContentsCorrect == XmlSchemaContentProcessing.Lax)
					{
						attributeMatchState = AttributeMatchState.AnyAttributeLax;
					}
				}
				else
				{
					attributeMatchState = AttributeMatchState.AnyAttributeSkip;
				}
			}
			else if (ed.ProhibitedAttributes.ContainsKey(qname))
			{
				attributeMatchState = AttributeMatchState.ProhibitedAttribute;
			}
		}
		else if (partialValidationType != null)
		{
			if (partialValidationType is XmlSchemaAttribute xmlSchemaAttribute)
			{
				if (qname.Equals(xmlSchemaAttribute.QualifiedName))
				{
					value = xmlSchemaAttribute.AttDef;
					attributeMatchState = AttributeMatchState.AttributeFound;
				}
				else
				{
					attributeMatchState = AttributeMatchState.AttributeNameMismatch;
				}
			}
			else
			{
				attributeMatchState = AttributeMatchState.ValidateAttributeInvalidCall;
			}
		}
		else if (attributeDecls.TryGetValue(qname, out value))
		{
			attributeMatchState = AttributeMatchState.AttributeFound;
		}
		else
		{
			attributeMatchState = AttributeMatchState.UndeclaredElementAndAttribute;
		}
		return value;
	}

	internal SchemaAttDef GetAttributeXsd(SchemaElementDecl ed, XmlQualifiedName qname, ref bool skip)
	{
		AttributeMatchState attributeMatchState;
		SchemaAttDef attributeXsd = GetAttributeXsd(ed, qname, null, out attributeMatchState);
		switch (attributeMatchState)
		{
		case AttributeMatchState.UndeclaredAttribute:
			throw new XmlSchemaException("The '{0}' attribute is not declared.", qname.ToString());
		case AttributeMatchState.ProhibitedAnyAttribute:
		case AttributeMatchState.ProhibitedAttribute:
			throw new XmlSchemaException("The '{0}' attribute is not allowed.", qname.ToString());
		case AttributeMatchState.AnyAttributeSkip:
			skip = true;
			break;
		}
		return attributeXsd;
	}

	internal void Add(SchemaInfo sinfo, ValidationEventHandler eventhandler)
	{
		if (schemaType == SchemaType.None)
		{
			schemaType = sinfo.SchemaType;
		}
		else if (schemaType != sinfo.SchemaType)
		{
			eventhandler?.Invoke(this, new ValidationEventArgs(new XmlSchemaException("Different schema types cannot be mixed.", string.Empty)));
			return;
		}
		foreach (string key in sinfo.TargetNamespaces.Keys)
		{
			if (!targetNamespaces.ContainsKey(key))
			{
				targetNamespaces.Add(key, value: true);
			}
		}
		foreach (KeyValuePair<XmlQualifiedName, SchemaElementDecl> elementDecl in sinfo.elementDecls)
		{
			if (!elementDecls.ContainsKey(elementDecl.Key))
			{
				elementDecls.Add(elementDecl.Key, elementDecl.Value);
			}
		}
		foreach (KeyValuePair<XmlQualifiedName, SchemaElementDecl> item in sinfo.elementDeclsByType)
		{
			if (!elementDeclsByType.ContainsKey(item.Key))
			{
				elementDeclsByType.Add(item.Key, item.Value);
			}
		}
		foreach (SchemaAttDef value in sinfo.AttributeDecls.Values)
		{
			if (!attributeDecls.ContainsKey(value.Name))
			{
				attributeDecls.Add(value.Name, value);
			}
		}
		foreach (SchemaNotation value2 in sinfo.Notations.Values)
		{
			if (!Notations.ContainsKey(value2.Name.Name))
			{
				Notations.Add(value2.Name.Name, value2);
			}
		}
	}

	internal void Finish()
	{
		Dictionary<XmlQualifiedName, SchemaElementDecl> dictionary = elementDecls;
		for (int i = 0; i < 2; i++)
		{
			foreach (SchemaElementDecl value in dictionary.Values)
			{
				if (value.HasNonCDataAttribute)
				{
					hasNonCDataAttributes = true;
				}
				if (value.DefaultAttDefs != null)
				{
					hasDefaultAttributes = true;
				}
			}
			dictionary = undeclaredElementDecls;
		}
	}

	IDtdAttributeListInfo IDtdInfo.LookupAttributeList(string prefix, string localName)
	{
		XmlQualifiedName key = new XmlQualifiedName(prefix, localName);
		if (!elementDecls.TryGetValue(key, out var value))
		{
			undeclaredElementDecls.TryGetValue(key, out value);
		}
		return value;
	}

	IEnumerable<IDtdAttributeListInfo> IDtdInfo.GetAttributeLists()
	{
		foreach (SchemaElementDecl value in elementDecls.Values)
		{
			yield return value;
		}
	}

	IDtdEntityInfo IDtdInfo.LookupEntity(string name)
	{
		if (generalEntities == null)
		{
			return null;
		}
		XmlQualifiedName key = new XmlQualifiedName(name);
		if (generalEntities.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}
}
