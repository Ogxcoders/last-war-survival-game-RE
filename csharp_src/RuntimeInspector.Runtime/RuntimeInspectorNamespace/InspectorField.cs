using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public abstract class InspectorField : MonoBehaviour, ITooltipContent
{
	public delegate object Getter();

	public delegate void Setter(object value);

	[SerializeField]
	protected LayoutElement layoutElement;

	[SerializeField]
	protected Text variableNameText;

	[SerializeField]
	protected Image variableNameMask;

	[SerializeField]
	private MaskableGraphic visibleArea;

	private RuntimeInspector m_inspector;

	private int m_skinVersion;

	private UISkin m_skin;

	private Type m_boundVariableType;

	private object m_value;

	private int m_depth = -1;

	private bool m_isVisible = true;

	private Getter getter;

	private Setter setter;

	public RuntimeInspector Inspector
	{
		get
		{
			return m_inspector;
		}
		set
		{
			if (m_inspector != value)
			{
				m_inspector = value;
				OnInspectorChanged();
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
				m_skin = value;
				m_skinVersion = m_skin.Version;
				OnSkinChanged();
				OnDepthChanged();
			}
		}
	}

	protected Type BoundVariableType => m_boundVariableType;

	public object Value
	{
		get
		{
			return m_value;
		}
		protected set
		{
			try
			{
				setter(value);
				m_value = value;
			}
			catch
			{
			}
		}
	}

	public int Depth
	{
		get
		{
			return m_depth;
		}
		set
		{
			if (m_depth != value)
			{
				m_depth = value;
				OnDepthChanged();
			}
		}
	}

	public bool IsVisible => m_isVisible;

	public string Name
	{
		get
		{
			if ((bool)variableNameText)
			{
				return variableNameText.text;
			}
			return string.Empty;
		}
		set
		{
			if ((bool)variableNameText)
			{
				variableNameText.text = (Inspector.UseTitleCaseNaming ? value.ToTitleCase() : value);
			}
		}
	}

	public string NameRaw
	{
		get
		{
			if ((bool)variableNameText)
			{
				return variableNameText.text;
			}
			return string.Empty;
		}
		set
		{
			if ((bool)variableNameText)
			{
				variableNameText.text = value;
			}
		}
	}

	bool ITooltipContent.IsActive
	{
		get
		{
			if ((bool)this)
			{
				return base.gameObject.activeSelf;
			}
			return false;
		}
	}

	string ITooltipContent.TooltipText => NameRaw;

	public virtual bool ShouldRefresh => m_isVisible;

	protected virtual float HeightMultiplier => 1f;

	public virtual void Initialize()
	{
		if ((bool)visibleArea)
		{
			visibleArea.onCullStateChanged.AddListener(delegate(bool isCulled)
			{
				m_isVisible = !isCulled;
			});
		}
	}

	public abstract bool SupportsType(Type type);

	public virtual bool CanBindTo(Type type, MemberInfo variable)
	{
		return true;
	}

	public void BindTo(InspectorField parent, MemberInfo variable, string variableName = null)
	{
		if (variable is FieldInfo)
		{
			FieldInfo field = (FieldInfo)variable;
			if (variableName == null)
			{
				variableName = field.Name;
			}
			if (!parent.BoundVariableType.IsValueType)
			{
				BindTo(field.FieldType, variableName, () => field.GetValue(parent.Value), delegate(object value)
				{
					field.SetValue(parent.Value, value);
				}, variable);
				return;
			}
			BindTo(field.FieldType, variableName, () => field.GetValue(parent.Value), delegate(object value)
			{
				field.SetValue(parent.Value, value);
				parent.Value = parent.Value;
			}, variable);
			return;
		}
		if (variable is PropertyInfo)
		{
			PropertyInfo property = (PropertyInfo)variable;
			if (variableName == null)
			{
				variableName = property.Name;
			}
			if (!parent.BoundVariableType.IsValueType)
			{
				BindTo(property.PropertyType, variableName, () => property.GetValue(parent.Value, null), delegate(object value)
				{
					property.SetValue(parent.Value, value, null);
				}, variable);
				return;
			}
			BindTo(property.PropertyType, variableName, () => property.GetValue(parent.Value, null), delegate(object value)
			{
				property.SetValue(parent.Value, value, null);
				parent.Value = parent.Value;
			}, variable);
			return;
		}
		throw new ArgumentException("Variable can either be a field or a property");
	}

	public void BindTo(Type variableType, string variableName, Getter getter, Setter setter, MemberInfo variable = null)
	{
		m_boundVariableType = variableType;
		Name = variableName;
		this.getter = getter;
		this.setter = setter;
		OnBound(variable);
	}

	public void Unbind()
	{
		m_boundVariableType = null;
		getter = null;
		setter = null;
		OnUnbound();
		Inspector.PoolDrawer(this);
	}

	protected virtual void OnBound(MemberInfo variable)
	{
		RefreshValue();
	}

	protected virtual void OnUnbound()
	{
		m_value = null;
	}

	protected virtual void OnInspectorChanged()
	{
		if (!variableNameText)
		{
			return;
		}
		if (m_inspector.ShowTooltips)
		{
			TooltipArea tooltipArea = variableNameText.GetComponent<TooltipArea>();
			if (!tooltipArea)
			{
				tooltipArea = variableNameText.gameObject.AddComponent<TooltipArea>();
			}
			tooltipArea.Initialize(m_inspector.TooltipListener, this);
			variableNameText.raycastTarget = true;
		}
		else
		{
			TooltipArea component = variableNameText.GetComponent<TooltipArea>();
			if ((bool)component)
			{
				UnityEngine.Object.Destroy(component);
				variableNameText.raycastTarget = false;
			}
		}
	}

	protected virtual void OnSkinChanged()
	{
		if ((bool)layoutElement)
		{
			layoutElement.SetHeight((float)Skin.LineHeight * HeightMultiplier);
		}
		if ((bool)variableNameText)
		{
			variableNameText.SetSkinText(Skin);
		}
		if ((bool)variableNameMask)
		{
			variableNameMask.color = Skin.BackgroundColor;
		}
	}

	protected virtual void OnDepthChanged()
	{
		if (variableNameText != null)
		{
			variableNameText.rectTransform.sizeDelta = new Vector2(-Skin.IndentAmount * Depth, 0f);
		}
	}

	public virtual void Refresh()
	{
		RefreshValue();
	}

	private void RefreshValue()
	{
		try
		{
			m_value = getter();
		}
		catch
		{
			if (BoundVariableType.IsValueType)
			{
				m_value = Activator.CreateInstance(BoundVariableType);
			}
			else
			{
				m_value = null;
			}
		}
	}
}
