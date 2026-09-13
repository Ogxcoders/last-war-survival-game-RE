using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Leopotam.EcsLite.Di;
using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public class EcsWorld
{
	public struct Config
	{
		public int Entities;

		public int RecycledEntities;

		public int Pools;

		public int Filters;

		public int PoolDenseSize;

		public int PoolRecycledSize;

		public int EntityComponentsSize;

		internal const int EntitiesDefault = 512;

		internal const int RecycledEntitiesDefault = 512;

		internal const int PoolsDefault = 512;

		internal const int FiltersDefault = 512;

		internal const int PoolDenseSizeDefault = 512;

		internal const int PoolRecycledSizeDefault = 512;

		internal const int EntityComponentsSizeDefault = 8;
	}

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class Mask
	{
		private readonly EcsWorld _world;

		internal int[] Include;

		internal int[] Exclude;

		internal int IncludeCount;

		internal int ExcludeCount;

		internal int Hash;

		internal Mask(EcsWorld world)
		{
			_world = world;
			Include = new int[8];
			Exclude = new int[2];
			Reset();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Reset()
		{
			IncludeCount = 0;
			ExcludeCount = 0;
			Hash = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Mask Inc<T>() where T : struct
		{
			int id = _world.GetPool<T>().GetId();
			if (IncludeCount == Include.Length)
			{
				Array.Resize(ref Include, IncludeCount << 1);
			}
			Include[IncludeCount++] = id;
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Mask Exc<T>() where T : struct
		{
			int id = _world.GetPool<T>().GetId();
			if (ExcludeCount == Exclude.Length)
			{
				Array.Resize(ref Exclude, ExcludeCount << 1);
			}
			Exclude[ExcludeCount++] = id;
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public EcsFilter End(int capacity = 512)
		{
			Array.Sort(Include, 0, IncludeCount);
			Array.Sort(Exclude, 0, ExcludeCount);
			Hash = IncludeCount + ExcludeCount;
			int i = 0;
			for (int includeCount = IncludeCount; i < includeCount; i++)
			{
				Hash = Hash * 314159 + Include[i];
			}
			int j = 0;
			for (int excludeCount = ExcludeCount; j < excludeCount; j++)
			{
				Hash = Hash * 314159 - Exclude[j];
			}
			(EcsFilter, bool) filterInternal = _world.GetFilterInternal(this, capacity);
			var (result, _) = filterInternal;
			if (!filterInternal.Item2)
			{
				Recycle();
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Recycle()
		{
			Reset();
			if (_world._masksCount == _world._masks.Length)
			{
				Array.Resize(ref _world._masks, _world._masksCount << 1);
			}
			_world._masks[_world._masksCount++] = this;
		}
	}

	private short[] _entities;

	private int _entitiesItemSize;

	private int _entitiesCount;

	private int[] _recycledEntities;

	private int _recycledEntitiesCount;

	private IEcsPool[] _pools;

	private short _poolsCount;

	private readonly int _poolDenseSize;

	private readonly int _poolRecycledSize;

	private readonly Dictionary<Type, IEcsPool> _poolHashes;

	private readonly Dictionary<Type, IEcsPoolDelegate> _poolDelegates;

	private readonly Dictionary<int, EcsFilter> _hashedFilters;

	private readonly List<EcsFilter> _allFilters;

	private List<EcsFilter>[] _filtersByIncludedComponents;

	private List<EcsFilter>[] _filtersByExcludedComponents;

	private Mask[] _masks;

	private int _masksCount;

	private bool _fixedPoolOrder;

	private bool _destroyed;

	private object _shared;

	public IEcsDebugger Debugger { get; }

	public List<IEcsPool> GetSnapshotPools(List<Type> includes = null, List<Type> excludes = null, List<IEcsPool> pools = null)
	{
		if (pools != null)
		{
			pools.Clear();
		}
		else
		{
			pools = new List<IEcsPool>();
		}
		if (includes == null || includes.Count == 0)
		{
			for (int i = 0; i < _poolsCount; i++)
			{
				if (excludes == null || !excludes.Contains(_pools[i].GetComponentType()))
				{
					pools.Add(_pools[i]);
				}
			}
		}
		else
		{
			for (int j = 0; includes.Count > j; j++)
			{
				if (excludes == null || !excludes.Contains(includes[j]))
				{
					pools.Add(GetPoolByType(includes[j]));
				}
			}
		}
		return pools;
	}

	private static int SortEntities(EcsEntitySnapshot lhs, EcsEntitySnapshot rhs)
	{
		return lhs.EntityID.CompareTo(rhs.EntityID);
	}

	public EcsAliveEntitiesSnapshot TakeEntitiesStates(List<Type> includes, List<Type> exclutes, List<EcsFilter> filters)
	{
		if (filters == null || filters.Count == 0)
		{
			return TakeEntitiesStates(includes, exclutes);
		}
		EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot = null;
		List<EcsEntitySnapshot> list = new List<EcsEntitySnapshot>();
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < filters.Count; i++)
		{
			EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot2 = TakeEntitiesStates(includes, exclutes, filters[i]);
			if (ecsAliveEntitiesSnapshot == null)
			{
				ecsAliveEntitiesSnapshot = ecsAliveEntitiesSnapshot2;
			}
			EcsEntitySnapshot[] allEntities = ecsAliveEntitiesSnapshot2.AllEntities;
			foreach (EcsEntitySnapshot ecsEntitySnapshot in allEntities)
			{
				if (hashSet.Add(ecsEntitySnapshot.EntityID))
				{
					list.Add(ecsEntitySnapshot);
				}
			}
		}
		ecsAliveEntitiesSnapshot.AllEntities = list.ToArray();
		Array.Sort(ecsAliveEntitiesSnapshot.AllEntities, SortEntities);
		return ecsAliveEntitiesSnapshot;
	}

	public EcsAliveEntitiesSnapshot TakeEntitiesStates<TInc>(List<Type> includes = null, List<Type> excludes = null) where TInc : struct, IEcsInclude
	{
		EcsFilter filter = default(TInc).Fill(this).End();
		return TakeEntitiesStates(includes, excludes, filter);
	}

	public EcsAliveEntitiesSnapshot TakeEntitiesStates<TInc, TExc>(List<Type> includes = null, List<Type> excludes = null) where TInc : struct, IEcsInclude where TExc : struct, IEcsExclude
	{
		EcsFilter filter = default(TExc).Fill(default(TInc).Fill(this)).End();
		return TakeEntitiesStates(includes, excludes, filter);
	}

	public EcsAliveEntitiesSnapshot TakeEntitiesStates(List<Type> includes = null, List<Type> excludes = null, EcsFilter filter = null)
	{
		List<IEcsPool> snapshotPools = GetSnapshotPools(includes, excludes);
		EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot = new EcsAliveEntitiesSnapshot();
		ecsAliveEntitiesSnapshot.EntitiesItemSize = 0;
		ecsAliveEntitiesSnapshot.EntitiesCount = _entitiesCount - _recycledEntitiesCount;
		ecsAliveEntitiesSnapshot.EntitiesCapacity = 0;
		ecsAliveEntitiesSnapshot.RecycledEntitiesCount = 0;
		ecsAliveEntitiesSnapshot.RecycledEntitiesCapacity = 0;
		if (filter == null)
		{
			ecsAliveEntitiesSnapshot.AllEntities = new EcsEntitySnapshot[_entitiesCount - _recycledEntitiesCount];
			int i = 0;
			int num = 0;
			for (; i < _entitiesCount; i++)
			{
				if (IsEntityAliveInternal(i))
				{
					ecsAliveEntitiesSnapshot.AllEntities[num] = DoSnapshotTakeEntityPartial(i, snapshotPools);
					num++;
				}
			}
		}
		else
		{
			ecsAliveEntitiesSnapshot.AllEntities = new EcsEntitySnapshot[filter.GetEntitiesCount()];
			int num2 = 0;
			foreach (int item in filter)
			{
				ecsAliveEntitiesSnapshot.AllEntities[num2] = DoSnapshotTakeEntityPartial(item, snapshotPools);
				num2++;
			}
		}
		ecsAliveEntitiesSnapshot.PoolCount = 0;
		Array.Sort(ecsAliveEntitiesSnapshot.AllEntities, SortEntities);
		return ecsAliveEntitiesSnapshot;
	}

	public EcsAliveEntitiesSnapshot TakeEntitiesSnapshot()
	{
		EcsAliveEntitiesSnapshot ecsAliveEntitiesSnapshot = new EcsAliveEntitiesSnapshot();
		ecsAliveEntitiesSnapshot.EntitiesItemSize = _entitiesItemSize;
		ecsAliveEntitiesSnapshot.EntitiesCount = _entitiesCount;
		ecsAliveEntitiesSnapshot.EntitiesCapacity = _entities.Length;
		ecsAliveEntitiesSnapshot.RecycledEntitiesCount = _recycledEntitiesCount;
		ecsAliveEntitiesSnapshot.RecycledEntitiesCapacity = _recycledEntities.Length;
		ecsAliveEntitiesSnapshot.AllEntities = new EcsEntitySnapshot[_entitiesCount - _recycledEntitiesCount];
		int i = 0;
		int num = 0;
		for (; i < _entitiesCount; i++)
		{
			if (IsEntityAliveInternal(i))
			{
				ecsAliveEntitiesSnapshot.AllEntities[num] = DoSnapshotTakeEntity(i);
				num++;
			}
		}
		ecsAliveEntitiesSnapshot.PoolCount = _poolsCount;
		return ecsAliveEntitiesSnapshot;
	}

	public void RestoreEntitiesSnapshot(EcsAliveEntitiesSnapshot snapshot)
	{
		DoSnapshotEnsurePoolCapacity(_poolsCount + snapshot.PoolCount);
		_filtersByIncludedComponents = new List<EcsFilter>[_filtersByIncludedComponents.Length];
		_filtersByExcludedComponents = new List<EcsFilter>[_filtersByExcludedComponents.Length];
		DoSnapshotCleanup();
		DoRestoreAliveEntitiesSnapshot(snapshot);
		DoRestoreAliveEntitiesFilters();
	}

	private void DoRestoreAliveEntitiesSnapshot(EcsAliveEntitiesSnapshot snapshot)
	{
		_entities = new short[snapshot.EntitiesCapacity];
		_entitiesItemSize = snapshot.EntitiesItemSize;
		_entitiesCount = snapshot.EntitiesCount;
		_recycledEntities = new int[snapshot.RecycledEntitiesCapacity];
		_recycledEntitiesCount = snapshot.RecycledEntitiesCount;
		int num = snapshot.AllEntities.Length;
		int i = 0;
		int num2 = 0;
		int num3 = 0;
		for (; i < _entitiesCount; i++)
		{
			EcsEntitySnapshot ecsEntitySnapshot = ((num2 < num) ? snapshot.AllEntities[num2] : null);
			int num4 = ecsEntitySnapshot?.EntityID ?? (-1);
			if (i != num4)
			{
				_recycledEntities[num3] = i;
				_entities[GetRawEntityOffset(i) + 1] = -1;
				num3++;
			}
			else
			{
				DoSnapshoRestoreEntity(ecsEntitySnapshot);
				num2++;
			}
		}
	}

	private void DoRestoreAliveEntitiesFilters()
	{
		for (int i = 0; i < _allFilters.Count; i++)
		{
			EcsFilter ecsFilter = _allFilters[i];
			Mask mask = ecsFilter.GetMask();
			ecsFilter.Clear();
			int j = 0;
			for (int includeCount = mask.IncludeCount; j < includeCount; j++)
			{
				List<EcsFilter> list = _filtersByIncludedComponents[mask.Include[j]];
				if (list == null)
				{
					list = new List<EcsFilter>(8);
					_filtersByIncludedComponents[mask.Include[j]] = list;
				}
				list.Add(ecsFilter);
			}
			int k = 0;
			for (int excludeCount = mask.ExcludeCount; k < excludeCount; k++)
			{
				List<EcsFilter> list2 = _filtersByExcludedComponents[mask.Exclude[k]];
				if (list2 == null)
				{
					list2 = new List<EcsFilter>(8);
					_filtersByExcludedComponents[mask.Exclude[k]] = list2;
				}
				list2.Add(ecsFilter);
			}
			int l = 0;
			for (int entitiesCount = _entitiesCount; l < entitiesCount; l++)
			{
				if (_entities[GetRawEntityOffset(l)] > 0 && IsMaskCompatible(mask, l))
				{
					ecsFilter.AddEntity(l);
				}
			}
		}
	}

	public EcsWorldSnapshot TakeSnapshot(object env)
	{
		EcsWorldSnapshot ecsWorldSnapshot = new EcsWorldSnapshot();
		ecsWorldSnapshot.EntitiesItemSize = _entitiesItemSize;
		ecsWorldSnapshot.EntitiesCount = _entitiesCount;
		ecsWorldSnapshot.EntitiesCapacity = _entities.Length;
		ecsWorldSnapshot.RecycledEntities = new int[_recycledEntitiesCount];
		Array.Copy(_recycledEntities, ecsWorldSnapshot.RecycledEntities, _recycledEntitiesCount);
		ecsWorldSnapshot.RecycledEntitiesCount = _recycledEntitiesCount;
		ecsWorldSnapshot.RecycledEntitiesCapacity = _recycledEntities.Length;
		ecsWorldSnapshot.AllFilters = new EcsFilterSnapshot[_allFilters.Count];
		for (int i = 0; i < _allFilters.Count; i++)
		{
			ecsWorldSnapshot.AllFilters[i] = _allFilters[i].TakeSnapshot();
		}
		ecsWorldSnapshot.AllEntities = new EcsEntitySnapshot[_entitiesCount];
		for (int j = 0; j < _entitiesCount; j++)
		{
			ecsWorldSnapshot.AllEntities[j] = DoSnapshotTakeEntity(j);
		}
		ecsWorldSnapshot.PoolCount = _poolsCount;
		ecsWorldSnapshot.AllPools = new Type[_poolsCount];
		for (int k = 0; k < _poolsCount; k++)
		{
			ecsWorldSnapshot.AllPools[k] = _pools[k].GetType();
		}
		return ecsWorldSnapshot;
	}

	public void RestoreSnapshot(EcsWorldSnapshot snapshot)
	{
		DoSnapshotEnsurePoolCapacity(_poolsCount + snapshot.PoolCount);
		List<EcsFilter>[] filtersByIncludedComponents = _filtersByIncludedComponents;
		List<EcsFilter>[] filtersByExcludedComponents = _filtersByExcludedComponents;
		_filtersByIncludedComponents = new List<EcsFilter>[filtersByIncludedComponents.Length];
		_filtersByExcludedComponents = new List<EcsFilter>[filtersByExcludedComponents.Length];
		DoSnapshotCleanup();
		DoSnapshotRestoreEntities(snapshot);
		_filtersByIncludedComponents = filtersByIncludedComponents;
		_filtersByExcludedComponents = filtersByExcludedComponents;
		DoSnapshotRestoreFilter(snapshot);
		DoSnapshotTrigger();
	}

	public EcsEntitySnapshot SaveEntity(int entity)
	{
		EcsEntitySnapshot ecsEntitySnapshot = DoSnapshotTakeEntity(entity);
		ecsEntitySnapshot.EntityID = -1;
		ecsEntitySnapshot.EntityGen = -1;
		return ecsEntitySnapshot;
	}

	private void DoSnapshotCleanup()
	{
		for (int num = _entitiesCount - 1; num >= 0; num--)
		{
			if (_entities[GetRawEntityOffset(num)] > 0)
			{
				DelEntity(num);
			}
		}
	}

	private void DoSnapshotEnsurePoolCapacity(int sizeMax)
	{
		if (sizeMax >= _pools.Length)
		{
			EnsurePoolSize(_pools.Length);
			DoSnapshotEnsurePoolCapacity(sizeMax);
		}
	}

	private void DoSnapshotRecoverPoolOrder(Type[] allPools)
	{
		for (int i = 0; i < allPools.Length; i++)
		{
			Type type = allPools[i];
			Type key = type.GetGenericArguments()[0];
			if (_poolHashes.TryGetValue(key, out var value))
			{
				value.RestoreSnapshotId((short)i);
				_poolHashes.Remove(key);
			}
			else
			{
				value = Activator.CreateInstance(type, this, (short)i, _poolDenseSize, GetWorldSize(), _poolRecycledSize) as IEcsPool;
			}
			_pools[i] = value;
		}
		_poolsCount = (short)allPools.Length;
		int num = allPools.Length;
		foreach (IEcsPool value2 in _poolHashes.Values)
		{
			value2.RestoreSnapshotId((short)num);
			_pools[num++] = value2;
			_poolsCount++;
		}
		for (int j = 0; j < allPools.Length; j++)
		{
			_poolHashes[allPools[j].GetGenericArguments()[0]] = _pools[j];
		}
	}

	private void DoSnapshotRestoreEntities(EcsWorldSnapshot snapshot)
	{
		_entities = new short[snapshot.EntitiesCapacity];
		_entitiesItemSize = snapshot.EntitiesItemSize;
		_entitiesCount = snapshot.EntitiesCount;
		_recycledEntities = new int[snapshot.RecycledEntitiesCapacity];
		Array.Copy(snapshot.RecycledEntities, _recycledEntities, snapshot.RecycledEntitiesCount);
		_recycledEntitiesCount = snapshot.RecycledEntitiesCount;
		for (int i = 0; i < snapshot.AllEntities.Length; i++)
		{
			DoSnapshoRestoreEntity(snapshot.AllEntities[i]);
		}
	}

	private void DoSnapshotRestoreFilter(EcsWorldSnapshot snapshot)
	{
		if (_allFilters.Count != snapshot.AllFilters.Length)
		{
			throw new InvalidOperationException("All filter count mismatch");
		}
		for (int i = 0; i < snapshot.AllFilters.Length; i++)
		{
			DoSnapshotRestoreFilter(snapshot.AllFilters[i]);
		}
	}

	private void DoSnapshotTrigger()
	{
	}

	private EcsEntitySnapshot DoSnapshotTakeEntity(int entity)
	{
		EcsEntitySnapshot ecsEntitySnapshot = new EcsEntitySnapshot();
		ecsEntitySnapshot.EntityID = entity;
		ecsEntitySnapshot.EntityGen = GetEntityGen(entity);
		int componentsCount = GetComponentsCount(entity);
		if (componentsCount == 0)
		{
			ecsEntitySnapshot.Components = null;
			return ecsEntitySnapshot;
		}
		object shared = GetShared();
		ecsEntitySnapshot.Components = new(short, Type, object)[componentsCount];
		int num = GetRawEntityOffset(entity) + 2;
		for (int i = 0; i < componentsCount; i++)
		{
			short num2 = _entities[num + i];
			IEcsPool ecsPool = _pools[num2];
			object item = ecsPool.TakeSnapshot(entity, shared);
			ecsEntitySnapshot.Components[i] = (PoolID: num2, ComponentType: ecsPool.GetComponentType(), Data: item);
		}
		return ecsEntitySnapshot;
	}

	public EcsEntitySnapshot DoSnapshotTakeEntityPartial(int entity, List<IEcsPool> pools)
	{
		EcsEntitySnapshot ecsEntitySnapshot = new EcsEntitySnapshot();
		ecsEntitySnapshot.EntityID = entity;
		ecsEntitySnapshot.EntityGen = GetEntityGen(entity);
		if (pools.Count == 0)
		{
			ecsEntitySnapshot.Components = null;
			return ecsEntitySnapshot;
		}
		List<(short, Type, object)> list = new List<(short, Type, object)>();
		for (int i = 0; i < pools.Count; i++)
		{
			IEcsPool ecsPool = pools[i];
			if (ecsPool.Has(entity))
			{
				object item = ecsPool.TakeSnapshot(entity, GetShared());
				list.Add(((short)ecsPool.GetId(), ecsPool.GetComponentType(), item));
			}
		}
		ecsEntitySnapshot.Components = list.ToArray();
		return ecsEntitySnapshot;
	}

	public void DoSnapshotRestoreEntityPartial(int entity, EcsEntitySnapshot snapshot, object env)
	{
		if (snapshot.Components != null && snapshot.Components.Length != 0)
		{
			for (int i = 0; i < snapshot.Components.Length; i++)
			{
				(short, Type, object) tuple = snapshot.Components[i];
				IEcsPool ecsPool = null;
				ecsPool = ((!(tuple.Item2 != null)) ? GetPoolById(tuple.Item1) : GetPoolByType(tuple.Item2));
				ecsPool.RestoreSnapshot(entity, tuple.Item3, env, overwrite: true);
			}
		}
	}

	private void DoSnapshoRestoreEntity(EcsEntitySnapshot snapshot)
	{
		_entities[GetRawEntityOffset(snapshot.EntityID) + 1] = snapshot.EntityGen;
		_entities[GetRawEntityOffset(snapshot.EntityID)] = 0;
		if (snapshot.Components == null && snapshot.Components.Length == 0)
		{
			return;
		}
		object shared = GetShared();
		if (snapshot.Components != null)
		{
			for (int i = 0; i < snapshot.Components.Length; i++)
			{
				(short, Type, object) tuple = snapshot.Components[i];
				IEcsPool ecsPool = null;
				ecsPool = ((!(tuple.Item2 != null)) ? GetPoolById(tuple.Item1) : GetPoolByType(tuple.Item2));
				ecsPool.RestoreSnapshot(snapshot.EntityID, tuple.Item3, shared, overwrite: true);
			}
		}
	}

	private void DoSnapshotRestoreFilter(EcsFilterSnapshot snapshot)
	{
		if (!_hashedFilters.TryGetValue(snapshot.Hash, out var value))
		{
			throw new InvalidOperationException("Filter hash mismatch");
		}
		value.RestoreSnapshot(snapshot);
	}

	public object GetShared()
	{
		return _shared;
	}

	public virtual T GetShared<T>() where T : class
	{
		return _shared as T;
	}

	public void LogDebug(string log)
	{
		if (Debugger != null)
		{
			Debugger.LogDebug(log);
		}
	}

	public void LogInfo(string log)
	{
		if (Debugger != null)
		{
			Debugger.LogInfo(log);
		}
	}

	public void LogError(string log)
	{
		if (Debugger != null)
		{
			Debugger.LogError(log);
		}
	}

	public void LogWarning(string log)
	{
		if (Debugger != null)
		{
			Debugger.LogWarning(log);
		}
	}

	public EcsWorld(object shared, IEnumerable<IEcsPoolDelegate> delegates = null, IEcsDebugger debugger = null, in Config cfg = default(Config))
		: this(shared, fixedPoolOrder: true, delegates, in cfg)
	{
		Debugger = debugger ?? new EcsDebugger();
	}

	public EcsWorld(object shared, bool fixedPoolOrder, IEnumerable<IEcsPoolDelegate> delegates = null, in Config cfg = default(Config))
	{
		_shared = shared;
		_fixedPoolOrder = fixedPoolOrder;
		int num = ((cfg.Entities > 0) ? cfg.Entities : 512);
		_entitiesItemSize = 2 + ((cfg.EntityComponentsSize > 0) ? cfg.EntityComponentsSize : 8);
		_entities = new short[num * _entitiesItemSize];
		_recycledEntities = new int[(cfg.RecycledEntities > 0) ? cfg.RecycledEntities : 512];
		_entitiesCount = 0;
		_recycledEntitiesCount = 0;
		num = ((cfg.Pools > 0) ? cfg.Pools : 512);
		_pools = new IEcsPool[num];
		_poolHashes = new Dictionary<Type, IEcsPool>(num);
		_poolDelegates = new Dictionary<Type, IEcsPoolDelegate>();
		if (delegates != null)
		{
			foreach (IEcsPoolDelegate @delegate in delegates)
			{
				_poolDelegates.Add(@delegate.DelegateType, @delegate);
			}
		}
		_filtersByIncludedComponents = new List<EcsFilter>[num];
		_filtersByExcludedComponents = new List<EcsFilter>[num];
		_poolDenseSize = ((cfg.PoolDenseSize > 0) ? cfg.PoolDenseSize : 512);
		_poolRecycledSize = ((cfg.PoolRecycledSize > 0) ? cfg.PoolRecycledSize : 512);
		_poolsCount = 0;
		num = ((cfg.Filters > 0) ? cfg.Filters : 512);
		_hashedFilters = new Dictionary<int, EcsFilter>(num);
		_allFilters = new List<EcsFilter>(num);
		_masks = new Mask[64];
		_masksCount = 0;
		_destroyed = false;
	}

	public void Destroy()
	{
		_destroyed = true;
		for (int num = _entitiesCount - 1; num >= 0; num--)
		{
			if (_entities[GetRawEntityOffset(num)] > 0)
			{
				DelEntity(num);
			}
		}
		_pools = Array.Empty<IEcsPool>();
		_poolHashes.Clear();
		_poolDelegates.Clear();
		_hashedFilters.Clear();
		_allFilters.Clear();
		_filtersByIncludedComponents = Array.Empty<List<EcsFilter>>();
		_filtersByExcludedComponents = Array.Empty<List<EcsFilter>>();
		_shared = null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetRawEntityOffset(int entity)
	{
		return entity * _entitiesItemSize;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsAlive()
	{
		return !_destroyed;
	}

	public int NewEntity()
	{
		int num;
		if (_recycledEntitiesCount > 0)
		{
			num = _recycledEntities[--_recycledEntitiesCount];
			_entities[GetRawEntityOffset(num) + 1] *= -1;
		}
		else
		{
			if (_entitiesCount * _entitiesItemSize == _entities.Length)
			{
				int num2 = _entitiesCount << 1;
				Array.Resize(ref _entities, num2 * _entitiesItemSize);
				int i = 0;
				for (int poolsCount = _poolsCount; i < poolsCount; i++)
				{
					_pools[i].Resize(num2);
				}
				int j = 0;
				for (int count = _allFilters.Count; j < count; j++)
				{
					_allFilters[j].ResizeSparseIndex(num2);
				}
			}
			num = _entitiesCount++;
			_entities[GetRawEntityOffset(num) + 1] = 1;
		}
		return num;
	}

	public void DelEntity(int entity)
	{
		int rawEntityOffset = GetRawEntityOffset(entity);
		short num = _entities[rawEntityOffset];
		ref short reference = ref _entities[rawEntityOffset + 1];
		if (reference < 0)
		{
			return;
		}
		if (num > 0)
		{
			for (int num2 = rawEntityOffset + 2 + num - 1; num2 >= rawEntityOffset + 2; num2--)
			{
				_pools[_entities[num2]].Del(entity);
			}
			return;
		}
		reference = (short)((reference == short.MaxValue) ? (-1) : (-(reference + 1)));
		if (_recycledEntitiesCount == _recycledEntities.Length)
		{
			Array.Resize(ref _recycledEntities, _recycledEntitiesCount << 1);
		}
		_recycledEntities[_recycledEntitiesCount++] = entity;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetComponentsCount(int entity)
	{
		return _entities[GetRawEntityOffset(entity)];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public short GetEntityGen(int entity)
	{
		return _entities[GetRawEntityOffset(entity) + 1];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetRawEntityItemSize()
	{
		return _entitiesItemSize;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetUsedEntitiesCount()
	{
		return _entitiesCount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetWorldSize()
	{
		return _entities.Length / _entitiesItemSize;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetPoolsCount()
	{
		return _poolsCount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryGetPoolDelegate<T>(out IEcsPoolDelegate poolDelegate)
	{
		return _poolDelegates.TryGetValue(typeof(T), out poolDelegate);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetEntitiesCount()
	{
		return _entitiesCount - _recycledEntitiesCount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public short[] GetRawEntities()
	{
		return _entities;
	}

	public EcsPool<T> GetPool<T>() where T : struct
	{
		Type typeFromHandle = typeof(T);
		if (_poolHashes.TryGetValue(typeFromHandle, out var value))
		{
			return (EcsPool<T>)value;
		}
		if (_fixedPoolOrder)
		{
			throw new NotSupportedException($"fixed pools are not supported auto generated pools.{typeof(T)}");
		}
		EcsPool<T> ecsPool = new EcsPool<T>(this, _poolsCount, _poolDenseSize, GetWorldSize(), _poolRecycledSize);
		_poolHashes[typeFromHandle] = ecsPool;
		EnsurePoolSize(_poolsCount);
		_pools[_poolsCount++] = ecsPool;
		return ecsPool;
	}

	public EcsWorld InitPool<T>() where T : struct
	{
		if (!_fixedPoolOrder)
		{
			throw new NotSupportedException("auto generated pools are not supported when fixed pool order is enabled.");
		}
		if (_poolHashes.ContainsKey(typeof(T)))
		{
			throw new Exception($"Pool already exists. {typeof(T)}");
		}
		EcsPool<T> ecsPool = new EcsPool<T>(this, _poolsCount, _poolDenseSize, GetWorldSize(), _poolRecycledSize);
		_poolHashes[typeof(T)] = ecsPool;
		EnsurePoolSize(_poolsCount);
		_pools[_poolsCount++] = ecsPool;
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void EnsurePoolSize(int poolSize)
	{
		if (_pools.Length <= poolSize)
		{
			int newSize = poolSize << 1;
			Array.Resize(ref _pools, newSize);
			Array.Resize(ref _filtersByIncludedComponents, newSize);
			Array.Resize(ref _filtersByExcludedComponents, newSize);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEcsPool GetPoolById(int typeId)
	{
		if (typeId < 0 || typeId >= _poolsCount)
		{
			return null;
		}
		return _pools[typeId];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public IEcsPool GetPoolByType(Type type)
	{
		if (!_poolHashes.TryGetValue(type, out var value))
		{
			return null;
		}
		return value;
	}

	public int GetAllEntities(ref int[] entities)
	{
		int num = _entitiesCount - _recycledEntitiesCount;
		if (entities == null || entities.Length < num)
		{
			entities = new int[num];
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int entitiesCount = _entitiesCount;
		while (num4 < entitiesCount)
		{
			if (_entities[num3 + 1] > 0 && _entities[num3] >= 0)
			{
				entities[num2++] = num4;
			}
			num4++;
			num3 += _entitiesItemSize;
		}
		return num;
	}

	public int GetAllPools(ref IEcsPool[] pools)
	{
		short poolsCount = _poolsCount;
		if (pools == null || pools.Length < poolsCount)
		{
			pools = new IEcsPool[poolsCount];
		}
		Array.Copy(_pools, 0, pools, 0, _poolsCount);
		return _poolsCount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Mask Filter<T>() where T : struct
	{
		return ((_masksCount > 0) ? _masks[--_masksCount] : new Mask(this)).Inc<T>();
	}

	public int GetComponents(int entity, ref object[] list)
	{
		int rawEntityOffset = GetRawEntityOffset(entity);
		short num = _entities[rawEntityOffset];
		if (num == 0)
		{
			return 0;
		}
		if (list == null || list.Length < num)
		{
			list = new object[_pools.Length];
		}
		int num2 = rawEntityOffset + 2;
		for (int i = 0; i < num; i++)
		{
			list[i] = _pools[_entities[num2 + i]].GetRaw(entity);
		}
		return num;
	}

	public int GetComponentTypes(int entity, ref Type[] list)
	{
		int rawEntityOffset = GetRawEntityOffset(entity);
		short num = _entities[rawEntityOffset];
		if (num == 0)
		{
			return 0;
		}
		if (list == null || list.Length < num)
		{
			list = new Type[_pools.Length];
		}
		int num2 = rawEntityOffset + 2;
		for (int i = 0; i < num; i++)
		{
			list[i] = _pools[_entities[num2 + i]].GetComponentType();
		}
		return num;
	}

	public void CopyEntity(int srcEntity, int dstEntity)
	{
		int rawEntityOffset = GetRawEntityOffset(srcEntity);
		short num = _entities[rawEntityOffset];
		if (num > 0)
		{
			int num2 = rawEntityOffset + 2;
			for (int i = 0; i < num; i++)
			{
				_pools[_entities[num2 + i]].Copy(srcEntity, dstEntity);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsEntityAliveInternal(int entity)
	{
		if (entity >= 0 && entity < _entitiesCount)
		{
			return _entities[GetRawEntityOffset(entity) + 1] > 0;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void AddComponentToRawEntityInternal(int entity, short poolId)
	{
		int rawEntityOffset = GetRawEntityOffset(entity);
		short num = _entities[rawEntityOffset];
		if (num + 2 == _entitiesItemSize)
		{
			ExtendEntitiesCache();
			rawEntityOffset = GetRawEntityOffset(entity);
		}
		_entities[rawEntityOffset]++;
		_entities[rawEntityOffset + 2 + num] = poolId;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal short RemoveComponentFromRawEntityInternal(int entity, short poolId)
	{
		int rawEntityOffset = GetRawEntityOffset(entity);
		short num = _entities[rawEntityOffset];
		num--;
		_entities[rawEntityOffset] = num;
		int num2 = rawEntityOffset + 2;
		for (int i = 0; i <= num; i++)
		{
			if (_entities[num2 + i] == poolId)
			{
				if (i < num)
				{
					_entities[num2 + i] = _entities[num2 + num];
				}
				return num;
			}
		}
		return 0;
	}

	private void ExtendEntitiesCache()
	{
		int num = 2 + (_entitiesItemSize - 2 << 1);
		short[] array = new short[GetWorldSize() * num];
		int num2 = 0;
		int num3 = 0;
		int i = 0;
		for (int entitiesCount = _entitiesCount; i < entitiesCount; i++)
		{
			int num4 = _entities[num2] + 2;
			for (int j = 0; j < num4; j++)
			{
				array[num3 + j] = _entities[num2 + j];
			}
			num2 += _entitiesItemSize;
			num3 += num;
		}
		_entitiesItemSize = num;
		_entities = array;
	}

	private (EcsFilter, bool) GetFilterInternal(Mask mask, int capacity = 512)
	{
		int hash = mask.Hash;
		if (_hashedFilters.TryGetValue(hash, out var value))
		{
			return (value, false);
		}
		value = new EcsFilter(this, mask, capacity, GetWorldSize());
		_hashedFilters[hash] = value;
		_allFilters.Add(value);
		int i = 0;
		for (int includeCount = mask.IncludeCount; i < includeCount; i++)
		{
			List<EcsFilter> list = _filtersByIncludedComponents[mask.Include[i]];
			if (list == null)
			{
				list = new List<EcsFilter>(8);
				_filtersByIncludedComponents[mask.Include[i]] = list;
			}
			list.Add(value);
		}
		int j = 0;
		for (int excludeCount = mask.ExcludeCount; j < excludeCount; j++)
		{
			List<EcsFilter> list2 = _filtersByExcludedComponents[mask.Exclude[j]];
			if (list2 == null)
			{
				list2 = new List<EcsFilter>(8);
				_filtersByExcludedComponents[mask.Exclude[j]] = list2;
			}
			list2.Add(value);
		}
		int k = 0;
		for (int entitiesCount = _entitiesCount; k < entitiesCount; k++)
		{
			if (_entities[GetRawEntityOffset(k)] > 0 && IsMaskCompatible(mask, k))
			{
				value.AddEntity(k);
			}
		}
		return (value, true);
	}

	public void OnEntityChangeInternal(int entity, short componentType, bool added)
	{
		List<EcsFilter> list = _filtersByIncludedComponents[componentType];
		List<EcsFilter> list2 = _filtersByExcludedComponents[componentType];
		if (added)
		{
			if (list != null)
			{
				foreach (EcsFilter item in list)
				{
					if (IsMaskCompatible(item.GetMask(), entity))
					{
						item.AddEntity(entity);
					}
				}
			}
			if (list2 == null)
			{
				return;
			}
			{
				foreach (EcsFilter item2 in list2)
				{
					if (IsMaskCompatibleWithout(item2.GetMask(), entity, componentType))
					{
						item2.RemoveEntity(entity);
					}
				}
				return;
			}
		}
		if (list != null)
		{
			foreach (EcsFilter item3 in list)
			{
				if (IsMaskCompatible(item3.GetMask(), entity))
				{
					item3.RemoveEntity(entity);
				}
			}
		}
		if (list2 == null)
		{
			return;
		}
		foreach (EcsFilter item4 in list2)
		{
			if (IsMaskCompatibleWithout(item4.GetMask(), entity, componentType))
			{
				item4.AddEntity(entity);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool IsMaskCompatible(Mask filterMask, int entity)
	{
		int i = 0;
		for (int includeCount = filterMask.IncludeCount; i < includeCount; i++)
		{
			if (!_pools[filterMask.Include[i]].Has(entity))
			{
				return false;
			}
		}
		int j = 0;
		for (int excludeCount = filterMask.ExcludeCount; j < excludeCount; j++)
		{
			if (_pools[filterMask.Exclude[j]].Has(entity))
			{
				return false;
			}
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool IsMaskCompatibleWithout(Mask filterMask, int entity, int componentId)
	{
		int i = 0;
		for (int includeCount = filterMask.IncludeCount; i < includeCount; i++)
		{
			int num = filterMask.Include[i];
			if (num == componentId || !_pools[num].Has(entity))
			{
				return false;
			}
		}
		int j = 0;
		for (int excludeCount = filterMask.ExcludeCount; j < excludeCount; j++)
		{
			int num2 = filterMask.Exclude[j];
			if (num2 != componentId && _pools[num2].Has(entity))
			{
				return false;
			}
		}
		return true;
	}
}
