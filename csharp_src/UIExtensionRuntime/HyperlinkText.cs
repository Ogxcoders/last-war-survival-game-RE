using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class HyperlinkText : Text, IPointerClickHandler, IEventSystemHandler
{
	private class HyperlinkInfo
	{
		public int startIndex;

		public int endIndex;

		public string name;

		public readonly List<Rect> boxes = new List<Rect>();
	}

	[Serializable]
	public class HrefClickEvent : UnityEvent<string>
	{
	}

	private string m_OutputText;

	private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

	protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

	[SerializeField]
	private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

	private static readonly Regex s_HrefRegex = new Regex("<a href=([^>\\n\\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

	private HyperlinkText mHyperlinkText;

	[SerializeField]
	public string mLink = "www.baidu.com";

	[SerializeField]
	public string mName = "百度";

	public HrefClickEvent onHrefClick
	{
		get
		{
			return m_OnHrefClick;
		}
		set
		{
			m_OnHrefClick = value;
		}
	}

	public string GetHyperlinkInfo => $"<a href={mLink:link}>[{mName:name}]</a>";

	protected override void Awake()
	{
		base.Awake();
		mHyperlinkText = GetComponent<HyperlinkText>();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		mHyperlinkText.onHrefClick.AddListener(OnHyperlinkTextInfo);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		mHyperlinkText.onHrefClick.RemoveListener(OnHyperlinkTextInfo);
	}

	public override void SetVerticesDirty()
	{
		base.SetVerticesDirty();
		m_OutputText = GetOutputText(text);
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		string text = m_Text;
		m_Text = m_OutputText;
		base.OnPopulateMesh(toFill);
		m_Text = text;
		UIVertex vertex = default(UIVertex);
		foreach (HyperlinkInfo hrefInfo in m_HrefInfos)
		{
			hrefInfo.boxes.Clear();
			if (hrefInfo.startIndex >= toFill.currentVertCount)
			{
				continue;
			}
			toFill.PopulateUIVertex(ref vertex, hrefInfo.startIndex);
			Vector3 position = vertex.position;
			Bounds bounds = new Bounds(position, Vector3.zero);
			int i = hrefInfo.startIndex;
			for (int endIndex = hrefInfo.endIndex; i < endIndex && i < toFill.currentVertCount; i++)
			{
				toFill.PopulateUIVertex(ref vertex, i);
				position = vertex.position;
				if (position.x < bounds.min.x)
				{
					hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
					bounds = new Bounds(position, Vector3.zero);
				}
				else
				{
					bounds.Encapsulate(position);
				}
			}
			hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
		}
	}

	protected virtual string GetOutputText(string outputText)
	{
		s_TextBuilder.Length = 0;
		m_HrefInfos.Clear();
		int num = 0;
		foreach (Match item2 in s_HrefRegex.Matches(outputText))
		{
			s_TextBuilder.Append(outputText.Substring(num, item2.Index - num));
			s_TextBuilder.Append("<color=blue>");
			Group obj = item2.Groups[1];
			HyperlinkInfo item = new HyperlinkInfo
			{
				startIndex = s_TextBuilder.Length * 4,
				endIndex = (s_TextBuilder.Length + item2.Groups[2].Length - 1) * 4 + 3,
				name = obj.Value
			};
			m_HrefInfos.Add(item);
			s_TextBuilder.Append(item2.Groups[2].Value);
			s_TextBuilder.Append("</color>");
			num = item2.Index + item2.Length;
		}
		s_TextBuilder.Append(outputText.Substring(num, outputText.Length - num));
		return s_TextBuilder.ToString();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Vector2 localPoint = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
		foreach (HyperlinkInfo hrefInfo in m_HrefInfos)
		{
			List<Rect> boxes = hrefInfo.boxes;
			for (int i = 0; i < boxes.Count; i++)
			{
				if (boxes[i].Contains(localPoint))
				{
					m_OnHrefClick.Invoke(hrefInfo.name);
					return;
				}
			}
		}
	}

	private void OnHyperlinkTextInfo(string info)
	{
	}
}
