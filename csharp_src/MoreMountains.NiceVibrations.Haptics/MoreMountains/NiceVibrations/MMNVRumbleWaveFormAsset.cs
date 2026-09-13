using UnityEngine;

namespace MoreMountains.NiceVibrations;

[CreateAssetMenu(fileName = "RumbleWaveFormAsset", menuName = "MoreMountains/NiceVibrations/RumbleWaveFormAsset")]
public class MMNVRumbleWaveFormAsset : ScriptableObject
{
	[Header("Properties")]
	public MMNVRumbleWaveForm WaveForm;

	[Header("AHAP")]
	public TextAsset AHAPFile;

	public float IntensityMultiplier = 1f;

	public float SharpnessMultiplier = 1f;
}
