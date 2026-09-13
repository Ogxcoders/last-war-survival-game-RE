public interface IDynamicLoadBuilding
{
	void OnDynamicModelLoad(int skinId, SimpleAnimation buildingAnim, WorldBuildingAniEffect buildingAniEffect, WorldBuildingAniEffectAni buildingAniEffectAnim);

	void OnDynamicModelUnload();
}
