using System.Collections.Generic;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.UIElements;

internal struct TextHandle
{
	public bool useLegacy;

	private static Dictionary<Font, FontAsset> fontAssetCache = new Dictionary<Font, FontAsset>();

	private Vector2 m_PreferredSize;

	private int m_PreviousGenerationSettingsHash;

	private UnityEngine.TextCore.TextGenerationSettings m_CurrentGenerationSettings;

	private int m_PreviousLayoutSettingsHash;

	private UnityEngine.TextCore.TextGenerationSettings m_CurrentLayoutSettings;

	private TextInfo m_TextInfo;

	internal TextInfo textInfo
	{
		get
		{
			if (m_TextInfo == null)
			{
				m_TextInfo = new TextInfo();
			}
			return m_TextInfo;
		}
	}

	public static TextHandle New()
	{
		return new TextHandle
		{
			useLegacy = false,
			m_CurrentGenerationSettings = new UnityEngine.TextCore.TextGenerationSettings(),
			m_CurrentLayoutSettings = new UnityEngine.TextCore.TextGenerationSettings()
		};
	}

	private static FontAsset GetFontAsset(Font font)
	{
		FontAsset value = null;
		if (fontAssetCache.TryGetValue(font, out value) && value != null)
		{
			return value;
		}
		value = FontAsset.CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024);
		return fontAssetCache[font] = value;
	}

	internal bool IsTextInfoAllocated()
	{
		return m_TextInfo != null;
	}

	public Vector2 GetCursorPosition(CursorPositionStylePainterParameters parms, float scaling)
	{
		if (useLegacy)
		{
			return TextNative.GetCursorPosition(parms.GetTextNativeSettings(scaling), parms.rect, parms.cursorIndex);
		}
		return UnityEngine.TextCore.TextGenerator.GetCursorPosition(textInfo, parms.rect, parms.cursorIndex);
	}

	public float ComputeTextWidth(MeshGenerationContextUtils.TextParams parms, float scaling)
	{
		if (useLegacy)
		{
			return TextNative.ComputeTextWidth(MeshGenerationContextUtils.TextParams.GetTextNativeSettings(parms, scaling));
		}
		UpdatePreferredValues(parms);
		return m_PreferredSize.x;
	}

	public float ComputeTextHeight(MeshGenerationContextUtils.TextParams parms, float scaling)
	{
		if (useLegacy)
		{
			return TextNative.ComputeTextHeight(MeshGenerationContextUtils.TextParams.GetTextNativeSettings(parms, scaling));
		}
		UpdatePreferredValues(parms);
		return m_PreferredSize.y;
	}

	internal TextInfo Update(MeshGenerationContextUtils.TextParams parms, float pixelsPerPoint)
	{
		parms.rect = new Rect(Vector2.zero, parms.rect.size);
		int hashCode = parms.GetHashCode();
		if (m_PreviousGenerationSettingsHash == hashCode)
		{
			return textInfo;
		}
		UpdateGenerationSettingsCommon(parms, m_CurrentGenerationSettings);
		m_CurrentGenerationSettings.color = parms.fontColor;
		m_CurrentGenerationSettings.inverseYAxis = true;
		m_CurrentGenerationSettings.scale = pixelsPerPoint;
		textInfo.isDirty = true;
		UnityEngine.TextCore.TextGenerator.GenerateText(m_CurrentGenerationSettings, textInfo);
		m_PreviousGenerationSettingsHash = hashCode;
		return textInfo;
	}

	private void UpdatePreferredValues(MeshGenerationContextUtils.TextParams parms)
	{
		parms.rect = new Rect(Vector2.zero, parms.rect.size);
		int hashCode = parms.GetHashCode();
		if (m_PreviousLayoutSettingsHash != hashCode)
		{
			UpdateGenerationSettingsCommon(parms, m_CurrentLayoutSettings);
			m_PreferredSize = UnityEngine.TextCore.TextGenerator.GetPreferredValues(m_CurrentLayoutSettings, textInfo);
			m_PreviousLayoutSettingsHash = hashCode;
		}
	}

	private static void UpdateGenerationSettingsCommon(MeshGenerationContextUtils.TextParams painterParams, UnityEngine.TextCore.TextGenerationSettings settings)
	{
		settings.fontAsset = GetFontAsset(painterParams.font);
		settings.material = settings.fontAsset.material;
		Rect rect = painterParams.rect;
		if (float.IsNaN(rect.width))
		{
			rect.width = painterParams.wordWrapWidth;
		}
		settings.screenRect = rect;
		settings.text = (string.IsNullOrEmpty(painterParams.text) ? " " : painterParams.text);
		settings.fontSize = ((painterParams.fontSize > 0) ? painterParams.fontSize : painterParams.font.fontSize);
		settings.fontStyle = TextGeneratorUtilities.LegacyStyleToNewStyle(painterParams.fontStyle);
		settings.textAlignment = TextGeneratorUtilities.LegacyAlignmentToNewAlignment(painterParams.anchor);
		settings.wordWrap = painterParams.wordWrap;
		settings.richText = false;
		settings.overflowMode = TextOverflowMode.Overflow;
	}

	public static float ComputeTextScaling(Matrix4x4 worldMatrix, float pixelsPerPoint)
	{
		Vector3 vector = new Vector3(worldMatrix.m00, worldMatrix.m10, worldMatrix.m20);
		Vector3 vector2 = new Vector3(worldMatrix.m01, worldMatrix.m11, worldMatrix.m21);
		float num = (vector.magnitude + vector2.magnitude) / 2f;
		return num * pixelsPerPoint;
	}
}
