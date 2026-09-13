using System;
using UnityEngine.Scripting;

namespace UnityEngine.TextCore.LowLevel;

[Flags]
[UsedByNativeCode]
public enum GlyphLoadFlags
{
	LOAD_DEFAULT = 0,
	LOAD_NO_SCALE = 1,
	LOAD_NO_HINTING = 2,
	LOAD_RENDER = 4,
	LOAD_NO_BITMAP = 8,
	LOAD_FORCE_AUTOHINT = 0x20,
	LOAD_MONOCHROME = 0x1000,
	LOAD_NO_AUTOHINT = 0x8000,
	LOAD_COMPUTE_METRICS = 0x200000,
	LOAD_BITMAP_METRICS_ONLY = 0x400000
}
