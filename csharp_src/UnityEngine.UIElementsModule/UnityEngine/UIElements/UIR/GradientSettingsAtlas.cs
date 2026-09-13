#define UNITY_ASSERTIONS
using System;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR;

internal class GradientSettingsAtlas : IDisposable
{
	private struct RawTexture
	{
		public Color32[] rgba;

		public int width;

		public int height;

		public void WriteRawInt2Packed(int v0, int v1, int destX, int destY)
		{
			byte b = (byte)(v0 / 255);
			byte g = (byte)(v0 - b * 255);
			byte b2 = (byte)(v1 / 255);
			byte a = (byte)(v1 - b2 * 255);
			int num = destY * width + destX;
			rgba[num] = new Color32(b, g, b2, a);
		}

		public void WriteRawFloat4Packed(float f0, float f1, float f2, float f3, int destX, int destY)
		{
			byte r = (byte)(f0 * 255f + 0.5f);
			byte g = (byte)(f1 * 255f + 0.5f);
			byte b = (byte)(f2 * 255f + 0.5f);
			byte a = (byte)(f3 * 255f + 0.5f);
			int num = destY * width + destX;
			rgba[num] = new Color32(r, g, b, a);
		}
	}

	private static ProfilerMarker s_MarkerWrite = new ProfilerMarker("UIR.GradientSettingsAtlas.Write");

	private static ProfilerMarker s_MarkerCommit = new ProfilerMarker("UIR.GradientSettingsAtlas.Commit");

	private readonly int m_Length;

	private readonly int m_ElemWidth;

	private BestFitAllocator m_Allocator;

	private Texture2D m_Atlas;

	private RawTexture m_RawAtlas;

	internal int length => m_Length;

	protected bool disposed { get; private set; }

	public Texture2D atlas => m_Atlas;

	public bool MustCommit { get; private set; }

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				UIRUtility.Destroy(m_Atlas);
			}
			disposed = true;
		}
	}

	public GradientSettingsAtlas(int length = 4096)
	{
		m_Length = length;
		m_ElemWidth = 3;
		Reset();
	}

	public void Reset()
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		m_Allocator = new BestFitAllocator((uint)m_Length);
		UIRUtility.Destroy(m_Atlas);
		m_RawAtlas = default(RawTexture);
		MustCommit = false;
	}

	public Alloc Add(int count)
	{
		Debug.Assert(count > 0);
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return default(Alloc);
		}
		return m_Allocator.Allocate((uint)count);
	}

	public void Remove(Alloc alloc)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		else
		{
			m_Allocator.Free(alloc);
		}
	}

	public void Write(Alloc alloc, GradientSettings[] settings, GradientRemap remap)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		if (m_RawAtlas.rgba == null)
		{
			m_RawAtlas = new RawTexture
			{
				rgba = new Color32[m_ElemWidth * m_Length],
				width = m_ElemWidth,
				height = m_Length
			};
			int num = m_ElemWidth * m_Length;
			for (int i = 0; i < num; i++)
			{
				m_RawAtlas.rgba[i] = Color.black;
			}
		}
		int num2 = (int)alloc.start;
		int j = 0;
		for (int num3 = settings.Length; j < num3; j++)
		{
			int num4 = 0;
			GradientSettings gradientSettings = settings[j];
			Debug.Assert(remap == null || num2 == remap.destIndex);
			if (gradientSettings.gradientType == GradientType.Radial)
			{
				Vector2 radialFocus = gradientSettings.radialFocus;
				radialFocus += Vector2.one;
				radialFocus /= 2f;
				radialFocus.y = 1f - radialFocus.y;
				m_RawAtlas.WriteRawFloat4Packed(0.003921569f, (float)gradientSettings.addressMode / 255f, radialFocus.x, radialFocus.y, num4++, num2);
			}
			else if (gradientSettings.gradientType == GradientType.Linear)
			{
				m_RawAtlas.WriteRawFloat4Packed(0f, (float)gradientSettings.addressMode / 255f, 0f, 0f, num4++, num2);
			}
			Vector2Int vector2Int = new Vector2Int(gradientSettings.location.x, gradientSettings.location.y);
			Vector2 vector = new Vector2(gradientSettings.location.width - 1, gradientSettings.location.height - 1);
			if (remap != null)
			{
				vector2Int = new Vector2Int(remap.location.x, remap.location.y);
				vector = new Vector2(remap.location.width - 1, remap.location.height - 1);
			}
			m_RawAtlas.WriteRawInt2Packed(vector2Int.x, vector2Int.y, num4++, num2);
			m_RawAtlas.WriteRawInt2Packed((int)vector.x, (int)vector.y, num4++, num2);
			remap = remap?.next;
			num2++;
		}
		MustCommit = true;
	}

	public void Commit()
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		else if (MustCommit)
		{
			PrepareAtlas();
			m_Atlas.SetPixels32(m_RawAtlas.rgba);
			m_Atlas.Apply();
			MustCommit = false;
		}
	}

	private void PrepareAtlas()
	{
		if (!(m_Atlas != null))
		{
			m_Atlas = new Texture2D(m_ElemWidth, m_Length, TextureFormat.ARGB32, 0, linear: true)
			{
				hideFlags = HideFlags.HideAndDontSave,
				name = "GradientSettings " + Random.Range(int.MinValue, int.MaxValue),
				filterMode = FilterMode.Point
			};
		}
	}
}
