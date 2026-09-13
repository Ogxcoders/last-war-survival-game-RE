using System.Collections.Generic;
using UnityEngine;

namespace LW_JPS;

public class JPSPathfinding
{
	private Vector2 Size;

	public int[,] map;

	private List<Point> openList = new List<Point>();

	private List<Point> closeList = new List<Point>();

	public List<Point> GizmosListForline = new List<Point>();

	private Point destination;

	private bool obstacleEndIsOn;

	public Point Destination
	{
		get
		{
			return destination;
		}
		set
		{
			if (value.x < 0 || (float)value.x > Size.x - 1f || value.y < 0 || (float)value.y > Size.y - 1f || map[value.x, value.y] == 1)
			{
				if (obstacleEndIsOn)
				{
					destination = value;
				}
				else
				{
					Debug.Log("设置错误");
				}
			}
			else
			{
				destination = value;
			}
		}
	}

	public void InitMap(Vector2Int size, Vector2Int[] Obstacles, Vector3 pos, bool obstacleEndIsOn)
	{
		int x = size.x;
		int y = size.y;
		Size.x = size.x;
		Size.y = size.y;
		map = new int[x, y];
		for (int i = 0; i < x; i++)
		{
			for (int j = 0; j < y; j++)
			{
				map[i, j] = 0;
			}
		}
		for (int k = 0; k < Obstacles.Length; k++)
		{
			if (Obstacles[k].x < map.GetLength(0) && Obstacles[k].y < map.GetLength(1))
			{
				map[Obstacles[k].x, Obstacles[k].y] = 1;
			}
		}
		this.obstacleEndIsOn = obstacleEndIsOn;
	}

	public void UpdateMap(Vector2Int[] Obstacles, int isOn)
	{
		for (int i = 0; i < Obstacles.Length; i++)
		{
			if (Obstacles[i].x < map.GetLength(0) && Obstacles[i].y < map.GetLength(1))
			{
				map[Obstacles[i].x, Obstacles[i].y] = isOn;
			}
		}
	}

	private float CalculateG(Point start, Point tarpoint)
	{
		float num = Vector2.Distance(new Vector2(start.x, start.y), new Vector2(tarpoint.x, tarpoint.y));
		float num2 = ((tarpoint.parent == null) ? 0f : tarpoint.parent.G);
		return num + num2;
	}

	private float CalculateH(Point point, Point end)
	{
		return Mathf.Abs(end.x - point.x) + Mathf.Abs(end.y - point.y);
	}

	private float CalculateF(Point point)
	{
		return point.G + point.H;
	}

	private Point FindleastF()
	{
		if (openList.Count > 0)
		{
			Point point = openList[0];
			{
				foreach (Point open in openList)
				{
					if (open.F < point.F)
					{
						point = open;
					}
				}
				return point;
			}
		}
		return null;
	}

	private bool isWalkable(float x, float y)
	{
		if (x < 0f || x > Size.x - 1f || y < 0f || y > Size.y - 1f)
		{
			return false;
		}
		if (map[(int)x, (int)y] == 1)
		{
			return false;
		}
		return true;
	}

	private Point LineSearch(Point current, Vector2 dir)
	{
		if (dir.magnitude == 0f)
		{
			Debug.Log("Error!");
			return null;
		}
		Point point = new Point(current.x + (int)dir.x, current.y + (int)dir.y);
		while (true)
		{
			if (point.x == destination.x && point.y == destination.y)
			{
				return point;
			}
			if (!isWalkable(point.x, point.y))
			{
				break;
			}
			if (dir.x != 0f && dir.y == 0f && ((!isWalkable(point.x, point.y + 1) && isWalkable((float)point.x + dir.x, point.y + 1) && isWalkable((float)point.x + dir.x, point.y)) || (!isWalkable(point.x, point.y - 1) && isWalkable((float)point.x + dir.x, point.y - 1) && isWalkable((float)point.x + dir.x, point.y))))
			{
				return point;
			}
			if (dir.y != 0f && dir.x == 0f && ((!isWalkable(point.x + 1, point.y) && isWalkable(point.x + 1, (float)point.y + dir.y) && isWalkable(point.x, (float)point.y + dir.y)) || (!isWalkable(point.x - 1, point.y) && isWalkable(point.x - 1, (float)point.y + dir.y) && isWalkable(point.x, (float)point.y + dir.y))))
			{
				return point;
			}
			point = new Point(point.x + (int)dir.x, point.y + (int)dir.y);
		}
		return null;
	}

