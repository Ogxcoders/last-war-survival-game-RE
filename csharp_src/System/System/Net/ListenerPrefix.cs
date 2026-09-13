namespace System.Net;

internal sealed class ListenerPrefix
{
	private string original;

	private string host;

	private ushort port;

	private string path;

	private bool secure;

	private IPAddress[] addresses;

	public HttpListener Listener;

	public IPAddress[] Addresses
	{
		get
		{
			return addresses;
		}
		set
		{
			addresses = value;
		}
	}

	public bool Secure => secure;

	public string Host => host;

	public int Port => port;

	public string Path => path;

	public ListenerPrefix(string prefix)
	{
		original = prefix;
		Parse(prefix);
	}

	public override string ToString()
	{
		return original;
	}

	public override bool Equals(object o)
	{
		if (!(o is ListenerPrefix listenerPrefix))
		{
			return false;
		}
		return original == listenerPrefix.original;
	}

	public override int GetHashCode()
	{
		return original.GetHashCode();
	}

	private void Parse(string uri)
	{
		ushort num = 80;
		if (uri.StartsWith("https://"))
		{
			num = 443;
			secure = true;
		}
		int length = uri.Length;
		int num2 = uri.IndexOf(':') + 3;
		if (num2 >= length)
		{
			throw new ArgumentException("No host specified.");
		}
		int num3 = uri.IndexOf(':', num2, length - num2);
		if (uri[num2] == '[')
		{
			num3 = uri.IndexOf("]:") + 1;
		}
		if (num2 == num3)
		{
			throw new ArgumentException("No host specified.");
		}
		int num4 = uri.IndexOf('/', num2, length - num2);
		if (num4 == -1)
		{
			throw new ArgumentException("No path specified.");
		}
		if (num3 > 0)
		{
			host = uri.Substring(num2, num3 - num2).Trim('[', ']');
			port = ushort.Parse(uri.Substring(num3 + 1, num4 - num3 - 1));
		}
		else
		{
			host = uri.Substring(num2, num4 - num2).Trim('[', ']');
			port = num;
		}
		path = uri.Substring(num4);
		if (path.Length != 1)
		{
			path = path.Substring(0, path.Length - 1);
		}
	}

	public static void CheckUri(string uri)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uriPrefix");
		}
		if (!uri.StartsWith("http://") && !uri.StartsWith("https://"))
		{
			throw new ArgumentException("Only 'http' and 'https' schemes are supported.");
		}
		int length = uri.Length;
		int num = uri.IndexOf(':') + 3;
		if (num >= length)
		{
			throw new ArgumentException("No host specified.");
		}
		int num2 = uri.IndexOf(':', num, length - num);
		if (uri[num] == '[')
		{
			num2 = uri.IndexOf("]:") + 1;
		}
		if (num == num2)
		{
			throw new ArgumentException("No host specified.");
		}
		int num3 = uri.IndexOf('/', num, length - num);
		if (num3 == -1)
		{
			throw new ArgumentException("No path specified.");
		}
		if (num2 > 0)
		{
			try
			{
				int num4 = int.Parse(uri.Substring(num2 + 1, num3 - num2 - 1));
				if (num4 <= 0 || num4 >= 65536)
				{
					throw new Exception();
				}
			}
			catch
			{
				throw new ArgumentException("Invalid port.");
			}
		}
		if (uri[uri.Length - 1] != '/')
		{
			throw new ArgumentException("The prefix must end with '/'");
		}
	}
}
