using System.Collections.Generic;

namespace UnityEngine.UIElements;

public abstract class BaseField<TValueType> : BindableElement, INotifyValueChanged<TValueType>
{
	public new class UxmlTraits : BindableElement.UxmlTraits
	{
		private UxmlStringAttributeDescription m_Label = new UxmlStringAttributeDescription
		{
			name = "label"
		};

		public UxmlTraits()
		{
			base.focusIndex.defaultValue = 0;
			base.focusable.defaultValue = true;
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			((BaseField<TValueType>)ve).label = m_Label.GetValueFromBag(bag, cc);
		}
	}

	public static readonly string ussClassName = "unity-base-field";

	public static readonly string labelUssClassName = ussClassName + "__label";

	public static readonly string inputUssClassName = ussClassName + "__input";

	public static readonly string noLabelVariantUssClassName = ussClassName + "--no-label";

	public static readonly string labelDraggerVariantUssClassName = labelUssClassName + "--with-dragger";

	private VisualElement m_VisualInput;

	[SerializeField]
	private TValueType m_Value;

	internal VisualElement visualInput
	{
		get
		{
			return m_VisualInput;
		}
		set
		{
			if (m_VisualInput != null)
			{
				if (m_VisualInput.parent == this)
				{
					m_VisualInput.RemoveFromHierarchy();
				}
				m_VisualInput = null;
			}
			if (value != null)
			{
				m_VisualInput = value;
			}
			else
			{
				m_VisualInput = new VisualElement
				{
					pickingMode = PickingMode.Ignore
				};
			}
			m_VisualInput.focusable = true;
			m_VisualInput.AddToClassList(inputUssClassName);
			Add(m_VisualInput);
		}
	}

	protected TValueType rawValue
	{
		get
		{
			return m_Value;
		}
		set
		{
			m_Value = value;
		}
	}

	public virtual TValueType value
	{
		get
		{
			return m_Value;
		}
		set
		{
			if (EqualityComparer<TValueType>.Default.Equals(m_Value, value))
			{
				return;
			}
			if (base.panel != null)
			{
				using (ChangeEvent<TValueType> changeEvent = ChangeEvent<TValueType>.GetPooled(m_Value, value))
				{
					changeEvent.target = this;
					SetValueWithoutNotify(value);
					SendEvent(changeEvent);
					return;
				}
			}
			SetValueWithoutNotify(value);
		}
	}

	public Label labelElement { get; private set; }

	public string label
	{
		get
		{
			return labelElement.text;
		}
		set
		{
			if (labelElement.text != value)
			{
				labelElement.text = value;
				if (string.IsNullOrEmpty(labelElement.text))
				{
					AddToClassList(noLabelVariantUssClassName);
					labelElement.RemoveFromHierarchy();
				}
				else if (!Contains(labelElement))
				{
					Insert(0, labelElement);
					RemoveFromClassList(noLabelVariantUssClassName);
				}
			}
		}
	}

	internal BaseField(string label)
	{
		base.isCompositeRoot = true;
		base.focusable = true;
		base.tabIndex = 0;
		base.excludeFromFocusRing = true;
		base.delegatesFocus = true;
		AddToClassList(ussClassName);
		labelElement = new Label
		{
			focusable = true,
			tabIndex = -1
		};
		labelElement.AddToClassList(labelUssClassName);
		if (label != null)
		{
			this.label = label;
		}
		else
		{
			AddToClassList(noLabelVariantUssClassName);
		}
		m_VisualInput = null;
	}

	protected BaseField(string label, VisualElement visualInput)
		: this(label)
	{
		this.visualInput = visualInput;
	}

	public virtual void SetValueWithoutNotify(TValueType newValue)
	{
		m_Value = newValue;
		if (!string.IsNullOrEmpty(base.viewDataKey))
		{
			SaveViewData();
		}
		MarkDirtyRepaint();
	}

	internal override void OnViewDataReady()
	{
		base.OnViewDataReady();
		if (m_VisualInput == null)
		{
			return;
		}
		string fullHierarchicalViewDataKey = GetFullHierarchicalViewDataKey();
		TValueType val = m_Value;
		OverwriteFromViewData(this, fullHierarchicalViewDataKey);
		if (!EqualityComparer<TValueType>.Default.Equals(val, m_Value))
		{
			using (ChangeEvent<TValueType> changeEvent = ChangeEvent<TValueType>.GetPooled(val, m_Value))
			{
				changeEvent.target = this;
				SetValueWithoutNotify(m_Value);
				SendEvent(changeEvent);
			}
		}
	}
}
