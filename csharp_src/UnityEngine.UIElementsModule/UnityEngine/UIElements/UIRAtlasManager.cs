using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements;

internal class UIRAtlasManager : IDisposable
{
	public struct ReadOnlyList<T> : IEnumerable<T>, IEnumerable
	{
		private List<T> m_List;

		public int Count => m_List.Count;

		public T this[int i] => m_List[i];

		public ReadOnlyList(List<T> list)
		{
			m_List = list;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return m_List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
		}
	}

	private static List<UIRAtlasManager> s_Instances = new List<UIRAtlasManager>();

	private static ReadOnlyList<UIRAtlasManager> s_InstancesreadOnly = new ReadOnlyList<UIRAtlasManager>(s_Instances);

	private int m_InitialSize;

	private UIRAtlasAllocator m_Allocator;

	private Dictionary<Texture2D, RectInt> m_UVs;

	private bool m_ForceReblitAll;

	private bool m_FloatFormat;

	private FilterMode m_FilterMode;

	private ColorSpace m_ColorSpace;

	private TextureBlitter m_Blitter;

	private int m_2SidePadding;

	private int m_1SidePadding;

	private static ProfilerMarker s_MarkerReset = new ProfilerMarker("UIR.AtlasManager.Reset");

	private static int s_GlobalResetVersion;

	private int m_ResetVersion = s_GlobalResetVersion;

	public int maxImageSize { get; }

	public RenderTextureFormat format { get; }

	public RenderTexture atlas { get; private set; }

	protected bool disposed { get; private set; }

	public static event Action<UIRAtlasManager> atlasManagerCreated;

	public static event Action<UIRAtlasManager> atlasManagerDisposed;

	public static ReadOnlyList<UIRAtlasManager> Instances()
	{
		return s_InstancesreadOnly;
	}

