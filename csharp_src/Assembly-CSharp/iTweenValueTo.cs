using System;
using System.Collections;
using UnityEngine.Events;

public class iTweenValueTo : iTweenEditor
{
	[Serializable]
	public class OnStart : UnityEvent
	{
	}

	[Serializable]
	public class OnUpdate : UnityEvent<float>
	{
	}

	[Serializable]
	public class OnComplete : UnityEvent
	{
	}

	public float valueFrom;

	public float valueTo = 1f;

	public OnStart onStart;

	public OnUpdate onUpdate;

	public OnComplete onComplete;

	private void Awake()
	{
		if (autoPlay)
		{
			iTweenPlay();
		}
	}

	public override void iTweenPlay()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", valueFrom);
		hashtable.Add("to", valueTo);
		hashtable.Add("time", tweenTime);
		hashtable.Add("delay", waitTime);
		hashtable.Add("looptype", loopType);
		hashtable.Add("easetype", easeType);
		hashtable.Add("onstart", (Action<object>)delegate
		{
			if (onStart != null)
			{
				onStart.Invoke();
			}
			if (onUpdate != null)
			{
				onUpdate.Invoke(valueFrom);
			}
		});
		hashtable.Add("onupdate", (Action<object>)delegate(object newVal)
		{
			if (onUpdate != null)
			{
				onUpdate.Invoke((float)newVal);
			}
		});
		hashtable.Add("oncomplete", (Action<object>)delegate
		{
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		});
		hashtable.Add("ignoretimescale", ignoreTimescale);
		iTween.ValueTo(base.gameObject, hashtable);
	}
}
