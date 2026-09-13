using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/ScriptBindings/AnimationPosePlayable.bindings.h")]
[NativeHeader("Modules/Animation/Director/AnimationPosePlayable.h")]
[StaticAccessor("AnimationPosePlayableBindings", StaticAccessorType.DoubleColon)]
[RequiredByNativeCode]
[NativeHeader("Runtime/Director/Core/HPlayable.h")]
internal struct AnimationPosePlayable : IPlayable, IEquatable<AnimationPosePlayable>
{
	private PlayableHandle m_Handle;

	private static readonly AnimationPosePlayable m_NullPlayable = new AnimationPosePlayable(PlayableHandle.Null);

	public static AnimationPosePlayable Null => m_NullPlayable;

	public static AnimationPosePlayable Create(PlayableGraph graph)
	{
		PlayableHandle handle = CreateHandle(graph);
		return new AnimationPosePlayable(handle);
	}

	private static PlayableHandle CreateHandle(PlayableGraph graph)
	{
		PlayableHandle handle = PlayableHandle.Null;
		if (!CreateHandleInternal(graph, ref handle))
		{
			return PlayableHandle.Null;
		}
		return handle;
	}

	internal AnimationPosePlayable(PlayableHandle handle)
	{
		if (handle.IsValid() && !handle.IsPlayableOfType<AnimationPosePlayable>())
		{
			throw new InvalidCastException("Can't set handle: the playable is not an AnimationPosePlayable.");
		}
		m_Handle = handle;
	}

	public PlayableHandle GetHandle()
	{
		return m_Handle;
	}

	public static implicit operator Playable(AnimationPosePlayable playable)
	{
		return new Playable(playable.GetHandle());
	}

	public static explicit operator AnimationPosePlayable(Playable playable)
	{
		return new AnimationPosePlayable(playable.GetHandle());
	}

	public bool Equals(AnimationPosePlayable other)
	{
		return Equals(other.GetHandle());
	}

	public bool GetMustReadPreviousPose()
	{
		return GetMustReadPreviousPoseInternal(ref m_Handle);
	}

	public void SetMustReadPreviousPose(bool value)
	{
		SetMustReadPreviousPoseInternal(ref m_Handle, value);
	}

	public bool GetReadDefaultPose()
	{
		return GetReadDefaultPoseInternal(ref m_Handle);
	}

	public void SetReadDefaultPose(bool value)
	{
		SetReadDefaultPoseInternal(ref m_Handle, value);
	}

	public bool GetApplyFootIK()
	{
		return GetApplyFootIKInternal(ref m_Handle);
	}

	public void SetApplyFootIK(bool value)
	{
		SetApplyFootIKInternal(ref m_Handle, value);
	}

	[NativeThrows]
	private static bool CreateHandleInternal(PlayableGraph graph, ref PlayableHandle handle)
	{
		return CreateHandleInternal_Injected(ref graph, ref handle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetMustReadPreviousPoseInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetMustReadPreviousPoseInternal(ref PlayableHandle handle, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetReadDefaultPoseInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetReadDefaultPoseInternal(ref PlayableHandle handle, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetApplyFootIKInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetApplyFootIKInternal(ref PlayableHandle handle, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, ref PlayableHandle handle);
}
