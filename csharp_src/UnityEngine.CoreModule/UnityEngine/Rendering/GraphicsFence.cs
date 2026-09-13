using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[UsedByNativeCode]
[NativeHeader("Runtime/Graphics/GPUFence.h")]
public struct GraphicsFence
{
	internal IntPtr m_Ptr;

	internal int m_Version;

	internal GraphicsFenceType m_FenceType;

	public bool passed
	{
		get
		{
			Validate();
			if (!SystemInfo.supportsGraphicsFence || (m_FenceType == GraphicsFenceType.AsyncQueueSynchronisation && !SystemInfo.supportsAsyncCompute))
			{
				throw new NotSupportedException("Cannot determine if this GraphicsFence has passed as this platform has not implemented GraphicsFences.");
			}
			if (!IsFencePending())
			{
				return true;
			}
			return HasFencePassed_Internal(m_Ptr);
		}
	}

	internal static SynchronisationStageFlags TranslateSynchronizationStageToFlags(SynchronisationStage s)
	{
		return (s == SynchronisationStage.VertexProcessing) ? SynchronisationStageFlags.VertexProcessing : SynchronisationStageFlags.PixelProcessing;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GPUFenceInternals::HasFencePassed_Internal")]
	private static extern bool HasFencePassed_Internal(IntPtr fencePtr);

	internal void InitPostAllocation()
	{
		if (m_Ptr == IntPtr.Zero)
		{
			if (SystemInfo.supportsGraphicsFence)
			{
				throw new NullReferenceException("The internal fence ptr is null, this should not be possible for fences that have been correctly constructed using Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
			}
			m_Version = GetPlatformNotSupportedVersion();
		}
		else
		{
			m_Version = GetVersionNumber(m_Ptr);
		}
	}

	internal bool IsFencePending()
	{
		if (m_Ptr == IntPtr.Zero)
		{
			return false;
		}
		return m_Version == GetVersionNumber(m_Ptr);
	}

	internal void Validate()
	{
		if (m_Version == 0 || (SystemInfo.supportsGraphicsFence && m_Version == GetPlatformNotSupportedVersion()))
		{
			throw new InvalidOperationException("This GraphicsFence object has not been correctly constructed see Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
		}
	}

	private int GetPlatformNotSupportedVersion()
	{
		return -1;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GPUFenceInternals::GetVersionNumber")]
	[NativeThrows]
	private static extern int GetVersionNumber(IntPtr fencePtr);
}
