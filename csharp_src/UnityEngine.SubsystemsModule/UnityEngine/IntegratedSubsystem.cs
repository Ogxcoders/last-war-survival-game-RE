using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[NativeType(Header = "Modules/Subsystems/Subsystem.h")]
[UsedByNativeCode]
public class IntegratedSubsystem : ISubsystem
{
	internal IntPtr m_Ptr;

	internal ISubsystemDescriptor m_subsystemDescriptor;

	public bool running => valid && Internal_IsRunning();

	internal bool valid => m_Ptr != IntPtr.Zero;

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void SetHandle(IntegratedSubsystem inst);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Start();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Stop();

	public void Destroy()
	{
		IntPtr ptr = m_Ptr;
		Internal_SubsystemInstances.Internal_RemoveInstanceByPtr(m_Ptr);
		SubsystemManager.DestroyInstance_Internal(ptr);
		m_Ptr = IntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern bool Internal_IsRunning();
}
[UsedByNativeCode("Subsystem_TSubsystemDescriptor")]
public class IntegratedSubsystem<TSubsystemDescriptor> : IntegratedSubsystem where TSubsystemDescriptor : ISubsystemDescriptor
{
	public TSubsystemDescriptor SubsystemDescriptor => (TSubsystemDescriptor)m_subsystemDescriptor;
}
