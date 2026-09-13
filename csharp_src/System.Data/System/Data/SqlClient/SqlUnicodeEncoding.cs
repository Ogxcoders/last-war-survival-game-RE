using System.Text;

namespace System.Data.SqlClient;

internal sealed class SqlUnicodeEncoding : UnicodeEncoding
{
	private sealed class SqlUnicodeDecoder : Decoder
	{
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return count / 2;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			Convert(bytes, byteIndex, byteCount, chars, charIndex, chars.Length - charIndex, flush: true, out var _, out var charsUsed, out var _);
			return charsUsed;
		}

		public override void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
		{
			charsUsed = Math.Min(charCount, byteCount / 2);
			bytesUsed = charsUsed * 2;
			completed = bytesUsed == byteCount;
			Buffer.BlockCopy(bytes, byteIndex, chars, charIndex * 2, bytesUsed);
		}
	}

	private static SqlUnicodeEncoding s_singletonEncoding = new SqlUnicodeEncoding();

	public static Encoding SqlUnicodeEncodingInstance => s_singletonEncoding;

	private SqlUnicodeEncoding()
		: base(bigEndian: false, byteOrderMark: false, throwOnInvalidBytes: false)
	{
	}

	public override Decoder GetDecoder()
	{
		return new SqlUnicodeDecoder();
	}

	public override int GetMaxByteCount(int charCount)
	{
		return charCount * 2;
	}
}
