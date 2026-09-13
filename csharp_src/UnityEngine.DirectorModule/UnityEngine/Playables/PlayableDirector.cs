using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables;

[RequiredByNativeCode]
[NativeHeader("Runtime/Mono/MonoBehaviour.h")]
[NativeHeader("Modules/Director/PlayableDirector.h")]
public class PlayableDirector : Behaviour, IExposedPropertyTable
{
	public PlayState state => GetPlayState();

	public DirectorWrapMode extrapolationMode
	{
		get
		{
			return GetWrapMode();
		}
		set
		{
			SetWrapMode(value);
		}
	}

	public PlayableAsset playableAsset
	{
		get
		{
			return Internal_GetPlayableAsset() as PlayableAsset;
		}
		set
		{
			SetPlayableAsset(value);
		}
	}

	public PlayableGraph playableGraph => GetGraphHandle();

	public bool playOnAwake
	{
		get
		{
			return GetPlayOnAwake();
		}
		set
		{
			SetPlayOnAwake(value);
		}
	}

	public extern DirectorUpdateMode timeUpdateMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern double time
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern double initialTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern double duration
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public event Action<PlayableDirector> played;

	public event Action<PlayableDirector> paused;

	public event Action<PlayableDirector> stopped;

	public void DeferredEvaluate()
	{
		EvaluateNextFrame();
	}

	public void Play(PlayableAsset asset)
	{
		if (asset == null)
		{
			throw new ArgumentNullException("asset");
		}
		Play(asset, extrapolationMode);
	}

	public void Play(PlayableAsset asset, DirectorWrapMode mode)
	{
		if (asset == null)
		{
			throw new ArgumentNullException("asset");
		}
		playableAsset = asset;
		extrapolationMode = mode;
		Play();
	}

	public void SetGenericBinding(Object key, Object value)
	{
		Internal_SetGenericBinding(key, value);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Evaluate();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Play();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Stop();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Pause();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Resume();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void RebuildGraph();

	public void ClearReferenceValue(PropertyName id)
	{
		ClearReferenceValue_Injected(ref id);
	}

	public void SetReferenceValue(PropertyName id, Object value)
	{
		SetReferenceValue_Injected(ref id, value);
	}

	public Object GetReferenceValue(PropertyName id, out bool idValid)
	{
		return GetReferenceValue_Injected(ref id, out idValid);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetBindingFor")]
	public extern Object GetGenericBinding(Object key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("ClearBindingFor")]
	public extern void ClearGenericBinding(Object key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void RebindPlayableGraphOutputs();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void ProcessPendingGraphChanges();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("HasBinding")]
	internal extern bool HasGenericBinding(Object key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern PlayState GetPlayState();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetWrapMode(DirectorWrapMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern DirectorWrapMode GetWrapMode();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void EvaluateNextFrame();

	private PlayableGraph GetGraphHandle()
	{
		GetGraphHandle_Injected(out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetPlayOnAwake(bool on);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern bool GetPlayOnAwake();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Internal_SetGenericBinding(Object key, Object value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetPlayableAsset(ScriptableObject asset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern ScriptableObject Internal_GetPlayableAsset();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeHeader("Runtime/Director/Core/DirectorManager.h")]
	[StaticAccessor("GetDirectorManager()", StaticAccessorType.Dot)]
	internal static extern void ResetFrameTiming();

	[RequiredByNativeCode]
	private void SendOnPlayableDirectorPlay()
	{
		if (this.played != null)
		{
			this.played(this);
		}
	}

	[RequiredByNativeCode]
	private void SendOnPlayableDirectorPause()
	{
		if (this.paused != null)
		{
			this.paused(this);
		}
	}

	[RequiredByNativeCode]
	private void SendOnPlayableDirectorStop()
	{
		if (this.stopped != null)
		{
			this.stopped(this);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void ClearReferenceValue_Injected(ref PropertyName id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void SetReferenceValue_Injected(ref PropertyName id, Object value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern Object GetReferenceValue_Injected(ref PropertyName id, out bool idValid);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void GetGraphHandle_Injected(out PlayableGraph ret);
}
