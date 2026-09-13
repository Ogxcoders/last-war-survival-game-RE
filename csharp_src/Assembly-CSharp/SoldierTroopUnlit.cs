using UnityEngine;

public class SoldierTroopUnlit : WorldTroopUnit
{
	private const string attackEffectPath = "Assets/_Art/Effect/prefab/Arms/Jiqiangbing/VFX_jiqiangbing_attack_one.prefab";

	private const string attackHitPath = "Assets/_Art/Effect/prefab/Arms/Jiqiangbing/VFX_jiqiangbing_attack_one_hit01.prefab";

	private const string effectHangPoint = "A_soldier_jqb01_01/A_soldie@jqb_skin/DeformationSystem/root";

	private InstanceRequest atkEffectInst;

	private GameObject atkEffectObject;

	private WaitForSeconds loopWait;

	private WaitForSeconds birthDelay = new WaitForSeconds(0.8f);

	private bool isRun;

	public SoldierTroopUnlit(UnitType type, WorldTroop owner)
		: base(type, owner)
	{
	}

	public override void PlayAttackEffect()
	{
		base.PlayAttackEffect();
		float num = 1f;
		if (anim != null)
		{
			num = anim.GetState("attack").length;
		}
		YieldUtils.DelayActionWithOutContext(delegate
		{
			atkEffectInst = CreateEffect("Assets/_Art/Effect/prefab/Arms/Jiqiangbing/VFX_jiqiangbing_attack_one.prefab", "A_soldier_jqb01_01/A_soldie@jqb_skin/DeformationSystem/root", delegate
			{
				effectList.Add(atkEffectInst);
			});
		}, num);
		YieldUtils.DelayActionWithOutContext(delegate
		{
			PlayHitEffect("Assets/_Art/Effect/prefab/Arms/Jiqiangbing/VFX_jiqiangbing_attack_one_hit01.prefab", target);
		}, num + 0.8f);
		YieldUtils.DelayActionWithOutContext(delegate
		{
			if (atkEffectInst != null)
			{
				atkEffectInst.Destroy();
			}
		}, num + 0.8f + 1f);
	}

	public override void StopAttackEffect()
	{
		base.StopAttackEffect();
	}

	public override void Destroy()
	{
		base.Destroy();
	}

	public override void LootAt(Vector3 target)
	{
	}
}
