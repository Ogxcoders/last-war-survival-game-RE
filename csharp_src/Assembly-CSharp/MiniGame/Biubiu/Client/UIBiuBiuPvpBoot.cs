using System;
using GameKit.Base;
using UnityEngine;
using XLua;

namespace MiniGame.Biubiu.Client;

public class UIBiuBiuPvpBoot : MonoBehaviour, IBindUI
{
	private Action<IRender> Callback;

	private DataUIAdapt DataUIAdapt;

	private GameBiuBiuRunTime BiuBiuRunTime;

	private UIBootPvpConnect PvpConnect;

	private Transform GameRoot;

	public void BindCallback(Action<IRender> handle)
	{
		Callback = (Action<IRender>)Delegate.Combine(Callback, handle);
	}

	public void UnBindCallback(Action<IRender> handle)
	{
		Callback = (Action<IRender>)Delegate.Remove(Callback, handle);
	}

	public void BindAdapt(DataUIAdapt dataUIAdapt)
	{
		DataUIAdapt = dataUIAdapt;
		DataUIAdapt.AddUISyncEvent(Callback);
	}

	public void StartGame(int serverId, string levelPath, LuaTable data, Transform gameRoot, Action<string> onError = null)
	{
		GameRoot = gameRoot;
		PvpConnect = new UIBootPvpConnect();
		PvpConnect.ConnectGameLift(serverId, data, 2, levelPath, onError, StartGame);
	}

	public void StartGameByPvpConnect(UIBootPvpConnect pvpConnect, Transform gameRoot)
	{
		PvpConnect = pvpConnect;
		GameRoot = gameRoot;
		StartGame();
	}

	private void StartGame()
	{
		BiuBiuRunTime = base.gameObject.GetOrAddComponent<GameBiubiuMultiRunTime>();
		PvpPlayerRuntime pvpPlayerRuntime = PvpConnect.PvpPlayerRuntime;
		GameBiuBiuPlayerPvp player = pvpPlayerRuntime.Player;
		pvpPlayerRuntime.BindEnv(env: new GameBiubiuPlayerEnv(player, GameRoot), runTime: BiuBiuRunTime);
		BiuBiuRunTime.BindRunEnv(this, pvpPlayerRuntime.Env);
		player.SetupRunTime(BiuBiuRunTime);
		player.SetupNetWork(OnReConnectSuccess, OnReconnectFair);
		pvpPlayerRuntime.Run();
		if (PvpConnect.ServerID != -1)
		{
			Transform transform = base.transform.Find("Ping");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: true);
				transform.GetComponent<DataUIPing>().SetPingInfo(PvpConnect.ServerID);
			}
		}
	}

	private void OnReconnectFair()
	{
		Callback?.Invoke(new DataUIRenderMessage.UINetWork
		{
			SuccOrFair = false
		});
	}

	private void OnReConnectSuccess()
	{
		Callback?.Invoke(new DataUIRenderMessage.UINetWork
		{
			SuccOrFair = true
		});
	}

	public void EndGame()
	{
		if (PvpConnect != null)
		{
			PvpConnect.Dispose();
			PvpConnect = null;
		}
	}

	public void Dispose()
	{
		if (PvpConnect != null)
		{
			PvpConnect.Dispose();
			PvpConnect = null;
		}
	}

	public bool IsDone()
	{
		if (BiuBiuRunTime == null)
		{
			return false;
		}
		if (BiuBiuRunTime.Loader == null)
		{
			return false;
		}
		return BiuBiuRunTime.Loader.IsDone;
	}

	private void OnDestroy()
	{
		if (DataUIAdapt != null)
		{
			DataUIAdapt.RemoveUISyncEvent(Callback);
			DataUIAdapt = null;
		}
		if (BiuBiuRunTime != null)
		{
			BiuBiuRunTime.Dispose();
			BiuBiuRunTime = null;
		}
	}
}
