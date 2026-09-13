using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[UsedByNativeCode]
[NativeHeader("Modules/Animation/Avatar.h")]
public class Avatar : Object
{
	public extern bool isValid
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsValid")]
		get;
	}

	public extern bool isHuman
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("IsHuman")]
		get;
	}

	public HumanDescription humanDescription
	{
		get
		{
			get_humanDescription_Injected(out var ret);
			return ret;
		}
	}

	private Avatar()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void SetMuscleMinMax(int muscleId, float min, float max);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void SetParameter(int parameterId, float value);

	internal float GetAxisLength(int humanId)
	{
		return Internal_GetAxisLength(HumanTrait.GetBoneIndexFromMono(humanId));
	}

	internal Quaternion GetPreRotation(int humanId)
	{
		return Internal_GetPreRotation(HumanTrait.GetBoneIndexFromMono(humanId));
	}

	internal Quaternion GetPostRotation(int humanId)
	{
		return Internal_GetPostRotation(HumanTrait.GetBoneIndexFromMono(humanId));
	}

	internal Quaternion GetZYPostQ(int humanId, Quaternion parentQ, Quaternion q)
	{
		return Internal_GetZYPostQ(HumanTrait.GetBoneIndexFromMono(humanId), parentQ, q);
	}

	internal Quaternion GetZYRoll(int humanId, Vector3 uvw)
	{
		return Internal_GetZYRoll(HumanTrait.GetBoneIndexFromMono(humanId), uvw);
	}

	internal Vector3 GetLimitSign(int humanId)
	{
		return Internal_GetLimitSign(HumanTrait.GetBoneIndexFromMono(humanId));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAxisLength")]
	internal extern float Internal_GetAxisLength(int humanId);

	[NativeMethod("GetPreRotation")]
	internal Quaternion Internal_GetPreRotation(int humanId)
	{
		Internal_GetPreRotation_Injected(humanId, out var ret);
		return ret;
	}

	[NativeMethod("GetPostRotation")]
	internal Quaternion Internal_GetPostRotation(int humanId)
	{
		Internal_GetPostRotation_Injected(humanId, out var ret);
		return ret;
	}

	[NativeMethod("GetZYPostQ")]
	internal Quaternion Internal_GetZYPostQ(int humanId, Quaternion parentQ, Quaternion q)
	{
		Internal_GetZYPostQ_Injected(humanId, ref parentQ, ref q, out var ret);
		return ret;
	}

	[NativeMethod("GetZYRoll")]
	internal Quaternion Internal_GetZYRoll(int humanId, Vector3 uvw)
	{
		Internal_GetZYRoll_Injected(humanId, ref uvw, out var ret);
		return ret;
	}

	[NativeMethod("GetLimitSign")]
	internal Vector3 Internal_GetLimitSign(int humanId)
	{
		Internal_GetLimitSign_Injected(humanId, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_humanDescription_Injected(out HumanDescription ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_GetPreRotation_Injected(int humanId, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_GetPostRotation_Injected(int humanId, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_GetZYPostQ_Injected(int humanId, ref Quaternion parentQ, ref Quaternion q, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_GetZYRoll_Injected(int humanId, ref Vector3 uvw, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_GetLimitSign_Injected(int humanId, out Vector3 ret);
}
