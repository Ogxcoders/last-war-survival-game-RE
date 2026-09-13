using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("碰撞的层", "碰撞")]
public struct CollectionLayerCondition : ICondition
{
	public S5Game.S5GameColliderLayer Layer;

	public bool IsMe;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		CollectionLayerCondition collectionLayerCondition = ((condition is CollectionLayerCondition) ? ((CollectionLayerCondition)(object)condition) : default(CollectionLayerCondition));
		EcsPool<ComponentPhysics> pool = world.GetPool<ComponentPhysics>();
		EcsPackedEntity ecsPackedEntity = world.PackEntity(entity);
		if (collectionLayerCondition.IsMe && e.Sender != ecsPackedEntity && e.Target != ecsPackedEntity)
		{
			return false;
		}
		EcsPackedEntity packed = ((e.Sender == ecsPackedEntity) ? e.Target : e.Sender);
		if (packed.IsValid(world) && pool.Has(packed.Id))
		{
			ComponentPhysics componentPhysics = pool.Get(packed.Id);
			return collectionLayerCondition.Layer.HasFlag(((S5Game.BodyLogic)componentPhysics.Body.UserData).Layer);
		}
		if (e is EventPhysicCollection eventPhysicCollection)
		{
			int num = ((packed == e.Sender) ? eventPhysicCollection.SenderLayer : eventPhysicCollection.TargetLayer);
			return collectionLayerCondition.Layer.HasFlag((S5Game.S5GameColliderLayer)num);
		}
		return false;
	}
}
