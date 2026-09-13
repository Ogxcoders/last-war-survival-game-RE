using System;
using UnityEngine;

public class MapData : ScriptableObject
{
	public GameObject[] go;

	public string[] pathList;

	public GameObject TryGetGameObject(string key)
	{
		int num = Array.IndexOf(pathList, key);
		if (num >= 0 && num < go.Length && go[num] != null)
		{
			return UnityEngine.Object.Instantiate(go[num]);
		}
		return null;
	}
}
