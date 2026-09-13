using System.Threading.Tasks;

namespace System.Xml;

internal class XmlRawWriterBase64Encoder : Base64Encoder
{
	private XmlRawWriter rawWriter;

	internal XmlRawWriterBase64Encoder(XmlRawWriter rawWriter)
	{
		this.rawWriter = rawWriter;
	}

	internal override void WriteChars(char[] chars, int index, int count)
	{
		rawWriter.WriteRaw(chars, index, count);
	}

	internal override Task WriteCharsAsync(char[] chars, int index, int count)
	{
		return rawWriter.WriteRawAsync(chars, index, count);
	}
}
