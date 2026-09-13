using System.IO;
using System.Text;

namespace System.Xml;

internal class XmlDOMTextWriter : XmlTextWriter
{
	public XmlDOMTextWriter(Stream w, Encoding encoding)
		: base(w, encoding)
	{
	}

	public XmlDOMTextWriter(string filename, Encoding encoding)
		: base(filename, encoding)
	{
	}

	public XmlDOMTextWriter(TextWriter w)
		: base(w)
	{
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		if (ns.Length == 0 && prefix.Length != 0)
		{
			prefix = "";
		}
		base.WriteStartElement(prefix, localName, ns);
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		if (ns.Length == 0 && prefix.Length != 0)
		{
			prefix = "";
		}
		base.WriteStartAttribute(prefix, localName, ns);
	}
}
