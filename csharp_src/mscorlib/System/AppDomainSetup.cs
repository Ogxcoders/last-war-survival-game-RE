using System.Collections.Generic;
using System.IO;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using Mono.Security;

namespace System;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[ClassInterface(ClassInterfaceType.None)]
[ComVisible(true)]
public sealed class AppDomainSetup
{
	private string application_base;

	private string application_name;

	private string cache_path;

	private string configuration_file;

	private string dynamic_base;

	private string license_file;

	private string private_bin_path;

	private string private_bin_path_probe;

	private string shadow_copy_directories;

	private string shadow_copy_files;

	private bool publisher_policy;

	private bool path_changed;

	private int loader_optimization;

	private bool disallow_binding_redirects;

	private bool disallow_code_downloads;

	private object _activationArguments;

	private object domain_initializer;

	private object application_trust;

	private string[] domain_initializer_args;

	private bool disallow_appbase_probe;

	private byte[] configuration_bytes;

	private byte[] serialized_non_primitives;

	private string manager_assembly;

	private string manager_type;

	private string[] partial_visible_assemblies;

	public string ApplicationBase
	{
		get
		{
			return GetAppBase(application_base);
		}
		set
		{
			application_base = value;
		}
	}

	public string ApplicationName
	{
		get
		{
			return application_name;
		}
		set
		{
			application_name = value;
		}
	}

	public string CachePath
	{
		get
		{
			return cache_path;
		}
		set
		{
			cache_path = value;
		}
	}

	public string ConfigurationFile
	{
		get
		{
			if (configuration_file == null)
			{
				return null;
			}
			if (Path.IsPathRooted(configuration_file))
			{
				return configuration_file;
			}
			if (ApplicationBase == null)
			{
				throw new MemberAccessException("The ApplicationBase must be set before retrieving this property.");
			}
			return Path.Combine(ApplicationBase, configuration_file);
		}
		set
		{
			configuration_file = value;
		}
	}

	public bool DisallowPublisherPolicy
	{
		get
		{
			return publisher_policy;
		}
		set
		{
			publisher_policy = value;
		}
	}

	public string DynamicBase
	{
		get
		{
			if (dynamic_base == null)
			{
				return null;
			}
			if (Path.IsPathRooted(dynamic_base))
			{
				return dynamic_base;
			}
			if (ApplicationBase == null)
			{
				throw new MemberAccessException("The ApplicationBase must be set before retrieving this property.");
			}
			return Path.Combine(ApplicationBase, dynamic_base);
		}
		set
		{
			if (application_name == null)
			{
				throw new MemberAccessException("ApplicationName must be set before the DynamicBase can be set.");
			}
			dynamic_base = Path.Combine(value, ((uint)application_name.GetHashCode()).ToString("x"));
		}
	}

	public string LicenseFile
	{
		get
		{
			return license_file;
		}
		set
		{
			license_file = value;
		}
	}

	[MonoLimitation("In Mono this is controlled by the --share-code flag")]
	public LoaderOptimization LoaderOptimization
	{
		get
		{
			return (LoaderOptimization)loader_optimization;
		}
		set
		{
			loader_optimization = (int)value;
		}
	}

	public string AppDomainManagerAssembly
	{
		get
		{
			return manager_assembly;
		}
		set
		{
			manager_assembly = value;
		}
	}

	public string AppDomainManagerType
	{
		get
		{
			return manager_type;
		}
		set
		{
			manager_type = value;
		}
	}

