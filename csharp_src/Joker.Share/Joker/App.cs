using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Joker;

public static class App
{
	public enum EMode
	{
		Default = -1,
		ClientServerDev,
		ClientServerDist,
		ClientDev,
		ClientDist,
		ServerDev,
		ServerDist
	}

	public static Action OnStartup;

	public static Action<float> OnUpdate;

	public static Action<float> OnLateUpdate;

	public static Action OnShutdown;

	private static readonly ConcurrentDictionary<Type, IService> _msUniqueServices = new ConcurrentDictionary<Type, IService>();

	private static readonly ConcurrentDictionary<Type, IService> _msServices = new ConcurrentDictionary<Type, IService>();

	private static readonly Dictionary<string, string> _msArgs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	public static EMode Mode { get; set; }

	public static short Process { get; set; }

	public static bool IsExit { get; private set; } = false;

	public static string Name { get; set; } = string.Empty;

	public static T Get<T>()
	{
		if (_msServices.TryGetValue(typeof(T), out var value))
		{
			return (T)value;
		}
		return default(T);
	}

	public static void ParseArgs(string[] args)
	{
		foreach (string text in args)
		{
			if (text.StartsWith("--"))
			{
				string[] array = text.Substring(2).Split(new char[1] { '=' });
				_msArgs[array[0]] = ((array.Length > 1) ? array[1] : null);
			}
			else if (text.StartsWith("-"))
			{
				string[] array2 = text.Substring(1).Split(new char[1] { '=' });
				_msArgs[array2[0]] = ((array2.Length > 1) ? array2[1] : null);
			}
		}
	}

	public static string GetArgument(string arg)
	{
		if (_msArgs.TryGetValue(arg, out var value))
		{
			return value;
		}
		return string.Empty;
	}

	public static bool GetArgumentBool(string arg, bool defaultValue = false)
	{
		if (_msArgs.TryGetValue(arg, out var value))
		{
			if (value == "0")
			{
				return false;
			}
			if (value == "1")
			{
				return true;
			}
			if (bool.TryParse(value, out var result))
			{
				return result;
			}
		}
		return defaultValue;
	}

	public static int GetArgumentInt(string arg, int defaultValue = 0)
	{
		if (_msArgs.TryGetValue(arg, out var _) && int.TryParse(arg, out var result))
		{
			return result;
		}
		return defaultValue;
	}

	public static void MainLoop(int sleep = 6)
	{
		MainLoopEx(sleep, -1f);
	}

	public static void MainLoopEx(int sleep = 6, float profilerInterval = 10f, string mainThreadName = "Main")
	{
		if (!string.IsNullOrEmpty(mainThreadName))
		{
			Thread.CurrentThread.Name = mainThreadName;
		}
		if (profilerInterval > 0f && ProfilerService.Instance == null)
		{
			AddService<ProfilerService, float, float>(profilerInterval, 0f);
		}
		ProfilerService instance = ProfilerService.Instance;
		Startup();
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		while (!IsExit)
		{
			using (instance?.CreateSample("App.Loop"))
			{
				float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
				stopwatch.Restart();
				using (instance?.CreateSample("App.Update"))
				{
					Update(deltaTime);
				}
				using (instance?.CreateSample("App.LateUpdate"))
				{
					LateUpdate(deltaTime);
				}
				using (instance?.CreateSample("App.Sleep"))
				{
					Thread.Sleep(sleep);
				}
			}
		}
		Shutdown();
		Console.Out.Flush();
		Console.Error.Flush();
	}

	public static void Exit()
	{
		IsExit = true;
	}

	public static void Update(float deltaTime)
	{
		OnUpdate?.Invoke(deltaTime);
	}

	public static void LateUpdate(float deltaTime)
	{
		OnLateUpdate?.Invoke(deltaTime);
	}

	public static void Shutdown()
	{
		OnShutdown?.Invoke();
		foreach (IService value in _msUniqueServices.Values)
		{
			value.Shutdown();
		}
		foreach (IService value2 in _msUniqueServices.Values)
		{
			value2.Destroy();
		}
	}

	public static void Startup()
	{
		foreach (IService value in _msUniqueServices.Values)
		{
			value.Awake();
		}
		foreach (IService value2 in _msUniqueServices.Values)
		{
			value2.Startup();
		}
		foreach (IService value3 in _msUniqueServices.Values)
		{
			if (value3 is IUpdateService updateService)
			{
				OnUpdate = (Action<float>)Delegate.Combine(OnUpdate, new Action<float>(updateService.Update));
			}
			if (value3 is ILateUpdateService lateUpdateService)
			{
				OnLateUpdate = (Action<float>)Delegate.Combine(OnLateUpdate, new Action<float>(lateUpdateService.LateUpdate));
			}
		}
		OnStartup?.Invoke();
	}

	public static void AddService<TS, T>() where TS : IService where T : TS, new()
	{
		T val = new T();
		_msServices.TryAdd(typeof(T), val);
		_msServices.TryAdd(typeof(TS), val);
		if (!_msUniqueServices.TryAdd(typeof(T), val))
		{
			throw new Exception("Duplicate service:" + typeof(T).FullName);
		}
	}

	public static void AddService<TS, T, P>(P param) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), param);
		_msServices.TryAdd(typeof(T), val);
		_msServices.TryAdd(typeof(TS), val);
		if (!_msUniqueServices.TryAdd(typeof(T), val))
		{
			throw new Exception("Duplicate service:" + typeof(T).FullName);
		}
	}

	public static void AddService<TS, T, P1, P2>(P1 p1, P2 p2) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), p1, p2);
		_msServices.TryAdd(typeof(T), val);
		_msServices.TryAdd(typeof(TS), val);
		if (!_msUniqueServices.TryAdd(typeof(T), val))
		{
			throw new Exception("Duplicate service:" + typeof(T).FullName);
		}
	}

	public static void AddService<TS, T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), p1, p2, p3);
		_msServices.TryAdd(typeof(T), val);
		_msServices.TryAdd(typeof(TS), val);
		if (!_msUniqueServices.TryAdd(typeof(T), val))
		{
			throw new Exception("Duplicate service:" + typeof(T).FullName);
		}
	}

	public static void AddService<T>() where T : IService, new()
	{
		T val = new T();
		_msServices.TryAdd(typeof(T), val);
		_msUniqueServices.TryAdd(typeof(T), val);
	}

	public static void AddService<T, P>(P param) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), param);
		_msServices.TryAdd(typeof(T), val);
		_msUniqueServices.TryAdd(typeof(T), val);
	}

	public static void AddService<T, P1, P2>(P1 p1, P2 p2) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), p1, p2);
		_msServices.TryAdd(typeof(T), val);
		_msUniqueServices.TryAdd(typeof(T), val);
	}

	public static void AddService<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), p1, p2, p3);
		_msServices.TryAdd(typeof(T), val);
		_msUniqueServices.TryAdd(typeof(T), val);
	}

	public static void AddService<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : IService
	{
		T val = (T)Activator.CreateInstance(typeof(T), p1, p2, p3, p4);
		_msServices.TryAdd(typeof(T), val);
		_msUniqueServices.TryAdd(typeof(T), val);
	}
}
