using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class QuadTree<T>
{
	private readonly QuadTreeNode<T> _root;

	private readonly QuadTreeConfig _treeCfg;

	private readonly Dictionary<T, QuadTreeData<T>> _objectMap;

	private readonly List<QuadTreeData<T>> _resultList = new List<QuadTreeData<T>>();

	public int objectCount => _objectMap.Count;

	public int nodeCount => _root.nodeCount;

	public int depth => _root.depth;

	public QuadTree(Rect range, QuadTreeConfig cfg = null)
	{
		_treeCfg = cfg ?? QuadTreeConfig.@default;
		_root = new QuadTreeNode<T>(range, null, _treeCfg);
		_objectMap = new Dictionary<T, QuadTreeData<T>>(_treeCfg.initObjectsCount);
	}

	public void Add(T data, Vector2 point, Vector2 radius)
	{
		QuadTreeData<T> quadTreeData = new QuadTreeData<T>();
		quadTreeData.data = data;
		quadTreeData.point = point;
		quadTreeData.radius = radius;
		_objectMap.Add(data, quadTreeData);
		_root.Insert(quadTreeData);
	}

	public void Add(T data, Vector2 point)
	{
		QuadTreeData<T> quadTreeData = new QuadTreeData<T>();
		quadTreeData.data = data;
		quadTreeData.point = point;
		_objectMap.Add(data, quadTreeData);
		_root.Insert(quadTreeData);
	}

	public void RegisterDataMap(T data, QuadTreeData<T> treeData)
	{
		_objectMap.Add(data, treeData);
	}

	public bool Remove(T data)
	{
		if (_objectMap.TryGetValue(data, out var value))
		{
			value.owner.Remove(value);
			_objectMap.Remove(data);
			value.OnFree();
			return true;
		}
		return false;
	}

	public QuadTreeNode<T> GetLeafNodeAtPosition(ref Vector2 position)
	{
		return _root.GetLeafNodeAtPosition(ref position);
	}

	public List<QuadTreeData<T>> GetObjects(Rect range)
	{
		_resultList.Clear();
		_root.GetObjects(range, _resultList);
		return _resultList;
	}

	public void Clear(Action<T> disposeFunc)
	{
		if (_root != null)
		{
			_root.Clear();
		}
		if (_objectMap == null)
		{
			return;
		}
		if (disposeFunc != null)
		{
			foreach (T key in _objectMap.Keys)
			{
				disposeFunc(key);
			}
		}
		_objectMap.Clear();
	}

	public bool Contains(T data)
	{
		return _objectMap.ContainsKey(data);
	}

	public QuadTreeData<T> GetObject(T data)
	{
		_objectMap.TryGetValue(data, out var value);
		return value;
	}

	public bool TryUpdateObjectPosition(T data, Vector2 pos)
	{
		if (_objectMap.TryGetValue(data, out var value))
		{
			if (value.point != pos)
			{
				Vector2 radius = value.radius;
				Remove(data);
				Add(data, pos, radius);
			}
			return true;
		}
		return false;
	}

	public void Visit(Action<QuadTreeNode<T>> visitFunc)
	{
		_root?.Visit(visitFunc);
	}
}
