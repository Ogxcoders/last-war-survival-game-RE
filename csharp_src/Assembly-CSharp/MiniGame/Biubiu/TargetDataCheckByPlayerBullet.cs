using System.Runtime.InteropServices;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("玩家是否打完了子弹", "数据")]
public struct TargetDataCheckByPlayerBullet : ICondition
{
	public ICondition Clone()
	{
		return (ICondition)MemberwiseClone();
	}

	public static bool Check(EcsWorld world, int entity, Trigger trigger, ICondition condition, IEvent e)
	{
		int num = 0;
		foreach (int item in world.Filter<ComponentPlayer>().End())
		{
			num += FuncData.GetIntData(world, item, PropertyID.BulletCount);
		}
		return num == 0;
	}
}
