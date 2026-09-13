using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class TestServerCheck : MonoBehaviour
{
	public const string kRootPath = "./";

	public int targetLogicTick = 9999999;

	private GameWorld game;

	private string md5;

	private bool complete;

	public int CurrentLogicTick
	{
		get
		{
			if (game == null)
			{
				return 0;
			}
			if (game.Env == null)
			{
				return 0;
			}
			return game.Env.LogicTickCount - 1;
		}
	}

	private void Start()
	{
		Play();
	}

	private void Update()
	{
		if (game == null || game.Env == null || (targetLogicTick >= 0 && game.Env.LogicTickCount > targetLogicTick) || complete)
		{
			return;
		}
		if (game.State == EGameWorldState.Settlement || game.Env.LogicTickCount > 1000000)
		{
			complete = true;
			string validationResultMD = GameBiubiuShare.GetValidationResultMD5(game.World);
			if (validationResultMD == md5)
			{
				Debug.Log(validationResultMD);
			}
			else
			{
				Debug.LogError("md5 is invalid: " + validationResultMD + " - " + md5);
			}
		}
		else if (targetLogicTick < 0)
		{
			TickLogicStep(99999999);
		}
		else if (targetLogicTick > game.Env.LogicTickCount)
		{
			TickLogicStep(targetLogicTick - game.Env.LogicTickCount + 1);
		}
	}

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void TickLogicStep(int step)
	{
		int num = game.Env.LogicTickCount + step;
		while (game.Env.LogicTickCount < num && game.State != EGameWorldState.Settlement)
		{
			game.Update(game.Env.LogicTickDelta.AsFloat - 0.001f);
		}
	}

	public void TickLogicOnStep()
	{
		TickLogicStep(1);
	}

	public void Play()
	{
	}
}
