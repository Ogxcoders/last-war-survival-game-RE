using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class SystemClientPrefabMovablePredictionSimulate : SystemClientPrefabMovablePrediction
{
	private readonly EcsPoolInject<ComponentPhysicsWorldSimulate> _poolPhysicsWorldSimulate;

	private readonly EcsPoolInject<ComponentPhysicsSimulate> _poolPhysicsSimulate;

	protected override int GetPredictionStep()
	{
		_shared.Value.PhysicsWorld.Unpack(_poolPhysicsSimulate.Value.GetWorld(), out var entity);
		return _poolPhysicsWorldSimulate.Value.Get(entity).SimulatePhysicsFrame;
	}

	protected override bool Prediction(int entity, int step, float time)
	{
		ComponentPhysicsSimulate componentPhysicsSimulate = _poolPhysicsSimulate.Value.Get(entity);
		FloatVector3 pos = componentPhysicsSimulate.Body.GetPosition().ToCSharpVector3();
		FloatVector3 vec = componentPhysicsSimulate.Body.LinearVelocity.ToCSharpVector3();
		return Prediction(entity, step, time, pos, vec);
	}
}
