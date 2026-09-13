using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TimelineAudioComponent : MonoBehaviour
{
	private AudioSource audioSource;

	private void OnEnable()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
		TimelineAudioManager.Inst.AddComponent(this);
	}

	private void OnDisable()
	{
		TimelineAudioManager.Inst.RemoveComponent(this);
	}

	public void ChangeVolume(float volume)
	{
		audioSource.volume = volume;
	}

	public void ChangeMute(bool mute)
	{
		audioSource.mute = mute;
	}
}
