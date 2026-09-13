using System;
using System.Collections.Generic;
using FibMatrix.Rendering;
using Joker;
using Joker.Client;
using MiniGame.Core.Server;
using Newtonsoft.Json;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class AppBiubiu : AppClient
{
	public static bool InitAppClient;

	public static void InitApp(App.EMode mode = App.EMode.ClientDist, List<int> PingSimulate = null, float PingJitterSimulate = 0f)
	{
		if (!InitAppClient)
		{
			GameObject obj = new GameObject("AppBiubiu");
			AppBiubiu orAddComponent = obj.GetOrAddComponent<AppBiubiu>();
			orAddComponent.Mode = mode;
			orAddComponent.PingSimulate = PingSimulate;
			orAddComponent.PingJitterSimulate = PingJitterSimulate;
			orAddComponent.StartUp();
			UnityEngine.Object.DontDestroyOnLoad(obj);
			InitAppClient = true;
		}
	}

	protected override void InitShareService(bool isDist)
	{
		base.InitShareService(isDist);
		App.AddService<IMessageService, MessageServiceJson, List<Type>, JsonSerializerSettings>(RoomMessage.MessageTypes, null);
	}

	protected override void InitServerService(bool isDist)
	{
		base.InitServerService(isDist);
		App.AddService<GameRoomSessionService>();
	}

	protected override void InitClient(bool isDist)
	{
		if (!isDist)
		{
			Singleton<WorldService>.Instance.Create<GameBiubiuPlayerEditorSingleton>(ESchedulerType.Main, -1, "biubiu", Array.Empty<object>());
		}
	}

	protected override void InitServer(bool isDist)
	{
		if (PingSimulate != null && PingSimulate.Count > 0)
		{
			Singleton<WorldService>.Instance.Create<GameRoomPingDebugServer>(ESchedulerType.Main, 0, "UnityServerMainWorld", new object[2] { PingSimulate, PingJitterSimulate });
		}
		else
		{
			Singleton<WorldService>.Instance.Create<GameRoomServer>(ESchedulerType.Main, 0, "UnityServerMainWorld", Array.Empty<object>());
		}
	}
}
