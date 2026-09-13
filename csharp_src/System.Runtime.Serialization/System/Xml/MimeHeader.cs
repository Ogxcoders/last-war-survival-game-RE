using System.Runtime.Serialization;

namespace System.Xml;

internal class MimeHeader
{
	private string name;

	private string value;

	public string Name => name;

	public string Value => value;

	public MimeHeader(string name, string value)
	{
		if (name == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
		}
		this.name = name;
		this.value = value;
	}
}
