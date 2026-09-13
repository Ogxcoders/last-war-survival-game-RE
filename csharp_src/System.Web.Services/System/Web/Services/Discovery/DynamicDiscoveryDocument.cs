using System.IO;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery;

[XmlRoot("dynamicDiscovery", Namespace = "urn:schemas-dynamicdiscovery:disco.2000-03-17", IsNullable = true)]
public sealed class DynamicDiscoveryDocument
{
	public const string Namespace = "urn:schemas-dynamicdiscovery:disco.2000-03-17";

	private ExcludePathInfo[] excludes;

	[XmlElement("exclude", typeof(ExcludePathInfo))]
	public ExcludePathInfo[] ExcludePaths
	{
		get
		{
			return excludes;
		}
		set
		{
			excludes = value;
		}
	}

	public static DynamicDiscoveryDocument Load(Stream stream)
	{
		return (DynamicDiscoveryDocument)new XmlSerializer(typeof(DynamicDiscoveryDocument)).Deserialize(stream);
	}

	public void Write(Stream stream)
	{
		new XmlSerializer(typeof(DynamicDiscoveryDocument)).Serialize(stream, this);
	}

	internal bool IsExcluded(string path)
	{
		if (excludes == null)
		{
			return false;
		}
		ExcludePathInfo[] array = excludes;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Path == path)
			{
				return true;
			}
		}
		return false;
	}
}
