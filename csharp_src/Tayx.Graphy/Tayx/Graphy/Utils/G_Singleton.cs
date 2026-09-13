using UnityEngine;

namespace Tayx.Graphy.Utils;

public class G_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static object _lock = new object();

	private static bool _applicationIsQuitting = false;

	public static T Instance
	{
		get
		{
			if (_applicationIsQuitting)
			{
				return null;
			}
			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = (T)Object.FindObjectOfType(typeof(T));
					if (Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						return _instance;
					}
					if (_instance == null)
					{
						Debug.Log(string.Concat("[Singleton] An instance of ", typeof(T), " is trying to be accessed, but it wasn't initialized first. Make sure to add an instance of ", typeof(T), " in the scene before  trying to access it."));
					}
				}
				return _instance;
			}
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			_instance = GetComponent<T>();
		}
	}

	private void OnDestroy()
	{
		_applicationIsQuitting = true;
	}
}
