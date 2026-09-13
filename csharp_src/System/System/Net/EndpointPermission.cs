using System.Net.Sockets;

namespace System.Net;

[Serializable]
public class EndpointPermission
{
	private static char[] dot_char = new char[1] { '.' };

	private string hostname;

	private int port;

	private TransportType transport;

	private bool resolved;

	private bool hasWildcard;

	private IPAddress[] addresses;

	public string Hostname => hostname;

	public int Port => port;

	public TransportType Transport => transport;

	internal EndpointPermission(string hostname, int port, TransportType transport)
	{
		if (hostname == null)
		{
			throw new ArgumentNullException("hostname");
		}
		this.hostname = hostname;
		this.port = port;
		this.transport = transport;
		resolved = false;
		hasWildcard = false;
		addresses = null;
	}

	public override bool Equals(object obj)
	{
		if (obj is EndpointPermission endpointPermission && port == endpointPermission.port && transport == endpointPermission.transport)
		{
			return string.Compare(hostname, endpointPermission.hostname, ignoreCase: true) == 0;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public override string ToString()
	{
		string[] obj = new string[5]
		{
			hostname,
			"#",
			port.ToString(),
			"#",
			null
		};
		int num = (int)transport;
		obj[4] = num.ToString();
		return string.Concat(obj);
	}

	internal bool IsSubsetOf(EndpointPermission perm)
	{
		if (perm == null)
		{
			return false;
		}
		if (perm.port != -1 && port != perm.port)
		{
			return false;
		}
		if (perm.transport != TransportType.All && transport != perm.transport)
		{
			return false;
		}
		Resolve();
		perm.Resolve();
		if (hasWildcard)
		{
			if (perm.hasWildcard)
			{
				return IsSubsetOf(hostname, perm.hostname);
			}
			return false;
		}
		if (addresses == null)
		{
			return false;
		}
		IPAddress[] array;
		if (perm.hasWildcard)
		{
			array = addresses;
			foreach (IPAddress iPAddress in array)
			{
				if (IsSubsetOf(iPAddress.ToString(), perm.hostname))
				{
					return true;
				}
			}
		}
		if (perm.addresses == null)
		{
			return false;
		}
		array = perm.addresses;
		foreach (IPAddress iPAddress2 in array)
		{
			if (IsSubsetOf(hostname, iPAddress2.ToString()))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSubsetOf(string addr1, string addr2)
	{
		string[] array = addr1.Split(dot_char);
		string[] array2 = addr2.Split(dot_char);
		for (int i = 0; i < 4; i++)
		{
			int num = ToNumber(array[i]);
			if (num == -1)
			{
				return false;
			}
			int num2 = ToNumber(array2[i]);
			if (num2 == -1)
			{
				return false;
			}
			if (num != 256 && num != num2 && num2 != 256)
			{
				return false;
			}
		}
		return true;
	}

	internal EndpointPermission Intersect(EndpointPermission perm)
	{
		if (perm == null)
		{
			return null;
		}
		int num;
		if (port == perm.port)
		{
			num = port;
		}
		else if (port == -1)
		{
			num = perm.port;
		}
		else
		{
			if (perm.port != -1)
			{
				return null;
			}
			num = port;
		}
		TransportType transportType;
		if (transport == perm.transport)
		{
			transportType = transport;
		}
		else if (transport == TransportType.All)
		{
			transportType = perm.transport;
		}
		else
		{
			if (perm.transport != TransportType.All)
			{
				return null;
			}
			transportType = transport;
		}
		string text = IntersectHostname(perm);
		if (text == null)
		{
			return null;
		}
		if (!hasWildcard)
		{
			return this;
		}
		if (!perm.hasWildcard)
		{
			return perm;
		}
		return new EndpointPermission(text, num, transportType)
		{
			hasWildcard = true,
			resolved = true
		};
	}

	private string IntersectHostname(EndpointPermission perm)
	{
		if (hostname == perm.hostname)
		{
			return hostname;
		}
		Resolve();
		perm.Resolve();
		string text = null;
		if (hasWildcard)
		{
			if (perm.hasWildcard)
			{
				text = Intersect(hostname, perm.hostname);
			}
			else if (perm.addresses != null)
			{
				for (int i = 0; i < perm.addresses.Length; i++)
				{
					text = Intersect(hostname, perm.addresses[i].ToString());
					if (text != null)
					{
						break;
					}
				}
			}
		}
		else if (addresses != null)
		{
			for (int j = 0; j < addresses.Length; j++)
			{
				string addr = addresses[j].ToString();
				if (perm.hasWildcard)
				{
					text = Intersect(addr, perm.hostname);
				}
				else
				{
					if (perm.addresses == null)
					{
						continue;
					}
					for (int k = 0; k < perm.addresses.Length; k++)
					{
						text = Intersect(addr, perm.addresses[k].ToString());
						if (text != null)
						{
							break;
						}
					}
				}
			}
		}
		return text;
	}

	private string Intersect(string addr1, string addr2)
	{
		string[] array = addr1.Split(dot_char);
		string[] array2 = addr2.Split(dot_char);
		string[] array3 = new string[7];
		for (int i = 0; i < 4; i++)
		{
			int num = ToNumber(array[i]);
			if (num == -1)
			{
				return null;
			}
			int num2 = ToNumber(array2[i]);
			if (num2 == -1)
			{
				return null;
			}
			if (num == 256)
			{
				array3[i << 1] = ((num2 == 256) ? "*" : (string.Empty + num2));
				continue;
			}
			if (num2 == 256)
			{
				array3[i << 1] = ((num == 256) ? "*" : (string.Empty + num));
				continue;
			}
			if (num == num2)
			{
				array3[i << 1] = string.Empty + num;
				continue;
			}
			return null;
		}
		array3[1] = (array3[3] = (array3[5] = "."));
		return string.Concat(array3);
	}

	private int ToNumber(string value)
	{
		if (value == "*")
		{
			return 256;
		}
		int length = value.Length;
		if (length < 1 || length > 3)
		{
			return -1;
		}
		int num = 0;
		for (int i = 0; i < length; i++)
		{
			char c = value[i];
			if ('0' <= c && c <= '9')
			{
				num = checked(num * 10 + (c - 48));
				continue;
			}
			return -1;
		}
		if (num > 255)
		{
			return -1;
		}
		return num;
	}

	internal void Resolve()
	{
		if (resolved)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		addresses = null;
		string[] array = hostname.Split(dot_char);
		if (array.Length != 4)
		{
			flag = true;
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				switch (ToNumber(array[i]))
				{
				case -1:
					break;
				case 256:
					flag2 = true;
					continue;
				default:
					continue;
				}
				flag = true;
				break;
			}
		}
		if (flag)
		{
			hasWildcard = false;
			try
			{
				addresses = Dns.GetHostAddresses(hostname);
			}
			catch (SocketException)
			{
			}
		}
		else
		{
			hasWildcard = flag2;
			if (!flag2)
			{
				addresses = new IPAddress[1];
				addresses[0] = IPAddress.Parse(hostname);
			}
		}
		resolved = true;
	}

	internal void UndoResolve()
	{
		resolved = false;
	}
}
