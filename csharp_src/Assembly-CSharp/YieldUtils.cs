using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldUtils
{
	private class CoroutineUtil : MonoBehaviour
	{
		private static CoroutineUtil _instance;

		private static CoroutineUtil Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}
				_instance = new GameObject("YieldUtil", typeof(CoroutineUtil)).GetComponent<CoroutineUtil>();
				GameObject obj = _instance.gameObject;
				obj.hideFlags = HideFlags.HideAndDontSave;
				UnityEngine.Object.DontDestroyOnLoad(obj);
				return _instance;
			}
		}

		public static Coroutine NStartCoroutine(IEnumerator cor)
		{
			return Instance.StartCoroutine(cor);
		}

		public static void NStopCoroutine(Coroutine cor)
		{
			Instance.StopCoroutine(cor);
		}

		private static IEnumerator DelayAction(float delay, Action action)
		{
			yield return WaitForSeconds(delay);
			action?.Invoke();
		}

		public static Coroutine DoDelayAction(float delay, Action action)
		{
			return Instance.StartCoroutine(DelayAction(delay, action));
		}

		public static void StopDelayAction(Coroutine coroutine)
		{
			Instance.StopCoroutine(coroutine);
		}

		public static Coroutine DoDelayByContext(MonoBehaviour context, float delay, Action action)
		{
			return context.StartCoroutine(DelayAction(delay, action));
		}

		public static Coroutine DoEndOfFrame(MonoBehaviour context, Action action)
		{
			return context.StartCoroutine(DelayEndOfFrame(action));
		}

		private static IEnumerator DelayEndOfFrame(Action action)
		{
			yield return EndOfFrame;
			action?.Invoke();
		}

		public static void DoDelayRealTimeAction(float dTime, Action callback)
		{
			Instance.StartCoroutine(DelayRealTimeAction(dTime, callback));
		}

		public static void DoRealDelayByContext(MonoBehaviour context, float dTime, Action callback)
		{
			context.StartCoroutine(DelayRealTimeAction(dTime, callback));
		}

		public static IEnumerator DelayRealTimeAction(float dTime, Action callback)
		{
			yield return WaitForSecondsRealtime(dTime);
			callback();
		}
	}

	private static readonly WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

	private static readonly WaitForFixedUpdate FixedUpdate = new WaitForFixedUpdate();

	private static readonly Dictionary<float, WaitForSeconds> _waitSecondsCache = new Dictionary<float, WaitForSeconds>(256, new FloatComparer());

	private static readonly Dictionary<float, WaitForSecondsRealtime> _waitSecondsRealTimeCache = new Dictionary<float, WaitForSecondsRealtime>(256, new FloatComparer());

	public static WaitForFixedUpdate WaitForFixedUpdate()
	{
		return FixedUpdate;
	}

	public static WaitForEndOfFrame WaitForEndOfFrame()
	{
		return EndOfFrame;
	}

	public static WaitForSeconds WaitForSeconds(float seconds)
	{
		if (!_waitSecondsCache.TryGetValue(seconds, out var value))
		{
			_ = 5f;
			_waitSecondsCache.Add(seconds, value = new WaitForSeconds(seconds));
		}
		return value;
	}

	public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds)
	{
		if (!_waitSecondsRealTimeCache.TryGetValue(seconds, out var value))
		{
			_ = 5f;
			_waitSecondsRealTimeCache.Add(seconds, value = new WaitForSecondsRealtime(seconds));
		}
		return value;
	}

	public static Coroutine DelayAction(MonoBehaviour context, Action action, float delay)
	{
		if (action != null)
		{
			return CoroutineUtil.DoDelayByContext(context, delay, action);
		}
		return null;
	}

	public static void StopDelayAction(MonoBehaviour context, Coroutine coroutine)
	{
		if (coroutine != null)
		{
			context.StopCoroutine(coroutine);
		}
	}

	public static Coroutine DoEndOfFrame(MonoBehaviour context, Action action)
	{
		if (action != null)
		{
			return CoroutineUtil.DoEndOfFrame(context, action);
		}
		return null;
	}

	public static Coroutine DelayActionWithOutContext(Action action, float delay)
	{
		return CoroutineUtil.DoDelayAction(delay, action);
	}

	public static void StopDelayActionWithOutContext(Coroutine coroutine)
	{
		CoroutineUtil.StopDelayAction(coroutine);
	}

	public static Coroutine StartCoroutine(IEnumerator cor)
	{
		return CoroutineUtil.NStartCoroutine(cor);
	}

	public static void StopCoroutine(Coroutine cor)
	{
		CoroutineUtil.NStopCoroutine(cor);
	}
}
