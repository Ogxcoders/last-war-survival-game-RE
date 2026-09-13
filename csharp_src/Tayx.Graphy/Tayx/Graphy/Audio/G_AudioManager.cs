using System.Collections.Generic;
using Tayx.Graphy.UI;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Tayx.Graphy.Audio;

public class G_AudioManager : MonoBehaviour, IMovable, IModifiableState
{
	[SerializeField]
	private GameObject m_audioGraphGameObject;

	[SerializeField]
	private Text m_audioDbText;

	[SerializeField]
	private List<Image> m_backgroundImages = new List<Image>();

	private GraphyManager m_graphyManager;

	private G_AudioGraph m_audioGraph;

	private G_AudioMonitor m_audioMonitor;

	private G_AudioText m_audioText;

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
		m_audioDbText.alignment = TextAnchor.UpperRight;
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
		case GraphyManager.ModuleState.BASIC:
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
		case GraphyManager.ModuleState.BACKGROUND:
			base.gameObject.SetActive(value: true);
			SetGraphActive(active: false);
			m_childrenGameObjects.SetAllActive(active: false);
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
		m_audioGraph.UpdateParameters();
		m_audioMonitor.UpdateParameters();
		m_audioText.UpdateParameters();
		SetState(m_graphyManager.AudioModuleState);
	}

	public void RefreshParameters()
	{
		foreach (Image backgroundImage in m_backgroundImages)
		{
			backgroundImage.color = m_graphyManager.BackgroundColor;
		}
		m_audioGraph.UpdateParameters();
		m_audioMonitor.UpdateParameters();
		m_audioText.UpdateParameters();
		SetState(m_currentModuleState, silentUpdate: true);
	}

	private void Init()
	{
		m_graphyManager = base.transform.root.GetComponentInChildren<GraphyManager>();
		m_rectTransform = GetComponent<RectTransform>();
		m_audioGraph = GetComponent<G_AudioGraph>();
		m_audioMonitor = GetComponent<G_AudioMonitor>();
		m_audioText = GetComponent<G_AudioText>();
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
		m_audioGraph.enabled = active;
		m_audioGraphGameObject.SetActive(active);
	}
}
