namespace UnityEngine;

public class GUILayout
{
	private sealed class LayoutedWindow
	{
		private readonly GUI.WindowFunction m_Func;

		private readonly Rect m_ScreenRect;

		private readonly GUILayoutOption[] m_Options;

		private readonly GUIStyle m_Style;

		internal LayoutedWindow(GUI.WindowFunction f, Rect screenRect, GUIContent content, GUILayoutOption[] options, GUIStyle style)
		{
			m_Func = f;
			m_ScreenRect = screenRect;
			m_Options = options;
			m_Style = style;
		}

		public void DoWindow(int windowID)
		{
			GUILayoutGroup topLevel = GUILayoutUtility.current.topLevel;
			EventType type = Event.current.type;
			if (type == EventType.Layout)
			{
				topLevel.resetCoords = true;
				topLevel.rect = m_ScreenRect;
				if (m_Options != null)
				{
					topLevel.ApplyOptions(m_Options);
				}
				topLevel.isWindow = true;
				topLevel.windowID = windowID;
				topLevel.style = m_Style;
			}
			else
			{
				topLevel.ResetCursor();
			}
			m_Func(windowID);
		}
	}

	public class HorizontalScope : GUI.Scope
	{
		public HorizontalScope(params GUILayoutOption[] options)
		{
			BeginHorizontal(options);
		}

		public HorizontalScope(GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(style, options);
		}

		public HorizontalScope(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(text, style, options);
		}

		public HorizontalScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(image, style, options);
		}

