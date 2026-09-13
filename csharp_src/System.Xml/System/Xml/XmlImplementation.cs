namespace System.Xml;

public class XmlImplementation
{
	private XmlNameTable nameTable;

	internal XmlNameTable NameTable => nameTable;

	public XmlImplementation()
		: this(new NameTable())
	{
	}

	public XmlImplementation(XmlNameTable nt)
	{
		nameTable = nt;
	}

	public bool HasFeature(string strFeature, string strVersion)
	{
		if (string.Compare("XML", strFeature, StringComparison.OrdinalIgnoreCase) == 0)
		{
			switch (strVersion)
			{
			case null:
			case "1.0":
			case "2.0":
				return true;
			}
		}
		return false;
	}

	public virtual XmlDocument CreateDocument()
	{
		return new XmlDocument(this);
	}
}
