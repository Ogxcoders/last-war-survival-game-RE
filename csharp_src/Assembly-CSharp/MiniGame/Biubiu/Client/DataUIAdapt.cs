using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class DataUIAdapt : IDisposable
{
	private EcsWorld World;

	private int ControllerEntityID;

	private DataResourceLoaderEnv Env;

	private Action<IRender> DirtyAction { get; set; }

	private SharedRuntime Shard => World.GetShared<SharedRuntime>();

	public void AddUISyncEvent(Action<IRender> action)
	{
		DirtyAction = action;
	}

	public void RemoveUISyncEvent(Action<IRender> action)
	{
		DirtyAction = null;
	}

	public void BindEnv(EcsWorld ecsWorld)
	{
		World = ecsWorld;
		Env = ((GameLoader)Shard.ResourceLoader).LoaderEnv;
		FuncEntity.TryGetControllerEntity(ecsWorld, out ControllerEntityID);
		RefreshUIShow();
	}

	public bool TryGetUIResult(out DataUIRenderMessage.UIResult result)
	{
		result = default(DataUIRenderMessage.UIResult);
		if (!Shard.GameOver)
		{
			return false;
		}
		SharedRuntime shared = World.GetShared<SharedRuntime>();
		result.Result = shared.GameResult.Result;
		result.Win = Shard.GameResult.Statistics.WinPlayerID == Shard.InitData.PlayerID;
		result.WinPlayerID = Shard.GameResult.Statistics.WinPlayerID;
		result.FireCount = Shard.GameResult.Statistics.BulletNum;
		result.BattleTimeMills = Shard.GameResult.Statistics.BattleTimeMills;
		result.SelfPlayerID = (int)Shard.InitData.PlayerID;
		result.HeadShot = Shard.GameResult.Statistics.HeadShot;
		if (Shard.GameResult.Replay != null)
		{
			result.ValidationStr = GameBiubiuClient.Serializer.ToJson(Shard.GameResult.Replay);
		}
		return true;
	}

	public float GetLeftTime()
	{
		if (ControllerEntityID < 0)
		{
			return 0f;
		}
		EcsPool<ComponentGunReload> pool = World.GetPool<ComponentGunReload>();
		if (pool.Has(ControllerEntityID))
		{
			return pool.Get(ControllerEntityID).LeftTime.ToFloat();
		}
		return 0f;
	}

	private (int[] curHp, int[] maxHp) GetHpInfo()
	{
		EcsFilter ecsFilter = World.Filter<ComponentPlayer>().Inc<ComponentData>().End();
		int[] array = new int[ecsFilter.GetEntitiesCount()];
		int[] array2 = new int[ecsFilter.GetEntitiesCount()];
		foreach (int item in ecsFilter)
		{
			ref ComponentPlayer reference = ref World.GetPool<ComponentPlayer>().Get(item);
			array[(int)reference.PlayerID] = FuncData.GetIntData(World, item, PropertyID.CurHp);
			array2[(int)reference.PlayerID] = FuncData.GetIntData(World, item, PropertyID.MaxHp);
		}
		return (curHp: array, maxHp: array2);
	}

	public (int, int, float, float) GetGunInfo()
	{
		int item = 0;
		int item2 = 0;
		float num = 0f;
		float item3 = 0f;
		if (ControllerEntityID < 0)
		{
			return (item, item2, num, item3);
		}
		EcsPool<ComponentData> pool = World.GetPool<ComponentData>();
		if (pool.Has(ControllerEntityID))
		{
			ref ComponentData componentData = ref pool.Get(ControllerEntityID);
			item = componentData.GetPropertyValue(PropertyID.BulletCount, 0).AsInt;
			item2 = componentData.GetPropertyValue(PropertyID.BulletMax, 0).AsInt;
			item3 = componentData.GetPropertyValue(PropertyID.BulletReloadTime, 0).AsFloat;
		}
		EcsPool<ComponentControllerClient> pool2 = World.GetPool<ComponentControllerClient>();
		if (pool2.Has(ControllerEntityID))
		{
			DataUIPlayerController controller = pool2.Get(ControllerEntityID).GetController(World, ControllerEntityID);
			if (controller != null)
			{
				num = controller.ClientFireCD;
			}
		}
		num = Mathf.Max(num, 0f);
		return (item, item2, num, item3);
	}

	public void RefreshUIShow()
	{
		var (gunBulletCount, gunBulletMax, gunReloadCurCD, gunReloadMaxCD) = GetGunInfo();
		var (curHp, maxHp) = GetHpInfo();
		FireRender(new DataUIRenderMessage.UIShowInfo
		{
			GunBulletCount = gunBulletCount,
			GunBulletMax = gunBulletMax,
			GunReloadCurCD = gunReloadCurCD,
			GunReloadMaxCD = gunReloadMaxCD,
			GameType = Shard.GameType,
			PlayerID = (int)Shard.InitData.PlayerID,
			CurHp = curHp,
			MaxHp = maxHp
		}, World);
	}

	public void FireRender(IRender render, EcsWorld world)
	{
		DirtyAction?.Invoke(render);
		DataUIRenderLogic.RenderContext context = new DataUIRenderLogic.RenderContext
		{
			go_effec_z_x = Env.GoEffectZX,
			Camera = Env.Camera,
			go_effect_hit = Env.GoEffectHit,
			go_effect_tanshe = Env.GoEffectTanshe,
			go_effect_tanshe_other = Env.GoEffectTansheOther,
			go_effec_explode = Env.GoEffectExplode,
			go_effect_impact = Env.GoEffectImpact,
			go_effect_headshot = Env.GoEffectHeadShot,
			World = world,
			Callack = delegate
			{
			}
		};
		DataUIRenderLogic.Render(render, context);
	}

	public void Dispose()
	{
		World = null;
		ControllerEntityID = 0;
		DirtyAction = null;
	}

	public void OnUpdate()
	{
	}
}
