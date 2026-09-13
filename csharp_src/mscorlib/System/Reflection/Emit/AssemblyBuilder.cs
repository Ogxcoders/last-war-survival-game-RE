using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;
using Mono.Security;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public sealed class AssemblyBuilder : Assembly
{
	internal IntPtr _mono_assembly;

	private object _evidence;

	private UIntPtr dynamic_assembly;

	private MethodInfo entry_point;

	private ModuleBuilder[] modules;

	private string name;

	private string dir;

	private CustomAttributeBuilder[] cattrs;

	private MonoResource[] resources;

	private byte[] public_key;

	private string version;

	private string culture;

	private uint algid;

	private uint flags;

	private PEFileKinds pekind = PEFileKinds.Dll;

	private bool delay_sign;

	private uint access;

	private Module[] loaded_modules;

	private MonoWin32Resource[] win32_resources;

	private RefEmitPermissionSet[] permissions_minimum;

	private RefEmitPermissionSet[] permissions_optional;

	private RefEmitPermissionSet[] permissions_refused;

	private PortableExecutableKinds peKind;

	private ImageFileMachine machine;

	private bool corlib_internal;

	private Type[] type_forwarders;

	private byte[] pktoken;

	private object _minimum;

	private object _optional;

	private object _refuse;

	private object _granted;

	private object _denied;

	private string assemblyName;

	internal Type corlib_object_type = typeof(object);

	internal Type corlib_value_type = typeof(ValueType);

	internal Type corlib_enum_type = typeof(Enum);

	internal Type corlib_void_type = typeof(void);

	private ArrayList resource_writers;

	private Win32VersionResource version_res;

	private bool created;

	private bool is_module_only;

	private Mono.Security.StrongName sn;

	private NativeResourceType native_resource;

	private string versioninfo_culture;

	private const AssemblyBuilderAccess COMPILER_ACCESS = (AssemblyBuilderAccess)2048;

	private ModuleBuilder manifest_module;

	public override string CodeBase
	{
		get
		{
			throw not_supported();
		}
	}

	public override string EscapedCodeBase => RuntimeAssembly.GetCodeBase(this, escaped: true);

	public override MethodInfo EntryPoint => entry_point;

	public override string Location
	{
		get
		{
			throw not_supported();
		}
	}

	public override string ImageRuntimeVersion => RuntimeAssembly.InternalImageRuntimeVersion(this);

	public override bool ReflectionOnly => access == 6;

	internal bool IsSave => access != 1;

	internal bool IsRun
	{
		get
		{
			if (access != 1 && access != 3)
			{
				return access == 9;
			}
			return true;
		}
	}

	internal string AssemblyDir => dir;

	internal bool IsModuleOnly
	{
		get
		{
			return is_module_only;
		}
		set
		{
			is_module_only = value;
		}
	}

	public override Module ManifestModule => GetManifestModule();

	public override bool GlobalAssemblyCache => false;

	public override bool IsDynamic => true;

	public override string FullName => RuntimeAssembly.get_fullname(this);

	internal override IntPtr MonoAssembly => _mono_assembly;

	public override Evidence Evidence
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
		get
		{
			return UnprotectedGetEvidence();
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void basic_init(AssemblyBuilder ab);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateNativeCustomAttributes(AssemblyBuilder ab);

	[PreserveDependency("RuntimeResolve", "System.Reflection.Emit.ModuleBuilder")]
	internal AssemblyBuilder(AssemblyName n, string directory, AssemblyBuilderAccess access, bool corlib_internal)
	{
		if ((access & (AssemblyBuilderAccess)2048) != 0)
		{
			throw new NotImplementedException("COMPILER_ACCESS is no longer supperted, use a newer mcs.");
		}
		if (!Enum.IsDefined(typeof(AssemblyBuilderAccess), access))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Argument value {0} is not valid.", (int)access), "access");
		}
		name = n.Name;
		this.access = (uint)access;
		flags = (uint)n.Flags;
		if (IsSave && (directory == null || directory.Length == 0))
		{
			dir = Directory.GetCurrentDirectory();
		}
		else
		{
			dir = directory;
		}
		if (n.CultureInfo != null)
		{
			culture = n.CultureInfo.Name;
			versioninfo_culture = n.CultureInfo.Name;
		}
		Version version = n.Version;
		if (version != null)
		{
			this.version = version.ToString();
		}
		if (n.KeyPair != null)
		{
			sn = n.KeyPair.StrongName();
		}
		else
		{
			byte[] publicKey = n.GetPublicKey();
			if (publicKey != null && publicKey.Length != 0)
			{
				sn = new Mono.Security.StrongName(publicKey);
			}
		}
		if (sn != null)
		{
			flags |= 1u;
		}
		this.corlib_internal = corlib_internal;
		if (sn != null)
		{
			pktoken = new byte[sn.PublicKeyToken.Length * 2];
			int num = 0;
			byte[] publicKeyToken = sn.PublicKeyToken;
			foreach (byte b in publicKeyToken)
			{
				string text = b.ToString("x2");
				pktoken[num++] = (byte)text[0];
				pktoken[num++] = (byte)text[1];
			}
		}
		basic_init(this);
	}

	public void AddResourceFile(string name, string fileName)
	{
		AddResourceFile(name, fileName, ResourceAttributes.Public);
	}

	public void AddResourceFile(string name, string fileName, ResourceAttributes attribute)
	{
		AddResourceFile(name, fileName, attribute, fileNeedsToExists: true);
	}

	private void AddResourceFile(string name, string fileName, ResourceAttributes attribute, bool fileNeedsToExists)
	{
		check_name_and_filename(name, fileName, fileNeedsToExists);
		if (dir != null)
		{
			fileName = Path.Combine(dir, fileName);
		}
		if (resources != null)
		{
			MonoResource[] destinationArray = new MonoResource[resources.Length + 1];
			Array.Copy(resources, destinationArray, resources.Length);
			resources = destinationArray;
		}
		else
		{
			resources = new MonoResource[1];
		}
		int num = resources.Length - 1;
		resources[num].name = name;
		resources[num].filename = fileName;
		resources[num].attrs = attribute;
	}

	internal void AddPermissionRequests(PermissionSet required, PermissionSet optional, PermissionSet refused)
	{
	}

	internal void EmbedResourceFile(string name, string fileName)
	{
		EmbedResourceFile(name, fileName, ResourceAttributes.Public);
	}

	private void EmbedResourceFile(string name, string fileName, ResourceAttributes attribute)
	{
		if (resources != null)
		{
			MonoResource[] destinationArray = new MonoResource[resources.Length + 1];
			Array.Copy(resources, destinationArray, resources.Length);
			resources = destinationArray;
		}
		else
		{
			resources = new MonoResource[1];
		}
		int num = resources.Length - 1;
		resources[num].name = name;
		resources[num].attrs = attribute;
		try
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			long length = fileStream.Length;
			resources[num].data = new byte[length];
			fileStream.Read(resources[num].data, 0, (int)length);
			fileStream.Close();
		}
		catch
		{
		}
	}

	public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new AssemblyBuilder(name, null, access, corlib_internal: false);
	}

	public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
	{
		AssemblyBuilder assemblyBuilder = DefineDynamicAssembly(name, access);
		foreach (CustomAttributeBuilder assemblyAttribute in assemblyAttributes)
		{
			assemblyBuilder.SetCustomAttribute(assemblyAttribute);
		}
		return assemblyBuilder;
	}

	public ModuleBuilder DefineDynamicModule(string name)
	{
		return DefineDynamicModule(name, name, emitSymbolInfo: false, transient: true);
	}

	public ModuleBuilder DefineDynamicModule(string name, bool emitSymbolInfo)
	{
		return DefineDynamicModule(name, name, emitSymbolInfo, transient: true);
	}

	public ModuleBuilder DefineDynamicModule(string name, string fileName)
	{
		return DefineDynamicModule(name, fileName, emitSymbolInfo: false, transient: false);
	}

	public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
	{
		return DefineDynamicModule(name, fileName, emitSymbolInfo, transient: false);
	}

	private ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo, bool transient)
	{
		check_name_and_filename(name, fileName, fileNeedsToExists: false);
		if (!transient)
		{
			if (Path.GetExtension(fileName) == string.Empty)
			{
				throw new ArgumentException("Module file name '" + fileName + "' must have file extension.");
			}
			if (!IsSave)
			{
				throw new NotSupportedException("Persistable modules are not supported in a dynamic assembly created with AssemblyBuilderAccess.Run");
			}
			if (created)
			{
				throw new InvalidOperationException("Assembly was already saved.");
			}
		}
		ModuleBuilder moduleBuilder = new ModuleBuilder(this, name, fileName, emitSymbolInfo, transient);
		if (modules != null && is_module_only)
		{
			throw new InvalidOperationException("A module-only assembly can only contain one module.");
		}
		if (modules != null)
		{
			ModuleBuilder[] destinationArray = new ModuleBuilder[modules.Length + 1];
			Array.Copy(modules, destinationArray, modules.Length);
			modules = destinationArray;
		}
		else
		{
			modules = new ModuleBuilder[1];
		}
		modules[modules.Length - 1] = moduleBuilder;
		return moduleBuilder;
	}

	public IResourceWriter DefineResource(string name, string description, string fileName)
	{
		return DefineResource(name, description, fileName, ResourceAttributes.Public);
	}

	public IResourceWriter DefineResource(string name, string description, string fileName, ResourceAttributes attribute)
	{
		AddResourceFile(name, fileName, attribute, fileNeedsToExists: false);
		IResourceWriter resourceWriter = new ResourceWriter(fileName);
		if (resource_writers == null)
		{
			resource_writers = new ArrayList();
		}
		resource_writers.Add(resourceWriter);
		return resourceWriter;
	}

	private void AddUnmanagedResource(Win32Resource res)
	{
		MemoryStream memoryStream = new MemoryStream();
		res.WriteTo(memoryStream);
		if (win32_resources != null)
		{
			MonoWin32Resource[] destinationArray = new MonoWin32Resource[win32_resources.Length + 1];
			Array.Copy(win32_resources, destinationArray, win32_resources.Length);
			win32_resources = destinationArray;
		}
		else
		{
			win32_resources = new MonoWin32Resource[1];
		}
		win32_resources[win32_resources.Length - 1] = new MonoWin32Resource(res.Type.Id, res.Name.Id, res.Language, memoryStream.ToArray());
	}

	[MonoTODO("Not currently implemenented")]
	public void DefineUnmanagedResource(byte[] resource)
	{
		if (resource == null)
		{
			throw new ArgumentNullException("resource");
		}
		if (native_resource != NativeResourceType.None)
		{
			throw new ArgumentException("Native resource has already been defined.");
		}
		native_resource = NativeResourceType.Unmanaged;
		throw new NotImplementedException();
	}

	public void DefineUnmanagedResource(string resourceFileName)
	{
		if (resourceFileName == null)
		{
			throw new ArgumentNullException("resourceFileName");
		}
		if (resourceFileName.Length == 0)
		{
			throw new ArgumentException("resourceFileName");
		}
		if (!File.Exists(resourceFileName) || Directory.Exists(resourceFileName))
		{
			throw new FileNotFoundException("File '" + resourceFileName + "' does not exist or is a directory.");
		}
		if (native_resource != NativeResourceType.None)
		{
			throw new ArgumentException("Native resource has already been defined.");
		}
		native_resource = NativeResourceType.Unmanaged;
		using FileStream s = new FileStream(resourceFileName, FileMode.Open, FileAccess.Read);
		foreach (Win32EncodedResource item in new Win32ResFileReader(s).ReadResources())
		{
			if (item.Name.IsName || item.Type.IsName)
			{
				throw new InvalidOperationException("resource files with named resources or non-default resource types are not supported.");
			}
			AddUnmanagedResource(item);
		}
	}

	public void DefineVersionInfoResource()
	{
		if (native_resource != NativeResourceType.None)
		{
			throw new ArgumentException("Native resource has already been defined.");
		}
		native_resource = NativeResourceType.Assembly;
		version_res = new Win32VersionResource(1, 0, compilercontext: false);
	}

	public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
	{
		if (native_resource != NativeResourceType.None)
		{
			throw new ArgumentException("Native resource has already been defined.");
		}
		native_resource = NativeResourceType.Explicit;
		version_res = new Win32VersionResource(1, 0, compilercontext: false);
		version_res.ProductName = ((product != null) ? product : " ");
		version_res.ProductVersion = ((productVersion != null) ? productVersion : " ");
		version_res.CompanyName = ((company != null) ? company : " ");
		version_res.LegalCopyright = ((copyright != null) ? copyright : " ");
		version_res.LegalTrademarks = ((trademark != null) ? trademark : " ");
	}

	private void DefineVersionInfoResourceImpl(string fileName)
	{
		if (versioninfo_culture != null)
		{
			version_res.FileLanguage = new CultureInfo(versioninfo_culture).LCID;
		}
		version_res.Version = ((version == null) ? "0.0.0.0" : version);
		if (cattrs != null)
		{
			switch (native_resource)
			{
			case NativeResourceType.Assembly:
			{
				CustomAttributeBuilder[] array = cattrs;
				foreach (CustomAttributeBuilder customAttributeBuilder2 in array)
				{
					switch (customAttributeBuilder2.Ctor.ReflectedType.FullName)
					{
					case "System.Reflection.AssemblyProductAttribute":
						version_res.ProductName = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyCompanyAttribute":
						version_res.CompanyName = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyCopyrightAttribute":
						version_res.LegalCopyright = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyTrademarkAttribute":
						version_res.LegalTrademarks = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyCultureAttribute":
						version_res.FileLanguage = new CultureInfo(customAttributeBuilder2.string_arg()).LCID;
						break;
					case "System.Reflection.AssemblyFileVersionAttribute":
						version_res.FileVersion = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyInformationalVersionAttribute":
						version_res.ProductVersion = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyTitleAttribute":
						version_res.FileDescription = customAttributeBuilder2.string_arg();
						break;
					case "System.Reflection.AssemblyDescriptionAttribute":
						version_res.Comments = customAttributeBuilder2.string_arg();
						break;
					}
				}
				break;
			}
			case NativeResourceType.Explicit:
			{
				CustomAttributeBuilder[] array = cattrs;
				foreach (CustomAttributeBuilder customAttributeBuilder in array)
				{
					string fullName = customAttributeBuilder.Ctor.ReflectedType.FullName;
					if (fullName == "System.Reflection.AssemblyCultureAttribute")
					{
						version_res.FileLanguage = new CultureInfo(customAttributeBuilder.string_arg()).LCID;
					}
					else if (fullName == "System.Reflection.AssemblyDescriptionAttribute")
					{
						version_res.Comments = customAttributeBuilder.string_arg();
					}
				}
				break;
			}
			}
		}
		version_res.OriginalFilename = fileName;
		version_res.InternalName = Path.GetFileNameWithoutExtension(fileName);
		AddUnmanagedResource(version_res);
	}

	public ModuleBuilder GetDynamicModule(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("Empty name is not legal.", "name");
		}
		if (modules != null)
		{
			for (int i = 0; i < modules.Length; i++)
			{
				if (modules[i].name == name)
				{
					return modules[i];
				}
			}
		}
		return null;
	}

	public override Type[] GetExportedTypes()
	{
		throw not_supported();
	}

	public override FileStream GetFile(string name)
	{
		throw not_supported();
	}

	public override FileStream[] GetFiles(bool getResourceModules)
	{
		throw not_supported();
	}

	internal override Module[] GetModulesInternal()
	{
		if (modules == null)
		{
			return new Module[0];
		}
		return (Module[])modules.Clone();
	}

	internal override Type[] GetTypes(bool exportedOnly)
	{
		Type[] array = null;
		if (modules != null)
		{
			for (int i = 0; i < modules.Length; i++)
			{
				Type[] types = modules[i].GetTypes();
				if (array == null)
				{
					array = types;
					continue;
				}
				Type[] destinationArray = new Type[array.Length + types.Length];
				Array.Copy(array, 0, destinationArray, 0, array.Length);
				Array.Copy(types, 0, destinationArray, array.Length, types.Length);
			}
		}
		if (loaded_modules != null)
		{
			for (int j = 0; j < loaded_modules.Length; j++)
			{
				Type[] types2 = loaded_modules[j].GetTypes();
				if (array == null)
				{
					array = types2;
					continue;
				}
				Type[] destinationArray2 = new Type[array.Length + types2.Length];
				Array.Copy(array, 0, destinationArray2, 0, array.Length);
				Array.Copy(types2, 0, destinationArray2, array.Length, types2.Length);
			}
		}
		if (array != null)
		{
			List<Exception> list = null;
			Type[] array2 = array;
			foreach (Type type in array2)
			{
				if (type is TypeBuilder)
				{
					if (list == null)
					{
						list = new List<Exception>();
					}
					list.Add(new TypeLoadException($"Type '{type.FullName}' is not finished"));
				}
			}
			if (list != null)
			{
				throw new ReflectionTypeLoadException(new Type[list.Count], list.ToArray());
			}
		}
		if (array != null)
		{
			return array;
		}
		return Type.EmptyTypes;
	}

	public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
	{
		throw not_supported();
	}

	public override string[] GetManifestResourceNames()
	{
		throw not_supported();
	}

	public override Stream GetManifestResourceStream(string name)
	{
		throw not_supported();
	}

	public override Stream GetManifestResourceStream(Type type, string name)
	{
		throw not_supported();
	}

	internal override Module GetManifestModule()
	{
		if (manifest_module == null)
		{
			manifest_module = DefineDynamicModule("Default Dynamic Module");
		}
		return manifest_module;
	}

	[MonoLimitation("No support for PE32+ assemblies for AMD64 and IA64")]
	public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
	{
		peKind = portableExecutableKind;
		machine = imageFileMachine;
		if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage || (peKind & PortableExecutableKinds.Unmanaged32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
		{
			throw new NotImplementedException(peKind.ToString());
		}
		if (machine == ImageFileMachine.IA64 || machine == ImageFileMachine.AMD64)
		{
			throw new NotImplementedException(machine.ToString());
		}
		if (resource_writers != null)
		{
			foreach (IResourceWriter resource_writer in resource_writers)
			{
				resource_writer.Generate();
				resource_writer.Close();
			}
		}
		ModuleBuilder moduleBuilder = null;
		ModuleBuilder[] array;
		if (modules != null)
		{
			array = modules;
			foreach (ModuleBuilder moduleBuilder2 in array)
			{
				if (moduleBuilder2.FileName == assemblyFileName)
				{
					moduleBuilder = moduleBuilder2;
				}
			}
		}
		if (moduleBuilder == null)
		{
			moduleBuilder = DefineDynamicModule("RefEmit_OnDiskManifestModule", assemblyFileName);
		}
		if (!is_module_only)
		{
			moduleBuilder.IsMain = true;
		}
		if (entry_point != null && entry_point.DeclaringType.Module != moduleBuilder)
		{
			Type[] array2 = ((entry_point.GetParametersCount() != 1) ? Type.EmptyTypes : new Type[1] { typeof(string) });
			MethodBuilder methodBuilder = moduleBuilder.DefineGlobalMethod("__EntryPoint$", MethodAttributes.Static, entry_point.ReturnType, array2);
			ILGenerator iLGenerator = methodBuilder.GetILGenerator();
			if (array2.Length == 1)
			{
				iLGenerator.Emit(OpCodes.Ldarg_0);
			}
			iLGenerator.Emit(OpCodes.Tailcall);
			iLGenerator.Emit(OpCodes.Call, entry_point);
			iLGenerator.Emit(OpCodes.Ret);
			entry_point = methodBuilder;
		}
		if (version_res != null)
		{
			DefineVersionInfoResourceImpl(assemblyFileName);
		}
		if (sn != null)
		{
			public_key = sn.PublicKey;
		}
		array = modules;
		foreach (ModuleBuilder moduleBuilder3 in array)
		{
			if (moduleBuilder3 != moduleBuilder)
			{
				moduleBuilder3.Save();
			}
		}
		moduleBuilder.Save();
		if (sn != null && sn.CanSign)
		{
			sn.Sign(Path.Combine(AssemblyDir, assemblyFileName));
		}
		created = true;
	}

	public void Save(string assemblyFileName)
	{
		Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
	}

	public void SetEntryPoint(MethodInfo entryMethod)
	{
		SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
	}

	public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
	{
		if (entryMethod == null)
		{
			throw new ArgumentNullException("entryMethod");
		}
		if (entryMethod.DeclaringType.Assembly != this)
		{
			throw new InvalidOperationException("Entry method is not defined in the same assembly.");
		}
		entry_point = entryMethod;
		pekind = fileKind;
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		if (customBuilder == null)
		{
			throw new ArgumentNullException("customBuilder");
		}
		if (cattrs != null)
		{
			CustomAttributeBuilder[] array = new CustomAttributeBuilder[cattrs.Length + 1];
			cattrs.CopyTo(array, 0);
			array[cattrs.Length] = customBuilder;
			cattrs = array;
		}
		else
		{
			cattrs = new CustomAttributeBuilder[1];
			cattrs[0] = customBuilder;
		}
		if (customBuilder.Ctor != null && customBuilder.Ctor.DeclaringType == typeof(RuntimeCompatibilityAttribute))
		{
			UpdateNativeCustomAttributes(this);
		}
	}

	[ComVisible(true)]
	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
		if (con == null)
		{
			throw new ArgumentNullException("con");
		}
		if (binaryAttribute == null)
		{
			throw new ArgumentNullException("binaryAttribute");
		}
		SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
	}

	private Exception not_supported()
	{
		return new NotSupportedException("The invoked member is not supported in a dynamic module.");
	}

	private void check_name_and_filename(string name, string fileName, bool fileNeedsToExists)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (fileName == null)
		{
			throw new ArgumentNullException("fileName");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("Empty name is not legal.", "name");
		}
		if (fileName.Length == 0)
		{
			throw new ArgumentException("Empty file name is not legal.", "fileName");
		}
		if (Path.GetFileName(fileName) != fileName)
		{
			throw new ArgumentException("fileName '" + fileName + "' must not include a path.", "fileName");
		}
		string text = fileName;
		if (dir != null)
		{
			text = Path.Combine(dir, fileName);
		}
		if (fileNeedsToExists && !File.Exists(text))
		{
			throw new FileNotFoundException("Could not find file '" + fileName + "'");
		}
		if (resources != null)
		{
			for (int i = 0; i < resources.Length; i++)
			{
				if (resources[i].filename == text)
				{
					throw new ArgumentException("Duplicate file name '" + fileName + "'");
				}
				if (resources[i].name == name)
				{
					throw new ArgumentException("Duplicate name '" + name + "'");
				}
			}
		}
		if (modules == null)
		{
			return;
		}
		for (int j = 0; j < modules.Length; j++)
		{
			if (!modules[j].IsTransient() && modules[j].FileName == fileName)
			{
				throw new ArgumentException("Duplicate file name '" + fileName + "'");
			}
			if (modules[j].Name == name)
			{
				throw new ArgumentException("Duplicate name '" + name + "'");
			}
		}
	}

	private string create_assembly_version(string version)
	{
		string[] array = version.Split('.');
		int[] array2 = new int[4];
		if (array.Length < 0 || array.Length > 4)
		{
			throw new ArgumentException("The version specified '" + version + "' is invalid");
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == "*")
			{
				DateTime now = DateTime.Now;
				switch (i)
				{
				case 2:
					array2[2] = (now - new DateTime(2000, 1, 1)).Days;
					if (array.Length == 3)
					{
						array2[3] = (now.Second + now.Minute * 60 + now.Hour * 3600) / 2;
					}
					break;
				case 3:
					array2[3] = (now.Second + now.Minute * 60 + now.Hour * 3600) / 2;
					break;
				default:
					throw new ArgumentException("The version specified '" + version + "' is invalid");
				}
			}
			else
			{
				try
				{
					array2[i] = int.Parse(array[i]);
				}
				catch (FormatException)
				{
					throw new ArgumentException("The version specified '" + version + "' is invalid");
				}
			}
		}
		return array2[0] + "." + array2[1] + "." + array2[2] + "." + array2[3];
	}

	private string GetCultureString(string str)
	{
		if (!(str == "neutral"))
		{
			return str;
		}
		return string.Empty;
	}

	internal Type MakeGenericType(Type gtd, Type[] typeArguments)
	{
		return new TypeBuilderInstantiation(gtd, typeArguments);
	}

	public override Type GetType(string name, bool throwOnError, bool ignoreCase)
	{
		if (name == null)
		{
			throw new ArgumentNullException(name);
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("name", "Name cannot be empty");
		}
		Type type = InternalGetType(null, name, throwOnError, ignoreCase);
		if (type is TypeBuilder)
		{
			if (throwOnError)
			{
				throw new TypeLoadException($"Could not load type '{name}' from assembly '{this.name}'");
			}
			return null;
		}
		return type;
	}

	public override Module GetModule(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("Name can't be empty");
		}
		if (modules == null)
		{
			return null;
		}
		ModuleBuilder[] array = modules;
		foreach (Module module in array)
		{
			if (module.ScopeName == name)
			{
				return module;
			}
		}
		return null;
	}

	public override Module[] GetModules(bool getResourceModules)
	{
		Module[] modulesInternal = GetModulesInternal();
		if (!getResourceModules)
		{
			List<Module> list = new List<Module>(modulesInternal.Length);
			Module[] array = modulesInternal;
			foreach (Module module in array)
			{
				if (!module.IsResource())
				{
					list.Add(module);
				}
			}
			return list.ToArray();
		}
		return modulesInternal;
	}

	public override AssemblyName GetName(bool copiedName)
	{
		AssemblyName assemblyName = AssemblyName.Create(this, fillCodebase: false);
		if (sn != null)
		{
			assemblyName.SetPublicKey(sn.PublicKey);
			assemblyName.SetPublicKeyToken(sn.PublicKeyToken);
		}
		return assemblyName;
	}

	[MonoTODO("This always returns an empty array")]
	public override AssemblyName[] GetReferencedAssemblies()
	{
		return Assembly.GetReferencedAssemblies(this);
	}

	public override Module[] GetLoadedModules(bool getResourceModules)
	{
		return GetModules(getResourceModules);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Assembly GetSatelliteAssembly(CultureInfo culture)
	{
		StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
		return GetSatelliteAssembly(culture, null, throwOnError: true, ref stackMark);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
	{
		StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
		return GetSatelliteAssembly(culture, version, throwOnError: true, ref stackMark);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		if (assemblyName != null)
		{
			return assemblyName;
		}
		assemblyName = FullName;
		return assemblyName;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return MonoCustomAttrs.GetCustomAttributes(this, inherit);
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
	}

	internal override Evidence UnprotectedGetEvidence()
	{
		return null;
	}
}
