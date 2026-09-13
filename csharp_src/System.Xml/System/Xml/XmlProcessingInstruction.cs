using System.Xml.XPath;

namespace System.Xml;

public class XmlProcessingInstruction : XmlLinkedNode
{
	private string target;

	private string data;

	public override string Name
	{
		get
		{
			if (target != null)
			{
				return target;
			}
			return string.Empty;
		}
	}

	public override string LocalName => Name;

	public override string Value
	{
		get
		{
			return data;
		}
		set
		{
			Data = value;
		}
	}

	public string Target => target;

	public string Data
	{
		get
		{
			return data;
		}
		set
		{
			XmlNode xmlNode = ParentNode;
			XmlNodeChangedEventArgs eventArgs = GetEventArgs(this, xmlNode, xmlNode, data, value, XmlNodeChangedAction.Change);
			if (eventArgs != null)
			{
				BeforeEvent(eventArgs);
			}
			data = value;
			if (eventArgs != null)
			{
				AfterEvent(eventArgs);
			}
		}
	}

	public override string InnerText
	{
		get
		{
			return data;
		}
		set
		{
			Data = value;
		}
	}

	public override XmlNodeType NodeType => XmlNodeType.ProcessingInstruction;

	internal override string XPLocalName => Name;

	internal override XPathNodeType XPNodeType => XPathNodeType.ProcessingInstruction;

	protected internal XmlProcessingInstruction(string target, string data, XmlDocument doc)
		: base(doc)
	{
		this.target = target;
		this.data = data;
	}

	public override XmlNode CloneNode(bool deep)
	{
		return OwnerDocument.CreateProcessingInstruction(target, data);
	}

	public override void WriteTo(XmlWriter w)
	{
		w.WriteProcessingInstruction(target, data);
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}
}
