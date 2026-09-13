using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[UsedByNativeCode]
[NativeConditional("ENABLE_XR")]
[NativeType(Header = "Modules/XR/Subsystems/Input/XRInputSubsystem.h")]
public class XRInputSubsystem : IntegratedSubsystem<XRInputSubsystemDescriptor>
{
	private List<ulong> m_DeviceIdsCache;

	public event Action<XRInputSubsystem> trackingOriginUpdated;

	public event Action<XRInputSubsystem> boundaryChanged;

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern uint GetIndex();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool TryRecenter();

	public bool TryGetInputDevices(List<InputDevice> devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		devices.Clear();
		if (m_DeviceIdsCache == null)
		{
			m_DeviceIdsCache = new List<ulong>();
		}
		m_DeviceIdsCache.Clear();
		TryGetDeviceIds_AsList(m_DeviceIdsCache);
		for (int i = 0; i < m_DeviceIdsCache.Count; i++)
		{
			devices.Add(new InputDevice(m_DeviceIdsCache[i]));
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool TrySetTrackingOriginMode(TrackingOriginModeFlags origin);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern TrackingOriginModeFlags GetTrackingOriginMode();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern TrackingOriginModeFlags GetSupportedTrackingOriginModes();

	public bool TryGetBoundaryPoints(List<Vector3> boundaryPoints)
	{
		if (boundaryPoints == null)
		{
			throw new ArgumentNullException("boundaryPoints");
		}
		return TryGetBoundaryPoints_AsList(boundaryPoints);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool TryGetBoundaryPoints_AsList(List<Vector3> boundaryPoints);

	[RequiredByNativeCode(GenerateProxy = true)]
	private static void InvokeTrackingOriginUpdatedEvent(IntPtr internalPtr)
	{
		IntegratedSubsystem integratedSubsystem = Internal_SubsystemInstances.Internal_GetInstanceByPtr(internalPtr);
		if (integratedSubsystem is XRInputSubsystem { trackingOriginUpdated: not null } xRInputSubsystem)
		{
			xRInputSubsystem.trackingOriginUpdated(xRInputSubsystem);
		}
	}

	[RequiredByNativeCode(GenerateProxy = true)]
	private static void InvokeBoundaryChangedEvent(IntPtr internalPtr)
	{
		IntegratedSubsystem integratedSubsystem = Internal_SubsystemInstances.Internal_GetInstanceByPtr(internalPtr);
		if (integratedSubsystem is XRInputSubsystem { boundaryChanged: not null } xRInputSubsystem)
		{
			xRInputSubsystem.boundaryChanged(xRInputSubsystem);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void TryGetDeviceIds_AsList(List<ulong> deviceIds);
}
