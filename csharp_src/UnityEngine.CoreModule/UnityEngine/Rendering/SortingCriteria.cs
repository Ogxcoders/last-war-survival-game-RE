using System;

namespace UnityEngine.Rendering;

[Flags]
public enum SortingCriteria
{
	None = 0,
	SortingLayer = 1,
	RenderQueue = 2,
	BackToFront = 4,
	QuantizedFrontToBack = 8,
	OptimizeStateChanges = 0x10,
	CanvasOrder = 0x20,
	RendererPriority = 0x40,
	CommonOpaque = 0x3B,
	CommonTransparent = 0x17
}
