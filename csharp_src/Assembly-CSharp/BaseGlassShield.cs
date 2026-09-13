using UnityEngine;

public class BaseGlassShield : MonoBehaviour
{
	private enum State
	{
		Normal,
		Show,
		Hide,
		Hit
	}

	[SerializeField]
	private Material _material;

	public static float GlassShieldShowTime = 1f;

	public static float GlassShieldHideTime = 1f;

	public static float GlassShieldHitShowDuringTime = 3f;

	public static float GlassShieldShowMaxValue = 1.2f;

	public static float GlassShieldHideMaxValue = 1.2f;

	public static float GlassShieldHitTime = 1f;

	public static float GlassShieldHitMaxValue = 3f;

	public static float GlassShieldHitMinValue = -3f;

	private State _state;

	private float _time;

	private void Awake()
	{
		_state = State.Normal;
	}

	public void Hit()
	{
		_time = 0f;
		_state = State.Show;
		_material.SetFloat("_Dissolve_Show", 0f);
		_material.SetFloat("_Dissolve_Delete", GlassShieldHideMaxValue);
		GameEntry.Timer.RegisterTimer(GlassShieldHitShowDuringTime, delegate
		{
			_time = 0f;
			_state = State.Hide;
		});
	}

	private void Update()
	{
		if (_state == State.Normal)
		{
			return;
		}
		_time += Time.deltaTime;
		switch (_state)
		{
		case State.Show:
			if (_time < GlassShieldShowTime)
			{
				_material.SetFloat("_Dissolve_Show", _time / GlassShieldShowTime * GlassShieldShowMaxValue);
				break;
			}
			_material.SetFloat("_Dissolve_Show", GlassShieldShowMaxValue);
			_state = State.Normal;
			_time = 0f;
			break;
		case State.Hide:
			if (_time < GlassShieldHideTime)
			{
				_material.SetFloat("_Dissolve_Delete", GlassShieldHideMaxValue - _time / GlassShieldHideTime * GlassShieldHideMaxValue);
				break;
			}
			_material.SetFloat("_Dissolve_Delete", 0f);
			_state = State.Normal;
			_time = 0f;
			base.gameObject.SetActive(value: false);
			break;
		case State.Hit:
			if (_time < GlassShieldHitTime)
			{
				_material.SetFloat("_kuosan_Speed", GlassShieldHitMaxValue - _time / GlassShieldHideTime * (GlassShieldHitMaxValue - GlassShieldHitMinValue));
				break;
			}
			_material.SetFloat("_kuosan_Speed", GlassShieldHitMinValue);
			_state = State.Normal;
			_time = 0f;
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Vector3 position = other.transform.position;
		_material.SetVector("_World_Position", position);
		_time = 0f;
		_state = State.Hit;
	}
}
