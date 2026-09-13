using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class QuadTreeNode<T>
{
	private Rect _rect;

	private List<QuadTreeData<T>> _objects;

	private QuadTreeNode<T> _parent;

	private QuadTreeNode<T> _childBl;

	private QuadTreeNode<T> _childTl;

	private QuadTreeNode<T> _childTr;

	private QuadTreeNode<T> _childBr;

	private QuadTreeConfig _treeCfg;

	public Rect quadRect => _rect;

	public List<QuadTreeData<T>> dataList
	{
		get
		{
			return _objects;
		}
		set
		{
			_objects = value;
		}
	}

	public int objectCount
	{
		get
		{
			int num = _objects?.Count ?? 0;
			if (_childTl != null)
			{
				num += _childTl.objectCount + _childTr.objectCount + _childBl.objectCount + _childBr.objectCount;
			}
			return num;
		}
	}

	public int nodeCount
	{
		get
		{
			int num = ((_childTl != null) ? 4 : 0);
			if (_childTl != null)
			{
				num += _childTl.nodeCount + _childTr.nodeCount + _childBl.nodeCount + _childBr.nodeCount;
			}
			return num;
		}
	}

	public int depth
	{
		get
		{
			if (_childTl == null)
			{
				return 0;
			}
			return Mathf.Max(Mathf.Max(Mathf.Max(_childTl.depth, _childTr.depth), _childBl.depth), _childBr.depth) + 1;
		}
	}

	public int offsetToRoot
	{
		get
		{
			if (_parent == null)
			{
				return 0;
			}
			return _parent.offsetToRoot + 1;
		}
	}

	public bool isEmpty
	{
		get
		{
			if (_childTl == null)
			{
				if (_objects != null)
				{
					return _objects.Count == 0;
				}
				return true;
			}
			return false;
		}
	}

	public QuadTreeNode<T> childTl => _childTl;

	public QuadTreeNode<T> childTr => _childTr;

	public QuadTreeNode<T> childBl => _childBl;

	public QuadTreeNode<T> childBr => _childBr;

	public QuadTreeNode<T> parent
	{
		get
		{
			return _parent;
		}
		internal set
		{
			_parent = value;
		}
	}

	public QuadTreeNode(Rect rect, QuadTreeNode<T> parent, QuadTreeConfig cfg)
	{
		_rect = rect;
		_parent = parent;
		_treeCfg = cfg;
		if (_treeCfg != null && !_treeCfg.isDynamic)
		{
			Subdivide();
		}
	}

	public QuadTreeNode(float x, float y, float width, float height, QuadTreeNode<T> parent, QuadTreeConfig cfg)
	{
		_rect = new Rect(x, y, width, height);
		_parent = parent;
		_treeCfg = cfg;
		if (_treeCfg != null && !_treeCfg.isDynamic)
		{
			Subdivide();
		}
	}

	public void InitObjSize(int reserveSize)
	{
		if (_objects == null)
		{
			_objects = new List<QuadTreeData<T>>(reserveSize);
		}
		else if (_objects.Capacity < reserveSize)
		{
			_objects.Capacity = reserveSize;
		}
	}

	private void Add(T objData, Vector2 objPos)
	{
		if (_objects == null)
		{
			_objects = new List<QuadTreeData<T>>(_treeCfg.maxObjectsPerNode);
		}
		QuadTreeData<T> quadTreeData = new QuadTreeData<T>();
		quadTreeData.data = objData;
		quadTreeData.point = objPos;
		quadTreeData.dataIndex = _objects.Count;
		quadTreeData.owner = this;
		_objects.Add(quadTreeData);
	}

	private void Add(QuadTreeData<T> obj)
	{
		if (_objects == null)
		{
			_objects = new List<QuadTreeData<T>>(_treeCfg.maxObjectsPerNode);
		}
		obj.dataIndex = _objects.Count;
		obj.owner = this;
		_objects.Add(obj);
	}

	private void Add(List<T> objDataList, List<Vector2> objPosList)
	{
		int count = objDataList.Count;
		if (_objects == null)
		{
			_objects = new List<QuadTreeData<T>>(count);
		}
		for (int i = 0; i < count; i++)
		{
			QuadTreeData<T> quadTreeData = new QuadTreeData<T>();
			quadTreeData.data = objDataList[i];
			quadTreeData.point = objPosList[i];
			quadTreeData.dataIndex = _objects.Count;
			quadTreeData.owner = this;
			_objects.Add(quadTreeData);
		}
	}

	public bool Remove(QuadTreeData<T> quadTreeObj)
	{
		if (_objects != null)
		{
			int count = _objects.Count;
			int dataIndex = quadTreeObj.dataIndex;
			if (dataIndex != -1)
			{
				if (dataIndex != count - 1)
				{
					QuadTreeData<T> quadTreeData = _objects[count - 1];
					_objects[dataIndex] = quadTreeData;
					quadTreeData.dataIndex = dataIndex;
				}
				_objects.RemoveAt(count - 1);
				return true;
			}
		}
		return false;
	}

	public QuadTreeNode<T> GetLeafNodeAtPosition(ref Vector2 position)
	{
		QuadTreeNode<T> quadTreeNode = null;
		if (_rect.Contains(position))
		{
			if (_childTl == null)
			{
				quadTreeNode = this;
			}
			else
			{
				quadTreeNode = _childBl.GetLeafNodeAtPosition(ref position);
				if (quadTreeNode == null)
				{
					quadTreeNode = _childBr.GetLeafNodeAtPosition(ref position);
					if (quadTreeNode == null)
					{
						quadTreeNode = _childTl.GetLeafNodeAtPosition(ref position);
						if (quadTreeNode == null)
						{
							quadTreeNode = _childTr.GetLeafNodeAtPosition(ref position);
						}
					}
				}
			}
		}
		return quadTreeNode;
	}

	public void GetObjects(Rect range, List<QuadTreeData<T>> resultList)
	{
		if (range.Contains(_rect))
		{
			GetAllObjects(resultList);
		}
		else
		{
			if (!range.Overlaps(_rect))
			{
				return;
			}
			if (_objects != null)
			{
				int count = _objects.Count;
				for (int i = 0; i < count; i++)
				{
					QuadTreeData<T> quadTreeData = _objects[i];
					if (range.Overlaps(quadTreeData.asRect))
					{
						resultList.Add(quadTreeData);
					}
				}
			}
			if (_childTl != null)
			{
				_childTl.GetObjects(range, resultList);
				_childTr.GetObjects(range, resultList);
				_childBl.GetObjects(range, resultList);
				_childBr.GetObjects(range, resultList);
			}
		}
	}

	private void GetAllObjects(List<QuadTreeData<T>> objList)
	{
		if (_objects != null)
		{
			objList.AddRange(_objects);
		}
		if (_childTl != null)
		{
			_childTl.GetAllObjects(objList);
			_childTr.GetAllObjects(objList);
			_childBl.GetAllObjects(objList);
			_childBr.GetAllObjects(objList);
		}
	}

	private void Subdived(Vector2 point, bool recursive = true)
	{
		_childBl = new QuadTreeNode<T>(_rect.xMin, _rect.yMin, point.x - _rect.xMin, point.y - _rect.yMin, this, _treeCfg);
		_childTl = new QuadTreeNode<T>(_rect.xMin, point.y, point.x - _rect.xMin, _rect.yMax - point.y, this, _treeCfg);
		_childTr = new QuadTreeNode<T>(point.x, point.y, _rect.xMax - point.x, _rect.yMax - point.y, this, _treeCfg);
		_childBr = new QuadTreeNode<T>(point.x, _rect.yMin, _rect.xMax - point.x, point.y - _rect.yMin, this, _treeCfg);
		if (_objects != null)
		{
			int count = _objects.Count;
			for (int i = 0; i < count; i++)
			{
				Insert(_objects[i], recursive);
			}
			_objects = null;
		}
	}

	private void Subdivide(bool recursive = true)
	{
		float num = _rect.width * _rect.height;
		if (!(num < _treeCfg.minmumQuad) && !float.IsInfinity(num))
		{
			Vector2 point = new Vector2(_rect.xMin + _rect.width / 2f, _rect.yMin + _rect.height / 2f);
			Subdived(point, recursive);
		}
	}

	public void Insert(QuadTreeData<T> item, bool canSubdivided = true)
	{
		if (!quadRect.Contains(item.point))
		{
			if (_parent != null)
			{
				_parent.Insert(item, canSubdivided);
			}
		}
		else if (_treeCfg.isDynamic)
		{
			if ((_objects == null || (_childTl == null && _objects.Count + 1 <= _treeCfg.maxObjectsPerNode)) && offsetToRoot >= _treeCfg.minmumAddDataDepthToRoot)
			{
				Add(item);
				return;
			}
			if (_childTl == null)
			{
				if (!canSubdivided)
				{
					Add(item);
					return;
				}
				Subdivide();
			}
			QuadTreeNode<T> destinationTreeNode = GetDestinationTreeNode(item);
			if (destinationTreeNode == this)
			{
				Add(item);
			}
			else
			{
				destinationTreeNode.Insert(item, canSubdivided);
			}
		}
		else if (_childTl == null)
		{
			Add(item);
		}
		else
		{
			QuadTreeNode<T> destinationTreeNode2 = GetDestinationTreeNode(item);
			if (destinationTreeNode2 == this)
			{
				Add(item);
			}
			else
			{
				destinationTreeNode2.Insert(item, canSubdivided);
			}
		}
	}

	private QuadTreeNode<T> GetDestinationTreeNode(QuadTreeData<T> item)
	{
		if (_childTl == null)
		{
			return this;
		}
		if (_childTl.ContainObject(item))
		{
			return _childTl;
		}
		if (_childTr.ContainObject(item))
		{
			return _childTr;
		}
		if (_childBl.ContainObject(item))
		{
			return _childBl;
		}
		if (_childBr.ContainObject(item))
		{
			return _childBr;
		}
		return this;
	}

	private bool ContainObject(QuadTreeData<T> item)
	{
		return _rect.Contains(item.point);
	}

	private IEnumerable<QuadTreeNode<T>> GetChildren()
	{
		if (childTl != null)
		{
			yield return childTl;
			yield return childTr;
			yield return childBl;
			yield return childBr;
		}
	}

	private void ClearChildren()
	{
		foreach (QuadTreeNode<T> child in GetChildren())
		{
			child.parent = null;
		}
		_childTl = (_childTr = (_childBl = (_childBr = null)));
	}

	public void Clear()
	{
		if (childTl != null)
		{
			ClearChildren();
		}
		if (_objects != null)
		{
			_objects = null;
		}
	}

	public void Visit(Action<QuadTreeNode<T>> visitFunc)
	{
		visitFunc(this);
		if (childTl == null)
		{
			return;
		}
		foreach (QuadTreeNode<T> child in GetChildren())
		{
			child.Visit(visitFunc);
		}
	}
}
