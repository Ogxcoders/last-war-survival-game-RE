using System;
using GameKit.Base;

public class GameLODManager
{
	private SceneLODManualUpdate _manualUpdate;

	public GameLODManager()
	{
		Initialize();
	}

	public void Initialize()
	{
		try
		{
			if (_manualUpdate == null)
			{
				_manualUpdate = new SceneLODManualUpdate();
				SingletonBehaviour<SceneLODManager>.Instance.AddNode(_manualUpdate);
			}
			SingletonBehaviour<SceneLODManager>.Instance.ClearAllStrategies();
			SingletonBehaviour<SceneLODManager>.Instance.SetupLODRange(0, 4);
			InitializeEffectLOD();
			InitializeSquadLOD();
			InitializeCPULOD();
		}
		catch (Exception)
		{
		}
	}

	public void InitializeEffectLOD()
	{
		WorldCameraLODStrategy worldCameraLODStrategy = new WorldCameraLODStrategy();
		worldCameraLODStrategy.Configure(new int[3] { 30, 50, 70 });
		PVESceneLODStrategy strategy = new PVESceneLODStrategy();
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect, worldCameraLODStrategy);
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect, strategy);
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Firework, new GearQualityStrategy());
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect_WorldCommon, new CostLODStrategy());
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect_WorldCommon, new CountLODStrategy());
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect_WorldTroop, new CostLODStrategy());
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect_WorldTroop, new CountLODStrategy());
	}

	public void InitializeSquadLOD()
	{
	}

	public void InitializeCPULOD()
	{
		SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.CPU, new CPULODStrategy());
	}

	public void ConfigureLODRange(int min, int max)
	{
		SingletonBehaviour<SceneLODManager>.Instance.SetupLODRange(min, max);
	}

	public void ConfigureEffectLOD(int[] thresholds)
	{
		if (SingletonBehaviour<SceneLODManager>.Instance.GetStrategy(LODType.Effect, typeof(WorldCameraLODStrategy)) is WorldCameraLODStrategy worldCameraLODStrategy)
		{
			worldCameraLODStrategy.Configure(thresholds);
		}
	}

	public void ConfigureEffectWorldCommonLOD(float[] costThresholds, int[] countThresholds)
	{
		(SingletonBehaviour<SceneLODManager>.Instance.GetStrategy(LODType.Effect_WorldCommon, typeof(CostLODStrategy)) as CostLODStrategy)?.Configure(costThresholds);
		(SingletonBehaviour<SceneLODManager>.Instance.GetStrategy(LODType.Effect_WorldCommon, typeof(CountLODStrategy)) as CountLODStrategy)?.Configure(countThresholds);
	}

	public void ConfigureEffectWorldTroopLOD(float[] costThresholds, int[] countThresholds)
	{
		(SingletonBehaviour<SceneLODManager>.Instance.GetStrategy(LODType.Effect_WorldTroop, typeof(CostLODStrategy)) as CostLODStrategy)?.Configure(costThresholds);
		(SingletonBehaviour<SceneLODManager>.Instance.GetStrategy(LODType.Effect_WorldTroop, typeof(CountLODStrategy)) as CountLODStrategy)?.Configure(countThresholds);
	}

	public void ConfigureSkinnedMeshLOD(int[] thresholds)
	{
		if (SingletonBehaviour<SceneLODManager>.Instance.GetStrategy(LODType.Squad, typeof(WorldCameraLODStrategy)) is WorldCameraLODStrategy worldCameraLODStrategy)
		{
			worldCameraLODStrategy.Configure(null, thresholds);
		}
	}

	public void SetupEffectLODStaticCost(float[] staticCosts)
	{
	}

	public void SetupEffectLODDynamicCost(float upThresholds, float downThresholds, float hysteresisTime = 5f)
	{
	}

	public void Shutdown()
	{
		SingletonBehaviour<SceneLODManager>.Instance.ClearAllStrategies();
		if (_manualUpdate != null)
		{
			SingletonBehaviour<SceneLODManager>.Instance.RemoveNode(_manualUpdate);
			_manualUpdate = null;
		}
	}
}
