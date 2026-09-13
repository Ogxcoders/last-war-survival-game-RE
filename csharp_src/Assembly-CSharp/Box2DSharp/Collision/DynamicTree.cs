using System;
using System.Collections.Generic;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Dynamics.Internal;

namespace Box2DSharp.Collision;

public class DynamicTree
{
	public const int NullNode = -1;

	private int _freeList;

	private int _nodeCapacity;

	private int _nodeCount;

	private int _root;

	private TreeNode[] _treeNodes;

	private readonly Stack<int> _queryStack = new Stack<int>(256);

	private readonly Stack<int> _rayCastStack = new Stack<int>(256);

	public DynamicTree()
	{
		_root = -1;
		_nodeCapacity = 16;
		_nodeCount = 0;
		_treeNodes = new TreeNode[_nodeCapacity];
		for (int i = 0; i < _nodeCapacity; i++)
		{
			_treeNodes[i] = new TreeNode
			{
				Next = i + 1,
				Height = -1
			};
		}
		_treeNodes[_nodeCapacity - 1].Next = -1;
		_treeNodes[_nodeCapacity - 1].Height = -1;
		_freeList = 0;
	}

	private int AllocateNode()
	{
		if (_freeList == -1)
		{
			_ = _treeNodes;
			_nodeCapacity *= 2;
			Array.Resize(ref _treeNodes, _nodeCapacity);
			for (int i = _nodeCount; i < _nodeCapacity; i++)
			{
				_treeNodes[i] = new TreeNode
				{
					Next = i + 1,
					Height = -1
				};
			}
			_treeNodes[_nodeCapacity - 1].Next = -1;
			_treeNodes[_nodeCapacity - 1].Height = -1;
			_freeList = _nodeCount;
		}
		int freeList = _freeList;
		_freeList = _treeNodes[freeList].Next;
		ref TreeNode reference = ref _treeNodes[freeList];
		reference.Parent = -1;
		reference.Child1 = -1;
		reference.Child2 = -1;
		reference.Height = 0;
		reference.UserData = null;
		reference.Moved = false;
		_nodeCount++;
		return freeList;
	}

	private void FreeNode(int nodeId)
	{
		ref TreeNode reference = ref _treeNodes[nodeId];
		reference.Reset();
		reference.Next = _freeList;
		reference.Height = -1;
		_freeList = nodeId;
		_nodeCount--;
	}

	public int CreateProxy(in AABB aabb, object userData)
	{
		int num = AllocateNode();
		ref TreeNode reference = ref _treeNodes[num];
		FVector2 fVector = new FVector2(Settings.AABBExtension, Settings.AABBExtension);
		reference.AABB.LowerBound = aabb.LowerBound - fVector;
		reference.AABB.UpperBound = aabb.UpperBound + fVector;
		reference.UserData = userData;
		reference.Height = 0;
		reference.Moved = true;
		InsertLeaf(num);
		return num;
	}

	public void DestroyProxy(int proxyId)
	{
		RemoveLeaf(proxyId);
		FreeNode(proxyId);
	}

	public bool MoveProxy(int proxyId, in AABB aabb, in FVector2 displacement)
	{
		FVector2 fVector = new FVector2(Settings.AABBExtension, Settings.AABBExtension);
		AABB aABB = new AABB
		{
			LowerBound = aabb.LowerBound - fVector,
			UpperBound = aabb.UpperBound + fVector
		};
		FVector2 fVector2 = 4 * displacement;
		if (fVector2.X < FP.Zero)
		{
			ref FP x = ref aABB.LowerBound.X;
			x += fVector2.X;
		}
		else
		{
			ref FP x2 = ref aABB.UpperBound.X;
			x2 += fVector2.X;
		}
		if (fVector2.Y < FP.Zero)
		{
			ref FP y = ref aABB.LowerBound.Y;
			y += fVector2.Y;
		}
		else
		{
			ref FP y2 = ref aABB.UpperBound.Y;
			y2 += fVector2.Y;
		}
		ref TreeNode reference = ref _treeNodes[proxyId];
		ref AABB aABB2 = ref reference.AABB;
		if (aABB2.Contains(in aabb))
		{
			AABB aABB3 = new AABB
			{
				LowerBound = aABB.LowerBound - 4f * fVector,
				UpperBound = aABB.UpperBound + 4f * fVector
			};
			if (aABB3.Contains(in aABB2))
			{
				return false;
			}
		}
		RemoveLeaf(proxyId);
		reference.AABB = aABB;
		InsertLeaf(proxyId);
		reference.Moved = true;
		return true;
	}

