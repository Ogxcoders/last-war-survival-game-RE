using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations;

[StaticAccessor("AnimationOffsetPlayableBindings", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/Animation/Director/AnimationOffsetPlayable.h")]
[NativeHeader("Modules/Animation/ScriptBindings/AnimationOffsetPlayable.bindings.h")]
[RequiredByNativeCode]
[NativeHeader("Runtime/Director/Core/HPlayable.h")]
internal struct AnimationOffsetPlayable : IPlayable, IEquatable<AnimationOffsetPlayable>
{
	private PlayableHandle m_Handle;

	private static readonly AnimationOffsetPlayable m_NullPlayable = new AnimationOffsetPlayable(PlayableHandle.Null);

	public static AnimationOffsetPlayable Null => m_NullPlayable;

	public static AnimationOffsetPlayable Create(PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
	{
		PlayableHandle handle = CreateHandle(graph, position, rotation, inputCount);
		return new AnimationOffsetPlayable(handle);
	}

	private static PlayableHandle CreateHandle(PlayableGraph graph, Vector3 position, Quaternion rotation, int inputCount)
	{
		PlayableHandle handle = PlayableHandle.Null;
		if (!CreateHandleInternal(graph, position, rotation, ref handle))
		{
			return PlayableHandle.Null;
		}
		handle.SetInputCount(inputCount);
		return handle;
	}

	internal AnimationOffsetPlayable(PlayableHandle handle)
	{
		if (handle.IsValid() && !handle.IsPlayableOfType<AnimationOffsetPlayable>())
		{
			throw new InvalidCastException("Can't set handle: the playable is not an AnimationOffsetPlayable.");
		}
		m_Handle = handle;
	}

	public PlayableHandle GetHandle()
	{
		return m_Handle;
	}

	public static implicit operator Playable(AnimationOffsetPlayable playable)
	{
		return new Playable(playable.GetHandle());
	}

	public static explicit operator AnimationOffsetPlayable(Playable playable)
	{
		return new AnimationOffsetPlayable(playable.GetHandle());
	}

	public bool Equals(AnimationOffsetPlayable other)
	{
		return Equals(other.GetHandle());
	}

	public Vector3 GetPosition()
	{
		return GetPositionInternal(ref m_Handle);
	}

	public void SetPosition(Vector3 value)
	{
		SetPositionInternal(ref m_Handle, value);
	}

	public Quaternion GetRotation()
	{
		return GetRotationInternal(ref m_Handle);
	}

	public void SetRotation(Quaternion value)
	{
		SetRotationInternal(ref m_Handle, value);
	}

	[NativeThrows]
	private static bool CreateHandleInternal(PlayableGraph graph, Vector3 position, Quaternion rotation, ref PlayableHandle handle)
	{
		return CreateHandleInternal_Injected(ref graph, ref position, ref rotation, ref handle);
	}

	[NativeThrows]
	private static Vector3 GetPositionInternal(ref PlayableHandle handle)
	{
		GetPositionInternal_Injected(ref handle, out var ret);
		return ret;
	}

	[NativeThrows]
	private static void SetPositionInternal(ref PlayableHandle handle, Vector3 value)
	{
		SetPositionInternal_Injected(ref handle, ref value);
	}

	[NativeThrows]
	private static Quaternion GetRotationInternal(ref PlayableHandle handle)
	{
		GetRotationInternal_Injected(ref handle, out var ret);
		return ret;
	}

	[NativeThrows]
	private static void SetRotationInternal(ref PlayableHandle handle, Quaternion value)
	{
		SetRotationInternal_Injected(ref handle, ref value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, ref Vector3 position, ref Quaternion rotation, ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetPositionInternal_Injected(ref PlayableHandle handle, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetPositionInternal_Injected(ref PlayableHandle handle, ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetRotationInternal_Injected(ref PlayableHandle handle, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetRotationInternal_Injected(ref PlayableHandle handle, ref Quaternion value);
}
