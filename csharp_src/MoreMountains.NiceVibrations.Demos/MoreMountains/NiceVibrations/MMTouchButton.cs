using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

[RequireComponent(typeof(Rect))]
[RequireComponent(typeof(CanvasGroup))]
public class MMTouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, ISubmitHandler
{
	public enum ButtonStates
	{
		Off,
		ButtonDown,
		ButtonPressed,
		ButtonUp,
		Disabled
	}

	[Header("Binding")]
	public UnityEvent ButtonPressedFirstTime;

	public UnityEvent ButtonReleased;

	public UnityEvent ButtonPressed;

	[Header("Sprite Swap")]
	public Sprite DisabledSprite;

	public Sprite PressedSprite;

	public Sprite HighlightedSprite;

	[Header("Color Changes")]
	public bool PressedChangeColor;

	public Color PressedColor = Color.white;

	public bool LerpColor = true;

	public float LerpColorDuration = 0.2f;

	public AnimationCurve LerpColorCurve;

	[Header("Opacity")]
	public float PressedOpacity = 1f;

	public float IdleOpacity = 1f;

	public float DisabledOpacity = 1f;

	[Header("Delays")]
	public float PressedFirstTimeDelay;

	public float ReleasedDelay;

	[Header("Buffer")]
	public float BufferDuration;

	[Header("Animation")]
	public Animator Animator;

	public string IdleAnimationParameterName = "Idle";

	public string DisabledAnimationParameterName = "Disabled";

	public string PressedAnimationParameterName = "Pressed";

	[Header("Mouse Mode")]
	public bool MouseMode;

	protected bool _zonePressed;

	protected CanvasGroup _canvasGroup;

	protected float _initialOpacity;

	protected Animator _animator;

	protected Image _image;

	protected Sprite _initialSprite;

	protected Color _initialColor;

	protected float _lastClickTimestamp;

	protected Selectable _selectable;

	protected float _lastStateChangeAt = -50f;

	protected Color _imageColor;

	protected Color _fromColor;

	protected Color _toColor;

	public bool ReturnToInitialSpriteAutomatically { get; set; }

	public ButtonStates CurrentState { get; protected set; }

	protected virtual void Awake()
	{
		Initialization();
	}

	protected virtual void Initialization()
	{
		ReturnToInitialSpriteAutomatically = true;
		_selectable = GetComponent<Selectable>();
		_image = GetComponent<Image>();
		if (_image != null)
		{
			_initialColor = _image.color;
			_initialSprite = _image.sprite;
		}
		_animator = GetComponent<Animator>();
		if (Animator != null)
		{
			_animator = Animator;
		}
		_canvasGroup = GetComponent<CanvasGroup>();
		if (_canvasGroup != null)
		{
			_initialOpacity = IdleOpacity;
			_canvasGroup.alpha = _initialOpacity;
			_initialOpacity = _canvasGroup.alpha;
		}
		ResetButton();
	}

	protected virtual void Update()
	{
		switch (CurrentState)
		{
		case ButtonStates.Off:
			SetOpacity(IdleOpacity);
			if (_image != null && ReturnToInitialSpriteAutomatically)
			{
				_image.sprite = _initialSprite;
			}
			if (_selectable != null)
			{
				_selectable.interactable = true;
				if (EventSystem.current.currentSelectedGameObject == base.gameObject && HighlightedSprite != null)
				{
					_image.sprite = HighlightedSprite;
				}
			}
			break;
		case ButtonStates.Disabled:
			SetOpacity(DisabledOpacity);
			if (_image != null && DisabledSprite != null)
			{
				_image.sprite = DisabledSprite;
			}
			if (_selectable != null)
			{
				_selectable.interactable = false;
			}
			break;
		case ButtonStates.ButtonPressed:
			SetOpacity(PressedOpacity);
			OnPointerPressed();
			if (_image != null)
			{
				if (PressedSprite != null)
				{
					_image.sprite = PressedSprite;
				}
				if (PressedChangeColor)
				{
					_image.color = PressedColor;
				}
			}
			break;
		}
		if (_image != null && PressedChangeColor && Time.time - _lastStateChangeAt < LerpColorDuration)
		{
			float t = LerpColorCurve.Evaluate(Remap(Time.time - _lastStateChangeAt, 0f, LerpColorDuration, 0f, 1f));
			_image.color = Color.Lerp(_fromColor, _toColor, t);
		}
		UpdateAnimatorStates();
	}

