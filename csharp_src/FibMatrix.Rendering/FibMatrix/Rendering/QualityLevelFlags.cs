using System;

namespace FibMatrix.Rendering;

[Flags]
public enum QualityLevelFlags
{
	Low = 1,
	MediumLow = 2,
	Medium = 4,
	MediumHigh = 8,
	High = 0x10,
	All = 0x1F
}
