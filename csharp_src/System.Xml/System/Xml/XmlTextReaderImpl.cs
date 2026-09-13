using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XmlConfiguration;

namespace System.Xml;

internal class XmlTextReaderImpl : XmlReader, IXmlLineInfo, IXmlNamespaceResolver
{
	private enum ParsingFunction
	{
		ElementContent,
		NoData,
		OpenUrl,
		SwitchToInteractive,
		SwitchToInteractiveXmlDecl,
		DocumentContent,
		MoveToElementContent,
		PopElementContext,
		PopEmptyElementContext,
		ResetAttributesRootLevel,
		Error,
		Eof,
		ReaderClosed,
		EntityReference,
		InIncrementalRead,
		FragmentAttribute,
		ReportEndEntity,
		AfterResolveEntityInContent,
		AfterResolveEmptyEntityInContent,
		XmlDeclarationFragment,
		GoToEof,
		PartialTextValue,
		InReadAttributeValue,
		InReadValueChunk,
		InReadContentAsBinary,
		InReadElementContentAsBinary
	}

	private enum ParsingMode
	{
		Full,
		SkipNode,
		SkipContent
	}

	private enum EntityType
	{
		CharacterDec,
		CharacterHex,
		CharacterNamed,
		Expanded,
		Skipped,
		FakeExpanded,
		Unexpanded,
		ExpandedInAttribute
	}

	private enum EntityExpandType
	{
		All,
		OnlyGeneral,
		OnlyCharacter
	}

	private enum IncrementalReadState
	{
		Text,
		StartTag,
		PI,
		CDATA,
		Comment,
		Attributes,
		AttributeValue,
		ReadData,
		EndElement,
		End,
		ReadValueChunk_OnCachedValue,
		ReadValueChunk_OnPartialValue,
		ReadContentAsBinary_OnCachedValue,
		ReadContentAsBinary_OnPartialValue,
		ReadContentAsBinary_End
	}

	private class LaterInitParam
	{
		public bool useAsync;

		public Stream inputStream;

		public byte[] inputBytes;

		public int inputByteCount;

		public Uri inputbaseUri;

		public string inputUriStr;

		public XmlResolver inputUriResolver;

		public XmlParserContext inputContext;

		public TextReader inputTextReader;

		public InitInputType initType = InitInputType.Invalid;
	}

	private enum InitInputType
	{
		UriString,
		Stream,
		TextReader,
		Invalid
	}

	private enum ParseEndElementParseFunction
	{
		CheckEndTag,
		ReadData,
		Done
	}

	private class ParseTextState
	{
		public int outOrChars;

		public char[] chars;

		public int pos;

		public int rcount;

		public int rpos;

		public int orChars;

		public char c;

		public ParseTextState(int outOrChars, char[] chars, int pos, int rcount, int rpos, int orChars, char c)
		{
			this.outOrChars = outOrChars;
			this.chars = chars;
			this.pos = pos;
			this.rcount = rcount;
			this.rpos = rpos;
			this.orChars = orChars;
			this.c = c;
		}
	}

	private enum ParseTextFunction
	{
		ParseText,
		Entity,
		Surrogate,
		ReadData,
		NoValue,
		PartialValue
	}

	private struct ParsingState
	{
		internal char[] chars;

		internal int charPos;

		internal int charsUsed;

		internal Encoding encoding;

		internal bool appendMode;

		internal Stream stream;

		internal Decoder decoder;

		internal byte[] bytes;

		internal int bytePos;

		internal int bytesUsed;

		internal TextReader textReader;

		internal int lineNo;

		internal int lineStartPos;

		internal string baseUriStr;

		internal Uri baseUri;

		internal bool isEof;

		internal bool isStreamEof;

		internal IDtdEntityInfo entity;

		internal int entityId;

		internal bool eolNormalized;

		internal bool entityResolvedManually;

		internal int LineNo => lineNo;

		internal int LinePos => charPos - lineStartPos;

		internal void Clear()
		{
			chars = null;
			charPos = 0;
			charsUsed = 0;
			encoding = null;
			stream = null;
			decoder = null;
			bytes = null;
			bytePos = 0;
			bytesUsed = 0;
			textReader = null;
			lineNo = 1;
			lineStartPos = -1;
			baseUriStr = string.Empty;
			baseUri = null;
			isEof = false;
			isStreamEof = false;
			eolNormalized = true;
			entityResolvedManually = false;
		}

		internal void Close(bool closeInput)
		{
			if (closeInput)
			{
				if (stream != null)
				{
					stream.Close();
				}
				else if (textReader != null)
				{
					textReader.Close();
				}
			}
		}
	}

	private class XmlContext
	{
		internal XmlSpace xmlSpace;

		internal string xmlLang;

		internal string defaultNamespace;

		internal XmlContext previousContext;

		internal XmlContext()
		{
			xmlSpace = XmlSpace.None;
			xmlLang = string.Empty;
			defaultNamespace = string.Empty;
			previousContext = null;
		}

		internal XmlContext(XmlContext previousContext)
		{
			xmlSpace = previousContext.xmlSpace;
			xmlLang = previousContext.xmlLang;
			defaultNamespace = previousContext.defaultNamespace;
			this.previousContext = previousContext;
		}
	}

	private class NoNamespaceManager : XmlNamespaceManager
	{
		public override string DefaultNamespace => string.Empty;

		public override void PushScope()
		{
		}

		public override bool PopScope()
		{
			return false;
		}

		public override void AddNamespace(string prefix, string uri)
		{
		}

		public override void RemoveNamespace(string prefix, string uri)
		{
		}

		public override IEnumerator GetEnumerator()
		{
			return null;
		}

		public override IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return null;
		}

		public override string LookupNamespace(string prefix)
		{
			return string.Empty;
		}

		public override string LookupPrefix(string uri)
		{
			return null;
		}

		public override bool HasNamespace(string prefix)
		{
			return false;
		}
	}

	internal class DtdParserProxy : IDtdParserAdapterV1, IDtdParserAdapterWithValidation, IDtdParserAdapter
	{
		private XmlTextReaderImpl reader;

		XmlNameTable IDtdParserAdapter.NameTable => reader.DtdParserProxy_NameTable;

		IXmlNamespaceResolver IDtdParserAdapter.NamespaceResolver => reader.DtdParserProxy_NamespaceResolver;

		Uri IDtdParserAdapter.BaseUri => reader.DtdParserProxy_BaseUri;

		bool IDtdParserAdapter.IsEof => reader.DtdParserProxy_IsEof;

		char[] IDtdParserAdapter.ParsingBuffer => reader.DtdParserProxy_ParsingBuffer;

		int IDtdParserAdapter.ParsingBufferLength => reader.DtdParserProxy_ParsingBufferLength;

		int IDtdParserAdapter.CurrentPosition
		{
			get
			{
				return reader.DtdParserProxy_CurrentPosition;
			}
			set
			{
				reader.DtdParserProxy_CurrentPosition = value;
			}
		}

		int IDtdParserAdapter.EntityStackLength => reader.DtdParserProxy_EntityStackLength;

		bool IDtdParserAdapter.IsEntityEolNormalized => reader.DtdParserProxy_IsEntityEolNormalized;

		int IDtdParserAdapter.LineNo => reader.DtdParserProxy_LineNo;

		int IDtdParserAdapter.LineStartPosition => reader.DtdParserProxy_LineStartPosition;

		bool IDtdParserAdapterWithValidation.DtdValidation => reader.DtdParserProxy_DtdValidation;

		IValidationEventHandling IDtdParserAdapterWithValidation.ValidationEventHandling => reader.DtdParserProxy_ValidationEventHandling;

		bool IDtdParserAdapterV1.Normalization => reader.DtdParserProxy_Normalization;

		bool IDtdParserAdapterV1.Namespaces => reader.DtdParserProxy_Namespaces;

		bool IDtdParserAdapterV1.V1CompatibilityMode => reader.DtdParserProxy_V1CompatibilityMode;

		internal DtdParserProxy(XmlTextReaderImpl reader)
		{
			this.reader = reader;
		}

		void IDtdParserAdapter.OnNewLine(int pos)
		{
			reader.DtdParserProxy_OnNewLine(pos);
		}

		int IDtdParserAdapter.ReadData()
		{
			return reader.DtdParserProxy_ReadData();
		}

		int IDtdParserAdapter.ParseNumericCharRef(StringBuilder internalSubsetBuilder)
		{
			return reader.DtdParserProxy_ParseNumericCharRef(internalSubsetBuilder);
		}

		int IDtdParserAdapter.ParseNamedCharRef(bool expand, StringBuilder internalSubsetBuilder)
		{
			return reader.DtdParserProxy_ParseNamedCharRef(expand, internalSubsetBuilder);
		}

		void IDtdParserAdapter.ParsePI(StringBuilder sb)
		{
			reader.DtdParserProxy_ParsePI(sb);
		}

		void IDtdParserAdapter.ParseComment(StringBuilder sb)
		{
			reader.DtdParserProxy_ParseComment(sb);
		}

		bool IDtdParserAdapter.PushEntity(IDtdEntityInfo entity, out int entityId)
		{
			return reader.DtdParserProxy_PushEntity(entity, out entityId);
		}

		bool IDtdParserAdapter.PopEntity(out IDtdEntityInfo oldEntity, out int newEntityId)
		{
			return reader.DtdParserProxy_PopEntity(out oldEntity, out newEntityId);
		}

		bool IDtdParserAdapter.PushExternalSubset(string systemId, string publicId)
		{
			return reader.DtdParserProxy_PushExternalSubset(systemId, publicId);
		}

		void IDtdParserAdapter.PushInternalDtd(string baseUri, string internalDtd)
		{
			reader.DtdParserProxy_PushInternalDtd(baseUri, internalDtd);
		}

		void IDtdParserAdapter.Throw(Exception e)
		{
			reader.DtdParserProxy_Throw(e);
		}

		void IDtdParserAdapter.OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo)
		{
			reader.DtdParserProxy_OnSystemId(systemId, keywordLineInfo, systemLiteralLineInfo);
		}

		void IDtdParserAdapter.OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo)
		{
			reader.DtdParserProxy_OnPublicId(publicId, keywordLineInfo, publicLiteralLineInfo);
		}

		Task<int> IDtdParserAdapter.ReadDataAsync()
		{
			return reader.DtdParserProxy_ReadDataAsync();
		}

		Task<int> IDtdParserAdapter.ParseNumericCharRefAsync(StringBuilder internalSubsetBuilder)
		{
			return reader.DtdParserProxy_ParseNumericCharRefAsync(internalSubsetBuilder);
		}

		Task<int> IDtdParserAdapter.ParseNamedCharRefAsync(bool expand, StringBuilder internalSubsetBuilder)
		{
			return reader.DtdParserProxy_ParseNamedCharRefAsync(expand, internalSubsetBuilder);
		}

		Task IDtdParserAdapter.ParsePIAsync(StringBuilder sb)
		{
			return reader.DtdParserProxy_ParsePIAsync(sb);
		}

		Task IDtdParserAdapter.ParseCommentAsync(StringBuilder sb)
		{
			return reader.DtdParserProxy_ParseCommentAsync(sb);
		}

		Task<Tuple<int, bool>> IDtdParserAdapter.PushEntityAsync(IDtdEntityInfo entity)
		{
			return reader.DtdParserProxy_PushEntityAsync(entity);
		}

		Task<bool> IDtdParserAdapter.PushExternalSubsetAsync(string systemId, string publicId)
		{
			return reader.DtdParserProxy_PushExternalSubsetAsync(systemId, publicId);
		}
	}

	private class NodeData : IComparable
	{
		private static volatile NodeData s_None;

		internal XmlNodeType type;

		internal string localName;

		internal string prefix;

		internal string ns;

		internal string nameWPrefix;

		private string value;

		private char[] chars;

		private int valueStartPos;

		private int valueLength;

		internal LineInfo lineInfo;

		internal LineInfo lineInfo2;

		internal char quoteChar;

		internal int depth;

		private bool isEmptyOrDefault;

		internal int entityId;

		internal bool xmlContextPushed;

		internal NodeData nextAttrValueChunk;

		internal object schemaType;

		internal object typedValue;

		internal static NodeData None
		{
			get
			{
				if (s_None == null)
				{
					s_None = new NodeData();
				}
				return s_None;
			}
		}

		internal int LineNo => lineInfo.lineNo;

		internal int LinePos => lineInfo.linePos;

		internal bool IsEmptyElement
		{
			get
			{
				if (type == XmlNodeType.Element)
				{
					return isEmptyOrDefault;
				}
				return false;
			}
			set
			{
				isEmptyOrDefault = value;
			}
		}

		internal bool IsDefaultAttribute
		{
			get
			{
				if (type == XmlNodeType.Attribute)
				{
					return isEmptyOrDefault;
				}
				return false;
			}
			set
			{
				isEmptyOrDefault = value;
			}
		}

		internal bool ValueBuffered => value == null;

		internal string StringValue
		{
			get
			{
				if (value == null)
				{
					value = new string(chars, valueStartPos, valueLength);
				}
				return value;
			}
		}

		internal NodeData()
		{
			Clear(XmlNodeType.None);
			xmlContextPushed = false;
		}

		internal void TrimSpacesInValue()
		{
			if (ValueBuffered)
			{
				StripSpaces(chars, valueStartPos, ref valueLength);
			}
			else
			{
				value = StripSpaces(value);
			}
		}

		internal void Clear(XmlNodeType type)
		{
			this.type = type;
			ClearName();
			value = string.Empty;
			valueStartPos = -1;
			nameWPrefix = string.Empty;
			schemaType = null;
			typedValue = null;
		}

		internal void ClearName()
		{
			localName = string.Empty;
			prefix = string.Empty;
			ns = string.Empty;
			nameWPrefix = string.Empty;
		}

		internal void SetLineInfo(int lineNo, int linePos)
		{
			lineInfo.Set(lineNo, linePos);
		}

		internal void SetLineInfo2(int lineNo, int linePos)
		{
			lineInfo2.Set(lineNo, linePos);
		}

		internal void SetValueNode(XmlNodeType type, string value)
		{
			this.type = type;
			ClearName();
			this.value = value;
			valueStartPos = -1;
		}

		internal void SetValueNode(XmlNodeType type, char[] chars, int startPos, int len)
		{
			this.type = type;
			ClearName();
			value = null;
			this.chars = chars;
			valueStartPos = startPos;
			valueLength = len;
		}

		internal void SetNamedNode(XmlNodeType type, string localName)
		{
			SetNamedNode(type, localName, string.Empty, localName);
		}

		internal void SetNamedNode(XmlNodeType type, string localName, string prefix, string nameWPrefix)
		{
			this.type = type;
			this.localName = localName;
			this.prefix = prefix;
			this.nameWPrefix = nameWPrefix;
			ns = string.Empty;
			value = string.Empty;
			valueStartPos = -1;
		}

		internal void SetValue(string value)
		{
			valueStartPos = -1;
			this.value = value;
		}

		internal void SetValue(char[] chars, int startPos, int len)
		{
			value = null;
			this.chars = chars;
			valueStartPos = startPos;
			valueLength = len;
		}

		internal void OnBufferInvalidated()
		{
			if (value == null)
			{
				value = new string(chars, valueStartPos, valueLength);
			}
			valueStartPos = -1;
		}

		internal void CopyTo(int valueOffset, StringBuilder sb)
		{
			if (value == null)
			{
				sb.Append(chars, valueStartPos + valueOffset, valueLength - valueOffset);
			}
			else if (valueOffset <= 0)
			{
				sb.Append(value);
			}
			else
			{
				sb.Append(value, valueOffset, value.Length - valueOffset);
			}
		}

		internal int CopyTo(int valueOffset, char[] buffer, int offset, int length)
		{
			if (value == null)
			{
				int num = valueLength - valueOffset;
				if (num > length)
				{
					num = length;
				}
				BlockCopyChars(chars, valueStartPos + valueOffset, buffer, offset, num);
				return num;
			}
			int num2 = value.Length - valueOffset;
			if (num2 > length)
			{
				num2 = length;
			}
			value.CopyTo(valueOffset, buffer, offset, num2);
			return num2;
		}

		internal int CopyToBinary(IncrementalReadDecoder decoder, int valueOffset)
		{
			if (value == null)
			{
				return decoder.Decode(chars, valueStartPos + valueOffset, valueLength - valueOffset);
			}
			return decoder.Decode(value, valueOffset, value.Length - valueOffset);
		}

		internal void AdjustLineInfo(int valueOffset, bool isNormalized, ref LineInfo lineInfo)
		{
			if (valueOffset != 0)
			{
				if (valueStartPos != -1)
				{
					XmlTextReaderImpl.AdjustLineInfo(chars, valueStartPos, valueStartPos + valueOffset, isNormalized, ref lineInfo);
				}
				else
				{
					XmlTextReaderImpl.AdjustLineInfo(value, 0, valueOffset, isNormalized, ref lineInfo);
				}
			}
		}

		internal string GetNameWPrefix(XmlNameTable nt)
		{
			if (nameWPrefix != null)
			{
				return nameWPrefix;
			}
			return CreateNameWPrefix(nt);
		}

		internal string CreateNameWPrefix(XmlNameTable nt)
		{
			if (prefix.Length == 0)
			{
				nameWPrefix = localName;
			}
			else
			{
				nameWPrefix = nt.Add(prefix + ":" + localName);
			}
			return nameWPrefix;
		}

		int IComparable.CompareTo(object obj)
		{
			if (obj is NodeData nodeData)
			{
				if (Ref.Equal(localName, nodeData.localName))
				{
					if (Ref.Equal(ns, nodeData.ns))
					{
						return 0;
					}
					return string.CompareOrdinal(ns, nodeData.ns);
				}
				return string.CompareOrdinal(localName, nodeData.localName);
			}
			return 1;
		}
	}

	private class DtdDefaultAttributeInfoToNodeDataComparer : IComparer<object>
	{
		private static IComparer<object> s_instance = new DtdDefaultAttributeInfoToNodeDataComparer();

		internal static IComparer<object> Instance => s_instance;

		public int Compare(object x, object y)
		{
			if (x == null)
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
			if (y == null)
			{
				return 1;
			}
			string localName;
			string prefix;
			if (x is NodeData nodeData)
			{
				localName = nodeData.localName;
				prefix = nodeData.prefix;
			}
			else
			{
				if (!(x is IDtdDefaultAttributeInfo dtdDefaultAttributeInfo))
				{
					throw new XmlException("An XML error has occurred.", string.Empty);
				}
				localName = dtdDefaultAttributeInfo.LocalName;
				prefix = dtdDefaultAttributeInfo.Prefix;
			}
			string localName2;
			string prefix2;
			if (y is NodeData nodeData2)
			{
				localName2 = nodeData2.localName;
				prefix2 = nodeData2.prefix;
			}
			else
			{
				if (!(y is IDtdDefaultAttributeInfo dtdDefaultAttributeInfo2))
				{
					throw new XmlException("An XML error has occurred.", string.Empty);
				}
				localName2 = dtdDefaultAttributeInfo2.LocalName;
				prefix2 = dtdDefaultAttributeInfo2.Prefix;
			}
			int num = string.Compare(localName, localName2, StringComparison.Ordinal);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(prefix, prefix2, StringComparison.Ordinal);
		}
	}

	internal delegate void OnDefaultAttributeUseDelegate(IDtdDefaultAttributeInfo defaultAttribute, XmlTextReaderImpl coreReader);

	private readonly bool useAsync;

	private LaterInitParam laterInitParam;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	private ParsingState ps;

	private ParsingFunction parsingFunction;

	private ParsingFunction nextParsingFunction;

	private ParsingFunction nextNextParsingFunction;

	private NodeData[] nodes;

	private NodeData curNode;

	private int index;

	private int curAttrIndex = -1;

	private int attrCount;

	private int attrHashtable;

	private int attrDuplWalkCount;

	private bool attrNeedNamespaceLookup;

	private bool fullAttrCleanup;

	private NodeData[] attrDuplSortingArray;

	private XmlNameTable nameTable;

	private bool nameTableFromSettings;

	private XmlResolver xmlResolver;

	private string url = string.Empty;

	private bool normalize;

	private bool supportNamespaces = true;

	private WhitespaceHandling whitespaceHandling;

	private DtdProcessing dtdProcessing = DtdProcessing.Parse;

	private EntityHandling entityHandling;

	private bool ignorePIs;

	private bool ignoreComments;

	private bool checkCharacters;

	private int lineNumberOffset;

	private int linePositionOffset;

	private bool closeInput;

	private long maxCharactersInDocument;

	private long maxCharactersFromEntities;

	private bool v1Compat;

	private XmlNamespaceManager namespaceManager;

	private string lastPrefix = string.Empty;

	private XmlContext xmlContext;

	private ParsingState[] parsingStatesStack;

	private int parsingStatesStackTop = -1;

	private string reportedBaseUri;

	private Encoding reportedEncoding;

	private IDtdInfo dtdInfo;

	private XmlNodeType fragmentType = XmlNodeType.Document;

	private XmlParserContext fragmentParserContext;

	private bool fragment;

	private IncrementalReadDecoder incReadDecoder;

	private IncrementalReadState incReadState;

	private LineInfo incReadLineInfo;

	private BinHexDecoder binHexDecoder;

	private Base64Decoder base64Decoder;

	private int incReadDepth;

	private int incReadLeftStartPos;

	private int incReadLeftEndPos;

	private IncrementalReadCharsDecoder readCharsDecoder;

	private int attributeValueBaseEntityId;

	private bool emptyEntityInAttributeResolved;

	private IValidationEventHandling validationEventHandling;

	private OnDefaultAttributeUseDelegate onDefaultAttributeUse;

	private bool validatingReaderCompatFlag;

	private bool addDefaultAttributesAndNormalize;

	private StringBuilder stringBuilder;

	private bool rootElementParsed;

	private bool standalone;

	private int nextEntityId = 1;

	private ParsingMode parsingMode;

	private ReadState readState;

	private IDtdEntityInfo lastEntity;

	private bool afterResetState;

	private int documentStartBytePos;

	private int readValueOffset;

	private long charactersInDocument;

	private long charactersFromEntities;

	private Dictionary<IDtdEntityInfo, IDtdEntityInfo> currentEntities;

	private bool disableUndeclaredEntityCheck;

	private XmlReader outerReader;

	private bool xmlResolverIsSet;

	private string Xml;

	private string XmlNs;

	private const int MaxBytesToMove = 128;

	private const int ApproxXmlDeclLength = 80;

	private const int NodesInitialSize = 8;

	private const int InitialAttributesCount = 4;

	private const int InitialParsingStateStackSize = 2;

	private const int InitialParsingStatesDepth = 2;

	private const int DtdChidrenInitialSize = 2;

	private const int MaxByteSequenceLen = 6;

	private const int MaxAttrDuplWalkCount = 250;

	private const int MinWhitespaceLookahedCount = 4096;

	private const string XmlDeclarationBegining = "<?xml";

	private ParseEndElementParseFunction parseEndElement_NextFunc;

	private ParseTextFunction parseText_NextFunction;

	private ParseTextState lastParseTextState;

	private Task<Tuple<int, int, int, bool>> parseText_dummyTask = Task.FromResult(new Tuple<int, int, int, bool>(0, 0, 0, item4: false));

	public override XmlReaderSettings Settings
	{
		get
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			if (nameTableFromSettings)
			{
				xmlReaderSettings.NameTable = nameTable;
			}
			switch (fragmentType)
			{
			default:
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Auto;
				break;
			case XmlNodeType.Element:
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
				break;
			case XmlNodeType.Document:
				xmlReaderSettings.ConformanceLevel = ConformanceLevel.Document;
				break;
			}
			xmlReaderSettings.CheckCharacters = checkCharacters;
			xmlReaderSettings.LineNumberOffset = lineNumberOffset;
			xmlReaderSettings.LinePositionOffset = linePositionOffset;
			xmlReaderSettings.IgnoreWhitespace = whitespaceHandling == WhitespaceHandling.Significant;
			xmlReaderSettings.IgnoreProcessingInstructions = ignorePIs;
			xmlReaderSettings.IgnoreComments = ignoreComments;
			xmlReaderSettings.DtdProcessing = dtdProcessing;
			xmlReaderSettings.MaxCharactersInDocument = maxCharactersInDocument;
			xmlReaderSettings.MaxCharactersFromEntities = maxCharactersFromEntities;
			if (!XmlReaderSettings.EnableLegacyXmlSettings())
			{
				xmlReaderSettings.XmlResolver = xmlResolver;
			}
			xmlReaderSettings.ReadOnly = true;
			return xmlReaderSettings;
		}
	}

	public override XmlNodeType NodeType => curNode.type;

	public override string Name => curNode.GetNameWPrefix(nameTable);

	public override string LocalName => curNode.localName;

	public override string NamespaceURI => curNode.ns;

	public override string Prefix => curNode.prefix;

	public override string Value
	{
		get
		{
			if (parsingFunction >= ParsingFunction.PartialTextValue)
			{
				if (parsingFunction == ParsingFunction.PartialTextValue)
				{
					FinishPartialValue();
					parsingFunction = nextParsingFunction;
				}
				else
				{
					FinishOtherValueIterator();
				}
			}
			return curNode.StringValue;
		}
	}

	public override int Depth => curNode.depth;

	public override string BaseURI => reportedBaseUri;

	public override bool IsEmptyElement => curNode.IsEmptyElement;

	public override bool IsDefault => curNode.IsDefaultAttribute;

	public override char QuoteChar
	{
		get
		{
			if (curNode.type != XmlNodeType.Attribute)
			{
				return '"';
			}
			return curNode.quoteChar;
		}
	}

	public override XmlSpace XmlSpace => xmlContext.xmlSpace;

	public override string XmlLang => xmlContext.xmlLang;

	public override ReadState ReadState => readState;

	public override bool EOF => parsingFunction == ParsingFunction.Eof;

	public override XmlNameTable NameTable => nameTable;

	public override bool CanResolveEntity => true;

	public override int AttributeCount => attrCount;

	internal XmlReader OuterReader
	{
		get
		{
			return outerReader;
		}
		set
		{
			outerReader = value;
		}
	}

	public override bool CanReadBinaryContent => true;

	public override bool CanReadValueChunk => true;

	public int LineNumber => curNode.LineNo;

	public int LinePosition => curNode.LinePos;

	internal bool Namespaces
	{
		get
		{
			return supportNamespaces;
		}
		set
		{
			if (readState != ReadState.Initial)
			{
				throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
			}
			supportNamespaces = value;
			if (value)
			{
				if (namespaceManager is NoNamespaceManager)
				{
					if (fragment && fragmentParserContext != null && fragmentParserContext.NamespaceManager != null)
					{
						namespaceManager = fragmentParserContext.NamespaceManager;
					}
					else
					{
						namespaceManager = new XmlNamespaceManager(nameTable);
					}
				}
				xmlContext.defaultNamespace = namespaceManager.LookupNamespace(string.Empty);
			}
			else
			{
				if (!(namespaceManager is NoNamespaceManager))
				{
					namespaceManager = new NoNamespaceManager();
				}
				xmlContext.defaultNamespace = string.Empty;
			}
		}
	}

	internal bool Normalization
	{
		get
		{
			return normalize;
		}
		set
		{
			if (readState == ReadState.Closed)
			{
				throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
			}
			normalize = value;
			if (ps.entity == null || ps.entity.IsExternal)
			{
				ps.eolNormalized = !value;
			}
		}
	}

	internal Encoding Encoding
	{
		get
		{
			if (readState != ReadState.Interactive)
			{
				return null;
			}
			return reportedEncoding;
		}
	}

	internal WhitespaceHandling WhitespaceHandling
	{
		get
		{
			return whitespaceHandling;
		}
		set
		{
			if (readState == ReadState.Closed)
			{
				throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
			}
			if ((uint)value > 2u)
			{
				throw new XmlException("Expected WhitespaceHandling.None, or WhitespaceHandling.All, or WhitespaceHandling.Significant.", string.Empty);
			}
			whitespaceHandling = value;
		}
	}

	internal DtdProcessing DtdProcessing
	{
		get
		{
			return dtdProcessing;
		}
		set
		{
			if ((uint)value > 2u)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			dtdProcessing = value;
		}
	}

	internal EntityHandling EntityHandling
	{
		get
		{
			return entityHandling;
		}
		set
		{
			if (value != EntityHandling.ExpandEntities && value != EntityHandling.ExpandCharEntities)
			{
				throw new XmlException("Expected EntityHandling.ExpandEntities or EntityHandling.ExpandCharEntities.", string.Empty);
			}
			entityHandling = value;
		}
	}

	internal bool IsResolverSet => xmlResolverIsSet;

	internal XmlResolver XmlResolver
	{
		set
		{
			xmlResolver = value;
			xmlResolverIsSet = true;
			ps.baseUri = null;
			for (int i = 0; i <= parsingStatesStackTop; i++)
			{
				parsingStatesStack[i].baseUri = null;
			}
		}
	}

	internal XmlNameTable DtdParserProxy_NameTable => nameTable;

	internal IXmlNamespaceResolver DtdParserProxy_NamespaceResolver => namespaceManager;

	internal bool DtdParserProxy_DtdValidation => DtdValidation;

	internal bool DtdParserProxy_Normalization => normalize;

	internal bool DtdParserProxy_Namespaces => supportNamespaces;

	internal bool DtdParserProxy_V1CompatibilityMode => v1Compat;

	internal Uri DtdParserProxy_BaseUri
	{
		get
		{
			if (ps.baseUriStr.Length > 0 && ps.baseUri == null && xmlResolver != null)
			{
				ps.baseUri = xmlResolver.ResolveUri(null, ps.baseUriStr);
			}
			return ps.baseUri;
		}
	}

	internal bool DtdParserProxy_IsEof => ps.isEof;

	internal char[] DtdParserProxy_ParsingBuffer => ps.chars;

	internal int DtdParserProxy_ParsingBufferLength => ps.charsUsed;

	internal int DtdParserProxy_CurrentPosition
	{
		get
		{
			return ps.charPos;
		}
		set
		{
			ps.charPos = value;
		}
	}

	internal int DtdParserProxy_EntityStackLength => parsingStatesStackTop + 1;

	internal bool DtdParserProxy_IsEntityEolNormalized => ps.eolNormalized;

	internal IValidationEventHandling DtdParserProxy_ValidationEventHandling
	{
		get
		{
			return validationEventHandling;
		}
		set
		{
			validationEventHandling = value;
		}
	}

	internal int DtdParserProxy_LineNo => ps.LineNo;

	internal int DtdParserProxy_LineStartPosition => ps.lineStartPos;

	private bool IsResolverNull
	{
		get
		{
			if (xmlResolver != null)
			{
				if (XmlReaderSection.ProhibitDefaultUrlResolver)
				{
					return !xmlResolverIsSet;
				}
				return false;
			}
			return true;
		}
	}

	private bool InAttributeValueIterator
	{
		get
		{
			if (attrCount > 0)
			{
				return parsingFunction >= ParsingFunction.InReadAttributeValue;
			}
			return false;
		}
	}

	private bool DtdValidation => validationEventHandling != null;

	private bool InEntity => parsingStatesStackTop >= 0;

	internal override IDtdInfo DtdInfo => dtdInfo;

	internal IValidationEventHandling ValidationEventHandling
	{
		set
		{
			validationEventHandling = value;
		}
	}

	internal OnDefaultAttributeUseDelegate OnDefaultAttributeUse
	{
		set
		{
			onDefaultAttributeUse = value;
		}
	}

	internal bool XmlValidatingReaderCompatibilityMode
	{
		set
		{
			validatingReaderCompatFlag = value;
			if (value)
			{
				nameTable.Add("http://www.w3.org/2001/XMLSchema");
				nameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
				nameTable.Add("urn:schemas-microsoft-com:datatypes");
			}
		}
	}

	internal XmlNodeType FragmentType => fragmentType;

	internal object InternalSchemaType
	{
		get
		{
			return curNode.schemaType;
		}
		set
		{
			curNode.schemaType = value;
		}
	}

	internal object InternalTypedValue
	{
		get
		{
			return curNode.typedValue;
		}
		set
		{
			curNode.typedValue = value;
		}
	}

	internal bool StandAlone => standalone;

	internal override XmlNamespaceManager NamespaceManager => namespaceManager;

	internal bool V1Compat => v1Compat;

	internal ConformanceLevel V1ComformanceLevel
	{
		get
		{
			if (fragmentType != XmlNodeType.Element)
			{
				return ConformanceLevel.Document;
			}
			return ConformanceLevel.Fragment;
		}
	}

	internal bool DisableUndeclaredEntityCheck
	{
		set
		{
			disableUndeclaredEntityCheck = value;
		}
	}

	internal XmlTextReaderImpl()
	{
		curNode = new NodeData();
		parsingFunction = ParsingFunction.NoData;
	}

	internal XmlTextReaderImpl(XmlNameTable nt)
	{
		v1Compat = true;
		outerReader = this;
		nameTable = nt;
		nt.Add(string.Empty);
		if (!XmlReaderSettings.EnableLegacyXmlSettings())
		{
			xmlResolver = null;
		}
		else
		{
			xmlResolver = new XmlUrlResolver();
		}
		Xml = nt.Add("xml");
		XmlNs = nt.Add("xmlns");
		nodes = new NodeData[8];
		nodes[0] = new NodeData();
		curNode = nodes[0];
		stringBuilder = new StringBuilder();
		xmlContext = new XmlContext();
		parsingFunction = ParsingFunction.SwitchToInteractiveXmlDecl;
		nextParsingFunction = ParsingFunction.DocumentContent;
		entityHandling = EntityHandling.ExpandCharEntities;
		whitespaceHandling = WhitespaceHandling.All;
		closeInput = true;
		maxCharactersInDocument = 0L;
		maxCharactersFromEntities = 10000000L;
		charactersInDocument = 0L;
		charactersFromEntities = 0L;
		ps.lineNo = 1;
		ps.lineStartPos = -1;
	}

	private XmlTextReaderImpl(XmlResolver resolver, XmlReaderSettings settings, XmlParserContext context)
	{
		useAsync = settings.Async;
		v1Compat = false;
		outerReader = this;
		xmlContext = new XmlContext();
		XmlNameTable xmlNameTable = settings.NameTable;
		if (context == null)
		{
			if (xmlNameTable == null)
			{
				xmlNameTable = new NameTable();
			}
			else
			{
				nameTableFromSettings = true;
			}
			nameTable = xmlNameTable;
			namespaceManager = new XmlNamespaceManager(xmlNameTable);
		}
		else
		{
			SetupFromParserContext(context, settings);
			xmlNameTable = nameTable;
		}
		xmlNameTable.Add(string.Empty);
		Xml = xmlNameTable.Add("xml");
		XmlNs = xmlNameTable.Add("xmlns");
		xmlResolver = resolver;
		nodes = new NodeData[8];
		nodes[0] = new NodeData();
		curNode = nodes[0];
		stringBuilder = new StringBuilder();
		entityHandling = EntityHandling.ExpandEntities;
		xmlResolverIsSet = settings.IsXmlResolverSet;
		whitespaceHandling = (settings.IgnoreWhitespace ? WhitespaceHandling.Significant : WhitespaceHandling.All);
		normalize = true;
		ignorePIs = settings.IgnoreProcessingInstructions;
		ignoreComments = settings.IgnoreComments;
		checkCharacters = settings.CheckCharacters;
		lineNumberOffset = settings.LineNumberOffset;
		linePositionOffset = settings.LinePositionOffset;
		ps.lineNo = lineNumberOffset + 1;
		ps.lineStartPos = -linePositionOffset - 1;
		curNode.SetLineInfo(ps.LineNo - 1, ps.LinePos - 1);
		dtdProcessing = settings.DtdProcessing;
		maxCharactersInDocument = settings.MaxCharactersInDocument;
		maxCharactersFromEntities = settings.MaxCharactersFromEntities;
		charactersInDocument = 0L;
		charactersFromEntities = 0L;
		fragmentParserContext = context;
		parsingFunction = ParsingFunction.SwitchToInteractiveXmlDecl;
		nextParsingFunction = ParsingFunction.DocumentContent;
		switch (settings.ConformanceLevel)
		{
		case ConformanceLevel.Auto:
			fragmentType = XmlNodeType.None;
			fragment = true;
			break;
		case ConformanceLevel.Fragment:
			fragmentType = XmlNodeType.Element;
			fragment = true;
			break;
		default:
			fragmentType = XmlNodeType.Document;
			break;
		}
	}

	internal XmlTextReaderImpl(Stream input)
		: this(string.Empty, input, new NameTable())
	{
	}

	internal XmlTextReaderImpl(Stream input, XmlNameTable nt)
		: this(string.Empty, input, nt)
	{
	}

	internal XmlTextReaderImpl(string url, Stream input)
		: this(url, input, new NameTable())
	{
	}

	internal XmlTextReaderImpl(string url, Stream input, XmlNameTable nt)
		: this(nt)
	{
		namespaceManager = new XmlNamespaceManager(nt);
		if (url == null || url.Length == 0)
		{
			InitStreamInput(input, null);
		}
		else
		{
			InitStreamInput(url, input, null);
		}
		reportedBaseUri = ps.baseUriStr;
		reportedEncoding = ps.encoding;
	}

	internal XmlTextReaderImpl(TextReader input)
		: this(string.Empty, input, new NameTable())
	{
	}

	internal XmlTextReaderImpl(TextReader input, XmlNameTable nt)
		: this(string.Empty, input, nt)
	{
	}

	internal XmlTextReaderImpl(string url, TextReader input)
		: this(url, input, new NameTable())
	{
	}

	internal XmlTextReaderImpl(string url, TextReader input, XmlNameTable nt)
		: this(nt)
	{
		namespaceManager = new XmlNamespaceManager(nt);
		reportedBaseUri = ((url != null) ? url : string.Empty);
		InitTextReaderInput(reportedBaseUri, input);
		reportedEncoding = ps.encoding;
	}

	internal XmlTextReaderImpl(Stream xmlFragment, XmlNodeType fragType, XmlParserContext context)
		: this((context != null && context.NameTable != null) ? context.NameTable : new NameTable())
	{
		Encoding encoding = context?.Encoding;
		if (context == null || context.BaseURI == null || context.BaseURI.Length == 0)
		{
			InitStreamInput(xmlFragment, encoding);
		}
		else
		{
			InitStreamInput(GetTempResolver().ResolveUri(null, context.BaseURI), xmlFragment, encoding);
		}
		InitFragmentReader(fragType, context, allowXmlDeclFragment: false);
		reportedBaseUri = ps.baseUriStr;
		reportedEncoding = ps.encoding;
	}

	internal XmlTextReaderImpl(string xmlFragment, XmlNodeType fragType, XmlParserContext context)
		: this((context == null || context.NameTable == null) ? new NameTable() : context.NameTable)
	{
		if (xmlFragment == null)
		{
			xmlFragment = string.Empty;
		}
		if (context == null)
		{
			InitStringInput(string.Empty, Encoding.Unicode, xmlFragment);
		}
		else
		{
			reportedBaseUri = context.BaseURI;
			InitStringInput(context.BaseURI, Encoding.Unicode, xmlFragment);
		}
		InitFragmentReader(fragType, context, allowXmlDeclFragment: false);
		reportedEncoding = ps.encoding;
	}

	internal XmlTextReaderImpl(string xmlFragment, XmlParserContext context)
		: this((context == null || context.NameTable == null) ? new NameTable() : context.NameTable)
	{
		InitStringInput((context == null) ? string.Empty : context.BaseURI, Encoding.Unicode, "<?xml " + xmlFragment + "?>");
		InitFragmentReader(XmlNodeType.XmlDeclaration, context, allowXmlDeclFragment: true);
	}

	public XmlTextReaderImpl(string url)
		: this(url, new NameTable())
	{
	}

	public XmlTextReaderImpl(string url, XmlNameTable nt)
		: this(nt)
	{
		if (url == null)
		{
			throw new ArgumentNullException("url");
		}
		if (url.Length == 0)
		{
			throw new ArgumentException(Res.GetString("The URL cannot be empty."), "url");
		}
		namespaceManager = new XmlNamespaceManager(nt);
		this.url = url;
		ps.baseUri = GetTempResolver().ResolveUri(null, url);
		ps.baseUriStr = ps.baseUri.ToString();
		reportedBaseUri = ps.baseUriStr;
		parsingFunction = ParsingFunction.OpenUrl;
	}

	internal XmlTextReaderImpl(string uriStr, XmlReaderSettings settings, XmlParserContext context, XmlResolver uriResolver)
		: this(settings.GetXmlResolver(), settings, context)
	{
		Uri uri = uriResolver.ResolveUri(null, uriStr);
		string text = uri.ToString();
		if (context != null && context.BaseURI != null && context.BaseURI.Length > 0 && !UriEqual(uri, text, context.BaseURI, settings.GetXmlResolver()))
		{
			if (text.Length > 0)
			{
				Throw("BaseUri must be specified either as an argument of XmlReader.Create or on the XmlParserContext. If it is specified on both, it must be the same base URI.");
			}
			text = context.BaseURI;
		}
		reportedBaseUri = text;
		closeInput = true;
		laterInitParam = new LaterInitParam();
		laterInitParam.inputUriStr = uriStr;
		laterInitParam.inputbaseUri = uri;
		laterInitParam.inputContext = context;
		laterInitParam.inputUriResolver = uriResolver;
		laterInitParam.initType = InitInputType.UriString;
		if (!settings.Async)
		{
			FinishInitUriString();
		}
		else
		{
			laterInitParam.useAsync = true;
		}
	}

	private void FinishInitUriString()
	{
		Stream stream = null;
		if (laterInitParam.useAsync)
		{
			Task<object> entityAsync = laterInitParam.inputUriResolver.GetEntityAsync(laterInitParam.inputbaseUri, string.Empty, typeof(Stream));
			entityAsync.Wait();
			stream = (Stream)entityAsync.Result;
		}
		else
		{
			stream = (Stream)laterInitParam.inputUriResolver.GetEntity(laterInitParam.inputbaseUri, string.Empty, typeof(Stream));
		}
		if (stream == null)
		{
			throw new XmlException("Cannot resolve '{0}'.", laterInitParam.inputUriStr);
		}
		Encoding encoding = null;
		if (laterInitParam.inputContext != null)
		{
			encoding = laterInitParam.inputContext.Encoding;
		}
		try
		{
			InitStreamInput(laterInitParam.inputbaseUri, reportedBaseUri, stream, null, 0, encoding);
			reportedEncoding = ps.encoding;
			if (laterInitParam.inputContext != null && laterInitParam.inputContext.HasDtdInfo)
			{
				ProcessDtdFromParserContext(laterInitParam.inputContext);
			}
		}
		catch
		{
			stream.Close();
			throw;
		}
		laterInitParam = null;
	}

	internal XmlTextReaderImpl(Stream stream, byte[] bytes, int byteCount, XmlReaderSettings settings, Uri baseUri, string baseUriStr, XmlParserContext context, bool closeInput)
		: this(settings.GetXmlResolver(), settings, context)
	{
		if (context != null && context.BaseURI != null && context.BaseURI.Length > 0 && !UriEqual(baseUri, baseUriStr, context.BaseURI, settings.GetXmlResolver()))
		{
			if (baseUriStr.Length > 0)
			{
				Throw("BaseUri must be specified either as an argument of XmlReader.Create or on the XmlParserContext. If it is specified on both, it must be the same base URI.");
			}
			baseUriStr = context.BaseURI;
		}
		reportedBaseUri = baseUriStr;
		this.closeInput = closeInput;
		laterInitParam = new LaterInitParam();
		laterInitParam.inputStream = stream;
		laterInitParam.inputBytes = bytes;
		laterInitParam.inputByteCount = byteCount;
		laterInitParam.inputbaseUri = baseUri;
		laterInitParam.inputContext = context;
		laterInitParam.initType = InitInputType.Stream;
		if (!settings.Async)
		{
			FinishInitStream();
		}
		else
		{
			laterInitParam.useAsync = true;
		}
	}

	private void FinishInitStream()
	{
		Encoding encoding = null;
		if (laterInitParam.inputContext != null)
		{
			encoding = laterInitParam.inputContext.Encoding;
		}
		InitStreamInput(laterInitParam.inputbaseUri, reportedBaseUri, laterInitParam.inputStream, laterInitParam.inputBytes, laterInitParam.inputByteCount, encoding);
		reportedEncoding = ps.encoding;
		if (laterInitParam.inputContext != null && laterInitParam.inputContext.HasDtdInfo)
		{
			ProcessDtdFromParserContext(laterInitParam.inputContext);
		}
		laterInitParam = null;
	}

	internal XmlTextReaderImpl(TextReader input, XmlReaderSettings settings, string baseUriStr, XmlParserContext context)
		: this(settings.GetXmlResolver(), settings, context)
	{
		if (context != null && context.BaseURI != null)
		{
			baseUriStr = context.BaseURI;
		}
		reportedBaseUri = baseUriStr;
		closeInput = settings.CloseInput;
		laterInitParam = new LaterInitParam();
		laterInitParam.inputTextReader = input;
		laterInitParam.inputContext = context;
		laterInitParam.initType = InitInputType.TextReader;
		if (!settings.Async)
		{
			FinishInitTextReader();
		}
		else
		{
			laterInitParam.useAsync = true;
		}
	}

	private void FinishInitTextReader()
	{
		InitTextReaderInput(reportedBaseUri, laterInitParam.inputTextReader);
		reportedEncoding = ps.encoding;
		if (laterInitParam.inputContext != null && laterInitParam.inputContext.HasDtdInfo)
		{
			ProcessDtdFromParserContext(laterInitParam.inputContext);
		}
		laterInitParam = null;
	}

	internal XmlTextReaderImpl(string xmlFragment, XmlParserContext context, XmlReaderSettings settings)
		: this(null, settings, context)
	{
		InitStringInput(string.Empty, Encoding.Unicode, xmlFragment);
		reportedBaseUri = ps.baseUriStr;
		reportedEncoding = ps.encoding;
	}

	public override string GetAttribute(string name)
	{
		int num = ((name.IndexOf(':') != -1) ? GetIndexOfAttributeWithPrefix(name) : GetIndexOfAttributeWithoutPrefix(name));
		if (num < 0)
		{
			return null;
		}
		return nodes[num].StringValue;
	}

	public override string GetAttribute(string localName, string namespaceURI)
	{
		namespaceURI = ((namespaceURI == null) ? string.Empty : nameTable.Get(namespaceURI));
		localName = nameTable.Get(localName);
		for (int i = index + 1; i < index + attrCount + 1; i++)
		{
			if (Ref.Equal(nodes[i].localName, localName) && Ref.Equal(nodes[i].ns, namespaceURI))
			{
				return nodes[i].StringValue;
			}
		}
		return null;
	}

	public override string GetAttribute(int i)
	{
		if (i < 0 || i >= attrCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		return nodes[index + i + 1].StringValue;
	}

	public override bool MoveToAttribute(string name)
	{
		int num = ((name.IndexOf(':') != -1) ? GetIndexOfAttributeWithPrefix(name) : GetIndexOfAttributeWithoutPrefix(name));
		if (num >= 0)
		{
			if (InAttributeValueIterator)
			{
				FinishAttributeValueIterator();
			}
			curAttrIndex = num - index - 1;
			curNode = nodes[num];
			return true;
		}
		return false;
	}

	public override bool MoveToAttribute(string localName, string namespaceURI)
	{
		namespaceURI = ((namespaceURI == null) ? string.Empty : nameTable.Get(namespaceURI));
		localName = nameTable.Get(localName);
		for (int i = index + 1; i < index + attrCount + 1; i++)
		{
			if (Ref.Equal(nodes[i].localName, localName) && Ref.Equal(nodes[i].ns, namespaceURI))
			{
				curAttrIndex = i - index - 1;
				curNode = nodes[i];
				if (InAttributeValueIterator)
				{
					FinishAttributeValueIterator();
				}
				return true;
			}
		}
		return false;
	}

	public override void MoveToAttribute(int i)
	{
		if (i < 0 || i >= attrCount)
		{
			throw new ArgumentOutOfRangeException("i");
		}
		if (InAttributeValueIterator)
		{
			FinishAttributeValueIterator();
		}
		curAttrIndex = i;
		curNode = nodes[index + 1 + curAttrIndex];
	}

	public override bool MoveToFirstAttribute()
	{
		if (attrCount == 0)
		{
			return false;
		}
		if (InAttributeValueIterator)
		{
			FinishAttributeValueIterator();
		}
		curAttrIndex = 0;
		curNode = nodes[index + 1];
		return true;
	}

	public override bool MoveToNextAttribute()
	{
		if (curAttrIndex + 1 < attrCount)
		{
			if (InAttributeValueIterator)
			{
				FinishAttributeValueIterator();
			}
			curNode = nodes[index + 1 + ++curAttrIndex];
			return true;
		}
		return false;
	}

	public override bool MoveToElement()
	{
		if (InAttributeValueIterator)
		{
			FinishAttributeValueIterator();
		}
		else if (curNode.type != XmlNodeType.Attribute)
		{
			return false;
		}
		curAttrIndex = -1;
		curNode = nodes[index];
		return true;
	}

	private void FinishInit()
	{
		switch (laterInitParam.initType)
		{
		case InitInputType.UriString:
			FinishInitUriString();
			break;
		case InitInputType.Stream:
			FinishInitStream();
			break;
		case InitInputType.TextReader:
			FinishInitTextReader();
			break;
		}
	}

	public override bool Read()
	{
		if (laterInitParam != null)
		{
			FinishInit();
		}
		while (true)
		{
			switch (parsingFunction)
			{
			case ParsingFunction.ElementContent:
				return ParseElementContent();
			case ParsingFunction.DocumentContent:
				return ParseDocumentContent();
			case ParsingFunction.OpenUrl:
				OpenUrl();
				goto case ParsingFunction.SwitchToInteractiveXmlDecl;
			case ParsingFunction.SwitchToInteractive:
				readState = ReadState.Interactive;
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.SwitchToInteractiveXmlDecl:
				readState = ReadState.Interactive;
				parsingFunction = nextParsingFunction;
				if (ParseXmlDeclaration(isTextDecl: false))
				{
					reportedEncoding = ps.encoding;
					return true;
				}
				reportedEncoding = ps.encoding;
				break;
			case ParsingFunction.ResetAttributesRootLevel:
				ResetAttributes();
				curNode = nodes[index];
				parsingFunction = ((index == 0) ? ParsingFunction.DocumentContent : ParsingFunction.ElementContent);
				break;
			case ParsingFunction.MoveToElementContent:
				ResetAttributes();
				index++;
				curNode = AddNode(index, index);
				parsingFunction = ParsingFunction.ElementContent;
				break;
			case ParsingFunction.PopElementContext:
				PopElementContext();
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.PopEmptyElementContext:
				curNode = nodes[index];
				curNode.IsEmptyElement = false;
				ResetAttributes();
				PopElementContext();
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.EntityReference:
				parsingFunction = nextParsingFunction;
				ParseEntityReference();
				return true;
			case ParsingFunction.ReportEndEntity:
				SetupEndEntityNodeInContent();
				parsingFunction = nextParsingFunction;
				return true;
			case ParsingFunction.AfterResolveEntityInContent:
				curNode = AddNode(index, index);
				reportedEncoding = ps.encoding;
				reportedBaseUri = ps.baseUriStr;
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.AfterResolveEmptyEntityInContent:
				curNode = AddNode(index, index);
				curNode.SetValueNode(XmlNodeType.Text, string.Empty);
				curNode.SetLineInfo(ps.lineNo, ps.LinePos);
				reportedEncoding = ps.encoding;
				reportedBaseUri = ps.baseUriStr;
				parsingFunction = nextParsingFunction;
				return true;
			case ParsingFunction.InReadAttributeValue:
				FinishAttributeValueIterator();
				curNode = nodes[index];
				break;
			case ParsingFunction.InIncrementalRead:
				FinishIncrementalRead();
				return true;
			case ParsingFunction.FragmentAttribute:
				return ParseFragmentAttribute();
			case ParsingFunction.XmlDeclarationFragment:
				ParseXmlDeclarationFragment();
				parsingFunction = ParsingFunction.GoToEof;
				return true;
			case ParsingFunction.GoToEof:
				OnEof();
				return false;
			case ParsingFunction.Error:
			case ParsingFunction.Eof:
			case ParsingFunction.ReaderClosed:
				return false;
			case ParsingFunction.NoData:
				ThrowWithoutLineInfo("Root element is missing.");
				return false;
			case ParsingFunction.PartialTextValue:
				SkipPartialTextValue();
				break;
			case ParsingFunction.InReadValueChunk:
				FinishReadValueChunk();
				break;
			case ParsingFunction.InReadContentAsBinary:
				FinishReadContentAsBinary();
				break;
			case ParsingFunction.InReadElementContentAsBinary:
				FinishReadElementContentAsBinary();
				break;
			}
		}
	}

	public override void Close()
	{
		Close(closeInput);
	}

	public override void Skip()
	{
		if (readState != ReadState.Interactive)
		{
			return;
		}
		if (InAttributeValueIterator)
		{
			FinishAttributeValueIterator();
			curNode = nodes[index];
		}
		else
		{
			switch (parsingFunction)
			{
			case ParsingFunction.InIncrementalRead:
				FinishIncrementalRead();
				break;
			case ParsingFunction.PartialTextValue:
				SkipPartialTextValue();
				break;
			case ParsingFunction.InReadValueChunk:
				FinishReadValueChunk();
				break;
			case ParsingFunction.InReadContentAsBinary:
				FinishReadContentAsBinary();
				break;
			case ParsingFunction.InReadElementContentAsBinary:
				FinishReadElementContentAsBinary();
				break;
			}
		}
		XmlNodeType type = curNode.type;
		if (type != XmlNodeType.Element)
		{
			if (type != XmlNodeType.Attribute)
			{
				goto IL_00dc;
			}
			outerReader.MoveToElement();
		}
		if (!curNode.IsEmptyElement)
		{
			int num = index;
			parsingMode = ParsingMode.SkipContent;
			while (outerReader.Read() && index > num)
			{
			}
			parsingMode = ParsingMode.Full;
		}
		goto IL_00dc;
		IL_00dc:
		outerReader.Read();
	}

	public override string LookupNamespace(string prefix)
	{
		if (!supportNamespaces)
		{
			return null;
		}
		return namespaceManager.LookupNamespace(prefix);
	}

	public override bool ReadAttributeValue()
	{
		if (parsingFunction != ParsingFunction.InReadAttributeValue)
		{
			if (curNode.type != XmlNodeType.Attribute)
			{
				return false;
			}
			if (readState != ReadState.Interactive || curAttrIndex < 0)
			{
				return false;
			}
			if (parsingFunction == ParsingFunction.InReadValueChunk)
			{
				FinishReadValueChunk();
			}
			if (parsingFunction == ParsingFunction.InReadContentAsBinary)
			{
				FinishReadContentAsBinary();
			}
			if (curNode.nextAttrValueChunk == null || entityHandling == EntityHandling.ExpandEntities)
			{
				NodeData nodeData = AddNode(index + attrCount + 1, curNode.depth + 1);
				nodeData.SetValueNode(XmlNodeType.Text, curNode.StringValue);
				nodeData.lineInfo = curNode.lineInfo2;
				nodeData.depth = curNode.depth + 1;
				curNode = nodeData;
				nodeData.nextAttrValueChunk = null;
			}
			else
			{
				curNode = curNode.nextAttrValueChunk;
				AddNode(index + attrCount + 1, index + 2);
				nodes[index + attrCount + 1] = curNode;
				fullAttrCleanup = true;
			}
			nextParsingFunction = parsingFunction;
			parsingFunction = ParsingFunction.InReadAttributeValue;
			attributeValueBaseEntityId = ps.entityId;
			return true;
		}
		if (ps.entityId == attributeValueBaseEntityId)
		{
			if (curNode.nextAttrValueChunk != null)
			{
				curNode = curNode.nextAttrValueChunk;
				nodes[index + attrCount + 1] = curNode;
				return true;
			}
			return false;
		}
		return ParseAttributeValueChunk();
	}

	public override void ResolveEntity()
	{
		if (curNode.type != XmlNodeType.EntityReference)
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
		if (parsingFunction == ParsingFunction.InReadAttributeValue || parsingFunction == ParsingFunction.FragmentAttribute)
		{
			switch (HandleGeneralEntityReference(curNode.localName, isInAttributeValue: true, pushFakeEntityIfNullResolver: true, curNode.LinePos))
			{
			case EntityType.Expanded:
			case EntityType.ExpandedInAttribute:
				if (ps.charsUsed - ps.charPos == 0)
				{
					emptyEntityInAttributeResolved = true;
				}
				break;
			case EntityType.FakeExpanded:
				emptyEntityInAttributeResolved = true;
				break;
			default:
				throw new XmlException("An internal error has occurred.", string.Empty);
			}
		}
		else
		{
			switch (HandleGeneralEntityReference(curNode.localName, isInAttributeValue: false, pushFakeEntityIfNullResolver: true, curNode.LinePos))
			{
			case EntityType.Expanded:
			case EntityType.ExpandedInAttribute:
				nextParsingFunction = parsingFunction;
				if (ps.charsUsed - ps.charPos == 0 && !ps.entity.IsExternal)
				{
					parsingFunction = ParsingFunction.AfterResolveEmptyEntityInContent;
				}
				else
				{
					parsingFunction = ParsingFunction.AfterResolveEntityInContent;
				}
				break;
			case EntityType.FakeExpanded:
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.AfterResolveEmptyEntityInContent;
				break;
			default:
				throw new XmlException("An internal error has occurred.", string.Empty);
			}
		}
		ps.entityResolvedManually = true;
		index++;
	}

	internal void MoveOffEntityReference()
	{
		if (outerReader.NodeType == XmlNodeType.EntityReference && parsingFunction == ParsingFunction.AfterResolveEntityInContent && !outerReader.Read())
		{
			throw new InvalidOperationException(Res.GetString("Operation is not valid due to the current state of the object."));
		}
	}

	public override string ReadString()
	{
		MoveOffEntityReference();
		return base.ReadString();
	}

	public override int ReadContentAsBase64(byte[] buffer, int index, int count)
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
		if (parsingFunction == ParsingFunction.InReadContentAsBinary)
		{
			if (incReadDecoder == base64Decoder)
			{
				return ReadContentAsBinary(buffer, index, count);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (!XmlReader.CanReadContentAs(curNode.type))
			{
				throw CreateReadContentAsException("ReadContentAsBase64");
			}
			if (!InitReadContentAsBinary())
			{
				return 0;
			}
		}
		InitBase64Decoder();
		return ReadContentAsBinary(buffer, index, count);
	}

	public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
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
		if (parsingFunction == ParsingFunction.InReadContentAsBinary)
		{
			if (incReadDecoder == binHexDecoder)
			{
				return ReadContentAsBinary(buffer, index, count);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (!XmlReader.CanReadContentAs(curNode.type))
			{
				throw CreateReadContentAsException("ReadContentAsBinHex");
			}
			if (!InitReadContentAsBinary())
			{
				return 0;
			}
		}
		InitBinHexDecoder();
		return ReadContentAsBinary(buffer, index, count);
	}

	public override int ReadElementContentAsBase64(byte[] buffer, int index, int count)
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
		if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
		{
			if (incReadDecoder == base64Decoder)
			{
				return ReadElementContentAsBinary(buffer, index, count);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.InReadContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (curNode.type != XmlNodeType.Element)
			{
				throw CreateReadElementContentAsException("ReadElementContentAsBinHex");
			}
			if (!InitReadElementContentAsBinary())
			{
				return 0;
			}
		}
		InitBase64Decoder();
		return ReadElementContentAsBinary(buffer, index, count);
	}

	public override int ReadElementContentAsBinHex(byte[] buffer, int index, int count)
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
		if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
		{
			if (incReadDecoder == binHexDecoder)
			{
				return ReadElementContentAsBinary(buffer, index, count);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.InReadContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (curNode.type != XmlNodeType.Element)
			{
				throw CreateReadElementContentAsException("ReadElementContentAsBinHex");
			}
			if (!InitReadElementContentAsBinary())
			{
				return 0;
			}
		}
		InitBinHexDecoder();
		return ReadElementContentAsBinary(buffer, index, count);
	}

	public override int ReadValueChunk(char[] buffer, int index, int count)
	{
		if (!XmlReader.HasValueInternal(curNode.type))
		{
			throw new InvalidOperationException(Res.GetString("The ReadValueAsChunk method is not supported on node type {0}.", curNode.type));
		}
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
		if (parsingFunction != ParsingFunction.InReadValueChunk)
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.PartialTextValue)
			{
				incReadState = IncrementalReadState.ReadValueChunk_OnPartialValue;
			}
			else
			{
				incReadState = IncrementalReadState.ReadValueChunk_OnCachedValue;
				nextNextParsingFunction = nextParsingFunction;
				nextParsingFunction = parsingFunction;
			}
			parsingFunction = ParsingFunction.InReadValueChunk;
			readValueOffset = 0;
		}
		if (count == 0)
		{
			return 0;
		}
		int num = 0;
		int num2 = curNode.CopyTo(readValueOffset, buffer, index + num, count - num);
		num += num2;
		readValueOffset += num2;
		if (num == count)
		{
			if (XmlCharType.IsHighSurrogate(buffer[index + count - 1]))
			{
				num--;
				readValueOffset--;
				if (num == 0)
				{
					Throw("The buffer is not large enough to fit a surrogate pair. Please provide a buffer of size at least 2 characters.");
				}
			}
			return num;
		}
		if (incReadState == IncrementalReadState.ReadValueChunk_OnPartialValue)
		{
			curNode.SetValue(string.Empty);
			bool flag = false;
			int startPos = 0;
			int endPos = 0;
			while (num < count && !flag)
			{
				int outOrChars = 0;
				flag = ParseText(out startPos, out endPos, ref outOrChars);
				int num3 = count - num;
				if (num3 > endPos - startPos)
				{
					num3 = endPos - startPos;
				}
				BlockCopyChars(ps.chars, startPos, buffer, index + num, num3);
				num += num3;
				startPos += num3;
			}
			incReadState = (flag ? IncrementalReadState.ReadValueChunk_OnCachedValue : IncrementalReadState.ReadValueChunk_OnPartialValue);
			if (num == count && XmlCharType.IsHighSurrogate(buffer[index + count - 1]))
			{
				num--;
				startPos--;
				if (num == 0)
				{
					Throw("The buffer is not large enough to fit a surrogate pair. Please provide a buffer of size at least 2 characters.");
				}
			}
			readValueOffset = 0;
			curNode.SetValue(ps.chars, startPos, endPos - startPos);
		}
		return num;
	}

	public bool HasLineInfo()
	{
		return true;
	}

	IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return GetNamespacesInScope(scope);
	}

	string IXmlNamespaceResolver.LookupNamespace(string prefix)
	{
		return LookupNamespace(prefix);
	}

	string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
	{
		return LookupPrefix(namespaceName);
	}

	internal IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
	{
		return namespaceManager.GetNamespacesInScope(scope);
	}

	internal string LookupPrefix(string namespaceName)
	{
		return namespaceManager.LookupPrefix(namespaceName);
	}

	internal void ResetState()
	{
		if (fragment)
		{
			Throw(new InvalidOperationException(Res.GetString("Cannot call ResetState when parsing an XML fragment.")));
		}
		if (readState != ReadState.Initial)
		{
			ResetAttributes();
			while (namespaceManager.PopScope())
			{
			}
			while (InEntity)
			{
				HandleEntityEnd(checkEntityNesting: true);
			}
			readState = ReadState.Initial;
			parsingFunction = ParsingFunction.SwitchToInteractiveXmlDecl;
			nextParsingFunction = ParsingFunction.DocumentContent;
			curNode = nodes[0];
			curNode.Clear(XmlNodeType.None);
			curNode.SetLineInfo(0, 0);
			index = 0;
			rootElementParsed = false;
			charactersInDocument = 0L;
			charactersFromEntities = 0L;
			afterResetState = true;
		}
	}

	internal TextReader GetRemainder()
	{
		switch (parsingFunction)
		{
		case ParsingFunction.Eof:
		case ParsingFunction.ReaderClosed:
			return new StringReader(string.Empty);
		case ParsingFunction.OpenUrl:
			OpenUrl();
			break;
		case ParsingFunction.InIncrementalRead:
			if (!InEntity)
			{
				stringBuilder.Append(ps.chars, incReadLeftStartPos, incReadLeftEndPos - incReadLeftStartPos);
			}
			break;
		}
		while (InEntity)
		{
			HandleEntityEnd(checkEntityNesting: true);
		}
		ps.appendMode = false;
		do
		{
			stringBuilder.Append(ps.chars, ps.charPos, ps.charsUsed - ps.charPos);
			ps.charPos = ps.charsUsed;
		}
		while (ReadData() != 0);
		OnEof();
		string s = stringBuilder.ToString();
		stringBuilder.Length = 0;
		return new StringReader(s);
	}

	internal int ReadChars(char[] buffer, int index, int count)
	{
		if (parsingFunction == ParsingFunction.InIncrementalRead)
		{
			if (incReadDecoder != readCharsDecoder)
			{
				if (readCharsDecoder == null)
				{
					readCharsDecoder = new IncrementalReadCharsDecoder();
				}
				readCharsDecoder.Reset();
				incReadDecoder = readCharsDecoder;
			}
			return IncrementalRead(buffer, index, count);
		}
		if (curNode.type != XmlNodeType.Element)
		{
			return 0;
		}
		if (curNode.IsEmptyElement)
		{
			outerReader.Read();
			return 0;
		}
		if (readCharsDecoder == null)
		{
			readCharsDecoder = new IncrementalReadCharsDecoder();
		}
		InitIncrementalRead(readCharsDecoder);
		return IncrementalRead(buffer, index, count);
	}

	internal int ReadBase64(byte[] array, int offset, int len)
	{
		if (parsingFunction == ParsingFunction.InIncrementalRead)
		{
			if (incReadDecoder != base64Decoder)
			{
				InitBase64Decoder();
			}
			return IncrementalRead(array, offset, len);
		}
		if (curNode.type != XmlNodeType.Element)
		{
			return 0;
		}
		if (curNode.IsEmptyElement)
		{
			outerReader.Read();
			return 0;
		}
		if (base64Decoder == null)
		{
			base64Decoder = new Base64Decoder();
		}
		InitIncrementalRead(base64Decoder);
		return IncrementalRead(array, offset, len);
	}

	internal int ReadBinHex(byte[] array, int offset, int len)
	{
		if (parsingFunction == ParsingFunction.InIncrementalRead)
		{
			if (incReadDecoder != binHexDecoder)
			{
				InitBinHexDecoder();
			}
			return IncrementalRead(array, offset, len);
		}
		if (curNode.type != XmlNodeType.Element)
		{
			return 0;
		}
		if (curNode.IsEmptyElement)
		{
			outerReader.Read();
			return 0;
		}
		if (binHexDecoder == null)
		{
			binHexDecoder = new BinHexDecoder();
		}
		InitIncrementalRead(binHexDecoder);
		return IncrementalRead(array, offset, len);
	}

	internal void DtdParserProxy_OnNewLine(int pos)
	{
		OnNewLine(pos);
	}

	internal int DtdParserProxy_ReadData()
	{
		return ReadData();
	}

	internal int DtdParserProxy_ParseNumericCharRef(StringBuilder internalSubsetBuilder)
	{
		EntityType entityType;
		return ParseNumericCharRef(expand: true, internalSubsetBuilder, out entityType);
	}

	internal int DtdParserProxy_ParseNamedCharRef(bool expand, StringBuilder internalSubsetBuilder)
	{
		return ParseNamedCharRef(expand, internalSubsetBuilder);
	}

	internal void DtdParserProxy_ParsePI(StringBuilder sb)
	{
		if (sb == null)
		{
			ParsingMode parsingMode = this.parsingMode;
			this.parsingMode = ParsingMode.SkipNode;
			ParsePI(null);
			this.parsingMode = parsingMode;
		}
		else
		{
			ParsePI(sb);
		}
	}

	internal void DtdParserProxy_ParseComment(StringBuilder sb)
	{
		try
		{
			if (sb == null)
			{
				ParsingMode parsingMode = this.parsingMode;
				this.parsingMode = ParsingMode.SkipNode;
				ParseCDataOrComment(XmlNodeType.Comment);
				this.parsingMode = parsingMode;
			}
			else
			{
				NodeData nodeData = curNode;
				curNode = AddNode(index + attrCount + 1, index);
				ParseCDataOrComment(XmlNodeType.Comment);
				curNode.CopyTo(0, sb);
				curNode = nodeData;
			}
		}
		catch (XmlException ex)
		{
			if (ex.ResString == "Unexpected end of file while parsing {0} has occurred." && ps.entity != null)
			{
				SendValidationEvent(XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", null, ps.LineNo, ps.LinePos);
				return;
			}
			throw;
		}
	}

	private XmlResolver GetTempResolver()
	{
		if (xmlResolver != null)
		{
			return xmlResolver;
		}
		return new XmlUrlResolver();
	}

	internal bool DtdParserProxy_PushEntity(IDtdEntityInfo entity, out int entityId)
	{
		bool result;
		if (entity.IsExternal)
		{
			if (IsResolverNull)
			{
				entityId = -1;
				return false;
			}
			result = PushExternalEntity(entity);
		}
		else
		{
			PushInternalEntity(entity);
			result = true;
		}
		entityId = ps.entityId;
		return result;
	}

	internal bool DtdParserProxy_PopEntity(out IDtdEntityInfo oldEntity, out int newEntityId)
	{
		if (parsingStatesStackTop == -1)
		{
			oldEntity = null;
			newEntityId = -1;
			return false;
		}
		oldEntity = ps.entity;
		PopEntity();
		newEntityId = ps.entityId;
		return true;
	}

	internal bool DtdParserProxy_PushExternalSubset(string systemId, string publicId)
	{
		if (IsResolverNull)
		{
			return false;
		}
		if (ps.baseUri == null && !string.IsNullOrEmpty(ps.baseUriStr))
		{
			ps.baseUri = xmlResolver.ResolveUri(null, ps.baseUriStr);
		}
		PushExternalEntityOrSubset(publicId, systemId, ps.baseUri, null);
		ps.entity = null;
		ps.entityId = 0;
		int charPos = ps.charPos;
		if (v1Compat)
		{
			EatWhitespaces(null);
		}
		if (!ParseXmlDeclaration(isTextDecl: true))
		{
			ps.charPos = charPos;
		}
		return true;
	}

	internal void DtdParserProxy_PushInternalDtd(string baseUri, string internalDtd)
	{
		PushParsingState();
		RegisterConsumedCharacters(internalDtd.Length, inEntityReference: false);
		InitStringInput(baseUri, Encoding.Unicode, internalDtd);
		ps.entity = null;
		ps.entityId = 0;
		ps.eolNormalized = false;
	}

	internal void DtdParserProxy_Throw(Exception e)
	{
		Throw(e);
	}

	internal void DtdParserProxy_OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo)
	{
		NodeData nodeData = AddAttributeNoChecks("SYSTEM", index + 1);
		nodeData.SetValue(systemId);
		nodeData.lineInfo = keywordLineInfo;
		nodeData.lineInfo2 = systemLiteralLineInfo;
	}

	internal void DtdParserProxy_OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo)
	{
		NodeData nodeData = AddAttributeNoChecks("PUBLIC", index + 1);
		nodeData.SetValue(publicId);
		nodeData.lineInfo = keywordLineInfo;
		nodeData.lineInfo2 = publicLiteralLineInfo;
	}

	private void Throw(int pos, string res, string arg)
	{
		ps.charPos = pos;
		Throw(res, arg);
	}

	private void Throw(int pos, string res, string[] args)
	{
		ps.charPos = pos;
		Throw(res, args);
	}

	private void Throw(int pos, string res)
	{
		ps.charPos = pos;
		Throw(res, string.Empty);
	}

	private void Throw(string res)
	{
		Throw(res, string.Empty);
	}

	private void Throw(string res, int lineNo, int linePos)
	{
		Throw(new XmlException(res, string.Empty, lineNo, linePos, ps.baseUriStr));
	}

	private void Throw(string res, string arg)
	{
		Throw(new XmlException(res, arg, ps.LineNo, ps.LinePos, ps.baseUriStr));
	}

	private void Throw(string res, string arg, int lineNo, int linePos)
	{
		Throw(new XmlException(res, arg, lineNo, linePos, ps.baseUriStr));
	}

	private void Throw(string res, string[] args)
	{
		Throw(new XmlException(res, args, ps.LineNo, ps.LinePos, ps.baseUriStr));
	}

	private void Throw(string res, string arg, Exception innerException)
	{
		Throw(res, new string[1] { arg }, innerException);
	}

	private void Throw(string res, string[] args, Exception innerException)
	{
		Throw(new XmlException(res, args, innerException, ps.LineNo, ps.LinePos, ps.baseUriStr));
	}

	private void Throw(Exception e)
	{
		SetErrorState();
		if (e is XmlException ex)
		{
			curNode.SetLineInfo(ex.LineNumber, ex.LinePosition);
		}
		throw e;
	}

	private void ReThrow(Exception e, int lineNo, int linePos)
	{
		Throw(new XmlException(e.Message, (Exception)null, lineNo, linePos, ps.baseUriStr));
	}

	private void ThrowWithoutLineInfo(string res)
	{
		Throw(new XmlException(res, string.Empty, ps.baseUriStr));
	}

	private void ThrowWithoutLineInfo(string res, string arg)
	{
		Throw(new XmlException(res, arg, ps.baseUriStr));
	}

	private void ThrowWithoutLineInfo(string res, string[] args, Exception innerException)
	{
		Throw(new XmlException(res, args, innerException, 0, 0, ps.baseUriStr));
	}

	private void ThrowInvalidChar(char[] data, int length, int invCharPos)
	{
		Throw(invCharPos, "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, length, invCharPos));
	}

	private void SetErrorState()
	{
		parsingFunction = ParsingFunction.Error;
		readState = ReadState.Error;
	}

	private void SendValidationEvent(XmlSeverityType severity, string code, string arg, int lineNo, int linePos)
	{
		SendValidationEvent(severity, new XmlSchemaException(code, arg, ps.baseUriStr, lineNo, linePos));
	}

	private void SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception)
	{
		if (validationEventHandling != null)
		{
			validationEventHandling.SendEvent(exception, severity);
		}
	}

	private void FinishAttributeValueIterator()
	{
		if (parsingFunction == ParsingFunction.InReadValueChunk)
		{
			FinishReadValueChunk();
		}
		else if (parsingFunction == ParsingFunction.InReadContentAsBinary)
		{
			FinishReadContentAsBinary();
		}
		if (parsingFunction == ParsingFunction.InReadAttributeValue)
		{
			while (ps.entityId != attributeValueBaseEntityId)
			{
				HandleEntityEnd(checkEntityNesting: false);
			}
			emptyEntityInAttributeResolved = false;
			parsingFunction = nextParsingFunction;
			nextParsingFunction = ((index <= 0) ? ParsingFunction.DocumentContent : ParsingFunction.ElementContent);
		}
	}

	private void InitStreamInput(Stream stream, Encoding encoding)
	{
		InitStreamInput(null, string.Empty, stream, null, 0, encoding);
	}

	private void InitStreamInput(string baseUriStr, Stream stream, Encoding encoding)
	{
		InitStreamInput(null, baseUriStr, stream, null, 0, encoding);
	}

	private void InitStreamInput(Uri baseUri, Stream stream, Encoding encoding)
	{
		InitStreamInput(baseUri, baseUri.ToString(), stream, null, 0, encoding);
	}

	private void InitStreamInput(Uri baseUri, string baseUriStr, Stream stream, Encoding encoding)
	{
		InitStreamInput(baseUri, baseUriStr, stream, null, 0, encoding);
	}

	private void InitStreamInput(Uri baseUri, string baseUriStr, Stream stream, byte[] bytes, int byteCount, Encoding encoding)
	{
		ps.stream = stream;
		ps.baseUri = baseUri;
		ps.baseUriStr = baseUriStr;
		int num;
		if (bytes != null)
		{
			ps.bytes = bytes;
			ps.bytesUsed = byteCount;
			num = ps.bytes.Length;
		}
		else
		{
			num = ((laterInitParam == null || !laterInitParam.useAsync) ? XmlReader.CalcBufferSize(stream) : 65536);
			if (ps.bytes == null || ps.bytes.Length < num)
			{
				ps.bytes = new byte[num];
			}
		}
		if (ps.chars == null || ps.chars.Length < num + 1)
		{
			ps.chars = new char[num + 1];
		}
		ps.bytePos = 0;
		while (ps.bytesUsed < 4 && ps.bytes.Length - ps.bytesUsed > 0)
		{
			int num2 = stream.Read(ps.bytes, ps.bytesUsed, ps.bytes.Length - ps.bytesUsed);
			if (num2 == 0)
			{
				ps.isStreamEof = true;
				break;
			}
			ps.bytesUsed += num2;
		}
		if (encoding == null)
		{
			encoding = DetectEncoding();
		}
		SetupEncoding(encoding);
		byte[] preamble = ps.encoding.GetPreamble();
		int num3 = preamble.Length;
		int i;
		for (i = 0; i < num3 && i < ps.bytesUsed && ps.bytes[i] == preamble[i]; i++)
		{
		}
		if (i == num3)
		{
			ps.bytePos = num3;
		}
		documentStartBytePos = ps.bytePos;
		ps.eolNormalized = !normalize;
		ps.appendMode = true;
		ReadData();
	}

	private void InitTextReaderInput(string baseUriStr, TextReader input)
	{
		InitTextReaderInput(baseUriStr, null, input);
	}

	private void InitTextReaderInput(string baseUriStr, Uri baseUri, TextReader input)
	{
		ps.textReader = input;
		ps.baseUriStr = baseUriStr;
		ps.baseUri = baseUri;
		if (ps.chars == null)
		{
			if (laterInitParam != null && laterInitParam.useAsync)
			{
				ps.chars = new char[65537];
			}
			else
			{
				ps.chars = new char[4097];
			}
		}
		ps.encoding = Encoding.Unicode;
		ps.eolNormalized = !normalize;
		ps.appendMode = true;
		ReadData();
	}

	private void InitStringInput(string baseUriStr, Encoding originalEncoding, string str)
	{
		ps.baseUriStr = baseUriStr;
		ps.baseUri = null;
		int length = str.Length;
		ps.chars = new char[length + 1];
		str.CopyTo(0, ps.chars, 0, str.Length);
		ps.charsUsed = length;
		ps.chars[length] = '\0';
		ps.encoding = originalEncoding;
		ps.eolNormalized = !normalize;
		ps.isEof = true;
	}

	private void InitFragmentReader(XmlNodeType fragmentType, XmlParserContext parserContext, bool allowXmlDeclFragment)
	{
		fragmentParserContext = parserContext;
		if (parserContext != null)
		{
			if (parserContext.NamespaceManager != null)
			{
				namespaceManager = parserContext.NamespaceManager;
				xmlContext.defaultNamespace = namespaceManager.LookupNamespace(string.Empty);
			}
			else
			{
				namespaceManager = new XmlNamespaceManager(nameTable);
			}
			ps.baseUriStr = parserContext.BaseURI;
			ps.baseUri = null;
			xmlContext.xmlLang = parserContext.XmlLang;
			xmlContext.xmlSpace = parserContext.XmlSpace;
		}
		else
		{
			namespaceManager = new XmlNamespaceManager(nameTable);
			ps.baseUriStr = string.Empty;
			ps.baseUri = null;
		}
		reportedBaseUri = ps.baseUriStr;
		if (fragmentType <= XmlNodeType.Attribute)
		{
			if (fragmentType != XmlNodeType.Element)
			{
				if (fragmentType != XmlNodeType.Attribute)
				{
					goto IL_012e;
				}
				ps.appendMode = false;
				parsingFunction = ParsingFunction.SwitchToInteractive;
				nextParsingFunction = ParsingFunction.FragmentAttribute;
			}
			else
			{
				nextParsingFunction = ParsingFunction.DocumentContent;
			}
		}
		else if (fragmentType != XmlNodeType.Document)
		{
			if (fragmentType != XmlNodeType.XmlDeclaration || !allowXmlDeclFragment)
			{
				goto IL_012e;
			}
			ps.appendMode = false;
			parsingFunction = ParsingFunction.SwitchToInteractive;
			nextParsingFunction = ParsingFunction.XmlDeclarationFragment;
		}
		this.fragmentType = fragmentType;
		fragment = true;
		return;
		IL_012e:
		Throw("XmlNodeType {0} is not supported for partial content parsing.", fragmentType.ToString());
	}

	private void ProcessDtdFromParserContext(XmlParserContext context)
	{
		switch (dtdProcessing)
		{
		case DtdProcessing.Prohibit:
			ThrowWithoutLineInfo("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.");
			break;
		case DtdProcessing.Parse:
			ParseDtdFromParserContext();
			break;
		case DtdProcessing.Ignore:
			break;
		}
	}

	private void OpenUrl()
	{
		XmlResolver tempResolver = GetTempResolver();
		if (!(ps.baseUri != null))
		{
			ps.baseUri = tempResolver.ResolveUri(null, url);
			ps.baseUriStr = ps.baseUri.ToString();
		}
		try
		{
			OpenUrlDelegate(tempResolver);
		}
		catch
		{
			SetErrorState();
			throw;
		}
		if (ps.stream == null)
		{
			ThrowWithoutLineInfo("Cannot resolve '{0}'.", ps.baseUriStr);
		}
		InitStreamInput(ps.baseUri, ps.baseUriStr, ps.stream, null);
		reportedEncoding = ps.encoding;
	}

	private void OpenUrlDelegate(object xmlResolver)
	{
		ps.stream = (Stream)GetTempResolver().GetEntity(ps.baseUri, null, typeof(Stream));
	}

	private Encoding DetectEncoding()
	{
		if (ps.bytesUsed < 2)
		{
			return null;
		}
		int num = (ps.bytes[0] << 8) | ps.bytes[1];
		int num2 = ((ps.bytesUsed >= 4) ? ((ps.bytes[2] << 8) | ps.bytes[3]) : 0);
		switch (num)
		{
		case 0:
			switch (num2)
			{
			case 65279:
				return Ucs4Encoding.UCS4_Bigendian;
			case 60:
				return Ucs4Encoding.UCS4_Bigendian;
			case 65534:
				return Ucs4Encoding.UCS4_2143;
			case 15360:
				return Ucs4Encoding.UCS4_2143;
			}
			break;
		case 65279:
			if (num2 == 0)
			{
				return Ucs4Encoding.UCS4_3412;
			}
			return Encoding.BigEndianUnicode;
		case 65534:
			if (num2 == 0)
			{
				return Ucs4Encoding.UCS4_Littleendian;
			}
			return Encoding.Unicode;
		case 15360:
			if (num2 == 0)
			{
				return Ucs4Encoding.UCS4_Littleendian;
			}
			return Encoding.Unicode;
		case 60:
			if (num2 == 0)
			{
				return Ucs4Encoding.UCS4_3412;
			}
			return Encoding.BigEndianUnicode;
		case 19567:
			if (num2 == 42900)
			{
				Throw("System does not support '{0}' encoding.", "ebcdic");
			}
			break;
		case 61371:
			if ((num2 & 0xFF00) == 48896)
			{
				return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);
			}
			break;
		}
		return null;
	}

	private void SetupEncoding(Encoding encoding)
	{
		if (encoding == null)
		{
			ps.encoding = Encoding.UTF8;
			ps.decoder = new SafeAsciiDecoder();
			return;
		}
		ps.encoding = encoding;
		string webName = ps.encoding.WebName;
		if (!(webName == "utf-16"))
		{
			if (webName == "utf-16BE")
			{
				ps.decoder = new UTF16Decoder(bigEndian: true);
			}
			else
			{
				ps.decoder = encoding.GetDecoder();
			}
		}
		else
		{
			ps.decoder = new UTF16Decoder(bigEndian: false);
		}
	}

	private void SwitchEncoding(Encoding newEncoding)
	{
		if ((newEncoding.WebName != ps.encoding.WebName || ps.decoder is SafeAsciiDecoder) && !afterResetState)
		{
			UnDecodeChars();
			ps.appendMode = false;
			SetupEncoding(newEncoding);
			ReadData();
		}
	}

	private Encoding CheckEncoding(string newEncodingName)
	{
		if (ps.stream == null)
		{
			return ps.encoding;
		}
		if (string.Compare(newEncodingName, "ucs-2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "utf-16", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "iso-10646-ucs-2", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(newEncodingName, "ucs-4", StringComparison.OrdinalIgnoreCase) == 0)
		{
			if (ps.encoding.WebName != "utf-16BE" && ps.encoding.WebName != "utf-16" && string.Compare(newEncodingName, "ucs-4", StringComparison.OrdinalIgnoreCase) != 0)
			{
				if (afterResetState)
				{
					Throw("'{0}' is an invalid value for the 'encoding' attribute. The encoding cannot be switched after a call to ResetState.", newEncodingName);
				}
				else
				{
					ThrowWithoutLineInfo("There is no Unicode byte order mark. Cannot switch to Unicode.");
				}
			}
			return ps.encoding;
		}
		Encoding encoding = null;
		if (string.Compare(newEncodingName, "utf-8", StringComparison.OrdinalIgnoreCase) == 0)
		{
			encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);
		}
		else
		{
			try
			{
				encoding = Encoding.GetEncoding(newEncodingName);
			}
			catch (NotSupportedException innerException)
			{
				Throw("System does not support '{0}' encoding.", newEncodingName, innerException);
			}
			catch (ArgumentException innerException2)
			{
				Throw("System does not support '{0}' encoding.", newEncodingName, innerException2);
			}
		}
		if (afterResetState && ps.encoding.WebName != encoding.WebName)
		{
			Throw("'{0}' is an invalid value for the 'encoding' attribute. The encoding cannot be switched after a call to ResetState.", newEncodingName);
		}
		return encoding;
	}

	private void UnDecodeChars()
	{
		if (maxCharactersInDocument > 0)
		{
			charactersInDocument -= ps.charsUsed - ps.charPos;
		}
		if (maxCharactersFromEntities > 0 && InEntity)
		{
			charactersFromEntities -= ps.charsUsed - ps.charPos;
		}
		ps.bytePos = documentStartBytePos;
		if (ps.charPos > 0)
		{
			ps.bytePos += ps.encoding.GetByteCount(ps.chars, 0, ps.charPos);
		}
		ps.charsUsed = ps.charPos;
		ps.isEof = false;
	}

	private void SwitchEncodingToUTF8()
	{
		SwitchEncoding(new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true));
	}

	private int ReadData()
	{
		if (ps.isEof)
		{
			return 0;
		}
		int num;
		if (ps.appendMode)
		{
			if (ps.charsUsed == ps.chars.Length - 1)
			{
				for (int i = 0; i < attrCount; i++)
				{
					nodes[index + i + 1].OnBufferInvalidated();
				}
				char[] array = new char[ps.chars.Length * 2];
				BlockCopyChars(ps.chars, 0, array, 0, ps.chars.Length);
				ps.chars = array;
			}
			if (ps.stream != null && ps.bytesUsed - ps.bytePos < 6 && ps.bytes.Length - ps.bytesUsed < 6)
			{
				byte[] array2 = new byte[ps.bytes.Length * 2];
				BlockCopy(ps.bytes, 0, array2, 0, ps.bytesUsed);
				ps.bytes = array2;
			}
			num = ps.chars.Length - ps.charsUsed - 1;
			if (num > 80)
			{
				num = 80;
			}
		}
		else
		{
			int num2 = ps.chars.Length;
			if (num2 - ps.charsUsed <= num2 / 2)
			{
				for (int j = 0; j < attrCount; j++)
				{
					nodes[index + j + 1].OnBufferInvalidated();
				}
				int num3 = ps.charsUsed - ps.charPos;
				if (num3 < num2 - 1)
				{
					ps.lineStartPos -= ps.charPos;
					if (num3 > 0)
					{
						BlockCopyChars(ps.chars, ps.charPos, ps.chars, 0, num3);
					}
					ps.charPos = 0;
					ps.charsUsed = num3;
				}
				else
				{
					char[] array3 = new char[ps.chars.Length * 2];
					BlockCopyChars(ps.chars, 0, array3, 0, ps.chars.Length);
					ps.chars = array3;
				}
			}
			if (ps.stream != null)
			{
				int num4 = ps.bytesUsed - ps.bytePos;
				if (num4 <= 128)
				{
					if (num4 == 0)
					{
						ps.bytesUsed = 0;
					}
					else
					{
						BlockCopy(ps.bytes, ps.bytePos, ps.bytes, 0, num4);
						ps.bytesUsed = num4;
					}
					ps.bytePos = 0;
				}
			}
			num = ps.chars.Length - ps.charsUsed - 1;
		}
		if (ps.stream != null)
		{
			if (!ps.isStreamEof && ps.bytePos == ps.bytesUsed && ps.bytes.Length - ps.bytesUsed > 0)
			{
				int num5 = ps.stream.Read(ps.bytes, ps.bytesUsed, ps.bytes.Length - ps.bytesUsed);
				if (num5 == 0)
				{
					ps.isStreamEof = true;
				}
				ps.bytesUsed += num5;
			}
			int bytePos = ps.bytePos;
			num = GetChars(num);
			if (num == 0 && ps.bytePos != bytePos)
			{
				return ReadData();
			}
		}
		else if (ps.textReader != null)
		{
			num = ps.textReader.Read(ps.chars, ps.charsUsed, ps.chars.Length - ps.charsUsed - 1);
			ps.charsUsed += num;
		}
		else
		{
			num = 0;
		}
		RegisterConsumedCharacters(num, InEntity);
		if (num == 0)
		{
			ps.isEof = true;
		}
		ps.chars[ps.charsUsed] = '\0';
		return num;
	}

	private int GetChars(int maxCharsCount)
	{
		int bytesUsed = ps.bytesUsed - ps.bytePos;
		if (bytesUsed == 0)
		{
			return 0;
		}
		int charsUsed;
		try
		{
			ps.decoder.Convert(ps.bytes, ps.bytePos, bytesUsed, ps.chars, ps.charsUsed, maxCharsCount, flush: false, out bytesUsed, out charsUsed, out var _);
		}
		catch (ArgumentException)
		{
			InvalidCharRecovery(ref bytesUsed, out charsUsed);
		}
		ps.bytePos += bytesUsed;
		ps.charsUsed += charsUsed;
		return charsUsed;
	}

	private void InvalidCharRecovery(ref int bytesCount, out int charsCount)
	{
		int num = 0;
		int i = 0;
		try
		{
			int bytesUsed;
			for (; i < bytesCount; i += bytesUsed)
			{
				ps.decoder.Convert(ps.bytes, ps.bytePos + i, 1, ps.chars, ps.charsUsed + num, 1, flush: false, out bytesUsed, out var charsUsed, out var _);
				num += charsUsed;
			}
		}
		catch (ArgumentException)
		{
		}
		if (num == 0)
		{
			Throw(ps.charsUsed, "Invalid character in the given encoding.");
		}
		charsCount = num;
		bytesCount = i;
	}

	internal void Close(bool closeInput)
	{
		if (parsingFunction != ParsingFunction.ReaderClosed)
		{
			while (InEntity)
			{
				PopParsingState();
			}
			ps.Close(closeInput);
			curNode = NodeData.None;
			parsingFunction = ParsingFunction.ReaderClosed;
			reportedEncoding = null;
			reportedBaseUri = string.Empty;
			readState = ReadState.Closed;
			fullAttrCleanup = false;
			ResetAttributes();
			laterInitParam = null;
		}
	}

	private void ShiftBuffer(int sourcePos, int destPos, int count)
	{
		BlockCopyChars(ps.chars, sourcePos, ps.chars, destPos, count);
	}

	private bool ParseXmlDeclaration(bool isTextDecl)
	{
		do
		{
			if (ps.charsUsed - ps.charPos < 6)
			{
				continue;
			}
			if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 5, "<?xml") || xmlCharType.IsNameSingleChar(ps.chars[ps.charPos + 5]))
			{
				break;
			}
			if (!isTextDecl)
			{
				curNode.SetLineInfo(ps.LineNo, ps.LinePos + 2);
				curNode.SetNamedNode(XmlNodeType.XmlDeclaration, Xml);
			}
			ps.charPos += 5;
			StringBuilder stringBuilder = (isTextDecl ? new StringBuilder() : this.stringBuilder);
			int num = 0;
			Encoding encoding = null;
			while (true)
			{
				int length = stringBuilder.Length;
				int num2 = EatWhitespaces((num == 0) ? null : stringBuilder);
				if (ps.chars[ps.charPos] == '?')
				{
					stringBuilder.Length = length;
					if (ps.chars[ps.charPos + 1] == '>')
					{
						break;
					}
					if (ps.charPos + 1 == ps.charsUsed)
					{
						goto IL_07b8;
					}
					ThrowUnexpectedToken("'>'");
				}
				if (num2 == 0 && num != 0)
				{
					ThrowUnexpectedToken("?>");
				}
				int num3 = ParseName();
				NodeData nodeData = null;
				char c = ps.chars[ps.charPos];
				if (c != 'e')
				{
					if (c != 's')
					{
						if (c != 'v' || !XmlConvert.StrEqual(ps.chars, ps.charPos, num3 - ps.charPos, "version") || num != 0)
						{
							goto IL_03b5;
						}
						if (!isTextDecl)
						{
							nodeData = AddAttributeNoChecks("version", 1);
						}
					}
					else
					{
						if (!XmlConvert.StrEqual(ps.chars, ps.charPos, num3 - ps.charPos, "standalone") || (num != 1 && num != 2) || isTextDecl)
						{
							goto IL_03b5;
						}
						if (!isTextDecl)
						{
							nodeData = AddAttributeNoChecks("standalone", 1);
						}
						num = 2;
					}
				}
				else
				{
					if (!XmlConvert.StrEqual(ps.chars, ps.charPos, num3 - ps.charPos, "encoding") || (num != 1 && (!isTextDecl || num != 0)))
					{
						goto IL_03b5;
					}
					if (!isTextDecl)
					{
						nodeData = AddAttributeNoChecks("encoding", 1);
					}
					num = 1;
				}
				goto IL_03ca;
				IL_03ca:
				if (!isTextDecl)
				{
					nodeData.SetLineInfo(ps.LineNo, ps.LinePos);
				}
				stringBuilder.Append(ps.chars, ps.charPos, num3 - ps.charPos);
				ps.charPos = num3;
				if (ps.chars[ps.charPos] != '=')
				{
					EatWhitespaces(stringBuilder);
					if (ps.chars[ps.charPos] != '=')
					{
						ThrowUnexpectedToken("=");
					}
				}
				stringBuilder.Append('=');
				ps.charPos++;
				char c2 = ps.chars[ps.charPos];
				if (c2 != '"' && c2 != '\'')
				{
					EatWhitespaces(stringBuilder);
					c2 = ps.chars[ps.charPos];
					if (c2 != '"' && c2 != '\'')
					{
						ThrowUnexpectedToken("\"", "'");
					}
				}
				stringBuilder.Append(c2);
				ps.charPos++;
				if (!isTextDecl)
				{
					nodeData.quoteChar = c2;
					nodeData.SetLineInfo2(ps.LineNo, ps.LinePos);
				}
				int i = ps.charPos;
				char[] chars;
				while (true)
				{
					for (chars = ps.chars; (xmlCharType.charProperties[(uint)chars[i]] & 0x80) != 0; i++)
					{
					}
					if (ps.chars[i] == c2)
					{
						break;
					}
					if (i == ps.charsUsed)
					{
						if (ReadData() != 0)
						{
							continue;
						}
						goto IL_0796;
					}
					goto IL_07a3;
				}
				switch (num)
				{
				case 0:
					if (XmlConvert.StrEqual(ps.chars, ps.charPos, i - ps.charPos, "1.0"))
					{
						if (!isTextDecl)
						{
							nodeData.SetValue(ps.chars, ps.charPos, i - ps.charPos);
						}
						num = 1;
					}
					else
					{
						string arg = new string(ps.chars, ps.charPos, i - ps.charPos);
						Throw("Version number '{0}' is invalid.", arg);
					}
					break;
				case 1:
				{
					string text = new string(ps.chars, ps.charPos, i - ps.charPos);
					encoding = CheckEncoding(text);
					if (!isTextDecl)
					{
						nodeData.SetValue(text);
					}
					num = 2;
					break;
				}
				case 2:
					if (XmlConvert.StrEqual(ps.chars, ps.charPos, i - ps.charPos, "yes"))
					{
						standalone = true;
					}
					else if (XmlConvert.StrEqual(ps.chars, ps.charPos, i - ps.charPos, "no"))
					{
						standalone = false;
					}
					else
					{
						Throw("Syntax for an XML declaration is invalid.", ps.LineNo, ps.LinePos - 1);
					}
					if (!isTextDecl)
					{
						nodeData.SetValue(ps.chars, ps.charPos, i - ps.charPos);
					}
					num = 3;
					break;
				}
				stringBuilder.Append(chars, ps.charPos, i - ps.charPos);
				stringBuilder.Append(c2);
				ps.charPos = i + 1;
				continue;
				IL_07b8:
				if (ps.isEof || ReadData() == 0)
				{
					Throw("Unexpected end of file has occurred.");
				}
				continue;
				IL_07a3:
				Throw(isTextDecl ? "Invalid text declaration." : "Syntax for an XML declaration is invalid.");
				goto IL_07b8;
				IL_0796:
				Throw("There is an unclosed literal string.");
				goto IL_07b8;
				IL_03b5:
				Throw(isTextDecl ? "Invalid text declaration." : "Syntax for an XML declaration is invalid.");
				goto IL_03ca;
			}
			if (num == 0)
			{
				Throw(isTextDecl ? "Invalid text declaration." : "Syntax for an XML declaration is invalid.");
			}
			ps.charPos += 2;
			if (!isTextDecl)
			{
				curNode.SetValue(stringBuilder.ToString());
				stringBuilder.Length = 0;
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.ResetAttributesRootLevel;
			}
			if (encoding == null)
			{
				if (isTextDecl)
				{
					Throw("Invalid text declaration.");
				}
				if (afterResetState)
				{
					string webName = ps.encoding.WebName;
					if (webName != "utf-8" && webName != "utf-16" && webName != "utf-16BE" && !(ps.encoding is Ucs4Encoding))
					{
						Throw("'{0}' is an invalid value for the 'encoding' attribute. The encoding cannot be switched after a call to ResetState.", (ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
					}
				}
				if (ps.decoder is SafeAsciiDecoder)
				{
					SwitchEncodingToUTF8();
				}
			}
			else
			{
				SwitchEncoding(encoding);
			}
			ps.appendMode = false;
			return true;
		}
		while (ReadData() != 0);
		if (!isTextDecl)
		{
			parsingFunction = nextParsingFunction;
		}
		if (afterResetState)
		{
			string webName2 = ps.encoding.WebName;
			if (webName2 != "utf-8" && webName2 != "utf-16" && webName2 != "utf-16BE" && !(ps.encoding is Ucs4Encoding))
			{
				Throw("'{0}' is an invalid value for the 'encoding' attribute. The encoding cannot be switched after a call to ResetState.", (ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
			}
		}
		if (ps.decoder is SafeAsciiDecoder)
		{
			SwitchEncodingToUTF8();
		}
		ps.appendMode = false;
		return false;
	}

	private bool ParseDocumentContent()
	{
		bool flag = false;
		while (true)
		{
			bool flag2 = false;
			int charPos = ps.charPos;
			char[] chars = ps.chars;
			if (chars[charPos] == '<')
			{
				flag2 = true;
				if (ps.charsUsed - charPos >= 4)
				{
					charPos++;
					switch (chars[charPos])
					{
					case '?':
						ps.charPos = charPos + 1;
						if (!ParsePI())
						{
							continue;
						}
						return true;
					case '!':
						charPos++;
						if (ps.charsUsed - charPos < 2)
						{
							break;
						}
						if (chars[charPos] == '-')
						{
							if (chars[charPos + 1] == '-')
							{
								ps.charPos = charPos + 2;
								if (!ParseComment())
								{
									continue;
								}
								return true;
							}
							ThrowUnexpectedToken(charPos + 1, "-");
							break;
						}
						if (chars[charPos] == '[')
						{
							if (fragmentType != XmlNodeType.Document)
							{
								charPos++;
								if (ps.charsUsed - charPos < 6)
								{
									break;
								}
								if (XmlConvert.StrEqual(chars, charPos, 6, "CDATA["))
								{
									ps.charPos = charPos + 6;
									ParseCData();
									if (fragmentType == XmlNodeType.None)
									{
										fragmentType = XmlNodeType.Element;
									}
									return true;
								}
								ThrowUnexpectedToken(charPos, "CDATA[");
							}
							else
							{
								Throw(ps.charPos, "Data at the root level is invalid.");
							}
							break;
						}
						if (fragmentType == XmlNodeType.Document || fragmentType == XmlNodeType.None)
						{
							fragmentType = XmlNodeType.Document;
							ps.charPos = charPos;
							if (!ParseDoctypeDecl())
							{
								continue;
							}
							return true;
						}
						if (ParseUnexpectedToken(charPos) == "DOCTYPE")
						{
							Throw("Unexpected DTD declaration.");
						}
						else
						{
							ThrowUnexpectedToken(charPos, "<!--", "<[CDATA[");
						}
						break;
					case '/':
						Throw(charPos + 1, "Unexpected end tag.");
						break;
					default:
						if (rootElementParsed)
						{
							if (fragmentType == XmlNodeType.Document)
							{
								Throw(charPos, "There are multiple root elements.");
							}
							if (fragmentType == XmlNodeType.None)
							{
								fragmentType = XmlNodeType.Element;
							}
						}
						ps.charPos = charPos;
						rootElementParsed = true;
						ParseElement();
						return true;
					}
				}
			}
			else if (chars[charPos] == '&')
			{
				if (fragmentType != XmlNodeType.Document)
				{
					if (fragmentType == XmlNodeType.None)
					{
						fragmentType = XmlNodeType.Element;
					}
					int charRefEndPos;
					switch (HandleEntityReference(isInAttributeValue: false, EntityExpandType.OnlyGeneral, out charRefEndPos))
					{
					case EntityType.Unexpanded:
						if (parsingFunction == ParsingFunction.EntityReference)
						{
							parsingFunction = nextParsingFunction;
						}
						ParseEntityReference();
						return true;
					case EntityType.CharacterDec:
					case EntityType.CharacterHex:
					case EntityType.CharacterNamed:
						if (ParseText())
						{
							return true;
						}
						break;
					default:
						chars = ps.chars;
						charPos = ps.charPos;
						break;
					}
					continue;
				}
				Throw(charPos, "Data at the root level is invalid.");
			}
			else if (charPos != ps.charsUsed && (!(v1Compat || flag) || chars[charPos] != 0))
			{
				if (fragmentType == XmlNodeType.Document)
				{
					if (ParseRootLevelWhitespace())
					{
						return true;
					}
				}
				else if (ParseText())
				{
					if (fragmentType == XmlNodeType.None && curNode.type == XmlNodeType.Text)
					{
						fragmentType = XmlNodeType.Element;
					}
					return true;
				}
				continue;
			}
			if (ReadData() != 0)
			{
				charPos = ps.charPos;
				charPos = ps.charPos;
				chars = ps.chars;
				continue;
			}
			if (flag2)
			{
				Throw("Data at the root level is invalid.");
			}
			if (!InEntity)
			{
				break;
			}
			if (HandleEntityEnd(checkEntityNesting: true))
			{
				SetupEndEntityNodeInContent();
				return true;
			}
		}
		if (!rootElementParsed && fragmentType == XmlNodeType.Document)
		{
			ThrowWithoutLineInfo("Root element is missing.");
		}
		if (fragmentType == XmlNodeType.None)
		{
			fragmentType = ((!rootElementParsed) ? XmlNodeType.Element : XmlNodeType.Document);
		}
		OnEof();
		return false;
	}

	private bool ParseElementContent()
	{
		while (true)
		{
			int charPos = ps.charPos;
			char[] chars = ps.chars;
			switch (chars[charPos])
			{
			case '<':
				switch (chars[charPos + 1])
				{
				case '?':
					ps.charPos = charPos + 2;
					if (!ParsePI())
					{
						continue;
					}
					return true;
				case '!':
					charPos += 2;
					if (ps.charsUsed - charPos < 2)
					{
						break;
					}
					if (chars[charPos] == '-')
					{
						if (chars[charPos + 1] == '-')
						{
							ps.charPos = charPos + 2;
							if (!ParseComment())
							{
								continue;
							}
							return true;
						}
						ThrowUnexpectedToken(charPos + 1, "-");
					}
					else if (chars[charPos] == '[')
					{
						charPos++;
						if (ps.charsUsed - charPos >= 6)
						{
							if (XmlConvert.StrEqual(chars, charPos, 6, "CDATA["))
							{
								ps.charPos = charPos + 6;
								ParseCData();
								return true;
							}
							ThrowUnexpectedToken(charPos, "CDATA[");
						}
					}
					else if (ParseUnexpectedToken(charPos) == "DOCTYPE")
					{
						Throw("Unexpected DTD declaration.");
					}
					else
					{
						ThrowUnexpectedToken(charPos, "<!--", "<[CDATA[");
					}
					break;
				case '/':
					ps.charPos = charPos + 2;
					ParseEndElement();
					return true;
				default:
					if (charPos + 1 != ps.charsUsed)
					{
						ps.charPos = charPos + 1;
						ParseElement();
						return true;
					}
					break;
				}
				break;
			case '&':
				if (!ParseText())
				{
					continue;
				}
				return true;
			default:
				if (charPos != ps.charsUsed)
				{
					if (!ParseText())
					{
						continue;
					}
					return true;
				}
				break;
			}
			if (ReadData() != 0)
			{
				continue;
			}
			if (ps.charsUsed - ps.charPos != 0)
			{
				ThrowUnclosedElements();
			}
			if (!InEntity)
			{
				if (index == 0 && fragmentType != XmlNodeType.Document)
				{
					OnEof();
					return false;
				}
				ThrowUnclosedElements();
			}
			if (HandleEntityEnd(checkEntityNesting: true))
			{
				break;
			}
		}
		SetupEndEntityNodeInContent();
		return true;
	}

	private void ThrowUnclosedElements()
	{
		if (index == 0 && curNode.type != XmlNodeType.Element)
		{
			Throw(ps.charsUsed, "Unexpected end of file has occurred.");
			return;
		}
		int num = ((parsingFunction == ParsingFunction.InIncrementalRead) ? index : (index - 1));
		stringBuilder.Length = 0;
		while (num >= 0)
		{
			NodeData nodeData = nodes[num];
			if (nodeData.type == XmlNodeType.Element)
			{
				stringBuilder.Append(nodeData.GetNameWPrefix(nameTable));
				if (num > 0)
				{
					stringBuilder.Append(", ");
				}
				else
				{
					stringBuilder.Append(".");
				}
			}
			num--;
		}
		Throw(ps.charsUsed, "Unexpected end of file has occurred. The following elements are not closed: {0}", stringBuilder.ToString());
	}

	private void ParseElement()
	{
		int num = ps.charPos;
		char[] chars = ps.chars;
		int colonPos = -1;
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[num]] & 4) != 0)
			{
				num++;
				while (true)
				{
					if ((xmlCharType.charProperties[(uint)chars[num]] & 8) != 0)
					{
						num++;
						continue;
					}
					if (chars[num] != ':')
					{
						break;
					}
					if (colonPos == -1)
					{
						goto IL_009a;
					}
					if (!supportNamespaces)
					{
						num++;
						continue;
					}
					goto IL_007e;
				}
				if (num + 1 < ps.charsUsed)
				{
					break;
				}
			}
			goto IL_00b2;
			IL_009a:
			colonPos = num;
			num++;
			continue;
			IL_00b2:
			num = ParseQName(out colonPos);
			chars = ps.chars;
			break;
			IL_007e:
			Throw(num, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
			goto IL_00b2;
		}
		namespaceManager.PushScope();
		if (colonPos == -1 || !supportNamespaces)
		{
			curNode.SetNamedNode(XmlNodeType.Element, nameTable.Add(chars, ps.charPos, num - ps.charPos));
		}
		else
		{
			int charPos = ps.charPos;
			int num2 = colonPos - charPos;
			if (num2 == lastPrefix.Length && XmlConvert.StrEqual(chars, charPos, num2, lastPrefix))
			{
				curNode.SetNamedNode(XmlNodeType.Element, nameTable.Add(chars, colonPos + 1, num - colonPos - 1), lastPrefix, null);
			}
			else
			{
				curNode.SetNamedNode(XmlNodeType.Element, nameTable.Add(chars, colonPos + 1, num - colonPos - 1), nameTable.Add(chars, ps.charPos, num2), null);
				lastPrefix = curNode.prefix;
			}
		}
		char c = chars[num];
		if ((xmlCharType.charProperties[(uint)c] & 1) != 0)
		{
			ps.charPos = num;
			ParseAttributes();
			return;
		}
		switch (c)
		{
		case '>':
			ps.charPos = num + 1;
			parsingFunction = ParsingFunction.MoveToElementContent;
			break;
		case '/':
			if (num + 1 == ps.charsUsed)
			{
				ps.charPos = num;
				if (ReadData() == 0)
				{
					Throw(num, "Unexpected end of file while parsing {0} has occurred.", ">");
				}
				num = ps.charPos;
				chars = ps.chars;
			}
			if (chars[num + 1] == '>')
			{
				curNode.IsEmptyElement = true;
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.PopEmptyElementContext;
				ps.charPos = num + 2;
			}
			else
			{
				ThrowUnexpectedToken(num, ">");
			}
			break;
		default:
			Throw(num, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(chars, ps.charsUsed, num));
			break;
		}
		if (addDefaultAttributesAndNormalize)
		{
			AddDefaultAttributesAndNormalize();
		}
		ElementNamespaceLookup();
	}

	private void AddDefaultAttributesAndNormalize()
	{
		IDtdAttributeListInfo dtdAttributeListInfo = dtdInfo.LookupAttributeList(curNode.localName, curNode.prefix);
		if (dtdAttributeListInfo == null)
		{
			return;
		}
		if (normalize && dtdAttributeListInfo.HasNonCDataAttributes)
		{
			for (int i = index + 1; i < index + 1 + attrCount; i++)
			{
				NodeData nodeData = nodes[i];
				IDtdAttributeInfo dtdAttributeInfo = dtdAttributeListInfo.LookupAttribute(nodeData.prefix, nodeData.localName);
				if (dtdAttributeInfo == null || !dtdAttributeInfo.IsNonCDataType)
				{
					continue;
				}
				if (DtdValidation && standalone && dtdAttributeInfo.IsDeclaredInExternal)
				{
					string stringValue = nodeData.StringValue;
					nodeData.TrimSpacesInValue();
					if (stringValue != nodeData.StringValue)
					{
						SendValidationEvent(XmlSeverityType.Error, "StandAlone is 'yes' and the value of the attribute '{0}' contains a definition in an external document that changes on normalization.", nodeData.GetNameWPrefix(nameTable), nodeData.LineNo, nodeData.LinePos);
					}
				}
				else
				{
					nodeData.TrimSpacesInValue();
				}
			}
		}
		IEnumerable<IDtdDefaultAttributeInfo> enumerable = dtdAttributeListInfo.LookupDefaultAttributes();
		if (enumerable == null)
		{
			return;
		}
		int num = attrCount;
		NodeData[] array = null;
		if (attrCount >= 250)
		{
			array = new NodeData[attrCount];
			Array.Copy(nodes, index + 1, array, 0, attrCount);
			object[] array2 = array;
			Array.Sort(array2, DtdDefaultAttributeInfoToNodeDataComparer.Instance);
		}
		foreach (IDtdDefaultAttributeInfo item in enumerable)
		{
			if (AddDefaultAttributeDtd(item, definedInDtd: true, array) && DtdValidation && standalone && item.IsDeclaredInExternal)
			{
				string prefix = item.Prefix;
				string arg = ((prefix.Length == 0) ? item.LocalName : (prefix + ":" + item.LocalName));
				SendValidationEvent(XmlSeverityType.Error, "Markup for unspecified default attribute '{0}' is external and standalone='yes'.", arg, curNode.LineNo, curNode.LinePos);
			}
		}
		if (num == 0 && attrNeedNamespaceLookup)
		{
			AttributeNamespaceLookup();
			attrNeedNamespaceLookup = false;
		}
	}

	private void ParseEndElement()
	{
		NodeData nodeData = nodes[index - 1];
		int length = nodeData.prefix.Length;
		int length2 = nodeData.localName.Length;
		while (ps.charsUsed - ps.charPos < length + length2 + 1 && ReadData() != 0)
		{
		}
		char[] chars = ps.chars;
		int num;
		if (nodeData.prefix.Length == 0)
		{
			if (!XmlConvert.StrEqual(chars, ps.charPos, length2, nodeData.localName))
			{
				ThrowTagMismatch(nodeData);
			}
			num = length2;
		}
		else
		{
			int num2 = ps.charPos + length;
			if (!XmlConvert.StrEqual(chars, ps.charPos, length, nodeData.prefix) || chars[num2] != ':' || !XmlConvert.StrEqual(chars, num2 + 1, length2, nodeData.localName))
			{
				ThrowTagMismatch(nodeData);
			}
			num = length2 + length + 1;
		}
		LineInfo lineInfo = new LineInfo(ps.lineNo, ps.LinePos);
		int num3;
		while (true)
		{
			num3 = ps.charPos + num;
			chars = ps.chars;
			if (num3 != ps.charsUsed)
			{
				if ((xmlCharType.charProperties[(uint)chars[num3]] & 8) != 0 || chars[num3] == ':')
				{
					ThrowTagMismatch(nodeData);
				}
				if (chars[num3] != '>')
				{
					char c;
					while (xmlCharType.IsWhiteSpace(c = chars[num3]))
					{
						num3++;
						switch (c)
						{
						case '\n':
							OnNewLine(num3);
							break;
						case '\r':
							if (chars[num3] == '\n')
							{
								num3++;
							}
							else if (num3 == ps.charsUsed && !ps.isEof)
							{
								break;
							}
							OnNewLine(num3);
							break;
						}
					}
				}
				if (chars[num3] == '>')
				{
					break;
				}
				if (num3 != ps.charsUsed)
				{
					ThrowUnexpectedToken(num3, ">");
				}
			}
			if (ReadData() == 0)
			{
				ThrowUnclosedElements();
			}
		}
		index--;
		curNode = nodes[index];
		nodeData.lineInfo = lineInfo;
		nodeData.type = XmlNodeType.EndElement;
		ps.charPos = num3 + 1;
		nextParsingFunction = ((index > 0) ? parsingFunction : ParsingFunction.DocumentContent);
		parsingFunction = ParsingFunction.PopElementContext;
	}

	private void ThrowTagMismatch(NodeData startTag)
	{
		if (startTag.type == XmlNodeType.Element)
		{
			int colonPos;
			int num = ParseQName(out colonPos);
			Throw("The '{0}' start tag on line {1} position {2} does not match the end tag of '{3}'.", new string[4]
			{
				startTag.GetNameWPrefix(nameTable),
				startTag.lineInfo.lineNo.ToString(CultureInfo.InvariantCulture),
				startTag.lineInfo.linePos.ToString(CultureInfo.InvariantCulture),
				new string(ps.chars, ps.charPos, num - ps.charPos)
			});
		}
		else
		{
			Throw("Unexpected end tag.");
		}
	}

	private void ParseAttributes()
	{
		int num = ps.charPos;
		char[] chars = ps.chars;
		NodeData nodeData = null;
		while (true)
		{
			int num2 = 0;
			while (true)
			{
				char c;
				int num3;
				if ((xmlCharType.charProperties[(uint)(c = chars[num])] & 1) != 0)
				{
					switch (c)
					{
					case '\n':
						OnNewLine(num + 1);
						num2++;
						goto IL_0085;
					case '\r':
						if (chars[num + 1] == '\n')
						{
							OnNewLine(num + 2);
							num2++;
							num++;
							goto IL_0085;
						}
						if (num + 1 != ps.charsUsed)
						{
							OnNewLine(num + 1);
							num2++;
							goto IL_0085;
						}
						break;
					default:
						goto IL_0085;
					}
					ps.charPos = num;
				}
				else
				{
					num3 = 0;
					char c2;
					if ((xmlCharType.charProperties[(uint)(c2 = chars[num])] & 4) != 0)
					{
						num3 = 1;
					}
					if (num3 != 0)
					{
						goto IL_0186;
					}
					if (c2 == '>')
					{
						ps.charPos = num + 1;
						parsingFunction = ParsingFunction.MoveToElementContent;
						goto IL_046c;
					}
					if (c2 == '/')
					{
						if (num + 1 != ps.charsUsed)
						{
							if (chars[num + 1] == '>')
							{
								ps.charPos = num + 2;
								curNode.IsEmptyElement = true;
								nextParsingFunction = parsingFunction;
								parsingFunction = ParsingFunction.PopEmptyElementContext;
								goto IL_046c;
							}
							ThrowUnexpectedToken(num + 1, ">");
							goto IL_0186;
						}
					}
					else if (num != ps.charsUsed)
					{
						if (c2 != ':' || supportNamespaces)
						{
							Throw(num, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(chars, ps.charsUsed, num));
						}
						goto IL_0186;
					}
				}
				ps.lineNo -= num2;
				if (ReadData() != 0)
				{
					num = ps.charPos;
					chars = ps.chars;
				}
				else
				{
					ThrowUnclosedElements();
				}
				break;
				IL_046c:
				if (addDefaultAttributesAndNormalize)
				{
					AddDefaultAttributesAndNormalize();
				}
				ElementNamespaceLookup();
				if (attrNeedNamespaceLookup)
				{
					AttributeNamespaceLookup();
					attrNeedNamespaceLookup = false;
				}
				if (attrDuplWalkCount >= 250)
				{
					AttributeDuplCheck();
				}
				return;
				IL_0085:
				num++;
				continue;
				IL_0186:
				if (num == ps.charPos)
				{
					ThrowExpectingWhitespace(num);
				}
				ps.charPos = num;
				int linePos = ps.LinePos;
				int colonPos = -1;
				num += num3;
				while (true)
				{
					char c3;
					if ((xmlCharType.charProperties[(uint)(c3 = chars[num])] & 8) != 0)
					{
						num++;
						continue;
					}
					if (c3 == ':')
					{
						if (colonPos != -1)
						{
							if (supportNamespaces)
							{
								Throw(num, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
								break;
							}
							num++;
							continue;
						}
						colonPos = num;
						num++;
						if ((xmlCharType.charProperties[(uint)chars[num]] & 4) != 0)
						{
							num++;
							continue;
						}
						num = ParseQName(out colonPos);
						chars = ps.chars;
						break;
					}
					if (num + 1 >= ps.charsUsed)
					{
						num = ParseQName(out colonPos);
						chars = ps.chars;
					}
					break;
				}
				nodeData = AddAttribute(num, colonPos);
				nodeData.SetLineInfo(ps.LineNo, linePos);
				if (chars[num] != '=')
				{
					ps.charPos = num;
					EatWhitespaces(null);
					num = ps.charPos;
					if (chars[num] != '=')
					{
						ThrowUnexpectedToken("=");
					}
				}
				num++;
				char c4 = chars[num];
				if (c4 != '"' && c4 != '\'')
				{
					ps.charPos = num;
					EatWhitespaces(null);
					num = ps.charPos;
					c4 = chars[num];
					if (c4 != '"' && c4 != '\'')
					{
						ThrowUnexpectedToken("\"", "'");
					}
				}
				num++;
				ps.charPos = num;
				nodeData.quoteChar = c4;
				nodeData.SetLineInfo2(ps.LineNo, ps.LinePos);
				char c5;
				while ((xmlCharType.charProperties[(uint)(c5 = chars[num])] & 0x80) != 0)
				{
					num++;
				}
				if (c5 == c4)
				{
					nodeData.SetValue(chars, ps.charPos, num - ps.charPos);
					num++;
					ps.charPos = num;
				}
				else
				{
					ParseAttributeValueSlow(num, c4, nodeData);
					num = ps.charPos;
					chars = ps.chars;
				}
				if (nodeData.prefix.Length == 0)
				{
					if (Ref.Equal(nodeData.localName, XmlNs))
					{
						OnDefaultNamespaceDecl(nodeData);
					}
				}
				else if (Ref.Equal(nodeData.prefix, XmlNs))
				{
					OnNamespaceDecl(nodeData);
				}
				else if (Ref.Equal(nodeData.prefix, Xml))
				{
					OnXmlReservedAttribute(nodeData);
				}
				break;
			}
		}
	}

	private void ElementNamespaceLookup()
	{
		if (curNode.prefix.Length == 0)
		{
			curNode.ns = xmlContext.defaultNamespace;
		}
		else
		{
			curNode.ns = LookupNamespace(curNode);
		}
	}

	private void AttributeNamespaceLookup()
	{
		for (int i = index + 1; i < index + attrCount + 1; i++)
		{
			NodeData nodeData = nodes[i];
			if (nodeData.type == XmlNodeType.Attribute && nodeData.prefix.Length > 0)
			{
				nodeData.ns = LookupNamespace(nodeData);
			}
		}
	}

	private void AttributeDuplCheck()
	{
		if (attrCount < 250)
		{
			for (int i = index + 1; i < index + 1 + attrCount; i++)
			{
				NodeData nodeData = nodes[i];
				for (int j = i + 1; j < index + 1 + attrCount; j++)
				{
					if (Ref.Equal(nodeData.localName, nodes[j].localName) && Ref.Equal(nodeData.ns, nodes[j].ns))
					{
						Throw("'{0}' is a duplicate attribute name.", nodes[j].GetNameWPrefix(nameTable), nodes[j].LineNo, nodes[j].LinePos);
					}
				}
			}
			return;
		}
		if (attrDuplSortingArray == null || attrDuplSortingArray.Length < attrCount)
		{
			attrDuplSortingArray = new NodeData[attrCount];
		}
		Array.Copy(nodes, index + 1, attrDuplSortingArray, 0, attrCount);
		Array.Sort(attrDuplSortingArray, 0, attrCount);
		NodeData nodeData2 = attrDuplSortingArray[0];
		for (int k = 1; k < attrCount; k++)
		{
			NodeData nodeData3 = attrDuplSortingArray[k];
			if (Ref.Equal(nodeData2.localName, nodeData3.localName) && Ref.Equal(nodeData2.ns, nodeData3.ns))
			{
				Throw("'{0}' is a duplicate attribute name.", nodeData3.GetNameWPrefix(nameTable), nodeData3.LineNo, nodeData3.LinePos);
			}
			nodeData2 = nodeData3;
		}
	}

	private void OnDefaultNamespaceDecl(NodeData attr)
	{
		if (supportNamespaces)
		{
			string text = nameTable.Add(attr.StringValue);
			attr.ns = nameTable.Add("http://www.w3.org/2000/xmlns/");
			if (!curNode.xmlContextPushed)
			{
				PushXmlContext();
			}
			xmlContext.defaultNamespace = text;
			AddNamespace(string.Empty, text, attr);
		}
	}

	private void OnNamespaceDecl(NodeData attr)
	{
		if (supportNamespaces)
		{
			string text = nameTable.Add(attr.StringValue);
			if (text.Length == 0)
			{
				Throw("Invalid namespace declaration.", attr.lineInfo2.lineNo, attr.lineInfo2.linePos - 1);
			}
			AddNamespace(attr.localName, text, attr);
		}
	}

	private void OnXmlReservedAttribute(NodeData attr)
	{
		string localName = attr.localName;
		if (!(localName == "space"))
		{
			if (localName == "lang")
			{
				if (!curNode.xmlContextPushed)
				{
					PushXmlContext();
				}
				xmlContext.xmlLang = attr.StringValue;
			}
			return;
		}
		if (!curNode.xmlContextPushed)
		{
			PushXmlContext();
		}
		string text = XmlConvert.TrimString(attr.StringValue);
		if (!(text == "preserve"))
		{
			if (text == "default")
			{
				xmlContext.xmlSpace = XmlSpace.Default;
			}
			else
			{
				Throw("'{0}' is an invalid xml:space value.", attr.StringValue, attr.lineInfo.lineNo, attr.lineInfo.linePos);
			}
		}
		else
		{
			xmlContext.xmlSpace = XmlSpace.Preserve;
		}
	}

	private void ParseAttributeValueSlow(int curPos, char quoteChar, NodeData attr)
	{
		int charRefEndPos = curPos;
		char[] chars = ps.chars;
		int entityId = ps.entityId;
		int num = 0;
		LineInfo lineInfo = new LineInfo(ps.lineNo, ps.LinePos);
		NodeData lastChunk = null;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[charRefEndPos]] & 0x80) != 0)
			{
				charRefEndPos++;
				continue;
			}
			if (charRefEndPos - ps.charPos > 0)
			{
				stringBuilder.Append(chars, ps.charPos, charRefEndPos - ps.charPos);
				ps.charPos = charRefEndPos;
			}
			if (chars[charRefEndPos] == quoteChar && entityId == ps.entityId)
			{
				break;
			}
			switch (chars[charRefEndPos])
			{
			case '\n':
				charRefEndPos++;
				OnNewLine(charRefEndPos);
				if (normalize)
				{
					stringBuilder.Append(' ');
					ps.charPos++;
				}
				continue;
			case '\r':
				if (chars[charRefEndPos + 1] == '\n')
				{
					charRefEndPos += 2;
					if (normalize)
					{
						stringBuilder.Append(ps.eolNormalized ? "  " : " ");
						ps.charPos = charRefEndPos;
					}
				}
				else
				{
					if (charRefEndPos + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					charRefEndPos++;
					if (normalize)
					{
						stringBuilder.Append(' ');
						ps.charPos = charRefEndPos;
					}
				}
				OnNewLine(charRefEndPos);
				continue;
			case '\t':
				charRefEndPos++;
				if (normalize)
				{
					stringBuilder.Append(' ');
					ps.charPos++;
				}
				continue;
			case '"':
			case '\'':
			case '>':
				charRefEndPos++;
				continue;
			case '<':
				Throw(charRefEndPos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
				break;
			case '&':
			{
				if (charRefEndPos - ps.charPos > 0)
				{
					stringBuilder.Append(chars, ps.charPos, charRefEndPos - ps.charPos);
				}
				ps.charPos = charRefEndPos;
				int entityId2 = ps.entityId;
				LineInfo lineInfo2 = new LineInfo(ps.lineNo, ps.LinePos + 1);
				switch (HandleEntityReference(isInAttributeValue: true, EntityExpandType.All, out charRefEndPos))
				{
				case EntityType.Unexpanded:
					if (parsingMode == ParsingMode.Full && ps.entityId == entityId)
					{
						int num3 = stringBuilder.Length - num;
						if (num3 > 0)
						{
							NodeData nodeData3 = new NodeData();
							nodeData3.lineInfo = lineInfo;
							nodeData3.depth = attr.depth + 1;
							nodeData3.SetValueNode(XmlNodeType.Text, stringBuilder.ToString(num, num3));
							AddAttributeChunkToList(attr, nodeData3, ref lastChunk);
						}
						ps.charPos++;
						string text = ParseEntityName();
						NodeData nodeData4 = new NodeData();
						nodeData4.lineInfo = lineInfo2;
						nodeData4.depth = attr.depth + 1;
						nodeData4.SetNamedNode(XmlNodeType.EntityReference, text);
						AddAttributeChunkToList(attr, nodeData4, ref lastChunk);
						stringBuilder.Append('&');
						stringBuilder.Append(text);
						stringBuilder.Append(';');
						num = stringBuilder.Length;
						lineInfo.Set(ps.LineNo, ps.LinePos);
						fullAttrCleanup = true;
					}
					else
					{
						ps.charPos++;
						ParseEntityName();
					}
					charRefEndPos = ps.charPos;
					break;
				case EntityType.ExpandedInAttribute:
					if (parsingMode == ParsingMode.Full && entityId2 == entityId)
					{
						int num2 = stringBuilder.Length - num;
						if (num2 > 0)
						{
							NodeData nodeData = new NodeData();
							nodeData.lineInfo = lineInfo;
							nodeData.depth = attr.depth + 1;
							nodeData.SetValueNode(XmlNodeType.Text, stringBuilder.ToString(num, num2));
							AddAttributeChunkToList(attr, nodeData, ref lastChunk);
						}
						NodeData nodeData2 = new NodeData();
						nodeData2.lineInfo = lineInfo2;
						nodeData2.depth = attr.depth + 1;
						nodeData2.SetNamedNode(XmlNodeType.EntityReference, ps.entity.Name);
						AddAttributeChunkToList(attr, nodeData2, ref lastChunk);
						fullAttrCleanup = true;
					}
					charRefEndPos = ps.charPos;
					break;
				default:
					charRefEndPos = ps.charPos;
					break;
				case EntityType.CharacterDec:
				case EntityType.CharacterHex:
				case EntityType.CharacterNamed:
					break;
				}
				chars = ps.chars;
				continue;
			}
			default:
				if (charRefEndPos == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[charRefEndPos]))
				{
					if (charRefEndPos + 1 == ps.charsUsed)
					{
						break;
					}
					charRefEndPos++;
					if (XmlCharType.IsLowSurrogate(chars[charRefEndPos]))
					{
						charRefEndPos++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, charRefEndPos);
				break;
			}
			if (ReadData() == 0)
			{
				if (ps.charsUsed - ps.charPos > 0)
				{
					if (ps.chars[ps.charPos] != '\r')
					{
						Throw("Unexpected end of file has occurred.");
					}
				}
				else
				{
					if (!InEntity)
					{
						if (fragmentType == XmlNodeType.Attribute)
						{
							if (entityId != ps.entityId)
							{
								Throw("Entity replacement text must nest properly within markup declarations.");
							}
							break;
						}
						Throw("There is an unclosed literal string.");
					}
					if (HandleEntityEnd(checkEntityNesting: true))
					{
						Throw("An internal error has occurred.");
					}
					if (entityId == ps.entityId)
					{
						num = stringBuilder.Length;
						lineInfo.Set(ps.LineNo, ps.LinePos);
					}
				}
			}
			charRefEndPos = ps.charPos;
			chars = ps.chars;
		}
		if (attr.nextAttrValueChunk != null)
		{
			int num4 = stringBuilder.Length - num;
			if (num4 > 0)
			{
				NodeData nodeData5 = new NodeData();
				nodeData5.lineInfo = lineInfo;
				nodeData5.depth = attr.depth + 1;
				nodeData5.SetValueNode(XmlNodeType.Text, stringBuilder.ToString(num, num4));
				AddAttributeChunkToList(attr, nodeData5, ref lastChunk);
			}
		}
		ps.charPos = charRefEndPos + 1;
		attr.SetValue(stringBuilder.ToString());
		stringBuilder.Length = 0;
	}

	private void AddAttributeChunkToList(NodeData attr, NodeData chunk, ref NodeData lastChunk)
	{
		if (lastChunk == null)
		{
			lastChunk = chunk;
			attr.nextAttrValueChunk = chunk;
		}
		else
		{
			lastChunk.nextAttrValueChunk = chunk;
			lastChunk = chunk;
		}
	}

	private bool ParseText()
	{
		int outOrChars = 0;
		int startPos;
		int endPos;
		if (parsingMode != ParsingMode.Full)
		{
			while (!ParseText(out startPos, out endPos, ref outOrChars))
			{
			}
		}
		else
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			if (ParseText(out startPos, out endPos, ref outOrChars))
			{
				if (endPos - startPos != 0)
				{
					XmlNodeType textNodeType = GetTextNodeType(outOrChars);
					if (textNodeType != XmlNodeType.None)
					{
						curNode.SetValueNode(textNodeType, ps.chars, startPos, endPos - startPos);
						return true;
					}
				}
			}
			else if (v1Compat)
			{
				do
				{
					if (endPos - startPos > 0)
					{
						stringBuilder.Append(ps.chars, startPos, endPos - startPos);
					}
				}
				while (!ParseText(out startPos, out endPos, ref outOrChars));
				if (endPos - startPos > 0)
				{
					stringBuilder.Append(ps.chars, startPos, endPos - startPos);
				}
				XmlNodeType textNodeType2 = GetTextNodeType(outOrChars);
				if (textNodeType2 != XmlNodeType.None)
				{
					curNode.SetValueNode(textNodeType2, stringBuilder.ToString());
					stringBuilder.Length = 0;
					return true;
				}
				stringBuilder.Length = 0;
			}
			else
			{
				bool flag = false;
				if (outOrChars > 32)
				{
					curNode.SetValueNode(XmlNodeType.Text, ps.chars, startPos, endPos - startPos);
					nextParsingFunction = parsingFunction;
					parsingFunction = ParsingFunction.PartialTextValue;
					return true;
				}
				if (endPos - startPos > 0)
				{
					stringBuilder.Append(ps.chars, startPos, endPos - startPos);
				}
				do
				{
					flag = ParseText(out startPos, out endPos, ref outOrChars);
					if (endPos - startPos > 0)
					{
						stringBuilder.Append(ps.chars, startPos, endPos - startPos);
					}
				}
				while (!flag && outOrChars <= 32 && stringBuilder.Length < 4096);
				XmlNodeType xmlNodeType = ((stringBuilder.Length < 4096) ? GetTextNodeType(outOrChars) : XmlNodeType.Text);
				if (xmlNodeType != XmlNodeType.None)
				{
					curNode.SetValueNode(xmlNodeType, stringBuilder.ToString());
					stringBuilder.Length = 0;
					if (!flag)
					{
						nextParsingFunction = parsingFunction;
						parsingFunction = ParsingFunction.PartialTextValue;
					}
					return true;
				}
				stringBuilder.Length = 0;
				if (!flag)
				{
					while (!ParseText(out startPos, out endPos, ref outOrChars))
					{
					}
				}
			}
		}
		if (parsingFunction == ParsingFunction.ReportEndEntity)
		{
			SetupEndEntityNodeInContent();
			parsingFunction = nextParsingFunction;
			return true;
		}
		if (parsingFunction == ParsingFunction.EntityReference)
		{
			parsingFunction = nextNextParsingFunction;
			ParseEntityReference();
			return true;
		}
		return false;
	}

	private bool ParseText(out int startPos, out int endPos, ref int outOrChars)
	{
		char[] chars = ps.chars;
		int charRefEndPos = ps.charPos;
		int num = 0;
		int num2 = -1;
		int num3 = outOrChars;
		char c;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)(c = chars[charRefEndPos])] & 0x40) != 0)
			{
				num3 |= c;
				charRefEndPos++;
				continue;
			}
			switch (c)
			{
			case '\t':
				charRefEndPos++;
				continue;
			case '\n':
				charRefEndPos++;
				OnNewLine(charRefEndPos);
				continue;
			case '\r':
				if (chars[charRefEndPos + 1] == '\n')
				{
					if (!ps.eolNormalized && parsingMode == ParsingMode.Full)
					{
						if (charRefEndPos - ps.charPos > 0)
						{
							if (num == 0)
							{
								num = 1;
								num2 = charRefEndPos;
							}
							else
							{
								ShiftBuffer(num2 + num, num2, charRefEndPos - num2 - num);
								num2 = charRefEndPos - num;
								num++;
							}
						}
						else
						{
							ps.charPos++;
						}
					}
					charRefEndPos += 2;
				}
				else
				{
					if (charRefEndPos + 1 >= ps.charsUsed && !ps.isEof)
					{
						goto IL_0366;
					}
					if (!ps.eolNormalized)
					{
						chars[charRefEndPos] = '\n';
					}
					charRefEndPos++;
				}
				OnNewLine(charRefEndPos);
				continue;
			case '&':
			{
				int num5;
				if ((num5 = ParseCharRefInline(charRefEndPos, out var charCount, out var entityType)) > 0)
				{
					if (num > 0)
					{
						ShiftBuffer(num2 + num, num2, charRefEndPos - num2 - num);
					}
					num2 = charRefEndPos - num;
					num += num5 - charRefEndPos - charCount;
					charRefEndPos = num5;
					if (!xmlCharType.IsWhiteSpace(chars[num5 - charCount]) || (v1Compat && entityType == EntityType.CharacterDec))
					{
						num3 |= 0xFF;
					}
					continue;
				}
				if (charRefEndPos > ps.charPos)
				{
					break;
				}
				switch (HandleEntityReference(isInAttributeValue: false, EntityExpandType.All, out charRefEndPos))
				{
				case EntityType.Unexpanded:
					break;
				case EntityType.CharacterDec:
					if (!v1Compat)
					{
						goto case EntityType.CharacterHex;
					}
					num3 |= 0xFF;
					goto IL_0255;
				case EntityType.CharacterHex:
				case EntityType.CharacterNamed:
					if (!xmlCharType.IsWhiteSpace(ps.chars[charRefEndPos - 1]))
					{
						num3 |= 0xFF;
					}
					goto IL_0255;
				default:
					{
						charRefEndPos = ps.charPos;
						goto IL_0255;
					}
					IL_0255:
					chars = ps.chars;
					continue;
				}
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.EntityReference;
				goto IL_0423;
			}
			case ']':
				if (ps.charsUsed - charRefEndPos >= 3 || ps.isEof)
				{
					if (chars[charRefEndPos + 1] == ']' && chars[charRefEndPos + 2] == '>')
					{
						Throw(charRefEndPos, "']]>' is not allowed in character data.");
					}
					num3 |= 0x5D;
					charRefEndPos++;
					continue;
				}
				goto IL_0366;
			default:
				if (charRefEndPos != ps.charsUsed)
				{
					char c2 = chars[charRefEndPos];
					if (XmlCharType.IsHighSurrogate(c2))
					{
						if (charRefEndPos + 1 == ps.charsUsed)
						{
							goto IL_0366;
						}
						charRefEndPos++;
						if (XmlCharType.IsLowSurrogate(chars[charRefEndPos]))
						{
							charRefEndPos++;
							num3 |= c2;
							continue;
						}
					}
					int num4 = charRefEndPos - ps.charPos;
					if (ZeroEndingStream(charRefEndPos))
					{
						chars = ps.chars;
						charRefEndPos = ps.charPos + num4;
						break;
					}
					ThrowInvalidChar(ps.chars, ps.charsUsed, ps.charPos + num4);
				}
				goto IL_0366;
			case '<':
				break;
				IL_0366:
				if (charRefEndPos > ps.charPos)
				{
					break;
				}
				if (ReadData() == 0)
				{
					if (ps.charsUsed - ps.charPos <= 0)
					{
						if (InEntity)
						{
							if (!HandleEntityEnd(checkEntityNesting: true))
							{
								goto IL_0406;
							}
							nextParsingFunction = parsingFunction;
							parsingFunction = ParsingFunction.ReportEndEntity;
						}
						goto IL_0423;
					}
					if (ps.chars[ps.charPos] != '\r' && ps.chars[ps.charPos] != ']')
					{
						Throw("Unexpected end of file has occurred.");
					}
				}
				goto IL_0406;
				IL_0406:
				charRefEndPos = ps.charPos;
				chars = ps.chars;
				continue;
				IL_0423:
				startPos = (endPos = charRefEndPos);
				return true;
			}
			break;
		}
		if (parsingMode == ParsingMode.Full && num > 0)
		{
			ShiftBuffer(num2 + num, num2, charRefEndPos - num2 - num);
		}
		startPos = ps.charPos;
		endPos = charRefEndPos - num;
		ps.charPos = charRefEndPos;
		outOrChars = num3;
		return c == '<';
	}

	private void FinishPartialValue()
	{
		curNode.CopyTo(readValueOffset, stringBuilder);
		int outOrChars = 0;
		int startPos;
		int endPos;
		while (!ParseText(out startPos, out endPos, ref outOrChars))
		{
			stringBuilder.Append(ps.chars, startPos, endPos - startPos);
		}
		stringBuilder.Append(ps.chars, startPos, endPos - startPos);
		curNode.SetValue(stringBuilder.ToString());
		stringBuilder.Length = 0;
	}

	private void FinishOtherValueIterator()
	{
		switch (parsingFunction)
		{
		case ParsingFunction.InReadValueChunk:
			if (incReadState == IncrementalReadState.ReadValueChunk_OnPartialValue)
			{
				FinishPartialValue();
				incReadState = IncrementalReadState.ReadValueChunk_OnCachedValue;
			}
			else if (readValueOffset > 0)
			{
				curNode.SetValue(curNode.StringValue.Substring(readValueOffset));
				readValueOffset = 0;
			}
			break;
		case ParsingFunction.InReadContentAsBinary:
		case ParsingFunction.InReadElementContentAsBinary:
			switch (incReadState)
			{
			case IncrementalReadState.ReadContentAsBinary_OnPartialValue:
				FinishPartialValue();
				incReadState = IncrementalReadState.ReadContentAsBinary_OnCachedValue;
				break;
			case IncrementalReadState.ReadContentAsBinary_OnCachedValue:
				if (readValueOffset > 0)
				{
					curNode.SetValue(curNode.StringValue.Substring(readValueOffset));
					readValueOffset = 0;
				}
				break;
			case IncrementalReadState.ReadContentAsBinary_End:
				curNode.SetValue(string.Empty);
				break;
			}
			break;
		case ParsingFunction.InReadAttributeValue:
			break;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SkipPartialTextValue()
	{
		int outOrChars = 0;
		parsingFunction = nextParsingFunction;
		int startPos;
		int endPos;
		while (!ParseText(out startPos, out endPos, ref outOrChars))
		{
		}
	}

	private void FinishReadValueChunk()
	{
		readValueOffset = 0;
		if (incReadState == IncrementalReadState.ReadValueChunk_OnPartialValue)
		{
			SkipPartialTextValue();
			return;
		}
		parsingFunction = nextParsingFunction;
		nextParsingFunction = nextNextParsingFunction;
	}

	private void FinishReadContentAsBinary()
	{
		readValueOffset = 0;
		if (incReadState == IncrementalReadState.ReadContentAsBinary_OnPartialValue)
		{
			SkipPartialTextValue();
		}
		else
		{
			parsingFunction = nextParsingFunction;
			nextParsingFunction = nextNextParsingFunction;
		}
		if (incReadState != IncrementalReadState.ReadContentAsBinary_End)
		{
			while (MoveToNextContentNode(moveIfOnContentNode: true))
			{
			}
		}
	}

	private void FinishReadElementContentAsBinary()
	{
		FinishReadContentAsBinary();
		if (curNode.type != XmlNodeType.EndElement)
		{
			Throw("'{0}' is an invalid XmlNodeType.", curNode.type.ToString());
		}
		outerReader.Read();
	}

	private bool ParseRootLevelWhitespace()
	{
		XmlNodeType whitespaceType = GetWhitespaceType();
		if (whitespaceType == XmlNodeType.None)
		{
			EatWhitespaces(null);
			if (ps.chars[ps.charPos] == '<' || ps.charsUsed - ps.charPos == 0 || ZeroEndingStream(ps.charPos))
			{
				return false;
			}
		}
		else
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			EatWhitespaces(stringBuilder);
			if (ps.chars[ps.charPos] == '<' || ps.charsUsed - ps.charPos == 0 || ZeroEndingStream(ps.charPos))
			{
				if (stringBuilder.Length > 0)
				{
					curNode.SetValueNode(whitespaceType, stringBuilder.ToString());
					stringBuilder.Length = 0;
					return true;
				}
				return false;
			}
		}
		if (xmlCharType.IsCharData(ps.chars[ps.charPos]))
		{
			Throw("Data at the root level is invalid.");
		}
		else
		{
			ThrowInvalidChar(ps.chars, ps.charsUsed, ps.charPos);
		}
		return false;
	}

	private void ParseEntityReference()
	{
		ps.charPos++;
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		curNode.SetNamedNode(XmlNodeType.EntityReference, ParseEntityName());
	}

	private EntityType HandleEntityReference(bool isInAttributeValue, EntityExpandType expandType, out int charRefEndPos)
	{
		if (ps.charPos + 1 == ps.charsUsed && ReadData() == 0)
		{
			Throw("Unexpected end of file has occurred.");
		}
		if (ps.chars[ps.charPos + 1] == '#')
		{
			charRefEndPos = ParseNumericCharRef(expandType != EntityExpandType.OnlyGeneral, null, out var entityType);
			return entityType;
		}
		charRefEndPos = ParseNamedCharRef(expandType != EntityExpandType.OnlyGeneral, null);
		if (charRefEndPos >= 0)
		{
			return EntityType.CharacterNamed;
		}
		if (expandType == EntityExpandType.OnlyCharacter || (entityHandling != EntityHandling.ExpandEntities && (!isInAttributeValue || !validatingReaderCompatFlag)))
		{
			return EntityType.Unexpanded;
		}
		ps.charPos++;
		int linePos = ps.LinePos;
		int num;
		try
		{
			num = ParseName();
		}
		catch (XmlException)
		{
			Throw("An error occurred while parsing EntityName.", ps.LineNo, linePos);
			return EntityType.Skipped;
		}
		if (ps.chars[num] != ';')
		{
			ThrowUnexpectedToken(num, ";");
		}
		int linePos2 = ps.LinePos;
		string name = nameTable.Add(ps.chars, ps.charPos, num - ps.charPos);
		ps.charPos = num + 1;
		charRefEndPos = -1;
		EntityType result = HandleGeneralEntityReference(name, isInAttributeValue, pushFakeEntityIfNullResolver: false, linePos2);
		reportedBaseUri = ps.baseUriStr;
		reportedEncoding = ps.encoding;
		return result;
	}

	private EntityType HandleGeneralEntityReference(string name, bool isInAttributeValue, bool pushFakeEntityIfNullResolver, int entityStartLinePos)
	{
		IDtdEntityInfo dtdEntityInfo = null;
		if (dtdInfo == null && fragmentParserContext != null && fragmentParserContext.HasDtdInfo && dtdProcessing == DtdProcessing.Parse)
		{
			ParseDtdFromParserContext();
		}
		if (dtdInfo == null || (dtdEntityInfo = dtdInfo.LookupEntity(name)) == null)
		{
			if (disableUndeclaredEntityCheck)
			{
				dtdEntityInfo = new SchemaEntity(new XmlQualifiedName(name), isParameter: false)
				{
					Text = string.Empty
				};
			}
			else
			{
				Throw("Reference to undeclared entity '{0}'.", name, ps.LineNo, entityStartLinePos);
			}
		}
		if (dtdEntityInfo.IsUnparsedEntity)
		{
			if (disableUndeclaredEntityCheck)
			{
				dtdEntityInfo = new SchemaEntity(new XmlQualifiedName(name), isParameter: false)
				{
					Text = string.Empty
				};
			}
			else
			{
				Throw("Reference to unparsed entity '{0}'.", name, ps.LineNo, entityStartLinePos);
			}
		}
		if (standalone && dtdEntityInfo.IsDeclaredInExternal)
		{
			Throw("Standalone document declaration must have a value of 'no' because an external entity '{0}' is referenced.", dtdEntityInfo.Name, ps.LineNo, entityStartLinePos);
		}
		if (dtdEntityInfo.IsExternal)
		{
			if (isInAttributeValue)
			{
				Throw("External entity '{0}' reference cannot appear in the attribute value.", name, ps.LineNo, entityStartLinePos);
				return EntityType.Skipped;
			}
			if (parsingMode == ParsingMode.SkipContent)
			{
				return EntityType.Skipped;
			}
			if (IsResolverNull)
			{
				if (pushFakeEntityIfNullResolver)
				{
					PushExternalEntity(dtdEntityInfo);
					curNode.entityId = ps.entityId;
					return EntityType.FakeExpanded;
				}
				return EntityType.Skipped;
			}
			PushExternalEntity(dtdEntityInfo);
			curNode.entityId = ps.entityId;
			if (!isInAttributeValue || !validatingReaderCompatFlag)
			{
				return EntityType.Expanded;
			}
			return EntityType.ExpandedInAttribute;
		}
		if (parsingMode == ParsingMode.SkipContent)
		{
			return EntityType.Skipped;
		}
		PushInternalEntity(dtdEntityInfo);
		curNode.entityId = ps.entityId;
		if (!isInAttributeValue || !validatingReaderCompatFlag)
		{
			return EntityType.Expanded;
		}
		return EntityType.ExpandedInAttribute;
	}

	private bool HandleEntityEnd(bool checkEntityNesting)
	{
		if (parsingStatesStackTop == -1)
		{
			Throw("An internal error has occurred.");
		}
		if (ps.entityResolvedManually)
		{
			index--;
			if (checkEntityNesting && ps.entityId != nodes[index].entityId)
			{
				Throw("Incomplete entity contents.");
			}
			lastEntity = ps.entity;
			PopEntity();
			return true;
		}
		if (checkEntityNesting && ps.entityId != nodes[index].entityId)
		{
			Throw("Incomplete entity contents.");
		}
		PopEntity();
		reportedEncoding = ps.encoding;
		reportedBaseUri = ps.baseUriStr;
		return false;
	}

	private void SetupEndEntityNodeInContent()
	{
		reportedEncoding = ps.encoding;
		reportedBaseUri = ps.baseUriStr;
		curNode = nodes[index];
		curNode.SetNamedNode(XmlNodeType.EndEntity, lastEntity.Name);
		curNode.lineInfo.Set(ps.lineNo, ps.LinePos - 1);
		if (index == 0 && parsingFunction == ParsingFunction.ElementContent)
		{
			parsingFunction = ParsingFunction.DocumentContent;
		}
	}

	private void SetupEndEntityNodeInAttribute()
	{
		curNode = nodes[index + attrCount + 1];
		curNode.lineInfo.linePos += curNode.localName.Length;
		curNode.type = XmlNodeType.EndEntity;
	}

	private bool ParsePI()
	{
		return ParsePI(null);
	}

	private bool ParsePI(StringBuilder piInDtdStringBuilder)
	{
		if (parsingMode == ParsingMode.Full)
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		}
		int num = ParseName();
		string text = nameTable.Add(ps.chars, ps.charPos, num - ps.charPos);
		if (string.Compare(text, "xml", StringComparison.OrdinalIgnoreCase) == 0)
		{
			Throw(text.Equals("xml") ? "Unexpected XML declaration. The XML declaration must be the first node in the document, and no white space characters are allowed to appear before it." : "'{0}' is an invalid name for processing instructions.", text);
		}
		ps.charPos = num;
		if (piInDtdStringBuilder == null)
		{
			if (!ignorePIs && parsingMode == ParsingMode.Full)
			{
				curNode.SetNamedNode(XmlNodeType.ProcessingInstruction, text);
			}
		}
		else
		{
			piInDtdStringBuilder.Append(text);
		}
		char c = ps.chars[ps.charPos];
		if (EatWhitespaces(piInDtdStringBuilder) == 0)
		{
			if (ps.charsUsed - ps.charPos < 2)
			{
				ReadData();
			}
			if (c != '?' || ps.chars[ps.charPos + 1] != '>')
			{
				Throw("The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(ps.chars, ps.charsUsed, ps.charPos));
			}
		}
		if (ParsePIValue(out var outStartPos, out var outEndPos))
		{
			if (piInDtdStringBuilder == null)
			{
				if (ignorePIs)
				{
					return false;
				}
				if (parsingMode == ParsingMode.Full)
				{
					curNode.SetValue(ps.chars, outStartPos, outEndPos - outStartPos);
				}
			}
			else
			{
				piInDtdStringBuilder.Append(ps.chars, outStartPos, outEndPos - outStartPos);
			}
		}
		else
		{
			StringBuilder stringBuilder;
			if (piInDtdStringBuilder == null)
			{
				if (ignorePIs || parsingMode != ParsingMode.Full)
				{
					while (!ParsePIValue(out outStartPos, out outEndPos))
					{
					}
					return false;
				}
				stringBuilder = this.stringBuilder;
			}
			else
			{
				stringBuilder = piInDtdStringBuilder;
			}
			do
			{
				stringBuilder.Append(ps.chars, outStartPos, outEndPos - outStartPos);
			}
			while (!ParsePIValue(out outStartPos, out outEndPos));
			stringBuilder.Append(ps.chars, outStartPos, outEndPos - outStartPos);
			if (piInDtdStringBuilder == null)
			{
				curNode.SetValue(this.stringBuilder.ToString());
				this.stringBuilder.Length = 0;
			}
		}
		return true;
	}

	private bool ParsePIValue(out int outStartPos, out int outEndPos)
	{
		if (ps.charsUsed - ps.charPos < 2 && ReadData() == 0)
		{
			Throw(ps.charsUsed, "Unexpected end of file while parsing {0} has occurred.", "PI");
		}
		int num = ps.charPos;
		char[] chars = ps.chars;
		int num2 = 0;
		int num3 = -1;
		while (true)
		{
			char c;
			if ((xmlCharType.charProperties[(uint)(c = chars[num])] & 0x40) != 0 && c != '?')
			{
				num++;
				continue;
			}
			switch (chars[num])
			{
			case '?':
				if (chars[num + 1] == '>')
				{
					if (num2 > 0)
					{
						ShiftBuffer(num3 + num2, num3, num - num3 - num2);
						outEndPos = num - num2;
					}
					else
					{
						outEndPos = num;
					}
					outStartPos = ps.charPos;
					ps.charPos = num + 2;
					return true;
				}
				if (num + 1 != ps.charsUsed)
				{
					num++;
					continue;
				}
				break;
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					if (!ps.eolNormalized && parsingMode == ParsingMode.Full)
					{
						if (num - ps.charPos > 0)
						{
							if (num2 == 0)
							{
								num2 = 1;
								num3 = num;
							}
							else
							{
								ShiftBuffer(num3 + num2, num3, num - num3 - num2);
								num3 = num - num2;
								num2++;
							}
						}
						else
						{
							ps.charPos++;
						}
					}
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (!ps.eolNormalized)
					{
						chars[num] = '\n';
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '\t':
			case '&':
			case '<':
			case ']':
				num++;
				continue;
			default:
				if (num == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[num]))
				{
					if (num + 1 == ps.charsUsed)
					{
						break;
					}
					num++;
					if (XmlCharType.IsLowSurrogate(chars[num]))
					{
						num++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, num);
				continue;
			}
			break;
		}
		if (num2 > 0)
		{
			ShiftBuffer(num3 + num2, num3, num - num3 - num2);
			outEndPos = num - num2;
		}
		else
		{
			outEndPos = num;
		}
		outStartPos = ps.charPos;
		ps.charPos = num;
		return false;
	}

	private bool ParseComment()
	{
		if (ignoreComments)
		{
			ParsingMode parsingMode = this.parsingMode;
			this.parsingMode = ParsingMode.SkipNode;
			ParseCDataOrComment(XmlNodeType.Comment);
			this.parsingMode = parsingMode;
			return false;
		}
		ParseCDataOrComment(XmlNodeType.Comment);
		return true;
	}

	private void ParseCData()
	{
		ParseCDataOrComment(XmlNodeType.CDATA);
	}

	private void ParseCDataOrComment(XmlNodeType type)
	{
		int outStartPos;
		int outEndPos;
		if (parsingMode == ParsingMode.Full)
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			if (ParseCDataOrComment(type, out outStartPos, out outEndPos))
			{
				curNode.SetValueNode(type, ps.chars, outStartPos, outEndPos - outStartPos);
				return;
			}
			do
			{
				stringBuilder.Append(ps.chars, outStartPos, outEndPos - outStartPos);
			}
			while (!ParseCDataOrComment(type, out outStartPos, out outEndPos));
			stringBuilder.Append(ps.chars, outStartPos, outEndPos - outStartPos);
			curNode.SetValueNode(type, stringBuilder.ToString());
			stringBuilder.Length = 0;
		}
		else
		{
			while (!ParseCDataOrComment(type, out outStartPos, out outEndPos))
			{
			}
		}
	}

	private bool ParseCDataOrComment(XmlNodeType type, out int outStartPos, out int outEndPos)
	{
		if (ps.charsUsed - ps.charPos < 3 && ReadData() == 0)
		{
			Throw("Unexpected end of file while parsing {0} has occurred.", (type == XmlNodeType.Comment) ? "Comment" : "CDATA");
		}
		int num = ps.charPos;
		char[] chars = ps.chars;
		int num2 = 0;
		int num3 = -1;
		char c = ((type == XmlNodeType.Comment) ? '-' : ']');
		while (true)
		{
			char c2;
			if ((xmlCharType.charProperties[(uint)(c2 = chars[num])] & 0x40) != 0 && c2 != c)
			{
				num++;
				continue;
			}
			if (chars[num] == c)
			{
				if (chars[num + 1] == c)
				{
					if (chars[num + 2] == '>')
					{
						if (num2 > 0)
						{
							ShiftBuffer(num3 + num2, num3, num - num3 - num2);
							outEndPos = num - num2;
						}
						else
						{
							outEndPos = num;
						}
						outStartPos = ps.charPos;
						ps.charPos = num + 3;
						return true;
					}
					if (num + 2 == ps.charsUsed)
					{
						break;
					}
					if (type == XmlNodeType.Comment)
					{
						Throw(num, "An XML comment cannot contain '--', and '-' cannot be the last character.");
					}
				}
				else if (num + 1 == ps.charsUsed)
				{
					break;
				}
				num++;
				continue;
			}
			switch (chars[num])
			{
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					if (!ps.eolNormalized && parsingMode == ParsingMode.Full)
					{
						if (num - ps.charPos > 0)
						{
							if (num2 == 0)
							{
								num2 = 1;
								num3 = num;
							}
							else
							{
								ShiftBuffer(num3 + num2, num3, num - num3 - num2);
								num3 = num - num2;
								num2++;
							}
						}
						else
						{
							ps.charPos++;
						}
					}
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (!ps.eolNormalized)
					{
						chars[num] = '\n';
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '\t':
			case '&':
			case '<':
			case ']':
				num++;
				continue;
			default:
				if (num == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[num]))
				{
					if (num + 1 == ps.charsUsed)
					{
						break;
					}
					num++;
					if (XmlCharType.IsLowSurrogate(chars[num]))
					{
						num++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, num);
				break;
			}
			break;
		}
		if (num2 > 0)
		{
			ShiftBuffer(num3 + num2, num3, num - num3 - num2);
			outEndPos = num - num2;
		}
		else
		{
			outEndPos = num;
		}
		outStartPos = ps.charPos;
		ps.charPos = num;
		return false;
	}

	private bool ParseDoctypeDecl()
	{
		if (dtdProcessing == DtdProcessing.Prohibit)
		{
			ThrowWithoutLineInfo(v1Compat ? "DTD is prohibited in this XML document." : "For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.");
		}
		while (ps.charsUsed - ps.charPos < 8)
		{
			if (ReadData() == 0)
			{
				Throw("Unexpected end of file while parsing {0} has occurred.", "DOCTYPE");
			}
		}
		if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 7, "DOCTYPE"))
		{
			ThrowUnexpectedToken((!rootElementParsed && dtdInfo == null) ? "DOCTYPE" : "<!--");
		}
		if (!xmlCharType.IsWhiteSpace(ps.chars[ps.charPos + 7]))
		{
			ThrowExpectingWhitespace(ps.charPos + 7);
		}
		if (dtdInfo != null)
		{
			Throw(ps.charPos - 2, "Cannot have multiple DTDs.");
		}
		if (rootElementParsed)
		{
			Throw(ps.charPos - 2, "DTD must be defined before the document root element.");
		}
		ps.charPos += 8;
		EatWhitespaces(null);
		if (dtdProcessing == DtdProcessing.Parse)
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			ParseDtd();
			nextParsingFunction = parsingFunction;
			parsingFunction = ParsingFunction.ResetAttributesRootLevel;
			return true;
		}
		SkipDtd();
		return false;
	}

	private void ParseDtd()
	{
		IDtdParser dtdParser = DtdParser.Create();
		dtdInfo = dtdParser.ParseInternalDtd(new DtdParserProxy(this), saveInternalSubset: true);
		if ((validatingReaderCompatFlag || !v1Compat) && (dtdInfo.HasDefaultAttributes || dtdInfo.HasNonCDataAttributes))
		{
			addDefaultAttributesAndNormalize = true;
		}
		curNode.SetNamedNode(XmlNodeType.DocumentType, dtdInfo.Name.ToString(), string.Empty, null);
		curNode.SetValue(dtdInfo.InternalDtdSubset);
	}

	private void SkipDtd()
	{
		int colonPos;
		int charPos = ParseQName(out colonPos);
		ps.charPos = charPos;
		EatWhitespaces(null);
		if (ps.chars[ps.charPos] == 'P')
		{
			while (ps.charsUsed - ps.charPos < 6)
			{
				if (ReadData() == 0)
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 6, "PUBLIC"))
			{
				ThrowUnexpectedToken("PUBLIC");
			}
			ps.charPos += 6;
			if (EatWhitespaces(null) == 0)
			{
				ThrowExpectingWhitespace(ps.charPos);
			}
			SkipPublicOrSystemIdLiteral();
			if (EatWhitespaces(null) == 0)
			{
				ThrowExpectingWhitespace(ps.charPos);
			}
			SkipPublicOrSystemIdLiteral();
			EatWhitespaces(null);
		}
		else if (ps.chars[ps.charPos] == 'S')
		{
			while (ps.charsUsed - ps.charPos < 6)
			{
				if (ReadData() == 0)
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 6, "SYSTEM"))
			{
				ThrowUnexpectedToken("SYSTEM");
			}
			ps.charPos += 6;
			if (EatWhitespaces(null) == 0)
			{
				ThrowExpectingWhitespace(ps.charPos);
			}
			SkipPublicOrSystemIdLiteral();
			EatWhitespaces(null);
		}
		else if (ps.chars[ps.charPos] != '[' && ps.chars[ps.charPos] != '>')
		{
			Throw("Expecting external ID, '[' or '>'.");
		}
		if (ps.chars[ps.charPos] == '[')
		{
			ps.charPos++;
			SkipUntil(']', recognizeLiterals: true);
			EatWhitespaces(null);
			if (ps.chars[ps.charPos] != '>')
			{
				ThrowUnexpectedToken(">");
			}
		}
		else if (ps.chars[ps.charPos] == '>')
		{
			curNode.SetValue(string.Empty);
		}
		else
		{
			Throw("Expecting an internal subset or the end of the DOCTYPE declaration.");
		}
		ps.charPos++;
	}

	private void SkipPublicOrSystemIdLiteral()
	{
		char c = ps.chars[ps.charPos];
		if (c != '"' && c != '\'')
		{
			ThrowUnexpectedToken("\"", "'");
		}
		ps.charPos++;
		SkipUntil(c, recognizeLiterals: false);
	}

	private void SkipUntil(char stopChar, bool recognizeLiterals)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		char c = '"';
		char[] chars = ps.chars;
		int num = ps.charPos;
		while (true)
		{
			char c2;
			if ((xmlCharType.charProperties[(uint)(c2 = chars[num])] & 0x80) != 0 && chars[num] != stopChar && c2 != '-' && c2 != '?')
			{
				num++;
				continue;
			}
			if (c2 == stopChar && !flag)
			{
				break;
			}
			ps.charPos = num;
			switch (c2)
			{
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '<':
				if (chars[num + 1] == '?')
				{
					if (recognizeLiterals && !flag && !flag2)
					{
						flag3 = true;
						num += 2;
						continue;
					}
				}
				else if (chars[num + 1] == '!')
				{
					if (num + 3 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (chars[num + 2] == '-' && chars[num + 3] == '-' && recognizeLiterals && !flag && !flag3)
					{
						flag2 = true;
						num += 4;
						continue;
					}
				}
				else if (num + 1 >= ps.charsUsed && !ps.isEof)
				{
					break;
				}
				num++;
				continue;
			case '-':
				if (flag2)
				{
					if (num + 2 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (chars[num + 1] == '-' && chars[num + 2] == '>')
					{
						flag2 = false;
						num += 2;
						continue;
					}
				}
				num++;
				continue;
			case '?':
				if (flag3)
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (chars[num + 1] == '>')
					{
						flag3 = false;
						num++;
						continue;
					}
				}
				num++;
				continue;
			case '\t':
			case '&':
			case '>':
			case ']':
				num++;
				continue;
			case '"':
			case '\'':
				if (flag)
				{
					if (c == c2)
					{
						flag = false;
					}
				}
				else if (recognizeLiterals && !flag2 && !flag3)
				{
					flag = true;
					c = c2;
				}
				num++;
				continue;
			default:
				if (num == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[num]))
				{
					if (num + 1 == ps.charsUsed)
					{
						break;
					}
					num++;
					if (XmlCharType.IsLowSurrogate(chars[num]))
					{
						num++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, num);
				break;
			}
			if (ReadData() == 0)
			{
				if (ps.charsUsed - ps.charPos > 0)
				{
					if (ps.chars[ps.charPos] != '\r')
					{
						Throw("Unexpected end of file has occurred.");
					}
				}
				else
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			chars = ps.chars;
			num = ps.charPos;
		}
		ps.charPos = num + 1;
	}

	private int EatWhitespaces(StringBuilder sb)
	{
		int num = ps.charPos;
		int num2 = 0;
		char[] chars = ps.chars;
		while (true)
		{
			switch (chars[num])
			{
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					int num4 = num - ps.charPos;
					if (sb != null && !ps.eolNormalized)
					{
						if (num4 > 0)
						{
							sb.Append(chars, ps.charPos, num4);
							num2 += num4;
						}
						ps.charPos = num + 1;
					}
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (!ps.eolNormalized)
					{
						chars[num] = '\n';
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '\t':
			case ' ':
				num++;
				continue;
			default:
				if (num != ps.charsUsed)
				{
					int num3 = num - ps.charPos;
					if (num3 > 0)
					{
						sb?.Append(ps.chars, ps.charPos, num3);
						ps.charPos = num;
						num2 += num3;
					}
					return num2;
				}
				break;
			}
			int num5 = num - ps.charPos;
			if (num5 > 0)
			{
				sb?.Append(ps.chars, ps.charPos, num5);
				ps.charPos = num;
				num2 += num5;
			}
			if (ReadData() == 0)
			{
				if (ps.charsUsed - ps.charPos == 0)
				{
					break;
				}
				if (ps.chars[ps.charPos] != '\r')
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			num = ps.charPos;
			chars = ps.chars;
		}
		return num2;
	}

	private int ParseCharRefInline(int startPos, out int charCount, out EntityType entityType)
	{
		if (ps.chars[startPos + 1] == '#')
		{
			return ParseNumericCharRefInline(startPos, expand: true, null, out charCount, out entityType);
		}
		charCount = 1;
		entityType = EntityType.CharacterNamed;
		return ParseNamedCharRefInline(startPos, expand: true, null);
	}

	private int ParseNumericCharRef(bool expand, StringBuilder internalSubsetBuilder, out EntityType entityType)
	{
		int num;
		int charCount;
		while ((num = ParseNumericCharRefInline(ps.charPos, expand, internalSubsetBuilder, out charCount, out entityType)) == -2)
		{
			if (ReadData() == 0)
			{
				Throw("Unexpected end of file while parsing {0} has occurred.");
			}
		}
		if (expand)
		{
			ps.charPos = num - charCount;
		}
		return num;
	}

	private int ParseNumericCharRefInline(int startPos, bool expand, StringBuilder internalSubsetBuilder, out int charCount, out EntityType entityType)
	{
		int num = 0;
		string res = null;
		char[] chars = ps.chars;
		int i = startPos + 2;
		charCount = 0;
		int num2 = 0;
		try
		{
			if (chars[i] == 'x')
			{
				i++;
				num2 = i;
				res = "Invalid syntax for a hexadecimal numeric entity reference.";
				while (true)
				{
					char c = chars[i];
					checked
					{
						if (c >= '0' && c <= '9')
						{
							num = num * 16 + c - 48;
						}
						else if (c >= 'a' && c <= 'f')
						{
							num = num * 16 + 10 + c - 97;
						}
						else
						{
							if (c < 'A' || c > 'F')
							{
								break;
							}
							num = num * 16 + 10 + c - 65;
						}
					}
					i++;
				}
				entityType = EntityType.CharacterHex;
			}
			else
			{
				if (i >= ps.charsUsed)
				{
					entityType = EntityType.Skipped;
					return -2;
				}
				num2 = i;
				res = "Invalid syntax for a decimal numeric entity reference.";
				for (; chars[i] >= '0' && chars[i] <= '9'; i++)
				{
					num = checked(num * 10 + chars[i] - 48);
				}
				entityType = EntityType.CharacterDec;
			}
		}
		catch (OverflowException innerException)
		{
			ps.charPos = i;
			entityType = EntityType.Skipped;
			Throw("Invalid value of a character entity reference.", (string)null, (Exception)innerException);
		}
		if (chars[i] != ';' || num2 == i)
		{
			if (i == ps.charsUsed)
			{
				return -2;
			}
			Throw(i, res);
		}
		if (num <= 65535)
		{
			char c2 = (char)num;
			if (!xmlCharType.IsCharData(c2) && ((v1Compat && normalize) || (!v1Compat && checkCharacters)))
			{
				Throw((ps.chars[startPos + 2] == 'x') ? (startPos + 3) : (startPos + 2), "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(c2, '\0'));
			}
			if (expand)
			{
				internalSubsetBuilder?.Append(ps.chars, ps.charPos, i - ps.charPos + 1);
				chars[i] = c2;
			}
			charCount = 1;
			return i + 1;
		}
		XmlCharType.SplitSurrogateChar(num, out var lowChar, out var highChar);
		if (normalize && (!XmlCharType.IsHighSurrogate(highChar) || !XmlCharType.IsLowSurrogate(lowChar)))
		{
			Throw((ps.chars[startPos + 2] == 'x') ? (startPos + 3) : (startPos + 2), "'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(highChar, lowChar));
		}
		if (expand)
		{
			internalSubsetBuilder?.Append(ps.chars, ps.charPos, i - ps.charPos + 1);
			chars[i - 1] = highChar;
			chars[i] = lowChar;
		}
		charCount = 2;
		return i + 1;
	}

	private int ParseNamedCharRef(bool expand, StringBuilder internalSubsetBuilder)
	{
		do
		{
			int num;
			switch (num = ParseNamedCharRefInline(ps.charPos, expand, internalSubsetBuilder))
			{
			case -1:
				return -1;
			case -2:
				continue;
			}
			if (expand)
			{
				ps.charPos = num - 1;
			}
			return num;
		}
		while (ReadData() != 0);
		return -1;
	}

	private int ParseNamedCharRefInline(int startPos, bool expand, StringBuilder internalSubsetBuilder)
	{
		int num = startPos + 1;
		char[] chars = ps.chars;
		char c = chars[num];
		char c2;
		if ((uint)c <= 103u)
		{
			if (c != 'a')
			{
				if (c != 'g')
				{
					goto IL_0170;
				}
				if (ps.charsUsed - num >= 3)
				{
					if (chars[num + 1] == 't' && chars[num + 2] == ';')
					{
						num += 3;
						c2 = '>';
						goto IL_0175;
					}
					return -1;
				}
			}
			else
			{
				num++;
				if (chars[num] == 'm')
				{
					if (ps.charsUsed - num >= 3)
					{
						if (chars[num + 1] == 'p' && chars[num + 2] == ';')
						{
							num += 3;
							c2 = '&';
							goto IL_0175;
						}
						return -1;
					}
				}
				else if (chars[num] == 'p')
				{
					if (ps.charsUsed - num >= 4)
					{
						if (chars[num + 1] == 'o' && chars[num + 2] == 's' && chars[num + 3] == ';')
						{
							num += 4;
							c2 = '\'';
							goto IL_0175;
						}
						return -1;
					}
				}
				else if (num < ps.charsUsed)
				{
					return -1;
				}
			}
			goto IL_0172;
		}
		if (c != 'l')
		{
			if (c != 'q')
			{
				goto IL_0170;
			}
			if (ps.charsUsed - num < 5)
			{
				goto IL_0172;
			}
			if (chars[num + 1] != 'u' || chars[num + 2] != 'o' || chars[num + 3] != 't' || chars[num + 4] != ';')
			{
				return -1;
			}
			num += 5;
			c2 = '"';
		}
		else
		{
			if (ps.charsUsed - num < 3)
			{
				goto IL_0172;
			}
			if (chars[num + 1] != 't' || chars[num + 2] != ';')
			{
				return -1;
			}
			num += 3;
			c2 = '<';
		}
		goto IL_0175;
		IL_0170:
		return -1;
		IL_0172:
		return -2;
		IL_0175:
		if (expand)
		{
			internalSubsetBuilder?.Append(ps.chars, ps.charPos, num - ps.charPos);
			ps.chars[num - 1] = c2;
		}
		return num;
	}

	private int ParseName()
	{
		int colonPos;
		return ParseQName(isQName: false, 0, out colonPos);
	}

	private int ParseQName(out int colonPos)
	{
		return ParseQName(isQName: true, 0, out colonPos);
	}

	private int ParseQName(bool isQName, int startOffset, out int colonPos)
	{
		int num = -1;
		int pos = ps.charPos + startOffset;
		while (true)
		{
			char[] chars = ps.chars;
			if ((xmlCharType.charProperties[(uint)chars[pos]] & 4) != 0)
			{
				pos++;
			}
			else
			{
				if (pos + 1 >= ps.charsUsed)
				{
					if (ReadDataInName(ref pos))
					{
						continue;
					}
					Throw(pos, "Unexpected end of file while parsing {0} has occurred.", "Name");
				}
				if (chars[pos] != ':' || supportNamespaces)
				{
					Throw(pos, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(chars, ps.charsUsed, pos));
				}
			}
			while (true)
			{
				if ((xmlCharType.charProperties[(uint)chars[pos]] & 8) != 0)
				{
					pos++;
					continue;
				}
				if (chars[pos] == ':')
				{
					if (supportNamespaces)
					{
						break;
					}
					num = pos - ps.charPos;
					pos++;
					continue;
				}
				if (pos == ps.charsUsed)
				{
					if (ReadDataInName(ref pos))
					{
						chars = ps.chars;
						continue;
					}
					Throw(pos, "Unexpected end of file while parsing {0} has occurred.", "Name");
				}
				colonPos = ((num == -1) ? (-1) : (ps.charPos + num));
				return pos;
			}
			if (num != -1 || !isQName)
			{
				Throw(pos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
			}
			num = pos - ps.charPos;
			pos++;
		}
	}

	private bool ReadDataInName(ref int pos)
	{
		int num = pos - ps.charPos;
		bool result = ReadData() != 0;
		pos = ps.charPos + num;
		return result;
	}

	private string ParseEntityName()
	{
		int num;
		try
		{
			num = ParseName();
		}
		catch (XmlException)
		{
			Throw("An error occurred while parsing EntityName.");
			return null;
		}
		if (ps.chars[num] != ';')
		{
			Throw("An error occurred while parsing EntityName.");
		}
		string result = nameTable.Add(ps.chars, ps.charPos, num - ps.charPos);
		ps.charPos = num + 1;
		return result;
	}

	private NodeData AddNode(int nodeIndex, int nodeDepth)
	{
		NodeData nodeData = nodes[nodeIndex];
		if (nodeData != null)
		{
			nodeData.depth = nodeDepth;
			return nodeData;
		}
		return AllocNode(nodeIndex, nodeDepth);
	}

	private NodeData AllocNode(int nodeIndex, int nodeDepth)
	{
		if (nodeIndex >= nodes.Length - 1)
		{
			NodeData[] destinationArray = new NodeData[nodes.Length * 2];
			Array.Copy(nodes, 0, destinationArray, 0, nodes.Length);
			nodes = destinationArray;
		}
		NodeData nodeData = nodes[nodeIndex];
		if (nodeData == null)
		{
			nodeData = new NodeData();
			nodes[nodeIndex] = nodeData;
		}
		nodeData.depth = nodeDepth;
		return nodeData;
	}

	private NodeData AddAttributeNoChecks(string name, int attrDepth)
	{
		NodeData nodeData = AddNode(index + attrCount + 1, attrDepth);
		nodeData.SetNamedNode(XmlNodeType.Attribute, nameTable.Add(name));
		attrCount++;
		return nodeData;
	}

	private NodeData AddAttribute(int endNamePos, int colonPos)
	{
		if (colonPos == -1 || !supportNamespaces)
		{
			string text = nameTable.Add(ps.chars, ps.charPos, endNamePos - ps.charPos);
			return AddAttribute(text, string.Empty, text);
		}
		attrNeedNamespaceLookup = true;
		int charPos = ps.charPos;
		int num = colonPos - charPos;
		if (num != lastPrefix.Length || !XmlConvert.StrEqual(ps.chars, charPos, num, lastPrefix))
		{
			return AddAttribute(prefix: lastPrefix = nameTable.Add(ps.chars, charPos, num), localName: nameTable.Add(ps.chars, colonPos + 1, endNamePos - colonPos - 1), nameWPrefix: null);
		}
		return AddAttribute(nameTable.Add(ps.chars, colonPos + 1, endNamePos - colonPos - 1), lastPrefix, null);
	}

	private NodeData AddAttribute(string localName, string prefix, string nameWPrefix)
	{
		NodeData nodeData = AddNode(index + attrCount + 1, index + 1);
		nodeData.SetNamedNode(XmlNodeType.Attribute, localName, prefix, nameWPrefix);
		int num = 1 << (localName[0] & 0x1F);
		if ((attrHashtable & num) == 0)
		{
			attrHashtable |= num;
		}
		else if (attrDuplWalkCount < 250)
		{
			attrDuplWalkCount++;
			for (int i = index + 1; i < index + attrCount + 1; i++)
			{
				if (Ref.Equal(nodes[i].localName, nodeData.localName))
				{
					attrDuplWalkCount = 250;
					break;
				}
			}
		}
		attrCount++;
		return nodeData;
	}

	private void PopElementContext()
	{
		namespaceManager.PopScope();
		if (curNode.xmlContextPushed)
		{
			PopXmlContext();
		}
	}

	private void OnNewLine(int pos)
	{
		ps.lineNo++;
		ps.lineStartPos = pos - 1;
	}

	private void OnEof()
	{
		curNode = nodes[0];
		curNode.Clear(XmlNodeType.None);
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		parsingFunction = ParsingFunction.Eof;
		readState = ReadState.EndOfFile;
		reportedEncoding = null;
	}

	private string LookupNamespace(NodeData node)
	{
		string text = namespaceManager.LookupNamespace(node.prefix);
		if (text != null)
		{
			return text;
		}
		Throw("'{0}' is an undeclared prefix.", node.prefix, node.LineNo, node.LinePos);
		return null;
	}

	private void AddNamespace(string prefix, string uri, NodeData attr)
	{
		if (uri == "http://www.w3.org/2000/xmlns/")
		{
			if (Ref.Equal(prefix, XmlNs))
			{
				Throw("Prefix \"xmlns\" is reserved for use by XML.", attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
			}
			else
			{
				Throw("Prefix '{0}' cannot be mapped to namespace name reserved for \"xml\" or \"xmlns\".", prefix, attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
			}
		}
		else if (uri == "http://www.w3.org/XML/1998/namespace" && !Ref.Equal(prefix, Xml) && !v1Compat)
		{
			Throw("Prefix '{0}' cannot be mapped to namespace name reserved for \"xml\" or \"xmlns\".", prefix, attr.lineInfo2.lineNo, attr.lineInfo2.linePos);
		}
		if (uri.Length == 0 && prefix.Length > 0)
		{
			Throw("Invalid namespace declaration.", attr.lineInfo.lineNo, attr.lineInfo.linePos);
		}
		try
		{
			namespaceManager.AddNamespace(prefix, uri);
		}
		catch (ArgumentException e)
		{
			ReThrow(e, attr.lineInfo.lineNo, attr.lineInfo.linePos);
		}
	}

	private void ResetAttributes()
	{
		if (fullAttrCleanup)
		{
			FullAttributeCleanup();
		}
		curAttrIndex = -1;
		attrCount = 0;
		attrHashtable = 0;
		attrDuplWalkCount = 0;
	}

	private void FullAttributeCleanup()
	{
		for (int i = index + 1; i < index + attrCount + 1; i++)
		{
			NodeData obj = nodes[i];
			obj.nextAttrValueChunk = null;
			obj.IsDefaultAttribute = false;
		}
		fullAttrCleanup = false;
	}

	private void PushXmlContext()
	{
		xmlContext = new XmlContext(xmlContext);
		curNode.xmlContextPushed = true;
	}

	private void PopXmlContext()
	{
		xmlContext = xmlContext.previousContext;
		curNode.xmlContextPushed = false;
	}

	private XmlNodeType GetWhitespaceType()
	{
		if (whitespaceHandling != WhitespaceHandling.None)
		{
			if (xmlContext.xmlSpace == XmlSpace.Preserve)
			{
				return XmlNodeType.SignificantWhitespace;
			}
			if (whitespaceHandling == WhitespaceHandling.All)
			{
				return XmlNodeType.Whitespace;
			}
		}
		return XmlNodeType.None;
	}

	private XmlNodeType GetTextNodeType(int orChars)
	{
		if (orChars > 32)
		{
			return XmlNodeType.Text;
		}
		return GetWhitespaceType();
	}

	private void PushExternalEntityOrSubset(string publicId, string systemId, Uri baseUri, string entityName)
	{
		Uri uri;
		if (!string.IsNullOrEmpty(publicId))
		{
			try
			{
				uri = xmlResolver.ResolveUri(baseUri, publicId);
				if (OpenAndPush(uri))
				{
					return;
				}
			}
			catch (Exception)
			{
			}
		}
		uri = xmlResolver.ResolveUri(baseUri, systemId);
		try
		{
			if (OpenAndPush(uri))
			{
				return;
			}
		}
		catch (Exception ex2)
		{
			if (v1Compat)
			{
				throw;
			}
			string message = ex2.Message;
			Throw(new XmlException((entityName == null) ? "An error has occurred while opening external DTD '{0}': {1}" : "An error has occurred while opening external entity '{0}': {1}", new string[2]
			{
				uri.ToString(),
				message
			}, ex2, 0, 0));
		}
		if (entityName == null)
		{
			ThrowWithoutLineInfo("Cannot resolve external DTD subset - public ID = '{0}', system ID = '{1}'.", new string[2]
			{
				(publicId != null) ? publicId : string.Empty,
				systemId
			}, null);
		}
		else
		{
			Throw((dtdProcessing == DtdProcessing.Ignore) ? "Cannot resolve entity reference '{0}' because the DTD has been ignored. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method." : "Cannot resolve entity reference '{0}'.", entityName);
		}
	}

	private bool OpenAndPush(Uri uri)
	{
		if (xmlResolver.SupportsType(uri, typeof(TextReader)))
		{
			TextReader textReader = (TextReader)xmlResolver.GetEntity(uri, null, typeof(TextReader));
			if (textReader == null)
			{
				return false;
			}
			PushParsingState();
			InitTextReaderInput(uri.ToString(), uri, textReader);
		}
		else
		{
			Stream stream = (Stream)xmlResolver.GetEntity(uri, null, typeof(Stream));
			if (stream == null)
			{
				return false;
			}
			PushParsingState();
			InitStreamInput(uri, stream, null);
		}
		return true;
	}

	private bool PushExternalEntity(IDtdEntityInfo entity)
	{
		if (!IsResolverNull)
		{
			Uri baseUri = null;
			if (!string.IsNullOrEmpty(entity.BaseUriString))
			{
				baseUri = xmlResolver.ResolveUri(null, entity.BaseUriString);
			}
			PushExternalEntityOrSubset(entity.PublicId, entity.SystemId, baseUri, entity.Name);
			RegisterEntity(entity);
			int charPos = ps.charPos;
			if (v1Compat)
			{
				EatWhitespaces(null);
			}
			if (!ParseXmlDeclaration(isTextDecl: true))
			{
				ps.charPos = charPos;
			}
			return true;
		}
		Encoding encoding = ps.encoding;
		PushParsingState();
		InitStringInput(entity.SystemId, encoding, string.Empty);
		RegisterEntity(entity);
		RegisterConsumedCharacters(0L, inEntityReference: true);
		return false;
	}

	private void PushInternalEntity(IDtdEntityInfo entity)
	{
		Encoding encoding = ps.encoding;
		PushParsingState();
		InitStringInput((entity.DeclaredUriString != null) ? entity.DeclaredUriString : string.Empty, encoding, entity.Text ?? string.Empty);
		RegisterEntity(entity);
		ps.lineNo = entity.LineNumber;
		ps.lineStartPos = -entity.LinePosition - 1;
		ps.eolNormalized = true;
		RegisterConsumedCharacters(entity.Text.Length, inEntityReference: true);
	}

	private void PopEntity()
	{
		if (ps.stream != null)
		{
			ps.stream.Close();
		}
		UnregisterEntity();
		PopParsingState();
		curNode.entityId = ps.entityId;
	}

	private void RegisterEntity(IDtdEntityInfo entity)
	{
		if (currentEntities != null && currentEntities.ContainsKey(entity))
		{
			Throw(entity.IsParameterEntity ? "Parameter entity '{0}' references itself." : "General entity '{0}' references itself.", entity.Name, parsingStatesStack[parsingStatesStackTop].LineNo, parsingStatesStack[parsingStatesStackTop].LinePos);
		}
		ps.entity = entity;
		ps.entityId = nextEntityId++;
		if (entity != null)
		{
			if (currentEntities == null)
			{
				currentEntities = new Dictionary<IDtdEntityInfo, IDtdEntityInfo>();
			}
			currentEntities.Add(entity, entity);
		}
	}

	private void UnregisterEntity()
	{
		if (ps.entity != null)
		{
			currentEntities.Remove(ps.entity);
		}
	}

	private void PushParsingState()
	{
		if (parsingStatesStack == null)
		{
			parsingStatesStack = new ParsingState[2];
		}
		else if (parsingStatesStackTop + 1 == parsingStatesStack.Length)
		{
			ParsingState[] destinationArray = new ParsingState[parsingStatesStack.Length * 2];
			Array.Copy(parsingStatesStack, 0, destinationArray, 0, parsingStatesStack.Length);
			parsingStatesStack = destinationArray;
		}
		parsingStatesStackTop++;
		parsingStatesStack[parsingStatesStackTop] = ps;
		ps.Clear();
	}

	private void PopParsingState()
	{
		ps.Close(closeInput: true);
		ps = parsingStatesStack[parsingStatesStackTop--];
	}

	private void InitIncrementalRead(IncrementalReadDecoder decoder)
	{
		ResetAttributes();
		decoder.Reset();
		incReadDecoder = decoder;
		incReadState = IncrementalReadState.Text;
		incReadDepth = 1;
		incReadLeftStartPos = ps.charPos;
		incReadLeftEndPos = ps.charPos;
		incReadLineInfo.Set(ps.LineNo, ps.LinePos);
		parsingFunction = ParsingFunction.InIncrementalRead;
	}

	private int IncrementalRead(Array array, int index, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException((incReadDecoder is IncrementalReadCharsDecoder) ? "buffer" : "array");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException((incReadDecoder is IncrementalReadCharsDecoder) ? "count" : "len");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException((incReadDecoder is IncrementalReadCharsDecoder) ? "index" : "offset");
		}
		if (array.Length - index < count)
		{
			throw new ArgumentException((incReadDecoder is IncrementalReadCharsDecoder) ? "count" : "len");
		}
		if (count == 0)
		{
			return 0;
		}
		curNode.lineInfo = incReadLineInfo;
		incReadDecoder.SetNextOutputBuffer(array, index, count);
		IncrementalRead();
		return incReadDecoder.DecodedCount;
	}

	private int IncrementalRead()
	{
		int num = 0;
		int num3;
		while (true)
		{
			int num2 = incReadLeftEndPos - incReadLeftStartPos;
			if (num2 > 0)
			{
				try
				{
					num3 = incReadDecoder.Decode(ps.chars, incReadLeftStartPos, num2);
				}
				catch (XmlException e)
				{
					ReThrow(e, incReadLineInfo.lineNo, incReadLineInfo.linePos);
					return 0;
				}
				if (num3 < num2)
				{
					incReadLeftStartPos += num3;
					incReadLineInfo.linePos += num3;
					return num3;
				}
				incReadLeftStartPos = 0;
				incReadLeftEndPos = 0;
				incReadLineInfo.linePos += num3;
				if (incReadDecoder.IsFull)
				{
					break;
				}
			}
			int outStartPos = 0;
			int outEndPos = 0;
			while (true)
			{
				switch (incReadState)
				{
				case IncrementalReadState.PI:
					if (ParsePIValue(out outStartPos, out outEndPos))
					{
						ps.charPos -= 2;
						incReadState = IncrementalReadState.Text;
					}
					break;
				case IncrementalReadState.Comment:
					if (ParseCDataOrComment(XmlNodeType.Comment, out outStartPos, out outEndPos))
					{
						ps.charPos -= 3;
						incReadState = IncrementalReadState.Text;
					}
					break;
				case IncrementalReadState.CDATA:
					if (ParseCDataOrComment(XmlNodeType.CDATA, out outStartPos, out outEndPos))
					{
						ps.charPos -= 3;
						incReadState = IncrementalReadState.Text;
					}
					break;
				case IncrementalReadState.EndElement:
					parsingFunction = ParsingFunction.PopElementContext;
					nextParsingFunction = ((index <= 0 && fragmentType == XmlNodeType.Document) ? ParsingFunction.DocumentContent : ParsingFunction.ElementContent);
					outerReader.Read();
					incReadState = IncrementalReadState.End;
					goto case IncrementalReadState.End;
				case IncrementalReadState.End:
					return num;
				case IncrementalReadState.ReadData:
					if (ReadData() == 0)
					{
						ThrowUnclosedElements();
					}
					incReadState = IncrementalReadState.Text;
					outStartPos = ps.charPos;
					outEndPos = outStartPos;
					goto default;
				default:
				{
					char[] chars = ps.chars;
					outStartPos = ps.charPos;
					outEndPos = outStartPos;
					while (true)
					{
						incReadLineInfo.Set(ps.LineNo, ps.LinePos);
						if (incReadState == IncrementalReadState.Attributes)
						{
							char c;
							while ((xmlCharType.charProperties[(uint)(c = chars[outEndPos])] & 0x80) != 0 && c != '/')
							{
								outEndPos++;
							}
						}
						else
						{
							char c;
							while ((xmlCharType.charProperties[(uint)(c = chars[outEndPos])] & 0x80) != 0)
							{
								outEndPos++;
							}
						}
						if (chars[outEndPos] == '&' || chars[outEndPos] == '\t')
						{
							outEndPos++;
							continue;
						}
						if (outEndPos - outStartPos <= 0)
						{
							char c2 = chars[outEndPos];
							if ((uint)c2 <= 34u)
							{
								if (c2 == '\n')
								{
									outEndPos++;
									OnNewLine(outEndPos);
									continue;
								}
								if (c2 == '\r')
								{
									if (chars[outEndPos + 1] == '\n')
									{
										outEndPos += 2;
									}
									else
									{
										if (outEndPos + 1 >= ps.charsUsed)
										{
											goto IL_0691;
										}
										outEndPos++;
									}
									OnNewLine(outEndPos);
									continue;
								}
								if (c2 == '"')
								{
									goto IL_062f;
								}
							}
							else if ((uint)c2 <= 47u)
							{
								if (c2 == '\'')
								{
									goto IL_062f;
								}
								if (c2 == '/')
								{
									if (incReadState == IncrementalReadState.Attributes)
									{
										if (ps.charsUsed - outEndPos < 2)
										{
											goto IL_0691;
										}
										if (chars[outEndPos + 1] == '>')
										{
											incReadState = IncrementalReadState.Text;
											incReadDepth--;
										}
									}
									outEndPos++;
									continue;
								}
							}
							else
							{
								if (c2 == '<')
								{
									if (incReadState != IncrementalReadState.Text)
									{
										outEndPos++;
										continue;
									}
									if (ps.charsUsed - outEndPos < 2)
									{
										goto IL_0691;
									}
									char c3 = chars[outEndPos + 1];
									if (c3 != '!')
									{
										switch (c3)
										{
										case '?':
											outEndPos += 2;
											incReadState = IncrementalReadState.PI;
											break;
										case '/':
										{
											int colonPos2;
											int num5 = ParseQName(isQName: true, 2, out colonPos2);
											if (XmlConvert.StrEqual(chars, ps.charPos + 2, num5 - ps.charPos - 2, curNode.GetNameWPrefix(nameTable)) && (ps.chars[num5] == '>' || xmlCharType.IsWhiteSpace(ps.chars[num5])))
											{
												if (--incReadDepth > 0)
												{
													outEndPos = num5 + 1;
													continue;
												}
												ps.charPos = num5;
												if (xmlCharType.IsWhiteSpace(ps.chars[num5]))
												{
													EatWhitespaces(null);
												}
												if (ps.chars[ps.charPos] != '>')
												{
													ThrowUnexpectedToken(">");
												}
												goto end_IL_00bb;
											}
											outEndPos = num5;
											outStartPos = ps.charPos;
											chars = ps.chars;
											continue;
										}
										default:
										{
											int colonPos;
											int num4 = ParseQName(isQName: true, 1, out colonPos);
											if (XmlConvert.StrEqual(ps.chars, ps.charPos + 1, num4 - ps.charPos - 1, curNode.localName) && (ps.chars[num4] == '>' || ps.chars[num4] == '/' || xmlCharType.IsWhiteSpace(ps.chars[num4])))
											{
												incReadDepth++;
												incReadState = IncrementalReadState.Attributes;
												outEndPos = num4;
												break;
											}
											outEndPos = num4;
											outStartPos = ps.charPos;
											chars = ps.chars;
											continue;
										}
										}
									}
									else
									{
										if (ps.charsUsed - outEndPos < 4)
										{
											goto IL_0691;
										}
										if (chars[outEndPos + 2] == '-' && chars[outEndPos + 3] == '-')
										{
											outEndPos += 4;
											incReadState = IncrementalReadState.Comment;
										}
										else
										{
											if (ps.charsUsed - outEndPos < 9)
											{
												goto IL_0691;
											}
											if (!XmlConvert.StrEqual(chars, outEndPos + 2, 7, "[CDATA["))
											{
												continue;
											}
											outEndPos += 9;
											incReadState = IncrementalReadState.CDATA;
										}
									}
									goto IL_0698;
								}
								if (c2 == '>')
								{
									if (incReadState == IncrementalReadState.Attributes)
									{
										incReadState = IncrementalReadState.Text;
									}
									outEndPos++;
									continue;
								}
							}
							if (outEndPos != ps.charsUsed)
							{
								outEndPos++;
								continue;
							}
							goto IL_0691;
						}
						goto IL_0698;
						IL_0698:
						ps.charPos = outEndPos;
						break;
						IL_0691:
						incReadState = IncrementalReadState.ReadData;
						goto IL_0698;
						IL_062f:
						switch (incReadState)
						{
						case IncrementalReadState.AttributeValue:
							if (chars[outEndPos] == curNode.quoteChar)
							{
								incReadState = IncrementalReadState.Attributes;
							}
							break;
						case IncrementalReadState.Attributes:
							curNode.quoteChar = chars[outEndPos];
							incReadState = IncrementalReadState.AttributeValue;
							break;
						}
						outEndPos++;
					}
					break;
				}
				}
				int num6 = outEndPos - outStartPos;
				if (num6 > 0)
				{
					int num7;
					try
					{
						num7 = incReadDecoder.Decode(ps.chars, outStartPos, num6);
					}
					catch (XmlException e2)
					{
						ReThrow(e2, incReadLineInfo.lineNo, incReadLineInfo.linePos);
						return 0;
					}
					num += num7;
					if (incReadDecoder.IsFull)
					{
						incReadLeftStartPos = outStartPos + num7;
						incReadLeftEndPos = outEndPos;
						incReadLineInfo.linePos += num7;
						return num;
					}
				}
				continue;
				end_IL_00bb:
				break;
			}
			ps.charPos++;
			incReadState = IncrementalReadState.EndElement;
		}
		return num3;
	}

	private void FinishIncrementalRead()
	{
		incReadDecoder = new IncrementalReadDummyDecoder();
		IncrementalRead();
		incReadDecoder = null;
	}

	private bool ParseFragmentAttribute()
	{
		if (curNode.type == XmlNodeType.None)
		{
			curNode.type = XmlNodeType.Attribute;
			curAttrIndex = 0;
			ParseAttributeValueSlow(ps.charPos, ' ', curNode);
		}
		else
		{
			parsingFunction = ParsingFunction.InReadAttributeValue;
		}
		if (ReadAttributeValue())
		{
			parsingFunction = ParsingFunction.FragmentAttribute;
			return true;
		}
		OnEof();
		return false;
	}

	private bool ParseAttributeValueChunk()
	{
		char[] chars = ps.chars;
		int charRefEndPos = ps.charPos;
		curNode = AddNode(index + attrCount + 1, index + 2);
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		if (emptyEntityInAttributeResolved)
		{
			curNode.SetValueNode(XmlNodeType.Text, string.Empty);
			emptyEntityInAttributeResolved = false;
			return true;
		}
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[charRefEndPos]] & 0x80) != 0)
			{
				charRefEndPos++;
				continue;
			}
			switch (chars[charRefEndPos])
			{
			case '\r':
				charRefEndPos++;
				continue;
			case '\t':
			case '\n':
				if (normalize)
				{
					chars[charRefEndPos] = ' ';
				}
				charRefEndPos++;
				continue;
			case '"':
			case '\'':
			case '>':
				charRefEndPos++;
				continue;
			case '<':
				Throw(charRefEndPos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
				goto IL_0271;
			case '&':
			{
				if (charRefEndPos - ps.charPos > 0)
				{
					stringBuilder.Append(chars, ps.charPos, charRefEndPos - ps.charPos);
				}
				ps.charPos = charRefEndPos;
				EntityType entityType = HandleEntityReference(isInAttributeValue: true, EntityExpandType.OnlyCharacter, out charRefEndPos);
				if ((uint)entityType > 2u)
				{
					if (entityType == EntityType.Unexpanded)
					{
						if (stringBuilder.Length == 0)
						{
							curNode.lineInfo.linePos++;
							ps.charPos++;
							curNode.SetNamedNode(XmlNodeType.EntityReference, ParseEntityName());
							return true;
						}
						break;
					}
				}
				else
				{
					chars = ps.chars;
					if (normalize && xmlCharType.IsWhiteSpace(chars[ps.charPos]) && charRefEndPos - ps.charPos == 1)
					{
						chars[ps.charPos] = ' ';
					}
				}
				chars = ps.chars;
				continue;
			}
			default:
				{
					if (charRefEndPos != ps.charsUsed)
					{
						if (XmlCharType.IsHighSurrogate(chars[charRefEndPos]))
						{
							if (charRefEndPos + 1 == ps.charsUsed)
							{
								goto IL_0271;
							}
							charRefEndPos++;
							if (XmlCharType.IsLowSurrogate(chars[charRefEndPos]))
							{
								charRefEndPos++;
								continue;
							}
						}
						ThrowInvalidChar(chars, ps.charsUsed, charRefEndPos);
					}
					goto IL_0271;
				}
				IL_0271:
				if (charRefEndPos - ps.charPos > 0)
				{
					stringBuilder.Append(chars, ps.charPos, charRefEndPos - ps.charPos);
					ps.charPos = charRefEndPos;
				}
				if (ReadData() == 0)
				{
					if (stringBuilder.Length > 0)
					{
						break;
					}
					if (HandleEntityEnd(checkEntityNesting: false))
					{
						SetupEndEntityNodeInAttribute();
						return true;
					}
				}
				charRefEndPos = ps.charPos;
				chars = ps.chars;
				continue;
			}
			break;
		}
		if (charRefEndPos - ps.charPos > 0)
		{
			stringBuilder.Append(chars, ps.charPos, charRefEndPos - ps.charPos);
			ps.charPos = charRefEndPos;
		}
		curNode.SetValueNode(XmlNodeType.Text, stringBuilder.ToString());
		stringBuilder.Length = 0;
		return true;
	}

	private void ParseXmlDeclarationFragment()
	{
		try
		{
			ParseXmlDeclaration(isTextDecl: false);
		}
		catch (XmlException ex)
		{
			ReThrow(ex, ex.LineNumber, ex.LinePosition - 6);
		}
	}

	private void ThrowUnexpectedToken(int pos, string expectedToken)
	{
		ThrowUnexpectedToken(pos, expectedToken, null);
	}

	private void ThrowUnexpectedToken(string expectedToken1)
	{
		ThrowUnexpectedToken(expectedToken1, null);
	}

	private void ThrowUnexpectedToken(int pos, string expectedToken1, string expectedToken2)
	{
		ps.charPos = pos;
		ThrowUnexpectedToken(expectedToken1, expectedToken2);
	}

	private void ThrowUnexpectedToken(string expectedToken1, string expectedToken2)
	{
		string text = ParseUnexpectedToken();
		if (text == null)
		{
			Throw("Unexpected end of file has occurred.");
		}
		if (expectedToken2 != null)
		{
			Throw("'{0}' is an unexpected token. The expected token is '{1}' or '{2}'.", new string[3] { text, expectedToken1, expectedToken2 });
		}
		else
		{
			Throw("'{0}' is an unexpected token. The expected token is '{1}'.", new string[2] { text, expectedToken1 });
		}
	}

	private string ParseUnexpectedToken(int pos)
	{
		ps.charPos = pos;
		return ParseUnexpectedToken();
	}

	private string ParseUnexpectedToken()
	{
		if (ps.charPos == ps.charsUsed)
		{
			return null;
		}
		if (xmlCharType.IsNCNameSingleChar(ps.chars[ps.charPos]))
		{
			int i;
			for (i = ps.charPos + 1; xmlCharType.IsNCNameSingleChar(ps.chars[i]); i++)
			{
			}
			return new string(ps.chars, ps.charPos, i - ps.charPos);
		}
		return new string(ps.chars, ps.charPos, 1);
	}

	private void ThrowExpectingWhitespace(int pos)
	{
		string text = ParseUnexpectedToken(pos);
		if (text == null)
		{
			Throw(pos, "Unexpected end of file has occurred.");
		}
		else
		{
			Throw(pos, "'{0}' is an unexpected token. Expecting white space.", text);
		}
	}

	private int GetIndexOfAttributeWithoutPrefix(string name)
	{
		name = nameTable.Get(name);
		if (name == null)
		{
			return -1;
		}
		for (int i = index + 1; i < index + attrCount + 1; i++)
		{
			if (Ref.Equal(nodes[i].localName, name) && nodes[i].prefix.Length == 0)
			{
				return i;
			}
		}
		return -1;
	}

	private int GetIndexOfAttributeWithPrefix(string name)
	{
		name = nameTable.Add(name);
		if (name == null)
		{
			return -1;
		}
		for (int i = index + 1; i < index + attrCount + 1; i++)
		{
			if (Ref.Equal(nodes[i].GetNameWPrefix(nameTable), name))
			{
				return i;
			}
		}
		return -1;
	}

	private bool ZeroEndingStream(int pos)
	{
		if (v1Compat && pos == ps.charsUsed - 1 && ps.chars[pos] == '\0' && ReadData() == 0 && ps.isStreamEof)
		{
			ps.charsUsed--;
			return true;
		}
		return false;
	}

	private void ParseDtdFromParserContext()
	{
		IDtdParser dtdParser = DtdParser.Create();
		dtdInfo = dtdParser.ParseFreeFloatingDtd(fragmentParserContext.BaseURI, fragmentParserContext.DocTypeName, fragmentParserContext.PublicId, fragmentParserContext.SystemId, fragmentParserContext.InternalSubset, new DtdParserProxy(this));
		if ((validatingReaderCompatFlag || !v1Compat) && (dtdInfo.HasDefaultAttributes || dtdInfo.HasNonCDataAttributes))
		{
			addDefaultAttributesAndNormalize = true;
		}
	}

	private bool InitReadContentAsBinary()
	{
		if (parsingFunction == ParsingFunction.InReadValueChunk)
		{
			throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
		}
		if (parsingFunction == ParsingFunction.InIncrementalRead)
		{
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadChars, ReadBase64, and ReadBinHex."));
		}
		if (!XmlReader.IsTextualNode(curNode.type) && !MoveToNextContentNode(moveIfOnContentNode: false))
		{
			return false;
		}
		SetupReadContentAsBinaryState(ParsingFunction.InReadContentAsBinary);
		incReadLineInfo.Set(curNode.LineNo, curNode.LinePos);
		return true;
	}

	private bool InitReadElementContentAsBinary()
	{
		bool isEmptyElement = curNode.IsEmptyElement;
		outerReader.Read();
		if (isEmptyElement)
		{
			return false;
		}
		if (!MoveToNextContentNode(moveIfOnContentNode: false))
		{
			if (curNode.type != XmlNodeType.EndElement)
			{
				Throw("'{0}' is an invalid XmlNodeType.", curNode.type.ToString());
			}
			outerReader.Read();
			return false;
		}
		SetupReadContentAsBinaryState(ParsingFunction.InReadElementContentAsBinary);
		incReadLineInfo.Set(curNode.LineNo, curNode.LinePos);
		return true;
	}

	private bool MoveToNextContentNode(bool moveIfOnContentNode)
	{
		do
		{
			switch (curNode.type)
			{
			case XmlNodeType.Attribute:
				return !moveIfOnContentNode;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if (!moveIfOnContentNode)
				{
					return true;
				}
				break;
			case XmlNodeType.EntityReference:
				outerReader.ResolveEntity();
				break;
			default:
				return false;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.EndEntity:
				break;
			}
			moveIfOnContentNode = false;
		}
		while (outerReader.Read());
		return false;
	}

	private void SetupReadContentAsBinaryState(ParsingFunction inReadBinaryFunction)
	{
		if (parsingFunction == ParsingFunction.PartialTextValue)
		{
			incReadState = IncrementalReadState.ReadContentAsBinary_OnPartialValue;
		}
		else
		{
			incReadState = IncrementalReadState.ReadContentAsBinary_OnCachedValue;
			nextNextParsingFunction = nextParsingFunction;
			nextParsingFunction = parsingFunction;
		}
		readValueOffset = 0;
		parsingFunction = inReadBinaryFunction;
	}

	private void SetupFromParserContext(XmlParserContext context, XmlReaderSettings settings)
	{
		XmlNameTable xmlNameTable = settings.NameTable;
		nameTableFromSettings = xmlNameTable != null;
		if (context.NamespaceManager != null)
		{
			if (xmlNameTable != null && xmlNameTable != context.NamespaceManager.NameTable)
			{
				throw new XmlException("XmlReaderSettings.XmlNameTable must be the same name table as in XmlParserContext.NameTable or XmlParserContext.NamespaceManager.NameTable, or it must be null.");
			}
			namespaceManager = context.NamespaceManager;
			xmlContext.defaultNamespace = namespaceManager.LookupNamespace(string.Empty);
			xmlNameTable = namespaceManager.NameTable;
		}
		else if (context.NameTable != null)
		{
			if (xmlNameTable != null && xmlNameTable != context.NameTable)
			{
				throw new XmlException("XmlReaderSettings.XmlNameTable must be the same name table as in XmlParserContext.NameTable or XmlParserContext.NamespaceManager.NameTable, or it must be null.", string.Empty);
			}
			xmlNameTable = context.NameTable;
		}
		else if (xmlNameTable == null)
		{
			xmlNameTable = new NameTable();
		}
		nameTable = xmlNameTable;
		if (namespaceManager == null)
		{
			namespaceManager = new XmlNamespaceManager(xmlNameTable);
		}
		xmlContext.xmlSpace = context.XmlSpace;
		xmlContext.xmlLang = context.XmlLang;
	}

	internal void SetDtdInfo(IDtdInfo newDtdInfo)
	{
		dtdInfo = newDtdInfo;
		if (dtdInfo != null && (validatingReaderCompatFlag || !v1Compat) && (dtdInfo.HasDefaultAttributes || dtdInfo.HasNonCDataAttributes))
		{
			addDefaultAttributesAndNormalize = true;
		}
	}

	internal void ChangeCurrentNodeType(XmlNodeType newNodeType)
	{
		curNode.type = newNodeType;
	}

	internal XmlResolver GetResolver()
	{
		if (IsResolverNull)
		{
			return null;
		}
		return xmlResolver;
	}

	private bool AddDefaultAttributeDtd(IDtdDefaultAttributeInfo defAttrInfo, bool definedInDtd, NodeData[] nameSortedNodeData)
	{
		if (defAttrInfo.Prefix.Length > 0)
		{
			attrNeedNamespaceLookup = true;
		}
		string localName = defAttrInfo.LocalName;
		string prefix = defAttrInfo.Prefix;
		if (nameSortedNodeData != null)
		{
			if (Array.BinarySearch(nameSortedNodeData, defAttrInfo, DtdDefaultAttributeInfoToNodeDataComparer.Instance) >= 0)
			{
				return false;
			}
		}
		else
		{
			for (int i = index + 1; i < index + 1 + attrCount; i++)
			{
				if ((object)nodes[i].localName == localName && (object)nodes[i].prefix == prefix)
				{
					return false;
				}
			}
		}
		NodeData nodeData = AddDefaultAttributeInternal(defAttrInfo.LocalName, null, defAttrInfo.Prefix, defAttrInfo.DefaultValueExpanded, defAttrInfo.LineNumber, defAttrInfo.LinePosition, defAttrInfo.ValueLineNumber, defAttrInfo.ValueLinePosition, defAttrInfo.IsXmlAttribute);
		if (DtdValidation)
		{
			if (onDefaultAttributeUse != null)
			{
				onDefaultAttributeUse(defAttrInfo, this);
			}
			nodeData.typedValue = defAttrInfo.DefaultValueTyped;
		}
		return nodeData != null;
	}

	internal bool AddDefaultAttributeNonDtd(SchemaAttDef attrDef)
	{
		string text = nameTable.Add(attrDef.Name.Name);
		string text2 = nameTable.Add(attrDef.Prefix);
		string text3 = nameTable.Add(attrDef.Name.Namespace);
		if (text2.Length == 0 && text3.Length > 0)
		{
			text2 = namespaceManager.LookupPrefix(text3);
			if (text2 == null)
			{
				text2 = string.Empty;
			}
		}
		for (int i = index + 1; i < index + 1 + attrCount; i++)
		{
			if ((object)nodes[i].localName == text && ((object)nodes[i].prefix == text2 || ((object)nodes[i].ns == text3 && text3 != null)))
			{
				return false;
			}
		}
		NodeData nodeData = AddDefaultAttributeInternal(text, text3, text2, attrDef.DefaultValueExpanded, attrDef.LineNumber, attrDef.LinePosition, attrDef.ValueLineNumber, attrDef.ValueLinePosition, attrDef.Reserved != SchemaAttDef.Reserve.None);
		nodeData.schemaType = ((attrDef.SchemaType == null) ? ((object)attrDef.Datatype) : ((object)attrDef.SchemaType));
		nodeData.typedValue = attrDef.DefaultValueTyped;
		return true;
	}

	private NodeData AddDefaultAttributeInternal(string localName, string ns, string prefix, string value, int lineNo, int linePos, int valueLineNo, int valueLinePos, bool isXmlAttribute)
	{
		NodeData nodeData = AddAttribute(localName, prefix, (prefix.Length > 0) ? null : localName);
		if (ns != null)
		{
			nodeData.ns = ns;
		}
		nodeData.SetValue(value);
		nodeData.IsDefaultAttribute = true;
		nodeData.lineInfo.Set(lineNo, linePos);
		nodeData.lineInfo2.Set(valueLineNo, valueLinePos);
		if (nodeData.prefix.Length == 0)
		{
			if (Ref.Equal(nodeData.localName, XmlNs))
			{
				OnDefaultNamespaceDecl(nodeData);
				if (!attrNeedNamespaceLookup && nodes[index].prefix.Length == 0)
				{
					nodes[index].ns = xmlContext.defaultNamespace;
				}
			}
		}
		else if (Ref.Equal(nodeData.prefix, XmlNs))
		{
			OnNamespaceDecl(nodeData);
			if (!attrNeedNamespaceLookup)
			{
				string localName2 = nodeData.localName;
				for (int i = index; i < index + attrCount + 1; i++)
				{
					if (nodes[i].prefix.Equals(localName2))
					{
						nodes[i].ns = namespaceManager.LookupNamespace(localName2);
					}
				}
			}
		}
		else if (isXmlAttribute)
		{
			OnXmlReservedAttribute(nodeData);
		}
		fullAttrCleanup = true;
		return nodeData;
	}

	private int ReadContentAsBinary(byte[] buffer, int index, int count)
	{
		if (incReadState == IncrementalReadState.ReadContentAsBinary_End)
		{
			return 0;
		}
		incReadDecoder.SetNextOutputBuffer(buffer, index, count);
		ParsingFunction inReadBinaryFunction;
		while (true)
		{
			int num = 0;
			try
			{
				num = curNode.CopyToBinary(incReadDecoder, readValueOffset);
			}
			catch (XmlException e)
			{
				curNode.AdjustLineInfo(readValueOffset, ps.eolNormalized, ref incReadLineInfo);
				ReThrow(e, incReadLineInfo.lineNo, incReadLineInfo.linePos);
			}
			readValueOffset += num;
			if (incReadDecoder.IsFull)
			{
				return incReadDecoder.DecodedCount;
			}
			if (incReadState == IncrementalReadState.ReadContentAsBinary_OnPartialValue)
			{
				curNode.SetValue(string.Empty);
				bool flag = false;
				int startPos = 0;
				int endPos = 0;
				while (!incReadDecoder.IsFull && !flag)
				{
					int outOrChars = 0;
					incReadLineInfo.Set(ps.LineNo, ps.LinePos);
					flag = ParseText(out startPos, out endPos, ref outOrChars);
					try
					{
						num = incReadDecoder.Decode(ps.chars, startPos, endPos - startPos);
					}
					catch (XmlException e2)
					{
						ReThrow(e2, incReadLineInfo.lineNo, incReadLineInfo.linePos);
					}
					startPos += num;
				}
				incReadState = (flag ? IncrementalReadState.ReadContentAsBinary_OnCachedValue : IncrementalReadState.ReadContentAsBinary_OnPartialValue);
				readValueOffset = 0;
				if (incReadDecoder.IsFull)
				{
					curNode.SetValue(ps.chars, startPos, endPos - startPos);
					AdjustLineInfo(ps.chars, startPos - num, startPos, ps.eolNormalized, ref incReadLineInfo);
					curNode.SetLineInfo(incReadLineInfo.lineNo, incReadLineInfo.linePos);
					return incReadDecoder.DecodedCount;
				}
			}
			inReadBinaryFunction = parsingFunction;
			parsingFunction = nextParsingFunction;
			nextParsingFunction = nextNextParsingFunction;
			if (!MoveToNextContentNode(moveIfOnContentNode: true))
			{
				break;
			}
			SetupReadContentAsBinaryState(inReadBinaryFunction);
			incReadLineInfo.Set(curNode.LineNo, curNode.LinePos);
		}
		SetupReadContentAsBinaryState(inReadBinaryFunction);
		incReadState = IncrementalReadState.ReadContentAsBinary_End;
		return incReadDecoder.DecodedCount;
	}

	private int ReadElementContentAsBinary(byte[] buffer, int index, int count)
	{
		if (count == 0)
		{
			return 0;
		}
		int num = ReadContentAsBinary(buffer, index, count);
		if (num > 0)
		{
			return num;
		}
		if (curNode.type != XmlNodeType.EndElement)
		{
			throw new XmlException("'{0}' is an invalid XmlNodeType.", curNode.type.ToString(), this);
		}
		parsingFunction = nextParsingFunction;
		nextParsingFunction = nextNextParsingFunction;
		outerReader.Read();
		return 0;
	}

	private void InitBase64Decoder()
	{
		if (base64Decoder == null)
		{
			base64Decoder = new Base64Decoder();
		}
		else
		{
			base64Decoder.Reset();
		}
		incReadDecoder = base64Decoder;
	}

	private void InitBinHexDecoder()
	{
		if (binHexDecoder == null)
		{
			binHexDecoder = new BinHexDecoder();
		}
		else
		{
			binHexDecoder.Reset();
		}
		incReadDecoder = binHexDecoder;
	}

	private bool UriEqual(Uri uri1, string uri1Str, string uri2Str, XmlResolver resolver)
	{
		if (resolver == null)
		{
			return uri1Str == uri2Str;
		}
		if (uri1 == null)
		{
			uri1 = resolver.ResolveUri(null, uri1Str);
		}
		Uri obj = resolver.ResolveUri(null, uri2Str);
		return uri1.Equals(obj);
	}

	private void RegisterConsumedCharacters(long characters, bool inEntityReference)
	{
		if (maxCharactersInDocument > 0)
		{
			long num = charactersInDocument + characters;
			if (num < charactersInDocument)
			{
				ThrowWithoutLineInfo("The input document has exceeded a limit set by {0}.", "MaxCharactersInDocument");
			}
			else
			{
				charactersInDocument = num;
			}
			if (charactersInDocument > maxCharactersInDocument)
			{
				ThrowWithoutLineInfo("The input document has exceeded a limit set by {0}.", "MaxCharactersInDocument");
			}
		}
		if (maxCharactersFromEntities > 0 && inEntityReference)
		{
			long num2 = charactersFromEntities + characters;
			if (num2 < charactersFromEntities)
			{
				ThrowWithoutLineInfo("The input document has exceeded a limit set by {0}.", "MaxCharactersFromEntities");
			}
			else
			{
				charactersFromEntities = num2;
			}
			if (charactersFromEntities > maxCharactersFromEntities)
			{
				ThrowWithoutLineInfo("The input document has exceeded a limit set by {0}.", "MaxCharactersFromEntities");
			}
		}
	}

	internal unsafe static void AdjustLineInfo(char[] chars, int startPos, int endPos, bool isNormalized, ref LineInfo lineInfo)
	{
		fixed (char* pChars = &chars[startPos])
		{
			AdjustLineInfo(pChars, endPos - startPos, isNormalized, ref lineInfo);
		}
	}

	internal unsafe static void AdjustLineInfo(string str, int startPos, int endPos, bool isNormalized, ref LineInfo lineInfo)
	{
		fixed (char* ptr = str)
		{
			AdjustLineInfo(ptr + startPos, endPos - startPos, isNormalized, ref lineInfo);
		}
	}

	internal unsafe static void AdjustLineInfo(char* pChars, int length, bool isNormalized, ref LineInfo lineInfo)
	{
		int num = -1;
		for (int i = 0; i < length; i++)
		{
			switch (pChars[i])
			{
			case '\n':
				lineInfo.lineNo++;
				num = i;
				break;
			case '\r':
				if (!isNormalized)
				{
					lineInfo.lineNo++;
					num = i;
					if (i + 1 < length && pChars[i + 1] == '\n')
					{
						i++;
						num++;
					}
				}
				break;
			}
		}
		if (num >= 0)
		{
			lineInfo.linePos = length - num;
		}
	}

	internal static string StripSpaces(string value)
	{
		int length = value.Length;
		if (length <= 0)
		{
			return string.Empty;
		}
		int num = 0;
		StringBuilder stringBuilder = null;
		while (value[num] == ' ')
		{
			num++;
			if (num == length)
			{
				return " ";
			}
		}
		int i;
		for (i = num; i < length; i++)
		{
			if (value[i] != ' ')
			{
				continue;
			}
			int j;
			for (j = i + 1; j < length && value[j] == ' '; j++)
			{
			}
			if (j == length)
			{
				if (stringBuilder == null)
				{
					return value.Substring(num, i - num);
				}
				stringBuilder.Append(value, num, i - num);
				return stringBuilder.ToString();
			}
			if (j > i + 1)
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length);
				}
				stringBuilder.Append(value, num, i - num + 1);
				num = j;
				i = j - 1;
			}
		}
		if (stringBuilder == null)
		{
			if (num != 0)
			{
				return value.Substring(num, length - num);
			}
			return value;
		}
		if (i > num)
		{
			stringBuilder.Append(value, num, i - num);
		}
		return stringBuilder.ToString();
	}

	internal static void StripSpaces(char[] value, int index, ref int len)
	{
		if (len <= 0)
		{
			return;
		}
		int num = index;
		int num2 = index + len;
		while (value[num] == ' ')
		{
			num++;
			if (num == num2)
			{
				len = 1;
				return;
			}
		}
		int num3 = num - index;
		for (int i = num; i < num2; i++)
		{
			char c;
			if ((c = value[i]) == ' ')
			{
				int j;
				for (j = i + 1; j < num2 && value[j] == ' '; j++)
				{
				}
				if (j == num2)
				{
					num3 += j - i;
					break;
				}
				if (j > i + 1)
				{
					num3 += j - i - 1;
					i = j - 1;
				}
			}
			value[i - num3] = c;
		}
		len -= num3;
	}

	internal static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
	{
		Buffer.BlockCopy(src, srcOffset * 2, dst, dstOffset * 2, count * 2);
	}

	internal static void BlockCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
	{
		Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
	}

	private void CheckAsyncCall()
	{
		if (!useAsync)
		{
			throw new InvalidOperationException(Res.GetString("Set XmlReaderSettings.Async to true if you want to use Async Methods."));
		}
	}

	public override Task<string> GetValueAsync()
	{
		CheckAsyncCall();
		if (parsingFunction >= ParsingFunction.PartialTextValue)
		{
			return _GetValueAsync();
		}
		return Task.FromResult(curNode.StringValue);
	}

	private async Task<string> _GetValueAsync()
	{
		if (parsingFunction >= ParsingFunction.PartialTextValue)
		{
			if (parsingFunction != ParsingFunction.PartialTextValue)
			{
				await FinishOtherValueIteratorAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await FinishPartialValueAsync().ConfigureAwait(continueOnCapturedContext: false);
				parsingFunction = nextParsingFunction;
			}
		}
		return curNode.StringValue;
	}

	private Task FinishInitAsync()
	{
		return laterInitParam.initType switch
		{
			InitInputType.UriString => FinishInitUriStringAsync(), 
			InitInputType.Stream => FinishInitStreamAsync(), 
			InitInputType.TextReader => FinishInitTextReaderAsync(), 
			_ => AsyncHelper.DoneTask, 
		};
	}

	private async Task FinishInitUriStringAsync()
	{
		Stream stream = (Stream)(await laterInitParam.inputUriResolver.GetEntityAsync(laterInitParam.inputbaseUri, string.Empty, typeof(Stream)).ConfigureAwait(continueOnCapturedContext: false));
		if (stream == null)
		{
			throw new XmlException("Cannot resolve '{0}'.", laterInitParam.inputUriStr);
		}
		Encoding encoding = null;
		if (laterInitParam.inputContext != null)
		{
			encoding = laterInitParam.inputContext.Encoding;
		}
		try
		{
			await InitStreamInputAsync(laterInitParam.inputbaseUri, reportedBaseUri, stream, null, 0, encoding).ConfigureAwait(continueOnCapturedContext: false);
			reportedEncoding = ps.encoding;
			if (laterInitParam.inputContext != null && laterInitParam.inputContext.HasDtdInfo)
			{
				await ProcessDtdFromParserContextAsync(laterInitParam.inputContext).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			stream.Close();
			throw;
		}
		laterInitParam = null;
	}

	private async Task FinishInitStreamAsync()
	{
		Encoding encoding = null;
		if (laterInitParam.inputContext != null)
		{
			encoding = laterInitParam.inputContext.Encoding;
		}
		await InitStreamInputAsync(laterInitParam.inputbaseUri, reportedBaseUri, laterInitParam.inputStream, laterInitParam.inputBytes, laterInitParam.inputByteCount, encoding).ConfigureAwait(continueOnCapturedContext: false);
		reportedEncoding = ps.encoding;
		if (laterInitParam.inputContext != null && laterInitParam.inputContext.HasDtdInfo)
		{
			await ProcessDtdFromParserContextAsync(laterInitParam.inputContext).ConfigureAwait(continueOnCapturedContext: false);
		}
		laterInitParam = null;
	}

	private async Task FinishInitTextReaderAsync()
	{
		await InitTextReaderInputAsync(reportedBaseUri, laterInitParam.inputTextReader).ConfigureAwait(continueOnCapturedContext: false);
		reportedEncoding = ps.encoding;
		if (laterInitParam.inputContext != null && laterInitParam.inputContext.HasDtdInfo)
		{
			await ProcessDtdFromParserContextAsync(laterInitParam.inputContext).ConfigureAwait(continueOnCapturedContext: false);
		}
		laterInitParam = null;
	}

	public override Task<bool> ReadAsync()
	{
		CheckAsyncCall();
		if (laterInitParam != null)
		{
			return FinishInitAsync().CallBoolTaskFuncWhenFinish(ReadAsync);
		}
		while (true)
		{
			switch (parsingFunction)
			{
			case ParsingFunction.ElementContent:
				return ParseElementContentAsync();
			case ParsingFunction.DocumentContent:
				return ParseDocumentContentAsync();
			case ParsingFunction.SwitchToInteractive:
				readState = ReadState.Interactive;
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.SwitchToInteractiveXmlDecl:
				return ReadAsync_SwitchToInteractiveXmlDecl();
			case ParsingFunction.ResetAttributesRootLevel:
				ResetAttributes();
				curNode = nodes[index];
				parsingFunction = ((index == 0) ? ParsingFunction.DocumentContent : ParsingFunction.ElementContent);
				break;
			case ParsingFunction.MoveToElementContent:
				ResetAttributes();
				index++;
				curNode = AddNode(index, index);
				parsingFunction = ParsingFunction.ElementContent;
				break;
			case ParsingFunction.PopElementContext:
				PopElementContext();
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.PopEmptyElementContext:
				curNode = nodes[index];
				curNode.IsEmptyElement = false;
				ResetAttributes();
				PopElementContext();
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.EntityReference:
				parsingFunction = nextParsingFunction;
				return ParseEntityReferenceAsync().ReturnTaskBoolWhenFinish(ret: true);
			case ParsingFunction.ReportEndEntity:
				SetupEndEntityNodeInContent();
				parsingFunction = nextParsingFunction;
				return AsyncHelper.DoneTaskTrue;
			case ParsingFunction.AfterResolveEntityInContent:
				curNode = AddNode(index, index);
				reportedEncoding = ps.encoding;
				reportedBaseUri = ps.baseUriStr;
				parsingFunction = nextParsingFunction;
				break;
			case ParsingFunction.AfterResolveEmptyEntityInContent:
				curNode = AddNode(index, index);
				curNode.SetValueNode(XmlNodeType.Text, string.Empty);
				curNode.SetLineInfo(ps.lineNo, ps.LinePos);
				reportedEncoding = ps.encoding;
				reportedBaseUri = ps.baseUriStr;
				parsingFunction = nextParsingFunction;
				return AsyncHelper.DoneTaskTrue;
			case ParsingFunction.InReadAttributeValue:
				FinishAttributeValueIterator();
				curNode = nodes[index];
				break;
			case ParsingFunction.InIncrementalRead:
				FinishIncrementalRead();
				return AsyncHelper.DoneTaskTrue;
			case ParsingFunction.FragmentAttribute:
				return Task.FromResult(ParseFragmentAttribute());
			case ParsingFunction.XmlDeclarationFragment:
				ParseXmlDeclarationFragment();
				parsingFunction = ParsingFunction.GoToEof;
				return AsyncHelper.DoneTaskTrue;
			case ParsingFunction.GoToEof:
				OnEof();
				return AsyncHelper.DoneTaskFalse;
			case ParsingFunction.Error:
			case ParsingFunction.Eof:
			case ParsingFunction.ReaderClosed:
				return AsyncHelper.DoneTaskFalse;
			case ParsingFunction.NoData:
				ThrowWithoutLineInfo("Root element is missing.");
				return AsyncHelper.DoneTaskFalse;
			case ParsingFunction.PartialTextValue:
				return SkipPartialTextValueAsync().CallBoolTaskFuncWhenFinish(ReadAsync);
			case ParsingFunction.InReadValueChunk:
				return FinishReadValueChunkAsync().CallBoolTaskFuncWhenFinish(ReadAsync);
			case ParsingFunction.InReadContentAsBinary:
				return FinishReadContentAsBinaryAsync().CallBoolTaskFuncWhenFinish(ReadAsync);
			case ParsingFunction.InReadElementContentAsBinary:
				return FinishReadElementContentAsBinaryAsync().CallBoolTaskFuncWhenFinish(ReadAsync);
			}
		}
	}

	private Task<bool> ReadAsync_SwitchToInteractiveXmlDecl()
	{
		readState = ReadState.Interactive;
		parsingFunction = nextParsingFunction;
		Task<bool> task = ParseXmlDeclarationAsync(isTextDecl: false);
		if (task.IsSuccess())
		{
			return ReadAsync_SwitchToInteractiveXmlDecl_Helper(task.Result);
		}
		return _ReadAsync_SwitchToInteractiveXmlDecl(task);
	}

	private async Task<bool> _ReadAsync_SwitchToInteractiveXmlDecl(Task<bool> task)
	{
		return await ReadAsync_SwitchToInteractiveXmlDecl_Helper(await task.ConfigureAwait(continueOnCapturedContext: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task<bool> ReadAsync_SwitchToInteractiveXmlDecl_Helper(bool finish)
	{
		if (finish)
		{
			reportedEncoding = ps.encoding;
			return AsyncHelper.DoneTaskTrue;
		}
		reportedEncoding = ps.encoding;
		return ReadAsync();
	}

	public override async Task SkipAsync()
	{
		CheckAsyncCall();
		if (readState != ReadState.Interactive)
		{
			return;
		}
		if (InAttributeValueIterator)
		{
			FinishAttributeValueIterator();
			curNode = nodes[index];
		}
		else
		{
			switch (parsingFunction)
			{
			case ParsingFunction.InIncrementalRead:
				FinishIncrementalRead();
				break;
			case ParsingFunction.PartialTextValue:
				await SkipPartialTextValueAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case ParsingFunction.InReadValueChunk:
				await FinishReadValueChunkAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case ParsingFunction.InReadContentAsBinary:
				await FinishReadContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			case ParsingFunction.InReadElementContentAsBinary:
				await FinishReadElementContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
		}
		XmlNodeType type = curNode.type;
		if (type != XmlNodeType.Element)
		{
			if (type != XmlNodeType.Attribute)
			{
				goto IL_0318;
			}
			outerReader.MoveToElement();
		}
		if (!curNode.IsEmptyElement)
		{
			int initialDepth = index;
			parsingMode = ParsingMode.SkipContent;
			while (await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false) && index > initialDepth)
			{
			}
			parsingMode = ParsingMode.Full;
		}
		goto IL_0318;
		IL_0318:
		await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<int> ReadContentAsBase64_AsyncHelper(Task<bool> task, byte[] buffer, int index, int count)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		if (!task.Result)
		{
			return 0;
		}
		InitBase64Decoder();
		return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count)
	{
		CheckAsyncCall();
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
		if (parsingFunction == ParsingFunction.InReadContentAsBinary)
		{
			if (incReadDecoder == base64Decoder)
			{
				return ReadContentAsBinaryAsync(buffer, index, count);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return AsyncHelper.DoneTaskZero;
			}
			if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (!XmlReader.CanReadContentAs(curNode.type))
			{
				throw CreateReadContentAsException("ReadContentAsBase64");
			}
			Task<bool> task = InitReadContentAsBinaryAsync();
			if (!task.IsSuccess())
			{
				return ReadContentAsBase64_AsyncHelper(task, buffer, index, count);
			}
			if (!task.Result)
			{
				return AsyncHelper.DoneTaskZero;
			}
		}
		InitBase64Decoder();
		return ReadContentAsBinaryAsync(buffer, index, count);
	}

	public override async Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		CheckAsyncCall();
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
		if (parsingFunction == ParsingFunction.InReadContentAsBinary)
		{
			if (incReadDecoder == binHexDecoder)
			{
				return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (!XmlReader.CanReadContentAs(curNode.type))
			{
				throw CreateReadContentAsException("ReadContentAsBinHex");
			}
			if (!(await InitReadContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
		}
		InitBinHexDecoder();
		return await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<int> ReadElementContentAsBase64Async_Helper(Task<bool> task, byte[] buffer, int index, int count)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		if (!task.Result)
		{
			return 0;
		}
		InitBase64Decoder();
		return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count)
	{
		CheckAsyncCall();
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
		if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
		{
			if (incReadDecoder == base64Decoder)
			{
				return ReadElementContentAsBinaryAsync(buffer, index, count);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return AsyncHelper.DoneTaskZero;
			}
			if (parsingFunction == ParsingFunction.InReadContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (curNode.type != XmlNodeType.Element)
			{
				throw CreateReadElementContentAsException("ReadElementContentAsBinHex");
			}
			Task<bool> task = InitReadElementContentAsBinaryAsync();
			if (!task.IsSuccess())
			{
				return ReadElementContentAsBase64Async_Helper(task, buffer, index, count);
			}
			if (!task.Result)
			{
				return AsyncHelper.DoneTaskZero;
			}
		}
		InitBase64Decoder();
		return ReadElementContentAsBinaryAsync(buffer, index, count);
	}

	public override async Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count)
	{
		CheckAsyncCall();
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
		if (parsingFunction == ParsingFunction.InReadElementContentAsBinary)
		{
			if (incReadDecoder == binHexDecoder)
			{
				return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		else
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.InReadContentAsBinary)
			{
				throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadElementContentAsBase64 and ReadElementContentAsBinHex."));
			}
			if (curNode.type != XmlNodeType.Element)
			{
				throw CreateReadElementContentAsException("ReadElementContentAsBinHex");
			}
			if (!(await InitReadElementContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false)))
			{
				return 0;
			}
		}
		InitBinHexDecoder();
		return await ReadElementContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<int> ReadValueChunkAsync(char[] buffer, int index, int count)
	{
		CheckAsyncCall();
		if (!XmlReader.HasValueInternal(curNode.type))
		{
			throw new InvalidOperationException(Res.GetString("The ReadValueAsChunk method is not supported on node type {0}.", curNode.type));
		}
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
		if (parsingFunction != ParsingFunction.InReadValueChunk)
		{
			if (readState != ReadState.Interactive)
			{
				return 0;
			}
			if (parsingFunction == ParsingFunction.PartialTextValue)
			{
				incReadState = IncrementalReadState.ReadValueChunk_OnPartialValue;
			}
			else
			{
				incReadState = IncrementalReadState.ReadValueChunk_OnCachedValue;
				nextNextParsingFunction = nextParsingFunction;
				nextParsingFunction = parsingFunction;
			}
			parsingFunction = ParsingFunction.InReadValueChunk;
			readValueOffset = 0;
		}
		if (count == 0)
		{
			return 0;
		}
		int readCount = 0;
		int num = curNode.CopyTo(readValueOffset, buffer, index + readCount, count - readCount);
		readCount += num;
		readValueOffset += num;
		if (readCount == count)
		{
			if (XmlCharType.IsHighSurrogate(buffer[index + count - 1]))
			{
				readCount--;
				readValueOffset--;
				if (readCount == 0)
				{
					Throw("The buffer is not large enough to fit a surrogate pair. Please provide a buffer of size at least 2 characters.");
				}
			}
			return readCount;
		}
		if (incReadState == IncrementalReadState.ReadValueChunk_OnPartialValue)
		{
			curNode.SetValue(string.Empty);
			bool flag = false;
			int num2 = 0;
			int num3 = 0;
			while (readCount < count && !flag)
			{
				int outOrChars = 0;
				Tuple<int, int, int, bool> obj = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
				num2 = obj.Item1;
				num3 = obj.Item2;
				_ = obj.Item3;
				flag = obj.Item4;
				int num4 = count - readCount;
				if (num4 > num3 - num2)
				{
					num4 = num3 - num2;
				}
				BlockCopyChars(ps.chars, num2, buffer, index + readCount, num4);
				readCount += num4;
				num2 += num4;
			}
			incReadState = (flag ? IncrementalReadState.ReadValueChunk_OnCachedValue : IncrementalReadState.ReadValueChunk_OnPartialValue);
			if (readCount == count && XmlCharType.IsHighSurrogate(buffer[index + count - 1]))
			{
				readCount--;
				num2--;
				if (readCount == 0)
				{
					Throw("The buffer is not large enough to fit a surrogate pair. Please provide a buffer of size at least 2 characters.");
				}
			}
			readValueOffset = 0;
			curNode.SetValue(ps.chars, num2, num3 - num2);
		}
		return readCount;
	}

	internal Task<int> DtdParserProxy_ReadDataAsync()
	{
		CheckAsyncCall();
		return ReadDataAsync();
	}

	internal async Task<int> DtdParserProxy_ParseNumericCharRefAsync(StringBuilder internalSubsetBuilder)
	{
		CheckAsyncCall();
		return (await ParseNumericCharRefAsync(expand: true, internalSubsetBuilder).ConfigureAwait(continueOnCapturedContext: false)).Item2;
	}

	internal Task<int> DtdParserProxy_ParseNamedCharRefAsync(bool expand, StringBuilder internalSubsetBuilder)
	{
		CheckAsyncCall();
		return ParseNamedCharRefAsync(expand, internalSubsetBuilder);
	}

	internal async Task DtdParserProxy_ParsePIAsync(StringBuilder sb)
	{
		CheckAsyncCall();
		if (sb == null)
		{
			ParsingMode pm = parsingMode;
			parsingMode = ParsingMode.SkipNode;
			await ParsePIAsync(null).ConfigureAwait(continueOnCapturedContext: false);
			parsingMode = pm;
		}
		else
		{
			await ParsePIAsync(sb).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	internal async Task DtdParserProxy_ParseCommentAsync(StringBuilder sb)
	{
		CheckAsyncCall();
		try
		{
			if (sb == null)
			{
				ParsingMode savedParsingMode = parsingMode;
				parsingMode = ParsingMode.SkipNode;
				await ParseCDataOrCommentAsync(XmlNodeType.Comment).ConfigureAwait(continueOnCapturedContext: false);
				parsingMode = savedParsingMode;
			}
			else
			{
				NodeData originalCurNode = curNode;
				curNode = AddNode(index + attrCount + 1, index);
				await ParseCDataOrCommentAsync(XmlNodeType.Comment).ConfigureAwait(continueOnCapturedContext: false);
				curNode.CopyTo(0, sb);
				curNode = originalCurNode;
			}
		}
		catch (XmlException ex)
		{
			if (ex.ResString == "Unexpected end of file while parsing {0} has occurred." && ps.entity != null)
			{
				SendValidationEvent(XmlSeverityType.Error, "The parameter entity replacement text must nest properly within markup declarations.", null, ps.LineNo, ps.LinePos);
				return;
			}
			throw;
		}
	}

	internal async Task<Tuple<int, bool>> DtdParserProxy_PushEntityAsync(IDtdEntityInfo entity)
	{
		CheckAsyncCall();
		bool item;
		if (entity.IsExternal)
		{
			if (IsResolverNull)
			{
				return new Tuple<int, bool>(-1, item2: false);
			}
			item = await PushExternalEntityAsync(entity).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			PushInternalEntity(entity);
			item = true;
		}
		return new Tuple<int, bool>(ps.entityId, item);
	}

	internal async Task<bool> DtdParserProxy_PushExternalSubsetAsync(string systemId, string publicId)
	{
		CheckAsyncCall();
		if (IsResolverNull)
		{
			return false;
		}
		if (ps.baseUri == null && !string.IsNullOrEmpty(ps.baseUriStr))
		{
			ps.baseUri = xmlResolver.ResolveUri(null, ps.baseUriStr);
		}
		await PushExternalEntityOrSubsetAsync(publicId, systemId, ps.baseUri, null).ConfigureAwait(continueOnCapturedContext: false);
		ps.entity = null;
		ps.entityId = 0;
		int initialPos = ps.charPos;
		if (v1Compat)
		{
			await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (!(await ParseXmlDeclarationAsync(isTextDecl: true).ConfigureAwait(continueOnCapturedContext: false)))
		{
			ps.charPos = initialPos;
		}
		return true;
	}

	private Task InitStreamInputAsync(Uri baseUri, Stream stream, Encoding encoding)
	{
		return InitStreamInputAsync(baseUri, baseUri.ToString(), stream, null, 0, encoding);
	}

	private Task InitStreamInputAsync(Uri baseUri, string baseUriStr, Stream stream, Encoding encoding)
	{
		return InitStreamInputAsync(baseUri, baseUriStr, stream, null, 0, encoding);
	}

	private async Task InitStreamInputAsync(Uri baseUri, string baseUriStr, Stream stream, byte[] bytes, int byteCount, Encoding encoding)
	{
		ps.stream = stream;
		ps.baseUri = baseUri;
		ps.baseUriStr = baseUriStr;
		int num;
		if (bytes != null)
		{
			ps.bytes = bytes;
			ps.bytesUsed = byteCount;
			num = ps.bytes.Length;
		}
		else
		{
			num = ((laterInitParam == null || !laterInitParam.useAsync) ? XmlReader.CalcBufferSize(stream) : 65536);
			if (ps.bytes == null || ps.bytes.Length < num)
			{
				ps.bytes = new byte[num];
			}
		}
		if (ps.chars == null || ps.chars.Length < num + 1)
		{
			ps.chars = new char[num + 1];
		}
		ps.bytePos = 0;
		while (ps.bytesUsed < 4 && ps.bytes.Length - ps.bytesUsed > 0)
		{
			int num2 = await stream.ReadAsync(ps.bytes, ps.bytesUsed, ps.bytes.Length - ps.bytesUsed).ConfigureAwait(continueOnCapturedContext: false);
			if (num2 == 0)
			{
				ps.isStreamEof = true;
				break;
			}
			ps.bytesUsed += num2;
		}
		if (encoding == null)
		{
			encoding = DetectEncoding();
		}
		SetupEncoding(encoding);
		byte[] preamble = ps.encoding.GetPreamble();
		int num3 = preamble.Length;
		int i;
		for (i = 0; i < num3 && i < ps.bytesUsed && ps.bytes[i] == preamble[i]; i++)
		{
		}
		if (i == num3)
		{
			ps.bytePos = num3;
		}
		documentStartBytePos = ps.bytePos;
		ps.eolNormalized = !normalize;
		ps.appendMode = true;
		await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task InitTextReaderInputAsync(string baseUriStr, TextReader input)
	{
		return InitTextReaderInputAsync(baseUriStr, null, input);
	}

	private Task InitTextReaderInputAsync(string baseUriStr, Uri baseUri, TextReader input)
	{
		ps.textReader = input;
		ps.baseUriStr = baseUriStr;
		ps.baseUri = baseUri;
		if (ps.chars == null)
		{
			int num = ((laterInitParam == null || !laterInitParam.useAsync) ? 4096 : 65536);
			ps.chars = new char[num + 1];
		}
		ps.encoding = Encoding.Unicode;
		ps.eolNormalized = !normalize;
		ps.appendMode = true;
		return ReadDataAsync();
	}

	private Task ProcessDtdFromParserContextAsync(XmlParserContext context)
	{
		switch (dtdProcessing)
		{
		case DtdProcessing.Prohibit:
			ThrowWithoutLineInfo("For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.");
			break;
		case DtdProcessing.Parse:
			return ParseDtdFromParserContextAsync();
		}
		return AsyncHelper.DoneTask;
	}

	private Task SwitchEncodingAsync(Encoding newEncoding)
	{
		if ((newEncoding.WebName != ps.encoding.WebName || ps.decoder is SafeAsciiDecoder) && !afterResetState)
		{
			UnDecodeChars();
			ps.appendMode = false;
			SetupEncoding(newEncoding);
			return ReadDataAsync();
		}
		return AsyncHelper.DoneTask;
	}

	private Task SwitchEncodingToUTF8Async()
	{
		return SwitchEncodingAsync(new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true));
	}

	private async Task<int> ReadDataAsync()
	{
		if (ps.isEof)
		{
			return 0;
		}
		int charsRead;
		if (ps.appendMode)
		{
			if (ps.charsUsed == ps.chars.Length - 1)
			{
				for (int i = 0; i < attrCount; i++)
				{
					nodes[index + i + 1].OnBufferInvalidated();
				}
				char[] array = new char[ps.chars.Length * 2];
				BlockCopyChars(ps.chars, 0, array, 0, ps.chars.Length);
				ps.chars = array;
			}
			if (ps.stream != null && ps.bytesUsed - ps.bytePos < 6 && ps.bytes.Length - ps.bytesUsed < 6)
			{
				byte[] array2 = new byte[ps.bytes.Length * 2];
				BlockCopy(ps.bytes, 0, array2, 0, ps.bytesUsed);
				ps.bytes = array2;
			}
			charsRead = ps.chars.Length - ps.charsUsed - 1;
			if (charsRead > 80)
			{
				charsRead = 80;
			}
		}
		else
		{
			int num = ps.chars.Length;
			if (num - ps.charsUsed <= num / 2)
			{
				for (int j = 0; j < attrCount; j++)
				{
					nodes[index + j + 1].OnBufferInvalidated();
				}
				int num2 = ps.charsUsed - ps.charPos;
				if (num2 < num - 1)
				{
					ps.lineStartPos -= ps.charPos;
					if (num2 > 0)
					{
						BlockCopyChars(ps.chars, ps.charPos, ps.chars, 0, num2);
					}
					ps.charPos = 0;
					ps.charsUsed = num2;
				}
				else
				{
					char[] array3 = new char[ps.chars.Length * 2];
					BlockCopyChars(ps.chars, 0, array3, 0, ps.chars.Length);
					ps.chars = array3;
				}
			}
			if (ps.stream != null)
			{
				int num3 = ps.bytesUsed - ps.bytePos;
				if (num3 <= 128)
				{
					if (num3 == 0)
					{
						ps.bytesUsed = 0;
					}
					else
					{
						BlockCopy(ps.bytes, ps.bytePos, ps.bytes, 0, num3);
						ps.bytesUsed = num3;
					}
					ps.bytePos = 0;
				}
			}
			charsRead = ps.chars.Length - ps.charsUsed - 1;
		}
		if (ps.stream != null)
		{
			if (!ps.isStreamEof && ps.bytePos == ps.bytesUsed && ps.bytes.Length - ps.bytesUsed > 0)
			{
				int num4 = await ps.stream.ReadAsync(ps.bytes, ps.bytesUsed, ps.bytes.Length - ps.bytesUsed).ConfigureAwait(continueOnCapturedContext: false);
				if (num4 == 0)
				{
					ps.isStreamEof = true;
				}
				ps.bytesUsed += num4;
			}
			int bytePos = ps.bytePos;
			charsRead = GetChars(charsRead);
			if (charsRead == 0 && ps.bytePos != bytePos)
			{
				return await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		else if (ps.textReader != null)
		{
			charsRead = await ps.textReader.ReadAsync(ps.chars, ps.charsUsed, ps.chars.Length - ps.charsUsed - 1).ConfigureAwait(continueOnCapturedContext: false);
			ps.charsUsed += charsRead;
		}
		else
		{
			charsRead = 0;
		}
		RegisterConsumedCharacters(charsRead, InEntity);
		if (charsRead == 0)
		{
			ps.isEof = true;
		}
		ps.chars[ps.charsUsed] = '\0';
		return charsRead;
	}

	private async Task<bool> ParseXmlDeclarationAsync(bool isTextDecl)
	{
		do
		{
			if (ps.charsUsed - ps.charPos < 6)
			{
				continue;
			}
			if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 5, "<?xml") || xmlCharType.IsNameSingleChar(ps.chars[ps.charPos + 5]))
			{
				break;
			}
			if (!isTextDecl)
			{
				curNode.SetLineInfo(ps.LineNo, ps.LinePos + 2);
				curNode.SetNamedNode(XmlNodeType.XmlDeclaration, Xml);
			}
			ps.charPos += 5;
			StringBuilder sb = (isTextDecl ? new StringBuilder() : stringBuilder);
			int xmlDeclState = 0;
			Encoding encoding = null;
			while (true)
			{
				int originalSbLen = sb.Length;
				int num = await EatWhitespacesAsync((xmlDeclState == 0) ? null : sb).ConfigureAwait(continueOnCapturedContext: false);
				if (ps.chars[ps.charPos] == '?')
				{
					sb.Length = originalSbLen;
					if (ps.chars[ps.charPos + 1] == '>')
					{
						break;
					}
					if (ps.charPos + 1 == ps.charsUsed)
					{
						goto IL_0ca5;
					}
					ThrowUnexpectedToken("'>'");
				}
				if (num == 0 && xmlDeclState != 0)
				{
					ThrowUnexpectedToken("?>");
				}
				int num2 = await ParseNameAsync().ConfigureAwait(continueOnCapturedContext: false);
				NodeData attr = null;
				char c = ps.chars[ps.charPos];
				if (c != 'e')
				{
					if (c != 's')
					{
						if (c != 'v' || !XmlConvert.StrEqual(ps.chars, ps.charPos, num2 - ps.charPos, "version") || xmlDeclState != 0)
						{
							goto IL_0699;
						}
						if (!isTextDecl)
						{
							attr = AddAttributeNoChecks("version", 1);
						}
					}
					else
					{
						if (!XmlConvert.StrEqual(ps.chars, ps.charPos, num2 - ps.charPos, "standalone") || (xmlDeclState != 1 && xmlDeclState != 2) || isTextDecl)
						{
							goto IL_0699;
						}
						if (!isTextDecl)
						{
							attr = AddAttributeNoChecks("standalone", 1);
						}
						xmlDeclState = 2;
					}
				}
				else
				{
					if (!XmlConvert.StrEqual(ps.chars, ps.charPos, num2 - ps.charPos, "encoding") || (xmlDeclState != 1 && (!isTextDecl || xmlDeclState != 0)))
					{
						goto IL_0699;
					}
					if (!isTextDecl)
					{
						attr = AddAttributeNoChecks("encoding", 1);
					}
					xmlDeclState = 1;
				}
				goto IL_06b3;
				IL_06b3:
				if (!isTextDecl)
				{
					attr.SetLineInfo(ps.LineNo, ps.LinePos);
				}
				sb.Append(ps.chars, ps.charPos, num2 - ps.charPos);
				ps.charPos = num2;
				if (ps.chars[ps.charPos] != '=')
				{
					await EatWhitespacesAsync(sb).ConfigureAwait(continueOnCapturedContext: false);
					if (ps.chars[ps.charPos] != '=')
					{
						ThrowUnexpectedToken("=");
					}
				}
				sb.Append('=');
				ps.charPos++;
				char quoteChar = ps.chars[ps.charPos];
				if (quoteChar != '"' && quoteChar != '\'')
				{
					await EatWhitespacesAsync(sb).ConfigureAwait(continueOnCapturedContext: false);
					quoteChar = ps.chars[ps.charPos];
					if (quoteChar != '"' && quoteChar != '\'')
					{
						ThrowUnexpectedToken("\"", "'");
					}
				}
				sb.Append(quoteChar);
				ps.charPos++;
				if (!isTextDecl)
				{
					attr.quoteChar = quoteChar;
					attr.SetLineInfo2(ps.LineNo, ps.LinePos);
				}
				int pos = ps.charPos;
				char[] chars;
				while (true)
				{
					for (chars = ps.chars; (xmlCharType.charProperties[(uint)chars[pos]] & 0x80) != 0; pos++)
					{
					}
					if (ps.chars[pos] == quoteChar)
					{
						break;
					}
					if (pos == ps.charsUsed)
					{
						if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0)
						{
							continue;
						}
						goto IL_0c7e;
					}
					goto IL_0c8b;
				}
				switch (xmlDeclState)
				{
				case 0:
					if (XmlConvert.StrEqual(ps.chars, ps.charPos, pos - ps.charPos, "1.0"))
					{
						if (!isTextDecl)
						{
							attr.SetValue(ps.chars, ps.charPos, pos - ps.charPos);
						}
						xmlDeclState = 1;
					}
					else
					{
						string arg = new string(ps.chars, ps.charPos, pos - ps.charPos);
						Throw("Version number '{0}' is invalid.", arg);
					}
					break;
				case 1:
				{
					string text = new string(ps.chars, ps.charPos, pos - ps.charPos);
					encoding = CheckEncoding(text);
					if (!isTextDecl)
					{
						attr.SetValue(text);
					}
					xmlDeclState = 2;
					break;
				}
				case 2:
					if (XmlConvert.StrEqual(ps.chars, ps.charPos, pos - ps.charPos, "yes"))
					{
						standalone = true;
					}
					else if (XmlConvert.StrEqual(ps.chars, ps.charPos, pos - ps.charPos, "no"))
					{
						standalone = false;
					}
					else
					{
						Throw("Syntax for an XML declaration is invalid.", ps.LineNo, ps.LinePos - 1);
					}
					if (!isTextDecl)
					{
						attr.SetValue(ps.chars, ps.charPos, pos - ps.charPos);
					}
					xmlDeclState = 3;
					break;
				}
				sb.Append(chars, ps.charPos, pos - ps.charPos);
				sb.Append(quoteChar);
				ps.charPos = pos + 1;
				continue;
				IL_0ca5:
				bool flag = ps.isEof;
				if (!flag)
				{
					flag = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0;
				}
				if (flag)
				{
					Throw("Unexpected end of file has occurred.");
				}
				continue;
				IL_0c8b:
				Throw(isTextDecl ? "Invalid text declaration." : "Syntax for an XML declaration is invalid.");
				goto IL_0ca5;
				IL_0c7e:
				Throw("There is an unclosed literal string.");
				goto IL_0ca5;
				IL_0699:
				Throw(isTextDecl ? "Invalid text declaration." : "Syntax for an XML declaration is invalid.");
				goto IL_06b3;
			}
			if (xmlDeclState == 0)
			{
				Throw(isTextDecl ? "Invalid text declaration." : "Syntax for an XML declaration is invalid.");
			}
			ps.charPos += 2;
			if (!isTextDecl)
			{
				curNode.SetValue(sb.ToString());
				sb.Length = 0;
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.ResetAttributesRootLevel;
			}
			if (encoding != null)
			{
				await SwitchEncodingAsync(encoding).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				if (isTextDecl)
				{
					Throw("Invalid text declaration.");
				}
				if (afterResetState)
				{
					string webName = ps.encoding.WebName;
					if (webName != "utf-8" && webName != "utf-16" && webName != "utf-16BE" && !(ps.encoding is Ucs4Encoding))
					{
						Throw("'{0}' is an invalid value for the 'encoding' attribute. The encoding cannot be switched after a call to ResetState.", (ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
					}
				}
				if (ps.decoder is SafeAsciiDecoder)
				{
					await SwitchEncodingToUTF8Async().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			ps.appendMode = false;
			return true;
		}
		while (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0);
		if (!isTextDecl)
		{
			parsingFunction = nextParsingFunction;
		}
		if (afterResetState)
		{
			string webName2 = ps.encoding.WebName;
			if (webName2 != "utf-8" && webName2 != "utf-16" && webName2 != "utf-16BE" && !(ps.encoding is Ucs4Encoding))
			{
				Throw("'{0}' is an invalid value for the 'encoding' attribute. The encoding cannot be switched after a call to ResetState.", (ps.encoding.GetByteCount("A") == 1) ? "UTF-8" : "UTF-16");
			}
		}
		if (ps.decoder is SafeAsciiDecoder)
		{
			await SwitchEncodingToUTF8Async().ConfigureAwait(continueOnCapturedContext: false);
		}
		ps.appendMode = false;
		return false;
	}

	private Task<bool> ParseDocumentContentAsync()
	{
		char[] chars;
		bool needMoreChars;
		int charPos;
		while (true)
		{
			needMoreChars = false;
			charPos = ps.charPos;
			chars = ps.chars;
			if (chars[charPos] != '<')
			{
				break;
			}
			needMoreChars = true;
			if (ps.charsUsed - charPos < 4)
			{
				return ParseDocumentContentAsync_ReadData(needMoreChars);
			}
			charPos++;
			switch (chars[charPos])
			{
			case '?':
				ps.charPos = charPos + 1;
				return ParsePIAsync().ContinueBoolTaskFuncWhenFalse(ParseDocumentContentAsync);
			case '!':
				charPos++;
				if (ps.charsUsed - charPos < 2)
				{
					return ParseDocumentContentAsync_ReadData(needMoreChars);
				}
				if (chars[charPos] == '-')
				{
					if (chars[charPos + 1] == '-')
					{
						ps.charPos = charPos + 2;
						return ParseCommentAsync().ContinueBoolTaskFuncWhenFalse(ParseDocumentContentAsync);
					}
					ThrowUnexpectedToken(charPos + 1, "-");
				}
				else if (chars[charPos] == '[')
				{
					if (fragmentType != XmlNodeType.Document)
					{
						charPos++;
						if (ps.charsUsed - charPos < 6)
						{
							return ParseDocumentContentAsync_ReadData(needMoreChars);
						}
						if (XmlConvert.StrEqual(chars, charPos, 6, "CDATA["))
						{
							ps.charPos = charPos + 6;
							return ParseCDataAsync().CallBoolTaskFuncWhenFinish(ParseDocumentContentAsync_CData);
						}
						ThrowUnexpectedToken(charPos, "CDATA[");
					}
					else
					{
						Throw(ps.charPos, "Data at the root level is invalid.");
					}
				}
				else
				{
					if (fragmentType == XmlNodeType.Document || fragmentType == XmlNodeType.None)
					{
						fragmentType = XmlNodeType.Document;
						ps.charPos = charPos;
						return ParseDoctypeDeclAsync().ContinueBoolTaskFuncWhenFalse(ParseDocumentContentAsync);
					}
					if (ParseUnexpectedToken(charPos) == "DOCTYPE")
					{
						Throw("Unexpected DTD declaration.");
					}
					else
					{
						ThrowUnexpectedToken(charPos, "<!--", "<[CDATA[");
					}
				}
				continue;
			case '/':
				Throw(charPos + 1, "Unexpected end tag.");
				continue;
			}
			if (rootElementParsed)
			{
				if (fragmentType == XmlNodeType.Document)
				{
					Throw(charPos, "There are multiple root elements.");
				}
				if (fragmentType == XmlNodeType.None)
				{
					fragmentType = XmlNodeType.Element;
				}
			}
			ps.charPos = charPos;
			rootElementParsed = true;
			return ParseElementAsync().ReturnTaskBoolWhenFinish(ret: true);
		}
		if (chars[charPos] == '&')
		{
			return ParseDocumentContentAsync_ParseEntity();
		}
		if (charPos == ps.charsUsed || (v1Compat && chars[charPos] == '\0'))
		{
			return ParseDocumentContentAsync_ReadData(needMoreChars);
		}
		if (fragmentType == XmlNodeType.Document)
		{
			return ParseRootLevelWhitespaceAsync().ContinueBoolTaskFuncWhenFalse(ParseDocumentContentAsync);
		}
		return ParseDocumentContentAsync_WhiteSpace();
	}

	private Task<bool> ParseDocumentContentAsync_CData()
	{
		if (fragmentType == XmlNodeType.None)
		{
			fragmentType = XmlNodeType.Element;
		}
		return AsyncHelper.DoneTaskTrue;
	}

	private async Task<bool> ParseDocumentContentAsync_ParseEntity()
	{
		int charPos = ps.charPos;
		if (fragmentType == XmlNodeType.Document)
		{
			Throw(charPos, "Data at the root level is invalid.");
			return false;
		}
		if (fragmentType == XmlNodeType.None)
		{
			fragmentType = XmlNodeType.Element;
		}
		switch ((await HandleEntityReferenceAsync(isInAttributeValue: false, EntityExpandType.OnlyGeneral).ConfigureAwait(continueOnCapturedContext: false)).Item2)
		{
		case EntityType.Unexpanded:
			if (parsingFunction == ParsingFunction.EntityReference)
			{
				parsingFunction = nextParsingFunction;
			}
			await ParseEntityReferenceAsync().ConfigureAwait(continueOnCapturedContext: false);
			return true;
		case EntityType.CharacterDec:
		case EntityType.CharacterHex:
		case EntityType.CharacterNamed:
			if (await ParseTextAsync().ConfigureAwait(continueOnCapturedContext: false))
			{
				return true;
			}
			return await ParseDocumentContentAsync().ConfigureAwait(continueOnCapturedContext: false);
		default:
			return await ParseDocumentContentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private Task<bool> ParseDocumentContentAsync_WhiteSpace()
	{
		Task<bool> task = ParseTextAsync();
		if (task.IsSuccess())
		{
			if (task.Result)
			{
				if (fragmentType == XmlNodeType.None && curNode.type == XmlNodeType.Text)
				{
					fragmentType = XmlNodeType.Element;
				}
				return AsyncHelper.DoneTaskTrue;
			}
			return ParseDocumentContentAsync();
		}
		return _ParseDocumentContentAsync_WhiteSpace(task);
	}

	private async Task<bool> _ParseDocumentContentAsync_WhiteSpace(Task<bool> task)
	{
		if (await task.ConfigureAwait(continueOnCapturedContext: false))
		{
			if (fragmentType == XmlNodeType.None && curNode.type == XmlNodeType.Text)
			{
				fragmentType = XmlNodeType.Element;
			}
			return true;
		}
		return await ParseDocumentContentAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<bool> ParseDocumentContentAsync_ReadData(bool needMoreChars)
	{
		if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0)
		{
			return await ParseDocumentContentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		if (needMoreChars)
		{
			Throw("Data at the root level is invalid.");
		}
		if (InEntity)
		{
			if (HandleEntityEnd(checkEntityNesting: true))
			{
				SetupEndEntityNodeInContent();
				return true;
			}
			return await ParseDocumentContentAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		if (!rootElementParsed && fragmentType == XmlNodeType.Document)
		{
			ThrowWithoutLineInfo("Root element is missing.");
		}
		if (fragmentType == XmlNodeType.None)
		{
			fragmentType = ((!rootElementParsed) ? XmlNodeType.Element : XmlNodeType.Document);
		}
		OnEof();
		return false;
	}

	private Task<bool> ParseElementContentAsync()
	{
		while (true)
		{
			int charPos = ps.charPos;
			char[] chars = ps.chars;
			switch (chars[charPos])
			{
			case '<':
				switch (chars[charPos + 1])
				{
				case '?':
					ps.charPos = charPos + 2;
					return ParsePIAsync().ContinueBoolTaskFuncWhenFalse(ParseElementContentAsync);
				case '!':
					charPos += 2;
					if (ps.charsUsed - charPos < 2)
					{
						return ParseElementContent_ReadData();
					}
					if (chars[charPos] == '-')
					{
						if (chars[charPos + 1] == '-')
						{
							ps.charPos = charPos + 2;
							return ParseCommentAsync().ContinueBoolTaskFuncWhenFalse(ParseElementContentAsync);
						}
						ThrowUnexpectedToken(charPos + 1, "-");
					}
					else if (chars[charPos] == '[')
					{
						charPos++;
						if (ps.charsUsed - charPos < 6)
						{
							return ParseElementContent_ReadData();
						}
						if (XmlConvert.StrEqual(chars, charPos, 6, "CDATA["))
						{
							ps.charPos = charPos + 6;
							return ParseCDataAsync().ReturnTaskBoolWhenFinish(ret: true);
						}
						ThrowUnexpectedToken(charPos, "CDATA[");
					}
					else if (ParseUnexpectedToken(charPos) == "DOCTYPE")
					{
						Throw("Unexpected DTD declaration.");
					}
					else
					{
						ThrowUnexpectedToken(charPos, "<!--", "<[CDATA[");
					}
					break;
				case '/':
					ps.charPos = charPos + 2;
					return ParseEndElementAsync().ReturnTaskBoolWhenFinish(ret: true);
				default:
					if (charPos + 1 == ps.charsUsed)
					{
						return ParseElementContent_ReadData();
					}
					ps.charPos = charPos + 1;
					return ParseElementAsync().ReturnTaskBoolWhenFinish(ret: true);
				}
				break;
			case '&':
				return ParseTextAsync().ContinueBoolTaskFuncWhenFalse(ParseElementContentAsync);
			default:
				if (charPos == ps.charsUsed)
				{
					return ParseElementContent_ReadData();
				}
				return ParseTextAsync().ContinueBoolTaskFuncWhenFalse(ParseElementContentAsync);
			}
		}
	}

	private async Task<bool> ParseElementContent_ReadData()
	{
		if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			if (ps.charsUsed - ps.charPos != 0)
			{
				ThrowUnclosedElements();
			}
			if (!InEntity)
			{
				if (index == 0 && fragmentType != XmlNodeType.Document)
				{
					OnEof();
					return false;
				}
				ThrowUnclosedElements();
			}
			if (HandleEntityEnd(checkEntityNesting: true))
			{
				SetupEndEntityNodeInContent();
				return true;
			}
		}
		return await ParseElementContentAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task ParseElementAsync()
	{
		int num = ps.charPos;
		char[] chars = ps.chars;
		int num2 = -1;
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[num]] & 4) != 0)
			{
				num++;
				while (true)
				{
					if ((xmlCharType.charProperties[(uint)chars[num]] & 8) != 0)
					{
						num++;
						continue;
					}
					if (chars[num] != ':')
					{
						break;
					}
					if (num2 == -1)
					{
						goto IL_009a;
					}
					if (!supportNamespaces)
					{
						num++;
						continue;
					}
					goto IL_007e;
				}
				if (num + 1 < ps.charsUsed)
				{
					break;
				}
			}
			goto IL_00b2;
			IL_009a:
			num2 = num;
			num++;
			continue;
			IL_00b2:
			Task<Tuple<int, int>> task = ParseQNameAsync();
			return ParseElementAsync_ContinueWithSetElement(task);
			IL_007e:
			Throw(num, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
			goto IL_00b2;
		}
		return ParseElementAsync_SetElement(num2, num);
	}

	private Task ParseElementAsync_ContinueWithSetElement(Task<Tuple<int, int>> task)
	{
		if (task.IsSuccess())
		{
			Tuple<int, int> result = task.Result;
			int item = result.Item1;
			int item2 = result.Item2;
			return ParseElementAsync_SetElement(item, item2);
		}
		return _ParseElementAsync_ContinueWithSetElement(task);
	}

	private async Task _ParseElementAsync_ContinueWithSetElement(Task<Tuple<int, int>> task)
	{
		Tuple<int, int> obj = await task.ConfigureAwait(continueOnCapturedContext: false);
		int item = obj.Item1;
		int item2 = obj.Item2;
		await ParseElementAsync_SetElement(item, item2).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task ParseElementAsync_SetElement(int colonPos, int pos)
	{
		char[] chars = ps.chars;
		namespaceManager.PushScope();
		if (colonPos == -1 || !supportNamespaces)
		{
			curNode.SetNamedNode(XmlNodeType.Element, nameTable.Add(chars, ps.charPos, pos - ps.charPos));
		}
		else
		{
			int charPos = ps.charPos;
			int num = colonPos - charPos;
			if (num == lastPrefix.Length && XmlConvert.StrEqual(chars, charPos, num, lastPrefix))
			{
				curNode.SetNamedNode(XmlNodeType.Element, nameTable.Add(chars, colonPos + 1, pos - colonPos - 1), lastPrefix, null);
			}
			else
			{
				curNode.SetNamedNode(XmlNodeType.Element, nameTable.Add(chars, colonPos + 1, pos - colonPos - 1), nameTable.Add(chars, ps.charPos, num), null);
				lastPrefix = curNode.prefix;
			}
		}
		char c = chars[pos];
		bool num2 = (xmlCharType.charProperties[(uint)c] & 1) != 0;
		ps.charPos = pos;
		if (num2)
		{
			return ParseAttributesAsync();
		}
		return ParseElementAsync_NoAttributes();
	}

	private Task ParseElementAsync_NoAttributes()
	{
		int charPos = ps.charPos;
		char[] chars = ps.chars;
		switch (chars[charPos])
		{
		case '>':
			ps.charPos = charPos + 1;
			parsingFunction = ParsingFunction.MoveToElementContent;
			break;
		case '/':
			if (charPos + 1 == ps.charsUsed)
			{
				ps.charPos = charPos;
				return ParseElementAsync_ReadData(charPos);
			}
			if (chars[charPos + 1] == '>')
			{
				curNode.IsEmptyElement = true;
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.PopEmptyElementContext;
				ps.charPos = charPos + 2;
			}
			else
			{
				ThrowUnexpectedToken(charPos, ">");
			}
			break;
		default:
			Throw(charPos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(chars, ps.charsUsed, charPos));
			break;
		}
		if (addDefaultAttributesAndNormalize)
		{
			AddDefaultAttributesAndNormalize();
		}
		ElementNamespaceLookup();
		return AsyncHelper.DoneTask;
	}

	private async Task ParseElementAsync_ReadData(int pos)
	{
		if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			Throw(pos, "Unexpected end of file while parsing {0} has occurred.", ">");
		}
		await ParseElementAsync_NoAttributes().ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task ParseEndElementAsync()
	{
		NodeData obj = nodes[index - 1];
		int length = obj.prefix.Length;
		int length2 = obj.localName.Length;
		if (ps.charsUsed - ps.charPos < length + length2 + 1)
		{
			return _ParseEndElmentAsync();
		}
		return ParseEndElementAsync_CheckNameAndParse();
	}

	private async Task _ParseEndElmentAsync()
	{
		await ParseEndElmentAsync_PrepareData().ConfigureAwait(continueOnCapturedContext: false);
		await ParseEndElementAsync_CheckNameAndParse().ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task ParseEndElmentAsync_PrepareData()
	{
		NodeData nodeData = nodes[index - 1];
		int prefLen = nodeData.prefix.Length;
		int locLen = nodeData.localName.Length;
		while (ps.charsUsed - ps.charPos < prefLen + locLen + 1 && await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0)
		{
		}
	}

	private Task ParseEndElementAsync_CheckNameAndParse()
	{
		NodeData nodeData = nodes[index - 1];
		int length = nodeData.prefix.Length;
		int length2 = nodeData.localName.Length;
		char[] chars = ps.chars;
		int nameLen;
		if (nodeData.prefix.Length == 0)
		{
			if (!XmlConvert.StrEqual(chars, ps.charPos, length2, nodeData.localName))
			{
				return ThrowTagMismatchAsync(nodeData);
			}
			nameLen = length2;
		}
		else
		{
			int num = ps.charPos + length;
			if (!XmlConvert.StrEqual(chars, ps.charPos, length, nodeData.prefix) || chars[num] != ':' || !XmlConvert.StrEqual(chars, num + 1, length2, nodeData.localName))
			{
				return ThrowTagMismatchAsync(nodeData);
			}
			nameLen = length2 + length + 1;
		}
		LineInfo endTagLineInfo = new LineInfo(ps.lineNo, ps.LinePos);
		return ParseEndElementAsync_Finish(nameLen, nodeData, endTagLineInfo);
	}

	private Task ParseEndElementAsync_Finish(int nameLen, NodeData startTagNode, LineInfo endTagLineInfo)
	{
		Task task = ParseEndElementAsync_CheckEndTag(nameLen, startTagNode, endTagLineInfo);
		while (task.IsSuccess())
		{
			switch (parseEndElement_NextFunc)
			{
			case ParseEndElementParseFunction.CheckEndTag:
				task = ParseEndElementAsync_CheckEndTag(nameLen, startTagNode, endTagLineInfo);
				break;
			case ParseEndElementParseFunction.ReadData:
				task = ParseEndElementAsync_ReadData();
				break;
			case ParseEndElementParseFunction.Done:
				return task;
			}
		}
		return ParseEndElementAsync_Finish(task, nameLen, startTagNode, endTagLineInfo);
	}

	private async Task ParseEndElementAsync_Finish(Task task, int nameLen, NodeData startTagNode, LineInfo endTagLineInfo)
	{
		while (true)
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			switch (parseEndElement_NextFunc)
			{
			case ParseEndElementParseFunction.CheckEndTag:
				task = ParseEndElementAsync_CheckEndTag(nameLen, startTagNode, endTagLineInfo);
				break;
			case ParseEndElementParseFunction.ReadData:
				task = ParseEndElementAsync_ReadData();
				break;
			case ParseEndElementParseFunction.Done:
				return;
			}
		}
	}

	private Task ParseEndElementAsync_CheckEndTag(int nameLen, NodeData startTagNode, LineInfo endTagLineInfo)
	{
		int num;
		while (true)
		{
			num = ps.charPos + nameLen;
			char[] chars = ps.chars;
			if (num == ps.charsUsed)
			{
				parseEndElement_NextFunc = ParseEndElementParseFunction.ReadData;
				return AsyncHelper.DoneTask;
			}
			bool flag = false;
			if ((xmlCharType.charProperties[(uint)chars[num]] & 8) != 0 || chars[num] == ':')
			{
				flag = true;
			}
			if (flag)
			{
				return ThrowTagMismatchAsync(startTagNode);
			}
			if (chars[num] != '>')
			{
				char c;
				while (xmlCharType.IsWhiteSpace(c = chars[num]))
				{
					num++;
					switch (c)
					{
					case '\n':
						OnNewLine(num);
						break;
					case '\r':
						if (chars[num] == '\n')
						{
							num++;
						}
						else if (num == ps.charsUsed && !ps.isEof)
						{
							break;
						}
						OnNewLine(num);
						break;
					}
				}
			}
			if (chars[num] == '>')
			{
				break;
			}
			if (num == ps.charsUsed)
			{
				parseEndElement_NextFunc = ParseEndElementParseFunction.ReadData;
				return AsyncHelper.DoneTask;
			}
			ThrowUnexpectedToken(num, ">");
		}
		index--;
		curNode = nodes[index];
		startTagNode.lineInfo = endTagLineInfo;
		startTagNode.type = XmlNodeType.EndElement;
		ps.charPos = num + 1;
		nextParsingFunction = ((index > 0) ? parsingFunction : ParsingFunction.DocumentContent);
		parsingFunction = ParsingFunction.PopElementContext;
		parseEndElement_NextFunc = ParseEndElementParseFunction.Done;
		return AsyncHelper.DoneTask;
	}

	private async Task ParseEndElementAsync_ReadData()
	{
		if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			ThrowUnclosedElements();
		}
		parseEndElement_NextFunc = ParseEndElementParseFunction.CheckEndTag;
	}

	private async Task ThrowTagMismatchAsync(NodeData startTag)
	{
		if (startTag.type == XmlNodeType.Element)
		{
			Tuple<int, int> obj = await ParseQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
			_ = obj.Item1;
			int item = obj.Item2;
			Throw("The '{0}' start tag on line {1} position {2} does not match the end tag of '{3}'.", new string[4]
			{
				startTag.GetNameWPrefix(nameTable),
				startTag.lineInfo.lineNo.ToString(CultureInfo.InvariantCulture),
				startTag.lineInfo.linePos.ToString(CultureInfo.InvariantCulture),
				new string(ps.chars, ps.charPos, item - ps.charPos)
			});
		}
		else
		{
			Throw("Unexpected end tag.");
		}
	}

	private async Task ParseAttributesAsync()
	{
		int pos = ps.charPos;
		char[] chars = ps.chars;
		while (true)
		{
			int num = 0;
			while (true)
			{
				char c;
				int num2;
				if ((xmlCharType.charProperties[(uint)(c = chars[pos])] & 1) != 0)
				{
					switch (c)
					{
					case '\n':
						OnNewLine(pos + 1);
						num++;
						goto IL_00f2;
					case '\r':
						if (chars[pos + 1] == '\n')
						{
							OnNewLine(pos + 2);
							num++;
							pos++;
							goto IL_00f2;
						}
						if (pos + 1 != ps.charsUsed)
						{
							OnNewLine(pos + 1);
							num++;
							goto IL_00f2;
						}
						break;
					default:
						goto IL_00f2;
					}
					ps.charPos = pos;
				}
				else
				{
					num2 = 0;
					char c2;
					if ((xmlCharType.charProperties[(uint)(c2 = chars[pos])] & 4) != 0)
					{
						num2 = 1;
					}
					if (num2 != 0)
					{
						goto IL_0246;
					}
					if (c2 == '>')
					{
						ps.charPos = pos + 1;
						parsingFunction = ParsingFunction.MoveToElementContent;
						goto IL_0934;
					}
					if (c2 == '/')
					{
						if (pos + 1 != ps.charsUsed)
						{
							if (chars[pos + 1] == '>')
							{
								ps.charPos = pos + 2;
								curNode.IsEmptyElement = true;
								nextParsingFunction = parsingFunction;
								parsingFunction = ParsingFunction.PopEmptyElementContext;
								goto IL_0934;
							}
							ThrowUnexpectedToken(pos + 1, ">");
							goto IL_0246;
						}
					}
					else if (pos != ps.charsUsed)
					{
						if (c2 != ':' || supportNamespaces)
						{
							Throw(pos, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(chars, ps.charsUsed, pos));
						}
						goto IL_0246;
					}
				}
				ps.lineNo -= num;
				if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0)
				{
					pos = ps.charPos;
					chars = ps.chars;
				}
				else
				{
					ThrowUnclosedElements();
				}
				break;
				IL_0934:
				if (addDefaultAttributesAndNormalize)
				{
					AddDefaultAttributesAndNormalize();
				}
				ElementNamespaceLookup();
				if (attrNeedNamespaceLookup)
				{
					AttributeNamespaceLookup();
					attrNeedNamespaceLookup = false;
				}
				if (attrDuplWalkCount >= 250)
				{
					AttributeDuplCheck();
				}
				return;
				IL_00f2:
				pos++;
				continue;
				IL_0246:
				if (pos == ps.charPos)
				{
					ThrowExpectingWhitespace(pos);
				}
				ps.charPos = pos;
				int attrNameLinePos = ps.LinePos;
				int num3 = -1;
				pos += num2;
				while (true)
				{
					char c3;
					if ((xmlCharType.charProperties[(uint)(c3 = chars[pos])] & 8) != 0)
					{
						pos++;
						continue;
					}
					if (c3 == ':')
					{
						if (num3 != -1)
						{
							if (supportNamespaces)
							{
								Throw(pos, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
								break;
							}
							pos++;
							continue;
						}
						num3 = pos;
						pos++;
						if ((xmlCharType.charProperties[(uint)chars[pos]] & 4) != 0)
						{
							pos++;
							continue;
						}
						Tuple<int, int> tuple = await ParseQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
						num3 = tuple.Item1;
						pos = tuple.Item2;
						chars = ps.chars;
						break;
					}
					if (pos + 1 >= ps.charsUsed)
					{
						Tuple<int, int> tuple2 = await ParseQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
						num3 = tuple2.Item1;
						pos = tuple2.Item2;
						chars = ps.chars;
					}
					break;
				}
				NodeData attr = AddAttribute(pos, num3);
				attr.SetLineInfo(ps.LineNo, attrNameLinePos);
				if (chars[pos] != '=')
				{
					ps.charPos = pos;
					await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
					pos = ps.charPos;
					if (chars[pos] != '=')
					{
						ThrowUnexpectedToken("=");
					}
				}
				pos++;
				char c4 = chars[pos];
				if (c4 != '"' && c4 != '\'')
				{
					ps.charPos = pos;
					await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
					pos = ps.charPos;
					c4 = chars[pos];
					if (c4 != '"' && c4 != '\'')
					{
						ThrowUnexpectedToken("\"", "'");
					}
				}
				pos++;
				ps.charPos = pos;
				attr.quoteChar = c4;
				attr.SetLineInfo2(ps.LineNo, ps.LinePos);
				char c5;
				while ((xmlCharType.charProperties[(uint)(c5 = chars[pos])] & 0x80) != 0)
				{
					pos++;
				}
				if (c5 == c4)
				{
					attr.SetValue(chars, ps.charPos, pos - ps.charPos);
					pos++;
					ps.charPos = pos;
				}
				else
				{
					await ParseAttributeValueSlowAsync(pos, c4, attr).ConfigureAwait(continueOnCapturedContext: false);
					pos = ps.charPos;
					chars = ps.chars;
				}
				if (attr.prefix.Length == 0)
				{
					if (Ref.Equal(attr.localName, XmlNs))
					{
						OnDefaultNamespaceDecl(attr);
					}
				}
				else if (Ref.Equal(attr.prefix, XmlNs))
				{
					OnNamespaceDecl(attr);
				}
				else if (Ref.Equal(attr.prefix, Xml))
				{
					OnXmlReservedAttribute(attr);
				}
				break;
			}
		}
	}

	private async Task ParseAttributeValueSlowAsync(int curPos, char quoteChar, NodeData attr)
	{
		int pos = curPos;
		char[] chars = ps.chars;
		int attributeBaseEntityId = ps.entityId;
		int valueChunkStartPos = 0;
		LineInfo valueChunkLineInfo = new LineInfo(ps.lineNo, ps.LinePos);
		NodeData lastChunk = null;
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)chars[pos]] & 0x80) != 0)
			{
				pos++;
				continue;
			}
			if (pos - ps.charPos > 0)
			{
				stringBuilder.Append(chars, ps.charPos, pos - ps.charPos);
				ps.charPos = pos;
			}
			if (chars[pos] == quoteChar && attributeBaseEntityId == ps.entityId)
			{
				break;
			}
			switch (chars[pos])
			{
			case '\n':
				pos++;
				OnNewLine(pos);
				if (normalize)
				{
					stringBuilder.Append(' ');
					ps.charPos++;
				}
				continue;
			case '\r':
				if (chars[pos + 1] == '\n')
				{
					pos += 2;
					if (normalize)
					{
						stringBuilder.Append(ps.eolNormalized ? "  " : " ");
						ps.charPos = pos;
					}
				}
				else
				{
					if (pos + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					pos++;
					if (normalize)
					{
						stringBuilder.Append(' ');
						ps.charPos = pos;
					}
				}
				OnNewLine(pos);
				continue;
			case '\t':
				pos++;
				if (normalize)
				{
					stringBuilder.Append(' ');
					ps.charPos++;
				}
				continue;
			case '"':
			case '\'':
			case '>':
				pos++;
				continue;
			case '<':
				Throw(pos, "'{0}', hexadecimal value {1}, is an invalid attribute character.", XmlException.BuildCharExceptionArgs('<', '\0'));
				break;
			case '&':
			{
				if (pos - ps.charPos > 0)
				{
					stringBuilder.Append(chars, ps.charPos, pos - ps.charPos);
				}
				ps.charPos = pos;
				int enclosingEntityId = ps.entityId;
				LineInfo entityLineInfo = new LineInfo(ps.lineNo, ps.LinePos + 1);
				Tuple<int, EntityType> tuple = await HandleEntityReferenceAsync(isInAttributeValue: true, EntityExpandType.All).ConfigureAwait(continueOnCapturedContext: false);
				pos = tuple.Item1;
				switch (tuple.Item2)
				{
				case EntityType.Unexpanded:
					if (parsingMode == ParsingMode.Full && ps.entityId == attributeBaseEntityId)
					{
						int num2 = stringBuilder.Length - valueChunkStartPos;
						if (num2 > 0)
						{
							NodeData nodeData3 = new NodeData();
							nodeData3.lineInfo = valueChunkLineInfo;
							nodeData3.depth = attr.depth + 1;
							nodeData3.SetValueNode(XmlNodeType.Text, stringBuilder.ToString(valueChunkStartPos, num2));
							AddAttributeChunkToList(attr, nodeData3, ref lastChunk);
						}
						ps.charPos++;
						string text = await ParseEntityNameAsync().ConfigureAwait(continueOnCapturedContext: false);
						NodeData nodeData4 = new NodeData();
						nodeData4.lineInfo = entityLineInfo;
						nodeData4.depth = attr.depth + 1;
						nodeData4.SetNamedNode(XmlNodeType.EntityReference, text);
						AddAttributeChunkToList(attr, nodeData4, ref lastChunk);
						stringBuilder.Append('&');
						stringBuilder.Append(text);
						stringBuilder.Append(';');
						valueChunkStartPos = stringBuilder.Length;
						valueChunkLineInfo.Set(ps.LineNo, ps.LinePos);
						fullAttrCleanup = true;
					}
					else
					{
						ps.charPos++;
						await ParseEntityNameAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					pos = ps.charPos;
					break;
				case EntityType.ExpandedInAttribute:
					if (parsingMode == ParsingMode.Full && enclosingEntityId == attributeBaseEntityId)
					{
						int num = stringBuilder.Length - valueChunkStartPos;
						if (num > 0)
						{
							NodeData nodeData = new NodeData();
							nodeData.lineInfo = valueChunkLineInfo;
							nodeData.depth = attr.depth + 1;
							nodeData.SetValueNode(XmlNodeType.Text, stringBuilder.ToString(valueChunkStartPos, num));
							AddAttributeChunkToList(attr, nodeData, ref lastChunk);
						}
						NodeData nodeData2 = new NodeData();
						nodeData2.lineInfo = entityLineInfo;
						nodeData2.depth = attr.depth + 1;
						nodeData2.SetNamedNode(XmlNodeType.EntityReference, ps.entity.Name);
						AddAttributeChunkToList(attr, nodeData2, ref lastChunk);
						fullAttrCleanup = true;
					}
					pos = ps.charPos;
					break;
				default:
					pos = ps.charPos;
					break;
				case EntityType.CharacterDec:
				case EntityType.CharacterHex:
				case EntityType.CharacterNamed:
					break;
				}
				chars = ps.chars;
				continue;
			}
			default:
				if (pos == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[pos]))
				{
					if (pos + 1 == ps.charsUsed)
					{
						break;
					}
					pos++;
					if (XmlCharType.IsLowSurrogate(chars[pos]))
					{
						pos++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, pos);
				break;
			}
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				if (ps.charsUsed - ps.charPos > 0)
				{
					if (ps.chars[ps.charPos] != '\r')
					{
						Throw("Unexpected end of file has occurred.");
					}
				}
				else
				{
					if (!InEntity)
					{
						if (fragmentType == XmlNodeType.Attribute)
						{
							if (attributeBaseEntityId != ps.entityId)
							{
								Throw("Entity replacement text must nest properly within markup declarations.");
							}
							break;
						}
						Throw("There is an unclosed literal string.");
					}
					if (HandleEntityEnd(checkEntityNesting: true))
					{
						Throw("An internal error has occurred.");
					}
					if (attributeBaseEntityId == ps.entityId)
					{
						valueChunkStartPos = stringBuilder.Length;
						valueChunkLineInfo.Set(ps.LineNo, ps.LinePos);
					}
				}
			}
			pos = ps.charPos;
			chars = ps.chars;
		}
		if (attr.nextAttrValueChunk != null)
		{
			int num3 = stringBuilder.Length - valueChunkStartPos;
			if (num3 > 0)
			{
				NodeData nodeData5 = new NodeData();
				nodeData5.lineInfo = valueChunkLineInfo;
				nodeData5.depth = attr.depth + 1;
				nodeData5.SetValueNode(XmlNodeType.Text, stringBuilder.ToString(valueChunkStartPos, num3));
				AddAttributeChunkToList(attr, nodeData5, ref lastChunk);
			}
		}
		ps.charPos = pos + 1;
		attr.SetValue(stringBuilder.ToString());
		stringBuilder.Length = 0;
	}

	private Task<bool> ParseTextAsync()
	{
		int outOrChars = 0;
		if (parsingMode != ParsingMode.Full)
		{
			return _ParseTextAsync(null);
		}
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		Task<Tuple<int, int, int, bool>> task = ParseTextAsync(outOrChars);
		bool flag = false;
		if (!task.IsSuccess())
		{
			return _ParseTextAsync(task);
		}
		Tuple<int, int, int, bool> result = task.Result;
		int item = result.Item1;
		int item2 = result.Item2;
		outOrChars = result.Item3;
		if (result.Item4)
		{
			if (item2 - item == 0)
			{
				return ParseTextAsync_IgnoreNode();
			}
			XmlNodeType textNodeType = GetTextNodeType(outOrChars);
			if (textNodeType == XmlNodeType.None)
			{
				return ParseTextAsync_IgnoreNode();
			}
			curNode.SetValueNode(textNodeType, ps.chars, item, item2 - item);
			return AsyncHelper.DoneTaskTrue;
		}
		return _ParseTextAsync(task);
	}

	private async Task<bool> _ParseTextAsync(Task<Tuple<int, int, int, bool>> parseTask)
	{
		int outOrChars = 0;
		if (parseTask == null)
		{
			if (parsingMode != ParsingMode.Full)
			{
				Tuple<int, int, int, bool> tuple;
				do
				{
					tuple = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
					_ = tuple.Item1;
					_ = tuple.Item2;
					outOrChars = tuple.Item3;
				}
				while (!tuple.Item4);
				goto IL_0539;
			}
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			parseTask = ParseTextAsync(outOrChars);
		}
		Tuple<int, int, int, bool> obj = await parseTask.ConfigureAwait(continueOnCapturedContext: false);
		int item = obj.Item1;
		int item2 = obj.Item2;
		outOrChars = obj.Item3;
		if (obj.Item4)
		{
			if (item2 - item != 0)
			{
				XmlNodeType textNodeType = GetTextNodeType(outOrChars);
				if (textNodeType != XmlNodeType.None)
				{
					curNode.SetValueNode(textNodeType, ps.chars, item, item2 - item);
					return true;
				}
			}
		}
		else if (v1Compat)
		{
			Tuple<int, int, int, bool> tuple2;
			do
			{
				if (item2 - item > 0)
				{
					stringBuilder.Append(ps.chars, item, item2 - item);
				}
				tuple2 = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
				item = tuple2.Item1;
				item2 = tuple2.Item2;
				outOrChars = tuple2.Item3;
			}
			while (!tuple2.Item4);
			if (item2 - item > 0)
			{
				stringBuilder.Append(ps.chars, item, item2 - item);
			}
			XmlNodeType textNodeType2 = GetTextNodeType(outOrChars);
			if (textNodeType2 != XmlNodeType.None)
			{
				curNode.SetValueNode(textNodeType2, stringBuilder.ToString());
				stringBuilder.Length = 0;
				return true;
			}
			stringBuilder.Length = 0;
		}
		else
		{
			if (outOrChars > 32)
			{
				curNode.SetValueNode(XmlNodeType.Text, ps.chars, item, item2 - item);
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.PartialTextValue;
				return true;
			}
			if (item2 - item > 0)
			{
				stringBuilder.Append(ps.chars, item, item2 - item);
			}
			bool item3;
			do
			{
				Tuple<int, int, int, bool> obj2 = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
				item = obj2.Item1;
				item2 = obj2.Item2;
				outOrChars = obj2.Item3;
				item3 = obj2.Item4;
				if (item2 - item > 0)
				{
					stringBuilder.Append(ps.chars, item, item2 - item);
				}
			}
			while (!item3 && outOrChars <= 32 && stringBuilder.Length < 4096);
			XmlNodeType xmlNodeType = ((stringBuilder.Length < 4096) ? GetTextNodeType(outOrChars) : XmlNodeType.Text);
			if (xmlNodeType != XmlNodeType.None)
			{
				curNode.SetValueNode(xmlNodeType, stringBuilder.ToString());
				stringBuilder.Length = 0;
				if (!item3)
				{
					nextParsingFunction = parsingFunction;
					parsingFunction = ParsingFunction.PartialTextValue;
				}
				return true;
			}
			stringBuilder.Length = 0;
			if (!item3)
			{
				Tuple<int, int, int, bool> tuple3;
				do
				{
					tuple3 = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
					_ = tuple3.Item1;
					_ = tuple3.Item2;
					outOrChars = tuple3.Item3;
				}
				while (!tuple3.Item4);
			}
		}
		goto IL_0539;
		IL_0539:
		return await ParseTextAsync_IgnoreNode().ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task<bool> ParseTextAsync_IgnoreNode()
	{
		if (parsingFunction == ParsingFunction.ReportEndEntity)
		{
			SetupEndEntityNodeInContent();
			parsingFunction = nextParsingFunction;
			return AsyncHelper.DoneTaskTrue;
		}
		if (parsingFunction == ParsingFunction.EntityReference)
		{
			parsingFunction = nextNextParsingFunction;
			return ParseEntityReferenceAsync().ReturnTaskBoolWhenFinish(ret: true);
		}
		return AsyncHelper.DoneTaskFalse;
	}

	private Task<Tuple<int, int, int, bool>> ParseTextAsync(int outOrChars)
	{
		Task<Tuple<int, int, int, bool>> task = ParseTextAsync(outOrChars, ps.chars, ps.charPos, 0, -1, outOrChars, '\0');
		while (task.IsSuccess())
		{
			outOrChars = lastParseTextState.outOrChars;
			char[] chars = lastParseTextState.chars;
			int pos = lastParseTextState.pos;
			int rcount = lastParseTextState.rcount;
			int rpos = lastParseTextState.rpos;
			int orChars = lastParseTextState.orChars;
			char c = lastParseTextState.c;
			switch (parseText_NextFunction)
			{
			case ParseTextFunction.ParseText:
				task = ParseTextAsync(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.Entity:
				task = ParseTextAsync_ParseEntity(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.ReadData:
				task = ParseTextAsync_ReadData(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.Surrogate:
				task = ParseTextAsync_Surrogate(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.NoValue:
				return ParseTextAsync_NoValue(outOrChars, pos);
			case ParseTextFunction.PartialValue:
				return ParseTextAsync_PartialValue(pos, rcount, rpos, orChars, c);
			}
		}
		return ParseTextAsync_AsyncFunc(task);
	}

	private async Task<Tuple<int, int, int, bool>> ParseTextAsync_AsyncFunc(Task<Tuple<int, int, int, bool>> task)
	{
		while (true)
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			int outOrChars = lastParseTextState.outOrChars;
			char[] chars = lastParseTextState.chars;
			int pos = lastParseTextState.pos;
			int rcount = lastParseTextState.rcount;
			int rpos = lastParseTextState.rpos;
			int orChars = lastParseTextState.orChars;
			char c = lastParseTextState.c;
			switch (parseText_NextFunction)
			{
			case ParseTextFunction.ParseText:
				task = ParseTextAsync(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.Entity:
				task = ParseTextAsync_ParseEntity(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.ReadData:
				task = ParseTextAsync_ReadData(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.Surrogate:
				task = ParseTextAsync_Surrogate(outOrChars, chars, pos, rcount, rpos, orChars, c);
				break;
			case ParseTextFunction.NoValue:
				return await ParseTextAsync_NoValue(outOrChars, pos).ConfigureAwait(continueOnCapturedContext: false);
			case ParseTextFunction.PartialValue:
				return await ParseTextAsync_PartialValue(pos, rcount, rpos, orChars, c).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}

	private Task<Tuple<int, int, int, bool>> ParseTextAsync(int outOrChars, char[] chars, int pos, int rcount, int rpos, int orChars, char c)
	{
		while (true)
		{
			if ((xmlCharType.charProperties[(uint)(c = chars[pos])] & 0x40) != 0)
			{
				orChars |= c;
				pos++;
				continue;
			}
			switch (c)
			{
			case '\t':
				pos++;
				break;
			case '\n':
				pos++;
				OnNewLine(pos);
				break;
			case '\r':
				if (chars[pos + 1] == '\n')
				{
					if (!ps.eolNormalized && parsingMode == ParsingMode.Full)
					{
						if (pos - ps.charPos > 0)
						{
							if (rcount == 0)
							{
								rcount = 1;
								rpos = pos;
							}
							else
							{
								ShiftBuffer(rpos + rcount, rpos, pos - rpos - rcount);
								rpos = pos - rcount;
								rcount++;
							}
						}
						else
						{
							ps.charPos++;
						}
					}
					pos += 2;
				}
				else
				{
					if (pos + 1 >= ps.charsUsed && !ps.isEof)
					{
						lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
						parseText_NextFunction = ParseTextFunction.ReadData;
						return parseText_dummyTask;
					}
					if (!ps.eolNormalized)
					{
						chars[pos] = '\n';
					}
					pos++;
				}
				OnNewLine(pos);
				break;
			case '<':
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.PartialValue;
				return parseText_dummyTask;
			case '&':
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.Entity;
				return parseText_dummyTask;
			case ']':
				if (ps.charsUsed - pos < 3 && !ps.isEof)
				{
					lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
					parseText_NextFunction = ParseTextFunction.ReadData;
					return parseText_dummyTask;
				}
				if (chars[pos + 1] == ']' && chars[pos + 2] == '>')
				{
					Throw(pos, "']]>' is not allowed in character data.");
				}
				orChars |= 0x5D;
				pos++;
				break;
			default:
				if (pos == ps.charsUsed)
				{
					lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
					parseText_NextFunction = ParseTextFunction.ReadData;
					return parseText_dummyTask;
				}
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.Surrogate;
				return parseText_dummyTask;
			}
		}
	}

	private async Task<Tuple<int, int, int, bool>> ParseTextAsync_ParseEntity(int outOrChars, char[] chars, int pos, int rcount, int rpos, int orChars, char c)
	{
		int num;
		if ((num = ParseCharRefInline(pos, out var charCount, out var entityType)) > 0)
		{
			if (rcount > 0)
			{
				ShiftBuffer(rpos + rcount, rpos, pos - rpos - rcount);
			}
			rpos = pos - rcount;
			rcount += num - pos - charCount;
			pos = num;
			if (!xmlCharType.IsWhiteSpace(chars[num - charCount]) || (v1Compat && entityType == EntityType.CharacterDec))
			{
				orChars |= 0xFF;
			}
		}
		else
		{
			if (pos > ps.charPos)
			{
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.PartialValue;
				return parseText_dummyTask.Result;
			}
			Tuple<int, EntityType> tuple = await HandleEntityReferenceAsync(isInAttributeValue: false, EntityExpandType.All).ConfigureAwait(continueOnCapturedContext: false);
			pos = tuple.Item1;
			switch (tuple.Item2)
			{
			case EntityType.Unexpanded:
				nextParsingFunction = parsingFunction;
				parsingFunction = ParsingFunction.EntityReference;
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.NoValue;
				return parseText_dummyTask.Result;
			case EntityType.CharacterDec:
				if (v1Compat)
				{
					orChars |= 0xFF;
					break;
				}
				goto case EntityType.CharacterHex;
			case EntityType.CharacterHex:
			case EntityType.CharacterNamed:
				if (!xmlCharType.IsWhiteSpace(ps.chars[pos - 1]))
				{
					orChars |= 0xFF;
				}
				break;
			default:
				pos = ps.charPos;
				break;
			}
			chars = ps.chars;
		}
		lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
		parseText_NextFunction = ParseTextFunction.ParseText;
		return parseText_dummyTask.Result;
	}

	private async Task<Tuple<int, int, int, bool>> ParseTextAsync_Surrogate(int outOrChars, char[] chars, int pos, int rcount, int rpos, int orChars, char c)
	{
		char c2 = chars[pos];
		if (XmlCharType.IsHighSurrogate(c2))
		{
			if (pos + 1 == ps.charsUsed)
			{
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.ReadData;
				return parseText_dummyTask.Result;
			}
			pos++;
			if (XmlCharType.IsLowSurrogate(chars[pos]))
			{
				pos++;
				orChars |= c2;
				lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
				parseText_NextFunction = ParseTextFunction.ParseText;
				return parseText_dummyTask.Result;
			}
		}
		int offset = pos - ps.charPos;
		if (await ZeroEndingStreamAsync(pos).ConfigureAwait(continueOnCapturedContext: false))
		{
			chars = ps.chars;
			pos = ps.charPos + offset;
			lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
			parseText_NextFunction = ParseTextFunction.PartialValue;
			return parseText_dummyTask.Result;
		}
		ThrowInvalidChar(ps.chars, ps.charsUsed, ps.charPos + offset);
		throw new Exception();
	}

	private async Task<Tuple<int, int, int, bool>> ParseTextAsync_ReadData(int outOrChars, char[] chars, int pos, int rcount, int rpos, int orChars, char c)
	{
		if (pos > ps.charPos)
		{
			lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
			parseText_NextFunction = ParseTextFunction.PartialValue;
			return parseText_dummyTask.Result;
		}
		if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			if (ps.charsUsed - ps.charPos > 0)
			{
				if (ps.chars[ps.charPos] != '\r' && ps.chars[ps.charPos] != ']')
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			else
			{
				if (!InEntity)
				{
					lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
					parseText_NextFunction = ParseTextFunction.NoValue;
					return parseText_dummyTask.Result;
				}
				if (HandleEntityEnd(checkEntityNesting: true))
				{
					nextParsingFunction = parsingFunction;
					parsingFunction = ParsingFunction.ReportEndEntity;
					lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
					parseText_NextFunction = ParseTextFunction.NoValue;
					return parseText_dummyTask.Result;
				}
			}
		}
		pos = ps.charPos;
		chars = ps.chars;
		lastParseTextState = new ParseTextState(outOrChars, chars, pos, rcount, rpos, orChars, c);
		parseText_NextFunction = ParseTextFunction.ParseText;
		return parseText_dummyTask.Result;
	}

	private Task<Tuple<int, int, int, bool>> ParseTextAsync_NoValue(int outOrChars, int pos)
	{
		return Task.FromResult(new Tuple<int, int, int, bool>(pos, pos, outOrChars, item4: true));
	}

	private Task<Tuple<int, int, int, bool>> ParseTextAsync_PartialValue(int pos, int rcount, int rpos, int orChars, char c)
	{
		if (parsingMode == ParsingMode.Full && rcount > 0)
		{
			ShiftBuffer(rpos + rcount, rpos, pos - rpos - rcount);
		}
		int charPos = ps.charPos;
		int item = pos - rcount;
		ps.charPos = pos;
		return Task.FromResult(new Tuple<int, int, int, bool>(charPos, item, orChars, c == '<'));
	}

	private async Task FinishPartialValueAsync()
	{
		curNode.CopyTo(readValueOffset, stringBuilder);
		int outOrChars = 0;
		Tuple<int, int, int, bool> tuple = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
		int item = tuple.Item1;
		int item2 = tuple.Item2;
		outOrChars = tuple.Item3;
		while (!tuple.Item4)
		{
			stringBuilder.Append(ps.chars, item, item2 - item);
			tuple = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
			item = tuple.Item1;
			item2 = tuple.Item2;
			outOrChars = tuple.Item3;
		}
		stringBuilder.Append(ps.chars, item, item2 - item);
		curNode.SetValue(stringBuilder.ToString());
		stringBuilder.Length = 0;
	}

	private async Task FinishOtherValueIteratorAsync()
	{
		switch (parsingFunction)
		{
		case ParsingFunction.InReadValueChunk:
			if (incReadState == IncrementalReadState.ReadValueChunk_OnPartialValue)
			{
				await FinishPartialValueAsync().ConfigureAwait(continueOnCapturedContext: false);
				incReadState = IncrementalReadState.ReadValueChunk_OnCachedValue;
			}
			else if (readValueOffset > 0)
			{
				curNode.SetValue(curNode.StringValue.Substring(readValueOffset));
				readValueOffset = 0;
			}
			break;
		case ParsingFunction.InReadContentAsBinary:
		case ParsingFunction.InReadElementContentAsBinary:
			switch (incReadState)
			{
			case IncrementalReadState.ReadContentAsBinary_OnPartialValue:
				await FinishPartialValueAsync().ConfigureAwait(continueOnCapturedContext: false);
				incReadState = IncrementalReadState.ReadContentAsBinary_OnCachedValue;
				break;
			case IncrementalReadState.ReadContentAsBinary_OnCachedValue:
				if (readValueOffset > 0)
				{
					curNode.SetValue(curNode.StringValue.Substring(readValueOffset));
					readValueOffset = 0;
				}
				break;
			case IncrementalReadState.ReadContentAsBinary_End:
				curNode.SetValue(string.Empty);
				break;
			}
			break;
		case ParsingFunction.InReadAttributeValue:
			break;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private async Task SkipPartialTextValueAsync()
	{
		int outOrChars = 0;
		parsingFunction = nextParsingFunction;
		Tuple<int, int, int, bool> tuple;
		do
		{
			tuple = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
			_ = tuple.Item1;
			_ = tuple.Item2;
			outOrChars = tuple.Item3;
		}
		while (!tuple.Item4);
	}

	private Task FinishReadValueChunkAsync()
	{
		readValueOffset = 0;
		if (incReadState == IncrementalReadState.ReadValueChunk_OnPartialValue)
		{
			return SkipPartialTextValueAsync();
		}
		parsingFunction = nextParsingFunction;
		nextParsingFunction = nextNextParsingFunction;
		return AsyncHelper.DoneTask;
	}

	private async Task FinishReadContentAsBinaryAsync()
	{
		readValueOffset = 0;
		if (incReadState == IncrementalReadState.ReadContentAsBinary_OnPartialValue)
		{
			await SkipPartialTextValueAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			parsingFunction = nextParsingFunction;
			nextParsingFunction = nextNextParsingFunction;
		}
		if (incReadState != IncrementalReadState.ReadContentAsBinary_End)
		{
			while (await MoveToNextContentNodeAsync(moveIfOnContentNode: true).ConfigureAwait(continueOnCapturedContext: false))
			{
			}
		}
	}

	private async Task FinishReadElementContentAsBinaryAsync()
	{
		await FinishReadContentAsBinaryAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (curNode.type != XmlNodeType.EndElement)
		{
			Throw("'{0}' is an invalid XmlNodeType.", curNode.type.ToString());
		}
		await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<bool> ParseRootLevelWhitespaceAsync()
	{
		XmlNodeType nodeType = GetWhitespaceType();
		if (nodeType == XmlNodeType.None)
		{
			await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
			bool flag = ps.chars[ps.charPos] == '<' || ps.charsUsed - ps.charPos == 0;
			if (!flag)
			{
				flag = await ZeroEndingStreamAsync(ps.charPos).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (flag)
			{
				return false;
			}
		}
		else
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			await EatWhitespacesAsync(stringBuilder).ConfigureAwait(continueOnCapturedContext: false);
			bool flag = ps.chars[ps.charPos] == '<' || ps.charsUsed - ps.charPos == 0;
			if (!flag)
			{
				flag = await ZeroEndingStreamAsync(ps.charPos).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (flag)
			{
				if (stringBuilder.Length > 0)
				{
					curNode.SetValueNode(nodeType, stringBuilder.ToString());
					stringBuilder.Length = 0;
					return true;
				}
				return false;
			}
		}
		if (xmlCharType.IsCharData(ps.chars[ps.charPos]))
		{
			Throw("Data at the root level is invalid.");
		}
		else
		{
			ThrowInvalidChar(ps.chars, ps.charsUsed, ps.charPos);
		}
		return false;
	}

	private async Task ParseEntityReferenceAsync()
	{
		ps.charPos++;
		curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		NodeData nodeData = curNode;
		nodeData.SetNamedNode(XmlNodeType.EntityReference, await ParseEntityNameAsync().ConfigureAwait(continueOnCapturedContext: false));
	}

	private async Task<Tuple<int, EntityType>> HandleEntityReferenceAsync(bool isInAttributeValue, EntityExpandType expandType)
	{
		if (ps.charPos + 1 == ps.charsUsed && await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			Throw("Unexpected end of file has occurred.");
		}
		int charRefEndPos;
		if (ps.chars[ps.charPos + 1] == '#')
		{
			Tuple<EntityType, int> tuple = await ParseNumericCharRefAsync(expandType != EntityExpandType.OnlyGeneral, null).ConfigureAwait(continueOnCapturedContext: false);
			EntityType item = tuple.Item1;
			charRefEndPos = tuple.Item2;
			return new Tuple<int, EntityType>(charRefEndPos, item);
		}
		charRefEndPos = await ParseNamedCharRefAsync(expandType != EntityExpandType.OnlyGeneral, null).ConfigureAwait(continueOnCapturedContext: false);
		if (charRefEndPos >= 0)
		{
			return new Tuple<int, EntityType>(charRefEndPos, EntityType.CharacterNamed);
		}
		if (expandType == EntityExpandType.OnlyCharacter || (entityHandling != EntityHandling.ExpandEntities && (!isInAttributeValue || !validatingReaderCompatFlag)))
		{
			return new Tuple<int, EntityType>(charRefEndPos, EntityType.Unexpanded);
		}
		ps.charPos++;
		int savedLinePos = ps.LinePos;
		int num;
		try
		{
			num = await ParseNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (XmlException)
		{
			Throw("An error occurred while parsing EntityName.", ps.LineNo, savedLinePos);
			return new Tuple<int, EntityType>(charRefEndPos, EntityType.Skipped);
		}
		if (ps.chars[num] != ';')
		{
			ThrowUnexpectedToken(num, ";");
		}
		int linePos = ps.LinePos;
		string name = nameTable.Add(ps.chars, ps.charPos, num - ps.charPos);
		ps.charPos = num + 1;
		charRefEndPos = -1;
		EntityType item2 = await HandleGeneralEntityReferenceAsync(name, isInAttributeValue, pushFakeEntityIfNullResolver: false, linePos).ConfigureAwait(continueOnCapturedContext: false);
		reportedBaseUri = ps.baseUriStr;
		reportedEncoding = ps.encoding;
		return new Tuple<int, EntityType>(charRefEndPos, item2);
	}

	private async Task<EntityType> HandleGeneralEntityReferenceAsync(string name, bool isInAttributeValue, bool pushFakeEntityIfNullResolver, int entityStartLinePos)
	{
		IDtdEntityInfo entity = null;
		if (dtdInfo == null && fragmentParserContext != null && fragmentParserContext.HasDtdInfo && dtdProcessing == DtdProcessing.Parse)
		{
			await ParseDtdFromParserContextAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		if (dtdInfo != null)
		{
			IDtdEntityInfo dtdEntityInfo;
			entity = (dtdEntityInfo = dtdInfo.LookupEntity(name));
			if (dtdEntityInfo != null)
			{
				goto IL_012e;
			}
		}
		if (disableUndeclaredEntityCheck)
		{
			SchemaEntity schemaEntity = new SchemaEntity(new XmlQualifiedName(name), isParameter: false);
			schemaEntity.Text = string.Empty;
			entity = schemaEntity;
		}
		else
		{
			Throw("Reference to undeclared entity '{0}'.", name, ps.LineNo, entityStartLinePos);
		}
		goto IL_012e;
		IL_012e:
		if (entity.IsUnparsedEntity)
		{
			if (disableUndeclaredEntityCheck)
			{
				SchemaEntity schemaEntity2 = new SchemaEntity(new XmlQualifiedName(name), isParameter: false);
				schemaEntity2.Text = string.Empty;
				entity = schemaEntity2;
			}
			else
			{
				Throw("Reference to unparsed entity '{0}'.", name, ps.LineNo, entityStartLinePos);
			}
		}
		if (standalone && entity.IsDeclaredInExternal)
		{
			Throw("Standalone document declaration must have a value of 'no' because an external entity '{0}' is referenced.", entity.Name, ps.LineNo, entityStartLinePos);
		}
		if (entity.IsExternal)
		{
			if (isInAttributeValue)
			{
				Throw("External entity '{0}' reference cannot appear in the attribute value.", name, ps.LineNo, entityStartLinePos);
				return EntityType.Skipped;
			}
			if (parsingMode == ParsingMode.SkipContent)
			{
				return EntityType.Skipped;
			}
			if (IsResolverNull)
			{
				if (pushFakeEntityIfNullResolver)
				{
					await PushExternalEntityAsync(entity).ConfigureAwait(continueOnCapturedContext: false);
					curNode.entityId = ps.entityId;
					return EntityType.FakeExpanded;
				}
				return EntityType.Skipped;
			}
			await PushExternalEntityAsync(entity).ConfigureAwait(continueOnCapturedContext: false);
			curNode.entityId = ps.entityId;
			return (isInAttributeValue && validatingReaderCompatFlag) ? EntityType.ExpandedInAttribute : EntityType.Expanded;
		}
		if (parsingMode == ParsingMode.SkipContent)
		{
			return EntityType.Skipped;
		}
		PushInternalEntity(entity);
		curNode.entityId = ps.entityId;
		return (isInAttributeValue && validatingReaderCompatFlag) ? EntityType.ExpandedInAttribute : EntityType.Expanded;
	}

	private Task<bool> ParsePIAsync()
	{
		return ParsePIAsync(null);
	}

	private async Task<bool> ParsePIAsync(StringBuilder piInDtdStringBuilder)
	{
		if (parsingMode == ParsingMode.Full)
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
		}
		int num = await ParseNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		string text = nameTable.Add(ps.chars, ps.charPos, num - ps.charPos);
		if (string.Compare(text, "xml", StringComparison.OrdinalIgnoreCase) == 0)
		{
			Throw(text.Equals("xml") ? "Unexpected XML declaration. The XML declaration must be the first node in the document, and no white space characters are allowed to appear before it." : "'{0}' is an invalid name for processing instructions.", text);
		}
		ps.charPos = num;
		if (piInDtdStringBuilder == null)
		{
			if (!ignorePIs && parsingMode == ParsingMode.Full)
			{
				curNode.SetNamedNode(XmlNodeType.ProcessingInstruction, text);
			}
		}
		else
		{
			piInDtdStringBuilder.Append(text);
		}
		char ch = ps.chars[ps.charPos];
		if (await EatWhitespacesAsync(piInDtdStringBuilder).ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			if (ps.charsUsed - ps.charPos < 2)
			{
				await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			if (ch != '?' || ps.chars[ps.charPos + 1] != '>')
			{
				Throw("The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(ps.chars, ps.charsUsed, ps.charPos));
			}
		}
		Tuple<int, int, bool> obj = await ParsePIValueAsync().ConfigureAwait(continueOnCapturedContext: false);
		int item = obj.Item1;
		int item2 = obj.Item2;
		if (obj.Item3)
		{
			if (piInDtdStringBuilder == null)
			{
				if (ignorePIs)
				{
					return false;
				}
				if (parsingMode == ParsingMode.Full)
				{
					curNode.SetValue(ps.chars, item, item2 - item);
				}
			}
			else
			{
				piInDtdStringBuilder.Append(ps.chars, item, item2 - item);
			}
		}
		else
		{
			StringBuilder sb;
			if (piInDtdStringBuilder == null)
			{
				if (ignorePIs || parsingMode != ParsingMode.Full)
				{
					Tuple<int, int, bool> tuple;
					do
					{
						tuple = await ParsePIValueAsync().ConfigureAwait(continueOnCapturedContext: false);
						_ = tuple.Item1;
						_ = tuple.Item2;
					}
					while (!tuple.Item3);
					return false;
				}
				sb = stringBuilder;
			}
			else
			{
				sb = piInDtdStringBuilder;
			}
			Tuple<int, int, bool> tuple2;
			do
			{
				sb.Append(ps.chars, item, item2 - item);
				tuple2 = await ParsePIValueAsync().ConfigureAwait(continueOnCapturedContext: false);
				item = tuple2.Item1;
				item2 = tuple2.Item2;
			}
			while (!tuple2.Item3);
			sb.Append(ps.chars, item, item2 - item);
			if (piInDtdStringBuilder == null)
			{
				curNode.SetValue(stringBuilder.ToString());
				stringBuilder.Length = 0;
			}
		}
		return true;
	}

	private async Task<Tuple<int, int, bool>> ParsePIValueAsync()
	{
		if (ps.charsUsed - ps.charPos < 2 && await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			Throw(ps.charsUsed, "Unexpected end of file while parsing {0} has occurred.", "PI");
		}
		int num = ps.charPos;
		char[] chars = ps.chars;
		int num2 = 0;
		int num3 = -1;
		int item;
		while (true)
		{
			char c;
			if ((xmlCharType.charProperties[(uint)(c = chars[num])] & 0x40) != 0 && c != '?')
			{
				num++;
				continue;
			}
			switch (chars[num])
			{
			case '?':
				if (chars[num + 1] == '>')
				{
					if (num2 > 0)
					{
						ShiftBuffer(num3 + num2, num3, num - num3 - num2);
						item = num - num2;
					}
					else
					{
						item = num;
					}
					int charPos = ps.charPos;
					ps.charPos = num + 2;
					return new Tuple<int, int, bool>(charPos, item, item3: true);
				}
				if (num + 1 != ps.charsUsed)
				{
					num++;
					continue;
				}
				break;
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					if (!ps.eolNormalized && parsingMode == ParsingMode.Full)
					{
						if (num - ps.charPos > 0)
						{
							if (num2 == 0)
							{
								num2 = 1;
								num3 = num;
							}
							else
							{
								ShiftBuffer(num3 + num2, num3, num - num3 - num2);
								num3 = num - num2;
								num2++;
							}
						}
						else
						{
							ps.charPos++;
						}
					}
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (!ps.eolNormalized)
					{
						chars[num] = '\n';
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '\t':
			case '&':
			case '<':
			case ']':
				num++;
				continue;
			default:
				if (num == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[num]))
				{
					if (num + 1 == ps.charsUsed)
					{
						break;
					}
					num++;
					if (XmlCharType.IsLowSurrogate(chars[num]))
					{
						num++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, num);
				continue;
			}
			break;
		}
		if (num2 > 0)
		{
			ShiftBuffer(num3 + num2, num3, num - num3 - num2);
			item = num - num2;
		}
		else
		{
			item = num;
		}
		int charPos2 = ps.charPos;
		ps.charPos = num;
		return new Tuple<int, int, bool>(charPos2, item, item3: false);
	}

	private async Task<bool> ParseCommentAsync()
	{
		if (ignoreComments)
		{
			ParsingMode oldParsingMode = parsingMode;
			parsingMode = ParsingMode.SkipNode;
			await ParseCDataOrCommentAsync(XmlNodeType.Comment).ConfigureAwait(continueOnCapturedContext: false);
			parsingMode = oldParsingMode;
			return false;
		}
		await ParseCDataOrCommentAsync(XmlNodeType.Comment).ConfigureAwait(continueOnCapturedContext: false);
		return true;
	}

	private Task ParseCDataAsync()
	{
		return ParseCDataOrCommentAsync(XmlNodeType.CDATA);
	}

	private async Task ParseCDataOrCommentAsync(XmlNodeType type)
	{
		if (parsingMode == ParsingMode.Full)
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			Tuple<int, int, bool> obj = await ParseCDataOrCommentTupleAsync(type).ConfigureAwait(continueOnCapturedContext: false);
			int item = obj.Item1;
			int item2 = obj.Item2;
			if (obj.Item3)
			{
				curNode.SetValueNode(type, ps.chars, item, item2 - item);
				return;
			}
			Tuple<int, int, bool> tuple;
			do
			{
				stringBuilder.Append(ps.chars, item, item2 - item);
				tuple = await ParseCDataOrCommentTupleAsync(type).ConfigureAwait(continueOnCapturedContext: false);
				item = tuple.Item1;
				item2 = tuple.Item2;
			}
			while (!tuple.Item3);
			stringBuilder.Append(ps.chars, item, item2 - item);
			curNode.SetValueNode(type, stringBuilder.ToString());
			stringBuilder.Length = 0;
		}
		else
		{
			Tuple<int, int, bool> tuple2;
			do
			{
				tuple2 = await ParseCDataOrCommentTupleAsync(type).ConfigureAwait(continueOnCapturedContext: false);
				_ = tuple2.Item1;
				_ = tuple2.Item2;
			}
			while (!tuple2.Item3);
		}
	}

	private async Task<Tuple<int, int, bool>> ParseCDataOrCommentTupleAsync(XmlNodeType type)
	{
		if (ps.charsUsed - ps.charPos < 3 && await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
		{
			Throw("Unexpected end of file while parsing {0} has occurred.", (type == XmlNodeType.Comment) ? "Comment" : "CDATA");
		}
		int num = ps.charPos;
		char[] chars = ps.chars;
		int num2 = 0;
		int num3 = -1;
		char c = ((type == XmlNodeType.Comment) ? '-' : ']');
		int item;
		while (true)
		{
			char c2;
			if ((xmlCharType.charProperties[(uint)(c2 = chars[num])] & 0x40) != 0 && c2 != c)
			{
				num++;
				continue;
			}
			if (chars[num] == c)
			{
				if (chars[num + 1] == c)
				{
					if (chars[num + 2] == '>')
					{
						if (num2 > 0)
						{
							ShiftBuffer(num3 + num2, num3, num - num3 - num2);
							item = num - num2;
						}
						else
						{
							item = num;
						}
						int charPos = ps.charPos;
						ps.charPos = num + 3;
						return new Tuple<int, int, bool>(charPos, item, item3: true);
					}
					if (num + 2 == ps.charsUsed)
					{
						break;
					}
					if (type == XmlNodeType.Comment)
					{
						Throw(num, "An XML comment cannot contain '--', and '-' cannot be the last character.");
					}
				}
				else if (num + 1 == ps.charsUsed)
				{
					break;
				}
				num++;
				continue;
			}
			switch (chars[num])
			{
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					if (!ps.eolNormalized && parsingMode == ParsingMode.Full)
					{
						if (num - ps.charPos > 0)
						{
							if (num2 == 0)
							{
								num2 = 1;
								num3 = num;
							}
							else
							{
								ShiftBuffer(num3 + num2, num3, num - num3 - num2);
								num3 = num - num2;
								num2++;
							}
						}
						else
						{
							ps.charPos++;
						}
					}
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (!ps.eolNormalized)
					{
						chars[num] = '\n';
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '\t':
			case '&':
			case '<':
			case ']':
				num++;
				continue;
			default:
				if (num == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[num]))
				{
					if (num + 1 == ps.charsUsed)
					{
						break;
					}
					num++;
					if (XmlCharType.IsLowSurrogate(chars[num]))
					{
						num++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, num);
				break;
			}
			break;
		}
		if (num2 > 0)
		{
			ShiftBuffer(num3 + num2, num3, num - num3 - num2);
			item = num - num2;
		}
		else
		{
			item = num;
		}
		int charPos2 = ps.charPos;
		ps.charPos = num;
		return new Tuple<int, int, bool>(charPos2, item, item3: false);
	}

	private async Task<bool> ParseDoctypeDeclAsync()
	{
		if (dtdProcessing == DtdProcessing.Prohibit)
		{
			ThrowWithoutLineInfo(v1Compat ? "DTD is prohibited in this XML document." : "For security reasons DTD is prohibited in this XML document. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method.");
		}
		while (ps.charsUsed - ps.charPos < 8)
		{
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw("Unexpected end of file while parsing {0} has occurred.", "DOCTYPE");
			}
		}
		if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 7, "DOCTYPE"))
		{
			ThrowUnexpectedToken((!rootElementParsed && dtdInfo == null) ? "DOCTYPE" : "<!--");
		}
		if (!xmlCharType.IsWhiteSpace(ps.chars[ps.charPos + 7]))
		{
			ThrowExpectingWhitespace(ps.charPos + 7);
		}
		if (dtdInfo != null)
		{
			Throw(ps.charPos - 2, "Cannot have multiple DTDs.");
		}
		if (rootElementParsed)
		{
			Throw(ps.charPos - 2, "DTD must be defined before the document root element.");
		}
		ps.charPos += 8;
		await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
		if (dtdProcessing == DtdProcessing.Parse)
		{
			curNode.SetLineInfo(ps.LineNo, ps.LinePos);
			await ParseDtdAsync().ConfigureAwait(continueOnCapturedContext: false);
			nextParsingFunction = parsingFunction;
			parsingFunction = ParsingFunction.ResetAttributesRootLevel;
			return true;
		}
		await SkipDtdAsync().ConfigureAwait(continueOnCapturedContext: false);
		return false;
	}

	private async Task ParseDtdAsync()
	{
		dtdInfo = await DtdParser.Create().ParseInternalDtdAsync(new DtdParserProxy(this), saveInternalSubset: true).ConfigureAwait(continueOnCapturedContext: false);
		if ((validatingReaderCompatFlag || !v1Compat) && (dtdInfo.HasDefaultAttributes || dtdInfo.HasNonCDataAttributes))
		{
			addDefaultAttributesAndNormalize = true;
		}
		curNode.SetNamedNode(XmlNodeType.DocumentType, dtdInfo.Name.ToString(), string.Empty, null);
		curNode.SetValue(dtdInfo.InternalDtdSubset);
	}

	private async Task SkipDtdAsync()
	{
		Tuple<int, int> obj = await ParseQNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		_ = obj.Item1;
		int item = obj.Item2;
		ps.charPos = item;
		await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
		if (ps.chars[ps.charPos] == 'P')
		{
			while (ps.charsUsed - ps.charPos < 6)
			{
				if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 6, "PUBLIC"))
			{
				ThrowUnexpectedToken("PUBLIC");
			}
			ps.charPos += 6;
			if (await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				ThrowExpectingWhitespace(ps.charPos);
			}
			await SkipPublicOrSystemIdLiteralAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				ThrowExpectingWhitespace(ps.charPos);
			}
			await SkipPublicOrSystemIdLiteralAsync().ConfigureAwait(continueOnCapturedContext: false);
			await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
		}
		else if (ps.chars[ps.charPos] == 'S')
		{
			while (ps.charsUsed - ps.charPos < 6)
			{
				if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			if (!XmlConvert.StrEqual(ps.chars, ps.charPos, 6, "SYSTEM"))
			{
				ThrowUnexpectedToken("SYSTEM");
			}
			ps.charPos += 6;
			if (await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				ThrowExpectingWhitespace(ps.charPos);
			}
			await SkipPublicOrSystemIdLiteralAsync().ConfigureAwait(continueOnCapturedContext: false);
			await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
		}
		else if (ps.chars[ps.charPos] != '[' && ps.chars[ps.charPos] != '>')
		{
			Throw("Expecting external ID, '[' or '>'.");
		}
		if (ps.chars[ps.charPos] == '[')
		{
			ps.charPos++;
			await SkipUntilAsync(']', recognizeLiterals: true).ConfigureAwait(continueOnCapturedContext: false);
			await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
			if (ps.chars[ps.charPos] != '>')
			{
				ThrowUnexpectedToken(">");
			}
		}
		else if (ps.chars[ps.charPos] == '>')
		{
			curNode.SetValue(string.Empty);
		}
		else
		{
			Throw("Expecting an internal subset or the end of the DOCTYPE declaration.");
		}
		ps.charPos++;
	}

	private Task SkipPublicOrSystemIdLiteralAsync()
	{
		char c = ps.chars[ps.charPos];
		if (c != '"' && c != '\'')
		{
			ThrowUnexpectedToken("\"", "'");
		}
		ps.charPos++;
		return SkipUntilAsync(c, recognizeLiterals: false);
	}

	private async Task SkipUntilAsync(char stopChar, bool recognizeLiterals)
	{
		bool inLiteral = false;
		bool inComment = false;
		bool inPI = false;
		char literalQuote = '"';
		char[] chars = ps.chars;
		int num = ps.charPos;
		while (true)
		{
			char c;
			if ((xmlCharType.charProperties[(uint)(c = chars[num])] & 0x80) != 0 && chars[num] != stopChar && c != '-' && c != '?')
			{
				num++;
				continue;
			}
			if (c == stopChar && !inLiteral)
			{
				break;
			}
			ps.charPos = num;
			switch (c)
			{
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '<':
				if (chars[num + 1] == '?')
				{
					if (recognizeLiterals && !inLiteral && !inComment)
					{
						inPI = true;
						num += 2;
						continue;
					}
				}
				else if (chars[num + 1] == '!')
				{
					if (num + 3 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (chars[num + 2] == '-' && chars[num + 3] == '-' && recognizeLiterals && !inLiteral && !inPI)
					{
						inComment = true;
						num += 4;
						continue;
					}
				}
				else if (num + 1 >= ps.charsUsed && !ps.isEof)
				{
					break;
				}
				num++;
				continue;
			case '-':
				if (inComment)
				{
					if (num + 2 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (chars[num + 1] == '-' && chars[num + 2] == '>')
					{
						inComment = false;
						num += 2;
						continue;
					}
				}
				num++;
				continue;
			case '?':
				if (inPI)
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (chars[num + 1] == '>')
					{
						inPI = false;
						num++;
						continue;
					}
				}
				num++;
				continue;
			case '\t':
			case '&':
			case '>':
			case ']':
				num++;
				continue;
			case '"':
			case '\'':
				if (inLiteral)
				{
					if (literalQuote == c)
					{
						inLiteral = false;
					}
				}
				else if (recognizeLiterals && !inComment && !inPI)
				{
					inLiteral = true;
					literalQuote = c;
				}
				num++;
				continue;
			default:
				if (num == ps.charsUsed)
				{
					break;
				}
				if (XmlCharType.IsHighSurrogate(chars[num]))
				{
					if (num + 1 == ps.charsUsed)
					{
						break;
					}
					num++;
					if (XmlCharType.IsLowSurrogate(chars[num]))
					{
						num++;
						continue;
					}
				}
				ThrowInvalidChar(chars, ps.charsUsed, num);
				break;
			}
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				if (ps.charsUsed - ps.charPos > 0)
				{
					if (ps.chars[ps.charPos] != '\r')
					{
						Throw("Unexpected end of file has occurred.");
					}
				}
				else
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			chars = ps.chars;
			num = ps.charPos;
		}
		ps.charPos = num + 1;
	}

	private async Task<int> EatWhitespacesAsync(StringBuilder sb)
	{
		int num = ps.charPos;
		int wsCount = 0;
		char[] chars = ps.chars;
		while (true)
		{
			switch (chars[num])
			{
			case '\n':
				num++;
				OnNewLine(num);
				continue;
			case '\r':
				if (chars[num + 1] == '\n')
				{
					int num3 = num - ps.charPos;
					if (sb != null && !ps.eolNormalized)
					{
						if (num3 > 0)
						{
							sb.Append(chars, ps.charPos, num3);
							wsCount += num3;
						}
						ps.charPos = num + 1;
					}
					num += 2;
				}
				else
				{
					if (num + 1 >= ps.charsUsed && !ps.isEof)
					{
						break;
					}
					if (!ps.eolNormalized)
					{
						chars[num] = '\n';
					}
					num++;
				}
				OnNewLine(num);
				continue;
			case '\t':
			case ' ':
				num++;
				continue;
			default:
				if (num != ps.charsUsed)
				{
					int num2 = num - ps.charPos;
					if (num2 > 0)
					{
						sb?.Append(ps.chars, ps.charPos, num2);
						ps.charPos = num;
						wsCount += num2;
					}
					return wsCount;
				}
				break;
			}
			int num4 = num - ps.charPos;
			if (num4 > 0)
			{
				sb?.Append(ps.chars, ps.charPos, num4);
				ps.charPos = num;
				wsCount += num4;
			}
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				if (ps.charsUsed - ps.charPos == 0)
				{
					break;
				}
				if (ps.chars[ps.charPos] != '\r')
				{
					Throw("Unexpected end of file has occurred.");
				}
			}
			num = ps.charPos;
			chars = ps.chars;
		}
		return wsCount;
	}

	private async Task<Tuple<EntityType, int>> ParseNumericCharRefAsync(bool expand, StringBuilder internalSubsetBuilder)
	{
		int num;
		int charCount;
		EntityType entityType;
		while ((num = ParseNumericCharRefInline(ps.charPos, expand, internalSubsetBuilder, out charCount, out entityType)) == -2)
		{
			if (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0)
			{
				Throw("Unexpected end of file while parsing {0} has occurred.");
			}
		}
		if (expand)
		{
			ps.charPos = num - charCount;
		}
		return new Tuple<EntityType, int>(entityType, num);
	}

	private async Task<int> ParseNamedCharRefAsync(bool expand, StringBuilder internalSubsetBuilder)
	{
		do
		{
			int num;
			switch (num = ParseNamedCharRefInline(ps.charPos, expand, internalSubsetBuilder))
			{
			case -1:
				return -1;
			case -2:
				continue;
			}
			if (expand)
			{
				ps.charPos = num - 1;
			}
			return num;
		}
		while (await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0);
		return -1;
	}

	private async Task<int> ParseNameAsync()
	{
		return (await ParseQNameAsync(isQName: false, 0).ConfigureAwait(continueOnCapturedContext: false)).Item2;
	}

	private Task<Tuple<int, int>> ParseQNameAsync()
	{
		return ParseQNameAsync(isQName: true, 0);
	}

	private async Task<Tuple<int, int>> ParseQNameAsync(bool isQName, int startOffset)
	{
		int colonOffset = -1;
		int num = ps.charPos + startOffset;
		while (true)
		{
			char[] chars = ps.chars;
			bool flag = false;
			if ((xmlCharType.charProperties[(uint)chars[num]] & 4) != 0)
			{
				num++;
			}
			else if (num + 1 >= ps.charsUsed)
			{
				flag = true;
			}
			else if (chars[num] != ':' || supportNamespaces)
			{
				Throw(num, "Name cannot begin with the '{0}' character, hexadecimal value {1}.", XmlException.BuildCharExceptionArgs(chars, ps.charsUsed, num));
			}
			if (flag)
			{
				Tuple<int, bool> obj = await ReadDataInNameAsync(num).ConfigureAwait(continueOnCapturedContext: false);
				num = obj.Item1;
				if (obj.Item2)
				{
					continue;
				}
				Throw(num, "Unexpected end of file while parsing {0} has occurred.", "Name");
			}
			while (true)
			{
				if ((xmlCharType.charProperties[(uint)chars[num]] & 8) != 0)
				{
					num++;
					continue;
				}
				if (chars[num] == ':')
				{
					if (supportNamespaces)
					{
						break;
					}
					colonOffset = num - ps.charPos;
					num++;
					continue;
				}
				if (num == ps.charsUsed)
				{
					Tuple<int, bool> obj2 = await ReadDataInNameAsync(num).ConfigureAwait(continueOnCapturedContext: false);
					num = obj2.Item1;
					if (obj2.Item2)
					{
						chars = ps.chars;
						continue;
					}
					Throw(num, "Unexpected end of file while parsing {0} has occurred.", "Name");
				}
				return new Tuple<int, int>((colonOffset == -1) ? (-1) : (ps.charPos + colonOffset), num);
			}
			if (colonOffset != -1 || !isQName)
			{
				Throw(num, "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(':', '\0'));
			}
			colonOffset = num - ps.charPos;
			num++;
		}
	}

	private async Task<Tuple<int, bool>> ReadDataInNameAsync(int pos)
	{
		int offset = pos - ps.charPos;
		bool item = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) != 0;
		pos = ps.charPos + offset;
		return new Tuple<int, bool>(pos, item);
	}

	private async Task<string> ParseEntityNameAsync()
	{
		int num;
		try
		{
			num = await ParseNameAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (XmlException)
		{
			Throw("An error occurred while parsing EntityName.");
			return null;
		}
		if (ps.chars[num] != ';')
		{
			Throw("An error occurred while parsing EntityName.");
		}
		string result = nameTable.Add(ps.chars, ps.charPos, num - ps.charPos);
		ps.charPos = num + 1;
		return result;
	}

	private async Task PushExternalEntityOrSubsetAsync(string publicId, string systemId, Uri baseUri, string entityName)
	{
		Uri uri;
		if (!string.IsNullOrEmpty(publicId))
		{
			try
			{
				uri = xmlResolver.ResolveUri(baseUri, publicId);
				if (await OpenAndPushAsync(uri).ConfigureAwait(continueOnCapturedContext: false))
				{
					return;
				}
			}
			catch (Exception)
			{
			}
		}
		uri = xmlResolver.ResolveUri(baseUri, systemId);
		try
		{
			if (await OpenAndPushAsync(uri).ConfigureAwait(continueOnCapturedContext: false))
			{
				return;
			}
		}
		catch (Exception ex2)
		{
			if (v1Compat)
			{
				throw;
			}
			string message = ex2.Message;
			Throw(new XmlException((entityName == null) ? "An error has occurred while opening external DTD '{0}': {1}" : "An error has occurred while opening external entity '{0}': {1}", new string[2]
			{
				uri.ToString(),
				message
			}, ex2, 0, 0));
		}
		if (entityName == null)
		{
			ThrowWithoutLineInfo("Cannot resolve external DTD subset - public ID = '{0}', system ID = '{1}'.", new string[2]
			{
				(publicId != null) ? publicId : string.Empty,
				systemId
			}, null);
		}
		else
		{
			Throw((dtdProcessing == DtdProcessing.Ignore) ? "Cannot resolve entity reference '{0}' because the DTD has been ignored. To enable DTD processing set the DtdProcessing property on XmlReaderSettings to Parse and pass the settings into XmlReader.Create method." : "Cannot resolve entity reference '{0}'.", entityName);
		}
	}

	private async Task<bool> OpenAndPushAsync(Uri uri)
	{
		if (xmlResolver.SupportsType(uri, typeof(TextReader)))
		{
			TextReader textReader = (TextReader)(await xmlResolver.GetEntityAsync(uri, null, typeof(TextReader)).ConfigureAwait(continueOnCapturedContext: false));
			if (textReader == null)
			{
				return false;
			}
			PushParsingState();
			await InitTextReaderInputAsync(uri.ToString(), uri, textReader).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			Stream stream = (Stream)(await xmlResolver.GetEntityAsync(uri, null, typeof(Stream)).ConfigureAwait(continueOnCapturedContext: false));
			if (stream == null)
			{
				return false;
			}
			PushParsingState();
			await InitStreamInputAsync(uri, stream, null).ConfigureAwait(continueOnCapturedContext: false);
		}
		return true;
	}

	private async Task<bool> PushExternalEntityAsync(IDtdEntityInfo entity)
	{
		if (!IsResolverNull)
		{
			Uri baseUri = null;
			if (!string.IsNullOrEmpty(entity.BaseUriString))
			{
				baseUri = xmlResolver.ResolveUri(null, entity.BaseUriString);
			}
			await PushExternalEntityOrSubsetAsync(entity.PublicId, entity.SystemId, baseUri, entity.Name).ConfigureAwait(continueOnCapturedContext: false);
			RegisterEntity(entity);
			int initialPos = ps.charPos;
			if (v1Compat)
			{
				await EatWhitespacesAsync(null).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (!(await ParseXmlDeclarationAsync(isTextDecl: true).ConfigureAwait(continueOnCapturedContext: false)))
			{
				ps.charPos = initialPos;
			}
			return true;
		}
		Encoding encoding = ps.encoding;
		PushParsingState();
		InitStringInput(entity.SystemId, encoding, string.Empty);
		RegisterEntity(entity);
		RegisterConsumedCharacters(0L, inEntityReference: true);
		return false;
	}

	private async Task<bool> ZeroEndingStreamAsync(int pos)
	{
		bool flag = v1Compat && pos == ps.charsUsed - 1 && ps.chars[pos] == '\0';
		if (flag)
		{
			flag = await ReadDataAsync().ConfigureAwait(continueOnCapturedContext: false) == 0;
		}
		if (flag && ps.isStreamEof)
		{
			ps.charsUsed--;
			return true;
		}
		return false;
	}

	private async Task ParseDtdFromParserContextAsync()
	{
		dtdInfo = await DtdParser.Create().ParseFreeFloatingDtdAsync(fragmentParserContext.BaseURI, fragmentParserContext.DocTypeName, fragmentParserContext.PublicId, fragmentParserContext.SystemId, fragmentParserContext.InternalSubset, new DtdParserProxy(this)).ConfigureAwait(continueOnCapturedContext: false);
		if ((validatingReaderCompatFlag || !v1Compat) && (dtdInfo.HasDefaultAttributes || dtdInfo.HasNonCDataAttributes))
		{
			addDefaultAttributesAndNormalize = true;
		}
	}

	private async Task<bool> InitReadContentAsBinaryAsync()
	{
		if (parsingFunction == ParsingFunction.InReadValueChunk)
		{
			throw new InvalidOperationException(Res.GetString("ReadValueChunk calls cannot be mixed with ReadContentAsBase64 or ReadContentAsBinHex."));
		}
		if (parsingFunction == ParsingFunction.InIncrementalRead)
		{
			throw new InvalidOperationException(Res.GetString("ReadContentAsBase64 and ReadContentAsBinHex method calls cannot be mixed with calls to ReadChars, ReadBase64, and ReadBinHex."));
		}
		if (!XmlReader.IsTextualNode(curNode.type) && !(await MoveToNextContentNodeAsync(moveIfOnContentNode: false).ConfigureAwait(continueOnCapturedContext: false)))
		{
			return false;
		}
		SetupReadContentAsBinaryState(ParsingFunction.InReadContentAsBinary);
		incReadLineInfo.Set(curNode.LineNo, curNode.LinePos);
		return true;
	}

	private async Task<bool> InitReadElementContentAsBinaryAsync()
	{
		bool isEmpty = curNode.IsEmptyElement;
		await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (isEmpty)
		{
			return false;
		}
		if (!(await MoveToNextContentNodeAsync(moveIfOnContentNode: false).ConfigureAwait(continueOnCapturedContext: false)))
		{
			if (curNode.type != XmlNodeType.EndElement)
			{
				Throw("'{0}' is an invalid XmlNodeType.", curNode.type.ToString());
			}
			await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
			return false;
		}
		SetupReadContentAsBinaryState(ParsingFunction.InReadElementContentAsBinary);
		incReadLineInfo.Set(curNode.LineNo, curNode.LinePos);
		return true;
	}

	private async Task<bool> MoveToNextContentNodeAsync(bool moveIfOnContentNode)
	{
		do
		{
			switch (curNode.type)
			{
			case XmlNodeType.Attribute:
				return !moveIfOnContentNode;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if (!moveIfOnContentNode)
				{
					return true;
				}
				break;
			case XmlNodeType.EntityReference:
				outerReader.ResolveEntity();
				break;
			default:
				return false;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.EndEntity:
				break;
			}
			moveIfOnContentNode = false;
		}
		while (await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false));
		return false;
	}

	private async Task<int> ReadContentAsBinaryAsync(byte[] buffer, int index, int count)
	{
		if (incReadState == IncrementalReadState.ReadContentAsBinary_End)
		{
			return 0;
		}
		incReadDecoder.SetNextOutputBuffer(buffer, index, count);
		ParsingFunction tmp;
		while (true)
		{
			int charsRead = 0;
			try
			{
				charsRead = curNode.CopyToBinary(incReadDecoder, readValueOffset);
			}
			catch (XmlException e)
			{
				curNode.AdjustLineInfo(readValueOffset, ps.eolNormalized, ref incReadLineInfo);
				ReThrow(e, incReadLineInfo.lineNo, incReadLineInfo.linePos);
			}
			readValueOffset += charsRead;
			if (incReadDecoder.IsFull)
			{
				return incReadDecoder.DecodedCount;
			}
			if (incReadState == IncrementalReadState.ReadContentAsBinary_OnPartialValue)
			{
				curNode.SetValue(string.Empty);
				bool flag = false;
				int num = 0;
				int num2 = 0;
				while (!incReadDecoder.IsFull && !flag)
				{
					int outOrChars = 0;
					incReadLineInfo.Set(ps.LineNo, ps.LinePos);
					Tuple<int, int, int, bool> obj = await ParseTextAsync(outOrChars).ConfigureAwait(continueOnCapturedContext: false);
					num = obj.Item1;
					num2 = obj.Item2;
					_ = obj.Item3;
					flag = obj.Item4;
					try
					{
						charsRead = incReadDecoder.Decode(ps.chars, num, num2 - num);
					}
					catch (XmlException e2)
					{
						ReThrow(e2, incReadLineInfo.lineNo, incReadLineInfo.linePos);
					}
					num += charsRead;
				}
				incReadState = (flag ? IncrementalReadState.ReadContentAsBinary_OnCachedValue : IncrementalReadState.ReadContentAsBinary_OnPartialValue);
				readValueOffset = 0;
				if (incReadDecoder.IsFull)
				{
					curNode.SetValue(ps.chars, num, num2 - num);
					AdjustLineInfo(ps.chars, num - charsRead, num, ps.eolNormalized, ref incReadLineInfo);
					curNode.SetLineInfo(incReadLineInfo.lineNo, incReadLineInfo.linePos);
					return incReadDecoder.DecodedCount;
				}
			}
			tmp = parsingFunction;
			parsingFunction = nextParsingFunction;
			nextParsingFunction = nextNextParsingFunction;
			if (!(await MoveToNextContentNodeAsync(moveIfOnContentNode: true).ConfigureAwait(continueOnCapturedContext: false)))
			{
				break;
			}
			SetupReadContentAsBinaryState(tmp);
			incReadLineInfo.Set(curNode.LineNo, curNode.LinePos);
		}
		SetupReadContentAsBinaryState(tmp);
		incReadState = IncrementalReadState.ReadContentAsBinary_End;
		return incReadDecoder.DecodedCount;
	}

	private async Task<int> ReadElementContentAsBinaryAsync(byte[] buffer, int index, int count)
	{
		if (count == 0)
		{
			return 0;
		}
		int num = await ReadContentAsBinaryAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		if (num > 0)
		{
			return num;
		}
		if (curNode.type != XmlNodeType.EndElement)
		{
			throw new XmlException("'{0}' is an invalid XmlNodeType.", curNode.type.ToString(), this);
		}
		parsingFunction = nextParsingFunction;
		nextParsingFunction = nextNextParsingFunction;
		await outerReader.ReadAsync().ConfigureAwait(continueOnCapturedContext: false);
		return 0;
	}
}
