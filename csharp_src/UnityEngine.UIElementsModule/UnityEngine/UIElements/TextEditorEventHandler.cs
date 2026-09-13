namespace UnityEngine.UIElements;

internal class TextEditorEventHandler
{
	protected TextEditorEngine editorEngine { get; private set; }

	protected ITextInputField textInputField { get; private set; }

	protected TextEditorEventHandler(TextEditorEngine editorEngine, ITextInputField textInputField)
	{
		this.editorEngine = editorEngine;
		this.textInputField = textInputField;
		this.textInputField.SyncTextEngine();
	}

	public virtual void ExecuteDefaultActionAtTarget(EventBase evt)
	{
	}

	public virtual void ExecuteDefaultAction(EventBase evt)
	{
		if (evt.eventTypeId == EventBase<FocusEvent>.TypeId())
		{
			editorEngine.OnFocus();
			editorEngine.SelectAll();
		}
		else if (evt.eventTypeId == EventBase<BlurEvent>.TypeId())
		{
			editorEngine.OnLostFocus();
			editorEngine.SelectNone();
		}
	}
}
