using System.Collections;
using System.Security;
using System.Security.Permissions;

namespace System.Net;

[Serializable]
public sealed class SocketPermission : CodeAccessPermission, IUnrestrictedPermission
{
	private ArrayList m_acceptList = new ArrayList();

	private ArrayList m_connectList = new ArrayList();

	private bool m_noRestriction;

	public const int AllPorts = -1;

	public IEnumerator AcceptList => m_acceptList.GetEnumerator();

	public IEnumerator ConnectList => m_connectList.GetEnumerator();

	public SocketPermission(PermissionState state)
	{
		m_noRestriction = state == PermissionState.Unrestricted;
	}

	public SocketPermission(NetworkAccess access, TransportType transport, string hostName, int portNumber)
	{
		m_noRestriction = false;
		AddPermission(access, transport, hostName, portNumber);
	}

	public void AddPermission(NetworkAccess access, TransportType transport, string hostName, int portNumber)
	{
		if (!m_noRestriction)
		{
			EndpointPermission value = new EndpointPermission(hostName, portNumber, transport);
			if (access == NetworkAccess.Accept)
			{
				m_acceptList.Add(value);
			}
			else
			{
				m_connectList.Add(value);
			}
		}
	}

	public override IPermission Copy()
	{
		return new SocketPermission(m_noRestriction ? PermissionState.Unrestricted : PermissionState.None)
		{
			m_connectList = (ArrayList)m_connectList.Clone(),
			m_acceptList = (ArrayList)m_acceptList.Clone()
		};
	}

	public override IPermission Intersect(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		if (!(target is SocketPermission socketPermission))
		{
			throw new ArgumentException("Argument not of type SocketPermission");
		}
		if (m_noRestriction)
		{
			if (!IntersectEmpty(socketPermission))
			{
				return socketPermission.Copy();
			}
			return null;
		}
		if (socketPermission.m_noRestriction)
		{
			if (!IntersectEmpty(this))
			{
				return Copy();
			}
			return null;
		}
		SocketPermission socketPermission2 = new SocketPermission(PermissionState.None);
		Intersect(m_connectList, socketPermission.m_connectList, socketPermission2.m_connectList);
		Intersect(m_acceptList, socketPermission.m_acceptList, socketPermission2.m_acceptList);
		if (!IntersectEmpty(socketPermission2))
		{
			return socketPermission2;
		}
		return null;
	}

	private bool IntersectEmpty(SocketPermission permission)
	{
		if (!permission.m_noRestriction && permission.m_connectList.Count == 0)
		{
			return permission.m_acceptList.Count == 0;
		}
		return false;
	}

