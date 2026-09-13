using System.Runtime.CompilerServices;
using UnityEngine;

namespace PVEBattleLogic.Bullet;

public class BulletGameObject
{
	private string _path;

	private bool _isLoaded;

	public int Handle;

	internal bool _visible;

	private InstanceRequest _request;

	private GameObject _gameObject;

	internal Transform _transform;

	private bool _valid;

	private TrailRenderer[] _trails;

	private bool _hasSphereCollider;

	private SphereCollider _sphereCollider;

	private bool _hasCapsuleCollider;

	private CapsuleCollider _capsuleCollider;

	private ParticleSystem[] _particles;

	private int _particlesCount;

	private Renderer[] _allRenderers;

	private int _allRenderersCount;

	private uint _renderLayer;

	private Transform _parent;

	private Animation _animation;

	private bool _hasAnimation;

	public string Path => _path;

	public bool IsLoaded => _isLoaded;

	public Vector3 Position
	{
		get
		{
			if (_isLoaded)
			{
				return _transform.position;
			}
			return Vector3.zero;
		}
	}

	public Vector3 EulerAngles
	{
		get
		{
			if (_isLoaded)
			{
				return _transform.eulerAngles;
			}
			return Vector3.zero;
		}
	}

	public BulletGameObject(string bulletViewName)
	{
		_path = bulletViewName;
		_visible = false;
		_valid = true;
		_isLoaded = false;
	}

	internal virtual void Load()
	{
		if (!string.IsNullOrEmpty(_path) && _request == null)
		{
			_isLoaded = false;
			int property = ((Handle == -1) ? 1 : 2);
			_request = GameEntry.Resource.InstantiateAsync(_path, ObjectPoolTag.Normal, property);
			_request.completed += RequestOnCompleted;
		}
	}

	private void RequestOnCompleted(InstanceRequest req)
	{
		if (req.isError)
		{
			req.Destroy();
			return;
		}
		if (!_valid)
		{
			req.Destroy();
			return;
		}
		_gameObject = req.gameObject;
		_transform = _gameObject.transform;
		_isLoaded = true;
		_allRenderers = _gameObject.GetComponentsInChildren<Renderer>();
		_allRenderersCount = _allRenderers.Length;
		if (_allRenderersCount > 0)
		{
			_renderLayer = _allRenderers[0].renderingLayerMask;
		}
		_particles = _gameObject.GetComponentsInChildren<ParticleSystem>();
		_particlesCount = _particles.Length;
		_animation = _gameObject.GetComponentInChildren<Animation>();
		_hasAnimation = _animation != null;
		_gameObject.SetActive(value: true);
		SetVisibleImp(_visible);
		_hasSphereCollider = _gameObject.TryGetComponent<SphereCollider>(out _sphereCollider);
		_hasCapsuleCollider = _gameObject.TryGetComponent<CapsuleCollider>(out _capsuleCollider);
		if (!_visible)
		{
			_transform.position = Vector3.zero;
		}
		OnLoaded();
	}

	internal virtual void OnLoaded()
	{
	}

	public void Dispose()
	{
		_valid = false;
		_particles = null;
		_particlesCount = 0;
		ClearParent();
		SetVisible(visible: true);
		if (_request != null)
		{
			_request.Destroy();
			_request = null;
		}
		_allRenderersCount = 0;
		_allRenderers = null;
		_trails = null;
		_sphereCollider = null;
		_capsuleCollider = null;
		_gameObject = null;
		_transform = null;
		_path = null;
		_animation = null;
		_hasAnimation = false;
	}

	public void SetVisible(bool visible)
	{
		if (_visible != visible)
		{
			_visible = visible;
			SetVisibleImp(visible);
		}
	}

