using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
[TrackClipType(typeof(CinemachineShot))]
[TrackBindingType(typeof(CinemachineBrain))]
[TrackColor(0.53f, 0f, 0.08f)]
public class CinemachineTrack : TrackAsset
{
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		foreach (TimelineClip clip in GetClips())
		{
			CinemachineVirtualCameraBase cinemachineVirtualCameraBase = ((CinemachineShot)clip.asset).VirtualCamera.Resolve(graph.GetResolver());
			if (cinemachineVirtualCameraBase != null)
			{
				clip.displayName = cinemachineVirtualCameraBase.Name;
			}
		}
		ScriptPlayable<CinemachineMixer> scriptPlayable = ScriptPlayable<CinemachineMixer>.Create(graph);
		scriptPlayable.SetInputCount(inputCount);
		return scriptPlayable;
	}
}
