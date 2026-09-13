using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Xml;

internal sealed class XmlSubtreeReader : XmlWrappingReader, IXmlLineInfo, IXmlNamespaceResolver
{
	private class NodeData
	{
		internal XmlNodeType type;

		internal string localName;

		internal string prefix;

		internal string name;

		internal string namespaceUri;

		internal string value;

		internal NodeData()
		{
		}

		internal void Set(XmlNodeType nodeType, string localName, string prefix, string name, string namespaceUri, string value)
		{
			type = nodeType;
			this.localName = localName;
			this.prefix = prefix;
			this.name = name;
			this.namespaceUri = namespaceUri;
			this.value = value;
		}
	}

	private enum State
	{
		Initial,
		Interactive,
		Error,
		EndOfFile,
		Closed,
		PopNamespaceScope,
		ClearNsAttributes,
		ReadElementContentAsBase64,
		ReadElementContentAsBinHex,
		ReadContentAsBase64,
		ReadContentAsBinHex
	}

	private const int AttributeActiveStates = 98;

	private const int NamespaceActiveStates = 2018;

	private int initialDepth;

	private State state;

	private XmlNamespaceManager nsManager;

	private NodeData[] nsAttributes;

	private int nsAttrCount;

	private int curNsAttr = -1;

	private string xmlns;

	private string xmlnsUri;

	private int nsIncReadOffset;

	private IncrementalReadDecoder binDecoder;

	private bool useCurNode;

	private NodeData curNode;

	private NodeData tmpNode;

	internal int InitialNamespaceAttributeCount = 4;

	public override XmlNodeType NodeType
	{
		get
		{
			if (!useCurNode)
			{
				return reader.NodeType;
			}
			return curNode.type;
		}
	}

	public override string Name
	{
		get
		{
			if (!useCurNode)
			{
				return reader.Name;
			}
			return curNode.name;
		}
	}

	public override string LocalName
	{
		get
		{
			if (!useCurNode)
			{
				return reader.LocalName;
			}
			return curNode.localName;
		}
	}

	public override string NamespaceURI
	{
		get
		{
			if (!useCurNode)
			{
				return reader.NamespaceURI;
			}
			return curNode.namespaceUri;
		}
	}

	public override string Prefix
	{
		get
		{
			if (!useCurNode)
			{
				return reader.Prefix;
			}
			return curNode.prefix;
		}
	}

	public override string Value
	{
		get
		{
			if (!useCurNode)
			{
				return reader.Value;
			}
			return curNode.value;
		}
	}

	public override int Depth
	{
		get
		{
			int num = reader.Depth - initialDepth;
			if (curNsAttr != -1)
			{
				num = ((curNode.type != XmlNodeType.Text) ? (num + 1) : (num + 2));
			}
			return num;
		}
	}

	public override string BaseURI => reader.BaseURI;

	public override bool IsEmptyElement => reader.IsEmptyElement;

	public override bool EOF
	{
		get
		{
			if (state != State.EndOfFile)
			{
				return state == State.Closed;
			}
			return true;
		}
	}

	public override ReadState ReadState
	{
		get
		{
			if (reader.ReadState == ReadState.Error)
			{
				return ReadState.Error;
			}
			if (state <= State.Closed)
			{
				return (ReadState)state;
			}
			return ReadState.Interactive;
		}
	}

	public override XmlNameTable NameTable => reader.NameTable;

	public override int AttributeCount
	{
		get
		{
			if (!InAttributeActiveState)
			{
				return 0;
			}
			return reader.AttributeCount + nsAttrCount;
		}
	}

	public override bool CanReadBinaryContent => reader.CanReadBinaryContent;

	public override bool CanReadValueChunk => reader.CanReadValueChunk;

	int IXmlLineInfo.LineNumber
	{
		get
		{
			if (!useCurNode && reader is IXmlLineInfo xmlLineInfo)
			{
				return xmlLineInfo.LineNumber;
			}
			return 0;
		}
	}

	int IXmlLineInfo.LinePosition
	{
		get
		{
			if (!useCurNode && reader is IXmlLineInfo xmlLineInfo)
			{
				return xmlLineInfo.LinePosition;
			}
			return 0;
		}
	}

	private bool InAttributeActiveState => (0x62 & (1 << (int)state)) != 0;

	private bool InNamespaceActiveState => (0x7E2 & (1 << (int)state)) != 0;

