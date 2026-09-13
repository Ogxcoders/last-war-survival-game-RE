using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Animation/HumanTrait.h")]
public class HumanTrait
{
	public static extern int MuscleCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static extern string[] MuscleName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetMuscleNames")]
		get;
	}

	public static extern int BoneCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public static extern string[] BoneName
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("MonoBoneNames")]
		get;
	}

	public static extern int RequiredBoneCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("RequiredBoneCount")]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int GetBoneIndexFromMono(int humanId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int GetBoneIndexToMono(int boneIndex);

	public static int MuscleFromBone(int i, int dofIndex)
	{
		return Internal_MuscleFromBone(GetBoneIndexFromMono(i), dofIndex);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("MuscleFromBone")]
	private static extern int Internal_MuscleFromBone(int i, int dofIndex);

	public static int BoneFromMuscle(int i)
	{
		return GetBoneIndexToMono(Internal_BoneFromMuscle(i));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("BoneFromMuscle")]
	private static extern int Internal_BoneFromMuscle(int i);

	public static bool RequiredBone(int i)
	{
		return Internal_RequiredBone(GetBoneIndexFromMono(i));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("RequiredBone")]
	private static extern bool Internal_RequiredBone(int i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern float GetMuscleDefaultMin(int i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern float GetMuscleDefaultMax(int i);

	public static float GetBoneDefaultHierarchyMass(int i)
	{
		return Internal_GetBoneHierarchyMass(GetBoneIndexFromMono(i));
	}

	public static int GetParentBone(int i)
	{
		int num = Internal_GetParent(GetBoneIndexFromMono(i));
		return (num != -1) ? GetBoneIndexToMono(num) : (-1);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetBoneHierarchyMass")]
	private static extern float Internal_GetBoneHierarchyMass(int i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetParent")]
	private static extern int Internal_GetParent(int i);
}
