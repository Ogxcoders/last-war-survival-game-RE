using System;

namespace MiniGame.Core;

public class ResourceLoaderNone : IResourceLoader
{
	public float Progress { get; } = 1f;

	public bool IsDone { get; } = true;

	public bool IsLoading { get; }

	public bool IsError { get; }

	public IGameSerializer GetSerializer()
	{
		return null;
	}

	public IResourceHolder LoadAsset<T>(string name)
	{
		throw new NotImplementedException();
	}

	public IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null)
	{
		throw new NotImplementedException();
	}

	public void UnloadAsset(IResourceHolder resourceHolder)
	{
		throw new NotImplementedException();
	}

	public object LoadConfig(int id, Type type)
	{
		throw new NotImplementedException();
	}

	public T LoadConfig<T>(int id) where T : class
	{
		throw new NotImplementedException();
	}
}
