using System;
using System.Collections.Generic;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[UsedByNativeCode]
[NativeConditional("ENABLE_VR")]
public struct InputDevice : IEquatable<InputDevice>
{
	private static List<XRInputSubsystem> s_InputSubsystemCache;

	private ulong m_DeviceId;

	private bool m_Initialized;

	private ulong deviceId => m_Initialized ? m_DeviceId : ulong.MaxValue;

	public XRInputSubsystem subsystem
	{
		get
		{
			if (s_InputSubsystemCache == null)
			{
				s_InputSubsystemCache = new List<XRInputSubsystem>();
			}
			if (m_Initialized)
			{
				uint num = (uint)(m_DeviceId >> 32);
				SubsystemManager.GetInstances(s_InputSubsystemCache);
				for (int i = 0; i < s_InputSubsystemCache.Count; i++)
				{
					if (num == s_InputSubsystemCache[i].GetIndex())
					{
						return s_InputSubsystemCache[i];
					}
				}
			}
			return null;
		}
	}

	public bool isValid => IsValidId() && InputDevices.IsDeviceValid(m_DeviceId);

	public string name => IsValidId() ? InputDevices.GetDeviceName(m_DeviceId) : null;

	[Obsolete("This API has been marked as deprecated and will be removed in future versions. Please use InputDevice.characteristics instead.")]
	public InputDeviceRole role => IsValidId() ? InputDevices.GetDeviceRole(m_DeviceId) : InputDeviceRole.Unknown;

	public string manufacturer => IsValidId() ? InputDevices.GetDeviceManufacturer(m_DeviceId) : null;

	public string serialNumber => IsValidId() ? InputDevices.GetDeviceSerialNumber(m_DeviceId) : null;

	public InputDeviceCharacteristics characteristics => IsValidId() ? InputDevices.GetDeviceCharacteristics(m_DeviceId) : InputDeviceCharacteristics.None;

	internal InputDevice(ulong deviceId)
	{
		m_DeviceId = deviceId;
		m_Initialized = true;
	}

	private bool IsValidId()
	{
		return deviceId != ulong.MaxValue;
	}

	public bool SendHapticImpulse(uint channel, float amplitude, float duration = 1f)
	{
		if (!IsValidId())
		{
			return false;
		}
		if (amplitude < 0f)
		{
			throw new ArgumentException("Amplitude of SendHapticImpulse cannot be negative.");
		}
		if (duration < 0f)
		{
			throw new ArgumentException("Duration of SendHapticImpulse cannot be negative.");
		}
		return InputDevices.SendHapticImpulse(m_DeviceId, channel, amplitude, duration);
	}

	public bool SendHapticBuffer(uint channel, byte[] buffer)
	{
		if (!IsValidId())
		{
			return false;
		}
		return InputDevices.SendHapticBuffer(m_DeviceId, channel, buffer);
	}

	public bool TryGetHapticCapabilities(out HapticCapabilities capabilities)
	{
		if (CheckValidAndSetDefault<HapticCapabilities>(out capabilities))
		{
			return InputDevices.TryGetHapticCapabilities(m_DeviceId, out capabilities);
		}
		return false;
	}

	public void StopHaptics()
	{
		if (IsValidId())
		{
			InputDevices.StopHaptics(m_DeviceId);
		}
	}

