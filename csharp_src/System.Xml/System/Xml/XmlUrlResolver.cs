using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Xml;

public class XmlUrlResolver : XmlResolver
{
	private static object s_DownloadManager;

	private ICredentials _credentials;

	private IWebProxy _proxy;

	private RequestCachePolicy _cachePolicy;

	private static XmlDownloadManager DownloadManager
	{
		get
		{
			if (s_DownloadManager == null)
			{
				object value = new XmlDownloadManager();
				Interlocked.CompareExchange<object>(ref s_DownloadManager, value, (object)null);
			}
			return (XmlDownloadManager)s_DownloadManager;
		}
	}

	public override ICredentials Credentials
	{
		set
		{
			_credentials = value;
		}
	}

	public IWebProxy Proxy
	{
		set
		{
			_proxy = value;
		}
	}

	public RequestCachePolicy CachePolicy
	{
		set
		{
			_cachePolicy = value;
		}
	}

	public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream) || ofObjectToReturn == typeof(object))
		{
			return DownloadManager.GetStream(absoluteUri, _credentials, _proxy, _cachePolicy);
		}
		throw new XmlException("Object type is not supported.", string.Empty);
	}

	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public override Uri ResolveUri(Uri baseUri, string relativeUri)
	{
		return base.ResolveUri(baseUri, relativeUri);
	}

	public override async Task<object> GetEntityAsync(Uri absoluteUri, string role, Type ofObjectToReturn)
	{
		if (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream) || ofObjectToReturn == typeof(object))
		{
			return await DownloadManager.GetStreamAsync(absoluteUri, _credentials, _proxy, _cachePolicy).ConfigureAwait(continueOnCapturedContext: false);
		}
		throw new XmlException("Object type is not supported.", string.Empty);
	}
}
