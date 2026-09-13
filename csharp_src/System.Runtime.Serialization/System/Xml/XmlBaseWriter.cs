using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal abstract class XmlBaseWriter : XmlDictionaryWriter, IFragmentCapableXmlDictionaryWriter
{
	private class WriteBase64AsyncResult : AsyncResult
	{
		private static AsyncCompletion onComplete = OnComplete;

		private XmlBaseWriter writer;

		private byte[] buffer;

		private int offset;

		private int count;

		private int actualByteCount;

		private int totalByteCount;

		public WriteBase64AsyncResult(byte[] buffer, int offset, int count, XmlBaseWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.writer = writer;
			this.buffer = buffer;
			this.offset = offset;
			this.count = count;
			bool flag = true;
			if (this.count > 0)
			{
				if (writer.trailByteCount > 0)
				{
					while (writer.trailByteCount < 3 && this.count > 0)
					{
						writer.trailBytes[writer.trailByteCount++] = buffer[this.offset++];
						this.count--;
					}
				}
				totalByteCount = writer.trailByteCount + this.count;
				actualByteCount = totalByteCount - totalByteCount % 3;
				if (writer.trailBytes == null)
				{
					writer.trailBytes = new byte[3];
				}
				if (actualByteCount >= 3)
				{
					if (writer.attributeValue != null)
					{
						writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(writer.trailBytes, 0, writer.trailByteCount));
						writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(buffer, this.offset, actualByteCount - writer.trailByteCount));
					}
					flag = HandleWriteBase64Text(null);
				}
				else
				{
					Buffer.BlockCopy(buffer, this.offset, writer.trailBytes, writer.trailByteCount, this.count);
					writer.trailByteCount += this.count;
				}
			}
			if (flag)
			{
				Complete(completedSynchronously: true);
			}
		}

		private static bool OnComplete(IAsyncResult result)
		{
			return ((WriteBase64AsyncResult)result.AsyncState).HandleWriteBase64Text(result);
		}

		private bool HandleWriteBase64Text(IAsyncResult result)
		{
			if (!writer.isXmlnsAttribute)
			{
				if (result == null)
				{
					writer.StartContent();
					result = writer.writer.BeginWriteBase64Text(writer.trailBytes, writer.trailByteCount, buffer, offset, actualByteCount - writer.trailByteCount, PrepareAsyncCompletion(onComplete), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				writer.writer.EndWriteBase64Text(result);
				writer.EndContent();
			}
			writer.trailByteCount = totalByteCount - actualByteCount;
			if (writer.trailByteCount > 0)
			{
				int num = offset + count - writer.trailByteCount;
				for (int i = 0; i < writer.trailByteCount; i++)
				{
					writer.trailBytes[i] = buffer[num++];
				}
			}
			return true;
		}

		public static void End(IAsyncResult result)
		{
			AsyncResult.End<WriteBase64AsyncResult>(result);
		}
	}

	private class Element
	{
		private string prefix;

		private string localName;

		private int prefixId;

		public string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

		public string LocalName
		{
			get
			{
				return localName;
			}
			set
			{
				localName = value;
			}
		}

		public int PrefixId
		{
			get
			{
				return prefixId;
			}
			set
			{
				prefixId = value;
			}
		}

		public void Clear()
		{
			prefix = null;
			localName = null;
			prefixId = 0;
		}
	}

	private enum DocumentState : byte
	{
		None,
		Document,
		Epilog,
		End
	}

	private class NamespaceManager
	{
		private class XmlAttribute
		{
			private XmlSpace space;

			private string lang;

			private int depth;

			public int Depth
			{
				get
				{
					return depth;
				}
				set
				{
					depth = value;
				}
			}

			public string XmlLang
			{
				get
				{
					return lang;
				}
				set
				{
					lang = value;
				}
			}

			public XmlSpace XmlSpace
			{
				get
				{
					return space;
				}
				set
				{
					space = value;
				}
			}

			public void Clear()
			{
				lang = null;
			}
		}

		private class Namespace
		{
			private string prefix;

			private string ns;

			private XmlDictionaryString xNs;

			private int depth;

			private char prefixChar;

			public int Depth
			{
				get
				{
					return depth;
				}
				set
				{
					depth = value;
				}
			}

			public char PrefixChar => prefixChar;

			public string Prefix
			{
				get
				{
					return prefix;
				}
				set
				{
					if (value.Length == 1)
					{
						prefixChar = value[0];
					}
					else
					{
						prefixChar = '\0';
					}
					prefix = value;
				}
			}

			public string Uri
			{
				get
				{
					return ns;
				}
				set
				{
					ns = value;
				}
			}

			public XmlDictionaryString UriDictionaryString
			{
				get
				{
					return xNs;
				}
				set
				{
					xNs = value;
				}
			}

			public void Clear()
			{
				prefix = null;
				prefixChar = '\0';
				ns = null;
				xNs = null;
				depth = 0;
			}
		}

		private Namespace[] namespaces;

		private Namespace lastNameSpace;

		private int nsCount;

		private int depth;

		private XmlAttribute[] attributes;

		private int attributeCount;

		private XmlSpace space;

		private string lang;

		private int namespaceBoundary;

		private int nsTop;

		private Namespace defaultNamespace;

		public string XmlLang => lang;

		public XmlSpace XmlSpace => space;

		public int NamespaceBoundary
		{
			get
			{
				return namespaceBoundary;
			}
			set
			{
				int i;
				for (i = 0; i < nsCount && namespaces[i].Depth < value; i++)
				{
				}
				nsTop = i;
				namespaceBoundary = value;
				lastNameSpace = null;
			}
		}

		public NamespaceManager()
		{
			defaultNamespace = new Namespace();
			defaultNamespace.Depth = 0;
			defaultNamespace.Prefix = string.Empty;
			defaultNamespace.Uri = string.Empty;
			defaultNamespace.UriDictionaryString = null;
		}

		public void Clear()
		{
			if (namespaces == null)
			{
				namespaces = new Namespace[4];
				namespaces[0] = defaultNamespace;
			}
			nsCount = 1;
			nsTop = 0;
			depth = 0;
			attributeCount = 0;
			space = XmlSpace.None;
			lang = null;
			lastNameSpace = null;
			namespaceBoundary = 0;
		}

		public void Close()
		{
			if (depth == 0)
			{
				if (namespaces != null && namespaces.Length > 32)
				{
					namespaces = null;
				}
				if (attributes != null && attributes.Length > 4)
				{
					attributes = null;
				}
			}
			else
			{
				namespaces = null;
				attributes = null;
			}
			lang = null;
		}

		public void DeclareNamespaces(XmlNodeWriter writer)
		{
			int i = nsCount;
			while (i > 0 && namespaces[i - 1].Depth == depth)
			{
				i--;
			}
			for (; i < nsCount; i++)
			{
				Namespace obj = namespaces[i];
				if (obj.UriDictionaryString != null)
				{
					writer.WriteXmlnsAttribute(obj.Prefix, obj.UriDictionaryString);
				}
				else
				{
					writer.WriteXmlnsAttribute(obj.Prefix, obj.Uri);
				}
			}
		}

		public void EnterScope()
		{
			depth++;
		}

		public void ExitScope()
		{
			while (nsCount > 0)
			{
				Namespace obj = namespaces[nsCount - 1];
				if (obj.Depth != depth)
				{
					break;
				}
				if (lastNameSpace == obj)
				{
					lastNameSpace = null;
				}
				obj.Clear();
				nsCount--;
			}
			while (attributeCount > 0)
			{
				XmlAttribute xmlAttribute = attributes[attributeCount - 1];
				if (xmlAttribute.Depth != depth)
				{
					break;
				}
				space = xmlAttribute.XmlSpace;
				lang = xmlAttribute.XmlLang;
				xmlAttribute.Clear();
				attributeCount--;
			}
			depth--;
		}

		public void AddLangAttribute(string lang)
		{
			AddAttribute();
			this.lang = lang;
		}

		public void AddSpaceAttribute(XmlSpace space)
		{
			AddAttribute();
			this.space = space;
		}

		private void AddAttribute()
		{
			if (attributes == null)
			{
				attributes = new XmlAttribute[1];
			}
			else if (attributes.Length == attributeCount)
			{
				XmlAttribute[] destinationArray = new XmlAttribute[attributeCount * 2];
				Array.Copy(attributes, destinationArray, attributeCount);
				attributes = destinationArray;
			}
			XmlAttribute xmlAttribute = attributes[attributeCount];
			if (xmlAttribute == null)
			{
				xmlAttribute = new XmlAttribute();
				attributes[attributeCount] = xmlAttribute;
			}
			xmlAttribute.XmlLang = lang;
			xmlAttribute.XmlSpace = space;
			xmlAttribute.Depth = depth;
			attributeCount++;
		}

		public string AddNamespace(string uri, XmlDictionaryString uriDictionaryString)
		{
			if (uri.Length == 0)
			{
				AddNamespaceIfNotDeclared(string.Empty, uri, uriDictionaryString);
				return string.Empty;
			}
			for (int i = 0; i < prefixes.Length; i++)
			{
				string text = prefixes[i];
				bool flag = false;
				for (int num = nsCount - 1; num >= nsTop; num--)
				{
					if (namespaces[num].Prefix == text)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					AddNamespace(text, uri, uriDictionaryString);
					return text;
				}
			}
			return null;
		}

		public void AddNamespaceIfNotDeclared(string prefix, string uri, XmlDictionaryString uriDictionaryString)
		{
			if (LookupNamespace(prefix) != uri)
			{
				AddNamespace(prefix, uri, uriDictionaryString);
			}
		}

		public void AddNamespace(string prefix, string uri, XmlDictionaryString uriDictionaryString)
		{
			if (prefix.Length >= 3 && (prefix[0] & -33) == 88 && (prefix[1] & -33) == 77 && (prefix[2] & -33) == 76)
			{
				if ((!(prefix == "xml") || !(uri == "http://www.w3.org/XML/1998/namespace")) && (!(prefix == "xmlns") || !(uri == "http://www.w3.org/2000/xmlns/")))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Prefixes beginning with \"xml\" (regardless of casing) are reserved for use by XML."), "prefix"));
				}
				return;
			}
			Namespace obj;
			for (int num = nsCount - 1; num >= 0; num--)
			{
				obj = namespaces[num];
				if (obj.Depth != depth)
				{
					break;
				}
				if (obj.Prefix == prefix)
				{
					if (obj.Uri == uri)
					{
						return;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", prefix, obj.Uri, uri), "prefix"));
				}
			}
			if (prefix.Length != 0 && uri.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The empty namespace requires a null or empty prefix."), "prefix"));
			}
			if (uri.Length == "http://www.w3.org/2000/xmlns/".Length && uri == "http://www.w3.org/2000/xmlns/")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", "xmlns", uri)));
			}
			if (uri.Length == "http://www.w3.org/XML/1998/namespace".Length && uri[18] == 'X' && uri == "http://www.w3.org/XML/1998/namespace")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", "xml", uri)));
			}
			if (namespaces.Length == nsCount)
			{
				Namespace[] destinationArray = new Namespace[nsCount * 2];
				Array.Copy(namespaces, destinationArray, nsCount);
				namespaces = destinationArray;
			}
			obj = namespaces[nsCount];
			if (obj == null)
			{
				obj = new Namespace();
				namespaces[nsCount] = obj;
			}
			obj.Depth = depth;
			obj.Prefix = prefix;
			obj.Uri = uri;
			obj.UriDictionaryString = uriDictionaryString;
			nsCount++;
			lastNameSpace = null;
		}

		public string LookupPrefix(string ns)
		{
			if (lastNameSpace != null && lastNameSpace.Uri == ns)
			{
				return lastNameSpace.Prefix;
			}
			int num = nsCount;
			for (int num2 = num - 1; num2 >= nsTop; num2--)
			{
				Namespace obj = namespaces[num2];
				if ((object)obj.Uri == ns)
				{
					string prefix = obj.Prefix;
					bool flag = false;
					for (int i = num2 + 1; i < num; i++)
					{
						if (namespaces[i].Prefix == prefix)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						lastNameSpace = obj;
						return prefix;
					}
				}
			}
			for (int num3 = num - 1; num3 >= nsTop; num3--)
			{
				Namespace obj2 = namespaces[num3];
				if (obj2.Uri == ns)
				{
					string prefix2 = obj2.Prefix;
					bool flag2 = false;
					for (int j = num3 + 1; j < num; j++)
					{
						if (namespaces[j].Prefix == prefix2)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						lastNameSpace = obj2;
						return prefix2;
					}
				}
			}
			if (ns.Length == 0)
			{
				bool flag3 = true;
				for (int num4 = num - 1; num4 >= nsTop; num4--)
				{
					if (namespaces[num4].Prefix.Length == 0)
					{
						flag3 = false;
						break;
					}
				}
				if (flag3)
				{
					return string.Empty;
				}
			}
			if (ns == "http://www.w3.org/2000/xmlns/")
			{
				return "xmlns";
			}
			if (ns == "http://www.w3.org/XML/1998/namespace")
			{
				return "xml";
			}
			return null;
		}

		public string LookupAttributePrefix(string ns)
		{
			if (lastNameSpace != null && lastNameSpace.Uri == ns && lastNameSpace.Prefix.Length != 0)
			{
				return lastNameSpace.Prefix;
			}
			int num = nsCount;
			for (int num2 = num - 1; num2 >= nsTop; num2--)
			{
				Namespace obj = namespaces[num2];
				if ((object)obj.Uri == ns)
				{
					string prefix = obj.Prefix;
					if (prefix.Length != 0)
					{
						bool flag = false;
						for (int i = num2 + 1; i < num; i++)
						{
							if (namespaces[i].Prefix == prefix)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							lastNameSpace = obj;
							return prefix;
						}
					}
				}
			}
			for (int num3 = num - 1; num3 >= nsTop; num3--)
			{
				Namespace obj2 = namespaces[num3];
				if (obj2.Uri == ns)
				{
					string prefix2 = obj2.Prefix;
					if (prefix2.Length != 0)
					{
						bool flag2 = false;
						for (int j = num3 + 1; j < num; j++)
						{
							if (namespaces[j].Prefix == prefix2)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							lastNameSpace = obj2;
							return prefix2;
						}
					}
				}
			}
			if (ns.Length == 0)
			{
				return string.Empty;
			}
			return null;
		}

		public string LookupNamespace(string prefix)
		{
			int num = nsCount;
			if (prefix.Length == 0)
			{
				for (int num2 = num - 1; num2 >= nsTop; num2--)
				{
					Namespace obj = namespaces[num2];
					if (obj.Prefix.Length == 0)
					{
						return obj.Uri;
					}
				}
				return string.Empty;
			}
			if (prefix.Length == 1)
			{
				char c = prefix[0];
				for (int num3 = num - 1; num3 >= nsTop; num3--)
				{
					Namespace obj2 = namespaces[num3];
					if (obj2.PrefixChar == c)
					{
						return obj2.Uri;
					}
				}
				return null;
			}
			for (int num4 = num - 1; num4 >= nsTop; num4--)
			{
				Namespace obj3 = namespaces[num4];
				if (obj3.Prefix == prefix)
				{
					return obj3.Uri;
				}
			}
			if (prefix == "xmlns")
			{
				return "http://www.w3.org/2000/xmlns/";
			}
			if (prefix == "xml")
			{
				return "http://www.w3.org/XML/1998/namespace";
			}
			return null;
		}

		public void Sign(XmlCanonicalWriter signingWriter)
		{
			int num = nsCount;
			for (int i = 1; i < num; i++)
			{
				Namespace obj = namespaces[i];
				bool flag = false;
				for (int j = i + 1; j < num; j++)
				{
					if (flag)
					{
						break;
					}
					flag = obj.Prefix == namespaces[j].Prefix;
				}
				if (!flag)
				{
					signingWriter.WriteXmlnsAttribute(obj.Prefix, obj.Uri);
				}
			}
		}
	}

	private class XmlBaseWriterNodeWriterAsyncHelper
	{
		private static AsyncEventArgsCallback onWriteComplete;

		private XmlBaseWriter writer;

		private byte[] buffer;

		private int offset;

		private int count;

		private int actualByteCount;

		private int totalByteCount;

		private AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> nodeWriterAsyncState;

		private XmlNodeWriterWriteBase64TextArgs nodeWriterArgs;

		private AsyncEventArgs<XmlWriteBase64AsyncArguments> inputState;

		public XmlBaseWriterNodeWriterAsyncHelper(XmlBaseWriter writer)
		{
			this.writer = writer;
		}

		public void SetArguments(AsyncEventArgs<XmlWriteBase64AsyncArguments> inputState)
		{
			this.inputState = inputState;
			buffer = inputState.Arguments.Buffer;
			offset = inputState.Arguments.Offset;
			count = inputState.Arguments.Count;
		}

		public AsyncCompletionResult StartAsync()
		{
			bool flag = true;
			if (count > 0)
			{
				if (writer.trailByteCount > 0)
				{
					while (writer.trailByteCount < 3 && count > 0)
					{
						writer.trailBytes[writer.trailByteCount++] = buffer[offset++];
						count--;
					}
				}
				totalByteCount = writer.trailByteCount + count;
				actualByteCount = totalByteCount - totalByteCount % 3;
				if (writer.trailBytes == null)
				{
					writer.trailBytes = new byte[3];
				}
				if (actualByteCount >= 3)
				{
					if (writer.attributeValue != null)
					{
						writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(writer.trailBytes, 0, writer.trailByteCount));
						writer.WriteAttributeText(XmlConverter.Base64Encoding.GetString(buffer, offset, actualByteCount - writer.trailByteCount));
					}
					flag = HandleWriteBase64Text(isAsyncCallback: false);
				}
				else
				{
					Buffer.BlockCopy(buffer, offset, writer.trailBytes, writer.trailByteCount, count);
					writer.trailByteCount += count;
				}
			}
			if (flag)
			{
				Clear();
				return AsyncCompletionResult.Completed;
			}
			return AsyncCompletionResult.Queued;
		}

		private static void OnWriteComplete(IAsyncEventArgs asyncEventArgs)
		{
			bool flag = false;
			Exception exception = null;
			XmlBaseWriterNodeWriterAsyncHelper xmlBaseWriterNodeWriterAsyncHelper = (XmlBaseWriterNodeWriterAsyncHelper)asyncEventArgs.AsyncState;
			AsyncEventArgs<XmlWriteBase64AsyncArguments> e = xmlBaseWriterNodeWriterAsyncHelper.inputState;
			try
			{
				if (asyncEventArgs.Exception != null)
				{
					exception = asyncEventArgs.Exception;
					flag = true;
				}
				else
				{
					flag = xmlBaseWriterNodeWriterAsyncHelper.HandleWriteBase64Text(isAsyncCallback: true);
				}
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				exception = ex;
				flag = true;
			}
			if (flag)
			{
				xmlBaseWriterNodeWriterAsyncHelper.Clear();
				e.Complete(completedSynchronously: false, exception);
			}
		}

		private bool HandleWriteBase64Text(bool isAsyncCallback)
		{
			if (!writer.isXmlnsAttribute)
			{
				if (!isAsyncCallback)
				{
					if (nodeWriterAsyncState == null)
					{
						nodeWriterAsyncState = new AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs>();
						nodeWriterArgs = new XmlNodeWriterWriteBase64TextArgs();
					}
					if (onWriteComplete == null)
					{
						onWriteComplete = OnWriteComplete;
					}
					writer.StartContent();
					nodeWriterArgs.TrailBuffer = writer.trailBytes;
					nodeWriterArgs.TrailCount = writer.trailByteCount;
					nodeWriterArgs.Buffer = buffer;
					nodeWriterArgs.Offset = offset;
					nodeWriterArgs.Count = actualByteCount - writer.trailByteCount;
					nodeWriterAsyncState.Set(onWriteComplete, nodeWriterArgs, this);
					if (writer.writer.WriteBase64TextAsync(nodeWriterAsyncState) != AsyncCompletionResult.Completed)
					{
						return false;
					}
					nodeWriterAsyncState.Complete(completedSynchronously: true);
				}
				writer.EndContent();
			}
			writer.trailByteCount = totalByteCount - actualByteCount;
			if (writer.trailByteCount > 0)
			{
				int num = offset + count - writer.trailByteCount;
				for (int i = 0; i < writer.trailByteCount; i++)
				{
					writer.trailBytes[i] = buffer[num++];
				}
			}
			return true;
		}

		private void Clear()
		{
			inputState = null;
			buffer = null;
			offset = 0;
			count = 0;
			actualByteCount = 0;
			totalByteCount = 0;
		}
	}

	private XmlNodeWriter writer;

	private NamespaceManager nsMgr;

	private Element[] elements;

	private int depth;

	private string attributeLocalName;

	private string attributeValue;

	private bool isXmlAttribute;

	private bool isXmlnsAttribute;

	private WriteState writeState;

	private DocumentState documentState;

	private byte[] trailBytes;

	private int trailByteCount;

	private XmlStreamNodeWriter nodeWriter;

	private XmlSigningNodeWriter signingWriter;

	private XmlUTF8NodeWriter textFragmentWriter;

	private XmlNodeWriter oldWriter;

	private Stream oldStream;

	private int oldNamespaceBoundary;

	private bool inList;

	private const string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";

	private const string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

	private static BinHexEncoding binhexEncoding;

	private static string[] prefixes = new string[26]
	{
		"a", "b", "c", "d", "e", "f", "g", "h", "i", "j",
		"k", "l", "m", "n", "o", "p", "q", "r", "s", "t",
		"u", "v", "w", "x", "y", "z"
	};

	private XmlBaseWriterNodeWriterAsyncHelper nodeWriterAsyncHelper;

	protected bool IsClosed => writeState == WriteState.Closed;

	private static BinHexEncoding BinHexEncoding
	{
		get
		{
			if (binhexEncoding == null)
			{
				binhexEncoding = new BinHexEncoding();
			}
			return binhexEncoding;
		}
	}

	public override string XmlLang => nsMgr.XmlLang;

	public override XmlSpace XmlSpace => nsMgr.XmlSpace;

	public override WriteState WriteState => writeState;

	protected int NamespaceBoundary
	{
		get
		{
			return nsMgr.NamespaceBoundary;
		}
		set
		{
			nsMgr.NamespaceBoundary = value;
		}
	}

	public override bool CanCanonicalize => true;

	protected bool Signing => writer == signingWriter;

	public virtual bool CanFragment => true;

	protected XmlBaseWriter()
	{
		nsMgr = new NamespaceManager();
		writeState = WriteState.Start;
		documentState = DocumentState.None;
	}

	protected void SetOutput(XmlStreamNodeWriter writer)
	{
		inList = false;
		this.writer = writer;
		nodeWriter = writer;
		writeState = WriteState.Start;
		documentState = DocumentState.None;
		nsMgr.Clear();
		if (depth != 0)
		{
			elements = null;
			depth = 0;
		}
		attributeLocalName = null;
		attributeValue = null;
		oldWriter = null;
		oldStream = null;
	}

	public override void Flush()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		writer.Flush();
	}

	public override void Close()
	{
		if (IsClosed)
		{
			return;
		}
		try
		{
			FinishDocument();
			AutoComplete(WriteState.Closed);
			writer.Flush();
		}
		finally
		{
			nsMgr.Close();
			if (depth != 0)
			{
				elements = null;
				depth = 0;
			}
			attributeValue = null;
			attributeLocalName = null;
			nodeWriter.Close();
			if (signingWriter != null)
			{
				signingWriter.Close();
			}
			if (textFragmentWriter != null)
			{
				textFragmentWriter.Close();
			}
			oldWriter = null;
			oldStream = null;
		}
	}

	protected void ThrowClosed()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The XmlWriter is closed.")));
	}

	public override void WriteXmlnsAttribute(string prefix, string ns)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (ns == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
		}
		if (writeState != WriteState.Element)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteXmlnsAttribute", WriteState.ToString())));
		}
		if (prefix == null)
		{
			prefix = nsMgr.LookupPrefix(ns);
			if (prefix == null)
			{
				GeneratePrefix(ns, null);
			}
		}
		else
		{
			nsMgr.AddNamespaceIfNotDeclared(prefix, ns, null);
		}
	}

	public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (ns == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("ns");
		}
		if (writeState != WriteState.Element)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteXmlnsAttribute", WriteState.ToString())));
		}
		if (prefix == null)
		{
			prefix = nsMgr.LookupPrefix(ns.Value);
			if (prefix == null)
			{
				GeneratePrefix(ns.Value, ns);
			}
		}
		else
		{
			nsMgr.AddNamespaceIfNotDeclared(prefix, ns.Value, ns);
		}
	}

	private void StartAttribute(ref string prefix, string localName, string ns, XmlDictionaryString xNs)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState == WriteState.Attribute)
		{
			WriteEndAttribute();
		}
		if (localName == null || (localName.Length == 0 && prefix != "xmlns"))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (writeState != WriteState.Element)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteStartAttribute", WriteState.ToString())));
		}
		if (prefix == null)
		{
			if (ns == "http://www.w3.org/2000/xmlns/" && localName != "xmlns")
			{
				prefix = "xmlns";
			}
			else if (ns == "http://www.w3.org/XML/1998/namespace")
			{
				prefix = "xml";
			}
			else
			{
				prefix = string.Empty;
			}
		}
		if (prefix.Length == 0 && localName == "xmlns")
		{
			prefix = "xmlns";
			localName = string.Empty;
		}
		isXmlnsAttribute = false;
		isXmlAttribute = false;
		if (prefix == "xml")
		{
			if (ns != null && ns != "http://www.w3.org/XML/1998/namespace")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", "xml", "http://www.w3.org/XML/1998/namespace", ns), "ns"));
			}
			isXmlAttribute = true;
			attributeValue = string.Empty;
			attributeLocalName = localName;
		}
		else if (prefix == "xmlns")
		{
			if (ns != null && ns != "http://www.w3.org/2000/xmlns/")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.", "xmlns", "http://www.w3.org/2000/xmlns/", ns), "ns"));
			}
			isXmlnsAttribute = true;
			attributeValue = string.Empty;
			attributeLocalName = localName;
		}
		else if (ns == null)
		{
			if (prefix.Length == 0)
			{
				ns = string.Empty;
			}
			else
			{
				ns = nsMgr.LookupNamespace(prefix);
				if (ns == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is not defined.", prefix), "prefix"));
				}
			}
		}
		else if (ns.Length == 0)
		{
			if (prefix.Length != 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The empty namespace requires a null or empty prefix."), "prefix"));
			}
		}
		else if (prefix.Length == 0)
		{
			prefix = nsMgr.LookupAttributePrefix(ns);
			if (prefix == null)
			{
				if (ns.Length == "http://www.w3.org/2000/xmlns/".Length && ns == "http://www.w3.org/2000/xmlns/")
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", "xmlns", ns)));
				}
				if (ns.Length == "http://www.w3.org/XML/1998/namespace".Length && ns == "http://www.w3.org/XML/1998/namespace")
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The namespace '{1}' can only be bound to the prefix '{0}'.", "xml", ns)));
				}
				prefix = GeneratePrefix(ns, xNs);
			}
		}
		else
		{
			nsMgr.AddNamespaceIfNotDeclared(prefix, ns, xNs);
		}
		writeState = WriteState.Attribute;
	}

	public override void WriteStartAttribute(string prefix, string localName, string namespaceUri)
	{
		StartAttribute(ref prefix, localName, namespaceUri, null);
		if (!isXmlnsAttribute)
		{
			writer.WriteStartAttribute(prefix, localName);
		}
	}

	public override void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		StartAttribute(ref prefix, localName?.Value, namespaceUri?.Value, namespaceUri);
		if (!isXmlnsAttribute)
		{
			writer.WriteStartAttribute(prefix, localName);
		}
	}

	public override void WriteEndAttribute()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState != WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteEndAttribute", WriteState.ToString())));
		}
		FlushBase64();
		try
		{
			if (isXmlAttribute)
			{
				if (attributeLocalName == "lang")
				{
					nsMgr.AddLangAttribute(attributeValue);
				}
				else if (attributeLocalName == "space")
				{
					if (attributeValue == "preserve")
					{
						nsMgr.AddSpaceAttribute(XmlSpace.Preserve);
					}
					else
					{
						if (!(attributeValue == "default"))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("'{0}' is not a valid xml:space value. Valid values are 'default' and 'preserve'.", attributeValue)));
						}
						nsMgr.AddSpaceAttribute(XmlSpace.Default);
					}
				}
				isXmlAttribute = false;
				attributeLocalName = null;
				attributeValue = null;
			}
			if (isXmlnsAttribute)
			{
				nsMgr.AddNamespaceIfNotDeclared(attributeLocalName, attributeValue, null);
				isXmlnsAttribute = false;
				attributeLocalName = null;
				attributeValue = null;
			}
			else
			{
				writer.WriteEndAttribute();
			}
		}
		finally
		{
			writeState = WriteState.Element;
		}
	}

	public override void WriteComment(string text)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteComment", WriteState.ToString())));
		}
		if (text == null)
		{
			text = string.Empty;
		}
		else if (text.IndexOf("--", StringComparison.Ordinal) != -1 || (text.Length > 0 && text[text.Length - 1] == '-'))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("XML comments cannot contain '--' or end with '-'."), "text"));
		}
		StartComment();
		FlushBase64();
		writer.WriteComment(text);
		EndComment();
	}

	public override void WriteFullEndElement()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState == WriteState.Attribute)
		{
			WriteEndAttribute();
		}
		if (writeState != WriteState.Element && writeState != WriteState.Content)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteFullEndElement", WriteState.ToString())));
		}
		AutoComplete(WriteState.Content);
		WriteEndElement();
	}

	public override void WriteCData(string text)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteCData", WriteState.ToString())));
		}
		if (text == null)
		{
			text = string.Empty;
		}
		if (text.Length > 0)
		{
			StartContent();
			FlushBase64();
			writer.WriteCData(text);
			EndContent();
		}
	}

	public override void WriteDocType(string name, string pubid, string sysid, string subset)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("This XmlWriter implementation does not support the '{0}' method.", "WriteDocType")));
	}

	private void StartElement(ref string prefix, string localName, string ns, XmlDictionaryString xNs)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (documentState == DocumentState.Epilog)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Only one root element is permitted per document.")));
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (localName.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The empty string is not a valid local name."), "localName"));
		}
		if (writeState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteStartElement", WriteState.ToString())));
		}
		FlushBase64();
		AutoComplete(WriteState.Element);
		Element element = EnterScope();
		if (ns == null)
		{
			if (prefix == null)
			{
				prefix = string.Empty;
			}
			ns = nsMgr.LookupNamespace(prefix);
			if (ns == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The prefix '{0}' is not defined.", prefix), "prefix"));
			}
		}
		else if (prefix == null)
		{
			prefix = nsMgr.LookupPrefix(ns);
			if (prefix == null)
			{
				prefix = string.Empty;
				nsMgr.AddNamespace(string.Empty, ns, xNs);
			}
		}
		else
		{
			nsMgr.AddNamespaceIfNotDeclared(prefix, ns, xNs);
		}
		element.Prefix = prefix;
		element.LocalName = localName;
	}

	public override void WriteStartElement(string prefix, string localName, string namespaceUri)
	{
		StartElement(ref prefix, localName, namespaceUri, null);
		writer.WriteStartElement(prefix, localName);
	}

	public override void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		StartElement(ref prefix, localName?.Value, namespaceUri?.Value, namespaceUri);
		writer.WriteStartElement(prefix, localName);
	}

	public override void WriteEndElement()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Cannot call '{0}' while Depth is '{1}'.", "WriteEndElement", depth.ToString(CultureInfo.InvariantCulture))));
		}
		if (writeState == WriteState.Attribute)
		{
			WriteEndAttribute();
		}
		FlushBase64();
		if (writeState == WriteState.Element)
		{
			nsMgr.DeclareNamespaces(writer);
			writer.WriteEndStartElement(isEmpty: true);
		}
		else
		{
			Element element = elements[depth];
			writer.WriteEndElement(element.Prefix, element.LocalName);
		}
		ExitScope();
		writeState = WriteState.Content;
	}

	private Element EnterScope()
	{
		nsMgr.EnterScope();
		depth++;
		if (elements == null)
		{
			elements = new Element[4];
		}
		else if (elements.Length == depth)
		{
			Element[] destinationArray = new Element[depth * 2];
			Array.Copy(elements, destinationArray, depth);
			elements = destinationArray;
		}
		Element element = elements[depth];
		if (element == null)
		{
			element = new Element();
			elements[depth] = element;
		}
		return element;
	}

	private void ExitScope()
	{
		elements[depth].Clear();
		depth--;
		if (depth == 0 && documentState == DocumentState.Document)
		{
			documentState = DocumentState.Epilog;
		}
		nsMgr.ExitScope();
	}

	protected void FlushElement()
	{
		if (writeState == WriteState.Element)
		{
			AutoComplete(WriteState.Content);
		}
	}

	protected void StartComment()
	{
		FlushElement();
	}

	protected void EndComment()
	{
	}

	protected void StartContent()
	{
		FlushElement();
		if (depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Text cannot be written outside the root element.")));
		}
	}

	protected void StartContent(char ch)
	{
		FlushElement();
		if (depth == 0)
		{
			VerifyWhitespace(ch);
		}
	}

	protected void StartContent(string s)
	{
		FlushElement();
		if (depth == 0)
		{
			VerifyWhitespace(s);
		}
	}

	protected void StartContent(char[] chars, int offset, int count)
	{
		FlushElement();
		if (depth == 0)
		{
			VerifyWhitespace(chars, offset, count);
		}
	}

	private void VerifyWhitespace(char ch)
	{
		if (!IsWhitespace(ch))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Text cannot be written outside the root element.")));
		}
	}

	private void VerifyWhitespace(string s)
	{
		for (int i = 0; i < s.Length; i++)
		{
			if (!IsWhitespace(s[i]))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Text cannot be written outside the root element.")));
			}
		}
	}

	private void VerifyWhitespace(char[] chars, int offset, int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (!IsWhitespace(chars[offset + i]))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Text cannot be written outside the root element.")));
			}
		}
	}

	private bool IsWhitespace(char ch)
	{
		if (ch != ' ' && ch != '\n' && ch != '\r')
		{
			return ch == 't';
		}
		return true;
	}

	protected void EndContent()
	{
	}

	private void AutoComplete(WriteState writeState)
	{
		if (this.writeState == WriteState.Element)
		{
			EndStartElement();
		}
		this.writeState = writeState;
	}

	private void EndStartElement()
	{
		nsMgr.DeclareNamespaces(writer);
		writer.WriteEndStartElement(isEmpty: false);
	}

	public override string LookupPrefix(string ns)
	{
		if (ns == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("ns"));
		}
		if (IsClosed)
		{
			ThrowClosed();
		}
		return nsMgr.LookupPrefix(ns);
	}

	internal string LookupNamespace(string prefix)
	{
		if (prefix == null)
		{
			return null;
		}
		return nsMgr.LookupNamespace(prefix);
	}

	private string GetQualifiedNamePrefix(string namespaceUri, XmlDictionaryString xNs)
	{
		string text = nsMgr.LookupPrefix(namespaceUri);
		if (text == null)
		{
			if (writeState != WriteState.Attribute)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The namespace '{0}' is not defined.", namespaceUri), "namespaceUri"));
			}
			text = GeneratePrefix(namespaceUri, xNs);
		}
		return text;
	}

	public override void WriteQualifiedName(string localName, string namespaceUri)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (localName.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The empty string is not a valid local name."), "localName"));
		}
		if (namespaceUri == null)
		{
			namespaceUri = string.Empty;
		}
		string qualifiedNamePrefix = GetQualifiedNamePrefix(namespaceUri, null);
		if (qualifiedNamePrefix.Length != 0)
		{
			WriteString(qualifiedNamePrefix);
			WriteString(":");
		}
		WriteString(localName);
	}

	public override void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (localName.Value.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The empty string is not a valid local name."), "localName"));
		}
		if (namespaceUri == null)
		{
			namespaceUri = XmlDictionaryString.Empty;
		}
		string qualifiedNamePrefix = GetQualifiedNamePrefix(namespaceUri.Value, namespaceUri);
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(qualifiedNamePrefix + ":" + namespaceUri.Value);
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteQualifiedName(qualifiedNamePrefix, localName);
			EndContent();
		}
	}

	public override void WriteStartDocument()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState != WriteState.Start)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteStartDocument", WriteState.ToString())));
		}
		writeState = WriteState.Prolog;
		documentState = DocumentState.Document;
		writer.WriteDeclaration();
	}

	public override void WriteStartDocument(bool standalone)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		WriteStartDocument();
	}

	public override void WriteProcessingInstruction(string name, string text)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (name != "xml")
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Processing instructions (other than the XML declaration) and DTDs are not supported."), "name"));
		}
		if (writeState != WriteState.Start)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("XML declaration can only be written at the beginning of the document.")));
		}
		writer.WriteDeclaration();
	}

	private void FinishDocument()
	{
		if (writeState == WriteState.Attribute)
		{
			WriteEndAttribute();
		}
		while (depth > 0)
		{
			WriteEndElement();
		}
	}

	public override void WriteEndDocument()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (writeState == WriteState.Start || writeState == WriteState.Prolog)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The document does not have a root element.")));
		}
		FinishDocument();
		writeState = WriteState.Start;
		documentState = DocumentState.End;
	}

	public override void WriteEntityRef(string name)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("This XmlWriter implementation does not support the '{0}' method.", "WriteEntityRef")));
	}

	public override void WriteName(string name)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		WriteString(name);
	}

	public override void WriteNmToken(string name)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("This XmlWriter implementation does not support the '{0}' method.", "WriteNmToken")));
	}

	public override void WriteWhitespace(string whitespace)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (whitespace == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("whitespace");
		}
		foreach (char c in whitespace)
		{
			if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Only white space characters can be written with this method."), "whitespace"));
			}
		}
		WriteString(whitespace);
	}

	public override void WriteString(string value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			value = string.Empty;
		}
		if (value.Length > 0 || inList)
		{
			FlushBase64();
			if (attributeValue != null)
			{
				WriteAttributeText(value);
			}
			if (!isXmlnsAttribute)
			{
				StartContent(value);
				writer.WriteEscapedText(value);
				EndContent();
			}
		}
	}

	public override void WriteString(XmlDictionaryString value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		if (value.Value.Length > 0)
		{
			FlushBase64();
			if (attributeValue != null)
			{
				WriteAttributeText(value.Value);
			}
			if (!isXmlnsAttribute)
			{
				StartContent(value.Value);
				writer.WriteEscapedText(value);
				EndContent();
			}
		}
	}

	public override void WriteChars(char[] chars, int offset, int count)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		if (count > 0)
		{
			FlushBase64();
			if (attributeValue != null)
			{
				WriteAttributeText(new string(chars, offset, count));
			}
			if (!isXmlnsAttribute)
			{
				StartContent(chars, offset, count);
				writer.WriteEscapedText(chars, offset, count);
				EndContent();
			}
		}
	}

	public override void WriteRaw(string value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			value = string.Empty;
		}
		if (value.Length > 0)
		{
			FlushBase64();
			if (attributeValue != null)
			{
				WriteAttributeText(value);
			}
			if (!isXmlnsAttribute)
			{
				StartContent(value);
				writer.WriteText(value);
				EndContent();
			}
		}
	}

	public override void WriteRaw(char[] chars, int offset, int count)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (chars == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > chars.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", chars.Length - offset)));
		}
		if (count > 0)
		{
			FlushBase64();
			if (attributeValue != null)
			{
				WriteAttributeText(new string(chars, offset, count));
			}
			if (!isXmlnsAttribute)
			{
				StartContent(chars, offset, count);
				writer.WriteText(chars, offset, count);
				EndContent();
			}
		}
	}

	public override void WriteCharEntity(char ch)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (ch >= '\ud800' && ch <= '\udfff')
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("The surrogate pair is invalid. Missing a low surrogate character."), "ch"));
		}
		if (attributeValue != null)
		{
			WriteAttributeText(ch.ToString());
		}
		if (!isXmlnsAttribute)
		{
			StartContent(ch);
			FlushBase64();
			writer.WriteCharEntity(ch);
			EndContent();
		}
	}

	public override void WriteSurrogateCharEntity(char lowChar, char highChar)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		SurrogateChar surrogateChar = new SurrogateChar(lowChar, highChar);
		if (attributeValue != null)
		{
			char[] value = new char[2] { highChar, lowChar };
			WriteAttributeText(new string(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			FlushBase64();
			writer.WriteCharEntity(surrogateChar.Char);
			EndContent();
		}
	}

	public override void WriteValue(object value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (value is object[])
		{
			WriteValue((object[])value);
		}
		else if (value is Array)
		{
			WriteValue((Array)value);
		}
		else if (value is IStreamProvider)
		{
			WriteValue((IStreamProvider)value);
		}
		else
		{
			WritePrimitiveValue(value);
		}
	}

	protected void WritePrimitiveValue(object value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (value is ulong)
		{
			WriteValue((ulong)value);
			return;
		}
		if (value is string)
		{
			WriteValue((string)value);
			return;
		}
		if (value is int)
		{
			WriteValue((int)value);
			return;
		}
		if (value is long)
		{
			WriteValue((long)value);
			return;
		}
		if (value is bool)
		{
			WriteValue((bool)value);
			return;
		}
		if (value is double)
		{
			WriteValue((double)value);
			return;
		}
		if (value is DateTime)
		{
			WriteValue((DateTime)value);
			return;
		}
		if (value is float)
		{
			WriteValue((float)value);
			return;
		}
		if (value is decimal)
		{
			WriteValue((decimal)value);
			return;
		}
		if (value is XmlDictionaryString)
		{
			WriteValue((XmlDictionaryString)value);
			return;
		}
		if (value is UniqueId)
		{
			WriteValue((UniqueId)value);
			return;
		}
		if (value is Guid)
		{
			WriteValue((Guid)value);
			return;
		}
		if (value is TimeSpan)
		{
			WriteValue((TimeSpan)value);
			return;
		}
		if (value.GetType().IsArray)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Nested arrays are not supported."), "value"));
		}
		base.WriteValue(value);
	}

	public override void WriteValue(string value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		WriteString(value);
	}

	public override void WriteValue(int value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteInt32Text(value);
			EndContent();
		}
	}

	public override void WriteValue(long value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteInt64Text(value);
			EndContent();
		}
	}

	private void WriteValue(ulong value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteUInt64Text(value);
			EndContent();
		}
	}

	public override void WriteValue(bool value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteBoolText(value);
			EndContent();
		}
	}

	public override void WriteValue(decimal value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteDecimalText(value);
			EndContent();
		}
	}

	public override void WriteValue(float value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteFloatText(value);
			EndContent();
		}
	}

	public override void WriteValue(double value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteDoubleText(value);
			EndContent();
		}
	}

	public override void WriteValue(XmlDictionaryString value)
	{
		WriteString(value);
	}

	public override void WriteValue(DateTime value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteDateTimeText(value);
			EndContent();
		}
	}

	public override void WriteValue(UniqueId value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteUniqueIdText(value);
			EndContent();
		}
	}

	public override void WriteValue(Guid value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteGuidText(value);
			EndContent();
		}
	}

	public override void WriteValue(TimeSpan value)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		FlushBase64();
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.ToString(value));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteTimeSpanText(value);
			EndContent();
		}
	}

	public override void WriteBase64(byte[] buffer, int offset, int count)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		EnsureBufferBounds(buffer, offset, count);
		if (count <= 0)
		{
			return;
		}
		if (trailByteCount > 0)
		{
			while (trailByteCount < 3 && count > 0)
			{
				trailBytes[trailByteCount++] = buffer[offset++];
				count--;
			}
		}
		int num = trailByteCount + count;
		int num2 = num - num % 3;
		if (trailBytes == null)
		{
			trailBytes = new byte[3];
		}
		if (num2 >= 3)
		{
			if (attributeValue != null)
			{
				WriteAttributeText(XmlConverter.Base64Encoding.GetString(trailBytes, 0, trailByteCount));
				WriteAttributeText(XmlConverter.Base64Encoding.GetString(buffer, offset, num2 - trailByteCount));
			}
			if (!isXmlnsAttribute)
			{
				StartContent();
				writer.WriteBase64Text(trailBytes, trailByteCount, buffer, offset, num2 - trailByteCount);
				EndContent();
			}
			trailByteCount = num - num2;
			if (trailByteCount > 0)
			{
				int num3 = offset + count - trailByteCount;
				for (int i = 0; i < trailByteCount; i++)
				{
					trailBytes[i] = buffer[num3++];
				}
			}
		}
		else
		{
			Buffer.BlockCopy(buffer, offset, trailBytes, trailByteCount, count);
			trailByteCount += count;
		}
	}

	internal override IAsyncResult BeginWriteBase64(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		EnsureBufferBounds(buffer, offset, count);
		return new WriteBase64AsyncResult(buffer, offset, count, this, callback, state);
	}

	internal override void EndWriteBase64(IAsyncResult result)
	{
		WriteBase64AsyncResult.End(result);
	}

	internal override AsyncCompletionResult WriteBase64Async(AsyncEventArgs<XmlWriteBase64AsyncArguments> state)
	{
		if (nodeWriterAsyncHelper == null)
		{
			nodeWriterAsyncHelper = new XmlBaseWriterNodeWriterAsyncHelper(this);
		}
		nodeWriterAsyncHelper.SetArguments(state);
		if (nodeWriterAsyncHelper.StartAsync() == AsyncCompletionResult.Completed)
		{
			return AsyncCompletionResult.Completed;
		}
		return AsyncCompletionResult.Queued;
	}

	public override void WriteBinHex(byte[] buffer, int offset, int count)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		EnsureBufferBounds(buffer, offset, count);
		WriteRaw(BinHexEncoding.GetString(buffer, offset, count));
	}

	public override void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (Signing)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("XML canonicalization started")));
		}
		FlushElement();
		if (signingWriter == null)
		{
			signingWriter = CreateSigningNodeWriter();
		}
		signingWriter.SetOutput(writer, stream, includeComments, inclusivePrefixes);
		writer = signingWriter;
		SignScope(signingWriter.CanonicalWriter);
	}

	public override void EndCanonicalization()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (!Signing)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("XML canonicalization was not started.")));
		}
		signingWriter.Flush();
		writer = signingWriter.NodeWriter;
	}

	protected abstract XmlSigningNodeWriter CreateSigningNodeWriter();

	public void StartFragment(Stream stream, bool generateSelfContainedTextFragment)
	{
		if (!CanFragment)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("stream"));
		}
		if (oldStream != null || oldWriter != null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}
		if (WriteState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "StartFragment", WriteState.ToString())));
		}
		FlushElement();
		writer.Flush();
		oldNamespaceBoundary = NamespaceBoundary;
		XmlStreamNodeWriter xmlStreamNodeWriter = null;
		if (generateSelfContainedTextFragment)
		{
			NamespaceBoundary = depth + 1;
			if (textFragmentWriter == null)
			{
				textFragmentWriter = new XmlUTF8NodeWriter();
			}
			textFragmentWriter.SetOutput(stream, ownsStream: false, Encoding.UTF8);
			xmlStreamNodeWriter = textFragmentWriter;
		}
		if (Signing)
		{
			if (xmlStreamNodeWriter != null)
			{
				oldWriter = signingWriter.NodeWriter;
				signingWriter.NodeWriter = xmlStreamNodeWriter;
			}
			else
			{
				oldStream = ((XmlStreamNodeWriter)signingWriter.NodeWriter).Stream;
				((XmlStreamNodeWriter)signingWriter.NodeWriter).Stream = stream;
			}
		}
		else if (xmlStreamNodeWriter != null)
		{
			oldWriter = writer;
			writer = xmlStreamNodeWriter;
		}
		else
		{
			oldStream = nodeWriter.Stream;
			nodeWriter.Stream = stream;
		}
	}

	public void EndFragment()
	{
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (oldStream == null && oldWriter == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}
		if (WriteState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "EndFragment", WriteState.ToString())));
		}
		FlushElement();
		writer.Flush();
		if (Signing)
		{
			if (oldWriter != null)
			{
				signingWriter.NodeWriter = oldWriter;
			}
			else
			{
				((XmlStreamNodeWriter)signingWriter.NodeWriter).Stream = oldStream;
			}
		}
		else if (oldWriter != null)
		{
			writer = oldWriter;
		}
		else
		{
			nodeWriter.Stream = oldStream;
		}
		NamespaceBoundary = oldNamespaceBoundary;
		oldWriter = null;
		oldStream = null;
	}

	public void WriteFragment(byte[] buffer, int offset, int count)
	{
		if (!CanFragment)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
		}
		if (IsClosed)
		{
			ThrowClosed();
		}
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
		}
		if (WriteState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteFragment", WriteState.ToString())));
		}
		if (writer != nodeWriter)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException());
		}
		FlushElement();
		FlushBase64();
		nodeWriter.Flush();
		nodeWriter.Stream.Write(buffer, offset, count);
	}

	private void FlushBase64()
	{
		if (trailByteCount > 0)
		{
			FlushTrailBytes();
		}
	}

	private void FlushTrailBytes()
	{
		if (attributeValue != null)
		{
			WriteAttributeText(XmlConverter.Base64Encoding.GetString(trailBytes, 0, trailByteCount));
		}
		if (!isXmlnsAttribute)
		{
			StartContent();
			writer.WriteBase64Text(trailBytes, trailByteCount, trailBytes, 0, 0);
			EndContent();
		}
		trailByteCount = 0;
	}

	private void WriteValue(object[] array)
	{
		FlushBase64();
		StartContent();
		writer.WriteStartListText();
		inList = true;
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				writer.WriteListSeparator();
			}
			WritePrimitiveValue(array[i]);
		}
		inList = false;
		writer.WriteEndListText();
		EndContent();
	}

	private void WriteValue(Array array)
	{
		FlushBase64();
		StartContent();
		writer.WriteStartListText();
		inList = true;
		for (int i = 0; i < array.Length; i++)
		{
			if (i != 0)
			{
				writer.WriteListSeparator();
			}
			WritePrimitiveValue(array.GetValue(i));
		}
		inList = false;
		writer.WriteEndListText();
		EndContent();
	}

	protected void StartArray(int count)
	{
		FlushBase64();
		if (documentState == DocumentState.Epilog)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Only one root element is permitted per document.")));
		}
		if (documentState == DocumentState.Document && count > 1 && depth == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Only one root element is permitted per document.")));
		}
		if (writeState == WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("'{0}' cannot be called while WriteState is '{1}'.", "WriteStartElement", WriteState.ToString())));
		}
		AutoComplete(WriteState.Content);
	}

	protected void EndArray()
	{
	}

	private void EnsureBufferBounds(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > buffer.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
		}
	}

	private string GeneratePrefix(string ns, XmlDictionaryString xNs)
	{
		if (writeState != WriteState.Element && writeState != WriteState.Attribute)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("A prefix cannot be defined while WriteState is '{0}'.", WriteState.ToString())));
		}
		string text = nsMgr.AddNamespace(ns, xNs);
		if (text != null)
		{
			return text;
		}
		do
		{
			int num = elements[depth].PrefixId++;
			text = "d" + depth.ToString(CultureInfo.InvariantCulture) + "p" + num.ToString(CultureInfo.InvariantCulture);
		}
		while (nsMgr.LookupNamespace(text) != null);
		nsMgr.AddNamespace(text, ns, xNs);
		return text;
	}

	protected void SignScope(XmlCanonicalWriter signingWriter)
	{
		nsMgr.Sign(signingWriter);
	}

	private void WriteAttributeText(string value)
	{
		if (attributeValue.Length == 0)
		{
			attributeValue = value;
		}
		else
		{
			attributeValue += value;
		}
	}
}
