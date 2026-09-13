using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class iTweenPositionTo : iTweenEditor
{
	[Serializable]
	public class OnStart : UnityEvent
	{
	}

	[Serializable]
	public class OnComplete : UnityEvent
	{
	}

	public Vector3 valueFrom = Vector3.zero;

	public Vector3 valueTo = Vector3.one;

	public OnStart onStart;

	public OnComplete onComplete;

	private Vector3 _transition = Vector3.zero;

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
		hashtable.Add("from", 0f);
		hashtable.Add("to", 1f);
		hashtable.Add("time", tweenTime);
		hashtable.Add("delay", waitTime);
		hashtable.Add("looptype", loopType);
		hashtable.Add("easetype", easeType);
		hashtable.Add("onstart", (Action<object>)delegate
		{
			_onUpdate(0f);
			if (onStart != null)
			{
				onStart.Invoke();
			}
		});
		hashtable.Add("onupdate", (Action<object>)delegate(object newVal)
		{
			_onUpdate((float)newVal);
		});
		hashtable.Add("oncomplete", (Action<object>)delegate
		{
			if (onComplete != null)
			{
				onComplete.Invoke();
			}
		});
		hashtable.Add("ignoretimescale", ignoreTimescale);
		_transition = valueTo - valueFrom;
		iTween.ValueTo(base.gameObject, hashtable);
	}

	private void _onUpdate(float value)
	{
		base.transform.localPosition = valueFrom + _transition * value;
	}
}
