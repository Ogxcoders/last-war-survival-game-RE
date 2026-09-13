using System.IO;
using System.Runtime;
using System.Security;
using System.Text;

namespace System.Xml;

internal class XmlUTF8NodeWriter : XmlStreamNodeWriter
{
	private class InternalWriteBase64TextAsyncWriter
	{
		private AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> nodeState;

		private AsyncEventArgs<XmlWriteBase64AsyncArguments> writerState;

		private XmlWriteBase64AsyncArguments writerArgs;

		private XmlUTF8NodeWriter writer;

		private GetBufferAsyncEventArgs getBufferState;

		private GetBufferArgs getBufferArgs;

		private static AsyncEventArgsCallback onTrailByteComplete = OnTrailBytesComplete;

		private static AsyncEventArgsCallback onWriteComplete = OnWriteComplete;

		private static AsyncEventArgsCallback onGetBufferComplete = OnGetBufferComplete;

		public InternalWriteBase64TextAsyncWriter(XmlUTF8NodeWriter writer)
		{
			this.writer = writer;
			writerState = new AsyncEventArgs<XmlWriteBase64AsyncArguments>();
			writerArgs = new XmlWriteBase64AsyncArguments();
		}

		internal AsyncCompletionResult StartAsync(AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> xmlNodeWriterState)
		{
			nodeState = xmlNodeWriterState;
			XmlNodeWriterWriteBase64TextArgs arguments = xmlNodeWriterState.Arguments;
			if (arguments.TrailCount > 0)
			{
				writerArgs.Buffer = arguments.TrailBuffer;
				writerArgs.Offset = 0;
				writerArgs.Count = arguments.TrailCount;
				writerState.Set(onTrailByteComplete, writerArgs, this);
				if (InternalWriteBase64TextAsync(writerState) != AsyncCompletionResult.Completed)
				{
					return AsyncCompletionResult.Queued;
				}
				writerState.Complete(completedSynchronously: true);
			}
			if (WriteBufferAsync() == AsyncCompletionResult.Completed)
			{
				nodeState = null;
				return AsyncCompletionResult.Completed;
			}
			return AsyncCompletionResult.Queued;
		}

		private static void OnTrailBytesComplete(IAsyncEventArgs eventArgs)
		{
			InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter = (InternalWriteBase64TextAsyncWriter)eventArgs.AsyncState;
			bool flag = false;
			try
			{
				if (eventArgs.Exception != null)
				{
					_ = eventArgs.Exception;
					flag = true;
				}
				else if (internalWriteBase64TextAsyncWriter.WriteBufferAsync() == AsyncCompletionResult.Completed)
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				if (Fx.IsFatal(exception))
				{
					throw;
				}
				flag = true;
			}
			if (flag)
			{
				AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> e = internalWriteBase64TextAsyncWriter.nodeState;
				internalWriteBase64TextAsyncWriter.nodeState = null;
				e.Complete(completedSynchronously: false, eventArgs.Exception);
			}
		}

		private AsyncCompletionResult WriteBufferAsync()
		{
			writerArgs.Buffer = nodeState.Arguments.Buffer;
			writerArgs.Offset = nodeState.Arguments.Offset;
			writerArgs.Count = nodeState.Arguments.Count;
			writerState.Set(onWriteComplete, writerArgs, this);
			if (InternalWriteBase64TextAsync(writerState) == AsyncCompletionResult.Completed)
			{
				writerState.Complete(completedSynchronously: true);
				return AsyncCompletionResult.Completed;
			}
			return AsyncCompletionResult.Queued;
		}

		private static void OnWriteComplete(IAsyncEventArgs eventArgs)
		{
			InternalWriteBase64TextAsyncWriter obj = (InternalWriteBase64TextAsyncWriter)eventArgs.AsyncState;
			AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> e = obj.nodeState;
			obj.nodeState = null;
			e.Complete(completedSynchronously: false, eventArgs.Exception);
		}

