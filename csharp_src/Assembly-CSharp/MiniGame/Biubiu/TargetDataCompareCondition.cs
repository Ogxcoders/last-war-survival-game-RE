using Box2DSharp.Common;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

[TitleAndCategory("对比目标数据", "数据")]
public struct TargetDataCompareCondition : ICondition
{
	public int LUniqueID;

	public int RUniqueID;

	public PropertyID LPropertyID;

	public PropertyID RPropertyID;

	public ConditionOp ConditionOp;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is TargetDataCompareCondition targetDataCompareCondition)
		{
			int entity2 = entity;
			int entity3 = entity;
			if (targetDataCompareCondition.LUniqueID >= 0 && !FuncUniqueID.TryGetEntityByUniqueID(world, targetDataCompareCondition.LUniqueID, out entity2))
			{
				return false;
			}
			if (targetDataCompareCondition.RUniqueID >= 0 && !FuncUniqueID.TryGetEntityByUniqueID(world, targetDataCompareCondition.RUniqueID, out entity3))
			{
				return false;
			}
			FP fpData = FuncData.GetFpData(world, entity2, targetDataCompareCondition.LPropertyID, 0);
			FP fpData2 = FuncData.GetFpData(world, entity3, targetDataCompareCondition.RPropertyID, 0);
			return FuncCondition.Comparable(targetDataCompareCondition.ConditionOp, fpData, fpData2);
		}
		return false;
	}
}
