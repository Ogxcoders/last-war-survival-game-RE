namespace System.Xml.Serialization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal class XmlTypeConvertorAttribute : Attribute
{
	public string Method { get; private set; }

	public XmlTypeConvertorAttribute(string method)
	{
		Method = method;
	}
}
