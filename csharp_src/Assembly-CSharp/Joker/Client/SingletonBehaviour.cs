using UnityEngine;

namespace Joker.Client;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T ms_instance;

	public static T Instance
	{
		get
		{
			if (ms_instance == null)
			{
				ms_instance = Runtime.Instance.gameObject.AddComponent<T>();
			}
			return ms_instance;
		}
	}

	public static bool IsValid => ms_instance != null;
}
