using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SystemClientPrefabMovablePrediction : IEcsRunSystem, IEcsSystem
{
	protected readonly EcsSharedInject<SharedRuntime> _shared;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentBullet>, Exc<ComponentMovablePrediction>> _filterInit;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentBullet>> _filterBullet;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentVelocity>> _filterVelocity;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentPathVelocity>> _filterPathVelocity;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentEnemy>> _filterEneny;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentDecoration>> _filterDecoration;

	protected readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPrefabClient, ComponentPosition, ComponentPlayer>> _filterPlayer;

	protected readonly EcsFilterInject<Inc<ComponentMovablePrediction, ComponentPhysics, ComponentPrefabClient, ComponentPosition>> _filterPrediction;

	protected readonly EcsPoolInject<ComponentStop> _poolStop;

	protected readonly EcsPoolInject<ComponentMovablePrediction> _poolPrediction;

	protected readonly EcsPoolInject<ComponentPhysicsWorld> _poolPhysicsWorld;

	protected readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	protected readonly EcsPoolInject<ComponentDecoration> _poolDecoration;

	public void Run(IEcsSystems systems)
	{
		RunPrediction(systems);
	}

	private void RunPrediction(IEcsSystems systems)
	{
		int predictionStep = GetPredictionStep();
		float predictionTime = GetPredictionTime();
		foreach (int item in _filterBullet.Value)
		{
			Prediction(item, predictionStep, predictionTime);
		}
		foreach (int item2 in _filterVelocity.Value)
		{
			Prediction(item2, predictionStep, predictionTime);
		}
		foreach (int item3 in _filterPathVelocity.Value)
		{
			Prediction(item3, predictionStep, predictionTime);
		}
		foreach (int item4 in _filterPlayer.Value)
		{
			Prediction(item4, predictionStep, predictionTime);
		}
		foreach (int item5 in _filterEneny.Value)
		{
			Prediction(item5, predictionStep, predictionTime);
		}
		foreach (int item6 in _filterDecoration.Value)
		{
			ref ComponentDecoration reference = ref _poolDecoration.Value.Get(item6);
			if (reference.DecorationType == DecorationType.Bomb || reference.DecorationType == DecorationType.WoodBarrel)
			{
				Prediction(item6, predictionStep, predictionTime);
			}
		}
	}

	protected virtual int GetPredictionStep()
	{
		return FuncPhysics.GetPhysicsWorld(_shared.Value, _poolPhysicsWorld.Value).PhysicsFrame;
	}

	protected virtual float GetPredictionTime()
	{
		return 0.033f;
	}

	protected virtual bool Prediction(int entity, int step, float time)
	{
		ComponentPhysics componentPhysics = _poolPhysics.Value.Get(entity);
		FloatVector3 pos = componentPhysics.Body.GetPosition().ToCSharpVector3();
		FloatVector3 vec = componentPhysics.Body.LinearVelocity.ToCSharpVector3();
		return Prediction(entity, step, time, pos, vec);
	}

	protected bool Prediction(int entity, int step, float time, FloatVector3 pos, FloatVector3 vec)
	{
		EcsPool<ComponentMovablePrediction> value = _poolPrediction.Value;
		bool flag = !value.Has(entity);
		float asFloat = _shared.Value.PhysicsTickDelta.AsFloat;
		ref ComponentMovablePrediction orAdd = ref value.GetOrAdd(entity);
		if (flag || orAdd.FrameSync < step)
		{
			orAdd.TimeSimulate = 0f;
			if (_shared.Value.GameType == EGameType.PveClient)
			{
				orAdd.TimePrediction = (float)(step - orAdd.FrameSync) * asFloat;
				orAdd.Position = (flag ? pos : orAdd.Position);
			}
			else
			{
				orAdd.TimePrediction = 1f;
				orAdd.Position = (flag ? pos : orAdd.Position);
			}
			orAdd.PositionSyn = pos;
			orAdd.PositionPrediction = pos + vec * orAdd.TimePrediction;
			orAdd.Forward = vec;
			orAdd.FrameSync = step;
		}
		if (!flag)
		{
			UpdatePrediction(time, ref orAdd);
		}
		return flag;
	}

	private void UpdatePrediction(float time, ref ComponentMovablePrediction prediction)
	{
		prediction.TimeSimulate += Time.deltaTime;
		float factor = Mathf.Min(Mathf.Min(prediction.TimeSimulate, time) / prediction.TimePrediction, 1f);
		FloatVector3 right = prediction.PositionSyn.Lerp(prediction.PositionPrediction, factor);
		prediction.Position = prediction.Position.Lerp(right, 0.5f);
	}
}