	private void Intersect(ArrayList list1, ArrayList list2, ArrayList result)
	{
		foreach (EndpointPermission item in list1)
		{
			foreach (EndpointPermission item2 in list2)
			{
				EndpointPermission endpointPermission2 = item.Intersect(item2);
				if (endpointPermission2 == null)
				{
					continue;
				}
				bool flag = false;
				for (int i = 0; i < result.Count; i++)
				{
					EndpointPermission perm2 = (EndpointPermission)result[i];
					EndpointPermission endpointPermission3 = endpointPermission2.Intersect(perm2);
					if (endpointPermission3 != null)
					{
						result[i] = endpointPermission3;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result.Add(endpointPermission2);
				}
			}
		}
	}

	public override bool IsSubsetOf(IPermission target)
	{
		if (target == null)
		{
			if (!m_noRestriction && m_connectList.Count == 0)
			{
				return m_acceptList.Count == 0;
			}
			return false;
		}
		if (!(target is SocketPermission socketPermission))
		{
			throw new ArgumentException("Parameter target must be of type SocketPermission");
		}
		if (socketPermission.m_noRestriction)
		{
			return true;
		}
		if (m_noRestriction)
		{
			return false;
		}
		if (m_acceptList.Count == 0 && m_connectList.Count == 0)
		{
			return true;
		}
		if (socketPermission.m_acceptList.Count == 0 && socketPermission.m_connectList.Count == 0)
		{
			return false;
		}
		if (IsSubsetOf(m_connectList, socketPermission.m_connectList))
		{
			return IsSubsetOf(m_acceptList, socketPermission.m_acceptList);
		}
		return false;
	}

	private bool IsSubsetOf(ArrayList list1, ArrayList list2)
	{
		foreach (EndpointPermission item in list1)
		{
			bool flag = false;
			foreach (EndpointPermission item2 in list2)
			{
				if (item.IsSubsetOf(item2))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsUnrestricted()
	{
		return m_noRestriction;
	}

	public override SecurityElement ToXml()
	{
		SecurityElement securityElement = new SecurityElement("IPermission");
		securityElement.AddAttribute("class", GetType().AssemblyQualifiedName);
		securityElement.AddAttribute("version", "1");
		if (m_noRestriction)
		{
			securityElement.AddAttribute("Unrestricted", "true");
			return securityElement;
		}
		if (m_connectList.Count > 0)
		{
			ToXml(securityElement, "ConnectAccess", m_connectList.GetEnumerator());
		}
		if (m_acceptList.Count > 0)
		{
			ToXml(securityElement, "AcceptAccess", m_acceptList.GetEnumerator());
		}
		return securityElement;
	}

	private void ToXml(SecurityElement root, string childName, IEnumerator enumerator)
	{
		SecurityElement securityElement = new SecurityElement(childName);
		while (enumerator.MoveNext())
		{
			EndpointPermission endpointPermission = enumerator.Current as EndpointPermission;
			SecurityElement securityElement2 = new SecurityElement("ENDPOINT");
			securityElement2.AddAttribute("host", endpointPermission.Hostname);
			securityElement2.AddAttribute("transport", endpointPermission.Transport.ToString());
			securityElement2.AddAttribute("port", (endpointPermission.Port == -1) ? "All" : endpointPermission.Port.ToString());
			securityElement.AddChild(securityElement2);
		}
		root.AddChild(securityElement);
	}

	public override void FromXml(SecurityElement securityElement)
	{
		if (securityElement == null)
		{
			throw new ArgumentNullException("securityElement");
		}
		if (securityElement.Tag != "IPermission")
		{
			throw new ArgumentException("securityElement");
		}
		string text = securityElement.Attribute("Unrestricted");
		if (text != null)
		{
			m_noRestriction = string.Compare(text, "true", ignoreCase: true) == 0;
			if (m_noRestriction)
			{
				return;
			}
		}
		m_noRestriction = false;
		m_connectList = new ArrayList();
		m_acceptList = new ArrayList();
		foreach (SecurityElement child in securityElement.Children)
		{
			if (child.Tag == "ConnectAccess")
			{
				FromXml(child.Children, NetworkAccess.Connect);
			}
			else if (child.Tag == "AcceptAccess")
			{
				FromXml(child.Children, NetworkAccess.Accept);
			}
		}
	}

	private void FromXml(ArrayList endpoints, NetworkAccess access)
	{
		foreach (SecurityElement endpoint in endpoints)
		{
			if (!(endpoint.Tag != "ENDPOINT"))
			{
				string hostName = endpoint.Attribute("host");
				TransportType transport = (TransportType)Enum.Parse(typeof(TransportType), endpoint.Attribute("transport"), ignoreCase: true);
				string text = endpoint.Attribute("port");
				int num = 0;
				num = ((!(text == "All")) ? int.Parse(text) : (-1));
				AddPermission(access, transport, hostName, num);
			}
		}
	}

	public override IPermission Union(IPermission target)
	{
		if (target == null)
		{
			return null;
		}
		if (!(target is SocketPermission socketPermission))
		{
			throw new ArgumentException("Argument not of type SocketPermission");
		}
		if (m_noRestriction || socketPermission.m_noRestriction)
		{
			return new SocketPermission(PermissionState.Unrestricted);
		}
		SocketPermission socketPermission2 = (SocketPermission)socketPermission.Copy();
		socketPermission2.m_acceptList.InsertRange(socketPermission2.m_acceptList.Count, m_acceptList);
		socketPermission2.m_connectList.InsertRange(socketPermission2.m_connectList.Count, m_connectList);
		return socketPermission2;
	}
}
