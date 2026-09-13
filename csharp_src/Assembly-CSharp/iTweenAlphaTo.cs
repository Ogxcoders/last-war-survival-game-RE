using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class iTweenAlphaTo : iTweenEditor
{
	[Serializable]
	public class OnStart : UnityEvent
	{
	}

	[Serializable]
	public class OnComplete : UnityEvent
	{
	}

	public float valueFrom;

	public float valueTo = 1f;

	public OnStart onStart;

	public OnComplete onComplete;

	private SpriteRenderer _spriteRenderer;

	private Image _uiImage;

	private RawImage _uiRawImage;

	private CanvasGroup _uiCanvasGroup;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_uiImage = GetComponent<Image>();
		_uiRawImage = GetComponent<RawImage>();
		_uiCanvasGroup = GetComponent<CanvasGroup>();
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
			_onUpdate(valueFrom);
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
		iTween.ValueTo(base.gameObject, hashtable);
	}

	public void Resume()
	{
		if (GetComponent<iTween>() != null)
		{
			iTween.Resume(base.gameObject);
		}
	}

	public void DestoryiTween()
	{
		iTween component = base.gameObject.GetComponent<iTween>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	private void _onUpdate(float value)
	{
		if (_spriteRenderer != null)
		{
			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, value);
		}
		if (_uiImage != null)
		{
			_uiImage.color = new Color(_uiImage.color.r, _uiImage.color.g, _uiImage.color.b, value);
		}
		if (_uiRawImage != null)
		{
			_uiRawImage.color = new Color(_uiRawImage.color.r, _uiRawImage.color.g, _uiRawImage.color.b, value);
		}
		if (_uiCanvasGroup != null)
		{
			_uiCanvasGroup.alpha = value;
		}
	}
}
