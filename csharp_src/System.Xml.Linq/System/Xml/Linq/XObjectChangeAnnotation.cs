namespace System.Xml.Linq;

internal class XObjectChangeAnnotation
{
	internal EventHandler<XObjectChangeEventArgs> changing;

	internal EventHandler<XObjectChangeEventArgs> changed;
}
