using System.Collections;

namespace Spine.Unity;

public class WaitForSpineAnimationComplete : WaitForSpineAnimation, IEnumerator
{
	public WaitForSpineAnimationComplete(TrackEntry trackEntry, bool includeEndEvent = false)
		: base(trackEntry, includeEndEvent ? (AnimationEventTypes.End | AnimationEventTypes.Complete) : AnimationEventTypes.Complete)
	{
	}

	public WaitForSpineAnimationComplete NowWaitFor(TrackEntry trackEntry, bool includeEndEvent = false)
	{
		SafeSubscribe(trackEntry, includeEndEvent ? (AnimationEventTypes.End | AnimationEventTypes.Complete) : AnimationEventTypes.Complete);
		return this;
	}
}
