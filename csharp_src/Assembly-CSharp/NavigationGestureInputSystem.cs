using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NavigationGestureInputSystem : MonoBehaviour
{
	[Header("可视化设置")]
	public bool showVisualization = true;

	private NavigationGestureInputModule m_InputModule;

	private GUIStyle m_BoxStyle;

	private GUIStyle m_LabelStyle;

	private GUIStyle m_ButtonStyle;

	private GUIStyle m_MessageStyle;

	private GUIStyle m_StatusLabelStyle;

	private GUIStyle m_ValueLabelStyle;

	private Texture2D m_RedTexture;

	private Texture2D m_GreenTexture;

	private Texture2D m_BlueTexture;

	private GUIStyle m_RedStyle;

	private GUIStyle m_GreenStyle;

	private GUIStyle m_BlueStyle;

	private GUIStyle m_ArrowStyle;

	private bool m_StylesInitialized;

	private bool m_ShowMessage;

	private float m_Countdown;

	private const float c_DisplayDuration = 0.5f;

	private string m_CustomAreaStatus;

	public string CustomAreaStatus
	{
		get
		{
			return m_CustomAreaStatus;
		}
		set
		{
			m_CustomAreaStatus = value;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		_ = m_InputModule;
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			HandleEscapeKey();
		}
		if (m_ShowMessage)
		{
			m_Countdown -= Time.deltaTime;
			if (m_Countdown <= 0f)
			{
				m_ShowMessage = false;
			}
		}
	}

	private void HandleEscapeKey()
	{
		m_ShowMessage = true;
		m_Countdown = 0.5f;
	}

	private void OnGUI()
	{
		if (m_BoxStyle == null)
		{
			InitializeGUIStyles();
		}
		if (m_InputModule != null)
		{
			DrawVisualization();
			DrawConfigurationGUI();
		}
	}

	private void InitializeGUIStyles()
	{
		m_BoxStyle = new GUIStyle(GUI.skin.box)
		{
			fontSize = 18,
			alignment = TextAnchor.UpperLeft
		};
		m_LabelStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 18,
			alignment = TextAnchor.MiddleLeft,
			fixedHeight = 40f
		};
		m_ButtonStyle = new GUIStyle(GUI.skin.button)
		{
			fontSize = 18,
			wordWrap = true,
			fixedHeight = 60f
		};
		m_MessageStyle = new GUIStyle(GUI.skin.box)
		{
			fontSize = 22,
			alignment = TextAnchor.MiddleCenter
		};
		m_MessageStyle.normal.textColor = Color.white;
		m_StatusLabelStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 16,
			alignment = TextAnchor.MiddleLeft,
			fixedHeight = 40f
		};
		m_ValueLabelStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 18,
			alignment = TextAnchor.MiddleRight,
			fontStyle = FontStyle.Bold,
			fixedHeight = 60f
		};
	}

	private void DrawConfigurationGUI()
	{
		float num = Mathf.Min(750, Screen.width - 20);
		float num2 = Mathf.Min(1100, Screen.height - 20);
		float x = ((float)Screen.width - num) / 2f;
		float y = ((float)Screen.height - num2) / 2f - 300f;
		GUILayout.BeginArea(new Rect(x, y, num, num2), "Gesture Exclusion Controller", m_BoxStyle);
		GUILayout.Space(40f);
		GUILayout.Label($"Current Mode: {m_InputModule.CurrentMode} SDK: {m_InputModule.GetSDKLevel()}", m_LabelStyle, GUILayout.Height(40f));
		GUILayout.Space(30f);
		GUILayout.BeginHorizontal();
		GUI.enabled = m_InputModule.DefaultsFetched;
		if (GUILayout.Button("Apply Default Area\n" + m_InputModule.DefaultRectsString, m_ButtonStyle))
		{
			m_InputModule.ApplyExclusionRectsDefault();
			CustomAreaStatus = string.Empty;
		}
		GUI.enabled = true;
		if (GUILayout.Button("Apply Fullscreen Area", m_ButtonStyle))
		{
			m_InputModule.ApplyExclusionRectsFullscreen();
			CustomAreaStatus = string.Empty;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(30f);
		GUILayout.Label("Edge Detection Settings", m_BoxStyle, GUILayout.Height(40f));
		GUILayout.Space(15f);
		float num3 = DrawSlider("Edge Threshold", m_InputModule.edgeThreshold, 200f);
		m_InputModule.edgeThreshold = (int)num3;
		GUILayout.Space(25f);
		float num4 = DrawSlider("Min Swipe Distance", m_InputModule.minSwipeDistance, 200f);
		m_InputModule.minSwipeDistance = (int)num4;
		GUILayout.Space(30f);
		GUILayout.Label("Custom Area", m_BoxStyle, GUILayout.Height(40f));
		GUILayout.Space(15f);
		float customLeft = Mathf.Min(DrawSlider("Left", m_InputModule.CustomLeft, Screen.width), (float)Screen.width - m_InputModule.CustomRight - 100f);
		m_InputModule.CustomLeft = customLeft;
		GUILayout.Space(25f);
		float num5 = DrawSlider("Right", m_InputModule.CustomRight, Screen.width, reverse: true);
		m_InputModule.CustomRight = (float)Screen.width - Mathf.Max((float)Screen.width - num5, m_InputModule.CustomLeft + 100f);
		GUILayout.Space(25f);
		float customBottom = Mathf.Min(DrawSlider("Bottom", m_InputModule.CustomBottom, Screen.height), (float)Screen.height - m_InputModule.CustomTop - 100f);
		m_InputModule.CustomBottom = customBottom;
		GUILayout.Space(25f);
		float num6 = DrawSlider("Top", m_InputModule.CustomTop, Screen.height, reverse: true);
		m_InputModule.CustomTop = (float)Screen.height - Mathf.Max((float)Screen.height - num6, m_InputModule.CustomBottom + 100f);
		GUILayout.Space(30f);
		GUILayout.Label(CustomAreaStatus, m_StatusLabelStyle, GUILayout.Height(40f));
		GUILayout.Space(30f);
		if (GUILayout.Button("Apply Custom Area", m_ButtonStyle))
		{
			float width = (float)Screen.width - m_InputModule.CustomRight - m_InputModule.CustomLeft;
			float height = (float)Screen.height - m_InputModule.CustomTop - m_InputModule.CustomBottom;
			Rect rect = new Rect(m_InputModule.CustomLeft, m_InputModule.CustomBottom, width, height);
			m_InputModule.ApplyExclusionRects(new List<Rect> { rect });
			m_InputModule.CurrentMode = NavigationGestureInputModule.ExclusionMode.Custom;
			CustomAreaStatus = $"Success: Applied custom rect {rect}";
		}
		GUILayout.EndArea();
		if (m_ShowMessage)
		{
			float num7 = 600f;
			float num8 = 60f;
			GUI.Box(new Rect(((float)Screen.width - num7) / 2f, ((float)Screen.height - num8) / 2f, num7, num8), $"接收到返回(ESC)事件! 消息将在 {m_Countdown:F1} 秒后消失", m_MessageStyle);
		}
	}

	private float DrawSlider(string label, float value, float maxValue, bool reverse = false)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label($"{label}: {value:F0}", GUILayout.Width(120f));
		float num = ((!reverse) ? GUILayout.HorizontalSlider(value, 0f, maxValue, GUILayout.Height(60f), GUILayout.ExpandWidth(expand: true)) : GUILayout.HorizontalSlider(value, maxValue, 0f, GUILayout.Height(60f), GUILayout.ExpandWidth(expand: true)));
		GUILayout.Label($"{num:F0}", m_ValueLabelStyle, GUILayout.Width(80f), GUILayout.Height(60f));
		GUILayout.EndHorizontal();
		return num;
	}

	private void DrawVisualization()
	{
		if (!showVisualization)
		{
			return;
		}
		if (!m_StylesInitialized)
		{
			InitializeVisualizationStyles();
		}
		List<Rect> exclusionRects = m_InputModule.GetExclusionRects();
		GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "", m_RedStyle);
		foreach (Rect item in exclusionRects)
		{
			GUI.Label(new Rect(item.x, (float)Screen.height - item.yMax, item.width, item.height), "", m_GreenStyle);
		}
		Rect rect = new Rect(0f, 0f, m_InputModule.edgeThreshold, Screen.height);
		Rect rect2 = new Rect(Screen.width - m_InputModule.edgeThreshold, 0f, m_InputModule.edgeThreshold, Screen.height);
		foreach (Rect item2 in exclusionRects)
		{
			if (rect.Overlaps(item2))
			{
				Rect rect3 = Rect.MinMaxRect(Mathf.Max(rect.xMin, item2.xMin), Mathf.Max(rect.yMin, item2.yMin), Mathf.Min(rect.xMax, item2.xMax), Mathf.Min(rect.yMax, item2.yMax));
				GUI.Label(new Rect(rect3.x, (float)Screen.height - rect3.yMax, rect3.width, rect3.height), "", m_BlueStyle);
			}
			if (rect2.Overlaps(item2))
			{
				Rect rect4 = Rect.MinMaxRect(Mathf.Max(rect2.xMin, item2.xMin), Mathf.Max(rect2.yMin, item2.yMin), Mathf.Min(rect2.xMax, item2.xMax), Mathf.Min(rect2.yMax, item2.yMax));
				GUI.Label(new Rect(rect4.x, (float)Screen.height - rect4.yMax, rect4.width, rect4.height), "", m_BlueStyle);
			}
		}
		foreach (Rect item3 in exclusionRects)
		{
			if (rect.Overlaps(item3))
			{
				Rect rect5 = Rect.MinMaxRect(Mathf.Max(rect.xMin, item3.xMin), Mathf.Max(rect.yMin, item3.yMin), Mathf.Min(rect.xMax, item3.xMax), Mathf.Min(rect.yMax, item3.yMax));
				float y = (float)Screen.height - rect5.yMax + rect5.height / 2f - 20f;
				GUI.Label(new Rect(rect5.xMin + 10f, y, 30f, 40f), "→", m_ArrowStyle);
			}
			if (rect2.Overlaps(item3))
			{
				Rect rect6 = Rect.MinMaxRect(Mathf.Max(rect2.xMin, item3.xMin), Mathf.Max(rect2.yMin, item3.yMin), Mathf.Min(rect2.xMax, item3.xMax), Mathf.Min(rect2.yMax, item3.yMax));
				float y2 = (float)Screen.height - rect6.yMax + rect6.height / 2f - 20f;
				GUI.Label(new Rect(rect6.xMax - 40f, y2, 30f, 40f), "←", m_ArrowStyle);
			}
		}
	}

	private void InitializeVisualizationStyles()
	{
		m_RedTexture = new Texture2D(1, 1);
		m_RedTexture.SetPixel(0, 0, new Color(1f, 0f, 0f, 0.2f));
		m_RedTexture.Apply();
		m_RedStyle = new GUIStyle();
		m_RedStyle.normal.background = m_RedTexture;
		m_GreenTexture = new Texture2D(1, 1);
		m_GreenTexture.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.3f));
		m_GreenTexture.Apply();
		m_GreenStyle = new GUIStyle();
		m_GreenStyle.normal.background = m_GreenTexture;
		m_BlueTexture = new Texture2D(1, 1);
		m_BlueTexture.SetPixel(0, 0, new Color(0f, 0f, 1f, 0.3f));
		m_BlueTexture.Apply();
		m_BlueStyle = new GUIStyle();
		m_BlueStyle.normal.background = m_BlueTexture;
		m_ArrowStyle = new GUIStyle();
		m_ArrowStyle.normal.textColor = Color.white;
		m_ArrowStyle.fontSize = 24;
		m_ArrowStyle.alignment = TextAnchor.MiddleCenter;
		m_StylesInitialized = true;
	}
}
