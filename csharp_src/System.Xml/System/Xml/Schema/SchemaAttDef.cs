namespace System.Xml.Schema;

internal sealed class SchemaAttDef : SchemaDeclBase, IDtdDefaultAttributeInfo, IDtdAttributeInfo
{
	internal enum Reserve
	{
		None,
		XmlSpace,
		XmlLang
	}

	private string defExpanded;

	private int lineNum;

	private int linePos;

	private int valueLineNum;

	private int valueLinePos;

	private Reserve reserved;

	private bool defaultValueChecked;

	private bool hasEntityRef;

	private XmlSchemaAttribute schemaAttribute;

	public static readonly SchemaAttDef Empty = new SchemaAttDef();

	string IDtdAttributeInfo.Prefix => Prefix;

	string IDtdAttributeInfo.LocalName => Name.Name;

	int IDtdAttributeInfo.LineNumber => LineNumber;

	int IDtdAttributeInfo.LinePosition => LinePosition;

	bool IDtdAttributeInfo.IsNonCDataType => TokenizedType != XmlTokenizedType.CDATA;

	bool IDtdAttributeInfo.IsDeclaredInExternal => IsDeclaredInExternal;

	bool IDtdAttributeInfo.IsXmlAttribute => Reserved != Reserve.None;

	string IDtdDefaultAttributeInfo.DefaultValueExpanded => DefaultValueExpanded;

	object IDtdDefaultAttributeInfo.DefaultValueTyped => DefaultValueTyped;

	int IDtdDefaultAttributeInfo.ValueLineNumber => ValueLineNumber;

	int IDtdDefaultAttributeInfo.ValueLinePosition => ValueLinePosition;

	internal int LinePosition
	{
		get
		{
			return linePos;
		}
		set
		{
			linePos = value;
		}
	}

	internal int LineNumber
	{
		get
		{
			return lineNum;
		}
		set
		{
			lineNum = value;
		}
	}

	internal int ValueLinePosition
	{
		get
		{
			return valueLinePos;
		}
		set
		{
			valueLinePos = value;
		}
	}

	internal int ValueLineNumber
	{
		get
		{
			return valueLineNum;
		}
		set
		{
			valueLineNum = value;
		}
	}

	internal string DefaultValueExpanded
	{
		get
		{
			if (defExpanded == null)
			{
				return string.Empty;
			}
			return defExpanded;
		}
		set
		{
			defExpanded = value;
		}
	}

	internal XmlTokenizedType TokenizedType
	{
		get
		{
			return base.Datatype.TokenizedType;
		}
		set
		{
			base.Datatype = XmlSchemaDatatype.FromXmlTokenizedType(value);
		}
	}

	internal Reserve Reserved
	{
		get
		{
			return reserved;
		}
		set
		{
			reserved = value;
		}
	}

	internal bool DefaultValueChecked => defaultValueChecked;

	internal bool HasEntityRef
	{
		get
		{
			return hasEntityRef;
		}
		set
		{
			hasEntityRef = value;
		}
	}

	internal XmlSchemaAttribute SchemaAttribute
	{
		get
		{
			return schemaAttribute;
		}
		set
		{
			schemaAttribute = value;
		}
	}

	public SchemaAttDef(XmlQualifiedName name, string prefix)
		: base(name, prefix)
	{
	}

	public SchemaAttDef(XmlQualifiedName name)
		: base(name, null)
	{
	}

	private SchemaAttDef()
	{
	}

	internal void CheckXmlSpace(IValidationEventHandling validationEventHandling)
	{
		if (datatype.TokenizedType == XmlTokenizedType.ENUMERATION && values != null && values.Count <= 2)
		{
			string text = values[0].ToString();
			if (values.Count == 2)
			{
				string text2 = values[1].ToString();
				if ((text == "default" || text2 == "default") && (text == "preserve" || text2 == "preserve"))
				{
					return;
				}
			}
			else if (text == "default" || text == "preserve")
			{
				return;
			}
		}
		validationEventHandling.SendEvent(new XmlSchemaException("Invalid xml:space syntax.", string.Empty), XmlSeverityType.Error);
	}

	internal SchemaAttDef Clone()
	{
		return (SchemaAttDef)MemberwiseClone();
	}
}
