using UnityEngine;

namespace PVEBattleLogic.Effect;

public class EffectGameObject
{
	private string _path;

	internal bool _isLoaded;

	public int Handle;

	public long ObjId;

	public int Type;

	internal bool _visible;

	private InstanceRequest _request;

	internal GameObject _gameObject;

	internal Transform _transform;

	private bool _valid;

	private TrailRenderer[] _trails;

	private ParticleSystem[] _particles;

	private int _particlesCount;

	private Renderer[] _allRenderers;

	private int _allRenderersCount;

	private uint _renderLayer;

	private Vector3 _position;

	private Quaternion _rotation;

	internal float _countDown;

	private Transform _parent;

	private float _scale;

	public string Path => _path;

	public bool IsLoaded => _isLoaded;

	public EffectGameObject(string path)
	{
		_path = path;
		_visible = false;
		_valid = true;
		_isLoaded = false;
	}

	public void Show(Vector3 position, Quaternion rotation, float time, Transform parent, float scale = 1f)
	{
		_position = position;
		_rotation = rotation;
		_countDown = time;
		_parent = parent;
		_visible = true;
		_scale = scale;
		Load();
		if (_isLoaded)
		{
			OnShow();
			SetVisibleImp(_visible);
		}
	}

	internal void Load()
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
		}
		else if (_valid)
		{
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
			_gameObject.SetActive(value: true);
			OnLoaded();
			if (_visible)
			{
				OnShow();
			}
			else
			{
				_transform.position = Vector3.zero;
			}
			SetVisibleImp(_visible);
		}
	}

	internal virtual void OnLoaded()
	{
	}

	internal virtual void OnShow()
	{
		_transform.SetParent(_parent);
		_transform.localPosition = _position;
		_transform.localRotation = _rotation;
		_transform.localScale = Vector3.one * _scale;
	}

	public virtual void OnUpdate(float deltaTime)
	{
		if (_isLoaded && !(_countDown <= 0f))
		{
			_countDown -= deltaTime;
			if (_countDown <= 0f)
			{
				EffectViewFacade.RemoveEffect(ObjId);
			}
		}
	}

	internal void Dispose()
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
		_allRenderers = null;
		_allRenderersCount = 0;
		_gameObject = null;
		_transform = null;
		_path = null;
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
				_allRenderers[i].enabled = true;
			}
			if (_particlesCount > 0)
			{
				for (int j = 0; j < _particlesCount; j++)
				{
					ParticleSystem obj = _particles[j];
					obj.time = 0f;
					obj.Play();
				}
			}
			return;
		}
		for (int k = 0; k < _allRenderersCount; k++)
		{
			_allRenderers[k].enabled = false;
		}
		if (_particlesCount > 0)
		{
			for (int l = 0; l < _particlesCount; l++)
			{
				_particles[l].Stop();
			}
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

	public void SetParentShow(Transform parent)
	{
		if (_parent != parent)
		{
			_parent = parent;
			if (_isLoaded)
			{
				OnShow();
			}
		}
	}

	public void ResetPosition(Vector3 position)
	{
		_position = position;
		if (_isLoaded)
		{
			_transform.localPosition = _position;
		}
	}

	public void InPool()
	{
		ClearParent();
		SetVisible(visible: false);
		if (_isLoaded)
		{
			_transform.position = Vector3.zero;
		}
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
}
