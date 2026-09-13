using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineScript;

[TrackColor(0.6f, 0.5f, 0.5f)]
[TrackClipType(typeof(LoopControlClip))]
public class LoopControlTrack : TrackAsset
{
	private ScriptPlayable<LoopControlMixerBehaviour> _scriptPlayable;

	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		ScriptPlayable<LoopControlMixerBehaviour> scriptPlayable = (_scriptPlayable = ScriptPlayable<LoopControlMixerBehaviour>.Create(graph, inputCount));
		scriptPlayable.GetBehaviour();
		foreach (TimelineClip clip in GetClips())
		{
			LoopControlClip loopControlClip = (LoopControlClip)clip.asset;
			loopControlClip.template.startTime = clip.start;
			loopControlClip.template.endTime = clip.end;
			clip.displayName = $"↻ {clip.start:f2}->{clip.end:f2}";
			if (!loopControlClip.template.isInit)
			{
				clip.duration = 0.5;
				loopControlClip.template.isInit = true;
			}
		}
		return scriptPlayable;
	}

	public void TryCloseLoop(float time)
	{
		_scriptPlayable.GetBehaviour()?.CheckToClose(time);
	}
}