	private Point isInList(List<Point> list, Point point)
	{
		foreach (Point item in list)
		{
			if (item.x == point.x && item.y == point.y)
			{
				return item;
			}
		}
		return null;
	}

	private void StraightSearch(Point _curPoint)
	{
		Point point = LineSearch(_curPoint, new Vector2(1f, 0f));
		if (point != null && isInList(closeList, point) == null)
		{
			point.parent = _curPoint;
			point.G = CalculateG(point, _curPoint);
			point.H = CalculateH(point, destination);
			point.F = CalculateF(point);
			openList.Add(point);
		}
		Point point2 = LineSearch(_curPoint, new Vector2(0f, 1f));
		if (point2 != null && isInList(closeList, point2) == null)
		{
			point2.parent = _curPoint;
			point2.G = CalculateG(point2, _curPoint);
			point2.H = CalculateH(point2, destination);
			point2.F = CalculateF(point2);
			openList.Add(point2);
		}
		Point point3 = LineSearch(_curPoint, new Vector2(-1f, 0f));
		if (point3 != null && isInList(closeList, point3) == null)
		{
			point3.parent = _curPoint;
			point3.G = CalculateG(point3, _curPoint);
			point3.H = CalculateH(point3, destination);
			point3.F = CalculateF(point3);
			openList.Add(point3);
		}
		Point point4 = LineSearch(_curPoint, new Vector2(0f, -1f));
		if (point4 != null && isInList(closeList, point4) == null)
		{
			point4.parent = _curPoint;
			point4.G = CalculateG(point4, _curPoint);
			point4.H = CalculateH(point4, destination);
			point4.F = CalculateF(point4);
			openList.Add(point4);
		}
	}

	private Point LineSearch2(Point _curPoint, Vector2 dir)
	{
		if (dir.magnitude == 0f)
		{
			Debug.Log("Error");
			return null;
		}
		Point point = new Point(_curPoint.x + (int)dir.x, _curPoint.y + (int)dir.y);
		if (point.x == destination.x && point.y == destination.y)
		{
			return point;
		}
		if (!isWalkable(point.x, point.y))
		{
			return null;
		}
		if (dir.x > 0f && dir.y > 0f)
		{
			if ((!isWalkable(point.x, (float)point.y - dir.y) && isWalkable(point.x + 1, (float)point.y - dir.y) && isWalkable(point.x + 1, point.y)) || (!isWalkable((float)point.x - dir.x, point.y) && isWalkable((float)point.x - dir.x, point.y + 1) && isWalkable(point.x, point.y + 1)))
			{
				return point;
			}
			Point point2 = LineSearch(point, new Vector2(dir.x, 0f));
			Point point3 = LineSearch(point, new Vector2(0f, dir.y));
			if (point2 != null || point3 != null)
			{
				return point;
			}
			if (LineSearch2(point, new Vector2(1f, 1f)) != null)
			{
				return point;
			}
		}
		else if (dir.x > 0f && dir.y < 0f)
		{
			if ((!isWalkable(point.x, (float)point.y - dir.y) && isWalkable(point.x + 1, (float)point.y - dir.y) && isWalkable(point.x + 1, point.y)) || (!isWalkable((float)point.x - dir.x, point.y) && isWalkable((float)point.x - dir.x, point.y - 1) && isWalkable(point.x, point.y - 1)))
			{
				return point;
			}
			Point point4 = LineSearch(point, new Vector2(dir.x, 0f));
			Point point5 = LineSearch(point, new Vector2(0f, dir.y));
			if (point4 != null || point5 != null)
			{
				return point;
			}
			if (LineSearch2(point, new Vector2(1f, -1f)) != null)
			{
				return point;
			}
		}
		else if (dir.x < 0f && dir.y > 0f)
		{
			if ((!isWalkable((float)point.x - dir.x, point.y) && isWalkable((float)point.x - dir.x, point.y + 1) && isWalkable(point.x, point.y + 1)) || (!isWalkable(point.x, (float)point.y - dir.y) && isWalkable(point.x - 1, (float)point.y - dir.y) && isWalkable(point.x - 1, point.y)))
			{
				return point;
			}
			Point point6 = LineSearch(point, new Vector2(dir.x, 0f));
			Point point7 = LineSearch(point, new Vector2(0f, dir.y));
			if (point6 != null || point7 != null)
			{
				return point;
			}
			if (LineSearch2(point, new Vector2(-1f, 1f)) != null)
			{
				return point;
			}
		}
		else if (dir.x < 0f && dir.y < 0f)
		{
			if ((!isWalkable((float)point.x - dir.x, point.y) && isWalkable((float)point.x - dir.x, point.y - 1) && isWalkable(point.x, point.y - 1)) || (!isWalkable(point.x, (float)point.y - dir.y) && isWalkable(point.x - 1, (float)point.y - dir.y) && isWalkable(point.x - 1, point.y)))
			{
				return point;
			}
			Point point8 = LineSearch(point, new Vector2(dir.x, 0f));
			Point point9 = LineSearch(point, new Vector2(0f, dir.y));
			if (point8 != null || point9 != null)
			{
				return point;
			}
			if (LineSearch2(point, new Vector2(-1f, -1f)) != null)
			{
				return point;
			}
		}
		return null;
	}

