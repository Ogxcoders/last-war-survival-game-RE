using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TimelineAudioManager
{
	private static TimelineAudioManager _inst;

	private List<TimelineAudioComponent> _activeComponents = new List<TimelineAudioComponent>();

	private float _volume;

	private bool _mute;

	private const string _soundGroupName = "Effect";

	public static TimelineAudioManager Inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = new TimelineAudioManager();
			}
			return _inst;
		}
	}

	private TimelineAudioManager()
	{
		_volume = GameEntry.Setting.GetFloat("EFFECT_VOLUME", 1f);
		_mute = !GameEntry.Setting.GetBool("isEffectMusicOn");
	}

	public void ChangeVolume(string soundGroupName, float volume)
	{
		if ("Effect" != soundGroupName)
		{
			return;
		}
		_volume = volume;
		for (int i = 0; i < _activeComponents.Count; i++)
		{
			if ((bool)_activeComponents[i])
			{
				_activeComponents[i].ChangeVolume(volume);
			}
		}
	}

	public void ChangeMute(string soundGroupName, bool mute)
	{
		if ("Effect" != soundGroupName)
		{
			return;
		}
		_mute = mute;
		for (int i = 0; i < _activeComponents.Count; i++)
		{
			if ((bool)_activeComponents[i])
			{
				_activeComponents[i].ChangeMute(mute);
			}
		}
	}

	public void AddComponent(TimelineAudioComponent component)
	{
		if (Application.isPlaying)
		{
			_activeComponents.Add(component);
			component.ChangeVolume(_volume);
			component.ChangeMute(_mute);
		}
	}

	public void RemoveComponent(TimelineAudioComponent component)
	{
		if (Application.isPlaying)
		{
			_activeComponents.RemoveSwapBack(component);
		}
	}

	public void Clear()
	{
		_activeComponents.Clear();
	}
}
