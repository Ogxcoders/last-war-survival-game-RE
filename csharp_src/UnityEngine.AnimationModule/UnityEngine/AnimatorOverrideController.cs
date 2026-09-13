using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/Animation/AnimatorOverrideController.h")]
[NativeHeader("Modules/Animation/ScriptBindings/Animation.bindings.h")]
[UsedByNativeCode]
public class AnimatorOverrideController : RuntimeAnimatorController
{
	internal delegate void OnOverrideControllerDirtyCallback();

	internal OnOverrideControllerDirtyCallback OnOverrideControllerDirty;

	public extern RuntimeAnimatorController runtimeAnimatorController
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetAnimatorController")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetAnimatorController")]
		set;
	}

	public AnimationClip this[string name]
	{
		get
		{
			return Internal_GetClipByName(name, returnEffectiveClip: true);
		}
		set
		{
			Internal_SetClipByName(name, value);
		}
	}

	public AnimationClip this[AnimationClip clip]
	{
		get
		{
			return GetClip(clip, returnEffectiveClip: true);
		}
		set
		{
			SetClip(clip, value, notify: true);
		}
	}

	public extern int overridesCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetOriginalClipsCount")]
		get;
	}

	[Obsolete("AnimatorOverrideController.clips property is deprecated. Use AnimatorOverrideController.GetOverrides and AnimatorOverrideController.ApplyOverrides instead.")]
	public AnimationClipPair[] clips
	{
		get
		{
			int num = overridesCount;
			AnimationClipPair[] array = new AnimationClipPair[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new AnimationClipPair();
				array[i].originalClip = GetOriginalClip(i);
				array[i].overrideClip = GetOverrideClip(array[i].originalClip);
			}
			return array;
		}
		set
		{
			for (int i = 0; i < value.Length; i++)
			{
				SetClip(value[i].originalClip, value[i].overrideClip, notify: false);
			}
			SendNotification();
		}
	}

	public AnimatorOverrideController()
	{
		Internal_Create(this, null);
		OnOverrideControllerDirty = null;
	}

	public AnimatorOverrideController(RuntimeAnimatorController controller)
	{
		Internal_Create(this, controller);
		OnOverrideControllerDirty = null;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("AnimationBindings::CreateAnimatorOverrideController")]
	private static extern void Internal_Create([Writable] AnimatorOverrideController self, RuntimeAnimatorController controller);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetClip")]
	private extern AnimationClip Internal_GetClipByName(string name, bool returnEffectiveClip);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("SetClip")]
	private extern void Internal_SetClipByName(string name, AnimationClip clip);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern AnimationClip GetClip(AnimationClip originalClip, bool returnEffectiveClip);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetClip(AnimationClip originalClip, AnimationClip overrideClip, bool notify);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SendNotification();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern AnimationClip GetOriginalClip(int index);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern AnimationClip GetOverrideClip(AnimationClip originalClip);

	public void GetOverrides(List<KeyValuePair<AnimationClip, AnimationClip>> overrides)
	{
		if (overrides == null)
		{
			throw new ArgumentNullException("overrides");
		}
		int num = overridesCount;
		if (overrides.Capacity < num)
		{
			overrides.Capacity = num;
		}
		overrides.Clear();
		for (int i = 0; i < num; i++)
		{
			AnimationClip originalClip = GetOriginalClip(i);
			overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(originalClip, GetOverrideClip(originalClip)));
		}
	}

	public void ApplyOverrides(IList<KeyValuePair<AnimationClip, AnimationClip>> overrides)
	{
		if (overrides == null)
		{
			throw new ArgumentNullException("overrides");
		}
		for (int i = 0; i < overrides.Count; i++)
		{
			SetClip(overrides[i].Key, overrides[i].Value, notify: false);
		}
		SendNotification();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("UNITY_EDITOR")]
	internal extern void PerformOverrideClipListCleanup();

	[NativeConditional("UNITY_EDITOR")]
	[RequiredByNativeCode]
	internal static void OnInvalidateOverrideController(AnimatorOverrideController controller)
	{
		if (controller.OnOverrideControllerDirty != null)
		{
			controller.OnOverrideControllerDirty();
		}
	}
}
