using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mono.Net;

internal static class CFNetwork
{
	private class GetProxyData : IDisposable
	{
		public IntPtr script;

		public IntPtr targetUri;

		public IntPtr error;

		public IntPtr result;

		public ManualResetEvent evt = new ManualResetEvent(initialState: false);

		public void Dispose()
		{
			evt.Close();
		}
	}

	private delegate void CFProxyAutoConfigurationResultCallback(IntPtr client, IntPtr proxyList, IntPtr error);

	private class CFWebProxy : IWebProxy
	{
		private ICredentials credentials;

		private bool userSpecified;

		public ICredentials Credentials
		{
			get
			{
				return credentials;
			}
			set
			{
				userSpecified = true;
				credentials = value;
			}
		}

		private static Uri GetProxyUri(CFProxy proxy, out NetworkCredential credentials)
		{
			string text;
			switch (proxy.ProxyType)
			{
			case CFProxyType.FTP:
				text = "ftp://";
				break;
			case CFProxyType.HTTP:
			case CFProxyType.HTTPS:
				text = "http://";
				break;
			default:
				credentials = null;
				return null;
			}
			string username = proxy.Username;
			string password = proxy.Password;
			string hostName = proxy.HostName;
			int port = proxy.Port;
			if (username != null)
			{
				credentials = new NetworkCredential(username, password);
			}
			else
			{
				credentials = null;
			}
			return new Uri(text + hostName + ((port != 0) ? (":" + port) : string.Empty), UriKind.Absolute);
		}

		private static Uri GetProxyUriFromScript(IntPtr script, Uri targetUri, out NetworkCredential credentials)
		{
			return SelectProxy(GetProxiesForAutoConfigurationScript(script, targetUri), targetUri, out credentials);
		}

		private static Uri ExecuteProxyAutoConfigurationURL(IntPtr proxyAutoConfigURL, Uri targetUri, out NetworkCredential credentials)
		{
			return SelectProxy(CFNetwork.ExecuteProxyAutoConfigurationURL(proxyAutoConfigURL, targetUri), targetUri, out credentials);
		}

		private static Uri SelectProxy(CFProxy[] proxies, Uri targetUri, out NetworkCredential credentials)
		{
			if (proxies == null)
			{
				credentials = null;
				return targetUri;
			}
			for (int i = 0; i < proxies.Length; i++)
			{
				switch (proxies[i].ProxyType)
				{
				case CFProxyType.FTP:
				case CFProxyType.HTTP:
				case CFProxyType.HTTPS:
					return GetProxyUri(proxies[i], out credentials);
				case CFProxyType.None:
					credentials = null;
					return targetUri;
				}
			}
			credentials = null;
			return null;
		}

		public Uri GetProxy(Uri targetUri)
		{
			NetworkCredential networkCredential = null;
			Uri uri = null;
			if (targetUri == null)
			{
				throw new ArgumentNullException("targetUri");
			}
			try
			{
				CFProxySettings systemProxySettings = GetSystemProxySettings();
				CFProxy[] proxiesForUri = GetProxiesForUri(targetUri, systemProxySettings);
				if (proxiesForUri != null)
				{
					for (int i = 0; i < proxiesForUri.Length; i++)
					{
						if (!(uri == null))
						{
							break;
						}
						switch (proxiesForUri[i].ProxyType)
						{
						case CFProxyType.AutoConfigurationJavaScript:
							uri = GetProxyUriFromScript(proxiesForUri[i].AutoConfigurationJavaScript, targetUri, out networkCredential);
							break;
						case CFProxyType.AutoConfigurationUrl:
							uri = ExecuteProxyAutoConfigurationURL(proxiesForUri[i].AutoConfigurationUrl, targetUri, out networkCredential);
							break;
						case CFProxyType.FTP:
						case CFProxyType.HTTP:
						case CFProxyType.HTTPS:
							uri = GetProxyUri(proxiesForUri[i], out networkCredential);
							break;
						case CFProxyType.None:
							uri = targetUri;
							break;
						}
					}
					if (uri == null)
					{
						uri = targetUri;
					}
				}
				else
				{
					uri = targetUri;
				}
			}
			catch
			{
				uri = targetUri;
			}
			if (!userSpecified)
			{
				credentials = networkCredential;
			}
			return uri;
		}

