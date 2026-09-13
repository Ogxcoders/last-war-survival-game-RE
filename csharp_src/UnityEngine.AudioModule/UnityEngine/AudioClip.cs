using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("Modules/Audio/Public/ScriptBindings/Audio.bindings.h")]
[StaticAccessor("AudioClipBindings", StaticAccessorType.DoubleColon)]
public sealed class AudioClip : Object
{
	public delegate void PCMReaderCallback(float[] data);

	public delegate void PCMSetPositionCallback(int position);

	[NativeProperty("LengthSec")]
	public extern float length
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("SampleCount")]
	public extern int samples
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[NativeProperty("ChannelCount")]
	public extern int channels
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int frequency
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[Obsolete("Use AudioClip.loadState instead to get more detailed information about the loading process.")]
	public extern bool isReadyToPlay
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("ReadyToPlay")]
		get;
	}

	public extern AudioClipLoadType loadType
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool preloadAudioData
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool ambisonic
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern bool loadInBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern AudioDataLoadState loadState
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "AudioClipBindings::GetLoadState", HasExplicitThis = true)]
		get;
	}

	private event PCMReaderCallback m_PCMReaderCallback = null;

	private event PCMSetPositionCallback m_PCMSetPositionCallback = null;

	private AudioClip()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool GetData(AudioClip clip, [Out] float[] data, int numSamples, int samplesOffset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool SetData(AudioClip clip, float[] data, int numsamples, int samplesOffset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AudioClip Construct_Internal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern string GetName();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void CreateUserSound(string name, int lengthSamples, int channels, int frequency, bool stream);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool LoadAudioData();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool UnloadAudioData();

	public bool GetData(float[] data, int offsetSamples)
	{
		if (channels <= 0)
		{
			Debug.Log("AudioClip.GetData failed; AudioClip " + GetName() + " contains no data");
			return false;
		}
		int numSamples = ((data != null) ? (data.Length / channels) : 0);
		return GetData(this, data, numSamples, offsetSamples);
	}

	public bool SetData(float[] data, int offsetSamples)
	{
		if (channels <= 0)
		{
			Debug.Log("AudioClip.SetData failed; AudioClip " + GetName() + " contains no data");
			return false;
		}
		if (offsetSamples < 0 || offsetSamples >= samples)
		{
			throw new ArgumentException("AudioClip.SetData failed; invalid offsetSamples");
		}
		if (data == null || data.Length == 0)
		{
			throw new ArgumentException("AudioClip.SetData failed; invalid data");
		}
		return SetData(this, data, data.Length / channels, offsetSamples);
	}

	[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream)
	{
		return Create(name, lengthSamples, channels, frequency, stream);
	}

	[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, PCMReaderCallback pcmreadercallback)
	{
		return Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
	}

	[Obsolete("The _3D argument of AudioClip is deprecated. Use the spatialBlend property of AudioSource instead to morph between 2D and 3D playback.")]
	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool _3D, bool stream, PCMReaderCallback pcmreadercallback, PCMSetPositionCallback pcmsetpositioncallback)
	{
		return Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, pcmsetpositioncallback);
	}

	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream)
	{
		return Create(name, lengthSamples, channels, frequency, stream, null, null);
	}

	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, PCMReaderCallback pcmreadercallback)
	{
		return Create(name, lengthSamples, channels, frequency, stream, pcmreadercallback, null);
	}

	public static AudioClip Create(string name, int lengthSamples, int channels, int frequency, bool stream, PCMReaderCallback pcmreadercallback, PCMSetPositionCallback pcmsetpositioncallback)
	{
		if (name == null)
		{
			throw new NullReferenceException();
		}
		if (lengthSamples <= 0)
		{
			throw new ArgumentException("Length of created clip must be larger than 0");
		}
		if (channels <= 0)
		{
			throw new ArgumentException("Number of channels in created clip must be greater than 0");
		}
		if (frequency <= 0)
		{
			throw new ArgumentException("Frequency in created clip must be greater than 0");
		}
		AudioClip audioClip = Construct_Internal();
		if (pcmreadercallback != null)
		{
			audioClip.m_PCMReaderCallback += pcmreadercallback;
		}
		if (pcmsetpositioncallback != null)
		{
			audioClip.m_PCMSetPositionCallback += pcmsetpositioncallback;
		}
		audioClip.CreateUserSound(name, lengthSamples, channels, frequency, stream);
		return audioClip;
	}

	[RequiredByNativeCode]
	private void InvokePCMReaderCallback_Internal(float[] data)
	{
		if (this.m_PCMReaderCallback != null)
		{
			this.m_PCMReaderCallback(data);
		}
	}

	[RequiredByNativeCode]
	private void InvokePCMSetPositionCallback_Internal(int position)
	{
		if (this.m_PCMSetPositionCallback != null)
		{
			this.m_PCMSetPositionCallback(position);
		}
	}
}
