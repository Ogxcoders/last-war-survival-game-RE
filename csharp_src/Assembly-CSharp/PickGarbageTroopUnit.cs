using UnityEngine;

public class PickGarbageTroopUnit : WorldTroopUnit
{
	public PickGarbageTroopUnit(UnitType type, WorldTroop owner)
		: base(type, owner)
	{
	}

	public override void PlayAttackEffect()
	{
	}

	public override void StopAttackEffect()
	{
	}

	public override void LootAt(Vector3 target)
	{
		Quaternion rotation = Quaternion.LookRotation((target - gameObject.transform.position).normalized);
		gameObject.transform.rotation = rotation;
	}
}
