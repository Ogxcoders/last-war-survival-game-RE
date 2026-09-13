using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR;

internal class VectorImageManager : IDisposable
{
	private static ProfilerMarker s_MarkerRegister = new ProfilerMarker("UIR.VectorImageManager.Register");

	private static ProfilerMarker s_MarkerUnregister = new ProfilerMarker("UIR.VectorImageManager.Unregister");

	private readonly UIRAtlasManager m_AtlasManager;

	private Dictionary<VectorImage, VectorImageRenderInfo> m_Registered;

	private VectorImageRenderInfoPool m_RenderInfoPool;

	private GradientRemapPool m_GradientRemapPool;

	private GradientSettingsAtlas m_GradientSettingsAtlas;

	private bool m_LoggedExhaustedSettingsAtlas;

	private static int s_GlobalResetVersion;

	private int m_ResetVersion = s_GlobalResetVersion;

	public Texture2D atlas => m_GradientSettingsAtlas?.atlas;

	protected bool disposed { get; private set; }

	public VectorImageManager(UIRAtlasManager atlasManager)
	{
		m_AtlasManager = atlasManager;
		m_Registered = new Dictionary<VectorImage, VectorImageRenderInfo>(32);
		m_RenderInfoPool = new VectorImageRenderInfoPool();
		m_GradientRemapPool = new GradientRemapPool();
		m_GradientSettingsAtlas = new GradientSettingsAtlas();
	}

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
				m_Registered.Clear();
				m_RenderInfoPool.Clear();
				m_GradientRemapPool.Clear();
				m_GradientSettingsAtlas.Dispose();
			}
			disposed = true;
		}
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

	public void Reset()
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		m_Registered.Clear();
		m_RenderInfoPool.Clear();
		m_GradientRemapPool.Clear();
		m_GradientSettingsAtlas.Reset();
		m_ResetVersion = s_GlobalResetVersion;
	}

	public void Commit()
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		else
		{
			m_GradientSettingsAtlas.Commit();
		}
	}

	public GradientRemap AddUser(VectorImage vi)
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return null;
		}
		if (vi == null)
		{
			return null;
		}
		if (m_Registered.TryGetValue(vi, out var value))
		{
			value.useCount++;
		}
		else
		{
			value = Register(vi);
		}
		return value.firstGradientRemap;
	}

	public void RemoveUser(VectorImage vi)
	{
		VectorImageRenderInfo value;
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
		}
		else if (!(vi == null) && m_Registered.TryGetValue(vi, out value))
		{
			value.useCount--;
			if (value.useCount == 0)
			{
				Unregister(vi, value);
			}
		}
	}

	private VectorImageRenderInfo Register(VectorImage vi)
	{
		VectorImageRenderInfo vectorImageRenderInfo = m_RenderInfoPool.Get();
		vectorImageRenderInfo.useCount = 1;
		m_Registered[vi] = vectorImageRenderInfo;
		GradientSettings[] settings = vi.settings;
		if (settings != null && settings.Length != 0)
		{
			int num = vi.settings.Length;
			Alloc alloc = m_GradientSettingsAtlas.Add(num);
			if (alloc.size != 0)
			{
				if (m_AtlasManager.TryGetLocation(vi.atlas, out var uvs))
				{
					GradientRemap gradientRemap = null;
					for (int i = 0; i < num; i++)
					{
						GradientRemap gradientRemap2 = m_GradientRemapPool.Get();
						if (i > 0)
						{
							gradientRemap.next = gradientRemap2;
						}
						else
						{
							vectorImageRenderInfo.firstGradientRemap = gradientRemap2;
						}
						gradientRemap = gradientRemap2;
						gradientRemap2.origIndex = i;
						gradientRemap2.destIndex = (int)alloc.start + i;
						GradientSettings gradientSettings = vi.settings[i];
						RectInt location = gradientSettings.location;
						location.x += uvs.x;
						location.y += uvs.y;
						gradientRemap2.location = location;
						gradientRemap2.isAtlassed = true;
					}
					m_GradientSettingsAtlas.Write(alloc, vi.settings, vectorImageRenderInfo.firstGradientRemap);
				}
				else
				{
					GradientRemap gradientRemap3 = null;
					for (int j = 0; j < num; j++)
					{
						GradientRemap gradientRemap4 = m_GradientRemapPool.Get();
						if (j > 0)
						{
							gradientRemap3.next = gradientRemap4;
						}
						else
						{
							vectorImageRenderInfo.firstGradientRemap = gradientRemap4;
						}
						gradientRemap3 = gradientRemap4;
						gradientRemap4.origIndex = j;
						gradientRemap4.destIndex = (int)alloc.start + j;
						gradientRemap4.isAtlassed = false;
					}
					m_GradientSettingsAtlas.Write(alloc, vi.settings, null);
				}
			}
			else if (!m_LoggedExhaustedSettingsAtlas)
			{
				Debug.LogError("Exhausted max gradient settings (" + m_GradientSettingsAtlas.length + ") for atlas: " + m_GradientSettingsAtlas.atlas?.name);
				m_LoggedExhaustedSettingsAtlas = true;
			}
		}
		return vectorImageRenderInfo;
	}

	private void Unregister(VectorImage vi, VectorImageRenderInfo renderInfo)
	{
		if (renderInfo.gradientSettingsAlloc.size != 0)
		{
			m_GradientSettingsAtlas.Remove(renderInfo.gradientSettingsAlloc);
		}
		GradientRemap gradientRemap = renderInfo.firstGradientRemap;
		while (gradientRemap != null)
		{
			GradientRemap next = gradientRemap.next;
			m_GradientRemapPool.Return(gradientRemap);
			gradientRemap = next;
		}
		m_Registered.Remove(vi);
		m_RenderInfoPool.Return(renderInfo);
	}
}
