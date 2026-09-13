using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILanguageSetImageComponent : MonoBehaviour
{
	[Serializable]
	public class LanguageAndImageConfig
	{
		public string _language;

		public Image _image;

		public RawImage _rawImage;

		public Sprite _sprite;

		public Texture _texture;
	}

	public LanguageAndImageConfig[] _langeConfigs;

	public bool _onlyEffectiveOnAwake;

	private Image _curImage;

	private RawImage _curRawImage;

	private Sprite _curSprite;

	private Texture _curTexture;

	private void Awake()
	{
		if (_onlyEffectiveOnAwake)
		{
			ForceRefresh(GetConfig());
		}
	}

	private void OnEnable()
	{
		if (!_onlyEffectiveOnAwake)
		{
			ForceRefresh(GetConfig());
		}
	}

	private List<LanguageAndImageConfig> GetConfig()
	{
		List<LanguageAndImageConfig> list = new List<LanguageAndImageConfig>();
		if (_langeConfigs != null && _langeConfigs.Length != 0)
		{
			string languageName = GameEntry.Localization.GetLanguageName();
			for (int i = 0; i < _langeConfigs.Length; i++)
			{
				if (_langeConfigs[i]._language == languageName)
				{
					list.Add(_langeConfigs[i]);
				}
			}
		}
		return list;
	}

	private void ForceRefresh(List<LanguageAndImageConfig> configs)
	{
		if (configs == null || configs.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < configs.Count; i++)
		{
			_curImage = configs[i]._image;
			_curRawImage = configs[i]._rawImage;
			_curSprite = configs[i]._sprite;
			_curTexture = configs[i]._texture;
			if (_curImage != null)
			{
				_curImage.sprite = _curSprite;
			}
			else if (_curRawImage != null)
			{
				_curRawImage.texture = _curTexture;
			}
			_curImage = null;
			_curRawImage = null;
		}
	}
}
