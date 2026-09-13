namespace UnityEngine.UIElements;

internal class TextEditorEngine : TextEditor
{
	internal delegate void OnDetectFocusChangeFunction();

	internal delegate void OnIndexChangeFunction();

	private OnDetectFocusChangeFunction m_DetectFocusChangeFunction;

	private OnIndexChangeFunction m_IndexChangeFunction;

	internal override Rect localPosition => new Rect(0f, 0f, base.position.width, base.position.height);

	public TextEditorEngine(OnDetectFocusChangeFunction detectFocusChange, OnIndexChangeFunction indexChangeFunction)
	{
		m_DetectFocusChangeFunction = detectFocusChange;
		m_IndexChangeFunction = indexChangeFunction;
	}

	internal override void OnDetectFocusChange()
	{
		m_DetectFocusChangeFunction();
	}

	internal override void OnCursorIndexChange()
	{
		m_IndexChangeFunction();
	}

	internal override void OnSelectIndexChange()
	{
		m_IndexChangeFunction();
	}
}
