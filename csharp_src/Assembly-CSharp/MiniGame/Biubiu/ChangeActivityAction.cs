using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

[TitleAndCategory("设置显隐", "功能")]
public struct ChangeActivityAction : IAction
{
	public int UniqueID;

	public bool IsActive;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		ChangeActivityAction changeActivityAction = (ChangeActivityAction)(object)action;
		if (!FuncUniqueID.TryGetEntityByUniqueID(world, changeActivityAction.UniqueID, out var entity2))
		{
			entity2 = entity;
		}
		if (world.GetPool<ComponentActivitySnapshot>().Has(entity2))
		{
			EcsPool<ComponentActivityChanged> pool = world.GetPool<ComponentActivityChanged>();
			if (!pool.Has(entity2))
			{
				pool.Add(entity2).IsActive = changeActivityAction.IsActive;
			}
			else
			{
				pool.Get(entity2).IsActive = changeActivityAction.IsActive;
			}
		}
	}
}
