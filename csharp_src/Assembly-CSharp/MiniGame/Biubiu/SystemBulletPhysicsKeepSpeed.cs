using Box2DSharp.Common;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Biubiu;

public class SystemBulletPhysicsKeepSpeed : IEcsRunSystem, IEcsSystem
{
	private readonly EcsFilterInject<Inc<ComponentBullet, ComponentPhysics>> _filterBullet;

	public void Run(IEcsSystems systems)
	{
		EcsPool<ComponentBullet> inc = _filterBullet.Pools.Inc1;
		EcsPool<ComponentPhysics> inc2 = _filterBullet.Pools.Inc2;
		foreach (int item in _filterBullet.Value)
		{
			ref ComponentPhysics reference = ref inc2.Get(item);
			ref ComponentBullet reference2 = ref inc.Get(item);
			if (reference.Body != null)
			{
				FVector2 linearVelocity = reference.Body.LinearVelocity;
				FP fP = linearVelocity.LengthSquared() - reference2.Speed * reference2.Speed;
				if (fP > FP.EN2 || fP < -FP.EN2)
				{
					reference.Body.SetLinearVelocity(linearVelocity.normalized * reference2.Speed);
				}
				FP angle = FMath.Atan2(reference.Body.LinearVelocity.Y, reference.Body.LinearVelocity.X);
				reference.Body.SetTransform(reference.Body.GetPosition(), angle);
			}
		}
	}
}
