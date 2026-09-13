namespace System.Xml.Linq;

internal class LineInfoEndElementAnnotation : LineInfoAnnotation
{
	public LineInfoEndElementAnnotation(int lineNumber, int linePosition)
		: base(lineNumber, linePosition)
	{
	}
}
