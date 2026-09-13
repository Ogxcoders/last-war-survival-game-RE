using System;

namespace Mono.Net;

internal class CFProxy
{
	private static IntPtr kCFProxyAutoConfigurationJavaScriptKey;

	private static IntPtr kCFProxyAutoConfigurationURLKey;

	private static IntPtr kCFProxyHostNameKey;

	private static IntPtr kCFProxyPasswordKey;

	private static IntPtr kCFProxyPortNumberKey;

	private static IntPtr kCFProxyTypeKey;

	private static IntPtr kCFProxyUsernameKey;

	private static IntPtr kCFProxyTypeAutoConfigurationURL;

	private static IntPtr kCFProxyTypeAutoConfigurationJavaScript;

	private static IntPtr kCFProxyTypeFTP;

	private static IntPtr kCFProxyTypeHTTP;

	private static IntPtr kCFProxyTypeHTTPS;

	private static IntPtr kCFProxyTypeSOCKS;

	private CFDictionary settings;

	public IntPtr AutoConfigurationJavaScript
	{
		get
		{
			if (kCFProxyAutoConfigurationJavaScriptKey == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			return settings[kCFProxyAutoConfigurationJavaScriptKey];
		}
	}

	public IntPtr AutoConfigurationUrl
	{
		get
		{
			if (kCFProxyAutoConfigurationURLKey == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			return settings[kCFProxyAutoConfigurationURLKey];
		}
	}

	public string HostName
	{
		get
		{
			if (kCFProxyHostNameKey == IntPtr.Zero)
			{
				return null;
			}
			return CFString.AsString(settings[kCFProxyHostNameKey]);
		}
	}

	public string Password
	{
		get
		{
			if (kCFProxyPasswordKey == IntPtr.Zero)
			{
				return null;
			}
			return CFString.AsString(settings[kCFProxyPasswordKey]);
		}
	}

	public int Port
	{
		get
		{
			if (kCFProxyPortNumberKey == IntPtr.Zero)
			{
				return 0;
			}
			return CFNumber.AsInt32(settings[kCFProxyPortNumberKey]);
		}
	}

	public CFProxyType ProxyType
	{
		get
		{
			if (kCFProxyTypeKey == IntPtr.Zero)
			{
				return CFProxyType.None;
			}
			return CFProxyTypeToEnum(settings[kCFProxyTypeKey]);
		}
	}

	public string Username
	{
		get
		{
			if (kCFProxyUsernameKey == IntPtr.Zero)
			{
				return null;
			}
			return CFString.AsString(settings[kCFProxyUsernameKey]);
		}
	}

	static CFProxy()
	{
		IntPtr handle = CFObject.dlopen("/System/Library/Frameworks/CoreServices.framework/Frameworks/CFNetwork.framework/CFNetwork", 0);
		kCFProxyAutoConfigurationJavaScriptKey = CFObject.GetCFObjectHandle(handle, "kCFProxyAutoConfigurationJavaScriptKey");
		kCFProxyAutoConfigurationURLKey = CFObject.GetCFObjectHandle(handle, "kCFProxyAutoConfigurationURLKey");
		kCFProxyHostNameKey = CFObject.GetCFObjectHandle(handle, "kCFProxyHostNameKey");
		kCFProxyPasswordKey = CFObject.GetCFObjectHandle(handle, "kCFProxyPasswordKey");
		kCFProxyPortNumberKey = CFObject.GetCFObjectHandle(handle, "kCFProxyPortNumberKey");
		kCFProxyTypeKey = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeKey");
		kCFProxyUsernameKey = CFObject.GetCFObjectHandle(handle, "kCFProxyUsernameKey");
		kCFProxyTypeAutoConfigurationURL = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeAutoConfigurationURL");
		kCFProxyTypeAutoConfigurationJavaScript = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeAutoConfigurationJavaScript");
		kCFProxyTypeFTP = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeFTP");
		kCFProxyTypeHTTP = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeHTTP");
		kCFProxyTypeHTTPS = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeHTTPS");
		kCFProxyTypeSOCKS = CFObject.GetCFObjectHandle(handle, "kCFProxyTypeSOCKS");
		CFObject.dlclose(handle);
	}

	internal CFProxy(CFDictionary settings)
	{
		this.settings = settings;
	}

	private static CFProxyType CFProxyTypeToEnum(IntPtr type)
	{
		if (type == kCFProxyTypeAutoConfigurationJavaScript)
		{
			return CFProxyType.AutoConfigurationJavaScript;
		}
		if (type == kCFProxyTypeAutoConfigurationURL)
		{
			return CFProxyType.AutoConfigurationUrl;
		}
		if (type == kCFProxyTypeFTP)
		{
			return CFProxyType.FTP;
		}
		if (type == kCFProxyTypeHTTP)
		{
			return CFProxyType.HTTP;
		}
		if (type == kCFProxyTypeHTTPS)
		{
			return CFProxyType.HTTPS;
		}
		if (type == kCFProxyTypeSOCKS)
		{
			return CFProxyType.SOCKS;
		}
		if (CFString.Compare(type, kCFProxyTypeAutoConfigurationJavaScript) == 0)
		{
			return CFProxyType.AutoConfigurationJavaScript;
		}
		if (CFString.Compare(type, kCFProxyTypeAutoConfigurationURL) == 0)
		{
			return CFProxyType.AutoConfigurationUrl;
		}
		if (CFString.Compare(type, kCFProxyTypeFTP) == 0)
		{
			return CFProxyType.FTP;
		}
		if (CFString.Compare(type, kCFProxyTypeHTTP) == 0)
		{
			return CFProxyType.HTTP;
		}
		if (CFString.Compare(type, kCFProxyTypeHTTPS) == 0)
		{
			return CFProxyType.HTTPS;
		}
		if (CFString.Compare(type, kCFProxyTypeSOCKS) == 0)
		{
			return CFProxyType.SOCKS;
		}
		return CFProxyType.None;
	}
}
