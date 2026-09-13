using System.Text;
using UnityEngine;

public class SceneSkinManager
{
	private static SceneSkinManager _instance;

	private const int DEFAULT_ID = 1;

	private SceneSkinMeta curMeta;

	private SceneSkinMeta viewMeta;

	public static SceneSkinManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SceneSkinManager();
			}
			return _instance;
		}
	}

	public static void Purge()
	{
		_instance = null;
	}

	public void SetCurSkinMeta(SceneSkinMeta meta)
	{
		SceneSkinMeta sceneSkinMeta = curMeta;
		curMeta = meta;
		PlayerPrefs.SetString("UI_LOADING_BG", "");
		PlayerPrefs.SetString("UI_LOADING_LOGO", "");
		PlayerPrefs.SetInt("SEASON_MAP_TYPE", meta?.mapType ?? 0);
		int loading_bgm = curMeta.loading_bgm;
		string soundPath = SoundComponent.GetSoundPath(loading_bgm);
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", loading_bgm, "sound_change");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", loading_bgm, "reactive");
		string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", loading_bgm, "loop_gap");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("1");
		stringBuilder.Append("|");
		stringBuilder.Append(loading_bgm.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(soundPath);
		stringBuilder.Append("|");
		stringBuilder.Append(templateData);
		stringBuilder.Append("|");
		stringBuilder.Append(templateData2);
		stringBuilder.Append("|");
		stringBuilder.Append(templateData3);
		PlayerPrefs.SetString("SeasonBGM", (meta == null) ? "" : stringBuilder.ToString());
		if (sceneSkinMeta != null && curMeta != null && ((sceneSkinMeta.id != curMeta.id && sceneSkinMeta.mapType != 6) || sceneSkinMeta.mapType != curMeta.mapType) && SceneManager.World != null)
		{
			SceneManager.World.OnSkinChange(ignoreCache: true);
		}
		if (meta != null && (meta.IsMummyMode() || meta.IsDarknessMode() || meta.IsNineNationMode()))
		{
			GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1 = 0;
		}
	}

	public void SetViewModeSkinMeta(SceneSkinMeta meta)
	{
		if ((viewMeta != null || meta == null || curMeta == null || meta.id != curMeta.id) && (viewMeta == null || meta == null || meta.id != viewMeta.id) && (viewMeta != null || meta != null))
		{
			bool ignoreCache = meta != null;
			viewMeta = meta;
			if (SceneManager.World != null)
			{
				SceneManager.World.OnSkinChange(ignoreCache);
			}
		}
	}

	public SceneSkinMeta GetCurSkinMeta()
	{
		if (viewMeta != null)
		{
			return viewMeta;
		}
		return curMeta;
	}

	public SceneSkinMeta GetBaseSkinMeta()
	{
		return curMeta;
	}
}
