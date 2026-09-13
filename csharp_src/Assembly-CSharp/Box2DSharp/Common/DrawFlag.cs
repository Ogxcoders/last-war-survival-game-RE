using System;

namespace Box2DSharp.Common;

[Flags]
public enum DrawFlag
{
	DrawShape = 1,
	DrawJoint = 2,
	DrawAABB = 4,
	DrawPair = 8,
	DrawCenterOfMass = 0x10,
	DrawContactPoint = 0x20
}
