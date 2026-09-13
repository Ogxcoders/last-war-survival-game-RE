using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ColorPicker : SkinnedWindow
{
	private static ColorPicker m_instance;

	[SerializeField]
	private Image panel;

	[SerializeField]
	private ColorWheelControl colorWheel;

	[SerializeField]
	private ColorPickerAlphaSlider alphaSlider;

	[SerializeField]
	private Text rgbaText;

	[SerializeField]
	private BoundInputField rInput;

	[SerializeField]
	private BoundInputField gInput;

	[SerializeField]
	private BoundInputField bInput;

	[SerializeField]
	private BoundInputField aInput;

	[SerializeField]
	private LayoutElement rgbaLayoutElement;

	[SerializeField]
	private LayoutElement buttonsLayoutElement;

	[SerializeField]
	private Button cancelButton;

	[SerializeField]
	private Button okButton;

	private Canvas referenceCanvas;

	private Color initialValue;

	private ColorWheelControl.OnColorChangedDelegate onColorChanged;

	private ColorWheelControl.OnColorChangedDelegate onColorConfirmed;

	public static ColorPicker Instance
	{
		get
		{
			if (!m_instance)
			{
				m_instance = UnityEngine.Object.Instantiate(Resources.Load<ColorPicker>("RuntimeInspector/ColorPicker"));
				m_instance.gameObject.SetActive(value: false);
				RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Add(m_instance.transform);
			}
			return m_instance;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		rInput.Initialize();
		gInput.Initialize();
		bInput.Initialize();
		aInput.Initialize();
		cancelButton.onClick.AddListener(Cancel);
		okButton.onClick.AddListener(delegate
		{
			try
			{
				if (onColorConfirmed != null)
				{
					onColorConfirmed(colorWheel.Color);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			Close();
		});
	}

	private void Start()
	{
		colorWheel.OnColorChanged += OnSelectedColorChanged;
		ColorPickerAlphaSlider colorPickerAlphaSlider = alphaSlider;
		colorPickerAlphaSlider.OnValueChanged = (ColorPickerAlphaSlider.OnValueChangedDelegate)Delegate.Combine(colorPickerAlphaSlider.OnValueChanged, new ColorPickerAlphaSlider.OnValueChangedDelegate(OnAlphaChanged));
		rInput.DefaultEmptyValue = "0";
		gInput.DefaultEmptyValue = "0";
		bInput.DefaultEmptyValue = "0";
		aInput.DefaultEmptyValue = "0";
		rInput.Skin = base.Skin;
		gInput.Skin = base.Skin;
		bInput.Skin = base.Skin;
		aInput.Skin = base.Skin;
		BoundInputField boundInputField = rInput;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnRGBAChanged));
		BoundInputField boundInputField2 = gInput;
		boundInputField2.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnRGBAChanged));
		BoundInputField boundInputField3 = bInput;
		boundInputField3.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField3.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnRGBAChanged));
		BoundInputField boundInputField4 = aInput;
		boundInputField4.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField4.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnRGBAChanged));
		OnSelectedColorChanged(colorWheel.Color);
	}

	public void Show(ColorWheelControl.OnColorChangedDelegate onColorChanged, ColorWheelControl.OnColorChangedDelegate onColorConfirmed, Color initialColor, Canvas referenceCanvas)
	{
		initialValue = initialColor;
		this.onColorChanged = null;
		colorWheel.PickColor(initialColor);
		alphaSlider.Color = initialColor;
		alphaSlider.Value = initialColor.a;
		this.onColorChanged = onColorChanged;
		this.onColorConfirmed = onColorConfirmed;
		if ((bool)referenceCanvas && this.referenceCanvas != referenceCanvas)
		{
			this.referenceCanvas = referenceCanvas;
			Canvas component = GetComponent<Canvas>();
			component.CopyValuesFrom(referenceCanvas);
			component.sortingOrder = Mathf.Max(1000, referenceCanvas.sortingOrder + 100);
		}
		((RectTransform)panel.transform).anchoredPosition = Vector2.zero;
		base.gameObject.SetActive(value: true);
	}

	public void Cancel()
	{
		try
		{
			if (colorWheel.Color != initialValue && onColorChanged != null)
			{
				onColorChanged(initialValue);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		Close();
	}

	public void Close()
	{
		onColorChanged = null;
		onColorConfirmed = null;
		base.gameObject.SetActive(value: false);
	}

	protected override void RefreshSkin()
	{
		panel.color = base.Skin.WindowColor;
		rgbaLayoutElement.SetHeight(base.Skin.LineHeight);
		buttonsLayoutElement.SetHeight(Mathf.Min(45f, (float)base.Skin.LineHeight * 1.5f));
		rgbaText.SetSkinText(base.Skin);
		rInput.Skin = base.Skin;
		gInput.Skin = base.Skin;
		bInput.Skin = base.Skin;
		aInput.Skin = base.Skin;
		cancelButton.SetSkinButton(base.Skin);
		okButton.SetSkinButton(base.Skin);
	}

	private void OnSelectedColorChanged(Color32 color)
	{
		rInput.Text = color.r.ToString(RuntimeInspectorUtils.numberFormat);
		gInput.Text = color.g.ToString(RuntimeInspectorUtils.numberFormat);
		bInput.Text = color.b.ToString(RuntimeInspectorUtils.numberFormat);
		aInput.Text = color.a.ToString(RuntimeInspectorUtils.numberFormat);
		alphaSlider.Color = color;
		try
		{
			if (onColorChanged != null)
			{
				onColorChanged(color);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private void OnAlphaChanged(float alpha)
	{
		aInput.Text = ((int)(alpha * 255f)).ToString(RuntimeInspectorUtils.numberFormat);
		colorWheel.Alpha = alpha;
		Color color = colorWheel.Color;
		color.a = alpha;
		try
		{
			if (onColorChanged != null)
			{
				onColorChanged(color);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private bool OnRGBAChanged(BoundInputField source, string input)
	{
		if (byte.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out var result))
		{
			Color32 color = colorWheel.Color;
			if (source == rInput)
			{
				color.r = result;
			}
			else if (source == gInput)
			{
				color.g = result;
			}
			else if (source == bInput)
			{
				color.b = result;
			}
			else
			{
				color.a = result;
				alphaSlider.Value = (float)(int)result / 255f;
			}
			alphaSlider.Color = color;
			colorWheel.PickColor(color);
			return true;
		}
		return false;
	}

	public static void DestroyInstance()
	{
		if ((bool)m_instance)
		{
			RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Remove(m_instance.transform);
			UnityEngine.Object.Destroy(m_instance);
			m_instance = null;
		}
	}
}
