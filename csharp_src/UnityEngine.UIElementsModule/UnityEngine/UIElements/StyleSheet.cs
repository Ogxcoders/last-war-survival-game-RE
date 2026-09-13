using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

[Serializable]
public class StyleSheet : ScriptableObject
{
	[SerializeField]
	private StyleRule[] m_Rules;

	[SerializeField]
	private StyleComplexSelector[] m_ComplexSelectors;

	[SerializeField]
	internal float[] floats;

	[SerializeField]
	internal Dimension[] dimensions;

	[SerializeField]
	internal Color[] colors;

	[SerializeField]
	internal string[] strings;

	[SerializeField]
	internal Object[] assets;

	[SerializeField]
	internal ScalableImage[] scalableImages;

	[NonSerialized]
	internal Dictionary<string, StyleComplexSelector> orderedNameSelectors;

	[NonSerialized]
	internal Dictionary<string, StyleComplexSelector> orderedTypeSelectors;

	[NonSerialized]
	internal Dictionary<string, StyleComplexSelector> orderedClassSelectors;

	[NonSerialized]
	internal bool isUnityStyleSheet;

	private static string kCustomPropertyMarker = "--";

	internal StyleRule[] rules
	{
		get
		{
			return m_Rules;
		}
		set
		{
			m_Rules = value;
			SetupReferences();
		}
	}

	internal StyleComplexSelector[] complexSelectors
	{
		get
		{
			return m_ComplexSelectors;
		}
		set
		{
			m_ComplexSelectors = value;
			SetupReferences();
		}
	}

	private static bool TryCheckAccess<T>(T[] list, StyleValueType type, StyleValueHandle handle, out T value)
	{
		bool result = false;
		value = default(T);
		if (handle.valueType == type && handle.valueIndex >= 0 && handle.valueIndex < list.Length)
		{
			value = list[handle.valueIndex];
			result = true;
		}
		else
		{
			Debug.LogErrorFormat("Trying to read value of type {0} while reading a value of type {1}", type, handle.valueType);
		}
		return result;
	}

	private static T CheckAccess<T>(T[] list, StyleValueType type, StyleValueHandle handle)
	{
		T result = default(T);
		if (handle.valueType != type)
		{
			Debug.LogErrorFormat("Trying to read value of type {0} while reading a value of type {1}", type, handle.valueType);
		}
		else
		{
			if (list != null && handle.valueIndex >= 0 && handle.valueIndex < list.Length)
			{
				return list[handle.valueIndex];
			}
			Debug.LogError("Accessing invalid property");
		}
		return result;
	}

	private void OnEnable()
	{
		SetupReferences();
	}

	private void SetupReferences()
	{
		if (complexSelectors == null || rules == null)
		{
			return;
		}
		StyleRule[] array = rules;
		foreach (StyleRule styleRule in array)
		{
			StyleProperty[] properties = styleRule.properties;
			foreach (StyleProperty styleProperty in properties)
			{
				if (CustomStartsWith(styleProperty.name, kCustomPropertyMarker))
				{
					styleRule.customPropertiesCount++;
					styleProperty.isCustomProperty = true;
				}
				StyleValueHandle[] values = styleProperty.values;
				foreach (StyleValueHandle handle in values)
				{
					if (handle.IsVarFunction())
					{
						styleProperty.requireVariableResolve = true;
						break;
					}
				}
			}
		}
		int l = 0;
		for (int num = complexSelectors.Length; l < num; l++)
		{
			complexSelectors[l].CachePseudoStateMasks();
		}
		orderedClassSelectors = new Dictionary<string, StyleComplexSelector>(StringComparer.Ordinal);
		orderedNameSelectors = new Dictionary<string, StyleComplexSelector>(StringComparer.Ordinal);
		orderedTypeSelectors = new Dictionary<string, StyleComplexSelector>(StringComparer.Ordinal);
		for (int m = 0; m < complexSelectors.Length; m++)
		{
			StyleComplexSelector styleComplexSelector = complexSelectors[m];
			if (styleComplexSelector.ruleIndex < rules.Length)
			{
				styleComplexSelector.rule = rules[styleComplexSelector.ruleIndex];
			}
			styleComplexSelector.orderInStyleSheet = m;
			StyleSelector styleSelector = styleComplexSelector.selectors[styleComplexSelector.selectors.Length - 1];
			StyleSelectorPart styleSelectorPart = styleSelector.parts[0];
			string key = styleSelectorPart.value;
			Dictionary<string, StyleComplexSelector> dictionary = null;
			switch (styleSelectorPart.type)
			{
			case StyleSelectorType.Class:
				dictionary = orderedClassSelectors;
				break;
			case StyleSelectorType.ID:
				dictionary = orderedNameSelectors;
				break;
			case StyleSelectorType.Wildcard:
			case StyleSelectorType.Type:
				key = styleSelectorPart.value ?? "*";
				dictionary = orderedTypeSelectors;
				break;
			case StyleSelectorType.PseudoClass:
				key = "*";
				dictionary = orderedTypeSelectors;
				break;
			default:
				Debug.LogError($"Invalid first part type {styleSelectorPart.type}");
				break;
			}
			if (dictionary != null)
			{
				if (dictionary.TryGetValue(key, out var value))
				{
					styleComplexSelector.nextInTable = value;
				}
				dictionary[key] = styleComplexSelector;
			}
		}
	}

