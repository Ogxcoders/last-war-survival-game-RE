using UnityEngine;
using UnityEngine.UI;

namespace PVEBattleLogic.Unit;

public class UnitHpBarGameObject
{
	internal int Handle;

	internal Color Color;

	private readonly string _path = "Assets/Main/Prefabs/LWBattle/HpBarCommon.prefab";

	private bool _loaded;

	private bool _visible;

	private InstanceRequest _request;

	private GameObject _gameObject;

	private Transform _transform;

	private RectTransform _rectTransform;

	private bool _valid = true;

	private Transform _parent;

	private float _height;

	private float _offsetX;

	private RectTransform _content;

	private Image _fg;

	private Image _shieldFg;

	private Image _shieldBg;

	private GameObject _shieldFgGo;

	private GameObject _shieldBgGo;

	private bool _shieldFgVisible;

	private bool _shieldBgVisible;

	private int _curHp;

	private int _maxHp;

	private int _shieldValue;

	private CanvasGroup _canvasGroup;

	private bool _usePveReturnOpt;

	private bool _canvasDirty;

	public void Show(Transform parent, float height = 0f, float offsetX = 0f, int curHp = 0, int maxHp = 1, int shieldValue = 0)
	{
		_parent = parent;
		_height = height;
		_offsetX = offsetX;
		_visible = true;
		_curHp = curHp;
		_maxHp = maxHp;
		_shieldValue = shieldValue;
		_usePveReturnOpt = GameEntry.Data?.Player?.CheckImmediateSwitch(6, defaultVal: false) ?? false;
		Load();
		if (_loaded)
		{
			OnShow();
		}
	}

	private void Load()
	{
		if (!string.IsNullOrEmpty(_path) && _request == null)
		{
			_loaded = false;
			int property = ((Handle == -1) ? 1 : 2);
			_request = GameEntry.Resource.InstantiateAsync(_path, ObjectPoolTag.Normal, property);
			_request.completed += RequestOnCompleted;
		}
	}

	private void RequestOnCompleted(InstanceRequest req)
	{
		if (req.isError)
		{
			req.Destroy();
			return;
		}
		if (!_valid)
		{
			req.Destroy();
			return;
		}
		_gameObject = req.gameObject;
		_transform = _gameObject.transform;
		_rectTransform = _transform as RectTransform;
		_loaded = true;
		OnLoaded();
		if (_visible)
		{
			OnShow();
		}
		SetVisible(_visible, force: true);
	}

	private void OnLoaded()
	{
		_canvasGroup = _transform.GetComponent<CanvasGroup>();
		_content = _transform.Find("Content").GetComponent<RectTransform>();
		_content.localPosition = new Vector3(_offsetX, 0f, 0f);
		_fg = _transform.Find("Content/fg").GetComponent<Image>();
		_shieldFg = _transform.Find("Content/fg/shieldFg").GetComponent<Image>();
		_shieldBg = _transform.Find("Content/shieldBg").GetComponent<Image>();
		_shieldFgGo = _shieldFg.gameObject;
		_shieldBgGo = _shieldBg.gameObject;
		_shieldFgGo.SetActive(value: false);
		_shieldBgGo.SetActive(value: false);
		_shieldFgVisible = false;
		_shieldBgVisible = false;
		_fg.fillAmount = 1f;
		_transform.SetParent(UnitViewFacade.GetHpBarCanvasTransform());
		_transform.localScale = Vector3.one;
	}

	internal void SetHp(int curHp, int maxHp, int curShieldValue = 0)
	{
		if (!_loaded)
		{
			_curHp = curHp;
			_maxHp = maxHp;
			_shieldValue = curShieldValue;
			return;
		}
		float fillAmount = 0f;
		if (maxHp > 0)
		{
			fillAmount = (float)curHp * 1f / (float)maxHp;
		}
		_fg.fillAmount = fillAmount;
		if (curShieldValue > 0)
		{
			int a = maxHp - curHp;
			a = Mathf.Min(a, curShieldValue);
			if (a > 0)
			{
				if (!_shieldFgVisible)
				{
					_shieldFgVisible = true;
					_shieldFgGo.SetActive(value: true);
				}
				float fillAmount2 = (float)a * 1f / (float)maxHp;
				_shieldFg.fillAmount = fillAmount2;
			}
			else if (_shieldFgVisible)
			{
				_shieldFgVisible = false;
				_shieldFgGo.SetActive(value: false);
			}
			int num = curShieldValue - a;
			if (num > 0)
			{
				if (!_shieldBgVisible)
				{
					_shieldBgVisible = true;
					_shieldBgGo.SetActive(value: true);
				}
				float a2 = (float)num * 1f / (float)maxHp;
				a2 = Mathf.Min(a2, 1f);
				_shieldBg.fillAmount = a2;
			}
			else if (_shieldBgVisible)
			{
				_shieldBgVisible = false;
				_shieldBgGo.SetActive(value: false);
			}
		}
		else
		{
			if (_shieldBgVisible)
			{
				_shieldBgVisible = false;
				_shieldBgGo.SetActive(value: false);
			}
			if (_shieldFgVisible)
			{
				_shieldFgVisible = false;
				_shieldFgGo.SetActive(value: false);
			}
		}
	}

	private void OnShow()
	{
		_fg.color = Color;
		SetHp(_curHp, _maxHp, _shieldValue);
		UpdatePos();
		SetOffsetX(_offsetX);
	}

	internal void UpdatePos()
	{
		if (_loaded && _visible)
		{
			Vector3 worldPosition = _parent?.position ?? Vector3.zero;
			worldPosition.y += _height;
			Vector3 position = CSUtils.WorldPositionToUISpacePosition(worldPosition);
			_rectTransform.position = position;
		}
	}

	internal void UpdatePos(Camera mainCamera, Camera uiCamera)
	{
		if (_loaded && _visible)
		{
			Vector3 position = _parent?.position ?? Vector3.zero;
			position.y += _height;
			Vector3 position2 = mainCamera.WorldToScreenPoint(position);
			position2 = uiCamera.ScreenToWorldPoint(position2);
			_rectTransform.position = position2;
		}
	}

	internal void SetVisible(bool visible, bool force = false)
	{
		if (!force && visible == _visible)
		{
			return;
		}
		_visible = visible;
		if (_usePveReturnOpt && (bool)_canvasGroup)
		{
			_canvasGroup.alpha = (visible ? 1 : 0);
			_canvasDirty = true;
		}
		else if (_loaded)
		{
			_gameObject.SetActive(_visible);
			if (_canvasDirty && (bool)_canvasGroup)
			{
				_canvasGroup.alpha = 1f;
				_canvasDirty = false;
			}
		}
	}

	public void ReplaceTarget(Transform parent)
	{
		_parent = parent;
		UpdatePos();
	}

	public void SetOffsetX(float offsetX)
	{
		_offsetX = offsetX;
		if (_loaded)
		{
			_content.localPosition = new Vector3(_offsetX, 0f, 0f);
		}
	}

	internal void InPool()
	{
		SetVisible(visible: false);
	}

	internal void OutPool()
	{
		SetVisible(visible: true);
	}

	internal void Dispose()
	{
		SetVisible(visible: true);
		_valid = false;
		if (_request != null)
		{
			_request.Destroy();
			_request = null;
		}
		_canvasGroup = null;
		_canvasDirty = false;
		_gameObject = null;
		_transform = null;
		_rectTransform = null;
		_parent = null;
		_content = null;
		_fg = null;
		_shieldFg = null;
		_shieldBg = null;
		_shieldFgGo = null;
		_shieldBgGo = null;
	}
}
