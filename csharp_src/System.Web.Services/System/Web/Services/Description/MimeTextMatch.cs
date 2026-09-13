using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

public sealed class MimeTextMatch
{
	private int capture;

	private int group;

	private bool ignoreCase;

	private MimeTextMatchCollection matches;

	private string name;

	private string pattern;

	private int repeats;

	private string type;

	[DefaultValue(0)]
	[XmlAttribute("capture")]
	public int Capture
	{
		get
		{
			return capture;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException();
			}
			capture = value;
		}
	}

	[DefaultValue(1)]
	[XmlAttribute("group")]
	public int Group
	{
		get
		{
			return group;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException();
			}
			group = value;
		}
	}

	[XmlAttribute("ignoreCase")]
	public bool IgnoreCase
	{
		get
		{
			return ignoreCase;
		}
		set
		{
			ignoreCase = value;
		}
	}

	[XmlElement("match")]
	public MimeTextMatchCollection Matches => matches;

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

	[XmlAttribute("pattern")]
	public string Pattern
	{
		get
		{
			return pattern;
		}
		set
		{
			pattern = value;
		}
	}

	[XmlIgnore]
	public int Repeats
	{
		get
		{
			return repeats;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException();
			}
			repeats = value;
		}
	}

	[DefaultValue("1")]
	[XmlAttribute("repeats")]
	public string RepeatsString
	{
		get
		{
			return Repeats.ToString();
		}
		set
		{
			Repeats = int.Parse(value);
		}
	}

	[XmlAttribute("type")]
	public string Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public MimeTextMatch()
	{
		capture = 0;
		group = 1;
		ignoreCase = false;
		matches = null;
		name = string.Empty;
		pattern = string.Empty;
		repeats = 1;
		type = string.Empty;
	}

	internal void SetParent(MimeTextMatchCollection matches)
	{
		this.matches = matches;
	}
}
