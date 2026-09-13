using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Joker;

public abstract class AssemblyService : IAssemblyService, IService
{
	public static IAssemblyService Instance;

	private const string JokerDllClient = "Joker.Core.Client";

	private const string JokerDllServer = "Joker.Core.Server";

	private const string JokerDllShare = "Joker.Core.Share";

	protected readonly ConcurrentDictionary<string, Assembly> _assemblies = new ConcurrentDictionary<string, Assembly>();

	protected readonly Dictionary<string, Type> _allTypes = new Dictionary<string, Type>();

	protected readonly UnOrderMultiMapSet<Type, Type> _types = new UnOrderMultiMapSet<Type, Type>();

	protected readonly string[] _logicDlls;

	protected readonly List<Assembly> _logicAssemblies = new List<Assembly>();

	public AssemblyService()
		: this(Array.Empty<string>())
	{
	}

	public AssemblyService(string[] logicDlls)
	{
		Instance = this;
		List<string> list = new List<string> { "Joker.Core.Client", "Joker.Core.Server", "Joker.Core.Share" };
		foreach (string text in logicDlls)
		{
			if (!string.IsNullOrEmpty(text))
			{
				list.AddRange(text.Split(new char[1] { ',' }));
			}
		}
		_logicDlls = list.ToArray();
		_Init();
	}

	public virtual Action GetInvokeAction(string dll, string clzName, string methodName, object self = null, params object[] args)
	{
		if (!string.IsNullOrEmpty(dll) && !string.IsNullOrEmpty(clzName) && !string.IsNullOrEmpty(methodName) && _assemblies.TryGetValue(dll, out var value))
		{
			if ((object)value.GetType(clzName) == null)
			{
				throw new ArgumentException("can not find class in dll:" + dll + " class:" + clzName);
			}
			MethodInfo method = value.GetType(clzName).GetMethod(methodName);
			if ((object)method == null)
			{
				throw new ArgumentException("can not find method in dll:" + dll + " class:" + clzName + " method:" + methodName);
			}
			return delegate
			{
				method.Invoke(self, args);
			};
		}
		return null;
	}

	public virtual object Invoke(string dll, string clzName, string methodName, object self = null, params object[] args)
	{
		if (!string.IsNullOrEmpty(dll) && !string.IsNullOrEmpty(clzName) && !string.IsNullOrEmpty(methodName) && _assemblies.TryGetValue(dll, out var value))
		{
			if ((object)value.GetType(clzName) == null)
			{
				throw new ArgumentException("can not find class in dll:" + dll + " class:" + clzName);
			}
			MethodInfo method = value.GetType(clzName).GetMethod(methodName);
			if ((object)method == null)
			{
				throw new ArgumentException("can not find method in dll:" + dll + " class:" + clzName + " method:" + methodName);
			}
			return method.Invoke(self, args);
		}
		throw new Exception("can not find invoke " + dll + "." + clzName + "." + methodName);
	}

	public void SetInstanceType<T>(Type type)
	{
		SetInstanceType<T>(type.FullName);
	}

	public void SetInstanceType<T>(string type)
	{
		Singleton<ConfigService>.Instance.SetString(typeof(T).FullName, type);
	}

	public T CreateInstanceType<T>(Type defaultType = null)
	{
		if (Singleton<ConfigService>.Instance.TryGetString(typeof(T).FullName, out var value))
		{
			return (T)Activator.CreateInstance(GetType(value));
		}
		if ((object)defaultType == null)
		{
			return default(T);
		}
		return (T)Activator.CreateInstance(defaultType);
	}

	public virtual void LoadAotMetadata(string dll)
	{
		throw new NotImplementedException();
	}

	public virtual void LoadHotfix(string dll)
	{
		foreach (KeyValuePair<string, Assembly> assembly in _assemblies)
		{
			if (assembly.Key.Equals(dll))
			{
				_logicAssemblies.Add(assembly.Value);
				_InitTypes(assembly.Value);
			}
		}
	}

	private void _Init()
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly in assemblies)
		{
			_assemblies.TryAdd(assembly.GetName().Name, assembly);
			for (int j = 0; j < _logicDlls.Length; j++)
			{
				if (assembly.GetName().Name.Equals(_logicDlls[j]))
				{
					_logicAssemblies.Add(assembly);
					_InitTypes(assembly);
				}
			}
		}
	}

	public static Dictionary<string, Type> GetAssemblyTypes(params Assembly[] args)
	{
		Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
		for (int i = 0; i < args.Length; i++)
		{
			Type[] types = args[i].GetTypes();
			foreach (Type type in types)
			{
				dictionary[type.FullName] = type;
			}
		}
		return dictionary;
	}

	public HashSet<Type> GetTypes(Type systemAttributeType)
	{
		if (!_types.ContainsKey(systemAttributeType))
		{
			return new HashSet<Type>();
		}
		return _types[systemAttributeType];
	}

	public Dictionary<string, Type> GetTypes()
	{
		return _allTypes;
	}

	public Type GetType(string typeName)
	{
		return _allTypes[typeName];
	}

	public virtual void Awake()
	{
	}

	public virtual void Startup()
	{
		foreach (Type type in GetTypes(typeof(AssemblyModuleAttribute)))
		{
			_StartDll(type);
		}
	}

	public virtual void Shutdown()
	{
		foreach (Type type in GetTypes(typeof(AssemblyModuleAttribute)))
		{
			MethodInfo method = type.GetMethod("Destroy");
			if ((object)method == null)
			{
				Log.Error("can not find static Destroy function in " + type.ToString());
			}
			else
			{
				method.Invoke(null, null);
			}
		}
	}

	public virtual void Destroy()
	{
	}

	protected void _InitTypes(Assembly asm)
	{
		Type[] exportedTypes = asm.GetExportedTypes();
		foreach (Type type in exportedTypes)
		{
			_allTypes[type.FullName] = type;
			if (!type.IsAbstract)
			{
				object[] customAttributes = type.GetCustomAttributes(typeof(ClassAttribute), inherit: true);
				foreach (object obj in customAttributes)
				{
					_types.Add(obj.GetType(), type);
				}
			}
		}
	}

	private void _StartDll(Type t)
	{
		MethodInfo method = t.GetMethod("Initialize");
		if ((object)method == null)
		{
			Log.Error("can not find static Initialize function in " + t.ToString());
		}
		else
		{
			method.Invoke(null, null);
		}
	}
}