	public UIRAtlasManager(RenderTextureFormat format = RenderTextureFormat.ARGB32, FilterMode filterMode = FilterMode.Bilinear, int maxImageSize = 64, int initialSize = 64)
	{
		if (filterMode != FilterMode.Bilinear && filterMode != FilterMode.Point)
		{
			throw new NotSupportedException("The only supported atlas filter modes are point or bilinear");
		}
		this.format = format;
		this.maxImageSize = maxImageSize;
		m_FloatFormat = format == RenderTextureFormat.ARGBFloat;
		m_FilterMode = filterMode;
		m_UVs = new Dictionary<Texture2D, RectInt>(64);
		m_Blitter = new TextureBlitter(64);
		m_InitialSize = initialSize;
		m_2SidePadding = ((filterMode != FilterMode.Point) ? 2 : 0);
		m_1SidePadding = ((filterMode != FilterMode.Point) ? 1 : 0);
		Reset();
		s_Instances.Add(this);
		if (UIRAtlasManager.atlasManagerCreated != null)
		{
			UIRAtlasManager.atlasManagerCreated(this);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		s_Instances.Remove(this);
		if (disposed)
		{
			return;
		}
		if (disposing)
		{
			UIRUtility.Destroy(atlas);
			atlas = null;
			if (m_Allocator != null)
			{
				m_Allocator.Dispose();
				m_Allocator = null;
			}
			if (m_Blitter != null)
			{
				m_Blitter.Dispose();
				m_Blitter = null;
			}
			if (UIRAtlasManager.atlasManagerDisposed != null)
			{
				UIRAtlasManager.atlasManagerDisposed(this);
			}
		}
		disposed = true;
	}

	private static void LogDisposeError()
	{
		Debug.LogError("An attempt to use a disposed atlas manager has been detected.");
	}

	public static void MarkAllForReset()
	{
		s_GlobalResetVersion++;
	}

	public void MarkForReset()
	{
		m_ResetVersion = s_GlobalResetVersion - 1;
	}

	public bool RequiresReset()
	{
		return m_ResetVersion != s_GlobalResetVersion;
	}

	public bool IsReleased()
	{
		return atlas != null && !atlas.IsCreated();
	}

	public void Reset()
	{
		if (disposed)
		{
			LogDisposeError();
			return;
		}
		m_Blitter.Reset();
		m_UVs.Clear();
		m_Allocator = new UIRAtlasAllocator(m_InitialSize, 4096, m_1SidePadding);
		m_ForceReblitAll = false;
		m_ColorSpace = QualitySettings.activeColorSpace;
		UIRUtility.Destroy(atlas);
		m_ResetVersion = s_GlobalResetVersion;
	}

	public bool TryGetLocation(Texture2D image, out RectInt uvs)
	{
		uvs = default(RectInt);
		if (disposed)
		{
			LogDisposeError();
			return false;
		}
		if (image == null)
		{
			return false;
		}
		if (m_UVs.TryGetValue(image, out uvs))
		{
			return true;
		}
		if (!IsTextureValid(image))
		{
			return false;
		}
		if (!AllocateRect(image.width, image.height, out uvs))
		{
			return false;
		}
		m_UVs[image] = uvs;
		m_Blitter.QueueBlit(image, new RectInt(0, 0, image.width, image.height), new Vector2Int(uvs.x, uvs.y), addBorder: true, Color.white);
		return true;
	}

	public bool AllocateRect(int width, int height, out RectInt uvs)
	{
		if (!m_Allocator.TryAllocate(width + m_2SidePadding, height + m_2SidePadding, out uvs))
		{
			return false;
		}
		uvs = new RectInt(uvs.x + m_1SidePadding, uvs.y + m_1SidePadding, width, height);
		return true;
	}

	public void EnqueueBlit(Texture image, RectInt srcRect, int x, int y, bool addBorder, Color tint)
	{
		m_Blitter.QueueBlit(image, srcRect, new Vector2Int(x, y), addBorder, tint);
	}

	public static bool IsTextureFormatSupported(TextureFormat format)
	{
		switch (format)
		{
		case TextureFormat.Alpha8:
		case TextureFormat.ARGB4444:
		case TextureFormat.RGB24:
		case TextureFormat.RGBA32:
		case TextureFormat.ARGB32:
		case TextureFormat.RGB565:
		case TextureFormat.R16:
		case TextureFormat.DXT1:
		case TextureFormat.DXT5:
		case TextureFormat.RGBA4444:
		case TextureFormat.BGRA32:
		case TextureFormat.BC7:
		case TextureFormat.BC4:
		case TextureFormat.BC5:
		case TextureFormat.DXT1Crunched:
		case TextureFormat.DXT5Crunched:
		case TextureFormat.PVRTC_RGB2:
		case TextureFormat.PVRTC_RGBA2:
		case TextureFormat.PVRTC_RGB4:
		case TextureFormat.PVRTC_RGBA4:
		case TextureFormat.ETC_RGB4:
		case TextureFormat.EAC_R:
		case TextureFormat.EAC_R_SIGNED:
		case TextureFormat.EAC_RG:
		case TextureFormat.EAC_RG_SIGNED:
		case TextureFormat.ETC2_RGB:
		case TextureFormat.ETC2_RGBA1:
		case TextureFormat.ETC2_RGBA8:
		case TextureFormat.ASTC_4x4:
		case TextureFormat.ASTC_5x5:
		case TextureFormat.ASTC_6x6:
		case TextureFormat.ASTC_8x8:
		case TextureFormat.ASTC_10x10:
		case TextureFormat.ASTC_12x12:
		case TextureFormat.ASTC_RGBA_4x4:
		case TextureFormat.ASTC_RGBA_5x5:
		case TextureFormat.ASTC_RGBA_6x6:
		case TextureFormat.ASTC_RGBA_8x8:
		case TextureFormat.ASTC_RGBA_10x10:
		case TextureFormat.ASTC_RGBA_12x12:
		case TextureFormat.ETC_RGB4_3DS:
		case TextureFormat.ETC_RGBA8_3DS:
		case TextureFormat.RG16:
		case TextureFormat.R8:
		case TextureFormat.ETC_RGB4Crunched:
		case TextureFormat.ETC2_RGBA8Crunched:
			return true;
		case TextureFormat.RHalf:
		case TextureFormat.RGHalf:
		case TextureFormat.RGBAHalf:
		case TextureFormat.RFloat:
		case TextureFormat.RGFloat:
		case TextureFormat.RGBAFloat:
		case TextureFormat.YUY2:
		case TextureFormat.RGB9e5Float:
		case TextureFormat.BC6H:
		case TextureFormat.ASTC_HDR_4x4:
		case TextureFormat.ASTC_HDR_5x5:
		case TextureFormat.ASTC_HDR_6x6:
		case TextureFormat.ASTC_HDR_8x8:
		case TextureFormat.ASTC_HDR_10x10:
		case TextureFormat.ASTC_HDR_12x12:
		case TextureFormat.RG32:
		case TextureFormat.RGB48:
		case TextureFormat.RGBA64:
			return false;
		default:
			return false;
		}
	}

	private bool IsTextureValid(Texture2D image)
	{
		if (image.isReadable)
		{
			return false;
		}
		if (image.width > maxImageSize || image.height > maxImageSize)
		{
			return false;
		}
		if (!IsTextureFormatSupported(image.format))
		{
			return false;
		}
		if (!m_FloatFormat && m_ColorSpace == ColorSpace.Linear && image.activeTextureColorSpace != ColorSpace.Gamma)
		{
			return false;
		}
		if (SystemInfo.graphicsShaderLevel >= 35)
		{
			if (image.filterMode != FilterMode.Bilinear && image.filterMode != FilterMode.Point)
			{
				return false;
			}
		}
		else if (m_FilterMode != image.filterMode)
		{
			return false;
		}
		if (image.wrapMode != TextureWrapMode.Clamp)
		{
			return false;
		}
		return true;
	}

	public void Commit()
	{
		if (disposed)
		{
			LogDisposeError();
			return;
		}
		UpdateAtlasTexture();
		if (m_ForceReblitAll)
		{
			m_ForceReblitAll = false;
			m_Blitter.Reset();
			foreach (KeyValuePair<Texture2D, RectInt> uV in m_UVs)
			{
				m_Blitter.QueueBlit(uV.Key, new RectInt(0, 0, uV.Key.width, uV.Key.height), new Vector2Int(uV.Value.x, uV.Value.y), addBorder: true, Color.white);
			}
		}
		m_Blitter.Commit(atlas);
	}

	private void UpdateAtlasTexture()
	{
		if (atlas == null)
		{
			if (m_UVs.Count > m_Blitter.queueLength)
			{
				m_ForceReblitAll = true;
			}
			atlas = CreateAtlasTexture();
		}
		else if (atlas.width != m_Allocator.physicalWidth || atlas.height != m_Allocator.physicalHeight)
		{
			RenderTexture dst = CreateAtlasTexture();
			m_Blitter.BlitOneNow(dst, atlas, new RectInt(0, 0, atlas.width, atlas.height), new Vector2Int(0, 0), addBorder: false, Color.white);
			UIRUtility.Destroy(atlas);
			atlas = dst;
		}
	}

	private RenderTexture CreateAtlasTexture()
	{
		if (m_Allocator.physicalWidth == 0 || m_Allocator.physicalHeight == 0)
		{
			return null;
		}
		return new RenderTexture(m_Allocator.physicalWidth, m_Allocator.physicalHeight, 0, format)
		{
			hideFlags = HideFlags.HideAndDontSave,
			name = "UIR Atlas " + Random.Range(int.MinValue, int.MaxValue),
			filterMode = m_FilterMode
		};
	}
}
