using System;
using System.Collections.Generic;
using Joker;
using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameLoader : IResourceLoader, IDisposable
{
	private ConfigHolder _configHolder;

	private Dictionary<string, ISerializerTextHolder> _cachedSnapshot = new Dictionary<string, ISerializerTextHolder>();

	private const string kResourceRoot = "Assets/Main/MiniGameRes/BiuBiu/";

	private Dictionary<string, EnumClient.AssetType> AssetTypeMap = new Dictionary<string, EnumClient.AssetType>
	{
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Env/",
			EnumClient.AssetType.Env
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/UI",
			EnumClient.AssetType.UI
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Map",
			EnumClient.AssetType.Map
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Character",
			EnumClient.AssetType.Character
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Bullet",
			EnumClient.AssetType.Bullet
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Barrel",
			EnumClient.AssetType.Barrel
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Obstacle",
			EnumClient.AssetType.Obstacle
		},
		{
			"Assets/Main/MiniGameRes/BiuBiu/Prefab/Toggle",
			EnumClient.AssetType.Toggle
		}
	};

	private IBindUI BindUI { get; }

	public DataResourceLoaderEnv LoaderEnv { get; }

	private IGameSerializer Serializer { get; }

	public GameLoader(DataResourceLoaderEnv resourceLoaderEnv, IBindUI bindUI, IGameSerializer serializer)
	{
		LoaderEnv = resourceLoaderEnv;
		Serializer = serializer;
		BindUI = bindUI;
	}

	public IGameSerializer GetSerializer()
	{
		return Serializer;
	}

	public void BindConfig(ConfigHolder holder)
	{
		_configHolder = holder;
	}

	public IResourceHolder LoadAsset<T>(string name)
	{
		ISerializerTextHolder value = null;
		if (!string.IsNullOrEmpty(name) && _cachedSnapshot.TryGetValue(name, out value))
		{
			value.Retain();
			return value;
		}
		Log.Debug("[BiuBiu]:GameLoader:LoadAsset " + name);
		string text = name;
		if (!string.IsNullOrEmpty(text) && !text.StartsWith("Assets/Main/MiniGameRes/BiuBiu/"))
		{
			text = "Assets/Main/MiniGameRes/BiuBiu/Template/" + text + ".txt";
		}
		if (typeof(T) == typeof(EcsEntitySnapshot))
		{
			TextAssetHolder<T> textAssetHolder = new TextAssetHolder<T>(new ResourceHolder(ResourceManager.LoadAssetStatic(text, typeof(TextAsset))), text, Serializer);
			textAssetHolder.Retain();
			_cachedSnapshot.Add(name, textAssetHolder);
			return textAssetHolder;
		}
		if (typeof(T) == typeof(GameLevel))
		{
			return new TextAssetHolder<T>(new ResourceHolder(ResourceManager.LoadAssetStatic(text, typeof(TextAsset))), text, Serializer);
		}
		if (typeof(T) == typeof(SharedConfigData))
		{
			return new TextAssetHolder<T>(new ResourceHolder(ResourceManager.LoadAssetStatic(text, typeof(TextAsset))), text, Serializer);
		}
		if (typeof(T) == typeof(DataUIAdapt))
		{
			return new UIAdaptHolder(BindUI);
		}
		return new ResourceHolder(ResourceManager.LoadAssetStatic(text, typeof(T)));
	}

	public IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		Log.Debug("[BiuBiu]:GameLoader:LoadAssetAsync " + name);
		bool num = typeof(T) == typeof(GameObjectHolder);
		IResourceHolder resourceHolder = null;
		Type type = (num ? typeof(GameObject) : typeof(T));
		resourceHolder = new ResourceHolder(ResourceManager.LoadAssetAsyncStatic(name, type));
		if (!num)
		{
			IResourceHolder resourceHolder2 = resourceHolder;
			resourceHolder2.OnDone = (Action<IResourceHolder>)Delegate.Combine(resourceHolder2.OnDone, callback);
			return resourceHolder;
		}
		EnumClient.AssetType assetType = EnumClient.AssetType.None;
		foreach (KeyValuePair<string, EnumClient.AssetType> item in AssetTypeMap)
		{
			if (name.StartsWith(item.Key))
			{
				assetType = item.Value;
				break;
			}
		}
		GameObjectHolder gameObjectHolder = new GameObjectHolder(resourceHolder, LoaderEnv, assetType);
		gameObjectHolder.OnDone = (Action<IResourceHolder>)Delegate.Combine(gameObjectHolder.OnDone, callback);
		return gameObjectHolder;
	}

	public object LoadConfig(int id, Type type)
	{
		if (type == typeof(SharedConfigData))
		{
			return _configHolder.Data;
		}
		if (typeof(IConfig).IsAssignableFrom(type))
		{
			return _configHolder.GetConfig(type, id);
		}
		throw new Exception($"LoadConfig not support type {type}");
	}

	public T LoadConfig<T>(int id) where T : class
	{
		if (typeof(T) == typeof(SharedConfigData))
		{
			return _configHolder.As<T>();
		}
		if (typeof(IConfig).IsAssignableFrom(typeof(T)))
		{
			return _configHolder.GetConfig<T>(id);
		}
		throw new Exception("LoadConfig not support type");
	}

	public void Dispose()
	{
		foreach (KeyValuePair<string, ISerializerTextHolder> item in _cachedSnapshot)
		{
			item.Value.Dispose();
		}
		_cachedSnapshot.Clear();
	}
}
