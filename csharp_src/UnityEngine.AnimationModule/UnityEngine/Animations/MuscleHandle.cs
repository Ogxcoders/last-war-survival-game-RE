using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Animations;

[MovedFrom("UnityEngine.Experimental.Animations")]
[NativeHeader("Modules/Animation/Animator.h")]
[NativeHeader("Modules/Animation/MuscleHandle.h")]
public struct MuscleHandle
{
	public HumanPartDof humanPartDof { get; private set; }

	public int dof { get; private set; }

	public string name => GetName();

	public static int muscleHandleCount => GetMuscleHandleCount();

	public MuscleHandle(BodyDof bodyDof)
	{
		humanPartDof = HumanPartDof.Body;
		dof = (int)bodyDof;
	}

	public MuscleHandle(HeadDof headDof)
	{
		humanPartDof = HumanPartDof.Head;
		dof = (int)headDof;
	}

	public MuscleHandle(HumanPartDof partDof, LegDof legDof)
	{
		if (partDof != HumanPartDof.LeftLeg && partDof != HumanPartDof.RightLeg)
		{
			throw new InvalidOperationException("Invalid HumanPartDof for a leg, please use either HumanPartDof.LeftLeg or HumanPartDof.RightLeg.");
		}
		humanPartDof = partDof;
		dof = (int)legDof;
	}

	public MuscleHandle(HumanPartDof partDof, ArmDof armDof)
	{
		if (partDof != HumanPartDof.LeftArm && partDof != HumanPartDof.RightArm)
		{
			throw new InvalidOperationException("Invalid HumanPartDof for an arm, please use either HumanPartDof.LeftArm or HumanPartDof.RightArm.");
		}
		humanPartDof = partDof;
		dof = (int)armDof;
	}

	public MuscleHandle(HumanPartDof partDof, FingerDof fingerDof)
	{
		if (partDof < HumanPartDof.LeftThumb || partDof > HumanPartDof.RightLittle)
		{
			throw new InvalidOperationException("Invalid HumanPartDof for a finger.");
		}
		humanPartDof = partDof;
		dof = (int)fingerDof;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void GetMuscleHandles([Out][NotNull] MuscleHandle[] muscleHandles);

	private string GetName()
	{
		return GetName_Injected(ref this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetMuscleHandleCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string GetName_Injected(ref MuscleHandle _unity_self);
}
