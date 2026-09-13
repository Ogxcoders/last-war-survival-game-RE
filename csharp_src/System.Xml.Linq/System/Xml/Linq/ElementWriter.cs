using System.Threading;
using System.Threading.Tasks;

namespace System.Xml.Linq;

internal struct ElementWriter
{
	private XmlWriter _writer;

	private NamespaceResolver _resolver;

	public ElementWriter(XmlWriter writer)
	{
		_writer = writer;
		_resolver = default(NamespaceResolver);
	}

	public void WriteElement(XElement e)
	{
		PushAncestors(e);
		XElement xElement = e;
		XNode xNode = e;
		while (true)
		{
			e = xNode as XElement;
			if (e != null)
			{
				WriteStartElement(e);
				if (e.content == null)
				{
					WriteEndElement();
				}
				else
				{
					if (!(e.content is string text))
					{
						xNode = ((XNode)e.content).next;
						continue;
					}
					_writer.WriteString(text);
					WriteFullEndElement();
				}
			}
			else
			{
				xNode.WriteTo(_writer);
			}
			while (xNode != xElement && xNode == xNode.parent.content)
			{
				xNode = xNode.parent;
				WriteFullEndElement();
			}
			if (xNode != xElement)
			{
				xNode = xNode.next;
				continue;
			}
			break;
		}
	}

	public async Task WriteElementAsync(XElement e, CancellationToken cancellationToken)
	{
		PushAncestors(e);
		XElement root = e;
		XNode n = e;
		while (true)
		{
			e = n as XElement;
			if (e == null)
			{
				await n.WriteToAsync(_writer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await WriteStartElementAsync(e, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (e.content == null)
				{
					await WriteEndElementAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				else
				{
					if (!(e.content is string text))
					{
						n = ((XNode)e.content).next;
						continue;
					}
					cancellationToken.ThrowIfCancellationRequested();
					await _writer.WriteStringAsync(text).ConfigureAwait(continueOnCapturedContext: false);
					await WriteFullEndElementAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			while (n != root && n == n.parent.content)
			{
				n = n.parent;
				await WriteFullEndElementAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (n != root)
			{
				n = n.next;
				continue;
			}
			break;
		}
	}

	private string GetPrefixOfNamespace(XNamespace ns, bool allowDefaultNamespace)
	{
		string namespaceName = ns.NamespaceName;
		if (namespaceName.Length == 0)
		{
			return string.Empty;
		}
		string prefixOfNamespace = _resolver.GetPrefixOfNamespace(ns, allowDefaultNamespace);
		if (prefixOfNamespace != null)
		{
			return prefixOfNamespace;
		}
		if ((object)namespaceName == "http://www.w3.org/XML/1998/namespace")
		{
			return "xml";
		}
		if ((object)namespaceName == "http://www.w3.org/2000/xmlns/")
		{
			return "xmlns";
		}
		return null;
	}

	private void PushAncestors(XElement e)
	{
		while (true)
		{
			e = e.parent as XElement;
			if (e == null)
			{
				break;
			}
			XAttribute xAttribute = e.lastAttr;
			if (xAttribute == null)
			{
				continue;
			}
			do
			{
				xAttribute = xAttribute.next;
				if (xAttribute.IsNamespaceDeclaration)
				{
					_resolver.AddFirst((xAttribute.Name.NamespaceName.Length == 0) ? string.Empty : xAttribute.Name.LocalName, XNamespace.Get(xAttribute.Value));
				}
			}
			while (xAttribute != e.lastAttr);
		}
	}

	private void PushElement(XElement e)
	{
		_resolver.PushScope();
		XAttribute xAttribute = e.lastAttr;
		if (xAttribute == null)
		{
			return;
		}
		do
		{
			xAttribute = xAttribute.next;
			if (xAttribute.IsNamespaceDeclaration)
			{
				_resolver.Add((xAttribute.Name.NamespaceName.Length == 0) ? string.Empty : xAttribute.Name.LocalName, XNamespace.Get(xAttribute.Value));
			}
		}
		while (xAttribute != e.lastAttr);
	}

	private void WriteEndElement()
	{
		_writer.WriteEndElement();
		_resolver.PopScope();
	}

	private async Task WriteEndElementAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await _writer.WriteEndElementAsync().ConfigureAwait(continueOnCapturedContext: false);
		_resolver.PopScope();
	}

	private void WriteFullEndElement()
	{
		_writer.WriteFullEndElement();
		_resolver.PopScope();
	}

	private async Task WriteFullEndElementAsync(CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		await _writer.WriteFullEndElementAsync().ConfigureAwait(continueOnCapturedContext: false);
		_resolver.PopScope();
	}

	private void WriteStartElement(XElement e)
	{
		PushElement(e);
		XNamespace xNamespace = e.Name.Namespace;
		_writer.WriteStartElement(GetPrefixOfNamespace(xNamespace, allowDefaultNamespace: true), e.Name.LocalName, xNamespace.NamespaceName);
		XAttribute xAttribute = e.lastAttr;
		if (xAttribute != null)
		{
			do
			{
				xAttribute = xAttribute.next;
				xNamespace = xAttribute.Name.Namespace;
				string localName = xAttribute.Name.LocalName;
				string namespaceName = xNamespace.NamespaceName;
				_writer.WriteAttributeString(GetPrefixOfNamespace(xNamespace, allowDefaultNamespace: false), localName, (namespaceName.Length == 0 && localName == "xmlns") ? "http://www.w3.org/2000/xmlns/" : namespaceName, xAttribute.Value);
			}
			while (xAttribute != e.lastAttr);
		}
	}

	private async Task WriteStartElementAsync(XElement e, CancellationToken cancellationToken)
	{
		PushElement(e);
		XNamespace xNamespace = e.Name.Namespace;
		await _writer.WriteStartElementAsync(GetPrefixOfNamespace(xNamespace, allowDefaultNamespace: true), e.Name.LocalName, xNamespace.NamespaceName).ConfigureAwait(continueOnCapturedContext: false);
		XAttribute a = e.lastAttr;
		if (a != null)
		{
			do
			{
				a = a.next;
				xNamespace = a.Name.Namespace;
				string localName = a.Name.LocalName;
				string namespaceName = xNamespace.NamespaceName;
				await _writer.WriteAttributeStringAsync(GetPrefixOfNamespace(xNamespace, allowDefaultNamespace: false), localName, (namespaceName.Length == 0 && localName == "xmlns") ? "http://www.w3.org/2000/xmlns/" : namespaceName, a.Value).ConfigureAwait(continueOnCapturedContext: false);
			}
			while (a != e.lastAttr);
		}
	}
}
