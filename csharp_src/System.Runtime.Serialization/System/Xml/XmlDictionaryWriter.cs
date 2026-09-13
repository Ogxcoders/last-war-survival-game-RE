using System.Globalization;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace System.Xml;

public abstract class XmlDictionaryWriter : XmlWriter
{
	private class WriteValueFastAsyncResult : AsyncResult
	{
		private enum Operation
		{
			Read,
			Write,
			Complete
		}

		private bool completed;

		private int blockSize;

		private byte[] block;

		private int bytesRead;

		private Stream stream;

		private Operation nextOperation;

		private IStreamProvider streamProvider;

		private XmlDictionaryWriter writer;

		private AsyncEventArgs<XmlWriteBase64AsyncArguments> writerAsyncState;

		private XmlWriteBase64AsyncArguments writerAsyncArgs;

		private static AsyncCallback onReadComplete = Fx.ThunkCallback(OnReadComplete);

		private static AsyncEventArgsCallback onWriteComplete;

		public WriteValueFastAsyncResult(XmlDictionaryWriter writer, IStreamProvider value, AsyncCallback callback, object state)
			: base(callback, state)
		{
			streamProvider = value;
			this.writer = writer;
			stream = value.GetStream();
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Stream returned by IStreamProvider cannot be null.")));
			}
			blockSize = 256;
			bytesRead = 0;
			block = new byte[blockSize];
			nextOperation = Operation.Read;
			ContinueWork(completedSynchronously: true);
		}

		private void CompleteAndReleaseStream(bool completedSynchronously, Exception completionException = null)
		{
			if (completionException == null)
			{
				streamProvider.ReleaseStream(stream);
				stream = null;
			}
			Complete(completedSynchronously, completionException);
		}

		private void ContinueWork(bool completedSynchronously, Exception completionException = null)
		{
			try
			{
				while (true)
				{
					if (nextOperation == Operation.Read)
					{
						if (ReadAsync() != AsyncCompletionResult.Completed)
						{
							return;
						}
					}
					else if (nextOperation == Operation.Write)
					{
						if (WriteAsync() != AsyncCompletionResult.Completed)
						{
							return;
						}
					}
					else if (nextOperation == Operation.Complete)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				if (completedSynchronously)
				{
					throw;
				}
				if (completionException == null)
				{
					completionException = ex;
				}
			}
			if (!completed)
			{
				completed = true;
				CompleteAndReleaseStream(completedSynchronously, completionException);
			}
		}

		private AsyncCompletionResult ReadAsync()
		{
			IAsyncResult asyncResult = stream.BeginRead(block, 0, blockSize, onReadComplete, this);
			if (asyncResult.CompletedSynchronously)
			{
				HandleReadComplete(asyncResult);
				return AsyncCompletionResult.Completed;
			}
			return AsyncCompletionResult.Queued;
		}

		private void HandleReadComplete(IAsyncResult result)
		{
			bytesRead = stream.EndRead(result);
			if (bytesRead > 0)
			{
				nextOperation = Operation.Write;
			}
			else
			{
				nextOperation = Operation.Complete;
			}
		}

		private static void OnReadComplete(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			Exception completionException = null;
			WriteValueFastAsyncResult writeValueFastAsyncResult = (WriteValueFastAsyncResult)result.AsyncState;
			bool flag = false;
			try
			{
				writeValueFastAsyncResult.HandleReadComplete(result);
				flag = true;
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				completionException = ex;
			}
			if (!flag)
			{
				writeValueFastAsyncResult.nextOperation = Operation.Complete;
			}
			writeValueFastAsyncResult.ContinueWork(completedSynchronously: false, completionException);
		}

		private AsyncCompletionResult WriteAsync()
		{
			if (writerAsyncState == null)
			{
				writerAsyncArgs = new XmlWriteBase64AsyncArguments();
				writerAsyncState = new AsyncEventArgs<XmlWriteBase64AsyncArguments>();
			}
			if (onWriteComplete == null)
			{
				onWriteComplete = OnWriteComplete;
			}
			writerAsyncArgs.Buffer = block;
			writerAsyncArgs.Offset = 0;
			writerAsyncArgs.Count = bytesRead;
			writerAsyncState.Set(onWriteComplete, writerAsyncArgs, this);
			if (writer.WriteBase64Async(writerAsyncState) == AsyncCompletionResult.Completed)
			{
				HandleWriteComplete();
				writerAsyncState.Complete(completedSynchronously: true);
				return AsyncCompletionResult.Completed;
			}
			return AsyncCompletionResult.Queued;
		}

		private void HandleWriteComplete()
		{
			nextOperation = Operation.Read;
			if (blockSize < 65536 && bytesRead == blockSize)
			{
				blockSize *= 16;
				block = new byte[blockSize];
			}
		}

		private static void OnWriteComplete(IAsyncEventArgs asyncState)
		{
			WriteValueFastAsyncResult writeValueFastAsyncResult = (WriteValueFastAsyncResult)asyncState.AsyncState;
			Exception completionException = null;
			bool flag = false;
			try
			{
				if (asyncState.Exception != null)
				{
					completionException = asyncState.Exception;
				}
				else
				{
					writeValueFastAsyncResult.HandleWriteComplete();
					flag = true;
				}
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				completionException = ex;
			}
			if (!flag)
			{
				writeValueFastAsyncResult.nextOperation = Operation.Complete;
			}
			writeValueFastAsyncResult.ContinueWork(completedSynchronously: false, completionException);
		}

		internal static void End(IAsyncResult result)
		{
			AsyncResult.End<WriteValueFastAsyncResult>(result);
		}
	}

