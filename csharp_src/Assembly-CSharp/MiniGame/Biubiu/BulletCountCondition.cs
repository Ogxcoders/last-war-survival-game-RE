using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("地图子弹数量", "实体")]
public struct BulletCountCondition : ICondition
{
	public int Count;

	public ConditionOp Op;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is BulletCountCondition bulletCountCondition)
		{
			int entitiesCount = world.Filter<ComponentBullet>().End().GetEntitiesCount();
			return FuncCondition.Comparable(bulletCountCondition.Op, entitiesCount, bulletCountCondition.Count);
		}
		return false;
	}
}
