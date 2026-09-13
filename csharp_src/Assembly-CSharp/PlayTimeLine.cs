using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayTimeLine : MonoBehaviour
{
	[SerializeField]
	public TimelineAsset timelineAsset;

	[SerializeField]
	public PlayableDirector playeAble;

	private void Start()
	{
		playeAble.playableAsset = timelineAsset;
		playeAble.time = 5.283332824707031;
		playeAble.Play();
	}

	private void PlayTimelineStopHandle(PlayableDirector director)
	{
	}
}
