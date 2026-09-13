using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Animations;

[NativeHeader("Modules/Animation/ScriptBindings/AnimatorControllerPlayable.bindings.h")]
[NativeHeader("Modules/Animation/ScriptBindings/Animator.bindings.h")]
[NativeHeader("Modules/Animation/Director/AnimatorControllerPlayable.h")]
[NativeHeader("Modules/Animation/AnimatorInfo.h")]
[StaticAccessor("AnimatorControllerPlayableBindings", StaticAccessorType.DoubleColon)]
[RequiredByNativeCode]
[NativeHeader("Modules/Animation/RuntimeAnimatorController.h")]
public struct AnimatorControllerPlayable : IPlayable, IEquatable<AnimatorControllerPlayable>
{
	private PlayableHandle m_Handle;

	private static readonly AnimatorControllerPlayable m_NullPlayable = new AnimatorControllerPlayable(PlayableHandle.Null);

	public static AnimatorControllerPlayable Null => m_NullPlayable;

	public static AnimatorControllerPlayable Create(PlayableGraph graph, RuntimeAnimatorController controller)
	{
		PlayableHandle handle = CreateHandle(graph, controller);
		return new AnimatorControllerPlayable(handle);
	}

	private static PlayableHandle CreateHandle(PlayableGraph graph, RuntimeAnimatorController controller)
	{
		PlayableHandle handle = PlayableHandle.Null;
		if (!CreateHandleInternal(graph, controller, ref handle))
		{
			return PlayableHandle.Null;
		}
		return handle;
	}

	internal AnimatorControllerPlayable(PlayableHandle handle)
	{
		m_Handle = PlayableHandle.Null;
		SetHandle(handle);
	}

	public PlayableHandle GetHandle()
	{
		return m_Handle;
	}

	public void SetHandle(PlayableHandle handle)
	{
		if (m_Handle.IsValid())
		{
			throw new InvalidOperationException("Cannot call IPlayable.SetHandle on an instance that already contains a valid handle.");
		}
		if (handle.IsValid() && !handle.IsPlayableOfType<AnimatorControllerPlayable>())
		{
			throw new InvalidCastException("Can't set handle: the playable is not an AnimatorControllerPlayable.");
		}
		m_Handle = handle;
	}

	public static implicit operator Playable(AnimatorControllerPlayable playable)
	{
		return new Playable(playable.GetHandle());
	}

	public static explicit operator AnimatorControllerPlayable(Playable playable)
	{
		return new AnimatorControllerPlayable(playable.GetHandle());
	}

	public bool Equals(AnimatorControllerPlayable other)
	{
		return GetHandle() == other.GetHandle();
	}

	public float GetFloat(string name)
	{
		return GetFloatString(ref m_Handle, name);
	}

	public float GetFloat(int id)
	{
		return GetFloatID(ref m_Handle, id);
	}

	public void SetFloat(string name, float value)
	{
		SetFloatString(ref m_Handle, name, value);
	}

	public void SetFloat(int id, float value)
	{
		SetFloatID(ref m_Handle, id, value);
	}

	public bool GetBool(string name)
	{
		return GetBoolString(ref m_Handle, name);
	}

	public bool GetBool(int id)
	{
		return GetBoolID(ref m_Handle, id);
	}

	public void SetBool(string name, bool value)
	{
		SetBoolString(ref m_Handle, name, value);
	}

	public void SetBool(int id, bool value)
	{
		SetBoolID(ref m_Handle, id, value);
	}

	public int GetInteger(string name)
	{
		return GetIntegerString(ref m_Handle, name);
	}

	public int GetInteger(int id)
	{
		return GetIntegerID(ref m_Handle, id);
	}

	public void SetInteger(string name, int value)
	{
		SetIntegerString(ref m_Handle, name, value);
	}

	public void SetInteger(int id, int value)
	{
		SetIntegerID(ref m_Handle, id, value);
	}

	public void SetTrigger(string name)
	{
		SetTriggerString(ref m_Handle, name);
	}

	public void SetTrigger(int id)
	{
		SetTriggerID(ref m_Handle, id);
	}

