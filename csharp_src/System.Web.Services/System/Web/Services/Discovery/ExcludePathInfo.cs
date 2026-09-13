using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

public sealed class ExcludePathInfo
{
	private string path;

	[XmlAttribute("path")]
	public string Path
	{
		get
		{
			return path;
		}
		set
		{
			path = value;
		}
	}

	public ExcludePathInfo()
	{
	}

	public ExcludePathInfo(string path)
	{
		this.path = path;
	}
}
