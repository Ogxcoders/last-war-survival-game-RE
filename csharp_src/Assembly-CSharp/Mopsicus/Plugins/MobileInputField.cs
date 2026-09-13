using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ArabicSupport;
using NiceJson;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Mopsicus.Plugins;

public class MobileInputField : MobileInputReceiver
{
	private struct MobileInputConfig
	{
		public bool Multiline;

		public Color TextColor;

		public Color BackgroundColor;

		public string ContentType;

		public string InputType;

		public string KeyboardType;

		public float FontSize;

		public string Align;

		public string Placeholder;

		public Color PlaceholderColor;

		public int CharacterLimit;
	}

	public struct Mention
	{
		public string text;

		public string color;

		public int index;
	}

	public enum ReturnKeyType
	{
		Default,
		Next,
		Done,
		Search,
		Send
	}

	public class OnChangeEvent : UnityEvent<string>
	{
	}

	public Action<string> OnTextChangeFromPlatform;

	private OnChangeEvent m_OnValueChanged = new OnChangeEvent();

	public string CustomFont = "default";

	public string CustomFontDir = "";

	public bool IsManualHideControl;

	public bool IsWithDoneButton;

	public bool IsWithClearButton = true;

	public ReturnKeyType ReturnKey;

	public Action OnReturnPressed = delegate
	{
	};

	public Action<bool> OnFocusChanged = delegate
	{
	};

	public UnityEvent OnReturnPressedEvent;

	private bool _isMobileInputCreated;

	private InputField _inputObject;

	private TMP_InputField _inputObjectTmproEx;

	private Text _inputObjectText;

	private TextMeshProUGUIEx _inputObjectTmproExText;

	private bool _isFocusOnCreate;

	private bool _isVisibleOnCreate = true;

	private Rect _lastRect;

	private MobileInputConfig _config;

	private RangeInt _selection;

	private int maxLine = 6;

	private const string CREATE = "CREATE_EDIT";

	private const string REMOVE = "REMOVE_EDIT";

	private const string IGNORE_CLICK = "IGNORE_CLICK";

	private const string SET_TEXT = "SET_TEXT";

	private const string SET_RECT = "SET_RECT";

	private const string SET_FOCUS = "SET_FOCUS";

	private const string ON_FOCUS = "ON_FOCUS";

	private const string ON_UNFOCUS = "ON_UNFOCUS";

	private const string SET_VISIBLE = "SET_VISIBLE";

	private const string TEXT_CHANGE = "TEXT_CHANGE";

	private const string TEXT_END_EDIT = "TEXT_END_EDIT";

	private const string ANDROID_KEY_DOWN = "ANDROID_KEY_DOWN";

	private const string RETURN_PRESSED = "RETURN_PRESSED";

	private const string READY = "READY";

	private const string INSERT_TEXT_SCROLL = "INSERT_TEXT_SCROLL";

	private const string DELETE_BUTTON = "DELETE_BUTTON";

	private const string SET_SELECTION = "SET_SELECTION";

	private const string SET_MAX_LINE = "SET_MAX_LINE";

	private const string SET_TEXT_COLOR = "SET_TEXT_COLOR";

	private const string SET_BACKGROUND_COLOR = "SET_BACKGROUND_COLOR";

	private const string SET_HINTTEXT_COLOR = "SET_HINTTEXT_COLOR";

	private const string INSERT_MENTION = "INSERT_MENTION";

	private const string DELETE_MENTION = "DELETE_MENTION";

	private const string RESET_MENTIONS = "RESET_MENTIONS";

	private List<Mention> _mentions;

	private bool inputFixIsOn = true;

	private const string iosCustomFontPath = "Assets/Main/CustomFont/LWEmojiFont-ios-sbix.ttf";

	private const string androidCustomFontPath = "Assets/Main/CustomFont/LWEmojiFont-android-CBDT.ttf";

	private bool isUnlockEmojiInput;

	private bool isUnlockAt;

	private bool isUnlockPredict;

	private int functionFlags;

	private bool isUnlockSuitableInputField;

	public int lineCount = 1;

	public float charHeight = 40f;

	private Color? textColor;

	private Color? backgroundColor;

	private Color? placeholderColor;

	private bool isUnSupportMultiple;

	private bool isUnCustomMobilColor;

	private bool isDefaultVisibleOnEnable = true;

	public OnChangeEvent onValueChanged => m_OnValueChanged;

