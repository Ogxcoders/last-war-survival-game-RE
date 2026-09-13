namespace System.Xml;

internal class XmlAsyncCheckReaderWithLineInfo : XmlAsyncCheckReader, IXmlLineInfo
{
	private readonly IXmlLineInfo readerAsIXmlLineInfo;

	public virtual int LineNumber => readerAsIXmlLineInfo.LineNumber;

	public virtual int LinePosition => readerAsIXmlLineInfo.LinePosition;

	public XmlAsyncCheckReaderWithLineInfo(XmlReader reader)
		: base(reader)
	{
		readerAsIXmlLineInfo = (IXmlLineInfo)reader;
	}

	public virtual bool HasLineInfo()
	{
		return readerAsIXmlLineInfo.HasLineInfo();
	}
}
