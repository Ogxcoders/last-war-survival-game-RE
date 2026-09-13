using System;
using UnityEngine.Bindings;

namespace UnityEngine.Animations;

[NativeType("Modules/Animation/Constraints/ConstraintEnums.h")]
[Flags]
public enum Axis
{
	None = 0,
	X = 1,
	Y = 2,
	Z = 4
}
