using System;

namespace MoreMountains.NiceVibrations;

[Serializable]
public class MMNVRumbleWaveForm
{
	public long[] Pattern;

	public int[] LowFrequencyAmplitudes;

	public int[] HighFrequencyAmplitudes;
}
