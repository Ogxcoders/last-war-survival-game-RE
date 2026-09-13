using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public static class FuncAction
{
	public static Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>> ActionDict = new Dictionary<Type, Action<EcsWorld, int, IAction, IEvent>>
	{
		{
			typeof(EffectAction),
			EffectAction.Execute
		},
		{
			typeof(UIEntityAction),
			UIEntityAction.Execute
		},
		{
			typeof(RefreshUIAction),
			RefreshUIAction.Execute
		},
		{
			typeof(GameWaitAction),
			GameWaitAction.Execute
		},
		{
			typeof(GameOverAction),
			GameOverAction.Execute
		},
		{
			typeof(SetDataAction),
			SetDataAction.Execute
		},
		{
			typeof(DieAction),
			DieAction.Execute
		},
		{
			typeof(BombAction),
			BombAction.Execute
		},
		{
			typeof(StopContactAction),
			StopContactAction.Execute
		},
		{
			typeof(ChangeActivityAction),
			ChangeActivityAction.Execute
		},
		{
			typeof(ToggleActivityAction),
			ToggleActivityAction.Execute
		},
		{
			typeof(ToggleAction),
			ToggleAction.Execute
		},
		{
			typeof(TriggerStateChangeAction),
			TriggerStateChangeAction.Execute
		},
		{
			typeof(EventTriggerBroadAction),
			EventTriggerBroadAction.Execute
		},
		{
			typeof(TriggerType2StateChangeAction),
			TriggerType2StateChangeAction.Execute
		}
	};

	public static void DoActions(EcsWorld world, int entity, Trigger trigger, IEvent e)
	{
		for (int i = 0; i < trigger.Actions.Count; i++)
		{
			IAction action = trigger.Actions[i];
			DoAction(world, entity, trigger, action, e);
		}
	}

	public static void DoAction(EcsWorld world, int entity, Trigger trigger, IAction action, IEvent e)
	{
		if (ActionDict.TryGetValue(action.GetType(), out var value))
		{
			value(world, entity, action, e);
		}
	}
}
