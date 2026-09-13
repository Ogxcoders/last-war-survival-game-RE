using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class BoundSlider : MonoBehaviour
{
	public delegate void OnValueChangedDelegate(BoundSlider source, float value);

	[SerializeField]
	private Slider slider;

	[SerializeField]
	private Image sliderBackground;

	[SerializeField]
	private Image thumb;

	private bool sliderFocused;

	private int m_skinVersion;

	private UISkin m_skin;

	public OnValueChangedDelegate OnValueChanged;

	public Slider BackingField => slider;

	public bool IsFocused => sliderFocused;

	public float Value
	{
		get
		{
			return slider.value;
		}
		set
		{
			slider.value = value;
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
				m_skin = value;
				m_skinVersion = m_skin.Version;
				sliderBackground.color = m_skin.SliderBackgroundColor;
				thumb.color = m_skin.SliderThumbColor;
			}
		}
	}

	private void Awake()
	{
		PointerEventListener pointerEventListener = slider.gameObject.AddComponent<PointerEventListener>();
		pointerEventListener.PointerDown += delegate
		{
			sliderFocused = true;
		};
		pointerEventListener.PointerUp += delegate
		{
			sliderFocused = false;
		};
		slider.onValueChanged.AddListener(SliderValueChanged);
	}

	private void OnDisable()
	{
		sliderFocused = false;
	}

	public void SetRange(float min, float max)
	{
		sliderFocused = false;
		if (min > max)
		{
			float num = min;
			min = max;
			max = num;
		}
		slider.minValue = min;
		slider.maxValue = max;
	}

	private void SliderValueChanged(float value)
	{
		if (sliderFocused && OnValueChanged != null)
		{
			OnValueChanged(this, value);
		}
	}
}
