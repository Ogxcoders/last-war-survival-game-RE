using System;
using System.Collections;
using UnityEngine;

namespace Spine.Unity;

public class WaitForSpineAnimation : IEnumerator
{
	[Flags]
	public enum AnimationEventTypes
	{
		Start = 1,
		Interrupt = 2,
		End = 4,
		Dispose = 8,
		Complete = 0x10
	}

	private bool m_WasFired;

	object IEnumerator.Current => null;

	public WaitForSpineAnimation(TrackEntry trackEntry, AnimationEventTypes eventsToWaitFor)
	{
		SafeSubscribe(trackEntry, eventsToWaitFor);
	}

	public WaitForSpineAnimation NowWaitFor(TrackEntry trackEntry, AnimationEventTypes eventsToWaitFor)
	{
		SafeSubscribe(trackEntry, eventsToWaitFor);
		return this;
	}

	bool IEnumerator.MoveNext()
	{
		if (m_WasFired)
		{
			((IEnumerator)this).Reset();
			return false;
		}
		return true;
	}

	void IEnumerator.Reset()
	{
		m_WasFired = false;
	}

	protected void SafeSubscribe(TrackEntry trackEntry, AnimationEventTypes eventsToWaitFor)
	{
		if (trackEntry == null)
		{
			Debug.LogWarning("TrackEntry was null. Coroutine will continue immediately.");
			m_WasFired = true;
			return;
		}
		if ((eventsToWaitFor & AnimationEventTypes.Start) != 0)
		{
			trackEntry.Start += HandleComplete;
		}
		if ((eventsToWaitFor & AnimationEventTypes.Interrupt) != 0)
		{
			trackEntry.Interrupt += HandleComplete;
		}
		if ((eventsToWaitFor & AnimationEventTypes.End) != 0)
		{
			trackEntry.End += HandleComplete;
		}
		if ((eventsToWaitFor & AnimationEventTypes.Dispose) != 0)
		{
			trackEntry.Dispose += HandleComplete;
		}
		if ((eventsToWaitFor & AnimationEventTypes.Complete) != 0)
		{
			trackEntry.Complete += HandleComplete;
		}
	}

	private void HandleComplete(TrackEntry trackEntry)
	{
		m_WasFired = true;
	}
}
