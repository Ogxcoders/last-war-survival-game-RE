using System.Collections.Generic;

namespace System.Xml;

internal class XmlAsyncCheckReaderWithLineInfoNS : XmlAsyncCheckReaderWithLineInfo, IXmlNamespaceResolver
{
	private readonly IXmlNamespaceResolver readerAsIXmlNamespaceResolver;

	public XmlAsyncCheckReaderWithLineInfoNS(XmlReader reader)
		: base(reader)
	{
		readerAsIXmlNamespaceResolver = (IXmlNamespaceResolver)reader;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return readerAsIXmlNamespaceResolver.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		return readerAsIXmlNamespaceResolver.LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return readerAsIXmlNamespaceResolver.LookupPrefix(namespaceName);
	}
}
