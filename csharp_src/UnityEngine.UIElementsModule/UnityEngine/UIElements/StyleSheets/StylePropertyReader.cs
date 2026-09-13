using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets;

internal class StylePropertyReader : IStylePropertyReader
{
	internal delegate int GetCursorIdFunction(StyleSheet sheet, StyleValueHandle handle);

	internal static GetCursorIdFunction getCursorIdFunc = null;

	private List<StylePropertyValue> m_Values = new List<StylePropertyValue>();

	private List<int> m_ValueCount = new List<int>();

	private StyleVariableResolver m_Resolver = new StyleVariableResolver();

	private StyleSheet m_Sheet;

	private StyleProperty[] m_Properties;

	private StylePropertyID[] m_PropertyIDs;

	private int m_CurrentValueIndex;

	private int m_CurrentPropertyIndex;

	public StyleProperty property { get; private set; }

	public StylePropertyID propertyID { get; private set; }

	public int valueCount { get; private set; }

	public int specificity { get; private set; }

	public float dpiScaling { get; private set; }

	public void SetContext(StyleSheet sheet, StyleComplexSelector selector, StyleVariableContext varContext, float dpiScaling = 1f)
	{
		m_Sheet = sheet;
		m_Properties = selector.rule.properties;
		m_PropertyIDs = StyleSheetCache.GetPropertyIDs(sheet, selector.ruleIndex);
		m_Resolver.variableContext = varContext;
		specificity = (sheet.isUnityStyleSheet ? (-1) : selector.specificity);
		this.dpiScaling = dpiScaling;
		LoadProperties();
	}

	public void SetInlineContext(StyleSheet sheet, StyleRule rule, int ruleIndex, float dpiScaling = 1f)
	{
		m_Sheet = sheet;
		m_Properties = rule.properties;
		m_PropertyIDs = StyleSheetCache.GetPropertyIDs(sheet, ruleIndex);
		specificity = int.MaxValue;
		this.dpiScaling = dpiScaling;
		LoadProperties();
	}

	public StylePropertyID MoveNextProperty()
	{
		m_CurrentPropertyIndex++;
		m_CurrentValueIndex += valueCount;
		SetCurrentProperty();
		return propertyID;
	}

	public StylePropertyValue GetValue(int index)
	{
		return m_Values[m_CurrentValueIndex + index];
	}

	public StyleValueType GetValueType(int index)
	{
		return m_Values[m_CurrentValueIndex + index].handle.valueType;
	}

	public bool IsValueType(int index, StyleValueType type)
	{
		return m_Values[m_CurrentValueIndex + index].handle.valueType == type;
	}