	public object GetUserData(int proxyId)
	{
		return _treeNodes[proxyId].UserData;
	}

	public bool WasMoved(int proxyId)
	{
		return _treeNodes[proxyId].Moved;
	}

	public void ClearMoved(int proxyId)
	{
		_treeNodes[proxyId].Moved = false;
	}

	public AABB GetFatAABB(int proxyId)
	{
		return _treeNodes[proxyId].AABB;
	}

	public void Query(in ITreeQueryCallback callback, in AABB aabb)
	{
		Stack<int> queryStack = _queryStack;
		queryStack.Clear();
		queryStack.Push(_root);
		while (queryStack.Count > 0)
		{
			int num = queryStack.Pop();
			if (num == -1)
			{
				continue;
			}
			ref TreeNode reference = ref _treeNodes[num];
			if (!CollisionUtils.TestOverlap(in reference.AABB, aabb))
			{
				continue;
			}
			TreeNode treeNode = reference;
			if (treeNode.IsLeaf())
			{
				if (!callback.QueryCallback(num))
				{
					return;
				}
			}
			else
			{
				queryStack.Push(reference.Child1);
				queryStack.Push(reference.Child2);
			}
		}
		queryStack.Clear();
	}

	public void RayCast(in ITreeRayCastCallback inputCallback, in RayCastInput input)
	{
		FVector2 p = input.P1;
		FVector2 p2 = input.P2;
		FVector2 a = p2 - p;
		a.Normalize();
		FVector2 fVector = MathUtils.Cross(FP.One, in a);
		FVector2 value = FVector2.Abs(fVector);
		FP fP = input.MaxFraction;
		AABB b = default(AABB);
		FVector2 value2 = p + fP * (p2 - p);
		b.LowerBound = FVector2.Min(p, value2);
		b.UpperBound = FVector2.Max(p, value2);
		Stack<int> rayCastStack = _rayCastStack;
		rayCastStack.Clear();
		rayCastStack.Push(_root);
		while (rayCastStack.Count > 0)
		{
			int num = rayCastStack.Pop();
			if (num == -1)
			{
				continue;
			}
			ref TreeNode reference = ref _treeNodes[num];
			if (!CollisionUtils.TestOverlap(in reference.AABB, b))
			{
				continue;
			}
			AABB aABB = reference.AABB;
			FVector2 center = aABB.GetCenter();
			aABB = reference.AABB;
			FVector2 extents = aABB.GetExtents();
			if (FP.Abs(FVector2.Dot(fVector, p - center)) - FVector2.Dot(value, extents) > FP.Zero)
			{
				continue;
			}
			TreeNode treeNode = reference;
			if (treeNode.IsLeaf())
			{
				RayCastInput input2 = new RayCastInput
				{
					P1 = input.P1,
					P2 = input.P2,
					MaxFraction = fP
				};
				FP fP2 = inputCallback.RayCastCallback(in input2, num);
				if (fP2.Equals(0f))
				{
					break;
				}
				if (fP2 > 0f)
				{
					fP = fP2;
					FVector2 value3 = p + fP * (p2 - p);
					b.LowerBound = FVector2.Min(p, value3);
					b.UpperBound = FVector2.Max(p, value3);
				}
			}
			else
			{
				rayCastStack.Push(reference.Child1);
				rayCastStack.Push(reference.Child2);
			}
		}
	}

	public void Validate()
	{
	}

	public int GetHeight()
	{
		if (_root == -1)
		{
			return 0;
		}
		return _treeNodes[_root].Height;
	}

	public int GetMaxBalance()
	{
		int num = 0;
		for (int i = 0; i < _nodeCapacity; i++)
		{
			ref TreeNode reference = ref _treeNodes[i];
			if (reference.Height > 1)
			{
				int child = reference.Child1;
				int child2 = reference.Child2;
				int val = Math.Abs(_treeNodes[child2].Height - _treeNodes[child].Height);
				num = Math.Max(num, val);
			}
		}
		return num;
	}