	public void ResetTrigger(string name)
	{
		ResetTriggerString(ref m_Handle, name);
	}

	public void ResetTrigger(int id)
	{
		ResetTriggerID(ref m_Handle, id);
	}

	public bool IsParameterControlledByCurve(string name)
	{
		return IsParameterControlledByCurveString(ref m_Handle, name);
	}

	public bool IsParameterControlledByCurve(int id)
	{
		return IsParameterControlledByCurveID(ref m_Handle, id);
	}

	public int GetLayerCount()
	{
		return GetLayerCountInternal(ref m_Handle);
	}

	public string GetLayerName(int layerIndex)
	{
		return GetLayerNameInternal(ref m_Handle, layerIndex);
	}

	public int GetLayerIndex(string layerName)
	{
		return GetLayerIndexInternal(ref m_Handle, layerName);
	}

	public float GetLayerWeight(int layerIndex)
	{
		return GetLayerWeightInternal(ref m_Handle, layerIndex);
	}

	public void SetLayerWeight(int layerIndex, float weight)
	{
		SetLayerWeightInternal(ref m_Handle, layerIndex, weight);
	}

	public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
	{
		return GetCurrentAnimatorStateInfoInternal(ref m_Handle, layerIndex);
	}

	public AnimatorStateInfo GetNextAnimatorStateInfo(int layerIndex)
	{
		return GetNextAnimatorStateInfoInternal(ref m_Handle, layerIndex);
	}

	public AnimatorTransitionInfo GetAnimatorTransitionInfo(int layerIndex)
	{
		return GetAnimatorTransitionInfoInternal(ref m_Handle, layerIndex);
	}

	public AnimatorClipInfo[] GetCurrentAnimatorClipInfo(int layerIndex)
	{
		return GetCurrentAnimatorClipInfoInternal(ref m_Handle, layerIndex);
	}

	public void GetCurrentAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
	{
		if (clips == null)
		{
			throw new ArgumentNullException("clips");
		}
		GetAnimatorClipInfoInternal(ref m_Handle, layerIndex, isCurrent: true, clips);
	}

	public void GetNextAnimatorClipInfo(int layerIndex, List<AnimatorClipInfo> clips)
	{
		if (clips == null)
		{
			throw new ArgumentNullException("clips");
		}
		GetAnimatorClipInfoInternal(ref m_Handle, layerIndex, isCurrent: false, clips);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void GetAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex, bool isCurrent, object clips);

	public int GetCurrentAnimatorClipInfoCount(int layerIndex)
	{
		return GetAnimatorClipInfoCountInternal(ref m_Handle, layerIndex, current: true);
	}

	public int GetNextAnimatorClipInfoCount(int layerIndex)
	{
		return GetAnimatorClipInfoCountInternal(ref m_Handle, layerIndex, current: false);
	}

	public AnimatorClipInfo[] GetNextAnimatorClipInfo(int layerIndex)
	{
		return GetNextAnimatorClipInfoInternal(ref m_Handle, layerIndex);
	}

	public bool IsInTransition(int layerIndex)
	{
		return IsInTransitionInternal(ref m_Handle, layerIndex);
	}

	public int GetParameterCount()
	{
		return GetParameterCountInternal(ref m_Handle);
	}

	public AnimatorControllerParameter GetParameter(int index)
	{
		AnimatorControllerParameter[] parametersArrayInternal = GetParametersArrayInternal(ref m_Handle);
		if (index < 0 || index >= parametersArrayInternal.Length)
		{
			throw new IndexOutOfRangeException("Invalid parameter index.");
		}
		return parametersArrayInternal[index];
	}

	public void CrossFadeInFixedTime(string stateName, float transitionDuration)
	{
		CrossFadeInFixedTimeInternal(ref m_Handle, StringToHash(stateName), transitionDuration, -1, 0f);
	}

