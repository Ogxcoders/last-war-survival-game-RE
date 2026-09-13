using System.Collections.Generic;
using UnityEngine;

public class SortHelpHeap
{
	private PointInfo[] heap;

	private int Size;

	private int capacity;

	private Vector2Int _comparePoint;

	public SortHelpHeap(int capacity)
	{
		if (heap == null)
		{
			heap = new PointInfo[capacity];
		}
		this.capacity = capacity;
	}

	public void GetSortList(Vector2Int cameraPoint, Dictionary<int, PointInfo> sortList, List<PointInfo> outList)
	{
		_comparePoint = cameraPoint;
		Size = 0;
		foreach (KeyValuePair<int, PointInfo> sort in sortList)
		{
			Add(sort.Value);
		}
		for (int i = 0; i < Size; i++)
		{
			PointInfo pointInfo = heap[i];
			outList.Add(pointInfo);
			sortList.Remove(pointInfo.pointIndex);
		}
	}

	private void Add(PointInfo node)
	{
		if (Size == 0)
		{
			heap[0] = node;
			Size++;
		}
		else if (Size == capacity)
		{
			ProcessFullHeap(node);
		}
		else if (Size < capacity)
		{
			heap[Size] = node;
			int num = Size - 1 >> 1;
			int num2 = Size;
			while (num >= 0 && !comparePoint(heap[num].pointIndex, heap[num2].pointIndex))
			{
				Swap(ref heap[num], ref heap[num2]);
				num2 = num;
				num = num2 - 1 >> 1;
			}
			Size++;
		}
	}

	private bool comparePoint(int index1, int index2)
	{
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(index1);
		Vector2Int vector2Int2 = SceneManager.World.IndexToTilePos(index2);
		int num = fastAbs(vector2Int.x - _comparePoint.x) + fastAbs(vector2Int.y - _comparePoint.y);
		int num2 = fastAbs(vector2Int2.x - _comparePoint.x) + fastAbs(vector2Int2.y - _comparePoint.y);
		return num > num2;
	}

	private void ProcessFullHeap(PointInfo node)
	{
		if (comparePoint(node.pointIndex, heap[0].pointIndex))
		{
			return;
		}
		heap[0] = node;
		int num = 0;
		int num2 = (num << 1) + 1;
		int num3 = (num << 1) + 2;
		while (num2 < Size)
		{
			_ = heap[num];
			int num4 = num2;
			if (num3 < Size && !comparePoint(heap[num2].pointIndex, heap[num3].pointIndex))
			{
				num4 = num3;
			}
			if (!comparePoint(heap[num].pointIndex, heap[num4].pointIndex))
			{
				Swap(ref heap[num], ref heap[num4]);
				num = num4;
				num2 = (num << 1) + 1;
				num3 = (num << 1) + 2;
				continue;
			}
			break;
		}
	}

	private void Swap(ref PointInfo a, ref PointInfo b)
	{
		PointInfo pointInfo = a;
		a = b;
		b = pointInfo;
	}

	public static int fastAbs(int value)
	{
		return (value ^ (value >> 31)) - (value >> 31);
	}
}