	protected virtual void LateUpdate()
	{
		if (CurrentState == ButtonStates.ButtonUp)
		{
			_lastStateChangeAt = Time.time;
			_fromColor = PressedColor;
			_toColor = _initialColor;
			CurrentState = ButtonStates.Off;
		}
		if (CurrentState == ButtonStates.ButtonDown)
		{
			_lastStateChangeAt = Time.time;
			_fromColor = _initialColor;
			_toColor = PressedColor;
			CurrentState = ButtonStates.ButtonPressed;
		}
	}

	public virtual void OnPointerDown(PointerEventData data)
	{
		if (!(Time.time - _lastClickTimestamp < BufferDuration) && CurrentState == ButtonStates.Off)
		{
			CurrentState = ButtonStates.ButtonDown;
			_lastClickTimestamp = Time.time;
			if (Time.timeScale != 0f && PressedFirstTimeDelay > 0f)
			{
				Invoke("InvokePressedFirstTime", PressedFirstTimeDelay);
			}
			else
			{
				ButtonPressedFirstTime.Invoke();
			}
		}
	}

	protected virtual void InvokePressedFirstTime()
	{
		if (ButtonPressedFirstTime != null)
		{
			ButtonPressedFirstTime.Invoke();
		}
	}

	public virtual void OnPointerUp(PointerEventData data)
	{
		if (CurrentState == ButtonStates.ButtonPressed || CurrentState == ButtonStates.ButtonDown)
		{
			CurrentState = ButtonStates.ButtonUp;
			if (Time.timeScale != 0f && ReleasedDelay > 0f)
			{
				Invoke("InvokeReleased", ReleasedDelay);
			}
			else
			{
				ButtonReleased.Invoke();
			}
		}
	}

	protected virtual void InvokeReleased()
	{
		if (ButtonReleased != null)
		{
			ButtonReleased.Invoke();
		}
	}

	public virtual void OnPointerPressed()
	{
		CurrentState = ButtonStates.ButtonPressed;
		if (ButtonPressed != null)
		{
			ButtonPressed.Invoke();
		}
	}

	protected virtual void ResetButton()
	{
		SetOpacity(_initialOpacity);
		CurrentState = ButtonStates.Off;
	}

	public virtual void OnPointerEnter(PointerEventData data)
	{
		if (!MouseMode)
		{
			OnPointerDown(data);
		}
	}

	public virtual void OnPointerExit(PointerEventData data)
	{
		if (!MouseMode)
		{
			OnPointerUp(data);
		}
	}

	protected virtual void OnEnable()
	{
		ResetButton();
	}

	public virtual void DisableButton()
	{
		CurrentState = ButtonStates.Disabled;
	}

	public virtual void EnableButton()
	{
		if (CurrentState == ButtonStates.Disabled)
		{
			CurrentState = ButtonStates.Off;
		}
	}

	protected virtual void SetOpacity(float newOpacity)
	{
		if (_canvasGroup != null)
		{
			_canvasGroup.alpha = newOpacity;
		}
	}

	protected virtual void UpdateAnimatorStates()
	{
		if (!(_animator == null))
		{
			if (DisabledAnimationParameterName != null)
			{
				_animator.SetBool(DisabledAnimationParameterName, CurrentState == ButtonStates.Disabled);
			}
			if (PressedAnimationParameterName != null)
			{
				_animator.SetBool(PressedAnimationParameterName, CurrentState == ButtonStates.ButtonPressed);
			}
			if (IdleAnimationParameterName != null)
			{
				_animator.SetBool(IdleAnimationParameterName, CurrentState == ButtonStates.Off);
			}
		}
	}

	public virtual void OnSubmit(BaseEventData eventData)
	{
		if (ButtonPressedFirstTime != null)
		{
			ButtonPressedFirstTime.Invoke();
		}
		if (ButtonReleased != null)
		{
			ButtonReleased.Invoke();
		}
	}

	protected virtual float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