		public bool IsBypassed(Uri targetUri)
		{
			if (targetUri == null)
			{
				throw new ArgumentNullException("targetUri");
			}
			return GetProxy(targetUri) == targetUri;
		}
	}

	public const string CFNetworkLibrary = "/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork";

	private static object lock_obj = new object();

	private static Queue<GetProxyData> get_proxy_queue;

	private static AutoResetEvent proxy_event;

	[DllImport("/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork", EntryPoint = "CFNetworkCopyProxiesForAutoConfigurationScript")]
	private static extern IntPtr CFNetworkCopyProxiesForAutoConfigurationScriptSequential(IntPtr proxyAutoConfigurationScript, IntPtr targetURL, out IntPtr error);

	[DllImport("/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork")]
	private static extern IntPtr CFNetworkExecuteProxyAutoConfigurationURL(IntPtr proxyAutoConfigURL, IntPtr targetURL, CFProxyAutoConfigurationResultCallback cb, ref CFStreamClientContext clientContext);

	private static void CFNetworkCopyProxiesForAutoConfigurationScriptThread()
	{
		bool flag = true;
		while (true)
		{
			proxy_event.WaitOne();
			do
			{
				GetProxyData getProxyData;
				lock (lock_obj)
				{
					if (get_proxy_queue.Count == 0)
					{
						break;
					}
					getProxyData = get_proxy_queue.Dequeue();
					flag = get_proxy_queue.Count > 0;
					goto IL_0050;
				}
				IL_0050:
				getProxyData.result = CFNetworkCopyProxiesForAutoConfigurationScriptSequential(getProxyData.script, getProxyData.targetUri, out getProxyData.error);
				getProxyData.evt.Set();
			}
			while (flag);
		}
	}

	private static IntPtr CFNetworkCopyProxiesForAutoConfigurationScript(IntPtr proxyAutoConfigurationScript, IntPtr targetURL, out IntPtr error)
	{
		using GetProxyData getProxyData = new GetProxyData();
		getProxyData.script = proxyAutoConfigurationScript;
		getProxyData.targetUri = targetURL;
		lock (lock_obj)
		{
			if (get_proxy_queue == null)
			{
				get_proxy_queue = new Queue<GetProxyData>();
				proxy_event = new AutoResetEvent(initialState: false);
				Thread thread = new Thread(CFNetworkCopyProxiesForAutoConfigurationScriptThread);
				thread.IsBackground = true;
				thread.Start();
			}
			get_proxy_queue.Enqueue(getProxyData);
			proxy_event.Set();
		}
		getProxyData.evt.WaitOne();
		error = getProxyData.error;
		return getProxyData.result;
	}

	private static CFArray CopyProxiesForAutoConfigurationScript(IntPtr proxyAutoConfigurationScript, CFUrl targetURL)
	{
		IntPtr error = IntPtr.Zero;
		IntPtr intPtr = CFNetworkCopyProxiesForAutoConfigurationScript(proxyAutoConfigurationScript, targetURL.Handle, out error);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new CFArray(intPtr, own: true);
	}

	public static CFProxy[] GetProxiesForAutoConfigurationScript(IntPtr proxyAutoConfigurationScript, CFUrl targetURL)
	{
		if (proxyAutoConfigurationScript == IntPtr.Zero)
		{
			throw new ArgumentNullException("proxyAutoConfigurationScript");
		}
		if (targetURL == null)
		{
			throw new ArgumentNullException("targetURL");
		}
		CFArray cFArray = CopyProxiesForAutoConfigurationScript(proxyAutoConfigurationScript, targetURL);
		if (cFArray == null)
		{
			return null;
		}
		CFProxy[] array = new CFProxy[cFArray.Count];
		for (int i = 0; i < array.Length; i++)
		{
			CFDictionary settings = new CFDictionary(cFArray[i], own: false);
			array[i] = new CFProxy(settings);
		}
		cFArray.Dispose();
		return array;
	}

