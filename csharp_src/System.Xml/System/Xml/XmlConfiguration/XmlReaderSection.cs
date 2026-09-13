using System.ComponentModel;

namespace System.Xml.XmlConfiguration;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class XmlReaderSection
{
	internal static bool ProhibitDefaultUrlResolver => false;

	public string CollapseWhiteSpaceIntoEmptyStringString
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	private bool _CollapseWhiteSpaceIntoEmptyString
	{
		get
		{
			XmlConvert.TryToBoolean(CollapseWhiteSpaceIntoEmptyStringString, out var result);
			return result;
		}
	}

	internal static bool CollapseWhiteSpaceIntoEmptyString => false;

	internal static XmlResolver CreateDefaultResolver()
	{
		if (ProhibitDefaultUrlResolver)
		{
			return null;
		}
		return new XmlUrlResolver();
	}
}
