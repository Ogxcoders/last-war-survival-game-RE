using System.Runtime.InteropServices;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("按钮开关状态改变", "功能")]
public struct TriggerType2StateChangeAction : IAction
{
	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		bool boolData = FuncData.GetBoolData(world, entity, PropertyID.ToggleState);
		bool hasValue;
		ComponentComposeEntityRoot.Item entitiesByComponentEntityRoot = FuncComposeEntity.GetEntitiesByComponentEntityRoot(world, entity, 1, out hasValue);
		if (hasValue)
		{
			EcsPool<ComponentActivityChanged> pool = world.GetPool<ComponentActivityChanged>();
			if (!pool.Has(entitiesByComponentEntityRoot.ID))
			{
				pool.Add(entitiesByComponentEntityRoot.ID).IsActive = !boolData;
			}
			else
			{
				pool.Get(entitiesByComponentEntityRoot.ID).IsActive = boolData;
			}
		}
	}
}
