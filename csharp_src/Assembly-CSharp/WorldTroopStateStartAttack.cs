using System.Collections.Generic;
using UnityEngine;

public class WorldTroopStateStartAttack : WorldTroopStateBase
{
	private int createdCount;

	private List<WorldTroop> troopList = new List<WorldTroop>();

	private int[] pos = new int[5] { 4, -4, 8, -8, 12 };

	public WorldTroopStateStartAttack(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		base.OnStateEnter();
		if (worldTroop == null)
		{
			return;
		}
		worldTroop.SetRotationRoot();
		List<ArmyInfo> armyInfos = worldTroop.GetMarchInfo().armyInfos;
		WorldMarch march = SceneManager.World.GetMarch(worldTroop.GetMarchUUID());
		worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
		GameEntry.Event.Fire(EventId.HideTroopName, worldTroop.GetMarchUUID());
		GameEntry.Event.Fire(EventId.ShowTroopHeadInBattle, worldTroop.GetMarchUUID());
		if (armyInfos.Count > 1)
		{
			worldTroop.MoveFd();
			if (worldTroop.GetTransform() != null)
			{
				Transform transform = worldTroop.GetTransform().Find("Model/A_vehicle_ybc_prefab");
				if (transform != null)
				{
					transform.gameObject.SetActive(value: true);
				}
			}
		}
		foreach (ArmyInfo item2 in armyInfos)
		{
			if (item2.uuid != worldTroop.GetMarchUUID())
			{
				WorldMarch march2 = SceneManager.World.GetMarch(item2.uuid);
				if (march2 != null)
				{
					march2.type = march.type;
					march2.status = march.status;
				}
				WorldTroop item = SceneManager.World.CreateGroupTroop(march2);
				troopList.Add(item);
			}
		}
		if (troopList.Count == 0 && worldTroop.GetMarchInfo() != null)
		{
			ChangeState(WorldTroopState.Attack);
		}
	}

	public override void OnStateLeave()
	{
		createdCount = 0;
		base.OnStateLeave();
		troopList.Clear();
	}

	public override void OnStateUpdate(float deltaTime)
	{
		base.OnStateUpdate(deltaTime);
		for (int i = 0; i < troopList.Count; i++)
		{
			WorldTroop worldTroop = troopList[i];
			if (worldTroop.WorldTroopObjectIsCreate())
			{
				Vector3 position = base.worldTroop.GetModel().transform.position + base.worldTroop.GetModel().transform.right * pos[createdCount];
				worldTroop.SetPosition(position);
				worldTroop.SetRotation(Quaternion.LookRotation(base.worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
				worldTroop.ChangeFsmState(WorldTroopState.Attack);
				createdCount++;
			}
		}
		if (createdCount == troopList.Count && createdCount > 0 && base.worldTroop.GetMarchInfo() != null)
		{
			ChangeState(WorldTroopState.Attack);
		}
	}
}
