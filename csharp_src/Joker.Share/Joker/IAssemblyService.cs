using System;
using System.Collections.Generic;

namespace Joker;

public interface IAssemblyService : IService
{
	Action GetInvokeAction(string dll, string clzName, string methodName, object self = null, params object[] args);

	object Invoke(string dll, string clzName, string methodName, object self = null, params object[] args);

	void SetInstanceType<T>(Type type);

	void SetInstanceType<T>(string type);

	T CreateInstanceType<T>(Type defaultType = null);

	HashSet<Type> GetTypes(Type attr);

	Type GetType(string typeName);

	Dictionary<string, Type> GetTypes();

	void LoadAotMetadata(string dll);

	void LoadHotfix(string dll);
}