	internal StyleValueKeyword ReadKeyword(StyleValueHandle handle)
	{
		return (StyleValueKeyword)handle.valueIndex;
	}

	internal float ReadFloat(StyleValueHandle handle)
	{
		if (handle.valueType == StyleValueType.Dimension)
		{
			return CheckAccess(dimensions, StyleValueType.Dimension, handle).value;
		}
		return CheckAccess(floats, StyleValueType.Float, handle);
	}

	internal bool TryReadFloat(StyleValueHandle handle, out float value)
	{
		if (TryCheckAccess(floats, StyleValueType.Float, handle, out value))
		{
			return true;
		}
		Dimension value2;
		bool result = TryCheckAccess(dimensions, StyleValueType.Float, handle, out value2);
		value = value2.value;
		return result;
	}

	internal Dimension ReadDimension(StyleValueHandle handle)
	{
		if (handle.valueType == StyleValueType.Float)
		{
			float value = CheckAccess(floats, StyleValueType.Float, handle);
			return new Dimension(value, Dimension.Unit.Unitless);
		}
		return CheckAccess(dimensions, StyleValueType.Dimension, handle);
	}

	internal bool TryReadDimension(StyleValueHandle handle, out Dimension value)
	{
		if (TryCheckAccess(dimensions, StyleValueType.Dimension, handle, out value))
		{
			return true;
		}
		float value2 = 0f;
		bool result = TryCheckAccess(floats, StyleValueType.Float, handle, out value2);
		value = new Dimension(value2, Dimension.Unit.Unitless);
		return result;
	}

	internal Color ReadColor(StyleValueHandle handle)
	{
		return CheckAccess(colors, StyleValueType.Color, handle);
	}

	internal bool TryReadColor(StyleValueHandle handle, out Color value)
	{
		return TryCheckAccess(colors, StyleValueType.Color, handle, out value);
	}

	internal string ReadString(StyleValueHandle handle)
	{
		return CheckAccess(strings, StyleValueType.String, handle);
	}

	internal bool TryReadString(StyleValueHandle handle, out string value)
	{
		return TryCheckAccess(strings, StyleValueType.String, handle, out value);
	}

	internal string ReadEnum(StyleValueHandle handle)
	{
		return CheckAccess(strings, StyleValueType.Enum, handle);
	}

	internal bool TryReadEnum(StyleValueHandle handle, out string value)
	{
		return TryCheckAccess(strings, StyleValueType.Enum, handle, out value);
	}

	internal string ReadVariable(StyleValueHandle handle)
	{
		return CheckAccess(strings, StyleValueType.Variable, handle);
	}

	internal bool TryReadVariable(StyleValueHandle handle, out string value)
	{
		return TryCheckAccess(strings, StyleValueType.Variable, handle, out value);
	}

	internal string ReadResourcePath(StyleValueHandle handle)
	{
		return CheckAccess(strings, StyleValueType.ResourcePath, handle);
	}

	internal bool TryReadResourcePath(StyleValueHandle handle, out string value)
	{
		return TryCheckAccess(strings, StyleValueType.ResourcePath, handle, out value);
	}

	internal Object ReadAssetReference(StyleValueHandle handle)
	{
		return CheckAccess(assets, StyleValueType.AssetReference, handle);
	}

	internal bool TryReadAssetReference(StyleValueHandle handle, out Object value)
	{
		return TryCheckAccess(assets, StyleValueType.AssetReference, handle, out value);
	}

	internal StyleValueFunction ReadFunction(StyleValueHandle handle)
	{
		return (StyleValueFunction)handle.valueIndex;
	}

	internal string ReadFunctionName(StyleValueHandle handle)
	{
		if (handle.valueType != StyleValueType.Function)
		{
			Debug.LogErrorFormat($"Trying to read value of type {StyleValueType.Function} while reading a value of type {handle.valueType}");
			return string.Empty;
		}
		StyleValueFunction valueIndex = (StyleValueFunction)handle.valueIndex;
		return valueIndex.ToUssString();
	}

	internal ScalableImage ReadScalableImage(StyleValueHandle handle)
	{
		return CheckAccess(scalableImages, StyleValueType.ScalableImage, handle);
	}

	private static bool CustomStartsWith(string originalString, string pattern)
	{
		int length = originalString.Length;
		int length2 = pattern.Length;
		int num = 0;
		int num2 = 0;
		while (num < length && num2 < length2 && originalString[num] == pattern[num2])
		{
			num++;
			num2++;
		}
		return (num2 == length2 && length >= length2) || (num == length && length2 >= length);
	}
}
