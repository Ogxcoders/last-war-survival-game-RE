using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Animation/OptimizeTransformHierarchy.h")]
public class AnimatorUtility
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void OptimizeTransformHierarchy(GameObject go, string[] exposedTransforms);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	public static extern void DeoptimizeTransformHierarchy(GameObject go);
}
