using Box2DSharp.Dynamics;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SystemClientPhysicsSync : IEcsRunSystem, IEcsSystem
{
	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsFilterInject<Inc<ComponentPrefabClient, ComponentPhysics, ComponentPhysicsSync>> _filterSync;

	private readonly EcsPoolInject<ComponentPrefabClient> _poolPrefab;

	private readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	private readonly EcsPoolInject<ComponentMovablePrediction> _poolPrediction;

	public void Run(IEcsSystems systems)
	{
		if (_shared.Value.GameState != EGameWorldState.Running)
		{
			return;
		}
		GameLoader gameLoader = (GameLoader)_shared.Value.ResourceLoader;
		float num = 1f / gameLoader.LoaderEnv.SizeToUnit;
		foreach (int item in _filterSync.Value)
		{
			GameObject prefab = _poolPrefab.Value.Get(item).GetPrefab();
			if (prefab == null)
			{
				continue;
			}
			Body body = _poolPhysics.Value.Get(item).Body;
			if (!(body.UserData is S5Game.BodyLogic bodyLogic))
			{
				continue;
			}
			_ = ((GameLoader)_shared.Value.ResourceLoader).LoaderEnv;
			if (bodyLogic.Layer == S5Game.S5GameColliderLayer.Bullet)
			{
				if (_poolPrediction.Value.Has(item))
				{
					ref ComponentMovablePrediction reference = ref _poolPrediction.Value.Get(item);
					Transform transform = prefab.transform;
					transform.localPosition = new Vector3(reference.Position.X, reference.Position.Y, reference.Position.Z) * num;
					if (reference.Forward.LengthSquared() > 0f)
					{
						transform.localRotation = Quaternion.LookRotation(new Vector3(reference.Forward.X, reference.Forward.Y, reference.Forward.Z), Vector3.up) * Quaternion.Euler(new Vector3(0f, -90f, 0f));
					}
				}
				else
				{
					Transform transform2 = prefab.transform;
					Quaternion quaternion = Quaternion.LookRotation(body.LinearVelocity.ToUnityVector3(), Vector3.up);
					transform2.localRotation = quaternion * Quaternion.Euler(new Vector3(0f, -90f, 0f));
					transform2.localPosition = body.GetPosition().ToUnityVector3() * num;
				}
			}
			else if (_poolPrediction.Value.Has(item))
			{
				ref ComponentMovablePrediction reference2 = ref _poolPrediction.Value.Get(item);
				prefab.transform.localPosition = new Vector3(reference2.Position.X, reference2.Position.Y, reference2.Position.Z) * num;
			}
			else
			{
				prefab.transform.localPosition = body.GetPosition().ToUnityVector3() * num;
			}
		}
	}
}
