using System;
using System.Text;
using GameFramework;
using Mopsicus.Plugins;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputFieldTextListener : MonoBehaviour
{
	public class OnChangeEvent : UnityEvent<string, string, int>
	{
	}

	private TMP_InputField tmpInputField;

	private MobileInputField umiInputField;

	private bool useUMI;

	private string previousText = "";

	private int previousSelectionStart;

	private int previousSelectionEnd;

	private OnChangeEvent m_OnTextInsertEvent = new OnChangeEvent();

	private OnChangeEvent m_OnTextDeleteEvent = new OnChangeEvent();

	private bool useUnicodeLength;

	private bool useCompositionLength;

	private bool textChanging;

	public OnChangeEvent OnTextInsertEvent => m_OnTextInsertEvent;

	public OnChangeEvent OnTextDeleteEvent => m_OnTextDeleteEvent;

	private void Awake()
	{
		umiInputField = GetComponent<MobileInputField>();
		if (umiInputField == null)
		{
			tmpInputField = GetComponent<TMP_InputField>();
			if (tmpInputField == null)
			{
				Log.Error("InputFieldTextListener Need TMP_InputField or MobileInputField!");
			}
			else
			{
				useUMI = false;
			}
		}
		else
		{
			useUMI = true;
		}
	}

	private void OnEnable()
	{
		if (useUMI)
		{
			umiInputField.onValueChanged.AddListener(OnTextChanged);
		}
		else
		{
			tmpInputField.onValueChanged.AddListener(OnTextChanged);
		}
	}

	private void OnTextChanged(string newText)
	{
		if (textChanging || (!useUnicodeLength && newText == previousText))
		{
			return;
		}
		textChanging = true;
		int textLength = GetTextLength(newText);
		int textLength2 = GetTextLength(previousText);
		int length = newText.Length;
		int length2 = previousText.Length;
		int num = previousSelectionEnd - previousSelectionStart;
		if (textLength == 0)
		{
			OnTextDelete(0, previousText, previousText);
		}
		else if (num > 0)
		{
			int num2 = Mathf.Min(previousSelectionStart, previousSelectionEnd);
			int num3 = Mathf.Max(previousSelectionStart, previousSelectionEnd);
			string deleteText = SafeSubString(previousText, num2, num3 - num2);
			int start = previousSelectionStart;
			int length3 = newText.Length - (length2 - length);
			string insertText = SafeSubString(newText, start, length3);
			OnTextDelete(num2, deleteText, previousText);
			OnTextInsert(start, insertText, previousText);
		}
		else if (textLength > textLength2)
		{
			if (useUMI)
			{
				int start2 = GetSelection().end - (length - length2);
				string text = SafeSubString(newText, start2, length - length2);
				if (text.Length == 1)
				{
					byte[] bytes = Encoding.UTF8.GetBytes(text);
					text = Encoding.UTF8.GetString(bytes);
				}
				OnTextInsert(start2, text, previousText);
			}
			else
			{
				int start3 = previousSelectionEnd;
				string insertText2 = SafeSubString(newText, start3, length - length2);
				OnTextInsert(start3, insertText2, previousText);
			}
		}
		else if (textLength < textLength2)
		{
			if (textLength2 - textLength > 1)
			{
				int end = GetSelection().end;
				string deleteText2 = SafeSubString(previousText, end, length2 - length);
				OnTextDelete(end, deleteText2, previousText);
			}
			else if (useUMI)
			{
				int end2 = GetSelection().end;
				string text2 = SafeSubString(previousText, end2, 1);
				if (text2.Length == 1)
				{
					byte[] bytes2 = Encoding.UTF8.GetBytes(text2);
					text2 = Encoding.UTF8.GetString(bytes2);
				}
				OnTextDelete(end2, text2, previousText);
			}
			else
			{
				int num4 = previousSelectionStart;
				int num5 = 0;
				num5 = ((num4 != GetSelection().start) ? ((num4 > 0) ? (num4 - 1) : 0) : num4);
				if (num4 == textLength2)
				{
					num5 = textLength2 - 1;
				}
				string text3 = SafeSubString(previousText, num5, 1);
				if (text3.Length == 1 && char.IsLowSurrogate(text3[0]))
				{
					num5--;
					text3 = SafeSubString(previousText, num5, 2);
				}
				OnTextDelete(num5, text3, previousText);
			}
		}
		else if (!useUnicodeLength)
		{
			int num6 = GetSelection().end - 1;
			string deleteText3 = SafeSubString(previousText, num6, 1);
			OnTextDelete(num6, deleteText3, previousText);
			string insertText3 = SafeSubString(newText, num6, 1);
			OnTextInsert(num6, insertText3, previousText);
		}
		if (useUMI)
		{
			previousText = umiInputField.Text;
		}
		else
		{
			previousText = tmpInputField.text;
		}
		RangeInt selection = GetSelection();
		previousSelectionStart = selection.start;
		previousSelectionEnd = selection.end;
		textChanging = false;
	}

	public void CheckReplace(string newText, int currentCharLength)
	{
		bool flag = false;
		int num = -1;
		for (int i = 0; i < currentCharLength; i++)
		{
			if (newText[i] != previousText[i])
			{
				flag = true;
				num = i;
				break;
			}
		}
		if (flag)
		{
			int start = num;
			string insertText = SafeSubString(newText, start, currentCharLength - num);
			OnTextInsert(start, insertText, previousText);
		}
	}

	private void OnTextInsert(int start, string insertText, string previous)
	{
		OnTextInsertEvent.Invoke(insertText, previous, start);
	}

	private void OnTextDelete(int deleteStart, string deleteText, string previous)
	{
		OnTextDeleteEvent.Invoke(deleteText, previous, deleteStart);
	}

	private void OnDisable()
	{
		if (useUMI)
		{
			umiInputField.onValueChanged.RemoveListener(OnTextChanged);
		}
		else
		{
			tmpInputField.onValueChanged.RemoveListener(OnTextChanged);
		}
	}

	private void Update()
	{
		RangeInt selection = GetSelection();
		previousSelectionStart = selection.start;
		previousSelectionEnd = selection.end;
		if (useCompositionLength)
		{
			previousSelectionStart -= GetCompositionLength();
			previousSelectionEnd -= GetCompositionLength();
		}
	}

	public void SetCustomSetting(bool useCompositionLength, bool useUnicodeLength)
	{
		this.useCompositionLength = useCompositionLength;
		this.useUnicodeLength = useUnicodeLength;
	}

	private int GetCompositionLength()
	{
		if (useUMI)
		{
			return 0;
		}
		return tmpInputField.GetCompositionLength();
	}

	private int GetTextLength(string text)
	{
		if (!useUnicodeLength)
		{
			return text.Length;
		}
		return GetTextCharInfoLength(text);
	}

	private int GetTextCharInfoLength(string text)
	{
		int num = 0;
		for (int i = 0; i < text.Length; i++)
		{
			if (char.IsHighSurrogate(text[i]))
			{
				num++;
				if (i + 1 < text.Length && char.IsLowSurrogate(text[i + 1]))
				{
					i++;
				}
			}
			else
			{
				num++;
			}
		}
		return num;
	}

	private RangeInt GetSelection()
	{
		if (useUMI)
		{
			return umiInputField.GetSelection();
		}
		int num = Math.Min(tmpInputField.selectionStringAnchorPosition, tmpInputField.selectionStringFocusPosition);
		int num2 = Math.Max(tmpInputField.selectionStringAnchorPosition, tmpInputField.selectionStringFocusPosition);
		return new RangeInt(num, num2 - num);
	}

	private void SetSelection(RangeInt rangeInt)
	{
		if (useUMI)
		{
			umiInputField.SetSelection(rangeInt);
			return;
		}
		tmpInputField.selectionStringAnchorPosition = rangeInt.start;
		tmpInputField.selectionStringFocusPosition = rangeInt.end;
	}

	private string SafeSubString(string text, int start, int length)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		if (start < 0 || length < 0)
		{
			return string.Empty;
		}
		int textLength = GetTextLength(text);
		if (start >= textLength)
		{
			return string.Empty;
		}
		if (start + length > textLength)
		{
			length = textLength - start;
		}
		return text.Substring(start, length);
	}

	private void OnDestroy()
	{
		tmpInputField = null;
		umiInputField = null;
	}
}