	public InputField InputField => _inputObject;

	public TMP_InputField TmproInputField => _inputObjectTmproEx;

	public bool Visible { get; private set; }

	public string Text
	{
		get
		{
			if (!_inputObject)
			{
				return _inputObjectTmproEx.text;
			}
			return _inputObject.text;
		}
		set
		{
			if ((bool)_inputObject)
			{
				_inputObject.text = value;
			}
			else if ((bool)_inputObjectTmproEx)
			{
				_inputObjectTmproEx.text = value;
			}
			SetTextNative(value);
		}
	}

	private void Awake()
	{
		InitId();
		_inputObject = GetComponent<InputField>();
		if ((object)_inputObject != null)
		{
			_inputObjectText = _inputObject.textComponent;
		}
		else
		{
			_inputObjectTmproEx = GetComponent<TMP_InputField>();
			if (_inputObjectTmproEx == null)
			{
				Debug.LogError($"No found TMPInputField for {base.name} MobileInput");
				throw new MissingComponentException();
			}
			if (_inputObjectTmproEx.textComponent is TextMeshProUGUIEx inputObjectTmproExText)
			{
				_inputObjectTmproExText = inputObjectTmproExText;
			}
		}
		inputFixIsOn = ClientSwitch.IsOn(45);
		isUnlockEmojiInput = IsUnlockEmojiInput();
		isUnCustomMobilColor = IsUnCustomMobilColor();
		isUnSupportMultiple = IsUnSupportMultiple();
		isUnlockAt = IsUnlockAt();
		isUnlockPredict = IsUnlockPredict();
		functionFlags = GetFunctionFlags();
		MobileInput.Plugin.isUnSupportMultiple = isUnSupportMultiple;
	}

	public int GetMobilId()
	{
		return _id;
	}

	protected override void Start()
	{
		base.Start();
		isUnlockSuitableInputField = IsUnlockSuitableInputField();
		if (!isUnlockSuitableInputField)
		{
			if ((bool)_inputObject)
			{
				_inputObject.lineType = InputField.LineType.SingleLine;
			}
			else if ((bool)_inputObjectTmproEx)
			{
				_inputObjectTmproEx.lineType = TMP_InputField.LineType.SingleLine;
			}
		}
		if ((bool)_inputObjectTmproEx && _inputObjectTmproEx.UseNativeKeyboard)
		{
			_inputObjectTmproEx.lineType = TMP_InputField.LineType.MultiLineNewline;
		}
		if (isUnlockEmojiInput)
		{
			GetCustomFontDir("Assets/Main/CustomFont/LWEmojiFont-android-CBDT.ttf");
		}
		StartCoroutine(InitialzieOnNextFrame());
	}

	private void GetCustomFontDir(string path)
	{
		string rawFilePath = GameEntry.Resource.GetRawFilePath(path);
		CustomFontDir = rawFilePath;
	}

	private void OnEnable()
	{
		if (_isMobileInputCreated && isDefaultVisibleOnEnable)
		{
			SetVisible(isVisible: true);
		}
	}

	private void OnDisable()
	{
		if (_isMobileInputCreated)
		{
			SetFocus(isFocus: false);
			SetVisible(isVisible: false);
		}
	}

	protected override void OnDestroy()
	{
		RemoveNative();
		base.OnDestroy();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
	}

	private IEnumerator InitialzieOnNextFrame()
	{
		yield return null;
		PrepareNativeEdit();
		CreateNativeEdit();
		if ((bool)_inputObject)
		{
			SetTextNative(_inputObject.text);
			_inputObject.placeholder.gameObject.SetActive(value: false);
			_inputObject.enabled = false;
			_inputObjectText.enabled = false;
		}
		else if ((bool)_inputObjectTmproEx)
		{
			SetTextNative(_inputObjectTmproEx.text);
			if (!_inputObjectTmproEx.UseNativeKeyboard)
			{
				_inputObjectTmproEx.placeholder.gameObject.SetActive(value: false);
				_inputObjectTmproEx.enabled = false;
				_inputObjectTmproExText.enabled = false;
			}
		}
		if (_inputObjectTmproEx != null && _inputObjectTmproEx.UseNativeKeyboard)
		{
			_inputObjectTmproEx.OpenNativeKeyboard = delegate
			{
				SetFocus(isFocus: true);
			};
			_inputObjectTmproEx.SetNativeKeyboardSelection = delegate(RangeInt range)
			{
				SetSelection(range);
			};
			_inputObjectTmproEx.GetNativeKeyboardSelection = () => GetSelection();
		}
	}