	public bool IsKeyword(int index, StyleValueKeyword keyword)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		return stylePropertyValue.handle.valueType == StyleValueType.Keyword && stylePropertyValue.handle.valueIndex == (int)keyword;
	}

	public string ReadAsString(int index)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		return stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
	}

	public StyleLength ReadStyleLength(int index)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		StyleLength result;
		if (stylePropertyValue.handle.valueType == StyleValueType.Keyword)
		{
			StyleValueKeyword valueIndex = (StyleValueKeyword)stylePropertyValue.handle.valueIndex;
			result = new StyleLength(valueIndex.ToStyleKeyword());
			result.specificity = specificity;
			return result;
		}
		result = new StyleLength(stylePropertyValue.sheet.ReadDimension(stylePropertyValue.handle).ToLength());
		result.specificity = specificity;
		return result;
	}

	public StyleFloat ReadStyleFloat(int index)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		StyleFloat result = new StyleFloat(stylePropertyValue.sheet.ReadFloat(stylePropertyValue.handle));
		result.specificity = specificity;
		return result;
	}

	public StyleInt ReadStyleInt(int index)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		StyleInt result = new StyleInt((int)stylePropertyValue.sheet.ReadFloat(stylePropertyValue.handle));
		result.specificity = specificity;
		return result;
	}

	public StyleColor ReadStyleColor(int index)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		Color color = Color.clear;
		if (stylePropertyValue.handle.valueType == StyleValueType.Enum)
		{
			string text = stylePropertyValue.sheet.ReadAsString(stylePropertyValue.handle);
			StyleSheetColor.TryGetColor(text.ToLower(), out color);
		}
		else
		{
			color = stylePropertyValue.sheet.ReadColor(stylePropertyValue.handle);
		}
		StyleColor result = new StyleColor(color);
		result.specificity = specificity;
		return result;
	}

	public StyleInt ReadStyleEnum<T>(int index)
	{
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		StyleInt result = new StyleInt(StyleSheetCache.GetEnumValue<T>(stylePropertyValue.sheet, stylePropertyValue.handle));
		result.specificity = specificity;
		return result;
	}

	public StyleFont ReadStyleFont(int index)
	{
		Font font = null;
		StylePropertyValue stylePropertyValue = m_Values[m_CurrentValueIndex + index];
		switch (stylePropertyValue.handle.valueType)
		{
		case StyleValueType.ResourcePath:
		{
			string text = stylePropertyValue.sheet.ReadResourcePath(stylePropertyValue.handle);
			if (!string.IsNullOrEmpty(text))
			{
				font = Panel.LoadResource(text, typeof(Font), dpiScaling) as Font;
			}
			if (font == null)
			{
				Debug.LogWarning($"Font not found for path: {text}");
			}
			break;
		}
		case StyleValueType.AssetReference:
			font = stylePropertyValue.sheet.ReadAssetReference(stylePropertyValue.handle) as Font;
			if (font == null)
			{
				Debug.LogWarning("Invalid font reference");
			}
			break;
		default:
			Debug.LogWarning("Invalid value for font " + stylePropertyValue.handle.valueType);
			break;
		}
		StyleFont result = new StyleFont(font);
		result.specificity = specificity;
		return result;
	}

	public StyleBackground ReadStyleBackground(int index)
	{
		ImageSource source = default(ImageSource);
		StylePropertyValue propertyValue = m_Values[m_CurrentValueIndex + index];
		if (propertyValue.handle.valueType == StyleValueType.Keyword)
		{
			if (propertyValue.handle.valueIndex != 6)
			{
				Debug.LogWarning("Invalid keyword for image source " + (StyleValueKeyword)propertyValue.handle.valueIndex);
			}
		}
		else if (TryGetImageSourceFromValue(propertyValue, dpiScaling, out source))
		{
		}
		if (source.texture != null)
		{
			StyleBackground result = new StyleBackground(source.texture);
			result.specificity = specificity;
			return result;
		}
		if (source.vectorImage != null)
		{
			StyleBackground result = new StyleBackground(source.vectorImage);
			result.specificity = specificity;
			return result;
		}
		return new StyleBackground
		{
			specificity = specificity
		};
	}

	public StyleCursor ReadStyleCursor(int index)
	{
		float x = 0f;
		float y = 0f;
		int defaultCursorId = 0;
		Texture2D texture = null;
		StyleValueType valueType = GetValueType(index);
		if (valueType == StyleValueType.ResourcePath || valueType == StyleValueType.AssetReference || valueType == StyleValueType.ScalableImage)
		{
			if (valueCount < 1)
			{
				Debug.LogWarning($"USS 'cursor' has invalid value at {index}.");
			}
			else
			{
				ImageSource source = default(ImageSource);
				StylePropertyValue value = GetValue(index);
				if (TryGetImageSourceFromValue(value, dpiScaling, out source))
				{
					texture = source.texture;
					if (valueCount >= 3)
					{
						StylePropertyValue value2 = GetValue(index + 1);
						StylePropertyValue value3 = GetValue(index + 2);
						if (value2.handle.valueType != StyleValueType.Float || value3.handle.valueType != StyleValueType.Float)
						{
							Debug.LogWarning("USS 'cursor' property requires two integers for the hot spot value.");
						}
						else
						{
							x = value2.sheet.ReadFloat(value2.handle);
							y = value3.sheet.ReadFloat(value3.handle);
						}
					}
				}
			}
		}
		else if (getCursorIdFunc != null)
		{
			StylePropertyValue value4 = GetValue(index);
			defaultCursorId = getCursorIdFunc(value4.sheet, value4.handle);
		}
		Cursor v = new Cursor
		{
			texture = texture,
			hotspot = new Vector2(x, y),
			defaultCursorId = defaultCursorId
		};
		StyleCursor result = new StyleCursor(v);
		result.specificity = specificity;
		return result;
	}

	private void LoadProperties()
	{
		m_CurrentPropertyIndex = 0;
		m_CurrentValueIndex = 0;
		m_Values.Clear();
		m_ValueCount.Clear();
		StyleProperty[] properties = m_Properties;
		foreach (StyleProperty styleProperty in properties)
		{
			int num = 0;
			bool flag = true;
			if (styleProperty.requireVariableResolve)
			{
				m_Resolver.Init(styleProperty, m_Sheet, styleProperty.values);
				for (int j = 0; j < styleProperty.values.Length && flag; j++)
				{
					StyleValueHandle handle = styleProperty.values[j];
					if (handle.IsVarFunction())
					{
						if (m_Resolver.ResolveVarFunction(ref j) != StyleVariableResolver.Result.Valid)
						{
							StyleValueHandle handle2 = new StyleValueHandle
							{
								valueType = StyleValueType.Keyword,
								valueIndex = 3
							};
							m_Values.Add(new StylePropertyValue
							{
								sheet = m_Sheet,
								handle = handle2
							});
							num++;
							flag = false;
						}
					}
					else
					{
						m_Resolver.AddValue(handle);
					}
				}
				if (flag)
				{
					m_Values.AddRange(m_Resolver.resolvedValues);
					num += m_Resolver.resolvedValues.Count;
				}
			}
			else
			{
				num = styleProperty.values.Length;
				for (int k = 0; k < num; k++)
				{
					StyleValueHandle styleValueHandle = styleProperty.values[k];
					m_Values.Add(new StylePropertyValue
					{
						sheet = m_Sheet,
						handle = styleProperty.values[k]
					});
				}
			}
			m_ValueCount.Add(num);
		}
		SetCurrentProperty();
	}

	private void SetCurrentProperty()
	{
		if (m_CurrentPropertyIndex < m_PropertyIDs.Length)
		{
			property = m_Properties[m_CurrentPropertyIndex];
			propertyID = m_PropertyIDs[m_CurrentPropertyIndex];
			valueCount = m_ValueCount[m_CurrentPropertyIndex];
		}
		else
		{
			property = null;
			propertyID = StylePropertyID.Unknown;
			valueCount = 0;
		}
	}

	internal static bool TryGetImageSourceFromValue(StylePropertyValue propertyValue, float dpiScaling, out ImageSource source)
	{
		source = default(ImageSource);
		switch (propertyValue.handle.valueType)
		{
		case StyleValueType.ResourcePath:
		{
			string text = propertyValue.sheet.ReadResourcePath(propertyValue.handle);
			if (!string.IsNullOrEmpty(text))
			{
				source.texture = Panel.LoadResource(text, typeof(Texture2D), dpiScaling) as Texture2D;
				if (source.texture == null)
				{
					source.vectorImage = Panel.LoadResource(text, typeof(VectorImage), dpiScaling) as VectorImage;
				}
			}
			if (source.texture == null && source.vectorImage == null)
			{
				Debug.LogWarning($"Image not found for path: {text}");
				return false;
			}
			break;
		}
		case StyleValueType.AssetReference:
		{
			Object obj = propertyValue.sheet.ReadAssetReference(propertyValue.handle);
			source.texture = obj as Texture2D;
			source.vectorImage = obj as VectorImage;
			if (source.texture == null && source.vectorImage == null)
			{
				Debug.LogWarning("Invalid image specified");
				return false;
			}
			break;
		}
		case StyleValueType.ScalableImage:
		{
			ScalableImage scalableImage = propertyValue.sheet.ReadScalableImage(propertyValue.handle);
			if (scalableImage.normalImage == null && scalableImage.highResolutionImage == null)
			{
				Debug.LogWarning("Invalid scalable image specified");
				return false;
			}
			source.texture = scalableImage.normalImage;
			if (!Mathf.Approximately(dpiScaling % 1f, 0f))
			{
				source.texture.filterMode = FilterMode.Bilinear;
			}
			break;
		}
		default:
			Debug.LogWarning("Invalid value for image texture " + propertyValue.handle.valueType);
			return false;
		}
		return true;
	}
}
