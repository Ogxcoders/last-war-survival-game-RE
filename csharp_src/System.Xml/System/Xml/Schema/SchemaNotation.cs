namespace System.Xml.Schema;

internal sealed class SchemaNotation
{
	internal const int SYSTEM = 0;

	internal const int PUBLIC = 1;

	private XmlQualifiedName name;

	private string systemLiteral;

	private string pubid;

	internal XmlQualifiedName Name => name;

	internal string SystemLiteral
	{
		get
		{
			return systemLiteral;
		}
		set
		{
			systemLiteral = value;
		}
	}

	internal string Pubid
	{
		get
		{
			return pubid;
		}
		set
		{
			pubid = value;
		}
	}

	internal SchemaNotation(XmlQualifiedName name)
	{
		this.name = name;
	}
}
