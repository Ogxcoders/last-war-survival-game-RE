using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class CircleGesture : MonoBehaviour
{
	private static GameObject Instance;

	public Rect touchRect = new Rect(0f, 0.6f, 1f, 0.4f);

	public bool gizmos;

	private Vector2 screen;

	private List<Vector2> m_GestureDetector = new List<Vector2>(256);

	private Vector2 m_GestureSum = Vector2.zero;

	private float m_GestureLength;

	private int m_GestureCount;

	private int m_GestureEvent;

	private int GestureCount
	{
		get
		{
			return m_GestureCount;
		}
		set
		{
			m_GestureCount = value;
			m_GestureEvent = Math.Min(m_GestureCount, m_GestureEvent);
		}
	}

	public static event Action<int> OnCircleGesture;

	protected void Update()
	{
		DetectAndAction();
		screen = new Vector2(Screen.width, Screen.height);
	}

	protected void OnDestroy()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(Instance);
			Instance = null;
		}
	}

	private bool DetectAndAction()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (Input.touchCount != 1 || !touchRect.Contains(new Vector2(Input.GetTouch(0).position.x / (float)Screen.width, Input.GetTouch(0).position.y / (float)Screen.height)))
			{
				m_GestureDetector.Clear();
				GestureCount = 0;
			}
			else
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
				{
					m_GestureDetector.Clear();
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					Vector2 position = touch.position;
					if (m_GestureDetector.Count == 0 || (position - m_GestureDetector[m_GestureDetector.Count - 1]).magnitude > 10f)
					{
						m_GestureDetector.Add(position);
					}
				}
			}
		}
		else if (Input.GetMouseButtonUp(0) || !touchRect.Contains(new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.height)))
		{
			m_GestureDetector.Clear();
			GestureCount = 0;
		}
		else if (Input.GetMouseButton(0))
		{
			Vector2 vector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if (m_GestureDetector.Count == 0 || (vector - m_GestureDetector[m_GestureDetector.Count - 1]).magnitude > 10f)
			{
				m_GestureDetector.Add(vector);
			}
		}
		if (m_GestureDetector.Count < 10)
		{
			return false;
		}
		m_GestureSum = Vector2.zero;
		m_GestureLength = 0f;
		Vector2 rhs = Vector2.zero;
		for (int i = 0; i < m_GestureDetector.Count - 2; i++)
		{
			Vector2 vector2 = m_GestureDetector[i + 1] - m_GestureDetector[i];
			float magnitude = vector2.magnitude;
			m_GestureSum += vector2;
			m_GestureLength += magnitude;
			if (Vector2.Dot(vector2, rhs) < 0f)
			{
				m_GestureDetector.Clear();
				GestureCount = 0;
				return false;
			}
			rhs = vector2;
		}
		int num = (Screen.width + Screen.height) / 4;
		if (m_GestureLength > (float)num && m_GestureSum.magnitude < (float)(num / 2))
		{
			m_GestureDetector.Clear();
			GestureCount++;
		}
		if (GestureCount > m_GestureEvent)
		{
			m_GestureEvent = GestureCount;
			CircleGesture.OnCircleGesture?.Invoke(m_GestureEvent);
			Debug.Log($"on detect {m_GestureCount} circle gesture");
		}
		return false;
	}
}
