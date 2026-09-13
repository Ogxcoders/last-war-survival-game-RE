using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework;
using PVEBattleLogic.Unit;
using UnityEngine;
using XLua;

namespace PVEBattleLogic.Effect;

public static class EffectViewFacade
{
	private enum EffectType
	{
		Normal,
		Sprite
	}

	private static class ListPool
	{
		private static readonly Stack<List<long>> _listPool = new Stack<List<long>>();

		public static List<long> Get()
		{
			lock (_listPool)
			{
				return (_listPool.Count > 0) ? _listPool.Pop() : new List<long>();
			}
		}

		public static void Return(List<long> list)
		{
			if (list == null)
			{
				return;
			}
			list.Clear();
			lock (_listPool)
			{
				_listPool.Push(list);
			}
		}
	}

	public const int INVALID_HANDLE = -1;

	private static Dictionary<string, Stack<EffectGameObject>> _effectGameObjectPool;

	private static Dictionary<string, Stack<EffectSpriteObject>> _effectSpriteObjectPool;

	private static BattleIndexedList<EffectGameObject> _effectGameObjectList;

	private static Dictionary<long, int> _objIdToHandleMap;

	private static List<long> _toRemoveList;

	private static long _nextId = 0L;

	private static Dictionary<string, int> _preloadEffectGameObjectCache = new Dictionary<string, int>();

	private static HashSet<string> _preloadSet = new HashSet<string>();

	private static List<string> _toRemovePreload = new List<string>(8);

	private static LuaArrAccess _effectViewLuaArrAccess;

	private static Dictionary<int, string> _viewNameIdMap;

	private static Dictionary<int, List<long>> _parentViewHandleToHitEffectObjIdMap;

	private static Dictionary<long, int> _hitEffectObjIdToParentViewHandleMap;

	public static void Init()
	{
		if (_effectGameObjectPool == null)
		{
			_effectGameObjectPool = new Dictionary<string, Stack<EffectGameObject>>();
		}
		if (_effectSpriteObjectPool == null)
		{
			_effectSpriteObjectPool = new Dictionary<string, Stack<EffectSpriteObject>>();
		}
		if (_effectGameObjectList == null)
		{
			_effectGameObjectList = new BattleIndexedList<EffectGameObject>(4096);
		}
		if (_objIdToHandleMap == null)
		{
			_objIdToHandleMap = new Dictionary<long, int>(1024);
		}
		if (_toRemoveList == null)
		{
			_toRemoveList = new List<long>(256);
		}
		if (_viewNameIdMap == null)
		{
			_viewNameIdMap = new Dictionary<int, string>(64);
		}
		_nextId = 0L;
	}

	public static void InitEffectViewArrayAccess(LuaArrAccess luaArrAccess)
	{
		_effectViewLuaArrAccess = luaArrAccess;
		Init();
	}

	public static void UnInitEffectViewArrayAccess()
	{
		_effectViewLuaArrAccess = null;
	}

	public static void SyncNameId(string viewName, int viewId)
	{
		_viewNameIdMap[viewId] = viewName;
	}

	public static long ShowEffect(string path, float time, int type, Vector3 position, Quaternion rotation, Transform parent, float scale = 1f)
	{
		int value2;
		long num;
		if (type == 1)
		{
			EffectSpriteObject effectSpriteObject;
			if (_effectSpriteObjectPool.TryGetValue(path, out var value) && value.Count > 0)
			{
				effectSpriteObject = value.Pop();
				effectSpriteObject.OutPool();
				value2 = (effectSpriteObject.Handle = _effectGameObjectList.Add(effectSpriteObject));
				num = (effectSpriteObject.ObjId = ++_nextId);
				_objIdToHandleMap[num] = value2;
				effectSpriteObject.Show(position, rotation, time, parent);
				return num;
			}
			effectSpriteObject = new EffectSpriteObject(path);
			value2 = (effectSpriteObject.Handle = _effectGameObjectList.Add(effectSpriteObject));
			num = (effectSpriteObject.ObjId = ++_nextId);
			effectSpriteObject.Type = type;
			_objIdToHandleMap[num] = value2;
			effectSpriteObject.Show(position, rotation, time, parent);
			return num;
		}
		EffectGameObject effectGameObject;
		if (_effectGameObjectPool.TryGetValue(path, out var value3) && value3.Count > 0)
		{
			effectGameObject = value3.Pop();
			effectGameObject.OutPool();
			value2 = (effectGameObject.Handle = _effectGameObjectList.Add(effectGameObject));
			num = (effectGameObject.ObjId = ++_nextId);
			_objIdToHandleMap[num] = value2;
			effectGameObject.Show(position, rotation, time, parent, scale);
			return num;
		}
		effectGameObject = new EffectGameObject(path);
		value2 = (effectGameObject.Handle = _effectGameObjectList.Add(effectGameObject));
		num = (effectGameObject.ObjId = ++_nextId);
		effectGameObject.Type = type;
		_objIdToHandleMap[num] = value2;
		effectGameObject.Show(position, rotation, time, parent, scale);
		return num;
	}

