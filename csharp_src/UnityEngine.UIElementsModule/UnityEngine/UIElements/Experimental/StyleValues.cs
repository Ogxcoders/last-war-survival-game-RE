using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements.Experimental;

public struct StyleValues
{
	internal StyleValueCollection m_StyleValues;

	public float top
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PositionTop).value;
		}
		set
		{
			SetValue(StylePropertyID.PositionTop, value);
		}
	}

	public float left
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PositionLeft).value;
		}
		set
		{
			SetValue(StylePropertyID.PositionLeft, value);
		}
	}

	public float width
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.Width).value;
		}
		set
		{
			SetValue(StylePropertyID.Width, value);
		}
	}

	public float height
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.Height).value;
		}
		set
		{
			SetValue(StylePropertyID.Height, value);
		}
	}

	public float right
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PositionRight).value;
		}
		set
		{
			SetValue(StylePropertyID.PositionRight, value);
		}
	}

	public float bottom
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PositionBottom).value;
		}
		set
		{
			SetValue(StylePropertyID.PositionBottom, value);
		}
	}

	public Color color
	{
		get
		{
			return Values().GetStyleColor(StylePropertyID.Color).value;
		}
		set
		{
			SetValue(StylePropertyID.Color, value);
		}
	}

	public Color backgroundColor
	{
		get
		{
			return Values().GetStyleColor(StylePropertyID.BackgroundColor).value;
		}
		set
		{
			SetValue(StylePropertyID.BackgroundColor, value);
		}
	}

	public Color unityBackgroundImageTintColor
	{
		get
		{
			return Values().GetStyleColor(StylePropertyID.BackgroundImageTintColor).value;
		}
		set
		{
			SetValue(StylePropertyID.BackgroundImageTintColor, value);
		}
	}

	public Color borderColor
	{
		get
		{
			return Values().GetStyleColor(StylePropertyID.BorderColor).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderColor, value);
		}
	}

	public float marginLeft
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.MarginLeft).value;
		}
		set
		{
			SetValue(StylePropertyID.MarginLeft, value);
		}
	}

	public float marginTop
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.MarginTop).value;
		}
		set
		{
			SetValue(StylePropertyID.MarginTop, value);
		}
	}

	public float marginRight
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.MarginRight).value;
		}
		set
		{
			SetValue(StylePropertyID.MarginRight, value);
		}
	}

	public float marginBottom
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.MarginBottom).value;
		}
		set
		{
			SetValue(StylePropertyID.MarginBottom, value);
		}
	}

	public float paddingLeft
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PaddingLeft).value;
		}
		set
		{
			SetValue(StylePropertyID.PaddingLeft, value);
		}
	}

	public float paddingTop
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PaddingTop).value;
		}
		set
		{
			SetValue(StylePropertyID.PaddingTop, value);
		}
	}

	public float paddingRight
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PaddingRight).value;
		}
		set
		{
			SetValue(StylePropertyID.PaddingRight, value);
		}
	}

	public float paddingBottom
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.PaddingBottom).value;
		}
		set
		{
			SetValue(StylePropertyID.PaddingBottom, value);
		}
	}

	public float borderLeftWidth
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderLeftWidth).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderLeftWidth, value);
		}
	}

	public float borderRightWidth
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderRightWidth).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderRightWidth, value);
		}
	}

	public float borderTopWidth
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderTopWidth).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderTopWidth, value);
		}
	}

	public float borderBottomWidth
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderBottomWidth).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderBottomWidth, value);
		}
	}

	public float borderTopLeftRadius
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderTopLeftRadius).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderTopLeftRadius, value);
		}
	}

	public float borderTopRightRadius
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderTopRightRadius).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderTopRightRadius, value);
		}
	}

	public float borderBottomLeftRadius
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderBottomLeftRadius).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderBottomLeftRadius, value);
		}
	}

	public float borderBottomRightRadius
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.BorderBottomRightRadius).value;
		}
		set
		{
			SetValue(StylePropertyID.BorderBottomRightRadius, value);
		}
	}

	public float opacity
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.Opacity).value;
		}
		set
		{
			SetValue(StylePropertyID.Opacity, value);
		}
	}

	public float flexGrow
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.FlexGrow).value;
		}
		set
		{
			SetValue(StylePropertyID.FlexGrow, value);
		}
	}

	public float flexShrink
	{
		get
		{
			return Values().GetStyleFloat(StylePropertyID.FlexShrink).value;
		}
		set
		{
			SetValue(StylePropertyID.FlexGrow, value);
		}
	}

	internal void SetValue(StylePropertyID id, float value)
	{
		StyleValue styleValue = new StyleValue
		{
			id = id,
			number = value
		};
		Values().SetStyleValue(styleValue);
	}

	internal void SetValue(StylePropertyID id, Color value)
	{
		StyleValue styleValue = new StyleValue
		{
			id = id,
			color = value
		};
		Values().SetStyleValue(styleValue);
	}

	internal StyleValueCollection Values()
	{
		if (m_StyleValues == null)
		{
			m_StyleValues = new StyleValueCollection();
		}
		return m_StyleValues;
	}
}
