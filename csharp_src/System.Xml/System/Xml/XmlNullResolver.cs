using System.Net;

namespace System.Xml;

internal class XmlNullResolver : XmlResolver
{
	public static readonly XmlNullResolver Singleton = new XmlNullResolver();

	public override ICredentials Credentials
	{
		set
		{
		}
	}

	private XmlNullResolver()
	{
	}

	public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		throw new XmlException("Resolving of external URIs was prohibited.", string.Empty);
	}
}
