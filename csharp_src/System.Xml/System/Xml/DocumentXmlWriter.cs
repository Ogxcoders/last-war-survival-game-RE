using System.Collections.Generic;

namespace System.Xml;

internal sealed class DocumentXmlWriter : XmlRawWriter, IXmlNamespaceResolver
{
	private enum State
	{
		Error,
		Attribute,
		Prolog,
		Fragment,
		Content,
		Last
	}

	private enum Method
	{
		WriteXmlDeclaration,
		WriteStartDocument,
		WriteEndDocument,
		WriteDocType,
		WriteStartElement,
		WriteEndElement,
		WriteFullEndElement,
		WriteStartAttribute,
		WriteEndAttribute,
		WriteStartNamespaceDeclaration,
		WriteEndNamespaceDeclaration,
		WriteCData,
		WriteComment,
		WriteProcessingInstruction,
		WriteEntityRef,
		WriteWhitespace,
		WriteString
	}

	private DocumentXmlWriterType type;

	private XmlNode start;

	private XmlDocument document;

	private XmlNamespaceManager namespaceManager;

	private State state;

	private XmlNode write;

	private List<XmlNode> fragment;

	private XmlWriterSettings settings;

	private DocumentXPathNavigator navigator;

	private XmlNode end;

	private static State[] changeState = new State[85]
	{
		State.Error,
		State.Error,
		State.Prolog,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Prolog,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Content,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Error,
		State.Content,
		State.Error,
		State.Error,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Error,
		State.Content,
		State.Error,
		State.Error,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Content,
		State.Error,
		State.Error,
		State.Prolog,
		State.Content,
		State.Content,
		State.Error,
		State.Error,
		State.Prolog,
		State.Content,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Content,
		State.Error,
		State.Error,
		State.Prolog,
		State.Content,
		State.Content,
		State.Error,
		State.Error,
		State.Error,
		State.Content,
		State.Content
	};

	public XmlNamespaceManager NamespaceManager
	{
		set
		{
			namespaceManager = value;
		}
	}

	public override XmlWriterSettings Settings => settings;

	public DocumentXPathNavigator Navigator
	{
		set
		{
			navigator = value;
		}
	}

	public XmlNode EndNode
	{
		set
		{
			end = value;
		}
	}

	internal override bool SupportsNamespaceDeclarationInChunks => true;

	public DocumentXmlWriter(DocumentXmlWriterType type, XmlNode start, XmlDocument document)
	{
		this.type = type;
		this.start = start;
		this.document = document;
		state = StartState();
		fragment = new List<XmlNode>();
		settings = new XmlWriterSettings();
		settings.ReadOnly = false;
		settings.CheckCharacters = false;
		settings.CloseOutput = false;
		settings.ConformanceLevel = ((state != State.Prolog) ? ConformanceLevel.Fragment : ConformanceLevel.Document);
		settings.ReadOnly = true;
	}

	internal void SetSettings(XmlWriterSettings value)
	{
		settings = value;
	}

	internal override void WriteXmlDeclaration(XmlStandalone standalone)
	{
		VerifyState(Method.WriteXmlDeclaration);
		if (standalone != XmlStandalone.Omit)
		{
			XmlNode node = document.CreateXmlDeclaration("1.0", string.Empty, (standalone == XmlStandalone.Yes) ? "yes" : "no");
			AddChild(node, write);
		}
	}

	internal override void WriteXmlDeclaration(string xmldecl)
	{
		VerifyState(Method.WriteXmlDeclaration);
		XmlLoader.ParseXmlDeclarationValue(xmldecl, out var version, out var encoding, out var standalone);
		XmlNode node = document.CreateXmlDeclaration(version, encoding, standalone);
		AddChild(node, write);
	}

	public override void WriteStartDocument()
	{
		VerifyState(Method.WriteStartDocument);
	}

	public override void WriteStartDocument(bool standalone)
	{
		VerifyState(Method.WriteStartDocument);
	}

	public override void WriteEndDocument()
	{
		VerifyState(Method.WriteEndDocument);
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		VerifyState(Method.WriteDocType);
		XmlNode node = document.CreateDocumentType(name, pubid, sysid, subset);
		AddChild(node, write);
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		VerifyState(Method.WriteStartElement);
		XmlNode node = document.CreateElement(prefix, localName, ns);
		AddChild(node, write);
		write = node;
	}

	public override void WriteEndElement()
	{
		VerifyState(Method.WriteEndElement);
		if (write == null)
		{
			throw new InvalidOperationException();
		}
		write = write.ParentNode;
	}

	internal override void WriteEndElement(string prefix, string localName, string ns)
	{
		WriteEndElement();
	}

