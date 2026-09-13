using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[UsedByNativeCode]
[NativeHeader("Modules/Animation/ScriptBindings/Animation.bindings.h")]
[NativeHeader("Modules/Animation/AnimatorInfo.h")]
public struct AnimatorClipInfo
{
	private int m_ClipInstanceID;

	private float m_Weight;

	public AnimationClip clip => (m_ClipInstanceID != 0) ? InstanceIDToAnimationClipPPtr(m_ClipInstanceID) : null;

	public float weight => m_Weight;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("AnimationBindings::InstanceIDToAnimationClipPPtr")]
	private static extern AnimationClip InstanceIDToAnimationClipPPtr(int instanceID);
}