		private AsyncCompletionResult InternalWriteBase64TextAsync(AsyncEventArgs<XmlWriteBase64AsyncArguments> writerState)
		{
			GetBufferAsyncEventArgs e = getBufferState;
			GetBufferArgs getBufferArgs = this.getBufferArgs;
			XmlWriteBase64AsyncArguments arguments = writerState.Arguments;
			if (e == null)
			{
				e = new GetBufferAsyncEventArgs();
				getBufferArgs = new GetBufferArgs();
				getBufferState = e;
				this.getBufferArgs = getBufferArgs;
			}
			Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
			while (arguments.Count >= 3)
			{
				int num = Math.Min(384, arguments.Count - arguments.Count % 3);
				int count = num / 3 * 4;
				getBufferArgs.Count = count;
				e.Set(onGetBufferComplete, getBufferArgs, this);
				if (writer.GetBufferAsync(e) == AsyncCompletionResult.Completed)
				{
					GetBufferEventResult result = e.Result;
					e.Complete(completedSynchronously: true);
					writer.Advance(base64Encoding.GetChars(arguments.Buffer, arguments.Offset, num, result.Buffer, result.Offset));
					arguments.Offset += num;
					arguments.Count -= num;
					continue;
				}
				return AsyncCompletionResult.Queued;
			}
			if (arguments.Count > 0)
			{
				getBufferArgs.Count = 4;
				e.Set(onGetBufferComplete, getBufferArgs, this);
				if (writer.GetBufferAsync(e) != AsyncCompletionResult.Completed)
				{
					return AsyncCompletionResult.Queued;
				}
				GetBufferEventResult result2 = e.Result;
				e.Complete(completedSynchronously: true);
				writer.Advance(base64Encoding.GetChars(arguments.Buffer, arguments.Offset, arguments.Count, result2.Buffer, result2.Offset));
			}
			return AsyncCompletionResult.Completed;
		}

		private static void OnGetBufferComplete(IAsyncEventArgs state)
		{
			GetBufferEventResult result = ((GetBufferAsyncEventArgs)state).Result;
			InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter = (InternalWriteBase64TextAsyncWriter)state.AsyncState;
			XmlWriteBase64AsyncArguments arguments = internalWriteBase64TextAsyncWriter.writerState.Arguments;
			Exception exception = null;
			bool flag = false;
			try
			{
				if (state.Exception != null)
				{
					exception = state.Exception;
					flag = true;
				}
				else
				{
					byte[] buffer = result.Buffer;
					int offset = result.Offset;
					Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
					int num = Math.Min(384, arguments.Count - arguments.Count % 3);
					_ = num / 3;
					internalWriteBase64TextAsyncWriter.writer.Advance(base64Encoding.GetChars(arguments.Buffer, arguments.Offset, num, buffer, offset));
					if (num >= 3)
					{
						arguments.Offset += num;
						arguments.Count -= num;
					}
					if (internalWriteBase64TextAsyncWriter.InternalWriteBase64TextAsync(internalWriteBase64TextAsyncWriter.writerState) == AsyncCompletionResult.Completed)
					{
						flag = true;
					}
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
				internalWriteBase64TextAsyncWriter.writerState.Complete(completedSynchronously: false, exception);
			}
		}
	}

	private class WriteBase64TextAsyncResult : AsyncResult
	{
		private static AsyncCompletion onTrailBytesComplete = OnTrailBytesComplete;

		private static AsyncCompletion onComplete = OnComplete;

		private byte[] trailBytes;

		private int trailByteCount;

		private byte[] buffer;

		private int offset;

		private int count;

		private XmlUTF8NodeWriter writer;

		public WriteBase64TextAsyncResult(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count, XmlUTF8NodeWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.writer = writer;
			this.trailBytes = trailBytes;
			this.trailByteCount = trailByteCount;
			this.buffer = buffer;
			this.offset = offset;
			this.count = count;
			if (HandleWriteTrailBytes(null))
			{
				Complete(completedSynchronously: true);
			}
		}

