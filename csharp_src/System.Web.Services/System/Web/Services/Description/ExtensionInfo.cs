using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal class ExtensionInfo
{
	private ArrayList _namespaceDeclarations;

	private string _namespace;

	private string _elementName;

	private Type _type;

	private XmlSerializer _serializer;

	public ArrayList NamespaceDeclarations
	{
		get
		{
			if (_namespaceDeclarations == null)
			{
				_namespaceDeclarations = new ArrayList();
			}
			return _namespaceDeclarations;
		}
	}

	public string Namespace
	{
		get
		{
			return _namespace;
		}
		set
		{
			_namespace = value;
		}
	}

	public string ElementName
	{
		get
		{
			return _elementName;
		}
		set
		{
			_elementName = value;
		}
	}

	public Type Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public XmlSerializer Serializer
	{
		get
		{
			return _serializer;
		}
		set
		{
			_serializer = value;
		}
	}
}