	private class WriteValueAsyncResult : AsyncResult
	{
		private enum Operation
		{
			Read,
			Write
		}

		private int blockSize;

		private byte[] block;

		private int bytesRead;

		private Stream stream;

		private Operation operation;

		private IStreamProvider streamProvider;

		private XmlDictionaryWriter writer;

		private Func<IAsyncResult, WriteValueAsyncResult, bool> writeBlockHandler;

		private static Func<IAsyncResult, WriteValueAsyncResult, bool> handleWriteBlock = HandleWriteBlock;

		private static Func<IAsyncResult, WriteValueAsyncResult, bool> handleWriteBlockAsync = HandleWriteBlockAsync;

		private static AsyncCallback onContinueWork = Fx.ThunkCallback(OnContinueWork);

		public WriteValueAsyncResult(XmlDictionaryWriter writer, IStreamProvider value, AsyncCallback callback, object state)
			: base(callback, state)
		{
			streamProvider = value;
			this.writer = writer;
			writeBlockHandler = ((this.writer.Settings != null && this.writer.Settings.Async) ? handleWriteBlockAsync : handleWriteBlock);
			stream = value.GetStream();
			if (stream == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Stream returned by IStreamProvider cannot be null.")));
			}
			blockSize = 256;
			bytesRead = 0;
			block = new byte[blockSize];
			if (ContinueWork(null))
			{
				CompleteAndReleaseStream(completedSynchronously: true, null);
			}
		}

		private void AdjustBlockSize()
		{
			if (blockSize < 65536 && bytesRead == blockSize)
			{
				blockSize *= 16;
				block = new byte[blockSize];
			}
		}

		private void CompleteAndReleaseStream(bool completedSynchronously, Exception completionException)
		{
			if (completionException == null)
			{
				streamProvider.ReleaseStream(stream);
				stream = null;
			}
			Complete(completedSynchronously, completionException);
		}

		private bool ContinueWork(IAsyncResult result)
		{
			while (true)
			{
				if (operation == Operation.Read)
				{
					if (!HandleReadBlock(result))
					{
						return false;
					}
					if (bytesRead <= 0)
					{
						return true;
					}
					operation = Operation.Write;
				}
				else
				{
					if (!writeBlockHandler(result, this))
					{
						break;
					}
					AdjustBlockSize();
					operation = Operation.Read;
				}
				result = null;
			}
			return false;
		}