	public static CFProxy[] GetProxiesForAutoConfigurationScript(IntPtr proxyAutoConfigurationScript, Uri targetUri)
	{
		if (proxyAutoConfigurationScript == IntPtr.Zero)
		{
			throw new ArgumentNullException("proxyAutoConfigurationScript");
		}
		if (targetUri == null)
		{
			throw new ArgumentNullException("targetUri");
		}
		CFUrl cFUrl = CFUrl.Create(targetUri.AbsoluteUri);
		CFProxy[] proxiesForAutoConfigurationScript = GetProxiesForAutoConfigurationScript(proxyAutoConfigurationScript, cFUrl);
		cFUrl.Dispose();
		return proxiesForAutoConfigurationScript;
	}

	public static CFProxy[] ExecuteProxyAutoConfigurationURL(IntPtr proxyAutoConfigURL, Uri targetURL)
	{
		CFUrl cFUrl = CFUrl.Create(targetURL.AbsoluteUri);
		if (cFUrl == null)
		{
			return null;
		}
		CFProxy[] proxies = null;
		CFRunLoop runLoop = CFRunLoop.CurrentRunLoop;
		CFProxyAutoConfigurationResultCallback cb = delegate(IntPtr client, IntPtr proxyList, IntPtr error)
		{
			if (proxyList != IntPtr.Zero)
			{
				CFArray cFArray = new CFArray(proxyList, own: false);
				proxies = new CFProxy[cFArray.Count];
				for (int i = 0; i < proxies.Length; i++)
				{
					CFDictionary settings = new CFDictionary(cFArray[i], own: false);
					proxies[i] = new CFProxy(settings);
				}
				cFArray.Dispose();
			}
			runLoop.Stop();
		};
		CFStreamClientContext clientContext = default(CFStreamClientContext);
		IntPtr source = CFNetworkExecuteProxyAutoConfigurationURL(proxyAutoConfigURL, cFUrl.Handle, cb, ref clientContext);
		CFString mode = CFString.Create("Mono.MacProxy");
		runLoop.AddSource(source, mode);
		runLoop.RunInMode(mode, double.MaxValue, returnAfterSourceHandled: false);
		runLoop.RemoveSource(source, mode);
		return proxies;
	}

	[DllImport("/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork")]
	private static extern IntPtr CFNetworkCopyProxiesForURL(IntPtr url, IntPtr proxySettings);

	private static CFArray CopyProxiesForURL(CFUrl url, CFDictionary proxySettings)
	{
		IntPtr intPtr = CFNetworkCopyProxiesForURL(url.Handle, proxySettings?.Handle ?? IntPtr.Zero);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new CFArray(intPtr, own: true);
	}

	public static CFProxy[] GetProxiesForURL(CFUrl url, CFProxySettings proxySettings)
	{
		if (url == null || url.Handle == IntPtr.Zero)
		{
			throw new ArgumentNullException("url");
		}
		if (proxySettings == null)
		{
			proxySettings = GetSystemProxySettings();
		}
		CFArray cFArray = CopyProxiesForURL(url, proxySettings.Dictionary);
		if (cFArray == null)
		{
			return null;
		}
		CFProxy[] array = new CFProxy[cFArray.Count];
		for (int i = 0; i < array.Length; i++)
		{
			CFDictionary settings = new CFDictionary(cFArray[i], own: false);
			array[i] = new CFProxy(settings);
		}
		cFArray.Dispose();
		return array;
	}

	public static CFProxy[] GetProxiesForUri(Uri uri, CFProxySettings proxySettings)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		CFUrl cFUrl = CFUrl.Create(uri.AbsoluteUri);
		if (cFUrl == null)
		{
			return null;
		}
		CFProxy[] proxiesForURL = GetProxiesForURL(cFUrl, proxySettings);
		cFUrl.Dispose();
		return proxiesForURL;
	}

	[DllImport("/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork")]
	private static extern IntPtr CFNetworkCopySystemProxySettings();

	public static CFProxySettings GetSystemProxySettings()
	{
		IntPtr intPtr = CFNetworkCopySystemProxySettings();
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		return new CFProxySettings(new CFDictionary(intPtr, own: true));
	}

	public static IWebProxy GetDefaultProxy()
	{
		return new CFWebProxy();
	}
}
