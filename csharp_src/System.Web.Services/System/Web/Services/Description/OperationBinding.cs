using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class OperationBinding : NamedItem
{
	private Binding binding;

	private ServiceDescriptionFormatExtensionCollection extensions;

	private FaultBindingCollection faults;

	private InputBinding input;

	private OutputBinding output;

	public Binding Binding => binding;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	[XmlElement("fault")]
	public FaultBindingCollection Faults => faults;

	[XmlElement("input")]
	public InputBinding Input
	{
		get
		{
			return input;
		}
		set
		{
			input = value;
			if (input != null)
			{
				input.SetParent(this);
			}
		}
	}

	[XmlElement("output")]
	public OutputBinding Output
	{
		get
		{
			return output;
		}
		set
		{
			output = value;
			if (output != null)
			{
				output.SetParent(this);
			}
		}
	}

	public OperationBinding()
	{
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
		faults = new FaultBindingCollection(this);
		input = null;
		output = null;
	}

	internal void SetParent(Binding binding)
	{
		this.binding = binding;
	}
}
