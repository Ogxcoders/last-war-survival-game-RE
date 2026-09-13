using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Net;

[Serializable]
public class WebProxy : IAutoWebProxy, IWebProxy, ISerializable
{
	private bool _UseRegistry;

	private bool _BypassOnLocal;

	private bool m_EnableAutoproxy;

	private Uri _ProxyAddress;

	private ArrayList _BypassList;

	private ICredentials _Credentials;

	private Regex[] _RegExBypassList;

	private Hashtable _ProxyHostAddresses;

	private AutoWebProxyScriptEngine m_ScriptEngine;

	public Uri Address
	{
		get
		{
			return _ProxyAddress;
		}
		set
		{
			_UseRegistry = false;
			DeleteScriptEngine();
			_ProxyHostAddresses = null;
			_ProxyAddress = value;
		}
	}

	internal bool AutoDetect
	{
		set
		{
			if (ScriptEngine == null)
			{
				ScriptEngine = new AutoWebProxyScriptEngine(this, useRegistry: false);
			}
			ScriptEngine.AutomaticallyDetectSettings = value;
		}
	}

	internal Uri ScriptLocation
	{
		set
		{
			if (ScriptEngine == null)
			{
				ScriptEngine = new AutoWebProxyScriptEngine(this, useRegistry: false);
			}
			ScriptEngine.AutomaticConfigurationScript = value;
		}
	}

	public bool BypassProxyOnLocal
	{
		get
		{
			return _BypassOnLocal;
		}
		set
		{
			_UseRegistry = false;
			DeleteScriptEngine();
			_BypassOnLocal = value;
		}
	}

	public string[] BypassList
	{
		get
		{
			if (_BypassList == null)
			{
				_BypassList = new ArrayList();
			}
			return (string[])_BypassList.ToArray(typeof(string));
		}
		set
		{
			_UseRegistry = false;
			DeleteScriptEngine();
			_BypassList = new ArrayList(value);
			UpdateRegExList(canThrow: true);
		}
	}

	public ICredentials Credentials
	{
		get
		{
			return _Credentials;
		}
		set
		{
			_Credentials = value;
		}
	}

	public bool UseDefaultCredentials
	{
		get
		{
			if (!(Credentials is SystemNetworkCredential))
			{
				return false;
			}
			return true;
		}
		set
		{
			_Credentials = (value ? CredentialCache.DefaultCredentials : null);
		}
	}

	public ArrayList BypassArrayList
	{
		get
		{
			if (_BypassList == null)
			{
				_BypassList = new ArrayList();
			}
			return _BypassList;
		}
	}

	internal AutoWebProxyScriptEngine ScriptEngine
	{
		get
		{
			return m_ScriptEngine;
		}
		set
		{
			m_ScriptEngine = value;
		}
	}

	public WebProxy()
		: this((Uri)null, BypassOnLocal: false, (string[])null, (ICredentials)null)
	{
	}

	public WebProxy(Uri Address)
		: this(Address, BypassOnLocal: false, null, null)
	{
	}

	public WebProxy(Uri Address, bool BypassOnLocal)
		: this(Address, BypassOnLocal, null, null)
	{
	}

	public WebProxy(Uri Address, bool BypassOnLocal, string[] BypassList)
		: this(Address, BypassOnLocal, BypassList, null)
	{
	}

	public WebProxy(Uri Address, bool BypassOnLocal, string[] BypassList, ICredentials Credentials)
	{
		_ProxyAddress = Address;
		_BypassOnLocal = BypassOnLocal;
		if (BypassList != null)
		{
			_BypassList = new ArrayList(BypassList);
			UpdateRegExList(canThrow: true);
		}
		_Credentials = Credentials;
		m_EnableAutoproxy = true;
	}

	public WebProxy(string Host, int Port)
		: this(new Uri("http://" + Host + ":" + Port.ToString(CultureInfo.InvariantCulture)), BypassOnLocal: false, null, null)
	{
	}

	public WebProxy(string Address)
		: this(CreateProxyUri(Address), BypassOnLocal: false, null, null)
	{
	}

	public WebProxy(string Address, bool BypassOnLocal)
		: this(CreateProxyUri(Address), BypassOnLocal, null, null)
	{
	}

	public WebProxy(string Address, bool BypassOnLocal, string[] BypassList)
		: this(CreateProxyUri(Address), BypassOnLocal, BypassList, null)
	{
	}

	public WebProxy(string Address, bool BypassOnLocal, string[] BypassList, ICredentials Credentials)
		: this(CreateProxyUri(Address), BypassOnLocal, BypassList, Credentials)
	{
	}

