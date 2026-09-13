using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("爆炸", "实体")]
public struct BombAction : IAction
{
	public FP Radius;

	public S5Game.S5GameColliderLayer Layer;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is BombAction bombAction) || !e.Sender.Unpack(world, out var entity2))
		{
			return;
		}
		EcsFilter ecsFilter = world.Filter<ComponentData>().End();
		EcsPool<ComponentPhysics> pool = world.GetPool<ComponentPhysics>();
		ref ComponentPhysics reference = ref pool.Get(entity2);
		foreach (int item in ecsFilter)
		{
			if (!pool.Has(item))
			{
				continue;
			}
			ref ComponentPhysics reference2 = ref pool.Get(item);
			if (FVector2.Distance(reference2.Body.GetPosition(), reference.Body.GetPosition()) < bombAction.Radius)
			{
				IBodyLogic bodyLogic = reference2.Body.UserData as IBodyLogic;
				if (bombAction.Layer.HasFlag((S5Game.S5GameColliderLayer)bodyLogic.ILayer))
				{
					FuncEntity.DelEntity(world, item);
				}
			}
		}
	}
}
