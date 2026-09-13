#define UNITY_ASSERTIONS
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.UIElements;

[NativeHeader("Modules/UIElements/UIElementsRuntimeUtility.h")]
internal static class UIElementsRuntimeUtility
{
	private static EventDispatcher s_RuntimeDispatcher = new EventDispatcher();

	private static bool s_RegisteredPlayerloopCallback = false;

	private static List<Panel> panelsIteration = new List<Panel>();

	internal static readonly string s_RepaintProfilerMarkerName = "UIElementsRuntimeUtility.DoDispatch(Repaint Event)";

	private static readonly ProfilerMarker s_RepaintProfilerMarker = new ProfilerMarker(s_RepaintProfilerMarkerName);

	public static EventBase CreateEvent(Event systemEvent)
	{
		Debug.Assert(s_RuntimeDispatcher != null, "Call UIElementsRuntimeUtility.InitRuntimeEventSystem before sending any event.");
		return UIElementsUtility.CreateEvent(systemEvent, systemEvent.rawType);
	}

	public static IPanel CreateRuntimePanel(ScriptableObject ownerObject)
	{
		return FindOrCreateRuntimePanel(ownerObject);
	}

	public static IPanel FindOrCreateRuntimePanel(ScriptableObject ownerObject)
	{
		if (!UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out var panel))
		{
			panel = new RuntimePanel(ownerObject, s_RuntimeDispatcher)
			{
				IMGUIEventInterests = new EventInterests
				{
					wantsMouseMove = true,
					wantsMouseEnterLeaveWindow = true
				}
			};
			RegisterCachedPanelInternal(ownerObject.GetInstanceID(), panel);
		}
		else
		{
			Debug.Assert(panel.contextType == ContextType.Player, "Panel is not a runtime panel.");
		}
		return panel;
	}

	public static void DisposeRuntimePanel(ScriptableObject ownerObject)
	{
		if (UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out var panel))
		{
			panel.Dispose();
			RemoveCachedPanelInternal(ownerObject.GetInstanceID());
		}
	}

	public static void RegisterCachedPanel(int instanceID, IPanel panel)
	{
		RegisterCachedPanelInternal(instanceID, panel);
	}

	private static void RegisterCachedPanelInternal(int instanceID, IPanel panel)
	{
		UIElementsUtility.RegisterCachedPanel(instanceID, panel as Panel);
		if (!s_RegisteredPlayerloopCallback)
		{
			s_RegisteredPlayerloopCallback = true;
			RegisterPlayerloopCallback();
		}
	}

	public static void RemoveCachedPanel(int instanceID)
	{
		RemoveCachedPanelInternal(instanceID);
	}

	private static void RemoveCachedPanelInternal(int instanceID)
	{
		UIElementsUtility.RemoveCachedPanel(instanceID);
		UIElementsUtility.GetAllPanels(panelsIteration, ContextType.Player);
		if (panelsIteration.Count == 0)
		{
			s_RegisteredPlayerloopCallback = false;
			UnregisterPlayerloopCallback();
		}
	}

	[RequiredByNativeCode]
	public static void RepaintOverlayPanels()
	{
		UIElementsUtility.GetAllPanels(panelsIteration, ContextType.Player);
		foreach (Panel item in panelsIteration)
		{
			if ((item as RuntimePanel).targetTexture == null)
			{
				using (s_RepaintProfilerMarker.Auto())
				{
					item.Repaint(Event.current);
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void RegisterPlayerloopCallback();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void UnregisterPlayerloopCallback();
}
