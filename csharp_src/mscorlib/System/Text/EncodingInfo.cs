namespace System.Text;

[Serializable]
public sealed class EncodingInfo
{
	private int iCodePage;

	private string strEncodingName;

	private string strDisplayName;

	public int CodePage => iCodePage;

	public string Name => strEncodingName;

	public string DisplayName => strDisplayName;

	internal EncodingInfo(int codePage, string name, string displayName)
	{
		iCodePage = codePage;
		strEncodingName = name;
		strDisplayName = displayName;
	}

	public Encoding GetEncoding()
	{
		return Encoding.GetEncoding(iCodePage);
	}

	public override bool Equals(object value)
	{
		if (value is EncodingInfo encodingInfo)
		{
			return CodePage == encodingInfo.CodePage;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return CodePage;
	}
}
