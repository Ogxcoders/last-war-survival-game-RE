using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security.Claims;

[ComVisible(false)]
internal class RoleClaimProvider
{
	private string m_issuer;

	private string[] m_roles;

	private ClaimsIdentity m_subject;

	public IEnumerable<Claim> Claims
	{
		get
		{
			for (int i = 0; i < m_roles.Length; i++)
			{
				if (m_roles[i] != null)
				{
					yield return new Claim(m_subject.RoleClaimType, m_roles[i], "http://www.w3.org/2001/XMLSchema#string", m_issuer, m_issuer, m_subject);
				}
			}
		}
	}

	public RoleClaimProvider(string issuer, string[] roles, ClaimsIdentity subject)
	{
		m_issuer = issuer;
		m_roles = roles;
		m_subject = subject;
	}
}
