using GameFramework;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TimelineScript;

public class TimelineInteractionManager
{
	private static TimelineInteractionManager _inst;

	private PlayableDirector _activeDirector;

	private InteractionBehaviour _activeInteractionBehaviour;

	private bool _isExtOpen;

	public static TimelineInteractionManager Inst
	{
		get
		{
			if (_inst == null)
			{
				_inst = new TimelineInteractionManager();
			}
			return _inst;
		}
	}

	public void SetTimelineExtOpen(bool isOpen)
	{
		_isExtOpen = isOpen;
	}

	internal bool IsTimelineExtOpen()
	{
		return _isExtOpen;
	}

	public bool RecordInteractionTimeline(PlayableDirector director, InteractionBehaviour behaviour)
	{
		if (_activeDirector != null && _activeInteractionBehaviour != null)
		{
			return false;
		}
		_activeDirector = director;
		_activeInteractionBehaviour = behaviour;
		return true;
	}

	internal bool ShowQTE1(float duration)
	{
		bool num = GameEntry.Lua.CallWithReturn<bool, float>("CSharpCallLuaInterface.TimelineInteractionShowQTE1", duration);
		if (!num)
		{
			OnQTE1Done();
		}
		return num;
	}

	internal void CloseQTE1()
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.TimelineInteractionCloseQTE1");
	}

	internal void FinishQTE1()
	{
		CloseQTE1();
		OnQTE1Done();
	}

	public void OnQTE1Done()
	{
		if (!(_activeDirector != null))
		{
			return;
		}
		if (_activeInteractionBehaviour != null)
		{
			PlayableGraph playableGraph = _activeDirector.playableGraph;
			if (!playableGraph.IsValid())
			{
				Log.Error($"TimelineInteractionManager.OnQTE1Done director:{_activeDirector} graph is invalid !");
				_activeDirector = null;
				_activeInteractionBehaviour = null;
				return;
			}
			ETimelineInteractionEndAction timelineInteractionEndAction = _activeInteractionBehaviour.TimelineInteractionEndAction;
			if ((timelineInteractionEndAction & ETimelineInteractionEndAction.Resume) > (ETimelineInteractionEndAction)0)
			{
				ResumeTimeline(_activeDirector);
			}
			if ((timelineInteractionEndAction & ETimelineInteractionEndAction.JumpToTime) > (ETimelineInteractionEndAction)0)
			{
				TryCloseLoop(_activeDirector, _activeInteractionBehaviour.TargetTime);
				Playable rootPlayable = playableGraph.GetRootPlayable(0);
				if (rootPlayable.IsValid())
				{
					rootPlayable.SetTime(_activeInteractionBehaviour.TargetTime);
				}
			}
		}
		_activeDirector = null;
		_activeInteractionBehaviour = null;
	}

	public bool ShowPlot(int plotId)
	{
		bool num = GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.TimelineInteractionShowPlot", plotId);
		if (!num)
		{
			OnPlotDone(plotId);
		}
		return num;
	}

	public void FinishPlot()
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.TimelineInteractionClosePlot");
	}

	public void OnPlotDone(int plotId)
	{
		if (!(_activeDirector != null))
		{
			return;
		}
		if (_activeInteractionBehaviour != null)
		{
			PlayableGraph playableGraph = _activeDirector.playableGraph;
			if (!playableGraph.IsValid())
			{
				Log.Error($"TimelineInteractionManager.OnPlotDone director:{_activeDirector} plot:{plotId} graph is invalid !");
				_activeDirector = null;
				_activeInteractionBehaviour = null;
				return;
			}
			ETimelineInteractionEndAction timelineInteractionEndAction = _activeInteractionBehaviour.TimelineInteractionEndAction;
			if ((timelineInteractionEndAction & ETimelineInteractionEndAction.Resume) > (ETimelineInteractionEndAction)0)
			{
				ResumeTimeline(_activeDirector);
			}
			if ((timelineInteractionEndAction & ETimelineInteractionEndAction.JumpToTime) > (ETimelineInteractionEndAction)0)
			{
				TryCloseLoop(_activeDirector, _activeInteractionBehaviour.TargetTime);
				Playable rootPlayable = playableGraph.GetRootPlayable(0);
				if (rootPlayable.IsValid())
				{
					rootPlayable.SetTime(_activeInteractionBehaviour.TargetTime);
				}
			}
		}
		_activeDirector = null;
		_activeInteractionBehaviour = null;
	}

	private void TryCloseLoop(PlayableDirector director, float time)
	{
		TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
		if (timelineAsset == null)
		{
			return;
		}
		foreach (TrackAsset outputTrack in timelineAsset.GetOutputTracks())
		{
			if (outputTrack is LoopControlTrack loopControlTrack)
			{
				loopControlTrack.TryCloseLoop(time);
			}
		}
	}

	public void PauseTimeline(PlayableDirector director, float targetSpeed = 0f)
	{
		if (!(director != null))
		{
			return;
		}
		PlayableGraph playableGraph = director.playableGraph;
		if (playableGraph.IsValid())
		{
			Playable rootPlayable = playableGraph.GetRootPlayable(0);
			if (rootPlayable.IsValid())
			{
				rootPlayable.SetSpeed(targetSpeed);
			}
		}
	}

	private void ResumeTimeline(PlayableDirector director)
	{
		if (director == null)
		{
			return;
		}
		PlayableGraph playableGraph = director.playableGraph;
		if (playableGraph.IsValid())
		{
			Playable rootPlayable = playableGraph.GetRootPlayable(0);
			if (rootPlayable.IsValid())
			{
				rootPlayable.SetSpeed(1.0);
			}
		}
	}

	public void Clear()
	{
		_activeDirector = null;
		_activeInteractionBehaviour = null;
	}
}
