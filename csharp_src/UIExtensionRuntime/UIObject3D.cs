using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
[ExecuteInEditMode]
[AddComponentMenu("UI/UIObject3D/UIObject3D")]
public class UIObject3D : MonoBehaviour
{
	[Header("Target")]
	[SerializeField]
	private GameObject _target;

	[SerializeField]
	private Transform _targetContainer;

	[SerializeField]
	private Vector3 _targetRotation = Vector3.zero;

	[SerializeField]
	[Range(-10f, 10f)]
	private float _targetOffsetX;

	[SerializeField]
	[Range(-10f, 10f)]
	private float _targetOffsetY;

	[Header("Camera Settings")]
	[SerializeField]
	private Camera _targetCamera;

	[SerializeField]
	[Range(20f, 100f)]
	private float _cameraFOV = 35f;

	[SerializeField]
	[Range(-10f, -1f)]
	private float _cameraDistance = -3.5f;

	[SerializeField]
	private Vector2 _textureSize;

	[NonSerialized]
	private bool renderQueued;

	private RectTransform _rectTransform;

	[SerializeField]
	[HideInInspector]
	private UIObject3DRawImage _imageComponent;

	private RenderTexture _renderTexture;

	private static int _objectLayer = -1;

	public GameObject Target
	{
		get
		{
			return _target;
		}
		set
		{
			_target = value;
			HardUpdateDisplay();
		}
	}

	private Vector2 TextureSize
	{
		get
		{
			if (_textureSize != default(Vector2))
			{
				return _textureSize;
			}
			if (_target != null)
			{
				Vector2 vector = new Vector2(Mathf.Abs(Mathf.Floor(rectTransform.rect.width)), Mathf.Abs(Mathf.Floor(rectTransform.rect.height)));
				if (vector.x == 0f || vector.y == 0f)
				{
					vector = new Vector2(256f, 256f);
				}
				_textureSize = vector;
				return vector;
			}
			return Vector2.one;
		}
	}

	private RectTransform rectTransform
	{
		get
		{
			if (_rectTransform == null)
			{
				_rectTransform = GetComponent<RectTransform>();
			}
			return _rectTransform;
		}
	}

	public UIObject3DRawImage imageComponent
	{
		get
		{
			if (_imageComponent == null)
			{
				_imageComponent = GetComponent<UIObject3DRawImage>();
			}
			if (_imageComponent == null)
			{
				_imageComponent = base.gameObject.AddComponent<UIObject3DRawImage>();
			}
			return _imageComponent;
		}
	}

	private RenderTexture renderTexture
	{
		get
		{
			if (_renderTexture == null)
			{
				_renderTexture = new RenderTexture((int)TextureSize.x, (int)TextureSize.y, 16, RenderTextureFormat.ARGB32);
				_renderTexture.filterMode = FilterMode.Bilinear;
				_renderTexture.useMipMap = false;
			}
			return _renderTexture;
		}
	}

	private static int objectLayer
	{
		get
		{
			if (_objectLayer == -1)
			{
				_objectLayer = LayerMask.NameToLayer("UIObject3D");
			}
			return _objectLayer;
		}
	}

	private void DestroyResources()
	{
		if (_targetCamera != null)
		{
			_targetCamera.targetTexture = null;
		}
		if (_renderTexture != null)
		{
			_Destroy(_renderTexture);
			_renderTexture = null;
			_textureSize = default(Vector2);
		}
	}

	public void HardUpdateDisplay()
	{
		DestroyResources();
		Transform targetContainer = _targetContainer;
		if (targetContainer == null)
		{
			targetContainer = base.transform;
		}
		_target.transform.SetParent(targetContainer, worldPositionStays: false);
		SetLayerRecursively(_target.transform, objectLayer);
		UpdateDisplay(instantRender: true);
	}

	private void _Destroy(UnityEngine.Object o)
	{
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(o);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(o);
		}
	}

	public void UpdateDisplay(bool instantRender = false)
	{
		if (imageComponent.texture != renderTexture)
		{
			imageComponent.texture = renderTexture;
		}
		UpdateTargetPositioningAndScale();
		UpdateTargetCameraPositioningEtc();
		Render(instantRender);
	}

	private void OnEnable()
	{
		UpdateDisplay(instantRender: true);
	}

	private void Render(bool instant = false)
	{
		if (Application.isPlaying && !instant)
		{
			renderQueued = true;
		}
		else if (!(_targetCamera == null) && !(_target == null))
		{
			if (_targetCamera.targetTexture != renderTexture)
			{
				_targetCamera.targetTexture = renderTexture;
			}
			_targetCamera.Render();
			renderQueued = false;
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			Render(instant: true);
		}
	}

	private void UpdateTargetPositioningAndScale()
	{
		if (!(_targetContainer == null))
		{
			_targetContainer.transform.localPosition = new Vector3(_targetOffsetX, _targetOffsetY, 0f);
			_targetContainer.transform.localEulerAngles = _targetRotation;
		}
	}

	private void SetLayerRecursively(Transform transform, int layer)
	{
		transform.gameObject.layer = layer;
		foreach (Transform item in transform)
		{
			SetLayerRecursively(item, layer);
		}
	}

	private void UpdateTargetCameraPositioningEtc()
	{
		if (!(_targetCamera == null))
		{
			_targetCamera.transform.localPosition = Vector3.zero + new Vector3(0f, 0f, _cameraDistance);
			_targetCamera.transform.rotation = Quaternion.identity;
			_targetCamera.targetTexture = renderTexture;
			_targetCamera.fieldOfView = _cameraFOV;
			_targetCamera.gameObject.layer = objectLayer;
			_targetCamera.cullingMask = LayerMask.GetMask(LayerMask.LayerToName(objectLayer));
			_targetCamera.backgroundColor = Color.clear;
		}
	}
}
