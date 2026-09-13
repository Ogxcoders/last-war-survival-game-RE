using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GuideTimelineMarker : MonoBehaviour, INotificationReceiver
{
	private enum ShowMarkType
	{
		Zero = 0,
		One = 1,
		End = 999
	}

	public Func<bool> IsContinue;

	private double _markerTime;

	private void Awake()
	{
		_markerTime = 0.0;
	}

	public void OnNotify(Playable origin, INotification notification, object context)
	{
		SignalEmitter signalEmitter = (SignalEmitter)notification;
		string text = signalEmitter.asset.name;
		if (text.Equals("GuideMarkerEnd"))
		{
			GameEntry.Event?.Fire(EventId.GuideTimelineMarker, 999);
		}
		else if (text.Contains("GuideMarker"))
		{
			int num = text.Replace("GuideMarker", "").ToInt();
			GameEntry.Event?.Fire(EventId.GuideTimelineMarker, num);
		}
		if (IsContinue != null && IsContinue())
		{
			return;
		}
		PlayableDirector playableDirector = (PlayableDirector)origin.GetGraph().GetResolver();
		if (text.Equals("GuideRewindSignal"))
		{
			if (signalEmitter.parent.timelineAsset != null)
			{
				playableDirector.Pause();
				playableDirector.time = _markerTime;
				playableDirector.Play();
			}
		}
		else if (text.Equals("GuideRewindMarker"))
		{
			_markerTime = playableDirector.time;
		}
	}
}
