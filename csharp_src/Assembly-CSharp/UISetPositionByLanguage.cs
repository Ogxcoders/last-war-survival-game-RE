using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISetPositionByLanguage : MonoBehaviour
{
	[Serializable]
	public class RegionAndParameter
	{
		public string _language;

		public float _positionX;

		public float _positionY;

		public bool _changeSizeDelta;

		public float _width;

		public float _height;
	}

	public RegionAndParameter[] _configs;

	public bool _onlyEffectiveOnAwake;

	private RectTransform _rectTransform;

	private float _curPositionX;

	private float _curPositionY;

	private float _prePositionX;

	private float _prePositionY;

	private float _curWidth;

	private float _curHeight;

	private float _preWidth;

	private float _preHeight;

	private bool widthAndHeight;

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

	private RegionAndParameter GetConfig()
	{
		RegionAndParameter result = null;
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

	private void ForceRefresh(RegionAndParameter config)
	{
		if (config != null)
		{
			_curPositionX = config._positionX;
			_curPositionY = config._positionY;
			_curHeight = config._height;
			_curWidth = config._width;
			widthAndHeight = config._changeSizeDelta;
			_rectTransform = GetComponent<RectTransform>();
			if ((_prePositionX != _curPositionX || _prePositionY != _curPositionY) && _rectTransform != null)
			{
				_rectTransform.anchoredPosition = new Vector2(_curPositionX, _curPositionY);
			}
			if ((_preWidth != _curWidth || _preHeight != _curHeight) && _rectTransform != null && widthAndHeight)
			{
				_rectTransform.sizeDelta = new Vector2(_curWidth, _curHeight);
			}
			_preWidth = _curWidth;
			_preHeight = _curHeight;
			_prePositionY = _curPositionY;
			_prePositionX = _curPositionX;
		}
	}
}