	public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer)
	{
		CrossFadeInFixedTimeInternal(ref m_Handle, StringToHash(stateName), transitionDuration, layer, 0f);
	}

	public void CrossFadeInFixedTime(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
	{
		CrossFadeInFixedTimeInternal(ref m_Handle, StringToHash(stateName), transitionDuration, layer, fixedTime);
	}

	public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration)
	{
		CrossFadeInFixedTimeInternal(ref m_Handle, stateNameHash, transitionDuration, -1, 0f);
	}

	public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, int layer)
	{
		CrossFadeInFixedTimeInternal(ref m_Handle, stateNameHash, transitionDuration, layer, 0f);
	}

	public void CrossFadeInFixedTime(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("0.0f")] float fixedTime)
	{
		CrossFadeInFixedTimeInternal(ref m_Handle, stateNameHash, transitionDuration, layer, fixedTime);
	}

	public void CrossFade(string stateName, float transitionDuration)
	{
		CrossFadeInternal(ref m_Handle, StringToHash(stateName), transitionDuration, -1, float.NegativeInfinity);
	}

	public void CrossFade(string stateName, float transitionDuration, int layer)
	{
		CrossFadeInternal(ref m_Handle, StringToHash(stateName), transitionDuration, layer, float.NegativeInfinity);
	}

	public void CrossFade(string stateName, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
	{
		CrossFadeInternal(ref m_Handle, StringToHash(stateName), transitionDuration, layer, normalizedTime);
	}

	public void CrossFade(int stateNameHash, float transitionDuration)
	{
		CrossFadeInternal(ref m_Handle, stateNameHash, transitionDuration, -1, float.NegativeInfinity);
	}

	public void CrossFade(int stateNameHash, float transitionDuration, int layer)
	{
		CrossFadeInternal(ref m_Handle, stateNameHash, transitionDuration, layer, float.NegativeInfinity);
	}

	public void CrossFade(int stateNameHash, float transitionDuration, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
	{
		CrossFadeInternal(ref m_Handle, stateNameHash, transitionDuration, layer, normalizedTime);
	}

	public void PlayInFixedTime(string stateName)
	{
		PlayInFixedTimeInternal(ref m_Handle, StringToHash(stateName), -1, float.NegativeInfinity);
	}

	public void PlayInFixedTime(string stateName, int layer)
	{
		PlayInFixedTimeInternal(ref m_Handle, StringToHash(stateName), layer, float.NegativeInfinity);
	}

	public void PlayInFixedTime(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
	{
		PlayInFixedTimeInternal(ref m_Handle, StringToHash(stateName), layer, fixedTime);
	}

	public void PlayInFixedTime(int stateNameHash)
	{
		PlayInFixedTimeInternal(ref m_Handle, stateNameHash, -1, float.NegativeInfinity);
	}

	public void PlayInFixedTime(int stateNameHash, int layer)
	{
		PlayInFixedTimeInternal(ref m_Handle, stateNameHash, layer, float.NegativeInfinity);
	}

	public void PlayInFixedTime(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float fixedTime)
	{
		PlayInFixedTimeInternal(ref m_Handle, stateNameHash, layer, fixedTime);
	}

	public void Play(string stateName)
	{
		PlayInternal(ref m_Handle, StringToHash(stateName), -1, float.NegativeInfinity);
	}

	public void Play(string stateName, int layer)
	{
		PlayInternal(ref m_Handle, StringToHash(stateName), layer, float.NegativeInfinity);
	}

	public void Play(string stateName, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
	{
		PlayInternal(ref m_Handle, StringToHash(stateName), layer, normalizedTime);
	}

	public void Play(int stateNameHash)
	{
		PlayInternal(ref m_Handle, stateNameHash, -1, float.NegativeInfinity);
	}

	public void Play(int stateNameHash, int layer)
	{
		PlayInternal(ref m_Handle, stateNameHash, layer, float.NegativeInfinity);
	}

	public void Play(int stateNameHash, [DefaultValue("-1")] int layer, [DefaultValue("float.NegativeInfinity")] float normalizedTime)
	{
		PlayInternal(ref m_Handle, stateNameHash, layer, normalizedTime);
	}

	public bool HasState(int layerIndex, int stateID)
	{
		return HasStateInternal(ref m_Handle, layerIndex, stateID);
	}

	internal string ResolveHash(int hash)
	{
		return ResolveHashInternal(ref m_Handle, hash);
	}

	[NativeThrows]
	private static bool CreateHandleInternal(PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle)
	{
		return CreateHandleInternal_Injected(ref graph, controller, ref handle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern RuntimeAnimatorController GetAnimatorControllerInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int GetLayerCountInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern string GetLayerNameInternal(ref PlayableHandle handle, int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int GetLayerIndexInternal(ref PlayableHandle handle, string layerName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern float GetLayerWeightInternal(ref PlayableHandle handle, int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetLayerWeightInternal(ref PlayableHandle handle, int layerIndex, float weight);

	[NativeThrows]
	private static AnimatorStateInfo GetCurrentAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
	{
		GetCurrentAnimatorStateInfoInternal_Injected(ref handle, layerIndex, out var ret);
		return ret;
	}

	[NativeThrows]
	private static AnimatorStateInfo GetNextAnimatorStateInfoInternal(ref PlayableHandle handle, int layerIndex)
	{
		GetNextAnimatorStateInfoInternal_Injected(ref handle, layerIndex, out var ret);
		return ret;
	}

	[NativeThrows]
	private static AnimatorTransitionInfo GetAnimatorTransitionInfoInternal(ref PlayableHandle handle, int layerIndex)
	{
		GetAnimatorTransitionInfoInternal_Injected(ref handle, layerIndex, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern AnimatorClipInfo[] GetCurrentAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int GetAnimatorClipInfoCountInternal(ref PlayableHandle handle, int layerIndex, bool current);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern AnimatorClipInfo[] GetNextAnimatorClipInfoInternal(ref PlayableHandle handle, int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern string ResolveHashInternal(ref PlayableHandle handle, int hash);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool IsInTransitionInternal(ref PlayableHandle handle, int layerIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern AnimatorControllerParameter[] GetParametersArrayInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int GetParameterCountInternal(ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private static extern int StringToHash(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void CrossFadeInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer, float fixedTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void CrossFadeInternal(ref PlayableHandle handle, int stateNameHash, float transitionDuration, int layer, float normalizedTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void PlayInFixedTimeInternal(ref PlayableHandle handle, int stateNameHash, int layer, float fixedTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void PlayInternal(ref PlayableHandle handle, int stateNameHash, int layer, float normalizedTime);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool HasStateInternal(ref PlayableHandle handle, int layerIndex, int stateID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetFloatString(ref PlayableHandle handle, string name, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetFloatID(ref PlayableHandle handle, int id, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern float GetFloatString(ref PlayableHandle handle, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern float GetFloatID(ref PlayableHandle handle, int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetBoolString(ref PlayableHandle handle, string name, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetBoolID(ref PlayableHandle handle, int id, bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetBoolString(ref PlayableHandle handle, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool GetBoolID(ref PlayableHandle handle, int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetIntegerString(ref PlayableHandle handle, string name, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetIntegerID(ref PlayableHandle handle, int id, int value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int GetIntegerString(ref PlayableHandle handle, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern int GetIntegerID(ref PlayableHandle handle, int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetTriggerString(ref PlayableHandle handle, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetTriggerID(ref PlayableHandle handle, int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void ResetTriggerString(ref PlayableHandle handle, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void ResetTriggerID(ref PlayableHandle handle, int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool IsParameterControlledByCurveString(ref PlayableHandle handle, string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern bool IsParameterControlledByCurveID(ref PlayableHandle handle, int id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool CreateHandleInternal_Injected(ref PlayableGraph graph, RuntimeAnimatorController controller, ref PlayableHandle handle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetCurrentAnimatorStateInfoInternal_Injected(ref PlayableHandle handle, int layerIndex, out AnimatorStateInfo ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetNextAnimatorStateInfoInternal_Injected(ref PlayableHandle handle, int layerIndex, out AnimatorStateInfo ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetAnimatorTransitionInfoInternal_Injected(ref PlayableHandle handle, int layerIndex, out AnimatorTransitionInfo ret);
}