		private static bool OnTrailBytesComplete(IAsyncResult result)
		{
			return ((WriteBase64TextAsyncResult)result.AsyncState).HandleWriteTrailBytes(result);
		}

		private static bool OnComplete(IAsyncResult result)
		{
			return ((WriteBase64TextAsyncResult)result.AsyncState).HandleWriteBase64Text(result);
		}

		private bool HandleWriteTrailBytes(IAsyncResult result)
		{
			if (trailByteCount > 0)
			{
				if (result == null)
				{
					result = writer.BeginInternalWriteBase64Text(trailBytes, 0, trailByteCount, PrepareAsyncCompletion(onTrailBytesComplete), this);
					if (!result.CompletedSynchronously)
					{
						return false;
					}
				}
				writer.EndInternalWriteBase64Text(result);
			}
			return HandleWriteBase64Text(null);
		}

		private bool HandleWriteBase64Text(IAsyncResult result)
		{
			if (result == null)
			{
				result = writer.BeginInternalWriteBase64Text(buffer, offset, count, PrepareAsyncCompletion(onComplete), this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			writer.EndInternalWriteBase64Text(result);
			return true;
		}

		public static void End(IAsyncResult result)
		{
			AsyncResult.End<WriteBase64TextAsyncResult>(result);
		}
	}

	private class InternalWriteBase64TextAsyncResult : AsyncResult
	{
		private byte[] buffer;

		private int offset;

		private int count;

		private Base64Encoding encoding;

		private XmlUTF8NodeWriter writer;

		private static AsyncCallback onWriteCharacters = Fx.ThunkCallback(OnWriteCharacters);

		private static AsyncCompletion onWriteTrailingCharacters = OnWriteTrailingCharacters;

		public InternalWriteBase64TextAsyncResult(byte[] buffer, int offset, int count, XmlUTF8NodeWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.buffer = buffer;
			this.offset = offset;
			this.count = count;
			this.writer = writer;
			encoding = XmlConverter.Base64Encoding;
			if (ContinueWork())
			{
				Complete(completedSynchronously: true);
			}
		}

		private static bool OnWriteTrailingCharacters(IAsyncResult result)
		{
			return ((InternalWriteBase64TextAsyncResult)result.AsyncState).HandleWriteTrailingCharacters(result);
		}

		private bool ContinueWork()
		{
			while (count >= 3)
			{
				if (!HandleWriteCharacters(null))
				{
					return false;
				}
			}
			if (count > 0)
			{
				return HandleWriteTrailingCharacters(null);
			}
			return true;
		}

		private bool HandleWriteCharacters(IAsyncResult result)
		{
			int num = Math.Min(384, count - count % 3);
			int num2 = num / 3 * 4;
			if (result == null)
			{
				result = writer.BeginGetBuffer(num2, onWriteCharacters, this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			int charIndex;
			byte[] chars = writer.EndGetBuffer(result, out charIndex);
			writer.Advance(encoding.GetChars(buffer, offset, num, chars, charIndex));
			offset += num;
			count -= num;
			return true;
		}

		private bool HandleWriteTrailingCharacters(IAsyncResult result)
		{
			if (result == null)
			{
				result = writer.BeginGetBuffer(4, PrepareAsyncCompletion(onWriteTrailingCharacters), this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			int charIndex;
			byte[] chars = writer.EndGetBuffer(result, out charIndex);
			writer.Advance(encoding.GetChars(buffer, offset, count, chars, charIndex));
			return true;
		}

		private static void OnWriteCharacters(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			InternalWriteBase64TextAsyncResult internalWriteBase64TextAsyncResult = (InternalWriteBase64TextAsyncResult)result.AsyncState;
			Exception ex = null;
			bool flag = false;
			try
			{
				internalWriteBase64TextAsyncResult.HandleWriteCharacters(result);
				flag = internalWriteBase64TextAsyncResult.ContinueWork();
			}
			catch (Exception ex2)
			{
				if (Fx.IsFatal(ex2))
				{
					throw;
				}
				flag = true;
				ex = ex2;
			}
			if (flag)
			{
				internalWriteBase64TextAsyncResult.Complete(completedSynchronously: false, ex);
			}
		}

		public static void End(IAsyncResult result)
		{
			AsyncResult.End<InternalWriteBase64TextAsyncResult>(result);
		}
	}

	private byte[] entityChars;

	private bool[] isEscapedAttributeChar;

	private bool[] isEscapedElementChar;

	private bool inAttribute;

	private const int bufferLength = 512;

	private const int maxEntityLength = 32;

	private const int maxBytesPerChar = 3;

	private Encoding encoding;

	private char[] chars;

	private InternalWriteBase64TextAsyncWriter internalWriteBase64TextAsyncWriter;

	private static readonly byte[] startDecl = new byte[30]
	{
		60, 63, 120, 109, 108, 32, 118, 101, 114, 115,
		105, 111, 110, 61, 34, 49, 46, 48, 34, 32,
		101, 110, 99, 111, 100, 105, 110, 103, 61, 34
	};

	private static readonly byte[] endDecl = new byte[3] { 34, 63, 62 };

	private static readonly byte[] utf8Decl = new byte[38]
	{
		60, 63, 120, 109, 108, 32, 118, 101, 114, 115,
		105, 111, 110, 61, 34, 49, 46, 48, 34, 32,
		101, 110, 99, 111, 100, 105, 110, 103, 61, 34,
		117, 116, 102, 45, 56, 34, 63, 62
	};

	private static readonly byte[] digits = new byte[16]
	{
		48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
		65, 66, 67, 68, 69, 70
	};

	private static readonly bool[] defaultIsEscapedAttributeChar = new bool[64]
	{
		true, true, true, true, true, true, true, true, true, true,
		true, true, true, true, true, true, true, true, true, true,
		true, true, true, true, true, true, true, true, true, true,
		true, true, false, false, true, false, false, false, true, false,
		false, false, false, false, false, false, false, false, false, false,
		false, false, false, false, false, false, false, false, false, false,
		true, false, true, false
	};

	private static readonly bool[] defaultIsEscapedElementChar = new bool[64]
	{
		true, true, true, true, true, true, true, true, true, false,
		false, true, true, true, true, true, true, true, true, true,
		true, true, true, true, true, true, true, true, true, true,
		true, true, false, false, false, false, false, false, true, false,
		false, false, false, false, false, false, false, false, false, false,
		false, false, false, false, false, false, false, false, false, false,
		true, false, true, false
	};

	public Encoding Encoding => encoding;

	public XmlUTF8NodeWriter()
		: this(defaultIsEscapedAttributeChar, defaultIsEscapedElementChar)
	{
	}

	public XmlUTF8NodeWriter(bool[] isEscapedAttributeChar, bool[] isEscapedElementChar)
	{
		this.isEscapedAttributeChar = isEscapedAttributeChar;
		this.isEscapedElementChar = isEscapedElementChar;
		inAttribute = false;
	}

	public new void SetOutput(Stream stream, bool ownsStream, Encoding encoding)
	{
		Encoding encoding2 = null;
		if (encoding != null && encoding.CodePage == Encoding.UTF8.CodePage)
		{
			encoding2 = encoding;
			encoding = null;
		}
		base.SetOutput(stream, ownsStream, encoding2);
		this.encoding = encoding;
		inAttribute = false;
	}

	private byte[] GetCharEntityBuffer()
	{
		if (entityChars == null)
		{
			entityChars = new byte[32];
		}
		return entityChars;
	}

	private char[] GetCharBuffer(int charCount)
	{
		if (charCount >= 256)
		{
			return new char[charCount];
		}
		if (chars == null || chars.Length < charCount)
		{
			chars = new char[charCount];
		}
		return chars;
	}

	public override void WriteDeclaration()
	{
		if (encoding == null)
		{
			WriteUTF8Chars(utf8Decl, 0, utf8Decl.Length);
			return;
		}
		WriteUTF8Chars(startDecl, 0, startDecl.Length);
		if (encoding.WebName == Encoding.BigEndianUnicode.WebName)
		{
			WriteUTF8Chars("utf-16BE");
		}
		else
		{
			WriteUTF8Chars(encoding.WebName);
		}
		WriteUTF8Chars(endDecl, 0, endDecl.Length);
	}

	public override void WriteCData(string text)
	{
		int num;
		byte[] array = GetBuffer(9, out num);
		array[num] = 60;
		array[num + 1] = 33;
		array[num + 2] = 91;
		array[num + 3] = 67;
		array[num + 4] = 68;
		array[num + 5] = 65;
		array[num + 6] = 84;
		array[num + 7] = 65;
		array[num + 8] = 91;
		Advance(9);
		WriteUTF8Chars(text);
		byte[] array2 = GetBuffer(3, out num);
		array2[num] = 93;
		array2[num + 1] = 93;
		array2[num + 2] = 62;
		Advance(3);
	}

	private void WriteStartComment()
	{
		int num;
		byte[] array = GetBuffer(4, out num);
		array[num] = 60;
		array[num + 1] = 33;
		array[num + 2] = 45;
		array[num + 3] = 45;
		Advance(4);
	}

	private void WriteEndComment()
	{
		int num;
		byte[] array = GetBuffer(3, out num);
		array[num] = 45;
		array[num + 1] = 45;
		array[num + 2] = 62;
		Advance(3);
	}

	public override void WriteComment(string text)
	{
		WriteStartComment();
		WriteUTF8Chars(text);
		WriteEndComment();
	}

	public override void WriteStartElement(string prefix, string localName)
	{
		WriteByte('<');
		if (prefix.Length != 0)
		{
			WritePrefix(prefix);
			WriteByte(':');
		}
		WriteLocalName(localName);
	}

	public override void WriteStartElement(string prefix, XmlDictionaryString localName)
	{
		WriteStartElement(prefix, localName.Value);
	}

	public override void WriteStartElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
	{
		WriteByte('<');
		if (prefixLength != 0)
		{
			WritePrefix(prefixBuffer, prefixOffset, prefixLength);
			WriteByte(':');
		}
		WriteLocalName(localNameBuffer, localNameOffset, localNameLength);
	}

	public override void WriteEndStartElement(bool isEmpty)
	{
		if (!isEmpty)
		{
			WriteByte('>');
		}
		else
		{
			WriteBytes('/', '>');
		}
	}

	public override void WriteEndElement(string prefix, string localName)
	{
		WriteBytes('<', '/');
		if (prefix.Length != 0)
		{
			WritePrefix(prefix);
			WriteByte(':');
		}
		WriteLocalName(localName);
		WriteByte('>');
	}

	public override void WriteEndElement(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
	{
		WriteBytes('<', '/');
		if (prefixLength != 0)
		{
			WritePrefix(prefixBuffer, prefixOffset, prefixLength);
			WriteByte(':');
		}
		WriteLocalName(localNameBuffer, localNameOffset, localNameLength);
		WriteByte('>');
	}

	private void WriteStartXmlnsAttribute()
	{
		int num;
		byte[] array = GetBuffer(6, out num);
		array[num] = 32;
		array[num + 1] = 120;
		array[num + 2] = 109;
		array[num + 3] = 108;
		array[num + 4] = 110;
		array[num + 5] = 115;
		Advance(6);
		inAttribute = true;
	}

	public override void WriteXmlnsAttribute(string prefix, string ns)
	{
		WriteStartXmlnsAttribute();
		if (prefix.Length != 0)
		{
			WriteByte(':');
			WritePrefix(prefix);
		}
		WriteBytes('=', '"');
		WriteEscapedText(ns);
		WriteEndAttribute();
	}

	public override void WriteXmlnsAttribute(string prefix, XmlDictionaryString ns)
	{
		WriteXmlnsAttribute(prefix, ns.Value);
	}

	public override void WriteXmlnsAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] nsBuffer, int nsOffset, int nsLength)
	{
		WriteStartXmlnsAttribute();
		if (prefixLength != 0)
		{
			WriteByte(':');
			WritePrefix(prefixBuffer, prefixOffset, prefixLength);
		}
		WriteBytes('=', '"');
		WriteEscapedText(nsBuffer, nsOffset, nsLength);
		WriteEndAttribute();
	}

	public override void WriteStartAttribute(string prefix, string localName)
	{
		WriteByte(' ');
		if (prefix.Length != 0)
		{
			WritePrefix(prefix);
			WriteByte(':');
		}
		WriteLocalName(localName);
		WriteBytes('=', '"');
		inAttribute = true;
	}

	public override void WriteStartAttribute(string prefix, XmlDictionaryString localName)
	{
		WriteStartAttribute(prefix, localName.Value);
	}

	public override void WriteStartAttribute(byte[] prefixBuffer, int prefixOffset, int prefixLength, byte[] localNameBuffer, int localNameOffset, int localNameLength)
	{
		WriteByte(' ');
		if (prefixLength != 0)
		{
			WritePrefix(prefixBuffer, prefixOffset, prefixLength);
			WriteByte(':');
		}
		WriteLocalName(localNameBuffer, localNameOffset, localNameLength);
		WriteBytes('=', '"');
		inAttribute = true;
	}

	public override void WriteEndAttribute()
	{
		WriteByte('"');
		inAttribute = false;
	}

	private void WritePrefix(string prefix)
	{
		if (prefix.Length == 1)
		{
			WriteUTF8Char(prefix[0]);
		}
		else
		{
			WriteUTF8Chars(prefix);
		}
	}

	private void WritePrefix(byte[] prefixBuffer, int prefixOffset, int prefixLength)
	{
		if (prefixLength == 1)
		{
			WriteUTF8Char(prefixBuffer[prefixOffset]);
		}
		else
		{
			WriteUTF8Chars(prefixBuffer, prefixOffset, prefixLength);
		}
	}

	private void WriteLocalName(string localName)
	{
		WriteUTF8Chars(localName);
	}

	private void WriteLocalName(byte[] localNameBuffer, int localNameOffset, int localNameLength)
	{
		WriteUTF8Chars(localNameBuffer, localNameOffset, localNameLength);
	}

	public override void WriteEscapedText(XmlDictionaryString s)
	{
		WriteEscapedText(s.Value);
	}

	[SecuritySafeCritical]
	public unsafe override void WriteEscapedText(string s)
	{
		int length = s.Length;
		if (length > 0)
		{
			fixed (char* ptr = s)
			{
				UnsafeWriteEscapedText(ptr, length);
			}
		}
	}

	[SecuritySafeCritical]
	public unsafe override void WriteEscapedText(char[] s, int offset, int count)
	{
		if (count > 0)
		{
			fixed (char* ptr = &s[offset])
			{
				UnsafeWriteEscapedText(ptr, count);
			}
		}
	}

	[SecurityCritical]
	private unsafe void UnsafeWriteEscapedText(char* chars, int count)
	{
		bool[] array = (inAttribute ? isEscapedAttributeChar : isEscapedElementChar);
		int num = array.Length;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			char c = chars[i];
			if ((c < num && array[(uint)c]) || c >= '\ufffe')
			{
				UnsafeWriteUTF8Chars(chars + num2, i - num2);
				WriteCharEntity(c);
				num2 = i + 1;
			}
		}
		UnsafeWriteUTF8Chars(chars + num2, count - num2);
	}

	public override void WriteEscapedText(byte[] chars, int offset, int count)
	{
		bool[] array = (inAttribute ? isEscapedAttributeChar : isEscapedElementChar);
		int num = array.Length;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			byte b = chars[offset + i];
			if (b < num && array[b])
			{
				WriteUTF8Chars(chars, offset + num2, i - num2);
				WriteCharEntity(b);
				num2 = i + 1;
			}
			else if (b == 239 && offset + i + 2 < count)
			{
				byte num3 = chars[offset + i + 1];
				byte b2 = chars[offset + i + 2];
				if (num3 == 191 && (b2 == 190 || b2 == 191))
				{
					WriteUTF8Chars(chars, offset + num2, i - num2);
					WriteCharEntity((b2 == 190) ? 65534 : 65535);
					num2 = i + 3;
				}
			}
		}
		WriteUTF8Chars(chars, offset + num2, count - num2);
	}

	public void WriteText(int ch)
	{
		WriteUTF8Char(ch);
	}

	public override void WriteText(byte[] chars, int offset, int count)
	{
		WriteUTF8Chars(chars, offset, count);
	}

	[SecuritySafeCritical]
	public unsafe override void WriteText(char[] chars, int offset, int count)
	{
		if (count > 0)
		{
			fixed (char* ptr = &chars[offset])
			{
				UnsafeWriteUTF8Chars(ptr, count);
			}
		}
	}

	public override void WriteText(string value)
	{
		WriteUTF8Chars(value);
	}

	public override void WriteText(XmlDictionaryString value)
	{
		WriteUTF8Chars(value.Value);
	}

	public void WriteLessThanCharEntity()
	{
		int num;
		byte[] array = GetBuffer(4, out num);
		array[num] = 38;
		array[num + 1] = 108;
		array[num + 2] = 116;
		array[num + 3] = 59;
		Advance(4);
	}

	public void WriteGreaterThanCharEntity()
	{
		int num;
		byte[] array = GetBuffer(4, out num);
		array[num] = 38;
		array[num + 1] = 103;
		array[num + 2] = 116;
		array[num + 3] = 59;
		Advance(4);
	}

	public void WriteAmpersandCharEntity()
	{
		int num;
		byte[] array = GetBuffer(5, out num);
		array[num] = 38;
		array[num + 1] = 97;
		array[num + 2] = 109;
		array[num + 3] = 112;
		array[num + 4] = 59;
		Advance(5);
	}

	public void WriteApostropheCharEntity()
	{
		int num;
		byte[] array = GetBuffer(6, out num);
		array[num] = 38;
		array[num + 1] = 97;
		array[num + 2] = 112;
		array[num + 3] = 111;
		array[num + 4] = 115;
		array[num + 5] = 59;
		Advance(6);
	}

	public void WriteQuoteCharEntity()
	{
		int num;
		byte[] array = GetBuffer(6, out num);
		array[num] = 38;
		array[num + 1] = 113;
		array[num + 2] = 117;
		array[num + 3] = 111;
		array[num + 4] = 116;
		array[num + 5] = 59;
		Advance(6);
	}

	private void WriteHexCharEntity(int ch)
	{
		byte[] charEntityBuffer = GetCharEntityBuffer();
		int num = 32;
		charEntityBuffer[--num] = 59;
		num -= ToBase16(charEntityBuffer, num, (uint)ch);
		charEntityBuffer[--num] = 120;
		charEntityBuffer[--num] = 35;
		charEntityBuffer[--num] = 38;
		WriteUTF8Chars(charEntityBuffer, num, 32 - num);
	}

	public override void WriteCharEntity(int ch)
	{
		switch (ch)
		{
		case 60:
			WriteLessThanCharEntity();
			break;
		case 62:
			WriteGreaterThanCharEntity();
			break;
		case 38:
			WriteAmpersandCharEntity();
			break;
		case 39:
			WriteApostropheCharEntity();
			break;
		case 34:
			WriteQuoteCharEntity();
			break;
		default:
			WriteHexCharEntity(ch);
			break;
		}
	}

	private int ToBase16(byte[] chars, int offset, uint value)
	{
		int num = 0;
		do
		{
			num++;
			chars[--offset] = digits[value & 0xF];
			value /= 16;
		}
		while (value != 0);
		return num;
	}

	public override void WriteBoolText(bool value)
	{
		int num;
		byte[] array = GetBuffer(5, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteDecimalText(decimal value)
	{
		int num;
		byte[] array = GetBuffer(40, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteDoubleText(double value)
	{
		int num;
		byte[] array = GetBuffer(32, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteFloatText(float value)
	{
		int num;
		byte[] array = GetBuffer(16, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteDateTimeText(DateTime value)
	{
		int num;
		byte[] array = GetBuffer(64, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteUniqueIdText(UniqueId value)
	{
		if (value.IsGuid)
		{
			int charArrayLength = value.CharArrayLength;
			char[] charBuffer = GetCharBuffer(charArrayLength);
			value.ToCharArray(charBuffer, 0);
			WriteText(charBuffer, 0, charArrayLength);
		}
		else
		{
			WriteEscapedText(value.ToString());
		}
	}

	public override void WriteInt32Text(int value)
	{
		int num;
		byte[] array = GetBuffer(16, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteInt64Text(long value)
	{
		int num;
		byte[] array = GetBuffer(32, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteUInt64Text(ulong value)
	{
		int num;
		byte[] array = GetBuffer(32, out num);
		Advance(XmlConverter.ToChars(value, array, num));
	}

	public override void WriteGuidText(Guid value)
	{
		WriteText(value.ToString());
	}

	public override void WriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count)
	{
		if (trailByteCount > 0)
		{
			InternalWriteBase64Text(trailBytes, 0, trailByteCount);
		}
		InternalWriteBase64Text(buffer, offset, count);
	}

	private void InternalWriteBase64Text(byte[] buffer, int offset, int count)
	{
		Base64Encoding base64Encoding = XmlConverter.Base64Encoding;
		while (count >= 3)
		{
			int num = Math.Min(384, count - count % 3);
			int count2 = num / 3 * 4;
			int charIndex;
			byte[] array = GetBuffer(count2, out charIndex);
			Advance(base64Encoding.GetChars(buffer, offset, num, array, charIndex));
			offset += num;
			count -= num;
		}
		if (count > 0)
		{
			int charIndex2;
			byte[] array2 = GetBuffer(4, out charIndex2);
			Advance(base64Encoding.GetChars(buffer, offset, count, array2, charIndex2));
		}
	}

	internal override AsyncCompletionResult WriteBase64TextAsync(AsyncEventArgs<XmlNodeWriterWriteBase64TextArgs> xmlNodeWriterState)
	{
		if (internalWriteBase64TextAsyncWriter == null)
		{
			internalWriteBase64TextAsyncWriter = new InternalWriteBase64TextAsyncWriter(this);
		}
		return internalWriteBase64TextAsyncWriter.StartAsync(xmlNodeWriterState);
	}

	public override IAsyncResult BeginWriteBase64Text(byte[] trailBytes, int trailByteCount, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return new WriteBase64TextAsyncResult(trailBytes, trailByteCount, buffer, offset, count, this, callback, state);
	}

	public override void EndWriteBase64Text(IAsyncResult result)
	{
		WriteBase64TextAsyncResult.End(result);
	}

	private IAsyncResult BeginInternalWriteBase64Text(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return new InternalWriteBase64TextAsyncResult(buffer, offset, count, this, callback, state);
	}

	private void EndInternalWriteBase64Text(IAsyncResult result)
	{
		InternalWriteBase64TextAsyncResult.End(result);
	}

	public override void WriteTimeSpanText(TimeSpan value)
	{
		WriteText(XmlConvert.ToString(value));
	}

	public override void WriteStartListText()
	{
	}

	public override void WriteListSeparator()
	{
		WriteByte(' ');
	}

	public override void WriteEndListText()
	{
	}

	public override void WriteQualifiedName(string prefix, XmlDictionaryString localName)
	{
		if (prefix.Length != 0)
		{
			WritePrefix(prefix);
			WriteByte(':');
		}
		WriteText(localName);
	}
}
