using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;
using System.Security.Principal;

namespace System;

[Guid("05F696DC-2B29-3663-AD8B-C4389CF2A713")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[CLSCompliant(false)]
[ComVisible(true)]
public interface _AppDomain
{
	Evidence Evidence { get; }

	string FriendlyName { get; }

	string BaseDirectory { get; }

	string RelativeSearchPath { get; }

	bool ShadowCopyFiles { get; }

	string DynamicDirectory { get; }

	event EventHandler DomainUnload;

	event AssemblyLoadEventHandler AssemblyLoad;

	event EventHandler ProcessExit;

	event ResolveEventHandler TypeResolve;

	event ResolveEventHandler ResourceResolve;

	event ResolveEventHandler AssemblyResolve;

	event UnhandledExceptionEventHandler UnhandledException;

	void GetTypeInfoCount(out uint pcTInfo);

	void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

	void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

	void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

	new string ToString();

	new bool Equals(object other);

	new int GetHashCode();

	new Type GetType();

	[SecurityCritical]
	object InitializeLifetimeService();

	[SecurityCritical]
	object GetLifetimeService();

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions);

	AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized);

	ObjectHandle CreateInstance(string assemblyName, string typeName);

	ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName);

	ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes);

	ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes);

	ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes);

	ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes);

	Assembly Load(AssemblyName assemblyRef);

	Assembly Load(string assemblyString);

	Assembly Load(byte[] rawAssembly);

	Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore);

	Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence);

	Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity);

	Assembly Load(string assemblyString, Evidence assemblySecurity);

	int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity);

	int ExecuteAssembly(string assemblyFile);

	int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args);

	Assembly[] GetAssemblies();

	[SecurityCritical]
	void AppendPrivatePath(string path);

	[SecurityCritical]
	void ClearPrivatePath();

	[SecurityCritical]
	void SetShadowCopyPath(string s);

	[SecurityCritical]
	void ClearShadowCopyPath();

	[SecurityCritical]
	void SetCachePath(string s);

	[SecurityCritical]
	void SetData(string name, object data);

	object GetData(string name);

	[SecurityCritical]
	void SetAppDomainPolicy(PolicyLevel domainPolicy);

	void SetThreadPrincipal(IPrincipal principal);

	void SetPrincipalPolicy(PrincipalPolicy policy);

	void DoCallBack(CrossAppDomainDelegate theDelegate);
}
