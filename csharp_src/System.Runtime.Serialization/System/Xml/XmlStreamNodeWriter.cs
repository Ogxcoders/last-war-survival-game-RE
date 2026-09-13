using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Xml;

internal abstract class XmlStreamNodeWriter : XmlNodeWriter
{
	private class GetBufferAsyncResult : AsyncResult
	{
		private XmlStreamNodeWriter writer;

		private int offset;

		private int count;

		private static AsyncCompletion onComplete = OnComplete;

		public GetBufferAsyncResult(int count, XmlStreamNodeWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.count = count;
			this.writer = writer;
			int num = writer.offset;
			bool flag = false;
			if (num + count <= 512)
			{
				offset = num;
				flag = true;
			}
			else
			{
				IAsyncResult result = writer.BeginFlushBuffer(PrepareAsyncCompletion(onComplete), this);
				flag = SyncContinue(result);
			}
			if (flag)
			{
				Complete(completedSynchronously: true);
			}
		}

		private static bool OnComplete(IAsyncResult result)
		{
			return ((GetBufferAsyncResult)result.AsyncState).HandleFlushBuffer(result);
		}

		private bool HandleFlushBuffer(IAsyncResult result)
		{
			writer.EndFlushBuffer(result);
			offset = 0;
			return true;
		}

		public static byte[] End(IAsyncResult result, out int offset)
		{
			GetBufferAsyncResult getBufferAsyncResult = AsyncResult.End<GetBufferAsyncResult>(result);
			offset = getBufferAsyncResult.offset;
			return getBufferAsyncResult.writer.buffer;
		}
	}

	private class WriteBytesAsyncResult : AsyncResult
	{
		private static AsyncCompletion onHandleGetBufferComplete = OnHandleGetBufferComplete;

		private static AsyncCompletion onHandleFlushBufferComplete = OnHandleFlushBufferComplete;

		private static AsyncCompletion onHandleWrite = OnHandleWrite;

		private byte[] byteBuffer;

		private int byteOffset;

		private int byteCount;

		private XmlStreamNodeWriter writer;

		public WriteBytesAsyncResult(byte[] byteBuffer, int byteOffset, int byteCount, XmlStreamNodeWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.byteBuffer = byteBuffer;
			this.byteOffset = byteOffset;
			this.byteCount = byteCount;
			this.writer = writer;
			bool flag = false;
			if ((byteCount >= 512) ? HandleFlushBuffer(null) : HandleGetBuffer(null))
			{
				Complete(completedSynchronously: true);
			}
		}

		private static bool OnHandleGetBufferComplete(IAsyncResult result)
		{
			return ((WriteBytesAsyncResult)result.AsyncState).HandleGetBuffer(result);
		}

		private static bool OnHandleFlushBufferComplete(IAsyncResult result)
		{
			return ((WriteBytesAsyncResult)result.AsyncState).HandleFlushBuffer(result);
		}

		private static bool OnHandleWrite(IAsyncResult result)
		{
			return ((WriteBytesAsyncResult)result.AsyncState).HandleWrite(result);
		}

