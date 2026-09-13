using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public sealed class EcsPool<T> : IEcsPool where T : struct
{
	private delegate void AutoResetHandler(ref T component, EcsWorld world, int entity);

	private delegate void AutoCopyHandler(ref T srcComponent, ref T dstComponent);

	private delegate object TakeSnapshotHandler(ref T c, EcsWorld world, int entity, object env);

	private delegate void RestoreSnapshotHandler(ref T c, EcsWorld world, int entity, object data, object env);

	private delegate bool IsSnapshotEqualHandler(object lhs, object rhs, EcsWorld world);

	private readonly Type _type;

	private readonly EcsWorld _world;

	private readonly AutoResetHandler _autoResetHandler;

	private readonly AutoCopyHandler _autoCopyHandler;

	private short _id;

	private T[] _denseItems;

	private int[] _sparseItems;

	private int _denseItemsCount;

	private int[] _recycledItems;

	private int _recycledItemsCount;

	private object _fakeInstance;

	private TakeSnapshotHandler _autoTakeSnapshotHandler;

	private RestoreSnapshotHandler _autoRestoreSnapshotHandler;

	private IsSnapshotEqualHandler _isSnapshotEqualHandler;

	public EcsPool(EcsWorld world, short id, int denseCapacity, int sparseCapacity, int recycledCapacity)
	{
		_type = typeof(T);
		_world = world;
		_id = id;
		_denseItems = new T[denseCapacity + 1];
		_sparseItems = new int[sparseCapacity];
		_denseItemsCount = 1;
		_recycledItems = new int[recycledCapacity];
		_recycledItemsCount = 0;
		Type type = _type;
		if (world.TryGetPoolDelegate<T>(out var poolDelegate))
		{
			type = poolDelegate.GetType();
			_fakeInstance = poolDelegate;
		}
		else
		{
			_fakeInstance = new T();
		}
		if (typeof(IEcsAutoReset<T>).IsAssignableFrom(type))
		{
			MethodInfo method = type.GetMethod("AutoReset");
			_autoResetHandler = (AutoResetHandler)Delegate.CreateDelegate(typeof(AutoResetHandler), _fakeInstance, method);
		}
		if (typeof(IEcsAutoCopy<T>).IsAssignableFrom(type))
		{
			MethodInfo method2 = type.GetMethod("AutoCopy");
			_autoCopyHandler = (AutoCopyHandler)Delegate.CreateDelegate(typeof(AutoCopyHandler), _fakeInstance, method2);
		}
		if (typeof(IEcsAutoSnapshot<T>).IsAssignableFrom(type))
		{
			MethodInfo method3 = type.GetMethod("TakeSnapshot");
			_autoTakeSnapshotHandler = (TakeSnapshotHandler)Delegate.CreateDelegate(typeof(TakeSnapshotHandler), _fakeInstance, method3);
			MethodInfo method4 = type.GetMethod("RestoreSnapshot");
			_autoRestoreSnapshotHandler = (RestoreSnapshotHandler)Delegate.CreateDelegate(typeof(RestoreSnapshotHandler), _fakeInstance, method4);
			MethodInfo method5 = type.GetMethod("IsSnapshotEqual");
			_isSnapshotEqualHandler = (IsSnapshotEqualHandler)Delegate.CreateDelegate(typeof(IsSnapshotEqualHandler), _fakeInstance, method5);
		}
	}

	private void ReflectionSupportHack()
	{
		_world.GetPool<T>();
		_world.Filter<T>().Exc<T>().End();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public EcsWorld GetWorld()
	{
		return _world;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetId()
	{
		return _id;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Type GetComponentType()
	{
		return _type;
	}

	void IEcsPool.Resize(int capacity)
	{
		Array.Resize(ref _sparseItems, capacity);
	}

	object IEcsPool.GetRaw(int entity)
	{
		return Get(entity);
	}

	void IEcsPool.SetRaw(int entity, object dataRaw)
	{
		_denseItems[_sparseItems[entity]] = (T)dataRaw;
	}

	void IEcsPool.AddRaw(int entity, object dataRaw)
	{
		Add(entity) = (T)dataRaw;
	}

	public T[] GetRawDenseItems()
	{
		return _denseItems;
	}

	public ref int GetRawDenseItemsCount()
	{
		return ref _denseItemsCount;
	}

	public int[] GetRawSparseItems()
	{
		return _sparseItems;
	}

	public int[] GetRawRecycledItems()
	{
		return _recycledItems;
	}

	public ref int GetRawRecycledItemsCount()
	{
		return ref _recycledItemsCount;
	}

	public ref T Add(int entity)
	{
		int num;
		if (_recycledItemsCount > 0)
		{
			num = _recycledItems[--_recycledItemsCount];
		}
		else
		{
			num = _denseItemsCount;
			if (_denseItemsCount == _denseItems.Length)
			{
				Array.Resize(ref _denseItems, _denseItemsCount << 1);
			}
			_denseItemsCount++;
			_autoResetHandler?.Invoke(ref _denseItems[num], _world, entity);
		}
		_sparseItems[entity] = num;
		_world.OnEntityChangeInternal(entity, _id, added: true);
		_world.AddComponentToRawEntityInternal(entity, _id);
		return ref _denseItems[num];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref T Get(int entity)
	{
		return ref _denseItems[_sparseItems[entity]];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref T GetOrAdd(int entity)
	{
		if (_sparseItems[entity] > 0)
		{
			return ref _denseItems[_sparseItems[entity]];
		}
		return ref Add(entity);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Has(int entity)
	{
		return _sparseItems[entity] > 0;
	}

	public void Del(int entity)
	{
		ref int reference = ref _sparseItems[entity];
		if (reference > 0)
		{
			_world.OnEntityChangeInternal(entity, _id, added: false);
			if (_recycledItemsCount == _recycledItems.Length)
			{
				Array.Resize(ref _recycledItems, _recycledItemsCount << 1);
			}
			_recycledItems[_recycledItemsCount++] = reference;
			if (_autoResetHandler != null)
			{
				_autoResetHandler(ref _denseItems[reference], _world, entity);
			}
			else
			{
				_denseItems[reference] = default(T);
			}
			reference = 0;
			if (_world.RemoveComponentFromRawEntityInternal(entity, _id) == 0)
			{
				_world.DelEntity(entity);
			}
		}
	}

	public void Copy(int srcEntity, int dstEntity)
	{
		if (Has(srcEntity))
		{
			ref T reference = ref Get(srcEntity);
			if (!Has(dstEntity))
			{
				Add(dstEntity);
			}
			ref T reference2 = ref Get(dstEntity);
			if (_autoCopyHandler != null)
			{
				_autoCopyHandler(ref reference, ref reference2);
			}
			else
			{
				reference2 = reference;
			}
		}
	}

	public object TakeSnapshot(int entity, object env)
	{
		ref T reference = ref Get(entity);
		if (_autoTakeSnapshotHandler != null)
		{
			return _autoTakeSnapshotHandler(ref reference, _world, entity, env);
		}
		return reference;
	}

	public void RestoreSnapshot(int entity, object data, object env, bool overwrite)
	{
		bool flag = Has(entity);
		if (!flag || overwrite)
		{
			ref T reference = ref flag ? ref Get(entity) : ref Add(entity);
			if (_autoRestoreSnapshotHandler != null)
			{
				_autoRestoreSnapshotHandler(ref reference, _world, entity, data, env);
			}
			else
			{
				reference = (T)data;
			}
		}
	}

	public bool IsSnapshotEqual(object lhs, object rhs)
	{
		if (_isSnapshotEqualHandler != null)
		{
			return _isSnapshotEqualHandler(lhs, rhs, _world);
		}
		return lhs.Equals(rhs);
	}

	public void RestoreSnapshotId(short id)
	{
		_id = id;
	}
}
