using System;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Internal;

namespace Box2DSharp.Collision;

public class BroadPhase : ITreeQueryCallback
{
	public const int NullProxy = -1;

	private DynamicTree _tree;

	private int _proxyCount;

	private int[] _moveBuffer;

	private int _moveCapacity;

	private int _moveCount;

	private Pair[] _pairBuffer;

	private int _pairCapacity;

	private int _pairCount;

	private int _queryProxyId;

	public BroadPhase()
	{
		_proxyCount = 0;
		_tree = new DynamicTree();
		_pairCapacity = 16;
		_pairCount = 0;
		_pairBuffer = new Pair[_pairCapacity];
		_moveCapacity = 16;
		_moveCount = 0;
		_moveBuffer = new int[_moveCapacity];
	}

	public int CreateProxy(in AABB aabb, FixtureProxy userData)
	{
		int num = _tree.CreateProxy(in aabb, userData);
		_proxyCount++;
		BufferMove(num);
		return num;
	}

	public void DestroyProxy(int proxyId)
	{
		UnBufferMove(proxyId);
		_proxyCount--;
		_tree.DestroyProxy(proxyId);
	}

	public void MoveProxy(int proxyId, in AABB aabb, in FVector2 displacement)
	{
		if (_tree.MoveProxy(proxyId, in aabb, in displacement))
		{
			BufferMove(proxyId);
		}
	}

	public void TouchProxy(int proxyId)
	{
		BufferMove(proxyId);
	}

	public AABB GetFatAABB(int proxyId)
	{
		return _tree.GetFatAABB(proxyId);
	}

	internal object GetUserData(int proxyId)
	{
		return _tree.GetUserData(proxyId);
	}

	public bool TestOverlap(int proxyIdA, int proxyIdB)
	{
		return CollisionUtils.TestOverlap(_tree.GetFatAABB(proxyIdA), _tree.GetFatAABB(proxyIdB));
	}

	public int GetProxyCount()
	{
		return _proxyCount;
	}

	internal void UpdatePairs<T>(T callback) where T : IAddPairCallback
	{
		_pairCount = 0;
		for (int i = 0; i < _moveCount; i++)
		{
			_queryProxyId = _moveBuffer[i];
			if (_queryProxyId != -1)
			{
				AABB aabb = _tree.GetFatAABB(_queryProxyId);
				DynamicTree tree = _tree;
				ITreeQueryCallback callback2 = this;
				tree.Query(in callback2, in aabb);
			}
		}
		for (int j = 0; j < _pairCount; j++)
		{
			ref Pair reference = ref _pairBuffer[j];
			object userData = _tree.GetUserData(reference.ProxyIdA);
			object userData2 = _tree.GetUserData(reference.ProxyIdB);
			callback.AddPairCallback(userData, userData2);
		}
		for (int k = 0; k < _moveCount; k++)
		{
			int num = _moveBuffer[k];
			if (num != -1)
			{
				_tree.ClearMoved(num);
			}
		}
		_moveCount = 0;
	}

	public void Query(in ITreeQueryCallback callback, in AABB aabb)
	{
		_tree.Query(in callback, in aabb);
	}

	public void RayCast(in ITreeRayCastCallback callback, in RayCastInput input)
	{
		_tree.RayCast(in callback, in input);
	}

	public int GetTreeHeight()
	{
		return _tree.GetHeight();
	}

	public int GetTreeBalance()
	{
		return _tree.GetMaxBalance();
	}

	public FP GetTreeQuality()
	{
		return _tree.GetAreaRatio();
	}

	public void ShiftOrigin(in FVector2 newOrigin)
	{
		_tree.ShiftOrigin(in newOrigin);
	}

	private void BufferMove(int proxyId)
	{
		if (_moveCount == _moveCapacity)
		{
			_moveCapacity *= 2;
			Array.Resize(ref _moveBuffer, _moveCapacity);
		}
		_moveBuffer[_moveCount] = proxyId;
		_moveCount++;
	}

	private void UnBufferMove(int proxyId)
	{
		for (int i = 0; i < _moveCount; i++)
		{
			if (_moveBuffer[i] == proxyId)
			{
				_moveBuffer[i] = -1;
			}
		}
	}

	public bool QueryCallback(int proxyId)
	{
		if (proxyId == _queryProxyId)
		{
			return true;
		}
		if (_tree.WasMoved(proxyId) && proxyId > _queryProxyId)
		{
			return true;
		}
		if (_pairCount == _pairCapacity)
		{
			_pairCapacity += _pairCapacity >> 1;
			Array.Resize(ref _pairBuffer, _pairCapacity);
		}
		_pairBuffer[_pairCount].ProxyIdA = Math.Min(proxyId, _queryProxyId);
		_pairBuffer[_pairCount].ProxyIdB = Math.Max(proxyId, _queryProxyId);
		_pairCount++;
		return true;
	}
}
