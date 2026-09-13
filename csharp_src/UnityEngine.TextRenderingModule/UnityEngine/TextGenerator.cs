using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[UsedByNativeCode]
[NativeHeader("Modules/TextRendering/TextGenerator.h")]
public sealed class TextGenerator : IDisposable
{
	internal IntPtr m_Ptr;

	private string m_LastString;

	private TextGenerationSettings m_LastSettings;

	private bool m_HasGenerated;

	private TextGenerationError m_LastValid;

	private readonly List<UIVertex> m_Verts;

	private readonly List<UICharInfo> m_Characters;

	private readonly List<UILineInfo> m_Lines;

	private bool m_CachedVerts;

	private bool m_CachedCharacters;

	private bool m_CachedLines;

	public int characterCountVisible => characterCount - 1;

	public IList<UIVertex> verts
	{
		get
		{
			if (!m_CachedVerts)
			{
				GetVertices(m_Verts);
				m_CachedVerts = true;
			}
			return m_Verts;
		}
	}

	public IList<UICharInfo> characters
	{
		get
		{
			if (!m_CachedCharacters)
			{
				GetCharacters(m_Characters);
				m_CachedCharacters = true;
			}
			return m_Characters;
		}
	}

	public IList<UILineInfo> lines
	{
		get
		{
			if (!m_CachedLines)
			{
				GetLines(m_Lines);
				m_CachedLines = true;
			}
			return m_Lines;
		}
	}

	public Rect rectExtents
	{
		get
		{
			get_rectExtents_Injected(out var ret);
			return ret;
		}
	}

