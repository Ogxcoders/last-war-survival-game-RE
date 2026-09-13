using System;

namespace FibMatrix;

[Flags]
public enum LogTargetType
{
	None = 0,
	Runtime = 1,
	Network = 2
}
