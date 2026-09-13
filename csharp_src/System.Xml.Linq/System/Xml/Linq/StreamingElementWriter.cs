using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Linq;

internal struct StreamingElementWriter
{
	private XmlWriter _writer;

	private XStreamingElement _element;

	private List<XAttribute> _attributes;

	private NamespaceResolver _resolver;

	public StreamingElementWriter(XmlWriter w)
	{
		_writer = w;
		_element = null;
		_attributes = new List<XAttribute>();
		_resolver = default(NamespaceResolver);
	}

	private void FlushElement()
	{
		if (_element == null)
		{
			return;
		}
		PushElement();
		XNamespace xNamespace = _element.Name.Namespace;
		_writer.WriteStartElement(GetPrefixOfNamespace(xNamespace, allowDefaultNamespace: true), _element.Name.LocalName, xNamespace.NamespaceName);
		foreach (XAttribute attribute in _attributes)
		{
			xNamespace = attribute.Name.Namespace;
			string localName = attribute.Name.LocalName;
			string namespaceName = xNamespace.NamespaceName;
			_writer.WriteAttributeString(GetPrefixOfNamespace(xNamespace, allowDefaultNamespace: false), localName, (namespaceName.Length == 0 && localName == "xmlns") ? "http://www.w3.org/2000/xmlns/" : namespaceName, attribute.Value);
		}
		_element = null;
		_attributes.Clear();
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

	private void PushElement()
	{
		_resolver.PushScope();
		foreach (XAttribute attribute in _attributes)
		{
			if (attribute.IsNamespaceDeclaration)
			{
				_resolver.Add((attribute.Name.NamespaceName.Length == 0) ? string.Empty : attribute.Name.LocalName, XNamespace.Get(attribute.Value));
			}
		}
	}

	private void Write(object content)
	{
		if (content == null)
		{
			return;
		}
		if (content is XNode n)
		{
			WriteNode(n);
			return;
		}
		if (content is string s)
		{
			WriteString(s);
			return;
		}
		if (content is XAttribute a)
		{
			WriteAttribute(a);
			return;
		}
		if (content is XStreamingElement e)
		{
			WriteStreamingElement(e);
			return;
		}
		if (content is object[] array)
		{
			object[] array2 = array;
			foreach (object content2 in array2)
			{
				Write(content2);
			}
			return;
		}
		if (content is IEnumerable enumerable)
		{
			{
				foreach (object item in enumerable)
				{
					Write(item);
				}
				return;
			}
		}
		WriteString(XContainer.GetStringValue(content));
	}

	private void WriteAttribute(XAttribute a)
	{
		if (_element == null)
		{
			throw new InvalidOperationException("An attribute cannot be written after content.");
		}
		_attributes.Add(a);
	}

	private void WriteNode(XNode n)
	{
		FlushElement();
		n.WriteTo(_writer);
	}

	internal void WriteStreamingElement(XStreamingElement e)
	{
		FlushElement();
		_element = e;
		Write(e.content);
		FlushElement();
		_writer.WriteEndElement();
		_resolver.PopScope();
	}

	private void WriteString(string s)
	{
		FlushElement();
		_writer.WriteString(s);
	}
}
