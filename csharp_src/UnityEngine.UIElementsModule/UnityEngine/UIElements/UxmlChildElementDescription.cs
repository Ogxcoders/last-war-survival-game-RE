using System;

namespace UnityEngine.UIElements;

public class UxmlChildElementDescription
{
	public string elementName { get; protected set; }

	public string elementNamespace { get; protected set; }

	public UxmlChildElementDescription(Type t)
	{
		if ((object)t == null)
		{
			throw new ArgumentNullException("t");
		}
		elementName = t.Name;
		elementNamespace = t.Namespace;
	}
}
