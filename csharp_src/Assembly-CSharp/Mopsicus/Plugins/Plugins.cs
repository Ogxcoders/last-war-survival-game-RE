using System;
using System.Collections.Generic;
using NiceJson;
using UnityEngine;

namespace Mopsicus.Plugins;

public class Plugins : MonoBehaviour
{
	public const string ANDROID_CLASS_MASK = "ru.mopsicus.{0}.Plugin";

	private const string _dataObject = "Plugins";

	private const string _dataReceiver = "OnDataReceive";

	private Dictionary<string, IPlugin> _plugins;

	private void Awake()
	{
		base.name = "Plugins";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitPlugins();
	}

	private void OnDestroy()
	{
		_plugins = null;
	}

	private void InitPlugins()
	{
		base.gameObject.AddComponent<MobileInput>();
		IPlugin[] components = GetComponents<IPlugin>();
		_plugins = new Dictionary<string, IPlugin>(components.Length);
		IPlugin[] array = components;
		foreach (IPlugin plugin in array)
		{
			_plugins.Add(plugin.Name, plugin);
		}
		new JsonObject
		{
			["object"] = "Plugins",
			["receiver"] = "OnDataReceive"
		};
		Debug.Log("Plugins init");
	}

	private void OnDataReceive(string data)
	{
		try
		{
			JsonObject jsonObject = (JsonObject)JsonNode.ParseJsonString(data);
			if (_plugins.ContainsKey(jsonObject["name"]))
			{
				IPlugin plugin = _plugins[jsonObject["name"]];
				if (jsonObject.ContainsKey("error"))
				{
					plugin.OnError(jsonObject);
				}
				else
				{
					plugin.OnData(jsonObject);
				}
			}
			else
			{
				Debug.LogError(string.Format("{0} plugin does not exists", jsonObject["name"]));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError($"Plugins receive error: {ex.Message}, stack: {ex.StackTrace}");
		}
	}
}
