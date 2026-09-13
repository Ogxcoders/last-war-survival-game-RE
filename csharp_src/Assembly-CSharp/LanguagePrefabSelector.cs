using System;
using System.Collections.Generic;
using GameFramework.Localization;
using UnityEngine;

public class LanguagePrefabSelector : MonoBehaviour
{
	[Serializable]
	public class Entry
	{
		public Language language;

		public string localPrefabPath;
	}

	public List<Entry> entries = new List<Entry>();

	public string GetPrefabPathFromConfig(Language current)
	{
		foreach (Entry entry in entries)
		{
			if (entry.language == current && !string.IsNullOrEmpty(entry.localPrefabPath))
			{
				return entry.localPrefabPath;
			}
		}
		return "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";
	}
}
