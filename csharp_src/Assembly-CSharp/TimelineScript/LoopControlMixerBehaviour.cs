using UnityEngine.Playables;

namespace TimelineScript;

public class LoopControlMixerBehaviour : PlayableBehaviour
{
	private double _startTime;

	private double _endTime;

	private bool _isLooping;

	private PlayableDirector _director;

	private float _validTime = -1f;

	public override void OnGraphStart(Playable playable)
	{
		_isLooping = false;
		_validTime = -1f;
		_director = playable.GetGraph().GetResolver() as PlayableDirector;
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		int inputCount = playable.GetInputCount();
		if (TimelineInteractionManager.Inst.IsTimelineExtOpen() && _isLooping && _director.time >= _endTime && (_validTime < 0f || _endTime > (double)_validTime))
		{
			_director.time = _startTime;
			return;
		}
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			LoopControlBehaviour behaviour = ((ScriptPlayable<LoopControlBehaviour>)playable.GetInput(i)).GetBehaviour();
			if (inputWeight > 0f)
			{
				_startTime = behaviour.startTime;
				_endTime = behaviour.endTime;
				_isLooping = true;
				break;
			}
		}
	}

	public override void OnGraphStop(Playable playable)
	{
		_isLooping = false;
	}

	public void CheckToClose(float time)
	{
		if (!(_validTime >= time))
		{
			_validTime = time;
		}
	}
}
