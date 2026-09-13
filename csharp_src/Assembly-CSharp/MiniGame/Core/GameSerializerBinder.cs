using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiniGame.Core;

public abstract class GameSerializerBinder : SerializationBinder, ISerializationBinder
{
	private const string kmscorlib = "mscorlib";

	private const string kSystemCoreLib = "System.Private.CoreLib";

	private const string kmscorlibGeneric = ", mscorlib";

	private const string kSystemCoreLibGeneric = ", System.Private.CoreLib";

	private readonly ThreadSafeStore<StructMultiKey<string, string>, Type> _typeCache;

	private readonly ThreadSafeStore<Type, StructMultiKey<string, string>> _nameCache;

	public GameSerializerBinder()
	{
		_typeCache = new ThreadSafeStore<StructMultiKey<string, string>, Type>(GetTypeFromTypeNameKey);
		_nameCache = new ThreadSafeStore<Type, StructMultiKey<string, string>>(GetTypeNameFromTypeKey);
	}

	private StructMultiKey<string, string> GetTypeNameFromTypeKey(Type serializedType)
	{
		string text = serializedType.Assembly.GetName().Name;
		if (text == "mscorlib" || text == "System.Private.CoreLib")
		{
			text = null;
		}
		string fullName = serializedType.FullName;
		fullName = ReflectionUtils.RemoveAssemblyDetails(fullName);
		fullName = fullName.Replace(", mscorlib", string.Empty);
		fullName = fullName.Replace(", System.Private.CoreLib", string.Empty);
		text = text?.Trim();
		fullName = fullName.Trim();
		SerializeTypeAssembly(ref text, ref fullName);
		return new StructMultiKey<string, string>(text, fullName);
	}

	private Type GetTypeFromTypeNameKey(StructMultiKey<string, string> typeNameKey)
	{
		string assemblyName = typeNameKey.Value1?.Trim();
		string typeName = typeNameKey.Value2.Trim();
		DeserializeTypeAssembly(ref assemblyName, ref typeName);
		if (assemblyName == null)
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			throw new ArgumentException("The type [" + typeName + ",null] could not be found.");
		}
		if (assemblyName.Contains("MiniGame.biubiu.Share"))
		{
			DeserializeTypeAssembly(ref assemblyName, ref typeName);
		}
		Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
		if (assembly == null)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly2 in assemblies)
			{
				if (assembly2.FullName == assemblyName || assembly2.GetName().Name == assemblyName)
				{
					assembly = assembly2;
					break;
				}
			}
		}
		if (assembly == null)
		{
			throw new JsonSerializationException("Could not load assembly '" + assemblyName + "'.");
		}
		Type type2 = assembly.GetType(typeName);
		if (type2 == null)
		{
			if (typeName.IndexOf('`') >= 0)
			{
				try
				{
					type2 = GetGenericTypeFromTypeName(typeName, assembly);
				}
				catch (Exception innerException)
				{
					throw new JsonSerializationException("Could not find type '" + typeName + "' in assembly '" + assembly.FullName + "'.", innerException);
				}
			}
			if (type2 == null)
			{
				throw new JsonSerializationException("Could not find type '" + typeName + "' in assembly '" + assembly.FullName + "'.");
			}
		}
		return type2;
	}

	private Type GetGenericTypeFromTypeName(string typeName, Assembly assembly)
	{
		Type result = null;
		int num = typeName.IndexOf('[');
		if (num >= 0)
		{
			string name = typeName.Substring(0, num);
			Type type = assembly.GetType(name);
			if (type != null)
			{
				List<Type> list = new List<Type>();
				int num2 = 0;
				int num3 = 0;
				int num4 = typeName.Length - 1;
				for (int i = num + 1; i < num4; i++)
				{
					switch (typeName[i])
					{
					case '[':
						if (num2 == 0)
						{
							num3 = i + 1;
						}
						num2++;
						break;
					case ']':
						num2--;
						if (num2 == 0)
						{
							StructMultiKey<string, string> typeNameKey = ReflectionUtils.SplitFullyQualifiedTypeName(typeName.Substring(num3, i - num3));
							list.Add(GetTypeByName(typeNameKey));
						}
						break;
					}
				}
				result = type.MakeGenericType(list.ToArray());
			}
		}
		return result;
	}

	private Type GetTypeByName(StructMultiKey<string, string> typeNameKey)
	{
		return _typeCache.Get(typeNameKey);
	}

	private StructMultiKey<string, string> GetNameByType(Type type)
	{
		return _nameCache.Get(type);
	}

	public override Type BindToType(string assemblyName, string typeName)
	{
		return GetTypeByName(new StructMultiKey<string, string>(assemblyName, typeName));
	}

	public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
	{
		StructMultiKey<string, string> nameByType = GetNameByType(serializedType);
		assemblyName = nameByType.Value1;
		typeName = nameByType.Value2;
	}

	protected abstract void SerializeTypeAssembly(ref string assemblyName, ref string typeName);

	protected abstract void DeserializeTypeAssembly(ref string assemblyName, ref string typeName);
}
