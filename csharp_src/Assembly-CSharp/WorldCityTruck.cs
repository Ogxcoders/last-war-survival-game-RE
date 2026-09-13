using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class WorldCityTruck : WorldPeopleTruckBase
{
	public class Param
	{
		public List<List<Vector2Int>> _pathList;

		public bool isRadom;

		public bool isCircle;

		public float radius;

		public Vector3 mainPos;

		public float initAngle;

		public bool isInner;
	}

	private static readonly string Moving = "run";

	protected internal void CSInit()
	{
		BaseInit();
		_Tsm = new Dictionary<States, ITruckPeopleSate>
		{
			{
				States.Init,
				new TruckInitSate()
			},
			{
				States.GotoTarget,
				new TruckGotoTargetState()
			},
			{
				States.WaitTarget,
				new TruckWaitTargetState()
			},
			{
				States.GoBack,
				new TruckGoBackState()
			}
		};
		base._curState = States.Init;
		base.gameObject.SetActive(value: false);
		hasInit = true;
	}

	protected internal void CSShow(object userData)
	{
		Param param = userData as Param;
		isRandom = param.isRadom;
		pathList = param._pathList;
		isCircle = param.isCircle;
		mainPos = param.mainPos;
		radius = param.radius;
		initAngle = param.initAngle;
		isInner = param.isInner;
		if (isCircle)
		{
			_Tsm = new Dictionary<States, ITruckPeopleSate>
			{
				{
					States.Init,
					new TruckInitSate()
				},
				{
					States.Circle,
					new TruckGoCircleState()
				}
			};
		}
		BaseShow();
		PlayAnim(Moving);
		if (isCircle)
		{
			ChangeState(States.Circle);
		}
		else
		{
			ChangeState(States.GotoTarget);
		}
	}

	private void Update()
	{
		if (_Tsm != null && _Tsm.ContainsKey(base._curState))
		{
			_Tsm[base._curState].OnUpdate(this, Time.deltaTime);
			BaseUpdate();
		}
	}

	public void OnDestroy()
	{
		if (base._curState != States.Init)
		{
			if (!_Tsm.ContainsKey(base._curState))
			{
				Log.Error("curState not in tsm : {0}, {1}", base._curState, _Tsm.Keys);
			}
			else
			{
				_Tsm[base._curState].OnLeave(this);
			}
		}
	}
}
