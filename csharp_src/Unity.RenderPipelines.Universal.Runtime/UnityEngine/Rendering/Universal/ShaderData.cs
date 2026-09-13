using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering.Universal;

internal class ShaderData : IDisposable
{
	private static ShaderData m_Instance;

	private ComputeBuffer m_LightDataBuffer;

	private ComputeBuffer m_LightIndicesBuffer;

	private ComputeBuffer m_ShadowDataBuffer;

	private ComputeBuffer m_ShadowIndicesBuffer;

	internal static ShaderData instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new ShaderData();
			}
			return m_Instance;
		}
	}

	private ShaderData()
	{
	}

	public void Dispose()
	{
		DisposeBuffer(ref m_LightDataBuffer);
		DisposeBuffer(ref m_LightIndicesBuffer);
		DisposeBuffer(ref m_ShadowDataBuffer);
		DisposeBuffer(ref m_ShadowIndicesBuffer);
	}

	internal ComputeBuffer GetLightDataBuffer(int size)
	{
		return GetOrUpdateBuffer<ShaderInput.LightData>(ref m_LightDataBuffer, size);
	}

	internal ComputeBuffer GetLightIndicesBuffer(int size)
	{
		return GetOrUpdateBuffer<int>(ref m_LightIndicesBuffer, size);
	}

	internal ComputeBuffer GetShadowDataBuffer(int size)
	{
		return GetOrUpdateBuffer<ShaderInput.ShadowData>(ref m_ShadowDataBuffer, size);
	}

	internal ComputeBuffer GetShadowIndicesBuffer(int size)
	{
		return GetOrUpdateBuffer<int>(ref m_ShadowIndicesBuffer, size);
	}

	private ComputeBuffer GetOrUpdateBuffer<T>(ref ComputeBuffer buffer, int size) where T : struct
	{
		if (buffer == null)
		{
			buffer = new ComputeBuffer(size, Marshal.SizeOf<T>());
		}
		else if (size > buffer.count)
		{
			buffer.Dispose();
			buffer = new ComputeBuffer(size, Marshal.SizeOf<T>());
		}
		return buffer;
	}

	private void DisposeBuffer(ref ComputeBuffer buffer)
	{
		if (buffer != null)
		{
			buffer.Dispose();
			buffer = null;
		}
	}
}
