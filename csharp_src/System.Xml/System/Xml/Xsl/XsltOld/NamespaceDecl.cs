namespace System.Xml.Xsl.XsltOld;

internal class NamespaceDecl
{
	private string prefix;

	private string nsUri;

	private string prevDefaultNsUri;

	private NamespaceDecl next;

	internal string Prefix => prefix;

	internal string Uri => nsUri;

	internal string PrevDefaultNsUri => prevDefaultNsUri;

	internal NamespaceDecl Next => next;

	internal NamespaceDecl(string prefix, string nsUri, string prevDefaultNsUri, NamespaceDecl next)
	{
		Init(prefix, nsUri, prevDefaultNsUri, next);
	}

	internal void Init(string prefix, string nsUri, string prevDefaultNsUri, NamespaceDecl next)
	{
		this.prefix = prefix;
		this.nsUri = nsUri;
		this.prevDefaultNsUri = prevDefaultNsUri;
		this.next = next;
	}
}
