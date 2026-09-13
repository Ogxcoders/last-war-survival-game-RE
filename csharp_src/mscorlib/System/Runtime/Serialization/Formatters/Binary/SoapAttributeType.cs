namespace System.Runtime.Serialization.Formatters.Binary;

[Serializable]
internal enum SoapAttributeType
{
	None = 0,
	SchemaType = 1,
	Embedded = 2,
	XmlElement = 4,
	XmlAttribute = 8
}