	private void SetVisibleImp(bool visible)
	{
		if (!_isLoaded)
		{
			return;
		}
		if (visible)
		{
			for (int i = 0; i < _allRenderersCount; i++)
			{
				Renderer renderer = _allRenderers[i];
				if (renderer != null)
				{
					renderer.renderingLayerMask = _renderLayer;
				}
			}
			if (_particlesCount > 0)
			{
				for (int j = 0; j < _particlesCount; j++)
				{
					ParticleSystem obj = _particles[j];
					obj.time = 0f;
					obj.Clear();
					obj.Play();
				}
			}
			if (_hasAnimation && _animation != null)
			{
				_animation.Play();
			}
			return;
		}
		for (int k = 0; k < _allRenderersCount; k++)
		{
			Renderer renderer2 = _allRenderers[k];
			if (renderer2 != null)
			{
				renderer2.renderingLayerMask = 0u;
			}
		}
		if (_particlesCount > 0)
		{
			for (int l = 0; l < _particlesCount; l++)
			{
				_particles[l].Stop();
			}
		}
		if (_hasAnimation && _animation != null)
		{
			_animation.Stop();
		}
	}

	public void InPool()
	{
		SetVisible(visible: false);
		if (_isLoaded)
		{
			_transform.position = Vector3.zero;
		}
		ClearParent();
	}

	public void OutPool()
	{
		SetVisible(visible: true);
	}

	public void ClearTrailRenderer()
	{
		if (_isLoaded)
		{
			if (_trails == null)
			{
				_trails = _gameObject.GetComponentsInChildren<TrailRenderer>();
			}
			int i = 0;
			for (int num = _trails.Length; i < num; i++)
			{
				_trails[i].Clear();
			}
		}
	}

	public LineRenderer[] GetLineRendererArray()
	{
		return _gameObject.GetComponentsInChildren<LineRenderer>();
	}

	public bool TryGetSphereCollider(out SphereCollider sphereCollider)
	{
		if (_isLoaded)
		{
			sphereCollider = _sphereCollider;
			return _hasSphereCollider;
		}
		sphereCollider = null;
		return false;
	}

	public bool TryGetCapsuleCollider(out CapsuleCollider capsuleCollider)
	{
		if (_isLoaded)
		{
			capsuleCollider = _capsuleCollider;
			return _hasCapsuleCollider;
		}
		capsuleCollider = null;
		return false;
	}

	public Vector3 GetTransformPoint(Vector3 point)
	{
		if (_isLoaded)
		{
			return _transform.TransformPoint(point);
		}
		return Vector3.zero;
	}

	public Vector3 GetForward()
	{
		if (_isLoaded)
		{
			return _transform.forward;
		}
		return Vector3.forward;
	}

	public Vector3 GetRight()
	{
		if (_isLoaded)
		{
			return _transform.right;
		}
		return Vector3.right;
	}

	public Vector3 GetUp()
	{
		if (_isLoaded)
		{
			return _transform.up;
		}
		return Vector3.up;
	}

	public void Translate(float translationZ)
	{
		if (_isLoaded)
		{
			_transform.Translate(0f, 0f, translationZ);
		}
	}

	public void Translate(Vector3 translation)
	{
		if (_isLoaded)
		{
			_transform.Translate(translation);
		}
	}

	public void LookAt(Vector3 point)
	{
		if (_isLoaded)
		{
			_transform.LookAt(point);
		}
	}

	public Transform GetTransform()
	{
		return _transform;
	}

	public void SetForward(Vector3 forward)
	{
		if (_isLoaded)
		{
			_transform.forward = forward;
		}
	}

	public void ResetLocalRotation()
	{
		if (_isLoaded)
		{
			_transform.localRotation = Quaternion.identity;
		}
	}

	public void SetEulerAngles(Vector3 angle)
	{
		if (_isLoaded)
		{
			_transform.eulerAngles = angle;
		}
	}

	public void SetLocalScale(Vector3 scale)
	{
		if (_isLoaded)
		{
			_transform.localScale = scale;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetPosition(Vector3 pos)
	{
		if (_isLoaded)
		{
			_transform.position = pos;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetLocalPosition(Vector3 pos)
	{
		if (_isLoaded)
		{
			_transform.localPosition = pos;
		}
	}

	public void SetParent(Transform parent)
	{
		if (_isLoaded)
		{
			_transform.SetParent(parent);
			_parent = parent;
		}
	}

	private void ClearParent()
	{
		if (_isLoaded && _parent != null)
		{
			_transform.SetParent(null);
		}
		_parent = null;
	}

	public virtual Vector3 GetColliderCenterWorldPos()
	{
		if (_isLoaded)
		{
			return _transform.position;
		}
		return Vector3.zero;
	}

	public virtual float GetDotMaxCD()
	{
		return 0f;
	}
}
