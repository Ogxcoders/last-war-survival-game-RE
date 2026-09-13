using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

public class MMProgressBar : MonoBehaviour
{
	public enum FillModes
	{
		LocalScale,
		FillAmount,
		Width,
		Height
	}

	public enum BarDirections
	{
		LeftToRight,
		RightToLeft,
		UpToDown,
		DownToUp
	}

	public enum TimeScales
	{
		UnscaledTime,
		Time
	}

	[Header("General Settings")]
	public float StartValue;

	public float EndValue = 1f;

	public BarDirections BarDirection;

	public FillModes FillMode;

	public TimeScales TimeScale;

	[Header("Foreground Bar Settings")]
	public bool LerpForegroundBar = true;

	public float LerpForegroundBarSpeed = 15f;

	[Header("Delayed Bar Settings")]
	public float Delay = 1f;

	public bool LerpDelayedBar = true;

	public float LerpDelayedBarSpeed = 15f;

	[Header("Bindings")]
	public string PlayerID;

	public Transform DelayedBar;

	public Transform ForegroundBar;

	[Header("Bump")]
	public bool BumpScaleOnChange = true;

	public bool BumpOnIncrease;

	public float BumpDuration = 0.2f;

	public bool ChangeColorWhenBumping = true;

	public Color BumpColor = Color.white;

	public AnimationCurve BumpAnimationCurve = new AnimationCurve(new Keyframe(1f, 1f), new Keyframe(0.3f, 1.05f), new Keyframe(1f, 1f));

