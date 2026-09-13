using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class WorldCityPeople : WorldPeopleTruckBase
{
	public enum CityPeopleType
	{
		Normal,
		RoundBase,
		WalkAround
	}

	public class Param
	{
		public List<List<Vector2Int>> _pathList;

		public CityPeopleType PeopleType;

		public int index;
	}

	private Dictionary<CityPeopleType, float> _moveTypeSpeed = new Dictionary<CityPeopleType, float>
	{
		{
			CityPeopleType.Normal,
			1.5f
		},
		{
			CityPeopleType.RoundBase,
			1.4f
		},
		{
			CityPeopleType.WalkAround,
			0.3f
		}
	};

	public CityPeopleType CurCityPeopleType;

	public int index;

	private bool _pause;

	protected internal void CSInit()
	{
		BaseInit();
		_Tsm = new Dictionary<States, ITruckPeopleSate>
		{
			{
				States.Init,
				new CityPeopleInitState()
			},
			{
				States.GotoTarget,
				new CityPeopleGoTargetState()
			}
		};
		base._curState = States.Init;
		base.gameObject.SetActive(value: false);
		hasInit = true;
	}

	protected internal void CSShow(object userData)
	{
		Param param = userData as Param;
		CurCityPeopleType = param.PeopleType;
		pathList = param._pathList;
		index = param.index;
		Speed = _moveTypeSpeed[CurCityPeopleType];
		_pause = false;
		BaseShow();
		ChangeState(States.GotoTarget);
	}

	private void Update()
	{
		if (!_pause && _Tsm != null && _Tsm.ContainsKey(base._curState))
		{
			_Tsm[base._curState].OnUpdate(this, Time.deltaTime);
			BaseUpdate();
		}
	}

	public float PauseAndPlayAnim(string anim)
	{
		if (Anim2 == null)
		{
			return 0f;
		}
		float clipLength = Anim2.GetClipLength(anim);
		if (clipLength > 0f)
		{
			_pause = true;
			Anim2.Rewind(anim);
			Anim2.Play(anim);
			return clipLength;
		}
		return 0f;
	}

	public void Resume()
	{
		if (_pause)
		{
			_pause = false;
			if (string.IsNullOrEmpty(_curAnim))
			{
				Anim2.CrossFade("Default", 0.2f);
			}
			else
			{
				Anim2.CrossFade(_curAnim, 0.2f);
			}
		}
	}

	public void OnDestroy()
	{
		if (base._curState != States.Init)
		{
			if (!_Tsm.ContainsKey(base._curState))
			{
				Log.Error("curState not in tsm : {0}, {1}", base._curState, _Tsm.Keys);
				return;
			}
			GameEntry.Event.Fire(EventId.CitySolderDelete, index);
			_Tsm[base._curState].OnLeave(this);
		}
	}
}
