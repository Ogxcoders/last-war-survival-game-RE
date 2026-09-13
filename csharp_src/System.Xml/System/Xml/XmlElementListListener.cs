namespace System.Xml;

internal class XmlElementListListener
{
	private WeakReference elemList;

	private XmlDocument doc;

	private XmlNodeChangedEventHandler nodeChangeHandler;

	internal XmlElementListListener(XmlDocument doc, XmlElementList elemList)
	{
		this.doc = doc;
		this.elemList = new WeakReference(elemList);
		nodeChangeHandler = OnListChanged;
		doc.NodeInserted += nodeChangeHandler;
		doc.NodeRemoved += nodeChangeHandler;
	}

	private void OnListChanged(object sender, XmlNodeChangedEventArgs args)
	{
		lock (this)
		{
			if (elemList != null)
			{
				XmlElementList xmlElementList = (XmlElementList)elemList.Target;
				if (xmlElementList != null)
				{
					xmlElementList.ConcurrencyCheck(args);
					return;
				}
				doc.NodeInserted -= nodeChangeHandler;
				doc.NodeRemoved -= nodeChangeHandler;
				elemList = null;
			}
		}
	}

	internal void Unregister()
	{
		lock (this)
		{
			if (elemList != null)
			{
				doc.NodeInserted -= nodeChangeHandler;
				doc.NodeRemoved -= nodeChangeHandler;
				elemList = null;
			}
		}
	}
}
