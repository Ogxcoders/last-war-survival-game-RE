using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ManualUpdateTimeline : ManualUpdatorComponent
{
	public PlayableDirector director;

	[NonSerialized]
	private bool playOnAwake;

	[NonSerialized]
	private double duration;

	private void Awake()
	{
		if (director == null)
		{
			director = GetComponent<PlayableDirector>();
		}
	}

	private new void OnEnable()
	{
		playOnAwake = director.playOnAwake;
		duration = director.duration;
		director.playOnAwake = false;
		director.timeUpdateMode = DirectorUpdateMode.Manual;
		base.OnEnable();
		foreach (TrackAsset rootTrack in (director.playableAsset as TimelineAsset).GetRootTracks())
		{
			rootTrack.muted = true;
		}
	}

	private new void OnDisable()
	{
		director.playOnAwake = playOnAwake;
		director.timeUpdateMode = DirectorUpdateMode.GameTime;
		base.OnDisable();
		foreach (TrackAsset rootTrack in (director.playableAsset as TimelineAsset).GetRootTracks())
		{
			rootTrack.muted = false;
		}
		director.enabled = false;
		director.enabled = true;
		director.Resume();
	}

	public override void ManualUpdate(float delta)
	{
		if (director.time + (double)delta > duration)
		{
			director.time = director.time + (double)delta - duration;
		}
		else
		{
			director.time += (double)delta;
		}
		director.Evaluate();
	}
}