	public AnimationCurve BumpColorAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.3f, 1f), new Keyframe(1f, 0f));

	[Header("Realtime")]
	public bool AutoUpdating;

	[Range(0f, 1f)]
	public float BarProgress;

	protected float _targetFill;

	protected Vector3 _targetLocalScale = Vector3.one;

	protected float _newPercent;

	protected float _lastPercent;

	protected float _lastUpdateTimestamp;

	protected bool _bump;

	protected Color _initialColor;

	protected Vector3 _initialScale;

	protected Vector3 _newScale;

	protected Image _foregroundImage;

	protected Image _delayedImage;

	protected bool _initialized;

	protected Vector2 _initialFrontBarSize;

	public bool Bumping { get; protected set; }

	protected virtual void Start()
	{
		_initialScale = base.transform.localScale;
		if (ForegroundBar != null)
		{
			_foregroundImage = ForegroundBar.GetComponent<Image>();
			_initialFrontBarSize = _foregroundImage.rectTransform.sizeDelta;
		}
		if (DelayedBar != null)
		{
			_delayedImage = DelayedBar.GetComponent<Image>();
		}
		_initialized = true;
	}

	protected virtual void Update()
	{
		AutoUpdate();
		UpdateFrontBar();
		UpdateDelayedBar();
	}

	protected virtual void AutoUpdate()
	{
		if (AutoUpdating)
		{
			_newPercent = Remap(BarProgress, 0f, 1f, StartValue, EndValue);
			_targetFill = _newPercent;
			_lastUpdateTimestamp = ((TimeScale == TimeScales.Time) ? Time.time : Time.unscaledTime);
		}
	}

	protected virtual void UpdateFrontBar()
	{
		float num = ((TimeScale == TimeScales.Time) ? Time.deltaTime : Time.unscaledTime);
		if (!(ForegroundBar != null))
		{
			return;
		}
		switch (FillMode)
		{
		case FillModes.LocalScale:
			_targetLocalScale = Vector3.one;
			switch (BarDirection)
			{
			case BarDirections.LeftToRight:
				_targetLocalScale.x = _targetFill;
				break;
			case BarDirections.RightToLeft:
				_targetLocalScale.x = 1f - _targetFill;
				break;
			case BarDirections.DownToUp:
				_targetLocalScale.y = _targetFill;
				break;
			case BarDirections.UpToDown:
				_targetLocalScale.y = 1f - _targetFill;
				break;
			}
			if (LerpForegroundBar)
			{
				_newScale = Vector3.Lerp(ForegroundBar.localScale, _targetLocalScale, num * LerpForegroundBarSpeed);
			}
			else
			{
				_newScale = _targetLocalScale;
			}
			ForegroundBar.localScale = _newScale;
			break;
		case FillModes.Width:
			if (!(_foregroundImage == null))
			{
				float b2 = Remap(_targetFill, 0f, 1f, 0f, _initialFrontBarSize.x);
				b2 = Mathf.Lerp(_foregroundImage.rectTransform.sizeDelta.x, b2, num * LerpForegroundBarSpeed);
				_foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, b2);
			}
			break;
		case FillModes.Height:
			if (!(_foregroundImage == null))
			{
				float b = Remap(_targetFill, 0f, 1f, 0f, _initialFrontBarSize.y);
				b = Mathf.Lerp(_foregroundImage.rectTransform.sizeDelta.x, b, num * LerpForegroundBarSpeed);
				_foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, b);
			}
			break;
		case FillModes.FillAmount:
			if (!(_foregroundImage == null))
			{
				if (LerpForegroundBar)
				{
					_foregroundImage.fillAmount = Mathf.Lerp(_foregroundImage.fillAmount, _targetFill, num * LerpForegroundBarSpeed);
				}
				else
				{
					_foregroundImage.fillAmount = _targetFill;
				}
			}
			break;
		}
	}

	protected virtual void UpdateDelayedBar()
	{
		float num = ((TimeScale == TimeScales.Time) ? Time.deltaTime : Time.unscaledDeltaTime);
		float num2 = ((TimeScale == TimeScales.Time) ? Time.time : Time.unscaledTime);
		if (!(DelayedBar != null) || !(num2 - _lastUpdateTimestamp > Delay))
		{
			return;
		}
		if (FillMode == FillModes.LocalScale)
		{
			_targetLocalScale = Vector3.one;
			switch (BarDirection)
			{
			case BarDirections.LeftToRight:
				_targetLocalScale.x = _targetFill;
				break;
			case BarDirections.RightToLeft:
				_targetLocalScale.x = 1f - _targetFill;
				break;
			case BarDirections.DownToUp:
				_targetLocalScale.y = _targetFill;
				break;
			case BarDirections.UpToDown:
				_targetLocalScale.y = 1f - _targetFill;
				break;
			}
			if (LerpDelayedBar)
			{
				_newScale = Vector3.Lerp(DelayedBar.localScale, _targetLocalScale, num * LerpDelayedBarSpeed);
			}
			else
			{
				_newScale = _targetLocalScale;
			}
			DelayedBar.localScale = _newScale;
		}
		if (FillMode == FillModes.FillAmount && _delayedImage != null)
		{
			if (LerpDelayedBar)
			{
				_delayedImage.fillAmount = Mathf.Lerp(_delayedImage.fillAmount, _targetFill, num * LerpDelayedBarSpeed);
			}
			else
			{
				_delayedImage.fillAmount = _targetFill;
			}
		}
	}

	public virtual void UpdateBar(float currentValue, float minValue, float maxValue)
	{
		_newPercent = Remap(currentValue, minValue, maxValue, StartValue, EndValue);
		if (_newPercent != BarProgress && !Bumping)
		{
			Bump();
		}
		BarProgress = _newPercent;
		_targetFill = _newPercent;
		_lastUpdateTimestamp = ((TimeScale == TimeScales.Time) ? Time.time : Time.unscaledTime);
		_lastPercent = _newPercent;
	}

	public virtual void Bump()
	{
		if (BumpScaleOnChange && _initialized && (BumpOnIncrease || !(_lastPercent < _newPercent)) && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(BumpCoroutine());
		}
	}

	protected virtual IEnumerator BumpCoroutine()
	{
		float journey = 0f;
		float currentDeltaTime = ((TimeScale == TimeScales.Time) ? Time.deltaTime : Time.unscaledDeltaTime);
		Bumping = true;
		if (_foregroundImage != null)
		{
			_initialColor = _foregroundImage.color;
		}
		while (journey <= BumpDuration)
		{
			journey += currentDeltaTime;
			float time = Mathf.Clamp01(journey / BumpDuration);
			float num = BumpAnimationCurve.Evaluate(time);
			float t = BumpColorAnimationCurve.Evaluate(time);
			base.transform.localScale = num * _initialScale;
			if (ChangeColorWhenBumping && _foregroundImage != null)
			{
				_foregroundImage.color = Color.Lerp(_initialColor, BumpColor, t);
			}
			yield return null;
		}
		_foregroundImage.color = _initialColor;
		Bumping = false;
		yield return null;
	}

	protected virtual float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
