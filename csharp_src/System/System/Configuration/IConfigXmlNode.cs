namespace System.Configuration;

internal interface IConfigXmlNode
{
	string Filename { get; }

	int LineNumber { get; }
}
