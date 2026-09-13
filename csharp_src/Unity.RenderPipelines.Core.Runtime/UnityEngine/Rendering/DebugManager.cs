using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Rendering.UI;

namespace UnityEngine.Rendering;

public sealed class DebugManager
{
	private const string kEnableDebugBtn1 = "Enable Debug Button 1";

	private const string kEnableDebugBtn2 = "Enable Debug Button 2";

	private const string kDebugPreviousBtn = "Debug Previous";

	private const string kDebugNextBtn = "Debug Next";

	private const string kValidateBtn = "Debug Validate";

	private const string kPersistentBtn = "Debug Persistent";

	private const string kDPadVertical = "Debug Vertical";

	private const string kDPadHorizontal = "Debug Horizontal";

	private const string kMultiplierBtn = "Debug Multiplier";

	private const string kResetBtn = "Debug Reset";

	private DebugActionDesc[] m_DebugActions;

	private DebugActionState[] m_DebugActionStates;

	private static readonly Lazy<DebugManager> s_Instance = new Lazy<DebugManager>(() => new DebugManager());

	private ReadOnlyCollection<DebugUI.Panel> m_ReadOnlyPanels;

	private readonly List<DebugUI.Panel> m_Panels = new List<DebugUI.Panel>();

	public bool refreshEditorRequested;

	private GameObject m_Root;

	private DebugUIHandlerCanvas m_RootUICanvas;

	private GameObject m_PersistentRoot;

	private DebugUIHandlerPersistentCanvas m_RootUIPersistentCanvas;

	private bool m_EditorOpen;

	public static DebugManager instance => s_Instance.Value;

	public ReadOnlyCollection<DebugUI.Panel> panels
	{
		get
		{
			if (m_ReadOnlyPanels == null)
			{
				UpdateReadOnlyCollection();
			}
			return m_ReadOnlyPanels;
		}
	}

	public bool displayEditorUI => m_EditorOpen;

	public bool displayRuntimeUI
	{
		get
		{
			if (m_Root != null)
			{
				return m_Root.activeInHierarchy;
			}
			return false;
		}
		set
		{
			if (value)
			{
				m_Root = Object.Instantiate(Resources.Load<Transform>("DebugUI Canvas")).gameObject;
				m_Root.name = "[Debug Canvas]";
				m_Root.transform.localPosition = Vector3.zero;
				m_RootUICanvas = m_Root.GetComponent<DebugUIHandlerCanvas>();
				m_Root.SetActive(value: true);
			}
			else
			{
				CoreUtils.Destroy(m_Root);
				m_Root = null;
				m_RootUICanvas = null;
			}
			this.onDisplayRuntimeUIChanged(value);
		}
	}

	public bool displayPersistentRuntimeUI
	{
		get
		{
			if (m_RootUIPersistentCanvas != null)
			{
				return m_PersistentRoot.activeInHierarchy;
			}
			return false;
		}
		set
		{
			CheckPersistentCanvas();
			m_PersistentRoot.SetActive(value);
		}
	}

	public event Action<bool> onDisplayRuntimeUIChanged = delegate
	{
	};

	public event Action onSetDirty = delegate
	{
	};

	private event Action resetData;

