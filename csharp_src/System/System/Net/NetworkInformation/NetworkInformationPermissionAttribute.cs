using System.Security;
using System.Security.Permissions;

namespace System.Net.NetworkInformation;

[Serializable]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class NetworkInformationPermissionAttribute : CodeAccessSecurityAttribute
{
	private const string strAccess = "Access";

	private string access;

	public string Access
	{
		get
		{
			return access;
		}
		set
		{
			access = value;
		}
	}

	public NetworkInformationPermissionAttribute(SecurityAction action)
		: base(action)
	{
	}

	public override IPermission CreatePermission()
	{
		NetworkInformationPermission networkInformationPermission = null;
		if (base.Unrestricted)
		{
			networkInformationPermission = new NetworkInformationPermission(PermissionState.Unrestricted);
		}
		else
		{
			networkInformationPermission = new NetworkInformationPermission(PermissionState.None);
			if (access != null)
			{
				if (string.Compare(access, "Read", StringComparison.OrdinalIgnoreCase) == 0)
				{
					networkInformationPermission.AddPermission(NetworkInformationAccess.Read);
				}
				else if (string.Compare(access, "Ping", StringComparison.OrdinalIgnoreCase) == 0)
				{
					networkInformationPermission.AddPermission(NetworkInformationAccess.Ping);
				}
				else
				{
					if (string.Compare(access, "None", StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new ArgumentException(global::SR.GetString("The parameter value '{0}={1}' is invalid.", "Access", access));
					}
					networkInformationPermission.AddPermission(NetworkInformationAccess.None);
				}
			}
		}
		return networkInformationPermission;
	}
}
