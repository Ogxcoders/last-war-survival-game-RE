using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering;

[NativeHeader("Runtime/Export/RenderPipeline/ScriptableRenderPipeline.bindings.h")]
[NativeHeader("Runtime/Export/RenderPipeline/ScriptableRenderContext.bindings.h")]
[NativeHeader("Runtime/Graphics/ScriptableRenderLoop/ScriptableDrawRenderersUtility.h")]
[NativeType("Runtime/Graphics/ScriptableRenderLoop/ScriptableRenderContext.h")]
[NativeHeader("Modules/UI/CanvasManager.h")]
[NativeHeader("Modules/UI/Canvas.h")]
public struct ScriptableRenderContext : IEquatable<ScriptableRenderContext>
{
	private IntPtr m_Ptr;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptableRenderContext::BeginRenderPass")]
	private static extern void BeginRenderPass_Internal(IntPtr self, int width, int height, int samples, IntPtr colors, int colorCount, int depthAttachmentIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptableRenderContext::BeginSubPass")]
	private static extern void BeginSubPass_Internal(IntPtr self, IntPtr colors, int colorCount, IntPtr inputs, int inputCount, bool isDepthReadOnly);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptableRenderContext::EndSubPass")]
	private static extern void EndSubPass_Internal(IntPtr self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptableRenderContext::EndRenderPass")]
	private static extern void EndRenderPass_Internal(IntPtr self);

	[FreeFunction("ScriptableRenderPipeline_Bindings::Internal_Cull")]
	private static void Internal_Cull(ref ScriptableCullingParameters parameters, ScriptableRenderContext renderLoop, IntPtr results)
	{
		Internal_Cull_Injected(ref parameters, ref renderLoop, results);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("InitializeSortSettings")]
	internal static extern void InitializeSortSettings(Camera camera, out SortingSettings sortingSettings);

	private void Submit_Internal()
	{
		Submit_Internal_Injected(ref this);
	}

	private int GetNumberOfCameras_Internal()
	{
		return GetNumberOfCameras_Internal_Injected(ref this);
	}

	private Camera GetCamera_Internal(int index)
	{
		return GetCamera_Internal_Injected(ref this, index);
	}

	private void DrawRenderers_Internal(IntPtr cullResults, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings, IntPtr renderTypes, IntPtr stateBlocks, int stateCount)
	{
		DrawRenderers_Internal_Injected(ref this, cullResults, ref drawingSettings, ref filteringSettings, renderTypes, stateBlocks, stateCount);
	}

	private void DrawShadows_Internal(IntPtr shadowDrawingSettings)
	{
		DrawShadows_Internal_Injected(ref this, shadowDrawingSettings);
	}

	private void ExecuteCommandBuffer_Internal(CommandBuffer commandBuffer)
	{
		ExecuteCommandBuffer_Internal_Injected(ref this, commandBuffer);
	}

	private void ExecuteCommandBufferAsync_Internal(CommandBuffer commandBuffer, ComputeQueueType queueType)
	{
		ExecuteCommandBufferAsync_Internal_Injected(ref this, commandBuffer, queueType);
	}

	private void SetupCameraProperties_Internal(Camera camera, bool stereoSetup, int eye)
	{
		SetupCameraProperties_Internal_Injected(ref this, camera, stereoSetup, eye);
	}

	private void StereoEndRender_Internal(Camera camera, int eye, bool isFinalPass)
	{
		StereoEndRender_Internal_Injected(ref this, camera, eye, isFinalPass);
	}

	private void StartMultiEye_Internal(Camera camera, int eye)
	{
		StartMultiEye_Internal_Injected(ref this, camera, eye);
	}

	private void StopMultiEye_Internal(Camera camera)
	{
		StopMultiEye_Internal_Injected(ref this, camera);
	}

	private void DrawSkybox_Internal(Camera camera)
	{
		DrawSkybox_Internal_Injected(ref this, camera);
	}

	private void InvokeOnRenderObjectCallback_Internal()
	{
		InvokeOnRenderObjectCallback_Internal_Injected(ref this);
	}

	private void DrawGizmos_Internal(Camera camera, GizmoSubset gizmoSubset)
	{
		DrawGizmos_Internal_Injected(ref this, camera, gizmoSubset);
	}

	internal IntPtr Internal_GetPtr()
	{
		return m_Ptr;
	}

	internal ScriptableRenderContext(IntPtr ptr)
	{
		m_Ptr = ptr;
	}

	public unsafe void BeginRenderPass(int width, int height, int samples, NativeArray<AttachmentDescriptor> attachments, int depthAttachmentIndex = -1)
	{
		BeginRenderPass_Internal(m_Ptr, width, height, samples, (IntPtr)attachments.GetUnsafeReadOnlyPtr(), attachments.Length, depthAttachmentIndex);
	}

	public ScopedRenderPass BeginScopedRenderPass(int width, int height, int samples, NativeArray<AttachmentDescriptor> attachments, int depthAttachmentIndex = -1)
	{
		BeginRenderPass(width, height, samples, attachments, depthAttachmentIndex);
		return new ScopedRenderPass(this);
	}

	public unsafe void BeginSubPass(NativeArray<int> colors, NativeArray<int> inputs, bool isDepthReadOnly = false)
	{
		BeginSubPass_Internal(m_Ptr, (IntPtr)colors.GetUnsafeReadOnlyPtr(), colors.Length, (IntPtr)inputs.GetUnsafeReadOnlyPtr(), inputs.Length, isDepthReadOnly);
	}

	public unsafe void BeginSubPass(NativeArray<int> colors, bool isDepthReadOnly = false)
	{
		BeginSubPass_Internal(m_Ptr, (IntPtr)colors.GetUnsafeReadOnlyPtr(), colors.Length, IntPtr.Zero, 0, isDepthReadOnly);
	}

	public ScopedSubPass BeginScopedSubPass(NativeArray<int> colors, NativeArray<int> inputs, bool isDepthReadOnly = false)
	{
		BeginSubPass(colors, inputs, isDepthReadOnly);
		return new ScopedSubPass(this);
	}

	public ScopedSubPass BeginScopedSubPass(NativeArray<int> colors, bool isDepthReadOnly = false)
	{
		BeginSubPass(colors, isDepthReadOnly);
		return new ScopedSubPass(this);
	}

	public void EndSubPass()
	{
		EndSubPass_Internal(m_Ptr);
	}

	public void EndRenderPass()
	{
		EndRenderPass_Internal(m_Ptr);
	}

	public void Submit()
	{
		Submit_Internal();
	}

	internal int GetNumberOfCameras()
	{
		return GetNumberOfCameras_Internal();
	}

	internal Camera GetCamera(int index)
	{
		return GetCamera_Internal(index);
	}

	public void DrawRenderers(CullingResults cullingResults, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings)
	{
		DrawRenderers_Internal(cullingResults.ptr, ref drawingSettings, ref filteringSettings, IntPtr.Zero, IntPtr.Zero, 0);
	}

	public unsafe void DrawRenderers(CullingResults cullingResults, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings, ref RenderStateBlock stateBlock)
	{
		ShaderTagId shaderTagId = default(ShaderTagId);
		fixed (RenderStateBlock* ptr = &stateBlock)
		{
			DrawRenderers_Internal(cullingResults.ptr, ref drawingSettings, ref filteringSettings, (IntPtr)(&shaderTagId), (IntPtr)ptr, 1);
		}
	}

	public unsafe void DrawRenderers(CullingResults cullingResults, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings, NativeArray<ShaderTagId> renderTypes, NativeArray<RenderStateBlock> stateBlocks)
	{
		if (renderTypes.Length != stateBlocks.Length)
		{
			throw new ArgumentException(string.Format("Arrays {0} and {1} should have same length, but {2} had length {3} while {4} had length {5}.", "renderTypes", "stateBlocks", "renderTypes", renderTypes.Length, "stateBlocks", stateBlocks.Length));
		}
		DrawRenderers_Internal(cullingResults.ptr, ref drawingSettings, ref filteringSettings, (IntPtr)renderTypes.GetUnsafeReadOnlyPtr(), (IntPtr)stateBlocks.GetUnsafeReadOnlyPtr(), renderTypes.Length);
	}

	public unsafe void DrawShadows(ref ShadowDrawingSettings settings)
	{
		fixed (ShadowDrawingSettings* ptr = &settings)
		{
			DrawShadows_Internal((IntPtr)ptr);
		}
	}

	public void ExecuteCommandBuffer(CommandBuffer commandBuffer)
	{
		if (commandBuffer == null)
		{
			throw new ArgumentNullException("commandBuffer");
		}
		ExecuteCommandBuffer_Internal(commandBuffer);
	}

	public void ExecuteCommandBufferAsync(CommandBuffer commandBuffer, ComputeQueueType queueType)
	{
		if (commandBuffer == null)
		{
			throw new ArgumentNullException("commandBuffer");
		}
		ExecuteCommandBufferAsync_Internal(commandBuffer, queueType);
	}

	public void SetupCameraProperties(Camera camera, bool stereoSetup = false)
	{
		SetupCameraProperties(camera, stereoSetup, 0);
	}

	public void SetupCameraProperties(Camera camera, bool stereoSetup, int eye)
	{
		SetupCameraProperties_Internal(camera, stereoSetup, eye);
	}

	public void StereoEndRender(Camera camera)
	{
		StereoEndRender(camera, 0, isFinalPass: true);
	}

	public void StereoEndRender(Camera camera, int eye)
	{
		StereoEndRender(camera, eye, isFinalPass: true);
	}

	public void StereoEndRender(Camera camera, int eye, bool isFinalPass)
	{
		StereoEndRender_Internal(camera, eye, isFinalPass);
	}

	public void StartMultiEye(Camera camera)
	{
		StartMultiEye(camera, 0);
	}

	public void StartMultiEye(Camera camera, int eye)
	{
		StartMultiEye_Internal(camera, eye);
	}

	public void StopMultiEye(Camera camera)
	{
		StopMultiEye_Internal(camera);
	}

	public void DrawSkybox(Camera camera)
	{
		DrawSkybox_Internal(camera);
	}

	public void InvokeOnRenderObjectCallback()
	{
		InvokeOnRenderObjectCallback_Internal();
	}

	public void DrawGizmos(Camera camera, GizmoSubset gizmoSubset)
	{
		DrawGizmos_Internal(camera, gizmoSubset);
	}

	public unsafe CullingResults Cull(ref ScriptableCullingParameters parameters)
	{
		CullingResults result = default(CullingResults);
		Internal_Cull(ref parameters, this, (IntPtr)(&result));
		return result;
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	internal void Validate()
	{
	}

	public bool Equals(ScriptableRenderContext other)
	{
		return m_Ptr.Equals(other.m_Ptr);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is ScriptableRenderContext && Equals((ScriptableRenderContext)obj);
	}

	public override int GetHashCode()
	{
		return m_Ptr.GetHashCode();
	}

	public static bool operator ==(ScriptableRenderContext left, ScriptableRenderContext right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ScriptableRenderContext left, ScriptableRenderContext right)
	{
		return !left.Equals(right);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Internal_Cull_Injected(ref ScriptableCullingParameters parameters, ref ScriptableRenderContext renderLoop, IntPtr results);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Submit_Internal_Injected(ref ScriptableRenderContext _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetNumberOfCameras_Internal_Injected(ref ScriptableRenderContext _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Camera GetCamera_Internal_Injected(ref ScriptableRenderContext _unity_self, int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DrawRenderers_Internal_Injected(ref ScriptableRenderContext _unity_self, IntPtr cullResults, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings, IntPtr renderTypes, IntPtr stateBlocks, int stateCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DrawShadows_Internal_Injected(ref ScriptableRenderContext _unity_self, IntPtr shadowDrawingSettings);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ExecuteCommandBuffer_Internal_Injected(ref ScriptableRenderContext _unity_self, CommandBuffer commandBuffer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ExecuteCommandBufferAsync_Internal_Injected(ref ScriptableRenderContext _unity_self, CommandBuffer commandBuffer, ComputeQueueType queueType);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetupCameraProperties_Internal_Injected(ref ScriptableRenderContext _unity_self, Camera camera, bool stereoSetup, int eye);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void StereoEndRender_Internal_Injected(ref ScriptableRenderContext _unity_self, Camera camera, int eye, bool isFinalPass);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void StartMultiEye_Internal_Injected(ref ScriptableRenderContext _unity_self, Camera camera, int eye);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void StopMultiEye_Internal_Injected(ref ScriptableRenderContext _unity_self, Camera camera);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DrawSkybox_Internal_Injected(ref ScriptableRenderContext _unity_self, Camera camera);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InvokeOnRenderObjectCallback_Internal_Injected(ref ScriptableRenderContext _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void DrawGizmos_Internal_Injected(ref ScriptableRenderContext _unity_self, Camera camera, GizmoSubset gizmoSubset);
}
