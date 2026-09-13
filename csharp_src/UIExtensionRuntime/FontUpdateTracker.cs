using System.Collections.Generic;
using UnityEngine;

internal static class FontUpdateTracker
{
	private class FontMaterial
	{
		public Font font;

		public Material matZTestOn;

		public Material matZTestOff;
	}

	private static Dictionary<Font, HashSet<SuperTextMesh>> m_Tracked = new Dictionary<Font, HashSet<SuperTextMesh>>();

	private static Dictionary<Font, FontMaterial> m_FontMaterial = new Dictionary<Font, FontMaterial>();

	public static void TrackText(SuperTextMesh t)
	{
		if (t.font == null)
		{
			return;
		}
		m_Tracked.TryGetValue(t.font, out var value);
		if (value == null)
		{
			if (m_Tracked.Count == 0)
			{
				Font.textureRebuilt += RebuildForFont;
			}
			value = new HashSet<SuperTextMesh>();
			m_Tracked.Add(t.font, value);
		}
		if (!value.Contains(t))
		{
			value.Add(t);
		}
		if (!m_FontMaterial.TryGetValue(t.font, out var value2) && t.textMat != null)
		{
			value2 = new FontMaterial();
			value2.matZTestOn = new Material(t.textMat);
			value2.matZTestOn.SetFloat("_ZTest", 4f);
			value2.matZTestOff = new Material(t.textMat);
			value2.matZTestOff.SetFloat("_ZTest", 0f);
			m_FontMaterial.Add(t.font, value2);
		}
	}

	private static void RebuildForFont(Font f)
	{
		m_Tracked.TryGetValue(f, out var value);
		if (value == null)
		{
			return;
		}
		foreach (SuperTextMesh item in value)
		{
			item.FontTextureChanged();
		}
	}

	public static Material GetMaterial(SuperTextMesh t)
	{
		if (m_FontMaterial.TryGetValue(t.font, out var value))
		{
			if (!t.ztest)
			{
				return value.matZTestOff;
			}
			return value.matZTestOn;
		}
		return null;
	}

	public static void UntrackText(SuperTextMesh t)
	{
		if (t.font == null)
		{
			return;
		}
		m_Tracked.TryGetValue(t.font, out var value);
		if (value == null)
		{
			return;
		}
		value.Remove(t);
		if (value.Count == 0)
		{
			m_Tracked.Remove(t.font);
			if (m_FontMaterial.TryGetValue(t.font, out var value2))
			{
				DestroyImmediate(value2.matZTestOn);
				DestroyImmediate(value2.matZTestOff);
				m_FontMaterial.Remove(t.font);
			}
			if (m_Tracked.Count == 0)
			{
				Font.textureRebuilt -= RebuildForFont;
			}
		}
	}

	private static void DestroyImmediate(Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(obj);
			}
			else
			{
				Object.Destroy(obj);
			}
		}
	}
}
