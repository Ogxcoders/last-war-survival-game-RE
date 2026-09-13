using System;
using System.ComponentModel;

namespace UnityEngine;

public enum CollisionDetectionMode2D
{
	[Obsolete("Enum member CollisionDetectionMode2D.None has been deprecated. Use CollisionDetectionMode2D.Discrete instead (UnityUpgradable) -> Discrete", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	None = 0,
	Discrete = 0,
	Continuous = 1
}
