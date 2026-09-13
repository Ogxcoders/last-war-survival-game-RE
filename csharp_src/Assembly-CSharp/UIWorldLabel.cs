using UnityEngine;

public class UIWorldLabel : MonoBehaviour
{
	[SerializeField]
	private new SuperTextMesh name;

	private string showingName;

	private Color showingColor;

	[SerializeField]
	private SpriteRenderer nameTitle;

	[SerializeField]
	private SuperTextMesh level;

	private int showingLevel;

	[SerializeField]
	private SpriteRenderer nameBg;

	[SerializeField]
	private SuperTextMesh tip;

	[SerializeField]
	private SpriteRenderer flag;

	[SerializeField]
	private TextMeshProEx tipTMP;

	[SerializeField]
	private TextMeshProEx nameTmp;

	private int nameBGType;

	private const string DefaultBgName = "appellation_icon_arena";

	private int _skinId;

	private bool _flagShow;

	private GameObject _fireworkGo;

	private TouchObjectEventTrigger _fireworkTouchObjectEventTrigger;

	private bool _fireworkGoInitFlag;

	private string _playerUid;

	private float _flagSpace;

	private float _flagWidth;

	private float _nameWidth;

	private float _nameOffsetX;

	private void OnDestroy()
	{
		_fireworkGo = null;
		_fireworkGoInitFlag = false;
		_playerUid = null;
		if (_fireworkTouchObjectEventTrigger != null)
		{
			_fireworkTouchObjectEventTrigger.onPointerClick = null;
			_fireworkTouchObjectEventTrigger = null;
		}
	}

	public void SetLevel(string str)
	{
		level.text = str;
	}

	public void SetLevel(int l)
	{
		if (!(level == null) && showingLevel != l)
		{
			showingLevel = l;
			level.text = l.ToString();
		}
	}

	public void SetLevel(int l, GameDefines.CityLabelColorType color)
	{
		if (!(level == null))
		{
			if (showingLevel != l)
			{
				showingLevel = l;
				level.text = l.ToString();
			}
			Color labelSkinColor = SceneManager.World.GetLabelSkinColor(_skinId, (int)color);
			level.color32 = labelSkinColor;
		}
	}

	public void SetTip(string tipStr)
	{
		if (tip != null && tipStr != null)
		{
			tip.text = tipStr;
		}
		else if (tipTMP != null && tipStr != null)
		{
			tipTMP.text = tipStr;
		}
	}

	public void SetName(string n)
	{
		if (showingName == n)
		{
			return;
		}
		showingName = n;
		if (nameTmp != null)
		{
			nameTmp.text = n;
			nameTmp.gameObject.SetActive(value: true);
			if (name != null)
			{
				name.gameObject.SetActive(value: false);
			}
			Invoke("TmpNameBgAutoFit", 0.02f);
		}
		else if (name != null)
		{
			name.text = n;
			name.SetCallBack(NameBgAutoFit);
		}
	}

	public void SetName(string n, GameDefines.CityLabelColorType color)
	{
		Color labelSkinColor = SceneManager.World.GetLabelSkinColor(_skinId, (int)color);
		if (showingColor == labelSkinColor && showingName == n)
		{
			return;
		}
		showingColor = labelSkinColor;
		showingName = n;
		if (nameTmp != null)
		{
			nameTmp.text = n;
			nameTmp.color32 = labelSkinColor;
			nameTmp.gameObject.SetActive(value: true);
			if (name != null)
			{
				name.gameObject.SetActive(value: false);
			}
			Invoke("TmpNameBgAutoFit", 0.02f);
		}
		else if (name != null)
		{
			name.text = n;
			name.SetCallBack(NameBgAutoFit);
		}
	}

	public void SetNameColor(GameDefines.CityLabelColorType color)
	{
		Color labelSkinColor = SceneManager.World.GetLabelSkinColor(_skinId, (int)color);
		if (showingColor == labelSkinColor)
		{
			return;
		}
		showingColor = labelSkinColor;
		if (nameTmp != null)
		{
			nameTmp.color32 = labelSkinColor;
			nameTmp.gameObject.SetActive(value: true);
			if (name != null)
			{
				name.gameObject.SetActive(value: false);
			}
		}
		else if (name != null)
		{
			name.color32 = labelSkinColor;
		}
	}

	public void SetLevel(bool active)
	{
		level.transform.parent.gameObject.SetActive(active);
	}

	public void ShowNameTitle(bool s)
	{
		nameTitle.gameObject.SetActive(s);
	}

