using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[ExcludeFromObjectFactory]
[NativeHeader("Modules/Animation/RuntimeAnimatorController.h")]
[UsedByNativeCode]
public class RuntimeAnimatorController : Object
{
	public extern AnimationClip[] animationClips
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	protected RuntimeAnimatorController()
	{
	}
}