	public override void WriteFullEndElement()
	{
		VerifyState(Method.WriteFullEndElement);
		if (!(write is XmlElement xmlElement))
		{
			throw new InvalidOperationException();
		}
		xmlElement.IsEmpty = false;
		write = xmlElement.ParentNode;
	}

	internal override void WriteFullEndElement(string prefix, string localName, string ns)
	{
		WriteFullEndElement();
	}

	internal override void StartElementContent()
	{
	}

	public override void WriteStartAttribute(string prefix, string localName, string ns)
	{
		VerifyState(Method.WriteStartAttribute);
		XmlAttribute attr = document.CreateAttribute(prefix, localName, ns);
		AddAttribute(attr, write);
		write = attr;
	}

	public override void WriteEndAttribute()
	{
		VerifyState(Method.WriteEndAttribute);
		if (!(write is XmlAttribute xmlAttribute))
		{
			throw new InvalidOperationException();
		}
		if (!xmlAttribute.HasChildNodes)
		{
			XmlNode node = document.CreateTextNode(string.Empty);
			AddChild(node, xmlAttribute);
		}
		write = xmlAttribute.OwnerElement;
	}

	internal override void WriteNamespaceDeclaration(string prefix, string ns)
	{
		WriteStartNamespaceDeclaration(prefix);
		WriteString(ns);
		WriteEndNamespaceDeclaration();
	}

	internal override void WriteStartNamespaceDeclaration(string prefix)
	{
		VerifyState(Method.WriteStartNamespaceDeclaration);
		XmlAttribute attr = ((prefix.Length != 0) ? document.CreateAttribute(document.strXmlns, prefix, document.strReservedXmlns) : document.CreateAttribute(prefix, document.strXmlns, document.strReservedXmlns));
		AddAttribute(attr, write);
		write = attr;
	}

	internal override void WriteEndNamespaceDeclaration()
	{
		VerifyState(Method.WriteEndNamespaceDeclaration);
		if (!(write is XmlAttribute xmlAttribute))
		{
			throw new InvalidOperationException();
		}
		if (!xmlAttribute.HasChildNodes)
		{
			XmlNode node = document.CreateTextNode(string.Empty);
			AddChild(node, xmlAttribute);
		}
		write = xmlAttribute.OwnerElement;
	}

	public override void WriteCData(string text)
	{
		VerifyState(Method.WriteCData);
		XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
		XmlNode node = document.CreateCDataSection(text);
		AddChild(node, write);
	}

	public override void WriteComment(string text)
	{
		VerifyState(Method.WriteComment);
		XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
		XmlNode node = document.CreateComment(text);
		AddChild(node, write);
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		VerifyState(Method.WriteProcessingInstruction);
		XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
		XmlNode node = document.CreateProcessingInstruction(name, text);
		AddChild(node, write);
	}

	public override void WriteEntityRef(string name)
	{
		VerifyState(Method.WriteEntityRef);
		XmlNode node = document.CreateEntityReference(name);
		AddChild(node, write);
	}

	public override void WriteCharEntity(char ch)
	{
		WriteString(new string(ch, 1));
	}

	public override void WriteWhitespace(string text)
	{
		VerifyState(Method.WriteWhitespace);
		XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
		if (document.PreserveWhitespace)
		{
			XmlNode node = document.CreateWhitespace(text);
			AddChild(node, write);
		}
	}

	public override void WriteString(string text)
	{
		VerifyState(Method.WriteString);
		XmlConvert.VerifyCharData(text, ExceptionType.ArgumentException);
		XmlNode node = document.CreateTextNode(text);
		AddChild(node, write);
	}

	public override void WriteSurrogateCharEntity(char lowCh, char highCh)
	{
		WriteString(new string(new char[2] { highCh, lowCh }));
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		WriteString(new string(buffer, index, count));
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		WriteString(new string(buffer, index, count));
	}

	public override void WriteRaw(string data)
	{
		WriteString(data);
	}

	public override void Close()
	{
	}

