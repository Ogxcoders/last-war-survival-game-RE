using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("判断发生了什么", "触发")]
public struct EventTriggerCondition : ICondition
{
	public EventTriggerType Type;

	public bool IsMe;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is EventTriggerCondition eventTriggerCondition)
		{
			EcsPackedEntity ecsPackedEntity = world.PackEntity(entity);
			if (eventTriggerCondition.IsMe && ecsPackedEntity != e.Sender)
			{
				return false;
			}
			if (e is EventTrigger eventTrigger)
			{
				return eventTriggerCondition.Type == eventTrigger.Type;
			}
		}
		return false;
	}
}
