namespace UnityEngine.VFX;

internal enum VFXSystemFlag
{
	SystemDefault = 0,
	SystemHasKill = 1,
	SystemHasIndirectBuffer = 2,
	SystemReceivedEventGPU = 4,
	SystemHasStrips = 8
}
