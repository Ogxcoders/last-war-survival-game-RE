namespace System.Xml;

internal class Ucs4Decoder4321 : Ucs4Decoder
{
	internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
	{
		byteCount += byteIndex;
		int i = byteIndex;
		int num = charIndex;
		for (; i + 3 < byteCount; i += 4)
		{
			uint num2 = (uint)((bytes[i + 3] << 24) | (bytes[i + 2] << 16) | (bytes[i + 1] << 8) | bytes[i]);
			if (num2 > 1114111)
			{
				throw new ArgumentException(Res.GetString("Invalid byte was found at index {0}.", i), (string)null);
			}
			if (num2 > 65535)
			{
				Ucs4ToUTF16(num2, chars, num);
				num++;
			}
			else
			{
				if (XmlCharType.IsSurrogate((int)num2))
				{
					throw new XmlException("Invalid character in the given encoding.", string.Empty);
				}
				chars[num] = (char)num2;
			}
			num++;
		}
		return num - charIndex;
	}
}
