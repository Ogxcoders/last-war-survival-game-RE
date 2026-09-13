using System;
using System.Collections.Generic;
using UnityEngine;

public class NavigationGestureInputModule
{
	public enum ExclusionMode
	{
		Uninitialized,
		Default,
		Fullscreen,
		Custom
	}

	private bool m_Enabled;

	public int edgeThreshold = 100;

	public int minSwipeDistance = 50;

	private ExclusionMode m_CurrentMode;

	private List<Rect> m_DefaultRects = new List<Rect>();

	private List<Rect> m_CachedRects = new List<Rect>();

	private float m_CustomLeft;

	private float m_CustomTop;

	private float m_CustomRight;

	private float m_CustomBottom;

	private bool m_IsTracking;

	private Vector2 m_StartTouchPosition;

	private float m_StartTime;

	private int m_ScreenWidthOld;

	private int m_ScreenHeightOld;

	private NavigationGestureAndroidUtility m_AndroidUtility;

	public Action OnNavigationGestureEscape;

	public ExclusionMode CurrentMode
	{
		get
		{
			return m_CurrentMode;
		}
		set
		{
			m_CurrentMode = value;
		}
	}

	public bool DefaultsFetched => m_AndroidUtility.DefaultsFetched;

	public string DefaultRectsString => m_AndroidUtility.DefaultRectsString;

	public float CustomLeft
	{
		get
		{
			return m_CustomLeft;
		}
		set
		{
			m_CustomLeft = value;
		}
	}

	public float CustomRight
	{
		get
		{
			return m_CustomRight;
		}
		set
		{
			m_CustomRight = value;
		}
	}

	public float CustomTop
	{
		get
		{
			return m_CustomTop;
		}
		set
		{
			m_CustomTop = value;
		}
	}

	public float CustomBottom
	{
		get
		{
			return m_CustomBottom;
		}
		set
		{
			m_CustomBottom = value;
		}
	}

	public NavigationGestureInputModule()
	{
		m_AndroidUtility = new NavigationGestureAndroidUtility();
		InitializeCustomRect();
	}

	public void Initialize(int leftEdgePixels, int rightEdgePixels, int bottomEdgePixels, int topEdgePixels, int edgeThresholdPixels, int minSwipePixels)
	{
		FetchSystemRects();
		CustomLeft = leftEdgePixels;
		CustomRight = rightEdgePixels;
		CustomBottom = bottomEdgePixels;
		CustomTop = topEdgePixels;
		edgeThreshold = edgeThresholdPixels;
		minSwipeDistance = minSwipePixels;
		m_ScreenWidthOld = Screen.width;
		m_ScreenHeightOld = Screen.height;
		ApplyExclusionRectsCustom();
	}

	public void SetEnable(bool enabled)
	{
		if (m_Enabled == enabled)
		{
			return;
		}
		if (enabled)
		{
			if (OnNavigationGestureEscape != null)
			{
				ApplyExclusionRectsCustom();
			}
			OnNavigationGestureEscape = OnNavigationGestureEscape ?? ((Action)delegate
			{
				GameEntry.Event.Fire(EventId.OnAndroidNavigationGestureEscape);
			});
			m_Enabled = true;
		}
		else
		{
			ApplyExclusionRectsDefault();
			m_Enabled = false;
		}
	}

	public void Update()
	{
		if (m_Enabled)
		{
			if (m_ScreenWidthOld != Screen.width || m_ScreenHeightOld != Screen.height)
			{
				ResizeExclusionRectsCurrent();
				m_ScreenWidthOld = Screen.width;
				m_ScreenHeightOld = Screen.height;
			}
			HandleTouchInput();
		}
	}

	private void InitializeCustomRect()
	{
		m_CustomLeft = 0f;
		m_CustomBottom = 0f;
		m_CustomRight = 0f;
		m_CustomTop = 0f;
	}

