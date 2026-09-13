using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/Director/AnimationStream.h")]
[RequiredByNativeCode]
[MovedFrom("UnityEngine.Experimental.Animations")]
[NativeHeader("Modules/Animation/ScriptBindings/AnimationStream.bindings.h")]
public struct AnimationStream
{
	private uint m_AnimatorBindingsVersion;

	private IntPtr constant;

	private IntPtr input;

	private IntPtr output;

	private IntPtr workspace;

	private IntPtr inputStreamAccessor;

	private IntPtr animationHandleBinder;

	internal const int InvalidIndex = -1;

	internal uint animatorBindingsVersion => m_AnimatorBindingsVersion;

	public bool isValid => m_AnimatorBindingsVersion >= 2 && constant != IntPtr.Zero && input != IntPtr.Zero && output != IntPtr.Zero && workspace != IntPtr.Zero && animationHandleBinder != IntPtr.Zero;

	public float deltaTime
	{
		get
		{
			CheckIsValid();
			return GetDeltaTime();
		}
	}

	public Vector3 velocity
	{
		get
		{
			CheckIsValid();
			return GetVelocity();
		}
		set
		{
			CheckIsValid();
			SetVelocity(value);
		}
	}

	public Vector3 angularVelocity
	{
		get
		{
			CheckIsValid();
			return GetAngularVelocity();
		}
		set
		{
			CheckIsValid();
			SetAngularVelocity(value);
		}
	}

	public Vector3 rootMotionPosition
	{
		get
		{
			CheckIsValid();
			return GetRootMotionPosition();
		}
	}

	public Quaternion rootMotionRotation
	{
		get
		{
			CheckIsValid();
			return GetRootMotionRotation();
		}
	}

	public bool isHumanStream
	{
		get
		{
			CheckIsValid();
			return GetIsHumanStream();
		}
	}

	public int inputStreamCount
	{
		get
		{
			CheckIsValid();
			return GetInputStreamCount();
		}
	}

	internal void CheckIsValid()
	{
		if (!isValid)
		{
			throw new InvalidOperationException("The AnimationStream is invalid.");
		}
	}

	public AnimationHumanStream AsHuman()
	{
		CheckIsValid();
		if (!GetIsHumanStream())
		{
			throw new InvalidOperationException("Cannot create an AnimationHumanStream for a generic rig.");
		}
		return GetHumanStream();
	}

	public AnimationStream GetInputStream(int index)
	{
		CheckIsValid();
		return InternalGetInputStream(index);
	}

	public float GetInputWeight(int index)
	{
		CheckIsValid();
		return InternalGetInputWeight(index);
	}

	private void ReadSceneTransforms()
	{
		CheckIsValid();
		InternalReadSceneTransforms();
	}

	private void WriteSceneTransforms()
	{
		CheckIsValid();
		InternalWriteSceneTransforms();
	}

	[NativeMethod(IsThreadSafe = true)]
	private float GetDeltaTime()
	{
		return GetDeltaTime_Injected(ref this);
	}

	[NativeMethod(IsThreadSafe = true)]
	private bool GetIsHumanStream()
	{
		return GetIsHumanStream_Injected(ref this);
	}

	[NativeMethod(Name = "AnimationStreamBindings::GetVelocity", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetVelocity()
	{
		GetVelocity_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationStreamBindings::SetVelocity", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void SetVelocity(Vector3 velocity)
	{
		SetVelocity_Injected(ref this, ref velocity);
	}

	[NativeMethod(Name = "AnimationStreamBindings::GetAngularVelocity", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetAngularVelocity()
	{
		GetAngularVelocity_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationStreamBindings::SetAngularVelocity", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void SetAngularVelocity(Vector3 velocity)
	{
		SetAngularVelocity_Injected(ref this, ref velocity);
	}

	[NativeMethod(Name = "AnimationStreamBindings::GetRootMotionPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetRootMotionPosition()
	{
		GetRootMotionPosition_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationStreamBindings::GetRootMotionRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion GetRootMotionRotation()
	{
		GetRootMotionRotation_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(IsThreadSafe = true)]
	private int GetInputStreamCount()
	{
		return GetInputStreamCount_Injected(ref this);
	}

	[NativeMethod(Name = "GetInputStream", IsThreadSafe = true)]
	private AnimationStream InternalGetInputStream(int index)
	{
		InternalGetInputStream_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "GetInputWeight", IsThreadSafe = true)]
	private float InternalGetInputWeight(int index)
	{
		return InternalGetInputWeight_Injected(ref this, index);
	}

	[NativeMethod(IsThreadSafe = true)]
	private AnimationHumanStream GetHumanStream()
	{
		GetHumanStream_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "ReadSceneTransforms", IsThreadSafe = true)]
	private void InternalReadSceneTransforms()
	{
		InternalReadSceneTransforms_Injected(ref this);
	}

	[NativeMethod(Name = "WriteSceneTransforms", IsThreadSafe = true)]
	private void InternalWriteSceneTransforms()
	{
		InternalWriteSceneTransforms_Injected(ref this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float GetDeltaTime_Injected(ref AnimationStream _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool GetIsHumanStream_Injected(ref AnimationStream _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetVelocity_Injected(ref AnimationStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetVelocity_Injected(ref AnimationStream _unity_self, ref Vector3 velocity);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetAngularVelocity_Injected(ref AnimationStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetAngularVelocity_Injected(ref AnimationStream _unity_self, ref Vector3 velocity);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetRootMotionPosition_Injected(ref AnimationStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetRootMotionRotation_Injected(ref AnimationStream _unity_self, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetInputStreamCount_Injected(ref AnimationStream _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetInputStream_Injected(ref AnimationStream _unity_self, int index, out AnimationStream ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float InternalGetInputWeight_Injected(ref AnimationStream _unity_self, int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetHumanStream_Injected(ref AnimationStream _unity_self, out AnimationHumanStream ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalReadSceneTransforms_Injected(ref AnimationStream _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalWriteSceneTransforms_Injected(ref AnimationStream _unity_self);
}
