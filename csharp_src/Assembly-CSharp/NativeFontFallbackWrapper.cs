using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using VEngine;

public class NativeFontFallbackWrapper
{
	private static NativeFontFallbackWrapper _instance;

	[SerializeField]
	private List<TMP_FontAsset> fontAssets;

	[SerializeField]
	private string[] requiredFonts = new string[7] { "NotoSansCJK-Regular", "NotoNaskhArabic-Regular", "DroidSansFallback", "Noto Sans", "Arial", "NotoSansTibetan-Regular", "NotoSansLao-Regular" };

	[SerializeField]
	private string[] requiredFontNamePrefix;

	[SerializeField]
	private Font[] monoEmojiFonts;

	[SerializeField]
	private string customEmojiFontForNativeInput;

	public static NativeFontFallbackWrapper Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new NativeFontFallbackWrapper();
			}
			return _instance;
		}
	}

	public void Init()
	{
		TMP_FontAsset tMP_FontAsset = GameEntry.Resource.LoadAsset("Assets/Main/TMPFont/Main/Chat_SDF.asset", typeof(TMP_FontAsset)).asset as TMP_FontAsset;
		List<TMP_FontAsset> fallbackFontAssetTable = tMP_FontAsset.fallbackFontAssetTable;
		if (fallbackFontAssetTable != null && fallbackFontAssetTable.Count > 0)
		{
			TMP_FontAsset tMP_FontAsset2 = tMP_FontAsset.fallbackFontAssetTable[tMP_FontAsset.fallbackFontAssetTable.Count - 1];
			if (tMP_FontAsset2 != null)
			{
				tMP_FontAsset2.isMultiAtlasTexturesEnabled = true;
				tMP_FontAsset2.atlasPopulationMode = AtlasPopulationMode.Dynamic;
				tMP_FontAsset2.loadGlyphFromSystemFonts = true;
			}
		}
	}

	private void CopyCustomFontFile(string filename)
	{
		if (string.IsNullOrEmpty(filename))
		{
			throw new ArgumentException("Filename cannot be null or empty.");
		}
		string text = Path.Combine(Application.streamingAssetsPath, filename + ".ttf");
		string text2 = Path.Combine(Application.persistentDataPath, filename + ".ttf");
		if (!File.Exists(text))
		{
			throw new FileNotFoundException("Source file not found: " + text);
		}
		if (!File.Exists(text2) || File.GetLastWriteTimeUtc(text) > File.GetLastWriteTimeUtc(text2))
		{
			File.Copy(text, text2, overwrite: true);
		}
	}

	private bool AddFallback(string[] allFontsPaths, string containsName, bool isEmoji)
	{
		string text = null;
		for (int i = 0; i < allFontsPaths.Length; i++)
		{
			if (allFontsPaths[i].IndexOf(containsName, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				text = allFontsPaths[i];
				break;
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			Debug.Log("Font not found: " + containsName);
			return false;
		}
		Font osFont = new Font(text);
		return AddOSFontFallback(osFont, isEmoji);
	}

	private bool AddOSFontFallback(Font osFont, bool isEmoji)
	{
		if (osFont == null)
		{
			Debug.LogError("The os font is null!");
			return false;
		}
		TMP_FontAsset tMP_FontAsset = null;
		tMP_FontAsset = (isEmoji ? TMP_FontAsset.CreateFontAsset(osFont, 90, 9, GlyphRenderMode.RASTER, 2048, 2048) : TMP_FontAsset.CreateFontAsset(osFont));
		if (tMP_FontAsset == null)
		{
			string text = ((osFont.fontNames != null) ? osFont.fontNames[0] : "Null");
			Debug.LogError("Create font asset error with " + text);
			return false;
		}
		for (int i = 0; i < fontAssets.Count; i++)
		{
			if (fontAssets[i] != null)
			{
				fontAssets[i].fallbackFontAssetTable.Add(tMP_FontAsset);
			}
		}
		return true;
	}

	private void LoadAssets()
	{
		fontAssets = new List<TMP_FontAsset>();
		string[] files = Directory.GetFiles("Assets/Main/TMPFont/Main");
		string text = "";
		string[] array = files;
		foreach (string text2 in array)
		{
			if (text2.EndsWith(".asset"))
			{
				text = text2.Replace("\\", "/");
				Asset asset = GameEntry.Resource.LoadAsset(text, typeof(TMP_FontAsset));
				if (asset != null)
				{
					TMP_FontAsset item = asset.asset as TMP_FontAsset;
					fontAssets.Add(item);
				}
			}
		}
	}
}