	internal XmlSubtreeReader(XmlReader reader)
		: base(reader)
	{
		initialDepth = reader.Depth;
		state = State.Initial;
		nsManager = new XmlNamespaceManager(reader.NameTable);
		xmlns = reader.NameTable.Add("xmlns");
		xmlnsUri = reader.NameTable.Add("http://www.w3.org/2000/xmlns/");
		tmpNode = new NodeData();
		tmpNode.Set(XmlNodeType.None, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
		SetCurrentNode(tmpNode);
	}

	public override string GetAttribute(string name)
	{
		if (!InAttributeActiveState)
		{
			return null;
		}
		string attribute = reader.GetAttribute(name);
		if (attribute != null)
		{
			return attribute;
		}
		for (int i = 0; i < nsAttrCount; i++)
		{
			if (name == nsAttributes[i].name)
			{
				return nsAttributes[i].value;
			}
		}
		return null;
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		if (!InAttributeActiveState)
		{
			return null;
		}
		string attribute = reader.GetAttribute(name, namespaceURI);
		if (attribute != null)
		{
			return attribute;
		}
		for (int i = 0; i < nsAttrCount; i++)
		{
			if (name == nsAttributes[i].localName && namespaceURI == xmlnsUri)
			{
				return nsAttributes[i].value;
			}
		}
		return null;
	}

	public override string GetAttribute(int i)
	{
		if (!InAttributeActiveState)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		int attributeCount = reader.AttributeCount;
		if (i < attributeCount)
		{
			return reader.GetAttribute(i);
		}
		if (i - attributeCount < nsAttrCount)
		{
			return nsAttributes[i - attributeCount].value;
		}
		throw new ArgumentOutOfRangeException("i");
	}

	public override bool MoveToAttribute(string name)
	{
		if (!InAttributeActiveState)
		{
			return false;
		}
		if (reader.MoveToAttribute(name))
		{
			curNsAttr = -1;
			useCurNode = false;
			return true;
		}
		for (int i = 0; i < nsAttrCount; i++)
		{
			if (name == nsAttributes[i].name)
			{
				MoveToNsAttribute(i);
				return true;
			}
		}
		return false;
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		if (!InAttributeActiveState)
		{
			return false;
		}
		if (reader.MoveToAttribute(name, ns))
		{
			curNsAttr = -1;
			useCurNode = false;
			return true;
		}
		for (int i = 0; i < nsAttrCount; i++)
		{
			if (name == nsAttributes[i].localName && ns == xmlnsUri)
			{
				MoveToNsAttribute(i);
				return true;
			}
		}
		return false;
	}

	public override void MoveToAttribute(int i)
	{
		if (!InAttributeActiveState)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		int attributeCount = reader.AttributeCount;
		if (i < attributeCount)
		{
			reader.MoveToAttribute(i);
			curNsAttr = -1;
			useCurNode = false;
			return;
		}
		if (i - attributeCount < nsAttrCount)
		{
			MoveToNsAttribute(i - attributeCount);
			return;
		}
		throw new ArgumentOutOfRangeException("i");
	}

	public override bool MoveToFirstAttribute()
	{
		if (!InAttributeActiveState)
		{
			return false;
		}
		if (reader.MoveToFirstAttribute())
		{
			useCurNode = false;
			return true;
		}
		if (nsAttrCount > 0)
		{
			MoveToNsAttribute(0);
			return true;
		}
		return false;
	}

	public override bool MoveToNextAttribute()
	{
		if (!InAttributeActiveState)
		{
			return false;
		}
		if (curNsAttr == -1 && reader.MoveToNextAttribute())
		{
			return true;
		}
		if (curNsAttr + 1 < nsAttrCount)
		{
			MoveToNsAttribute(curNsAttr + 1);
			return true;
		}
		return false;
	}

	public override bool MoveToElement()
	{
		if (!InAttributeActiveState)
		{
			return false;
		}
		useCurNode = false;
		if (curNsAttr >= 0)
		{
			curNsAttr = -1;
			return true;
		}
		return reader.MoveToElement();
	}

	public override bool ReadAttributeValue()
	{
		if (!InAttributeActiveState)
		{
			return false;
		}
		if (curNsAttr == -1)
		{
			return reader.ReadAttributeValue();
		}
		if (curNode.type == XmlNodeType.Text)
		{
			return false;
		}
		tmpNode.type = XmlNodeType.Text;
		tmpNode.value = curNode.value;
		SetCurrentNode(tmpNode);
		return true;
	}

	public override bool Read()
	{
		switch (state)
		{
		case State.Initial:
			useCurNode = false;
			state = State.Interactive;
			ProcessNamespaces();
			return true;
		case State.Interactive:
			curNsAttr = -1;
			useCurNode = false;
			reader.MoveToElement();
			if (reader.Depth == initialDepth && (reader.NodeType == XmlNodeType.EndElement || (reader.NodeType == XmlNodeType.Element && reader.IsEmptyElement)))
			{
				state = State.EndOfFile;
				SetEmptyNode();
				return false;
			}
			if (reader.Read())
			{
				ProcessNamespaces();
				return true;
			}
			SetEmptyNode();
			return false;
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return false;
		case State.PopNamespaceScope:
			nsManager.PopScope();
			goto case State.ClearNsAttributes;
		case State.ClearNsAttributes:
			nsAttrCount = 0;
			state = State.Interactive;
			goto case State.Interactive;
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
			if (!FinishReadElementContentAsBinary())
			{
				return false;
			}
			return Read();
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			if (!FinishReadContentAsBinary())
			{
				return false;
			}
			return Read();
		default:
			return false;
		}
	}

	public override void Close()
	{
		if (state == State.Closed)
		{
			return;
		}
		try
		{
			if (state != State.EndOfFile)
			{
				reader.MoveToElement();
				if (reader.Depth == initialDepth && reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					reader.Read();
				}
				while (reader.Depth > initialDepth && reader.Read())
				{
				}
			}
		}
		catch
		{
		}
		finally
		{
			curNsAttr = -1;
			useCurNode = false;
			state = State.Closed;
			SetEmptyNode();
		}
	}

	public override void Skip()
	{
		switch (state)
		{
		case State.Initial:
			Read();
			break;
		case State.Interactive:
			curNsAttr = -1;
			useCurNode = false;
			reader.MoveToElement();
			if (reader.Depth == initialDepth)
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement && reader.Read())
				{
					while (reader.NodeType != XmlNodeType.EndElement && reader.Depth > initialDepth)
					{
						reader.Skip();
					}
				}
				state = State.EndOfFile;
				SetEmptyNode();
			}
			else
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					nsManager.PopScope();
				}
				reader.Skip();
				ProcessNamespaces();
			}
			break;
		case State.EndOfFile:
		case State.Closed:
			break;
		case State.PopNamespaceScope:
			nsManager.PopScope();
			goto case State.ClearNsAttributes;
		case State.ClearNsAttributes:
			nsAttrCount = 0;
			state = State.Interactive;
			goto case State.Interactive;
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
			if (FinishReadElementContentAsBinary())
			{
				Skip();
			}
			break;
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			if (FinishReadContentAsBinary())
			{
				Skip();
			}
			break;
		case State.Error:
			break;
		}
	}

	public override object ReadContentAsObject()
	{
		try
		{
			InitReadContentAsType("ReadContentAsObject");
			object result = reader.ReadContentAsObject();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override bool ReadContentAsBoolean()
	{
		try
		{
			InitReadContentAsType("ReadContentAsBoolean");
			bool result = reader.ReadContentAsBoolean();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override DateTime ReadContentAsDateTime()
	{
		try
		{
			InitReadContentAsType("ReadContentAsDateTime");
			DateTime result = reader.ReadContentAsDateTime();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override double ReadContentAsDouble()
	{
		try
		{
			InitReadContentAsType("ReadContentAsDouble");
			double result = reader.ReadContentAsDouble();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override float ReadContentAsFloat()
	{
		try
		{
			InitReadContentAsType("ReadContentAsFloat");
			float result = reader.ReadContentAsFloat();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override decimal ReadContentAsDecimal()
	{
		try
		{
			InitReadContentAsType("ReadContentAsDecimal");
			decimal result = reader.ReadContentAsDecimal();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override int ReadContentAsInt()
	{
		try
		{
			InitReadContentAsType("ReadContentAsInt");
			int result = reader.ReadContentAsInt();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override long ReadContentAsLong()
	{
		try
		{
			InitReadContentAsType("ReadContentAsLong");
			long result = reader.ReadContentAsLong();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override string ReadContentAsString()
	{
		try
		{
			InitReadContentAsType("ReadContentAsString");
			string result = reader.ReadContentAsString();
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		try
		{
			InitReadContentAsType("ReadContentAs");
			object result = reader.ReadContentAs(returnType, namespaceResolver);
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			switch (NodeType)
			{
			case XmlNodeType.Element:
				throw CreateReadContentAsException("ReadContentAsBase64");
			case XmlNodeType.EndElement:
				return 0;
			case XmlNodeType.Attribute:
				if (curNsAttr != -1 && reader.CanReadBinaryContent)
				{
					CheckBuffer(buffer, index, count);
					if (count == 0)
					{
						return 0;
					}
					if (nsIncReadOffset == 0)
					{
						if (binDecoder != null && binDecoder is Base64Decoder)
						{
							binDecoder.Reset();
						}
						else
						{
							binDecoder = new Base64Decoder();
						}
					}
					if (nsIncReadOffset == curNode.value.Length)
					{
						return 0;
					}
					binDecoder.SetNextOutputBuffer(buffer, index, count);
					nsIncReadOffset += binDecoder.Decode(curNode.value, nsIncReadOffset, curNode.value.Length - nsIncReadOffset);
					return binDecoder.DecodedCount;
				}
				goto case XmlNodeType.Text;
			case XmlNodeType.Text:
				return reader.ReadContentAsBase64(buffer, index, count);
			default:
				return 0;
			}
		case State.Interactive:
			state = State.ReadContentAsBase64;
			goto case State.ReadContentAsBase64;
		case State.ReadContentAsBase64:
		{
			int num = reader.ReadContentAsBase64(buffer, index, count);
			if (num == 0)
			{
				state = State.Interactive;
				ProcessNamespaces();
			}
			return num;
		}
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.Interactive:
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			if (!InitReadElementContentAsBinary(State.ReadElementContentAsBase64))
			{
				return 0;
			}
			goto case State.ReadElementContentAsBase64;
		case State.ReadElementContentAsBase64:
		{
			int num = reader.ReadContentAsBase64(buffer, index, count);
			if (num > 0 || count == 0)
			{
				return num;
			}
			if (NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
			}
			state = State.Interactive;
			ProcessNamespaces();
			if (reader.Depth == initialDepth)
			{
				state = State.EndOfFile;
				SetEmptyNode();
			}
			else
			{
				Read();
			}
			return 0;
		}
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			switch (NodeType)
			{
			case XmlNodeType.Element:
				throw CreateReadContentAsException("ReadContentAsBinHex");
			case XmlNodeType.EndElement:
				return 0;
			case XmlNodeType.Attribute:
				if (curNsAttr != -1 && reader.CanReadBinaryContent)
				{
					CheckBuffer(buffer, index, count);
					if (count == 0)
					{
						return 0;
					}
					if (nsIncReadOffset == 0)
					{
						if (binDecoder != null && binDecoder is BinHexDecoder)
						{
							binDecoder.Reset();
						}
						else
						{
							binDecoder = new BinHexDecoder();
						}
					}
					if (nsIncReadOffset == curNode.value.Length)
					{
						return 0;
					}
					binDecoder.SetNextOutputBuffer(buffer, index, count);
					nsIncReadOffset += binDecoder.Decode(curNode.value, nsIncReadOffset, curNode.value.Length - nsIncReadOffset);
					return binDecoder.DecodedCount;
				}
				goto case XmlNodeType.Text;
			case XmlNodeType.Text:
				return reader.ReadContentAsBinHex(buffer, index, count);
			default:
				return 0;
			}
		case State.Interactive:
			state = State.ReadContentAsBinHex;
			goto case State.ReadContentAsBinHex;
		case State.ReadContentAsBinHex:
		{
			int num = reader.ReadContentAsBinHex(buffer, index, count);
			if (num == 0)
			{
				state = State.Interactive;
				ProcessNamespaces();
			}
			return num;
		}
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.Interactive:
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			if (!InitReadElementContentAsBinary(State.ReadElementContentAsBinHex))
			{
				return 0;
			}
			goto case State.ReadElementContentAsBinHex;
		case State.ReadElementContentAsBinHex:
		{
			int num = reader.ReadContentAsBinHex(buffer, index, count);
			if (num > 0 || count == 0)
			{
				return num;
			}
			if (NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
			}
			state = State.Interactive;
			ProcessNamespaces();
			if (reader.Depth == initialDepth)
			{
				state = State.EndOfFile;
				SetEmptyNode();
			}
			else
			{
				Read();
			}
			return 0;
		}
		case State.ReadElementContentAsBase64:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override int ReadValueChunk(char[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			if (curNsAttr != -1 && reader.CanReadValueChunk)
			{
				CheckBuffer(buffer, index, count);
				int num = curNode.value.Length - nsIncReadOffset;
				if (num > count)
				{
					num = count;
				}
				if (num > 0)
				{
					curNode.value.CopyTo(nsIncReadOffset, buffer, index, num);
				}
				nsIncReadOffset += num;
				return num;
			}
			goto case State.Interactive;
		case State.Interactive:
			return reader.ReadValueChunk(buffer, index, count);
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override string LookupNamespace(string prefix)
	{
		return ((IXmlNamespaceResolver)this).LookupNamespace(prefix);
	}

	protected override void Dispose(bool disposing)
	{
		Close();
	}

	bool IXmlLineInfo.HasLineInfo()
	{
		return reader is IXmlLineInfo;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		if (!InNamespaceActiveState)
		{
			return new Dictionary<string, string>();
		}
		return nsManager.GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		if (!InNamespaceActiveState)
		{
			return null;
		}
		return nsManager.LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		if (!InNamespaceActiveState)
		{
			return null;
		}
		return nsManager.LookupPrefix(namespaceName);
	}

	private void ProcessNamespaces()
	{
		switch (reader.NodeType)
		{
		case XmlNodeType.Element:
		{
			nsManager.PushScope();
			string prefix = reader.Prefix;
			string namespaceURI = reader.NamespaceURI;
			if (nsManager.LookupNamespace(prefix) != namespaceURI)
			{
				AddNamespace(prefix, namespaceURI);
			}
			if (reader.MoveToFirstAttribute())
			{
				do
				{
					prefix = reader.Prefix;
					namespaceURI = reader.NamespaceURI;
					if (Ref.Equal(namespaceURI, xmlnsUri))
					{
						if (prefix.Length == 0)
						{
							nsManager.AddNamespace(string.Empty, reader.Value);
							RemoveNamespace(string.Empty, xmlns);
						}
						else
						{
							prefix = reader.LocalName;
							nsManager.AddNamespace(prefix, reader.Value);
							RemoveNamespace(xmlns, prefix);
						}
					}
					else if (prefix.Length != 0 && nsManager.LookupNamespace(prefix) != namespaceURI)
					{
						AddNamespace(prefix, namespaceURI);
					}
				}
				while (reader.MoveToNextAttribute());
				reader.MoveToElement();
			}
			if (reader.IsEmptyElement)
			{
				state = State.PopNamespaceScope;
			}
			break;
		}
		case XmlNodeType.EndElement:
			state = State.PopNamespaceScope;
			break;
		}
	}

	private void AddNamespace(string prefix, string ns)
	{
		nsManager.AddNamespace(prefix, ns);
		int num = nsAttrCount++;
		if (nsAttributes == null)
		{
			nsAttributes = new NodeData[InitialNamespaceAttributeCount];
		}
		if (num == nsAttributes.Length)
		{
			NodeData[] destinationArray = new NodeData[nsAttributes.Length * 2];
			Array.Copy(nsAttributes, 0, destinationArray, 0, num);
			nsAttributes = destinationArray;
		}
		if (nsAttributes[num] == null)
		{
			nsAttributes[num] = new NodeData();
		}
		if (prefix.Length == 0)
		{
			nsAttributes[num].Set(XmlNodeType.Attribute, xmlns, string.Empty, xmlns, xmlnsUri, ns);
		}
		else
		{
			nsAttributes[num].Set(XmlNodeType.Attribute, prefix, xmlns, reader.NameTable.Add(xmlns + ":" + prefix), xmlnsUri, ns);
		}
		state = State.ClearNsAttributes;
		curNsAttr = -1;
	}

	private void RemoveNamespace(string prefix, string localName)
	{
		for (int i = 0; i < nsAttrCount; i++)
		{
			if (Ref.Equal(prefix, nsAttributes[i].prefix) && Ref.Equal(localName, nsAttributes[i].localName))
			{
				if (i < nsAttrCount - 1)
				{
					NodeData nodeData = nsAttributes[i];
					nsAttributes[i] = nsAttributes[nsAttrCount - 1];
					nsAttributes[nsAttrCount - 1] = nodeData;
				}
				nsAttrCount--;
				break;
			}
		}
	}

	private void MoveToNsAttribute(int index)
	{
		reader.MoveToElement();
		curNsAttr = index;
		nsIncReadOffset = 0;
		SetCurrentNode(nsAttributes[index]);
	}

	private bool InitReadElementContentAsBinary(State binaryState)
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
		}
		bool isEmptyElement = IsEmptyElement;
		if (!Read() || isEmptyElement)
		{
			return false;
		}
		switch (NodeType)
		{
		case XmlNodeType.Element:
			throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
		case XmlNodeType.EndElement:
			ProcessNamespaces();
			Read();
			return false;
		default:
			state = binaryState;
			return true;
		}
	}

	private bool FinishReadElementContentAsBinary()
	{
		byte[] buffer = new byte[256];
		if (state == State.ReadElementContentAsBase64)
		{
			while (reader.ReadContentAsBase64(buffer, 0, 256) > 0)
			{
			}
		}
		else
		{
			while (reader.ReadContentAsBinHex(buffer, 0, 256) > 0)
			{
			}
		}
		if (NodeType != XmlNodeType.EndElement)
		{
			throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
		}
		state = State.Interactive;
		ProcessNamespaces();
		if (reader.Depth == initialDepth)
		{
			state = State.EndOfFile;
			SetEmptyNode();
			return false;
		}
		return Read();
	}

	private bool FinishReadContentAsBinary()
	{
		byte[] buffer = new byte[256];
		if (state == State.ReadContentAsBase64)
		{
			while (reader.ReadContentAsBase64(buffer, 0, 256) > 0)
			{
			}
		}
		else
		{
			while (reader.ReadContentAsBinHex(buffer, 0, 256) > 0)
			{
			}
		}
		state = State.Interactive;
		ProcessNamespaces();
		if (reader.Depth == initialDepth)
		{
			state = State.EndOfFile;
			SetEmptyNode();
			return false;
		}
		return true;
	}

	private void SetEmptyNode()
	{
		tmpNode.type = XmlNodeType.None;
		tmpNode.value = string.Empty;
		curNode = tmpNode;
		useCurNode = true;
	}

	private void SetCurrentNode(NodeData node)
	{
		curNode = node;
		useCurNode = true;
	}

	private void InitReadContentAsType(string methodName)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			throw new InvalidOperationException(Res.GetString("The XmlReader is closed or in error state."));
		case State.Interactive:
			break;
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			break;
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
		default:
			throw CreateReadContentAsException(methodName);
		}
	}

	private void FinishReadContentAsType()
	{
		switch (NodeType)
		{
		case XmlNodeType.Element:
			ProcessNamespaces();
			break;
		case XmlNodeType.EndElement:
			state = State.PopNamespaceScope;
			break;
		}
	}

	private void CheckBuffer(Array buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (buffer.Length - index < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
	}

	public override Task<string> GetValueAsync()
	{
		if (useCurNode)
		{
			return Task.FromResult(curNode.value);
		}
		return reader.GetValueAsync();
	}

	public override async Task<bool> ReadAsync()
	{
		switch (state)
		{
		case State.Initial:
			useCurNode = false;
			state = State.Interactive;
			ProcessNamespaces();
			return true;
		case State.Interactive:
			curNsAttr = -1;
			useCurNode = false;
			reader.MoveToElement();
			if (reader.Depth == initialDepth && (reader.NodeType == XmlNodeType.EndElement || (reader.NodeType == XmlNodeType.Element && reader.IsEmptyElement)))
			{
				state = State.EndOfFile;
				SetEmptyNode();
				return false;
			}
			if (await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				ProcessNamespaces();
				return true;
			}
			SetEmptyNode();
			return false;
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return false;
		case State.PopNamespaceScope:
			nsManager.PopScope();
			goto case State.ClearNsAttributes;
		case State.ClearNsAttributes:
			nsAttrCount = 0;
			state = State.Interactive;
			goto case State.Interactive;
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
			if (!(await FinishReadElementContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return false;
			}
			return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			if (!(await FinishReadContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return false;
			}
			return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		default:
			return false;
		}
	}

	public override async Task SkipAsync()
	{
		switch (state)
		{
		case State.Initial:
			await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			break;
		case State.Interactive:
			curNsAttr = -1;
			useCurNode = false;
			reader.MoveToElement();
			if (reader.Depth == initialDepth)
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement && await reader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
				{
					while (reader.NodeType != XmlNodeType.EndElement && reader.Depth > initialDepth)
					{
						await reader.SkipAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				state = State.EndOfFile;
				SetEmptyNode();
			}
			else
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					nsManager.PopScope();
				}
				await reader.SkipAsync().ConfigureAwait(continueOnCapturedContext: false);
				ProcessNamespaces();
			}
			break;
		case State.EndOfFile:
		case State.Closed:
			break;
		case State.PopNamespaceScope:
			nsManager.PopScope();
			goto case State.ClearNsAttributes;
		case State.ClearNsAttributes:
			nsAttrCount = 0;
			state = State.Interactive;
			goto case State.Interactive;
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
			if (await FinishReadElementContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				await SkipAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			break;
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			if (await FinishReadContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				await SkipAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			break;
		case State.Error:
			break;
		}
	}

	public override async Task<object> ReadContentAsObjectAsync()
	{
		try
		{
			InitReadContentAsType("ReadContentAsObject");
			object result = await reader.ReadContentAsObjectAsync().ConfigureAwait(continueOnCapturedContext: false);
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override async Task<string> ReadContentAsStringAsync()
	{
		try
		{
			InitReadContentAsType("ReadContentAsString");
			string result = await reader.ReadContentAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override async Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver)
	{
		try
		{
			InitReadContentAsType("ReadContentAs");
			object result = await reader.ReadContentAsAsync(returnType, namespaceResolver).ConfigureAwait(continueOnCapturedContext: false);
			FinishReadContentAsType();
			return result;
		}
		catch
		{
			state = State.Error;
			throw;
		}
	}

	public override async Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			switch (NodeType)
			{
			case XmlNodeType.Element:
				throw CreateReadContentAsException("ReadContentAsBase64");
			case XmlNodeType.EndElement:
				return 0;
			case XmlNodeType.Attribute:
				if (curNsAttr != -1 && reader.CanReadBinaryContent)
				{
					CheckBuffer(buffer, index, count);
					if (count == 0)
					{
						return 0;
					}
					if (nsIncReadOffset == 0)
					{
						if (binDecoder != null && binDecoder is Base64Decoder)
						{
							binDecoder.Reset();
						}
						else
						{
							binDecoder = new Base64Decoder();
						}
					}
					if (nsIncReadOffset == curNode.value.Length)
					{
						return 0;
					}
					binDecoder.SetNextOutputBuffer(buffer, index, count);
					nsIncReadOffset += binDecoder.Decode(curNode.value, nsIncReadOffset, curNode.value.Length - nsIncReadOffset);
					return binDecoder.DecodedCount;
				}
				goto case XmlNodeType.Text;
			case XmlNodeType.Text:
				return await reader.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			default:
				return 0;
			}
		case State.Interactive:
			state = State.ReadContentAsBase64;
			goto case State.ReadContentAsBase64;
		case State.ReadContentAsBase64:
		{
			int num = await reader.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			if (num == 0)
			{
				state = State.Interactive;
				ProcessNamespaces();
			}
			return num;
		}
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override async Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.Interactive:
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			if (!(await InitReadElementContentAsBinaryAsync(State.ReadElementContentAsBase64).ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
			goto case State.ReadElementContentAsBase64;
		case State.ReadElementContentAsBase64:
		{
			int num = await reader.ReadContentAsBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			if (num > 0 || count == 0)
			{
				return num;
			}
			if (NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
			}
			state = State.Interactive;
			ProcessNamespaces();
			if (reader.Depth != initialDepth)
			{
				await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				state = State.EndOfFile;
				SetEmptyNode();
			}
			return 0;
		}
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			switch (NodeType)
			{
			case XmlNodeType.Element:
				throw CreateReadContentAsException("ReadContentAsBinHex");
			case XmlNodeType.EndElement:
				return 0;
			case XmlNodeType.Attribute:
				if (curNsAttr != -1 && reader.CanReadBinaryContent)
				{
					CheckBuffer(buffer, index, count);
					if (count == 0)
					{
						return 0;
					}
					if (nsIncReadOffset == 0)
					{
						if (binDecoder != null && binDecoder is BinHexDecoder)
						{
							binDecoder.Reset();
						}
						else
						{
							binDecoder = new BinHexDecoder();
						}
					}
					if (nsIncReadOffset == curNode.value.Length)
					{
						return 0;
					}
					binDecoder.SetNextOutputBuffer(buffer, index, count);
					nsIncReadOffset += binDecoder.Decode(curNode.value, nsIncReadOffset, curNode.value.Length - nsIncReadOffset);
					return binDecoder.DecodedCount;
				}
				goto case XmlNodeType.Text;
			case XmlNodeType.Text:
				return await reader.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			default:
				return 0;
			}
		case State.Interactive:
			state = State.ReadContentAsBinHex;
			goto case State.ReadContentAsBinHex;
		case State.ReadContentAsBinHex:
		{
			int num = await reader.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			if (num == 0)
			{
				state = State.Interactive;
				ProcessNamespaces();
			}
			return num;
		}
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return 0;
		case State.Interactive:
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			if (!(await InitReadElementContentAsBinaryAsync(State.ReadElementContentAsBinHex).ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
			goto case State.ReadElementContentAsBinHex;
		case State.ReadElementContentAsBinHex:
		{
			int num = await reader.ReadContentAsBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			if (num > 0 || count == 0)
			{
				return num;
			}
			if (NodeType != XmlNodeType.EndElement)
			{
				throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
			}
			state = State.Interactive;
			ProcessNamespaces();
			if (reader.Depth != initialDepth)
			{
				await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				state = State.EndOfFile;
				SetEmptyNode();
			}
			return 0;
		}
		case State.ReadElementContentAsBase64:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
		default:
			return 0;
		}
	}

	public override Task<int> ReadValueChunkAsync(char[] buffer, int index, int count)
	{
		switch (state)
		{
		case State.Initial:
		case State.Error:
		case State.EndOfFile:
		case State.Closed:
			return Task.FromResult(0);
		case State.PopNamespaceScope:
		case State.ClearNsAttributes:
			if (curNsAttr != -1 && reader.CanReadValueChunk)
			{
				CheckBuffer(buffer, index, count);
				int num = curNode.value.Length - nsIncReadOffset;
				if (num > count)
				{
					num = count;
				}
				if (num > 0)
				{
					curNode.value.CopyTo(nsIncReadOffset, buffer, index, num);
				}
				nsIncReadOffset += num;
				return Task.FromResult(num);
			}
			goto case State.Interactive;
		case State.Interactive:
			return reader.ReadValueChunkAsync(buffer, index, count);
		case State.ReadElementContentAsBase64:
		case State.ReadElementContentAsBinHex:
		case State.ReadContentAsBase64:
		case State.ReadContentAsBinHex:
			throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
		default:
			return Task.FromResult(0);
		}
	}

	private async Task<bool> InitReadElementContentAsBinaryAsync(State binaryState)
	{
		if (NodeType != XmlNodeType.Element)
		{
			throw reader.CreateReadElementContentAsException("ReadElementContentAsBase64");
		}
		bool isEmpty = IsEmptyElement;
		if (!(await ReadAsync().ConfigureAwait(continueOnCapturedContext: false)) || isEmpty)
		{
			return false;
		}
		switch (NodeType)
		{
		case XmlNodeType.Element:
			throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
		case XmlNodeType.EndElement:
			ProcessNamespaces();
			await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			return false;
		default:
			state = binaryState;
			return true;
		}
	}

	private async Task<bool> FinishReadElementContentAsBinaryAsync()
	{
		byte[] bytes = new byte[256];
		if (state == State.ReadElementContentAsBase64)
		{
			while (await reader.ReadContentAsBase64Async(bytes, 0, 256).ConfigureAwait(continueOnCapturedContext: false) > 0)
			{
			}
		}
		else
		{
			while (await reader.ReadContentAsBinHexAsync(bytes, 0, 256).ConfigureAwait(continueOnCapturedContext: false) > 0)
			{
			}
		}
		if (NodeType != XmlNodeType.EndElement)
		{
			throw new XmlException("'{0}' is an invalid XmlNodeType.", reader.NodeType.ToString(), reader as IXmlLineInfo);
		}
		state = State.Interactive;
		ProcessNamespaces();
		if (reader.Depth == initialDepth)
		{
			state = State.EndOfFile;
			SetEmptyNode();
			return false;
		}
		return await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<bool> FinishReadContentAsBinaryAsync()
	{
		byte[] bytes = new byte[256];
		if (state == State.ReadContentAsBase64)
		{
			while (await reader.ReadContentAsBase64Async(bytes, 0, 256).ConfigureAwait(continueOnCapturedContext: false) > 0)
			{
			}
		}
		else
		{
			while (await reader.ReadContentAsBinHexAsync(bytes, 0, 256).ConfigureAwait(continueOnCapturedContext: false) > 0)
			{
			}
		}
		state = State.Interactive;
		ProcessNamespaces();
		if (reader.Depth == initialDepth)
		{
			state = State.EndOfFile;
			SetEmptyNode();
			return false;
		}
		return true;
	}
}
