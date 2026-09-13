using System.Runtime.InteropServices;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("游戏结束", "功能")]
public struct GameOverAction : IAction
{
	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		SharedRuntime shared = world.GetShared<SharedRuntime>();
		if (!shared.GameOver)
		{
			shared.CanCheckGameOver = true;
		}
	}
}
