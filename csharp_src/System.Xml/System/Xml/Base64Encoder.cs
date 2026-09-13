using System.Threading.Tasks;

namespace System.Xml;

internal abstract class Base64Encoder
{
	private byte[] leftOverBytes;

	private int leftOverBytesCount;

	private char[] charsLine;

	internal const int Base64LineSize = 76;

	internal const int LineSizeInBytes = 57;

	internal Base64Encoder()
	{
		charsLine = new char[76];
	}

	internal abstract void WriteChars(char[] chars, int index, int count);

	internal void Encode(byte[] buffer, int index, int count)
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
		if (leftOverBytesCount > 0)
		{
			int num = leftOverBytesCount;
			while (num < 3 && count > 0)
			{
				leftOverBytes[num++] = buffer[index++];
				count--;
			}
			if (count == 0 && num < 3)
			{
				leftOverBytesCount = num;
				return;
			}
			int count2 = Convert.ToBase64CharArray(leftOverBytes, 0, 3, charsLine, 0);
			WriteChars(charsLine, 0, count2);
		}
		leftOverBytesCount = count % 3;
		if (leftOverBytesCount > 0)
		{
			count -= leftOverBytesCount;
			if (leftOverBytes == null)
			{
				leftOverBytes = new byte[3];
			}
			for (int i = 0; i < leftOverBytesCount; i++)
			{
				leftOverBytes[i] = buffer[index + count + i];
			}
		}
		int num2 = index + count;
		int num3 = 57;
		while (index < num2)
		{
			if (index + num3 > num2)
			{
				num3 = num2 - index;
			}
			int count3 = Convert.ToBase64CharArray(buffer, index, num3, charsLine, 0);
			WriteChars(charsLine, 0, count3);
			index += num3;
		}
	}

	internal void Flush()
	{
		if (leftOverBytesCount > 0)
		{
			int count = Convert.ToBase64CharArray(leftOverBytes, 0, leftOverBytesCount, charsLine, 0);
			WriteChars(charsLine, 0, count);
			leftOverBytesCount = 0;
		}
	}

	internal abstract Task WriteCharsAsync(char[] chars, int index, int count);

	internal async Task EncodeAsync(byte[] buffer, int index, int count)
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
		if (leftOverBytesCount > 0)
		{
			int num = leftOverBytesCount;
			while (num < 3 && count > 0)
			{
				leftOverBytes[num++] = buffer[index++];
				count--;
			}
			if (count == 0 && num < 3)
			{
				leftOverBytesCount = num;
				return;
			}
			int count2 = Convert.ToBase64CharArray(leftOverBytes, 0, 3, charsLine, 0);
			await WriteCharsAsync(charsLine, 0, count2).ConfigureAwait(continueOnCapturedContext: false);
		}
		leftOverBytesCount = count % 3;
		if (leftOverBytesCount > 0)
		{
			count -= leftOverBytesCount;
			if (leftOverBytes == null)
			{
				leftOverBytes = new byte[3];
			}
			for (int i = 0; i < leftOverBytesCount; i++)
			{
				leftOverBytes[i] = buffer[index + count + i];
			}
		}
		int endIndex = index + count;
		int chunkSize = 57;
		while (index < endIndex)
		{
			if (index + chunkSize > endIndex)
			{
				chunkSize = endIndex - index;
			}
			int count3 = Convert.ToBase64CharArray(buffer, index, chunkSize, charsLine, 0);
			await WriteCharsAsync(charsLine, 0, count3).ConfigureAwait(continueOnCapturedContext: false);
			index += chunkSize;
		}
	}

	internal async Task FlushAsync()
	{
		if (leftOverBytesCount > 0)
		{
			int count = Convert.ToBase64CharArray(leftOverBytes, 0, leftOverBytesCount, charsLine, 0);
			await WriteCharsAsync(charsLine, 0, count).ConfigureAwait(continueOnCapturedContext: false);
			leftOverBytesCount = 0;
		}
	}
}
