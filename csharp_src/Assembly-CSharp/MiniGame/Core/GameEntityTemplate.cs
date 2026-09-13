using System;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public class GameEntityTemplate
{
	public readonly string Name;

	public readonly long ComponentsVersion;

	public readonly EcsEntitySnapshot Template;

	private GameEntityTemplate(string name, long componentsVersion, EcsEntitySnapshot template)
	{
		Name = name;
		ComponentsVersion = componentsVersion;
		Template = template;
	}

	public void Update(EcsEntitySnapshot instance)
	{
		Type typeFromHandle = typeof(ComponentTemplate);
		(short, Type, object)[] array = new(short, Type, object)[Template.Components.Length];
		for (int i = 0; i < Template.Components.Length; i++)
		{
			(short, Type, object) tuple = Template.Components[i];
			if (tuple.Item2 == typeFromHandle)
			{
				array[i] = tuple;
				continue;
			}
			bool flag = false;
			for (int j = 0; j < instance.Components.Length; j++)
			{
				(short, Type, object) tuple2 = instance.Components[j];
				if (tuple2.Item2 == tuple.Item2)
				{
					array[i] = tuple2;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				array[i] = tuple;
			}
		}
		instance.Components = array;
	}

	public static GameEntityTemplate Create(EcsEntitySnapshot template)
	{
		for (int i = 0; i < template.Components.Length; i++)
		{
			(short, Type, object) tuple = template.Components[i];
			if (tuple.Item2 == typeof(ComponentTemplate))
			{
				ComponentTemplate componentTemplate = (ComponentTemplate)tuple.Item3;
				if (string.IsNullOrEmpty(componentTemplate.Name))
				{
					return null;
				}
				return new GameEntityTemplate(componentTemplate.Name, componentTemplate.ComponentsVersion, template);
			}
		}
		return null;
	}

	public static bool TryGetTemplate(EcsEntitySnapshot snapshot, out ComponentTemplate template)
	{
		Type typeFromHandle = typeof(ComponentTemplate);
		(short, Type, object)[] components = snapshot.Components;
		for (int i = 0; i < components.Length; i++)
		{
			(short, Type, object) tuple = components[i];
			if (tuple.Item2 == typeFromHandle)
			{
				template = (ComponentTemplate)tuple.Item3;
				return !string.IsNullOrEmpty(template.Name);
			}
		}
		template = default(ComponentTemplate);
		return false;
	}

	public static int NewEntity(EcsWorld world, EcsEntitySnapshot snapshot)
	{
		int entity = world.NewEntity();
		return NewEntity(world, entity, snapshot, overwrite: true);
	}

	public static int NewEntity(EcsWorld world, int entity)
	{
		EcsPool<ComponentTemplate> pool = world.GetPool<ComponentTemplate>();
		if (!pool.Has(entity))
		{
			return -1;
		}
		IResourceLoader resourceLoader = world.GetShared<IGameSharedEnv>().ResourceLoader;
		return NewEntity(world, entity, pool, resourceLoader);
	}

	public static int NewEntity(EcsWorld world, int entity, EcsPool<ComponentTemplate> pool, IResourceLoader loader)
	{
		int result = -1;
		ref ComponentTemplate reference = ref pool.Get(entity);
		if (string.IsNullOrEmpty(reference.Name))
		{
			return result;
		}
		IResourceHolder resourceHolder = loader.LoadAsset<EcsEntitySnapshot>(reference.Name);
		if (resourceHolder == null)
		{
			throw new Exception($"Cannot load entity: {entity} with template name: {reference.Name}");
		}
		EcsEntitySnapshot snapshot = resourceHolder.Data as EcsEntitySnapshot;
		result = NewEntity(world, entity, snapshot, overwrite: false);
		resourceHolder.Dispose();
		return result;
	}

	public static int NewEntity(EcsWorld world, int entity, EcsEntitySnapshot snapshot, bool overwrite)
	{
		if (snapshot.Components == null)
		{
			return entity;
		}
		object shared = world.GetShared();
		for (int i = 0; i < snapshot.Components.Length; i++)
		{
			(short, Type, object) tuple = snapshot.Components[i];
			IEcsPool ecsPool = null;
			ecsPool = ((!(tuple.Item2 != null)) ? world.GetPoolById(tuple.Item1) : world.GetPoolByType(tuple.Item2));
			ecsPool.RestoreSnapshot(entity, tuple.Item3, shared, overwrite: false);
		}
		return entity;
	}

	public static void TrimEntity(EcsWorld world, int entity)
	{
		EcsPool<ComponentTemplate> pool = world.GetPool<ComponentTemplate>();
		if (!pool.Has(entity))
		{
			return;
		}
		string name = pool.Get(entity).Name;
		IGameSharedEnv gameSharedEnv = world.GetShared() as IGameSharedEnv;
		IResourceHolder resourceHolder = gameSharedEnv.ResourceLoader.LoadAsset<EcsEntitySnapshot>(name);
		if (!(resourceHolder.Data is EcsEntitySnapshot ecsEntitySnapshot))
		{
			resourceHolder.Dispose();
			throw new Exception($"Cannot load entity: {entity} with template name: {name}");
		}
		for (int i = 0; i < ecsEntitySnapshot.Components.Length; i++)
		{
			(short, Type, object) tuple = ecsEntitySnapshot.Components[i];
			IEcsPool ecsPool = null;
			ecsPool = ((!(tuple.Item2 != null)) ? world.GetPoolById(tuple.Item1) : world.GetPoolByType(tuple.Item2));
			if (ecsPool != pool)
			{
				object lhs = ecsPool.TakeSnapshot(entity, gameSharedEnv);
				if (ecsPool.IsSnapshotEqual(lhs, tuple.Item3))
				{
					ecsPool.Del(entity);
				}
			}
		}
		resourceHolder.Dispose();
	}
}
