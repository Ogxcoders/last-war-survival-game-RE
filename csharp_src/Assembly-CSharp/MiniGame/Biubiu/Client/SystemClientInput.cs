using Box2DSharp.Common;
using Joker;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class SystemClientInput : IEcsRunSystem, IEcsSystem
{
	protected readonly EcsWorldInject _world;

	protected readonly EcsSharedInject<SharedRuntime> _shared;

	protected readonly EcsFilterInject<Inc<ComponentPlayer, ComponentControllerClient>> _filterInput;

	protected readonly EcsPoolInject<ComponentControllerClient> _poolInput;

	private readonly EcsPoolInject<ComponentData> _poolData;

	private readonly EcsFilterInject<Inc<ComponentPlayer>> _filterPlayer;

	public void Run(IEcsSystems systems)
	{
		if (_shared.Value.GameState != EGameWorldState.Running)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput GameState is not Running");
			}
			return;
		}
		if (_shared.Value.GameType == EGameType.PvpClient && _shared.Value.LogicTime < _shared.Value.CanInputTime)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput is not CanInputTime");
			}
			return;
		}
		GameLoader gameLoader = (GameLoader)_shared.Value.ResourceLoader;
		if (gameLoader.LoaderEnv.GameGo == null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput GameGo is null");
			}
			return;
		}
		if (!gameLoader.LoaderEnv.GameGo.TryGetComponent<RectTransform>(out var component))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput GameGo RectTransform is null");
			}
			return;
		}
		if (!FuncEntity.TryGetControllerEntity(_world.Value, out var entity))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput controllerEntity is null");
			}
			return;
		}
		DataUIPlayerController controller = _poolInput.Value.Get(entity).GetController(_world.Value, entity);
		if (controller == null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput controller is null");
			}
			return;
		}
		controller.TargetChangeDt = 0f;
		if (!RectTransformUtility.RectangleContainsScreenPoint(component, Input.mousePosition, controller.Camera))
		{
			if (Input.GetMouseButtonDown(0))
			{
				Log.Info("[BiuBiu] SystemClientInput RectangleContainsScreenPoint is not in Area");
			}
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			controller.Downing = true;
			controller.DowningTime = Time.time;
		}
		bool flag = false;
		if (Input.GetMouseButtonUp(0))
		{
			flag = controller.Drag;
			controller.Downing = false;
		}
		if (controller.Drag)
		{
			UpdatePlayerPose(controller);
		}
		if (!flag)
		{
			return;
		}
		foreach (int item in _filterInput.Value)
		{
			_poolInput.Value.Get(item);
			if (!(controller == null) && item == entity)
			{
				FireBullet(controller, item);
			}
		}
	}

	private void UpdatePlayerPose(DataUIPlayerController controller)
	{
		if (_shared.Value.GameState == EGameWorldState.Running)
		{
			controller.GetInputPos(out var chestPos, out var inputPos);
			int num = ((chestPos.x - controller.transform.position.x > 0f) ? 180 : 0);
			controller.transform.rotation = Quaternion.Euler(0f, num, 0f);
			controller.TargetChangeDt = Vector3.Angle(inputPos - controller.transform.position, controller._target.position - controller.transform.position);
			controller._target.position = inputPos;
			FuncUI.FireRender(default(DataUIRenderMessage.UIZhunXin), _world.Value);
		}
	}

	private void FireBullet(DataUIPlayerController controller, int entity)
	{
		if (controller.ClientFireCD > 0f)
		{
			return;
		}
		EcsPool<ComponentPrefabClient> pool = _world.Value.GetPool<ComponentPrefabClient>();
		if (!pool.Has(entity))
		{
			Debug.LogError($"{entity}:丢失位置");
			return;
		}
		if (_shared.Value.GameType == EGameType.PvpClient)
		{
			EcsPool<ComponentData> pool2 = _world.Value.GetPool<ComponentData>();
			if (pool2.Has(entity) && pool2.Get(entity).GetPropertyValue(PropertyID.BulletCount).AsInt <= 0 && GameEntry.Lua != null)
			{
				UIUtils.ShowTips("season_s5_activity_1200045_desc92", 3f);
				return;
			}
		}
		GameLoader gameLoader = _shared.Value.ResourceLoader as GameLoader;
		float sizeToUnit = gameLoader.LoaderEnv.SizeToUnit;
		Vector3 position = pool.Get(entity).GetPrefab().transform.position;
		FVector2 fVector = gameLoader.LoaderEnv.CalWorldToPlayerLocalPos(position).ToFVector2() * sizeToUnit;
		Vector3 position2 = controller._bulletSpawnPoint.position;
		FVector2 fVector2 = gameLoader.LoaderEnv.CalWorldToPlayerLocalPos(position2).ToFVector2() * sizeToUnit;
		int intData = FuncData.GetIntData(_world.Value, entity, PropertyID.ConfigID);
		PlayerConfig playerConfig = gameLoader.LoadConfig<PlayerConfig>(intData);
		FVector2 fVector3 = fVector2 - fVector;
		if (!(fVector3.X >= playerConfig.LowerBound.X) || !(fVector3.X <= playerConfig.UpperBound.X) || !(fVector3.Y >= playerConfig.LowerBound.Y) || !(fVector3.Y <= playerConfig.UpperBound.Y))
		{
			Debug.LogError($"{entity}:开枪点 异常");
			return;
		}
		Vector3 position3 = controller._target.position;
		FVector2 normalized = (gameLoader.LoaderEnv.CalWorldToPlayerLocalPos(position3).ToFVector2() * sizeToUnit - fVector2).normalized;
		controller.CanToCD = false;
		CreateBulletImpl(entity, fVector, fVector2, normalized, 0);
	}

	protected virtual void CreateBulletImpl(int entity, FVector2 bodyPos, FVector2 bulletPos, FVector2 dir, int id)
	{
		ServiceCommand commands = _shared.Value.Commands;
		CommandCreateBullet commandCreateBullet = new CommandCreateBullet
		{
			EntityID = entity,
			FrameIndex = _shared.Value.LogicTickCount,
			TypeID = 0,
			BodyPosition = bodyPos,
			Position = bulletPos,
			Direction = dir,
			BulletID = 0
		};
		commands.QueueEvent(commandCreateBullet);
	}
}
