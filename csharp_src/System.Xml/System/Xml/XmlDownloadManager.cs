using System.Collections;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace System.Xml;

internal class XmlDownloadManager
{
	private Hashtable connections;

	internal Stream GetStream(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
	{
		if (uri.Scheme == "file")
		{
			return new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1);
		}
		return GetNonFileStream(uri, credentials, proxy, cachePolicy);
	}

	private Stream GetNonFileStream(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
	{
		WebRequest webRequest = WebRequest.Create(uri);
		if (credentials != null)
		{
			webRequest.Credentials = credentials;
		}
		if (proxy != null)
		{
			webRequest.Proxy = proxy;
		}
		if (cachePolicy != null)
		{
			webRequest.CachePolicy = cachePolicy;
		}
		WebResponse response = webRequest.GetResponse();
		if (webRequest is HttpWebRequest httpWebRequest)
		{
			lock (this)
			{
				if (connections == null)
				{
					connections = new Hashtable();
				}
				OpenedHost openedHost = (OpenedHost)connections[httpWebRequest.Address.Host];
				if (openedHost == null)
				{
					openedHost = new OpenedHost();
				}
				if (openedHost.nonCachedConnectionsCount < httpWebRequest.ServicePoint.ConnectionLimit - 1)
				{
					if (openedHost.nonCachedConnectionsCount == 0)
					{
						connections.Add(httpWebRequest.Address.Host, openedHost);
					}
					openedHost.nonCachedConnectionsCount++;
					return new XmlRegisteredNonCachedStream(response.GetResponseStream(), this, httpWebRequest.Address.Host);
				}
				return new XmlCachedStream(response.ResponseUri, response.GetResponseStream());
			}
		}
		return response.GetResponseStream();
	}

	internal void Remove(string host)
	{
		lock (this)
		{
			OpenedHost openedHost = (OpenedHost)connections[host];
			if (openedHost != null && --openedHost.nonCachedConnectionsCount == 0)
			{
				connections.Remove(host);
			}
		}
	}

	internal Task<Stream> GetStreamAsync(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
	{
		if (uri.Scheme == "file")
		{
			return Task.Run((Func<Stream>)(() => new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1, useAsync: true)));
		}
		return GetNonFileStreamAsync(uri, credentials, proxy, cachePolicy);
	}

	private async Task<Stream> GetNonFileStreamAsync(Uri uri, ICredentials credentials, IWebProxy proxy, RequestCachePolicy cachePolicy)
	{
		WebRequest req = WebRequest.Create(uri);
		if (credentials != null)
		{
			req.Credentials = credentials;
		}
		if (proxy != null)
		{
			req.Proxy = proxy;
		}
		if (cachePolicy != null)
		{
			req.CachePolicy = cachePolicy;
		}
		WebResponse webResponse = await Task<WebResponse>.Factory.FromAsync(req.BeginGetResponse, req.EndGetResponse, null).ConfigureAwait(continueOnCapturedContext: false);
		if (req is HttpWebRequest httpWebRequest)
		{
			lock (this)
			{
				if (connections == null)
				{
					connections = new Hashtable();
				}
				OpenedHost openedHost = (OpenedHost)connections[httpWebRequest.Address.Host];
				if (openedHost == null)
				{
					openedHost = new OpenedHost();
				}
				if (openedHost.nonCachedConnectionsCount < httpWebRequest.ServicePoint.ConnectionLimit - 1)
				{
					if (openedHost.nonCachedConnectionsCount == 0)
					{
						connections.Add(httpWebRequest.Address.Host, openedHost);
					}
					openedHost.nonCachedConnectionsCount++;
					return new XmlRegisteredNonCachedStream(webResponse.GetResponseStream(), this, httpWebRequest.Address.Host);
				}
				return new XmlCachedStream(webResponse.ResponseUri, webResponse.GetResponseStream());
			}
		}
		return webResponse.GetResponseStream();
	}
}
