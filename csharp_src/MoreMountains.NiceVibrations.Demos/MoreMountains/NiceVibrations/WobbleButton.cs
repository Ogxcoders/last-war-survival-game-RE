using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations;

public class WobbleButton : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	[Header("Bindings")]
	public Camera TargetCamera;

	public AudioSource SpringAudioSource;

	public Animator TargetAnimator;

	[Header("Haptics")]
	public TextAsset AHAPFile;

	[Header("Colors")]
	public Image TargetModel;

	[Header("Wobble")]
	public float OffDuration = 0.1f;

	public float MaxRange;

	public AnimationCurve WobbleCurve;

	public float DragResetDuration = 4f;

	public float WobbleFactor = 2f;

	protected Vector3 _neutralPosition;

	protected Canvas _canvas;

	protected Vector3 _newTargetPosition;

	protected Vector3 _eventPosition;

	protected Vector2 _workPosition;

	protected float _initialZPosition;

	protected bool _dragging;

	protected int _pointerID;

	protected PointerEventData _pointerEventData;

	protected RectTransform _rectTransform;

	protected Vector3 _dragEndedPosition;

	protected float _dragEndedAt;

	protected Vector3 _dragResetDirection;

	protected bool _pointerOn;

	protected bool _draggedOnce;

	protected int _sparkAnimationParameter;

	protected long[] _wobbleAndroidPattern = new long[4] { 0L, 40L, 40L, 80L };

	protected int[] _wobbleAndroidAmplitude = new int[4] { 0, 40, 0, 80 };

	public RenderMode ParentCanvasRenderMode { get; protected set; }

	protected virtual void Start()
	{
	}

	public virtual void SetPitch(float newPitch)
	{
		SpringAudioSource.pitch = newPitch;
	}

	public virtual void Initialization()
	{
		_sparkAnimationParameter = Animator.StringToHash("Spark");
		ParentCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
		_canvas = GetComponentInParent<Canvas>();
		_initialZPosition = base.transform.position.z;
		_rectTransform = base.gameObject.GetComponent<RectTransform>();
		SetNeutralPosition();
	}

	public virtual void SetNeutralPosition()
	{
		_neutralPosition = _rectTransform.transform.position;
	}

	protected virtual Vector3 GetWorldPosition(Vector3 testPosition)
	{
		if (ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, testPosition, _canvas.worldCamera, out _workPosition);
			return _canvas.transform.TransformPoint(_workPosition);
		}
		return testPosition;
	}

	protected virtual void Update()
	{
		if (_pointerOn && !_dragging)
		{
			_newTargetPosition = GetWorldPosition(_pointerEventData.position);
			if ((_newTargetPosition - _neutralPosition).magnitude < MaxRange)
			{
				_dragging = true;
			}
			else
			{
				_dragging = false;
			}
		}
		if (_dragging)
		{
			StickToPointer();
		}
		else
		{
			GoBackToInitialPosition();
		}
	}

	protected virtual void StickToPointer()
	{
		_draggedOnce = true;
		_eventPosition = _pointerEventData.position;
		_newTargetPosition = GetWorldPosition(_eventPosition);
		_newTargetPosition = Vector2.ClampMagnitude(_newTargetPosition - _neutralPosition, MaxRange);
		_newTargetPosition = _neutralPosition + _newTargetPosition;
		_newTargetPosition.z = _initialZPosition;
		base.transform.position = _newTargetPosition;
	}

	protected virtual void GoBackToInitialPosition()
	{
		if (_draggedOnce)
		{
			if (Time.time - _dragEndedAt < DragResetDuration)
			{
				float time = Remap(Time.time - _dragEndedAt, 0f, DragResetDuration, 0f, 1f);
				float t = WobbleCurve.Evaluate(time) * WobbleFactor;
				_newTargetPosition = Vector3.LerpUnclamped(_neutralPosition, _dragEndedPosition, t);
				_newTargetPosition.z = _initialZPosition;
			}
			else
			{
				_newTargetPosition = _neutralPosition;
				_newTargetPosition.z = _initialZPosition;
			}
			base.transform.position = _newTargetPosition;
		}
	}

	public virtual void OnPointerEnter(PointerEventData data)
	{
		_pointerID = data.pointerId;
		_pointerEventData = data;
		_pointerOn = true;
	}

	public virtual void OnPointerExit(PointerEventData data)
	{
		_eventPosition = _pointerEventData.position;
		_newTargetPosition = GetWorldPosition(_eventPosition);
		_newTargetPosition = Vector2.ClampMagnitude(_newTargetPosition - _neutralPosition, MaxRange);
		_newTargetPosition = _neutralPosition + _newTargetPosition;
		_newTargetPosition.z = _initialZPosition;
		_dragging = false;
		_dragEndedPosition = _newTargetPosition;
		_dragEndedAt = Time.time;
		_dragResetDirection = _dragEndedPosition - _neutralPosition;
		_pointerOn = false;
		TargetAnimator.SetTrigger(_sparkAnimationParameter);
		SpringAudioSource.Play();
		MMVibrationManager.AdvancedHapticPattern(AHAPFile.text, _wobbleAndroidPattern, _wobbleAndroidAmplitude, -1, _wobbleAndroidPattern, _wobbleAndroidAmplitude, _wobbleAndroidAmplitude, -1, HapticTypes.LightImpact, this);
	}

	protected virtual float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
