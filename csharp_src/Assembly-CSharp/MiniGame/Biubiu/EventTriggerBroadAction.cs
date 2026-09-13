using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("广播自定义触发", "广播")]
public struct EventTriggerBroadAction : IAction
{
	public EventTriggerType Type;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (action is EventTriggerBroadAction eventTriggerBroadAction)
		{
			FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(world), new EventTrigger(world.PackEntity(entity), EcsPackedEntity.Invalid, eventTriggerBroadAction.Type));
		}
	}
}
