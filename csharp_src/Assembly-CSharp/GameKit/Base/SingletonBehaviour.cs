using GameFramework;
using UnityEngine;

namespace GameKit.Base;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	public enum UpdateMode
	{
		FIXED_UPDATE,
		UPDATE,
		LATE_UPDATE
	}

	private static T _instance;

	private static bool applicationIsQuitting;

	public UpdateMode updateMode = UpdateMode.UPDATE;

	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarning(string.Concat("[Singleton] Instance '", typeof(T), "' already destroyed on application quit. Won't create again - returning null."));
				return null;
			}
			if (_instance == null)
			{
				T[] array = Object.FindObjectsOfType<T>();
				if (array.Length > 1)
				{
					Log.Error("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
					_instance = array[0];
				}
				else if (array.Length != 0)
				{
					_instance = array[0];
				}
				if (_instance == null)
				{
					GameObject obj = new GameObject("(Singleton) " + typeof(T));
					_instance = obj.AddComponent<T>();
					Object.DontDestroyOnLoad(obj);
					obj.transform.SetParent(SingletonBehaviour<SingletonParent>.Instance.transform);
				}
			}
			return _instance;
		}
	}

	public static bool IsInstanceValid()
	{
		return _instance != null;
	}

	public static void DestroyInstance()
	{
		if (Instance != null)
		{
			applicationIsQuitting = true;
			Object.Destroy(_instance.gameObject);
			_instance = null;
		}
	}

	private void OnDestroy()
	{
		applicationIsQuitting = true;
		_instance = null;
	}

	private void Update()
	{
		if (updateMode == UpdateMode.UPDATE)
		{
			OnUpdate(Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{
		if (updateMode == UpdateMode.FIXED_UPDATE)
		{
			OnUpdate(Time.fixedDeltaTime);
		}
	}

	private void LateUpdate()
	{
		if (updateMode == UpdateMode.LATE_UPDATE)
		{
			OnUpdate(Time.deltaTime);
		}
	}

	protected virtual void OnUpdate(float delta)
	{
	}

	public virtual void Release()
	{
		Debug.Log("[Release] " + base.gameObject.name);
	}
}