	public string[] PartialTrustVisibleAssemblies
	{
		get
		{
			return partial_visible_assemblies;
		}
		set
		{
			if (value != null)
			{
				partial_visible_assemblies = (string[])value.Clone();
				Array.Sort(partial_visible_assemblies, StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				partial_visible_assemblies = null;
			}
		}
	}

	public string PrivateBinPath
	{
		get
		{
			return private_bin_path;
		}
		set
		{
			private_bin_path = value;
			path_changed = true;
		}
	}

	public string PrivateBinPathProbe
	{
		get
		{
			return private_bin_path_probe;
		}
		set
		{
			private_bin_path_probe = value;
			path_changed = true;
		}
	}

	public string ShadowCopyDirectories
	{
		get
		{
			return shadow_copy_directories;
		}
		set
		{
			shadow_copy_directories = value;
		}
	}

	public string ShadowCopyFiles
	{
		get
		{
			return shadow_copy_files;
		}
		set
		{
			shadow_copy_files = value;
		}
	}

	public bool DisallowBindingRedirects
	{
		get
		{
			return disallow_binding_redirects;
		}
		set
		{
			disallow_binding_redirects = value;
		}
	}

	public bool DisallowCodeDownload
	{
		get
		{
			return disallow_code_downloads;
		}
		set
		{
			disallow_code_downloads = value;
		}
	}

	public string TargetFrameworkName { get; set; }

	public ActivationArguments ActivationArguments
	{
		get
		{
			if (_activationArguments != null)
			{
				return (ActivationArguments)_activationArguments;
			}
			DeserializeNonPrimitives();
			return (ActivationArguments)_activationArguments;
		}
		set
		{
			_activationArguments = value;
		}
	}

	[MonoLimitation("it needs to be invoked within the created domain")]
	public AppDomainInitializer AppDomainInitializer
	{
		get
		{
			if (domain_initializer != null)
			{
				return (AppDomainInitializer)domain_initializer;
			}
			DeserializeNonPrimitives();
			return (AppDomainInitializer)domain_initializer;
		}
		set
		{
			domain_initializer = value;
		}
	}

	[MonoLimitation("it needs to be used to invoke the initializer within the created domain")]
	public string[] AppDomainInitializerArguments
	{
		get
		{
			return domain_initializer_args;
		}
		set
		{
			domain_initializer_args = value;
		}
	}

	[MonoNotSupported("This property exists but not considered.")]
	public ApplicationTrust ApplicationTrust
	{
		get
		{
			if (application_trust != null)
			{
				return (ApplicationTrust)application_trust;
			}
			DeserializeNonPrimitives();
			if (application_trust == null)
			{
				application_trust = new ApplicationTrust();
			}
			return (ApplicationTrust)application_trust;
		}
		set
		{
			application_trust = value;
		}
	}

	[MonoNotSupported("This property exists but not considered.")]
	public bool DisallowApplicationBaseProbing
	{
		get
		{
			return disallow_appbase_probe;
		}
		set
		{
			disallow_appbase_probe = value;
		}
	}

	public AppDomainSetup()
	{
	}

	internal AppDomainSetup(AppDomainSetup setup)
	{
		application_base = setup.application_base;
		application_name = setup.application_name;
		cache_path = setup.cache_path;
		configuration_file = setup.configuration_file;
		dynamic_base = setup.dynamic_base;
		license_file = setup.license_file;
		private_bin_path = setup.private_bin_path;
		private_bin_path_probe = setup.private_bin_path_probe;
		shadow_copy_directories = setup.shadow_copy_directories;
		shadow_copy_files = setup.shadow_copy_files;
		publisher_policy = setup.publisher_policy;
		path_changed = setup.path_changed;
		loader_optimization = setup.loader_optimization;
		disallow_binding_redirects = setup.disallow_binding_redirects;
		disallow_code_downloads = setup.disallow_code_downloads;
		_activationArguments = setup._activationArguments;
		domain_initializer = setup.domain_initializer;
		application_trust = setup.application_trust;
		domain_initializer_args = setup.domain_initializer_args;
		disallow_appbase_probe = setup.disallow_appbase_probe;
		configuration_bytes = setup.configuration_bytes;
		manager_assembly = setup.manager_assembly;
		manager_type = setup.manager_type;
		partial_visible_assemblies = setup.partial_visible_assemblies;
	}

	public AppDomainSetup(ActivationArguments activationArguments)
	{
		_activationArguments = activationArguments;
	}

	public AppDomainSetup(ActivationContext activationContext)
	{
		_activationArguments = new ActivationArguments(activationContext);
	}

	private static string GetAppBase(string appBase)
	{
		if (appBase == null)
		{
			return null;
		}
		if (appBase.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
		{
			appBase = new Uri(appBase).LocalPath;
			if (Path.DirectorySeparatorChar != '/')
			{
				appBase = appBase.Replace('/', Path.DirectorySeparatorChar);
			}
		}
		appBase = Path.GetFullPath(appBase);
		if (Path.DirectorySeparatorChar != '/')
		{
			bool flag = appBase.StartsWith("\\\\?\\", StringComparison.Ordinal);
			if (appBase.IndexOf(':', flag ? 6 : 2) != -1)
			{
				throw new NotSupportedException("The given path's format is not supported.");
			}
		}
		string directoryName = Path.GetDirectoryName(appBase);
		if (directoryName != null && directoryName.LastIndexOfAny(Path.GetInvalidPathChars()) >= 0)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid path characters in path: '{0}'"), appBase), "appBase");
		}
		string fileName = Path.GetFileName(appBase);
		if (fileName != null && fileName.LastIndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid filename characters in path: '{0}'"), appBase), "appBase");
		}
		return appBase;
	}

	[MonoNotSupported("This method exists but not considered.")]
	public byte[] GetConfigurationBytes()
	{
		if (configuration_bytes == null)
		{
			return null;
		}
		return configuration_bytes.Clone() as byte[];
	}

	[MonoNotSupported("This method exists but not considered.")]
	public void SetConfigurationBytes(byte[] value)
	{
		configuration_bytes = value;
	}

	private void DeserializeNonPrimitives()
	{
		lock (this)
		{
			if (serialized_non_primitives != null)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream serializationStream = new MemoryStream(serialized_non_primitives);
				object[] array = (object[])binaryFormatter.Deserialize(serializationStream);
				_activationArguments = (ActivationArguments)array[0];
				domain_initializer = (AppDomainInitializer)array[1];
				application_trust = (ApplicationTrust)array[2];
				serialized_non_primitives = null;
			}
		}
	}

	internal void SerializeNonPrimitives()
	{
		object[] graph = new object[3] { _activationArguments, domain_initializer, application_trust };
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		binaryFormatter.Serialize(memoryStream, graph);
		serialized_non_primitives = memoryStream.ToArray();
	}

	[MonoTODO("not implemented, does not throw because it's used in testing moonlight")]
	public void SetCompatibilitySwitches(IEnumerable<string> switches)
	{
	}
}
