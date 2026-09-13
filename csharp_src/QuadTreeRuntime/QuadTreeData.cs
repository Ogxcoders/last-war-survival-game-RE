using UnityEngine;

public class QuadTreeData<T>
{
	private T _data;

	private QuadTreeNode<T> _owner;

	private Vector2 _point;

	private Vector2 _radius = Vector2.zero;

	private int _index = -1;

	private int _flag;

	public T data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	internal QuadTreeNode<T> owner
	{
		get
		{
			return _owner;
		}
		set
		{
			_owner = value;
		}
	}

	internal Vector2 point
	{
		get
		{
			return _point;
		}
		set
		{
			_point = value;
		}
	}

	internal Vector2 radius
	{
		get
		{
			return _radius;
		}
		set
		{
			_radius = value;
		}
	}

	internal float maxRadius => Mathf.Max(_radius.x, _radius.y);

	public Rect asRect => new Rect(_point.x - _radius.x, _point.y - _radius.y, _radius.x + _radius.x, _radius.y + _radius.y);

	public int dataIndex
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public int flag
	{
		get
		{
			return _flag;
		}
		set
		{
			_flag = value;
		}
	}

	public void OnFree()
	{
		_flag = 0;
		_index = -1;
	}
}
