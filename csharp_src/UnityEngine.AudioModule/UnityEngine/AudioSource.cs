using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Audio;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine;

[StaticAccessor("AudioSourceBindings", StaticAccessorType.DoubleColon)]
[RequireComponent(typeof(Transform))]
public sealed class AudioSource : AudioBehaviour
{
	public extern float volume
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public float pitch
	{
		get
		{
			return GetPitch(this);
		}
		set
		{
			SetPitch(this, value);
		}
	}

	[NativeProperty("SecPosition")]
	public extern float time
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("SamplePosition")]
	public extern int timeSamples
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(IsThreadSafe = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(IsThreadSafe = true)]
		set;
	}

	[NativeProperty("AudioClip")]
	public extern AudioClip clip
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern AudioMixerGroup outputAudioMixerGroup
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool isPlaying
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("IsPlayingScripting")]
		get;
	}

	public extern bool isVirtual
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetLastVirtualState")]
		get;
	}

	public extern bool loop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool ignoreListenerVolume
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool playOnAwake
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool ignoreListenerPause
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern AudioVelocityUpdateMode velocityUpdateMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("StereoPan")]
	public extern float panStereo
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[NativeProperty("SpatialBlendMix")]
	public extern float spatialBlend
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool spatialize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool spatializePostEffects
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float reverbZoneMix
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool bypassEffects
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool bypassListenerEffects
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool bypassReverbZones
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float dopplerLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float spread
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int priority
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern bool mute
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float minDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float maxDistance
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern AudioRolloffMode rolloffMode
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
	public float minVolume
	{
		get
		{
			Debug.LogError("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
			return 0f;
		}
		set
		{
			Debug.LogError("minVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
		}
	}

	[Obsolete("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
	public float maxVolume
	{
		get
		{
			Debug.LogError("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
			return 0f;
		}
		set
		{
			Debug.LogError("maxVolume is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
		}
	}

	[Obsolete("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.", true)]
	public float rolloffFactor
	{
		get
		{
			Debug.LogError("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
			return 0f;
		}
		set
		{
			Debug.LogError("rolloffFactor is not supported anymore. Use min-, maxDistance and rolloffMode instead.");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern float GetPitch([NotNull] AudioSource source);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetPitch([NotNull] AudioSource source, float pitch);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void PlayHelper([NotNull] AudioSource source, ulong delay);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Play(double delay);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void PlayOneShotHelper([NotNull] AudioSource source, AudioClip clip, float volumeScale);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void Stop(bool stopOneShots);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetCustomCurveHelper([NotNull] AudioSource source, AudioSourceCurveType type, AnimationCurve curve);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AnimationCurve GetCustomCurveHelper([NotNull] AudioSource source, AudioSourceCurveType type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetOutputDataHelper([NotNull] AudioSource source, [Out] float[] samples, int channel);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void GetSpectrumDataHelper([NotNull] AudioSource source, [Out] float[] samples, int channel, FFTWindow window);

	[ExcludeFromDocs]
	public void Play()
	{
		PlayHelper(this, 0uL);
	}

	public void Play([DefaultValue("0")] ulong delay)
	{
		PlayHelper(this, delay);
	}

	public void PlayDelayed(float delay)
	{
		Play((delay < 0f) ? 0.0 : (0.0 - (double)delay));
	}

	public void PlayScheduled(double time)
	{
		Play((time < 0.0) ? 0.0 : time);
	}

	[ExcludeFromDocs]
	public void PlayOneShot(AudioClip clip)
	{
		PlayOneShot(clip, 1f);
	}

	public void PlayOneShot(AudioClip clip, [DefaultValue("1.0F")] float volumeScale)
	{
		if (clip == null)
		{
			Debug.LogWarning("PlayOneShot was called with a null AudioClip.");
		}
		else
		{
			PlayOneShotHelper(this, clip, volumeScale);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetScheduledStartTime(double time);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void SetScheduledEndTime(double time);

	public void Stop()
	{
		Stop(stopOneShots: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Pause();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void UnPause();

	[ExcludeFromDocs]
	public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
	{
		PlayClipAtPoint(clip, position, 1f);
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position, [DefaultValue("1.0F")] float volume)
	{
		GameObject gameObject = new GameObject("One shot audio");
		gameObject.transform.position = position;
		AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
		audioSource.clip = clip;
		audioSource.spatialBlend = 1f;
		audioSource.volume = volume;
		audioSource.Play();
		Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
	}

	public void SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve)
	{
		SetCustomCurveHelper(this, type, curve);
	}

	public AnimationCurve GetCustomCurve(AudioSourceCurveType type)
	{
		return GetCustomCurveHelper(this, type);
	}

	[Obsolete("GetOutputData returning a float[] is deprecated, use GetOutputData and pass a pre allocated array instead.")]
	public float[] GetOutputData(int numSamples, int channel)
	{
		float[] array = new float[numSamples];
		GetOutputDataHelper(this, array, channel);
		return array;
	}

	public void GetOutputData(float[] samples, int channel)
	{
		GetOutputDataHelper(this, samples, channel);
	}

	[Obsolete("GetSpectrumData returning a float[] is deprecated, use GetSpectrumData and pass a pre allocated array instead.")]
	public float[] GetSpectrumData(int numSamples, int channel, FFTWindow window)
	{
		float[] array = new float[numSamples];
		GetSpectrumDataHelper(this, array, channel, window);
		return array;
	}

	public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
	{
		GetSpectrumDataHelper(this, samples, channel, window);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool SetSpatializerFloat(int index, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool GetSpatializerFloat(int index, out float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool GetAmbisonicDecoderFloat(int index, out float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool SetAmbisonicDecoderFloat(int index, float value);
}