	public void InsertTextAndScroll(string text, bool isUnescape = true)
	{
		if (isUnlockEmojiInput)
		{
			JsonObject jsonObject = new JsonObject();
			string text2 = text;
			if (isUnescape)
			{
				text2 = Regex.Unescape(text);
			}
			jsonObject["msg"] = "INSERT_TEXT_SCROLL";
			jsonObject["text"] = text2;
			Execute(jsonObject);
		}
	}

	public void DeleteButtonClick()
	{
		if (isUnlockEmojiInput)
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["msg"] = "DELETE_BUTTON";
			Execute(jsonObject);
		}
	}

	public void SetUnityInputEnabled(bool enabled)
	{
		if ((bool)_inputObject)
		{
			_inputObject.enabled = enabled;
			_inputObjectText.enabled = enabled;
		}
		else if ((bool)_inputObjectTmproEx)
		{
			_inputObjectTmproEx.enabled = enabled;
			_inputObjectTmproExText.enabled = enabled;
		}
	}

	public void SetMaxLine(int maxLine)
	{
		this.maxLine = maxLine;
		if (_isMobileInputCreated)
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["msg"] = "SET_MAX_LINE";
			jsonObject["line"] = maxLine;
			Execute(jsonObject);
		}
	}

	private void Update()
	{
		UpdateForceKeyeventForAndroid();
		if (!_isMobileInputCreated || (!(_inputObject != null) && !(_inputObjectTmproEx != null)))
		{
			return;
		}
		int touchCount = Input.touchCount;
		if (touchCount <= 0)
		{
			return;
		}
		Rect rect = default(Rect);
		if ((bool)_inputObject)
		{
			rect = _inputObjectText.rectTransform.rect;
		}
		else if ((bool)_inputObjectTmproEx)
		{
			rect = _inputObjectTmproExText.rectTransform.rect;
		}
		for (int i = 0; i < touchCount; i++)
		{
			if (!rect.Contains(Input.GetTouch(i).position))
			{
				if (!IsManualHideControl)
				{
					Hide();
				}
				break;
			}
		}
	}

	private void LateUpdate()
	{
		if (_isMobileInputCreated && (_inputObject != null || _inputObjectTmproEx != null))
		{
			if ((bool)_inputObjectText)
			{
				SetRectNative(_inputObjectText.rectTransform);
			}
			else if ((bool)_inputObjectTmproExText)
			{
				SetRectNative(_inputObjectTmproExText.rectTransform);
			}
		}
	}

	public static Rect GetScreenRectFromRectTransform(RectTransform rect)
	{
		Vector3[] array = new Vector3[4];
		rect.GetWorldCorners(array);
		float num = float.PositiveInfinity;
		float num2 = float.NegativeInfinity;
		float num3 = float.PositiveInfinity;
		float num4 = float.NegativeInfinity;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = ((rect.GetComponentInParent<Canvas>().renderMode != RenderMode.ScreenSpaceOverlay) ? ((Vector3)RectTransformUtility.WorldToScreenPoint(GameEntry.UICamera, array[i])) : array[i]);
			if (vector.x < num)
			{
				num = vector.x;
			}
			if (vector.x > num2)
			{
				num2 = vector.x;
			}
			if (vector.y < num3)
			{
				num3 = vector.y;
			}
			if (vector.y > num4)
			{
				num4 = vector.y;
			}
		}
		return new Rect(num, (float)Screen.height - num4, num2 - num, num4 - num3);
	}

	private void PrepareNativeEdit()
	{
		Text placeHolder = null;
		TextMeshProUGUIEx textMeshProUGUIEx = null;
		if ((bool)_inputObject)
		{
			placeHolder = _inputObject.placeholder.GetComponent<Text>();
		}
		if ((bool)_inputObjectTmproEx)
		{
			textMeshProUGUIEx = _inputObjectTmproEx.placeholder.GetComponent<TextMeshProUGUIEx>();
		}
		string text = (placeHolder ? placeHolder.text : textMeshProUGUIEx.text);
		Color color = (placeHolder ? placeHolder.color : textMeshProUGUIEx.color);
		if (TextMeshProUGUIEx.CheckArabicByChar(text, out var _))
		{
			NewText newText = null;
			TextMeshProUGUIEx textMeshProUGUIEx2 = null;
			if ((bool)_inputObject)
			{
				newText = _inputObject.placeholder.GetComponent<NewText>();
				textMeshProUGUIEx2 = _inputObject.placeholder.GetComponent<TextMeshProUGUIEx>();
			}
			else
			{
				newText = _inputObjectTmproEx.placeholder.GetComponent<NewText>();
				textMeshProUGUIEx2 = _inputObjectTmproEx.placeholder.GetComponent<TextMeshProUGUIEx>();
			}
			if ((bool)newText)
			{
				string input = ArabicFixer.FixText(newText.oringinalText, (string str) => NewText.PopulateWithErrors(placeHolder, str));
				_config.Placeholder = ArabicFixer.ReverseString(input);
			}
			else if ((bool)textMeshProUGUIEx2)
			{
				_config.Placeholder = textMeshProUGUIEx2.GetOriginalText();
			}
		}
		else
		{
			_config.Placeholder = text;
		}
		_config.PlaceholderColor = placeholderColor ?? color;
		if ((bool)_inputObject)
		{
			_config.CharacterLimit = _inputObject.characterLimit;
			float num = GetScreenRectFromRectTransform(_inputObjectText.rectTransform).height / _inputObjectText.rectTransform.rect.height;
			_config.FontSize = (float)_inputObjectText.fontSize * num;
			_config.TextColor = _inputObjectText.color;
			_config.TextColor = textColor ?? _inputObjectText.color;
			_config.Align = _inputObjectText.alignment.ToString();
			_config.ContentType = _inputObject.contentType.ToString();
			_config.BackgroundColor = backgroundColor ?? _inputObject.colors.normalColor;
			_config.Multiline = ((_inputObject.lineType != InputField.LineType.SingleLine) ? true : false);
			_config.KeyboardType = _inputObject.keyboardType.ToString();
			_config.InputType = _inputObject.inputType.ToString();
		}
		else
		{
			_config.CharacterLimit = _inputObjectTmproEx.characterLimit;
			float num2 = GetScreenRectFromRectTransform(_inputObjectTmproExText.rectTransform).height / _inputObjectTmproExText.rectTransform.rect.height;
			_config.FontSize = _inputObjectTmproExText.fontSize * num2;
			_config.TextColor = textColor ?? _inputObjectTmproExText.color;
			_config.Align = _inputObjectTmproExText.alignment.ToString();
			_config.ContentType = _inputObjectTmproEx.contentType.ToString();
			_config.BackgroundColor = backgroundColor ?? _inputObjectTmproEx.colors.normalColor;
			_config.Multiline = ((_inputObjectTmproEx.lineType != TMP_InputField.LineType.SingleLine) ? true : false);
			_config.KeyboardType = _inputObjectTmproEx.keyboardType.ToString();
			_config.InputType = _inputObjectTmproEx.inputType.ToString();
		}
	}

	private void OnTextChange(string text)
	{
		if (isUnSupportMultiple)
		{
			if (isUnlockSuitableInputField && OnTextChangeFromPlatform != null)
			{
				OnTextChangeFromPlatform(text);
			}
		}
		else if (isUnlockSuitableInputField && MobileInput.OnTextChangeFromPlatform != null)
		{
			MobileInput.OnTextChangeFromPlatform(text);
		}
		if ((bool)_inputObject)
		{
			if (text != _inputObject.text)
			{
				_inputObject.text = text;
			}
		}
		else if ((bool)_inputObjectTmproEx && text != _inputObjectTmproEx.text)
		{
			_inputObjectTmproEx.text = text;
		}
		m_OnValueChanged.Invoke(text);
	}

	private void OnTextEditEnd(string text)
	{
		if ((bool)_inputObject)
		{
			_inputObject.text = text;
			if (_inputObject.onEndEdit != null)
			{
				_inputObject.onEndEdit.Invoke(text);
			}
		}
		else if ((bool)_inputObjectTmproEx)
		{
			_inputObjectTmproEx.text = text;
			if (_inputObjectTmproEx.onEndEdit != null)
			{
				_inputObjectTmproEx.onEndEdit.Invoke(text);
			}
		}
		SetFocus(isFocus: false);
	}

	public override void Send(JsonObject data)
	{
		MobileInput.Plugin.StartCoroutine(PluginsMessageRoutine(data));
	}

	public override void Hide()
	{
		SetFocus(isFocus: false);
	}

	private IEnumerator PluginsMessageRoutine(JsonObject data)
	{
		yield return null;
		string text = data["msg"];
		if (text.Equals("TEXT_CHANGE"))
		{
			string text2 = data["text"];
			if (isUnlockSuitableInputField && data.ContainsKey("lineCount"))
			{
				lineCount = data["lineCount"];
			}
			int num = 0;
			if (data.ContainsKey("selectionStart"))
			{
				num = data["selectionStart"];
			}
			int num2 = 0;
			if (data.ContainsKey("selectionEnd"))
			{
				num2 = data["selectionEnd"];
			}
			_selection = new RangeInt(num, Math.Max(0, num2 - num));
			OnTextChange(text2);
		}
		else if (text.Equals("READY"))
		{
			Ready();
			if (isUnlockSuitableInputField && data.ContainsKey("charHeight"))
			{
				charHeight = data["charHeight"];
				GameEntry.Event.Fire(EventId.UMIRefreshCharHeight);
			}
		}
		else if (text.Equals("ON_FOCUS"))
		{
			OnFocusChanged?.Invoke(obj: true);
		}
		else if (text.Equals("ON_UNFOCUS"))
		{
			OnFocusChanged?.Invoke(obj: false);
		}
		else if (text.Equals("TEXT_END_EDIT"))
		{
			string text3 = data["text"];
			OnTextEditEnd(text3);
		}
		else if (text.Equals("RETURN_PRESSED"))
		{
			OnReturnPressed();
			if (OnReturnPressedEvent != null)
			{
				OnReturnPressedEvent.Invoke();
			}
		}
		else if (text.Equals("SET_SELECTION"))
		{
			int num3 = data["start"];
			int num4 = data["end"];
			_selection = new RangeInt(num3, num4 - num3);
		}
		else if (text.Equals("DELETE_MENTION"))
		{
			int index = data["index"];
			OnMentionDelete(index);
		}
	}

	private string InvariantCultureString(float value)
	{
		return value.ToString("G", CultureInfo.InvariantCulture);
	}

	private void CreateNativeEdit()
	{
		Rect rect = Rect.zero;
		if ((bool)_inputObjectText)
		{
			rect = GetScreenRectFromRectTransform(_inputObjectText.rectTransform);
		}
		else if ((bool)_inputObjectTmproExText)
		{
			rect = GetScreenRectFromRectTransform(_inputObjectTmproExText.rectTransform);
		}
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "CREATE_EDIT";
		jsonObject["x"] = InvariantCultureString(rect.x / (float)Screen.width);
		jsonObject["y"] = InvariantCultureString(rect.y / (float)Screen.height);
		jsonObject["width"] = InvariantCultureString(rect.width / (float)Screen.width);
		jsonObject["height"] = InvariantCultureString(rect.height / (float)Screen.height);
		jsonObject["character_limit"] = _config.CharacterLimit;
		jsonObject["text_color_r"] = InvariantCultureString(_config.TextColor.r);
		jsonObject["font_dir"] = CustomFontDir;
		jsonObject["text_color_g"] = InvariantCultureString(_config.TextColor.g);
		jsonObject["text_color_b"] = InvariantCultureString(_config.TextColor.b);
		jsonObject["text_color_a"] = InvariantCultureString(_config.TextColor.a);
		jsonObject["back_color_r"] = InvariantCultureString(_config.BackgroundColor.r);
		jsonObject["back_color_g"] = InvariantCultureString(_config.BackgroundColor.g);
		jsonObject["back_color_b"] = InvariantCultureString(_config.BackgroundColor.b);
		jsonObject["back_color_a"] = InvariantCultureString(_config.BackgroundColor.a);
		jsonObject["font_size"] = InvariantCultureString(_config.FontSize);
		jsonObject["content_type"] = _config.ContentType;
		jsonObject["align"] = _config.Align;
		jsonObject["with_done_button"] = false;
		jsonObject["with_clear_button"] = IsWithClearButton;
		jsonObject["placeholder"] = _config.Placeholder;
		jsonObject["placeholder_color_r"] = InvariantCultureString(_config.PlaceholderColor.r);
		jsonObject["placeholder_color_g"] = InvariantCultureString(_config.PlaceholderColor.g);
		jsonObject["placeholder_color_b"] = InvariantCultureString(_config.PlaceholderColor.b);
		jsonObject["placeholder_color_a"] = InvariantCultureString(_config.PlaceholderColor.a);
		jsonObject["multiline"] = _config.Multiline;
		jsonObject["isUnlockEmojiInput"] = isUnlockEmojiInput;
		jsonObject["isUnlockAt"] = isUnlockAt;
		jsonObject["isUnlockPredict"] = isUnlockPredict;
		jsonObject["functionFlags"] = functionFlags;
		jsonObject["font"] = CustomFont;
		jsonObject["input_type"] = _config.InputType;
		jsonObject["maxLine"] = maxLine;
		jsonObject["keyboard_type"] = _config.KeyboardType;
		jsonObject["inputFixIsOn"] = inputFixIsOn;
		switch (ReturnKey)
		{
		case ReturnKeyType.Next:
			jsonObject["return_key_type"] = "Next";
			break;
		case ReturnKeyType.Done:
			jsonObject["return_key_type"] = "Done";
			break;
		case ReturnKeyType.Search:
			jsonObject["return_key_type"] = "Search";
			break;
		case ReturnKeyType.Send:
			jsonObject["return_key_type"] = "Send";
			break;
		default:
			jsonObject["return_key_type"] = "Default";
			break;
		}
		Execute(jsonObject);
	}

	private void Ready()
	{
		_isMobileInputCreated = true;
		if (!_isVisibleOnCreate)
		{
			SetVisible(isVisible: false);
		}
		if (_isFocusOnCreate)
		{
			SetFocus(isFocus: true);
		}
		if (_mentions != null)
		{
			ResetMentions(_mentions);
			_mentions = null;
		}
	}

	public void SetIngoreFocus(bool focus)
	{
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "IGNORE_CLICK";
		jsonObject["param"] = focus;
		Execute(jsonObject);
	}

	private void SetTextNative(string text)
	{
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "SET_TEXT";
		if (string.IsNullOrEmpty(text))
		{
			text = "";
		}
		jsonObject["text"] = text;
		Execute(jsonObject);
	}

	private void RemoveNative()
	{
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "REMOVE_EDIT";
		Execute(jsonObject);
	}

	public void SetRectNative(RectTransform inputRect)
	{
		Rect screenRectFromRectTransform = GetScreenRectFromRectTransform(inputRect);
		if (!(_lastRect == screenRectFromRectTransform))
		{
			if ((bool)_inputObjectTmproEx && _inputObjectTmproEx.UseNativeKeyboard)
			{
				screenRectFromRectTransform.x += 99999f;
				screenRectFromRectTransform.y += 99999f;
			}
			_lastRect = screenRectFromRectTransform;
			JsonObject jsonObject = new JsonObject();
			jsonObject["msg"] = "SET_RECT";
			jsonObject["x"] = InvariantCultureString(screenRectFromRectTransform.x / (float)Screen.width);
			jsonObject["y"] = InvariantCultureString(screenRectFromRectTransform.y / (float)Screen.height);
			jsonObject["width"] = InvariantCultureString(screenRectFromRectTransform.width / (float)Screen.width);
			jsonObject["height"] = InvariantCultureString(screenRectFromRectTransform.height / (float)Screen.height);
			Execute(jsonObject);
		}
	}

	public void SetFocus(bool isFocus)
	{
		if (!_isMobileInputCreated)
		{
			_isFocusOnCreate = isFocus;
			return;
		}
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "SET_FOCUS";
		jsonObject["is_focus"] = isFocus;
		Execute(jsonObject);
	}

	public void SetVisible(bool isVisible)
	{
		if (isVisible)
		{
			MobileInput.CurId = _id;
		}
		if (!_isMobileInputCreated)
		{
			_isVisibleOnCreate = isVisible;
			return;
		}
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "SET_VISIBLE";
		jsonObject["is_visible"] = isVisible;
		Execute(jsonObject);
		Visible = isVisible;
	}

	public void SetMobilTextColor(float r, float g, float b, float a)
	{
		if (isUnCustomMobilColor)
		{
			textColor = new Color(r, g, b, a);
			SetColor(r, g, b, a, "SET_TEXT_COLOR");
		}
	}

	public void SetMobilBackGroundColor(float r, float g, float b, float a)
	{
		if (isUnCustomMobilColor)
		{
			backgroundColor = new Color(r, g, b, a);
			SetColor(r, g, b, a, "SET_BACKGROUND_COLOR");
		}
	}

	public void SetMobilPlaceholderColor(float r, float g, float b, float a)
	{
		if (isUnCustomMobilColor)
		{
			placeholderColor = new Color(r, g, b, a);
			SetColor(r, g, b, a, "SET_HINTTEXT_COLOR");
		}
	}

	public void SetColor(float r, float g, float b, float a, string componentType)
	{
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = componentType;
		jsonObject["color_r"] = InvariantCultureString(r);
		jsonObject["color_g"] = InvariantCultureString(g);
		jsonObject["color_b"] = InvariantCultureString(b);
		jsonObject["color_a"] = InvariantCultureString(a);
		Execute(jsonObject);
	}

	public bool IsUnlockAt()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsUnlockAt");
	}

	public bool IsUnlockPredict()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsUnlockPredict");
	}

	public int GetFunctionFlags()
	{
		return GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetFunctionFlags");
	}

	public bool IsUnlockEmojiInput()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsUnlockEmojiInput");
	}

	public bool IsUnSupportMultiple()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetMobilSupportMultiple");
	}

	public bool IsUnCustomMobilColor()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetMobilSupportChangeColor");
	}

	public bool IsUnlockSuitableInputField()
	{
		return StringUtils.VersionCompare(GameEntry.Sdk.Version, "1.0.229") >= 0;
	}

	public void SetSelection(RangeInt rangeInt)
	{
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "SET_SELECTION";
		jsonObject["start"] = rangeInt.start;
		jsonObject["end"] = rangeInt.end;
		Execute(jsonObject);
		_selection = rangeInt;
	}

	public RangeInt GetSelection()
	{
		return _selection;
	}

	public void InsertMention(string text, int offset, string colorStr, int index)
	{
		if (isUnlockAt)
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["msg"] = "INSERT_MENTION";
			jsonObject["text"] = text;
			jsonObject["offset"] = offset;
			jsonObject["index"] = index;
			if (!ColorUtility.TryParseHtmlString(colorStr, out var color))
			{
				color = Color.blue;
			}
			jsonObject["color_r"] = InvariantCultureString(color.r);
			jsonObject["color_g"] = InvariantCultureString(color.g);
			jsonObject["color_b"] = InvariantCultureString(color.b);
			jsonObject["color_a"] = InvariantCultureString(color.a);
			Execute(jsonObject);
		}
	}

	public void ResetMentions(List<Mention> list)
	{
		if (!isUnlockAt)
		{
			return;
		}
		if (!_isMobileInputCreated)
		{
			_mentions = list;
			return;
		}
		JsonObject jsonObject = new JsonObject();
		JsonArray jsonArray = new JsonArray();
		foreach (Mention item in list)
		{
			if (!ColorUtility.TryParseHtmlString(item.color, out var color))
			{
				color = Color.blue;
			}
			JsonObject jsonObject2 = new JsonObject();
			jsonObject2["text"] = item.text;
			jsonObject2["index"] = item.index;
			jsonObject2["color_r"] = InvariantCultureString(color.r);
			jsonObject2["color_g"] = InvariantCultureString(color.g);
			jsonObject2["color_b"] = InvariantCultureString(color.b);
			jsonObject2["color_a"] = InvariantCultureString(color.a);
			jsonArray.Add(jsonObject2);
		}
		jsonObject["msg"] = "RESET_MENTIONS";
		jsonObject["list"] = jsonArray;
		Execute(jsonObject);
	}

	public void SetIsDefaultVisibleOnEnable(bool isDefaultVisible)
	{
		isDefaultVisibleOnEnable = isDefaultVisible;
	}

	private void OnMentionDelete(int index)
	{
		if (isUnlockAt)
		{
			GameEntry.Event.Fire(EventId.ChatAtMentionDelete, index);
		}
	}

	private void ForceSendKeydownAndroid(string key)
	{
		JsonObject jsonObject = new JsonObject();
		jsonObject["msg"] = "ANDROID_KEY_DOWN";
		jsonObject["key"] = key;
		Execute(jsonObject);
	}

	private void UpdateForceKeyeventForAndroid()
	{
		if (!Input.anyKeyDown)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			ForceSendKeydownAndroid("backspace");
			return;
		}
		string inputString = Input.inputString;
		for (int i = 0; i < inputString.Length; i++)
		{
			if (inputString[i] == '\n')
			{
				ForceSendKeydownAndroid("enter");
			}
			else
			{
				ForceSendKeydownAndroid(Input.inputString);
			}
		}
	}
}
