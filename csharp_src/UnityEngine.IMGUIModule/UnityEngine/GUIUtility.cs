using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/IMGUI/GUIUtility.h")]
[NativeHeader("Modules/IMGUI/GUIManager.h")]
[NativeHeader("Runtime/Input/InputBindings.h")]
[NativeHeader("Runtime/Camera/RenderLayers/GUITexture.h")]
[NativeHeader("Runtime/Utilities/CopyPaste.h")]
[NativeHeader("Runtime/Input/InputManager.h")]
public class GUIUtility
{
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static int s_SkinMode;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static int s_OriginalID;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Action takeCapture;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Action releaseCapture;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Func<int, IntPtr, bool> processEvent;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Action cleanupRoots;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Func<Exception, bool> endContainerGUIFromException;

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Action guiChanged;

	internal static Func<bool> s_HasCurrentWindowKeyFocusFunc;

	public static extern bool hasModalWindow
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("GetGUIState().m_PixelsPerPoint", true, TargetType.Field)]
	internal static extern float pixelsPerPoint
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		get;
	}

	[NativeProperty("GetGUIState().m_OnGUIDepth", true, TargetType.Field)]
	internal static extern int guiDepth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		get;
	}

	internal static Vector2 s_EditorScreenPointOffset
	{
		[NativeMethod("GetGUIManager().GetGUIPixelOffset", true)]
		get
		{
			get_s_EditorScreenPointOffset_Injected(out var ret);
			return ret;
		}
		[NativeMethod("GetGUIManager().SetGUIPixelOffset", true)]
		set
		{
			set_s_EditorScreenPointOffset_Injected(ref value);
		}
	}

	[NativeProperty("GetGUIState().m_CanvasGUIState.m_IsMouseUsed", true, TargetType.Field)]
	internal static extern bool mouseUsed
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[StaticAccessor("GetInputManager()", StaticAccessorType.Dot)]
	internal static extern bool textFieldInput
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	internal static extern bool manualTex2SRGBEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		[FreeFunction("GUITexture::IsManualTex2SRGBEnabled")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("GUITexture::SetManualTex2SRGBEnabled")]
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		set;
	}

	public static extern string systemCopyBuffer
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("GetCopyBuffer")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("SetCopyBuffer")]
		set;
	}

	[StaticAccessor("InputBindings", StaticAccessorType.DoubleColon)]
	internal static extern string compositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		get;
	}

	[StaticAccessor("InputBindings", StaticAccessorType.DoubleColon)]
	internal static Vector2 compositionCursorPos
	{
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		get
		{
			get_compositionCursorPos_Injected(out var ret);
			return ret;
		}
		[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
		set
		{
			set_compositionCursorPos_Injected(ref value);
		}
	}

	internal static bool guiIsExiting { get; set; }

	public static int hotControl
	{
		get
		{
			return Internal_GetHotControl();
		}
		set
		{
			Internal_SetHotControl(value);
		}
	}

	public static int keyboardControl
	{
		get
		{
			return Internal_GetKeyboardControl();
		}
		set
		{
			Internal_SetKeyboardControl(value);
		}
	}

	[StaticAccessor("GetGUIState()", StaticAccessorType.Dot)]
	public static int GetControlID(int hint, FocusType focusType, Rect rect)
	{
		return GetControlID_Injected(hint, focusType, ref rect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void BeginContainerFromOwner(ScriptableObject owner);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void BeginContainer(ObjectGUIState objectGUIState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("EndContainer")]
	internal static extern void Internal_EndContainer();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GetSpecificGUIState(0).m_EternalGUIState->GetNextUniqueID")]
	internal static extern int GetPermanentControlID();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern int CheckForTabEvent(Event evt);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void SetKeyboardControlToFirstControlId();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern void SetKeyboardControlToLastControlId();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern bool HasFocusableControls();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static extern bool OwnsId(int id);

	public static Rect AlignRectToDevice(Rect rect, out int widthInPixels, out int heightInPixels)
	{
		AlignRectToDevice_Injected(ref rect, out widthInPixels, out heightInPixels, out var ret);
		return ret;
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static Vector3 Internal_MultiplyPoint(Vector3 point, Matrix4x4 transform)
	{
		Internal_MultiplyPoint_Injected(ref point, ref transform, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetChanged();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetChanged(bool changed);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetDidGUIWindowsEatLastEvent(bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int Internal_GetHotControl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int Internal_GetKeyboardControl();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_SetHotControl(int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_SetKeyboardControl(int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern object Internal_GetDefaultSkin(int skinMode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Object Internal_GetBuiltinSkin(int skin);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_ExitGUI();

	private static Vector2 InternalWindowToScreenPoint(Vector2 windowPoint)
	{
		InternalWindowToScreenPoint_Injected(ref windowPoint, out var ret);
		return ret;
	}

	private static Vector2 InternalScreenToWindowPoint(Vector2 screenPoint)
	{
		InternalScreenToWindowPoint_Injected(ref screenPoint, out var ret);
		return ret;
	}

	[RequiredByNativeCode]
	private static void MarkGUIChanged()
	{
		guiChanged?.Invoke();
	}

	public static int GetControlID(FocusType focus)
	{
		return GetControlID(0, focus);
	}

	public static int GetControlID(GUIContent contents, FocusType focus)
	{
		return GetControlID(contents.hash, focus);
	}

	public static int GetControlID(FocusType focus, Rect position)
	{
		return GetControlID(0, focus, position);
	}

	public static int GetControlID(GUIContent contents, FocusType focus, Rect position)
	{
		return GetControlID(contents.hash, focus, position);
	}

	public static int GetControlID(int hint, FocusType focus)
	{
		return GetControlID(hint, focus, Rect.zero);
	}

	public static object GetStateObject(Type t, int controlID)
	{
		return GUIStateObjects.GetStateObject(t, controlID);
	}

	public static object QueryStateObject(Type t, int controlID)
	{
		return GUIStateObjects.QueryStateObject(t, controlID);
	}

	[RequiredByNativeCode]
	internal static void TakeCapture()
	{
		takeCapture?.Invoke();
	}

	[RequiredByNativeCode]
	internal static void RemoveCapture()
	{
		releaseCapture?.Invoke();
	}

	internal static bool HasKeyFocus(int controlID)
	{
		return controlID == keyboardControl && (s_HasCurrentWindowKeyFocusFunc == null || s_HasCurrentWindowKeyFocusFunc());
	}

	public static void ExitGUI()
	{
		throw new ExitGUIException();
	}

	internal static GUISkin GetDefaultSkin(int skinMode)
	{
		return Internal_GetDefaultSkin(skinMode) as GUISkin;
	}

	internal static GUISkin GetDefaultSkin()
	{
		return Internal_GetDefaultSkin(s_SkinMode) as GUISkin;
	}

	internal static GUISkin GetBuiltinSkin(int skin)
	{
		return Internal_GetBuiltinSkin(skin) as GUISkin;
	}

	[RequiredByNativeCode]
	internal static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
	{
		if (processEvent != null)
		{
			return processEvent(instanceID, nativeEventPtr);
		}
		return false;
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void EndContainer()
	{
		Internal_EndContainer();
		Internal_ExitGUI();
	}

	internal static void CleanupRoots()
	{
		cleanupRoots?.Invoke();
	}

	[RequiredByNativeCode]
	internal static void BeginGUI(int skinMode, int instanceID, int useGUILayout)
	{
		s_SkinMode = skinMode;
		s_OriginalID = instanceID;
		ResetGlobalState();
		if (useGUILayout != 0)
		{
			GUILayoutUtility.Begin(instanceID);
		}
	}

	[RequiredByNativeCode]
	internal static void EndGUI(int layoutType)
	{
		try
		{
			if (Event.current.type == EventType.Layout)
			{
				switch (layoutType)
				{
				case 1:
					GUILayoutUtility.Layout();
					break;
				case 2:
					GUILayoutUtility.LayoutFromEditorWindow();
					break;
				}
			}
			GUILayoutUtility.SelectIDList(s_OriginalID, isWindow: false);
			GUIContent.ClearStaticCache();
		}
		finally
		{
			Internal_ExitGUI();
		}
	}

	[RequiredByNativeCode]
	internal static bool EndGUIFromException(Exception exception)
	{
		Internal_ExitGUI();
		return ShouldRethrowException(exception);
	}

	[RequiredByNativeCode]
	internal static bool EndContainerGUIFromException(Exception exception)
	{
		if (endContainerGUIFromException != null)
		{
			return endContainerGUIFromException(exception);
		}
		return false;
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static void ResetGlobalState()
	{
		GUI.skin = null;
		guiIsExiting = false;
		GUI.changed = false;
		GUI.scrollViewStates.Clear();
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static bool IsExitGUIException(Exception exception)
	{
		while (exception is TargetInvocationException && exception.InnerException != null)
		{
			exception = exception.InnerException;
		}
		return exception is ExitGUIException;
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static bool ShouldRethrowException(Exception exception)
	{
		return IsExitGUIException(exception);
	}

	internal static void CheckOnGUI()
	{
		if (guiDepth <= 0)
		{
			throw new ArgumentException("You can only call GUI functions from inside OnGUI.");
		}
	}

	[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
	internal static float RoundToPixelGrid(float v)
	{
		return Mathf.Floor(v * pixelsPerPoint + 0.48f) / pixelsPerPoint;
	}

	public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
	{
		return InternalWindowToScreenPoint(GUIClip.UnclipToWindow(guiPoint));
	}

	public static Rect GUIToScreenRect(Rect guiRect)
	{
		Vector2 vector = GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
		guiRect.x = vector.x;
		guiRect.y = vector.y;
		return guiRect;
	}

	public static Vector2 ScreenToGUIPoint(Vector2 screenPoint)
	{
		return GUIClip.ClipToWindow(InternalScreenToWindowPoint(screenPoint));
	}

	public static Rect ScreenToGUIRect(Rect screenRect)
	{
		Vector2 vector = ScreenToGUIPoint(new Vector2(screenRect.x, screenRect.y));
		screenRect.x = vector.x;
		screenRect.y = vector.y;
		return screenRect;
	}

	public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
	{
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.identity;
		Vector2 vector = GUIClip.Unclip(pivotPoint);
		Matrix4x4 matrix4x = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, angle), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
		GUI.matrix = matrix4x * matrix;
	}

	public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
	{
		Matrix4x4 matrix = GUI.matrix;
		Vector2 vector = GUIClip.Unclip(pivotPoint);
		Matrix4x4 matrix4x = Matrix4x4.TRS(vector, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
		GUI.matrix = matrix4x * matrix;
	}

	public static Rect AlignRectToDevice(Rect rect)
	{
		int widthInPixels;
		int heightInPixels;
		return AlignRectToDevice(rect, out widthInPixels, out heightInPixels);
	}

	internal static bool HitTest(Rect rect, Vector2 point, int offset)
	{
		return point.x >= rect.xMin - (float)offset && point.x < rect.xMax + (float)offset && point.y >= rect.yMin - (float)offset && point.y < rect.yMax + (float)offset;
	}

	internal static bool HitTest(Rect rect, Vector2 point, bool isDirectManipulationDevice)
	{
		int offset = 0;
		return HitTest(rect, point, offset);
	}

	internal static bool HitTest(Rect rect, Event evt)
	{
		return HitTest(rect, evt.mousePosition, evt.isDirectManipulationDevice);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_s_EditorScreenPointOffset_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_s_EditorScreenPointOffset_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetControlID_Injected(int hint, FocusType focusType, ref Rect rect);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void AlignRectToDevice_Injected(ref Rect rect, out int widthInPixels, out int heightInPixels, out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void get_compositionCursorPos_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private static extern void set_compositionCursorPos_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_MultiplyPoint_Injected(ref Vector3 point, ref Matrix4x4 transform, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalWindowToScreenPoint_Injected(ref Vector2 windowPoint, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalScreenToWindowPoint_Injected(ref Vector2 screenPoint, out Vector2 ret);
}
