using System;
using UnityEngine;
using UnityEngine.Playables;

namespace TimelineScript;

[Serializable]
public class InteractionBehaviour : PlayableBehaviour
{
	[NonSerialized]
	public float Duration;

	public ETimelineInteractionShowType ShowType;

	public int PlotId;

	public bool HasToPause;

	public bool Once = true;

	public ETimelineInteractionEndAction TimelineInteractionEndAction;

	public float TargetTime;

	public float FloatValue1;

	private int _plotId;

	public bool ClipPlayed;

	private bool _pauseScheduled;

	private PlayableDirector _director;

	private int _tmpPlotId = -1;

	public override void OnPlayableCreate(Playable playable)
	{
		_director = playable.GetGraph().GetResolver() as PlayableDirector;
		_plotId = PlotId;
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!TimelineInteractionManager.Inst.IsTimelineExtOpen() || ClipPlayed || !(info.weight > 0f))
		{
			return;
		}
		if (Application.isPlaying)
		{
			switch (ShowType)
			{
			case ETimelineInteractionShowType.Plot:
				if (_tmpPlotId != _plotId)
				{
					_tmpPlotId = _plotId;
					if (TimelineInteractionManager.Inst.ShowPlot(_plotId))
					{
						TimelineInteractionManager.Inst.RecordInteractionTimeline(_director, this);
					}
					else
					{
						HasToPause = false;
					}
				}
				else
				{
					_tmpPlotId = -1;
					HasToPause = false;
				}
				if (HasToPause)
				{
					_pauseScheduled = true;
				}
				break;
			case ETimelineInteractionShowType.QTE1:
			{
				float duration = Duration;
				if (FloatValue1 > 0f)
				{
					duration = Duration / FloatValue1;
				}
				bool num = TimelineInteractionManager.Inst.ShowQTE1(duration);
				_pauseScheduled = false;
				if (num)
				{
					TimelineInteractionManager.Inst.RecordInteractionTimeline(_director, this);
					TimelineInteractionManager.Inst.PauseTimeline(_director, FloatValue1);
				}
				break;
			}
			}
		}
		ClipPlayed = true;
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (!Application.isPlaying || !TimelineInteractionManager.Inst.IsTimelineExtOpen() || info.evaluationType != FrameData.EvaluationType.Playback)
		{
			return;
		}
		if (_pauseScheduled)
		{
			_pauseScheduled = false;
			float targetSpeed = 0f;
			ETimelineInteractionShowType showType = ShowType;
			if (showType == ETimelineInteractionShowType.Plot)
			{
				targetSpeed = 0f;
			}
			TimelineInteractionManager.Inst.PauseTimeline(_director, targetSpeed);
		}
		else
		{
			ETimelineInteractionShowType showType = ShowType;
			if (showType != ETimelineInteractionShowType.Plot && showType == ETimelineInteractionShowType.QTE1)
			{
				TimelineInteractionManager.Inst.FinishQTE1();
			}
		}
		if (!Once)
		{
			ClipPlayed = false;
			_tmpPlotId = -1;
		}
	}

	public override void OnPlayableDestroy(Playable playable)
	{
		if (Application.isPlaying && TimelineInteractionManager.Inst.IsTimelineExtOpen())
		{
			switch (ShowType)
			{
			case ETimelineInteractionShowType.Plot:
				TimelineInteractionManager.Inst.FinishPlot();
				break;
			case ETimelineInteractionShowType.QTE1:
				TimelineInteractionManager.Inst.CloseQTE1();
				break;
			}
			TimelineInteractionManager.Inst.Clear();
		}
	}
}
