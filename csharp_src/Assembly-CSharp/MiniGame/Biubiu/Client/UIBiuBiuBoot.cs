using System;
using GameKit.Base;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class UIBiuBiuBoot : MonoBehaviour, IBindUI
{
	private Action<IRender> Callback;

	private DataUIAdapt DataUIAdapt;

	private GameBiuBiuRunTime BiuBiuRunTime;

	public DataResourceLoaderEnv LoaderEnv;

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

	public void StartGame(string levelPath, Transform gameRoot)
	{
		Debug.Log("[BiuBiu] Pve StartGame Start");
		AppBiubiu.InitApp();
		BiuBiuRunTime = base.gameObject.GetOrAddComponent<GameBiuBiuRunTime>();
		LoaderEnv = new DataResourceLoaderEnv(gameRoot, 100f);
		BiuBiuRunTime.BindRunEnv(this, LoaderEnv);
		BiuBiuRunTime.Enter(EGameType.PveClient, levelPath, null, verify: false);
		Debug.Log("[BiuBiu] Pve StartGame End");
	}

	public void EndGame()
	{
		if (BiuBiuRunTime != null)
		{
			BiuBiuRunTime.Exit();
		}
	}

	public void Dispose()
	{
		if (BiuBiuRunTime != null)
		{
			BiuBiuRunTime.Dispose();
		}
	}

	public bool IsDone()
	{
		if (BiuBiuRunTime == null)
		{
			return false;
		}
		return BiuBiuRunTime.Loader.IsDone;
	}

	private void OnDisable()
	{
		if (DataUIAdapt != null)
		{
			DataUIAdapt.RemoveUISyncEvent(Callback);
			DataUIAdapt = null;
		}
		Callback = null;
		if (BiuBiuRunTime != null)
		{
			BiuBiuRunTime.Dispose();
			BiuBiuRunTime = null;
		}
	}
}
