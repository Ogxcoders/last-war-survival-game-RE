namespace UnityEngine;

internal sealed class GUIGridSizer : GUILayoutEntry
{
	private readonly int m_Count;

	private readonly int m_XCount;

	private readonly float m_MinButtonWidth = -1f;

	private readonly float m_MaxButtonWidth = -1f;

	private readonly float m_MinButtonHeight = -1f;

	private readonly float m_MaxButtonHeight = -1f;

	private int rows
	{
		get
		{
			int num = m_Count / m_XCount;
			if (m_Count % m_XCount != 0)
			{
				num++;
			}
			return num;
		}
	}

	public static Rect GetRect(GUIContent[] contents, int xCount, GUIStyle style, GUILayoutOption[] options)
	{
		Rect result = new Rect(0f, 0f, 0f, 0f);
		switch (Event.current.type)
		{
		case EventType.Layout:
			GUILayoutUtility.current.topLevel.Add(new GUIGridSizer(contents, xCount, style, options));
			break;
		case EventType.Used:
			return GUILayoutEntry.kDummyRect;
		default:
			result = GUILayoutUtility.current.topLevel.GetNext().rect;
			break;
		}
		return result;
	}

	private GUIGridSizer(GUIContent[] contents, int xCount, GUIStyle buttonStyle, GUILayoutOption[] options)
		: base(0f, 0f, 0f, 0f, GUIStyle.none)
	{
		m_Count = contents.Length;
		m_XCount = xCount;
		ApplyStyleSettings(buttonStyle);
		ApplyOptions(options);
		if (xCount == 0 || contents.Length == 0)
		{
			return;
		}
		float num = Mathf.Max(buttonStyle.margin.left, buttonStyle.margin.right) * (m_XCount - 1);
		float num2 = Mathf.Max(buttonStyle.margin.top, buttonStyle.margin.bottom) * (rows - 1);
		if (buttonStyle.fixedWidth != 0f)
		{
			m_MinButtonWidth = (m_MaxButtonWidth = buttonStyle.fixedWidth);
		}
		if (buttonStyle.fixedHeight != 0f)
		{
			m_MinButtonHeight = (m_MaxButtonHeight = buttonStyle.fixedHeight);
		}
		if (m_MinButtonWidth == -1f)
		{
			if (minWidth != 0f)
			{
				m_MinButtonWidth = (minWidth - num) / (float)m_XCount;
			}
			if (maxWidth != 0f)
			{
				m_MaxButtonWidth = (maxWidth - num) / (float)m_XCount;
			}
		}
		if (m_MinButtonHeight == -1f)
		{
			if (minHeight != 0f)
			{
				m_MinButtonHeight = (minHeight - num2) / (float)rows;
			}
			if (maxHeight != 0f)
			{
				m_MaxButtonHeight = (maxHeight - num2) / (float)rows;
			}
		}
		if (m_MinButtonHeight == -1f || m_MaxButtonHeight == -1f || m_MinButtonWidth == -1f || m_MaxButtonWidth == -1f)
		{
			float num3 = 0f;
			float num4 = 0f;
			foreach (GUIContent content in contents)
			{
				Vector2 vector = buttonStyle.CalcSize(content);
				num4 = Mathf.Max(num4, vector.x);
				num3 = Mathf.Max(num3, vector.y);
			}
			if (m_MinButtonWidth == -1f)
			{
				if (m_MaxButtonWidth != -1f)
				{
					m_MinButtonWidth = Mathf.Min(num4, m_MaxButtonWidth);
				}
				else
				{
					m_MinButtonWidth = num4;
				}
			}
			if (m_MaxButtonWidth == -1f)
			{
				if (m_MinButtonWidth != -1f)
				{
					m_MaxButtonWidth = Mathf.Max(num4, m_MinButtonWidth);
				}
				else
				{
					m_MaxButtonWidth = num4;
				}
			}
			if (m_MinButtonHeight == -1f)
			{
				if (m_MaxButtonHeight != -1f)
				{
					m_MinButtonHeight = Mathf.Min(num3, m_MaxButtonHeight);
				}
				else
				{
					m_MinButtonHeight = num3;
				}
			}
			if (m_MaxButtonHeight == -1f)
			{
				if (m_MinButtonHeight != -1f)
				{
					maxHeight = Mathf.Max(maxHeight, m_MinButtonHeight);
				}
				m_MaxButtonHeight = maxHeight;
			}
		}
		minWidth = m_MinButtonWidth * (float)m_XCount + num;
		maxWidth = m_MaxButtonWidth * (float)m_XCount + num;
		minHeight = m_MinButtonHeight * (float)rows + num2;
		maxHeight = m_MaxButtonHeight * (float)rows + num2;
	}
}
