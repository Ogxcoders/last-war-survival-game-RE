using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine;

[NativeHeader("Modules/IMGUI/GUILayoutUtility.bindings.h")]
public class GUILayoutUtility
{
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal sealed class LayoutCache
	{
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		internal GUILayoutGroup topLevel = new GUILayoutGroup();

		internal GenericStack layoutGroups = new GenericStack();

		internal GUILayoutGroup windows = new GUILayoutGroup();

		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		internal LayoutCache()
		{
			layoutGroups.Push(topLevel);
		}

		internal LayoutCache(LayoutCache other)
		{
			topLevel = other.topLevel;
			layoutGroups = other.layoutGroups;
			windows = other.windows;
		}
	}

	private static readonly Dictionary<int, LayoutCache> s_StoredLayouts = new Dictionary<int, LayoutCache>();

	private static readonly Dictionary<int, LayoutCache> s_StoredWindows = new Dictionary<int, LayoutCache>();

	internal static LayoutCache current = new LayoutCache();

	internal static readonly Rect kDummyRect = new Rect(0f, 0f, 1f, 1f);

	private static GUIStyle s_SpaceStyle;

	internal static GUILayoutGroup topLevel => current.topLevel;

	internal static GUIStyle spaceStyle
	{
		get
		{
			if (s_SpaceStyle == null)
			{
				s_SpaceStyle = new GUIStyle();
			}
			s_SpaceStyle.stretchWidth = false;
			return s_SpaceStyle;
		}
	}

	private static Rect Internal_GetWindowRect(int windowID)
	{
		Internal_GetWindowRect_Injected(windowID, out var ret);
		return ret;
	}

	private static void Internal_MoveWindow(int windowID, Rect r)
	{
		Internal_MoveWindow_Injected(windowID, ref r);
	}

	internal static Rect GetWindowsBounds()
	{
		GetWindowsBounds_Injected(out var ret);
		return ret;
	}

