#define UNITY_ASSERTIONS
using System;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR;

internal class ShaderInfoStorage<T> : BaseShaderInfoStorage where T : struct
{
	private readonly int m_InitialSize;

	private readonly int m_MaxSize;

	private readonly TextureFormat m_Format;

	private readonly Func<Color, T> m_Convert;

	private UIRAtlasAllocator m_Allocator;

	private Texture2D m_Texture;

	private NativeArray<T> m_Texels;

	public override Texture2D texture => m_Texture;

	public ShaderInfoStorage(TextureFormat format, Func<Color, T> convert, int initialSize = 64, int maxSize = 4096)
	{
		Debug.Assert(maxSize <= SystemInfo.maxTextureSize);
		Debug.Assert(initialSize <= maxSize);
		Debug.Assert(Mathf.IsPowerOfTwo(initialSize));
		Debug.Assert(Mathf.IsPowerOfTwo(maxSize));
		Debug.Assert(convert != null);
		m_InitialSize = initialSize;
		m_MaxSize = maxSize;
		m_Format = format;
		m_Convert = convert;
	}

	protected override void Dispose(bool disposing)
	{
		if (!base.disposed && disposing)
		{
			UIRUtility.Destroy(m_Texture);
			m_Texture = null;
			m_Texels = default(NativeArray<T>);
			m_Allocator?.Dispose();
			m_Allocator = null;
		}
		base.Dispose(disposing);
	}

	public override bool AllocateRect(int width, int height, out RectInt uvs)
	{
		if (base.disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			uvs = default(RectInt);
			return false;
		}
		if (m_Allocator == null)
		{
			m_Allocator = new UIRAtlasAllocator(m_InitialSize, m_MaxSize, 0);
		}
		if (!m_Allocator.TryAllocate(width, height, out uvs))
		{
			return false;
		}
		uvs = new RectInt(uvs.x, uvs.y, width, height);
		CreateOrExpandTexture();
		return true;
	}

	public override void SetTexel(int x, int y, Color color)
	{
		if (base.disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		if (!m_Texels.IsCreated)
		{
			m_Texels = m_Texture.GetRawTextureData<T>();
		}
		m_Texels[x + y * m_Texture.width] = m_Convert(color);
	}

	public override void UpdateTexture()
	{
		if (base.disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		else if (!(m_Texture == null) && m_Texels.IsCreated)
		{
			m_Texture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			m_Texels = default(NativeArray<T>);
		}
	}

	private void CreateOrExpandTexture()
	{
		int physicalWidth = m_Allocator.physicalWidth;
		int physicalHeight = m_Allocator.physicalHeight;
		bool flag = false;
		if (m_Texture != null)
		{
			if (m_Texture.width == physicalWidth && m_Texture.height == physicalHeight)
			{
				return;
			}
			flag = true;
		}
		Texture2D texture2D = new Texture2D(m_Allocator.physicalWidth, m_Allocator.physicalHeight, m_Format, mipChain: false)
		{
			name = "UIR Shader Info " + BaseShaderInfoStorage.s_TextureCounter++,
			hideFlags = HideFlags.HideAndDontSave
		};
		if (flag)
		{
			NativeArray<T> src = (m_Texels.IsCreated ? m_Texels : m_Texture.GetRawTextureData<T>());
			NativeArray<T> rawTextureData = texture2D.GetRawTextureData<T>();
			CpuBlit(src, m_Texture.width, m_Texture.height, rawTextureData, texture2D.width, texture2D.height);
			m_Texels = rawTextureData;
		}
		else
		{
			m_Texels = default(NativeArray<T>);
		}
		UIRUtility.Destroy(m_Texture);
		m_Texture = texture2D;
	}

	private static void CpuBlit(NativeArray<T> src, int srcWidth, int srcHeight, NativeArray<T> dst, int dstWidth, int dstHeight)
	{
		Debug.Assert(dstWidth >= srcWidth && dstHeight >= srcHeight);
		int num = dstWidth - srcWidth;
		int num2 = dstHeight - srcHeight;
		int num3 = srcWidth * srcHeight;
		int i = 0;
		int num4 = 0;
		int num5 = srcWidth;
		while (i < num3)
		{
			for (; i < num5; i++)
			{
				dst[num4] = src[i];
				num4++;
			}
			num5 += srcWidth;
			num4 += num;
		}
	}
}
