using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime;

internal static class PartialTrustHelpers
{
	[SecurityCritical]
	private static Type aptca;

	[SecurityCritical]
	private static volatile bool checkedForFullTrust;

	[SecurityCritical]
	private static bool inFullTrust;

	internal static bool ShouldFlowSecurityContext
	{
		[SecurityCritical]
		get
		{
			return SecurityManager.CurrentThreadRequiresSecurityContextCapture();
		}
	}

	internal static bool AppDomainFullyTrusted
	{
		[SecuritySafeCritical]
		get
		{
			return true;
		}
	}

	[SecurityCritical]
	internal static bool IsInFullTrust()
	{
		return true;
	}

	[SecurityCritical]
	internal static bool IsTypeAptca(Type type)
	{
		Assembly assembly = type.Assembly;
		if (!IsAssemblyAptca(assembly))
		{
			return !IsAssemblySigned(assembly);
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	internal static void DemandForFullTrust()
	{
	}

	[SecurityCritical]
	private static bool IsAssemblyAptca(Assembly assembly)
	{
		if (aptca == null)
		{
			aptca = typeof(AllowPartiallyTrustedCallersAttribute);
		}
		return assembly.GetCustomAttributes(aptca, inherit: false).Length != 0;
	}

	[SecurityCritical]
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	private static bool IsAssemblySigned(Assembly assembly)
	{
		byte[] publicKeyToken = assembly.GetName().GetPublicKeyToken();
		return (publicKeyToken != null) & (publicKeyToken.Length != 0);
	}

	[SecurityCritical]
	internal static bool CheckAppDomainPermissions(PermissionSet permissions)
	{
		return true;
	}

	[SecurityCritical]
	internal static bool HasEtwPermissions()
	{
		return true;
	}
}