	public FP GetAreaRatio()
	{
		if (_root == -1)
		{
			return FP.Zero;
		}
		AABB aABB = _treeNodes[_root].AABB;
		FP perimeter = aABB.GetPerimeter();
		FP x = FP.Zero;
		for (int i = 0; i < _nodeCapacity; i++)
		{
			ref TreeNode reference = ref _treeNodes[i];
			if (reference.Height >= 0)
			{
				aABB = reference.AABB;
				x += aABB.GetPerimeter();
			}
		}
		return x / perimeter;
	}

	public void RebuildBottomUp()
	{
		int[] array = new int[_nodeCount];
		int num = 0;
		for (int i = 0; i < _nodeCapacity; i++)
		{
			ref TreeNode reference = ref _treeNodes[i];
			if (reference.Height >= 0)
			{
				if (reference.IsLeaf())
				{
					reference.Parent = -1;
					array[num] = i;
					num++;
				}
				else
				{
					FreeNode(i);
				}
			}
		}
		while (num > 1)
		{
			FP fP = Settings.MaxFloat;
			int num2 = -1;
			int num3 = -1;
			for (int j = 0; j < num; j++)
			{
				ref AABB aABB = ref _treeNodes[array[j]].AABB;
				for (int k = j + 1; k < num; k++)
				{
					AABB right = _treeNodes[array[k]].AABB;
					AABB.Combine(in aABB, in right, out var aabb);
					FP perimeter = aabb.GetPerimeter();
					if (perimeter < fP)
					{
						num2 = j;
						num3 = k;
						fP = perimeter;
					}
				}
			}
			int num4 = array[num2];
			int num5 = array[num3];
			ref TreeNode reference2 = ref _treeNodes[num4];
			ref TreeNode reference3 = ref _treeNodes[num5];
			int num6 = AllocateNode();
			ref TreeNode reference4 = ref _treeNodes[num6];
			reference4.Child1 = num4;
			reference4.Child2 = num5;
			reference4.Height = 1 + Math.Max(reference2.Height, reference3.Height);
			reference4.AABB.Combine(in reference2.AABB, in reference3.AABB);
			reference4.Parent = -1;
			reference2.Parent = num6;
			reference3.Parent = num6;
			array[num3] = array[num - 1];
			array[num2] = num6;
			num--;
		}
		_root = array[0];
		Validate();
	}

	public void ShiftOrigin(in FVector2 newOrigin)
	{
		for (int i = 0; i < _nodeCapacity; i++)
		{
			_treeNodes[i].AABB.LowerBound -= newOrigin;
			_treeNodes[i].AABB.UpperBound -= newOrigin;
		}
	}

