using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Newtonsoft.Json;

namespace MiniGame.Core;

public class GameLevel : IDisposable
{
	[JsonProperty]
	internal object _levelEnv;

	[JsonProperty]
	internal EcsAliveEntitiesSnapshot _levelEntities;

	[JsonIgnore]
	private Dictionary<string, GameEntityTemplate> _entityTemplates;

	public T GetEnv<T>() where T : class
	{
		return _levelEnv as T;
	}

	public virtual void SaveLevel(GameWorld world, bool saveEnv = true)
	{
		if (saveEnv)
		{
			_levelEnv = SaveLevelEnv(world);
		}
		_levelEntities = SaveLevelEntities(world);
	}

	public virtual void LoadLevel(GameWorld world, bool loadEnv = true)
	{
		if (loadEnv)
		{
			LoadLevelEnv(world, _levelEnv);
		}
		LoadLevelEntities(world, _levelEntities);
	}

	public virtual void UpdateTemplates(IResourceLoader loader)
	{
		for (int i = 0; i < _levelEntities.AllEntities.Length; i++)
		{
			EcsEntitySnapshot ecsEntitySnapshot = _levelEntities.AllEntities[i];
			if (GameEntityTemplate.TryGetTemplate(ecsEntitySnapshot, out var template) && !string.IsNullOrEmpty(template.Name) && template.ComponentsVersion != 0L)
			{
				GameEntityTemplate entityTemplate = GetEntityTemplate(loader, template.Name);
				if (entityTemplate != null && entityTemplate.ComponentsVersion != template.ComponentsVersion)
				{
					entityTemplate.Update(ecsEntitySnapshot);
				}
			}
		}
	}

	public virtual GameEntityTemplate GetEntityTemplate(IResourceLoader loader, string name)
	{
		IResourceHolder resourceHolder = loader.LoadAsset<EcsEntitySnapshot>(name);
		if (resourceHolder == null)
		{
			AddEntityTemplate(name, null);
			return null;
		}
		if (!(resourceHolder.Data is EcsEntitySnapshot template))
		{
			AddEntityTemplate(name, null);
			return null;
		}
		GameEntityTemplate gameEntityTemplate = GameEntityTemplate.Create(template);
		AddEntityTemplate(name, gameEntityTemplate);
		return gameEntityTemplate;
	}

	public void AddEntityTemplate(string templateName, GameEntityTemplate template)
	{
		if (_entityTemplates == null)
		{
			_entityTemplates = new Dictionary<string, GameEntityTemplate>();
		}
		_entityTemplates[templateName] = template;
	}

	protected virtual object SaveLevelEnv(GameWorld world)
	{
		return world.Env.TakeSnapshot();
	}

	protected virtual void LoadLevelEnv(GameWorld world, object env)
	{
		world.Env.RestoreSnapshot(env);
	}

	protected virtual EcsAliveEntitiesSnapshot SaveLevelEntities(GameWorld world)
	{
		EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot = world._world.TakeEntitiesSnapshot();
		List<EcsEntitySnapshot> list = new List<EcsEntitySnapshot>(ecsAliveEntitiesSnapshot.AllEntities);
		for (int num = ecsAliveEntitiesSnapshot.EntitiesCount - ecsAliveEntitiesSnapshot.RecycledEntitiesCount - 1; num >= 0; num--)
		{
			EcsEntitySnapshot snapshot = list[num];
			if (!FilterSavedLevelEntity(world, snapshot))
			{
				ecsAliveEntitiesSnapshot.RecycledEntitiesCount++;
				list.RemoveAt(num);
			}
		}
		ecsAliveEntitiesSnapshot.AllEntities = list.ToArray();
		return ecsAliveEntitiesSnapshot;
	}

	protected virtual bool FilterSavedLevelEntity(GameWorld world, EcsEntitySnapshot snapshot)
	{
		return true;
	}

	protected virtual void LoadLevelEntities(GameWorld world, EcsAliveEntitiesSnapshot entities)
	{
		world._world.RestoreEntitiesSnapshot(entities);
	}

	public void Dispose()
	{
		if (_levelEnv is IDisposable disposable)
		{
			disposable.Dispose();
		}
		_levelEnv = null;
		IDisposable levelEntities;
		if ((levelEntities = _levelEntities) != null)
		{
			levelEntities.Dispose();
		}
		_levelEntities = null;
		_entityTemplates?.Clear();
	}
}
