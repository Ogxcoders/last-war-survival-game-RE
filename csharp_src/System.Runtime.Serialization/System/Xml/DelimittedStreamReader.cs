using System.IO;
using System.Runtime.Serialization;

namespace System.Xml;

internal class DelimittedStreamReader
{
	private enum MatchState
	{
		True,
		False,
		InsufficientData
	}

	private class DelimittedReadStream : Stream
	{
		private DelimittedStreamReader reader;

		public override bool CanRead => true;

		public override bool CanSeek => false;

		public override bool CanWrite => false;

		public override long Length
		{
			get
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Seek operation is not supported on this Stream.", GetType().FullName)));
			}
		}

		public override long Position
		{
			get
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Seek operation is not supported on this Stream.", GetType().FullName)));
			}
			set
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Seek operation is not supported on this Stream.", GetType().FullName)));
			}
		}

		public DelimittedReadStream(DelimittedStreamReader reader)
		{
			if (reader == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("reader");
			}
			this.reader = reader;
		}

		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Write operation is not supported on this '{0}' Stream.", GetType().FullName)));
		}

		public override void Close()
		{
			reader.Close(this);
		}

		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Write operation is not supported on this '{0}' Stream.", GetType().FullName)));
		}

		public override void Flush()
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Write operation is not supported on this '{0}' Stream.", GetType().FullName)));
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", buffer.Length)));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", buffer.Length - offset)));
			}
			return reader.Read(this, buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Seek operation is not supported on this Stream.", GetType().FullName)));
		}

		public override void SetLength(long value)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Write operation is not supported on this '{0}' Stream.", GetType().FullName)));
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Write operation is not supported on this '{0}' Stream.", GetType().FullName)));
		}
	}

	private bool canGetNextStream = true;

	private DelimittedReadStream currentStream;

	private byte[] delimitter;

	private byte[] matchBuffer;

	private byte[] scratch;

	private BufferedReadStream stream;

	public DelimittedStreamReader(Stream stream)
	{
		this.stream = new BufferedReadStream(stream);
	}

	public void Close()
	{
		stream.Close();
	}

	private void Close(DelimittedReadStream caller)
	{
		if (currentStream != caller)
		{
			return;
		}
		if (delimitter == null)
		{
			stream.Close();
		}
		else
		{
			if (scratch == null)
			{
				scratch = new byte[1024];
			}
			while (Read(caller, scratch, 0, scratch.Length) != 0)
			{
			}
		}
		currentStream = null;
	}

	public Stream GetNextStream(byte[] delimitter)
	{
		if (currentStream != null)
		{
			currentStream.Close();
			currentStream = null;
		}
		if (!canGetNextStream)
		{
			return null;
		}
		this.delimitter = delimitter;
		canGetNextStream = delimitter != null;
		currentStream = new DelimittedReadStream(this);
		return currentStream;
	}

	private MatchState MatchDelimitter(byte[] buffer, int start, int end)
	{
		if (delimitter.Length > end - start)
		{
			for (int num = end - start - 1; num >= 1; num--)
			{
				if (buffer[start + num] != delimitter[num])
				{
					return MatchState.False;
				}
			}
			return MatchState.InsufficientData;
		}
		for (int num2 = delimitter.Length - 1; num2 >= 1; num2--)
		{
			if (buffer[start + num2] != delimitter[num2])
			{
				return MatchState.False;
			}
		}
		return MatchState.True;
	}

	private int ProcessRead(byte[] buffer, int offset, int read)
	{
		if (read == 0)
		{
			return read;
		}
		int i = offset;
		for (int num = offset + read; i < num; i++)
		{
			if (buffer[i] != delimitter[0])
			{
				continue;
			}
			switch (MatchDelimitter(buffer, i, num))
			{
			case MatchState.True:
			{
				int result = i - offset;
				i += delimitter.Length;
				stream.Push(buffer, i, num - i);
				currentStream = null;
				return result;
			}
			case MatchState.InsufficientData:
			{
				int num2 = i - offset;
				if (num2 > 0)
				{
					stream.Push(buffer, i, num - i);
					return num2;
				}
				return -1;
			}
			}
		}
		return read;
	}

	private int Read(DelimittedReadStream caller, byte[] buffer, int offset, int count)
	{
		if (currentStream != caller)
		{
			return 0;
		}
		int num = stream.Read(buffer, offset, count);
		if (num == 0)
		{
			canGetNextStream = false;
			currentStream = null;
			return num;
		}
		if (delimitter == null)
		{
			return num;
		}
		int num2 = ProcessRead(buffer, offset, num);
		if (num2 < 0)
		{
			if (matchBuffer == null || matchBuffer.Length < delimitter.Length - num)
			{
				matchBuffer = new byte[delimitter.Length - num];
			}
			int count2 = stream.ReadBlock(matchBuffer, 0, delimitter.Length - num);
			if (MatchRemainder(num, count2))
			{
				currentStream = null;
				num2 = 0;
			}
			else
			{
				stream.Push(matchBuffer, 0, count2);
				int i;
				for (i = 1; i < num && buffer[i] != delimitter[0]; i++)
				{
				}
				if (i < num)
				{
					stream.Push(buffer, offset + i, num - i);
				}
				num2 = i;
			}
		}
		return num2;
	}

	private bool MatchRemainder(int start, int count)
	{
		if (start + count != delimitter.Length)
		{
			return false;
		}
		for (count--; count >= 0; count--)
		{
			if (delimitter[start + count] != matchBuffer[count])
			{
				return false;
			}
		}
		return true;
	}

	internal void Push(byte[] buffer, int offset, int count)
	{
		stream.Push(buffer, offset, count);
	}
}
