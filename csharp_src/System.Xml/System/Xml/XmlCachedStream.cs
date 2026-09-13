using System.IO;

namespace System.Xml;

internal class XmlCachedStream : MemoryStream
{
	private const int MoveBufferSize = 4096;

	private Uri uri;

	internal XmlCachedStream(Uri uri, Stream stream)
	{
		this.uri = uri;
		try
		{
			byte[] buffer = new byte[4096];
			int num = 0;
			while ((num = stream.Read(buffer, 0, 4096)) > 0)
			{
				Write(buffer, 0, num);
			}
			base.Position = 0L;
		}
		finally
		{
			stream.Close();
		}
	}
}
