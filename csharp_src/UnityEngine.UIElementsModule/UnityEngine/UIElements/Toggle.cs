namespace UnityEngine.UIElements;

public class Toggle : BaseField<bool>
{
	public new class UxmlFactory : UxmlFactory<Toggle, UxmlTraits>
	{
	}

	public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription>
	{
		private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
		{
			name = "text"
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			((Toggle)ve).text = m_Text.GetValueFromBag(bag, cc);
		}
	}

	public new static readonly string ussClassName = "unity-toggle";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	public static readonly string noTextVariantUssClassName = ussClassName + "--no-text";

	public static readonly string checkmarkUssClassName = ussClassName + "__checkmark";

	public static readonly string textUssClassName = ussClassName + "__text";

	private Label m_Label;

	public string text
	{
		get
		{
			return m_Label?.text;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (m_Label == null)
				{
					m_Label = new Label
					{
						pickingMode = PickingMode.Ignore
					};
					m_Label.AddToClassList(textUssClassName);
					RemoveFromClassList(noTextVariantUssClassName);
					base.visualInput.Add(m_Label);
				}
				m_Label.text = value;
			}
			else if (m_Label != null)
			{
				Remove(m_Label);
				AddToClassList(noTextVariantUssClassName);
				m_Label = null;
			}
		}
	}

	public Toggle()
		: this(null)
	{
	}

	public Toggle(string label)
		: base(label, (VisualElement)null)
	{
		AddToClassList(ussClassName);
		AddToClassList(noTextVariantUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		VisualElement visualElement = new VisualElement
		{
			name = "unity-checkmark",
			pickingMode = PickingMode.Ignore
		};
		visualElement.AddToClassList(checkmarkUssClassName);
		base.visualInput.Add(visualElement);
		base.visualInput.pickingMode = PickingMode.Position;
		text = null;
		this.AddManipulator(new Clickable(OnClickEvent));
	}

	public override void SetValueWithoutNotify(bool newValue)
	{
		if (newValue)
		{
			base.visualInput.pseudoStates |= PseudoStates.Checked;
			base.pseudoStates |= PseudoStates.Checked;
		}
		else
		{
			base.visualInput.pseudoStates &= ~PseudoStates.Checked;
			base.pseudoStates &= ~PseudoStates.Checked;
		}
		base.SetValueWithoutNotify(newValue);
	}

	private void OnClickEvent(EventBase evt)
	{
		if (evt.eventTypeId == EventBase<MouseUpEvent>.TypeId())
		{
			IMouseEvent mouseEvent = (IMouseEvent)evt;
			if (mouseEvent.button == 0)
			{
				OnClick();
			}
		}
	}

	private void OnClick()
	{
		value = !value;
	}

	protected override void ExecuteDefaultActionAtTarget(EventBase evt)
	{
		base.ExecuteDefaultActionAtTarget(evt);
		if (evt == null)
		{
			return;
		}
		KeyDownEvent obj = evt as KeyDownEvent;
		if (obj == null || obj.keyCode != KeyCode.KeypadEnter)
		{
			KeyDownEvent obj2 = evt as KeyDownEvent;
			if (obj2 == null || obj2.keyCode != KeyCode.Return)
			{
				return;
			}
		}
		OnClick();
		evt.StopPropagation();
	}
}