	public void ShowFlag(bool s)
	{
		if (flag != null)
		{
			flag.gameObject.SetActive(s);
			_flagShow = s;
		}
	}

	public void SetFlag(string flag)
	{
		if (this.flag != null)
		{
			if (!WorldPointManager.allCountryFlagSet.Contains(flag))
			{
				flag = WorldPointManager.defaultCountryFlag;
			}
			this.flag.LoadSprite("Assets/Main/Sprites/CountryFlag/" + flag + ".png");
		}
	}

	private void NameBgAutoFit()
	{
		if (nameBg != null && (bool)nameBg.sprite)
		{
			Vector4 vector = GetWorldNameBgSize(textWidth: _nameWidth = name.GetWidth(), skinId: _skinId);
			_flagSpace = vector.z;
			_flagWidth = vector.w;
			if (flag != null && _flagShow)
			{
				nameBg.size = new Vector2(vector.x + (_flagSpace + _flagWidth) * 2f, vector.y);
				Transform obj = flag.transform;
				Vector3 localPosition = obj.localPosition;
				localPosition.x = _nameWidth / 2f + _flagSpace + _flagWidth / 2f + _nameOffsetX;
				obj.localPosition = localPosition;
			}
			else
			{
				nameBg.size = vector;
			}
		}
	}

	private void TmpNameBgAutoFit()
	{
		if (nameBg != null && (bool)nameBg.sprite)
		{
			Vector4 vector = GetWorldNameBgSize(textWidth: _nameWidth = nameTmp.GetWidth(), skinId: _skinId);
			_flagSpace = vector.z;
			_flagWidth = vector.w;
			if (flag != null && _flagShow)
			{
				nameBg.size = new Vector2(vector.x + (_flagSpace + _flagWidth) * 2f, vector.y);
				Transform obj = flag.transform;
				Vector3 localPosition = obj.localPosition;
				localPosition.x = _nameWidth / 2f + _flagSpace + _flagWidth / 2f + _nameOffsetX;
				obj.localPosition = localPosition;
			}
			else
			{
				nameBg.size = vector;
			}
		}
	}

	private Vector4 GetWorldNameBgSize(int skinId, float textWidth)
	{
		float num = 1.5f;
		if (skinId != 0)
		{
			num = SceneManager.World.GetLabelSkinSizeAdd(skinId);
		}
		return new Vector4(Mathf.Max(3.13f, textWidth + num), 1.09f, 0.1f, 0.4f);
	}

	public void SetNameBgSkin(int skinId = 0)
	{
		_skinId = skinId;
		if (!(nameBg != null))
		{
			return;
		}
		float num = 0f;
		string text = "appellation_icon_arena";
		if (_skinId != 0)
		{
			text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", _skinId, "image");
			if (!string.IsNullOrEmpty(text))
			{
				num = SceneManager.World.GetLabelSkinOffset(_skinId);
			}
		}
		_nameOffsetX = num;
		name.transform.SetLocalPositionX(num);
		nameBg.LoadSprite("Assets/Main/Sprites/UI/UITitleTag/" + text);
		if (flag != null && _flagShow)
		{
			Transform obj = flag.transform;
			Vector3 localPosition = obj.localPosition;
			localPosition.x = _nameWidth / 2f + _flagSpace + _flagWidth / 2f + _nameOffsetX;
			obj.localPosition = localPosition;
		}
	}

	public void SetFireworkQuickMode(bool isInFireworkQuickMode, string playerUid)
	{
		_playerUid = playerUid;
		if (_fireworkGo == null && !_fireworkGoInitFlag)
		{
			_fireworkGo = base.transform.Find("Firework")?.gameObject;
			if (_fireworkGo != null)
			{
				_fireworkTouchObjectEventTrigger = _fireworkGo.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (_fireworkTouchObjectEventTrigger != null)
				{
					_fireworkTouchObjectEventTrigger.previewType = WorldPreviewType.HighThanMultiObjects;
					_fireworkTouchObjectEventTrigger.onPointerClick = OnFireworkBtnClick;
				}
			}
			_fireworkGoInitFlag = true;
		}
		if (_fireworkGo != null && _fireworkGo.activeSelf != isInFireworkQuickMode)
		{
			_fireworkGo.SetActive(isInFireworkQuickMode);
		}
	}

	private void OnFireworkBtnClick()
	{
		if (!string.IsNullOrEmpty(_playerUid))
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.UseFireworkItem", _playerUid);
		}
	}
}
