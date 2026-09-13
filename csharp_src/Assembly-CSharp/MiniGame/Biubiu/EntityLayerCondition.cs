using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("事件发送者在那层", "实体")]
public struct EntityLayerCondition : ICondition
{
	public S5Game.S5GameColliderLayer Layer;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		EntityLayerCondition entityLayerCondition = ((condition is EntityLayerCondition) ? ((EntityLayerCondition)(object)condition) : default(EntityLayerCondition));
		EcsPool<ComponentPhysics> pool = world.GetPool<ComponentPhysics>();
		if (e.Sender.IsValid(world) && pool.Has(e.Sender.Id))
		{
			return ((IBodyLogic)pool.Get(e.Sender.Id).Body.UserData).ILayer == (int)entityLayerCondition.Layer;
		}
		if (e is EventEntityDie eventEntityDie)
		{
			return eventEntityDie.Layer == entityLayerCondition.Layer;
		}
		return false;
	}
}
