using System;

namespace UnityEngine.Lumin;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class UsesLuminPrivilegeAttribute : Attribute
{
	private readonly string m_Privilege;

	public string privilege => m_Privilege;

	public UsesLuminPrivilegeAttribute(string privilege)
	{
		m_Privilege = privilege;
	}
}
