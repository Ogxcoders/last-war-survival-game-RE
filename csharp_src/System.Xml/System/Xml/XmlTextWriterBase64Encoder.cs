using System.Threading.Tasks;

namespace System.Xml;

internal class XmlTextWriterBase64Encoder : Base64Encoder
{
	private XmlTextEncoder xmlTextEncoder;

	internal XmlTextWriterBase64Encoder(XmlTextEncoder xmlTextEncoder)
	{
		this.xmlTextEncoder = xmlTextEncoder;
	}

	internal override void WriteChars(char[] chars, int index, int count)
	{
		xmlTextEncoder.WriteRaw(chars, index, count);
	}

	internal override Task WriteCharsAsync(char[] chars, int index, int count)
	{
		throw new NotImplementedException();
	}
}
