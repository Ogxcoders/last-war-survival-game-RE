using System.Collections.Generic;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Fps;

public class G_FpsManager : MonoBehaviour, IMovable, IModifiableState
{
	[SerializeField]
	private GameObject m_fpsGraphGameObject;

	[SerializeField]
	private List<GameObject> m_nonBasicTextGameObjects = new List<GameObject>();

	[SerializeField]
	private List<Image> m_backgroundImages = new List<Image>();

	private GraphyManager m_graphyManager;

	private G_FpsGraph m_fpsGraph;

	private G_FpsMonitor m_fpsMonitor;

	private G_FpsText m_fpsText;

	private RectTransform m_rectTransform;

	private List<GameObject> m_childrenGameObjects = new List<GameObject>();

	private GraphyManager.ModuleState m_previousModuleState;

	private GraphyManager.ModuleState m_currentModuleState;

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		UpdateParameters();
	}

	public void SetPosition(GraphyManager.ModulePosition newModulePosition)
	{
		float num = Mathf.Abs(m_rectTransform.anchoredPosition.x);
		float num2 = Mathf.Abs(m_rectTransform.anchoredPosition.y);
		switch (newModulePosition)
		{
		case GraphyManager.ModulePosition.TOP_LEFT:
			m_rectTransform.anchorMax = Vector2.up;
			m_rectTransform.anchorMin = Vector2.up;
			m_rectTransform.anchoredPosition = new Vector2(num, 0f - num2);
			break;
		case GraphyManager.ModulePosition.TOP_RIGHT:
			m_rectTransform.anchorMax = Vector2.one;
			m_rectTransform.anchorMin = Vector2.one;
			m_rectTransform.anchoredPosition = new Vector2(0f - num, 0f - num2);
			break;
		case GraphyManager.ModulePosition.BOTTOM_LEFT:
			m_rectTransform.anchorMax = Vector2.zero;
			m_rectTransform.anchorMin = Vector2.zero;
			m_rectTransform.anchoredPosition = new Vector2(num, num2);
			break;
		case GraphyManager.ModulePosition.BOTTOM_RIGHT:
			m_rectTransform.anchorMax = Vector2.right;
			m_rectTransform.anchorMin = Vector2.right;
			m_rectTransform.anchoredPosition = new Vector2(0f - num, num2);
			break;
		case GraphyManager.ModulePosition.FREE:
			break;
		}
	}

	public void SetState(GraphyManager.ModuleState state, bool silentUpdate = false)
	{
		if (!silentUpdate)
		{
			m_previousModuleState = m_currentModuleState;
		}
		m_currentModuleState = state;
		switch (state)
		{
		case GraphyManager.ModuleState.FULL:
			base.gameObject.SetActive(value: true);
			m_childrenGameObjects.SetAllActive(active: true);
			SetGraphActive(active: true);
			if (m_graphyManager.Background)
			{
				m_backgroundImages.SetOneActive(0);
			}
			else
			{
				m_backgroundImages.SetAllActive(active: false);
			}
			break;
		case GraphyManager.ModuleState.TEXT:
			base.gameObject.SetActive(value: true);
			m_childrenGameObjects.SetAllActive(active: true);
			SetGraphActive(active: false);
			if (m_graphyManager.Background)
			{
				m_backgroundImages.SetOneActive(1);
			}
			else
			{
				m_backgroundImages.SetAllActive(active: false);
			}
			break;
		case GraphyManager.ModuleState.BASIC:
			base.gameObject.SetActive(value: true);
			m_childrenGameObjects.SetAllActive(active: true);
			m_nonBasicTextGameObjects.SetAllActive(active: false);
			SetGraphActive(active: false);
			if (m_graphyManager.Background)
			{
				m_backgroundImages.SetOneActive(2);
			}
			else
			{
				m_backgroundImages.SetAllActive(active: false);
			}
			break;
		case GraphyManager.ModuleState.BACKGROUND:
			base.gameObject.SetActive(value: true);
			m_childrenGameObjects.SetAllActive(active: false);
			SetGraphActive(active: false);
			m_backgroundImages.SetAllActive(active: false);
			break;
		case GraphyManager.ModuleState.OFF:
			base.gameObject.SetActive(value: false);
			break;
		}
	}

	public void RestorePreviousState()
	{
		SetState(m_previousModuleState);
	}

	public void UpdateParameters()
	{
		foreach (Image backgroundImage in m_backgroundImages)
		{
			backgroundImage.color = m_graphyManager.BackgroundColor;
		}
		m_fpsGraph.UpdateParameters();
		m_fpsMonitor.UpdateParameters();
		m_fpsText.UpdateParameters();
		SetState(m_graphyManager.FpsModuleState);
	}

	public void RefreshParameters()
	{
		foreach (Image backgroundImage in m_backgroundImages)
		{
			backgroundImage.color = m_graphyManager.BackgroundColor;
		}
		m_fpsGraph.UpdateParameters();
		m_fpsMonitor.UpdateParameters();
		m_fpsText.UpdateParameters();
		SetState(m_currentModuleState, silentUpdate: true);
	}

	private void Init()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_rectTransform = GetComponent<RectTransform>();
		m_fpsGraph = GetComponent<G_FpsGraph>();
		m_fpsMonitor = GetComponent<G_FpsMonitor>();
		m_fpsText = GetComponent<G_FpsText>();
		foreach (Transform item in base.transform)
		{
			if (item.parent == base.transform)
			{
				m_childrenGameObjects.Add(item.gameObject);
			}
		}
	}

	private void SetGraphActive(bool active)
	{
		m_fpsGraph.enabled = active;
		m_fpsGraphGameObject.SetActive(active);
	}
}
