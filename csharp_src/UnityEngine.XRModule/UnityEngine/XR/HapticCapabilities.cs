using System;
using UnityEngine.Bindings;

namespace UnityEngine.XR;

[NativeConditional("ENABLE_VR")]
public struct HapticCapabilities : IEquatable<HapticCapabilities>
{
	private uint m_NumChannels;

	private bool m_SupportsImpulse;

	private bool m_SupportsBuffer;

	private uint m_BufferFrequencyHz;

	private uint m_BufferMaxSize;

	private uint m_BufferOptimalSize;

	public uint numChannels
	{
		get
		{
			return m_NumChannels;
		}
		internal set
		{
			m_NumChannels = value;
		}
	}

	public bool supportsImpulse
	{
		get
		{
			return m_SupportsImpulse;
		}
		internal set
		{
			m_SupportsImpulse = value;
		}
	}

	public bool supportsBuffer
	{
		get
		{
			return m_SupportsBuffer;
		}
		internal set
		{
			m_SupportsBuffer = value;
		}
	}

	public uint bufferFrequencyHz
	{
		get
		{
			return m_BufferFrequencyHz;
		}
		internal set
		{
			m_BufferFrequencyHz = value;
		}
	}

	public uint bufferMaxSize
	{
		get
		{
			return m_BufferMaxSize;
		}
		internal set
		{
			m_BufferMaxSize = value;
		}
	}

	public uint bufferOptimalSize
	{
		get
		{
			return m_BufferOptimalSize;
		}
		internal set
		{
			m_BufferOptimalSize = value;
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is HapticCapabilities))
		{
			return false;
		}
		return Equals((HapticCapabilities)obj);
	}

	public bool Equals(HapticCapabilities other)
	{
		return numChannels == other.numChannels && supportsImpulse == other.supportsImpulse && supportsBuffer == other.supportsBuffer && bufferFrequencyHz == other.bufferFrequencyHz && bufferMaxSize == other.bufferMaxSize && bufferOptimalSize == other.bufferOptimalSize;
	}

	public override int GetHashCode()
	{
		return numChannels.GetHashCode() ^ (supportsImpulse.GetHashCode() << 1) ^ (supportsBuffer.GetHashCode() >> 1) ^ (bufferFrequencyHz.GetHashCode() << 2) ^ (bufferMaxSize.GetHashCode() >> 2) ^ (bufferOptimalSize.GetHashCode() << 3);
	}

	public static bool operator ==(HapticCapabilities a, HapticCapabilities b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(HapticCapabilities a, HapticCapabilities b)
	{
		return !(a == b);
	}
}
