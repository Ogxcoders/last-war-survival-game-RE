using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[RequiredByNativeCode]
[MovedFrom("UnityEngine.Experimental.Animations")]
[NativeHeader("Modules/Animation/ScriptBindings/AnimationHumanStream.bindings.h")]
[NativeHeader("Modules/Animation/Director/AnimationHumanStream.h")]
public struct AnimationHumanStream
{
	private IntPtr stream;

	public bool isValid => stream != IntPtr.Zero;

	public float humanScale
	{
		get
		{
			ThrowIfInvalid();
			return GetHumanScale();
		}
	}

	public float leftFootHeight
	{
		get
		{
			ThrowIfInvalid();
			return GetFootHeight(left: true);
		}
	}

	public float rightFootHeight
	{
		get
		{
			ThrowIfInvalid();
			return GetFootHeight(left: false);
		}
	}

	public Vector3 bodyLocalPosition
	{
		get
		{
			ThrowIfInvalid();
			return InternalGetBodyLocalPosition();
		}
		set
		{
			ThrowIfInvalid();
			InternalSetBodyLocalPosition(value);
		}
	}

	public Quaternion bodyLocalRotation
	{
		get
		{
			ThrowIfInvalid();
			return InternalGetBodyLocalRotation();
		}
		set
		{
			ThrowIfInvalid();
			InternalSetBodyLocalRotation(value);
		}
	}

	public Vector3 bodyPosition
	{
		get
		{
			ThrowIfInvalid();
			return InternalGetBodyPosition();
		}
		set
		{
			ThrowIfInvalid();
			InternalSetBodyPosition(value);
		}
	}

	public Quaternion bodyRotation
	{
		get
		{
			ThrowIfInvalid();
			return InternalGetBodyRotation();
		}
		set
		{
			ThrowIfInvalid();
			InternalSetBodyRotation(value);
		}
	}

	public Vector3 leftFootVelocity
	{
		get
		{
			ThrowIfInvalid();
			return GetLeftFootVelocity();
		}
	}

	public Vector3 rightFootVelocity
	{
		get
		{
			ThrowIfInvalid();
			return GetRightFootVelocity();
		}
	}

	private void ThrowIfInvalid()
	{
		if (!isValid)
		{
			throw new InvalidOperationException("The AnimationHumanStream is invalid.");
		}
	}

	public float GetMuscle(MuscleHandle muscle)
	{
		ThrowIfInvalid();
		return InternalGetMuscle(muscle);
	}

	public void SetMuscle(MuscleHandle muscle, float value)
	{
		ThrowIfInvalid();
		InternalSetMuscle(muscle, value);
	}

	public void ResetToStancePose()
	{
		ThrowIfInvalid();
		InternalResetToStancePose();
	}

	public Vector3 GetGoalPositionFromPose(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalPositionFromPose(index);
	}

	public Quaternion GetGoalRotationFromPose(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalRotationFromPose(index);
	}

	public Vector3 GetGoalLocalPosition(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalLocalPosition(index);
	}

	public void SetGoalLocalPosition(AvatarIKGoal index, Vector3 pos)
	{
		ThrowIfInvalid();
		InternalSetGoalLocalPosition(index, pos);
	}

	public Quaternion GetGoalLocalRotation(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalLocalRotation(index);
	}

	public void SetGoalLocalRotation(AvatarIKGoal index, Quaternion rot)
	{
		ThrowIfInvalid();
		InternalSetGoalLocalRotation(index, rot);
	}

	public Vector3 GetGoalPosition(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalPosition(index);
	}

	public void SetGoalPosition(AvatarIKGoal index, Vector3 pos)
	{
		ThrowIfInvalid();
		InternalSetGoalPosition(index, pos);
	}

	public Quaternion GetGoalRotation(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalRotation(index);
	}

	public void SetGoalRotation(AvatarIKGoal index, Quaternion rot)
	{
		ThrowIfInvalid();
		InternalSetGoalRotation(index, rot);
	}

