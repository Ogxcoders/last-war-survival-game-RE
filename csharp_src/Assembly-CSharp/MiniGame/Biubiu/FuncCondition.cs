using System;
using System.Collections.Generic;
using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

public static class FuncCondition
{
	public static Dictionary<Type, Func<EcsWorld, int, Trigger, ICondition, IEvent, bool>> ConditionDict = new Dictionary<Type, Func<EcsWorld, int, Trigger, ICondition, IEvent, bool>>
	{
		{
			typeof(CollectionLayerCondition),
			CollectionLayerCondition.Check
		},
		{
			typeof(CollectionSideCondition),
			CollectionSideCondition.Check
		},
		{
			typeof(CollectionFixtureTypeCondition),
			CollectionFixtureTypeCondition.Check
		},
		{
			typeof(EntityLayerCondition),
			EntityLayerCondition.Check
		},
		{
			typeof(EventWithMeCondition),
			EventWithMeCondition.Check
		},
		{
			typeof(BulletCountCondition),
			BulletCountCondition.Check
		},
		{
			typeof(DataChangeCondition),
			DataChangeCondition.Check
		},
		{
			typeof(DataCheckCondition),
			DataCheckCondition.Check
		},
		{
			typeof(TargetDataCheckCondition),
			TargetDataCheckCondition.Check
		},
		{
			typeof(TargetDataCheckByPlayerBullet),
			TargetDataCheckByPlayerBullet.Check
		},
		{
			typeof(TargetDataCompareCondition),
			TargetDataCompareCondition.Check
		},
		{
			typeof(CDCondition),
			CDCondition.Check
		},
		{
			typeof(EventTriggerCondition),
			EventTriggerCondition.Check
		},
		{
			typeof(CollectLayerEachOtherCondition),
			CollectLayerEachOtherCondition.Check
		}
	};

	public static bool CheckConditions(EcsWorld world, int entity, Trigger trigger, IEvent e)
	{
		for (int i = 0; i < trigger.Conditions.Count; i++)
		{
			ICondition condition = trigger.Conditions[i];
			if (!CheckCondition(world, entity, trigger, condition, e))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CheckCondition(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (ConditionDict.TryGetValue(condition.GetType(), out var value))
		{
			return value(world, entity, trigger, condition, e);
		}
		return false;
	}

	public static bool Comparable<T>(ConditionOp conditionOp, T intParam1, T intParam2) where T : IComparable<T>, IEquatable<T>
	{
		int num = intParam1.CompareTo(intParam2);
		return conditionOp switch
		{
			ConditionOp.Equal => intParam1.Equals(intParam2), 
			ConditionOp.NotEqual => !intParam1.Equals(intParam2), 
			ConditionOp.GreaterThan => num > 0, 
			ConditionOp.GreaterThanOrEqual => num >= 0, 
			ConditionOp.LessThan => num < 0, 
			ConditionOp.LessThanOrEqual => num <= 0, 
			_ => throw new Exception($"not support {conditionOp}"), 
		};
	}

	public static bool Compare(ConditionOp op, FP lhs, FP rhs)
	{
		return op switch
		{
			ConditionOp.Equal => lhs == rhs, 
			ConditionOp.NotEqual => lhs != rhs, 
			ConditionOp.GreaterThan => lhs > rhs, 
			ConditionOp.GreaterThanOrEqual => lhs >= rhs, 
			ConditionOp.LessThan => lhs < rhs, 
			ConditionOp.LessThanOrEqual => lhs <= rhs, 
			_ => throw new Exception($"not support {op}"), 
		};
	}
}
