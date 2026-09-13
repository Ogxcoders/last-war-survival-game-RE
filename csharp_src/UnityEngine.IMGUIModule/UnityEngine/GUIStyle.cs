using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[NativeHeader("IMGUIScriptingClasses.h")]
[NativeHeader("Modules/IMGUI/GUIStyle.bindings.h")]
[RequiredByNativeCode]
public sealed class GUIStyle
{
	[NonSerialized]
	internal IntPtr m_Ptr;

	[NonSerialized]
	private GUIStyleState m_Normal;

	[NonSerialized]
	private GUIStyleState m_Hover;

	[NonSerialized]
	private GUIStyleState m_Active;

	[NonSerialized]
	private GUIStyleState m_Focused;

	[NonSerialized]
	private GUIStyleState m_OnNormal;

	[NonSerialized]
	private GUIStyleState m_OnHover;

	[NonSerialized]
	private GUIStyleState m_OnActive;

	[NonSerialized]
	private GUIStyleState m_OnFocused;

	[NonSerialized]
	private RectOffset m_Border;

	[NonSerialized]
	private RectOffset m_Padding;

	[NonSerialized]
	private RectOffset m_Margin;

	[NonSerialized]
	private RectOffset m_Overflow;

	[NonSerialized]
	private string m_Name;

	internal static bool showKeyboardFocus = true;

	private static GUIStyle s_None;

