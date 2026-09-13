using UnityEngine;

public class PlaneTroopUnlit : WorldTroopUnit
{
	private const string effectHangPoint = "A_soldier_xfj/A_soldier@aircraft_skin/to_unity/Root";

	private const string attackEffectPath = "Assets/_Art/Effect/prefab/Arms/Xiaofeiji/VFX_xiaofeiji_attack.prefab";

	private const string attackHitPath = "Assets/_Art/Effect/prefab/Arms/Xiaofeiji/VFX_xiaofeiji_hit.prefab";

	private InstanceRequest atkEffectInst;

	public PlaneTroopUnlit(UnitType type, WorldTroop owner)
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
			atkEffectInst = CreateEffect("Assets/_Art/Effect/prefab/Arms/Xiaofeiji/VFX_xiaofeiji_attack.prefab", "A_soldier_xfj/A_soldier@aircraft_skin/to_unity/Root", delegate
			{
				effectList.Add(atkEffectInst);
			});
		}, num);
		YieldUtils.DelayActionWithOutContext(delegate
		{
			PlayHitEffect("Assets/_Art/Effect/prefab/Arms/Xiaofeiji/VFX_xiaofeiji_hit.prefab", target);
		}, num + 0.8f);
		YieldUtils.DelayActionWithOutContext(delegate
		{
			if (atkEffectInst != null)
			{
				atkEffectInst.Destroy();
			}
		}, num + 0.8f + 1f);
	}

	public override void LootAt(Vector3 target)
	{
	}

	public override void StopAttackEffect()
	{
		base.StopAttackEffect();
	}

	public override void Destroy()
	{
		base.Destroy();
	}
}
