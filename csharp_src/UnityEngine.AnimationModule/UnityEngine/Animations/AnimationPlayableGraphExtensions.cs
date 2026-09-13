using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;

namespace UnityEngine.Animations;

[StaticAccessor("AnimationPlayableGraphExtensionsBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/Animation/Animator.h")]
[NativeHeader("Modules/Animation/ScriptBindings/AnimationPlayableGraphExtensions.bindings.h")]
[NativeHeader("Runtime/Director/Core/HPlayableOutput.h")]
[NativeHeader("Runtime/Director/Core/HPlayable.h")]
internal static class AnimationPlayableGraphExtensions
{
	internal static void SyncUpdateAndTimeMode(this PlayableGraph graph, Animator animator)
	{
		InternalSyncUpdateAndTimeMode(ref graph, animator);
	}

	internal static void DestroyOutput(this PlayableGraph graph, PlayableOutputHandle handle)
	{
		InternalDestroyOutput(ref graph, ref handle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	internal static extern bool InternalCreateAnimationOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	internal static extern void InternalSyncUpdateAndTimeMode(ref PlayableGraph graph, [NotNull] Animator animator);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void InternalDestroyOutput(ref PlayableGraph graph, ref PlayableOutputHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int InternalAnimationOutputCount(ref PlayableGraph graph);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool InternalGetAnimationOutput(ref PlayableGraph graph, int index, out PlayableOutputHandle handle);
}
