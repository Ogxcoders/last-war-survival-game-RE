namespace System.Xml.Xsl;

internal struct StringPair
{
	private string left;

	private string right;

	public string Left => left;

	public string Right => right;

	public StringPair(string left, string right)
	{
		this.left = left;
		this.right = right;
	}
}
