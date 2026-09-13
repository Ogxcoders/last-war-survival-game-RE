using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline;

public class DirectorControlPlayable : PlayableBehaviour
{
	public enum PauseAction
	{
		StopDirector,
		PauseDirector
	}

	public PlayableDirector director;

	public PauseAction pauseAction;

	private bool m_SyncTime;

	private double m_AssetDuration = double.MaxValue;

	public static ScriptPlayable<DirectorControlPlayable> Create(PlayableGraph graph, PlayableDirector director)
	{
		if (director == null)
		{
			return ScriptPlayable<DirectorControlPlayable>.Null;
		}
		ScriptPlayable<DirectorControlPlayable> result = ScriptPlayable<DirectorControlPlayable>.Create(graph);
		result.GetBehaviour().director = director;
		return result;
	}

	public override void OnPlayableDestroy(Playable playable)
	{
		if (director != null && director.playableAsset != null)
		{
			director.Stop();
		}
	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
		if (!(director == null) && director.isActiveAndEnabled && !(director.playableAsset == null))
		{
			m_SyncTime |= info.evaluationType == FrameData.EvaluationType.Evaluate || DetectDiscontinuity(playable, info);
			SyncSpeed(info.effectiveSpeed);
			SyncStart(playable.GetGraph(), playable.GetTime());
			SyncStop(playable.GetGraph(), playable.GetTime());
		}
	}

	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		m_SyncTime = true;
		if (director != null && director.playableAsset != null)
		{
			m_AssetDuration = director.playableAsset.duration;
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (director != null && director.playableAsset != null)
		{
			if (info.effectivePlayState == PlayState.Playing || (info.effectivePlayState == PlayState.Paused && pauseAction == PauseAction.PauseDirector))
			{
				director.Pause();
			}
			else
			{
				director.Stop();
			}
		}
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (director == null || !director.isActiveAndEnabled || director.playableAsset == null)
		{
			return;
		}
		if (m_SyncTime || DetectOutOfSync(playable))
		{
			UpdateTime(playable);
			if (director.playableGraph.IsValid())
			{
				director.playableGraph.Evaluate();
			}
			else
			{
				director.Evaluate();
			}
		}
		m_SyncTime = false;
	}

	private void SyncSpeed(double speed)
	{
		if (!director.playableGraph.IsValid())
		{
			return;
		}
		int rootPlayableCount = director.playableGraph.GetRootPlayableCount();
		for (int i = 0; i < rootPlayableCount; i++)
		{
			Playable rootPlayable = director.playableGraph.GetRootPlayable(i);
			if (rootPlayable.IsValid())
			{
				rootPlayable.SetSpeed(speed);
			}
		}
	}

	private void SyncStart(PlayableGraph graph, double time)
	{
		if (director.state != PlayState.Playing && graph.IsPlaying() && (director.extrapolationMode != DirectorWrapMode.None || !(time > m_AssetDuration)))
		{
			director.Play();
		}
	}

	private void SyncStop(PlayableGraph graph, double time)
	{
		if (director.state != PlayState.Paused && (!graph.IsPlaying() || (director.extrapolationMode == DirectorWrapMode.None && !(time < m_AssetDuration))) && director.state != PlayState.Paused && ((director.extrapolationMode == DirectorWrapMode.None && time > m_AssetDuration) || !graph.IsPlaying()))
		{
			director.Pause();
		}
	}

	private bool DetectDiscontinuity(Playable playable, FrameData info)
	{
		return Math.Abs(playable.GetTime() - playable.GetPreviousTime() - info.m_DeltaTime * (double)info.m_EffectiveSpeed) > DiscreteTime.tickValue;
	}

	private bool DetectOutOfSync(Playable playable)
	{
		double num = playable.GetTime();
		if (playable.GetTime() >= m_AssetDuration)
		{
			switch (director.extrapolationMode)
			{
			case DirectorWrapMode.None:
				num = m_AssetDuration;
				break;
			case DirectorWrapMode.Hold:
				num = m_AssetDuration;
				break;
			case DirectorWrapMode.Loop:
				num %= m_AssetDuration;
				break;
			}
		}
		if (!Mathf.Approximately((float)num, (float)director.time))
		{
			return true;
		}
		return false;
	}

	private void UpdateTime(Playable playable)
	{
		double num = Math.Max(0.1, director.playableAsset.duration);
		switch (director.extrapolationMode)
		{
		case DirectorWrapMode.Hold:
			director.time = Math.Min(num, Math.Max(0.0, playable.GetTime()));
			break;
		case DirectorWrapMode.Loop:
			director.time = Math.Max(0.0, playable.GetTime() % num);
			break;
		case DirectorWrapMode.None:
			director.time = Math.Min(num, Math.Max(0.0, playable.GetTime()));
			break;
		}
	}
}