	public extern int vertexCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int characterCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int lineCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("FontSizeFoundForBestFit", false, TargetType.Function)]
	public extern int fontSizeUsedForBestFit
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public TextGenerator()
		: this(50)
	{
	}

	public TextGenerator(int initialCapacity)
	{
		m_Ptr = Internal_Create();
		m_Verts = new List<UIVertex>((initialCapacity + 1) * 4);
		m_Characters = new List<UICharInfo>(initialCapacity + 1);
		m_Lines = new List<UILineInfo>(20);
	}

	~TextGenerator()
	{
		((IDisposable)this).Dispose();
	}

	void IDisposable.Dispose()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Internal_Destroy(m_Ptr);
			m_Ptr = IntPtr.Zero;
		}
	}

	private TextGenerationSettings ValidatedSettings(TextGenerationSettings settings)
	{
		if (settings.font != null && settings.font.dynamic)
		{
			return settings;
		}
		if (settings.fontSize != 0 || settings.fontStyle != FontStyle.Normal)
		{
			if (settings.font != null)
			{
				Debug.LogWarningFormat(settings.font, "Font size and style overrides are only supported for dynamic fonts. Font '{0}' is not dynamic.", settings.font.name);
			}
			settings.fontSize = 0;
			settings.fontStyle = FontStyle.Normal;
		}
		if (settings.resizeTextForBestFit)
		{
			if (settings.font != null)
			{
				Debug.LogWarningFormat(settings.font, "BestFit is only supported for dynamic fonts. Font '{0}' is not dynamic.", settings.font.name);
			}
			settings.resizeTextForBestFit = false;
		}
		return settings;
	}

	public void Invalidate()
	{
		m_HasGenerated = false;
	}

	public void GetCharacters(List<UICharInfo> characters)
	{
		GetCharactersInternal(characters);
	}

	public void GetLines(List<UILineInfo> lines)
	{
		GetLinesInternal(lines);
	}

	public void GetVertices(List<UIVertex> vertices)
	{
		GetVerticesInternal(vertices);
	}

	public float GetPreferredWidth(string str, TextGenerationSettings settings)
	{
		settings.horizontalOverflow = HorizontalWrapMode.Overflow;
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		settings.updateBounds = true;
		Populate(str, settings);
		return rectExtents.width;
	}

	public float GetPreferredHeight(string str, TextGenerationSettings settings)
	{
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		settings.updateBounds = true;
		Populate(str, settings);
		return rectExtents.height;
	}

	public bool PopulateWithErrors(string str, TextGenerationSettings settings, GameObject context)
	{
		TextGenerationError textGenerationError = PopulateWithError(str, settings);
		if (textGenerationError == TextGenerationError.None)
		{
			return true;
		}
		if ((textGenerationError & TextGenerationError.CustomSizeOnNonDynamicFont) != TextGenerationError.None)
		{
			Debug.LogErrorFormat(context, "Font '{0}' is not dynamic, which is required to override its size", settings.font);
		}
		if ((textGenerationError & TextGenerationError.CustomStyleOnNonDynamicFont) != TextGenerationError.None)
		{
			Debug.LogErrorFormat(context, "Font '{0}' is not dynamic, which is required to override its style", settings.font);
		}
		return false;
	}

	public bool Populate(string str, TextGenerationSettings settings)
	{
		TextGenerationError textGenerationError = PopulateWithError(str, settings);
		return textGenerationError == TextGenerationError.None;
	}

	private TextGenerationError PopulateWithError(string str, TextGenerationSettings settings)
	{
		if (m_HasGenerated && str == m_LastString && settings.Equals(m_LastSettings))
		{
			return m_LastValid;
		}
		m_LastValid = PopulateAlways(str, settings);
		return m_LastValid;
	}

	private TextGenerationError PopulateAlways(string str, TextGenerationSettings settings)
	{
		m_LastString = str;
		m_HasGenerated = true;
		m_CachedVerts = false;
		m_CachedCharacters = false;
		m_CachedLines = false;
		m_LastSettings = settings;
		TextGenerationSettings textGenerationSettings = ValidatedSettings(settings);
		Populate_Internal(str, textGenerationSettings.font, textGenerationSettings.color, textGenerationSettings.fontSize, textGenerationSettings.scaleFactor, textGenerationSettings.lineSpacing, textGenerationSettings.fontStyle, textGenerationSettings.richText, textGenerationSettings.resizeTextForBestFit, textGenerationSettings.resizeTextMinSize, textGenerationSettings.resizeTextMaxSize, textGenerationSettings.verticalOverflow, textGenerationSettings.horizontalOverflow, textGenerationSettings.updateBounds, textGenerationSettings.textAnchor, textGenerationSettings.generationExtents, textGenerationSettings.pivot, textGenerationSettings.generateOutOfBounds, textGenerationSettings.alignByGeometry, out var error);
		m_LastValid = error;
		return error;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern IntPtr Internal_Create();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void Internal_Destroy(IntPtr ptr);

	internal bool Populate_Internal(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds, bool alignByGeometry, out uint error)
	{
		return Populate_Internal_Injected(str, font, ref color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, verticalOverFlow, horizontalOverflow, updateBounds, anchor, extentsX, extentsY, pivotX, pivotY, generateOutOfBounds, alignByGeometry, out error);
	}

	internal bool Populate_Internal(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, VerticalWrapMode verticalOverFlow, HorizontalWrapMode horizontalOverflow, bool updateBounds, TextAnchor anchor, Vector2 extents, Vector2 pivot, bool generateOutOfBounds, bool alignByGeometry, out TextGenerationError error)
	{
		if (font == null)
		{
			error = TextGenerationError.NoFont;
			return false;
		}
		uint error2 = 0u;
		bool result = Populate_Internal(str, font, color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, (int)verticalOverFlow, (int)horizontalOverflow, updateBounds, anchor, extents.x, extents.y, pivot.x, pivot.y, generateOutOfBounds, alignByGeometry, out error2);
		error = (TextGenerationError)error2;
		return result;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern UIVertex[] GetVerticesArray();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern UICharInfo[] GetCharactersArray();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern UILineInfo[] GetLinesArray();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private extern void GetVerticesInternal(object vertices);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private extern void GetCharactersInternal(object characters);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private extern void GetLinesInternal(object lines);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_rectExtents_Injected(out Rect ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool Populate_Internal_Injected(string str, Font font, ref Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds, bool alignByGeometry, out uint error);
}
