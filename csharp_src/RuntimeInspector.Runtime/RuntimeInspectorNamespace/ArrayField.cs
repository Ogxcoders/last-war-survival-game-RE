using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ArrayField : ExpandableInspectorField, IDropHandler, IEventSystemHandler
{
	[SerializeField]
	private LayoutElement sizeLayoutElement;

	[SerializeField]
	private Text sizeText;

	[SerializeField]
	private BoundInputField sizeInput;

	private bool isArray;

	private Type elementType;

	private readonly List<bool> elementsExpandedStates = new List<bool>();

	protected override int Length
	{
		get
		{
			if (isArray)
			{
				Array array = (Array)base.Value;
				if (array != null)
				{
					return array.Length;
				}
			}
			else
			{
				IList list = (IList)base.Value;
				if (list != null)
				{
					return list.Count;
				}
			}
			return 0;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		sizeInput.Initialize();
		BoundInputField boundInputField = sizeInput;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnSizeInputBeingChanged));
		BoundInputField boundInputField2 = sizeInput;
		boundInputField2.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnSizeChanged));
		sizeInput.DefaultEmptyValue = "0";
		sizeInput.CacheTextOnValueChange = false;
	}

	public override bool SupportsType(Type type)
	{
		if (!type.IsArray || type.GetArrayRank() != 1)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(List<>);
			}
			return false;
		}
		return true;
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		isArray = base.BoundVariableType.IsArray;
		elementType = (isArray ? base.BoundVariableType.GetElementType() : base.BoundVariableType.GetGenericArguments()[0]);
	}

	protected override void OnUnbound()
	{
		base.OnUnbound();
		sizeInput.Text = "0";
		elementsExpandedStates.Clear();
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		sizeInput.Skin = base.Skin;
		sizeLayoutElement.SetHeight(base.Skin.LineHeight);
		sizeText.SetSkinText(base.Skin);
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)sizeInput.transform).anchorMin = anchorMin;
	}

	protected override void OnDepthChanged()
	{
		base.OnDepthChanged();
		sizeText.rectTransform.sizeDelta = new Vector2(-base.Skin.IndentAmount * (base.Depth + 1), 0f);
	}

	protected override void ClearElements()
	{
		elementsExpandedStates.Clear();
		for (int i = 0; i < elements.Count; i++)
		{
			elementsExpandedStates.Add(elements[i] is ExpandableInspectorField && ((ExpandableInspectorField)elements[i]).IsExpanded);
		}
		base.ClearElements();
	}

	protected override void GenerateElements()
	{
		if (base.Value == null)
		{
			return;
		}
		if (isArray)
		{
			Array array = (Array)base.Value;
			for (int i = 0; i < array.Length; i++)
			{
				InspectorField inspectorField = base.Inspector.CreateDrawerForType(elementType, drawArea, base.Depth + 1);
				if (inspectorField == null)
				{
					break;
				}
				int j = i;
				inspectorField.BindTo(elementType, string.Empty, () => ((Array)base.Value).GetValue(j), delegate(object value)
				{
					Array array2 = (Array)base.Value;
					array2.SetValue(value, j);
					base.Value = array2;
				});
				if (i < elementsExpandedStates.Count && elementsExpandedStates[i] && inspectorField is ExpandableInspectorField)
				{
					((ExpandableInspectorField)inspectorField).IsExpanded = true;
				}
				inspectorField.NameRaw = (base.Inspector.ArrayIndicesStartAtOne ? (i + 1 + ":") : (i + ":"));
				elements.Add(inspectorField);
			}
		}
		else
		{
			IList list = (IList)base.Value;
			for (int num = 0; num < list.Count; num++)
			{
				InspectorField inspectorField2 = base.Inspector.CreateDrawerForType(elementType, drawArea, base.Depth + 1);
				if (inspectorField2 == null)
				{
					break;
				}
				int j2 = num;
				string variableName = (base.Inspector.ArrayIndicesStartAtOne ? (num + 1 + ":") : (num + ":"));
				inspectorField2.BindTo(elementType, variableName, () => ((IList)base.Value)[j2], delegate(object value)
				{
					IList list2 = (IList)base.Value;
					list2[j2] = value;
					base.Value = list2;
				});
				if (num < elementsExpandedStates.Count && elementsExpandedStates[num] && inspectorField2 is ExpandableInspectorField)
				{
					((ExpandableInspectorField)inspectorField2).IsExpanded = true;
				}
				elements.Add(inspectorField2);
			}
		}
		sizeInput.Text = Length.ToString(RuntimeInspectorUtils.numberFormat);
		elementsExpandedStates.Clear();
	}

	void IDropHandler.OnDrop(PointerEventData eventData)
	{
		object[] assignableObjectsFromDraggedReferenceItem = RuntimeInspectorUtils.GetAssignableObjectsFromDraggedReferenceItem(eventData, elementType);
		if (assignableObjectsFromDraggedReferenceItem == null || assignableObjectsFromDraggedReferenceItem.Length == 0)
		{
			return;
		}
		int length = Length;
		if (!OnSizeChanged(null, (length + assignableObjectsFromDraggedReferenceItem.Length).ToString(RuntimeInspectorUtils.numberFormat)))
		{
			return;
		}
		if (isArray)
		{
			Array array = (Array)base.Value;
			for (int i = 0; i < assignableObjectsFromDraggedReferenceItem.Length; i++)
			{
				array.SetValue(assignableObjectsFromDraggedReferenceItem[i], length + i);
			}
			base.Value = array;
		}
		else
		{
			IList list = (IList)base.Value;
			for (int j = 0; j < assignableObjectsFromDraggedReferenceItem.Length; j++)
			{
				list[length + j] = assignableObjectsFromDraggedReferenceItem[j];
			}
			base.Value = list;
		}
		if (!base.IsExpanded)
		{
			base.IsExpanded = true;
		}
	}

	private bool OnSizeInputBeingChanged(BoundInputField source, string input)
	{
		if (int.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out var result) && result >= 0)
		{
			return true;
		}
		return false;
	}

	private bool OnSizeChanged(BoundInputField source, string input)
	{
		if (int.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out var result) && result >= 0)
		{
			int length = Length;
			if (length != result)
			{
				if (isArray)
				{
					Array array = (Array)base.Value;
					Array array2 = Array.CreateInstance(base.BoundVariableType.GetElementType(), result);
					if (result > length)
					{
						if (array != null)
						{
							Array.ConstrainedCopy(array, 0, array2, 0, length);
						}
						for (int i = length; i < result; i++)
						{
							object templateElement = GetTemplateElement(array);
							if (templateElement != null)
							{
								array2.SetValue(templateElement, i);
							}
						}
					}
					else
					{
						Array.ConstrainedCopy(array, 0, array2, 0, result);
					}
					base.Value = array2;
				}
				else
				{
					IList list = (IList)base.Value;
					int num = result - length;
					if (num > 0)
					{
						if (list == null)
						{
							list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(base.BoundVariableType.GetGenericArguments()[0]));
						}
						for (int j = 0; j < num; j++)
						{
							list.Add(GetTemplateElement(list));
						}
					}
					else
					{
						for (int num2 = 0; num2 > num; num2--)
						{
							list.RemoveAt(list.Count - 1);
						}
					}
					base.Value = list;
				}
				base.Inspector.RefreshDelayed();
			}
			return true;
		}
		return false;
	}

	private object GetTemplateElement(object value)
	{
		Array array = null;
		IList list = null;
		if (isArray)
		{
			array = (Array)value;
		}
		else
		{
			list = (IList)value;
		}
		object obj = null;
		Type type = (isArray ? base.BoundVariableType.GetElementType() : base.BoundVariableType.GetGenericArguments()[0]);
		if (type.IsValueType)
		{
			if (isArray && array != null && array.Length > 0)
			{
				return array.GetValue(array.Length - 1);
			}
			if (!isArray && list != null && list.Count > 0)
			{
				return list[list.Count - 1];
			}
			return Activator.CreateInstance(type);
		}
		if (typeof(UnityEngine.Object).IsAssignableFrom(type))
		{
			if (isArray && array != null && array.Length > 0)
			{
				return array.GetValue(array.Length - 1);
			}
			if (!isArray && list != null && list.Count > 0)
			{
				return list[list.Count - 1];
			}
			return null;
		}
		if (type.IsArray)
		{
			return Array.CreateInstance(type, 0);
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			return Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
		}
		return type.Instantiate();
	}
}
