#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements;

internal static class UIElementsUtility
{
	private static Stack<IMGUIContainer> s_ContainerStack;

	private static Dictionary<int, Panel> s_UIElementsCache;

	private static Event s_EventInstance;

	internal static Color editorPlayModeTintColor;

	internal static readonly string s_RepaintProfilerMarkerName;

	internal static readonly string s_EventProfilerMarkerName;

	private static readonly ProfilerMarker s_RepaintProfilerMarker;

	private static readonly ProfilerMarker s_EventProfilerMarker;

	static UIElementsUtility()
	{
		s_ContainerStack = new Stack<IMGUIContainer>();
		s_UIElementsCache = new Dictionary<int, Panel>();
		s_EventInstance = new Event();
		editorPlayModeTintColor = Color.white;
		s_RepaintProfilerMarkerName = "UIElementsUtility.DoDispatch(Repaint Event)";
		s_EventProfilerMarkerName = "UIElementsUtility.DoDispatch(Non Repaint Event)";
		s_RepaintProfilerMarker = new ProfilerMarker(s_RepaintProfilerMarkerName);
		s_EventProfilerMarker = new ProfilerMarker(s_EventProfilerMarkerName);
		GUIUtility.takeCapture = (Action)Delegate.Combine(GUIUtility.takeCapture, new Action(TakeCapture));
		GUIUtility.releaseCapture = (Action)Delegate.Combine(GUIUtility.releaseCapture, new Action(ReleaseCapture));
		GUIUtility.processEvent = (Func<int, IntPtr, bool>)Delegate.Combine(GUIUtility.processEvent, new Func<int, IntPtr, bool>(ProcessEvent));
		GUIUtility.cleanupRoots = (Action)Delegate.Combine(GUIUtility.cleanupRoots, new Action(CleanupRoots));
		GUIUtility.endContainerGUIFromException = (Func<Exception, bool>)Delegate.Combine(GUIUtility.endContainerGUIFromException, new Func<Exception, bool>(EndContainerGUIFromException));
		GUIUtility.guiChanged = (Action)Delegate.Combine(GUIUtility.guiChanged, new Action(MakeCurrentIMGUIContainerDirty));
	}

	internal static IMGUIContainer GetCurrentIMGUIContainer()
	{
		if (s_ContainerStack.Count > 0)
		{
			return s_ContainerStack.Peek();
		}
		return null;
	}

	internal static void MakeCurrentIMGUIContainerDirty()
	{
		if (s_ContainerStack.Count > 0)
		{
			s_ContainerStack.Peek().MarkDirtyLayout();
		}
	}

	private static void TakeCapture()
	{
		if (s_ContainerStack.Count > 0)
		{
			IMGUIContainer iMGUIContainer = s_ContainerStack.Peek();
			IEventHandler capturingElement = iMGUIContainer.panel.GetCapturingElement(PointerId.mousePointerId);
			if (capturingElement != null && capturingElement != iMGUIContainer)
			{
				Debug.Log("Should not grab hot control with an active capture");
			}
			iMGUIContainer.CaptureMouse();
		}
	}

	private static void ReleaseCapture()
	{
	}

	private static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
	{
		if (nativeEventPtr != IntPtr.Zero && s_UIElementsCache.TryGetValue(instanceID, out var value) && value.contextType == ContextType.Editor)
		{
			s_EventInstance.CopyFromPtr(nativeEventPtr);
			return DoDispatch(value);
		}
		return false;
	}

	public static void RegisterCachedPanel(int instanceID, Panel panel)
	{
		s_UIElementsCache.Add(instanceID, panel);
	}

	public static void RemoveCachedPanel(int instanceID)
	{
		s_UIElementsCache.Remove(instanceID);
	}

	public static bool TryGetPanel(int instanceID, out Panel panel)
	{
		return s_UIElementsCache.TryGetValue(instanceID, out panel);
	}

	private static void CleanupRoots()
	{
		s_EventInstance = null;
		s_UIElementsCache = null;
		s_ContainerStack = null;
	}

	private static bool EndContainerGUIFromException(Exception exception)
	{
		if (s_ContainerStack.Count > 0)
		{
			GUIUtility.EndContainer();
			s_ContainerStack.Pop();
		}
		return GUIUtility.ShouldRethrowException(exception);
	}

	internal static void BeginContainerGUI(GUILayoutUtility.LayoutCache cache, Event evt, IMGUIContainer container)
	{
		if (container.useOwnerObjectGUIState)
		{
			GUIUtility.BeginContainerFromOwner(container.elementPanel.ownerObject);
		}
		else
		{
			GUIUtility.BeginContainer(container.guiState);
		}
		s_ContainerStack.Push(container);
		GUIUtility.s_SkinMode = (int)container.contextType;
		GUIUtility.s_OriginalID = container.elementPanel.ownerObject.GetInstanceID();
		if (Event.current == null)
		{
			Event.current = evt;
		}
		else
		{
			Event.current.CopyFrom(evt);
		}
		GUI.enabled = container.enabledInHierarchy;
		GUILayoutUtility.BeginContainer(cache);
		GUIUtility.ResetGlobalState();
	}