	public void SetGoalWeightPosition(AvatarIKGoal index, float value)
	{
		ThrowIfInvalid();
		InternalSetGoalWeightPosition(index, value);
	}

	public void SetGoalWeightRotation(AvatarIKGoal index, float value)
	{
		ThrowIfInvalid();
		InternalSetGoalWeightRotation(index, value);
	}

	public float GetGoalWeightPosition(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalWeightPosition(index);
	}

	public float GetGoalWeightRotation(AvatarIKGoal index)
	{
		ThrowIfInvalid();
		return InternalGetGoalWeightRotation(index);
	}

	public Vector3 GetHintPosition(AvatarIKHint index)
	{
		ThrowIfInvalid();
		return InternalGetHintPosition(index);
	}

	public void SetHintPosition(AvatarIKHint index, Vector3 pos)
	{
		ThrowIfInvalid();
		InternalSetHintPosition(index, pos);
	}

	public void SetHintWeightPosition(AvatarIKHint index, float value)
	{
		ThrowIfInvalid();
		InternalSetHintWeightPosition(index, value);
	}

	public float GetHintWeightPosition(AvatarIKHint index)
	{
		ThrowIfInvalid();
		return InternalGetHintWeightPosition(index);
	}

	public void SetLookAtPosition(Vector3 lookAtPosition)
	{
		ThrowIfInvalid();
		InternalSetLookAtPosition(lookAtPosition);
	}

	public void SetLookAtClampWeight(float weight)
	{
		ThrowIfInvalid();
		InternalSetLookAtClampWeight(weight);
	}

	public void SetLookAtBodyWeight(float weight)
	{
		ThrowIfInvalid();
		InternalSetLookAtBodyWeight(weight);
	}

	public void SetLookAtHeadWeight(float weight)
	{
		ThrowIfInvalid();
		InternalSetLookAtHeadWeight(weight);
	}

	public void SetLookAtEyesWeight(float weight)
	{
		ThrowIfInvalid();
		InternalSetLookAtEyesWeight(weight);
	}

	public void SolveIK()
	{
		ThrowIfInvalid();
		InternalSolveIK();
	}

	[NativeMethod(IsThreadSafe = true)]
	private float GetHumanScale()
	{
		return GetHumanScale_Injected(ref this);
	}

	[NativeMethod(IsThreadSafe = true)]
	private float GetFootHeight(bool left)
	{
		return GetFootHeight_Injected(ref this, left);
	}

