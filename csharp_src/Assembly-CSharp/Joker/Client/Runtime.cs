using UnityEngine;

namespace Joker.Client;

public class Runtime : MonoBehaviour
{
	private static Runtime ms_instance;

	public static Runtime Instance
	{
		get
		{
			if (ms_instance == null)
			{
				GameObject gameObject = new GameObject("__joker__runtime__");
				if (Application.isPlaying)
				{
					Object.DontDestroyOnLoad(gameObject);
				}
				ms_instance = gameObject.AddComponent<Runtime>();
			}
			return ms_instance;
		}
	}

	public static bool isValid => ms_instance != null;

	protected void OnDestroy()
	{
		ms_instance = null;
	}
}
