namespace System.Xml;

public enum ValidationType
{
	None,
	[Obsolete("Validation type should be specified as DTD or Schema.")]
	Auto,
	DTD,
	[Obsolete("XDR Validation through XmlValidatingReader is obsoleted")]
	XDR,
	Schema
}
