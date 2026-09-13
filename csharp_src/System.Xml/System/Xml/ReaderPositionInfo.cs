namespace System.Xml;

internal class ReaderPositionInfo : PositionInfo
{
	private IXmlLineInfo lineInfo;

	public override int LineNumber => lineInfo.LineNumber;

	public override int LinePosition => lineInfo.LinePosition;

	public ReaderPositionInfo(IXmlLineInfo lineInfo)
	{
		this.lineInfo = lineInfo;
	}

	public override bool HasLineInfo()
	{
		return lineInfo.HasLineInfo();
	}
}