	[NativeProperty("Name", false, TargetType.Function)]
	internal extern string rawName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("Font", false, TargetType.Function)]
	public extern Font font
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_ImagePosition", false, TargetType.Field)]
	public extern ImagePosition imagePosition
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_Alignment", false, TargetType.Field)]
	public extern TextAnchor alignment
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_WordWrap", false, TargetType.Field)]
	public extern bool wordWrap
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_Clipping", false, TargetType.Field)]
	public extern TextClipping clipping
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_ContentOffset", false, TargetType.Field)]
	public Vector2 contentOffset
	{
		get
		{
			get_contentOffset_Injected(out var ret);
			return ret;
		}
		set
		{
			set_contentOffset_Injected(ref value);
		}
	}

	[NativeProperty("m_FixedWidth", false, TargetType.Field)]
	public extern float fixedWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_FixedHeight", false, TargetType.Field)]
	public extern float fixedHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_StretchWidth", false, TargetType.Field)]
	public extern bool stretchWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_StretchHeight", false, TargetType.Field)]
	public extern bool stretchHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_FontSize", false, TargetType.Field)]
	public extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_FontStyle", false, TargetType.Field)]
	public extern FontStyle fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("m_RichText", false, TargetType.Field)]
	public extern bool richText
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("Don't use clipOffset - put things inside BeginGroup instead. This functionality will be removed in a later version.", false)]
	[NativeProperty("m_ClipOffset", false, TargetType.Field)]
	public Vector2 clipOffset
	{
		get
		{
			get_clipOffset_Injected(out var ret);
			return ret;
		}
		set
		{
			set_clipOffset_Injected(ref value);
		}
	}

	[NativeProperty("m_ClipOffset", false, TargetType.Field)]
	internal Vector2 Internal_clipOffset
	{
		get
		{
			get_Internal_clipOffset_Injected(out var ret);
			return ret;
		}
		set
		{
			set_Internal_clipOffset_Injected(ref value);
		}
	}

	public string name
	{
		get
		{
			return m_Name ?? (m_Name = rawName);
		}
		set
		{
			m_Name = value;
			rawName = value;
		}
	}

	public GUIStyleState normal
	{
		get
		{
			return m_Normal ?? (m_Normal = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(0)));
		}
		set
		{
			AssignStyleState(0, value.m_Ptr);
		}
	}

	public GUIStyleState hover
	{
		get
		{
			return m_Hover ?? (m_Hover = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(1)));
		}
		set
		{
			AssignStyleState(1, value.m_Ptr);
		}
	}

	public GUIStyleState active
	{
		get
		{
			return m_Active ?? (m_Active = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(2)));
		}
		set
		{
			AssignStyleState(2, value.m_Ptr);
		}
	}

	public GUIStyleState onNormal
	{
		get
		{
			return m_OnNormal ?? (m_OnNormal = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(4)));
		}
		set
		{
			AssignStyleState(4, value.m_Ptr);
		}
	}

	public GUIStyleState onHover
	{
		get
		{
			return m_OnHover ?? (m_OnHover = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(5)));
		}
		set
		{
			AssignStyleState(5, value.m_Ptr);
		}
	}

	public GUIStyleState onActive
	{
		get
		{
			return m_OnActive ?? (m_OnActive = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(6)));
		}
		set
		{
			AssignStyleState(6, value.m_Ptr);
		}
	}

	public GUIStyleState focused
	{
		get
		{
			return m_Focused ?? (m_Focused = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(3)));
		}
		set
		{
			AssignStyleState(3, value.m_Ptr);
		}
	}

	public GUIStyleState onFocused
	{
		get
		{
			return m_OnFocused ?? (m_OnFocused = GUIStyleState.GetGUIStyleState(this, GetStyleStatePtr(7)));
		}
		set
		{
			AssignStyleState(7, value.m_Ptr);
		}
	}

	public RectOffset border
	{
		get
		{
			return m_Border ?? (m_Border = new RectOffset(this, GetRectOffsetPtr(0)));
		}
		set
		{
			AssignRectOffset(0, value.m_Ptr);
		}
	}

	public RectOffset margin
	{
		get
		{
			return m_Margin ?? (m_Margin = new RectOffset(this, GetRectOffsetPtr(1)));
		}
		set
		{
			AssignRectOffset(1, value.m_Ptr);
		}
	}

	public RectOffset padding
	{
		get
		{
			return m_Padding ?? (m_Padding = new RectOffset(this, GetRectOffsetPtr(2)));
		}
		set
		{
			AssignRectOffset(2, value.m_Ptr);
		}
	}

	public RectOffset overflow
	{
		get
		{
			return m_Overflow ?? (m_Overflow = new RectOffset(this, GetRectOffsetPtr(3)));
		}
		set
		{
			AssignRectOffset(3, value.m_Ptr);
		}
	}

	public float lineHeight => Mathf.Round(Internal_GetLineHeight(m_Ptr));

	public static GUIStyle none => s_None ?? (s_None = new GUIStyle());

	public bool isHeightDependantOnWidth => fixedHeight == 0f && wordWrap && imagePosition != ImagePosition.ImageOnly;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_Create", IsThreadSafe = true)]
	private static extern IntPtr Internal_Create(GUIStyle self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_Copy", IsThreadSafe = true)]
	private static extern IntPtr Internal_Copy(GUIStyle self, GUIStyle other);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_Destroy", IsThreadSafe = true)]
	private static extern void Internal_Destroy(IntPtr self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::GetStyleStatePtr", IsThreadSafe = true, HasExplicitThis = true)]
	private extern IntPtr GetStyleStatePtr(int idx);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::AssignStyleState", HasExplicitThis = true)]
	private extern void AssignStyleState(int idx, IntPtr srcStyleState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::GetRectOffsetPtr", HasExplicitThis = true)]
	private extern IntPtr GetRectOffsetPtr(int idx);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::AssignRectOffset", HasExplicitThis = true)]
	private extern void AssignRectOffset(int idx, IntPtr srcRectOffset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetLineHeight")]
	private static extern float Internal_GetLineHeight(IntPtr target);

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_Draw", HasExplicitThis = true)]
	private void Internal_Draw(Rect screenRect, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Internal_Draw_Injected(ref screenRect, content, isHover, isActive, on, hasKeyboardFocus);
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_Draw2", HasExplicitThis = true)]
	private void Internal_Draw2(Rect position, GUIContent content, int controlID, bool on)
	{
		Internal_Draw2_Injected(ref position, content, controlID, on);
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_DrawCursor", HasExplicitThis = true)]
	private void Internal_DrawCursor(Rect position, GUIContent content, int pos, Color cursorColor)
	{
		Internal_DrawCursor_Injected(ref position, content, pos, ref cursorColor);
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_DrawWithTextSelection", HasExplicitThis = true)]
	private void Internal_DrawWithTextSelection(Rect screenRect, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus, bool drawSelectionAsComposition, int cursorFirst, int cursorLast, Color cursorColor, Color selectionColor)
	{
		Internal_DrawWithTextSelection_Injected(ref screenRect, content, isHover, isActive, on, hasKeyboardFocus, drawSelectionAsComposition, cursorFirst, cursorLast, ref cursorColor, ref selectionColor);
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetCursorPixelPosition", HasExplicitThis = true)]
	internal Vector2 Internal_GetCursorPixelPosition(Rect position, GUIContent content, int cursorStringIndex)
	{
		Internal_GetCursorPixelPosition_Injected(ref position, content, cursorStringIndex, out var ret);
		return ret;
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetCursorStringIndex", HasExplicitThis = true)]
	internal int Internal_GetCursorStringIndex(Rect position, GUIContent content, Vector2 cursorPixelPosition)
	{
		return Internal_GetCursorStringIndex_Injected(ref position, content, ref cursorPixelPosition);
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetSelectedRenderedText", HasExplicitThis = true)]
	internal string Internal_GetSelectedRenderedText(Rect localPosition, GUIContent mContent, int selectIndex, int cursorIndex)
	{
		return Internal_GetSelectedRenderedText_Injected(ref localPosition, mContent, selectIndex, cursorIndex);
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetHyperlinksRect", HasExplicitThis = true)]
	internal Rect[] Internal_GetHyperlinksRect(Rect localPosition, GUIContent mContent)
	{
		return Internal_GetHyperlinksRect_Injected(ref localPosition, mContent);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetNumCharactersThatFitWithinWidth", HasExplicitThis = true)]
	internal extern int Internal_GetNumCharactersThatFitWithinWidth(string text, float width);

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_CalcSize", HasExplicitThis = true)]
	internal Vector2 Internal_CalcSize(GUIContent content)
	{
		Internal_CalcSize_Injected(content, out var ret);
		return ret;
	}

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_CalcSizeWithConstraints", HasExplicitThis = true)]
	internal Vector2 Internal_CalcSizeWithConstraints(GUIContent content, Vector2 maxSize)
	{
		Internal_CalcSizeWithConstraints_Injected(content, ref maxSize, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_CalcHeight", HasExplicitThis = true)]
	private extern float Internal_CalcHeight(GUIContent content, float width);

	[FreeFunction(Name = "GUIStyle_Bindings::Internal_CalcMinMaxWidth", HasExplicitThis = true)]
	private Vector2 Internal_CalcMinMaxWidth(GUIContent content)
	{
		Internal_CalcMinMaxWidth_Injected(content, out var ret);
		return ret;
	}

	[FreeFunction(Name = "GUIStyle_Bindings::SetMouseTooltip")]
	internal static void SetMouseTooltip(string tooltip, Rect screenRect)
	{
		SetMouseTooltip_Injected(tooltip, ref screenRect);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle_Bindings::Internal_GetCursorFlashOffset")]
	private static extern float Internal_GetCursorFlashOffset();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "GUIStyle::SetDefaultFont")]
	internal static extern void SetDefaultFont(Font font);

	public GUIStyle()
	{
		m_Ptr = Internal_Create(this);
	}

	public GUIStyle(GUIStyle other)
	{
		if (other == null)
		{
			Debug.LogError("Copied style is null. Using StyleNotFound instead.");
			other = GUISkin.error;
		}
		m_Ptr = Internal_Copy(this, other);
	}

	~GUIStyle()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Internal_Destroy(m_Ptr);
			m_Ptr = IntPtr.Zero;
		}
	}

	internal static void CleanupRoots()
	{
		s_None = null;
	}

	internal void InternalOnAfterDeserialize()
	{
		m_Normal = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(0));
		m_Hover = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(1));
		m_Active = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(2));
		m_Focused = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(3));
		m_OnNormal = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(4));
		m_OnHover = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(5));
		m_OnActive = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(6));
		m_OnFocused = GUIStyleState.ProduceGUIStyleStateFromDeserialization(this, GetStyleStatePtr(7));
	}

	public void Draw(Rect position, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, GUIContent.none, -1, isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, string text, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, GUIContent.Temp(text), -1, isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, Texture image, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, GUIContent.Temp(image), -1, isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		Draw(position, content, -1, isHover, isActive, on, hasKeyboardFocus);
	}

	public void Draw(Rect position, GUIContent content, int controlID)
	{
		Draw(position, content, controlID, isHover: false, isActive: false, on: false, hasKeyboardFocus: false);
	}

	public void Draw(Rect position, GUIContent content, int controlID, bool on)
	{
		Draw(position, content, controlID, isHover: false, isActive: false, on, hasKeyboardFocus: false);
	}

	public void Draw(Rect position, GUIContent content, int controlID, bool on, bool hover)
	{
		Draw(position, content, controlID, hover, GUIUtility.hotControl == controlID, on, GUIUtility.HasKeyFocus(controlID));
	}

	private void Draw(Rect position, GUIContent content, int controlId, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
	{
		if (controlId == -1)
		{
			Internal_Draw(position, content, isHover, isActive, on, hasKeyboardFocus);
		}
		else
		{
			Internal_Draw2(position, content, controlId, on);
		}
	}

	public void DrawCursor(Rect position, GUIContent content, int controlID, int character)
	{
		Event current = Event.current;
		if (current.type == EventType.Repaint)
		{
			Color cursorColor = new Color(0f, 0f, 0f, 0f);
			float cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
			float num = (Time.realtimeSinceStartup - Internal_GetCursorFlashOffset()) % cursorFlashSpeed / cursorFlashSpeed;
			if (cursorFlashSpeed == 0f || num < 0.5f)
			{
				cursorColor = GUI.skin.settings.cursorColor;
			}
			Internal_DrawCursor(position, content, character, cursorColor);
		}
	}

	internal void DrawWithTextSelection(Rect position, GUIContent content, bool isActive, bool hasKeyboardFocus, int firstSelectedCharacter, int lastSelectedCharacter, bool drawSelectionAsComposition, Color selectionColor)
	{
		Color cursorColor = new Color(0f, 0f, 0f, 0f);
		float cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
		float num = (Time.realtimeSinceStartup - Internal_GetCursorFlashOffset()) % cursorFlashSpeed / cursorFlashSpeed;
		if (cursorFlashSpeed == 0f || num < 0.5f)
		{
			cursorColor = GUI.skin.settings.cursorColor;
		}
		bool isHover = position.Contains(Event.current.mousePosition);
		Internal_DrawWithTextSelection(position, content, isHover, isActive, on: false, hasKeyboardFocus, drawSelectionAsComposition, firstSelectedCharacter, lastSelectedCharacter, cursorColor, selectionColor);
	}

	internal void DrawWithTextSelection(Rect position, GUIContent content, int controlID, int firstSelectedCharacter, int lastSelectedCharacter, bool drawSelectionAsComposition)
	{
		DrawWithTextSelection(position, content, controlID == GUIUtility.hotControl, controlID == GUIUtility.keyboardControl && showKeyboardFocus, firstSelectedCharacter, lastSelectedCharacter, drawSelectionAsComposition, GUI.skin.settings.selectionColor);
	}

	public void DrawWithTextSelection(Rect position, GUIContent content, int controlID, int firstSelectedCharacter, int lastSelectedCharacter)
	{
		DrawWithTextSelection(position, content, controlID, firstSelectedCharacter, lastSelectedCharacter, drawSelectionAsComposition: false);
	}

	public static implicit operator GUIStyle(string str)
	{
		if (GUISkin.current == null)
		{
			Debug.LogError("Unable to use a named GUIStyle without a current skin. Most likely you need to move your GUIStyle initialization code to OnGUI");
			return GUISkin.error;
		}
		return GUISkin.current.GetStyle(str);
	}

	public Vector2 GetCursorPixelPosition(Rect position, GUIContent content, int cursorStringIndex)
	{
		return Internal_GetCursorPixelPosition(position, content, cursorStringIndex);
	}

	public int GetCursorStringIndex(Rect position, GUIContent content, Vector2 cursorPixelPosition)
	{
		return Internal_GetCursorStringIndex(position, content, cursorPixelPosition);
	}

	internal int GetNumCharactersThatFitWithinWidth(string text, float width)
	{
		return Internal_GetNumCharactersThatFitWithinWidth(text, width);
	}

	public Vector2 CalcSize(GUIContent content)
	{
		return Internal_CalcSize(content);
	}

	internal Vector2 CalcSizeWithConstraints(GUIContent content, Vector2 constraints)
	{
		return Internal_CalcSizeWithConstraints(content, constraints);
	}

	public Vector2 CalcScreenSize(Vector2 contentSize)
	{
		return new Vector2((fixedWidth != 0f) ? fixedWidth : Mathf.Ceil(contentSize.x + (float)padding.left + (float)padding.right), (fixedHeight != 0f) ? fixedHeight : Mathf.Ceil(contentSize.y + (float)padding.top + (float)padding.bottom));
	}

	public float CalcHeight(GUIContent content, float width)
	{
		return Internal_CalcHeight(content, width);
	}

	public void CalcMinMaxWidth(GUIContent content, out float minWidth, out float maxWidth)
	{
		Vector2 vector = Internal_CalcMinMaxWidth(content);
		minWidth = vector.x;
		maxWidth = vector.y;
	}

	public override string ToString()
	{
		return UnityString.Format("GUIStyle '{0}'", name);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_contentOffset_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_contentOffset_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_clipOffset_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_clipOffset_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_Internal_clipOffset_Injected(out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_Internal_clipOffset_Injected(ref Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_Draw_Injected(ref Rect screenRect, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_Draw2_Injected(ref Rect position, GUIContent content, int controlID, bool on);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_DrawCursor_Injected(ref Rect position, GUIContent content, int pos, ref Color cursorColor);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_DrawWithTextSelection_Injected(ref Rect screenRect, GUIContent content, bool isHover, bool isActive, bool on, bool hasKeyboardFocus, bool drawSelectionAsComposition, int cursorFirst, int cursorLast, ref Color cursorColor, ref Color selectionColor);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_GetCursorPixelPosition_Injected(ref Rect position, GUIContent content, int cursorStringIndex, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern int Internal_GetCursorStringIndex_Injected(ref Rect position, GUIContent content, ref Vector2 cursorPixelPosition);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern string Internal_GetSelectedRenderedText_Injected(ref Rect localPosition, GUIContent mContent, int selectIndex, int cursorIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern Rect[] Internal_GetHyperlinksRect_Injected(ref Rect localPosition, GUIContent mContent);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_CalcSize_Injected(GUIContent content, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_CalcSizeWithConstraints_Injected(GUIContent content, ref Vector2 maxSize, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_CalcMinMaxWidth_Injected(GUIContent content, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetMouseTooltip_Injected(string tooltip, ref Rect screenRect);
}
