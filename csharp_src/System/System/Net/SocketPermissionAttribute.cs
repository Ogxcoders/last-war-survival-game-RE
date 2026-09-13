using System.Security;
using System.Security.Permissions;

namespace System.Net;

[Serializable]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class SocketPermissionAttribute : CodeAccessSecurityAttribute
{
	private string m_access;

	private string m_host;

	private string m_port;

	private string m_transport;

	public string Access
	{
		get
		{
			return m_access;
		}
		set
		{
			if (m_access != null)
			{
				AlreadySet("Access");
			}
			m_access = value;
		}
	}

	public string Host
	{
		get
		{
			return m_host;
		}
		set
		{
			if (m_host != null)
			{
				AlreadySet("Host");
			}
			m_host = value;
		}
	}

	public string Port
	{
		get
		{
			return m_port;
		}
		set
		{
			if (m_port != null)
			{
				AlreadySet("Port");
			}
			m_port = value;
		}
	}

	public string Transport
	{
		get
		{
			return m_transport;
		}
		set
		{
			if (m_transport != null)
			{
				AlreadySet("Transport");
			}
			m_transport = value;
		}
	}

	public SocketPermissionAttribute(SecurityAction action)
		: base(action)
	{
	}

	public override IPermission CreatePermission()
	{
		if (base.Unrestricted)
		{
			return new SocketPermission(PermissionState.Unrestricted);
		}
		string text = string.Empty;
		if (m_access == null)
		{
			text += "Access, ";
		}
		if (m_host == null)
		{
			text += "Host, ";
		}
		if (m_port == null)
		{
			text += "Port, ";
		}
		if (m_transport == null)
		{
			text += "Transport, ";
		}
		if (text.Length > 0)
		{
			string text2 = Locale.GetText("The value(s) for {0} must be specified.");
			text = text.Substring(0, text.Length - 2);
			throw new ArgumentException(string.Format(text2, text));
		}
		int num = -1;
		NetworkAccess access;
		if (string.Compare(m_access, "Connect", ignoreCase: true) == 0)
		{
			access = NetworkAccess.Connect;
		}
		else
		{
			if (string.Compare(m_access, "Accept", ignoreCase: true) != 0)
			{
				throw new ArgumentException(string.Format(Locale.GetText("The parameter value for 'Access', '{1}, is invalid."), m_access));
			}
			access = NetworkAccess.Accept;
		}
		if (string.Compare(m_port, "All", ignoreCase: true) != 0)
		{
			try
			{
				num = int.Parse(m_port);
			}
			catch
			{
				throw new ArgumentException(string.Format(Locale.GetText("The parameter value for 'Port', '{1}, is invalid."), m_port));
			}
			new IPEndPoint(1L, num);
		}
		TransportType transport;
		try
		{
			transport = (TransportType)Enum.Parse(typeof(TransportType), m_transport, ignoreCase: true);
		}
		catch
		{
			throw new ArgumentException(string.Format(Locale.GetText("The parameter value for 'Transport', '{1}, is invalid."), m_transport));
		}
		SocketPermission socketPermission = new SocketPermission(PermissionState.None);
		socketPermission.AddPermission(access, transport, m_host, num);
		return socketPermission;
	}

	internal void AlreadySet(string property)
	{
		throw new ArgumentException(string.Format(Locale.GetText("The parameter '{0}' can be set only once."), property), property);
	}
}
