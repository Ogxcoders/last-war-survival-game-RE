using System.Collections.Generic;

namespace System.Xml;

internal class XmlAsyncCheckReaderWithNS : XmlAsyncCheckReader, IXmlNamespaceResolver
{
	private readonly IXmlNamespaceResolver readerAsIXmlNamespaceResolver;

	public XmlAsyncCheckReaderWithNS(XmlReader reader)
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
