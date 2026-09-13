namespace UnityEngine.UIElements;

public class TextField : TextInputBaseField<string>
{
	public new class UxmlFactory : UxmlFactory<TextField, UxmlTraits>
	{
	}

	public new class UxmlTraits : TextInputBaseField<string>.UxmlTraits
	{
		private UxmlBoolAttributeDescription m_Multiline = new UxmlBoolAttributeDescription
		{
			name = "multiline"
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			TextField textField = (TextField)ve;
			textField.multiline = m_Multiline.GetValueFromBag(bag, cc);
			base.Init(ve, bag, cc);
		}
	}

	private class TextInput : TextInputBase
	{
		private bool m_Multiline;

		private TextField parentTextField => (TextField)base.parent;

		public bool multiline
		{
			get
			{
				return m_Multiline;
			}
			set
			{
				m_Multiline = value;
				if (!value)
				{
					base.text = base.text.Replace("\n", "");
				}
			}
		}

		public override bool isPasswordField
		{
			set
			{
				base.isPasswordField = value;
				if (value)
				{
					multiline = false;
				}
			}
		}

		protected override string StringToValue(string str)
		{
			return str;
		}

		public void SelectRange(int cursorIndex, int selectionIndex)
		{
			if (base.editorEngine != null)
			{
				base.editorEngine.cursorIndex = cursorIndex;
				base.editorEngine.selectIndex = selectionIndex;
			}
		}

		internal override void SyncTextEngine()
		{
			if (parentTextField != null)
			{
				base.editorEngine.multiline = multiline;
				base.editorEngine.isPasswordField = isPasswordField;
			}
			base.SyncTextEngine();
		}

		protected override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			if (evt == null)
			{
				return;
			}
			if (evt.eventTypeId == EventBase<KeyDownEvent>.TypeId())
			{
				KeyDownEvent keyDownEvent = evt as KeyDownEvent;
				if (!parentTextField.isDelayed || (!multiline && ((keyDownEvent != null && keyDownEvent.keyCode == KeyCode.KeypadEnter) || (keyDownEvent != null && keyDownEvent.keyCode == KeyCode.Return))))
				{
					parentTextField.value = base.text;
				}
				if (multiline)
				{
					if (keyDownEvent?.character == '\t' && keyDownEvent.modifiers == EventModifiers.None)
					{
						keyDownEvent?.StopPropagation();
						keyDownEvent?.PreventDefault();
					}
					else if ((keyDownEvent?.character == '\u0003' && keyDownEvent != null && keyDownEvent.shiftKey) || (keyDownEvent?.character == '\n' && keyDownEvent != null && keyDownEvent.shiftKey))
					{
						base.parent.Focus();
					}
				}
				else if (keyDownEvent?.character == '\u0003' || keyDownEvent?.character == '\n')
				{
					base.parent.Focus();
				}
			}
			else if (evt.eventTypeId == EventBase<ExecuteCommandEvent>.TypeId())
			{
				ExecuteCommandEvent executeCommandEvent = evt as ExecuteCommandEvent;
				string commandName = executeCommandEvent.commandName;
				if (!parentTextField.isDelayed && (commandName == "Paste" || commandName == "Cut"))
				{
					parentTextField.value = base.text;
				}
			}
		}

		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (parentTextField.isDelayed && evt?.eventTypeId == EventBase<BlurEvent>.TypeId())
			{
				parentTextField.value = base.text;
			}
		}
	}

	private int m_VisualInputTabIndex;

	public new static readonly string ussClassName = "unity-text-field";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	private TextInput textInput => (TextInput)base.textInputBase;

	public bool multiline
	{
		get
		{
			return textInput.multiline;
		}
		set
		{
			textInput.multiline = value;
		}
	}

	public override string value
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
			base.text = base.rawValue;
		}
	}

	public void SelectRange(int rangeCursorIndex, int selectionIndex)
	{
		textInput.SelectRange(rangeCursorIndex, selectionIndex);
	}

	public TextField()
		: this(null)
	{
	}

	public TextField(int maxLength, bool multiline, bool isPasswordField, char maskChar)
		: this(null, maxLength, multiline, isPasswordField, maskChar)
	{
	}

	public TextField(string label)
		: this(label, -1, multiline: false, isPasswordField: false, '*')
	{
	}

	public TextField(string label, int maxLength, bool multiline, bool isPasswordField, char maskChar)
		: base(label, maxLength, maskChar, (TextInputBase)new TextInput())
	{
		AddToClassList(ussClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
		base.pickingMode = PickingMode.Ignore;
		SetValueWithoutNotify("");
		this.multiline = multiline;
		base.isPasswordField = isPasswordField;
	}

	public override void SetValueWithoutNotify(string newValue)
	{
		base.SetValueWithoutNotify(newValue);
		base.text = base.rawValue;
	}

	internal override void OnViewDataReady()
	{
		base.OnViewDataReady();
		string fullHierarchicalViewDataKey = GetFullHierarchicalViewDataKey();
		OverwriteFromViewData(this, fullHierarchicalViewDataKey);
		base.text = base.rawValue;
	}

	protected override void ExecuteDefaultActionAtTarget(EventBase evt)
	{
		base.ExecuteDefaultActionAtTarget(evt);
		if (multiline)
		{
			if ((evt?.eventTypeId == EventBase<FocusInEvent>.TypeId() && evt?.leafTarget == this) || (evt?.eventTypeId == EventBase<FocusInEvent>.TypeId() && evt?.leafTarget == base.labelElement))
			{
				m_VisualInputTabIndex = base.visualInput.tabIndex;
				base.visualInput.tabIndex = -1;
			}
			else if ((evt?.eventTypeId == EventBase<BlurEvent>.TypeId() && evt?.leafTarget == this) || (evt?.eventTypeId == EventBase<BlurEvent>.TypeId() && evt?.leafTarget == base.labelElement))
			{
				base.visualInput.tabIndex = m_VisualInputTabIndex;
			}
		}
	}
}
