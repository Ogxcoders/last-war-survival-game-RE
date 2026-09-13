using System;
using Unity.Profiling;

namespace UnityEngine.UIElements.UIR;

internal abstract class BaseShaderInfoStorage : IDisposable
{
	protected static int s_TextureCounter;

	internal static ProfilerMarker s_MarkerCopyTexture = new ProfilerMarker("UIR.ShaderInfoStorage.CopyTexture");

	internal static ProfilerMarker s_MarkerGetTextureData = new ProfilerMarker("UIR.ShaderInfoStorage.GetTextureData");

	internal static ProfilerMarker s_MarkerUpdateTexture = new ProfilerMarker("UIR.ShaderInfoStorage.UpdateTexture");

	public abstract Texture2D texture { get; }

	protected bool disposed { get; private set; }

	public abstract bool AllocateRect(int width, int height, out RectInt uvs);

	public abstract void SetTexel(int x, int y, Color color);

	public abstract void UpdateTexture();

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (!disposing)
			{
			}
			disposed = true;
		}
	}
}
