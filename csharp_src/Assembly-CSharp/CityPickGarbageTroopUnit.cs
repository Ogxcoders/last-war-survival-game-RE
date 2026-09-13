using UnityEngine;

public class CityPickGarbageTroopUnit : CityTroopUnit
{
	public CityPickGarbageTroopUnit(UnitType type, CityTroop owner)
		: base(type, owner)
	{
	}

	public override void LootAt(Vector3 target)
	{
		Quaternion rotation = Quaternion.LookRotation((target - gameObject.transform.position).normalized);
		gameObject.transform.rotation = rotation;
	}
}
