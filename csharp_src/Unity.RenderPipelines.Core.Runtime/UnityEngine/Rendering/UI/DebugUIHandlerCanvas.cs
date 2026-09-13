using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.Rendering.UI;

public class DebugUIHandlerCanvas : MonoBehaviour
{
	private int m_DebugTreeState;

	private Dictionary<Type, Transform> m_PrefabsMap;

	public Transform panelPrefab;

	public List<DebugUIPrefabBundle> prefabs;

	private List<DebugUIHandlerPanel> m_UIPanels;

	private int m_SelectedPanel;

	private DebugUIHandlerWidget m_SelectedWidget;

	private string m_CurrentQueryPath;

	private void OnEnable()
	{
		if (prefabs == null)
		{
			prefabs = new List<DebugUIPrefabBundle>();
		}
		if (m_PrefabsMap == null)
		{
			m_PrefabsMap = new Dictionary<Type, Transform>();
		}
		if (m_UIPanels == null)
		{
			m_UIPanels = new List<DebugUIHandlerPanel>();
		}
		DebugManager.instance.RegisterRootCanvas(this);
	}

	private void Update()
	{
		int state = DebugManager.instance.GetState();
		if (m_DebugTreeState != state)
		{
			ResetAllHierarchy();
		}
		HandleInput();
		if (m_UIPanels != null && m_SelectedPanel < m_UIPanels.Count && m_UIPanels[m_SelectedPanel] != null)
		{
			m_UIPanels[m_SelectedPanel].ScrollTo(m_SelectedWidget);
		}
	}

	internal void ResetAllHierarchy()
	{
		foreach (Transform item in base.transform)
		{
			CoreUtils.Destroy(item.gameObject);
		}
		Rebuild();
	}

	private void Rebuild()
	{
		m_PrefabsMap.Clear();
		foreach (DebugUIPrefabBundle prefab in prefabs)
		{
			Type type = Type.GetType(prefab.type);
			if (type != null && prefab.prefab != null)
			{
				m_PrefabsMap.Add(type, prefab.prefab);
			}
		}
		m_UIPanels.Clear();
		m_DebugTreeState = DebugManager.instance.GetState();
		foreach (DebugUI.Panel panel in DebugManager.instance.panels)
		{
			if (!panel.isEditorOnly && panel.children.Count((DebugUI.Widget x) => !x.isEditorOnly) != 0)
			{
				GameObject obj = Object.Instantiate(panelPrefab, base.transform, worldPositionStays: false).gameObject;
				obj.name = panel.displayName;
				DebugUIHandlerPanel component = obj.GetComponent<DebugUIHandlerPanel>();
				component.SetPanel(panel);
				m_UIPanels.Add(component);
				DebugUIHandlerContainer component2 = obj.GetComponent<DebugUIHandlerContainer>();
				Traverse(panel, component2.contentHolder, null);
			}
		}
		ActivatePanel(m_SelectedPanel, tryAndKeepSelection: true);
	}

	private void Traverse(DebugUI.IContainer container, Transform parentTransform, DebugUIHandlerWidget parentUIHandler)
	{
		DebugUIHandlerWidget debugUIHandlerWidget = null;
		for (int i = 0; i < container.children.Count; i++)
		{
			DebugUI.Widget widget = container.children[i];
			if (widget.isEditorOnly)
			{
				continue;
			}
			if (!m_PrefabsMap.TryGetValue(widget.GetType(), out var value))
			{
				Debug.LogWarning("DebugUI widget doesn't have a prefab: " + widget.GetType());
				continue;
			}
			GameObject gameObject = Object.Instantiate(value, parentTransform, worldPositionStays: false).gameObject;
			gameObject.name = widget.displayName;
			DebugUIHandlerWidget component = gameObject.GetComponent<DebugUIHandlerWidget>();
			if (component == null)
			{
				Debug.LogWarning("DebugUI prefab is missing a DebugUIHandler for: " + widget.GetType());
				continue;
			}
			if (debugUIHandlerWidget != null)
			{
				debugUIHandlerWidget.nextUIHandler = component;
			}
			component.previousUIHandler = debugUIHandlerWidget;
			debugUIHandlerWidget = component;
			component.parentUIHandler = parentUIHandler;
			component.SetWidget(widget);
			DebugUIHandlerContainer component2 = gameObject.GetComponent<DebugUIHandlerContainer>();
			if (component2 != null && widget is DebugUI.IContainer)
			{
				Traverse(widget as DebugUI.IContainer, component2.contentHolder, component);
			}
		}
	}

	private DebugUIHandlerWidget GetWidgetFromPath(string queryPath)
	{
		if (string.IsNullOrEmpty(queryPath))
		{
			return null;
		}
		return m_UIPanels[m_SelectedPanel].GetComponentsInChildren<DebugUIHandlerWidget>().FirstOrDefault((DebugUIHandlerWidget w) => w.GetWidget().queryPath == queryPath);
	}

