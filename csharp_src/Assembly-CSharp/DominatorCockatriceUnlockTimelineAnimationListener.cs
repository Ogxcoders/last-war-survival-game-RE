using System;
using UnityEngine;
using UnityEngine.Playables;

public class DominatorCockatriceUnlockTimelineAnimationListener : MonoBehaviour
{
	[SerializeField]
	private PlayableDirector _playableDirector;

	private int _soundUid;

	public Action animationMarker01 { get; set; }

	public Action animationMarker02 { get; set; }

	public Action animationMarker03 { get; set; }

	public void Awake()
	{
		_soundUid = GameEntry.Sound.PlayTimelineSound("Dominator/SFX_Env_Xunzhaohuoban_TL");
	}

	public void OnAnimationEvent01()
	{
		animationMarker01?.Invoke();
	}

	public void OnAnimationEvent02()
	{
		animationMarker02?.Invoke();
	}

	public void OnAnimationEvent03()
	{
		animationMarker03?.Invoke();
	}

	public void OnAnimationEvent04()
	{
		if (_playableDirector != null)
		{
			_playableDirector.time = 28.77;
			_playableDirector.Play();
		}
	}

	public void Play()
	{
		if (_playableDirector != null)
		{
			_playableDirector.Play();
		}
		if (_soundUid > 0)
		{
			GameEntry.Sound.ResumeSound(_soundUid);
		}
	}

	public void Pause()
	{
		if (_playableDirector != null)
		{
			_playableDirector.Pause();
		}
		if (_soundUid > 0)
		{
			GameEntry.Sound.PauseSound(_soundUid);
		}
	}

	public void Stop()
	{
		if (_playableDirector != null)
		{
			_playableDirector.Stop();
		}
	}
}
