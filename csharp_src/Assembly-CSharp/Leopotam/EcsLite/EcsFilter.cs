using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public sealed class EcsFilter
{
	public struct Enumerator : IDisposable
	{
		private readonly EcsFilter _filter;

		private readonly int[] _entities;

		private readonly int _count;

		private int _idx;

		public int Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _entities[_idx];
			}
		}

		public Enumerator(EcsFilter filter)
		{
			_filter = filter;
			_entities = filter._denseEntities;
			_count = filter._entitiesCount;
			_idx = -1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNext()
		{
			return ++_idx < _count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			_filter.Unlock();
		}
	}

	private struct DelayedOp
	{
		public bool Added;

		public int Entity;
	}

	private readonly EcsWorld _world;

	private readonly EcsWorld.Mask _mask;

	private int[] _denseEntities;

	private int _entitiesCount;

	internal int[] SparseEntities;

	private int _lockCount;

	private DelayedOp[] _delayedOps;

	private int _delayedOpsCount;

	internal EcsFilter(EcsWorld world, EcsWorld.Mask mask, int denseCapacity, int sparseCapacity)
	{
		_world = world;
		_mask = mask;
		_denseEntities = new int[denseCapacity];
		SparseEntities = new int[sparseCapacity];
		_entitiesCount = 0;
		_delayedOps = new DelayedOp[512];
		_delayedOpsCount = 0;
		_lockCount = 0;
	}

	internal void Clear()
	{
		Array.Clear(_denseEntities, 0, _denseEntities.Length);
		Array.Clear(SparseEntities, 0, SparseEntities.Length);
		Array.Clear(_delayedOps, 0, _delayedOps.Length);
		_entitiesCount = 0;
		_delayedOpsCount = 0;
		_lockCount = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public EcsWorld GetWorld()
	{
		return _world;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetEntitiesCount()
	{
		return _entitiesCount;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int[] GetRawEntities()
	{
		return _denseEntities;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int[] GetSparseIndex()
	{
		return SparseEntities;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Enumerator GetEnumerator()
	{
		_lockCount++;
		return new Enumerator(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ResizeSparseIndex(int capacity)
	{
		Array.Resize(ref SparseEntities, capacity);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal EcsWorld.Mask GetMask()
	{
		return _mask;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void AddEntity(int entity)
	{
		if (!AddDelayedOp(added: true, entity))
		{
			if (_entitiesCount == _denseEntities.Length)
			{
				Array.Resize(ref _denseEntities, _entitiesCount << 1);
			}
			_denseEntities[_entitiesCount++] = entity;
			SparseEntities[entity] = _entitiesCount;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void RemoveEntity(int entity)
	{
		if (!AddDelayedOp(added: false, entity))
		{
			int num = SparseEntities[entity] - 1;
			SparseEntities[entity] = 0;
			_entitiesCount--;
			if (num < _entitiesCount)
			{
				_denseEntities[num] = _denseEntities[_entitiesCount];
				SparseEntities[_denseEntities[num]] = num + 1;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool AddDelayedOp(bool added, int entity)
	{
		if (_lockCount <= 0)
		{
			return false;
		}
		if (_delayedOpsCount == _delayedOps.Length)
		{
			Array.Resize(ref _delayedOps, _delayedOpsCount << 1);
		}
		ref DelayedOp reference = ref _delayedOps[_delayedOpsCount++];
		reference.Added = added;
		reference.Entity = entity;
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Unlock()
	{
		_lockCount--;
		if (_lockCount != 0 || _delayedOpsCount <= 0)
		{
			return;
		}
		int i = 0;
		for (int delayedOpsCount = _delayedOpsCount; i < delayedOpsCount; i++)
		{
			ref DelayedOp reference = ref _delayedOps[i];
			if (reference.Added)
			{
				AddEntity(reference.Entity);
			}
			else
			{
				RemoveEntity(reference.Entity);
			}
		}
		_delayedOpsCount = 0;
	}

	public EcsFilterSnapshot TakeSnapshot()
	{
		EcsFilterSnapshot ecsFilterSnapshot = new EcsFilterSnapshot();
		ecsFilterSnapshot.Hash = _mask.Hash;
		ecsFilterSnapshot.DenseEntities = new int[_entitiesCount];
		Array.Copy(_denseEntities, ecsFilterSnapshot.DenseEntities, _entitiesCount);
		ecsFilterSnapshot.EntitiesCount = _entitiesCount;
		ecsFilterSnapshot.DenseEntitiesCapacity = _denseEntities.Length;
		ecsFilterSnapshot.SparseEntitiesCount = SparseEntities.Length;
		return ecsFilterSnapshot;
	}

	public void RestoreSnapshot(EcsFilterSnapshot snapshot)
	{
		_denseEntities = new int[snapshot.DenseEntitiesCapacity];
		Array.Copy(snapshot.DenseEntities, _denseEntities, snapshot.EntitiesCount);
		SparseEntities = new int[snapshot.SparseEntitiesCount];
		_entitiesCount = snapshot.EntitiesCount;
		for (int i = 0; i < snapshot.EntitiesCount; i++)
		{
			int num = snapshot.DenseEntities[i];
			SparseEntities[num] = i + 1;
		}
	}
}
