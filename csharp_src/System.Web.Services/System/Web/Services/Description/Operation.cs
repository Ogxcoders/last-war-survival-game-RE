using System.Collections;
using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

[XmlFormatExtensionPoint("Extensions")]
public sealed class Operation : NamedItem
{
	private OperationFaultCollection faults;

	private OperationMessageCollection messages;

	private string[] parameterOrder;

	private PortType portType;

	private ServiceDescriptionFormatExtensionCollection extensions;

	private static readonly char[] wsChars = new char[4] { ' ', '\r', '\n', '\t' };

	[XmlElement("fault")]
	public OperationFaultCollection Faults => faults;

	[XmlElement("output", typeof(OperationOutput))]
	[XmlElement("input", typeof(OperationInput))]
	public OperationMessageCollection Messages => messages;

	[XmlIgnore]
	public string[] ParameterOrder
	{
		get
		{
			return parameterOrder;
		}
		set
		{
			parameterOrder = value;
		}
	}

	[DefaultValue("")]
	[XmlAttribute("parameterOrder")]
	public string ParameterOrderString
	{
		get
		{
			if (parameterOrder == null)
			{
				return string.Empty;
			}
			return string.Join(" ", parameterOrder);
		}
		set
		{
			ArrayList arrayList = new ArrayList();
			string[] array = value.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				value = array[i].Trim(wsChars);
				if (value.Length > 0)
				{
					arrayList.Add(value);
				}
			}
			ParameterOrder = (string[])arrayList.ToArray(typeof(string));
		}
	}

	public PortType PortType => portType;

	[XmlIgnore]
	public override ServiceDescriptionFormatExtensionCollection Extensions => extensions;

	public Operation()
	{
		faults = new OperationFaultCollection(this);
		messages = new OperationMessageCollection(this);
		parameterOrder = null;
		portType = null;
		extensions = new ServiceDescriptionFormatExtensionCollection(this);
	}

	public bool IsBoundBy(OperationBinding operationBinding)
	{
		return operationBinding.Name == base.Name;
	}

	internal void SetParent(PortType portType)
	{
		this.portType = portType;
	}
}
