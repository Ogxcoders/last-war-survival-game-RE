using Box2DSharp.Common;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("CD", "功能")]
public struct CDCondition : ICondition
{
	public PropertyID PropertyID;

	public float CD;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		if (condition is CDCondition cDCondition)
		{
			FP y = FuncData.GetFpData(world, entity, cDCondition.PropertyID);
			if (world.GetShared<SharedRuntime>().LogicTime - y > cDCondition.CD)
			{
				FuncData.SetData(world, entity, cDCondition.PropertyID, cDCondition.CD);
				return true;
			}
		}
		return false;
	}
}