		public HorizontalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginHorizontal(content, style, options);
		}

		protected override void CloseScope()
		{
			EndHorizontal();
		}
	}

	public class VerticalScope : GUI.Scope
	{
		public VerticalScope(params GUILayoutOption[] options)
		{
			BeginVertical(options);
		}

		public VerticalScope(GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(style, options);
		}

		public VerticalScope(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(text, style, options);
		}

		public VerticalScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(image, style, options);
		}

		public VerticalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			BeginVertical(content, style, options);
		}

		protected override void CloseScope()
		{
			EndVertical();
		}
	}

	public class AreaScope : GUI.Scope
	{
		public AreaScope(Rect screenRect)
		{
			BeginArea(screenRect);
		}

		public AreaScope(Rect screenRect, string text)
		{
			BeginArea(screenRect, text);
		}

		public AreaScope(Rect screenRect, Texture image)
		{
			BeginArea(screenRect, image);
		}

		public AreaScope(Rect screenRect, GUIContent content)
		{
			BeginArea(screenRect, content);
		}

		public AreaScope(Rect screenRect, string text, GUIStyle style)
		{
			BeginArea(screenRect, text, style);
		}

		public AreaScope(Rect screenRect, Texture image, GUIStyle style)
		{
			BeginArea(screenRect, image, style);
		}

		public AreaScope(Rect screenRect, GUIContent content, GUIStyle style)
		{
			BeginArea(screenRect, content, style);
		}

		protected override void CloseScope()
		{
			EndArea();
		}
	}

	public class ScrollViewScope : GUI.Scope
	{
		public Vector2 scrollPosition { get; private set; }

		public bool handleScrollWheel { get; set; }

		public ScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			handleScrollWheel = true;
			this.scrollPosition = BeginScrollView(scrollPosition, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
		{
			handleScrollWheel = true;
			this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
		{
			handleScrollWheel = true;
			this.scrollPosition = BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
		{
			handleScrollWheel = true;
			this.scrollPosition = BeginScrollView(scrollPosition, style, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
		{
			handleScrollWheel = true;
			this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, options);
		}

		public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
		{
			handleScrollWheel = true;
			this.scrollPosition = BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
		}

		protected override void CloseScope()
		{
			EndScrollView(handleScrollWheel);
		}
	}

	public static void Label(Texture image, params GUILayoutOption[] options)
	{
		DoLabel(GUIContent.Temp(image), GUI.skin.label, options);
	}

	public static void Label(string text, params GUILayoutOption[] options)
	{
		DoLabel(GUIContent.Temp(text), GUI.skin.label, options);
	}

	public static void Label(GUIContent content, params GUILayoutOption[] options)
	{
		DoLabel(content, GUI.skin.label, options);
	}

	public static void Label(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		DoLabel(GUIContent.Temp(image), style, options);
	}

	public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		DoLabel(GUIContent.Temp(text), style, options);
	}

	public static void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		DoLabel(content, style, options);
	}

	private static void DoLabel(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		GUI.Label(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static void Box(Texture image, params GUILayoutOption[] options)
	{
		DoBox(GUIContent.Temp(image), GUI.skin.box, options);
	}

	public static void Box(string text, params GUILayoutOption[] options)
	{
		DoBox(GUIContent.Temp(text), GUI.skin.box, options);
	}

	public static void Box(GUIContent content, params GUILayoutOption[] options)
	{
		DoBox(content, GUI.skin.box, options);
	}

	public static void Box(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		DoBox(GUIContent.Temp(image), style, options);
	}

	public static void Box(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		DoBox(GUIContent.Temp(text), style, options);
	}

	public static void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		DoBox(content, style, options);
	}

	private static void DoBox(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		GUI.Box(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static bool Button(Texture image, params GUILayoutOption[] options)
	{
		return DoButton(GUIContent.Temp(image), GUI.skin.button, options);
	}

	public static bool Button(string text, params GUILayoutOption[] options)
	{
		return DoButton(GUIContent.Temp(text), GUI.skin.button, options);
	}

	public static bool Button(GUIContent content, params GUILayoutOption[] options)
	{
		return DoButton(content, GUI.skin.button, options);
	}

	public static bool Button(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoButton(GUIContent.Temp(image), style, options);
	}

	public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoButton(GUIContent.Temp(text), style, options);
	}

	public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoButton(content, style, options);
	}

	private static bool DoButton(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		return GUI.Button(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static bool RepeatButton(Texture image, params GUILayoutOption[] options)
	{
		return DoRepeatButton(GUIContent.Temp(image), GUI.skin.button, options);
	}

	public static bool RepeatButton(string text, params GUILayoutOption[] options)
	{
		return DoRepeatButton(GUIContent.Temp(text), GUI.skin.button, options);
	}

	public static bool RepeatButton(GUIContent content, params GUILayoutOption[] options)
	{
		return DoRepeatButton(content, GUI.skin.button, options);
	}

	public static bool RepeatButton(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoRepeatButton(GUIContent.Temp(image), style, options);
	}

	public static bool RepeatButton(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoRepeatButton(GUIContent.Temp(text), style, options);
	}

	public static bool RepeatButton(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoRepeatButton(content, style, options);
	}

	private static bool DoRepeatButton(GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		return GUI.RepeatButton(GUILayoutUtility.GetRect(content, style, options), content, style);
	}

	public static string TextField(string text, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: false, GUI.skin.textField, options);
	}

	public static string TextField(string text, int maxLength, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: false, GUI.skin.textField, options);
	}

	public static string TextField(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: false, style, options);
	}

	public static string TextField(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: false, style, options);
	}

	public static string PasswordField(string password, char maskChar, params GUILayoutOption[] options)
	{
		return PasswordField(password, maskChar, -1, GUI.skin.textField, options);
	}

	public static string PasswordField(string password, char maskChar, int maxLength, params GUILayoutOption[] options)
	{
		return PasswordField(password, maskChar, maxLength, GUI.skin.textField, options);
	}

	public static string PasswordField(string password, char maskChar, GUIStyle style, params GUILayoutOption[] options)
	{
		return PasswordField(password, maskChar, -1, style, options);
	}

	public static string PasswordField(string password, char maskChar, int maxLength, GUIStyle style, params GUILayoutOption[] options)
	{
		GUIContent content = GUIContent.Temp(GUI.PasswordFieldGetStrToShow(password, maskChar));
		return GUI.PasswordField(GUILayoutUtility.GetRect(content, GUI.skin.textField, options), password, maskChar, maxLength, style);
	}

	public static string TextArea(string text, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: true, GUI.skin.textArea, options);
	}

	public static string TextArea(string text, int maxLength, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: true, GUI.skin.textArea, options);
	}

	public static string TextArea(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, -1, multiline: true, style, options);
	}

	public static string TextArea(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoTextField(text, maxLength, multiline: true, style, options);
	}

	private static string DoTextField(string text, int maxLength, bool multiline, GUIStyle style, GUILayoutOption[] options)
	{
		int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
		GUIContent gUIContent = GUIContent.Temp(text);
		gUIContent = ((GUIUtility.keyboardControl == controlID) ? GUIContent.Temp(text + GUIUtility.compositionString) : GUIContent.Temp(text));
		Rect rect = GUILayoutUtility.GetRect(gUIContent, style, options);
		if (GUIUtility.keyboardControl == controlID)
		{
			gUIContent = GUIContent.Temp(text);
		}
		GUI.DoTextField(rect, controlID, gUIContent, multiline, maxLength, style);
		return gUIContent.text;
	}

	public static bool Toggle(bool value, Texture image, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUIContent.Temp(image), GUI.skin.toggle, options);
	}

	public static bool Toggle(bool value, string text, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUIContent.Temp(text), GUI.skin.toggle, options);
	}

	public static bool Toggle(bool value, GUIContent content, params GUILayoutOption[] options)
	{
		return DoToggle(value, content, GUI.skin.toggle, options);
	}

	public static bool Toggle(bool value, Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUIContent.Temp(image), style, options);
	}

	public static bool Toggle(bool value, string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoToggle(value, GUIContent.Temp(text), style, options);
	}

	public static bool Toggle(bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoToggle(value, content, style, options);
	}

	private static bool DoToggle(bool value, GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		return GUI.Toggle(GUILayoutUtility.GetRect(content, style, options), value, content, style);
	}

	public static int Toolbar(int selected, string[] texts, params GUILayoutOption[] options)
	{
		return Toolbar(selected, GUIContent.Temp(texts), GUI.skin.button, options);
	}

	public static int Toolbar(int selected, Texture[] images, params GUILayoutOption[] options)
	{
		return Toolbar(selected, GUIContent.Temp(images), GUI.skin.button, options);
	}

	public static int Toolbar(int selected, GUIContent[] contents, params GUILayoutOption[] options)
	{
		return Toolbar(selected, contents, GUI.skin.button, options);
	}

	public static int Toolbar(int selected, string[] texts, GUIStyle style, params GUILayoutOption[] options)
	{
		return Toolbar(selected, GUIContent.Temp(texts), style, options);
	}

	public static int Toolbar(int selected, Texture[] images, GUIStyle style, params GUILayoutOption[] options)
	{
		return Toolbar(selected, GUIContent.Temp(images), style, options);
	}

	public static int Toolbar(int selected, string[] texts, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
	{
		return Toolbar(selected, GUIContent.Temp(texts), style, buttonSize, options);
	}

	public static int Toolbar(int selected, Texture[] images, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
	{
		return Toolbar(selected, GUIContent.Temp(images), style, buttonSize, options);
	}

	public static int Toolbar(int selected, GUIContent[] contents, GUIStyle style, params GUILayoutOption[] options)
	{
		return Toolbar(selected, contents, style, GUI.ToolbarButtonSize.Fixed, options);
	}

	public static int Toolbar(int selected, GUIContent[] contents, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
	{
		return Toolbar(selected, contents, null, style, buttonSize, options);
	}

	public static int Toolbar(int selected, GUIContent[] contents, bool[] enabled, GUIStyle style, params GUILayoutOption[] options)
	{
		return Toolbar(selected, contents, enabled, style, GUI.ToolbarButtonSize.Fixed, options);
	}

	public static int Toolbar(int selected, GUIContent[] contents, bool[] enabled, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
	{
		GUI.FindStyles(ref style, out var firstStyle, out var midStyle, out var lastStyle, "left", "mid", "right");
		Vector2 vector = default(Vector2);
		int num = contents.Length;
		GUIStyle gUIStyle = ((num > 1) ? firstStyle : style);
		GUIStyle gUIStyle2 = ((num > 1) ? midStyle : style);
		GUIStyle gUIStyle3 = ((num > 1) ? lastStyle : style);
		float num2 = 0f;
		for (int i = 0; i < contents.Length; i++)
		{
			if (i == num - 2)
			{
				gUIStyle2 = gUIStyle3;
			}
			Vector2 vector2 = gUIStyle.CalcSize(contents[i]);
			switch (buttonSize)
			{
			case GUI.ToolbarButtonSize.Fixed:
				if (vector2.x > vector.x)
				{
					vector.x = vector2.x;
				}
				break;
			case GUI.ToolbarButtonSize.FitToContents:
				vector.x += vector2.x;
				break;
			}
			if (vector2.y > vector.y)
			{
				vector.y = vector2.y;
			}
			num2 = ((i != num - 1) ? (num2 + (float)Mathf.Max(gUIStyle.margin.right, gUIStyle2.margin.left)) : (num2 + (float)gUIStyle.margin.right));
			gUIStyle = gUIStyle2;
		}
		switch (buttonSize)
		{
		case GUI.ToolbarButtonSize.Fixed:
			vector.x = vector.x * (float)contents.Length + num2;
			break;
		case GUI.ToolbarButtonSize.FitToContents:
			vector.x += num2;
			break;
		}
		return GUI.Toolbar(GUILayoutUtility.GetRect(vector.x, vector.y, style, options), selected, contents, null, style, buttonSize, enabled);
	}

	public static int SelectionGrid(int selected, string[] texts, int xCount, params GUILayoutOption[] options)
	{
		return SelectionGrid(selected, GUIContent.Temp(texts), xCount, GUI.skin.button, options);
	}

	public static int SelectionGrid(int selected, Texture[] images, int xCount, params GUILayoutOption[] options)
	{
		return SelectionGrid(selected, GUIContent.Temp(images), xCount, GUI.skin.button, options);
	}

	public static int SelectionGrid(int selected, GUIContent[] content, int xCount, params GUILayoutOption[] options)
	{
		return SelectionGrid(selected, content, xCount, GUI.skin.button, options);
	}

	public static int SelectionGrid(int selected, string[] texts, int xCount, GUIStyle style, params GUILayoutOption[] options)
	{
		return SelectionGrid(selected, GUIContent.Temp(texts), xCount, style, options);
	}

	public static int SelectionGrid(int selected, Texture[] images, int xCount, GUIStyle style, params GUILayoutOption[] options)
	{
		return SelectionGrid(selected, GUIContent.Temp(images), xCount, style, options);
	}

	public static int SelectionGrid(int selected, GUIContent[] contents, int xCount, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUI.SelectionGrid(GUIGridSizer.GetRect(contents, xCount, style, options), selected, contents, xCount, style);
	}

	public static float HorizontalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
	{
		return DoHorizontalSlider(value, leftValue, rightValue, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, options);
	}

	public static float HorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
	{
		return DoHorizontalSlider(value, leftValue, rightValue, slider, thumb, options);
	}

	private static float DoHorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, GUILayoutOption[] options)
	{
		return GUI.HorizontalSlider(GUILayoutUtility.GetRect(GUIContent.Temp("mmmm"), slider, options), value, leftValue, rightValue, slider, thumb);
	}

	public static float VerticalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
	{
		return DoVerticalSlider(value, leftValue, rightValue, GUI.skin.verticalSlider, GUI.skin.verticalSliderThumb, options);
	}

	public static float VerticalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
	{
		return DoVerticalSlider(value, leftValue, rightValue, slider, thumb, options);
	}

	private static float DoVerticalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
	{
		return GUI.VerticalSlider(GUILayoutUtility.GetRect(GUIContent.Temp("\n\n\n\n\n"), slider, options), value, leftValue, rightValue, slider, thumb);
	}

	public static float HorizontalScrollbar(float value, float size, float leftValue, float rightValue, params GUILayoutOption[] options)
	{
		return HorizontalScrollbar(value, size, leftValue, rightValue, GUI.skin.horizontalScrollbar, options);
	}

	public static float HorizontalScrollbar(float value, float size, float leftValue, float rightValue, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUI.HorizontalScrollbar(GUILayoutUtility.GetRect(GUIContent.Temp("mmmm"), style, options), value, size, leftValue, rightValue, style);
	}

	public static float VerticalScrollbar(float value, float size, float topValue, float bottomValue, params GUILayoutOption[] options)
	{
		return VerticalScrollbar(value, size, topValue, bottomValue, GUI.skin.verticalScrollbar, options);
	}

	public static float VerticalScrollbar(float value, float size, float topValue, float bottomValue, GUIStyle style, params GUILayoutOption[] options)
	{
		return GUI.VerticalScrollbar(GUILayoutUtility.GetRect(GUIContent.Temp("\n\n\n\n"), style, options), value, size, topValue, bottomValue, style);
	}

	public static void Space(float pixels)
	{
		GUIUtility.CheckOnGUI();
		if (GUILayoutUtility.current.topLevel.isVertical)
		{
			GUILayoutUtility.GetRect(0f, pixels, GUILayoutUtility.spaceStyle, Height(pixels));
		}
		else
		{
			GUILayoutUtility.GetRect(pixels, 0f, GUILayoutUtility.spaceStyle, Width(pixels));
		}
		if (Event.current.type == EventType.Layout)
		{
			GUILayoutUtility.current.topLevel.entries[GUILayoutUtility.current.topLevel.entries.Count - 1].consideredForMargin = false;
		}
	}

	public static void FlexibleSpace()
	{
		GUIUtility.CheckOnGUI();
		GUILayoutOption gUILayoutOption = ((!GUILayoutUtility.current.topLevel.isVertical) ? ExpandWidth(expand: true) : ExpandHeight(expand: true));
		gUILayoutOption.value = 10000;
		GUILayoutUtility.GetRect(0f, 0f, GUILayoutUtility.spaceStyle, gUILayoutOption);
		if (Event.current.type == EventType.Layout)
		{
			GUILayoutUtility.current.topLevel.entries[GUILayoutUtility.current.topLevel.entries.Count - 1].consideredForMargin = false;
		}
	}

	public static void BeginHorizontal(params GUILayoutOption[] options)
	{
		BeginHorizontal(GUIContent.none, GUIStyle.none, options);
	}

	public static void BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
	{
		BeginHorizontal(GUIContent.none, style, options);
	}

	public static void BeginHorizontal(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginHorizontal(GUIContent.Temp(text), style, options);
	}

	public static void BeginHorizontal(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginHorizontal(GUIContent.Temp(image), style, options);
	}

	public static void BeginHorizontal(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		GUILayoutGroup gUILayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
		gUILayoutGroup.isVertical = false;
		if (style != GUIStyle.none || content != GUIContent.none)
		{
			GUI.Box(gUILayoutGroup.rect, content, style);
		}
	}

	public static void EndHorizontal()
	{
		GUILayoutUtility.EndLayoutGroup();
	}

	public static void BeginVertical(params GUILayoutOption[] options)
	{
		BeginVertical(GUIContent.none, GUIStyle.none, options);
	}

	public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options)
	{
		BeginVertical(GUIContent.none, style, options);
	}

	public static void BeginVertical(string text, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginVertical(GUIContent.Temp(text), style, options);
	}

	public static void BeginVertical(Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		BeginVertical(GUIContent.Temp(image), style, options);
	}

	public static void BeginVertical(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		GUILayoutGroup gUILayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
		gUILayoutGroup.isVertical = true;
		if (style != GUIStyle.none || content != GUIContent.none)
		{
			GUI.Box(gUILayoutGroup.rect, content, style);
		}
	}

	public static void EndVertical()
	{
		GUILayoutUtility.EndLayoutGroup();
	}

	public static void BeginArea(Rect screenRect)
	{
		BeginArea(screenRect, GUIContent.none, GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, string text)
	{
		BeginArea(screenRect, GUIContent.Temp(text), GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, Texture image)
	{
		BeginArea(screenRect, GUIContent.Temp(image), GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, GUIContent content)
	{
		BeginArea(screenRect, content, GUIStyle.none);
	}

	public static void BeginArea(Rect screenRect, GUIStyle style)
	{
		BeginArea(screenRect, GUIContent.none, style);
	}

	public static void BeginArea(Rect screenRect, string text, GUIStyle style)
	{
		BeginArea(screenRect, GUIContent.Temp(text), style);
	}

	public static void BeginArea(Rect screenRect, Texture image, GUIStyle style)
	{
		BeginArea(screenRect, GUIContent.Temp(image), style);
	}

	public static void BeginArea(Rect screenRect, GUIContent content, GUIStyle style)
	{
		GUIUtility.CheckOnGUI();
		GUILayoutGroup gUILayoutGroup = GUILayoutUtility.BeginLayoutArea(style, typeof(GUILayoutGroup));
		if (Event.current.type == EventType.Layout)
		{
			gUILayoutGroup.resetCoords = true;
			gUILayoutGroup.minWidth = (gUILayoutGroup.maxWidth = screenRect.width);
			gUILayoutGroup.minHeight = (gUILayoutGroup.maxHeight = screenRect.height);
			gUILayoutGroup.rect = Rect.MinMaxRect(screenRect.xMin, screenRect.yMin, gUILayoutGroup.rect.xMax, gUILayoutGroup.rect.yMax);
		}
		GUI.BeginGroup(gUILayoutGroup.rect, content, style);
	}

	public static void EndArea()
	{
		GUIUtility.CheckOnGUI();
		if (Event.current.type != EventType.Used)
		{
			GUILayoutUtility.current.layoutGroups.Pop();
			GUILayoutUtility.current.topLevel = (GUILayoutGroup)GUILayoutUtility.current.layoutGroups.Peek();
			GUI.EndGroup();
		}
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style)
	{
		GUILayoutOption[] options = null;
		return BeginScrollView(scrollPosition, style, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
	{
		string name = style.name;
		GUIStyle gUIStyle = GUI.skin.FindStyle(name + "VerticalScrollbar");
		if (gUIStyle == null)
		{
			gUIStyle = GUI.skin.verticalScrollbar;
		}
		GUIStyle gUIStyle2 = GUI.skin.FindStyle(name + "HorizontalScrollbar");
		if (gUIStyle2 == null)
		{
			gUIStyle2 = GUI.skin.horizontalScrollbar;
		}
		return BeginScrollView(scrollPosition, alwaysShowHorizontal: false, alwaysShowVertical: false, gUIStyle2, gUIStyle, style, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
	{
		return BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
	}

	public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
	{
		GUIUtility.CheckOnGUI();
		GUIScrollGroup gUIScrollGroup = (GUIScrollGroup)GUILayoutUtility.BeginLayoutGroup(background, null, typeof(GUIScrollGroup));
		EventType type = Event.current.type;
		if (type == EventType.Layout)
		{
			gUIScrollGroup.resetCoords = true;
			gUIScrollGroup.isVertical = true;
			gUIScrollGroup.stretchWidth = 1;
			gUIScrollGroup.stretchHeight = 1;
			gUIScrollGroup.verticalScrollbar = verticalScrollbar;
			gUIScrollGroup.horizontalScrollbar = horizontalScrollbar;
			gUIScrollGroup.needsVerticalScrollbar = alwaysShowVertical;
			gUIScrollGroup.needsHorizontalScrollbar = alwaysShowHorizontal;
			gUIScrollGroup.ApplyOptions(options);
		}
		return GUI.BeginScrollView(gUIScrollGroup.rect, scrollPosition, new Rect(0f, 0f, gUIScrollGroup.clientWidth, gUIScrollGroup.clientHeight), alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
	}

	public static void EndScrollView()
	{
		EndScrollView(handleScrollWheel: true);
	}

	internal static void EndScrollView(bool handleScrollWheel)
	{
		GUILayoutUtility.EndLayoutGroup();
		GUI.EndScrollView(handleScrollWheel);
	}

	public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, string text, params GUILayoutOption[] options)
	{
		return DoWindow(id, screenRect, func, GUIContent.Temp(text), GUI.skin.window, options);
	}

	public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, Texture image, params GUILayoutOption[] options)
	{
		return DoWindow(id, screenRect, func, GUIContent.Temp(image), GUI.skin.window, options);
	}

	public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, GUIContent content, params GUILayoutOption[] options)
	{
		return DoWindow(id, screenRect, func, content, GUI.skin.window, options);
	}

	public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, string text, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoWindow(id, screenRect, func, GUIContent.Temp(text), style, options);
	}

	public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, Texture image, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoWindow(id, screenRect, func, GUIContent.Temp(image), style, options);
	}

	public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
	{
		return DoWindow(id, screenRect, func, content, style, options);
	}

	private static Rect DoWindow(int id, Rect screenRect, GUI.WindowFunction func, GUIContent content, GUIStyle style, GUILayoutOption[] options)
	{
		GUIUtility.CheckOnGUI();
		LayoutedWindow layoutedWindow = new LayoutedWindow(func, screenRect, content, options, style);
		return GUI.Window(id, screenRect, layoutedWindow.DoWindow, content, style);
	}

	public static GUILayoutOption Width(float width)
	{
		return new GUILayoutOption(GUILayoutOption.Type.fixedWidth, width);
	}

	public static GUILayoutOption MinWidth(float minWidth)
	{
		return new GUILayoutOption(GUILayoutOption.Type.minWidth, minWidth);
	}

	public static GUILayoutOption MaxWidth(float maxWidth)
	{
		return new GUILayoutOption(GUILayoutOption.Type.maxWidth, maxWidth);
	}

	public static GUILayoutOption Height(float height)
	{
		return new GUILayoutOption(GUILayoutOption.Type.fixedHeight, height);
	}

	public static GUILayoutOption MinHeight(float minHeight)
	{
		return new GUILayoutOption(GUILayoutOption.Type.minHeight, minHeight);
	}

	public static GUILayoutOption MaxHeight(float maxHeight)
	{
		return new GUILayoutOption(GUILayoutOption.Type.maxHeight, maxHeight);
	}

	public static GUILayoutOption ExpandWidth(bool expand)
	{
		return new GUILayoutOption(GUILayoutOption.Type.stretchWidth, expand ? 1 : 0);
	}

	public static GUILayoutOption ExpandHeight(bool expand)
	{
		return new GUILayoutOption(GUILayoutOption.Type.stretchHeight, expand ? 1 : 0);
	}
}
