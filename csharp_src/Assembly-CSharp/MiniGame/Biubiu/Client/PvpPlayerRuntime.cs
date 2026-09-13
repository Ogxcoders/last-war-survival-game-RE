using System;
using Joker;
using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class PvpPlayerRuntime : IDisposable
{
	public GameBiuBiuRunTime RunTime;

	public GameBiuBiuPlayerPvp Player;

	public GameBiubiuPlayerEnv Env;

	public void BindEnv(GameBiuBiuRunTime runTime, GameBiubiuPlayerEnv env)
	{
		RunTime = runTime;
		Env = env;
	}

	public void Prepare()
	{
		Player.SendEnterRoom();
	}

	public bool IsPrepared()
	{
		if (Player != null)
		{
			return Player.MultiPlayerState == EGameMultiPlayerState.Prepared;
		}
		return false;
	}

	public void Run()
	{
		RunTime.Enter(EGameType.PvpClient, Player.LevelPath, Player.LevelJson, verify: false);
	}

	public void Dispose()
	{
		if (RunTime != null)
		{
			RunTime.Dispose();
			RunTime = null;
		}
		if (Player != null)
		{
			Singleton<WorldService>.Instance.DestroyWorld(Player);
			Player = null;
		}
		Env = null;
	}
}
