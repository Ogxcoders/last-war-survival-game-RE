using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaInclude : XmlSchemaExternal
{
	private XmlSchemaAnnotation annotation;

	[XmlElement("annotation", typeof(XmlSchemaAnnotation))]
	public XmlSchemaAnnotation Annotation
	{
		get
		{
			return annotation;
		}
		set
		{
			annotation = value;
		}
	}

	public XmlSchemaInclude()
	{
		base.Compositor = Compositor.Include;
	}

	internal override void AddAnnotation(XmlSchemaAnnotation annotation)
	{
		this.annotation = annotation;
	}
}
