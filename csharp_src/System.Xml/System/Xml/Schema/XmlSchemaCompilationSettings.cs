namespace System.Xml.Schema;

public sealed class XmlSchemaCompilationSettings
{
	private bool enableUpaCheck;

	public bool EnableUpaCheck
	{
		get
		{
			return enableUpaCheck;
		}
		set
		{
			enableUpaCheck = value;
		}
	}

	public XmlSchemaCompilationSettings()
	{
		enableUpaCheck = true;
	}
}
