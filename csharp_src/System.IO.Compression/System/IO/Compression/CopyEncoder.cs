namespace System.IO.Compression;

internal sealed class CopyEncoder
{
	private const int PaddingSize = 5;

	private const int MaxUncompressedBlockSize = 65536;

	public void GetBlock(DeflateInput input, OutputBuffer output, bool isFinal)
	{
		int num = 0;
		if (input != null)
		{
			num = Math.Min(input.Count, output.FreeBytes - 5 - output.BitsInBuffer);
			if (num > 65531)
			{
				num = 65531;
			}
		}
		if (isFinal)
		{
			output.WriteBits(3, 1u);
		}
		else
		{
			output.WriteBits(3, 0u);
		}
		output.FlushBits();
		WriteLenNLen((ushort)num, output);
		if (input != null && num > 0)
		{
			output.WriteBytes(input.Buffer, input.StartIndex, num);
			input.ConsumeBytes(num);
		}
	}

	private void WriteLenNLen(ushort len, OutputBuffer output)
	{
		output.WriteUInt16(len);
		ushort value = (ushort)(~len);
		output.WriteUInt16(value);
	}
}
