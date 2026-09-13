using System.Text;
using System.Xml.XPath;

namespace System.Xml;

public abstract class XmlCharacterData : XmlLinkedNode
{
	private string data;

	public override string Value
	{
		get
		{
			return Data;
		}
		set
		{
			Data = value;
		}
	}

	public override string InnerText
	{
		get
		{
			return Value;
		}
		set
		{
			Value = value;
		}
	}

	public virtual string Data
	{
		get
		{
			if (data != null)
			{
				return data;
			}
			return string.Empty;
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

	public virtual int Length
	{
		get
		{
			if (data != null)
			{
				return data.Length;
			}
			return 0;
		}
	}

	protected internal XmlCharacterData(string data, XmlDocument doc)
		: base(doc)
	{
		this.data = data;
	}

	public virtual string Substring(int offset, int count)
	{
		int num = ((data != null) ? data.Length : 0);
		if (num > 0)
		{
			if (num < offset + count)
			{
				count = num - offset;
			}
			return data.Substring(offset, count);
		}
		return string.Empty;
	}

	public virtual void AppendData(string strData)
	{
		XmlNode xmlNode = ParentNode;
		int num = ((data != null) ? data.Length : 0);
		if (strData != null)
		{
			num += strData.Length;
		}
		string newValue = new StringBuilder(num).Append(data).Append(strData).ToString();
		XmlNodeChangedEventArgs eventArgs = GetEventArgs(this, xmlNode, xmlNode, data, newValue, XmlNodeChangedAction.Change);
		if (eventArgs != null)
		{
			BeforeEvent(eventArgs);
		}
		data = newValue;
		if (eventArgs != null)
		{
			AfterEvent(eventArgs);
		}
	}

	public virtual void InsertData(int offset, string strData)
	{
		XmlNode xmlNode = ParentNode;
		int num = ((data != null) ? data.Length : 0);
		if (strData != null)
		{
			num += strData.Length;
		}
		string newValue = new StringBuilder(num).Append(data).Insert(offset, strData).ToString();
		XmlNodeChangedEventArgs eventArgs = GetEventArgs(this, xmlNode, xmlNode, data, newValue, XmlNodeChangedAction.Change);
		if (eventArgs != null)
		{
			BeforeEvent(eventArgs);
		}
		data = newValue;
		if (eventArgs != null)
		{
			AfterEvent(eventArgs);
		}
	}

	public virtual void DeleteData(int offset, int count)
	{
		int num = ((data != null) ? data.Length : 0);
		if (num > 0 && num < offset + count)
		{
			count = Math.Max(num - offset, 0);
		}
		string newValue = new StringBuilder(data).Remove(offset, count).ToString();
		XmlNode xmlNode = ParentNode;
		XmlNodeChangedEventArgs eventArgs = GetEventArgs(this, xmlNode, xmlNode, data, newValue, XmlNodeChangedAction.Change);
		if (eventArgs != null)
		{
			BeforeEvent(eventArgs);
		}
		data = newValue;
		if (eventArgs != null)
		{
			AfterEvent(eventArgs);
		}
	}

	public virtual void ReplaceData(int offset, int count, string strData)
	{
		int num = ((data != null) ? data.Length : 0);
		if (num > 0 && num < offset + count)
		{
			count = Math.Max(num - offset, 0);
		}
		string newValue = new StringBuilder(data).Remove(offset, count).Insert(offset, strData).ToString();
		XmlNode xmlNode = ParentNode;
		XmlNodeChangedEventArgs eventArgs = GetEventArgs(this, xmlNode, xmlNode, data, newValue, XmlNodeChangedAction.Change);
		if (eventArgs != null)
		{
			BeforeEvent(eventArgs);
		}
		data = newValue;
		if (eventArgs != null)
		{
			AfterEvent(eventArgs);
		}
	}

	internal bool CheckOnData(string data)
	{
		return XmlCharType.Instance.IsOnlyWhitespace(data);
	}

	internal bool DecideXPNodeTypeForTextNodes(XmlNode node, ref XPathNodeType xnt)
	{
		while (node != null)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.SignificantWhitespace:
				xnt = XPathNodeType.SignificantWhitespace;
				break;
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
				xnt = XPathNodeType.Text;
				return false;
			case XmlNodeType.EntityReference:
				if (!DecideXPNodeTypeForTextNodes(node.FirstChild, ref xnt))
				{
					return false;
				}
				break;
			default:
				return false;
			case XmlNodeType.Whitespace:
				break;
			}
			node = node.NextSibling;
		}
		return true;
	}
}
