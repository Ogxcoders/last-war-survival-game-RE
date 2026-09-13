using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR;

internal class TextureBlitter : IDisposable
{
	private struct BlitInfo
	{
		public Texture src;

		public RectInt srcRect;

		public Vector2Int dstPos;

		public int border;

		public Color tint;
	}

	private const int k_TextureSlotCount = 8;

	private static readonly int[] k_TextureIds;

	private static ProfilerMarker s_CommitSampler;

	private BlitInfo[] m_SingleBlit = new BlitInfo[1];

	private Material m_BlitMaterial;

	private RectInt m_Viewport;

	private RenderTexture m_PrevRT;

	private List<BlitInfo> m_PendingBlits;

	protected bool disposed { get; private set; }

	public int queueLength => m_PendingBlits.Count;

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
				UIRUtility.Destroy(m_BlitMaterial);
				m_BlitMaterial = null;
			}
			disposed = true;
		}
	}

	static TextureBlitter()
	{
		s_CommitSampler = new ProfilerMarker("UIR.TextureBlitter.Commit");
		k_TextureIds = new int[8];
		for (int i = 0; i < 8; i++)
		{
			k_TextureIds[i] = Shader.PropertyToID("_MainTex" + i);
		}
	}

	public TextureBlitter(int capacity = 512)
	{
		m_PendingBlits = new List<BlitInfo>(capacity);
	}

	public void QueueBlit(Texture src, RectInt srcRect, Vector2Int dstPos, bool addBorder, Color tint)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		m_PendingBlits.Add(new BlitInfo
		{
			src = src,
			srcRect = srcRect,
			dstPos = dstPos,
			border = (addBorder ? 1 : 0),
			tint = tint
		});
	}

	public void BlitOneNow(RenderTexture dst, Texture src, RectInt srcRect, Vector2Int dstPos, bool addBorder, Color tint)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		m_SingleBlit[0] = new BlitInfo
		{
			src = src,
			srcRect = srcRect,
			dstPos = dstPos,
			border = (addBorder ? 1 : 0),
			tint = tint
		};
		BeginBlit(dst);
		DoBlit(m_SingleBlit, 0);
		EndBlit();
	}

	public void Commit(RenderTexture dst)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		else if (m_PendingBlits.Count != 0)
		{
			BeginBlit(dst);
			for (int i = 0; i < m_PendingBlits.Count; i += 8)
			{
				DoBlit(m_PendingBlits, i);
			}
			EndBlit();
			m_PendingBlits.Clear();
		}
	}

	public void Reset()
	{
		m_PendingBlits.Clear();
	}

	private void BeginBlit(RenderTexture dst)
	{
		if (m_BlitMaterial == null)
		{
			Shader shader = Shader.Find("Hidden/Internal-UIRAtlasBlitCopy");
			m_BlitMaterial = new Material(shader);
			m_BlitMaterial.hideFlags |= HideFlags.DontSaveInEditor;
		}
		m_Viewport = Utility.GetActiveViewport();
		m_PrevRT = RenderTexture.active;
		GL.LoadPixelMatrix(0f, dst.width, 0f, dst.height);
		Graphics.SetRenderTarget(dst);
	}

	private void DoBlit(IList<BlitInfo> blitInfos, int startIndex)
	{
		int num = Mathf.Min(startIndex + 8, blitInfos.Count);
		int num2 = startIndex;
		int num3 = 0;
		while (num2 < num)
		{
			Texture src = blitInfos[num2].src;
			if (src != null)
			{
				m_BlitMaterial.SetTexture(k_TextureIds[num3], src);
			}
			num2++;
			num3++;
		}
		m_BlitMaterial.SetPass(0);
		GL.Begin(7);
		int num4 = startIndex;
		int num5 = 0;
		while (num4 < num)
		{
			BlitInfo blitInfo = blitInfos[num4];
			float num6 = 1f / (float)blitInfo.src.width;
			float num7 = 1f / (float)blitInfo.src.height;
			float x = blitInfo.dstPos.x - blitInfo.border;
			float y = blitInfo.dstPos.y - blitInfo.border;
			float x2 = blitInfo.dstPos.x + blitInfo.srcRect.width + blitInfo.border;
			float y2 = blitInfo.dstPos.y + blitInfo.srcRect.height + blitInfo.border;
			float x3 = (float)(blitInfo.srcRect.x - blitInfo.border) * num6;
			float y3 = (float)(blitInfo.srcRect.y - blitInfo.border) * num7;
			float x4 = (float)(blitInfo.srcRect.xMax + blitInfo.border) * num6;
			float y4 = (float)(blitInfo.srcRect.yMax + blitInfo.border) * num7;
			GL.Color(blitInfo.tint);
			GL.TexCoord3(x3, y3, num5);
			GL.Vertex3(x, y, 0f);
			GL.Color(blitInfo.tint);
			GL.TexCoord3(x3, y4, num5);
			GL.Vertex3(x, y2, 0f);
			GL.Color(blitInfo.tint);
			GL.TexCoord3(x4, y4, num5);
			GL.Vertex3(x2, y2, 0f);
			GL.Color(blitInfo.tint);
			GL.TexCoord3(x4, y3, num5);
			GL.Vertex3(x2, y, 0f);
			num4++;
			num5++;
		}
		GL.End();
	}

	private void EndBlit()
	{
		Graphics.SetRenderTarget(m_PrevRT);
		GL.Viewport(new Rect(m_Viewport.x, m_Viewport.y, m_Viewport.width, m_Viewport.height));
	}
}
