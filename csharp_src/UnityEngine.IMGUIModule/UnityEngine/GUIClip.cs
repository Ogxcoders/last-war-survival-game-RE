using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
[NativeHeader("Modules/IMGUI/GUIState.h")]
[NativeHeader("Modules/IMGUI/GUIClip.h")]
internal sealed class GUIClip
{
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal struct ParentClipScope : IDisposable
	{
		private bool m_Disposed;

		public ParentClipScope(Matrix4x4 objectTransform, Rect clipRect)
		{
			m_Disposed = false;
			Internal_PushParentClip(objectTransform, clipRect);
		}

		public void Dispose()
		{
			if (!m_Disposed)
			{
				m_Disposed = true;
				Internal_PopParentClip();
			}
		}
	}

	internal static extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetEnabled")]
		get;
	}

	internal static Rect visibleRect
	{
		[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetVisibleRect")]
		get
		{
			get_visibleRect_Injected(out var ret);
			return ret;
		}
	}

	internal static Rect topmostRect
	{
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetTopMostPhysicalRect")]
		get
		{
			get_topmostRect_Injected(out var ret);
			return ret;
		}
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void Internal_Push(Rect screenRect, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
	{
		Internal_Push_Injected(ref screenRect, ref scrollOffset, ref renderOffset, resetOffset);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void Internal_Pop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetCount")]
	internal static extern int Internal_GetCount();

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetTopRect")]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Rect GetTopRect()
	{
		GetTopRect_Injected(out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.Unclip")]
	private static Vector2 Unclip_Vector2(Vector2 pos)
	{
		Unclip_Vector2_Injected(ref pos, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.Unclip")]
	private static Rect Unclip_Rect(Rect rect)
	{
		Unclip_Rect_Injected(ref rect, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.Clip")]
	private static Vector2 Clip_Vector2(Vector2 absolutePos)
	{
		Clip_Vector2_Injected(ref absolutePos, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.Clip")]
	private static Rect Internal_Clip_Rect(Rect absoluteRect)
	{
		Internal_Clip_Rect_Injected(ref absoluteRect, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.UnclipToWindow")]
	private static Vector2 UnclipToWindow_Vector2(Vector2 pos)
	{
		UnclipToWindow_Vector2_Injected(ref pos, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.UnclipToWindow")]
	private static Rect UnclipToWindow_Rect(Rect rect)
	{
		UnclipToWindow_Rect_Injected(ref rect, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.ClipToWindow")]
	private static Vector2 ClipToWindow_Vector2(Vector2 absolutePos)
	{
		ClipToWindow_Vector2_Injected(ref absolutePos, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.ClipToWindow")]
	private static Rect ClipToWindow_Rect(Rect absoluteRect)
	{
		ClipToWindow_Rect_Injected(ref absoluteRect, out var ret);
		return ret;
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetAbsoluteMousePosition")]
	private static Vector2 Internal_GetAbsoluteMousePosition()
	{
		Internal_GetAbsoluteMousePosition_Injected(out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void Reapply();

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetUserMatrix")]
	internal static Matrix4x4 GetMatrix()
	{
		GetMatrix_Injected(out var ret);
		return ret;
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void SetMatrix(Matrix4x4 m)
	{
		SetMatrix_Injected(ref m);
	}

	[FreeFunction("GetGUIState().m_CanvasGUIState.m_GUIClipState.GetParentTransform")]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Matrix4x4 GetParentMatrix()
	{
		GetParentMatrix_Injected(out var ret);
		return ret;
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void Internal_PushParentClip(Matrix4x4 objectTransform, Rect clipRect)
	{
		Internal_PushParentClip_Injected(ref objectTransform, ref clipRect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void Internal_PopParentClip();

	internal static void Push(Rect screenRect, Vector2 scrollOffset, Vector2 renderOffset, bool resetOffset)
	{
		Internal_Push(screenRect, scrollOffset, renderOffset, resetOffset);
	}

	internal static void Pop()
	{
		Internal_Pop();
	}

	public static Vector2 Unclip(Vector2 pos)
	{
		return Unclip_Vector2(pos);
	}

	public static Rect Unclip(Rect rect)
	{
		return Unclip_Rect(rect);
	}

	public static Vector2 Clip(Vector2 absolutePos)
	{
		return Clip_Vector2(absolutePos);
	}

	public static Rect Clip(Rect absoluteRect)
	{
		return Internal_Clip_Rect(absoluteRect);
	}

	public static Vector2 UnclipToWindow(Vector2 pos)
	{
		return UnclipToWindow_Vector2(pos);
	}

	public static Rect UnclipToWindow(Rect rect)
	{
		return UnclipToWindow_Rect(rect);
	}

	public static Vector2 ClipToWindow(Vector2 absolutePos)
	{
		return ClipToWindow_Vector2(absolutePos);
	}

	public static Rect ClipToWindow(Rect absoluteRect)
	{
		return ClipToWindow_Rect(absoluteRect);
	}

	public static Vector2 GetAbsoluteMousePosition()
	{
		return Internal_GetAbsoluteMousePosition();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_visibleRect_Injected(out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_topmostRect_Injected(out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_Push_Injected(ref Rect screenRect, ref Vector2 scrollOffset, ref Vector2 renderOffset, bool resetOffset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetTopRect_Injected(out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Unclip_Vector2_Injected(ref Vector2 pos, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Unclip_Rect_Injected(ref Rect rect, out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Clip_Vector2_Injected(ref Vector2 absolutePos, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_Clip_Rect_Injected(ref Rect absoluteRect, out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UnclipToWindow_Vector2_Injected(ref Vector2 pos, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UnclipToWindow_Rect_Injected(ref Rect rect, out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ClipToWindow_Vector2_Injected(ref Vector2 absolutePos, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ClipToWindow_Rect_Injected(ref Rect absoluteRect, out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_GetAbsoluteMousePosition_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetMatrix_Injected(out Matrix4x4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetMatrix_Injected(ref Matrix4x4 m);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetParentMatrix_Injected(out Matrix4x4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_PushParentClip_Injected(ref Matrix4x4 objectTransform, ref Rect clipRect);
}