		private bool HandleGetBuffer(IAsyncResult result)
		{
			if (result == null)
			{
				result = writer.BeginGetBuffer(byteCount, PrepareAsyncCompletion(onHandleGetBufferComplete), this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			int offset;
			byte[] dst = writer.EndGetBuffer(result, out offset);
			Buffer.BlockCopy(byteBuffer, byteOffset, dst, offset, byteCount);
			writer.Advance(byteCount);
			return true;
		}

		private bool HandleFlushBuffer(IAsyncResult result)
		{
			if (result == null)
			{
				result = writer.BeginFlushBuffer(PrepareAsyncCompletion(onHandleFlushBufferComplete), this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			writer.EndFlushBuffer(result);
			return HandleWrite(null);
		}

		private bool HandleWrite(IAsyncResult result)
		{
			if (result == null)
			{
				result = writer.stream.BeginWrite(byteBuffer, byteOffset, byteCount, PrepareAsyncCompletion(onHandleWrite), this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			writer.stream.EndWrite(result);
			return true;
		}

		public static void End(IAsyncResult result)
		{
			AsyncResult.End<WriteBytesAsyncResult>(result);
		}
	}

	private class FlushBufferAsyncResult : AsyncResult
	{
		private static AsyncCompletion onComplete = OnComplete;

		private XmlStreamNodeWriter writer;

		public FlushBufferAsyncResult(XmlStreamNodeWriter writer, AsyncCallback callback, object state)
			: base(callback, state)
		{
			this.writer = writer;
			bool flag = true;
			if (writer.offset != 0)
			{
				flag = HandleFlushBuffer(null);
			}
			if (flag)
			{
				Complete(completedSynchronously: true);
			}
		}

		private static bool OnComplete(IAsyncResult result)
		{
			return ((FlushBufferAsyncResult)result.AsyncState).HandleFlushBuffer(result);
		}

		private bool HandleFlushBuffer(IAsyncResult result)
		{
			if (result == null)
			{
				result = writer.stream.BeginWrite(writer.buffer, 0, writer.offset, PrepareAsyncCompletion(onComplete), this);
				if (!result.CompletedSynchronously)
				{
					return false;
				}
			}
			writer.stream.EndWrite(result);
			writer.offset = 0;
			return true;
		}

		public static void End(IAsyncResult result)
		{
			AsyncResult.End<FlushBufferAsyncResult>(result);
		}
	}

	internal class GetBufferArgs
	{
		public int Count { get; set; }
	}

	internal class GetBufferEventResult
	{
		internal byte[] Buffer { get; set; }

		internal int Offset { get; set; }
	}

	internal class GetBufferAsyncEventArgs : AsyncEventArgs<GetBufferArgs, GetBufferEventResult>
	{
	}

	private Stream stream;

	private byte[] buffer;

	private int offset;

	private bool ownsStream;

	private const int bufferLength = 512;

	private const int maxEntityLength = 32;

	private const int maxBytesPerChar = 3;

	private Encoding encoding;

	private int hasPendingWrite;

	private AsyncEventArgs<object> flushBufferState;

	private static UTF8Encoding UTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

	private static AsyncCallback onFlushBufferComplete;

	private static AsyncEventArgsCallback onGetFlushComplete;

	public Stream Stream
	{
		get
		{
			return stream;
		}
		set
		{
			stream = value;
		}
	}

	public byte[] StreamBuffer => buffer;

	public int BufferOffset => offset;

	public int Position => (int)stream.Position + offset;

	protected XmlStreamNodeWriter()
	{
		buffer = new byte[512];
		encoding = UTF8Encoding;
	}

	protected void SetOutput(Stream stream, bool ownsStream, Encoding encoding)
	{
		this.stream = stream;
		this.ownsStream = ownsStream;
		offset = 0;
		if (encoding != null)
		{
			this.encoding = encoding;
		}
	}

	protected byte[] GetBuffer(int count, out int offset)
	{
		int num = this.offset;
		if (num + count <= 512)
		{
			offset = num;
		}
		else
		{
			FlushBuffer();
			offset = 0;
		}
		return buffer;
	}

	internal AsyncCompletionResult GetBufferAsync(GetBufferAsyncEventArgs getBufferState)
	{
		int count = getBufferState.Arguments.Count;
		int num = 0;
		int num2 = offset;
		if (num2 + count <= 512)
		{
			num = num2;
		}
		else
		{
			if (onGetFlushComplete == null)
			{
				onGetFlushComplete = GetBufferFlushComplete;
			}
			if (flushBufferState == null)
			{
				flushBufferState = new AsyncEventArgs<object>();
			}
			flushBufferState.Set(onGetFlushComplete, getBufferState, this);
			if (FlushBufferAsync(flushBufferState) != AsyncCompletionResult.Completed)
			{
				return AsyncCompletionResult.Queued;
			}
			num = 0;
			flushBufferState.Complete(completedSynchronously: true);
		}
		getBufferState.Result = getBufferState.Result ?? new GetBufferEventResult();
		getBufferState.Result.Buffer = buffer;
		getBufferState.Result.Offset = num;
		return AsyncCompletionResult.Completed;
	}

	private static void GetBufferFlushComplete(IAsyncEventArgs completionState)
	{
		XmlStreamNodeWriter xmlStreamNodeWriter = (XmlStreamNodeWriter)completionState.AsyncState;
		GetBufferAsyncEventArgs obj = (GetBufferAsyncEventArgs)xmlStreamNodeWriter.flushBufferState.Arguments;
		obj.Result = obj.Result ?? new GetBufferEventResult();
		obj.Result.Buffer = xmlStreamNodeWriter.buffer;
		obj.Result.Offset = 0;
		obj.Complete(completedSynchronously: false, completionState.Exception);
	}

	private AsyncCompletionResult FlushBufferAsync(AsyncEventArgs<object> state)
	{
		if (Interlocked.CompareExchange(ref hasPendingWrite, 1, 0) != 0)
		{
			throw FxTrace.Exception.AsError(new InvalidOperationException(SR.GetString("Flush buffer is already in use.")));
		}
		if (offset != 0)
		{
			if (onFlushBufferComplete == null)
			{
				onFlushBufferComplete = OnFlushBufferCompete;
			}
			IAsyncResult asyncResult = stream.BeginWrite(buffer, 0, offset, onFlushBufferComplete, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return AsyncCompletionResult.Queued;
			}
			stream.EndWrite(asyncResult);
			offset = 0;
		}
		if (Interlocked.CompareExchange(ref hasPendingWrite, 0, 1) != 1)
		{
			throw FxTrace.Exception.AsError(new InvalidOperationException(SR.GetString("No async write operation is pending.")));
		}
		return AsyncCompletionResult.Completed;
	}

	private static void OnFlushBufferCompete(IAsyncResult result)
	{
		if (result.CompletedSynchronously)
		{
			return;
		}
		XmlStreamNodeWriter xmlStreamNodeWriter = (XmlStreamNodeWriter)result.AsyncState;
		Exception exception = null;
		try
		{
			xmlStreamNodeWriter.stream.EndWrite(result);
			xmlStreamNodeWriter.offset = 0;
			if (Interlocked.CompareExchange(ref xmlStreamNodeWriter.hasPendingWrite, 0, 1) != 1)
			{
				throw FxTrace.Exception.AsError(new InvalidOperationException(SR.GetString("No async write operation is pending.")));
			}
		}
		catch (Exception ex)
		{
			if (Fx.IsFatal(ex))
			{
				throw;
			}
			exception = ex;
		}
		xmlStreamNodeWriter.flushBufferState.Complete(completedSynchronously: false, exception);
	}

	protected IAsyncResult BeginGetBuffer(int count, AsyncCallback callback, object state)
	{
		return new GetBufferAsyncResult(count, this, callback, state);
	}

	protected byte[] EndGetBuffer(IAsyncResult result, out int offset)
	{
		return GetBufferAsyncResult.End(result, out offset);
	}

	protected void Advance(int count)
	{
		offset += count;
	}

	private void EnsureByte()
	{
		if (offset >= 512)
		{
			FlushBuffer();
		}
	}

	protected void WriteByte(byte b)
	{
		EnsureByte();
		buffer[offset++] = b;
	}

	protected void WriteByte(char ch)
	{
		WriteByte((byte)ch);
	}

	protected void WriteBytes(byte b1, byte b2)
	{
		byte[] array = buffer;
		int num = offset;
		if (num + 1 >= 512)
		{
			FlushBuffer();
			num = 0;
		}
		array[num] = b1;
		array[num + 1] = b2;
		offset += 2;
	}

	protected void WriteBytes(char ch1, char ch2)
	{
		WriteBytes((byte)ch1, (byte)ch2);
	}

	public void WriteBytes(byte[] byteBuffer, int byteOffset, int byteCount)
	{
		if (byteCount < 512)
		{
			int dstOffset;
			byte[] dst = GetBuffer(byteCount, out dstOffset);
			Buffer.BlockCopy(byteBuffer, byteOffset, dst, dstOffset, byteCount);
			Advance(byteCount);
		}
		else
		{
			FlushBuffer();
			stream.Write(byteBuffer, byteOffset, byteCount);
		}
	}

	public IAsyncResult BeginWriteBytes(byte[] byteBuffer, int byteOffset, int byteCount, AsyncCallback callback, object state)
	{
		return new WriteBytesAsyncResult(byteBuffer, byteOffset, byteCount, this, callback, state);
	}

	public void EndWriteBytes(IAsyncResult result)
	{
		WriteBytesAsyncResult.End(result);
	}

	[SecurityCritical]
	protected unsafe void UnsafeWriteBytes(byte* bytes, int byteCount)
	{
		FlushBuffer();
		byte[] array = buffer;
		while (byteCount > 512)
		{
			for (int i = 0; i < 512; i++)
			{
				array[i] = bytes[i];
			}
			stream.Write(array, 0, 512);
			bytes += 512;
			byteCount -= 512;
		}
		if (byteCount > 0)
		{
			for (int j = 0; j < byteCount; j++)
			{
				array[j] = bytes[j];
			}
			stream.Write(array, 0, byteCount);
		}
	}

	[SecuritySafeCritical]
	protected unsafe void WriteUTF8Char(int ch)
	{
		if (ch < 128)
		{
			WriteByte((byte)ch);
		}
		else if (ch <= 65535)
		{
			char* ptr = stackalloc char[1];
			*ptr = (char)ch;
			UnsafeWriteUTF8Chars(ptr, 1);
		}
		else
		{
			SurrogateChar surrogateChar = new SurrogateChar(ch);
			char* ptr2 = stackalloc char[2];
			*ptr2 = surrogateChar.HighChar;
			ptr2[1] = surrogateChar.LowChar;
			UnsafeWriteUTF8Chars(ptr2, 2);
		}
	}

	protected void WriteUTF8Chars(byte[] chars, int charOffset, int charCount)
	{
		if (charCount < 512)
		{
			int dstOffset;
			byte[] dst = GetBuffer(charCount, out dstOffset);
			Buffer.BlockCopy(chars, charOffset, dst, dstOffset, charCount);
			Advance(charCount);
		}
		else
		{
			FlushBuffer();
			stream.Write(chars, charOffset, charCount);
		}
	}

	[SecuritySafeCritical]
	protected unsafe void WriteUTF8Chars(string value)
	{
		int length = value.Length;
		if (length > 0)
		{
			fixed (char* chars = value)
			{
				UnsafeWriteUTF8Chars(chars, length);
			}
		}
	}

	[SecurityCritical]
	protected unsafe void UnsafeWriteUTF8Chars(char* chars, int charCount)
	{
		while (charCount > 170)
		{
			int num = 170;
			if ((chars[num - 1] & 0xFC00) == 55296)
			{
				num--;
			}
			int num2;
			byte[] array = GetBuffer(num * 3, out num2);
			Advance(UnsafeGetUTF8Chars(chars, num, array, num2));
			charCount -= num;
			chars += num;
		}
		if (charCount > 0)
		{
			int num3;
			byte[] array2 = GetBuffer(charCount * 3, out num3);
			Advance(UnsafeGetUTF8Chars(chars, charCount, array2, num3));
		}
	}

	[SecurityCritical]
	protected unsafe void UnsafeWriteUnicodeChars(char* chars, int charCount)
	{
		while (charCount > 256)
		{
			int num = 256;
			if ((chars[num - 1] & 0xFC00) == 55296)
			{
				num--;
			}
			int num2;
			byte[] array = GetBuffer(num * 2, out num2);
			Advance(UnsafeGetUnicodeChars(chars, num, array, num2));
			charCount -= num;
			chars += num;
		}
		if (charCount > 0)
		{
			int num3;
			byte[] array2 = GetBuffer(charCount * 2, out num3);
			Advance(UnsafeGetUnicodeChars(chars, charCount, array2, num3));
		}
	}

	[SecurityCritical]
	protected unsafe int UnsafeGetUnicodeChars(char* chars, int charCount, byte[] buffer, int offset)
	{
		char* ptr = chars + charCount;
		while (chars < ptr)
		{
			char c = *(chars++);
			buffer[offset++] = (byte)c;
			c = (char)((int)c >> 8);
			buffer[offset++] = (byte)c;
		}
		return charCount * 2;
	}

	[SecurityCritical]
	protected unsafe int UnsafeGetUTF8Length(char* chars, int charCount)
	{
		char* ptr = chars + charCount;
		while (chars < ptr && *chars < '\u0080')
		{
			chars++;
		}
		if (chars == ptr)
		{
			return charCount;
		}
		return (int)(chars - (ptr - charCount)) + encoding.GetByteCount(chars, (int)(ptr - chars));
	}

	[SecurityCritical]
	protected unsafe int UnsafeGetUTF8Chars(char* chars, int charCount, byte[] buffer, int offset)
	{
		if (charCount > 0)
		{
			fixed (byte* ptr = &buffer[offset])
			{
				byte* ptr2 = ptr;
				byte* ptr3 = ptr2 + (buffer.Length - offset);
				char* ptr4 = chars + charCount;
				do
				{
					IL_0045:
					if (chars < ptr4)
					{
						char c = *chars;
						if (c < '\u0080')
						{
							*ptr2 = (byte)c;
							ptr2++;
							chars++;
							goto IL_0045;
						}
					}
					if (chars >= ptr4)
					{
						break;
					}
					char* ptr5 = chars;
					while (chars < ptr4 && *chars >= '\u0080')
					{
						chars++;
					}
					ptr2 += encoding.GetBytes(ptr5, (int)(chars - ptr5), ptr2, (int)(ptr3 - ptr2));
				}
				while (chars < ptr4);
				return (int)(ptr2 - ptr);
			}
		}
		return 0;
	}

	protected virtual void FlushBuffer()
	{
		if (offset != 0)
		{
			stream.Write(buffer, 0, offset);
			offset = 0;
		}
	}

	protected virtual IAsyncResult BeginFlushBuffer(AsyncCallback callback, object state)
	{
		return new FlushBufferAsyncResult(this, callback, state);
	}

	protected virtual void EndFlushBuffer(IAsyncResult result)
	{
		FlushBufferAsyncResult.End(result);
	}

	public override void Flush()
	{
		FlushBuffer();
		stream.Flush();
	}

	public override void Close()
	{
		if (stream != null)
		{
			if (ownsStream)
			{
				stream.Close();
			}
			stream = null;
		}
	}
}
