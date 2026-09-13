using System.Threading.Tasks;

namespace System.Xml;

internal static class BinHexEncoder
{
	private const string s_hexDigits = "0123456789ABCDEF";

	private const int CharsChunkSize = 128;

	internal static void Encode(byte[] buffer, int index, int count, XmlWriter writer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > buffer.Length - index)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		char[] array = new char[(count * 2 < 128) ? (count * 2) : 128];
		int num = index + count;
		while (index < num)
		{
			int num2 = ((count < 64) ? count : 64);
			int count2 = Encode(buffer, index, num2, array);
			writer.WriteRaw(array, 0, count2);
			index += num2;
			count -= num2;
		}
	}

	internal static string Encode(byte[] inArray, int offsetIn, int count)
	{
		if (inArray == null)
		{
			throw new ArgumentNullException("inArray");
		}
		if (0 > offsetIn)
		{
			throw new ArgumentOutOfRangeException("offsetIn");
		}
		if (0 > count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > inArray.Length - offsetIn)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		char[] array = new char[2 * count];
		int length = Encode(inArray, offsetIn, count, array);
		return new string(array, 0, length);
	}

	private static int Encode(byte[] inArray, int offsetIn, int count, char[] outArray)
	{
		int num = 0;
		int num2 = 0;
		int num3 = outArray.Length;
		for (int i = 0; i < count; i++)
		{
			byte b = inArray[offsetIn++];
			outArray[num++] = "0123456789ABCDEF"[b >> 4];
			if (num == num3)
			{
				break;
			}
			outArray[num++] = "0123456789ABCDEF"[b & 0xF];
			if (num == num3)
			{
				break;
			}
		}
		return num - num2;
	}

	internal static async Task EncodeAsync(byte[] buffer, int index, int count, XmlWriter writer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (index < 0)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		if (count > buffer.Length - index)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		char[] chars = new char[(count * 2 < 128) ? (count * 2) : 128];
		int endIndex = index + count;
		while (index < endIndex)
		{
			int cnt = ((count < 64) ? count : 64);
			int count2 = Encode(buffer, index, cnt, chars);
			await writer.WriteRawAsync(chars, 0, count2).ConfigureAwait(continueOnCapturedContext: false);
			index += cnt;
			count -= cnt;
		}
	}
}
