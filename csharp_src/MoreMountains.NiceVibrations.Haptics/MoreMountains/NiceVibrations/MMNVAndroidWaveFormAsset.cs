using UnityEngine;

namespace MoreMountains.NiceVibrations;

[CreateAssetMenu(fileName = "AndroidWaveFormAsset", menuName = "MoreMountains/NiceVibrations/AndroidWaveFormAsset")]
public class MMNVAndroidWaveFormAsset : ScriptableObject
{
	[Header("Properties")]
	public MMNVAndroidWaveForm WaveForm;

	[Header("AHAP")]
	public TextAsset AHAPFile;

	public float IntensityMultiplier = 1f;

	public float SharpnessMultiplier = 1f;
}
