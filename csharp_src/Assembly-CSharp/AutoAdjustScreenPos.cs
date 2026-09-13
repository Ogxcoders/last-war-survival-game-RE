using UnityEngine;

public class AutoAdjustScreenPos : MonoBehaviour
{
	private Vector3 _worldPos;

	private Vector3 _screenPos;

	private Transform _obj;

	private Vector3 _deltaPos;

	private void Awake()
	{
		_screenPos = Vector3.zero;
		_obj = null;
	}

	public void Init(Vector3 pos)
	{
		_worldPos = pos;
	}

	public void Init(Transform obj, Vector3 deltaPos)
	{
		_obj = obj;
		_deltaPos = deltaPos;
	}

	private void RefreshPos()
	{
		if (_obj != null)
		{
			_worldPos = _obj.position + _deltaPos;
		}
		Vector3 position = SceneManager.World.WorldToScreenPoint(_worldPos);
		position = GameEntry.UICamera.ScreenToWorldPoint(position);
		if (position != _screenPos)
		{
			_screenPos = position;
			base.transform.position = _screenPos;
		}
	}

	private void OnEnable()
	{
		SceneManager.World.AfterUpdate += RefreshPos;
	}

	private void OnDisable()
	{
		if (SceneManager.World != null)
		{
			SceneManager.World.AfterUpdate -= RefreshPos;
		}
	}
}
