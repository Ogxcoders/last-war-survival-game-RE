using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

public abstract class OperationMessage : NamedItem
{
	private XmlQualifiedName message;

	private Operation operation;

	[XmlAttribute("message")]
	public XmlQualifiedName Message
	{
		get
		{
			return message;
		}
		set
		{
			message = value;
		}
	}

	public Operation Operation => operation;

	protected OperationMessage()
	{
		message = XmlQualifiedName.Empty;
		operation = null;
	}

	internal void SetParent(Operation operation)
	{
		this.operation = operation;
	}
}
