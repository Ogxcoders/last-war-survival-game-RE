namespace System.Xml;

internal class MtomBinaryData
{
	internal MtomBinaryDataType type;

	internal IStreamProvider provider;

	internal byte[] chunk;

	internal long Length
	{
		get
		{
			if (type == MtomBinaryDataType.Segment)
			{
				return chunk.Length;
			}
			return -1L;
		}
	}

	internal MtomBinaryData(IStreamProvider provider)
	{
		type = MtomBinaryDataType.Provider;
		this.provider = provider;
	}

	internal MtomBinaryData(byte[] buffer, int offset, int count)
	{
		type = MtomBinaryDataType.Segment;
		chunk = new byte[count];
		Buffer.BlockCopy(buffer, offset, chunk, 0, count);
	}
}
