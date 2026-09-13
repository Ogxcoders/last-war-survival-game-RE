using System.Threading.Tasks;

namespace System.Xml;

internal class XsdCachingReader : XmlReader, IXmlLineInfo
{
	private enum CachingReaderState
	{
		None,
		Init,
		Record,
		Replay,
		ReaderClosed,
		Error
	}

	private XmlReader coreReader;

	private XmlNameTable coreReaderNameTable;

	private ValidatingReaderNodeData[] contentEvents;

	private ValidatingReaderNodeData[] attributeEvents;

	private ValidatingReaderNodeData cachedNode;

	private CachingReaderState cacheState;

	private int contentIndex;

	private int attributeCount;

	private bool returnOriginalStringValues;

	private CachingEventHandler cacheHandler;

	private int currentAttrIndex;

	private int currentContentIndex;

	private bool readAhead;

	private IXmlLineInfo lineInfo;

	private ValidatingReaderNodeData textNode;

	private const int InitialAttributeCount = 8;

	private const int InitialContentCount = 4;

	public override XmlReaderSettings Settings => coreReader.Settings;

	public override XmlNodeType NodeType => cachedNode.NodeType;

	public override string Name => cachedNode.GetAtomizedNameWPrefix(coreReaderNameTable);

	public override string LocalName => cachedNode.LocalName;

	public override string NamespaceURI => cachedNode.Namespace;

	public override string Prefix => cachedNode.Prefix;

	public override bool HasValue => XmlReader.HasValueInternal(cachedNode.NodeType);

	public override string Value
	{
		get
		{
			if (!returnOriginalStringValues)
			{
				return cachedNode.RawValue;
			}
			return cachedNode.OriginalStringValue;
		}
	}

	public override int Depth => cachedNode.Depth;

	public override string BaseURI => coreReader.BaseURI;

	public override bool IsEmptyElement => false;

	public override bool IsDefault => false;

	public override char QuoteChar => coreReader.QuoteChar;

	public override XmlSpace XmlSpace => coreReader.XmlSpace;

	public override string XmlLang => coreReader.XmlLang;

	public override int AttributeCount => attributeCount;

	public override string this[int i] => GetAttribute(i);

	public override string this[string name] => GetAttribute(name);

	public override string this[string name, string namespaceURI] => GetAttribute(name, namespaceURI);

	public override bool EOF
	{
		get
		{
			if (cacheState == CachingReaderState.ReaderClosed)
			{
				return coreReader.EOF;
			}
			return false;
		}
	}

	public override ReadState ReadState => coreReader.ReadState;

	public override XmlNameTable NameTable => coreReaderNameTable;

	int IXmlLineInfo.LineNumber => cachedNode.LineNumber;

	int IXmlLineInfo.LinePosition => cachedNode.LinePosition;

	internal XsdCachingReader(XmlReader reader, IXmlLineInfo lineInfo, CachingEventHandler handlerMethod)
	{
		coreReader = reader;
		this.lineInfo = lineInfo;
		cacheHandler = handlerMethod;
		attributeEvents = new ValidatingReaderNodeData[8];
		contentEvents = new ValidatingReaderNodeData[4];
		Init();
	}

	private void Init()
	{
		coreReaderNameTable = coreReader.NameTable;
		cacheState = CachingReaderState.Init;
		contentIndex = 0;
		currentAttrIndex = -1;
		currentContentIndex = -1;
		attributeCount = 0;
		cachedNode = null;
		readAhead = false;
		if (coreReader.NodeType == XmlNodeType.Element)
		{
			ValidatingReaderNodeData validatingReaderNodeData = AddContent(coreReader.NodeType);
			validatingReaderNodeData.SetItemData(coreReader.LocalName, coreReader.Prefix, coreReader.NamespaceURI, coreReader.Depth);
			validatingReaderNodeData.SetLineInfo(lineInfo);
			RecordAttributes();
		}
	}

	internal void Reset(XmlReader reader)
	{
		coreReader = reader;
		Init();
	}

	public override string GetAttribute(string name)
	{
		int num = ((name.IndexOf(':') != -1) ? GetAttributeIndexWithPrefix(name) : GetAttributeIndexWithoutPrefix(name));
		if (num < 0)
		{
			return null;
		}
		return attributeEvents[num].RawValue;
	}

	public override string GetAttribute(string name, string namespaceURI)
	{
		namespaceURI = ((namespaceURI == null) ? string.Empty : coreReaderNameTable.Get(namespaceURI));
		name = coreReaderNameTable.Get(name);
		for (int i = 0; i < attributeCount; i++)
		{
			ValidatingReaderNodeData validatingReaderNodeData = attributeEvents[i];
			if (Ref.Equal(validatingReaderNodeData.LocalName, name) && Ref.Equal(validatingReaderNodeData.Namespace, namespaceURI))
			{
				return validatingReaderNodeData.RawValue;
			}
		}
		return null;
	}