	[NativeMethod(Name = "ResetToStancePose", IsThreadSafe = true)]
	private void InternalResetToStancePose()
	{
		InternalResetToStancePose_Injected(ref this);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetGoalPositionFromPose", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 InternalGetGoalPositionFromPose(AvatarIKGoal index)
	{
		InternalGetGoalPositionFromPose_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetGoalRotationFromPose", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion InternalGetGoalRotationFromPose(AvatarIKGoal index)
	{
		InternalGetGoalRotationFromPose_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetBodyLocalPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 InternalGetBodyLocalPosition()
	{
		InternalGetBodyLocalPosition_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetBodyLocalPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetBodyLocalPosition(Vector3 value)
	{
		InternalSetBodyLocalPosition_Injected(ref this, ref value);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetBodyLocalRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion InternalGetBodyLocalRotation()
	{
		InternalGetBodyLocalRotation_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetBodyLocalRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetBodyLocalRotation(Quaternion value)
	{
		InternalSetBodyLocalRotation_Injected(ref this, ref value);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetBodyPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 InternalGetBodyPosition()
	{
		InternalGetBodyPosition_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetBodyPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetBodyPosition(Vector3 value)
	{
		InternalSetBodyPosition_Injected(ref this, ref value);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetBodyRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion InternalGetBodyRotation()
	{
		InternalGetBodyRotation_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetBodyRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetBodyRotation(Quaternion value)
	{
		InternalSetBodyRotation_Injected(ref this, ref value);
	}

	[NativeMethod(Name = "GetMuscle", IsThreadSafe = true)]
	private float InternalGetMuscle(MuscleHandle muscle)
	{
		return InternalGetMuscle_Injected(ref this, ref muscle);
	}

	[NativeMethod(Name = "SetMuscle", IsThreadSafe = true)]
	private void InternalSetMuscle(MuscleHandle muscle, float value)
	{
		InternalSetMuscle_Injected(ref this, ref muscle, value);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetLeftFootVelocity", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetLeftFootVelocity()
	{
		GetLeftFootVelocity_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetRightFootVelocity", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 GetRightFootVelocity()
	{
		GetRightFootVelocity_Injected(ref this, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetGoalLocalPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 InternalGetGoalLocalPosition(AvatarIKGoal index)
	{
		InternalGetGoalLocalPosition_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetGoalLocalPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetGoalLocalPosition(AvatarIKGoal index, Vector3 pos)
	{
		InternalSetGoalLocalPosition_Injected(ref this, index, ref pos);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetGoalLocalRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion InternalGetGoalLocalRotation(AvatarIKGoal index)
	{
		InternalGetGoalLocalRotation_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetGoalLocalRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetGoalLocalRotation(AvatarIKGoal index, Quaternion rot)
	{
		InternalSetGoalLocalRotation_Injected(ref this, index, ref rot);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetGoalPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 InternalGetGoalPosition(AvatarIKGoal index)
	{
		InternalGetGoalPosition_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetGoalPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetGoalPosition(AvatarIKGoal index, Vector3 pos)
	{
		InternalSetGoalPosition_Injected(ref this, index, ref pos);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetGoalRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Quaternion InternalGetGoalRotation(AvatarIKGoal index)
	{
		InternalGetGoalRotation_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetGoalRotation", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetGoalRotation(AvatarIKGoal index, Quaternion rot)
	{
		InternalSetGoalRotation_Injected(ref this, index, ref rot);
	}

	[NativeMethod(Name = "SetGoalWeightPosition", IsThreadSafe = true)]
	private void InternalSetGoalWeightPosition(AvatarIKGoal index, float value)
	{
		InternalSetGoalWeightPosition_Injected(ref this, index, value);
	}

	[NativeMethod(Name = "SetGoalWeightRotation", IsThreadSafe = true)]
	private void InternalSetGoalWeightRotation(AvatarIKGoal index, float value)
	{
		InternalSetGoalWeightRotation_Injected(ref this, index, value);
	}

	[NativeMethod(Name = "GetGoalWeightPosition", IsThreadSafe = true)]
	private float InternalGetGoalWeightPosition(AvatarIKGoal index)
	{
		return InternalGetGoalWeightPosition_Injected(ref this, index);
	}

	[NativeMethod(Name = "GetGoalWeightRotation", IsThreadSafe = true)]
	private float InternalGetGoalWeightRotation(AvatarIKGoal index)
	{
		return InternalGetGoalWeightRotation_Injected(ref this, index);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::GetHintPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private Vector3 InternalGetHintPosition(AvatarIKHint index)
	{
		InternalGetHintPosition_Injected(ref this, index, out var ret);
		return ret;
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetHintPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetHintPosition(AvatarIKHint index, Vector3 pos)
	{
		InternalSetHintPosition_Injected(ref this, index, ref pos);
	}

	[NativeMethod(Name = "SetHintWeightPosition", IsThreadSafe = true)]
	private void InternalSetHintWeightPosition(AvatarIKHint index, float value)
	{
		InternalSetHintWeightPosition_Injected(ref this, index, value);
	}

	[NativeMethod(Name = "GetHintWeightPosition", IsThreadSafe = true)]
	private float InternalGetHintWeightPosition(AvatarIKHint index)
	{
		return InternalGetHintWeightPosition_Injected(ref this, index);
	}

	[NativeMethod(Name = "AnimationHumanStreamBindings::SetLookAtPosition", IsFreeFunction = true, IsThreadSafe = true, HasExplicitThis = true)]
	private void InternalSetLookAtPosition(Vector3 lookAtPosition)
	{
		InternalSetLookAtPosition_Injected(ref this, ref lookAtPosition);
	}

	[NativeMethod(Name = "SetLookAtClampWeight", IsThreadSafe = true)]
	private void InternalSetLookAtClampWeight(float weight)
	{
		InternalSetLookAtClampWeight_Injected(ref this, weight);
	}

	[NativeMethod(Name = "SetLookAtBodyWeight", IsThreadSafe = true)]
	private void InternalSetLookAtBodyWeight(float weight)
	{
		InternalSetLookAtBodyWeight_Injected(ref this, weight);
	}

	[NativeMethod(Name = "SetLookAtHeadWeight", IsThreadSafe = true)]
	private void InternalSetLookAtHeadWeight(float weight)
	{
		InternalSetLookAtHeadWeight_Injected(ref this, weight);
	}

	[NativeMethod(Name = "SetLookAtEyesWeight", IsThreadSafe = true)]
	private void InternalSetLookAtEyesWeight(float weight)
	{
		InternalSetLookAtEyesWeight_Injected(ref this, weight);
	}

	[NativeMethod(Name = "SolveIK", IsThreadSafe = true)]
	private void InternalSolveIK()
	{
		InternalSolveIK_Injected(ref this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float GetHumanScale_Injected(ref AnimationHumanStream _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float GetFootHeight_Injected(ref AnimationHumanStream _unity_self, bool left);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalResetToStancePose_Injected(ref AnimationHumanStream _unity_self);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetGoalPositionFromPose_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetGoalRotationFromPose_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetBodyLocalPosition_Injected(ref AnimationHumanStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetBodyLocalPosition_Injected(ref AnimationHumanStream _unity_self, ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetBodyLocalRotation_Injected(ref AnimationHumanStream _unity_self, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetBodyLocalRotation_Injected(ref AnimationHumanStream _unity_self, ref Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetBodyPosition_Injected(ref AnimationHumanStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetBodyPosition_Injected(ref AnimationHumanStream _unity_self, ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetBodyRotation_Injected(ref AnimationHumanStream _unity_self, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetBodyRotation_Injected(ref AnimationHumanStream _unity_self, ref Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float InternalGetMuscle_Injected(ref AnimationHumanStream _unity_self, ref MuscleHandle muscle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetMuscle_Injected(ref AnimationHumanStream _unity_self, ref MuscleHandle muscle, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetLeftFootVelocity_Injected(ref AnimationHumanStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetRightFootVelocity_Injected(ref AnimationHumanStream _unity_self, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetGoalLocalPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetGoalLocalPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, ref Vector3 pos);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetGoalLocalRotation_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetGoalLocalRotation_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, ref Quaternion rot);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetGoalPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetGoalPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, ref Vector3 pos);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetGoalRotation_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetGoalRotation_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, ref Quaternion rot);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetGoalWeightPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetGoalWeightRotation_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float InternalGetGoalWeightPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float InternalGetGoalWeightRotation_Injected(ref AnimationHumanStream _unity_self, AvatarIKGoal index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalGetHintPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKHint index, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetHintPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKHint index, ref Vector3 pos);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetHintWeightPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKHint index, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float InternalGetHintWeightPosition_Injected(ref AnimationHumanStream _unity_self, AvatarIKHint index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetLookAtPosition_Injected(ref AnimationHumanStream _unity_self, ref Vector3 lookAtPosition);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetLookAtClampWeight_Injected(ref AnimationHumanStream _unity_self, float weight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetLookAtBodyWeight_Injected(ref AnimationHumanStream _unity_self, float weight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetLookAtHeadWeight_Injected(ref AnimationHumanStream _unity_self, float weight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetLookAtEyesWeight_Injected(ref AnimationHumanStream _unity_self, float weight);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSolveIK_Injected(ref AnimationHumanStream _unity_self);
}
