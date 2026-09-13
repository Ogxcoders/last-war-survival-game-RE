using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using Mono.Security;

namespace System;

[StructLayout(LayoutKind.Sequential)]
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class AppDomain : MarshalByRefObject
{
	private IntPtr _mono_app_domain;

	private static string _process_guid;

	[ThreadStatic]
	private static Dictionary<string, object> type_resolve_in_progress;

	[ThreadStatic]
	private static Dictionary<string, object> assembly_resolve_in_progress;

	[ThreadStatic]
	private static Dictionary<string, object> assembly_resolve_in_progress_refonly;

	private object _evidence;

	private object _granted;

	private int _principalPolicy;

	[ThreadStatic]
	private static object _principal;

	private static AppDomain default_domain;

	private object _domain_manager;

	private object _activation;

	private object _applicationIdentity;

	private List<string> compatibility_switch;

	internal AppDomainSetup SetupInformationNoCopy => getSetup();

	public AppDomainSetup SetupInformation => new AppDomainSetup(getSetup());

	public string BaseDirectory => SetupInformationNoCopy.ApplicationBase;

	public string RelativeSearchPath => SetupInformationNoCopy.PrivateBinPath;

	public string DynamicDirectory
	{
		get
		{
			AppDomainSetup setupInformationNoCopy = SetupInformationNoCopy;
			if (setupInformationNoCopy.DynamicBase == null)
			{
				return null;
			}
			return Path.Combine(setupInformationNoCopy.DynamicBase, setupInformationNoCopy.ApplicationName);
		}
	}

	public bool ShadowCopyFiles => SetupInformationNoCopy.ShadowCopyFiles == "true";

	public string FriendlyName => getFriendlyName();

	public Evidence Evidence
	{
		get
		{
			if (_evidence == null)
			{
				lock (this)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly == null)
					{
						if (this == DefaultDomain)
						{
							return new Evidence();
						}
						_evidence = DefaultDomain.Evidence;
					}
					else
					{
						_evidence = Evidence.GetDefaultHostEvidence(entryAssembly);
					}
				}
			}
			return new Evidence((Evidence)_evidence);
		}
	}

	internal IPrincipal DefaultPrincipal
	{
		get
		{
			if (_principal == null)
			{
				switch ((PrincipalPolicy)_principalPolicy)
				{
				case PrincipalPolicy.UnauthenticatedPrincipal:
					_principal = new GenericPrincipal(new GenericIdentity(string.Empty, string.Empty), null);
					break;
				case PrincipalPolicy.WindowsPrincipal:
					_principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
					break;
				}
			}
			return (IPrincipal)_principal;
		}
	}

	internal PermissionSet GrantedPermissionSet => (PermissionSet)_granted;

	public PermissionSet PermissionSet => ((PermissionSet)_granted) ?? ((PermissionSet)(_granted = new PermissionSet(PermissionState.Unrestricted)));

	public static AppDomain CurrentDomain => getCurDomain();

	internal static AppDomain DefaultDomain
	{
		get
		{
			if (default_domain == null)
			{
				AppDomain rootDomain = getRootDomain();
				if (rootDomain == CurrentDomain)
				{
					default_domain = rootDomain;
				}
				else
				{
					default_domain = (AppDomain)RemotingServices.GetDomainProxy(rootDomain);
				}
			}
			return default_domain;
		}
	}

	[MonoTODO]
	public bool IsHomogenous => true;

	[MonoTODO]
	public bool IsFullyTrusted => true;

	public AppDomainManager DomainManager => (AppDomainManager)_domain_manager;

	public ActivationContext ActivationContext => (ActivationContext)_activation;

	public ApplicationIdentity ApplicationIdentity => (ApplicationIdentity)_applicationIdentity;

	public int Id
	{
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		get
		{
			return getDomainID();
		}
	}

	[MonoTODO("Currently always returns false")]
	public static bool MonitoringIsEnabled
	{
		get
		{
			return false;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public long MonitoringSurvivedMemorySize
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public static long MonitoringSurvivedProcessMemorySize
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public long MonitoringTotalAllocatedMemorySize
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public TimeSpan MonitoringTotalProcessorTime
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event AssemblyLoadEventHandler AssemblyLoad;

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event ResolveEventHandler AssemblyResolve;

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event EventHandler DomainUnload;

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event EventHandler ProcessExit;

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event ResolveEventHandler ResourceResolve;

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event ResolveEventHandler TypeResolve;

	[method: SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public event UnhandledExceptionEventHandler UnhandledException;

	public event EventHandler<FirstChanceExceptionEventArgs> FirstChanceException;

	public event ResolveEventHandler ReflectionOnlyAssemblyResolve;

	internal static bool IsAppXModel()
	{
		return false;
	}

	internal static bool IsAppXDesignMode()
	{
		return false;
	}

	internal static void CheckReflectionOnlyLoadSupported()
	{
	}

	internal static void CheckLoadFromSupported()
	{
	}

	private AppDomain()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern AppDomainSetup getSetup();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern string getFriendlyName();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AppDomain getCurDomain();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AppDomain getRootDomain();

	[Obsolete("AppDomain.AppendPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead.")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void AppendPrivatePath(string path)
	{
		if (path == null || path.Length == 0)
		{
			return;
		}
		AppDomainSetup setupInformationNoCopy = SetupInformationNoCopy;
		string privateBinPath = setupInformationNoCopy.PrivateBinPath;
		if (privateBinPath == null || privateBinPath.Length == 0)
		{
			setupInformationNoCopy.PrivateBinPath = path;
			return;
		}
		privateBinPath = privateBinPath.Trim();
		if (privateBinPath[privateBinPath.Length - 1] != Path.PathSeparator)
		{
			privateBinPath += Path.PathSeparator;
		}
		setupInformationNoCopy.PrivateBinPath = privateBinPath + path;
	}

	[Obsolete("AppDomain.ClearPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead.")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void ClearPrivatePath()
	{
		SetupInformationNoCopy.PrivateBinPath = string.Empty;
	}

	[Obsolete("Use AppDomainSetup.ShadowCopyDirectories")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void ClearShadowCopyPath()
	{
		SetupInformationNoCopy.ShadowCopyDirectories = string.Empty;
	}

	internal ObjectHandle InternalCreateInstanceWithNoSecurity(string assemblyName, string typeName)
	{
		return CreateInstance(assemblyName, typeName);
	}

	internal ObjectHandle InternalCreateInstanceWithNoSecurity(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
	{
		return CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
	}

	internal ObjectHandle InternalCreateInstanceFromWithNoSecurity(string assemblyName, string typeName)
	{
		return CreateInstanceFrom(assemblyName, typeName);
	}

	internal ObjectHandle InternalCreateInstanceFromWithNoSecurity(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
	{
		return CreateInstanceFrom(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
	}

	public ObjectHandle CreateInstance(string assemblyName, string typeName)
	{
		if (assemblyName == null)
		{
			throw new ArgumentNullException("assemblyName");
		}
		return Activator.CreateInstance(assemblyName, typeName);
	}

	public ObjectHandle CreateInstance(string assemblyName, string typeName, object[] activationAttributes)
	{
		if (assemblyName == null)
		{
			throw new ArgumentNullException("assemblyName");
		}
		return Activator.CreateInstance(assemblyName, typeName, activationAttributes);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
	{
		if (assemblyName == null)
		{
			throw new ArgumentNullException("assemblyName");
		}
		return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
	}

	public object CreateInstanceAndUnwrap(string assemblyName, string typeName)
	{
		return CreateInstance(assemblyName, typeName)?.Unwrap();
	}

	public object CreateInstanceAndUnwrap(string assemblyName, string typeName, object[] activationAttributes)
	{
		return CreateInstance(assemblyName, typeName, activationAttributes)?.Unwrap();
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public object CreateInstanceAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
	{
		return CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes)?.Unwrap();
	}

	public ObjectHandle CreateInstance(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
	{
		if (assemblyName == null)
		{
			throw new ArgumentNullException("assemblyName");
		}
		return Activator.CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
	}

	public object CreateInstanceAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
	{
		return CreateInstance(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes)?.Unwrap();
	}

	public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
	{
		if (assemblyFile == null)
		{
			throw new ArgumentNullException("assemblyFile");
		}
		return Activator.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, null);
	}

	public object CreateInstanceFromAndUnwrap(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
	{
		return CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes)?.Unwrap();
	}

	public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName)
	{
		if (assemblyFile == null)
		{
			throw new ArgumentNullException("assemblyFile");
		}
		return Activator.CreateInstanceFrom(assemblyFile, typeName);
	}

	public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, object[] activationAttributes)
	{
		if (assemblyFile == null)
		{
			throw new ArgumentNullException("assemblyFile");
		}
		return Activator.CreateInstanceFrom(assemblyFile, typeName, activationAttributes);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public ObjectHandle CreateInstanceFrom(string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
	{
		if (assemblyFile == null)
		{
			throw new ArgumentNullException("assemblyFile");
		}
		return Activator.CreateInstanceFrom(assemblyFile, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes);
	}

	public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName)
	{
		return CreateInstanceFrom(assemblyName, typeName)?.Unwrap();
	}

	public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName, object[] activationAttributes)
	{
		return CreateInstanceFrom(assemblyName, typeName, activationAttributes)?.Unwrap();
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public object CreateInstanceFromAndUnwrap(string assemblyName, string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes, Evidence securityAttributes)
	{
		return CreateInstanceFrom(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, securityAttributes)?.Unwrap();
	}

	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
	{
		return DefineDynamicAssembly(name, access, null, null, null, null, null, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence)
	{
		return DefineDynamicAssembly(name, access, null, evidence, null, null, null, isSynchronized: false);
	}

	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir)
	{
		return DefineDynamicAssembly(name, access, dir, null, null, null, null, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence)
	{
		return DefineDynamicAssembly(name, access, dir, evidence, null, null, null, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
	{
		return DefineDynamicAssembly(name, access, null, null, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
	{
		return DefineDynamicAssembly(name, access, null, evidence, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
	{
		return DefineDynamicAssembly(name, access, dir, null, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions)
	{
		return DefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized: false);
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		ValidateAssemblyName(name.Name);
		AssemblyBuilder assemblyBuilder = new AssemblyBuilder(name, dir, access, corlib_internal: false);
		assemblyBuilder.AddPermissionRequests(requiredPermissions, optionalPermissions, refusedPermissions);
		return assemblyBuilder;
	}

	[Obsolete("Declarative security for assembly level is no longer enforced")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, Evidence evidence, PermissionSet requiredPermissions, PermissionSet optionalPermissions, PermissionSet refusedPermissions, bool isSynchronized, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
	{
		AssemblyBuilder assemblyBuilder = DefineDynamicAssembly(name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized);
		if (assemblyAttributes != null)
		{
			foreach (CustomAttributeBuilder assemblyAttribute in assemblyAttributes)
			{
				assemblyBuilder.SetCustomAttribute(assemblyAttribute);
			}
		}
		return assemblyBuilder;
	}

	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
	{
		return DefineDynamicAssembly(name, access, null, null, null, null, null, isSynchronized: false, assemblyAttributes);
	}

	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, string dir, bool isSynchronized, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
	{
		return DefineDynamicAssembly(name, access, dir, null, null, null, null, isSynchronized, assemblyAttributes);
	}

	[MonoLimitation("The argument securityContextSource is ignored")]
	public AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes, SecurityContextSource securityContextSource)
	{
		return DefineDynamicAssembly(name, access, assemblyAttributes);
	}

	internal AssemblyBuilder DefineInternalDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
	{
		return new AssemblyBuilder(name, null, access, corlib_internal: true);
	}

	public void DoCallBack(CrossAppDomainDelegate callBackDelegate)
	{
		callBackDelegate?.Invoke();
	}

	public int ExecuteAssembly(string assemblyFile)
	{
		return ExecuteAssembly(assemblyFile, null, null);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity)
	{
		return ExecuteAssembly(assemblyFile, assemblySecurity, null);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args)
	{
		Assembly a = Assembly.LoadFrom(assemblyFile, assemblySecurity);
		return ExecuteAssemblyInternal(a, args);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public int ExecuteAssembly(string assemblyFile, Evidence assemblySecurity, string[] args, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
	{
		Assembly a = Assembly.LoadFrom(assemblyFile, assemblySecurity, hashValue, hashAlgorithm);
		return ExecuteAssemblyInternal(a, args);
	}

	public int ExecuteAssembly(string assemblyFile, string[] args)
	{
		Assembly a = Assembly.LoadFrom(assemblyFile, null);
		return ExecuteAssemblyInternal(a, args);
	}

	public int ExecuteAssembly(string assemblyFile, string[] args, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
	{
		Assembly a = Assembly.LoadFrom(assemblyFile, null, hashValue, hashAlgorithm);
		return ExecuteAssemblyInternal(a, args);
	}

	private int ExecuteAssemblyInternal(Assembly a, string[] args)
	{
		if (a.EntryPoint == null)
		{
			throw new MissingMethodException("Entry point not found in assembly '" + a.FullName + "'.");
		}
		return ExecuteAssembly(a, args);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int ExecuteAssembly(Assembly a, string[] args);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern Assembly[] GetAssemblies(bool refOnly);

	public Assembly[] GetAssemblies()
	{
		return GetAssemblies(refOnly: false);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern object GetData(string name);

	public new Type GetType()
	{
		return base.GetType();
	}

	public override object InitializeLifetimeService()
	{
		return null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern Assembly LoadAssembly(string assemblyRef, Evidence securityEvidence, bool refOnly, ref StackCrawlMark stackMark);

	public Assembly Load(AssemblyName assemblyRef)
	{
		return Load(assemblyRef, null);
	}

	internal Assembly LoadSatellite(AssemblyName assemblyRef, bool throwOnError, ref StackCrawlMark stackMark)
	{
		if (assemblyRef == null)
		{
			throw new ArgumentNullException("assemblyRef");
		}
		Assembly assembly = LoadAssembly(assemblyRef.FullName, null, refOnly: false, ref stackMark);
		if (assembly == null && throwOnError)
		{
			throw new FileNotFoundException(null, assemblyRef.Name);
		}
		return assembly;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity)
	{
		if (assemblyRef == null)
		{
			throw new ArgumentNullException("assemblyRef");
		}
		if (assemblyRef.Name == null || assemblyRef.Name.Length == 0)
		{
			if (assemblyRef.CodeBase != null)
			{
				return Assembly.LoadFrom(assemblyRef.CodeBase, assemblySecurity);
			}
			throw new ArgumentException(Locale.GetText("assemblyRef.Name cannot be empty."), "assemblyRef");
		}
		StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
		Assembly assembly = LoadAssembly(assemblyRef.FullName, assemblySecurity, refOnly: false, ref stackMark);
		if (assembly != null)
		{
			return assembly;
		}
		if (assemblyRef.CodeBase == null)
		{
			throw new FileNotFoundException(null, assemblyRef.Name);
		}
		string text = assemblyRef.CodeBase;
		if (text.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
		{
			text = new Uri(text).LocalPath;
		}
		try
		{
			assembly = Assembly.LoadFrom(text, assemblySecurity);
		}
		catch
		{
			throw new FileNotFoundException(null, assemblyRef.Name);
		}
		AssemblyName name = assembly.GetName();
		if (assemblyRef.Name != name.Name)
		{
			throw new FileNotFoundException(null, assemblyRef.Name);
		}
		if (assemblyRef.Version != null && assemblyRef.Version != new Version(0, 0, 0, 0) && assemblyRef.Version != name.Version)
		{
			throw new FileNotFoundException(null, assemblyRef.Name);
		}
		if (assemblyRef.CultureInfo != null && assemblyRef.CultureInfo.Equals(name))
		{
			throw new FileNotFoundException(null, assemblyRef.Name);
		}
		byte[] publicKeyToken = assemblyRef.GetPublicKeyToken();
		if (publicKeyToken != null && publicKeyToken.Length != 0)
		{
			byte[] publicKeyToken2 = name.GetPublicKeyToken();
			if (publicKeyToken2 == null || publicKeyToken.Length != publicKeyToken2.Length)
			{
				throw new FileNotFoundException(null, assemblyRef.Name);
			}
			for (int num = publicKeyToken.Length - 1; num >= 0; num--)
			{
				if (publicKeyToken2[num] != publicKeyToken[num])
				{
					throw new FileNotFoundException(null, assemblyRef.Name);
				}
			}
		}
		return assembly;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Assembly Load(string assemblyString)
	{
		StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
		return Load(assemblyString, null, refonly: false, ref stackMark);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public Assembly Load(string assemblyString, Evidence assemblySecurity)
	{
		StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
		return Load(assemblyString, assemblySecurity, refonly: false, ref stackMark);
	}

	internal Assembly Load(string assemblyString, Evidence assemblySecurity, bool refonly, ref StackCrawlMark stackMark)
	{
		if (assemblyString == null)
		{
			throw new ArgumentNullException("assemblyString");
		}
		if (assemblyString.Length == 0)
		{
			throw new ArgumentException("assemblyString cannot have zero length");
		}
		Assembly assembly = LoadAssembly(assemblyString, assemblySecurity, refonly, ref stackMark);
		if (assembly == null)
		{
			throw new FileNotFoundException(null, assemblyString);
		}
		return assembly;
	}

	public Assembly Load(byte[] rawAssembly)
	{
		return Load(rawAssembly, null, null);
	}

	public Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore)
	{
		return Load(rawAssembly, rawSymbolStore, null);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern Assembly LoadAssemblyRaw(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence, bool refonly);

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
	{
		return Load(rawAssembly, rawSymbolStore, securityEvidence, refonly: false);
	}

	internal Assembly Load(byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence, bool refonly)
	{
		if (rawAssembly == null)
		{
			throw new ArgumentNullException("rawAssembly");
		}
		Assembly assembly = LoadAssemblyRaw(rawAssembly, rawSymbolStore, securityEvidence, refonly);
		assembly.FromByteArray = true;
		return assembly;
	}

	[Obsolete("AppDomain policy levels are obsolete")]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
	public void SetAppDomainPolicy(PolicyLevel domainPolicy)
	{
		if (domainPolicy == null)
		{
			throw new ArgumentNullException("domainPolicy");
		}
		if (_granted != null)
		{
			throw new PolicyException(Locale.GetText("An AppDomain policy is already specified."));
		}
		if (IsFinalizingForUnload())
		{
			throw new AppDomainUnloadedException();
		}
		PolicyStatement policyStatement = domainPolicy.Resolve((Evidence)_evidence);
		_granted = policyStatement.PermissionSet;
	}

	[Obsolete("Use AppDomainSetup.SetCachePath")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void SetCachePath(string path)
	{
		SetupInformationNoCopy.CachePath = path;
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public void SetPrincipalPolicy(PrincipalPolicy policy)
	{
		if (IsFinalizingForUnload())
		{
			throw new AppDomainUnloadedException();
		}
		_principalPolicy = (int)policy;
		_principal = null;
	}

	[Obsolete("Use AppDomainSetup.ShadowCopyFiles")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void SetShadowCopyFiles()
	{
		SetupInformationNoCopy.ShadowCopyFiles = "true";
	}

	[Obsolete("Use AppDomainSetup.ShadowCopyDirectories")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void SetShadowCopyPath(string path)
	{
		SetupInformationNoCopy.ShadowCopyDirectories = path;
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public void SetThreadPrincipal(IPrincipal principal)
	{
		if (principal == null)
		{
			throw new ArgumentNullException("principal");
		}
		if (_principal != null)
		{
			throw new PolicyException(Locale.GetText("principal already present."));
		}
		if (IsFinalizingForUnload())
		{
			throw new AppDomainUnloadedException();
		}
		_principal = principal;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AppDomain InternalSetDomainByID(int domain_id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AppDomain InternalSetDomain(AppDomain context);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void InternalPushDomainRef(AppDomain domain);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void InternalPushDomainRefByID(int domain_id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void InternalPopDomainRef();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Context InternalSetContext(Context context);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Context InternalGetContext();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Context InternalGetDefaultContext();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string InternalGetProcessGuid(string newguid);

	internal static object InvokeInDomain(AppDomain domain, MethodInfo method, object obj, object[] args)
	{
		AppDomain currentDomain = CurrentDomain;
		bool flag = false;
		try
		{
			InternalPushDomainRef(domain);
			flag = true;
			InternalSetDomain(domain);
			Exception exc;
			object result = ((RuntimeMethodInfo)method).InternalInvoke(obj, args, out exc);
			if (exc != null)
			{
				throw exc;
			}
			return result;
		}
		finally
		{
			InternalSetDomain(currentDomain);
			if (flag)
			{
				InternalPopDomainRef();
			}
		}
	}

	internal static object InvokeInDomainByID(int domain_id, MethodInfo method, object obj, object[] args)
	{
		AppDomain currentDomain = CurrentDomain;
		bool flag = false;
		try
		{
			InternalPushDomainRefByID(domain_id);
			flag = true;
			InternalSetDomainByID(domain_id);
			Exception exc;
			object result = ((RuntimeMethodInfo)method).InternalInvoke(obj, args, out exc);
			if (exc != null)
			{
				throw exc;
			}
			return result;
		}
		finally
		{
			InternalSetDomain(currentDomain);
			if (flag)
			{
				InternalPopDomainRef();
			}
		}
	}

	internal static string GetProcessGuid()
	{
		if (_process_guid == null)
		{
			_process_guid = InternalGetProcessGuid(Guid.NewGuid().ToString());
		}
		return _process_guid;
	}

	[Obsolete("AppDomain.CreateDomain will no longer be supported in .NET 5 and later.  Consider AssemblyLoadContext when it becomes available https://docs.microsoft.com/en-us/dotnet/core/dependency-loading/understanding-assemblyloadcontext")]
	public static AppDomain CreateDomain(string friendlyName)
	{
		return CreateDomain(friendlyName, null, null);
	}

	[Obsolete("AppDomain.CreateDomain will no longer be supported in .NET 5 and later.  Consider AssemblyLoadContext when it becomes available https://docs.microsoft.com/en-us/dotnet/core/dependency-loading/understanding-assemblyloadcontext")]
	public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo)
	{
		return CreateDomain(friendlyName, securityInfo, null);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AppDomain createDomain(string friendlyName, AppDomainSetup info);

	[MonoLimitation("Currently it does not allow the setup in the other domain")]
	[Obsolete("AppDomain.CreateDomain will no longer be supported in .NET 5 and later.  Consider AssemblyLoadContext when it becomes available https://docs.microsoft.com/en-us/dotnet/core/dependency-loading/understanding-assemblyloadcontext")]
	[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
	public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, AppDomainSetup info)
	{
		if (friendlyName == null)
		{
			throw new ArgumentNullException("friendlyName");
		}
		AppDomain defaultDomain = DefaultDomain;
		info = ((info != null) ? new AppDomainSetup(info) : ((defaultDomain != null) ? defaultDomain.SetupInformation : new AppDomainSetup()));
		if (defaultDomain != null)
		{
			if (!info.Equals(defaultDomain.SetupInformation))
			{
				if (info.ApplicationBase == null)
				{
					info.ApplicationBase = defaultDomain.SetupInformation.ApplicationBase;
				}
				if (info.ConfigurationFile == null)
				{
					info.ConfigurationFile = Path.GetFileName(defaultDomain.SetupInformation.ConfigurationFile);
				}
			}
		}
		else if (info.ConfigurationFile == null)
		{
			info.ConfigurationFile = "[I don't have a config file]";
		}
		info.SerializeNonPrimitives();
		AppDomain appDomain = (AppDomain)RemotingServices.GetDomainProxy(createDomain(friendlyName, info));
		if (securityInfo == null)
		{
			if (defaultDomain == null)
			{
				appDomain._evidence = null;
			}
			else
			{
				appDomain._evidence = defaultDomain.Evidence;
			}
		}
		else
		{
			appDomain._evidence = new Evidence(securityInfo);
		}
		return appDomain;
	}

	public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles)
	{
		return CreateDomain(friendlyName, securityInfo, CreateDomainSetup(appBasePath, appRelativeSearchPath, shadowCopyFiles));
	}

	private static AppDomainSetup CreateDomainSetup(string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles)
	{
		AppDomainSetup appDomainSetup = new AppDomainSetup();
		appDomainSetup.ApplicationBase = appBasePath;
		appDomainSetup.PrivateBinPath = appRelativeSearchPath;
		if (shadowCopyFiles)
		{
			appDomainSetup.ShadowCopyFiles = "true";
		}
		else
		{
			appDomainSetup.ShadowCopyFiles = "false";
		}
		return appDomainSetup;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool InternalIsFinalizingForUnload(int domain_id);

	public bool IsFinalizingForUnload()
	{
		return InternalIsFinalizingForUnload(getDomainID());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalUnload(int domain_id);

	private int getDomainID()
	{
		return Thread.GetDomainID();
	}

	[ReliabilityContract(Consistency.MayCorruptAppDomain, Cer.MayFail)]
	[SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
	public static void Unload(AppDomain domain)
	{
		if (domain == null)
		{
			throw new ArgumentNullException("domain");
		}
		InternalUnload(domain.getDomainID());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public extern void SetData(string name, object data);

	[MonoLimitation("The permission field is ignored")]
	public void SetData(string name, object data, IPermission permission)
	{
		SetData(name, data);
	}

	[Obsolete("Use AppDomainSetup.DynamicBase")]
	[SecurityPermission(SecurityAction.LinkDemand, ControlAppDomain = true)]
	public void SetDynamicBase(string path)
	{
		throw new PlatformNotSupportedException();
	}

	[Obsolete("AppDomain.GetCurrentThreadId has been deprecated because it does not provide a stable Id when managed threads are running on fibers (aka lightweight threads). To get a stable identifier for a managed thread, use the ManagedThreadId property on Thread.'")]
	public static int GetCurrentThreadId()
	{
		return Thread.CurrentThreadId;
	}

	public override string ToString()
	{
		return getFriendlyName();
	}

	private static void ValidateAssemblyName(string name)
	{
		if (name == null || name.Length == 0)
		{
			throw new ArgumentException("The Name of AssemblyName cannot be null or a zero-length string.");
		}
		bool flag = true;
		for (int i = 0; i < name.Length; i++)
		{
			char c = name[i];
			if (i == 0 && char.IsWhiteSpace(c))
			{
				flag = false;
				break;
			}
			if (c == '/' || c == '\\' || c == ':')
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			throw new ArgumentException("The Name of AssemblyName cannot start with whitespace, or contain '/', '\\'  or ':'.");
		}
	}

	private void DoAssemblyLoad(Assembly assembly)
	{
		if (this.AssemblyLoad != null)
		{
			this.AssemblyLoad(this, new AssemblyLoadEventArgs(assembly));
		}
	}

	private Assembly DoAssemblyResolve(string name, Assembly requestingAssembly, bool refonly)
	{
		ResolveEventHandler resolveEventHandler = ((!refonly) ? this.AssemblyResolve : this.ReflectionOnlyAssemblyResolve);
		if (resolveEventHandler == null)
		{
			return null;
		}
		Dictionary<string, object> dictionary;
		if (refonly)
		{
			dictionary = assembly_resolve_in_progress_refonly;
			if (dictionary == null)
			{
				dictionary = (assembly_resolve_in_progress_refonly = new Dictionary<string, object>());
			}
		}
		else
		{
			dictionary = assembly_resolve_in_progress;
			if (dictionary == null)
			{
				dictionary = (assembly_resolve_in_progress = new Dictionary<string, object>());
			}
		}
		if (dictionary.ContainsKey(name))
		{
			return null;
		}
		dictionary[name] = null;
		try
		{
			Delegate[] invocationList = resolveEventHandler.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(name, requestingAssembly));
				if (assembly != null)
				{
					return assembly;
				}
			}
			return null;
		}
		finally
		{
			dictionary.Remove(name);
		}
	}

	internal Assembly DoTypeBuilderResolve(TypeBuilder tb)
	{
		if (this.TypeResolve == null)
		{
			return null;
		}
		return DoTypeResolve(tb.FullName);
	}

	internal Assembly DoTypeResolve(string name)
	{
		if (this.TypeResolve == null)
		{
			return null;
		}
		Dictionary<string, object> dictionary = type_resolve_in_progress;
		if (dictionary == null)
		{
			dictionary = (type_resolve_in_progress = new Dictionary<string, object>());
		}
		if (dictionary.ContainsKey(name))
		{
			return null;
		}
		dictionary[name] = null;
		try
		{
			Delegate[] invocationList = this.TypeResolve.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(name));
				if (assembly != null)
				{
					return assembly;
				}
			}
			return null;
		}
		finally
		{
			dictionary.Remove(name);
		}
	}

	internal Assembly DoResourceResolve(string name, Assembly requesting)
	{
		if (this.ResourceResolve == null)
		{
			return null;
		}
		Delegate[] invocationList = this.ResourceResolve.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			Assembly assembly = ((ResolveEventHandler)invocationList[i])(this, new ResolveEventArgs(name, requesting));
			if (assembly != null)
			{
				return assembly;
			}
		}
		return null;
	}

	private void DoDomainUnload()
	{
		if (this.DomainUnload != null)
		{
			this.DomainUnload(this, null);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void DoUnhandledException(Exception e);

	internal void DoUnhandledException(UnhandledExceptionEventArgs args)
	{
		if (this.UnhandledException != null)
		{
			this.UnhandledException(this, args);
		}
	}

	internal byte[] GetMarshalledDomainObjRef()
	{
		return CADSerializer.SerializeObject(RemotingServices.Marshal(CurrentDomain, null, typeof(AppDomain))).GetBuffer();
	}

	internal void ProcessMessageInDomain(byte[] arrRequest, CADMethodCallMessage cadMsg, out byte[] arrResponse, out CADMethodReturnMessage cadMrm)
	{
		IMessage msg = ((arrRequest == null) ? new MethodCall(cadMsg) : CADSerializer.DeserializeMessage(new MemoryStream(arrRequest), null));
		IMessage message = ChannelServices.SyncDispatchMessage(msg);
		cadMrm = CADMethodReturnMessage.Create(message);
		if (cadMrm == null)
		{
			arrResponse = CADSerializer.SerializeMessage(message).GetBuffer();
		}
		else
		{
			arrResponse = null;
		}
	}

	[MonoTODO("This routine only returns the parameter currently")]
	[ComVisible(false)]
	public string ApplyPolicy(string assemblyName)
	{
		if (assemblyName == null)
		{
			throw new ArgumentNullException("assemblyName");
		}
		if (assemblyName.Length == 0)
		{
			throw new ArgumentException("assemblyName");
		}
		return assemblyName;
	}

	public static AppDomain CreateDomain(string friendlyName, Evidence securityInfo, string appBasePath, string appRelativeSearchPath, bool shadowCopyFiles, AppDomainInitializer adInit, string[] adInitArgs)
	{
		AppDomainSetup appDomainSetup = CreateDomainSetup(appBasePath, appRelativeSearchPath, shadowCopyFiles);
		appDomainSetup.AppDomainInitializerArguments = adInitArgs;
		appDomainSetup.AppDomainInitializer = adInit;
		return CreateDomain(friendlyName, securityInfo, appDomainSetup);
	}

	public int ExecuteAssemblyByName(string assemblyName)
	{
		return ExecuteAssemblyByName(assemblyName, (Evidence)null, (string[])null);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public int ExecuteAssemblyByName(string assemblyName, Evidence assemblySecurity)
	{
		return ExecuteAssemblyByName(assemblyName, assemblySecurity, (string[])null);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public int ExecuteAssemblyByName(string assemblyName, Evidence assemblySecurity, params string[] args)
	{
		Assembly a = Assembly.Load(assemblyName, assemblySecurity);
		return ExecuteAssemblyInternal(a, args);
	}

	[Obsolete("Use an overload that does not take an Evidence parameter")]
	public int ExecuteAssemblyByName(AssemblyName assemblyName, Evidence assemblySecurity, params string[] args)
	{
		Assembly a = Assembly.Load(assemblyName, assemblySecurity);
		return ExecuteAssemblyInternal(a, args);
	}

	public int ExecuteAssemblyByName(string assemblyName, params string[] args)
	{
		Assembly a = Assembly.Load(assemblyName, null);
		return ExecuteAssemblyInternal(a, args);
	}

	public int ExecuteAssemblyByName(AssemblyName assemblyName, params string[] args)
	{
		Assembly a = Assembly.Load(assemblyName, null);
		return ExecuteAssemblyInternal(a, args);
	}

	public bool IsDefaultAppDomain()
	{
		return this == DefaultDomain;
	}

	public Assembly[] ReflectionOnlyGetAssemblies()
	{
		return GetAssemblies(refOnly: true);
	}

	public bool? IsCompatibilitySwitchSet(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		return compatibility_switch != null && compatibility_switch.Contains(value);
	}

	internal void SetCompatibilitySwitch(string value)
	{
		if (compatibility_switch == null)
		{
			compatibility_switch = new List<string>();
		}
		compatibility_switch.Add(value);
	}
}
