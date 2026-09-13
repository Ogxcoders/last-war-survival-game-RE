using Leopotam.EcsLite;

namespace MiniGame.Biubiu;

[TitleAndCategory("表现", "播放特效")]
public struct EffectAction : IAction
{
	public enum EffectType
	{
		BulletWall = 1,
		Hit,
		Bomb,
		BulletBullet,
		HeadHit
	}

	public EffectType EEffectType;

	public IAction Clone()
	{
		return (IAction)MemberwiseClone();
	}

	public static void Execute(EcsWorld world, int entity, IAction action, IEvent e)
	{
	}
}
