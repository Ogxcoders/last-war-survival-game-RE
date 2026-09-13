using System;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class ConfigHolder : IResourceHolder
{
	public object Data { get; set; }

	public bool IsDone { get; }

	public bool IsError { get; }

	public bool IsValid { get; }

	public float Progress { get; }

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		return (T)Data;
	}

	public ConfigHolder(SharedConfigData configData)
	{
		Data = configData;
		IsDone = true;
		IsError = false;
		IsValid = true;
		Progress = 1f;
	}

	public void Dispose()
	{
		Data = null;
	}

	public object GetConfig(Type type, int id)
	{
		if (type == typeof(ToggleConfig))
		{
			return As<SharedConfigData>().ToggleConfigs[id];
		}
		if (type == typeof(PlayerConfig))
		{
			return As<SharedConfigData>().PlayerConfigs[id];
		}
		throw new Exception($"GetConfig not support type {type}");
	}

	public T GetConfig<T>(int id) where T : class
	{
		return GetConfig(typeof(T), id) as T;
	}
}
