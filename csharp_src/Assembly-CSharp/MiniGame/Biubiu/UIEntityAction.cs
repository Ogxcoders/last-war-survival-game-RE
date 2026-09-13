using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("表现", "动作")]
public struct UIEntityAction : IAction
{
	public enum ActionType
	{
		Die = 1,
		Fire,
		GameEnd,
		Reload
	}

	public ActionType Type;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
	}
}
