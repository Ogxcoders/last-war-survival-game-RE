using System.Xml.Serialization;

namespace System.Web.Services.Description;

public abstract class NamedItem : DocumentableItem
{
	private string name;

	[XmlAttribute("name")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}
}
