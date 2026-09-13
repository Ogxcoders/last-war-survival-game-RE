using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("互相碰撞", "碰撞")]
public struct CollectLayerEachOtherCondition : ICondition
{
	public S5Game.S5GameColliderLayer ALayer;

	public S5Game.S5GameColliderLayer BLayer;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		CollectLayerEachOtherCondition collectLayerEachOtherCondition = ((condition is CollectLayerEachOtherCondition) ? ((CollectLayerEachOtherCondition)(object)condition) : default(CollectLayerEachOtherCondition));
		if (e is EventPhysicCollection eventPhysicCollection)
		{
			if (collectLayerEachOtherCondition.ALayer.HasFlag((S5Game.S5GameColliderLayer)eventPhysicCollection.SenderLayer) && collectLayerEachOtherCondition.BLayer.HasFlag((S5Game.S5GameColliderLayer)eventPhysicCollection.TargetLayer))
			{
				return true;
			}
			if (collectLayerEachOtherCondition.ALayer.HasFlag((S5Game.S5GameColliderLayer)eventPhysicCollection.TargetLayer) && collectLayerEachOtherCondition.BLayer.HasFlag((S5Game.S5GameColliderLayer)eventPhysicCollection.SenderLayer))
			{
				return true;
			}
		}
		return false;
	}
}