	public override string GetAttribute(int i)
	{
		if (i < 0 || i >= attributeCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		return attributeEvents[i].RawValue;
	}

	public override bool MoveToAttribute(string name)
	{
		int num = ((name.IndexOf(':') != -1) ? GetAttributeIndexWithPrefix(name) : GetAttributeIndexWithoutPrefix(name));
		if (num >= 0)
		{
			currentAttrIndex = num;
			cachedNode = attributeEvents[num];
			return true;
		}
		return false;
	}

	public override bool MoveToAttribute(string name, string ns)
	{
		ns = ((ns == null) ? string.Empty : coreReaderNameTable.Get(ns));
		name = coreReaderNameTable.Get(name);
		for (int i = 0; i < attributeCount; i++)
		{
			ValidatingReaderNodeData validatingReaderNodeData = attributeEvents[i];
			if (Ref.Equal(validatingReaderNodeData.LocalName, name) && Ref.Equal(validatingReaderNodeData.Namespace, ns))
			{
				currentAttrIndex = i;
				cachedNode = attributeEvents[i];
				return true;
			}
		}
		return false;
	}

	public override void MoveToAttribute(int i)
	{
		if (i < 0 || i >= attributeCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		currentAttrIndex = i;
		cachedNode = attributeEvents[i];
	}

	public override bool MoveToFirstAttribute()
	{
		if (attributeCount == 0)
		{
			return false;
		}
		currentAttrIndex = 0;
		cachedNode = attributeEvents[0];
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (currentAttrIndex + 1 < attributeCount)
		{
			cachedNode = attributeEvents[++currentAttrIndex];
			return true;
		}
		return false;
	}

	public override bool MoveToElement()
	{
		if (cacheState != CachingReaderState.Replay || cachedNode.NodeType != XmlNodeType.Attribute)
		{
			return false;
		}
		currentContentIndex = 0;
		currentAttrIndex = -1;
		Read();
		return true;
	}

	public override bool Read()
	{
		switch (cacheState)
		{
		case CachingReaderState.Init:
			cacheState = CachingReaderState.Record;
			goto case CachingReaderState.Record;
		case CachingReaderState.Record:
		{
			ValidatingReaderNodeData validatingReaderNodeData = null;
			if (coreReader.Read())
			{
				switch (coreReader.NodeType)
				{
				case XmlNodeType.Element:
					cacheState = CachingReaderState.ReaderClosed;
					return false;
				case XmlNodeType.EndElement:
					validatingReaderNodeData = AddContent(coreReader.NodeType);
					validatingReaderNodeData.SetItemData(coreReader.LocalName, coreReader.Prefix, coreReader.NamespaceURI, coreReader.Depth);
					validatingReaderNodeData.SetLineInfo(lineInfo);
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
					validatingReaderNodeData = AddContent(coreReader.NodeType);
					validatingReaderNodeData.SetItemData(coreReader.Value);
					validatingReaderNodeData.SetLineInfo(lineInfo);
					validatingReaderNodeData.Depth = coreReader.Depth;
					break;
				}
				cachedNode = validatingReaderNodeData;
				return true;
			}
			cacheState = CachingReaderState.ReaderClosed;
			return false;
		}
		case CachingReaderState.Replay:
			if (currentContentIndex >= contentIndex)
			{
				cacheState = CachingReaderState.ReaderClosed;
				cacheHandler(this);
				if (coreReader.NodeType != XmlNodeType.Element || readAhead)
				{
					return coreReader.Read();
				}
				return true;
			}
			cachedNode = contentEvents[currentContentIndex];
			if (currentContentIndex > 0)
			{
				ClearAttributesInfo();
			}
			currentContentIndex++;
			return true;
		default:
			return false;
		}
	}

	internal ValidatingReaderNodeData RecordTextNode(string textValue, string originalStringValue, int depth, int lineNo, int linePos)
	{
		ValidatingReaderNodeData validatingReaderNodeData = AddContent(XmlNodeType.Text);
		validatingReaderNodeData.SetItemData(textValue, originalStringValue);
		validatingReaderNodeData.SetLineInfo(lineNo, linePos);
		validatingReaderNodeData.Depth = depth;
		return validatingReaderNodeData;
	}

	internal void SwitchTextNodeAndEndElement(string textValue, string originalStringValue)
	{
		ValidatingReaderNodeData validatingReaderNodeData = RecordTextNode(textValue, originalStringValue, coreReader.Depth + 1, 0, 0);
		int num = contentIndex - 2;
		ValidatingReaderNodeData validatingReaderNodeData2 = contentEvents[num];
		contentEvents[num] = validatingReaderNodeData;
		contentEvents[contentIndex - 1] = validatingReaderNodeData2;
	}

	internal void RecordEndElementNode()
	{
		ValidatingReaderNodeData validatingReaderNodeData = AddContent(XmlNodeType.EndElement);
		validatingReaderNodeData.SetItemData(coreReader.LocalName, coreReader.Prefix, coreReader.NamespaceURI, coreReader.Depth);
		validatingReaderNodeData.SetLineInfo(coreReader as IXmlLineInfo);
		if (coreReader.IsEmptyElement)
		{
			readAhead = true;
		}
	}

	internal string ReadOriginalContentAsString()
	{
		returnOriginalStringValues = true;
		string result = InternalReadContentAsString();
		returnOriginalStringValues = false;
		return result;
	}

	public override void Close()
	{
		coreReader.Close();
		cacheState = CachingReaderState.ReaderClosed;
	}

	public override void Skip()
	{
		XmlNodeType nodeType = cachedNode.NodeType;
		if (nodeType != XmlNodeType.Element)
		{
			if (nodeType != XmlNodeType.Attribute)
			{
				Read();
				return;
			}
			MoveToElement();
		}
		if (coreReader.NodeType != XmlNodeType.EndElement && !readAhead)
		{
			int num = coreReader.Depth - 1;
			while (coreReader.Read() && coreReader.Depth > num)
			{
			}
		}
		coreReader.Read();
		cacheState = CachingReaderState.ReaderClosed;
		cacheHandler(this);
	}

	public override string LookupNamespace(string prefix)
	{
		return coreReader.LookupNamespace(prefix);
	}

	public override void ResolveEntity()
	{
		throw new InvalidOperationException();
	}

	public override bool ReadAttributeValue()
	{
		if (cachedNode.NodeType != XmlNodeType.Attribute)
		{
			return false;
		}
		cachedNode = CreateDummyTextNode(cachedNode.RawValue, cachedNode.Depth + 1);
		return true;
	}

	bool IXmlLineInfo.HasLineInfo()
	{
		return true;
	}

	internal void SetToReplayMode()
	{
		cacheState = CachingReaderState.Replay;
		currentContentIndex = 0;
		currentAttrIndex = -1;
		Read();
	}

	internal XmlReader GetCoreReader()
	{
		return coreReader;
	}

	internal IXmlLineInfo GetLineInfo()
	{
		return lineInfo;
	}

	private void ClearAttributesInfo()
	{
		attributeCount = 0;
		currentAttrIndex = -1;
	}

	private ValidatingReaderNodeData AddAttribute(int attIndex)
	{
		ValidatingReaderNodeData validatingReaderNodeData = attributeEvents[attIndex];
		if (validatingReaderNodeData != null)
		{
			validatingReaderNodeData.Clear(XmlNodeType.Attribute);
			return validatingReaderNodeData;
		}
		if (attIndex >= attributeEvents.Length - 1)
		{
			ValidatingReaderNodeData[] destinationArray = new ValidatingReaderNodeData[attributeEvents.Length * 2];
			Array.Copy(attributeEvents, 0, destinationArray, 0, attributeEvents.Length);
			attributeEvents = destinationArray;
		}
		validatingReaderNodeData = attributeEvents[attIndex];
		if (validatingReaderNodeData == null)
		{
			validatingReaderNodeData = new ValidatingReaderNodeData(XmlNodeType.Attribute);
			attributeEvents[attIndex] = validatingReaderNodeData;
		}
		return validatingReaderNodeData;
	}

	private ValidatingReaderNodeData AddContent(XmlNodeType nodeType)
	{
		ValidatingReaderNodeData validatingReaderNodeData = contentEvents[contentIndex];
		if (validatingReaderNodeData != null)
		{
			validatingReaderNodeData.Clear(nodeType);
			contentIndex++;
			return validatingReaderNodeData;
		}
		if (contentIndex >= contentEvents.Length - 1)
		{
			ValidatingReaderNodeData[] destinationArray = new ValidatingReaderNodeData[contentEvents.Length * 2];
			Array.Copy(contentEvents, 0, destinationArray, 0, contentEvents.Length);
			contentEvents = destinationArray;
		}
		validatingReaderNodeData = contentEvents[contentIndex];
		if (validatingReaderNodeData == null)
		{
			validatingReaderNodeData = new ValidatingReaderNodeData(nodeType);
			contentEvents[contentIndex] = validatingReaderNodeData;
		}
		contentIndex++;
		return validatingReaderNodeData;
	}

	private void RecordAttributes()
	{
		attributeCount = coreReader.AttributeCount;
		if (coreReader.MoveToFirstAttribute())
		{
			int num = 0;
			do
			{
				ValidatingReaderNodeData validatingReaderNodeData = AddAttribute(num);
				validatingReaderNodeData.SetItemData(coreReader.LocalName, coreReader.Prefix, coreReader.NamespaceURI, coreReader.Depth);
				validatingReaderNodeData.SetLineInfo(lineInfo);
				validatingReaderNodeData.RawValue = coreReader.Value;
				num++;
			}
			while (coreReader.MoveToNextAttribute());
			coreReader.MoveToElement();
		}
	}

	private int GetAttributeIndexWithoutPrefix(string name)
	{
		name = coreReaderNameTable.Get(name);
		if (name == null)
		{
			return -1;
		}
		for (int i = 0; i < attributeCount; i++)
		{
			ValidatingReaderNodeData validatingReaderNodeData = attributeEvents[i];
			if (Ref.Equal(validatingReaderNodeData.LocalName, name) && validatingReaderNodeData.Prefix.Length == 0)
			{
				return i;
			}
		}
		return -1;
	}

	private int GetAttributeIndexWithPrefix(string name)
	{
		name = coreReaderNameTable.Get(name);
		if (name == null)
		{
			return -1;
		}
		for (int i = 0; i < attributeCount; i++)
		{
			if (Ref.Equal(attributeEvents[i].GetAtomizedNameWPrefix(coreReaderNameTable), name))
			{
				return i;
			}
		}
		return -1;
	}

	private ValidatingReaderNodeData CreateDummyTextNode(string attributeValue, int depth)
	{
		if (textNode == null)
		{
			textNode = new ValidatingReaderNodeData(XmlNodeType.Text);
		}
		textNode.Depth = depth;
		textNode.RawValue = attributeValue;
		return textNode;
	}

	public override Task<string> GetValueAsync()
	{
		if (returnOriginalStringValues)
		{
			return Task.FromResult(cachedNode.OriginalStringValue);
		}
		return Task.FromResult(cachedNode.RawValue);
	}

	public override async Task<bool> ReadAsync()
	{
		switch (cacheState)
		{
		case CachingReaderState.Init:
			cacheState = CachingReaderState.Record;
			goto case CachingReaderState.Record;
		case CachingReaderState.Record:
		{
			ValidatingReaderNodeData recordedNode = null;
			if (await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				switch (coreReader.NodeType)
				{
				case XmlNodeType.Element:
					cacheState = CachingReaderState.ReaderClosed;
					return false;
				case XmlNodeType.EndElement:
					recordedNode = AddContent(coreReader.NodeType);
					recordedNode.SetItemData(coreReader.LocalName, coreReader.Prefix, coreReader.NamespaceURI, coreReader.Depth);
					recordedNode.SetLineInfo(lineInfo);
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
				case XmlNodeType.Whitespace:
				case XmlNodeType.SignificantWhitespace:
				{
					recordedNode = AddContent(coreReader.NodeType);
					ValidatingReaderNodeData validatingReaderNodeData = recordedNode;
					validatingReaderNodeData.SetItemData(await coreReader.GetValueAsync().ConfigureAwait(continueOnCapturedContext: false));
					recordedNode.SetLineInfo(lineInfo);
					recordedNode.Depth = coreReader.Depth;
					break;
				}
				}
				cachedNode = recordedNode;
				return true;
			}
			cacheState = CachingReaderState.ReaderClosed;
			return false;
		}
		case CachingReaderState.Replay:
			if (currentContentIndex >= contentIndex)
			{
				cacheState = CachingReaderState.ReaderClosed;
				cacheHandler(this);
				if (coreReader.NodeType != XmlNodeType.Element || readAhead)
				{
					return await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				return true;
			}
			cachedNode = contentEvents[currentContentIndex];
			if (currentContentIndex > 0)
			{
				ClearAttributesInfo();
			}
			currentContentIndex++;
			return true;
		default:
			return false;
		}
	}

	public override async Task SkipAsync()
	{
		XmlNodeType nodeType = cachedNode.NodeType;
		if (nodeType != XmlNodeType.Element)
		{
			if (nodeType != XmlNodeType.Attribute)
			{
				await ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			MoveToElement();
		}
		if (coreReader.NodeType != XmlNodeType.EndElement && !readAhead)
		{
			int startDepth = coreReader.Depth - 1;
			while (await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false) && coreReader.Depth > startDepth)
			{
			}
		}
		await coreReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		cacheState = CachingReaderState.ReaderClosed;
		cacheHandler(this);
	}

	internal Task SetToReplayModeAsync()
	{
		cacheState = CachingReaderState.Replay;
		currentContentIndex = 0;
		currentAttrIndex = -1;
		return ReadAsync();
	}
}
