using System;

namespace UnityEngine.UIElements.Experimental;

public sealed class ValueAnimation<T> : IValueAnimationUpdate, IValueAnimation
{
	private const int k_DefaultDurationMs = 400;

	private const int k_DefaultMaxPoolSize = 100;

	private long m_StartTimeMs;

	private int m_DurationMs;

	private static ObjectPool<ValueAnimation<T>> sObjectPool = new ObjectPool<ValueAnimation<T>>();

	private T _from;

	private bool fromValueSet = false;

	public int durationMs
	{
		get
		{
			return m_DurationMs;
		}
		set
		{
			if (value < 1)
			{
				value = 1;
			}
			m_DurationMs = value;
		}
	}

	public Func<float, float> easingCurve { get; set; }

	public bool isRunning { get; private set; }

	public Action onAnimationCompleted { get; set; }

	public bool autoRecycle { get; set; }

	private bool recycled { get; set; }

	private VisualElement owner { get; set; }

	public Action<VisualElement, T> valueUpdated { get; set; }

	public Func<VisualElement, T> initialValue { get; set; }

	public Func<T, T, float, T> interpolator { get; set; }

	public T from
	{
		get
		{
			if (!fromValueSet && initialValue != null)
			{
				from = initialValue(owner);
			}
			return _from;
		}
		set
		{
			fromValueSet = true;
			_from = value;
		}
	}

	public T to { get; set; }

	public ValueAnimation()
	{
		SetDefaultValues();
	}

	public void Start()
	{
		CheckNotRecycled();
		if (owner != null)
		{
			m_StartTimeMs = Panel.TimeSinceStartupMs();
			Register();
			isRunning = true;
		}
	}

	public void Stop()
	{
		CheckNotRecycled();
		if (isRunning)
		{
			Unregister();
			isRunning = false;
			onAnimationCompleted?.Invoke();
			if (autoRecycle && !recycled)
			{
				Recycle();
			}
		}
	}

	public void Recycle()
	{
		CheckNotRecycled();
		if (isRunning)
		{
			if (autoRecycle)
			{
				Stop();
				return;
			}
			Stop();
		}
		SetDefaultValues();
		recycled = true;
		sObjectPool.Release(this);
	}

	void IValueAnimationUpdate.Tick(long currentTimeMs)
	{
		CheckNotRecycled();
		long num = currentTimeMs - m_StartTimeMs;
		float num2 = (float)num / (float)durationMs;
		bool flag = false;
		if (num2 >= 1f)
		{
			num2 = 1f;
			flag = true;
		}
		num2 = easingCurve?.Invoke(num2) ?? num2;
		if (interpolator != null)
		{
			T arg = interpolator(from, to, num2);
			valueUpdated?.Invoke(owner, arg);
		}
		if (flag)
		{
			Stop();
		}
	}

	private void SetDefaultValues()
	{
		m_DurationMs = 400;
		autoRecycle = true;
		owner = null;
		m_StartTimeMs = 0L;
		onAnimationCompleted = null;
		valueUpdated = null;
		initialValue = null;
		interpolator = null;
		to = default(T);
		from = default(T);
		fromValueSet = false;
		easingCurve = Easing.OutQuad;
	}

	private void Unregister()
	{
		if (owner != null)
		{
			owner.UnregisterAnimation(this);
		}
	}

	private void Register()
	{
		if (owner != null)
		{
			owner.RegisterAnimation(this);
		}
	}

	internal void SetOwner(VisualElement e)
	{
		if (isRunning)
		{
			Unregister();
		}
		owner = e;
		if (isRunning)
		{
			Register();
		}
	}

	private void CheckNotRecycled()
	{
		if (recycled)
		{
			throw new InvalidOperationException("Animation object has been recycled. Use KeepAlive() to keep a reference to an animation after it has been stopped.");
		}
	}

	public static ValueAnimation<T> Create(VisualElement e, Func<T, T, float, T> interpolator)
	{
		ValueAnimation<T> valueAnimation = sObjectPool.Get();
		valueAnimation.recycled = false;
		valueAnimation.SetOwner(e);
		valueAnimation.interpolator = interpolator;
		return valueAnimation;
	}

	public ValueAnimation<T> Ease(Func<float, float> easing)
	{
		easingCurve = easing;
		return this;
	}

	public ValueAnimation<T> OnCompleted(Action callback)
	{
		onAnimationCompleted = callback;
		return this;
	}

	public ValueAnimation<T> KeepAlive()
	{
		autoRecycle = false;
		return this;
	}
}
