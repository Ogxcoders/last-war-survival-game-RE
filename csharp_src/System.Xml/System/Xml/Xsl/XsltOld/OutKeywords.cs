using System.Diagnostics;

namespace System.Xml.Xsl.XsltOld;

internal class OutKeywords
{
	private string _AtomEmpty;

	private string _AtomLang;

	private string _AtomSpace;

	private string _AtomXmlns;

	private string _AtomXml;

	private string _AtomXmlNamespace;

	private string _AtomXmlnsNamespace;

	internal string Empty => _AtomEmpty;

	internal string Lang => _AtomLang;

	internal string Space => _AtomSpace;

	internal string Xmlns => _AtomXmlns;

	internal string Xml => _AtomXml;

	internal string XmlNamespace => _AtomXmlNamespace;

	internal string XmlnsNamespace => _AtomXmlnsNamespace;

	internal OutKeywords(XmlNameTable nameTable)
	{
		_AtomEmpty = nameTable.Add(string.Empty);
		_AtomLang = nameTable.Add("lang");
		_AtomSpace = nameTable.Add("space");
		_AtomXmlns = nameTable.Add("xmlns");
		_AtomXml = nameTable.Add("xml");
		_AtomXmlNamespace = nameTable.Add("http://www.w3.org/XML/1998/namespace");
		_AtomXmlnsNamespace = nameTable.Add("http://www.w3.org/2000/xmlns/");
	}

	[Conditional("DEBUG")]
	private void CheckKeyword(string keyword)
	{
	}
}
