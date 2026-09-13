using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIGuideMaskCtrl : MonoBehaviour, ICanvasRaycastFilter, IPointerClickHandler, IEventSystemHandler
{
	private Material _material;

	private RectTransform _target;

	public Vector2 interactOffset;

	public Vector2 visualOffset;

	private Color _color = new Color(0f, 0f, 0f, 0.9f);

	private float _duration;

	private float _anim;

	private Vector2 _center;

	private Vector2 _size;

	private Tween _tween;

	private bool _followTargetMode;

	public RectTransform Target => _target;

	public bool FollowTargetMode
	{
		get
		{
			return _followTargetMode;
		}
		set
		{
			_followTargetMode = value;
			if (value && _target != null && FollowModeCanvas != null)
			{
				Guide(FollowModeCanvas, _target, 0f);
			}
		}
	}

	public Canvas FollowModeCanvas { get; set; }

	public event Action OnClickBlocked;

	private void Awake()
	{
		_material = GetComponent<Image>().material;
		if (_material == null)
		{
			Debug.LogError("UIGuideCtrl: material is null");
		}
	}

	public void SetMaterialProperties(Color color, float rcr, float fade, bool inverse)
	{
		if (_material == null)
		{
			Debug.LogError("UIGuideCtrl: material is null");
			return;
		}
		_color = color;
		_material.SetColor("_Color", color);
		_material.SetFloat("_RCR", rcr);
		_material.SetFloat("_Fade", fade);
		_material.SetFloat("_Inverse", inverse ? 1 : 0);
	}

	public Vector2[] Guide(Canvas canvas, RectTransform target, float duration = 0.5f, Ease ease = Ease.OutQuart)
	{
		if (_material == null)
		{
			Debug.LogError("UIGuideCtrl: material is null");
			return null;
		}
		if (canvas == null)
		{
			Debug.LogError("UIGuideCtrl: canvas is null");
			return null;
		}
		if (target == null)
		{
			Debug.LogError("UIGuideCtrl: target is null");
			return null;
		}
		if (_tween != null)
		{
			_tween.Kill();
			_tween = null;
		}
		Vector3[] array = new Vector3[4];
		Vector3[] array2 = new Vector3[4];
		Vector2[] array3 = new Vector2[4];
		target.GetWorldCorners(array);
		for (int i = 0; i < 4; i++)
		{
			array2[i] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, array[i]);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), array2[i], canvas.worldCamera, out array3[i]);
		}
		_center = (array3[0] + array3[2]) * 0.5f;
		_size = (array3[2] - array3[0]) * 0.5f + interactOffset + visualOffset;
		_target = target;
		_duration = duration;
		_anim = 0f;
		if (_duration <= 0f)
		{
			_material.SetVector("_Rect", new Vector4(_center.x, _center.y, _size.x, _size.y));
		}
		else
		{
			_tween = DOTween.To(() => _anim, delegate(float x)
			{
				_anim = x;
			}, 1f, _duration).OnUpdate(delegate
			{
				float num = Mathf.Lerp(100f, 0f, _anim);
				Color value = Color.Lerp(new Color(_color.r, _color.g, _color.b, 0f), _color, _anim);
				_material.SetColor("_Color", value);
				_material.SetVector("_Rect", new Vector4(_center.x, _center.y, _size.x + num, _size.y + num));
			}).SetEase(ease)
				.OnComplete(delegate
				{
					_material.SetColor("_Color", _color);
					_material.SetVector("_Rect", new Vector4(_center.x, _center.y, _size.x, _size.y));
					_tween = null;
				});
		}
		return new Vector2[2] { _center, _size };
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (!(_target == null))
		{
			return !RectTransformUtility.RectangleContainsScreenPoint(_target, sp, eventCamera, new Vector4(0f - interactOffset.x, 0f - interactOffset.y, 0f - interactOffset.x, 0f - interactOffset.y));
		}
		return true;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.OnClickBlocked != null)
		{
			this.OnClickBlocked();
		}
	}

	private void Update()
	{
		if (FollowTargetMode && _target != null && FollowModeCanvas != null)
		{
			Guide(FollowModeCanvas, _target, 0f);
		}
	}
}
