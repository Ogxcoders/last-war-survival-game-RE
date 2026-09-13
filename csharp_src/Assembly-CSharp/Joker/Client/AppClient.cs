using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Joker.Client;

public abstract class AppClient : MonoBehaviour
{
	public App.EMode Mode;

	public static AppClient Instance;

	public List<int> PingSimulate;

	public float PingJitterSimulate;

	protected void Init()
	{
		Instance = this;
		App.Mode = Mode;
		switch (Mode)
		{
		case App.EMode.ClientServerDev:
			InitShareService(isDist: false);
			InitClientService(isDist: false);
			InitServerService(isDist: false);
			break;
		case App.EMode.ClientServerDist:
			InitShareService(isDist: true);
			InitClientService(isDist: true);
			InitServerService(isDist: true);
			break;
		case App.EMode.ClientDev:
			InitShareService(isDist: false);
			InitClientService(isDist: false);
			break;
		case App.EMode.ClientDist:
			InitShareService(isDist: true);
			InitClientService(isDist: true);
			break;
		case App.EMode.ServerDev:
			InitShareService(isDist: false);
			InitServerService(isDist: false);
			break;
		case App.EMode.ServerDist:
			InitShareService(isDist: true);
			InitServerService(isDist: true);
			break;
		default:
			throw new Exception($"AppClient Awake failed, Mode={Mode}");
		}
	}

	public void StartUp()
	{
		Init();
		App.Startup();
	}

	protected virtual void InitShareService(bool isDist)
	{
		App.AddService<IAssemblyService, AssemblyServiceStatic, Action, Action, MethodInfo[]>(delegate
		{
			if (Mode == App.EMode.ClientDev || Mode == App.EMode.ClientDist || Mode == App.EMode.ClientServerDev || Mode == App.EMode.ClientServerDist)
			{
				InitClient(isDist);
			}
		}, delegate
		{
			if (Mode == App.EMode.ServerDev || Mode == App.EMode.ServerDist || Mode == App.EMode.ClientServerDev || Mode == App.EMode.ClientServerDist)
			{
				InitServer(isDist);
			}
		}, null);
		App.AddService<ThreadService>();
		App.AddService<ISchedulerService, SchedulerServiceThread>();
		App.AddService<WorldService>();
		App.AddService<ConfigService>();
		App.AddService<ILogService, UnityLogService>();
	}

	protected virtual void InitClientService(bool isDist)
	{
	}

	protected virtual void InitServerService(bool isDist)
	{
		App.AddService<TimeService>();
	}

	protected abstract void InitClient(bool isDist);

	protected abstract void InitServer(bool isDist);

	private void Update()
	{
		App.Update(Time.deltaTime);
	}

	private void LateUpdate()
	{
		App.LateUpdate(Time.deltaTime);
	}

	private void OnApplicationQuit()
	{
		App.Shutdown();
	}
}
