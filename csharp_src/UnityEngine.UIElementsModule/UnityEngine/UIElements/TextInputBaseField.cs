using System;

namespace UnityEngine.UIElements;

public abstract class TextInputBaseField<TValueType> : BaseField<TValueType>
{
	public new class UxmlTraits : BaseFieldTraits<string, UxmlStringAttributeDescription>
	{
		private UxmlIntAttributeDescription m_MaxLength = new UxmlIntAttributeDescription
		{
			name = "max-length",
			obsoleteNames = new string[1] { "maxLength" },
			defaultValue = -1
		};

		private UxmlBoolAttributeDescription m_Password = new UxmlBoolAttributeDescription
		{
			name = "password"
		};

		private UxmlStringAttributeDescription m_MaskCharacter = new UxmlStringAttributeDescription
		{
			name = "mask-character",
			obsoleteNames = new string[1] { "maskCharacter" },
			defaultValue = '*'.ToString()
		};

		private UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
		{
			name = "text"
		};

		private UxmlBoolAttributeDescription m_IsReadOnly = new UxmlBoolAttributeDescription
		{
			name = "readonly"
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)ve;
			textInputBaseField.maxLength = m_MaxLength.GetValueFromBag(bag, cc);
			textInputBaseField.isPasswordField = m_Password.GetValueFromBag(bag, cc);
			textInputBaseField.isReadOnly = m_IsReadOnly.GetValueFromBag(bag, cc);
			string valueFromBag = m_MaskCharacter.GetValueFromBag(bag, cc);
			if (valueFromBag != null && valueFromBag.Length > 0)
			{
				textInputBaseField.maskChar = valueFromBag[0];
			}
			textInputBaseField.text = m_Text.GetValueFromBag(bag, cc);
		}
	}

	protected abstract class TextInputBase : VisualElement, ITextInputField, IEventHandler, ITextElement
	{
		private string m_OriginalText;

		private bool m_TouchScreenTextFieldInitialized;

		private IVisualElementScheduledItem m_HardwareKeyboardPoller = null;

		private Color m_SelectionColor = Color.clear;

		private Color m_CursorColor = Color.grey;

		private TextHandle m_TextHandle = TextHandle.New();

		private string m_Text;

		public int cursorIndex => editorEngine.cursorIndex;

		public int selectIndex => editorEngine.selectIndex;

		bool ITextInputField.isReadOnly => isReadOnly;

		public bool isReadOnly { get; set; }

		public int maxLength { get; set; }

		public char maskChar { get; set; }

		public virtual bool isPasswordField { get; set; }

		public bool doubleClickSelectsWord { get; set; }

		public bool tripleClickSelectsLine { get; set; }

		internal bool isDelayed { get; set; }

		internal bool isDragging { get; set; }

		private bool touchScreenTextField => TouchScreenKeyboard.isSupported && !TouchScreenKeyboard.isInPlaceEditingAllowed;

		private bool touchScreenTextFieldChanged => m_TouchScreenTextFieldInitialized != touchScreenTextField;

		public Color selectionColor => m_SelectionColor;

		public Color cursorColor => m_CursorColor;

		internal bool hasFocus => base.elementPanel != null && base.elementPanel.focusController.GetLeafFocusedElement() == this;

		internal TextEditorEventHandler editorEventHandler { get; private set; }

		internal TextEditorEngine editorEngine { get; private set; }

		public string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (!(m_Text == value))
				{
					m_Text = value;
					editorEngine.text = value;
					IncrementVersion(VersionChangeType.Layout | VersionChangeType.Repaint);
				}
			}
		}

		bool ITextInputField.hasFocus => hasFocus;

		TextEditorEngine ITextInputField.editorEngine => editorEngine;

		bool ITextInputField.isDelayed => isDelayed;

		private void SaveValueAndText()
		{
			m_OriginalText = text;
		}

		private void RestoreValueAndText()
		{
			text = m_OriginalText;
		}

		public void SelectAll()
		{
			editorEngine?.SelectAll();
		}

		internal void SelectNone()
		{
			editorEngine?.SelectNone();
		}

		private void UpdateText(string value)
		{
			if (text != value)
			{
				using (InputEvent inputEvent = InputEvent.GetPooled(text, value))
				{
					inputEvent.target = base.parent;
					text = value;
					base.parent?.SendEvent(inputEvent);
				}
			}
		}

		protected virtual TValueType StringToValue(string str)
		{
			throw new NotSupportedException();
		}

		internal void UpdateValueFromText()
		{
			TValueType value = StringToValue(text);
			TextInputBaseField<TValueType> textInputBaseField = (TextInputBaseField<TValueType>)base.parent;
			textInputBaseField.value = value;
		}

		internal TextInputBase()
		{
			isReadOnly = false;
			base.focusable = true;
			AddToClassList(TextInputBaseField<TValueType>.inputUssClassName);
			m_Text = string.Empty;
			base.name = TextInputBaseField<string>.textInputUssName;
			base.requireMeasureFunction = true;
			editorEngine = new TextEditorEngine(OnDetectFocusChange, OnCursorIndexChange);
			editorEngine.style.richText = false;
			InitTextEditorEventHandler();
			editorEngine.style = new GUIStyle(editorEngine.style);
			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(OnGenerateVisualContent));
		}

		private void InitTextEditorEventHandler()
		{
			m_TouchScreenTextFieldInitialized = touchScreenTextField;
			if (m_TouchScreenTextFieldInitialized)
			{
				editorEventHandler = new TouchScreenTextEditorEventHandler(editorEngine, this);
				return;
			}
			doubleClickSelectsWord = true;
			tripleClickSelectsLine = true;
			editorEventHandler = new KeyboardTextEditorEventHandler(editorEngine, this);
		}

		private DropdownMenuAction.Status CutCopyActionStatus(DropdownMenuAction a)
		{
			return (editorEngine.hasSelection && !isPasswordField) ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled;
		}

		private DropdownMenuAction.Status PasteActionStatus(DropdownMenuAction a)
		{
			return editorEngine.CanPaste() ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled;
		}

		private void ProcessMenuCommand(string command)
		{
			using ExecuteCommandEvent executeCommandEvent = CommandEventBase<ExecuteCommandEvent>.GetPooled(command);
			executeCommandEvent.target = this;
			SendEvent(executeCommandEvent);
		}

		private void Cut(DropdownMenuAction a)
		{
			ProcessMenuCommand("Cut");
		}

		private void Copy(DropdownMenuAction a)
		{
			ProcessMenuCommand("Copy");
		}

		private void Paste(DropdownMenuAction a)
		{
			ProcessMenuCommand("Paste");
		}

		private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
		{
			Color value = Color.clear;
			Color value2 = Color.clear;
			ICustomStyle customStyle = e.customStyle;
			if (customStyle.TryGetValue(TextInputBaseField<TValueType>.s_SelectionColorProperty, out value))
			{
				m_SelectionColor = value;
			}
			if (customStyle.TryGetValue(TextInputBaseField<TValueType>.s_CursorColorProperty, out value2))
			{
				m_CursorColor = value2;
			}
			SyncGUIStyle(this, editorEngine.style);
		}

		private void OnAttachToPanel(AttachToPanelEvent e)
		{
			m_TextHandle.useLegacy = e.destinationPanel.contextType == ContextType.Editor;
		}

		internal virtual void SyncTextEngine()
		{
			editorEngine.text = CullString(text);
			editorEngine.SaveBackup();
			editorEngine.position = base.layout;
			editorEngine.DetectFocusChange();
		}

		internal string CullString(string s)
		{
			if (maxLength >= 0 && s != null && s.Length > maxLength)
			{
				return s.Substring(0, maxLength);
			}
			return s;
		}

		internal void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			string newText = text;
			if (isPasswordField)
			{
				newText = "".PadRight(text.Length, maskChar);
			}
			if (m_TouchScreenTextFieldInitialized)
			{
				if (editorEventHandler is TouchScreenTextEditorEventHandler)
				{
					mgc.Text(MeshGenerationContextUtils.TextParams.MakeStyleBased(this, newText), m_TextHandle, base.scaledPixelsPerPoint);
				}
			}
			else if (!hasFocus)
			{
				mgc.Text(MeshGenerationContextUtils.TextParams.MakeStyleBased(this, newText), m_TextHandle, base.scaledPixelsPerPoint);
			}
			else
			{
				DrawWithTextSelectionAndCursor(mgc, newText, base.scaledPixelsPerPoint);
			}
		}

		internal void DrawWithTextSelectionAndCursor(MeshGenerationContext mgc, string newText, float pixelsPerPoint)
		{
			Color playmodeTintColor = ((base.panel.contextType == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white);
			if (!(editorEventHandler is KeyboardTextEditorEventHandler keyboardTextEditorEventHandler))
			{
				return;
			}
			keyboardTextEditorEventHandler.PreDrawCursor(newText);
			int num = editorEngine.cursorIndex;
			int num2 = editorEngine.selectIndex;
			Rect localPosition = editorEngine.localPosition;
			Vector2 vector = editorEngine.scrollOffset;
			float scaling = TextHandle.ComputeTextScaling(base.worldTransform, pixelsPerPoint);
			MeshGenerationContextUtils.TextParams parms = MeshGenerationContextUtils.TextParams.MakeStyleBased(this, text);
			parms.text = " ";
			parms.wordWrapWidth = 0f;
			parms.wordWrap = false;
			float num3 = m_TextHandle.ComputeTextHeight(parms, scaling);
			float wordWrapWidth = 0f;
			if (editorEngine.multiline && base.resolvedStyle.whiteSpace == WhiteSpace.Normal)
			{
				wordWrapWidth = base.contentRect.width;
				vector = Vector2.zero;
			}
			GUIUtility.compositionCursorPos = editorEngine.graphicalCursorPos - vector + new Vector2(localPosition.x, localPosition.y + num3);
			Color color = cursorColor;
			int num4 = (string.IsNullOrEmpty(GUIUtility.compositionString) ? num2 : (num + GUIUtility.compositionString.Length));
			if (num != num4 && !isDragging)
			{
				int num5 = ((num < num4) ? num : num4);
				int num6 = ((num > num4) ? num : num4);
				CursorPositionStylePainterParameters parms2 = CursorPositionStylePainterParameters.GetDefault(this, text);
				parms2.text = editorEngine.text;
				parms2.wordWrapWidth = wordWrapWidth;
				parms2.cursorIndex = num5;
				Vector2 cursorPosition = m_TextHandle.GetCursorPosition(parms2, scaling);
				parms2.cursorIndex = num6;
				Vector2 cursorPosition2 = m_TextHandle.GetCursorPosition(parms2, scaling);
				cursorPosition -= vector;
				cursorPosition2 -= vector;
				if (Mathf.Approximately(cursorPosition.y, cursorPosition2.y))
				{
					mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
					{
						rect = new Rect(cursorPosition.x, cursorPosition.y, cursorPosition2.x - cursorPosition.x, num3),
						color = selectionColor,
						playmodeTintColor = playmodeTintColor
					});
				}
				else
				{
					mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
					{
						rect = new Rect(cursorPosition.x, cursorPosition.y, base.contentRect.xMax - cursorPosition.x, num3),
						color = selectionColor,
						playmodeTintColor = playmodeTintColor
					});
					float num7 = cursorPosition2.y - cursorPosition.y - num3;
					if (num7 > 0f)
					{
						mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
						{
							rect = new Rect(base.contentRect.xMin, cursorPosition.y + num3, base.contentRect.width, num7),
							color = selectionColor,
							playmodeTintColor = playmodeTintColor
						});
					}
					if (cursorPosition2.x != base.contentRect.x)
					{
						mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
						{
							rect = new Rect(base.contentRect.xMin, cursorPosition2.y, cursorPosition2.x, num3),
							color = selectionColor,
							playmodeTintColor = playmodeTintColor
						});
					}
				}
			}
			if (!string.IsNullOrEmpty(editorEngine.text) && base.contentRect.width > 0f && base.contentRect.height > 0f)
			{
				parms = MeshGenerationContextUtils.TextParams.MakeStyleBased(this, text);
				parms.rect = new Rect(base.contentRect.x - vector.x, base.contentRect.y - vector.y, base.contentRect.width + vector.x, base.contentRect.height + vector.y);
				parms.text = editorEngine.text;
				mgc.Text(parms, m_TextHandle, base.scaledPixelsPerPoint);
			}
			if (!isReadOnly && !isDragging)
			{
				if (num == num4 && base.computedStyle.unityFont.value != null)
				{
					CursorPositionStylePainterParameters parms2 = CursorPositionStylePainterParameters.GetDefault(this, text);
					parms2.text = editorEngine.text;
					parms2.wordWrapWidth = wordWrapWidth;
					parms2.cursorIndex = num;
					Vector2 cursorPosition3 = m_TextHandle.GetCursorPosition(parms2, scaling);
					cursorPosition3 -= vector;
					mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
					{
						rect = new Rect(cursorPosition3.x, cursorPosition3.y, 1f, num3),
						color = color,
						playmodeTintColor = playmodeTintColor
					});
				}
				if (editorEngine.altCursorPosition != -1)
				{
					CursorPositionStylePainterParameters parms2 = CursorPositionStylePainterParameters.GetDefault(this, text);
					parms2.text = editorEngine.text.Substring(0, editorEngine.altCursorPosition);
					parms2.wordWrapWidth = wordWrapWidth;
					parms2.cursorIndex = editorEngine.altCursorPosition;
					Vector2 cursorPosition4 = m_TextHandle.GetCursorPosition(parms2, scaling);
					cursorPosition4 -= vector;
					mgc.Rectangle(new MeshGenerationContextUtils.RectangleParams
					{
						rect = new Rect(cursorPosition4.x, cursorPosition4.y, 1f, num3),
						color = color,
						playmodeTintColor = playmodeTintColor
					});
				}
			}
			keyboardTextEditorEventHandler.PostDrawCursor();
		}

		internal virtual bool AcceptCharacter(char c)
		{
			return !isReadOnly;
		}

		protected virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			if (evt?.target is TextInputBase)
			{
				if (!isReadOnly)
				{
					evt.menu.AppendAction("Cut", Cut, CutCopyActionStatus);
				}
				evt.menu.AppendAction("Copy", Copy, CutCopyActionStatus);
				if (!isReadOnly)
				{
					evt.menu.AppendAction("Paste", Paste, PasteActionStatus);
				}
			}
		}

		private void OnDetectFocusChange()
		{
			if (editorEngine.m_HasFocus && !hasFocus)
			{
				editorEngine.OnFocus();
			}
			if (!editorEngine.m_HasFocus && hasFocus)
			{
				editorEngine.OnLostFocus();
			}
		}

		private void OnCursorIndexChange()
		{
			IncrementVersion(VersionChangeType.Repaint);
		}

		protected internal override Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode)
		{
			string text = m_Text;
			if (string.IsNullOrEmpty(text))
			{
				text = " ";
			}
			return TextElement.MeasureVisualElementTextSize(this, text, desiredWidth, widthMode, desiredHeight, heightMode, m_TextHandle);
		}

		protected override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			if (base.elementPanel != null && base.elementPanel.contextualMenuManager != null)
			{
				base.elementPanel.contextualMenuManager.DisplayMenuIfEventMatches(evt, this);
			}
			if (evt?.eventTypeId == EventBase<ContextualMenuPopulateEvent>.TypeId())
			{
				ContextualMenuPopulateEvent contextualMenuPopulateEvent = evt as ContextualMenuPopulateEvent;
				int count = contextualMenuPopulateEvent.menu.MenuItems().Count;
				BuildContextualMenu(contextualMenuPopulateEvent);
				if (count > 0 && contextualMenuPopulateEvent.menu.MenuItems().Count > count)
				{
					contextualMenuPopulateEvent.menu.InsertSeparator(null, count);
				}
			}
			else if (evt.eventTypeId == EventBase<FocusInEvent>.TypeId())
			{
				SaveValueAndText();
				if (touchScreenTextFieldChanged)
				{
					InitTextEditorEventHandler();
				}
				if (m_HardwareKeyboardPoller == null)
				{
					m_HardwareKeyboardPoller = base.schedule.Execute((Action)delegate
					{
						if (touchScreenTextFieldChanged)
						{
							InitTextEditorEventHandler();
							Blur();
						}
					}).Every(250L);
				}
				else
				{
					m_HardwareKeyboardPoller.Resume();
				}
			}
			else if (evt.eventTypeId == EventBase<FocusOutEvent>.TypeId())
			{
				if (m_HardwareKeyboardPoller != null)
				{
					m_HardwareKeyboardPoller.Pause();
				}
			}
			else if (evt.eventTypeId == EventBase<KeyDownEvent>.TypeId() && evt is KeyDownEvent { keyCode: KeyCode.Escape })
			{
				RestoreValueAndText();
				base.parent.Focus();
			}
			editorEventHandler.ExecuteDefaultActionAtTarget(evt);
		}

		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			editorEventHandler.ExecuteDefaultAction(evt);
		}

		void ITextInputField.SyncTextEngine()
		{
			SyncTextEngine();
		}

		bool ITextInputField.AcceptCharacter(char c)
		{
			return AcceptCharacter(c);
		}

		string ITextInputField.CullString(string s)
		{
			return CullString(s);
		}

		void ITextInputField.UpdateText(string value)
		{
			UpdateText(value);
		}

		void ITextInputField.UpdateValueFromText()
		{
			UpdateValueFromText();
		}

		private void DeferGUIStyleRectSync()
		{
			RegisterCallback<GeometryChangedEvent>(OnPercentResolved);
		}

		private void OnPercentResolved(GeometryChangedEvent evt)
		{
			UnregisterCallback<GeometryChangedEvent>(OnPercentResolved);
			GUIStyle gUIStyle = editorEngine.style;
			int left = (int)base.resolvedStyle.marginLeft;
			int top = (int)base.resolvedStyle.marginTop;
			int right = (int)base.resolvedStyle.marginRight;
			int bottom = (int)base.resolvedStyle.marginBottom;
			AssignRect(gUIStyle.margin, left, top, right, bottom);
			left = (int)base.resolvedStyle.paddingLeft;
			top = (int)base.resolvedStyle.paddingTop;
			right = (int)base.resolvedStyle.paddingRight;
			bottom = (int)base.resolvedStyle.paddingBottom;
			AssignRect(gUIStyle.padding, left, top, right, bottom);
		}

		private static void SyncGUIStyle(TextInputBase textInput, GUIStyle style)
		{
			ComputedStyle computedStyle = textInput.computedStyle;
			style.alignment = computedStyle.unityTextAlign.GetSpecifiedValueOrDefault(style.alignment);
			style.wordWrap = ((computedStyle.whiteSpace.specificity != 0) ? (computedStyle.whiteSpace.value == WhiteSpace.Normal) : style.wordWrap);
			bool flag = ((computedStyle.overflow.specificity != 0) ? (computedStyle.overflow.value == Overflow.Visible) : (style.clipping == TextClipping.Overflow));
			style.clipping = ((!flag) ? TextClipping.Clip : TextClipping.Overflow);
			if (computedStyle.unityFont.value != null)
			{
				style.font = computedStyle.unityFont.value;
			}
			style.fontSize = (int)computedStyle.fontSize.GetSpecifiedValueOrDefault((float)style.fontSize);
			style.fontStyle = computedStyle.unityFontStyleAndWeight.GetSpecifiedValueOrDefault(style.fontStyle);
			int value = computedStyle.unitySliceLeft.value;
			int value2 = computedStyle.unitySliceTop.value;
			int value3 = computedStyle.unitySliceRight.value;
			int value4 = computedStyle.unitySliceBottom.value;
			AssignRect(style.border, value, value2, value3, value4);
			if (IsLayoutUsingPercent(textInput))
			{
				textInput.DeferGUIStyleRectSync();
				return;
			}
			value = (int)computedStyle.marginLeft.value.value;
			value2 = (int)computedStyle.marginTop.value.value;
			value3 = (int)computedStyle.marginRight.value.value;
			value4 = (int)computedStyle.marginBottom.value.value;
			AssignRect(style.margin, value, value2, value3, value4);
			value = (int)computedStyle.paddingLeft.value.value;
			value2 = (int)computedStyle.paddingTop.value.value;
			value3 = (int)computedStyle.paddingRight.value.value;
			value4 = (int)computedStyle.paddingBottom.value.value;
			AssignRect(style.padding, value, value2, value3, value4);
		}

		private static bool IsLayoutUsingPercent(VisualElement ve)
		{
			ComputedStyle computedStyle = ve.computedStyle;
			if (computedStyle.marginLeft.value.unit == LengthUnit.Percent || computedStyle.marginTop.value.unit == LengthUnit.Percent || computedStyle.marginRight.value.unit == LengthUnit.Percent || computedStyle.marginBottom.value.unit == LengthUnit.Percent)
			{
				return true;
			}
			if (computedStyle.paddingLeft.value.unit == LengthUnit.Percent || computedStyle.paddingTop.value.unit == LengthUnit.Percent || computedStyle.paddingRight.value.unit == LengthUnit.Percent || computedStyle.paddingBottom.value.unit == LengthUnit.Percent)
			{
				return true;
			}
			return false;
		}

		private static void AssignRect(RectOffset rect, int left, int top, int right, int bottom)
		{
			rect.left = left;
			rect.top = top;
			rect.right = right;
			rect.bottom = bottom;
		}
	}

	private static CustomStyleProperty<Color> s_SelectionColorProperty = new CustomStyleProperty<Color>("--unity-selection-color");

	private static CustomStyleProperty<Color> s_CursorColorProperty = new CustomStyleProperty<Color>("--unity-cursor-color");

	private TextInputBase m_TextInputBase;

	internal const int kMaxLengthNone = -1;

	internal const char kMaskCharDefault = '*';

	public new static readonly string ussClassName = "unity-base-text-field";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	public static readonly string textInputUssName = "unity-text-input";

	protected TextInputBase textInputBase => m_TextInputBase;

	internal TextHandle textHandle { get; private set; } = TextHandle.New();

	public string text
	{
		get
		{
			return m_TextInputBase.text;
		}
		protected set
		{
			m_TextInputBase.text = value;
		}
	}

	public bool isReadOnly
	{
		get
		{
			return m_TextInputBase.isReadOnly;
		}
		set
		{
			m_TextInputBase.isReadOnly = value;
			this.onIsReadOnlyChanged?.Invoke(value);
		}
	}

	public bool isPasswordField
	{
		get
		{
			return m_TextInputBase.isPasswordField;
		}
		set
		{
			m_TextInputBase.isPasswordField = value;
		}
	}

	public Color selectionColor => m_TextInputBase.selectionColor;

	public Color cursorColor => m_TextInputBase.cursorColor;

	public int cursorIndex => m_TextInputBase.cursorIndex;

	public int selectIndex => m_TextInputBase.selectIndex;

	public int maxLength
	{
		get
		{
			return m_TextInputBase.maxLength;
		}
		set
		{
			m_TextInputBase.maxLength = value;
		}
	}

	public bool doubleClickSelectsWord
	{
		get
		{
			return m_TextInputBase.doubleClickSelectsWord;
		}
		set
		{
			m_TextInputBase.doubleClickSelectsWord = value;
		}
	}

	public bool tripleClickSelectsLine
	{
		get
		{
			return m_TextInputBase.tripleClickSelectsLine;
		}
		set
		{
			m_TextInputBase.tripleClickSelectsLine = value;
		}
	}

	public bool isDelayed
	{
		get
		{
			return m_TextInputBase.isDelayed;
		}
		set
		{
			m_TextInputBase.isDelayed = value;
		}
	}

	public char maskChar
	{
		get
		{
			return m_TextInputBase.maskChar;
		}
		set
		{
			m_TextInputBase.maskChar = value;
		}
	}

	internal TextEditorEventHandler editorEventHandler => m_TextInputBase.editorEventHandler;

	internal TextEditorEngine editorEngine => m_TextInputBase.editorEngine;

	internal bool hasFocus => m_TextInputBase.hasFocus;

	protected event Action<bool> onIsReadOnlyChanged;

	public void SelectAll()
	{
		m_TextInputBase.SelectAll();
	}

	internal void SyncTextEngine()
	{
		m_TextInputBase.SyncTextEngine();
	}

	internal void DrawWithTextSelectionAndCursor(MeshGenerationContext mgc, string newText)
	{
		m_TextInputBase.DrawWithTextSelectionAndCursor(mgc, newText, base.scaledPixelsPerPoint);
	}

	protected TextInputBaseField(int maxLength, char maskChar, TextInputBase textInputBase)
		: this((string)null, maxLength, maskChar, textInputBase)
	{
	}

	protected TextInputBaseField(string label, int maxLength, char maskChar, TextInputBase textInputBase)
		: base(label, (VisualElement)textInputBase)
	{
		base.tabIndex = 0;
		base.delegatesFocus = false;
		AddToClassList(ussClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
		m_TextInputBase = textInputBase;
		m_TextInputBase.maxLength = maxLength;
		m_TextInputBase.maskChar = maskChar;
		RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
	}

	private void OnAttachToPanel(AttachToPanelEvent e)
	{
		TextHandle textHandle = this.textHandle;
		textHandle.useLegacy = e.destinationPanel.contextType == ContextType.Editor;
		this.textHandle = textHandle;
	}

	protected override void ExecuteDefaultActionAtTarget(EventBase evt)
	{
		base.ExecuteDefaultActionAtTarget(evt);
		if (evt != null && evt.eventTypeId == EventBase<KeyDownEvent>.TypeId())
		{
			KeyDownEvent keyDownEvent = evt as KeyDownEvent;
			if (keyDownEvent?.character == '\u0003' || keyDownEvent?.character == '\n')
			{
				base.visualInput?.Focus();
			}
		}
	}
}
