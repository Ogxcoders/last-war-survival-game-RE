using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

[TitleAndCategory("显示隐藏轮询", "功能")]
public struct ToggleActivityAction : IAction
{
	public int UniqueID;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!FuncUniqueID.TryGetEntityByUniqueID(world, ((ToggleActivityAction)(object)action).UniqueID, out var entity2))
		{
			entity2 = entity;
		}
		EcsPool<ComponentActivitySnapshot> pool = world.GetPool<ComponentActivitySnapshot>();
		if (pool.Has(entity2))
		{
			ref ComponentActivitySnapshot reference = ref pool.Get(entity2);
			EcsPool<ComponentActivityChanged> pool2 = world.GetPool<ComponentActivityChanged>();
			if (!pool2.Has(entity2))
			{
				pool2.Add(entity2).IsActive = !reference.IsActive;
				return;
			}
			ref ComponentActivityChanged reference2 = ref pool2.Get(entity2);
			reference2.IsActive = !reference2.IsActive;
		}
	}
}
