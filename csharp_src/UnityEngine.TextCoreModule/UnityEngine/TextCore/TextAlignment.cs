using UnityEngine.Bindings;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal enum TextAlignment
{
	TopLeft = 257,
	TopCenter = 258,
	TopRight = 260,
	TopJustified = 264,
	TopFlush = 272,
	TopGeoAligned = 288,
	MiddleLeft = 513,
	MiddleCenter = 514,
	MiddleRight = 516,
	MiddleJustified = 520,
	MiddleFlush = 528,
	MiddleGeoAligned = 544,
	BottomLeft = 1025,
	BottomCenter = 1026,
	BottomRight = 1028,
	BottomJustified = 1032,
	BottomFlush = 1040,
	BottomGeoAligned = 1056,
	BaselineLeft = 2049,
	BaselineCenter = 2050,
	BaselineRight = 2052,
	BaselineJustified = 2056,
	BaselineFlush = 2064,
	BaselineGeoAligned = 2080,
	MidlineLeft = 4097,
	MidlineCenter = 4098,
	MidlineRight = 4100,
	MidlineJustified = 4104,
	MidlineFlush = 4112,
	MidlineGeoAligned = 4128,
	CaplineLeft = 8193,
	CaplineCenter = 8194,
	CaplineRight = 8196,
	CaplineJustified = 8200,
	CaplineFlush = 8208,
	CaplineGeoAligned = 8224
}