	private void DiagonalSearch(Point _curPoint)
	{
		Point point = LineSearch2(_curPoint, new Vector2(1f, 1f));
		if (point != null && isInList(closeList, point) == null)
		{
			point.parent = _curPoint;
			point.G = CalculateG(point, _curPoint);
			point.H = CalculateH(point, destination);
			point.F = CalculateF(point);
			openList.Add(point);
		}
		Point point2 = LineSearch2(_curPoint, new Vector2(1f, -1f));
		if (point2 != null && isInList(closeList, point2) == null)
		{
			point2.parent = _curPoint;
			point2.G = CalculateG(point2, _curPoint);
			point2.H = CalculateH(point2, destination);
			point2.F = CalculateF(point2);
			openList.Add(point2);
		}
		Point point3 = LineSearch2(_curPoint, new Vector2(-1f, 1f));
		if (point3 != null && isInList(closeList, point3) == null)
		{
			point3.parent = _curPoint;
			point3.G = CalculateG(point3, _curPoint);
			point3.H = CalculateH(point3, destination);
			point3.F = CalculateF(point3);
			openList.Add(point3);
		}
		Point point4 = LineSearch2(_curPoint, new Vector2(-1f, -1f));
		if (point4 != null && isInList(closeList, point4) == null)
		{
			point4.parent = _curPoint;
			point4.G = CalculateG(point4, _curPoint);
			point4.H = CalculateH(point4, destination);
			point4.F = CalculateF(point4);
			openList.Add(point4);
		}
	}

	private Point JPS_search(Point start)
	{
		openList.Add(start);
		while (openList.Count > 0)
		{
			Point point = FindleastF();
			openList.Remove(point);
			StraightSearch(point);
			DiagonalSearch(point);
			closeList.Add(point);
			Point point2 = isInList(openList, destination);
			if (point2 != null)
			{
				return point2;
			}
		}
		return null;
	}

	public List<Point> GetPath(Vector2Int start, Vector2Int end)
	{
		Point start2 = new Point((int)Mathf.Round(start.x), (int)Mathf.Round(start.y));
		Destination = new Point((int)Mathf.Round(end.x), (int)Mathf.Round(end.y));
		GizmosListForline.Clear();
		Point point = JPS_search(start2);
		List<Point> list = new List<Point>();
		while (point != null)
		{
			list.Add(point);
			GizmosListForline.Add(point);
			point = point.parent;
		}
		openList.Clear();
		closeList.Clear();
		return list;
	}
}
