using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema;

public abstract class XmlSchemaFacet : XmlSchemaAnnotated
{
	private string value;

	private bool isFixed;

	private FacetType facetType;

	[XmlAttribute("value")]
	public string Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	[XmlAttribute("fixed")]
	[DefaultValue(false)]
	public virtual bool IsFixed
	{
		get
		{
			return isFixed;
		}
		set
		{
			if (!(this is XmlSchemaEnumerationFacet) && !(this is XmlSchemaPatternFacet))
			{
				isFixed = value;
			}
		}
	}

	internal FacetType FacetType
	{
		get
		{
			return facetType;
		}
		set
		{
			facetType = value;
		}
	}
}
