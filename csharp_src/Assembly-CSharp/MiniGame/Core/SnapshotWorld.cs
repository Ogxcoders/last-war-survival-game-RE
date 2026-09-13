using System;
using Leopotam.EcsLite;
using Newtonsoft.Json;

namespace MiniGame.Core;

public class SnapshotWorld : IDisposable
{
	[JsonProperty]
	internal object _envData;

	[JsonProperty]
	internal EcsWorldSnapshot _snapshot;

	[JsonIgnore]
	internal IGameSerializer _serializer;

	public void TakeSnapshot(GameWorld world)
	{
		_serializer = world.Serializer;
		Dispose();
		StoreEnv(world);
		StoreEcsWorld(world);
	}

	public void RestoreSnapshot(GameWorld world)
	{
		ReloadEnv(world);
		ReloadEcsWorld(world);
	}

	private void StoreEnv(GameWorld world)
	{
		_envData = world.Env.TakeSnapshot();
	}

	private void ReloadEnv(GameWorld world)
	{
		world.Env.RestoreSnapshot(_envData);
	}

	private void StoreEcsWorld(GameWorld world)
	{
		_snapshot = world._world.TakeSnapshot(world.Env);
	}

	private void ReloadEcsWorld(GameWorld world)
	{
		world._world.RestoreSnapshot(_snapshot);
	}

	public string ToJson()
	{
		return _serializer.ToJson(this);
	}

	public void FromJson(string json)
	{
		SnapshotWorld snapshotWorld = _serializer.FromJson<SnapshotWorld>(json);
		_envData = snapshotWorld._envData;
		_snapshot = snapshotWorld._snapshot;
	}

	public void Dispose()
	{
		if (_envData is IDisposable disposable)
		{
			disposable.Dispose();
		}
		_envData = null;
		_snapshot?.Dispose();
		_snapshot = null;
	}
}
