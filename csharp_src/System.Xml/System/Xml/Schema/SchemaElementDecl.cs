using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Schema;

internal sealed class SchemaElementDecl : SchemaDeclBase, IDtdAttributeListInfo
{
	private Dictionary<XmlQualifiedName, SchemaAttDef> attdefs = new Dictionary<XmlQualifiedName, SchemaAttDef>();

	private List<IDtdDefaultAttributeInfo> defaultAttdefs;

	private bool isIdDeclared;

	private bool hasNonCDataAttribute;

	private bool isAbstract;

	private bool isNillable;

	private bool hasRequiredAttribute;

	private bool isNotationDeclared;

	private Dictionary<XmlQualifiedName, XmlQualifiedName> prohibitedAttributes = new Dictionary<XmlQualifiedName, XmlQualifiedName>();

	private ContentValidator contentValidator;

	private XmlSchemaAnyAttribute anyAttribute;

	private XmlSchemaDerivationMethod block;

	private CompiledIdentityConstraint[] constraints;

	private XmlSchemaElement schemaElement;

	internal static readonly SchemaElementDecl Empty = new SchemaElementDecl();

	string IDtdAttributeListInfo.Prefix => Prefix;

	string IDtdAttributeListInfo.LocalName => Name.Name;

	bool IDtdAttributeListInfo.HasNonCDataAttributes => hasNonCDataAttribute;

	internal bool IsIdDeclared
	{
		get
		{
			return isIdDeclared;
		}
		set
		{
			isIdDeclared = value;
		}
	}

	internal bool HasNonCDataAttribute
	{
		get
		{
			return hasNonCDataAttribute;
		}
		set
		{
			hasNonCDataAttribute = value;
		}
	}

	internal bool IsAbstract
	{
		get
		{
			return isAbstract;
		}
		set
		{
			isAbstract = value;
		}
	}

	internal bool IsNillable
	{
		get
		{
			return isNillable;
		}
		set
		{
			isNillable = value;
		}
	}

	internal XmlSchemaDerivationMethod Block
	{
		get
		{
			return block;
		}
		set
		{
			block = value;
		}
	}

	internal bool IsNotationDeclared
	{
		get
		{
			return isNotationDeclared;
		}
		set
		{
			isNotationDeclared = value;
		}
	}

	internal bool HasDefaultAttribute => defaultAttdefs != null;

	internal bool HasRequiredAttribute
	{
		get
		{
			return hasRequiredAttribute;
		}
		set
		{
			hasRequiredAttribute = value;
		}
	}

	internal ContentValidator ContentValidator
	{
		get
		{
			return contentValidator;
		}
		set
		{
			contentValidator = value;
		}
	}

	internal XmlSchemaAnyAttribute AnyAttribute
	{
		get
		{
			return anyAttribute;
		}
		set
		{
			anyAttribute = value;
		}
	}

	internal CompiledIdentityConstraint[] Constraints
	{
		get
		{
			return constraints;
		}
		set
		{
			constraints = value;
		}
	}

	internal XmlSchemaElement SchemaElement
	{
		get
		{
			return schemaElement;
		}
		set
		{
			schemaElement = value;
		}
	}

	internal IList<IDtdDefaultAttributeInfo> DefaultAttDefs => defaultAttdefs;

	internal Dictionary<XmlQualifiedName, SchemaAttDef> AttDefs => attdefs;

	internal Dictionary<XmlQualifiedName, XmlQualifiedName> ProhibitedAttributes => prohibitedAttributes;

	internal SchemaElementDecl()
	{
	}

	internal SchemaElementDecl(XmlSchemaDatatype dtype)
	{
		base.Datatype = dtype;
		contentValidator = ContentValidator.TextOnly;
	}

	internal SchemaElementDecl(XmlQualifiedName name, string prefix)
		: base(name, prefix)
	{
	}

	internal static SchemaElementDecl CreateAnyTypeElementDecl()
	{
		return new SchemaElementDecl
		{
			Datatype = DatatypeImplementation.AnySimpleType.Datatype
		};
	}

	IDtdAttributeInfo IDtdAttributeListInfo.LookupAttribute(string prefix, string localName)
	{
		XmlQualifiedName key = new XmlQualifiedName(localName, prefix);
		if (attdefs.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	IEnumerable<IDtdDefaultAttributeInfo> IDtdAttributeListInfo.LookupDefaultAttributes()
	{
		return defaultAttdefs;
	}

	IDtdAttributeInfo IDtdAttributeListInfo.LookupIdAttribute()
	{
		foreach (SchemaAttDef value in attdefs.Values)
		{
			if (value.TokenizedType == XmlTokenizedType.ID)
			{
				return value;
			}
		}
		return null;
	}

	internal SchemaElementDecl Clone()
	{
		return (SchemaElementDecl)MemberwiseClone();
	}

	internal void AddAttDef(SchemaAttDef attdef)
	{
		attdefs.Add(attdef.Name, attdef);
		if (attdef.Presence == Use.Required || attdef.Presence == Use.RequiredFixed)
		{
			hasRequiredAttribute = true;
		}
		if (attdef.Presence == Use.Default || attdef.Presence == Use.Fixed)
		{
			if (defaultAttdefs == null)
			{
				defaultAttdefs = new List<IDtdDefaultAttributeInfo>();
			}
			defaultAttdefs.Add(attdef);
		}
	}

	internal SchemaAttDef GetAttDef(XmlQualifiedName qname)
	{
		if (attdefs.TryGetValue(qname, out var value))
		{
			return value;
		}
		return null;
	}

	internal void CheckAttributes(Hashtable presence, bool standalone)
	{
		foreach (SchemaAttDef value in attdefs.Values)
		{
			if (presence[value.Name] == null)
			{
				if (value.Presence == Use.Required)
				{
					throw new XmlSchemaException("The required attribute '{0}' is missing.", value.Name.ToString());
				}
				if (standalone && value.IsDeclaredInExternal && (value.Presence == Use.Default || value.Presence == Use.Fixed))
				{
					throw new XmlSchemaException("The standalone document declaration must have a value of 'no'.", string.Empty);
				}
			}
		}
	}
}
