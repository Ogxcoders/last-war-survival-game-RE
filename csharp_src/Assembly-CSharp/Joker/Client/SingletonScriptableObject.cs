using UnityEngine;

namespace Joker.Client;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = ScriptableObject.CreateInstance<T>();
			}
			return _instance;
		}
	}

	private static void Cleanup()
	{
		if (_instance != null)
		{
			Object.DestroyImmediate(_instance);
			_instance = null;
		}
	}
}
