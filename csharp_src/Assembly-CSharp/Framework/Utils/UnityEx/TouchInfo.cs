using UnityEngine;
using XLua;

namespace Framework.Utils.UnityEx;

[GCOptimize(OptimizeFlag.Default)]
[LuaCallCSharp(GenFlag.No)]
public struct TouchInfo
{
	public int pointerId;

	public Vector2 pointerPos;

	public Vector2 deltaPos;

	public TouchInfo(int pointerId, Vector2 pointerPos, Vector2 deltaPos)
	{
		this.pointerId = pointerId;
		this.pointerPos = pointerPos;
		this.deltaPos = deltaPos;
	}
}