	public static long ShowEffectLuaArray()
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float x = (float)_effectViewLuaArrAccess.GetDouble(4);
		float y = (float)_effectViewLuaArrAccess.GetDouble(5);
		float z = (float)_effectViewLuaArrAccess.GetDouble(6);
		float x2 = (float)_effectViewLuaArrAccess.GetDouble(7);
		float y2 = (float)_effectViewLuaArrAccess.GetDouble(8);
		float z2 = (float)_effectViewLuaArrAccess.GetDouble(9);
		float w = (float)_effectViewLuaArrAccess.GetDouble(10);
		return ShowEffect(viewName, time, type, new Vector3(x, y, z), new Quaternion(x2, y2, z2, w), null);
	}

	public static long ShowEffectParentLuaArray(Transform parent)
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float x = (float)_effectViewLuaArrAccess.GetDouble(4);
		float y = (float)_effectViewLuaArrAccess.GetDouble(5);
		float z = (float)_effectViewLuaArrAccess.GetDouble(6);
		float x2 = (float)_effectViewLuaArrAccess.GetDouble(7);
		float y2 = (float)_effectViewLuaArrAccess.GetDouble(8);
		float z2 = (float)_effectViewLuaArrAccess.GetDouble(9);
		float w = (float)_effectViewLuaArrAccess.GetDouble(10);
		return ShowEffect(viewName, time, type, new Vector3(x, y, z), new Quaternion(x2, y2, z2, w), parent);
	}

	public static long ShowEffectZeroPos(string path, float time, int type, Quaternion rotation, Transform parent)
	{
		return ShowEffect(path, time, type, Vector3.zero, rotation, parent);
	}

	public static long ShowEffectZeroPosLuaAccess()
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float x = (float)_effectViewLuaArrAccess.GetDouble(4);
		float y = (float)_effectViewLuaArrAccess.GetDouble(5);
		float z = (float)_effectViewLuaArrAccess.GetDouble(6);
		float w = (float)_effectViewLuaArrAccess.GetDouble(7);
		return ShowEffect(viewName, time, type, Vector3.zero, new Quaternion(x, y, z, w), null);
	}

	public static long ShowEffectZeroPosParentLuaAccess(Transform parent)
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float x = (float)_effectViewLuaArrAccess.GetDouble(4);
		float y = (float)_effectViewLuaArrAccess.GetDouble(5);
		float z = (float)_effectViewLuaArrAccess.GetDouble(6);
		float w = (float)_effectViewLuaArrAccess.GetDouble(7);
		return ShowEffect(viewName, time, type, Vector3.zero, new Quaternion(x, y, z, w), parent);
	}

	public static long ShowEffectZeroRot(string path, float time, int type, Vector3 position, Transform parent)
	{
		return ShowEffect(path, time, type, position, Quaternion.identity, parent);
	}

	public static long ShowEffectZeroRotLuaAccess()
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float x = (float)_effectViewLuaArrAccess.GetDouble(4);
		float y = (float)_effectViewLuaArrAccess.GetDouble(5);
		float z = (float)_effectViewLuaArrAccess.GetDouble(6);
		return ShowEffect(viewName, time, type, new Vector3(x, y, z), Quaternion.identity, null);
	}

	public static long ShowEffectZeroRotParentLuaAccess(Transform parent)
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float x = (float)_effectViewLuaArrAccess.GetDouble(4);
		float y = (float)_effectViewLuaArrAccess.GetDouble(5);
		float z = (float)_effectViewLuaArrAccess.GetDouble(6);
		return ShowEffect(viewName, time, type, new Vector3(x, y, z), Quaternion.identity, parent);
	}

	public static long ShowEffectOnlyParent(string path, float time, int type, Transform parent)
	{
		return ShowEffect(path, time, type, Vector3.zero, Quaternion.identity, parent);
	}

	private static bool CheckViewName(int viewId, out string viewName)
	{
		if (_viewNameIdMap.TryGetValue(viewId, out viewName))
		{
			return true;
		}
		Log.Error($"EffectViewFacade.CheckViewName {viewId} invalid !");
		return false;
	}

	public static long ShowEffectDefaultLuaAccess()
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		return ShowEffect(viewName, time, type, Vector3.zero, Quaternion.identity, null);
	}

	public static long ShowEffectDefaultParentLuaAccess(Transform parent)
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return -1L;
		}
		float time = (float)_effectViewLuaArrAccess.GetDouble(2);
		int type = _effectViewLuaArrAccess.GetInt(3);
		float scale = (float)_effectViewLuaArrAccess.GetDouble(4);
		return ShowEffect(viewName, time, type, Vector3.zero, Quaternion.identity, parent, scale);
	}

	public static void ShowHitEffectForViewTargetZeroRot()
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return;
		}
		Transform transform = UnitViewFacade.GetTransform(_effectViewLuaArrAccess.GetInt(7));
		if (!(transform == null))
		{
			float time = (float)_effectViewLuaArrAccess.GetDouble(2);
			int type = _effectViewLuaArrAccess.GetInt(3);
			float num = (float)_effectViewLuaArrAccess.GetDouble(4);
			Vector3 position;
			if (num < 0f)
			{
				position = Vector3.zero;
			}
			else
			{
				float y = (float)_effectViewLuaArrAccess.GetDouble(5);
				float z = (float)_effectViewLuaArrAccess.GetDouble(6);
				position = transform.InverseTransformPoint(new Vector3(num, y, z));
			}
			ShowEffect(viewName, time, type, position, Quaternion.identity, transform);
		}
	}

	public static void ShowHitEffectForViewTarget()
	{
		if (!CheckViewName(_effectViewLuaArrAccess.GetInt(1), out var viewName))
		{
			return;
		}
		int num = _effectViewLuaArrAccess.GetInt(11);
		Transform transform = UnitViewFacade.GetTransform(num);
		if (transform == null)
		{
			return;
		}
		int num2 = _effectViewLuaArrAccess.GetInt(12);
		if (num2 <= 0 || GetHitEffectShowNumByParentViewHandle(num) < num2)
		{
			float time = (float)_effectViewLuaArrAccess.GetDouble(2);
			int type = _effectViewLuaArrAccess.GetInt(3);
			float num3 = (float)_effectViewLuaArrAccess.GetDouble(4);
			Vector3 position;
			if (num3 < 0f)
			{
				position = Vector3.zero;
			}
			else
			{
				float y = (float)_effectViewLuaArrAccess.GetDouble(5);
				float z = (float)_effectViewLuaArrAccess.GetDouble(6);
				position = transform.InverseTransformPoint(new Vector3(num3, y, z));
			}
			float num4 = (float)_effectViewLuaArrAccess.GetDouble(7);
			float num5 = (float)_effectViewLuaArrAccess.GetDouble(8);
			float num6 = (float)_effectViewLuaArrAccess.GetDouble(9);
			int num7 = _effectViewLuaArrAccess.GetInt(10);
			long objId = ShowEffect(rotation: (num4 < float.Epsilon && num5 < float.Epsilon && num6 < float.Epsilon) ? ((num7 != 1) ? Quaternion.LookRotation(Vector3.back) : Quaternion.identity) : ((num7 != 1) ? Quaternion.LookRotation(new Vector3(0f - num4, 0f - num5, 0f - num6)) : Quaternion.LookRotation(new Vector3(num4, num5, num6))), path: viewName, time: time, type: type, position: position, parent: transform);
			if (num2 > 0)
			{
				AddHitEffectShowNum(objId, num);
			}
		}
	}

	public static void ShowEffectParents(long objId, Transform parent = null)
	{
		if (_objIdToHandleMap.TryGetValue(objId, out var value))
		{
			_effectGameObjectList[value]?.SetParentShow(parent);
		}
	}

	public static void ResetPosition(long objId, float x, float y, float z)
	{
		if (_objIdToHandleMap.TryGetValue(objId, out var value))
		{
			_effectGameObjectList[value]?.ResetPosition(new Vector3(x, y, z));
		}
	}

	public static void RemoveEffect(long objId)
	{
		_toRemoveList.Add(objId);
	}

	public static void Update(float deltaTime)
	{
		int count = _toRemoveList.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				long num = _toRemoveList[i];
				if (_objIdToHandleMap.TryGetValue(num, out var value))
				{
					RemoveEffectHandle(value);
					_objIdToHandleMap.Remove(num);
					TryRemoveHitEffectShowNum(num);
				}
			}
			_toRemoveList.Clear();
		}
		int count2 = _effectGameObjectList.Count;
		if (count2 == 0)
		{
			UpdatePreload();
			return;
		}
		for (int j = 0; j < count2; j++)
		{
			_effectGameObjectList[j]?.OnUpdate(deltaTime);
		}
		UpdatePreload();
	}

	private static void RemoveEffectHandle(int handle)
	{
		if (handle == -1)
		{
			return;
		}
		EffectGameObject effectGameObject = _effectGameObjectList.Remove(handle);
		if (effectGameObject == null || !CheckHandle(handle, effectGameObject))
		{
			return;
		}
		effectGameObject.InPool();
		effectGameObject.Handle = -1;
		effectGameObject.ObjId = -1L;
		string path = effectGameObject.Path;
		if (effectGameObject.Type == 1)
		{
			if (!_effectSpriteObjectPool.TryGetValue(path, out var value))
			{
				value = new Stack<EffectSpriteObject>(256);
				_effectSpriteObjectPool.Add(path, value);
			}
			value.Push(effectGameObject as EffectSpriteObject);
		}
		else
		{
			if (!_effectGameObjectPool.TryGetValue(path, out var value2))
			{
				value2 = new Stack<EffectGameObject>(256);
				_effectGameObjectPool.Add(path, value2);
			}
			value2.Push(effectGameObject);
		}
	}

	private static void UpdatePreload()
	{
		foreach (KeyValuePair<string, int> item in _preloadEffectGameObjectCache)
		{
			string key = item.Key;
			int value = item.Value;
			EffectGameObject effectGameObject = new EffectGameObject(key);
			effectGameObject.Type = 0;
			effectGameObject.Handle = -1;
			effectGameObject.ObjId = -1L;
			effectGameObject.Show(Vector3.zero, Quaternion.identity, 0f, null);
			effectGameObject.InPool();
			if (!_effectGameObjectPool.TryGetValue(key, out var value2))
			{
				value2 = new Stack<EffectGameObject>(256);
				_effectGameObjectPool.Add(key, value2);
			}
			value2.Push(effectGameObject);
			if (value - 1 == 0)
			{
				_toRemovePreload.Add(key);
			}
		}
		if (_toRemovePreload.Count > 0)
		{
			foreach (string item2 in _toRemovePreload)
			{
				_preloadEffectGameObjectCache.Remove(item2);
				_preloadSet.Remove(item2);
			}
			_toRemovePreload.Clear();
		}
		foreach (string item3 in _preloadSet)
		{
			if (_preloadEffectGameObjectCache.TryGetValue(item3, out var value3))
			{
				_preloadEffectGameObjectCache[item3] = value3 - 1;
			}
		}
		UnitViewFacade.UpdatePreload();
	}

	public static void PreloadEffectGameObject(string path, int count)
	{
		_preloadEffectGameObjectCache[path] = count;
		_preloadSet.Add(path);
	}

	public static void PreloadEffectGameObjectWhitId(int viewId, int count)
	{
		if (_viewNameIdMap.TryGetValue(viewId, out var value))
		{
			PreloadEffectGameObject(value, count);
		}
	}

	public static void PreloadEffectSpriteObject(string path, int count)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool CheckHandle(int handle, EffectGameObject effectGameObject)
	{
		if (effectGameObject.Handle != handle)
		{
			Log.Error("Handle is not the same as the effect gameObject , path = " + effectGameObject.Path + " ");
			return false;
		}
		return true;
	}

	private static void AddHitEffectShowNum(long objId, int parentViewHandle)
	{
		if (_parentViewHandleToHitEffectObjIdMap == null)
		{
			_parentViewHandleToHitEffectObjIdMap = new Dictionary<int, List<long>>();
		}
		if (_hitEffectObjIdToParentViewHandleMap == null)
		{
			_hitEffectObjIdToParentViewHandleMap = new Dictionary<long, int>();
		}
		_hitEffectObjIdToParentViewHandleMap[objId] = parentViewHandle;
		if (!_parentViewHandleToHitEffectObjIdMap.TryGetValue(parentViewHandle, out var value))
		{
			value = ListPool.Get();
			_parentViewHandleToHitEffectObjIdMap.Add(parentViewHandle, value);
		}
		value.Add(objId);
	}

	private static void TryRemoveHitEffectShowNum(long objId)
	{
		if (_hitEffectObjIdToParentViewHandleMap == null || !_hitEffectObjIdToParentViewHandleMap.TryGetValue(objId, out var value))
		{
			return;
		}
		if (_parentViewHandleToHitEffectObjIdMap != null && _parentViewHandleToHitEffectObjIdMap.TryGetValue(value, out var value2))
		{
			if (value2.Count > 0)
			{
				value2.Remove(objId);
			}
			if (value2.Count == 0)
			{
				ListPool.Return(value2);
				_parentViewHandleToHitEffectObjIdMap.Remove(value);
			}
		}
		_hitEffectObjIdToParentViewHandleMap.Remove(objId);
	}

	public static void RemoveAllHitEffectByParentViewHandle(int parentHandle)
	{
		if (_parentViewHandleToHitEffectObjIdMap != null && _parentViewHandleToHitEffectObjIdMap.TryGetValue(parentHandle, out var value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				long num = value[i];
				_toRemoveList.Add(num);
				_hitEffectObjIdToParentViewHandleMap?.Remove(num);
			}
			ListPool.Return(value);
			_parentViewHandleToHitEffectObjIdMap.Remove(parentHandle);
		}
	}

	private static int GetHitEffectShowNumByParentViewHandle(int parentViewHandle)
	{
		if (_parentViewHandleToHitEffectObjIdMap == null)
		{
			return 0;
		}
		if (!_parentViewHandleToHitEffectObjIdMap.TryGetValue(parentViewHandle, out var value))
		{
			return 0;
		}
		return value.Count;
	}

	public static void ClearAll()
	{
		if (_effectGameObjectPool != null)
		{
			foreach (KeyValuePair<string, Stack<EffectGameObject>> item in _effectGameObjectPool)
			{
				Stack<EffectGameObject> value = item.Value;
				if (value.Count <= 0)
				{
					continue;
				}
				foreach (EffectGameObject item2 in value)
				{
					item2?.Dispose();
				}
				value.Clear();
			}
			_effectGameObjectPool.Clear();
		}
		if (_effectSpriteObjectPool != null)
		{
			foreach (KeyValuePair<string, Stack<EffectSpriteObject>> item3 in _effectSpriteObjectPool)
			{
				Stack<EffectSpriteObject> value2 = item3.Value;
				if (value2.Count <= 0)
				{
					continue;
				}
				foreach (EffectSpriteObject item4 in value2)
				{
					item4?.Dispose();
				}
				value2.Clear();
			}
			_effectSpriteObjectPool.Clear();
		}
		if (_effectGameObjectList != null)
		{
			int count = _effectGameObjectList.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					_effectGameObjectList[i]?.Dispose();
				}
				_effectGameObjectList.Clear();
			}
		}
		if (_toRemoveList != null)
		{
			_toRemoveList.Clear();
		}
		if (_objIdToHandleMap != null)
		{
			_objIdToHandleMap.Clear();
		}
		_preloadEffectGameObjectCache.Clear();
		_toRemovePreload.Clear();
		_preloadSet.Clear();
		_hitEffectObjIdToParentViewHandleMap?.Clear();
		_parentViewHandleToHitEffectObjIdMap?.Clear();
	}

	public static void Dispose()
	{
		ClearAll();
		UnInitEffectViewArrayAccess();
	}

	public static void TestLuaCSharpArray(LuaArrAccess luaArrAccess)
	{
		uint arrayCapacity = luaArrAccess.GetArrayCapacity();
		for (int i = 1; i <= arrayCapacity; i++)
		{
			luaArrAccess.GetLong(i);
		}
	}

	public static void TestLuaTable(LuaTable table)
	{
		int length = table.Length;
		for (int i = 1; i < length; i++)
		{
			table.Get<int, int>(i);
		}
	}
}
