using System.Collections.Generic;
using System.Net.Sockets;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net;

internal class ServiceNameStore
{
	private List<string> serviceNames;

	private ServiceNameCollection serviceNameCollection;

	public ServiceNameCollection ServiceNames
	{
		get
		{
			if (serviceNameCollection == null)
			{
				serviceNameCollection = new ServiceNameCollection(serviceNames);
			}
			return serviceNameCollection;
		}
	}

	public ServiceNameStore()
	{
		serviceNames = new List<string>();
		serviceNameCollection = null;
	}

	private bool AddSingleServiceName(string spn)
	{
		spn = ServiceNameCollection.NormalizeServiceName(spn);
		if (Contains(spn))
		{
			return false;
		}
		serviceNames.Add(spn);
		return true;
	}

	public bool Add(string uriPrefix)
	{
		string[] array = BuildServiceNames(uriPrefix);
		bool flag = false;
		string[] array2 = array;
		foreach (string spn in array2)
		{
			if (AddSingleServiceName(spn))
			{
				flag = true;
				_ = Logging.On;
			}
		}
		if (flag)
		{
			serviceNameCollection = null;
		}
		else
		{
			_ = Logging.On;
		}
		return flag;
	}

	public bool Remove(string uriPrefix)
	{
		string inputServiceName = BuildSimpleServiceName(uriPrefix);
		inputServiceName = ServiceNameCollection.NormalizeServiceName(inputServiceName);
		bool flag = Contains(inputServiceName);
		if (flag)
		{
			serviceNames.Remove(inputServiceName);
			serviceNameCollection = null;
		}
		if (Logging.On)
		{
		}
		return flag;
	}

	private bool Contains(string newServiceName)
	{
		if (newServiceName == null)
		{
			return false;
		}
		return ServiceNameCollection.Contains(newServiceName, serviceNames);
	}

	public void Clear()
	{
		serviceNames.Clear();
		serviceNameCollection = null;
	}

	private string ExtractHostname(string uriPrefix, bool allowInvalidUriStrings)
	{
		if (Uri.IsWellFormedUriString(uriPrefix, UriKind.Absolute))
		{
			return new Uri(uriPrefix).Host;
		}
		if (allowInvalidUriStrings)
		{
			int num = uriPrefix.IndexOf("://") + 3;
			int i = num;
			for (bool flag = false; i < uriPrefix.Length && uriPrefix[i] != '/' && (uriPrefix[i] != ':' || flag); i++)
			{
				if (uriPrefix[i] == '[')
				{
					if (flag)
					{
						i = num;
						break;
					}
					flag = true;
				}
				if (flag && uriPrefix[i] == ']')
				{
					flag = false;
				}
			}
			return uriPrefix.Substring(num, i - num);
		}
		return null;
	}

	public string BuildSimpleServiceName(string uriPrefix)
	{
		string text = ExtractHostname(uriPrefix, allowInvalidUriStrings: false);
		if (text != null)
		{
			return "HTTP/" + text;
		}
		return null;
	}

	public string[] BuildServiceNames(string uriPrefix)
	{
		string text = ExtractHostname(uriPrefix, allowInvalidUriStrings: true);
		IPAddress address = null;
		if (string.Compare(text, "*", StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(text, "+", StringComparison.InvariantCultureIgnoreCase) == 0 || IPAddress.TryParse(text, out address))
		{
			try
			{
				string hostName = Dns.GetHostEntry(string.Empty).HostName;
				return new string[1] { "HTTP/" + hostName };
			}
			catch (SocketException)
			{
				return new string[0];
			}
			catch (SecurityException)
			{
				return new string[0];
			}
		}
		if (!text.Contains("."))
		{
			try
			{
				string hostName2 = Dns.GetHostEntry(text).HostName;
				return new string[2]
				{
					"HTTP/" + text,
					"HTTP/" + hostName2
				};
			}
			catch (SocketException)
			{
				return new string[1] { "HTTP/" + text };
			}
			catch (SecurityException)
			{
				return new string[1] { "HTTP/" + text };
			}
		}
		return new string[1] { "HTTP/" + text };
	}
}
