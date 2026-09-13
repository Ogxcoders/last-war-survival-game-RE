using System.Security;
using System.Security.Permissions;

namespace System.Net;

[Serializable]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class WebPermissionAttribute : CodeAccessSecurityAttribute
{
	private object m_accept;

	private object m_connect;

	public string Connect
	{
		get
		{
			return m_connect as string;
		}
		set
		{
			if (m_connect != null)
			{
				throw new ArgumentException(global::SR.GetString("The permission '{0}={1}' cannot be added. Add a separate Attribute statement.", "Connect", value), "value");
			}
			m_connect = value;
		}
	}

	public string Accept
	{
		get
		{
			return m_accept as string;
		}
		set
		{
			if (m_accept != null)
			{
				throw new ArgumentException(global::SR.GetString("The permission '{0}={1}' cannot be added. Add a separate Attribute statement.", "Accept", value), "value");
			}
			m_accept = value;
		}
	}

	public string ConnectPattern
	{
		get
		{
			if (!(m_connect is DelayedRegex))
			{
				if (!(m_connect is bool) || !(bool)m_connect)
				{
					return null;
				}
				return ".*";
			}
			return m_connect.ToString();
		}
		set
		{
			if (m_connect != null)
			{
				throw new ArgumentException(global::SR.GetString("The permission '{0}={1}' cannot be added. Add a separate Attribute statement.", "ConnectPatern", value), "value");
			}
			if (value == ".*")
			{
				m_connect = true;
			}
			else
			{
				m_connect = new DelayedRegex(value);
			}
		}
	}

	public string AcceptPattern
	{
		get
		{
			if (!(m_accept is DelayedRegex))
			{
				if (!(m_accept is bool) || !(bool)m_accept)
				{
					return null;
				}
				return ".*";
			}
			return m_accept.ToString();
		}
		set
		{
			if (m_accept != null)
			{
				throw new ArgumentException(global::SR.GetString("The permission '{0}={1}' cannot be added. Add a separate Attribute statement.", "AcceptPattern", value), "value");
			}
			if (value == ".*")
			{
				m_accept = true;
			}
			else
			{
				m_accept = new DelayedRegex(value);
			}
		}
	}

	public WebPermissionAttribute(SecurityAction action)
		: base(action)
	{
	}

	public override IPermission CreatePermission()
	{
		WebPermission webPermission = null;
		if (base.Unrestricted)
		{
			webPermission = new WebPermission(PermissionState.Unrestricted);
		}
		else
		{
			NetworkAccess networkAccess = (NetworkAccess)0;
			if (m_connect is bool)
			{
				if ((bool)m_connect)
				{
					networkAccess |= NetworkAccess.Connect;
				}
				m_connect = null;
			}
			if (m_accept is bool)
			{
				if ((bool)m_accept)
				{
					networkAccess |= NetworkAccess.Accept;
				}
				m_accept = null;
			}
			webPermission = new WebPermission(networkAccess);
			if (m_accept != null)
			{
				if (m_accept is DelayedRegex)
				{
					webPermission.AddAsPattern(NetworkAccess.Accept, (DelayedRegex)m_accept);
				}
				else
				{
					webPermission.AddPermission(NetworkAccess.Accept, (string)m_accept);
				}
			}
			if (m_connect != null)
			{
				if (m_connect is DelayedRegex)
				{
					webPermission.AddAsPattern(NetworkAccess.Connect, (DelayedRegex)m_connect);
				}
				else
				{
					webPermission.AddPermission(NetworkAccess.Connect, (string)m_connect);
				}
			}
		}
		return webPermission;
	}
}
