using System;

namespace UnityEngine.TextCore;

[Flags]
internal enum VerticalAlignment
{
	Top = 0x100,
	Middle = 0x200,
	Bottom = 0x400,
	Baseline = 0x800,
	Midline = 0x1000,
	Capline = 0x2000
}
