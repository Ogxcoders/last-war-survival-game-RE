using System;
using System.Runtime.InteropServices;

namespace UnityEngine.Rendering;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct VolumeIsolationScope : IDisposable
{
	public VolumeIsolationScope(bool unused)
	{
		VolumeManager.needIsolationFilteredByRenderer = true;
	}

	void IDisposable.Dispose()
	{
		VolumeManager.needIsolationFilteredByRenderer = false;
	}
}
