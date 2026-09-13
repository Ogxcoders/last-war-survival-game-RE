using System;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

[Serializable]
public class PresetDemoItem
{
	public string Name;

	public TextAsset AHAPFile;

	public Sprite AssociatedSprite;

	public AudioSource AssociatedSound;

	public MMNVAndroidWaveFormAsset WaveFormAsset;

	public MMNVRumbleWaveFormAsset RumbleWaveFormAsset;
}
