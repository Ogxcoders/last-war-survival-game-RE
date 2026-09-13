using System.Xml.Serialization;

namespace System.Xml.Schema;

public class XmlSchemaImport : XmlSchemaExternal
{
	private string ns;

	private XmlSchemaAnnotation annotation;

	[XmlAttribute("namespace", DataType = "anyURI")]
	public string Namespace
	{
		get
		{
			return ns;
		}
		set
		{
			ns = value;
		}
	}

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

	public XmlSchemaImport()
	{
		base.Compositor = Compositor.Import;
	}

	internal override void AddAnnotation(XmlSchemaAnnotation annotation)
	{
		this.annotation = annotation;
	}
}
