using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class XmlUTF8TextWriter : XmlBaseWriter, IXmlTextWriterInitializer
{
	private XmlUTF8NodeWriter writer;

	internal override bool FastAsync => true;

	public override bool CanFragment => writer.Encoding == null;

	public void SetOutput(Stream stream, Encoding encoding, bool ownsStream)
	{
		if (stream == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("stream");
		}
		if (encoding == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("encoding");
		}
		if (encoding.WebName != Encoding.UTF8.WebName)
		{
			stream = new EncodingStreamWrapper(stream, encoding, emitBOM: true);
		}
		if (writer == null)
		{
			writer = new XmlUTF8NodeWriter();
		}
		writer.SetOutput(stream, ownsStream, encoding);
		SetOutput(writer);
	}

	protected override XmlSigningNodeWriter CreateSigningNodeWriter()
	{
		return new XmlSigningNodeWriter(text: true);
	}
}