	private void RegisterActions()
	{
		m_DebugActions = new DebugActionDesc[9];
		m_DebugActionStates = new DebugActionState[9];
		DebugActionDesc debugActionDesc = new DebugActionDesc();
		debugActionDesc.buttonTriggerList.Add(new string[2] { "Enable Debug Button 1", "Enable Debug Button 2" });
		debugActionDesc.keyTriggerList.Add(new KeyCode[2]
		{
			KeyCode.LeftControl,
			KeyCode.Backspace
		});
		debugActionDesc.repeatMode = DebugActionRepeatMode.Never;
		AddAction(DebugAction.EnableDebugMenu, debugActionDesc);
		DebugActionDesc debugActionDesc2 = new DebugActionDesc();
		debugActionDesc2.buttonTriggerList.Add(new string[2] { "Debug Reset", "Enable Debug Button 2" });
		debugActionDesc2.keyTriggerList.Add(new KeyCode[2]
		{
			KeyCode.LeftAlt,
			KeyCode.Backspace
		});
		debugActionDesc2.repeatMode = DebugActionRepeatMode.Never;
		AddAction(DebugAction.ResetAll, debugActionDesc2);
		DebugActionDesc debugActionDesc3 = new DebugActionDesc();
		debugActionDesc3.buttonTriggerList.Add(new string[1] { "Debug Next" });
		debugActionDesc3.repeatMode = DebugActionRepeatMode.Never;
		AddAction(DebugAction.NextDebugPanel, debugActionDesc3);
		DebugActionDesc debugActionDesc4 = new DebugActionDesc();
		debugActionDesc4.buttonTriggerList.Add(new string[1] { "Debug Previous" });
		debugActionDesc4.repeatMode = DebugActionRepeatMode.Never;
		AddAction(DebugAction.PreviousDebugPanel, debugActionDesc4);
		DebugActionDesc debugActionDesc5 = new DebugActionDesc();
		debugActionDesc5.buttonTriggerList.Add(new string[1] { "Debug Validate" });
		debugActionDesc5.repeatMode = DebugActionRepeatMode.Never;
		AddAction(DebugAction.Action, debugActionDesc5);
		DebugActionDesc debugActionDesc6 = new DebugActionDesc();
		debugActionDesc6.buttonTriggerList.Add(new string[1] { "Debug Persistent" });
		debugActionDesc6.repeatMode = DebugActionRepeatMode.Never;
		AddAction(DebugAction.MakePersistent, debugActionDesc6);
		DebugActionDesc debugActionDesc7 = new DebugActionDesc();
		debugActionDesc7.buttonTriggerList.Add(new string[1] { "Debug Multiplier" });
		debugActionDesc7.repeatMode = DebugActionRepeatMode.Delay;
		debugActionDesc5.repeatDelay = 0f;
		AddAction(DebugAction.Multiplier, debugActionDesc7);
		AddAction(DebugAction.MoveVertical, new DebugActionDesc
		{
			axisTrigger = "Debug Vertical",
			repeatMode = DebugActionRepeatMode.Delay,
			repeatDelay = 0.16f
		});
		AddAction(DebugAction.MoveHorizontal, new DebugActionDesc
		{
			axisTrigger = "Debug Horizontal",
			repeatMode = DebugActionRepeatMode.Delay,
			repeatDelay = 0.16f
		});
	}

	private void AddAction(DebugAction action, DebugActionDesc desc)
	{
		m_DebugActions[(int)action] = desc;
		m_DebugActionStates[(int)action] = new DebugActionState();
	}

	private void SampleAction(int actionIndex)
	{
		DebugActionDesc debugActionDesc = m_DebugActions[actionIndex];
		DebugActionState debugActionState = m_DebugActionStates[actionIndex];
		if (debugActionState.runningAction)
		{
			return;
		}
		for (int i = 0; i < debugActionDesc.buttonTriggerList.Count; i++)
		{
			string[] array = debugActionDesc.buttonTriggerList[i];
			bool flag = true;
			string[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				flag = Input.GetButton(array2[j]);
				if (!flag)
				{
					break;
				}
			}
			if (flag)
			{
				debugActionState.TriggerWithButton(array, 1f);
				break;
			}
		}
		if (debugActionDesc.axisTrigger != "")
		{
			float axis = Input.GetAxis(debugActionDesc.axisTrigger);
			if (axis != 0f)
			{
				debugActionState.TriggerWithAxis(debugActionDesc.axisTrigger, axis);
			}
		}
		for (int k = 0; k < debugActionDesc.keyTriggerList.Count; k++)
		{
			bool flag2 = true;
			KeyCode[] array3 = debugActionDesc.keyTriggerList[k];
			KeyCode[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				flag2 = Input.GetKey(array4[j]);
				if (!flag2)
				{
					break;
				}
			}
			if (flag2)
			{
				debugActionState.TriggerWithKey(array3, 1f);
				break;
			}
		}
	}

	private void UpdateAction(int actionIndex)
	{
		DebugActionDesc desc = m_DebugActions[actionIndex];
		DebugActionState debugActionState = m_DebugActionStates[actionIndex];
		if (debugActionState.runningAction)
		{
			debugActionState.Update(desc);
		}
	}

	internal void UpdateActions()
	{
		for (int i = 0; i < m_DebugActions.Length; i++)
		{
			UpdateAction(i);
			SampleAction(i);
		}
	}

	internal float GetAction(DebugAction action)
	{
		return m_DebugActionStates[(int)action].actionState;
	}

	private void RegisterInputs()
	{
	}

	private void UpdateReadOnlyCollection()
	{
		m_Panels.Sort();
		m_ReadOnlyPanels = m_Panels.AsReadOnly();
	}

	public void ToggleEditorUI(bool open)
	{
		m_EditorOpen = open;
	}

	private DebugManager()
	{
		if (Debug.isDebugBuild)
		{
			RegisterInputs();
			RegisterActions();
		}
	}

