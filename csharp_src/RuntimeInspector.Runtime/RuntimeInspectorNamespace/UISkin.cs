using UnityEngine;

namespace RuntimeInspectorNamespace;

[CreateAssetMenu(fileName = "UI Skin", menuName = "yasirkula/RuntimeInspector/UI Skin", order = 111)]
public class UISkin : ScriptableObject
{
	private int m_version;

	[SerializeField]
	private Font m_font;

	[SerializeField]
	private int m_fontSize = 12;

	[SerializeField]
	private int m_lineHeight = 30;

	[SerializeField]
	private int m_indentAmount = 12;

	[SerializeField]
	private float m_labelWidthPercentage = 0.4f;

	[SerializeField]
	private float m_expandArrowSpacing = 10f;

	[SerializeField]
	private Color m_windowColor = Color.grey;

	[SerializeField]
	private Color m_backgroundColor = Color.grey;

	[SerializeField]
	private Color m_textColor = Color.black;

	[SerializeField]
	private Color m_scrollbarColor = Color.black;

	[SerializeField]
	private Color m_expandArrowColor = Color.black;

	[SerializeField]
	private Color m_inputFieldNormalBackgroundColor = Color.white;

	[SerializeField]
	private Color m_inputFieldInvalidBackgroundColor = Color.red;

	[SerializeField]
	private Color m_inputFieldTextColor = Color.black;

	[SerializeField]
	private Color m_toggleCheckmarkColor = Color.black;

	[SerializeField]
	private Color m_sliderBackgroundColor = Color.white;

	[SerializeField]
	private Color m_sliderThumbColor = Color.black;

	[SerializeField]
	private Color m_buttonBackgroundColor = Color.white;

	[SerializeField]
	private Color m_buttonTextColor = Color.black;

	[SerializeField]
	private Color m_selectedItemBackgroundColor = Color.blue;

	[SerializeField]
	private Color m_selectedItemTextColor = Color.black;

	public int Version => m_version;

	public Font Font
	{
		get
		{
			return m_font;
		}
		set
		{
			if (m_font != value)
			{
				m_font = value;
				m_version++;
			}
		}
	}

	public int FontSize
	{
		get
		{
			return m_fontSize;
		}
		set
		{
			if (m_fontSize != value)
			{
				m_fontSize = value;
				m_version++;
			}
		}
	}

	public int LineHeight
	{
		get
		{
			return m_lineHeight;
		}
		set
		{
			if (m_lineHeight != value)
			{
				m_lineHeight = value;
				m_version++;
			}
		}
	}

	public int IndentAmount
	{
		get
		{
			return m_indentAmount;
		}
		set
		{
			if (m_indentAmount != value)
			{
				m_indentAmount = value;
				m_version++;
			}
		}
	}

	public float LabelWidthPercentage
	{
		get
		{
			return m_labelWidthPercentage;
		}
		set
		{
			if (m_labelWidthPercentage != value)
			{
				m_labelWidthPercentage = value;
				m_version++;
			}
		}
	}

	public float ExpandArrowSpacing
	{
		get
		{
			return m_expandArrowSpacing;
		}
		set
		{
			if (m_expandArrowSpacing != value)
			{
				m_expandArrowSpacing = value;
				m_version++;
			}
		}
	}

	public Color WindowColor
	{
		get
		{
			return m_windowColor;
		}
		set
		{
			if (m_windowColor != value)
			{
				m_windowColor = value;
				m_version++;
			}
		}
	}

	public Color BackgroundColor
	{
		get
		{
			return m_backgroundColor;
		}
		set
		{
			if (m_backgroundColor != value)
			{
				m_backgroundColor = value;
				m_version++;
			}
		}
	}

	public Color TextColor
	{
		get
		{
			return m_textColor;
		}
		set
		{
			if (m_textColor != value)
			{
				m_textColor = value;
				m_version++;
			}
		}
	}

	public Color ScrollbarColor
	{
		get
		{
			return m_scrollbarColor;
		}
		set
		{
			if (m_scrollbarColor != value)
			{
				m_scrollbarColor = value;
				m_version++;
			}
		}
	}

	public Color ExpandArrowColor
	{
		get
		{
			return m_expandArrowColor;
		}
		set
		{
			if (m_expandArrowColor != value)
			{
				m_expandArrowColor = value;
				m_version++;
			}
		}
	}

	public Color InputFieldNormalBackgroundColor
	{
		get
		{
			return m_inputFieldNormalBackgroundColor;
		}
		set
		{
			if (m_inputFieldNormalBackgroundColor != value)
			{
				m_inputFieldNormalBackgroundColor = value;
				m_version++;
			}
		}
	}

	public Color InputFieldInvalidBackgroundColor
	{
		get
		{
			return m_inputFieldInvalidBackgroundColor;
		}
		set
		{
			if (m_inputFieldInvalidBackgroundColor != value)
			{
				m_inputFieldInvalidBackgroundColor = value;
				m_version++;
			}
		}
	}

	public Color InputFieldTextColor
	{
		get
		{
			return m_inputFieldTextColor;
		}
		set
		{
			if (m_inputFieldTextColor != value)
			{
				m_inputFieldTextColor = value;
				m_version++;
			}
		}
	}

	public Color ToggleCheckmarkColor
	{
		get
		{
			return m_toggleCheckmarkColor;
		}
		set
		{
			if (m_toggleCheckmarkColor != value)
			{
				m_toggleCheckmarkColor = value;
				m_version++;
			}
		}
	}

	public Color SliderBackgroundColor
	{
		get
		{
			return m_sliderBackgroundColor;
		}
		set
		{
			if (m_sliderBackgroundColor != value)
			{
				m_sliderBackgroundColor = value;
				m_version++;
			}
		}
	}

	public Color SliderThumbColor
	{
		get
		{
			return m_sliderThumbColor;
		}
		set
		{
			if (m_sliderThumbColor != value)
			{
				m_sliderThumbColor = value;
				m_version++;
			}
		}
	}

	public Color ButtonBackgroundColor
	{
		get
		{
			return m_buttonBackgroundColor;
		}
		set
		{
			if (m_buttonBackgroundColor != value)
			{
				m_buttonBackgroundColor = value;
				m_version++;
			}
		}
	}

	public Color ButtonTextColor
	{
		get
		{
			return m_buttonTextColor;
		}
		set
		{
			if (m_buttonTextColor != value)
			{
				m_buttonTextColor = value;
				m_version++;
			}
		}
	}

	public Color SelectedItemBackgroundColor
	{
		get
		{
			return m_selectedItemBackgroundColor;
		}
		set
		{
			if (m_selectedItemBackgroundColor != value)
			{
				m_selectedItemBackgroundColor = value;
				m_version++;
			}
		}
	}

	public Color SelectedItemTextColor
	{
		get
		{
			return m_selectedItemTextColor;
		}
		set
		{
			if (m_selectedItemTextColor != value)
			{
				m_selectedItemTextColor = value;
				m_version++;
			}
		}
	}

	[ContextMenu("Refresh UI")]
	private void Invalidate()
	{
		m_version = Random.Range(-1073741824, 1073741823);
	}
}
