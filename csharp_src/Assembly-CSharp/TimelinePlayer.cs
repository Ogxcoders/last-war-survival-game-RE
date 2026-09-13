using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelinePlayer : MonoBehaviour
{
	[Serializable]
	public class Data
	{
		public string animName;

		public TimelineAsset timelineAsset;
	}

	public List<Data> aniData = new List<Data>();

	private Dictionary<string, Data> _aniData = new Dictionary<string, Data>();

	public PlayableDirector playableDirector;

	private bool hasCallback;

	private void Awake()
	{
		for (int i = 0; i < aniData.Count; i++)
		{
			Data data = aniData[i];
			_aniData.Add(data.animName, data);
		}
	}

	private double PlayAnimation(string name, float startTime = 0f, DirectorWrapMode mode = DirectorWrapMode.Hold, bool rewind = true)
	{
		if (!_aniData.TryGetValue(name, out var value))
		{
			return 0.0;
		}
		if (rewind)
		{
			playableDirector.time = startTime;
		}
		else if (playableDirector.state == PlayState.Playing && playableDirector.playableAsset == value.timelineAsset)
		{
			return value.timelineAsset.duration;
		}
		playableDirector.Play(value.timelineAsset, mode);
		return value.timelineAsset.duration;
	}

	public double PlayTimeline(string name, bool toIdle = false, DirectorWrapMode mode = DirectorWrapMode.Hold, bool rewind = true)
	{
		double num = PlayAnimation(name, 0f, mode, rewind);
		if (num > 0.0)
		{
			if (toIdle && !hasCallback)
			{
				hasCallback = true;
				playableDirector.stopped += OnTimelineFinished;
				return num;
			}
			if (!toIdle && hasCallback)
			{
				hasCallback = false;
				playableDirector.stopped -= OnTimelineFinished;
			}
		}
		return num;
	}

	public void PlayIdleQueued()
	{
		if (!hasCallback)
		{
			hasCallback = true;
			playableDirector.stopped += OnTimelineFinished;
		}
	}

	private void OnTimelineFinished(PlayableDirector director)
	{
		if (director == playableDirector)
		{
			PlayAnimation("idle", 0f, DirectorWrapMode.Loop);
			hasCallback = false;
			playableDirector.stopped -= OnTimelineFinished;
		}
	}

	private void OnDestroy()
	{
		if (hasCallback)
		{
			playableDirector.stopped -= OnTimelineFinished;
			hasCallback = false;
		}
	}
}
