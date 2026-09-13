using System.Collections.Generic;
using UnityEngine;

public class WorldExploreObject : WorldDetectEventItemObject
{
	private GPUSkinningAnimator[] gpuAnims;

	private ExplorePointInfoState currentState;

	private float releaseSkillTick;

	private const string sheldPath = "Assets/_Art/Effect/prefab/hero/Shaonian/VFX_shaonian_hudun.prefab";

	public float RelaseSkillTick
	{
		get
		{
			return releaseSkillTick;
		}
		set
		{
			releaseSkillTick = value;
		}
	}

	public WorldExploreObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override bool DoDisappear()
	{
		return false;
	}

	protected override bool NeedShowDetectEventIcon()
	{
		return true;
	}

	public Transform GetTransform()
	{
		if ((bool)gameObject && gameObject.transform != null)
		{
			return gameObject.transform;
		}
		return null;
	}

	protected override bool NeedShowTime()
	{
		return false;
	}

	protected override string GetEventId()
	{
		return (world.GetPointInfo(pointIndex) as ExplorePointInfo).eventId;
	}

	protected override string GetModePath()
	{
		return GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldExploreDetectEventModel", eventId);
	}

	protected override void DoWhenCreateComplete(GameObject model)
	{
		GameEntry.Event.Subscribe(EventId.AttackExploreEnd, DoWhenAttackExploreEnd);
		model.transform.rotation = Quaternion.LookRotation(new Vector3(0.1f, 0f, -1f));
		UIWorldLabel[] componentsInChildren = gameObject.transform.GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetLevel(active: false);
		}
		SetState(ExplorePointInfoState.ExplorePointInfoStateNormal);
	}

	public void DoWhenAttackExploreStart(object userData, int soliderNum, int hp, int hpMax)
	{
		if (currentState != ExplorePointInfoState.ExplorePointInfoStateAttack)
		{
			WorldTroop troop = world.GetTroop((long)userData);
			string userData2 = pointIndex + ";" + soliderNum + ";" + hp + ";" + hpMax;
			GameEntry.Event.Fire(EventId.ShowExploreHeadInBattle, userData2);
			model.transform.rotation = Quaternion.LookRotation(troop.GetPosition() - model.transform.position);
		}
		SetState(ExplorePointInfoState.ExplorePointInfoStateAttack);
	}

	private void DoWhenAttackExploreEnd(object userData)
	{
		GameEntry.Event.Fire(EventId.HideExploreHead, pointIndex);
		SetState(ExplorePointInfoState.ExplorePointInfoStateNormal);
	}

	private void DoAttack(GameObject model)
	{
		DoAnimation(model, "attack");
	}

	public void UpdateBattleHeadUI(int anger, int hp, int hpMax)
	{
		string userData = pointIndex + ";" + anger + ";" + hp + ";" + hpMax;
		GameEntry.Event.Fire(EventId.ShowExploreBattleValue, userData);
	}

	private void DoIdle(GameObject model)
	{
		DoAnimation(model, "idle");
	}

	private void DoAnimation(GameObject model, string animationName)
	{
		if (!model)
		{
			return;
		}
		gpuAnims = model.GetComponentsInChildren<GPUSkinningAnimator>();
		if (gpuAnims != null)
		{
			for (int i = 0; i < gpuAnims.Length; i++)
			{
				gpuAnims[i].Play(animationName, Random.Range(0f, 1f));
			}
		}
	}

	public void SetState(ExplorePointInfoState state)
	{
		if (currentState != state)
		{
			currentState = state;
			if (currentState == ExplorePointInfoState.ExplorePointInfoStateNormal)
			{
				DoIdle(model);
			}
			else if (currentState == ExplorePointInfoState.ExplorePointInfoStateAttack)
			{
				DoAttack(model);
			}
		}
	}

	public override void Destroy()
	{
		GameEntry.Event.Unsubscribe(EventId.AttackExploreEnd, DoWhenAttackExploreEnd);
		GameEntry.Event.Fire(EventId.HideExploreHead, pointIndex);
		base.Destroy();
	}

	public void DoSkill(int useSkillID, DamageType damageType, int heroId, Dictionary<long, List<string>> useSkillList, long useSkillUid)
	{
		string effectPath = GameEntry.ConfigCache.GetTemplateData("skill", useSkillID, "effect_path");
		InstanceRequest requestInst = GameEntry.Resource.InstantiateAsync(effectPath);
		requestInst.completed += delegate
		{
			GameObject gameObject = model.gameObject;
			if (gameObject != null)
			{
				requestInst.gameObject.transform.SetParent(gameObject.transform);
			}
			else
			{
				requestInst.gameObject.transform.SetParent(model.transform);
			}
			requestInst.gameObject.transform.localPosition = Vector3.zero;
			requestInst.gameObject.transform.localRotation = Quaternion.identity;
			requestInst.gameObject.transform.localScale = Vector3.one;
			if (effectPath.IndexOf("VFX_Nvniuzai_attack") >= 0)
			{
				LineRenderer componentInChildren = requestInst.gameObject.transform.GetComponentInChildren<LineRenderer>();
				if (componentInChildren != null)
				{
					componentInChildren.SetPosition(0, componentInChildren.transform.position);
					WorldTroop troop = world.GetTroop(useSkillUid);
					if (troop == null)
					{
						requestInst.gameObject.Destroy();
						return;
					}
					componentInChildren.SetPosition(1, new Vector3(troop.GetTransform().position.x, componentInChildren.transform.position.y, troop.GetTransform().position.z));
				}
			}
			YieldUtils.DelayActionWithOutContext(delegate
			{
				requestInst.gameObject.Destroy();
			}, 2f);
		};
	}

	public void ShowBattleHurt(int hurt, WorldMarchDataManager.BattleWordType worldType)
	{
		if (!(model == null) && !(model.transform == null))
		{
			string path = string.Empty;
			switch (worldType)
			{
			case WorldMarchDataManager.BattleWordType.Cure:
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleCureBloodTip .prefab";
				break;
			case WorldMarchDataManager.BattleWordType.Normal:
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab";
				break;
			case WorldMarchDataManager.BattleWordType.Skill:
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab";
				break;
			}
			world.ShowBattleBlood(new BattleDecBloodTip.Param
			{
				startPos = model.gameObject.transform.position,
				num = hurt,
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab"
			}, path);
		}
	}
}
