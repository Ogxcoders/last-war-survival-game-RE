using System;
using System.Collections.Generic;
using GameKit.Base;
using TMPro;
using UnityEngine;

public class InputFieldMention : MonoBehaviour
{
	private List<MentionInfo> _mentions;

	private InputFieldTextListener _listener;

	private MentionInfo _newInsertMentions;

	private RangeInt lastSelection;

	private TMP_InputField inputField;

	private void Awake()
	{
		inputField = GetComponent<TMP_InputField>();
		_mentions = new List<MentionInfo>();
		lastSelection = default(RangeInt);
		_listener = base.gameObject.GetOrAddComponent<InputFieldTextListener>();
		_listener.OnTextInsertEvent.AddListener(OnTextInsert);
		_listener.OnTextDeleteEvent.AddListener(OnTextDelete);
	}

	private void Update()
	{
		SetTextColorRange(_mentions);
		RangeInt selection = GetSelection();
		if (!selection.Equals(lastSelection))
		{
			OnSelectionChange(selection, lastSelection);
		}
		lastSelection = selection;
	}

	private void OnDestroy()
	{
		_listener.OnTextInsertEvent.RemoveListener(OnTextInsert);
		_listener.OnTextDeleteEvent.RemoveListener(OnTextDelete);
		_listener = null;
		ClearMentions();
		_newInsertMentions = null;
	}

	private void OnSelectionChange(RangeInt now, RangeInt last)
	{
		if (_mentions.Count <= 0)
		{
			return;
		}
		foreach (MentionInfo mention in _mentions)
		{
			if (now.start < mention.range.end && now.end > mention.range.start)
			{
				SetSelection(mention.range);
			}
		}
	}

	public RangeInt GetSelection()
	{
		int num = Math.Min(inputField.selectionStringAnchorPosition, inputField.selectionStringFocusPosition);
		int num2 = Math.Max(inputField.selectionStringAnchorPosition, inputField.selectionStringFocusPosition);
		return new RangeInt(num, num2 - num);
	}

	public void SetSelection(RangeInt rangeInt)
	{
		inputField.selectionStringAnchorPosition = rangeInt.start;
		inputField.selectionStringFocusPosition = rangeInt.end;
	}

	public void SetTextColorRange(List<MentionInfo> mentions)
	{
		TMP_Text textComponent = inputField.textComponent;
		textComponent.ForceMeshUpdate(ignoreActiveState: true);
		TMP_TextInfo textInfo = textComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0)
		{
			return;
		}
		for (int i = 0; i < mentions.Count; i++)
		{
			MentionInfo mentionInfo = mentions[i];
			int num = Mathf.Clamp(mentionInfo.range.start, 0, characterCount - 1);
			int num2 = Mathf.Clamp(mentionInfo.range.end, 0, characterCount - 1);
			for (int j = num; j < num2; j++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[j];
				if (tMP_CharacterInfo.isVisible)
				{
					int vertexIndex = tMP_CharacterInfo.vertexIndex;
					int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
					Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
					for (int k = 0; k < 4; k++)
					{
						colors[vertexIndex + k] = mentionInfo.curColor;
					}
				}
			}
		}
		textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
	}

	public void InsertMention(string text, int offset, string colorStr, int index)
	{
		RangeInt selection = GetSelection();
		MentionInfo mentionInfo = new MentionInfo();
		mentionInfo.index = index;
		mentionInfo.range = new RangeInt(selection.start - offset, text.Length);
		mentionInfo.text = text;
		mentionInfo.color = colorStr;
		if (!ColorUtility.TryParseHtmlString(colorStr, out var color))
		{
			color = Color.blue;
		}
		mentionInfo.curColor = color;
		_mentions.Add(mentionInfo);
		_newInsertMentions = mentionInfo;
		InsertTextAtCaret(text, offset);
	}

	public void ResetMentions(List<MentionInfo> list)
	{
		_mentions.Clear();
		HashSet<int> hashSet = new HashSet<int>();
		string text = inputField.text;
		foreach (MentionInfo item in list)
		{
			string text2 = item.text;
			for (int num = text.IndexOf(text2, 0, StringComparison.CurrentCulture); num != -1; num = text.IndexOf(text2, num + 1, StringComparison.CurrentCulture))
			{
				if (hashSet.Add(num))
				{
					item.range = new RangeInt(num, text2.Length);
					if (!ColorUtility.TryParseHtmlString(item.color, out var color))
					{
						color = Color.blue;
					}
					item.curColor = color;
					_mentions.Add(item);
					break;
				}
			}
		}
	}

	public void InsertTextAtCaret(string textToInsert, int offset)
	{
		RangeInt selection = GetSelection();
		string text = inputField.text;
		int num = selection.start;
		int end = selection.end;
		if (num != end)
		{
			int num2 = Mathf.Min(num, end);
			int num3 = Mathf.Max(num, end);
			text = text.Remove(num2, num3 - num2);
			num = num2;
		}
		else if (offset > 0)
		{
			int num4 = Mathf.Max(num - offset, 0);
			text = text.Remove(num4, offset);
			num = num4;
		}
		inputField.text = text.Insert(num, textToInsert);
		SetSelection(new RangeInt(num + textToInsert.Length, 0));
	}

	private void OnTextInsert(string insertText, string previous, int start)
	{
		int num = insertText.Length;
		if (num == 1 && char.IsHighSurrogate(insertText[0]))
		{
			num = 2;
		}
		foreach (MentionInfo mention in _mentions)
		{
			if (start <= mention.range.start && _newInsertMentions != mention)
			{
				mention.range.start += num;
			}
		}
		_newInsertMentions = null;
	}

	private void OnTextDelete(string deleteText, string previous, int deleteStart)
	{
		int length = deleteText.Length;
		int num = deleteStart;
		int num2 = deleteStart + length;
		int num3 = num2;
		List<MentionInfo> list = new List<MentionInfo>();
		foreach (MentionInfo mention in _mentions)
		{
			int start = mention.range.start;
			int num4 = start + mention.range.length - 1;
			if (num3 > start && deleteStart <= num4)
			{
				list.Add(mention);
			}
		}
		foreach (MentionInfo item in list)
		{
			_mentions.Remove(item);
			GameEntry.Event.Fire(EventId.ChatAtMentionDelete, item.index);
			deleteStart = Math.Min(item.range.start, deleteStart);
			num3 = Math.Max(item.range.end, num3);
		}
		int num5 = num3 - deleteStart;
		foreach (MentionInfo mention2 in _mentions)
		{
			if (mention2.range.start >= num3)
			{
				mention2.range.start -= num5;
			}
		}
		if (list.Count > 0)
		{
			deleteStart = Mathf.Max(0, deleteStart);
			if (num3 > previous.Length)
			{
				num3 = previous.Length - deleteStart;
			}
			if (deleteStart != num || num3 != num2)
			{
				inputField.text = previous.Remove(deleteStart, num3 - deleteStart);
			}
		}
	}

	public void ClearMentions()
	{
		_mentions.Clear();
		_mentions = null;
	}
}
