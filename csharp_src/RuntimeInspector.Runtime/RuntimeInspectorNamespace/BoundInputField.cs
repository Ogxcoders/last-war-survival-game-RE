using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class BoundInputField : MonoBehaviour
{
	public delegate bool OnValueChangedDelegate(BoundInputField source, string input);

	private bool initialized;

	private bool inputValid = true;

	private bool inputAltered;

	private InputField inputField;

	private Image inputFieldBackground;

	[NonSerialized]
	public string DefaultEmptyValue = string.Empty;

	[NonSerialized]
	public bool CacheTextOnValueChange = true;

	private string recentText = string.Empty;

	private int m_skinVersion;

	private UISkin m_skin;

	public OnValueChangedDelegate OnValueChanged;

	public OnValueChangedDelegate OnValueSubmitted;

	public InputField BackingField => inputField;

	public string Text
	{
		get
		{
			return inputField.text;
		}
		set
		{
			recentText = value;
			if (!inputField.isFocused)
			{
				inputValid = true;
				inputField.text = value;
				inputFieldBackground.color = Skin.InputFieldNormalBackgroundColor;
			}
		}
	}

	public UISkin Skin
	{
		get
		{
			return m_skin;
		}
		set
		{
			if (m_skin != value || m_skinVersion != m_skin.Version)
			{
				Initialize();
				m_skin = value;
				m_skinVersion = m_skin.Version;
				inputField.textComponent.SetSkinInputFieldText(m_skin);
				inputFieldBackground.color = m_skin.InputFieldNormalBackgroundColor;
				Text text = inputField.placeholder as Text;
				if (text != null)
				{
					float a = text.color.a;
					text.SetSkinInputFieldText(m_skin);
					Color color = text.color;
					color.a = a;
					text.color = color;
				}
			}
		}
	}

	private void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (!initialized)
		{
			inputField = GetComponent<InputField>();
			inputFieldBackground = GetComponent<Image>();
			inputField.onValueChanged.AddListener(InputFieldValueChanged);
			inputField.onEndEdit.AddListener(InputFieldValueSubmitted);
			initialized = true;
		}
	}

	private void InputFieldValueChanged(string str)
	{
		if (!inputField.isFocused)
		{
			return;
		}
		inputAltered = true;
		if (str == null || str.Length == 0)
		{
			str = DefaultEmptyValue;
		}
		if (OnValueChanged != null)
		{
			inputValid = OnValueChanged(this, str);
			if (inputValid && CacheTextOnValueChange)
			{
				recentText = str;
			}
			inputFieldBackground.color = (inputValid ? Skin.InputFieldNormalBackgroundColor : Skin.InputFieldInvalidBackgroundColor);
		}
	}

	private void InputFieldValueSubmitted(string str)
	{
		inputFieldBackground.color = Skin.InputFieldNormalBackgroundColor;
		if (!inputAltered)
		{
			inputField.text = recentText;
			return;
		}
		inputAltered = false;
		if (str == null || str.Length == 0)
		{
			str = DefaultEmptyValue;
		}
		if (OnValueSubmitted != null)
		{
			if (OnValueSubmitted(this, str))
			{
				recentText = str;
			}
		}
		else if (inputValid)
		{
			recentText = str;
		}
		inputField.text = recentText;
		inputValid = true;
	}
}
