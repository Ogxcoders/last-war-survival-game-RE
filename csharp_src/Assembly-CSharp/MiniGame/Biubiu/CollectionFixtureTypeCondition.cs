using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("碰撞到的部位", "碰撞")]
public struct CollectionFixtureTypeCondition : ICondition
{
	public S5Game.FixtureType FixtureType;

	public bool InculdeType;

	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		CollectionFixtureTypeCondition collectionFixtureTypeCondition = ((condition is CollectionFixtureTypeCondition) ? ((CollectionFixtureTypeCondition)(object)condition) : default(CollectionFixtureTypeCondition));
		if (e is EventPhysicCollection eventPhysicCollection)
		{
			bool flag = collectionFixtureTypeCondition.FixtureType.HasFlag((S5Game.FixtureType)eventPhysicCollection.SendFixtureType) | collectionFixtureTypeCondition.FixtureType.HasFlag((S5Game.FixtureType)eventPhysicCollection.TargetFixtureType);
			if (collectionFixtureTypeCondition.InculdeType)
			{
				if (collectionFixtureTypeCondition.FixtureType.HasFlag(S5Game.FixtureType.Head) && flag)
				{
					world.GetShared<SharedRuntime>().GameResult.Statistics.HeadShot = true;
				}
				return flag;
			}
			return !flag;
		}
		return false;
	}
}