	private void InsertLeaf(int leaf)
	{
		if (_root == -1)
		{
			_root = leaf;
			_treeNodes[_root].Parent = -1;
			return;
		}
		AABB right = _treeNodes[leaf].AABB;
		int num = _root;
		while (!_treeNodes[num].IsLeaf())
		{
			ref TreeNode reference = ref _treeNodes[num];
			int child = reference.Child1;
			int child2 = reference.Child2;
			FP y = reference.AABB.GetPerimeter();
			AABB.Combine(in reference.AABB, in right, out var aabb);
			FP y2 = aabb.GetPerimeter();
			FP fP = (FP)2 * y2;
			FP y3 = (FP)2 * (y2 - y);
			FP fP2;
			if (_treeNodes[child].IsLeaf())
			{
				AABB.Combine(in right, in _treeNodes[child].AABB, out var aabb2);
				fP2 = aabb2.GetPerimeter() + y3;
			}
			else
			{
				AABB.Combine(in right, in _treeNodes[child].AABB, out var aabb3);
				FP y4 = _treeNodes[child].AABB.GetPerimeter();
				fP2 = aabb3.GetPerimeter() - y4 + y3;
			}
			FP fP3;
			if (_treeNodes[child2].IsLeaf())
			{
				AABB.Combine(in right, in _treeNodes[child2].AABB, out var aabb4);
				fP3 = aabb4.GetPerimeter() + y3;
			}
			else
			{
				AABB.Combine(in right, in _treeNodes[child2].AABB, out var aabb5);
				FP y5 = _treeNodes[child2].AABB.GetPerimeter();
				fP3 = aabb5.GetPerimeter() - y5 + y3;
			}
			if (fP < fP2 && fP < fP3)
			{
				break;
			}
			num = ((!(fP2 < fP3)) ? child2 : child);
		}
		int num2 = num;
		ref TreeNode reference2 = ref _treeNodes[num2];
		TreeNode treeNode = reference2;
		int parent = treeNode.Parent;
		int num3 = AllocateNode();
		ref TreeNode reference3 = ref _treeNodes[num3];
		reference3.Parent = parent;
		reference3.UserData = null;
		reference3.AABB.Combine(in right, in reference2.AABB);
		reference3.Height = reference2.Height + 1;
		if (parent != -1)
		{
			ref TreeNode reference4 = ref _treeNodes[parent];
			if (reference4.Child1 == num2)
			{
				reference4.Child1 = num3;
			}
			else
			{
				reference4.Child2 = num3;
			}
			reference3.Child1 = num2;
			reference3.Child2 = leaf;
			_treeNodes[num2].Parent = num3;
			_treeNodes[leaf].Parent = num3;
		}
		else
		{
			reference3.Child1 = num2;
			reference3.Child2 = leaf;
			_treeNodes[num2].Parent = num3;
			_treeNodes[leaf].Parent = num3;
			_root = num3;
		}
		num = _treeNodes[leaf].Parent;
		while (num != -1)
		{
			num = Balance(num);
			ref TreeNode reference5 = ref _treeNodes[num];
			ref TreeNode reference6 = ref _treeNodes[reference5.Child1];
			ref TreeNode reference7 = ref _treeNodes[reference5.Child2];
			reference5.Height = 1 + Math.Max(reference6.Height, reference7.Height);
			reference5.AABB.Combine(in reference6.AABB, in reference7.AABB);
			num = reference5.Parent;
		}
	}

	private void RemoveLeaf(int leaf)
	{
		if (leaf == _root)
		{
			_root = -1;
			return;
		}
		int parent = _treeNodes[leaf].Parent;
		ref TreeNode reference = ref _treeNodes[parent];
		int parent2 = reference.Parent;
		int num = ((reference.Child1 == leaf) ? reference.Child2 : reference.Child1);
		if (parent2 != -1)
		{
			ref TreeNode reference2 = ref _treeNodes[parent2];
			if (reference2.Child1 == parent)
			{
				reference2.Child1 = num;
			}
			else
			{
				reference2.Child2 = num;
			}
			_treeNodes[num].Parent = parent2;
			FreeNode(parent);
			int num2 = parent2;
			while (num2 != -1)
			{
				num2 = Balance(num2);
				ref TreeNode reference3 = ref _treeNodes[num2];
				ref TreeNode reference4 = ref _treeNodes[reference3.Child1];
				ref TreeNode reference5 = ref _treeNodes[reference3.Child2];
				reference3.AABB.Combine(in reference4.AABB, in reference5.AABB);
				reference3.Height = 1 + Math.Max(reference4.Height, reference5.Height);
				num2 = reference3.Parent;
			}
		}
		else
		{
			_root = num;
			_treeNodes[num].Parent = -1;
			FreeNode(parent);
		}
	}

