using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;

namespace System.Xml;

[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
public class XmlSecureResolver : XmlResolver
{
	private XmlResolver resolver;

	public override ICredentials Credentials
	{
		set
		{
			resolver.Credentials = value;
		}
	}

	public XmlSecureResolver(XmlResolver resolver, string securityUrl)
		: this(resolver, (PermissionSet)null)
	{
	}

	public XmlSecureResolver(XmlResolver resolver, Evidence evidence)
		: this(resolver, (PermissionSet)null)
	{
	}

	public XmlSecureResolver(XmlResolver resolver, PermissionSet permissionSet)
	{
		this.resolver = resolver;
	}

	public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		return resolver.GetEntity(absoluteUri, role, ofObjectToReturn);
	}

	public override Uri ResolveUri(Uri baseUri, string relativeUri)
	{
		return resolver.ResolveUri(baseUri, relativeUri);
	}

	public static Evidence CreateEvidenceForUrl(string securityUrl)
	{
		return null;
	}

	public override Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		return resolver.GetEntityAsync(absoluteUri, role, ofObjectToReturn);
	}
}