	private void ActivatePanel(int index, bool tryAndKeepSelection = false)
	{
		if (m_UIPanels.Count == 0)
		{
			return;
		}
		if (index >= m_UIPanels.Count)
		{
			index = m_UIPanels.Count - 1;
		}
		m_UIPanels.ForEach(delegate(DebugUIHandlerPanel p)
		{
			p.gameObject.SetActive(value: false);
		});
		m_UIPanels[index].gameObject.SetActive(value: true);
		m_SelectedPanel = index;
		DebugUIHandlerWidget debugUIHandlerWidget = null;
		if (tryAndKeepSelection && !string.IsNullOrEmpty(m_CurrentQueryPath))
		{
			debugUIHandlerWidget = m_UIPanels[m_SelectedPanel].GetComponentsInChildren<DebugUIHandlerWidget>().FirstOrDefault((DebugUIHandlerWidget w) => w.GetWidget().queryPath == m_CurrentQueryPath);
		}
		if (debugUIHandlerWidget == null)
		{
			debugUIHandlerWidget = m_UIPanels[index].GetFirstItem();
		}
		ChangeSelection(debugUIHandlerWidget, fromNext: true);
	}

	internal void ChangeSelection(DebugUIHandlerWidget widget, bool fromNext)
	{
		if (widget == null)
		{
			return;
		}
		if (m_SelectedWidget != null)
		{
			m_SelectedWidget.OnDeselection();
		}
		DebugUIHandlerWidget selectedWidget = m_SelectedWidget;
		m_SelectedWidget = widget;
		if (!m_SelectedWidget.OnSelection(fromNext, selectedWidget))
		{
			if (fromNext)
			{
				SelectNextItem();
			}
			else
			{
				SelectPreviousItem();
			}
		}
		else if (m_SelectedWidget == null || m_SelectedWidget.GetWidget() == null)
		{
			m_CurrentQueryPath = string.Empty;
		}
		else
		{
			m_CurrentQueryPath = m_SelectedWidget.GetWidget().queryPath;
		}
	}

	private void SelectPreviousItem()
	{
		if (!(m_SelectedWidget == null))
		{
			DebugUIHandlerWidget debugUIHandlerWidget = m_SelectedWidget.Previous();
			if (debugUIHandlerWidget != null)
			{
				ChangeSelection(debugUIHandlerWidget, fromNext: false);
			}
		}
	}

	private void SelectNextItem()
	{
		if (!(m_SelectedWidget == null))
		{
			DebugUIHandlerWidget debugUIHandlerWidget = m_SelectedWidget.Next();
			if (debugUIHandlerWidget != null)
			{
				ChangeSelection(debugUIHandlerWidget, fromNext: true);
			}
		}
	}

	private void ChangeSelectionValue(float multiplier)
	{
		if (!(m_SelectedWidget == null))
		{
			bool fast = DebugManager.instance.GetAction(DebugAction.Multiplier) != 0f;
			if (multiplier < 0f)
			{
				m_SelectedWidget.OnDecrement(fast);
			}
			else
			{
				m_SelectedWidget.OnIncrement(fast);
			}
		}
	}

	private void ActivateSelection()
	{
		if (!(m_SelectedWidget == null))
		{
			m_SelectedWidget.OnAction();
		}
	}

	private void HandleInput()
	{
		if (DebugManager.instance.GetAction(DebugAction.PreviousDebugPanel) != 0f)
		{
			int num = m_SelectedPanel - 1;
			if (num < 0)
			{
				num = m_UIPanels.Count - 1;
			}
			num = Mathf.Clamp(num, 0, m_UIPanels.Count - 1);
			ActivatePanel(num);
		}
		if (DebugManager.instance.GetAction(DebugAction.NextDebugPanel) != 0f)
		{
			int num2 = m_SelectedPanel + 1;
			if (num2 >= m_UIPanels.Count)
			{
				num2 = 0;
			}
			num2 = Mathf.Clamp(num2, 0, m_UIPanels.Count - 1);
			ActivatePanel(num2);
		}
		if (DebugManager.instance.GetAction(DebugAction.Action) != 0f)
		{
			ActivateSelection();
		}
		if (DebugManager.instance.GetAction(DebugAction.MakePersistent) != 0f && m_SelectedWidget != null)
		{
			DebugManager.instance.TogglePersistent(m_SelectedWidget.GetWidget());
		}
		float action = DebugManager.instance.GetAction(DebugAction.MoveHorizontal);
		if (action != 0f)
		{
			ChangeSelectionValue(action);
		}
		float action2 = DebugManager.instance.GetAction(DebugAction.MoveVertical);
		if (action2 != 0f)
		{
			if (action2 < 0f)
			{
				SelectNextItem();
			}
			else
			{
				SelectPreviousItem();
			}
		}
	}
}
