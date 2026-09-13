using System;
using UnityEngine;

public class CitySpaceMan
{
	private enum State
	{
		Idle,
		Walk,
		Cut,
		Count
	}

	public const string Anim_Walk = "dig_run";

	public const string Anim_Idle = "dig_idle";

	public const string Anim_Cut = "dig";

	private State _currState;

	private CitySpaceManState[] _states = new CitySpaceManState[3];

	private InstanceRequest _instance;

	private Vector3 _position;

	private Quaternion _rotation;

	private Vector3 _velocity;

	private SimpleAnimation[] _anims;

	public Transform _tranform;

	private GameObject _gameObject;

	private GameObject _weaponObj;

	private Rigidbody _body;

	private CitySpaceManAni spaceManAni;

	public Vector3 Position
	{
		get
		{
			if (_tranform != null)
			{
				return _tranform.position;
			}
			return _position;
		}
		set
		{
			if (_tranform != null)
			{
				_tranform.position = value;
			}
			_position = value;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return _rotation;
		}
		set
		{
			_rotation = value;
			if (_tranform != null)
			{
				_tranform.rotation = value;
			}
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return _velocity;
		}
		set
		{
			_velocity = value;
		}
	}

	public CitySpaceMan()
	{
		_states[0] = new CitySpaceManIdle(this);
		_states[1] = new CitySpaceManWalk(this);
		_states[2] = new CitySpaceManCut(this);
		_currState = State.Idle;
	}

	public void CreateGameObject(Action<GameObject> onComplete)
	{
		_instance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/CityScene/CitySpaceMan.prefab");
		_instance.completed += delegate
		{
			_gameObject = _instance.gameObject;
			_tranform = _gameObject.transform;
			_anims = _instance.gameObject.GetComponentsInChildren<SimpleAnimation>();
			SimpleAnimation[] anims = _anims;
			for (int i = 0; i < anims.Length; i++)
			{
				anims[i].GetState("dig_run");
			}
			spaceManAni = _tranform.GetComponentInChildren<CitySpaceManAni>(includeInactive: true);
			_tranform.position = _position;
			_tranform.rotation = _rotation;
			_weaponObj = _tranform.Find("A_soldie_shxr_weapons").gameObject;
			_weaponObj.SetActive(value: false);
			_body = _gameObject.GetComponent<Rigidbody>();
			_states[(int)_currState].OnEnter();
			onComplete?.Invoke(_gameObject);
		};
	}

	public void Destroy()
	{
		_instance?.Destroy();
		_gameObject = null;
		_tranform = null;
		_anims = null;
		_weaponObj = null;
	}

	public void OnUpdate()
	{
		_states[(int)_currState].OnUpdate(Time.deltaTime);
	}

	public GameObject GetObj()
	{
		return _gameObject;
	}

	public void PlayAnim(string animName)
	{
		if (_anims == null)
		{
			return;
		}
		SimpleAnimation[] anims = _anims;
		foreach (SimpleAnimation simpleAnimation in anims)
		{
			if (simpleAnimation.GetState(animName) != null)
			{
				if (animName.Equals("dig"))
				{
					simpleAnimation.GetState(animName).speed = 0.8f;
				}
				simpleAnimation.Play(animName);
			}
		}
		if ((bool)spaceManAni)
		{
			string trigger = "";
			switch (animName)
			{
			case "dig_idle":
				trigger = "idle";
				break;
			case "dig_run":
				trigger = "run";
				break;
			case "dig":
				trigger = "attack";
				break;
			}
			spaceManAni.SetTrigger(trigger);
		}
	}

	public void Walk(float vx, float vz)
	{
		Velocity = new Vector3(vx, 0f, vz);
		_body.velocity = Velocity;
		if (_currState == State.Idle)
		{
			SetState(State.Walk);
		}
	}

	public void StopWalk()
	{
		Velocity = Vector3.zero;
		_body.velocity = Velocity;
		if (_currState == State.Walk)
		{
			SetState(State.Idle);
		}
	}

	public void StartCut()
	{
		SetState(State.Cut);
	}

	public void StopCut()
	{
		if (_currState == State.Cut)
		{
			spaceManAni.SetTrigger("stopAttack");
		}
		if (Velocity == Vector3.zero)
		{
			SetState(State.Idle);
		}
		else
		{
			SetState(State.Walk);
		}
	}

	public void ShowWeapon(bool show)
	{
		if (_weaponObj != null)
		{
			_weaponObj.SetActive(show);
		}
	}

	private void SetState(State newState)
	{
		if (_currState != newState)
		{
			_states[(int)_currState].OnExit();
			_currState = newState;
			_states[(int)newState].OnEnter();
		}
	}
}
