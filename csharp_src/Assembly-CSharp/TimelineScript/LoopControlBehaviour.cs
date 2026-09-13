using System;
using UnityEngine.Playables;

namespace TimelineScript;

[Serializable]
public class LoopControlBehaviour : PlayableBehaviour
{
	public double startTime;

	public double endTime;

	public bool isInit;
}
