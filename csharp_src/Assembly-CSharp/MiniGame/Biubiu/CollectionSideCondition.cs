using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("碰撞方向", "碰撞")]
public struct CollectionSideCondition : ICondition
{
	public CollectionSide CollectionSide;

	public bool IsMe;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (e is EventPhysicCollection eventPhysicCollection)
		{
			CollectionSideCondition collectionSideCondition = ((condition is CollectionSideCondition) ? ((CollectionSideCondition)(object)condition) : default(CollectionSideCondition));
			EcsPackedEntity ecsPackedEntity = world.PackEntity(entity);
			if (collectionSideCondition.IsMe && e.Sender != ecsPackedEntity && e.Target != ecsPackedEntity)
			{
				return false;
			}
			bool flag = false;
			if (collectionSideCondition.CollectionSide.HasFlag(CollectionSide.Left))
			{
				flag = eventPhysicCollection.Manifold.Normal.X < -0.1f;
			}
			if (flag)
			{
				return true;
			}
			if (collectionSideCondition.CollectionSide == CollectionSide.Right)
			{
				flag = eventPhysicCollection.Manifold.Normal.X > 0.1;
			}
			if (flag)
			{
				return true;
			}
			if (collectionSideCondition.CollectionSide == CollectionSide.Up)
			{
				flag = eventPhysicCollection.Manifold.Normal.Y > 0.1f;
			}
			if (flag)
			{
				return true;
			}
			if (collectionSideCondition.CollectionSide == CollectionSide.Down)
			{
				flag = eventPhysicCollection.Manifold.Normal.Y < -0.1f;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}
}
