using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Animation/ScriptBindings/AvatarBuilder.bindings.h")]
public class AvatarBuilder
{
	public static Avatar BuildHumanAvatar(GameObject go, HumanDescription humanDescription)
	{
		if (go == null)
		{
			throw new NullReferenceException();
		}
		return BuildHumanAvatarInternal(go, humanDescription);
	}

	[FreeFunction("AvatarBuilderBindings::BuildHumanAvatar")]
	private static Avatar BuildHumanAvatarInternal(GameObject go, HumanDescription humanDescription)
	{
		return BuildHumanAvatarInternal_Injected(go, ref humanDescription);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("AvatarBuilderBindings::BuildGenericAvatar")]
	public static extern Avatar BuildGenericAvatar([NotNull] GameObject go, [NotNull] string rootMotionTransformName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Avatar BuildHumanAvatarInternal_Injected(GameObject go, ref HumanDescription humanDescription);
}
