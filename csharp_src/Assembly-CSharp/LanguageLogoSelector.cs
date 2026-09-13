using System;
using System.Collections.Generic;
using GameFramework.Localization;
using UnityEngine;

public class LanguageLogoSelector : MonoBehaviour
{
	[Serializable]
	public class Entry
	{
		public Language language;

		public string localLogoPath;
	}

	public List<Entry> entries = new List<Entry>();

	public string GetLogoPathFromConfig(Language current)
	{
		foreach (Entry entry in entries)
		{
			if (entry.language == current && !string.IsNullOrEmpty(entry.localLogoPath))
			{
				return entry.localLogoPath;
			}
		}
		return "";
	}
}
