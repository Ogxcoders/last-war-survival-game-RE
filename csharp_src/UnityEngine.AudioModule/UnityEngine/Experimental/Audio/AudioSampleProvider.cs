using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Audio;

[NativeType(Header = "Modules/Audio/Public/ScriptBindings/AudioSampleProvider.bindings.h")]
[StaticAccessor("AudioSampleProviderBindings", StaticAccessorType.DoubleColon)]
public class AudioSampleProvider : IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate uint ConsumeSampleFramesNativeFunction(uint providerId, IntPtr interleavedSampleFrames, uint sampleFrameCount);

	public delegate void SampleFramesHandler(AudioSampleProvider provider, uint sampleFrameCount);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SampleFramesEventNativeFunction(IntPtr userData, uint providerId, uint sampleFrameCount);

	private ConsumeSampleFramesNativeFunction m_ConsumeSampleFramesNativeFunction;

	public uint id { get; private set; }

	public ushort trackIndex { get; private set; }

	public Object owner { get; private set; }

	public bool valid => InternalIsValid(id);

	public ushort channelCount { get; private set; }

	public uint sampleRate { get; private set; }

	public uint maxSampleFrameCount => InternalGetMaxSampleFrameCount(id);

	public uint availableSampleFrameCount => InternalGetAvailableSampleFrameCount(id);

	public uint freeSampleFrameCount => InternalGetFreeSampleFrameCount(id);

	public uint freeSampleFrameCountLowThreshold
	{
		get
		{
			return InternalGetFreeSampleFrameCountLowThreshold(id);
		}
		set
		{
			InternalSetFreeSampleFrameCountLowThreshold(id, value);
		}
	}

	public bool enableSampleFramesAvailableEvents
	{
		get
		{
			return InternalGetEnableSampleFramesAvailableEvents(id);
		}
		set
		{
			InternalSetEnableSampleFramesAvailableEvents(id, value);
		}
	}

	public bool enableSilencePadding
	{
		get
		{
			return InternalGetEnableSilencePadding(id);
		}
		set
		{
			InternalSetEnableSilencePadding(id, value);
		}
	}

	public static ConsumeSampleFramesNativeFunction consumeSampleFramesNativeFunction => (ConsumeSampleFramesNativeFunction)Marshal.GetDelegateForFunctionPointer(InternalGetConsumeSampleFramesNativeFunctionPtr(), typeof(ConsumeSampleFramesNativeFunction));

	public event SampleFramesHandler sampleFramesAvailable;

	public event SampleFramesHandler sampleFramesOverflow;

	[VisibleToOtherModules]
	internal static AudioSampleProvider Lookup(uint providerId, Object ownerObj, ushort trackIndex)
	{
		AudioSampleProvider audioSampleProvider = InternalGetScriptingPtr(providerId);
		if (audioSampleProvider != null || !InternalIsValid(providerId))
		{
			return audioSampleProvider;
		}
		return new AudioSampleProvider(providerId, ownerObj, trackIndex);
	}

	internal static AudioSampleProvider Create(ushort channelCount, uint sampleRate)
	{
		uint providerId = InternalCreateSampleProvider(channelCount, sampleRate);
		if (!InternalIsValid(providerId))
		{
			return null;
		}
		return new AudioSampleProvider(providerId, null, 0);
	}

	private AudioSampleProvider(uint providerId, Object ownerObj, ushort trackIdx)
	{
		owner = ownerObj;
		id = providerId;
		trackIndex = trackIdx;
		m_ConsumeSampleFramesNativeFunction = (ConsumeSampleFramesNativeFunction)Marshal.GetDelegateForFunctionPointer(InternalGetConsumeSampleFramesNativeFunctionPtr(), typeof(ConsumeSampleFramesNativeFunction));
		ushort chCount = 0;
		uint sRate = 0u;
		InternalGetFormatInfo(providerId, out chCount, out sRate);
		channelCount = chCount;
		sampleRate = sRate;
		InternalSetScriptingPtr(providerId, this);
	}

	~AudioSampleProvider()
	{
		owner = null;
		Dispose();
	}

	public void Dispose()
	{
		if (id != 0)
		{
			InternalSetScriptingPtr(id, null);
			if (owner == null)
			{
				InternalRemove(id);
			}
			id = 0u;
		}
		GC.SuppressFinalize(this);
	}

	public unsafe uint ConsumeSampleFrames(NativeArray<float> sampleFrames)
	{
		if (channelCount == 0)
		{
			return 0u;
		}
		return m_ConsumeSampleFramesNativeFunction(id, (IntPtr)sampleFrames.GetUnsafePtr(), (uint)sampleFrames.Length / (uint)channelCount);
	}

	internal unsafe uint QueueSampleFrames(NativeArray<float> sampleFrames)
	{
		if (channelCount == 0)
		{
			return 0u;
		}
		return InternalQueueSampleFrames(id, (IntPtr)sampleFrames.GetUnsafeReadOnlyPtr(), (uint)(sampleFrames.Length / channelCount));
	}

	public void SetSampleFramesAvailableNativeHandler(SampleFramesEventNativeFunction handler, IntPtr userData)
	{
		InternalSetSampleFramesAvailableNativeHandler(id, Marshal.GetFunctionPointerForDelegate((Delegate)handler), userData);
	}

	public void ClearSampleFramesAvailableNativeHandler()
	{
		InternalClearSampleFramesAvailableNativeHandler(id);
	}

	public void SetSampleFramesOverflowNativeHandler(SampleFramesEventNativeFunction handler, IntPtr userData)
	{
		InternalSetSampleFramesOverflowNativeHandler(id, Marshal.GetFunctionPointerForDelegate((Delegate)handler), userData);
	}

	public void ClearSampleFramesOverflowNativeHandler()
	{
		InternalClearSampleFramesOverflowNativeHandler(id);
	}

	[RequiredByNativeCode]
	private void InvokeSampleFramesAvailable(int sampleFrameCount)
	{
		if (this.sampleFramesAvailable != null)
		{
			this.sampleFramesAvailable(this, (uint)sampleFrameCount);
		}
	}

	[RequiredByNativeCode]
	private void InvokeSampleFramesOverflow(int droppedSampleFrameCount)
	{
		if (this.sampleFramesOverflow != null)
		{
			this.sampleFramesOverflow(this, (uint)droppedSampleFrameCount);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern uint InternalCreateSampleProvider(ushort channelCount, uint sampleRate);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	internal static extern void InternalRemove(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void InternalGetFormatInfo(uint providerId, out ushort chCount, out uint sRate);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AudioSampleProvider InternalGetScriptingPtr(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void InternalSetScriptingPtr(uint providerId, AudioSampleProvider provider);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	internal static extern bool InternalIsValid(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern uint InternalGetMaxSampleFrameCount(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern uint InternalGetAvailableSampleFrameCount(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern uint InternalGetFreeSampleFrameCount(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern uint InternalGetFreeSampleFrameCountLowThreshold(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void InternalSetFreeSampleFrameCountLowThreshold(uint providerId, uint sampleFrameCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern bool InternalGetEnableSampleFramesAvailableEvents(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void InternalSetEnableSampleFramesAvailableEvents(uint providerId, bool enable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetSampleFramesAvailableNativeHandler(uint providerId, IntPtr handler, IntPtr userData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalClearSampleFramesAvailableNativeHandler(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetSampleFramesOverflowNativeHandler(uint providerId, IntPtr handler, IntPtr userData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalClearSampleFramesOverflowNativeHandler(uint providerId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern bool InternalGetEnableSilencePadding(uint id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern void InternalSetEnableSilencePadding(uint id, bool enabled);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern IntPtr InternalGetConsumeSampleFramesNativeFunctionPtr();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern uint InternalQueueSampleFrames(uint id, IntPtr interleavedSampleFrames, uint sampleFrameCount);
}