	internal void CheckForChanges()
	{
		if (ScriptEngine != null)
		{
			ScriptEngine.CheckForChanges();
		}
	}

	public Uri GetProxy(Uri destination)
	{
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (GetProxyAuto(destination, out var proxyUri))
		{
			return proxyUri;
		}
		if (IsBypassedManual(destination))
		{
			return destination;
		}
		Hashtable proxyHostAddresses = _ProxyHostAddresses;
		Uri uri = ((proxyHostAddresses != null) ? (proxyHostAddresses[destination.Scheme] as Uri) : _ProxyAddress);
		if (!(uri != null))
		{
			return destination;
		}
		return uri;
	}

	private static Uri CreateProxyUri(string address)
	{
		if (address == null)
		{
			return null;
		}
		if (address.IndexOf("://") == -1)
		{
			address = "http://" + address;
		}
		return new Uri(address);
	}

	private void UpdateRegExList(bool canThrow)
	{
		Regex[] array = null;
		ArrayList bypassList = _BypassList;
		try
		{
			if (bypassList != null && bypassList.Count > 0)
			{
				array = new Regex[bypassList.Count];
				for (int i = 0; i < bypassList.Count; i++)
				{
					array[i] = new Regex((string)bypassList[i], RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
				}
			}
		}
		catch
		{
			if (!canThrow)
			{
				_RegExBypassList = null;
				return;
			}
			throw;
		}
		_RegExBypassList = array;
	}

	private bool IsMatchInBypassList(Uri input)
	{
		UpdateRegExList(canThrow: false);
		if (_RegExBypassList == null)
		{
			return false;
		}
		string input2 = input.Scheme + "://" + input.Host + ((!input.IsDefaultPort) ? (":" + input.Port) : "");
		for (int i = 0; i < _BypassList.Count; i++)
		{
			if (_RegExBypassList[i].IsMatch(input2))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsLocal(Uri host)
	{
		string host2 = host.Host;
		if (IPAddress.TryParse(host2, out var address))
		{
			if (!IPAddress.IsLoopback(address))
			{
				return NclUtilities.IsAddressLocal(address);
			}
			return true;
		}
		int num = host2.IndexOf('.');
		if (num == -1)
		{
			return true;
		}
		string text = "." + IPGlobalProperties.InternalGetIPGlobalProperties().DomainName;
		if (text != null && text.Length == host2.Length - num && string.Compare(text, 0, host2, num, text.Length, StringComparison.OrdinalIgnoreCase) == 0)
		{
			return true;
		}
		return false;
	}

	private bool IsLocalInProxyHash(Uri host)
	{
		Hashtable proxyHostAddresses = _ProxyHostAddresses;
		if (proxyHostAddresses != null && (Uri)proxyHostAddresses[host.Scheme] == null)
		{
			return true;
		}
		return false;
	}

	public bool IsBypassed(Uri host)
	{
		if (host == null)
		{
			throw new ArgumentNullException("host");
		}
		if (IsBypassedAuto(host, out var isBypassed))
		{
			return isBypassed;
		}
		return IsBypassedManual(host);
	}

	private bool IsBypassedManual(Uri host)
	{
		if (host.IsLoopback)
		{
			return true;
		}
		if ((!(_ProxyAddress == null) || _ProxyHostAddresses != null) && (!_BypassOnLocal || !IsLocal(host)) && !IsMatchInBypassList(host))
		{
			return IsLocalInProxyHash(host);
		}
		return true;
	}

	[Obsolete("This method has been deprecated. Please use the proxy selected for you by default. http://go.microsoft.com/fwlink/?linkid=14202")]
	public static WebProxy GetDefaultProxy()
	{
		return new WebProxy(enableAutoproxy: true);
	}

	protected WebProxy(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		bool flag = false;
		try
		{
			flag = serializationInfo.GetBoolean("_UseRegistry");
		}
		catch
		{
		}
		if (flag)
		{
			UnsafeUpdateFromRegistry();
			return;
		}
		_ProxyAddress = (Uri)serializationInfo.GetValue("_ProxyAddress", typeof(Uri));
		_BypassOnLocal = serializationInfo.GetBoolean("_BypassOnLocal");
		_BypassList = (ArrayList)serializationInfo.GetValue("_BypassList", typeof(ArrayList));
		try
		{
			UseDefaultCredentials = serializationInfo.GetBoolean("_UseDefaultCredentials");
		}
		catch
		{
		}
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		GetObjectData(serializationInfo, streamingContext);
	}

	[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
	protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
	{
		serializationInfo.AddValue("_BypassOnLocal", _BypassOnLocal);
		serializationInfo.AddValue("_ProxyAddress", _ProxyAddress);
		serializationInfo.AddValue("_BypassList", _BypassList);
		serializationInfo.AddValue("_UseDefaultCredentials", UseDefaultCredentials);
		if (_UseRegistry)
		{
			serializationInfo.AddValue("_UseRegistry", value: true);
		}
	}

	public static IWebProxy CreateDefaultProxy()
	{
		IWebProxy defaultProxy = AndroidPlatform.GetDefaultProxy();
		if (defaultProxy != null)
		{
			return defaultProxy;
		}
		return new WebProxy(enableAutoproxy: true);
	}

	internal WebProxy(bool enableAutoproxy)
	{
		m_EnableAutoproxy = enableAutoproxy;
		UnsafeUpdateFromRegistry();
	}

	internal void DeleteScriptEngine()
	{
		if (ScriptEngine != null)
		{
			ScriptEngine.Close();
			ScriptEngine = null;
		}
	}

	internal void UnsafeUpdateFromRegistry()
	{
		_UseRegistry = true;
	}

	internal void Update(WebProxyData webProxyData)
	{
		lock (this)
		{
			_BypassOnLocal = webProxyData.bypassOnLocal;
			_ProxyAddress = webProxyData.proxyAddress;
			_ProxyHostAddresses = webProxyData.proxyHostAddresses;
			_BypassList = webProxyData.bypassList;
			ScriptEngine.AutomaticallyDetectSettings = m_EnableAutoproxy && webProxyData.automaticallyDetectSettings;
			ScriptEngine.AutomaticConfigurationScript = (m_EnableAutoproxy ? webProxyData.scriptLocation : null);
		}
	}

	ProxyChain IAutoWebProxy.GetProxies(Uri destination)
	{
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		return new ProxyScriptChain(this, destination);
	}

	private bool GetProxyAuto(Uri destination, out Uri proxyUri)
	{
		proxyUri = null;
		if (ScriptEngine == null)
		{
			return false;
		}
		IList<string> proxyList = null;
		if (!ScriptEngine.GetProxies(destination, out proxyList))
		{
			return false;
		}
		if (proxyList.Count > 0)
		{
			if (AreAllBypassed(proxyList, checkFirstOnly: true))
			{
				proxyUri = destination;
			}
			else
			{
				proxyUri = ProxyUri(proxyList[0]);
			}
		}
		return true;
	}

	private bool IsBypassedAuto(Uri destination, out bool isBypassed)
	{
		isBypassed = true;
		if (ScriptEngine == null)
		{
			return false;
		}
		if (!ScriptEngine.GetProxies(destination, out var proxyList))
		{
			return false;
		}
		if (proxyList.Count == 0)
		{
			isBypassed = false;
		}
		else
		{
			isBypassed = AreAllBypassed(proxyList, checkFirstOnly: true);
		}
		return true;
	}

	internal Uri[] GetProxiesAuto(Uri destination, ref int syncStatus)
	{
		if (ScriptEngine == null)
		{
			return null;
		}
		IList<string> proxyList = null;
		if (!ScriptEngine.GetProxies(destination, out proxyList, ref syncStatus))
		{
			return null;
		}
		Uri[] array = null;
		if (proxyList.Count == 0)
		{
			array = new Uri[0];
		}
		else if (AreAllBypassed(proxyList, checkFirstOnly: false))
		{
			array = new Uri[1];
		}
		else
		{
			array = new Uri[proxyList.Count];
			for (int i = 0; i < proxyList.Count; i++)
			{
				array[i] = ProxyUri(proxyList[i]);
			}
		}
		return array;
	}

	internal void AbortGetProxiesAuto(ref int syncStatus)
	{
		if (ScriptEngine != null)
		{
			ScriptEngine.Abort(ref syncStatus);
		}
	}

	internal Uri GetProxyAutoFailover(Uri destination)
	{
		if (IsBypassedManual(destination))
		{
			return null;
		}
		Uri result = _ProxyAddress;
		Hashtable proxyHostAddresses = _ProxyHostAddresses;
		if (proxyHostAddresses != null)
		{
			result = proxyHostAddresses[destination.Scheme] as Uri;
		}
		return result;
	}

	private static bool AreAllBypassed(IEnumerable<string> proxies, bool checkFirstOnly)
	{
		bool flag = true;
		foreach (string proxy in proxies)
		{
			flag = string.IsNullOrEmpty(proxy);
			if (checkFirstOnly || !flag)
			{
				break;
			}
		}
		return flag;
	}

	private static Uri ProxyUri(string proxyName)
	{
		if (proxyName != null && proxyName.Length != 0)
		{
			return new Uri("http://" + proxyName);
		}
		return null;
	}
}
