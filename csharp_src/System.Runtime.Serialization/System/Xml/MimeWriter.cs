using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class MimeWriter
{
	private Stream stream;

	private byte[] boundaryBytes;

	private MimeWriterState state;

	private BufferedWrite bufferedWrite;

	private Stream contentStream;

	internal MimeWriterState WriteState => state;

	internal MimeWriter(Stream stream, string boundary)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (boundary == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("boundary");
		}
		this.stream = stream;
		boundaryBytes = GetBoundaryBytes(boundary);
		state = MimeWriterState.Start;
		bufferedWrite = new BufferedWrite();
	}

	internal static int GetHeaderSize(string name, string value, int maxSizeInBytes)
	{
		if (name == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		int num = XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, 0, MimeGlobals.COLONSPACE.Length + MimeGlobals.CRLF.Length);
		num += XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, name.Length);
		return num + XmlMtomWriter.ValidateSizeOfMessage(maxSizeInBytes, num, value.Length);
	}

	internal static byte[] GetBoundaryBytes(string boundary)
	{
		byte[] array = new byte[boundary.Length + MimeGlobals.BoundaryPrefix.Length];
		for (int i = 0; i < MimeGlobals.BoundaryPrefix.Length; i++)
		{
			array[i] = MimeGlobals.BoundaryPrefix[i];
		}
		Encoding.ASCII.GetBytes(boundary, 0, boundary.Length, array, MimeGlobals.BoundaryPrefix.Length);
		return array;
	}

	internal int GetBoundarySize()
	{
		return boundaryBytes.Length;
	}

	internal void StartPreface()
	{
		if (state != MimeWriterState.Start)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("MIME writer is at invalid state for starting preface.", state.ToString())));
		}
		state = MimeWriterState.StartPreface;
	}

	internal void StartPart()
	{
		MimeWriterState mimeWriterState = state;
		if (mimeWriterState == MimeWriterState.StartPart || mimeWriterState == MimeWriterState.Closed)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("MIME writer is at invalid state for starting a part.", state.ToString())));
		}
		state = MimeWriterState.StartPart;
		if (contentStream != null)
		{
			contentStream.Flush();
			contentStream = null;
		}
		bufferedWrite.Write(boundaryBytes);
		bufferedWrite.Write(MimeGlobals.CRLF);
	}

	internal void Close()
	{
		if (state == MimeWriterState.Closed)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("MIME writer is at invalid state for closing.", state.ToString())));
		}
		state = MimeWriterState.Closed;
		if (contentStream != null)
		{
			contentStream.Flush();
			contentStream = null;
		}
		bufferedWrite.Write(boundaryBytes);
		bufferedWrite.Write(MimeGlobals.DASHDASH);
		bufferedWrite.Write(MimeGlobals.CRLF);
		Flush();
	}

	private void Flush()
	{
		if (bufferedWrite.Length > 0)
		{
			stream.Write(bufferedWrite.GetBuffer(), 0, bufferedWrite.Length);
			bufferedWrite.Reset();
		}
	}

	internal void WriteHeader(string name, string value)
	{
		if (name == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		MimeWriterState mimeWriterState = state;
		if (mimeWriterState == MimeWriterState.Start || (uint)(mimeWriterState - 4) <= 1u)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("MIME writer is at invalid state for header.", state.ToString())));
		}
		state = MimeWriterState.Header;
		bufferedWrite.Write(name);
		bufferedWrite.Write(MimeGlobals.COLONSPACE);
		bufferedWrite.Write(value);
		bufferedWrite.Write(MimeGlobals.CRLF);
	}

	internal Stream GetContentStream()
	{
		MimeWriterState mimeWriterState = state;
		if (mimeWriterState == MimeWriterState.Start || (uint)(mimeWriterState - 4) <= 1u)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("MIME writer is at invalid state for content.", state.ToString())));
		}
		state = MimeWriterState.Content;
		bufferedWrite.Write(MimeGlobals.CRLF);
		Flush();
		contentStream = stream;
		return contentStream;
	}
}
