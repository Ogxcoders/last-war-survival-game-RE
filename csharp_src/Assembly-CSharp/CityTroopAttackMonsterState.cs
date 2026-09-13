using UnityEngine;
using XLua;

public class CityTroopAttackMonsterState : BaseCityTroopState
{
	private Vector3 _pos;

	private long startTimeSecond;

	private long endTimeSecond;

	private long lastAttackSecond;

	private InstanceRequest normalAttackInst;

	private LuaTable monsterData;

	private int perBlood = 100;

	private int continueTime = 5;

	public CityTroopAttackMonsterState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		_pos = Vector3.zero;
		int param = SceneManager.World.WorldToTileIndex(cityTroop.EndPos);
		monsterData = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetCityPointDataByPointId", param);
		startTimeSecond = GameEntry.Timer.GetServerTimeSeconds();
		endTimeSecond = startTimeSecond + continueTime;
		lastAttackSecond = startTimeSecond;
	}

	public override void OnUpdate(float deltaTime)
	{
		CheckAttack();
	}

	public override void OnLeave()
	{
		RemoveAttack();
	}

	public override void OnBeginDrag()
	{
		SceneManager.World.CanMoving = false;
		cityTroop.OnCreateDragLine();
	}

	public override void OnDrag()
	{
		SceneManager.World.CanMoving = false;
		Vector3 touchPoint = SceneManager.World.GetTouchPoint();
		if (_pos != touchPoint)
		{
			_pos = touchPoint;
			cityTroop.OnDragLineUpdate(touchPoint);
		}
	}

	public override void OnEndDrag()
	{
		SceneManager.World.CanMoving = true;
		cityTroop.OnDragLineStop();
		Vector3 vector = CheckCanMovePosByPos(SceneManager.World.GetTouchPoint());
		if (vector != Vector3.zero)
		{
			cityTroop.EndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			RemoveAttack();
			machine.ChangeState(TroopState.Move);
		}
	}

	public override void CityTroopMoveSignal(Vector3 pos1)
	{
		Vector3 vector = CheckCanMovePosByPos(pos1);
		if (vector != Vector3.zero)
		{
			cityTroop.EndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			RemoveAttack();
			machine.ChangeState(TroopState.Move);
		}
	}

	private void AttackResult()
	{
		if (monsterData != null)
		{
			long param = monsterData.Get<long>("uuid");
			GameEntry.Lua.Call("DataCenter.GuideCityManager:SendCityWorldFight", param, SceneManager.World.GetFormationUuid());
		}
		machine.ChangeState(TroopState.FightResult);
	}

	private void CheckAttack()
	{
		if (cityTroop == null)
		{
			return;
		}
		int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
		if (serverTimeSeconds < endTimeSecond)
		{
			if (serverTimeSeconds - lastAttackSecond < 1)
			{
				return;
			}
			lastAttackSecond = serverTimeSeconds;
			InstanceRequest requestInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/VFX_putonggongji.prefab");
			normalAttackInst = requestInst;
			requestInst.completed += delegate
			{
				Transform transform = cityTroop.GetTransform();
				if (transform != null)
				{
					Transform transform2 = transform.Find("Model/WorldTroop(Clone)/Model/A_vehicle_ybc_prefab/root");
					if (transform2 != null)
					{
						requestInst.gameObject.transform.SetParent(transform2);
						requestInst.gameObject.transform.localPosition = Vector3.zero;
						requestInst.gameObject.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
						YieldUtils.DelayActionWithOutContext(delegate
						{
							if (requestInst != null)
							{
								requestInst.Destroy();
							}
						}, 1f);
					}
					else
					{
						RemoveAttack();
					}
				}
				else
				{
					RemoveAttack();
				}
			};
			ShowTroopBlood();
			ShowMonsterBlood();
		}
		else
		{
			AttackResult();
		}
	}

	private void RemoveAttack()
	{
		if (normalAttackInst != null)
		{
			normalAttackInst.Destroy();
			normalAttackInst = null;
		}
	}

	private void ShowMonsterBlood()
	{
		int num = Random.Range(35, 45);
		BattleDecBloodTip.Param param = new BattleDecBloodTip.Param
		{
			startPos = cityTroop.EndPos,
			num = num,
			path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab"
		};
		InstanceRequest temp = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab");
		temp.completed += delegate
		{
			GameObject gameObject = temp.gameObject;
			gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			gameObject.GetComponent<BattleDecBloodTip>().CSShow(param, temp);
		};
	}

	private void ShowTroopBlood()
	{
		int num = Random.Range(18, 23);
		BattleDecBloodTip.Param param = new BattleDecBloodTip.Param
		{
			startPos = cityTroop.transform.position,
			num = num,
			path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab"
		};
		InstanceRequest temp = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab");
		temp.completed += delegate
		{
			GameObject gameObject = temp.gameObject;
			gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			gameObject.GetComponent<BattleDecBloodTip>().CSShow(param, temp);
		};
	}
}
