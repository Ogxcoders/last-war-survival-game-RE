using System;
using System.Collections.Generic;
using GameFramework.Localization;
using UnityEngine;

[Serializable]
public class LoadingPrefabSelector : MonoBehaviour
{
	[Serializable]
	public class CountryConfig
	{
		[Tooltip("国家或地区代码")]
		public CountryCode countryCode;

		[Tooltip("国家内每个语言对应的 Prefab 路径")]
		public List<LanguagePrefabEntry> languagePrefabs = new List<LanguagePrefabEntry>();
	}

	[Serializable]
	public class LanguagePrefabEntry
	{
		public Language language;

		public string prefabPath;
	}

	[Header("老脚本引用")]
	[Tooltip("引用原有 LanguagePrefabSelector，用于复用语言配置")]
	public LanguagePrefabSelector oldSelector;

	[Header("国家或地区配置列表（一个地区一个配置）")]
	public List<CountryConfig> countryConfigs = new List<CountryConfig>();

	[Header("默认Loading（找不到匹配时使用）")]
	public string fallbackPrefabPath = "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";

	public string GetPrefabPath(Language currentLanguage, string countryCodeStr)
	{
		if (!Enum.TryParse<CountryCode>(countryCodeStr, ignoreCase: true, out var result))
		{
			result = CountryCode.Default;
		}
		return GetPrefabPath(currentLanguage, result);
	}

	public string GetPrefabPath(Language currentLanguage, CountryCode countryCode)
	{
		CountryConfig countryConfig = countryConfigs.Find((CountryConfig c) => c.countryCode == countryCode);
		if (countryConfig != null)
		{
			LanguagePrefabEntry languagePrefabEntry = countryConfig.languagePrefabs.Find((LanguagePrefabEntry e) => e.language == currentLanguage);
			if (languagePrefabEntry != null && !string.IsNullOrEmpty(languagePrefabEntry.prefabPath))
			{
				return languagePrefabEntry.prefabPath;
			}
		}
		if (oldSelector == null)
		{
			Debug.LogError("[LanguagePrefabSelector] oldSelector 未绑定");
			return fallbackPrefabPath;
		}
		string pathByLanguage = GetPathByLanguage(currentLanguage);
		if (!string.IsNullOrEmpty(pathByLanguage))
		{
			return pathByLanguage;
		}
		return fallbackPrefabPath;
	}

	private string GetPathByLanguage(Language lang)
	{
		foreach (LanguagePrefabSelector.Entry entry in oldSelector.entries)
		{
			if (entry.language == lang && !string.IsNullOrEmpty(entry.localPrefabPath))
			{
				return entry.localPrefabPath;
			}
		}
		return null;
	}
}