		private bool HandleReadBlock(IAsyncResult result)
		{
			if (result == null)
			{
				result = stream.BeginRead(block, 0, blockSize, onContinueWork, this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			bytesRead = stream.EndRead(result);
			return true;
		}

		private static bool HandleWriteBlock(IAsyncResult result, WriteValueAsyncResult thisPtr)
		{
			if (result == null)
			{
				result = thisPtr.writer.BeginWriteBase64(thisPtr.block, 0, thisPtr.bytesRead, onContinueWork, thisPtr);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			thisPtr.writer.EndWriteBase64(result);
			return true;
		}

		private static bool HandleWriteBlockAsync(IAsyncResult result, WriteValueAsyncResult thisPtr)
		{
			Task task = (Task)result;
			if (task == null)
			{
				task = thisPtr.writer.WriteBase64Async(thisPtr.block, 0, thisPtr.bytesRead);
				task.AsAsyncResult(onContinueWork, thisPtr);
				return false;
			}
			task.GetAwaiter().GetResult();
			return true;
		}

		private static void OnContinueWork(IAsyncResult result)
		{
			if (result.CompletedSynchronously && !(result is Task))
			{
				return;
			}
			Exception completionException = null;
			WriteValueAsyncResult writeValueAsyncResult = (WriteValueAsyncResult)result.AsyncState;
			bool flag = false;
			try
			{
				flag = writeValueAsyncResult.ContinueWork(result);
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
				flag = true;
				completionException = ex;
			}
			if (flag)
			{
				writeValueAsyncResult.CompleteAndReleaseStream(completedSynchronously: false, completionException);
			}
		}

		public static void End(IAsyncResult result)
		{
			AsyncResult.End<WriteValueAsyncResult>(result);
		}
	}

	private class WriteBase64AsyncResult : ScheduleActionItemAsyncResult
	{
		private byte[] buffer;

		private int index;

		private int count;

		private XmlDictionaryWriter writer;

		public WriteBase64AsyncResult(byte[] buffer, int index, int count, XmlDictionaryWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.buffer = buffer;
			this.index = index;
			this.count = count;
			this.writer = writer;
			Schedule();
		}

		protected override void OnDoWork()
		{
			writer.WriteBase64(buffer, index, count);
		}
	}

	private class XmlWrappedWriter : XmlDictionaryWriter
	{
		private XmlWriter writer;

		private int depth;

		private int prefix;

		public override WriteState WriteState => writer.WriteState;

		public override string XmlLang => writer.XmlLang;

		public override XmlSpace XmlSpace => writer.XmlSpace;

		public XmlWrappedWriter(XmlWriter writer)
		{
			this.writer = writer;
			depth = 0;
		}

		public override void Close()
		{
			writer.Close();
		}

		public override void Flush()
		{
			writer.Flush();
		}

		public override string LookupPrefix(string namespaceUri)
		{
			return writer.LookupPrefix(namespaceUri);
		}

		public override void WriteAttributes(XmlReader reader, bool defattr)
		{
			writer.WriteAttributes(reader, defattr);
		}

		public override void WriteBase64(byte[] buffer, int index, int count)
		{
			writer.WriteBase64(buffer, index, count);
		}

		public override void WriteBinHex(byte[] buffer, int index, int count)
		{
			writer.WriteBinHex(buffer, index, count);
		}

		public override void WriteCData(string text)
		{
			writer.WriteCData(text);
		}

		public override void WriteCharEntity(char ch)
		{
			writer.WriteCharEntity(ch);
		}

		public override void WriteChars(char[] buffer, int index, int count)
		{
			writer.WriteChars(buffer, index, count);
		}

		public override void WriteComment(string text)
		{
			writer.WriteComment(text);
		}

		public override void WriteDocType(string name, string pubid, string sysid, string subset)
		{
			writer.WriteDocType(name, pubid, sysid, subset);
		}

		public override void WriteEndAttribute()
		{
			writer.WriteEndAttribute();
		}

		public override void WriteEndDocument()
		{
			writer.WriteEndDocument();
		}

		public override void WriteEndElement()
		{
			writer.WriteEndElement();
			depth--;
		}

		public override void WriteEntityRef(string name)
		{
			writer.WriteEntityRef(name);
		}

		public override void WriteFullEndElement()
		{
			writer.WriteFullEndElement();
		}

		public override void WriteName(string name)
		{
			writer.WriteName(name);
		}

		public override void WriteNmToken(string name)
		{
			writer.WriteNmToken(name);
		}

		public override void WriteNode(XmlReader reader, bool defattr)
		{
			writer.WriteNode(reader, defattr);
		}

		public override void WriteProcessingInstruction(string name, string text)
		{
			writer.WriteProcessingInstruction(name, text);
		}

		public override void WriteQualifiedName(string localName, string namespaceUri)
		{
			writer.WriteQualifiedName(localName, namespaceUri);
		}

		public override void WriteRaw(char[] buffer, int index, int count)
		{
			writer.WriteRaw(buffer, index, count);
		}

		public override void WriteRaw(string data)
		{
			writer.WriteRaw(data);
		}

		public override void WriteStartAttribute(string prefix, string localName, string namespaceUri)
		{
			writer.WriteStartAttribute(prefix, localName, namespaceUri);
			this.prefix++;
		}

		public override void WriteStartDocument()
		{
			writer.WriteStartDocument();
		}

		public override void WriteStartDocument(bool standalone)
		{
			writer.WriteStartDocument(standalone);
		}

		public override void WriteStartElement(string prefix, string localName, string namespaceUri)
		{
			writer.WriteStartElement(prefix, localName, namespaceUri);
			depth++;
			this.prefix = 1;
		}

		public override void WriteString(string text)
		{
			writer.WriteString(text);
		}

		public override void WriteSurrogateCharEntity(char lowChar, char highChar)
		{
			writer.WriteSurrogateCharEntity(lowChar, highChar);
		}

		public override void WriteWhitespace(string whitespace)
		{
			writer.WriteWhitespace(whitespace);
		}

		public override void WriteValue(object value)
		{
			writer.WriteValue(value);
		}

		public override void WriteValue(string value)
		{
			writer.WriteValue(value);
		}

		public override void WriteValue(bool value)
		{
			writer.WriteValue(value);
		}

		public override void WriteValue(DateTime value)
		{
			writer.WriteValue(value);
		}

		public override void WriteValue(double value)
		{
			writer.WriteValue(value);
		}

		public override void WriteValue(int value)
		{
			writer.WriteValue(value);
		}

		public override void WriteValue(long value)
		{
			writer.WriteValue(value);
		}

		public override void WriteXmlnsAttribute(string prefix, string namespaceUri)
		{
			if (namespaceUri == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
			}
			if (prefix == null)
			{
				if (LookupPrefix(namespaceUri) != null)
				{
					return;
				}
				if (namespaceUri.Length == 0)
				{
					prefix = string.Empty;
				}
				else
				{
					string text = depth.ToString(NumberFormatInfo.InvariantInfo);
					string text2 = this.prefix.ToString(NumberFormatInfo.InvariantInfo);
					prefix = "d" + text + "p" + text2;
				}
			}
			WriteAttributeString("xmlns", prefix, null, namespaceUri);
		}
	}

	internal virtual bool FastAsync => false;

	public virtual bool CanCanonicalize => false;

	internal virtual AsyncCompletionResult WriteBase64Async(AsyncEventArgs<XmlWriteBase64AsyncArguments> state)
	{
		throw FxTrace.Exception.AsError(new NotSupportedException());
	}

	public override Task WriteBase64Async(byte[] buffer, int index, int count)
	{
		return Task.Factory.FromAsync(BeginWriteBase64, EndWriteBase64, buffer, index, count, null);
	}

	internal virtual IAsyncResult BeginWriteBase64(byte[] buffer, int index, int count, AsyncCallback callback, object state)
	{
		return new WriteBase64AsyncResult(buffer, index, count, this, callback, state);
	}

	internal virtual void EndWriteBase64(IAsyncResult result)
	{
		ScheduleActionItemAsyncResult.End(result);
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream)
	{
		return CreateBinaryWriter(stream, null);
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary dictionary)
	{
		return CreateBinaryWriter(stream, dictionary, null);
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session)
	{
		return CreateBinaryWriter(stream, dictionary, session, ownsStream: true);
	}

	public static XmlDictionaryWriter CreateBinaryWriter(Stream stream, IXmlDictionary dictionary, XmlBinaryWriterSession session, bool ownsStream)
	{
		XmlBinaryWriter xmlBinaryWriter = new XmlBinaryWriter();
		xmlBinaryWriter.SetOutput(stream, dictionary, session, ownsStream);
		return xmlBinaryWriter;
	}

	public static XmlDictionaryWriter CreateTextWriter(Stream stream)
	{
		return CreateTextWriter(stream, Encoding.UTF8, ownsStream: true);
	}

	public static XmlDictionaryWriter CreateTextWriter(Stream stream, Encoding encoding)
	{
		return CreateTextWriter(stream, encoding, ownsStream: true);
	}

	public static XmlDictionaryWriter CreateTextWriter(Stream stream, Encoding encoding, bool ownsStream)
	{
		XmlUTF8TextWriter xmlUTF8TextWriter = new XmlUTF8TextWriter();
		xmlUTF8TextWriter.SetOutput(stream, encoding, ownsStream);
		return xmlUTF8TextWriter;
	}

	public static XmlDictionaryWriter CreateMtomWriter(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo)
	{
		return CreateMtomWriter(stream, encoding, maxSizeInBytes, startInfo, null, null, writeMessageHeaders: true, ownsStream: true);
	}

	public static XmlDictionaryWriter CreateMtomWriter(Stream stream, Encoding encoding, int maxSizeInBytes, string startInfo, string boundary, string startUri, bool writeMessageHeaders, bool ownsStream)
	{
		XmlMtomWriter xmlMtomWriter = new XmlMtomWriter();
		xmlMtomWriter.SetOutput(stream, encoding, maxSizeInBytes, startInfo, boundary, startUri, writeMessageHeaders, ownsStream);
		return xmlMtomWriter;
	}

	public static XmlDictionaryWriter CreateDictionaryWriter(XmlWriter writer)
	{
		if (writer == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("writer");
		}
		XmlDictionaryWriter xmlDictionaryWriter = writer as XmlDictionaryWriter;
		if (xmlDictionaryWriter == null)
		{
			xmlDictionaryWriter = new XmlWrappedWriter(writer);
		}
		return xmlDictionaryWriter;
	}

	public void WriteStartElement(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		WriteStartElement(null, localName, namespaceUri);
	}

	public virtual void WriteStartElement(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		WriteStartElement(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
	}

	public void WriteStartAttribute(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		WriteStartAttribute(null, localName, namespaceUri);
	}

	public virtual void WriteStartAttribute(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		WriteStartAttribute(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri));
	}

	public void WriteAttributeString(XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
	{
		WriteAttributeString(null, localName, namespaceUri, value);
	}

	public virtual void WriteXmlnsAttribute(string prefix, string namespaceUri)
	{
		if (namespaceUri == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("namespaceUri");
		}
		if (prefix == null)
		{
			if (LookupPrefix(namespaceUri) != null)
			{
				return;
			}
			prefix = ((namespaceUri.Length == 0) ? string.Empty : ("d" + namespaceUri.Length.ToString(NumberFormatInfo.InvariantInfo)));
		}
		WriteAttributeString("xmlns", prefix, null, namespaceUri);
	}

	public virtual void WriteXmlnsAttribute(string prefix, XmlDictionaryString namespaceUri)
	{
		WriteXmlnsAttribute(prefix, XmlDictionaryString.GetString(namespaceUri));
	}

	public virtual void WriteXmlAttribute(string localName, string value)
	{
		WriteAttributeString("xml", localName, null, value);
	}

	public virtual void WriteXmlAttribute(XmlDictionaryString localName, XmlDictionaryString value)
	{
		WriteXmlAttribute(XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(value));
	}

	public void WriteAttributeString(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
	{
		WriteStartAttribute(prefix, localName, namespaceUri);
		WriteString(value);
		WriteEndAttribute();
	}

	public void WriteElementString(XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
	{
		WriteElementString(null, localName, namespaceUri, value);
	}

	public void WriteElementString(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, string value)
	{
		WriteStartElement(prefix, localName, namespaceUri);
		WriteString(value);
		WriteEndElement();
	}

	public virtual void WriteString(XmlDictionaryString value)
	{
		WriteString(XmlDictionaryString.GetString(value));
	}

	public virtual void WriteQualifiedName(XmlDictionaryString localName, XmlDictionaryString namespaceUri)
	{
		if (localName == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("localName"));
		}
		if (namespaceUri == null)
		{
			namespaceUri = XmlDictionaryString.Empty;
		}
		WriteQualifiedName(localName.Value, namespaceUri.Value);
	}

	public virtual void WriteValue(XmlDictionaryString value)
	{
		WriteValue(XmlDictionaryString.GetString(value));
	}

	public virtual void WriteValue(IStreamProvider value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		Stream stream = value.GetStream();
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(SR.GetString("Stream returned by IStreamProvider cannot be null.")));
		}
		int num = 256;
		int num2 = 0;
		byte[] buffer = new byte[num];
		while (true)
		{
			num2 = stream.Read(buffer, 0, num);
			if (num2 <= 0)
			{
				break;
			}
			WriteBase64(buffer, 0, num2);
			if (num < 65536 && num2 == num)
			{
				num *= 16;
				buffer = new byte[num];
			}
		}
		value.ReleaseStream(stream);
	}

	public virtual Task WriteValueAsync(IStreamProvider value)
	{
		return Task.Factory.FromAsync(BeginWriteValue, EndWriteValue, value, null);
	}

	internal virtual IAsyncResult BeginWriteValue(IStreamProvider value, AsyncCallback callback, object state)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (FastAsync)
		{
			return new WriteValueFastAsyncResult(this, value, callback, state);
		}
		return new WriteValueAsyncResult(this, value, callback, state);
	}

	internal virtual void EndWriteValue(IAsyncResult result)
	{
		if (FastAsync)
		{
			WriteValueFastAsyncResult.End(result);
		}
		else
		{
			WriteValueAsyncResult.End(result);
		}
	}

	public virtual void WriteValue(UniqueId value)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		WriteString(value.ToString());
	}

