using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

public sealed class DiscoveryClientResult
{
	private string filename;

	private string referenceTypeName;

	private string url;

	[XmlAttribute("filename")]
	public string Filename
	{
		get
		{
			return filename;
		}
		set
		{
			filename = value;
		}
	}

	[XmlAttribute("referenceType")]
	public string ReferenceTypeName
	{
		get
		{
			return referenceTypeName;
		}
		set
		{
			referenceTypeName = value;
		}
	}

	[XmlAttribute("url")]
	public string Url
	{
		get
		{
			return url;
		}
		set
		{
			url = value;
		}
	}

	public DiscoveryClientResult()
	{
	}

	public DiscoveryClientResult(Type referenceType, string url, string filename)
		: this()
	{
		this.filename = filename;
		this.url = url;
		referenceTypeName = referenceType.FullName;
	}
}
