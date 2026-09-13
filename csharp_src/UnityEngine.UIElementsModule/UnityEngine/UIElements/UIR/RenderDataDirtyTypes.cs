using System;

namespace UnityEngine.UIElements.UIR;

[Flags]
internal enum RenderDataDirtyTypes
{
	None = 0,
	Transform = 1,
	ClipRectSize = 2,
	Clipping = 4,
	ClippingHierarchy = 8,
	Visuals = 0x10,
	VisualsHierarchy = 0x20,
	Opacity = 0x40,
	OpacityHierarchy = 0x80
}