	public virtual void WriteValue(Guid value)
	{
		WriteString(value.ToString());
	}

	public virtual void WriteValue(TimeSpan value)
	{
		WriteString(XmlConvert.ToString(value));
	}

	public virtual void StartCanonicalization(Stream stream, bool includeComments, string[] inclusivePrefixes)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	public virtual void EndCanonicalization()
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	private void WriteElementNode(XmlDictionaryReader reader, bool defattr)
	{
		if (reader.TryGetLocalNameAsDictionaryString(out var localName) && reader.TryGetNamespaceUriAsDictionaryString(out var namespaceUri))
		{
			WriteStartElement(reader.Prefix, localName, namespaceUri);
		}
		else
		{
			WriteStartElement(reader.Prefix, reader.LocalName, reader.NamespaceURI);
		}
		if ((defattr || (!reader.IsDefault && (reader.SchemaInfo == null || !reader.SchemaInfo.IsDefault))) && reader.MoveToFirstAttribute())
		{
			do
			{
				if (reader.TryGetLocalNameAsDictionaryString(out localName) && reader.TryGetNamespaceUriAsDictionaryString(out namespaceUri))
				{
					WriteStartAttribute(reader.Prefix, localName, namespaceUri);
				}
				else
				{
					WriteStartAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
				}
				while (reader.ReadAttributeValue())
				{
					if (reader.NodeType == XmlNodeType.EntityReference)
					{
						WriteEntityRef(reader.Name);
					}
					else
					{
						WriteTextNode(reader, isAttribute: true);
					}
				}
				WriteEndAttribute();
			}
			while (reader.MoveToNextAttribute());
			reader.MoveToElement();
		}
		if (reader.IsEmptyElement)
		{
			WriteEndElement();
		}
	}

