using System.Runtime.InteropServices;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("死亡", "实体")]
public struct DieAction : IAction
{
	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (e is EventTime { Target: var packed })
		{
			if (packed.Unpack(world, out var entity2) && world.IsEntityAliveInternal(entity2))
			{
				FuncEntity.DelEntity(world, entity2);
			}
		}
		else if (action is DieAction && world.IsEntityAliveInternal(entity))
		{
			FuncEntity.DelEntity(world, entity);
		}
	}
}
