using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

[TitleAndCategory("开关事件", "功能")]
public struct ToggleAction : IAction
{
	public int UniqueID;

	public ToggleLogic OpenToggleLogic;

	public ToggleLogic CloseToggleLogic;

	public int MoveLength;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		if (!(action is ToggleAction toggleAction))
		{
			return;
		}
		bool flag = false;
		if (e is EventTrigger eventTrigger)
		{
			flag = eventTrigger.TriggerSource == 1;
		}
		ToggleLogic toggleLogic = (FuncData.GetBoolData(world, entity, PropertyID.ToggleState) ? toggleAction.OpenToggleLogic : toggleAction.CloseToggleLogic);
		FuncUniqueID.TryGetEntityByUniqueID(world, toggleAction.UniqueID, out var entity2);
		switch (toggleLogic)
		{
		case ToggleLogic.Hide:
		{
			EcsPool<ComponentActivityChanged> pool3 = world.GetPool<ComponentActivityChanged>();
			(pool3.Has(entity2) ? ref pool3.Get(entity2) : ref pool3.Add(entity2)).IsActive = false;
			break;
		}
		case ToggleLogic.Active:
		{
			EcsPool<ComponentActivityChanged> pool4 = world.GetPool<ComponentActivityChanged>();
			(pool4.Has(entity2) ? ref pool4.Get(entity2) : ref pool4.Add(entity2)).IsActive = true;
			break;
		}
		case ToggleLogic.Bomb:
			if (flag)
			{
				return;
			}
			FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(world), new EventTrigger(world.PackEntity(entity2), EcsPackedEntity.Invalid, EventTriggerType.Bomb));
			break;
		case ToggleLogic.Stop:
		{
			EcsPool<ComponentStop> pool5 = world.GetPool<ComponentStop>();
			if (!pool5.Has(entity2))
			{
				pool5.Add(entity2);
			}
			EcsPool<ComponentPhysics> pool6 = world.GetPool<ComponentPhysics>();
			if (pool6.Has(entity2))
			{
				pool6.Get(entity2).Body.SetLinearVelocity(FVector2.Zero);
			}
			break;
		}
		case ToggleLogic.Move:
		{
			EcsPool<ComponentStop> pool = world.GetPool<ComponentStop>();
			if (pool.Has(entity2))
			{
				pool.Del(entity2);
			}
			EcsPool<ComponentPathVelocity> pool2 = world.GetPool<ComponentPathVelocity>();
			if (pool2.Has(entity2))
			{
				pool2.Get(entity2).MoveLength = toggleAction.MoveLength;
			}
			break;
		}
		}
		if (!flag)
		{
			int intData = FuncData.GetIntData(world, entity, PropertyID.TriggerCount);
			if (intData != -1)
			{
				intData--;
				FuncData.SetData(world, entity, PropertyID.TriggerCount, intData);
			}
		}
	}
}
