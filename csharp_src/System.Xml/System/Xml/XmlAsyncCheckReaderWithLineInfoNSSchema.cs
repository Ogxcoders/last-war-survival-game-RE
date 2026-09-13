using System.Xml.Schema;

namespace System.Xml;

internal class XmlAsyncCheckReaderWithLineInfoNSSchema : XmlAsyncCheckReaderWithLineInfoNS, IXmlSchemaInfo
{
	private readonly IXmlSchemaInfo readerAsIXmlSchemaInfo;

	XmlSchemaValidity IXmlSchemaInfo.Validity => readerAsIXmlSchemaInfo.Validity;

	bool IXmlSchemaInfo.IsDefault => readerAsIXmlSchemaInfo.IsDefault;

	bool IXmlSchemaInfo.IsNil => readerAsIXmlSchemaInfo.IsNil;

	XmlSchemaSimpleType IXmlSchemaInfo.MemberType => readerAsIXmlSchemaInfo.MemberType;

	XmlSchemaType IXmlSchemaInfo.SchemaType => readerAsIXmlSchemaInfo.SchemaType;

	XmlSchemaElement IXmlSchemaInfo.SchemaElement => readerAsIXmlSchemaInfo.SchemaElement;

	XmlSchemaAttribute IXmlSchemaInfo.SchemaAttribute => readerAsIXmlSchemaInfo.SchemaAttribute;

	public XmlAsyncCheckReaderWithLineInfoNSSchema(XmlReader reader)
		: base(reader)
	{
		readerAsIXmlSchemaInfo = (IXmlSchemaInfo)reader;
	}
}
