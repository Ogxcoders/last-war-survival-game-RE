namespace System.Xml;

public class XmlNodeChangedEventArgs : EventArgs
{
	private XmlNodeChangedAction action;

	private XmlNode node;

	private XmlNode oldParent;

	private XmlNode newParent;

	private string oldValue;

	private string newValue;

	public XmlNodeChangedAction Action => action;

	public XmlNode Node => node;

	public XmlNode OldParent => oldParent;

	public XmlNode NewParent => newParent;

	public string OldValue => oldValue;

	public string NewValue => newValue;

	public XmlNodeChangedEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action)
	{
		this.node = node;
		this.oldParent = oldParent;
		this.newParent = newParent;
		this.action = action;
		this.oldValue = oldValue;
		this.newValue = newValue;
	}
}
