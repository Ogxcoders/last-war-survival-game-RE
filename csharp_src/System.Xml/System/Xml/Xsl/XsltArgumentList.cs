using System.Collections;

namespace System.Xml.Xsl;

public class XsltArgumentList
{
	private Hashtable parameters = new Hashtable();

	private Hashtable extensions = new Hashtable();

	internal XsltMessageEncounteredEventHandler xsltMessageEncountered;

	public event XsltMessageEncounteredEventHandler XsltMessageEncountered
	{
		add
		{
			xsltMessageEncountered = (XsltMessageEncounteredEventHandler)Delegate.Combine(xsltMessageEncountered, value);
		}
		remove
		{
			xsltMessageEncountered = (XsltMessageEncounteredEventHandler)Delegate.Remove(xsltMessageEncountered, value);
		}
	}

	public object GetParam(string name, string namespaceUri)
	{
		return parameters[new XmlQualifiedName(name, namespaceUri)];
	}

	public object GetExtensionObject(string namespaceUri)
	{
		return extensions[namespaceUri];
	}

	public void AddParam(string name, string namespaceUri, object parameter)
	{
		CheckArgumentNull(name, "name");
		CheckArgumentNull(namespaceUri, "namespaceUri");
		CheckArgumentNull(parameter, "parameter");
		XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, namespaceUri);
		xmlQualifiedName.Verify();
		parameters.Add(xmlQualifiedName, parameter);
	}

	public void AddExtensionObject(string namespaceUri, object extension)
	{
		CheckArgumentNull(namespaceUri, "namespaceUri");
		CheckArgumentNull(extension, "extension");
		extensions.Add(namespaceUri, extension);
	}

	public object RemoveParam(string name, string namespaceUri)
	{
		XmlQualifiedName key = new XmlQualifiedName(name, namespaceUri);
		object result = parameters[key];
		parameters.Remove(key);
		return result;
	}

	public object RemoveExtensionObject(string namespaceUri)
	{
		object result = extensions[namespaceUri];
		extensions.Remove(namespaceUri);
		return result;
	}

	public void Clear()
	{
		parameters.Clear();
		extensions.Clear();
		xsltMessageEncountered = null;
	}

	private static void CheckArgumentNull(object param, string paramName)
	{
		if (param == null)
		{
			throw new ArgumentNullException(paramName);
		}
	}
}