	private void WriteArrayNode(XmlDictionaryReader reader, string prefix, string localName, string namespaceUri, Type type)
	{
		if (type == typeof(bool))
		{
			BooleanArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(short))
		{
			Int16ArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(int))
		{
			Int32ArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(long))
		{
			Int64ArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(float))
		{
			SingleArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(double))
		{
			DoubleArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(decimal))
		{
			DecimalArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(DateTime))
		{
			DateTimeArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(Guid))
		{
			GuidArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(TimeSpan))
		{
			TimeSpanArrayHelperWithString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		WriteElementNode(reader, defattr: false);
		reader.Read();
	}

	private void WriteArrayNode(XmlDictionaryReader reader, string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Type type)
	{
		if (type == typeof(bool))
		{
			BooleanArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(short))
		{
			Int16ArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(int))
		{
			Int32ArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(long))
		{
			Int64ArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(float))
		{
			SingleArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(double))
		{
			DoubleArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(decimal))
		{
			DecimalArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(DateTime))
		{
			DateTimeArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(Guid))
		{
			GuidArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		if (type == typeof(TimeSpan))
		{
			TimeSpanArrayHelperWithDictionaryString.Instance.WriteArray(this, prefix, localName, namespaceUri, reader);
			return;
		}
		WriteElementNode(reader, defattr: false);
		reader.Read();
	}

	private void WriteArrayNode(XmlDictionaryReader reader, Type type)
	{
		if (reader.TryGetLocalNameAsDictionaryString(out var localName) && reader.TryGetNamespaceUriAsDictionaryString(out var namespaceUri))
		{
			WriteArrayNode(reader, reader.Prefix, localName, namespaceUri, type);
		}
		else
		{
			WriteArrayNode(reader, reader.Prefix, reader.LocalName, reader.NamespaceURI, type);
		}
	}

	protected virtual void WriteTextNode(XmlDictionaryReader reader, bool isAttribute)
	{
		if (reader.TryGetValueAsDictionaryString(out var value))
		{
			WriteString(value);
		}
		else
		{
			WriteString(reader.Value);
		}
		if (!isAttribute)
		{
			reader.Read();
		}
	}

	public override void WriteNode(XmlReader reader, bool defattr)
	{
		if (reader is XmlDictionaryReader reader2)
		{
			WriteNode(reader2, defattr);
		}
		else
		{
			base.WriteNode(reader, defattr);
		}
	}

	public virtual void WriteNode(XmlDictionaryReader reader, bool defattr)
	{
		if (reader == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("reader"));
		}
		int num = ((reader.NodeType == XmlNodeType.None) ? (-1) : reader.Depth);
		do
		{
			XmlNodeType nodeType = reader.NodeType;
			if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
			{
				WriteTextNode(reader, isAttribute: false);
				continue;
			}
			if (reader.Depth > num && reader.IsStartArray(out var type))
			{
				WriteArrayNode(reader, type);
				continue;
			}
			switch (nodeType)
			{
			case XmlNodeType.Element:
				WriteElementNode(reader, defattr);
				break;
			case XmlNodeType.CDATA:
				WriteCData(reader.Value);
				break;
			case XmlNodeType.EntityReference:
				WriteEntityRef(reader.Name);
				break;
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.XmlDeclaration:
				WriteProcessingInstruction(reader.Name, reader.Value);
				break;
			case XmlNodeType.DocumentType:
				WriteDocType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
				break;
			case XmlNodeType.Comment:
				WriteComment(reader.Value);
				break;
			case XmlNodeType.EndElement:
				WriteFullEndElement();
				break;
			}
			if (!reader.Read())
			{
				break;
			}
		}
		while (num < reader.Depth || (num == reader.Depth && reader.NodeType == XmlNodeType.EndElement));
	}

	private void CheckArray(Array array, int offset, int count)
	{
		if (array == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("array"));
		}
		if (offset < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (offset > array.Length)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", array.Length)));
		}
		if (count < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
		}
		if (count > array.Length - offset)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", array.Length - offset)));
		}
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, bool[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, bool[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, short[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, short[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, int[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, int[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, long[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, long[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, float[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, float[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, double[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, double[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, decimal[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, decimal[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, DateTime[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, DateTime[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, Guid[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, Guid[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}

	public virtual void WriteArray(string prefix, string localName, string namespaceUri, TimeSpan[] array, int offset, int count)
	{
		CheckArray(array, offset, count);
		for (int i = 0; i < count; i++)
		{
			WriteStartElement(prefix, localName, namespaceUri);
			WriteValue(array[offset + i]);
			WriteEndElement();
		}
	}

	public virtual void WriteArray(string prefix, XmlDictionaryString localName, XmlDictionaryString namespaceUri, TimeSpan[] array, int offset, int count)
	{
		WriteArray(prefix, XmlDictionaryString.GetString(localName), XmlDictionaryString.GetString(namespaceUri), array, offset, count);
	}
}
