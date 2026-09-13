using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SystemGameOver : IEcsRunSystem, IEcsSystem
{
	private EcsWorldInject world;

	public void Run(IEcsSystems systems)
	{
		SharedRuntime shared = world.Value.GetShared<SharedRuntime>();
		if (!shared.CanCheckGameOver)
		{
			return;
		}
		if (shared.GameType == EGameType.PvpClient)
		{
			shared.ClientGameOver = true;
			return;
		}
		EcsFilter ecsFilter = world.Value.Filter<ComponentPlayer>().End();
		int entitiesCount = ecsFilter.GetEntitiesCount();
		int num = -1;
		EcsFilter ecsFilter2 = world.Value.Filter<ComponentEnemy>().End();
		bool flag = true;
		foreach (int item in ecsFilter2)
		{
			flag = flag && FuncData.GetBoolData(world.Value, item, PropertyID.Die);
		}
		int num2 = 0;
		foreach (int item2 in ecsFilter)
		{
			if (!FuncData.GetBoolData(world.Value, item2, PropertyID.Die))
			{
				num2++;
			}
		}
		num = ((!flag || num2 != 1) ? (-1) : 0);
		if (num == -1)
		{
			int num3 = 0;
			foreach (int item3 in ecsFilter)
			{
				num3 += FuncData.GetIntData(world.Value, item3, PropertyID.BulletCount);
			}
			num = ((num3 == 0) ? 2 : (-1));
		}
		if (num == -1)
		{
			num = 1;
		}
		SharedGameResult gameResult = shared.GameResult;
		gameResult.Result = num;
		GameBiubiuVerify validationResult = GameBiubiuShare.GetValidationResult(world.Value);
		IGameSerializer serializer = shared.ResourceLoader.GetSerializer();
		gameResult.VerifyJson = serializer.ToJson(validationResult);
		string mD = serializer.GetMD5(gameResult.VerifyJson);
		gameResult.Commands = new List<ISyncCommand>();
		gameResult.Statistics.GameEndFrame = shared.LogicTickCount;
		gameResult.Statistics.BattleTimeMills = (long)(shared.GameTime.AsFloat * 1000f);
		gameResult.Statistics.BulletNum = new int[entitiesCount];
		EPlayerID winPlayerID = EPlayerID.ID_None;
		foreach (int item4 in ecsFilter)
		{
			ref ComponentPlayer reference = ref world.Value.GetPool<ComponentPlayer>().Get(item4);
			gameResult.Statistics.BulletNum[(int)reference.PlayerID] = FuncData.GetIntData(world.Value, item4, PropertyID.BulletFireCount);
		}
		if (num == 0)
		{
			foreach (int item5 in ecsFilter)
			{
				ref ComponentPlayer reference2 = ref world.Value.GetPool<ComponentPlayer>().Get(item5);
				if (!FuncData.GetBoolData(world.Value, item5, PropertyID.Die))
				{
					winPlayerID = reference2.PlayerID;
					break;
				}
			}
		}
		gameResult.Statistics.WinPlayerID = winPlayerID;
		shared.GameOver = true;
		List<ISyncCommand> commands = shared.Commands.Commands;
		for (int i = 0; i < commands.Count; i++)
		{
			gameResult.Commands.Add(commands[i].Clone());
		}
		gameResult.Replay = new GameBiubiuReplay
		{
			LevelPath = shared.InitData.LevelPath,
			MD5 = mD,
			Commands = gameResult.Commands
		};
		shared.GameState = EGameWorldState.Settlement;
	}
}
