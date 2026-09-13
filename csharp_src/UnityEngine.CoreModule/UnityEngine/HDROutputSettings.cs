using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

public static class HDROutputSettings
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
	public static extern void SetPaperWhiteInNits(float paperWhite);
}
