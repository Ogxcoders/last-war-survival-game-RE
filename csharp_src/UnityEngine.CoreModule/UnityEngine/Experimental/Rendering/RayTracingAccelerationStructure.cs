using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering;

[NativeHeader("Runtime/Export/Graphics/RayTracingAccelerationStructure.bindings.h")]
[UsedByNativeCode]
[NativeHeader("Runtime/Shaders/RayTracingAccelerationStructure.h")]
public sealed class RayTracingAccelerationStructure : IDisposable
{
	[Flags]
	public enum RayTracingModeMask
	{
		Nothing = 0,
		Static = 2,
		DynamicTransform = 4,
		DynamicGeometry = 8,
		Everything = 0xE
	}

	public enum ManagementMode
	{
		Manual,
		Automatic
	}

	public struct RASSettings
	{
		public ManagementMode managementMode;

		public RayTracingModeMask rayTracingModeMask;

		public int layerMask;

		public RASSettings(ManagementMode sceneManagementMode, RayTracingModeMask rayTracingModeMask, int layerMask)
		{
			managementMode = sceneManagementMode;
			this.rayTracingModeMask = rayTracingModeMask;
			this.layerMask = layerMask;
		}
	}

	internal IntPtr m_Ptr;

	~RayTracingAccelerationStructure()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			Destroy(this);
		}
		m_Ptr = IntPtr.Zero;
	}

	public RayTracingAccelerationStructure(RASSettings settings)
	{
		m_Ptr = Create(settings);
	}

	public RayTracingAccelerationStructure()
	{
		m_Ptr = Create(new RASSettings
		{
			rayTracingModeMask = RayTracingModeMask.Everything,
			managementMode = ManagementMode.Manual,
			layerMask = -1
		});
	}

	[FreeFunction("RayTracingAccelerationStructure_Bindings::Create")]
	private static IntPtr Create(RASSettings desc)
	{
		return Create_Injected(ref desc);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("RayTracingAccelerationStructure_Bindings::Destroy")]
	private static extern void Destroy(RayTracingAccelerationStructure accelStruct);

	public void Release()
	{
		Dispose();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Build", HasExplicitThis = true)]
	public extern void Build();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Update", HasExplicitThis = true)]
	public extern void Update();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstance", HasExplicitThis = true)]
	public extern void AddInstance([NotNull] Renderer targetRenderer, bool[] subMeshMask = null, bool[] subMeshTransparencyFlags = null, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 255u);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceTransform", HasExplicitThis = true)]
	public extern void UpdateInstanceTransform([NotNull] Renderer renderer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetSize", HasExplicitThis = true)]
	public extern ulong GetSize();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr Create_Injected(ref RASSettings desc);
}