	internal static void EndContainerGUI(Event evt, Rect layoutSize)
	{
		if (Event.current.type == EventType.Layout && s_ContainerStack.Count > 0)
		{
			GUILayoutUtility.LayoutFromContainer(layoutSize.width, layoutSize.height);
		}
		GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, isWindow: false);
		GUIContent.ClearStaticCache();
		if (s_ContainerStack.Count > 0)
		{
		}
		evt.CopyFrom(Event.current);
		if (s_ContainerStack.Count > 0)
		{
			GUIUtility.EndContainer();
			s_ContainerStack.Pop();
		}
	}

	internal static EventBase CreateEvent(Event systemEvent)
	{
		return CreateEvent(systemEvent, systemEvent.rawType);
	}

	internal static EventBase CreateEvent(Event systemEvent, EventType eventType)
	{
		switch (eventType)
		{
		case EventType.MouseMove:
			return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
		case EventType.MouseDrag:
			return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
		case EventType.MouseDown:
			if (PointerDeviceState.GetPressedButtons(PointerId.mousePointerId) != 0)
			{
				return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
			}
			return PointerEventBase<PointerDownEvent>.GetPooled(systemEvent);
		case EventType.MouseUp:
			if (PointerDeviceState.HasAdditionalPressedButtons(PointerId.mousePointerId, systemEvent.button))
			{
				return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
			}
			return PointerEventBase<PointerUpEvent>.GetPooled(systemEvent);
		case EventType.ContextClick:
			return MouseEventBase<ContextClickEvent>.GetPooled(systemEvent);
		case EventType.MouseEnterWindow:
			return MouseEventBase<MouseEnterWindowEvent>.GetPooled(systemEvent);
		case EventType.MouseLeaveWindow:
			return MouseLeaveWindowEvent.GetPooled(systemEvent);
		case EventType.ScrollWheel:
			return WheelEvent.GetPooled(systemEvent);
		case EventType.KeyDown:
			return KeyboardEventBase<KeyDownEvent>.GetPooled(systemEvent);
		case EventType.KeyUp:
			return KeyboardEventBase<KeyUpEvent>.GetPooled(systemEvent);
		case EventType.DragUpdated:
			return DragUpdatedEvent.GetPooled(systemEvent);
		case EventType.DragPerform:
			return MouseEventBase<DragPerformEvent>.GetPooled(systemEvent);
		case EventType.DragExited:
			return DragExitedEvent.GetPooled(systemEvent);
		case EventType.ValidateCommand:
			return CommandEventBase<ValidateCommandEvent>.GetPooled(systemEvent);
		case EventType.ExecuteCommand:
			return CommandEventBase<ExecuteCommandEvent>.GetPooled(systemEvent);
		default:
			return IMGUIEvent.GetPooled(systemEvent);
		}
	}

	private static bool DoDispatch(BaseVisualElementPanel panel)
	{
		bool result = false;
		if (s_EventInstance.type == EventType.Repaint)
		{
			using (s_RepaintProfilerMarker.Auto())
			{
				panel.Repaint(s_EventInstance);
			}
			result = panel.IMGUIContainersCount > 0;
		}
		else
		{
			panel.ValidateLayout();
			using EventBase eventBase = CreateEvent(s_EventInstance);
			bool flag = s_EventInstance.type == EventType.Used || s_EventInstance.type == EventType.Layout || s_EventInstance.type == EventType.ExecuteCommand || s_EventInstance.type == EventType.ValidateCommand;
			using (s_EventProfilerMarker.Auto())
			{
				panel.SendEvent(eventBase, (!flag) ? DispatchMode.Default : DispatchMode.Immediate);
			}
			if (eventBase.isPropagationStopped)
			{
				panel.visualTree.IncrementVersion(VersionChangeType.Repaint);
				result = true;
			}
		}
		return result;
	}

	internal static void GetAllPanels(List<Panel> panels, ContextType contextType)
	{
		panels.Clear();
		Dictionary<int, Panel>.Enumerator panelsIterator = GetPanelsIterator();
		while (panelsIterator.MoveNext())
		{
			if (panelsIterator.Current.Value.contextType == contextType)
			{
				panels.Add(panelsIterator.Current.Value);
			}
		}
	}

	internal static Dictionary<int, Panel>.Enumerator GetPanelsIterator()
	{
		return s_UIElementsCache.GetEnumerator();
	}

	internal static Panel FindOrCreateEditorPanel(ScriptableObject ownerObject)
	{
		if (!s_UIElementsCache.TryGetValue(ownerObject.GetInstanceID(), out var value))
		{
			value = Panel.CreateEditorPanel(ownerObject);
			RegisterCachedPanel(ownerObject.GetInstanceID(), value);
		}
		else
		{
			Debug.Assert(ContextType.Editor == value.contextType, "Panel is not an editor panel.");
		}
		return value;
	}
}
