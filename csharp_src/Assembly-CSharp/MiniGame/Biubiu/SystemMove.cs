using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SystemMove : IEcsRunSystem, IEcsSystem
{
	private readonly EcsWorldInject _world;

	private readonly EcsSharedInject<SharedRuntime> _shared;

	private readonly EcsFilterInject<Inc<ComponentPhysics, ComponentVelocity>, Exc<ComponentStop>> _filterVelocity;

	private readonly EcsFilterInject<Inc<ComponentPhysics, ComponentPathVelocity>, Exc<ComponentStop>> _filterPathVelocity;

	private readonly EcsFilterInject<Inc<ComponentVelocityTarget>, Exc<ComponentPathVelocity>> _filterDeleteTarget;

	private readonly EcsPoolInject<ComponentPhysics> _poolPhysics;

	private readonly EcsPoolInject<ComponentVelocity> _poolVelocity;

	private readonly EcsPoolInject<ComponentVelocityTarget> _poolVelocityTarget;

	private readonly EcsPoolInject<ComponentPathVelocity> _poolPathVelocity;

	private readonly EcsPoolInject<ComponentPosition> _poolPosition;

	private readonly EcsPoolInject<ComponentData> _poolData;

	public void Run(IEcsSystems systems)
	{
		ClearInvalidTargets();
		RunVelocity(systems);
		RunPathVelocity(systems);
	}

	private void ClearInvalidTargets()
	{
		foreach (int item in _filterDeleteTarget.Value)
		{
			_poolVelocityTarget.Value.Del(item);
		}
	}

	private FVector2 GetNextDir(int entity, out FVector2 targetPos)
	{
		ref ComponentPathVelocity reference = ref _poolPathVelocity.Value.Get(entity);
		targetPos = reference.Paths[reference.MoveIndex];
		int num = reference.MoveIndex - 1;
		num = ((num < 0) ? (reference.Paths.Length - 1) : num);
		FVector2 fVector = reference.Paths[num];
		return (targetPos - fVector).normalized;
	}

	private void RunPathVelocity(IEcsSystems systems)
	{
		int logicTickCount = _shared.Value.LogicTickCount;
		FP y = 1 / _shared.Value.LogicTickDelta;
		foreach (int item in _filterPathVelocity.Value)
		{
			ref ComponentPhysics reference = ref _poolPhysics.Value.Get(item);
			ref ComponentPathVelocity reference2 = ref _poolPathVelocity.Value.Get(item);
			if (reference2.MoveLength <= 0 && reference2.MoveLength != -1)
			{
				reference.Body.SetLinearVelocity(FVector2.Zero);
				ref ComponentVelocityTarget orAdd = ref _poolVelocityTarget.Value.GetOrAdd(item);
				orAdd.TargetFrame = logicTickCount;
				orAdd.TargetPosition = reference.Body.GetPosition();
				continue;
			}
			ref ComponentData componentData = ref _poolData.Value.Get(item);
			FVector2 position = reference2.Paths[reference2.MoveIndex];
			FVector2 position2 = reference.Body.GetPosition();
			int num = reference2.MoveIndex - 1;
			num = ((num < 0) ? (reference2.Paths.Length - 1) : num);
			FVector2 fVector = reference2.Paths[num];
			FVector2 normalized = (position - fVector).normalized;
			FVector2 fVector2 = position2 + normalized * componentData.GetPropertyValue(PropertyID.MoveSpeed) * _shared.Value.PhysicsTickDelta;
			FVector2 value = fVector2 - position2;
			FVector2 value2 = fVector2 - position;
			FP propertyValue = componentData.GetPropertyValue(PropertyID.MoveSpeed);
			if (FVector2.Dot(value, value2) > 0)
			{
				int num2 = reference2.MoveIndex + 1;
				num2 = ((num2 <= reference2.Paths.Length - 1) ? num2 : 0);
				reference2.MoveIndex = num2;
				if (reference2.MoveLength != -1)
				{
					reference2.MoveLength--;
				}
				_shared.Value.FrameSyncIsNeeded = true;
				FVector2 fVector3 = position - reference.Body.GetPosition();
				FVector2 targetPos;
				FVector2 nextDir = GetNextDir(item, out targetPos);
				reference.Body.SetTransform(in position, reference.Body.GetAngle());
				reference.Body.SetLinearVelocity(nextDir * propertyValue);
				ref ComponentVelocityTarget orAdd2 = ref _poolVelocityTarget.Value.GetOrAdd(item);
				orAdd2.TargetFrame = logicTickCount + FP.Floor((targetPos - position2).Length() * y / propertyValue).AsInt;
				orAdd2.TargetPosition = targetPos;
				if (!(reference.Body.UserData is IPlatformLogic { OnPlatformIDs: not null } platformLogic))
				{
					continue;
				}
				for (int i = 0; i < platformLogic.OnPlatformIDs.Count; i++)
				{
					int id = platformLogic.OnPlatformIDs[i];
					if (FuncUniqueID.TryGetEntityByUniqueID(_world.Value, id, out var entity) && _poolPhysics.Value.Has(entity))
					{
						ref ComponentPhysics reference3 = ref _poolPhysics.Value.Get(entity);
						reference3.Body.SetTransform(reference3.Body.GetPosition() + fVector3, reference3.Body.GetAngle());
						reference3.Body.SetLinearVelocity(reference.Body.LinearVelocity);
					}
				}
			}
			else
			{
				reference.Body.SetLinearVelocity(normalized * propertyValue);
				if (propertyValue <= FP.EN3)
				{
					_poolVelocityTarget.Value.Del(item);
					break;
				}
				ref ComponentVelocityTarget orAdd3 = ref _poolVelocityTarget.Value.GetOrAdd(item);
				orAdd3.TargetFrame = logicTickCount + FP.Floor((position - position2).Length() * y / propertyValue).AsInt;
				orAdd3.TargetPosition = position;
			}
		}
	}

	private void RunVelocity(IEcsSystems systems)
	{
		foreach (int item in _filterVelocity.Value)
		{
			ref ComponentVelocity reference = ref _poolVelocity.Value.Get(item);
			ref ComponentData componentData = ref _poolData.Value.Get(item);
			_poolPhysics.Value.Get(item).Body.SetLinearVelocity(componentData.GetPropertyValue(PropertyID.MoveSpeed) * reference.Direction.normalized);
		}
	}
}
