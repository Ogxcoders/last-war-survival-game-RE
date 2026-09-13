using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("刷新UI", "功能")]
public struct RefreshUIAction : IAction
{
	public enum RefreshUIType
	{
		Fire,
		ReloadStart,
		ReloadFinish,
		HpChange
	}

	public RefreshUIType UIState;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
	}
}
