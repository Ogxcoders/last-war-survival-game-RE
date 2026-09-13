using UnityEngine;

namespace FibMatrix.Rendering;

public class TrailRendererRestrict : MonoBehaviour
{
	private TrailRenderer _trailRenderer;

	private Transform _transform;

	private bool _waitingForDistanceChange;

	private Vector3 _lastPos;

	private int _posChangeValidCount;

	private void Awake()
	{
		_trailRenderer = GetComponent<TrailRenderer>();
		_transform = base.transform;
	}

	private void OnEnable()
	{
		if (_trailRenderer != null)
		{
			_trailRenderer.Clear();
			_trailRenderer.enabled = false;
			_waitingForDistanceChange = true;
			_lastPos = _transform.position;
			_posChangeValidCount = 0;
		}
	}

	private void OnDisable()
	{
		if (_trailRenderer != null)
		{
			_trailRenderer.Clear();
		}
	}

	private void Update()
	{
		if (!_waitingForDistanceChange)
		{
			return;
		}
		Vector3 position = _transform.position;
		float num = Vector3.SqrMagnitude(_lastPos - position);
		_lastPos = position;
		if (num != 0f && !(num > 10f) && ++_posChangeValidCount > 2)
		{
			_waitingForDistanceChange = false;
			if (_trailRenderer != null)
			{
				_trailRenderer.enabled = true;
			}
		}
	}
}
