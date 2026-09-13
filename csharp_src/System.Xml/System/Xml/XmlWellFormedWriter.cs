using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlWellFormedWriter : XmlWriter
{
	private enum State
	{
		Start = 0,
		TopLevel = 1,
		Document = 2,
		Element = 3,
		Content = 4,
		B64Content = 5,
		B64Attribute = 6,
		AfterRootEle = 7,
		Attribute = 8,
		SpecialAttr = 9,
		EndDocument = 10,
		RootLevelAttr = 11,
		RootLevelSpecAttr = 12,
		RootLevelB64Attr = 13,
		AfterRootLevelAttr = 14,
		Closed = 15,
		Error = 16,
		StartContent = 101,
		StartContentEle = 102,
		StartContentB64 = 103,
		StartDoc = 104,
		StartDocEle = 106,
		EndAttrSEle = 107,
		EndAttrEEle = 108,
		EndAttrSCont = 109,
		EndAttrSAttr = 111,
		PostB64Cont = 112,
		PostB64Attr = 113,
		PostB64RootAttr = 114,
		StartFragEle = 115,
		StartFragCont = 116,
		StartFragB64 = 117,
		StartRootLevelAttr = 118
	}

	private enum Token
	{
		StartDocument,
		EndDocument,
		PI,
		Comment,
		Dtd,
		StartElement,
		EndElement,
		StartAttribute,
		EndAttribute,
		Text,
		CData,
		AtomicValue,
		Base64,
		RawData,
		Whitespace
	}

	private class NamespaceResolverProxy : IXmlNamespaceResolver
	{
		private XmlWellFormedWriter wfWriter;

		internal NamespaceResolverProxy(XmlWellFormedWriter wfWriter)
		{
			this.wfWriter = wfWriter;
		}

		IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
		{
			throw new NotImplementedException();
		}

		string IXmlNamespaceResolver.LookupNamespace(string prefix)
		{
			return wfWriter.LookupNamespace(prefix);
		}

		string IXmlNamespaceResolver.LookupPrefix(string namespaceName)
		{
			return wfWriter.LookupPrefix(namespaceName);
		}
	}

	private struct ElementScope
	{
		internal int prevNSTop;

		internal string prefix;

		internal string localName;

		internal string namespaceUri;

		internal XmlSpace xmlSpace;

		internal string xmlLang;

		internal void Set(string prefix, string localName, string namespaceUri, int prevNSTop)
		{
			this.prevNSTop = prevNSTop;
			this.prefix = prefix;
			this.namespaceUri = namespaceUri;
			this.localName = localName;
			xmlSpace = (XmlSpace)(-1);
			xmlLang = null;
		}

		internal void WriteEndElement(XmlRawWriter rawWriter)
		{
			rawWriter.WriteEndElement(prefix, localName, namespaceUri);
		}

		internal void WriteFullEndElement(XmlRawWriter rawWriter)
		{
			rawWriter.WriteFullEndElement(prefix, localName, namespaceUri);
		}

		internal Task WriteEndElementAsync(XmlRawWriter rawWriter)
		{
			return rawWriter.WriteEndElementAsync(prefix, localName, namespaceUri);
		}

		internal Task WriteFullEndElementAsync(XmlRawWriter rawWriter)
		{
			return rawWriter.WriteFullEndElementAsync(prefix, localName, namespaceUri);
		}
	}

	private enum NamespaceKind
	{
		Written,
		NeedToWrite,
		Implied,
		Special
	}

	private struct Namespace
	{
		internal string prefix;

		internal string namespaceUri;

		internal NamespaceKind kind;

		internal int prevNsIndex;

		internal void Set(string prefix, string namespaceUri, NamespaceKind kind)
		{
			this.prefix = prefix;
			this.namespaceUri = namespaceUri;
			this.kind = kind;
			prevNsIndex = -1;
		}

		internal void WriteDecl(XmlWriter writer, XmlRawWriter rawWriter)
		{
			if (rawWriter != null)
			{
				rawWriter.WriteNamespaceDeclaration(prefix, namespaceUri);
				return;
			}
			if (prefix.Length == 0)
			{
				writer.WriteStartAttribute(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/");
			}
			else
			{
				writer.WriteStartAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
			}
			writer.WriteString(namespaceUri);
			writer.WriteEndAttribute();
		}

		internal async Task WriteDeclAsync(XmlWriter writer, XmlRawWriter rawWriter)
		{
			if (rawWriter != null)
			{
				await rawWriter.WriteNamespaceDeclarationAsync(prefix, namespaceUri).ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			if (prefix.Length != 0)
			{
				await writer.WriteStartAttributeAsync("xmlns", prefix, "http://www.w3.org/2000/xmlns/").ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await writer.WriteStartAttributeAsync(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/").ConfigureAwait(continueOnCapturedContext: false);
			}
			await writer.WriteStringAsync(namespaceUri).ConfigureAwait(continueOnCapturedContext: false);
			await writer.WriteEndAttributeAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private struct AttrName
	{
		internal string prefix;

		internal string namespaceUri;

		internal string localName;

		internal int prev;

		internal void Set(string prefix, string localName, string namespaceUri)
		{
			this.prefix = prefix;
			this.namespaceUri = namespaceUri;
			this.localName = localName;
			prev = 0;
		}

		internal bool IsDuplicate(string prefix, string localName, string namespaceUri)
		{
			if (this.localName == localName)
			{
				if (!(this.prefix == prefix))
				{
					return this.namespaceUri == namespaceUri;
				}
				return true;
			}
			return false;
		}
	}

	private enum SpecialAttribute
	{
		No,
		DefaultXmlns,
		PrefixedXmlns,
		XmlSpace,
		XmlLang
	}

	private class AttributeValueCache
	{
		private enum ItemType
		{
			EntityRef,
			CharEntity,
			SurrogateCharEntity,
			Whitespace,
			String,
			StringChars,
			Raw,
			RawChars,
			ValueString
		}

		private class Item
		{
			internal ItemType type;

			internal object data;

			internal Item()
			{
			}

			internal void Set(ItemType type, object data)
			{
				this.type = type;
				this.data = data;
			}
		}

		private class BufferChunk
		{
			internal char[] buffer;

			internal int index;

			internal int count;

			internal BufferChunk(char[] buffer, int index, int count)
			{
				this.buffer = buffer;
				this.index = index;
				this.count = count;
			}
		}

		private StringBuilder stringValue = new StringBuilder();

		private string singleStringValue;

		private Item[] items;

		private int firstItem;

		private int lastItem = -1;

		internal string StringValue
		{
			get
			{
				if (singleStringValue != null)
				{
					return singleStringValue;
				}
				return stringValue.ToString();
			}
		}

		internal void WriteEntityRef(string name)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			switch (name)
			{
			case "lt":
				stringValue.Append('<');
				break;
			case "gt":
				stringValue.Append('>');
				break;
			case "quot":
				stringValue.Append('"');
				break;
			case "apos":
				stringValue.Append('\'');
				break;
			case "amp":
				stringValue.Append('&');
				break;
			default:
				stringValue.Append('&');
				stringValue.Append(name);
				stringValue.Append(';');
				break;
			}
			AddItem(ItemType.EntityRef, name);
		}

		internal void WriteCharEntity(char ch)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(ch);
			AddItem(ItemType.CharEntity, ch);
		}

		internal void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(highChar);
			stringValue.Append(lowChar);
			AddItem(ItemType.SurrogateCharEntity, new char[2] { lowChar, highChar });
		}

		internal void WriteWhitespace(string ws)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(ws);
			AddItem(ItemType.Whitespace, ws);
		}

		internal void WriteString(string text)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			else if (lastItem == -1)
			{
				singleStringValue = text;
				return;
			}
			stringValue.Append(text);
			AddItem(ItemType.String, text);
		}

		internal void WriteChars(char[] buffer, int index, int count)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(buffer, index, count);
			AddItem(ItemType.StringChars, new BufferChunk(buffer, index, count));
		}

		internal void WriteRaw(char[] buffer, int index, int count)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(buffer, index, count);
			AddItem(ItemType.RawChars, new BufferChunk(buffer, index, count));
		}

		internal void WriteRaw(string data)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(data);
			AddItem(ItemType.Raw, data);
		}

		internal void WriteValue(string value)
		{
			if (singleStringValue != null)
			{
				StartComplexValue();
			}
			stringValue.Append(value);
			AddItem(ItemType.ValueString, value);
		}

		internal void Replay(XmlWriter writer)
		{
			if (singleStringValue != null)
			{
				writer.WriteString(singleStringValue);
				return;
			}
			for (int i = firstItem; i <= lastItem; i++)
			{
				Item item = items[i];
				switch (item.type)
				{
				case ItemType.EntityRef:
					writer.WriteEntityRef((string)item.data);
					break;
				case ItemType.CharEntity:
					writer.WriteCharEntity((char)item.data);
					break;
				case ItemType.SurrogateCharEntity:
				{
					char[] array = (char[])item.data;
					writer.WriteSurrogateCharEntity(array[0], array[1]);
					break;
				}
				case ItemType.Whitespace:
					writer.WriteWhitespace((string)item.data);
					break;
				case ItemType.String:
					writer.WriteString((string)item.data);
					break;
				case ItemType.StringChars:
				{
					BufferChunk bufferChunk = (BufferChunk)item.data;
					writer.WriteChars(bufferChunk.buffer, bufferChunk.index, bufferChunk.count);
					break;
				}
				case ItemType.Raw:
					writer.WriteRaw((string)item.data);
					break;
				case ItemType.RawChars:
				{
					BufferChunk bufferChunk = (BufferChunk)item.data;
					writer.WriteChars(bufferChunk.buffer, bufferChunk.index, bufferChunk.count);
					break;
				}
				case ItemType.ValueString:
					writer.WriteValue((string)item.data);
					break;
				}
			}
		}

		internal void Trim()
		{
			if (singleStringValue != null)
			{
				singleStringValue = XmlConvert.TrimString(singleStringValue);
				return;
			}
			string text = stringValue.ToString();
			string text2 = XmlConvert.TrimString(text);
			if (text != text2)
			{
				stringValue = new StringBuilder(text2);
			}
			XmlCharType instance = XmlCharType.Instance;
			int i;
			for (i = firstItem; i == firstItem && i <= lastItem; i++)
			{
				Item item = items[i];
				switch (item.type)
				{
				case ItemType.Whitespace:
					firstItem++;
					break;
				case ItemType.String:
				case ItemType.Raw:
				case ItemType.ValueString:
					item.data = XmlConvert.TrimStringStart((string)item.data);
					if (((string)item.data).Length == 0)
					{
						firstItem++;
					}
					break;
				case ItemType.StringChars:
				case ItemType.RawChars:
				{
					BufferChunk bufferChunk = (BufferChunk)item.data;
					int num = bufferChunk.index + bufferChunk.count;
					while (bufferChunk.index < num && instance.IsWhiteSpace(bufferChunk.buffer[bufferChunk.index]))
					{
						bufferChunk.index++;
						bufferChunk.count--;
					}
					if (bufferChunk.index == num)
					{
						firstItem++;
					}
					break;
				}
				}
			}
			i = lastItem;
			while (i == lastItem && i >= firstItem)
			{
				Item item2 = items[i];
				switch (item2.type)
				{
				case ItemType.Whitespace:
					lastItem--;
					break;
				case ItemType.String:
				case ItemType.Raw:
				case ItemType.ValueString:
					item2.data = XmlConvert.TrimStringEnd((string)item2.data);
					if (((string)item2.data).Length == 0)
					{
						lastItem--;
					}
					break;
				case ItemType.StringChars:
				case ItemType.RawChars:
				{
					BufferChunk bufferChunk2 = (BufferChunk)item2.data;
					while (bufferChunk2.count > 0 && instance.IsWhiteSpace(bufferChunk2.buffer[bufferChunk2.index + bufferChunk2.count - 1]))
					{
						bufferChunk2.count--;
					}
					if (bufferChunk2.count == 0)
					{
						lastItem--;
					}
					break;
				}
				}
				i--;
			}
		}

		internal void Clear()
		{
			singleStringValue = null;
			lastItem = -1;
			firstItem = 0;
			stringValue.Length = 0;
		}

		private void StartComplexValue()
		{
			stringValue.Append(singleStringValue);
			AddItem(ItemType.String, singleStringValue);
			singleStringValue = null;
		}

		private void AddItem(ItemType type, object data)
		{
			int num = lastItem + 1;
			if (items == null)
			{
				items = new Item[4];
			}
			else if (items.Length == num)
			{
				Item[] destinationArray = new Item[num * 2];
				Array.Copy(items, destinationArray, num);
				items = destinationArray;
			}
			if (items[num] == null)
			{
				items[num] = new Item();
			}
			items[num].Set(type, data);
			lastItem = num;
		}

		internal async Task ReplayAsync(XmlWriter writer)
		{
			if (singleStringValue != null)
			{
				await writer.WriteStringAsync(singleStringValue).ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			for (int i = firstItem; i <= lastItem; i++)
			{
				Item item = items[i];
				switch (item.type)
				{
				case ItemType.EntityRef:
					await writer.WriteEntityRefAsync((string)item.data).ConfigureAwait(continueOnCapturedContext: false);
					break;
				case ItemType.CharEntity:
					await writer.WriteCharEntityAsync((char)item.data).ConfigureAwait(continueOnCapturedContext: false);
					break;
				case ItemType.SurrogateCharEntity:
				{
					char[] array = (char[])item.data;
					await writer.WriteSurrogateCharEntityAsync(array[0], array[1]).ConfigureAwait(continueOnCapturedContext: false);
					break;
				}
				case ItemType.Whitespace:
					await writer.WriteWhitespaceAsync((string)item.data).ConfigureAwait(continueOnCapturedContext: false);
					break;
				case ItemType.String:
					await writer.WriteStringAsync((string)item.data).ConfigureAwait(continueOnCapturedContext: false);
					break;
				case ItemType.StringChars:
				{
					BufferChunk bufferChunk = (BufferChunk)item.data;
					await writer.WriteCharsAsync(bufferChunk.buffer, bufferChunk.index, bufferChunk.count).ConfigureAwait(continueOnCapturedContext: false);
					break;
				}
				case ItemType.Raw:
					await writer.WriteRawAsync((string)item.data).ConfigureAwait(continueOnCapturedContext: false);
					break;
				case ItemType.RawChars:
				{
					BufferChunk bufferChunk = (BufferChunk)item.data;
					await writer.WriteCharsAsync(bufferChunk.buffer, bufferChunk.index, bufferChunk.count).ConfigureAwait(continueOnCapturedContext: false);
					break;
				}
				case ItemType.ValueString:
					await writer.WriteStringAsync((string)item.data).ConfigureAwait(continueOnCapturedContext: false);
					break;
				}
			}
		}
	}

	private XmlWriter writer;

	private XmlRawWriter rawWriter;

	private IXmlNamespaceResolver predefinedNamespaces;

	private Namespace[] nsStack;

	private int nsTop;

	private Dictionary<string, int> nsHashtable;

	private bool useNsHashtable;

	private ElementScope[] elemScopeStack;

	private int elemTop;

	private AttrName[] attrStack;

	private int attrCount;

	private Dictionary<string, int> attrHashTable;

	private SpecialAttribute specAttr;

	private AttributeValueCache attrValueCache;

	private string curDeclPrefix;

	private State[] stateTable;

	private State currentState;

	private bool checkCharacters;

	private bool omitDuplNamespaces;

	private bool writeEndDocumentOnClose;

	private ConformanceLevel conformanceLevel;

	private bool dtdWritten;

	private bool xmlDeclFollows;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	private SecureStringHasher hasher;

	private const int ElementStackInitialSize = 8;

	private const int NamespaceStackInitialSize = 8;

	private const int AttributeArrayInitialSize = 8;

	private const int MaxAttrDuplWalkCount = 14;

	private const int MaxNamespacesWalkCount = 16;

	internal static readonly string[] stateName = new string[17]
	{
		"Start", "TopLevel", "Document", "Element Start Tag", "Element Content", "Element Content", "Attribute", "EndRootElement", "Attribute", "Special Attribute",
		"End Document", "Root Level Attribute Value", "Root Level Special Attribute Value", "Root Level Base64 Attribute Value", "After Root Level Attribute", "Closed", "Error"
	};

	internal static readonly string[] tokenName = new string[15]
	{
		"StartDocument", "EndDocument", "PI", "Comment", "DTD", "StartElement", "EndElement", "StartAttribute", "EndAttribute", "Text",
		"CDATA", "Atomic value", "Base64", "RawData", "Whitespace"
	};

	private static WriteState[] state2WriteState = new WriteState[17]
	{
		WriteState.Start,
		WriteState.Prolog,
		WriteState.Prolog,
		WriteState.Element,
		WriteState.Content,
		WriteState.Content,
		WriteState.Attribute,
		WriteState.Content,
		WriteState.Attribute,
		WriteState.Attribute,
		WriteState.Content,
		WriteState.Attribute,
		WriteState.Attribute,
		WriteState.Attribute,
		WriteState.Attribute,
		WriteState.Closed,
		WriteState.Error
	};

	private static readonly State[] StateTableDocument = new State[240]
	{
		State.Document,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.PostB64Cont,
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
		State.Error,
		State.Error,
		State.PostB64Cont,
		State.Error,
		State.EndDocument,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDoc,
		State.TopLevel,
		State.Document,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.EndAttrSCont,
		State.EndAttrSCont,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDoc,
		State.TopLevel,
		State.Document,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.EndAttrSCont,
		State.EndAttrSCont,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDoc,
		State.TopLevel,
		State.Document,
		State.Error,
		State.Error,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDocEle,
		State.Element,
		State.Element,
		State.StartContentEle,
		State.Element,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.EndAttrSEle,
		State.EndAttrSEle,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.EndAttrEEle,
		State.EndAttrEEle,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Attribute,
		State.Error,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.EndAttrSAttr,
		State.EndAttrSAttr,
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
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.Element,
		State.Element,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.Attribute,
		State.SpecialAttr,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.EndAttrSCont,
		State.EndAttrSCont,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.Attribute,
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
		State.StartContentB64,
		State.B64Content,
		State.B64Content,
		State.B64Attribute,
		State.Error,
		State.B64Attribute,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDoc,
		State.Error,
		State.Document,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.Attribute,
		State.SpecialAttr,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDoc,
		State.TopLevel,
		State.Document,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.Attribute,
		State.SpecialAttr,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error
	};

	private static readonly State[] StateTableAuto = new State[240]
	{
		State.Document,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.PostB64Cont,
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
		State.Error,
		State.Error,
		State.PostB64Cont,
		State.Error,
		State.EndDocument,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.TopLevel,
		State.TopLevel,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.EndAttrSCont,
		State.EndAttrSCont,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.TopLevel,
		State.TopLevel,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.EndAttrSCont,
		State.EndAttrSCont,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartDoc,
		State.TopLevel,
		State.Error,
		State.Error,
		State.Error,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartFragEle,
		State.Element,
		State.Error,
		State.StartContentEle,
		State.Element,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Element,
		State.EndAttrSEle,
		State.EndAttrSEle,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.EndAttrEEle,
		State.EndAttrEEle,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.RootLevelAttr,
		State.Error,
		State.Error,
		State.Attribute,
		State.Error,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.EndAttrSAttr,
		State.EndAttrSAttr,
		State.Error,
		State.StartRootLevelAttr,
		State.StartRootLevelAttr,
		State.PostB64RootAttr,
		State.RootLevelAttr,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Error,
		State.Element,
		State.Element,
		State.Error,
		State.AfterRootLevelAttr,
		State.AfterRootLevelAttr,
		State.PostB64RootAttr,
		State.Error,
		State.Error,
		State.StartFragCont,
		State.StartFragCont,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Content,
		State.Attribute,
		State.SpecialAttr,
		State.Error,
		State.RootLevelAttr,
		State.RootLevelSpecAttr,
		State.PostB64RootAttr,
		State.Error,
		State.Error,
		State.StartFragCont,
		State.StartFragCont,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Content,
		State.EndAttrSCont,
		State.EndAttrSCont,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.Error,
		State.StartFragCont,
		State.StartFragCont,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Content,
		State.Attribute,
		State.Error,
		State.Error,
		State.RootLevelAttr,
		State.Error,
		State.PostB64RootAttr,
		State.Error,
		State.Error,
		State.StartFragB64,
		State.StartFragB64,
		State.Error,
		State.StartContentB64,
		State.B64Content,
		State.B64Content,
		State.B64Attribute,
		State.B64Content,
		State.B64Attribute,
		State.Error,
		State.Error,
		State.RootLevelB64Attr,
		State.Error,
		State.RootLevelB64Attr,
		State.Error,
		State.Error,
		State.StartFragCont,
		State.TopLevel,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.Content,
		State.Attribute,
		State.SpecialAttr,
		State.Error,
		State.RootLevelAttr,
		State.RootLevelSpecAttr,
		State.PostB64RootAttr,
		State.AfterRootLevelAttr,
		State.Error,
		State.TopLevel,
		State.TopLevel,
		State.Error,
		State.StartContent,
		State.Content,
		State.PostB64Cont,
		State.PostB64Attr,
		State.AfterRootEle,
		State.Attribute,
		State.SpecialAttr,
		State.Error,
		State.RootLevelAttr,
		State.RootLevelSpecAttr,
		State.PostB64RootAttr,
		State.AfterRootLevelAttr,
		State.Error
	};

	public override WriteState WriteState
	{
		get
		{
			if (currentState <= State.Error)
			{
				return state2WriteState[(int)currentState];
			}
			return WriteState.Error;
		}
	}

	public override XmlWriterSettings Settings
	{
		get
		{
			XmlWriterSettings settings = writer.Settings;
			settings.ReadOnly = false;
			settings.ConformanceLevel = conformanceLevel;
			if (omitDuplNamespaces)
			{
				settings.NamespaceHandling |= NamespaceHandling.OmitDuplicates;
			}
			settings.WriteEndDocumentOnClose = writeEndDocumentOnClose;
			settings.ReadOnly = true;
			return settings;
		}
	}

	public override XmlSpace XmlSpace
	{
		get
		{
			int num = elemTop;
			while (num >= 0 && elemScopeStack[num].xmlSpace == (XmlSpace)(-1))
			{
				num--;
			}
			return elemScopeStack[num].xmlSpace;
		}
	}

	public override string XmlLang
	{
		get
		{
			int num = elemTop;
			while (num > 0 && elemScopeStack[num].xmlLang == null)
			{
				num--;
			}
			return elemScopeStack[num].xmlLang;
		}
	}

	internal XmlWriter InnerWriter => writer;

	internal XmlRawWriter RawWriter => rawWriter;

	private bool SaveAttrValue => specAttr != SpecialAttribute.No;

	private bool InBase64
	{
		get
		{
			if (currentState != State.B64Content && currentState != State.B64Attribute)
			{
				return currentState == State.RootLevelB64Attr;
			}
			return true;
		}
	}

	private bool IsClosedOrErrorState => currentState >= State.Closed;

	internal XmlWellFormedWriter(XmlWriter writer, XmlWriterSettings settings)
	{
		this.writer = writer;
		rawWriter = writer as XmlRawWriter;
		predefinedNamespaces = writer as IXmlNamespaceResolver;
		if (rawWriter != null)
		{
			rawWriter.NamespaceResolver = new NamespaceResolverProxy(this);
		}
		checkCharacters = settings.CheckCharacters;
		omitDuplNamespaces = (settings.NamespaceHandling & NamespaceHandling.OmitDuplicates) != 0;
		writeEndDocumentOnClose = settings.WriteEndDocumentOnClose;
		conformanceLevel = settings.ConformanceLevel;
		stateTable = ((conformanceLevel == ConformanceLevel.Document) ? StateTableDocument : StateTableAuto);
		currentState = State.Start;
		nsStack = new Namespace[8];
		nsStack[0].Set("xmlns", "http://www.w3.org/2000/xmlns/", NamespaceKind.Special);
		nsStack[1].Set("xml", "http://www.w3.org/XML/1998/namespace", NamespaceKind.Special);
		if (predefinedNamespaces == null)
		{
			nsStack[2].Set(string.Empty, string.Empty, NamespaceKind.Implied);
		}
		else
		{
			string text = predefinedNamespaces.LookupNamespace(string.Empty);
			nsStack[2].Set(string.Empty, (text == null) ? string.Empty : text, NamespaceKind.Implied);
		}
		nsTop = 2;
		elemScopeStack = new ElementScope[8];
		elemScopeStack[0].Set(string.Empty, string.Empty, string.Empty, nsTop);
		elemScopeStack[0].xmlSpace = XmlSpace.None;
		elemScopeStack[0].xmlLang = null;
		elemTop = 0;
		attrStack = new AttrName[8];
		hasher = new SecureStringHasher();
	}

	public override void WriteStartDocument()
	{
		WriteStartDocumentImpl(XmlStandalone.Omit);
	}

	public override void WriteStartDocument(bool standalone)
	{
		WriteStartDocumentImpl(standalone ? XmlStandalone.Yes : XmlStandalone.No);
	}

	public override void WriteEndDocument()
	{
		try
		{
			while (elemTop > 0)
			{
				WriteEndElement();
			}
			State num = currentState;
			AdvanceState(Token.EndDocument);
			if (num != State.AfterRootEle)
			{
				throw new ArgumentException(Res.GetString("Document does not have a root element."));
			}
			if (rawWriter == null)
			{
				writer.WriteEndDocument();
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		try
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			XmlConvert.VerifyQName(name, ExceptionType.XmlException);
			if (conformanceLevel == ConformanceLevel.Fragment)
			{
				throw new InvalidOperationException(Res.GetString("DTD is not allowed in XML fragments."));
			}
			AdvanceState(Token.Dtd);
			if (dtdWritten)
			{
				currentState = State.Error;
				throw new InvalidOperationException(Res.GetString("The DTD has already been written out."));
			}
			if (conformanceLevel == ConformanceLevel.Auto)
			{
				conformanceLevel = ConformanceLevel.Document;
				stateTable = StateTableDocument;
			}
			if (checkCharacters)
			{
				int invCharIndex;
				if (pubid != null && (invCharIndex = xmlCharType.IsPublicId(pubid)) >= 0)
				{
					object[] args = XmlException.BuildCharExceptionArgs(pubid, invCharIndex);
					throw new ArgumentException(Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args), "pubid");
				}
				if (sysid != null && (invCharIndex = xmlCharType.IsOnlyCharData(sysid)) >= 0)
				{
					object[] args = XmlException.BuildCharExceptionArgs(sysid, invCharIndex);
					throw new ArgumentException(Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args), "sysid");
				}
				if (subset != null && (invCharIndex = xmlCharType.IsOnlyCharData(subset)) >= 0)
				{
					object[] args = XmlException.BuildCharExceptionArgs(subset, invCharIndex);
					throw new ArgumentException(Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args), "subset");
				}
			}
			writer.WriteDocType(name, pubid, sysid, subset);
			dtdWritten = true;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteStartElement(string prefix, string localName, string ns)
	{
		try
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			CheckNCName(localName);
			AdvanceState(Token.StartElement);
			if (prefix == null)
			{
				if (ns != null)
				{
					prefix = LookupPrefix(ns);
				}
				if (prefix == null)
				{
					prefix = string.Empty;
				}
			}
			else if (prefix.Length > 0)
			{
				CheckNCName(prefix);
				if (ns == null)
				{
					ns = LookupNamespace(prefix);
				}
				if (ns == null || (ns != null && ns.Length == 0))
				{
					throw new ArgumentException(Res.GetString("Cannot use a prefix with an empty namespace."));
				}
			}
			if (ns == null)
			{
				ns = LookupNamespace(prefix);
				if (ns == null)
				{
					ns = string.Empty;
				}
			}
			if (elemTop == 0 && rawWriter != null)
			{
				rawWriter.OnRootElement(conformanceLevel);
			}
			writer.WriteStartElement(prefix, localName, ns);
			int num = ++elemTop;
			if (num == elemScopeStack.Length)
			{
				ElementScope[] destinationArray = new ElementScope[num * 2];
				Array.Copy(elemScopeStack, destinationArray, num);
				elemScopeStack = destinationArray;
			}
			elemScopeStack[num].Set(prefix, localName, ns, nsTop);
			PushNamespaceImplicit(prefix, ns);
			if (attrCount >= 14)
			{
				attrHashTable.Clear();
			}
			attrCount = 0;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteEndElement()
	{
		try
		{
			AdvanceState(Token.EndElement);
			int num = elemTop;
			if (num == 0)
			{
				throw new XmlException("There was no XML start tag open.", string.Empty);
			}
			if (rawWriter != null)
			{
				elemScopeStack[num].WriteEndElement(rawWriter);
			}
			else
			{
				writer.WriteEndElement();
			}
			int prevNSTop = elemScopeStack[num].prevNSTop;
			if (useNsHashtable && prevNSTop < nsTop)
			{
				PopNamespaces(prevNSTop + 1, nsTop);
			}
			nsTop = prevNSTop;
			if ((elemTop = num - 1) == 0)
			{
				if (conformanceLevel == ConformanceLevel.Document)
				{
					currentState = State.AfterRootEle;
				}
				else
				{
					currentState = State.TopLevel;
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteFullEndElement()
	{
		try
		{
			AdvanceState(Token.EndElement);
			int num = elemTop;
			if (num == 0)
			{
				throw new XmlException("There was no XML start tag open.", string.Empty);
			}
			if (rawWriter != null)
			{
				elemScopeStack[num].WriteFullEndElement(rawWriter);
			}
			else
			{
				writer.WriteFullEndElement();
			}
			int prevNSTop = elemScopeStack[num].prevNSTop;
			if (useNsHashtable && prevNSTop < nsTop)
			{
				PopNamespaces(prevNSTop + 1, nsTop);
			}
			nsTop = prevNSTop;
			if ((elemTop = num - 1) == 0)
			{
				if (conformanceLevel == ConformanceLevel.Document)
				{
					currentState = State.AfterRootEle;
				}
				else
				{
					currentState = State.TopLevel;
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteStartAttribute(string prefix, string localName, string namespaceName)
	{
		try
		{
			if (localName == null || localName.Length == 0)
			{
				if (!(prefix == "xmlns"))
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
				}
				localName = "xmlns";
				prefix = string.Empty;
			}
			CheckNCName(localName);
			AdvanceState(Token.StartAttribute);
			if (prefix == null)
			{
				if (namespaceName != null && (!(localName == "xmlns") || !(namespaceName == "http://www.w3.org/2000/xmlns/")))
				{
					prefix = LookupPrefix(namespaceName);
				}
				if (prefix == null)
				{
					prefix = string.Empty;
				}
			}
			if (namespaceName == null)
			{
				if (prefix != null && prefix.Length > 0)
				{
					namespaceName = LookupNamespace(prefix);
				}
				if (namespaceName == null)
				{
					namespaceName = string.Empty;
				}
			}
			if (prefix.Length == 0)
			{
				if (localName[0] != 'x' || !(localName == "xmlns"))
				{
					if (namespaceName.Length > 0)
					{
						prefix = LookupPrefix(namespaceName);
						if (prefix == null || prefix.Length == 0)
						{
							prefix = GeneratePrefix();
						}
					}
					goto IL_0214;
				}
				if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/2000/xmlns/")
				{
					throw new ArgumentException(Res.GetString("Prefix \"xmlns\" is reserved for use by XML."));
				}
				curDeclPrefix = string.Empty;
				SetSpecialAttribute(SpecialAttribute.DefaultXmlns);
			}
			else
			{
				if (prefix[0] != 'x')
				{
					goto IL_01e0;
				}
				if (prefix == "xmlns")
				{
					if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/2000/xmlns/")
					{
						throw new ArgumentException(Res.GetString("Prefix \"xmlns\" is reserved for use by XML."));
					}
					curDeclPrefix = localName;
					SetSpecialAttribute(SpecialAttribute.PrefixedXmlns);
				}
				else
				{
					if (!(prefix == "xml"))
					{
						goto IL_01e0;
					}
					if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/XML/1998/namespace")
					{
						throw new ArgumentException(Res.GetString("Prefix \"xml\" is reserved for use by XML and can be mapped only to namespace name \"http://www.w3.org/XML/1998/namespace\"."));
					}
					if (!(localName == "space"))
					{
						if (!(localName == "lang"))
						{
							goto IL_01e0;
						}
						SetSpecialAttribute(SpecialAttribute.XmlLang);
					}
					else
					{
						SetSpecialAttribute(SpecialAttribute.XmlSpace);
					}
				}
			}
			goto IL_0224;
			IL_0224:
			AddAttribute(prefix, localName, namespaceName);
			if (specAttr == SpecialAttribute.No)
			{
				writer.WriteStartAttribute(prefix, localName, namespaceName);
			}
			return;
			IL_0214:
			if (prefix.Length != 0)
			{
				PushNamespaceImplicit(prefix, namespaceName);
			}
			goto IL_0224;
			IL_01e0:
			CheckNCName(prefix);
			if (namespaceName.Length == 0)
			{
				prefix = string.Empty;
			}
			else
			{
				string text = LookupLocalNamespace(prefix);
				if (text != null && text != namespaceName)
				{
					prefix = GeneratePrefix();
				}
			}
			goto IL_0214;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteEndAttribute()
	{
		try
		{
			AdvanceState(Token.EndAttribute);
			if (specAttr != SpecialAttribute.No)
			{
				switch (specAttr)
				{
				case SpecialAttribute.DefaultXmlns:
				{
					string stringValue = attrValueCache.StringValue;
					if (PushNamespaceExplicit(string.Empty, stringValue))
					{
						if (rawWriter != null)
						{
							if (rawWriter.SupportsNamespaceDeclarationInChunks)
							{
								rawWriter.WriteStartNamespaceDeclaration(string.Empty);
								attrValueCache.Replay(rawWriter);
								rawWriter.WriteEndNamespaceDeclaration();
							}
							else
							{
								rawWriter.WriteNamespaceDeclaration(string.Empty, stringValue);
							}
						}
						else
						{
							writer.WriteStartAttribute(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/");
							attrValueCache.Replay(writer);
							writer.WriteEndAttribute();
						}
					}
					curDeclPrefix = null;
					break;
				}
				case SpecialAttribute.PrefixedXmlns:
				{
					string stringValue = attrValueCache.StringValue;
					if (stringValue.Length == 0)
					{
						throw new ArgumentException(Res.GetString("Cannot use a prefix with an empty namespace."));
					}
					if (stringValue == "http://www.w3.org/2000/xmlns/" || (stringValue == "http://www.w3.org/XML/1998/namespace" && curDeclPrefix != "xml"))
					{
						throw new ArgumentException(Res.GetString("Cannot bind to the reserved namespace."));
					}
					if (PushNamespaceExplicit(curDeclPrefix, stringValue))
					{
						if (rawWriter != null)
						{
							if (rawWriter.SupportsNamespaceDeclarationInChunks)
							{
								rawWriter.WriteStartNamespaceDeclaration(curDeclPrefix);
								attrValueCache.Replay(rawWriter);
								rawWriter.WriteEndNamespaceDeclaration();
							}
							else
							{
								rawWriter.WriteNamespaceDeclaration(curDeclPrefix, stringValue);
							}
						}
						else
						{
							writer.WriteStartAttribute("xmlns", curDeclPrefix, "http://www.w3.org/2000/xmlns/");
							attrValueCache.Replay(writer);
							writer.WriteEndAttribute();
						}
					}
					curDeclPrefix = null;
					break;
				}
				case SpecialAttribute.XmlSpace:
				{
					attrValueCache.Trim();
					string stringValue = attrValueCache.StringValue;
					if (stringValue == "default")
					{
						elemScopeStack[elemTop].xmlSpace = XmlSpace.Default;
					}
					else
					{
						if (!(stringValue == "preserve"))
						{
							throw new ArgumentException(Res.GetString("'{0}' is an invalid xml:space value.", stringValue));
						}
						elemScopeStack[elemTop].xmlSpace = XmlSpace.Preserve;
					}
					writer.WriteStartAttribute("xml", "space", "http://www.w3.org/XML/1998/namespace");
					attrValueCache.Replay(writer);
					writer.WriteEndAttribute();
					break;
				}
				case SpecialAttribute.XmlLang:
				{
					string stringValue = attrValueCache.StringValue;
					elemScopeStack[elemTop].xmlLang = stringValue;
					writer.WriteStartAttribute("xml", "lang", "http://www.w3.org/XML/1998/namespace");
					attrValueCache.Replay(writer);
					writer.WriteEndAttribute();
					break;
				}
				}
				specAttr = SpecialAttribute.No;
				attrValueCache.Clear();
			}
			else
			{
				writer.WriteEndAttribute();
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteCData(string text)
	{
		try
		{
			if (text == null)
			{
				text = string.Empty;
			}
			AdvanceState(Token.CData);
			writer.WriteCData(text);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteComment(string text)
	{
		try
		{
			if (text == null)
			{
				text = string.Empty;
			}
			AdvanceState(Token.Comment);
			writer.WriteComment(text);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		try
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			CheckNCName(name);
			if (text == null)
			{
				text = string.Empty;
			}
			if (name.Length == 3 && string.Compare(name, "xml", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (currentState != State.Start)
				{
					throw new ArgumentException(Res.GetString((conformanceLevel == ConformanceLevel.Document) ? "Cannot write XML declaration. WriteStartDocument method has already written it." : "Cannot write XML declaration. XML declaration can be only at the beginning of the document."));
				}
				xmlDeclFollows = true;
				AdvanceState(Token.PI);
				if (rawWriter != null)
				{
					rawWriter.WriteXmlDeclaration(text);
				}
				else
				{
					writer.WriteProcessingInstruction(name, text);
				}
			}
			else
			{
				AdvanceState(Token.PI);
				writer.WriteProcessingInstruction(name, text);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteEntityRef(string name)
	{
		try
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			CheckNCName(name);
			AdvanceState(Token.Text);
			if (SaveAttrValue)
			{
				attrValueCache.WriteEntityRef(name);
			}
			else
			{
				writer.WriteEntityRef(name);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteCharEntity(char ch)
	{
		try
		{
			if (char.IsSurrogate(ch))
			{
				throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
			}
			AdvanceState(Token.Text);
			if (SaveAttrValue)
			{
				attrValueCache.WriteCharEntity(ch);
			}
			else
			{
				writer.WriteCharEntity(ch);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		try
		{
			if (!char.IsSurrogatePair(highChar, lowChar))
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
			}
			AdvanceState(Token.Text);
			if (SaveAttrValue)
			{
				attrValueCache.WriteSurrogateCharEntity(lowChar, highChar);
			}
			else
			{
				writer.WriteSurrogateCharEntity(lowChar, highChar);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteWhitespace(string ws)
	{
		try
		{
			if (ws == null)
			{
				ws = string.Empty;
			}
			if (!XmlCharType.Instance.IsOnlyWhitespace(ws))
			{
				throw new ArgumentException(Res.GetString("Only white space characters should be used."));
			}
			AdvanceState(Token.Whitespace);
			if (SaveAttrValue)
			{
				attrValueCache.WriteWhitespace(ws);
			}
			else
			{
				writer.WriteWhitespace(ws);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteString(string text)
	{
		try
		{
			if (text != null)
			{
				AdvanceState(Token.Text);
				if (SaveAttrValue)
				{
					attrValueCache.WriteString(text);
				}
				else
				{
					writer.WriteString(text);
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteChars(char[] buffer, int index, int count)
	{
		try
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			AdvanceState(Token.Text);
			if (SaveAttrValue)
			{
				attrValueCache.WriteChars(buffer, index, count);
			}
			else
			{
				writer.WriteChars(buffer, index, count);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteRaw(char[] buffer, int index, int count)
	{
		try
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			AdvanceState(Token.RawData);
			if (SaveAttrValue)
			{
				attrValueCache.WriteRaw(buffer, index, count);
			}
			else
			{
				writer.WriteRaw(buffer, index, count);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteRaw(string data)
	{
		try
		{
			if (data != null)
			{
				AdvanceState(Token.RawData);
				if (SaveAttrValue)
				{
					attrValueCache.WriteRaw(data);
				}
				else
				{
					writer.WriteRaw(data);
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteBase64(byte[] buffer, int index, int count)
	{
		try
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			AdvanceState(Token.Base64);
			writer.WriteBase64(buffer, index, count);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void Close()
	{
		if (currentState == State.Closed)
		{
			return;
		}
		try
		{
			if (writeEndDocumentOnClose)
			{
				while (currentState != State.Error && elemTop > 0)
				{
					WriteEndElement();
				}
			}
			else if (currentState != State.Error && elemTop > 0)
			{
				try
				{
					AdvanceState(Token.EndElement);
				}
				catch
				{
					currentState = State.Error;
					throw;
				}
			}
			if (InBase64 && rawWriter != null)
			{
				rawWriter.WriteEndBase64();
			}
			writer.Flush();
		}
		finally
		{
			try
			{
				if (rawWriter != null)
				{
					rawWriter.Close(WriteState);
				}
				else
				{
					writer.Close();
				}
			}
			finally
			{
				currentState = State.Closed;
			}
		}
	}

	public override void Flush()
	{
		try
		{
			writer.Flush();
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override string LookupPrefix(string ns)
	{
		try
		{
			if (ns == null)
			{
				throw new ArgumentNullException("ns");
			}
			for (int num = nsTop; num >= 0; num--)
			{
				if (nsStack[num].namespaceUri == ns)
				{
					string prefix = nsStack[num].prefix;
					for (num++; num <= nsTop; num++)
					{
						if (nsStack[num].prefix == prefix)
						{
							return null;
						}
					}
					return prefix;
				}
			}
			return (predefinedNamespaces != null) ? predefinedNamespaces.LookupPrefix(ns) : null;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteQualifiedName(string localName, string ns)
	{
		try
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			CheckNCName(localName);
			AdvanceState(Token.Text);
			string text = string.Empty;
			if (ns != null && ns.Length != 0)
			{
				text = LookupPrefix(ns);
				if (text == null)
				{
					if (currentState != State.Attribute)
					{
						throw new ArgumentException(Res.GetString("The '{0}' namespace is not defined.", ns));
					}
					text = GeneratePrefix();
					PushNamespaceImplicit(text, ns);
				}
			}
			if (SaveAttrValue || rawWriter == null)
			{
				if (text.Length != 0)
				{
					WriteString(text);
					WriteString(":");
				}
				WriteString(localName);
			}
			else
			{
				rawWriter.WriteQualifiedName(text, localName, ns);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(bool value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(DateTime value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(DateTimeOffset value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(double value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(float value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(decimal value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(int value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(long value)
	{
		try
		{
			AdvanceState(Token.AtomicValue);
			writer.WriteValue(value);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(string value)
	{
		try
		{
			if (value != null)
			{
				if (SaveAttrValue)
				{
					AdvanceState(Token.Text);
					attrValueCache.WriteValue(value);
				}
				else
				{
					AdvanceState(Token.AtomicValue);
					writer.WriteValue(value);
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteValue(object value)
	{
		try
		{
			if (SaveAttrValue && value is string)
			{
				AdvanceState(Token.Text);
				attrValueCache.WriteValue((string)value);
			}
			else
			{
				AdvanceState(Token.AtomicValue);
				writer.WriteValue(value);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override void WriteBinHex(byte[] buffer, int index, int count)
	{
		if (IsClosedOrErrorState)
		{
			throw new InvalidOperationException(Res.GetString("The Writer is closed or in error state."));
		}
		try
		{
			AdvanceState(Token.Text);
			base.WriteBinHex(buffer, index, count);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private void SetSpecialAttribute(SpecialAttribute special)
	{
		specAttr = special;
		if (State.Attribute == currentState)
		{
			currentState = State.SpecialAttr;
		}
		else if (State.RootLevelAttr == currentState)
		{
			currentState = State.RootLevelSpecAttr;
		}
		if (attrValueCache == null)
		{
			attrValueCache = new AttributeValueCache();
		}
	}

	private void WriteStartDocumentImpl(XmlStandalone standalone)
	{
		try
		{
			AdvanceState(Token.StartDocument);
			if (conformanceLevel == ConformanceLevel.Auto)
			{
				conformanceLevel = ConformanceLevel.Document;
				stateTable = StateTableDocument;
			}
			else if (conformanceLevel == ConformanceLevel.Fragment)
			{
				throw new InvalidOperationException(Res.GetString("WriteStartDocument cannot be called on writers created with ConformanceLevel.Fragment."));
			}
			if (rawWriter != null)
			{
				if (!xmlDeclFollows)
				{
					rawWriter.WriteXmlDeclaration(standalone);
				}
			}
			else
			{
				writer.WriteStartDocument();
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private void StartFragment()
	{
		conformanceLevel = ConformanceLevel.Fragment;
	}

	private void PushNamespaceImplicit(string prefix, string ns)
	{
		int num = LookupNamespaceIndex(prefix);
		NamespaceKind kind;
		if (num != -1)
		{
			if (num > elemScopeStack[elemTop].prevNSTop)
			{
				if (nsStack[num].namespaceUri != ns)
				{
					throw new XmlException("The prefix '{0}' cannot be redefined from '{1}' to '{2}' within the same start element tag.", new string[3]
					{
						prefix,
						nsStack[num].namespaceUri,
						ns
					});
				}
				return;
			}
			if (nsStack[num].kind == NamespaceKind.Special)
			{
				if (!(prefix == "xml"))
				{
					throw new ArgumentException(Res.GetString("Prefix \"xmlns\" is reserved for use by XML."));
				}
				if (ns != nsStack[num].namespaceUri)
				{
					throw new ArgumentException(Res.GetString("Prefix \"xml\" is reserved for use by XML and can be mapped only to namespace name \"http://www.w3.org/XML/1998/namespace\"."));
				}
				kind = NamespaceKind.Implied;
			}
			else
			{
				kind = ((!(nsStack[num].namespaceUri == ns)) ? NamespaceKind.NeedToWrite : NamespaceKind.Implied);
			}
		}
		else
		{
			if ((ns == "http://www.w3.org/XML/1998/namespace" && prefix != "xml") || (ns == "http://www.w3.org/2000/xmlns/" && prefix != "xmlns"))
			{
				throw new ArgumentException(Res.GetString("Prefix '{0}' cannot be mapped to namespace name reserved for \"xml\" or \"xmlns\".", prefix));
			}
			kind = ((predefinedNamespaces == null) ? NamespaceKind.NeedToWrite : ((!(predefinedNamespaces.LookupNamespace(prefix) == ns)) ? NamespaceKind.NeedToWrite : NamespaceKind.Implied));
		}
		AddNamespace(prefix, ns, kind);
	}

	private bool PushNamespaceExplicit(string prefix, string ns)
	{
		bool result = true;
		int num = LookupNamespaceIndex(prefix);
		if (num != -1)
		{
			if (num > elemScopeStack[elemTop].prevNSTop)
			{
				if (nsStack[num].namespaceUri != ns)
				{
					throw new XmlException("The prefix '{0}' cannot be redefined from '{1}' to '{2}' within the same start element tag.", new string[3]
					{
						prefix,
						nsStack[num].namespaceUri,
						ns
					});
				}
				NamespaceKind kind = nsStack[num].kind;
				if (kind == NamespaceKind.Written)
				{
					throw DupAttrException((prefix.Length == 0) ? string.Empty : "xmlns", (prefix.Length == 0) ? "xmlns" : prefix);
				}
				if (omitDuplNamespaces && kind != NamespaceKind.NeedToWrite)
				{
					result = false;
				}
				nsStack[num].kind = NamespaceKind.Written;
				return result;
			}
			if (nsStack[num].namespaceUri == ns && omitDuplNamespaces)
			{
				result = false;
			}
		}
		else if (predefinedNamespaces != null && predefinedNamespaces.LookupNamespace(prefix) == ns && omitDuplNamespaces)
		{
			result = false;
		}
		if ((ns == "http://www.w3.org/XML/1998/namespace" && prefix != "xml") || (ns == "http://www.w3.org/2000/xmlns/" && prefix != "xmlns"))
		{
			throw new ArgumentException(Res.GetString("Prefix '{0}' cannot be mapped to namespace name reserved for \"xml\" or \"xmlns\".", prefix));
		}
		if (prefix.Length > 0 && prefix[0] == 'x')
		{
			if (prefix == "xml")
			{
				if (ns != "http://www.w3.org/XML/1998/namespace")
				{
					throw new ArgumentException(Res.GetString("Prefix \"xml\" is reserved for use by XML and can be mapped only to namespace name \"http://www.w3.org/XML/1998/namespace\"."));
				}
			}
			else if (prefix == "xmlns")
			{
				throw new ArgumentException(Res.GetString("Prefix \"xmlns\" is reserved for use by XML."));
			}
		}
		AddNamespace(prefix, ns, NamespaceKind.Written);
		return result;
	}

	private void AddNamespace(string prefix, string ns, NamespaceKind kind)
	{
		int num = ++nsTop;
		if (num == nsStack.Length)
		{
			Namespace[] destinationArray = new Namespace[num * 2];
			Array.Copy(nsStack, destinationArray, num);
			nsStack = destinationArray;
		}
		nsStack[num].Set(prefix, ns, kind);
		if (useNsHashtable)
		{
			AddToNamespaceHashtable(nsTop);
		}
		else if (nsTop == 16)
		{
			nsHashtable = new Dictionary<string, int>(hasher);
			for (int i = 0; i <= nsTop; i++)
			{
				AddToNamespaceHashtable(i);
			}
			useNsHashtable = true;
		}
	}

	private void AddToNamespaceHashtable(int namespaceIndex)
	{
		string prefix = nsStack[namespaceIndex].prefix;
		if (nsHashtable.TryGetValue(prefix, out var value))
		{
			nsStack[namespaceIndex].prevNsIndex = value;
		}
		nsHashtable[prefix] = namespaceIndex;
	}

	private int LookupNamespaceIndex(string prefix)
	{
		if (useNsHashtable)
		{
			if (nsHashtable.TryGetValue(prefix, out var value))
			{
				return value;
			}
		}
		else
		{
			for (int num = nsTop; num >= 0; num--)
			{
				if (nsStack[num].prefix == prefix)
				{
					return num;
				}
			}
		}
		return -1;
	}

	private void PopNamespaces(int indexFrom, int indexTo)
	{
		for (int num = indexTo; num >= indexFrom; num--)
		{
			if (nsStack[num].prevNsIndex == -1)
			{
				nsHashtable.Remove(nsStack[num].prefix);
			}
			else
			{
				nsHashtable[nsStack[num].prefix] = nsStack[num].prevNsIndex;
			}
		}
	}

	private static XmlException DupAttrException(string prefix, string localName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (prefix.Length > 0)
		{
			stringBuilder.Append(prefix);
			stringBuilder.Append(':');
		}
		stringBuilder.Append(localName);
		return new XmlException("'{0}' is a duplicate attribute name.", stringBuilder.ToString());
	}

	private void AdvanceState(Token token)
	{
		if (currentState >= State.Closed)
		{
			if (currentState == State.Closed || currentState == State.Error)
			{
				throw new InvalidOperationException(Res.GetString("The Writer is closed or in error state."));
			}
			throw new InvalidOperationException(Res.GetString("Token {0} in state {1} would result in an invalid XML document.", tokenName[(int)token], GetStateName(currentState)));
		}
		State state;
		while (true)
		{
			state = stateTable[(int)(((int)token << 4) + currentState)];
			switch (state)
			{
			case State.Error:
				ThrowInvalidStateTransition(token, currentState);
				break;
			case State.StartContent:
				StartElementContent();
				state = State.Content;
				break;
			case State.StartContentEle:
				StartElementContent();
				state = State.Element;
				break;
			case State.StartContentB64:
				StartElementContent();
				state = State.B64Content;
				break;
			case State.StartDoc:
				WriteStartDocument();
				state = State.Document;
				break;
			case State.StartDocEle:
				WriteStartDocument();
				state = State.Element;
				break;
			case State.EndAttrSEle:
				WriteEndAttribute();
				StartElementContent();
				state = State.Element;
				break;
			case State.EndAttrEEle:
				WriteEndAttribute();
				StartElementContent();
				state = State.Content;
				break;
			case State.EndAttrSCont:
				WriteEndAttribute();
				StartElementContent();
				state = State.Content;
				break;
			case State.EndAttrSAttr:
				WriteEndAttribute();
				state = State.Attribute;
				break;
			case State.PostB64Cont:
				if (rawWriter != null)
				{
					rawWriter.WriteEndBase64();
				}
				currentState = State.Content;
				continue;
			case State.PostB64Attr:
				if (rawWriter != null)
				{
					rawWriter.WriteEndBase64();
				}
				currentState = State.Attribute;
				continue;
			case State.PostB64RootAttr:
				if (rawWriter != null)
				{
					rawWriter.WriteEndBase64();
				}
				currentState = State.RootLevelAttr;
				continue;
			case State.StartFragEle:
				StartFragment();
				state = State.Element;
				break;
			case State.StartFragCont:
				StartFragment();
				state = State.Content;
				break;
			case State.StartFragB64:
				StartFragment();
				state = State.B64Content;
				break;
			case State.StartRootLevelAttr:
				WriteEndAttribute();
				state = State.RootLevelAttr;
				break;
			}
			break;
		}
		currentState = state;
	}

	private void StartElementContent()
	{
		int prevNSTop = elemScopeStack[elemTop].prevNSTop;
		for (int num = nsTop; num > prevNSTop; num--)
		{
			if (nsStack[num].kind == NamespaceKind.NeedToWrite)
			{
				nsStack[num].WriteDecl(writer, rawWriter);
			}
		}
		if (rawWriter != null)
		{
			rawWriter.StartElementContent();
		}
	}

	private static string GetStateName(State state)
	{
		if (state >= State.Error)
		{
			return "Error";
		}
		return stateName[(int)state];
	}

	internal string LookupNamespace(string prefix)
	{
		for (int num = nsTop; num >= 0; num--)
		{
			if (nsStack[num].prefix == prefix)
			{
				return nsStack[num].namespaceUri;
			}
		}
		if (predefinedNamespaces == null)
		{
			return null;
		}
		return predefinedNamespaces.LookupNamespace(prefix);
	}

	private string LookupLocalNamespace(string prefix)
	{
		for (int num = nsTop; num > elemScopeStack[elemTop].prevNSTop; num--)
		{
			if (nsStack[num].prefix == prefix)
			{
				return nsStack[num].namespaceUri;
			}
		}
		return null;
	}

	private string GeneratePrefix()
	{
		string text = "p" + (nsTop - 2).ToString("d", CultureInfo.InvariantCulture);
		if (LookupNamespace(text) == null)
		{
			return text;
		}
		int num = 0;
		string text2;
		do
		{
			text2 = text + num.ToString(CultureInfo.InvariantCulture);
			num++;
		}
		while (LookupNamespace(text2) != null);
		return text2;
	}

	private void CheckNCName(string ncname)
	{
		int length = ncname.Length;
		if ((xmlCharType.charProperties[(uint)ncname[0]] & 4) != 0)
		{
			for (int i = 1; i < length; i++)
			{
				if ((xmlCharType.charProperties[(uint)ncname[i]] & 8) == 0)
				{
					throw InvalidCharsException(ncname, i);
				}
			}
			return;
		}
		throw InvalidCharsException(ncname, 0);
	}

	private static Exception InvalidCharsException(string name, int badCharIndex)
	{
		string[] array = XmlException.BuildCharExceptionArgs(name, badCharIndex);
		string[] array2 = new string[3]
		{
			name,
			array[0],
			array[1]
		};
		object[] args = array2;
		return new ArgumentException(Res.GetString("Invalid name character in '{0}'. The '{1}' character, hexadecimal value {2}, cannot be included in a name.", args));
	}

	private void ThrowInvalidStateTransition(Token token, State currentState)
	{
		string text = Res.GetString("Token {0} in state {1} would result in an invalid XML document.", tokenName[(int)token], GetStateName(currentState));
		if ((currentState == State.Start || currentState == State.AfterRootEle) && conformanceLevel == ConformanceLevel.Document)
		{
			throw new InvalidOperationException(text + " " + Res.GetString("Make sure that the ConformanceLevel setting is set to ConformanceLevel.Fragment or ConformanceLevel.Auto if you want to write an XML fragment."));
		}
		throw new InvalidOperationException(text);
	}

	private void AddAttribute(string prefix, string localName, string namespaceName)
	{
		int num = attrCount++;
		if (num == attrStack.Length)
		{
			AttrName[] destinationArray = new AttrName[num * 2];
			Array.Copy(attrStack, destinationArray, num);
			attrStack = destinationArray;
		}
		attrStack[num].Set(prefix, localName, namespaceName);
		if (attrCount < 14)
		{
			for (int i = 0; i < num; i++)
			{
				if (attrStack[i].IsDuplicate(prefix, localName, namespaceName))
				{
					throw DupAttrException(prefix, localName);
				}
			}
			return;
		}
		if (attrCount == 14)
		{
			if (attrHashTable == null)
			{
				attrHashTable = new Dictionary<string, int>(hasher);
			}
			for (int j = 0; j < num; j++)
			{
				AddToAttrHashTable(j);
			}
		}
		AddToAttrHashTable(num);
		int prev;
		for (prev = attrStack[num].prev; prev > 0; prev = attrStack[prev].prev)
		{
			prev--;
			if (attrStack[prev].IsDuplicate(prefix, localName, namespaceName))
			{
				throw DupAttrException(prefix, localName);
			}
		}
	}

	private void AddToAttrHashTable(int attributeIndex)
	{
		string localName = attrStack[attributeIndex].localName;
		int count = attrHashTable.Count;
		attrHashTable[localName] = 0;
		if (count == attrHashTable.Count)
		{
			int num = attributeIndex - 1;
			while (num >= 0 && !(attrStack[num].localName == localName))
			{
				num--;
			}
			attrStack[attributeIndex].prev = num + 1;
		}
	}

	public override Task WriteStartDocumentAsync()
	{
		return WriteStartDocumentImplAsync(XmlStandalone.Omit);
	}

	public override Task WriteStartDocumentAsync(bool standalone)
	{
		return WriteStartDocumentImplAsync(standalone ? XmlStandalone.Yes : XmlStandalone.No);
	}

	public override async Task WriteEndDocumentAsync()
	{
		_ = 2;
		try
		{
			while (elemTop > 0)
			{
				await WriteEndElementAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			State prevState = currentState;
			await AdvanceStateAsync(Token.EndDocument).ConfigureAwait(continueOnCapturedContext: false);
			if (prevState != State.AfterRootEle)
			{
				throw new ArgumentException(Res.GetString("Document does not have a root element."));
			}
			if (rawWriter == null)
			{
				await writer.WriteEndDocumentAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset)
	{
		_ = 1;
		try
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			XmlConvert.VerifyQName(name, ExceptionType.XmlException);
			if (conformanceLevel == ConformanceLevel.Fragment)
			{
				throw new InvalidOperationException(Res.GetString("DTD is not allowed in XML fragments."));
			}
			await AdvanceStateAsync(Token.Dtd).ConfigureAwait(continueOnCapturedContext: false);
			if (dtdWritten)
			{
				currentState = State.Error;
				throw new InvalidOperationException(Res.GetString("The DTD has already been written out."));
			}
			if (conformanceLevel == ConformanceLevel.Auto)
			{
				conformanceLevel = ConformanceLevel.Document;
				stateTable = StateTableDocument;
			}
			if (checkCharacters)
			{
				int invCharIndex;
				if (pubid != null && (invCharIndex = xmlCharType.IsPublicId(pubid)) >= 0)
				{
					object[] args = XmlException.BuildCharExceptionArgs(pubid, invCharIndex);
					throw new ArgumentException(Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args), "pubid");
				}
				if (sysid != null && (invCharIndex = xmlCharType.IsOnlyCharData(sysid)) >= 0)
				{
					object[] args = XmlException.BuildCharExceptionArgs(sysid, invCharIndex);
					throw new ArgumentException(Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args), "sysid");
				}
				if (subset != null && (invCharIndex = xmlCharType.IsOnlyCharData(subset)) >= 0)
				{
					object[] args = XmlException.BuildCharExceptionArgs(subset, invCharIndex);
					throw new ArgumentException(Res.GetString("'{0}', hexadecimal value {1}, is an invalid character.", args), "subset");
				}
			}
			await writer.WriteDocTypeAsync(name, pubid, sysid, subset).ConfigureAwait(continueOnCapturedContext: false);
			dtdWritten = true;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task TryReturnTask(Task task)
	{
		if (task.IsSuccess())
		{
			return AsyncHelper.DoneTask;
		}
		return _TryReturnTask(task);
	}

	private async Task _TryReturnTask(Task task)
	{
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task SequenceRun(Task task, Func<Task> nextTaskFun)
	{
		if (task.IsSuccess())
		{
			return TryReturnTask(nextTaskFun());
		}
		return _SequenceRun(task, nextTaskFun);
	}

	private async Task _SequenceRun(Task task, Func<Task> nextTaskFun)
	{
		_ = 1;
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			await nextTaskFun().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override Task WriteStartElementAsync(string prefix, string localName, string ns)
	{
		try
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			CheckNCName(localName);
			Task task = AdvanceStateAsync(Token.StartElement);
			if (task.IsSuccess())
			{
				return WriteStartElementAsync_NoAdvanceState(prefix, localName, ns);
			}
			return WriteStartElementAsync_NoAdvanceState(task, prefix, localName, ns);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteStartElementAsync_NoAdvanceState(string prefix, string localName, string ns)
	{
		try
		{
			if (prefix == null)
			{
				if (ns != null)
				{
					prefix = LookupPrefix(ns);
				}
				if (prefix == null)
				{
					prefix = string.Empty;
				}
			}
			else if (prefix.Length > 0)
			{
				CheckNCName(prefix);
				if (ns == null)
				{
					ns = LookupNamespace(prefix);
				}
				if (ns == null || (ns != null && ns.Length == 0))
				{
					throw new ArgumentException(Res.GetString("Cannot use a prefix with an empty namespace."));
				}
			}
			if (ns == null)
			{
				ns = LookupNamespace(prefix);
				if (ns == null)
				{
					ns = string.Empty;
				}
			}
			if (elemTop == 0 && rawWriter != null)
			{
				rawWriter.OnRootElement(conformanceLevel);
			}
			Task task = writer.WriteStartElementAsync(prefix, localName, ns);
			if (task.IsSuccess())
			{
				WriteStartElementAsync_FinishWrite(prefix, localName, ns);
				return AsyncHelper.DoneTask;
			}
			return WriteStartElementAsync_FinishWrite(task, prefix, localName, ns);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteStartElementAsync_NoAdvanceState(Task task, string prefix, string localName, string ns)
	{
		_ = 1;
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			await WriteStartElementAsync_NoAdvanceState(prefix, localName, ns).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private void WriteStartElementAsync_FinishWrite(string prefix, string localName, string ns)
	{
		try
		{
			int num = ++elemTop;
			if (num == elemScopeStack.Length)
			{
				ElementScope[] destinationArray = new ElementScope[num * 2];
				Array.Copy(elemScopeStack, destinationArray, num);
				elemScopeStack = destinationArray;
			}
			elemScopeStack[num].Set(prefix, localName, ns, nsTop);
			PushNamespaceImplicit(prefix, ns);
			if (attrCount >= 14)
			{
				attrHashTable.Clear();
			}
			attrCount = 0;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteStartElementAsync_FinishWrite(Task t, string prefix, string localName, string ns)
	{
		try
		{
			await t.ConfigureAwait(continueOnCapturedContext: false);
			WriteStartElementAsync_FinishWrite(prefix, localName, ns);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override Task WriteEndElementAsync()
	{
		try
		{
			Task task = AdvanceStateAsync(Token.EndElement);
			return SequenceRun(task, WriteEndElementAsync_NoAdvanceState);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteEndElementAsync_NoAdvanceState()
	{
		try
		{
			int num = elemTop;
			if (num == 0)
			{
				throw new XmlException("There was no XML start tag open.", string.Empty);
			}
			Task task = ((rawWriter == null) ? writer.WriteEndElementAsync() : elemScopeStack[num].WriteEndElementAsync(rawWriter));
			return SequenceRun(task, WriteEndElementAsync_FinishWrite);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteEndElementAsync_FinishWrite()
	{
		try
		{
			int num = elemTop;
			int prevNSTop = elemScopeStack[num].prevNSTop;
			if (useNsHashtable && prevNSTop < nsTop)
			{
				PopNamespaces(prevNSTop + 1, nsTop);
			}
			nsTop = prevNSTop;
			if ((elemTop = num - 1) == 0)
			{
				if (conformanceLevel == ConformanceLevel.Document)
				{
					currentState = State.AfterRootEle;
				}
				else
				{
					currentState = State.TopLevel;
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
		return AsyncHelper.DoneTask;
	}

	public override Task WriteFullEndElementAsync()
	{
		try
		{
			Task task = AdvanceStateAsync(Token.EndElement);
			return SequenceRun(task, WriteFullEndElementAsync_NoAdvanceState);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteFullEndElementAsync_NoAdvanceState()
	{
		try
		{
			int num = elemTop;
			if (num == 0)
			{
				throw new XmlException("There was no XML start tag open.", string.Empty);
			}
			Task task = ((rawWriter == null) ? writer.WriteFullEndElementAsync() : elemScopeStack[num].WriteFullEndElementAsync(rawWriter));
			return SequenceRun(task, WriteEndElementAsync_FinishWrite);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	protected internal override Task WriteStartAttributeAsync(string prefix, string localName, string namespaceName)
	{
		try
		{
			if (localName == null || localName.Length == 0)
			{
				if (!(prefix == "xmlns"))
				{
					throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
				}
				localName = "xmlns";
				prefix = string.Empty;
			}
			CheckNCName(localName);
			Task task = AdvanceStateAsync(Token.StartAttribute);
			if (task.IsSuccess())
			{
				return WriteStartAttributeAsync_NoAdvanceState(prefix, localName, namespaceName);
			}
			return WriteStartAttributeAsync_NoAdvanceState(task, prefix, localName, namespaceName);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteStartAttributeAsync_NoAdvanceState(string prefix, string localName, string namespaceName)
	{
		try
		{
			if (prefix == null)
			{
				if (namespaceName != null && (!(localName == "xmlns") || !(namespaceName == "http://www.w3.org/2000/xmlns/")))
				{
					prefix = LookupPrefix(namespaceName);
				}
				if (prefix == null)
				{
					prefix = string.Empty;
				}
			}
			if (namespaceName == null)
			{
				if (prefix != null && prefix.Length > 0)
				{
					namespaceName = LookupNamespace(prefix);
				}
				if (namespaceName == null)
				{
					namespaceName = string.Empty;
				}
			}
			if (prefix.Length == 0)
			{
				if (localName[0] != 'x' || !(localName == "xmlns"))
				{
					if (namespaceName.Length > 0)
					{
						prefix = LookupPrefix(namespaceName);
						if (prefix == null || prefix.Length == 0)
						{
							prefix = GeneratePrefix();
						}
					}
					goto IL_01ce;
				}
				if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/2000/xmlns/")
				{
					throw new ArgumentException(Res.GetString("Prefix \"xmlns\" is reserved for use by XML."));
				}
				curDeclPrefix = string.Empty;
				SetSpecialAttribute(SpecialAttribute.DefaultXmlns);
			}
			else
			{
				if (prefix[0] != 'x')
				{
					goto IL_019a;
				}
				if (prefix == "xmlns")
				{
					if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/2000/xmlns/")
					{
						throw new ArgumentException(Res.GetString("Prefix \"xmlns\" is reserved for use by XML."));
					}
					curDeclPrefix = localName;
					SetSpecialAttribute(SpecialAttribute.PrefixedXmlns);
				}
				else
				{
					if (!(prefix == "xml"))
					{
						goto IL_019a;
					}
					if (namespaceName.Length > 0 && namespaceName != "http://www.w3.org/XML/1998/namespace")
					{
						throw new ArgumentException(Res.GetString("Prefix \"xml\" is reserved for use by XML and can be mapped only to namespace name \"http://www.w3.org/XML/1998/namespace\"."));
					}
					if (!(localName == "space"))
					{
						if (!(localName == "lang"))
						{
							goto IL_019a;
						}
						SetSpecialAttribute(SpecialAttribute.XmlLang);
					}
					else
					{
						SetSpecialAttribute(SpecialAttribute.XmlSpace);
					}
				}
			}
			goto IL_01de;
			IL_01de:
			AddAttribute(prefix, localName, namespaceName);
			if (specAttr == SpecialAttribute.No)
			{
				return TryReturnTask(writer.WriteStartAttributeAsync(prefix, localName, namespaceName));
			}
			return AsyncHelper.DoneTask;
			IL_01ce:
			if (prefix.Length != 0)
			{
				PushNamespaceImplicit(prefix, namespaceName);
			}
			goto IL_01de;
			IL_019a:
			CheckNCName(prefix);
			if (namespaceName.Length == 0)
			{
				prefix = string.Empty;
			}
			else
			{
				string text = LookupLocalNamespace(prefix);
				if (text != null && text != namespaceName)
				{
					prefix = GeneratePrefix();
				}
			}
			goto IL_01ce;
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteStartAttributeAsync_NoAdvanceState(Task task, string prefix, string localName, string namespaceName)
	{
		_ = 1;
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			await WriteStartAttributeAsync_NoAdvanceState(prefix, localName, namespaceName).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	protected internal override Task WriteEndAttributeAsync()
	{
		try
		{
			Task task = AdvanceStateAsync(Token.EndAttribute);
			return SequenceRun(task, WriteEndAttributeAsync_NoAdvance);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteEndAttributeAsync_NoAdvance()
	{
		try
		{
			if (specAttr != SpecialAttribute.No)
			{
				return WriteEndAttributeAsync_SepcialAtt();
			}
			return TryReturnTask(writer.WriteEndAttributeAsync());
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteEndAttributeAsync_SepcialAtt()
	{
		_ = 19;
		try
		{
			switch (specAttr)
			{
			case SpecialAttribute.DefaultXmlns:
			{
				string stringValue = attrValueCache.StringValue;
				if (PushNamespaceExplicit(string.Empty, stringValue))
				{
					if (rawWriter == null)
					{
						await writer.WriteStartAttributeAsync(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/").ConfigureAwait(continueOnCapturedContext: false);
						await attrValueCache.ReplayAsync(writer).ConfigureAwait(continueOnCapturedContext: false);
						await writer.WriteEndAttributeAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					else if (!rawWriter.SupportsNamespaceDeclarationInChunks)
					{
						await rawWriter.WriteNamespaceDeclarationAsync(string.Empty, stringValue).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						await rawWriter.WriteStartNamespaceDeclarationAsync(string.Empty).ConfigureAwait(continueOnCapturedContext: false);
						await attrValueCache.ReplayAsync(rawWriter).ConfigureAwait(continueOnCapturedContext: false);
						await rawWriter.WriteEndNamespaceDeclarationAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				curDeclPrefix = null;
				break;
			}
			case SpecialAttribute.PrefixedXmlns:
			{
				string stringValue = attrValueCache.StringValue;
				if (stringValue.Length == 0)
				{
					throw new ArgumentException(Res.GetString("Cannot use a prefix with an empty namespace."));
				}
				if (stringValue == "http://www.w3.org/2000/xmlns/" || (stringValue == "http://www.w3.org/XML/1998/namespace" && curDeclPrefix != "xml"))
				{
					throw new ArgumentException(Res.GetString("Cannot bind to the reserved namespace."));
				}
				if (PushNamespaceExplicit(curDeclPrefix, stringValue))
				{
					if (rawWriter == null)
					{
						await writer.WriteStartAttributeAsync("xmlns", curDeclPrefix, "http://www.w3.org/2000/xmlns/").ConfigureAwait(continueOnCapturedContext: false);
						await attrValueCache.ReplayAsync(writer).ConfigureAwait(continueOnCapturedContext: false);
						await writer.WriteEndAttributeAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
					else if (!rawWriter.SupportsNamespaceDeclarationInChunks)
					{
						await rawWriter.WriteNamespaceDeclarationAsync(curDeclPrefix, stringValue).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						await rawWriter.WriteStartNamespaceDeclarationAsync(curDeclPrefix).ConfigureAwait(continueOnCapturedContext: false);
						await attrValueCache.ReplayAsync(rawWriter).ConfigureAwait(continueOnCapturedContext: false);
						await rawWriter.WriteEndNamespaceDeclarationAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				curDeclPrefix = null;
				break;
			}
			case SpecialAttribute.XmlSpace:
			{
				attrValueCache.Trim();
				string stringValue = attrValueCache.StringValue;
				if (stringValue == "default")
				{
					elemScopeStack[elemTop].xmlSpace = XmlSpace.Default;
				}
				else
				{
					if (!(stringValue == "preserve"))
					{
						throw new ArgumentException(Res.GetString("'{0}' is an invalid xml:space value.", stringValue));
					}
					elemScopeStack[elemTop].xmlSpace = XmlSpace.Preserve;
				}
				await writer.WriteStartAttributeAsync("xml", "space", "http://www.w3.org/XML/1998/namespace").ConfigureAwait(continueOnCapturedContext: false);
				await attrValueCache.ReplayAsync(writer).ConfigureAwait(continueOnCapturedContext: false);
				await writer.WriteEndAttributeAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
			case SpecialAttribute.XmlLang:
			{
				string stringValue = attrValueCache.StringValue;
				elemScopeStack[elemTop].xmlLang = stringValue;
				await writer.WriteStartAttributeAsync("xml", "lang", "http://www.w3.org/XML/1998/namespace").ConfigureAwait(continueOnCapturedContext: false);
				await attrValueCache.ReplayAsync(writer).ConfigureAwait(continueOnCapturedContext: false);
				await writer.WriteEndAttributeAsync().ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
			}
			specAttr = SpecialAttribute.No;
			attrValueCache.Clear();
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteCDataAsync(string text)
	{
		_ = 1;
		try
		{
			if (text == null)
			{
				text = string.Empty;
			}
			await AdvanceStateAsync(Token.CData).ConfigureAwait(continueOnCapturedContext: false);
			await writer.WriteCDataAsync(text).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteCommentAsync(string text)
	{
		_ = 1;
		try
		{
			if (text == null)
			{
				text = string.Empty;
			}
			await AdvanceStateAsync(Token.Comment).ConfigureAwait(continueOnCapturedContext: false);
			await writer.WriteCommentAsync(text).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteProcessingInstructionAsync(string name, string text)
	{
		_ = 4;
		try
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			CheckNCName(name);
			if (text == null)
			{
				text = string.Empty;
			}
			if (name.Length != 3 || string.Compare(name, "xml", StringComparison.OrdinalIgnoreCase) != 0)
			{
				await AdvanceStateAsync(Token.PI).ConfigureAwait(continueOnCapturedContext: false);
				await writer.WriteProcessingInstructionAsync(name, text).ConfigureAwait(continueOnCapturedContext: false);
				return;
			}
			if (currentState != State.Start)
			{
				throw new ArgumentException(Res.GetString((conformanceLevel == ConformanceLevel.Document) ? "Cannot write XML declaration. WriteStartDocument method has already written it." : "Cannot write XML declaration. XML declaration can be only at the beginning of the document."));
			}
			xmlDeclFollows = true;
			await AdvanceStateAsync(Token.PI).ConfigureAwait(continueOnCapturedContext: false);
			if (rawWriter != null)
			{
				await rawWriter.WriteXmlDeclarationAsync(text).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await writer.WriteProcessingInstructionAsync(name, text).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteEntityRefAsync(string name)
	{
		_ = 1;
		try
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid name."));
			}
			CheckNCName(name);
			await AdvanceStateAsync(Token.Text).ConfigureAwait(continueOnCapturedContext: false);
			if (SaveAttrValue)
			{
				attrValueCache.WriteEntityRef(name);
			}
			else
			{
				await writer.WriteEntityRefAsync(name).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteCharEntityAsync(char ch)
	{
		_ = 1;
		try
		{
			if (char.IsSurrogate(ch))
			{
				throw new ArgumentException(Res.GetString("The surrogate pair is invalid. Missing a low surrogate character."));
			}
			await AdvanceStateAsync(Token.Text).ConfigureAwait(continueOnCapturedContext: false);
			if (SaveAttrValue)
			{
				attrValueCache.WriteCharEntity(ch);
			}
			else
			{
				await writer.WriteCharEntityAsync(ch).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteSurrogateCharEntityAsync(char lowChar, char highChar)
	{
		_ = 1;
		try
		{
			if (!char.IsSurrogatePair(highChar, lowChar))
			{
				throw XmlConvert.CreateInvalidSurrogatePairException(lowChar, highChar);
			}
			await AdvanceStateAsync(Token.Text).ConfigureAwait(continueOnCapturedContext: false);
			if (SaveAttrValue)
			{
				attrValueCache.WriteSurrogateCharEntity(lowChar, highChar);
			}
			else
			{
				await writer.WriteSurrogateCharEntityAsync(lowChar, highChar).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteWhitespaceAsync(string ws)
	{
		_ = 1;
		try
		{
			if (ws == null)
			{
				ws = string.Empty;
			}
			if (!XmlCharType.Instance.IsOnlyWhitespace(ws))
			{
				throw new ArgumentException(Res.GetString("Only white space characters should be used."));
			}
			await AdvanceStateAsync(Token.Whitespace).ConfigureAwait(continueOnCapturedContext: false);
			if (SaveAttrValue)
			{
				attrValueCache.WriteWhitespace(ws);
			}
			else
			{
				await writer.WriteWhitespaceAsync(ws).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override Task WriteStringAsync(string text)
	{
		try
		{
			if (text == null)
			{
				return AsyncHelper.DoneTask;
			}
			Task task = AdvanceStateAsync(Token.Text);
			if (task.IsSuccess())
			{
				return WriteStringAsync_NoAdvanceState(text);
			}
			return WriteStringAsync_NoAdvanceState(task, text);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task WriteStringAsync_NoAdvanceState(string text)
	{
		try
		{
			if (SaveAttrValue)
			{
				attrValueCache.WriteString(text);
				return AsyncHelper.DoneTask;
			}
			return TryReturnTask(writer.WriteStringAsync(text));
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteStringAsync_NoAdvanceState(Task task, string text)
	{
		_ = 1;
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			await WriteStringAsync_NoAdvanceState(text).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteCharsAsync(char[] buffer, int index, int count)
	{
		_ = 1;
		try
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			await AdvanceStateAsync(Token.Text).ConfigureAwait(continueOnCapturedContext: false);
			if (SaveAttrValue)
			{
				attrValueCache.WriteChars(buffer, index, count);
			}
			else
			{
				await writer.WriteCharsAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteRawAsync(char[] buffer, int index, int count)
	{
		_ = 1;
		try
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			await AdvanceStateAsync(Token.RawData).ConfigureAwait(continueOnCapturedContext: false);
			if (SaveAttrValue)
			{
				attrValueCache.WriteRaw(buffer, index, count);
			}
			else
			{
				await writer.WriteRawAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteRawAsync(string data)
	{
		_ = 1;
		try
		{
			if (data != null)
			{
				await AdvanceStateAsync(Token.RawData).ConfigureAwait(continueOnCapturedContext: false);
				if (SaveAttrValue)
				{
					attrValueCache.WriteRaw(data);
				}
				else
				{
					await writer.WriteRawAsync(data).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		try
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Task task = AdvanceStateAsync(Token.Base64);
			if (task.IsSuccess())
			{
				return TryReturnTask(writer.WriteBase64Async(buffer, index, count));
			}
			return WriteBase64Async_NoAdvanceState(task, buffer, index, count);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteBase64Async_NoAdvanceState(Task task, byte[] buffer, int index, int count)
	{
		_ = 1;
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
			await writer.WriteBase64Async(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task FlushAsync()
	{
		try
		{
			await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteQualifiedNameAsync(string localName, string ns)
	{
		_ = 4;
		try
		{
			if (localName == null || localName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("The empty string '' is not a valid local name."));
			}
			CheckNCName(localName);
			await AdvanceStateAsync(Token.Text).ConfigureAwait(continueOnCapturedContext: false);
			string text = string.Empty;
			if (ns != null && ns.Length != 0)
			{
				text = LookupPrefix(ns);
				if (text == null)
				{
					if (currentState != State.Attribute)
					{
						throw new ArgumentException(Res.GetString("The '{0}' namespace is not defined.", ns));
					}
					text = GeneratePrefix();
					PushNamespaceImplicit(text, ns);
				}
			}
			if (SaveAttrValue || rawWriter == null)
			{
				if (text.Length != 0)
				{
					await WriteStringAsync(text).ConfigureAwait(continueOnCapturedContext: false);
					await WriteStringAsync(":").ConfigureAwait(continueOnCapturedContext: false);
				}
				await WriteStringAsync(localName).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await rawWriter.WriteQualifiedNameAsync(text, localName, ns).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	public override async Task WriteBinHexAsync(byte[] buffer, int index, int count)
	{
		if (IsClosedOrErrorState)
		{
			throw new InvalidOperationException(Res.GetString("The Writer is closed or in error state."));
		}
		try
		{
			await AdvanceStateAsync(Token.Text).ConfigureAwait(continueOnCapturedContext: false);
			await base.WriteBinHexAsync(buffer, index, count).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private async Task WriteStartDocumentImplAsync(XmlStandalone standalone)
	{
		_ = 2;
		try
		{
			await AdvanceStateAsync(Token.StartDocument).ConfigureAwait(continueOnCapturedContext: false);
			if (conformanceLevel == ConformanceLevel.Auto)
			{
				conformanceLevel = ConformanceLevel.Document;
				stateTable = StateTableDocument;
			}
			else if (conformanceLevel == ConformanceLevel.Fragment)
			{
				throw new InvalidOperationException(Res.GetString("WriteStartDocument cannot be called on writers created with ConformanceLevel.Fragment."));
			}
			if (rawWriter != null)
			{
				if (!xmlDeclFollows)
				{
					await rawWriter.WriteXmlDeclarationAsync(standalone).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			else
			{
				await writer.WriteStartDocumentAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch
		{
			currentState = State.Error;
			throw;
		}
	}

	private Task AdvanceStateAsync_ReturnWhenFinish(Task task, State newState)
	{
		if (task.IsSuccess())
		{
			currentState = newState;
			return AsyncHelper.DoneTask;
		}
		return _AdvanceStateAsync_ReturnWhenFinish(task, newState);
	}

	private async Task _AdvanceStateAsync_ReturnWhenFinish(Task task, State newState)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		currentState = newState;
	}

	private Task AdvanceStateAsync_ContinueWhenFinish(Task task, State newState, Token token)
	{
		if (task.IsSuccess())
		{
			currentState = newState;
			return AdvanceStateAsync(token);
		}
		return _AdvanceStateAsync_ContinueWhenFinish(task, newState, token);
	}

	private async Task _AdvanceStateAsync_ContinueWhenFinish(Task task, State newState, Token token)
	{
		await task.ConfigureAwait(continueOnCapturedContext: false);
		currentState = newState;
		await AdvanceStateAsync(token).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task AdvanceStateAsync(Token token)
	{
		if (currentState >= State.Closed)
		{
			if (currentState == State.Closed || currentState == State.Error)
			{
				throw new InvalidOperationException(Res.GetString("The Writer is closed or in error state."));
			}
			throw new InvalidOperationException(Res.GetString("Token {0} in state {1} would result in an invalid XML document.", tokenName[(int)token], GetStateName(currentState)));
		}
		State state;
		while (true)
		{
			state = stateTable[(int)(((int)token << 4) + currentState)];
			switch (state)
			{
			case State.Error:
				ThrowInvalidStateTransition(token, currentState);
				break;
			case State.StartContent:
				return AdvanceStateAsync_ReturnWhenFinish(StartElementContentAsync(), State.Content);
			case State.StartContentEle:
				return AdvanceStateAsync_ReturnWhenFinish(StartElementContentAsync(), State.Element);
			case State.StartContentB64:
				return AdvanceStateAsync_ReturnWhenFinish(StartElementContentAsync(), State.B64Content);
			case State.StartDoc:
				return AdvanceStateAsync_ReturnWhenFinish(WriteStartDocumentAsync(), State.Document);
			case State.StartDocEle:
				return AdvanceStateAsync_ReturnWhenFinish(WriteStartDocumentAsync(), State.Element);
			case State.EndAttrSEle:
			{
				Task task = SequenceRun(WriteEndAttributeAsync(), StartElementContentAsync);
				return AdvanceStateAsync_ReturnWhenFinish(task, State.Element);
			}
			case State.EndAttrEEle:
			{
				Task task = SequenceRun(WriteEndAttributeAsync(), StartElementContentAsync);
				return AdvanceStateAsync_ReturnWhenFinish(task, State.Content);
			}
			case State.EndAttrSCont:
			{
				Task task = SequenceRun(WriteEndAttributeAsync(), StartElementContentAsync);
				return AdvanceStateAsync_ReturnWhenFinish(task, State.Content);
			}
			case State.EndAttrSAttr:
				return AdvanceStateAsync_ReturnWhenFinish(WriteEndAttributeAsync(), State.Attribute);
			case State.PostB64Cont:
				if (rawWriter != null)
				{
					return AdvanceStateAsync_ContinueWhenFinish(rawWriter.WriteEndBase64Async(), State.Content, token);
				}
				currentState = State.Content;
				continue;
			case State.PostB64Attr:
				if (rawWriter != null)
				{
					return AdvanceStateAsync_ContinueWhenFinish(rawWriter.WriteEndBase64Async(), State.Attribute, token);
				}
				currentState = State.Attribute;
				continue;
			case State.PostB64RootAttr:
				if (rawWriter != null)
				{
					return AdvanceStateAsync_ContinueWhenFinish(rawWriter.WriteEndBase64Async(), State.RootLevelAttr, token);
				}
				currentState = State.RootLevelAttr;
				continue;
			case State.StartFragEle:
				StartFragment();
				state = State.Element;
				break;
			case State.StartFragCont:
				StartFragment();
				state = State.Content;
				break;
			case State.StartFragB64:
				StartFragment();
				state = State.B64Content;
				break;
			case State.StartRootLevelAttr:
				return AdvanceStateAsync_ReturnWhenFinish(WriteEndAttributeAsync(), State.RootLevelAttr);
			}
			break;
		}
		currentState = state;
		return AsyncHelper.DoneTask;
	}

	private async Task StartElementContentAsync_WithNS()
	{
		int start = elemScopeStack[elemTop].prevNSTop;
		for (int i = nsTop; i > start; i--)
		{
			if (nsStack[i].kind == NamespaceKind.NeedToWrite)
			{
				await nsStack[i].WriteDeclAsync(writer, rawWriter).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		if (rawWriter != null)
		{
			rawWriter.StartElementContent();
		}
	}

	private Task StartElementContentAsync()
	{
		if (nsTop > elemScopeStack[elemTop].prevNSTop)
		{
			return StartElementContentAsync_WithNS();
		}
		if (rawWriter != null)
		{
			rawWriter.StartElementContent();
		}
		return AsyncHelper.DoneTask;
	}
}
