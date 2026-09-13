using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[StaticAccessor("GetAudioManager()", StaticAccessorType.Dot)]
public sealed class Microphone
{
	public static extern string[] devices
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeName("GetRecordDevices")]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern int GetMicrophoneDeviceIDFromName(string name);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AudioClip StartRecord(int deviceID, bool loop, float lengthSec, int frequency);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void EndRecord(int deviceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool IsRecording(int deviceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true)]
	private static extern int GetRecordPosition(int deviceID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetDeviceCaps(int deviceID, out int minFreq, out int maxFreq);

	public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency)
	{
		int microphoneDeviceIDFromName = GetMicrophoneDeviceIDFromName(deviceName);
		if (microphoneDeviceIDFromName == -1)
		{
			throw new ArgumentException("Couldn't acquire device ID for device name " + deviceName);
		}
		if (lengthSec <= 0)
		{
			throw new ArgumentException("Length of recording must be greater than zero seconds (was: " + lengthSec + " seconds)");
		}
		if (lengthSec > 3600)
		{
			throw new ArgumentException("Length of recording must be less than one hour (was: " + lengthSec + " seconds)");
		}
		if (frequency <= 0)
		{
			throw new ArgumentException("Frequency of recording must be greater than zero (was: " + frequency + " Hz)");
		}
		return StartRecord(microphoneDeviceIDFromName, loop, lengthSec, frequency);
	}

	public static void End(string deviceName)
	{
		int microphoneDeviceIDFromName = GetMicrophoneDeviceIDFromName(deviceName);
		if (microphoneDeviceIDFromName != -1)
		{
			EndRecord(microphoneDeviceIDFromName);
		}
	}

	public static bool IsRecording(string deviceName)
	{
		int microphoneDeviceIDFromName = GetMicrophoneDeviceIDFromName(deviceName);
		if (microphoneDeviceIDFromName == -1)
		{
			return false;
		}
		return IsRecording(microphoneDeviceIDFromName);
	}

	public static int GetPosition(string deviceName)
	{
		int microphoneDeviceIDFromName = GetMicrophoneDeviceIDFromName(deviceName);
		if (microphoneDeviceIDFromName == -1)
		{
			return 0;
		}
		return GetRecordPosition(microphoneDeviceIDFromName);
	}

	public static void GetDeviceCaps(string deviceName, out int minFreq, out int maxFreq)
	{
		minFreq = 0;
		maxFreq = 0;
		int microphoneDeviceIDFromName = GetMicrophoneDeviceIDFromName(deviceName);
		if (microphoneDeviceIDFromName != -1)
		{
			GetDeviceCaps(microphoneDeviceIDFromName, out minFreq, out maxFreq);
		}
	}
}
