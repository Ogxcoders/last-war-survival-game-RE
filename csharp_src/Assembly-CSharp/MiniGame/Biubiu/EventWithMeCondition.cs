using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("与我相关", "实体")]
public struct EventWithMeCondition : ICondition
{
	public EvenLaunchTargetType EvenLaunchTargetType;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		EventWithMeCondition eventWithMeCondition = ((condition is EventWithMeCondition) ? ((EventWithMeCondition)(object)condition) : default(EventWithMeCondition));
		EcsPackedEntity ecsPackedEntity = world.PackEntity(entity);
		if (eventWithMeCondition.EvenLaunchTargetType == EvenLaunchTargetType.Send)
		{
			return e.Sender == ecsPackedEntity;
		}
		if (eventWithMeCondition.EvenLaunchTargetType == EvenLaunchTargetType.Target)
		{
			return e.Target == ecsPackedEntity;
		}
		if (eventWithMeCondition.EvenLaunchTargetType == EvenLaunchTargetType.Other)
		{
			if (!(e.Sender == ecsPackedEntity))
			{
				return e.Target == ecsPackedEntity;
			}
			return true;
		}
		return false;
	}
}
