using System;

namespace Unity.Profiling.LowLevel;

[Flags]
public enum MarkerFlags
{
	Default = 0,
	Script = 2,
	ScriptInvoke = 0x20,
	ScriptDeepProfiler = 0x40,
	AvailabilityEditor = 4,
	Warning = 0x10
}