	private int Balance(int iA)
	{
		ref TreeNode reference = ref _treeNodes[iA];
		if (reference.IsLeaf() || reference.Height < 2)
		{
			return iA;
		}
		int child = reference.Child1;
		int child2 = reference.Child2;
		ref TreeNode reference2 = ref _treeNodes[child];
		ref TreeNode reference3 = ref _treeNodes[child2];
		int num = reference3.Height - reference2.Height;
		if (num > 1)
		{
			int child3 = reference3.Child1;
			int child4 = reference3.Child2;
			ref TreeNode reference4 = ref _treeNodes[child3];
			ref TreeNode reference5 = ref _treeNodes[child4];
			reference3.Child1 = iA;
			reference3.Parent = reference.Parent;
			reference.Parent = child2;
			if (reference3.Parent != -1)
			{
				ref TreeNode reference6 = ref _treeNodes[reference3.Parent];
				if (reference6.Child1 == iA)
				{
					reference6.Child1 = child2;
				}
				else
				{
					reference6.Child2 = child2;
				}
			}
			else
			{
				_root = child2;
			}
			if (reference4.Height > reference5.Height)
			{
				reference3.Child2 = child3;
				reference.Child2 = child4;
				reference5.Parent = iA;
				reference.AABB.Combine(in reference2.AABB, in reference5.AABB);
				reference3.AABB.Combine(in reference.AABB, in reference4.AABB);
				reference.Height = 1 + Math.Max(reference2.Height, reference5.Height);
				reference3.Height = 1 + Math.Max(reference.Height, reference4.Height);
			}
			else
			{
				reference3.Child2 = child4;
				reference.Child2 = child3;
				reference4.Parent = iA;
				reference.AABB.Combine(in reference2.AABB, in reference4.AABB);
				reference3.AABB.Combine(in reference.AABB, in reference5.AABB);
				reference.Height = 1 + Math.Max(reference2.Height, reference4.Height);
				reference3.Height = 1 + Math.Max(reference.Height, reference5.Height);
			}
			return child2;
		}
		if (num < -1)
		{
			int child5 = reference2.Child1;
			int child6 = reference2.Child2;
			ref TreeNode reference7 = ref _treeNodes[child5];
			ref TreeNode reference8 = ref _treeNodes[child6];
			reference2.Child1 = iA;
			reference2.Parent = reference.Parent;
			reference.Parent = child;
			if (reference2.Parent != -1)
			{
				ref TreeNode reference9 = ref _treeNodes[reference2.Parent];
				if (reference9.Child1 == iA)
				{
					reference9.Child1 = child;
				}
				else
				{
					reference9.Child2 = child;
				}
			}
			else
			{
				_root = child;
			}
			if (reference7.Height > reference8.Height)
			{
				reference2.Child2 = child5;
				reference.Child1 = child6;
				reference8.Parent = iA;
				reference.AABB.Combine(in reference3.AABB, in reference8.AABB);
				reference2.AABB.Combine(in reference.AABB, in reference7.AABB);
				reference.Height = 1 + Math.Max(reference3.Height, reference8.Height);
				reference2.Height = 1 + Math.Max(reference.Height, reference7.Height);
			}
			else
			{
				reference2.Child2 = child6;
				reference.Child1 = child5;
				reference7.Parent = iA;
				reference.AABB.Combine(in reference3.AABB, in reference7.AABB);
				reference2.AABB.Combine(in reference.AABB, in reference8.AABB);
				reference.Height = 1 + Math.Max(reference3.Height, reference7.Height);
				reference2.Height = 1 + Math.Max(reference.Height, reference8.Height);
			}
			return child;
		}
		return iA;
	}

	private int ComputeHeight()
	{
		return ComputeHeight(_root);
	}

	private int ComputeHeight(int nodeId)
	{
		ref TreeNode reference = ref _treeNodes[nodeId];
		TreeNode treeNode = reference;
		if (treeNode.IsLeaf())
		{
			return 0;
		}
		int val = ComputeHeight(reference.Child1);
		int val2 = ComputeHeight(reference.Child2);
		return 1 + Math.Max(val, val2);
	}

	private void ValidateStructure(int index)
	{
		if (index != -1)
		{
			_ = _root;
			ref TreeNode reference = ref _treeNodes[index];
			int child = reference.Child1;
			int child2 = reference.Child2;
			TreeNode treeNode = reference;
			if (!treeNode.IsLeaf())
			{
				ValidateStructure(child);
				ValidateStructure(child2);
			}
		}
	}

	private void ValidateMetrics(int index)
	{
		if (index != -1)
		{
			ref TreeNode reference = ref _treeNodes[index];
			int child = reference.Child1;
			int child2 = reference.Child2;
			TreeNode treeNode = reference;
			if (!treeNode.IsLeaf())
			{
				int height = _treeNodes[child].Height;
				int height2 = _treeNodes[child2].Height;
				Math.Max(height, height2);
				AABB.Combine(in _treeNodes[child].AABB, in _treeNodes[child2].AABB, out var _);
				ValidateMetrics(child);
				ValidateMetrics(child2);
			}
		}
	}
}