	private void HandleTouchInput()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.phase)
			{
			case TouchPhase.Began:
				StartTouchTracking(touch.position);
				break;
			case TouchPhase.Ended:
				EndTouchTracking(touch.position);
				break;
			case TouchPhase.Canceled:
				m_IsTracking = false;
				break;
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
				break;
			}
		}
	}

	private void StartTouchTracking(Vector2 position)
	{
		if ((position.x <= (float)edgeThreshold || position.x >= (float)(Screen.width - edgeThreshold)) && IsPositionInExclusionArea(position))
		{
			m_StartTouchPosition = position;
			m_StartTime = Time.time;
			m_IsTracking = true;
		}
	}

	private void EndTouchTracking(Vector2 position)
	{
		if (m_IsTracking)
		{
			m_IsTracking = false;
			_ = Time.time;
			_ = m_StartTime;
			if (Vector2.Distance(m_StartTouchPosition, position) >= (float)minSwipeDistance)
			{
				ProcessSwipe(m_StartTouchPosition, position);
			}
		}
	}

	private void ProcessSwipe(Vector2 start, Vector2 end)
	{
		Vector2 vector = end - start;
		if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
		{
			bool num = start.x <= (float)edgeThreshold;
			bool flag = start.x >= (float)(Screen.width - edgeThreshold);
			if (num && vector.x > 0f)
			{
				OnNavigationGestureEscape?.Invoke();
			}
			else if (flag && vector.x < 0f)
			{
				OnNavigationGestureEscape?.Invoke();
			}
		}
	}

	public bool IsPositionInExclusionArea(Vector2 position)
	{
		foreach (Rect exclusionRect in GetExclusionRects())
		{
			if (exclusionRect.Contains(position))
			{
				return true;
			}
		}
		return false;
	}

	public List<Rect> GetExclusionRects()
	{
		return GetExclusionRects(m_CurrentMode);
	}

	public List<Rect> GetExclusionRects(ExclusionMode exclusionMode)
	{
		m_CachedRects.Clear();
		switch (exclusionMode)
		{
		case ExclusionMode.Default:
			m_CachedRects.AddRange(m_DefaultRects);
			break;
		case ExclusionMode.Fullscreen:
			m_CachedRects.Add(new Rect(0f, 0f, Screen.width, Screen.height));
			break;
		case ExclusionMode.Custom:
		{
			float width = (float)Screen.width - m_CustomRight - m_CustomLeft;
			float height = (float)Screen.height - m_CustomTop - m_CustomBottom;
			m_CachedRects.Add(new Rect(m_CustomLeft, m_CustomBottom, width, height));
			break;
		}
		}
		return m_CachedRects;
	}

	public void ApplyExclusionRects(List<Rect> rectsToApply)
	{
		m_AndroidUtility.ApplyExclusionRects(rectsToApply);
	}

	public void ApplyExclusionRectsDefault()
	{
		if (m_AndroidUtility != null && m_AndroidUtility.DefaultsFetched)
		{
			ApplyExclusionRects(GetExclusionRects(ExclusionMode.Default));
			CurrentMode = ExclusionMode.Default;
		}
	}

	public void ApplyExclusionRectsFullscreen()
	{
		ApplyExclusionRects(GetExclusionRects(ExclusionMode.Fullscreen));
		CurrentMode = ExclusionMode.Fullscreen;
	}

	public void ApplyExclusionRectsCustom()
	{
		ApplyExclusionRects(GetExclusionRects(ExclusionMode.Custom));
		CurrentMode = ExclusionMode.Custom;
	}

	public void ResizeExclusionRectsCurrent()
	{
		ApplyExclusionRects(GetExclusionRects(CurrentMode));
	}

	public void FetchSystemRects()
	{
		if (m_AndroidUtility != null)
		{
			m_DefaultRects = m_AndroidUtility.FetchSystemRects();
		}
	}

	public int GetSDKLevel()
	{
		return m_AndroidUtility.GetSDKLevel();
	}
}
