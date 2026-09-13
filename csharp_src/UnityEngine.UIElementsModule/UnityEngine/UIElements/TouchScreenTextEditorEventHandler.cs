namespace UnityEngine.UIElements;

internal class TouchScreenTextEditorEventHandler : TextEditorEventHandler
{
	private IVisualElementScheduledItem m_TouchKeyboardPoller = null;

	public TouchScreenTextEditorEventHandler(TextEditorEngine editorEngine, ITextInputField textInputField)
		: base(editorEngine, textInputField)
	{
	}

	private void PollTouchScreenKeyboard()
	{
		if (TouchScreenKeyboard.isSupported && !TouchScreenKeyboard.isInPlaceEditingAllowed)
		{
			if (m_TouchKeyboardPoller == null)
			{
				m_TouchKeyboardPoller = (base.textInputField as VisualElement)?.schedule.Execute(DoPollTouchScreenKeyboard).Every(100L);
			}
			else
			{
				m_TouchKeyboardPoller.Resume();
			}
		}
	}

	private void DoPollTouchScreenKeyboard()
	{
		if (TouchScreenKeyboard.isSupported && !TouchScreenKeyboard.isInPlaceEditingAllowed)
		{
			if (base.textInputField.editorEngine.keyboardOnScreen == null)
			{
				return;
			}
			base.textInputField.UpdateText(base.textInputField.CullString(base.textInputField.editorEngine.keyboardOnScreen.text));
			if (!base.textInputField.isDelayed)
			{
				base.textInputField.UpdateValueFromText();
			}
			if (base.textInputField.editorEngine.keyboardOnScreen.status != TouchScreenKeyboard.Status.Visible)
			{
				base.textInputField.editorEngine.keyboardOnScreen = null;
				m_TouchKeyboardPoller.Pause();
				if (base.textInputField.isDelayed)
				{
					base.textInputField.UpdateValueFromText();
				}
			}
		}
		else
		{
			base.textInputField.editorEngine.keyboardOnScreen.active = false;
			base.textInputField.editorEngine.keyboardOnScreen = null;
			m_TouchKeyboardPoller.Pause();
		}
	}

	public override void ExecuteDefaultActionAtTarget(EventBase evt)
	{
		base.ExecuteDefaultActionAtTarget(evt);
		long num = EventBase<MouseDownEvent>.TypeId();
		if (!base.textInputField.isReadOnly && evt.eventTypeId == num && base.editorEngine.keyboardOnScreen == null)
		{
			base.textInputField.SyncTextEngine();
			base.textInputField.UpdateText(base.editorEngine.text);
			base.editorEngine.keyboardOnScreen = TouchScreenKeyboard.Open(base.textInputField.text, TouchScreenKeyboardType.Default, autocorrection: true, base.editorEngine.multiline, base.textInputField.isPasswordField);
			if (base.editorEngine.keyboardOnScreen != null)
			{
				PollTouchScreenKeyboard();
			}
			base.editorEngine.UpdateScrollOffset();
			evt.StopPropagation();
		}
	}
}
