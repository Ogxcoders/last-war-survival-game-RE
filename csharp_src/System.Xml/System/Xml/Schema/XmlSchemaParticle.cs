using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaParticle : XmlSchemaAnnotated
{
	[Flags]
	private enum Occurs
	{
		None = 0,
		Min = 1,
		Max = 2
	}

	private class EmptyParticle : XmlSchemaParticle
	{
		internal override bool IsEmpty => true;
	}

	private decimal minOccurs = 1m;

	private decimal maxOccurs = 1m;

	private Occurs flags;

	internal static readonly XmlSchemaParticle Empty = new EmptyParticle();

	[XmlAttribute("minOccurs")]
	public string MinOccursString
	{
		get
		{
			if ((flags & Occurs.Min) != Occurs.None)
			{
				return XmlConvert.ToString(minOccurs);
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				minOccurs = 1m;
				flags &= ~Occurs.Min;
				return;
			}
			minOccurs = XmlConvert.ToInteger(value);
			if (minOccurs < 0m)
			{
				throw new XmlSchemaException("The value for the 'minOccurs' attribute must be xsd:nonNegativeInteger.", string.Empty);
			}
			flags |= Occurs.Min;
		}
	}

	[XmlAttribute("maxOccurs")]
	public string MaxOccursString
	{
		get
		{
			if ((flags & Occurs.Max) != Occurs.None)
			{
				if (!(maxOccurs == decimal.MaxValue))
				{
					return XmlConvert.ToString(maxOccurs);
				}
				return "unbounded";
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				maxOccurs = 1m;
				flags &= ~Occurs.Max;
				return;
			}
			if (value == "unbounded")
			{
				maxOccurs = decimal.MaxValue;
			}
			else
			{
				maxOccurs = XmlConvert.ToInteger(value);
				if (maxOccurs < 0m)
				{
					throw new XmlSchemaException("The value for the 'maxOccurs' attribute must be xsd:nonNegativeInteger or 'unbounded'.", string.Empty);
				}
				if (maxOccurs == 0m && (flags & Occurs.Min) == 0)
				{
					minOccurs = default(decimal);
				}
			}
			flags |= Occurs.Max;
		}
	}

	[XmlIgnore]
	public decimal MinOccurs
	{
		get
		{
			return minOccurs;
		}
		set
		{
			if (value < 0m || value != decimal.Truncate(value))
			{
				throw new XmlSchemaException("The value for the 'minOccurs' attribute must be xsd:nonNegativeInteger.", string.Empty);
			}
			minOccurs = value;
			flags |= Occurs.Min;
		}
	}

	[XmlIgnore]
	public decimal MaxOccurs
	{
		get
		{
			return maxOccurs;
		}
		set
		{
			if (value < 0m || value != decimal.Truncate(value))
			{
				throw new XmlSchemaException("The value for the 'maxOccurs' attribute must be xsd:nonNegativeInteger or 'unbounded'.", string.Empty);
			}
			maxOccurs = value;
			if (maxOccurs == 0m && (flags & Occurs.Min) == 0)
			{
				minOccurs = default(decimal);
			}
			flags |= Occurs.Max;
		}
	}

	internal virtual bool IsEmpty => maxOccurs == 0m;

	internal bool IsMultipleOccurrence => maxOccurs > 1m;

	internal virtual string NameString => string.Empty;

	internal XmlQualifiedName GetQualifiedName()
	{
		if (this is XmlSchemaElement xmlSchemaElement)
		{
			return xmlSchemaElement.QualifiedName;
		}
		if (this is XmlSchemaAny { Namespace: var text })
		{
			string text2 = ((text == null) ? string.Empty : text.Trim());
			return new XmlQualifiedName("*", (text2.Length == 0) ? "##any" : text2);
		}
		return XmlQualifiedName.Empty;
	}
}
