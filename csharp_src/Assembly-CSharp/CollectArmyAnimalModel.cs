using UnityEngine;

public class CollectArmyAnimalModel : MonoBehaviour
{
	private enum AnimalAnimationState
	{
		None,
		FlayTarget,
		Work,
		FlaySelf,
		FlayEnd,
		Full
	}

	[SerializeField]
	private SimpleAnimation _anim;

	[SerializeField]
	private SimpleAnimation _animatorResource;

	public static float MoveSpeed = 1.7f;

	public static float TargetDistance = 2f;

	public static float SelfDistance = 1.3f;

	public static float RotateNeedTime = 0.3f;

	public static float ShowResourceTime = 2f;

	public static float EndFullSelfDistance = 0.8f;

	public WorldArmyCollectAnimalManager.Param _param;

	private Vector3 _targetPoint;

	private Vector3 _selfPoint;

	private AnimalAnimationState _curState;

	private float _needTime;

	private float _curTime;

	private float _allTime;

	private float _distance;

	private Quaternion _goQuaternion;

	private Quaternion _backQuaternion;

	private Quaternion _goQuaternion2;

	public void Init(WorldArmyCollectAnimalManager.Param param)
	{
		_param = param;
		_targetPoint = SceneManager.World.TileIndexToWorld(param.targetPointId);
		_selfPoint = SceneManager.World.TileIndexToWorld(param.pointIndex);
		_targetPoint += Vector3.Normalize(_selfPoint - _targetPoint) * TargetDistance;
		_selfPoint += Vector3.Normalize(_targetPoint - _selfPoint) * SelfDistance;
		_distance = Vector3.Distance(_targetPoint, _selfPoint);
		Vector3 to = Vector3.Normalize(_targetPoint - _selfPoint);
		float num = Vector3.Angle(Vector3.forward, to);
		if (_targetPoint.x < _selfPoint.x)
		{
			num = 360f - num;
		}
		_goQuaternion = Quaternion.Euler(0f, num, 0f);
		_backQuaternion = Quaternion.Euler(0f, num - 180f, 0f);
		_goQuaternion2 = Quaternion.Euler(0f, num - 360f, 0f);
		_curState = AnimalAnimationState.FlayTarget;
		ChangeAnimalState();
	}

	public void UnInit()
	{
	}

	private void ChangeAnimalState()
	{
		switch (_curState)
		{
		case AnimalAnimationState.None:
			base.transform.position = _selfPoint + Vector3.Normalize(_targetPoint - _selfPoint) * EndFullSelfDistance;
			_animatorResource.gameObject.SetActive(value: false);
			break;
		case AnimalAnimationState.FlayTarget:
			_animatorResource.gameObject.SetActive(value: false);
			_curTime = 0f;
			_allTime = 0f;
			_needTime = _distance / MoveSpeed;
			_anim.Play("xiaoren_run_01");
			break;
		case AnimalAnimationState.Work:
			_animatorResource.gameObject.SetActive(value: true);
			_animatorResource.Play("crystal_work");
			_curTime = 0f;
			_anim.Play("xiaoren_work");
			_needTime = _anim.GetClipLength("xiaoren_work");
			break;
		case AnimalAnimationState.FlaySelf:
			_animatorResource.gameObject.SetActive(value: true);
			_curTime = 0f;
			_needTime = _distance / MoveSpeed;
			_anim.Play("xiaoren_run_02");
			_animatorResource.Play("crystal_run");
			break;
		case AnimalAnimationState.FlayEnd:
			_animatorResource.gameObject.SetActive(value: true);
			_curTime = 0f;
			_needTime = _anim.GetClipLength("xiaoren_end");
			_anim.Play("xiaoren_end");
			_animatorResource.Play("crystal_end");
			break;
		case AnimalAnimationState.Full:
			base.transform.position = _selfPoint + Vector3.Normalize(_targetPoint - _selfPoint) * EndFullSelfDistance;
			_animatorResource.gameObject.SetActive(value: false);
			_anim.Play("xiaoren_scanning");
			break;
		}
	}

	private void Update()
	{
		_curTime += Time.deltaTime;
		_allTime += Time.deltaTime;
		switch (_curState)
		{
		case AnimalAnimationState.FlayTarget:
		{
			float num2 = _curTime / _needTime;
			base.transform.position = Vector3.Lerp(_selfPoint, _targetPoint, num2);
			float num3 = _curTime / RotateNeedTime;
			if (num3 <= 1f)
			{
				base.transform.rotation = Quaternion.Slerp(_backQuaternion, _goQuaternion2, num3);
			}
			if (num2 >= 1f)
			{
				base.transform.rotation = _goQuaternion;
				_curState = AnimalAnimationState.Work;
				ChangeAnimalState();
			}
			break;
		}
		case AnimalAnimationState.Work:
			if (_curTime >= _needTime)
			{
				_curState = AnimalAnimationState.FlaySelf;
				ChangeAnimalState();
			}
			break;
		case AnimalAnimationState.FlaySelf:
		{
			float num4 = _curTime / _needTime;
			base.transform.position = Vector3.Lerp(_targetPoint, _selfPoint, num4);
			float num5 = _curTime / RotateNeedTime;
			if (num5 <= 1f)
			{
				base.transform.rotation = Quaternion.Slerp(_goQuaternion, _backQuaternion, num5);
			}
			if (num4 >= 1f)
			{
				base.transform.rotation = _backQuaternion;
				_curState = AnimalAnimationState.FlayEnd;
				ChangeAnimalState();
			}
			break;
		}
		case AnimalAnimationState.FlayEnd:
		{
			float num = _curTime / _needTime;
			_ = 0.5f;
			if (num >= 1f)
			{
				GameEntry.Event.Fire(EventId.WorldArmyCollectAnimEnd, _param.uuid);
				_curState = AnimalAnimationState.FlayTarget;
				ChangeAnimalState();
			}
			break;
		}
		}
	}
}
