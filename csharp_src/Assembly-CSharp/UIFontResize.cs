using System;
using UnityEngine;

public class UIFontResize : MonoBehaviour
{
	[Serializable]
	public class RegionAndFontSize
	{
		public string _language;

		public int _fontSize;

		public float _charSpace;

		public float _wordSpace;

		public float _lineSpacing;

		public string _colorString;
	}

	public RegionAndFontSize[] _configs;

	public bool _onlyEffectiveOnAwake;

	private TextMeshProUGUIEx _textpro;

	private float _curTextSpace;

	private float _curWorldSpace;

	private float _curFontSize;

	private float _curLineSpace;

	private string _curColorString;

	private float _preTextSpace;

	private float _preWorldSpace;

	private float _preFontSize;

	private float _preLineSpace;

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

	private RegionAndFontSize GetConfig()
	{
		RegionAndFontSize result = null;
		if (_configs != null && _configs.Length != 0)
		{
			string languageName = GameEntry.Localization.GetLanguageName();
			for (int i = 0; i < _configs.Length; i++)
			{
				if (_configs[i]._language == languageName)
				{
					result = _configs[i];
					break;
				}
			}
		}
		return result;
	}

	private void ForceRefresh(RegionAndFontSize config)
	{
		if (config == null)
		{
			return;
		}
		_curTextSpace = config._charSpace;
		_curWorldSpace = config._wordSpace;
		_curFontSize = config._fontSize;
		_curLineSpace = config._lineSpacing;
		_curColorString = config._colorString;
		_textpro = GetComponent<TextMeshProUGUIEx>();
		if ((_curLineSpace != 0f || _preLineSpace != _curLineSpace) && _textpro != null)
		{
			_textpro.lineSpacing = _curLineSpace;
		}
		if ((_curTextSpace != 0f || _preTextSpace != _curTextSpace) && _textpro != null)
		{
			_textpro.characterSpacing = _curTextSpace;
		}
		if ((_curWorldSpace != 0f || _preWorldSpace != _curWorldSpace) && _textpro != null)
		{
			_textpro.wordSpacing = _curWorldSpace;
		}
		if ((_curFontSize != 0f || _preFontSize != _curFontSize) && _textpro != null)
		{
			_textpro.enableAutoSizing = false;
			_textpro.fontSize = _curFontSize;
		}
		if (!string.IsNullOrEmpty(_curColorString) && _textpro != null)
		{
			try
			{
				ColorUtility.TryParseHtmlString(_curColorString, out var color);
				_textpro.color = color;
			}
			catch
			{
			}
		}
		_preTextSpace = _curTextSpace;
		_preWorldSpace = _curWorldSpace;
		_preFontSize = _curFontSize;
	}
}
