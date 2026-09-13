using UnityEngine;

namespace Spine.Unity.Playables;

public abstract class SpinePlayableHandleBase : MonoBehaviour
{
	public abstract SkeletonData SkeletonData { get; }

	public abstract Skeleton Skeleton { get; }

	public event SpineEventDelegate AnimationEvents;

	public virtual void HandleEvents(ExposedList<Event> eventBuffer)
	{
		if (eventBuffer != null && this.AnimationEvents != null)
		{
			int i = 0;
			for (int count = eventBuffer.Count; i < count; i++)
			{
				this.AnimationEvents(eventBuffer.Items[i]);
			}
		}
	}
}
