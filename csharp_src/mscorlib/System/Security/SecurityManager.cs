using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Security;

[ComVisible(true)]
public static class SecurityManager
{
	[Obsolete]
	public static bool CheckExecutionRights
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Obsolete("The security manager cannot be turned off on MS runtime")]
	public static bool SecurityEnabled
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	internal static bool HasElevatedPermissions => true;

	internal static PolicyLevel ResolvingPolicyLevel
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	internal static bool CheckElevatedPermissions()
	{
		return true;
	}

	internal static void EnsureElevatedPermissions()
	{
	}

	[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000")]
	public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
	{
		zone = new ArrayList();
		origin = new ArrayList();
	}

	[Obsolete]
	public static bool IsGranted(IPermission perm)
	{
		return true;
	}

	[Obsolete]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
	public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
	public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
	public static IEnumerator PolicyHierarchy()
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	public static PermissionSet ResolvePolicy(Evidence evidence)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	public static PermissionSet ResolvePolicy(Evidence[] evidences)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	public static PermissionSet ResolveSystemPolicy(Evidence evidence)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	public static IEnumerator ResolvePolicyGroups(Evidence evidence)
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
	public static void SavePolicy()
	{
		throw new NotSupportedException();
	}

	[Obsolete]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
	public static void SavePolicyLevel(PolicyLevel level)
	{
		throw new NotSupportedException();
	}

	internal static bool ResolvePolicyLevel(ref PermissionSet ps, PolicyLevel pl, Evidence evidence)
	{
		throw new NotSupportedException();
	}

	internal static void ResolveIdentityPermissions(PermissionSet ps, Evidence evidence)
	{
		throw new NotSupportedException();
	}

	internal static IPermission CheckPermissionSet(Assembly a, PermissionSet ps, bool noncas)
	{
		return null;
	}

	internal static IPermission CheckPermissionSet(AppDomain ad, PermissionSet ps)
	{
		return null;
	}

	internal static PermissionSet Decode(IntPtr permissions, int length)
	{
		throw new NotSupportedException();
	}

	internal static PermissionSet Decode(byte[] encodedPermissions)
	{
		throw new NotSupportedException();
	}

	public static PermissionSet GetStandardSandbox(Evidence evidence)
	{
		if (evidence == null)
		{
			throw new ArgumentNullException("evidence");
		}
		throw new NotImplementedException();
	}

	public static bool CurrentThreadRequiresSecurityContextCapture()
	{
		throw new NotImplementedException();
	}
}