	internal override void Close(WriteState currentState)
	{
		if (currentState == WriteState.Error)
		{
			return;
		}
		try
		{
			switch (type)
			{
			case DocumentXmlWriterType.InsertSiblingAfter:
			{
				XmlNode parentNode = start.ParentNode;
				if (parentNode == null)
				{
					throw new InvalidOperationException(Res.GetString("The current position of the navigator is missing a valid parent."));
				}
				for (int num2 = fragment.Count - 1; num2 >= 0; num2--)
				{
					parentNode.InsertAfter(fragment[num2], start);
				}
				break;
			}
			case DocumentXmlWriterType.InsertSiblingBefore:
			{
				XmlNode parentNode = start.ParentNode;
				if (parentNode == null)
				{
					throw new InvalidOperationException(Res.GetString("The current position of the navigator is missing a valid parent."));
				}
				for (int j = 0; j < fragment.Count; j++)
				{
					parentNode.InsertBefore(fragment[j], start);
				}
				break;
			}
			case DocumentXmlWriterType.PrependChild:
			{
				for (int num = fragment.Count - 1; num >= 0; num--)
				{
					start.PrependChild(fragment[num]);
				}
				break;
			}
			case DocumentXmlWriterType.AppendChild:
			{
				for (int i = 0; i < fragment.Count; i++)
				{
					start.AppendChild(fragment[i]);
				}
				break;
			}
			case DocumentXmlWriterType.AppendAttribute:
				CloseWithAppendAttribute();
				break;
			case DocumentXmlWriterType.ReplaceToFollowingSibling:
				if (fragment.Count == 0)
				{
					throw new InvalidOperationException(Res.GetString("No content generated as the result of the operation."));
				}
				CloseWithReplaceToFollowingSibling();
				break;
			}
		}
		finally
		{
			fragment.Clear();
		}
	}

	private void CloseWithAppendAttribute()
	{
		XmlAttributeCollection attributes = (start as XmlElement).Attributes;
		for (int i = 0; i < fragment.Count; i++)
		{
			XmlAttribute xmlAttribute = fragment[i] as XmlAttribute;
			int num = attributes.FindNodeOffsetNS(xmlAttribute);
			if (num != -1 && ((XmlAttribute)attributes.nodes[num]).Specified)
			{
				throw new XmlException("'{0}' is a duplicate attribute name.", (xmlAttribute.Prefix.Length == 0) ? xmlAttribute.LocalName : (xmlAttribute.Prefix + ":" + xmlAttribute.LocalName));
			}
		}
		for (int j = 0; j < fragment.Count; j++)
		{
			XmlAttribute node = fragment[j] as XmlAttribute;
			attributes.Append(node);
		}
	}

	private void CloseWithReplaceToFollowingSibling()
	{
		XmlNode parentNode = start.ParentNode;
		if (parentNode == null)
		{
			throw new InvalidOperationException(Res.GetString("The current position of the navigator is missing a valid parent."));
		}
		if (start != end)
		{
			if (!DocumentXPathNavigator.IsFollowingSibling(start, end))
			{
				throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current position of the navigator."));
			}
			if (start.IsReadOnly)
			{
				throw new InvalidOperationException(Res.GetString("This node is read-only. It cannot be modified."));
			}
			DocumentXPathNavigator.DeleteToFollowingSibling(start.NextSibling, end);
		}
		XmlNode xmlNode = fragment[0];
		parentNode.ReplaceChild(xmlNode, start);
		for (int num = fragment.Count - 1; num >= 1; num--)
		{
			parentNode.InsertAfter(fragment[num], xmlNode);
		}
		navigator.ResetPosition(xmlNode);
	}

	public override void Flush()
	{
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return namespaceManager.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		return namespaceManager.LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return namespaceManager.LookupPrefix(namespaceName);
	}

	private void AddAttribute(XmlAttribute attr, XmlNode parent)
	{
		if (parent == null)
		{
			fragment.Add(attr);
		}
		else
		{
			((parent as XmlElement) ?? throw new InvalidOperationException()).Attributes.Append(attr);
		}
	}

	private void AddChild(XmlNode node, XmlNode parent)
	{
		if (parent == null)
		{
			fragment.Add(node);
		}
		else
		{
			parent.AppendChild(node);
		}
	}

	private State StartState()
	{
		XmlNodeType xmlNodeType = XmlNodeType.None;
		switch (type)
		{
		case DocumentXmlWriterType.InsertSiblingAfter:
		case DocumentXmlWriterType.InsertSiblingBefore:
		{
			XmlNode parentNode = start.ParentNode;
			if (parentNode != null)
			{
				xmlNodeType = parentNode.NodeType;
			}
			switch (xmlNodeType)
			{
			case XmlNodeType.Document:
				return State.Prolog;
			case XmlNodeType.DocumentFragment:
				return State.Fragment;
			}
			break;
		}
		case DocumentXmlWriterType.PrependChild:
		case DocumentXmlWriterType.AppendChild:
			switch (start.NodeType)
			{
			case XmlNodeType.Document:
				return State.Prolog;
			case XmlNodeType.DocumentFragment:
				return State.Fragment;
			}
			break;
		case DocumentXmlWriterType.AppendAttribute:
			return State.Attribute;
		}
		return State.Content;
	}

	private void VerifyState(Method method)
	{
		state = changeState[(int)((int)method * 5 + state)];
		if (state == State.Error)
		{
			throw new InvalidOperationException(Res.GetString("The Writer is closed or in error state."));
		}
	}
}