	public bool TryGetFeatureUsages(List<InputFeatureUsage> featureUsages)
	{
		if (IsValidId())
		{
			return InputDevices.TryGetFeatureUsages(m_DeviceId, featureUsages);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<bool> usage, out bool value)
	{
		if (CheckValidAndSetDefault<bool>(out value))
		{
			return InputDevices.TryGetFeatureValue_bool(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<uint> usage, out uint value)
	{
		if (CheckValidAndSetDefault<uint>(out value))
		{
			return InputDevices.TryGetFeatureValue_UInt32(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<float> usage, out float value)
	{
		if (CheckValidAndSetDefault<float>(out value))
		{
			return InputDevices.TryGetFeatureValue_float(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Vector2> usage, out Vector2 value)
	{
		if (CheckValidAndSetDefault<Vector2>(out value))
		{
			return InputDevices.TryGetFeatureValue_Vector2f(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Vector3> usage, out Vector3 value)
	{
		if (CheckValidAndSetDefault<Vector3>(out value))
		{
			return InputDevices.TryGetFeatureValue_Vector3f(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Quaternion> usage, out Quaternion value)
	{
		if (CheckValidAndSetDefault<Quaternion>(out value))
		{
			return InputDevices.TryGetFeatureValue_Quaternionf(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Hand> usage, out Hand value)
	{
		if (CheckValidAndSetDefault<Hand>(out value))
		{
			return InputDevices.TryGetFeatureValue_XRHand(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Bone> usage, out Bone value)
	{
		if (CheckValidAndSetDefault<Bone>(out value))
		{
			return InputDevices.TryGetFeatureValue_XRBone(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Eyes> usage, out Eyes value)
	{
		if (CheckValidAndSetDefault<Eyes>(out value))
		{
			return InputDevices.TryGetFeatureValue_XREyes(m_DeviceId, usage.name, out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<byte[]> usage, byte[] value)
	{
		if (IsValidId())
		{
			return InputDevices.TryGetFeatureValue_Custom(m_DeviceId, usage.name, value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<InputTrackingState> usage, out InputTrackingState value)
	{
		if (IsValidId())
		{
			uint value2 = 0u;
			if (InputDevices.TryGetFeatureValue_UInt32(m_DeviceId, usage.name, out value2))
			{
				value = (InputTrackingState)value2;
				return true;
			}
		}
		value = InputTrackingState.None;
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<bool> usage, DateTime time, out bool value)
	{
		if (CheckValidAndSetDefault<bool>(out value))
		{
			return InputDevices.TryGetFeatureValueAtTime_bool(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<uint> usage, DateTime time, out uint value)
	{
		if (CheckValidAndSetDefault<uint>(out value))
		{
			return InputDevices.TryGetFeatureValueAtTime_UInt32(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<float> usage, DateTime time, out float value)
	{
		if (CheckValidAndSetDefault<float>(out value))
		{
			return InputDevices.TryGetFeatureValueAtTime_float(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Vector2> usage, DateTime time, out Vector2 value)
	{
		if (CheckValidAndSetDefault<Vector2>(out value))
		{
			return InputDevices.TryGetFeatureValueAtTime_Vector2f(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Vector3> usage, DateTime time, out Vector3 value)
	{
		if (CheckValidAndSetDefault<Vector3>(out value))
		{
			return InputDevices.TryGetFeatureValueAtTime_Vector3f(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<Quaternion> usage, DateTime time, out Quaternion value)
	{
		if (CheckValidAndSetDefault<Quaternion>(out value))
		{
			return InputDevices.TryGetFeatureValueAtTime_Quaternionf(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value);
		}
		return false;
	}

	public bool TryGetFeatureValue(InputFeatureUsage<InputTrackingState> usage, DateTime time, out InputTrackingState value)
	{
		if (IsValidId())
		{
			uint value2 = 0u;
			if (InputDevices.TryGetFeatureValueAtTime_UInt32(m_DeviceId, usage.name, TimeConverter.LocalDateTimeToUnixTimeMilliseconds(time), out value2))
			{
				value = (InputTrackingState)value2;
				return true;
			}
		}
		value = InputTrackingState.None;
		return false;
	}

	private bool CheckValidAndSetDefault<T>(out T value)
	{
		value = default(T);
		return IsValidId();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is InputDevice))
		{
			return false;
		}
		return Equals((InputDevice)obj);
	}

	public bool Equals(InputDevice other)
	{
		return deviceId == other.deviceId;
	}

	public override int GetHashCode()
	{
		return deviceId.GetHashCode();
	}

	public static bool operator ==(InputDevice a, InputDevice b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(InputDevice a, InputDevice b)
	{
		return !(a == b);
	}
}
