using System.Runtime.InteropServices;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[TitleAndCategory("禁用碰撞", "碰撞")]
public struct StopContactAction : IAction
{
	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
		IEvent obj;
		if ((obj = e) is EventPhysicCollection)
		{
			_ = (EventPhysicCollection)(object)obj;
		}
	}
}
