using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class MimeReader
{
	private static byte[] CRLFCRLF = new byte[4] { 13, 10, 13, 10 };

	private byte[] boundaryBytes;

	private string content;

	private Stream currentStream;

	private MimeHeaderReader mimeHeaderReader;

	private DelimittedStreamReader reader;

	private byte[] scratch = new byte[2];

	public string Preface
	{
		get
		{
			if (content == null)
			{
				Stream nextStream = reader.GetNextStream(boundaryBytes);
				content = new StreamReader(nextStream, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, 256).ReadToEnd();
				nextStream.Close();
				if (content == null)
				{
					content = string.Empty;
				}
			}
			return content;
		}
	}

	public MimeReader(Stream stream, string boundary)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (boundary == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("boundary");
		}
		reader = new DelimittedStreamReader(stream);
		boundaryBytes = MimeWriter.GetBoundaryBytes(boundary);
		reader.Push(boundaryBytes, 0, 2);
	}

	public void Close()
	{
		reader.Close();
	}

	public Stream GetContentStream()
	{
		mimeHeaderReader.Close();
		return reader.GetNextStream(boundaryBytes);
	}

	public bool ReadNextPart()
	{
		_ = Preface;
		if (currentStream != null)
		{
			currentStream.Close();
			currentStream = null;
		}
		Stream nextStream = reader.GetNextStream(CRLFCRLF);
		if (nextStream == null)
		{
			return false;
		}
		if (BlockRead(nextStream, scratch, 0, 2) == 2)
		{
			if (scratch[0] == 13 && scratch[1] == 10)
			{
				if (mimeHeaderReader == null)
				{
					mimeHeaderReader = new MimeHeaderReader(nextStream);
				}
				else
				{
					mimeHeaderReader.Reset(nextStream);
				}
				return true;
			}
			if (scratch[0] == 45 && scratch[1] == 45 && (BlockRead(nextStream, scratch, 0, 2) < 2 || (scratch[0] == 13 && scratch[1] == 10)))
			{
				return false;
			}
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME parts are truncated.")));
	}

	public MimeHeaders ReadHeaders(int maxBuffer, ref int remaining)
	{
		MimeHeaders mimeHeaders = new MimeHeaders();
		while (mimeHeaderReader.Read(maxBuffer, ref remaining))
		{
			mimeHeaders.Add(mimeHeaderReader.Name, mimeHeaderReader.Value, ref remaining);
		}
		return mimeHeaders;
	}

	private int BlockRead(Stream stream, byte[] buffer, int offset, int count)
	{
		int num = 0;
		do
		{
			int num2 = stream.Read(buffer, offset + num, count - num);
			if (num2 == 0)
			{
				break;
			}
			num += num2;
		}
		while (num < count);
		return num;
	}
}
