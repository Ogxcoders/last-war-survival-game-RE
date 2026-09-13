using System.IO;
using System.IO.Compression;
using System.Text;

namespace FibMatrix;

public class StringArrayCompressor
{
	public static string Compress(string original)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(original);
		using MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionLevel.Optimal))
		{
			deflateStream.Write(bytes, 0, bytes.Length);
		}
		return Ascii85.Encode(memoryStream.ToArray());
	}

	public static string Decompress(string compressed)
	{
		using MemoryStream stream = new MemoryStream(Ascii85.Decode(compressed));
		using DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
		using MemoryStream memoryStream = new MemoryStream();
		deflateStream.CopyTo(memoryStream);
		byte[] bytes = memoryStream.ToArray();
		return Encoding.UTF8.GetString(bytes);
	}
}
