using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.XR.Provider;

public static class XRStats
{
	public static bool TryGetStat(IntegratedSubsystem xrSubsystem, string tag, out float value)
	{
		return TryGetStat_Internal(xrSubsystem.m_Ptr, tag, out value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeHeader("Modules/XR/Stats/XRStats.h")]
	[NativeConditional("ENABLE_XR")]
	[StaticAccessor("XRStats::Get()", StaticAccessorType.Dot)]
	[NativeMethod("TryGetStatByName_Internal")]
	private static extern bool TryGetStat_Internal(IntPtr ptr, string tag, out float value);
}