	internal static void CleanupRoots()
	{
		s_SpaceStyle = null;
		s_StoredLayouts.Clear();
		s_StoredWindows.Clear();
		current = new LayoutCache();
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static LayoutCache SelectIDList(int instanceID, bool isWindow)
	{
		Dictionary<int, LayoutCache> dictionary = (isWindow ? s_StoredWindows : s_StoredLayouts);
		if (!dictionary.TryGetValue(instanceID, out var value))
		{
			value = (dictionary[instanceID] = new LayoutCache());
		}
		current.topLevel = value.topLevel;
		current.layoutGroups = value.layoutGroups;
		current.windows = value.windows;
		return value;
	}

	internal static void Begin(int instanceID)
	{
		LayoutCache layoutCache = SelectIDList(instanceID, isWindow: false);
		if (Event.current.type == EventType.Layout)
		{
			current.topLevel = (layoutCache.topLevel = new GUILayoutGroup());
			current.layoutGroups.Clear();
			current.layoutGroups.Push(current.topLevel);
			current.windows = (layoutCache.windows = new GUILayoutGroup());
		}
		else
		{
			current.topLevel = layoutCache.topLevel;
			current.layoutGroups = layoutCache.layoutGroups;
			current.windows = layoutCache.windows;
		}
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void BeginContainer(LayoutCache cache)
	{
		if (Event.current.type == EventType.Layout)
		{
			cache.topLevel = new GUILayoutGroup();
			cache.layoutGroups.Clear();
			cache.layoutGroups.Push(cache.topLevel);
			cache.windows = new GUILayoutGroup();
		}
		current.topLevel = cache.topLevel;
		current.layoutGroups = cache.layoutGroups;
		current.windows = cache.windows;
	}

	internal static void BeginWindow(int windowID, GUIStyle style, GUILayoutOption[] options)
	{
		LayoutCache layoutCache = SelectIDList(windowID, isWindow: true);
		if (Event.current.type == EventType.Layout)
		{
			current.topLevel = (layoutCache.topLevel = new GUILayoutGroup());
			current.topLevel.style = style;
			current.topLevel.windowID = windowID;
			if (options != null)
			{
				current.topLevel.ApplyOptions(options);
			}
			current.layoutGroups.Clear();
			current.layoutGroups.Push(current.topLevel);
			current.windows = (layoutCache.windows = new GUILayoutGroup());
		}
		else
		{
			current.topLevel = layoutCache.topLevel;
			current.layoutGroups = layoutCache.layoutGroups;
			current.windows = layoutCache.windows;
		}
	}

	[Obsolete("BeginGroup has no effect and will be removed", false)]
	public static void BeginGroup(string GroupName)
	{
	}

	[Obsolete("EndGroup has no effect and will be removed", false)]
	public static void EndGroup(string groupName)
	{
	}

	internal static void Layout()
	{
		if (current.topLevel.windowID == -1)
		{
			current.topLevel.CalcWidth();
			current.topLevel.SetHorizontal(0f, Mathf.Min((float)Screen.width / GUIUtility.pixelsPerPoint, current.topLevel.maxWidth));
			current.topLevel.CalcHeight();
			current.topLevel.SetVertical(0f, Mathf.Min((float)Screen.height / GUIUtility.pixelsPerPoint, current.topLevel.maxHeight));
			LayoutFreeGroup(current.windows);
		}
		else
		{
			LayoutSingleGroup(current.topLevel);
			LayoutFreeGroup(current.windows);
		}
	}

	internal static void LayoutFromEditorWindow()
	{
		if (current.topLevel != null)
		{
			current.topLevel.CalcWidth();
			current.topLevel.SetHorizontal(0f, (float)Screen.width / GUIUtility.pixelsPerPoint);
			current.topLevel.CalcHeight();
			current.topLevel.SetVertical(0f, (float)Screen.height / GUIUtility.pixelsPerPoint);
			LayoutFreeGroup(current.windows);
		}
		else
		{
			Debug.LogError("GUILayout state invalid. Verify that all layout begin/end calls match.");
		}
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void LayoutFromContainer(float w, float h)
	{
		if (current.topLevel != null)
		{
			current.topLevel.CalcWidth();
			current.topLevel.SetHorizontal(0f, w);
			current.topLevel.CalcHeight();
			current.topLevel.SetVertical(0f, h);
			LayoutFreeGroup(current.windows);
		}
		else
		{
			Debug.LogError("GUILayout state invalid. Verify that all layout begin/end calls match.");
		}
	}

	internal static float LayoutFromInspector(float width)
	{
		if (current.topLevel != null && current.topLevel.windowID == -1)
		{
			current.topLevel.CalcWidth();
			current.topLevel.SetHorizontal(0f, width);
			current.topLevel.CalcHeight();
			current.topLevel.SetVertical(0f, Mathf.Min((float)Screen.height / GUIUtility.pixelsPerPoint, current.topLevel.maxHeight));
			float minHeight = current.topLevel.minHeight;
			LayoutFreeGroup(current.windows);
			return minHeight;
		}
		if (current.topLevel != null)
		{
			LayoutSingleGroup(current.topLevel);
		}
		return 0f;
	}

	internal static void LayoutFreeGroup(GUILayoutGroup toplevel)
	{
		foreach (GUILayoutGroup entry in toplevel.entries)
		{
			LayoutSingleGroup(entry);
		}
		toplevel.ResetCursor();
	}

	private static void LayoutSingleGroup(GUILayoutGroup i)
	{
		if (!i.isWindow)
		{
			float minWidth = i.minWidth;
			float maxWidth = i.maxWidth;
			i.CalcWidth();
			i.SetHorizontal(i.rect.x, Mathf.Clamp(i.maxWidth, minWidth, maxWidth));
			float minHeight = i.minHeight;
			float maxHeight = i.maxHeight;
			i.CalcHeight();
			i.SetVertical(i.rect.y, Mathf.Clamp(i.maxHeight, minHeight, maxHeight));
		}
		else
		{
			i.CalcWidth();
			Rect rect = Internal_GetWindowRect(i.windowID);
			i.SetHorizontal(rect.x, Mathf.Clamp(rect.width, i.minWidth, i.maxWidth));
			i.CalcHeight();
			i.SetVertical(rect.y, Mathf.Clamp(rect.height, i.minHeight, i.maxHeight));
			Internal_MoveWindow(i.windowID, i.rect);
		}
	}

	[SecuritySafeCritical]
	private static GUILayoutGroup CreateGUILayoutGroupInstanceOfType(Type LayoutType)
	{
		if (!typeof(GUILayoutGroup).IsAssignableFrom(LayoutType))
		{
			throw new ArgumentException("LayoutType needs to be of type GUILayoutGroup", "LayoutType");
		}
		return (GUILayoutGroup)Activator.CreateInstance(LayoutType);
	}

	internal static GUILayoutGroup BeginLayoutGroup(GUIStyle style, GUILayoutOption[] options, Type layoutType)
	{
		EventType type = Event.current.type;
		GUILayoutGroup gUILayoutGroup;
		if (type == EventType.Layout || type == EventType.Used)
		{
			gUILayoutGroup = CreateGUILayoutGroupInstanceOfType(layoutType);
			gUILayoutGroup.style = style;
			if (options != null)
			{
				gUILayoutGroup.ApplyOptions(options);
			}
			current.topLevel.Add(gUILayoutGroup);
		}
		else
		{
			gUILayoutGroup = current.topLevel.GetNext() as GUILayoutGroup;
			if (gUILayoutGroup == null)
			{
				throw new ExitGUIException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
			}
			gUILayoutGroup.ResetCursor();
		}
		current.layoutGroups.Push(gUILayoutGroup);
		current.topLevel = gUILayoutGroup;
		return gUILayoutGroup;
	}

	internal static void EndLayoutGroup()
	{
		if (current.layoutGroups.Count == 0)
		{
			Debug.LogError("EndLayoutGroup: BeginLayoutGroup must be called first.");
			return;
		}
		current.layoutGroups.Pop();
		if (0 < current.layoutGroups.Count)
		{
			current.topLevel = (GUILayoutGroup)current.layoutGroups.Peek();
		}
		else
		{
			current.topLevel = new GUILayoutGroup();
		}
	}

	internal static GUILayoutGroup BeginLayoutArea(GUIStyle style, Type layoutType)
	{
		EventType type = Event.current.type;
		GUILayoutGroup gUILayoutGroup;
		if (type == EventType.Layout || type == EventType.Used)
		{
			gUILayoutGroup = CreateGUILayoutGroupInstanceOfType(layoutType);
			gUILayoutGroup.style = style;
			current.windows.Add(gUILayoutGroup);
		}
		else
		{
			gUILayoutGroup = current.windows.GetNext() as GUILayoutGroup;
			if (gUILayoutGroup == null)
			{
				throw new ExitGUIException("GUILayout: Mismatched LayoutGroup." + Event.current.type);
			}
			gUILayoutGroup.ResetCursor();
		}
		current.layoutGroups.Push(gUILayoutGroup);
		current.topLevel = gUILayoutGroup;
		return gUILayoutGroup;
	}

	internal static GUILayoutGroup DoBeginLayoutArea(GUIStyle style, Type layoutType)
	{
		return BeginLayoutArea(style, layoutType);
	}

	public static Rect GetRect(GUIContent content, GUIStyle style)
	{
		return DoGetRect(content, style, null);
	}

	public static Rect GetRect(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetRect(content, style, options);
	}

	private static Rect DoGetRect(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		GUIUtility.CheckOnGUI();
		switch (Event.current.type)
		{
		case EventType.Layout:
			if (style.isHeightDependantOnWidth)
			{
				current.topLevel.Add(new GUIWordWrapSizer(style, content, options));
			}
			else
			{
				Vector2 constraints = new Vector2(0f, 0f);
				if (options != null)
				{
					foreach (GUILayoutOption gUILayoutOption in options)
					{
						switch (gUILayoutOption.type)
						{
						case GUILayoutOption.Type.maxHeight:
							constraints.y = (float)gUILayoutOption.value;
							break;
						case GUILayoutOption.Type.maxWidth:
							constraints.x = (float)gUILayoutOption.value;
							break;
						}
					}
				}
				Vector2 vector = style.CalcSizeWithConstraints(content, constraints);
				vector.x = Mathf.Ceil(vector.x);
				vector.y = Mathf.Ceil(vector.y);
				current.topLevel.Add(new GUILayoutEntry(vector.x, vector.x, vector.y, vector.y, style, options));
			}
			return kDummyRect;
		case EventType.Used:
			return kDummyRect;
		default:
		{
			GUILayoutEntry next = current.topLevel.GetNext();
			return next.rect;
		}
		}
	}

	public static Rect GetRect(float width, float height)
	{
		return DoGetRect(width, width, height, height, GUIStyle.none, null);
	}

	public static Rect GetRect(float width, float height, GUIStyle style)
	{
		return DoGetRect(width, width, height, height, style, null);
	}

	public static Rect GetRect(float width, float height, params GUILayoutOption[] options)
	{
		return DoGetRect(width, width, height, height, GUIStyle.none, options);
	}

	public static Rect GetRect(float width, float height, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetRect(width, width, height, height, style, options);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, null);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, null);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, params GUILayoutOption[] options)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, GUIStyle.none, options);
	}

	public static Rect GetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetRect(minWidth, maxWidth, minHeight, maxHeight, style, options);
	}

	private static Rect DoGetRect(float minWidth, float maxWidth, float minHeight, float maxHeight, GUIStyle style, GUILayoutOption[] options)
	{
		switch (Event.current.type)
		{
		case EventType.Layout:
			current.topLevel.Add(new GUILayoutEntry(minWidth, maxWidth, minHeight, maxHeight, style, options));
			return kDummyRect;
		case EventType.Used:
			return kDummyRect;
		default:
			return current.topLevel.GetNext().rect;
		}
	}

	public static Rect GetLastRect()
	{
		EventType type = Event.current.type;
		if (type == EventType.Layout || type == EventType.Used)
		{
			return kDummyRect;
		}
		return current.topLevel.GetLast();
	}

	public static Rect GetAspectRect(float aspect)
	{
		return DoGetAspectRect(aspect, null);
	}

	public static Rect GetAspectRect(float aspect, GUIStyle style)
	{
		return DoGetAspectRect(aspect, null);
	}

	public static Rect GetAspectRect(float aspect, params GUILayoutOption[] options)
	{
		return DoGetAspectRect(aspect, options);
	}

	public static Rect GetAspectRect(float aspect, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoGetAspectRect(aspect, options);
	}

	private static Rect DoGetAspectRect(float aspect, GUILayoutOption[] options)
	{
		switch (Event.current.type)
		{
		case EventType.Layout:
			current.topLevel.Add(new GUIAspectSizer(aspect, options));
			return kDummyRect;
		case EventType.Used:
			return kDummyRect;
		default:
			return current.topLevel.GetNext().rect;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_GetWindowRect_Injected(int windowID, out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_MoveWindow_Injected(int windowID, ref Rect r);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetWindowsBounds_Injected(out Rect ret);
}
