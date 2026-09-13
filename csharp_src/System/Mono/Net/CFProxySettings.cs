using System;

namespace Mono.Net;

internal class CFProxySettings
{
	private static IntPtr kCFNetworkProxiesHTTPEnable;

	private static IntPtr kCFNetworkProxiesHTTPPort;

	private static IntPtr kCFNetworkProxiesHTTPProxy;

	private static IntPtr kCFNetworkProxiesProxyAutoConfigEnable;

	private static IntPtr kCFNetworkProxiesProxyAutoConfigJavaScript;

	private static IntPtr kCFNetworkProxiesProxyAutoConfigURLString;

	private CFDictionary settings;

	public CFDictionary Dictionary => settings;

	public bool HTTPEnable
	{
		get
		{
			if (kCFNetworkProxiesHTTPEnable == IntPtr.Zero)
			{
				return false;
			}
			return CFNumber.AsBool(settings[kCFNetworkProxiesHTTPEnable]);
		}
	}

	public int HTTPPort
	{
		get
		{
			if (kCFNetworkProxiesHTTPPort == IntPtr.Zero)
			{
				return 0;
			}
			return CFNumber.AsInt32(settings[kCFNetworkProxiesHTTPPort]);
		}
	}

	public string HTTPProxy
	{
		get
		{
			if (kCFNetworkProxiesHTTPProxy == IntPtr.Zero)
			{
				return null;
			}
			return CFString.AsString(settings[kCFNetworkProxiesHTTPProxy]);
		}
	}

	public bool ProxyAutoConfigEnable
	{
		get
		{
			if (kCFNetworkProxiesProxyAutoConfigEnable == IntPtr.Zero)
			{
				return false;
			}
			return CFNumber.AsBool(settings[kCFNetworkProxiesProxyAutoConfigEnable]);
		}
	}

	public string ProxyAutoConfigJavaScript
	{
		get
		{
			if (kCFNetworkProxiesProxyAutoConfigJavaScript == IntPtr.Zero)
			{
				return null;
			}
			return CFString.AsString(settings[kCFNetworkProxiesProxyAutoConfigJavaScript]);
		}
	}

	public string ProxyAutoConfigURLString
	{
		get
		{
			if (kCFNetworkProxiesProxyAutoConfigURLString == IntPtr.Zero)
			{
				return null;
			}
			return CFString.AsString(settings[kCFNetworkProxiesProxyAutoConfigURLString]);
		}
	}

	static CFProxySettings()
	{
		IntPtr handle = CFObject.dlopen("/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork", 0);
		kCFNetworkProxiesHTTPEnable = CFObject.GetCFObjectHandle(handle, "kCFNetworkProxiesHTTPEnable");
		kCFNetworkProxiesHTTPPort = CFObject.GetCFObjectHandle(handle, "kCFNetworkProxiesHTTPPort");
		kCFNetworkProxiesHTTPProxy = CFObject.GetCFObjectHandle(handle, "kCFNetworkProxiesHTTPProxy");
		kCFNetworkProxiesProxyAutoConfigEnable = CFObject.GetCFObjectHandle(handle, "kCFNetworkProxiesProxyAutoConfigEnable");
		kCFNetworkProxiesProxyAutoConfigJavaScript = CFObject.GetCFObjectHandle(handle, "kCFNetworkProxiesProxyAutoConfigJavaScript");
		kCFNetworkProxiesProxyAutoConfigURLString = CFObject.GetCFObjectHandle(handle, "kCFNetworkProxiesProxyAutoConfigURLString");
		CFObject.dlclose(handle);
	}

	public CFProxySettings(CFDictionary settings)
	{
		this.settings = settings;
	}
}
