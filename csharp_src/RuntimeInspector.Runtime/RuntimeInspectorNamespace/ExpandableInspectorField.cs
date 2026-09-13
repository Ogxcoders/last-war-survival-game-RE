using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public abstract class ExpandableInspectorField : InspectorField
{
	[SerializeField]
	protected RectTransform drawArea;

	[SerializeField]
	private PointerEventListener expandToggle;

	private RectTransform expandToggleTransform;

	[SerializeField]
	private LayoutGroup layoutGroup;

	[SerializeField]
	private Image expandArrow;

	protected readonly List<InspectorField> elements = new List<InspectorField>(8);

	protected readonly List<ExposedMethodField> exposedMethods = new List<ExposedMethodField>();

	private bool m_isExpanded;

	private RuntimeInspector.HeaderVisibility m_headerVisibility;

	protected virtual int Length => elements.Count;

	public override bool ShouldRefresh => true;

	public bool IsExpanded
	{
		get
		{
			return m_isExpanded;
		}
		set
		{
			m_isExpanded = value;
			drawArea.gameObject.SetActive(m_isExpanded);
			if (expandArrow != null)
			{
				expandArrow.rectTransform.localEulerAngles = (m_isExpanded ? new Vector3(0f, 0f, -90f) : Vector3.zero);
			}
			if (m_isExpanded)
			{
				Refresh();
			}
		}
	}

	public RuntimeInspector.HeaderVisibility HeaderVisibility
	{
		get
		{
			return m_headerVisibility;
		}
		set
		{
			if (m_headerVisibility == value)
			{
				return;
			}
			if (m_headerVisibility == RuntimeInspector.HeaderVisibility.Hidden)
			{
				base.Depth++;
				layoutGroup.padding.top = base.Skin.LineHeight;
				expandToggle.gameObject.SetActive(value: true);
			}
			else if (value == RuntimeInspector.HeaderVisibility.Hidden)
			{
				base.Depth--;
				layoutGroup.padding.top = 0;
				expandToggle.gameObject.SetActive(value: false);
			}
			m_headerVisibility = value;
			if (m_headerVisibility == RuntimeInspector.HeaderVisibility.Collapsible)
			{
				if (expandArrow != null)
				{
					expandArrow.gameObject.SetActive(value: true);
				}
				variableNameText.rectTransform.sizeDelta = new Vector2(0f - (base.Skin.ExpandArrowSpacing + (float)base.Skin.LineHeight * 0.5f), 0f);
			}
			else if (m_headerVisibility == RuntimeInspector.HeaderVisibility.AlwaysVisible)
			{
				if (expandArrow != null)
				{
					expandArrow.gameObject.SetActive(value: false);
				}
				variableNameText.rectTransform.sizeDelta = new Vector2(0f, 0f);
				if (!m_isExpanded)
				{
					IsExpanded = true;
				}
			}
			else if (!m_isExpanded)
			{
				IsExpanded = true;
			}
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		expandToggleTransform = (RectTransform)expandToggle.transform;
		expandToggle.PointerClick += delegate
		{
			if (m_headerVisibility == RuntimeInspector.HeaderVisibility.Collapsible)
			{
				IsExpanded = !m_isExpanded;
			}
		};
		IsExpanded = m_isExpanded;
	}

	protected override void OnUnbound()
	{
		base.OnUnbound();
		IsExpanded = false;
		HeaderVisibility = RuntimeInspector.HeaderVisibility.Collapsible;
		ClearElements();
	}

	protected override void OnInspectorChanged()
	{
		base.OnInspectorChanged();
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].Inspector = base.Inspector;
		}
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		Vector2 sizeDelta = expandToggleTransform.sizeDelta;
		sizeDelta.y = base.Skin.LineHeight;
		expandToggleTransform.sizeDelta = sizeDelta;
		if (m_headerVisibility != RuntimeInspector.HeaderVisibility.Hidden)
		{
			layoutGroup.padding.top = base.Skin.LineHeight;
			if (m_headerVisibility == RuntimeInspector.HeaderVisibility.Collapsible)
			{
				variableNameText.rectTransform.sizeDelta = new Vector2(0f - (base.Skin.ExpandArrowSpacing + (float)base.Skin.LineHeight * 0.5f), 0f);
			}
		}
		if (expandArrow != null)
		{
			expandArrow.color = base.Skin.ExpandArrowColor;
			expandArrow.rectTransform.anchoredPosition = new Vector2((float)base.Skin.LineHeight * 0.25f, 0f);
			expandArrow.rectTransform.sizeDelta = new Vector2((float)base.Skin.LineHeight * 0.5f, (float)base.Skin.LineHeight * 0.5f);
		}
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].Skin = base.Skin;
		}
		for (int j = 0; j < exposedMethods.Count; j++)
		{
			exposedMethods[j].Skin = base.Skin;
		}
	}

	protected override void OnDepthChanged()
	{
		Vector2 sizeDelta = expandToggleTransform.sizeDelta;
		sizeDelta.x = -base.Skin.IndentAmount * base.Depth;
		expandToggleTransform.sizeDelta = sizeDelta;
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].Depth = base.Depth + 1;
		}
	}

	protected void RegenerateElements()
	{
		if (elements.Count > 0 || exposedMethods.Count > 0)
		{
			ClearElements();
		}
		if (base.Depth < base.Inspector.NestLimit)
		{
			drawArea.gameObject.SetActive(value: true);
			GenerateElements();
			GenerateExposedMethodButtons();
			drawArea.gameObject.SetActive(m_isExpanded);
		}
	}

	protected abstract void GenerateElements();

	private void GenerateExposedMethodButtons()
	{
		if (base.Inspector.ShowRemoveComponentButton && typeof(Component).IsAssignableFrom(base.BoundVariableType) && !typeof(Transform).IsAssignableFrom(base.BoundVariableType))
		{
			CreateExposedMethodButton(GameObjectField.removeComponentMethod, () => this, delegate
			{
			});
		}
		ExposedMethod[] array = base.BoundVariableType.GetExposedMethods();
		if (array == null)
		{
			return;
		}
		bool flag = base.Value != null && !base.Value.Equals(null);
		for (int num = 0; num < array.Length; num++)
		{
			ExposedMethod method = array[num];
			if ((flag && method.VisibleWhenInitialized) || (!flag && method.VisibleWhenUninitialized))
			{
				CreateExposedMethodButton(method, () => base.Value, delegate(object value)
				{
					base.Value = value;
				});
			}
		}
	}

	protected virtual void ClearElements()
	{
		for (int i = 0; i < elements.Count; i++)
		{
			elements[i].Unbind();
		}
		for (int j = 0; j < exposedMethods.Count; j++)
		{
			exposedMethods[j].Unbind();
		}
		elements.Clear();
		exposedMethods.Clear();
	}

	public override void Refresh()
	{
		base.Refresh();
		if (!m_isExpanded)
		{
			return;
		}
		if (Length != elements.Count)
		{
			RegenerateElements();
		}
		for (int i = 0; i < elements.Count; i++)
		{
			if (elements[i].ShouldRefresh)
			{
				elements[i].Refresh();
			}
		}
	}

	public InspectorField CreateDrawerForComponent(Component component, string variableName = null)
	{
		InspectorField inspectorField = base.Inspector.CreateDrawerForType(component.GetType(), drawArea, base.Depth + 1, drawObjectsAsFields: false);
		if (inspectorField != null)
		{
			if (variableName == null)
			{
				variableName = component.GetType().Name + " component";
			}
			inspectorField.BindTo(component.GetType(), string.Empty, () => component, delegate
			{
			});
			inspectorField.NameRaw = variableName;
			elements.Add(inspectorField);
		}
		return inspectorField;
	}

	public InspectorField CreateDrawerForVariable(MemberInfo variable, string variableName = null)
	{
		Type type = ((variable is FieldInfo) ? ((FieldInfo)variable).FieldType : ((PropertyInfo)variable).PropertyType);
		InspectorField inspectorField = base.Inspector.CreateDrawerForType(type, drawArea, base.Depth + 1, drawObjectsAsFields: true, variable);
		if (inspectorField != null)
		{
			inspectorField.BindTo(this, variable, (variableName == null) ? null : string.Empty);
			if (variableName != null)
			{
				inspectorField.NameRaw = variableName;
			}
			elements.Add(inspectorField);
		}
		return inspectorField;
	}

	public InspectorField CreateDrawer(Type variableType, string variableName, Getter getter, Setter setter, bool drawObjectsAsFields = true)
	{
		InspectorField inspectorField = base.Inspector.CreateDrawerForType(variableType, drawArea, base.Depth + 1, drawObjectsAsFields);
		if (inspectorField != null)
		{
			inspectorField.BindTo(variableType, (variableName == null) ? null : string.Empty, getter, setter);
			if (variableName != null)
			{
				inspectorField.NameRaw = variableName;
			}
			elements.Add(inspectorField);
		}
		return inspectorField;
	}

	public ExposedMethodField CreateExposedMethodButton(ExposedMethod method, Getter getter, Setter setter)
	{
		ExposedMethodField exposedMethodField = (ExposedMethodField)base.Inspector.CreateDrawerForType(typeof(ExposedMethod), drawArea, base.Depth + 1, drawObjectsAsFields: false);
		if (exposedMethodField != null)
		{
			exposedMethodField.BindTo(typeof(ExposedMethod), string.Empty, getter, setter);
			exposedMethodField.SetBoundMethod(method);
			exposedMethods.Add(exposedMethodField);
		}
		return exposedMethodField;
	}
}
