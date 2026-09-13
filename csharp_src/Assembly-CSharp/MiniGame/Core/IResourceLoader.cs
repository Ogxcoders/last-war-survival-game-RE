using System;

namespace MiniGame.Core;

public interface IResourceLoader
{
	IGameSerializer GetSerializer();

	IResourceHolder LoadAsset<T>(string name);

	IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null);

	object LoadConfig(int id, Type type);

	T LoadConfig<T>(int id) where T : class;
}