	public void RefreshEditor()
	{
		refreshEditorRequested = true;
	}

	public void Reset()
	{
		this.resetData?.Invoke();
		ReDrawOnScreenDebug();
	}

	public void ReDrawOnScreenDebug()
	{
		if (displayRuntimeUI)
		{
			m_RootUICanvas?.ResetAllHierarchy();
		}
	}

	public void RegisterData(IDebugData data)
	{
		resetData += data.GetReset();
	}

	public void UnregisterData(IDebugData data)
	{
		resetData -= data.GetReset();
	}

	public int GetState()
	{
		int num = 17;
		foreach (DebugUI.Panel panel in m_Panels)
		{
			num = num * 23 + panel.GetHashCode();
		}
		return num;
	}

	internal void RegisterRootCanvas(DebugUIHandlerCanvas root)
	{
		m_Root = root.gameObject;
		m_RootUICanvas = root;
	}

	internal void ChangeSelection(DebugUIHandlerWidget widget, bool fromNext)
	{
		m_RootUICanvas.ChangeSelection(widget, fromNext);
	}

	private void CheckPersistentCanvas()
	{
		if (m_RootUIPersistentCanvas == null)
		{
			DebugUIHandlerPersistentCanvas debugUIHandlerPersistentCanvas = Object.FindObjectOfType<DebugUIHandlerPersistentCanvas>();
			if (debugUIHandlerPersistentCanvas == null)
			{
				m_PersistentRoot = Object.Instantiate(Resources.Load<Transform>("DebugUI Persistent Canvas")).gameObject;
				m_PersistentRoot.name = "[Debug Canvas - Persistent]";
				m_PersistentRoot.transform.localPosition = Vector3.zero;
			}
			else
			{
				m_PersistentRoot = debugUIHandlerPersistentCanvas.gameObject;
			}
			m_RootUIPersistentCanvas = m_PersistentRoot.GetComponent<DebugUIHandlerPersistentCanvas>();
		}
	}

	internal void TogglePersistent(DebugUI.Widget widget)
	{
		if (widget != null)
		{
			if (!(widget is DebugUI.Value widget2))
			{
				Debug.Log("Only DebugUI.Value items can be made persistent.");
				return;
			}
			CheckPersistentCanvas();
			m_RootUIPersistentCanvas.Toggle(widget2);
		}
	}

	private void OnPanelDirty(DebugUI.Panel panel)
	{
		this.onSetDirty();
	}

	public DebugUI.Panel GetPanel(string displayName, bool createIfNull = false, int groupIndex = 0, bool overrideIfExist = false)
	{
		DebugUI.Panel panel = null;
		foreach (DebugUI.Panel panel2 in m_Panels)
		{
			if (panel2.displayName == displayName)
			{
				panel = panel2;
				break;
			}
		}
		if (panel != null)
		{
			if (!overrideIfExist)
			{
				return panel;
			}
			panel.onSetDirty -= OnPanelDirty;
			RemovePanel(panel);
			panel = null;
		}
		if (createIfNull)
		{
			panel = new DebugUI.Panel
			{
				displayName = displayName,
				groupIndex = groupIndex
			};
			panel.onSetDirty += OnPanelDirty;
			m_Panels.Add(panel);
			UpdateReadOnlyCollection();
		}
		return panel;
	}

	public void RemovePanel(string displayName)
	{
		DebugUI.Panel panel = null;
		foreach (DebugUI.Panel panel2 in m_Panels)
		{
			if (panel2.displayName == displayName)
			{
				panel2.onSetDirty -= OnPanelDirty;
				panel = panel2;
				break;
			}
		}
		RemovePanel(panel);
	}

	public void RemovePanel(DebugUI.Panel panel)
	{
		if (panel != null)
		{
			m_Panels.Remove(panel);
			UpdateReadOnlyCollection();
		}
	}

	public DebugUI.Widget GetItem(string queryPath)
	{
		foreach (DebugUI.Panel panel in m_Panels)
		{
			DebugUI.Widget item = GetItem(queryPath, panel);
			if (item != null)
			{
				return item;
			}
		}
		return null;
	}

	private DebugUI.Widget GetItem(string queryPath, DebugUI.IContainer container)
	{
		foreach (DebugUI.Widget child in container.children)
		{
			if (child.queryPath == queryPath)
			{
				return child;
			}
			if (child is DebugUI.IContainer container2)
			{
				DebugUI.Widget item = GetItem(queryPath, container2);
				if (item != null)
				{
					return item;
				}
			}
		}
		return null;
	}
}
