using System.Collections;

namespace Spine.Unity;

public class WaitForSpineAnimationEnd : WaitForSpineAnimation, IEnumerator
{
	public WaitForSpineAnimationEnd(TrackEntry trackEntry)
		: base(trackEntry, AnimationEventTypes.End)
	{
	}

	public WaitForSpineAnimationEnd NowWaitFor(TrackEntry trackEntry)
	{
		SafeSubscribe(trackEntry, AnimationEventTypes.End);
		return this;
	}
}
