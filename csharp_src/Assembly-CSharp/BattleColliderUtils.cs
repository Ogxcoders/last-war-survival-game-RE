using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Collider2D;
using GameFramework;
using PVEBattleLogic.Bullet;
using PVEBattleLogic.Unit;
using UnityEngine;
using XLua;

public static class BattleColliderUtils
{
	public enum ColliderType
	{
		None,
		Capsule,
		Box,
		Sphere
	}

	public class MonsterColliderData
	{
		public Transform transform;

		public Vector3 colliderCenter;

		public ColliderType colliderType;

		public int uid;

		public int viewHandle = -1;

		public float radius;

		public int targetLayerMask;

		public bool capsule;

		public Vector3 halfVector;

		public Vector3 endOffset;

		public Vector3 halfExtents;

		public void Clear()
		{
			transform = null;
			colliderCenter = Vector3.zero;
			uid = 0;
			radius = 0f;
			targetLayerMask = 0;
			capsule = false;
			halfVector = Vector3.zero;
			endOffset = Vector3.zero;
			halfExtents = Vector3.zero;
			viewHandle = -1;
		}
	}

	public class ObstacleColliderData
	{
		public Transform camTrans;

		public Transform playerTrans;

		public Vector3 colliderCenter;

		public ColliderType colliderType;

		public int uid;

		public float radius;

		public int targetLayerMask;

		public Vector3 halfVector;

		public float heightScale;

		public float height;

		public int colliderDir;

		public Vector3 lastCenterPos;

		public void Clear()
		{
			playerTrans = null;
			camTrans = null;
			colliderCenter = Vector3.zero;
			uid = 0;
			radius = 0f;
			targetLayerMask = 0;
			halfVector = Vector3.zero;
			heightScale = 1f;
			height = 0f;
			colliderDir = 0;
			lastCenterPos = Vector3.zero;
		}
	}

	public class PlayerColliderData
	{
		public Transform transform;

		public Vector3 colliderCenter;

		public ColliderType colliderType;

		public int uid;

		public float radius;

		public int targetLayerMask;

		public Vector3 halfVector;

		public float heightScale;

		public float height;

		public int colliderDir;

		public Vector3 lastCenterPos;

		public void Clear()
		{
			transform = null;
			colliderCenter = Vector3.zero;
			uid = 0;
			radius = 0f;
			targetLayerMask = 0;
			halfVector = Vector3.zero;
			heightScale = 1f;
			height = 0f;
			colliderDir = 0;
			lastCenterPos = Vector3.zero;
		}
	}

	public class UnitColliderData
	{
		public Transform transform;

		public Vector3 colliderCenter;

		public ColliderType colliderType;

		public int uid;

		public float radius;

		public int targetLayerMask;

		public Vector3 halfVector;

		public Vector3 endOffset;

		public Vector3 halfExtents;

		public void Clear()
		{
			transform = null;
			colliderCenter = Vector3.zero;
			uid = 0;
			radius = 0f;
			targetLayerMask = 0;
			halfVector = Vector3.zero;
			endOffset = Vector3.zero;
			halfExtents = Vector3.zero;
		}
	}

	public sealed class IndexedList<T> where T : class
	{
		private class LinearListNode
		{
			public int index;

			public T obj;
		}

		private List<LinearListNode> list;

		private LinearListNode head;

		private int count;

		public T this[int i]
		{
			get
			{
				if (i > 0 && i < count)
				{
					return list[i].obj;
				}
				return null;
			}
		}

		public int Count => count;

		public IndexedList(int capacity = 15)
		{
			list = new List<LinearListNode>(capacity);
			head = new LinearListNode
			{
				index = 0,
				obj = null
			};
			list.Add(head);
			list.Add(new LinearListNode
			{
				index = 1,
				obj = null
			});
			count = list.Count;
		}

		public void Clear()
		{
			list.Clear();
			head = new LinearListNode
			{
				index = 0,
				obj = null
			};
			list.Add(head);
			list.Add(new LinearListNode
			{
				index = 1,
				obj = null
			});
			count = list.Count;
		}

		public int Add(T obj)
		{
			int num = -1;
			if (head.index != 0)
			{
				num = head.index;
				list[num].obj = obj;
				head.index = list[num].index;
			}
			else
			{
				num = list.Count;
				list.Add(new LinearListNode
				{
					index = num,
					obj = obj
				});
				count = num + 1;
			}
			return num;
		}

		public T TryGetValue(int index)
		{
			if (index > 0 && index < count)
			{
				return list[index].obj;
			}
			return null;
		}

		public T Remove(int pos)
		{
			if (pos > 0 && pos < count)
			{
				T obj = list[pos].obj;
				list[pos].obj = null;
				list[pos].index = head.index;
				head.index = pos;
				return obj;
			}
			return null;
		}

		public T Replace(int pos, T obj)
		{
			if (pos > 0 && pos < count)
			{
				T obj2 = list[pos].obj;
				list[pos].obj = obj;
				return obj2;
			}
			return null;
		}
	}

	public class BulletData
	{
		public Transform transform;

		public int uid;

		public float radius;

		public int targetLayerMask;

		public bool capsule;

		public Vector3 startOffset;

		public Vector3 endOffset;

		public Vector3 lastPos;

		public float dotCD;

		public float dotCDMax;

		public bool CheckCD(float deltaTime)
		{
			dotCD -= deltaTime;
			if (dotCD <= 0f)
			{
				dotCD = dotCDMax;
				return true;
			}
			return false;
		}

		public void Clear()
		{
			transform = null;
			uid = 0;
			radius = 0f;
			targetLayerMask = 0;
			capsule = false;
			startOffset = Vector3.zero;
			endOffset = Vector3.zero;
			dotCD = 0f;
			dotCDMax = 0f;
		}
	}

	private static readonly Dictionary<int, float> _cacheAttackerCollideDirMap = new Dictionary<int, float>(4);

	private static IndexedList<MonsterColliderData> _monsterColliderDataList;

	private static Dictionary<long, int> _monsterColliderIdMap;

	private static Stack<MonsterColliderData> _monsterColliderDataPool = new Stack<MonsterColliderData>(16);

	private const int MONSTER_COLLIDER_MAX = 10;

	private static Collider[] _monsterColliders = new Collider[10];

	private static LuaArrAccess _monsterColliderResultArrAccess;

	private static ObstacleColliderData _obstructionColliderData;

	private static Stack<ObstacleColliderData> _obstructionColliderDataPool = new Stack<ObstacleColliderData>(4);

	private static Collider[] _obstructionColliders = new Collider[10];

	private static RaycastHit[] _obstructionHits = new RaycastHit[10];

	private static LuaArrAccess _obstructionColliderResultArrAccess;

	private static readonly List<Vector3> DefaultOffsets = new List<Vector3>
	{
		new Vector3(-1f, 6.2f, 8.6f),
		new Vector3(1f, 6.2f, 8.6f),
		new Vector3(0f, 1.8f, 0f),
		new Vector3(-2f, 0.7f, -6f),
		new Vector3(2f, 0.7f, -6f)
	};

	private static PlayerColliderData _playerColliderData;

	private static Stack<PlayerColliderData> _playerColliderDataPool = new Stack<PlayerColliderData>(16);

	private static Collider[] _playerColliders = new Collider[10];

	private static RaycastHit[] _playerHits = new RaycastHit[10];

	private static LuaArrAccess _playerColliderResultArrAccess;

	private static readonly Dictionary<int, float> _cacheAttackerCollideZMap = new Dictionary<int, float>(4);

	private static UnitColliderData _unitColliderData;

	private static Stack<UnitColliderData> _unitColliderDataPool = new Stack<UnitColliderData>(16);

	private static Collider[] _unitColliders = new Collider[10];

	private static LuaArrAccess _unitColliderResultArrAccess;

	private static IndexedList<BulletData> _bulletDataList;

	private static Dictionary<long, int> _bulletIdMap;

	private static Stack<BulletData> _bulletDataPool = new Stack<BulletData>(512);

	private static LuaTable _resultLuaTable;

	private const int COLLIDER_MAX = 64;

	private static Collider[] _colliders = new Collider[64];

	private static LuaArrAccess _colliderResultArrAccess;

	private static bool _useCollider2D = false;

	public static int[] _ColliderList;

	private const int COLLIDER_DEAULT_COUNT = 100;

	private static LuaArrAccess _overlapSphereNonAllocParamAccess;

	private static LuaArrAccess _overlapSphereNonAllocResultAccess;

	private static Collider[] _overlapSphereNonAllocColliders;

	public static bool GhostPlayerColliderLuaArray()
	{
		_cacheAttackerCollideDirMap.Clear();
		if (_playerColliderData == null)
		{
			return false;
		}
		if (_playerColliderResultArrAccess == null)
		{
			return false;
		}
		int num = 1;
		int cap = (int)_playerColliderResultArrAccess.GetArrayCapacity();
		if (_playerColliderData != null)
		{
			bool flag = false;
			_cacheAttackerCollideZMap.Clear();
			int num2 = 0;
			Vector3 position = _playerColliderData.colliderCenter * _playerColliderData.heightScale;
			Vector3 vector = _playerColliderData.transform.TransformPoint(position);
			if (_playerColliderData.colliderType == ColliderType.Capsule)
			{
				if (_playerColliderData.lastCenterPos == Vector3.zero)
				{
					flag = true;
					num2 = Physics.OverlapCapsuleNonAlloc(vector + _playerColliderData.halfVector, vector - _playerColliderData.halfVector, _playerColliderData.radius, _playerColliders, _playerColliderData.targetLayerMask);
				}
				else
				{
					Vector3 lastCenterPos = _playerColliderData.lastCenterPos;
					Vector3 vector2 = vector - lastCenterPos;
					num2 = Physics.CapsuleCastNonAlloc(lastCenterPos + _playerColliderData.halfVector, lastCenterPos - _playerColliderData.halfVector, _playerColliderData.radius, Vector3.Normalize(vector2), _playerHits, Vector3.Magnitude(vector2), _playerColliderData.targetLayerMask);
				}
				_playerColliderData.lastCenterPos = vector;
			}
			if (num2 > 0)
			{
				if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
				{
					return false;
				}
				_playerColliderResultArrAccess.SetInt(num, num2);
				num++;
				if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
				{
					return false;
				}
				for (int i = 0; i < num2; i++)
				{
					Collider collider = null;
					float num3 = 0f;
					float value = 0f;
					if (flag)
					{
						collider = _playerColliders[i];
					}
					else
					{
						RaycastHit raycastHit = _playerHits[i];
						collider = raycastHit.collider;
						num3 = raycastHit.point.z - _playerColliderData.radius;
						Vector3 normal = raycastHit.normal;
						value = ((!(raycastHit.normal.y < -0.5f)) ? Vector3.Dot(Vector3.forward, normal) : (-0.6f));
					}
					if (collider.CompareTag("Finish"))
					{
						continue;
					}
					CitySpaceManTrigger result;
					if (collider.TryGetComponent<CitySpaceManTrigger>(out var component) && component.ObjectId > 0)
					{
						if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
						{
							return false;
						}
						int num4 = (int)component.ObjectId;
						_playerColliderResultArrAccess.SetInt(num, num4);
						num++;
						if (num3 > 0f)
						{
							_cacheAttackerCollideZMap[num4] = num3;
						}
						_cacheAttackerCollideDirMap[num4] = value;
					}
					else if (collider.TryGetComponentInParent<CitySpaceManTrigger>(out result) && result.ObjectId > 0)
					{
						if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
						{
							return false;
						}
						int num5 = (int)result.ObjectId;
						_playerColliderResultArrAccess.SetInt(num, num5);
						num++;
						if (num3 > 0f)
						{
							_cacheAttackerCollideZMap[num5] = num3;
						}
						_cacheAttackerCollideDirMap[num5] = value;
					}
					else
					{
						if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
						{
							return false;
						}
						_playerColliderResultArrAccess.SetInt(num, 0);
						num++;
					}
				}
			}
		}
		if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_playerColliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	public static float TryGetGhostPlayerCollideDir(int attackerId)
	{
		if (_cacheAttackerCollideDirMap.TryGetValue(attackerId, out var value))
		{
			return value;
		}
		return 0f;
	}

	private static MonsterColliderData GetMonsterColliderData()
	{
		if (_monsterColliderDataPool.Count > 0)
		{
			return _monsterColliderDataPool.Pop();
		}
		return new MonsterColliderData();
	}

	private static void ReleaseMonsterColliderData(MonsterColliderData monsterColliderData)
	{
		if (monsterColliderData != null)
		{
			monsterColliderData.Clear();
			_monsterColliderDataPool.Push(monsterColliderData);
		}
	}

	private static void AddMonsterColliderImp(Transform transform, Collider collider, int objId, int layerMask, int viewHandle)
	{
		if (_monsterColliderDataList == null)
		{
			_monsterColliderDataList = new IndexedList<MonsterColliderData>(16);
			_monsterColliderIdMap = new Dictionary<long, int>(16);
		}
		MonsterColliderData monsterColliderData = null;
		if (collider is BoxCollider boxCollider)
		{
			monsterColliderData = GetMonsterColliderData();
			monsterColliderData.uid = objId;
			monsterColliderData.viewHandle = viewHandle;
			monsterColliderData.colliderType = ColliderType.Box;
			monsterColliderData.halfExtents = boxCollider.size / 2f;
			monsterColliderData.targetLayerMask = layerMask;
			monsterColliderData.colliderCenter = boxCollider.center;
		}
		else if (collider is CapsuleCollider capsuleCollider)
		{
			monsterColliderData = GetMonsterColliderData();
			monsterColliderData.uid = objId;
			monsterColliderData.viewHandle = viewHandle;
			monsterColliderData.colliderType = ColliderType.Capsule;
			switch (capsuleCollider.direction)
			{
			case 0:
				monsterColliderData.halfVector = Vector3.right * capsuleCollider.height / 2f;
				break;
			case 1:
				monsterColliderData.halfVector = Vector3.up * capsuleCollider.height / 2f;
				break;
			case 2:
				monsterColliderData.halfVector = Vector3.forward * capsuleCollider.height / 2f;
				break;
			}
			monsterColliderData.radius = capsuleCollider.radius;
			monsterColliderData.targetLayerMask = layerMask;
			monsterColliderData.colliderCenter = capsuleCollider.center;
		}
		else if (collider is SphereCollider sphereCollider)
		{
			monsterColliderData = GetMonsterColliderData();
			monsterColliderData.uid = objId;
			monsterColliderData.viewHandle = viewHandle;
			monsterColliderData.colliderType = ColliderType.Sphere;
			monsterColliderData.radius = sphereCollider.radius;
			monsterColliderData.targetLayerMask = layerMask;
			monsterColliderData.colliderCenter = sphereCollider.center;
		}
		if (monsterColliderData != null)
		{
			monsterColliderData.transform = transform;
			int value = _monsterColliderDataList.Add(monsterColliderData);
			_monsterColliderIdMap[objId] = value;
		}
	}

	public static void AddMonsterColliderTransform(Transform transform, int objId, int layerMask, int viewHandle)
	{
		if (transform.TryGetComponent<Collider>(out var component))
		{
			AddMonsterColliderImp(transform, component, objId, layerMask, viewHandle);
		}
	}

	public static void AddMonsterCollider(int viewHandle, int objId, int layerMask)
	{
		Transform transform = UnitViewFacade.GetTransform(viewHandle);
		if (transform == null)
		{
			return;
		}
		Collider collider = UnitViewFacade.GetCollider(viewHandle);
		if (!(collider == null))
		{
			int unitObjId = UnitViewFacade.GetUnitObjId(viewHandle);
			if (unitObjId == objId)
			{
				AddMonsterColliderImp(transform, collider, unitObjId, layerMask, viewHandle);
			}
		}
	}

	public static void RemoveMonsterCollider(int objId)
	{
		if (_monsterColliderIdMap != null && _monsterColliderIdMap.TryGetValue(objId, out var value))
		{
			MonsterColliderData monsterColliderData = _monsterColliderDataList.Remove(value);
			_monsterColliderIdMap.Remove(objId);
			ReleaseMonsterColliderData(monsterColliderData);
		}
	}

	public static void InitMonsterColliderResultAccess(LuaArrAccess access)
	{
		_monsterColliderResultArrAccess = access;
	}

	public static void UnInitMonsterColliderResultAccess()
	{
		_monsterColliderResultArrAccess = null;
	}

	public static bool MonsterColliderLuaArray()
	{
		if (_monsterColliderDataList == null)
		{
			return false;
		}
		if (_monsterColliderResultArrAccess == null)
		{
			return false;
		}
		if (_useCollider2D)
		{
			return MonsterCollider2D();
		}
		return MonsterCollider3D();
	}

	private static bool MonsterCollider2D()
	{
		if (_ColliderList == null)
		{
			_ColliderList = new int[100];
		}
		int count = _monsterColliderDataList.Count;
		int num = 1;
		int cap = (int)_monsterColliderResultArrAccess.GetArrayCapacity();
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			MonsterColliderData monsterColliderData = _monsterColliderDataList[num2];
			if (monsterColliderData != null)
			{
				int num3 = 0;
				Vector3 vector = monsterColliderData.transform.TransformPoint(monsterColliderData.colliderCenter);
				switch (monsterColliderData.colliderType)
				{
				case ColliderType.Capsule:
				{
					Vector2 vector3 = new Vector2(vector.x, vector.z);
					num3 = Collider2DUtils.OverlapCircle2DCollider(vector3, vector3, monsterColliderData.radius, monsterColliderData.targetLayerMask, ref _ColliderList);
					break;
				}
				case ColliderType.Box:
				{
					num3 = 0;
					if (Collider2DUtils.TryGetAgent(monsterColliderData.viewHandle, out var agent))
					{
						num3 = Collider2DUtils.OverlapAABB2DCollider(agent.TransformedAABBMinX, agent.TransformedAABBMinY, agent.TransformedAABBMaxX, agent.TransformedAABBMaxY, monsterColliderData.targetLayerMask, ref _ColliderList);
					}
					break;
				}
				case ColliderType.Sphere:
				{
					Vector2 vector2 = new Vector2(vector.x, vector.z);
					num3 = Collider2DUtils.OverlapCircle2DCollider(vector2, vector2, monsterColliderData.radius, monsterColliderData.targetLayerMask, ref _ColliderList);
					break;
				}
				}
				if (num3 > 0)
				{
					if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_monsterColliderResultArrAccess.SetInt(num, monsterColliderData.uid);
					num++;
					if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_monsterColliderResultArrAccess.SetInt(num, num3);
					num++;
					for (int i = 0; i < num3; i++)
					{
						int num4 = _ColliderList[i];
						if (num4 > 0)
						{
							if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_monsterColliderResultArrAccess.SetInt(num, num4);
							num++;
						}
						else
						{
							if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_monsterColliderResultArrAccess.SetInt(num, 0);
							num++;
						}
					}
				}
			}
		}
		if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_monsterColliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	private static bool MonsterCollider3D()
	{
		int count = _monsterColliderDataList.Count;
		int num = 1;
		int cap = (int)_monsterColliderResultArrAccess.GetArrayCapacity();
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			MonsterColliderData monsterColliderData = _monsterColliderDataList[num2];
			if (monsterColliderData != null)
			{
				int num3 = 0;
				Vector3 vector = monsterColliderData.transform.TransformPoint(monsterColliderData.colliderCenter);
				switch (monsterColliderData.colliderType)
				{
				case ColliderType.Capsule:
					num3 = Physics.OverlapCapsuleNonAlloc(vector + monsterColliderData.halfVector, vector - monsterColliderData.halfVector, monsterColliderData.radius, _monsterColliders, monsterColliderData.targetLayerMask);
					break;
				case ColliderType.Box:
					num3 = Physics.OverlapBoxNonAlloc(vector, monsterColliderData.halfExtents, _monsterColliders, monsterColliderData.transform.rotation, monsterColliderData.targetLayerMask);
					break;
				case ColliderType.Sphere:
					num3 = Physics.OverlapSphereNonAlloc(vector, monsterColliderData.radius, _monsterColliders, monsterColliderData.targetLayerMask);
					break;
				}
				if (num3 > 0)
				{
					if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_monsterColliderResultArrAccess.SetInt(num, monsterColliderData.uid);
					num++;
					if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_monsterColliderResultArrAccess.SetInt(num, num3);
					num++;
					for (int i = 0; i < num3; i++)
					{
						if (_monsterColliders[i].TryGetComponent<CitySpaceManTrigger>(out var component) && component.ObjectId > 0)
						{
							if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_monsterColliderResultArrAccess.SetInt(num, (int)component.ObjectId);
							num++;
						}
						else
						{
							if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_monsterColliderResultArrAccess.SetInt(num, 0);
							num++;
						}
					}
				}
			}
		}
		if (num > cap && !ResizeMonsterColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_monsterColliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool ResizeMonsterColliderLuaArray(int index, ref int cap)
	{
		if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEMonsterCollider", index))
		{
			Log.Error($"BattleColliderUtils.BulletMonsterColliderLuaArray(): resize array error : {index}");
			return false;
		}
		cap = (int)_monsterColliderResultArrAccess.GetArrayCapacity();
		if (index > cap)
		{
			Log.Error($"BattleColliderUtils.BulletMonsterColliderLuaArray(): resize array error : {index}");
			return false;
		}
		return true;
	}

	public static void ClearMonsterColliderData()
	{
		if (_monsterColliderDataList == null)
		{
			return;
		}
		for (int num = _monsterColliderDataList.Count - 1; num >= 0; num--)
		{
			MonsterColliderData monsterColliderData = _monsterColliderDataList[num];
			if (monsterColliderData != null)
			{
				_monsterColliderIdMap.Remove(monsterColliderData.uid);
				_monsterColliderDataList.Remove(num);
				ReleaseMonsterColliderData(monsterColliderData);
			}
		}
		_monsterColliderDataList.Clear();
	}

	private static ObstacleColliderData GetObstacleColliderData()
	{
		if (_obstructionColliderDataPool.Count > 0)
		{
			return _obstructionColliderDataPool.Pop();
		}
		return new ObstacleColliderData();
	}

	private static void ReleaseObstacleColliderData(ObstacleColliderData obstacleColliderData)
	{
		if (obstacleColliderData != null)
		{
			obstacleColliderData.Clear();
			_obstructionColliderDataPool.Push(obstacleColliderData);
		}
	}

	private static void AddObstacleColliderImp(Transform camTrans, Transform playerTrans, Collider collider, int objId, int layerMask)
	{
		ObstacleColliderData obstacleColliderData = null;
		if (collider is CapsuleCollider capsuleCollider)
		{
			obstacleColliderData = GetObstacleColliderData();
			obstacleColliderData.uid = objId;
			obstacleColliderData.colliderType = ColliderType.Capsule;
			obstacleColliderData.heightScale = 1f;
			float num = (obstacleColliderData.radius = capsuleCollider.radius);
			obstacleColliderData.height = capsuleCollider.height;
			float num2 = Mathf.Max(0f, obstacleColliderData.height * obstacleColliderData.heightScale * 0.5f - num);
			obstacleColliderData.colliderDir = capsuleCollider.direction;
			switch (obstacleColliderData.colliderDir)
			{
			case 0:
				obstacleColliderData.halfVector = Vector3.right * num2;
				break;
			case 1:
				obstacleColliderData.halfVector = Vector3.up * num2;
				break;
			case 2:
				obstacleColliderData.halfVector = Vector3.forward * num2;
				break;
			}
			obstacleColliderData.targetLayerMask = layerMask;
			obstacleColliderData.colliderCenter = capsuleCollider.center;
		}
		if (obstacleColliderData != null)
		{
			obstacleColliderData.camTrans = camTrans;
			obstacleColliderData.playerTrans = playerTrans;
			_obstructionColliderData = obstacleColliderData;
		}
	}

	public static void AddObstacleCollider(Transform cam, int viewHandle, int objId, int layerMask)
	{
		Transform transform = UnitViewFacade.GetTransform(viewHandle);
		if (transform == null || cam == null)
		{
			return;
		}
		Collider collider = null;
		collider = UnitViewFacade.GetCollider(viewHandle);
		if (!(collider == null))
		{
			int unitObjId = UnitViewFacade.GetUnitObjId(viewHandle);
			if (unitObjId == objId)
			{
				AddObstacleColliderImp(cam, transform, collider, unitObjId, layerMask);
			}
		}
	}

	public static void RemoveObstacleCollider()
	{
		if (_obstructionColliderData != null)
		{
			ReleaseObstacleColliderData(_obstructionColliderData);
			_obstructionColliderData = null;
		}
	}

	public static void InitObstacleColliderResultAccess(LuaArrAccess access)
	{
		_obstructionColliderResultArrAccess = access;
	}

	public static void UnInitObstacleColliderResultAccess()
	{
		_obstructionColliderResultArrAccess = null;
	}

	public static bool ObstacleColliderLuaArray()
	{
		if (_obstructionColliderData == null)
		{
			return false;
		}
		if (_obstructionColliderResultArrAccess == null)
		{
			return false;
		}
		int index = 1;
		int arrayCapacity = (int)_obstructionColliderResultArrAccess.GetArrayCapacity();
		Vector3 vector = _obstructionColliderData.camTrans.position + Vector3.up * 4f;
		Vector3 lastCenterPos = _obstructionColliderData.lastCenterPos;
		Vector3 position = _obstructionColliderData.playerTrans.position;
		bool flag = false;
		foreach (Vector3 defaultOffset in DefaultOffsets)
		{
			if (HandleLuaArray(lastCenterPos, ref index, arrayCapacity, vector, position + defaultOffset))
			{
				flag = true;
			}
		}
		_obstructionColliderData.lastCenterPos = vector;
		if (flag)
		{
			for (int i = index; i < arrayCapacity; i++)
			{
				_obstructionColliderResultArrAccess.SetInt(i, -1);
			}
		}
		return flag;
	}

	public static bool HandleLuaArray(Vector3 lastCenter, ref int index, int cap, Vector3 startPos, Vector3 endPos)
	{
		bool result = false;
		if (_obstructionColliderData != null)
		{
			bool flag = false;
			int num = 0;
			if (_obstructionColliderData.colliderType == ColliderType.Capsule)
			{
				if (_obstructionColliderData.lastCenterPos == Vector3.zero)
				{
					flag = true;
					num = 0;
				}
				else
				{
					Vector3 vector = endPos - startPos;
					float magnitude = vector.magnitude;
					if (magnitude > Mathf.Epsilon)
					{
						Vector3 direction = vector / magnitude;
						num = Physics.RaycastNonAlloc(startPos, direction, _obstructionHits, magnitude, _obstructionColliderData.targetLayerMask);
					}
				}
			}
			if (num > 0)
			{
				if (index > cap && !ResizeObstacleColliderLuaArray(index, ref cap))
				{
					return false;
				}
				_obstructionColliderResultArrAccess.SetInt(index, num);
				index++;
				if (index > cap && !ResizeObstacleColliderLuaArray(index, ref cap))
				{
					return false;
				}
				for (int i = 0; i < num; i++)
				{
					Collider collider = null;
					if (flag)
					{
						collider = _obstructionColliders[i];
					}
					else
					{
						RaycastHit raycastHit = _obstructionHits[i];
						collider = raycastHit.collider;
					}
					CitySpaceManTrigger result2;
					if (collider.TryGetComponent<CitySpaceManTrigger>(out var component) && component.ObjectId > 0)
					{
						if (index > cap && !ResizeObstacleColliderLuaArray(index, ref cap))
						{
							return false;
						}
						int value = (int)component.ObjectId;
						_obstructionColliderResultArrAccess.SetInt(index, value);
						index++;
						result = true;
					}
					else if (collider.TryGetComponentInParent<CitySpaceManTrigger>(out result2) && result2.ObjectId > 0)
					{
						if (index > cap && !ResizeObstacleColliderLuaArray(index, ref cap))
						{
							return false;
						}
						int value2 = (int)result2.ObjectId;
						_obstructionColliderResultArrAccess.SetInt(index, value2);
						index++;
						result = true;
					}
					else
					{
						if (index > cap && !ResizeObstacleColliderLuaArray(index, ref cap))
						{
							return false;
						}
						_obstructionColliderResultArrAccess.SetInt(index, 0);
						index++;
					}
				}
			}
		}
		if (index > cap && !ResizeObstacleColliderLuaArray(index, ref cap))
		{
			return false;
		}
		_obstructionColliderResultArrAccess.SetInt(index, -1);
		index++;
		return result;
	}

	public static void ResetObstacleColliderData()
	{
		if (_obstructionColliderData != null)
		{
			_obstructionColliderData.lastCenterPos = Vector3.zero;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool ResizeObstacleColliderLuaArray(int index, ref int cap)
	{
		if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEObstacleCollider", index))
		{
			Log.Error($"BattleColliderUtils.ResizeObstacleColliderLuaArray(): resize array error : {index}");
			return false;
		}
		cap = (int)_obstructionColliderResultArrAccess.GetArrayCapacity();
		if (index > cap)
		{
			Log.Error($"BattleColliderUtils.ResizeObstacleColliderLuaArray(): resize array error : {index}");
			return false;
		}
		return true;
	}

	public static void ClearObstacleColliderData()
	{
		if (_obstructionColliderData != null)
		{
			ReleaseObstacleColliderData(_obstructionColliderData);
			_obstructionColliderData = null;
		}
	}

	private static PlayerColliderData GetPlayerColliderData()
	{
		if (_playerColliderDataPool.Count > 0)
		{
			return _playerColliderDataPool.Pop();
		}
		return new PlayerColliderData();
	}

	private static void ReleasePlayerColliderData(PlayerColliderData playerColliderData)
	{
		if (playerColliderData != null)
		{
			playerColliderData.Clear();
			_playerColliderDataPool.Push(playerColliderData);
		}
	}

	private static void AddPlayerColliderImp(Transform transform, Collider collider, int objId, int layerMask)
	{
		PlayerColliderData playerColliderData = null;
		if (collider is CapsuleCollider capsuleCollider)
		{
			playerColliderData = GetPlayerColliderData();
			playerColliderData.uid = objId;
			playerColliderData.colliderType = ColliderType.Capsule;
			playerColliderData.heightScale = 1f;
			float num = (playerColliderData.radius = capsuleCollider.radius);
			playerColliderData.height = capsuleCollider.height;
			float num2 = Mathf.Max(0f, playerColliderData.height * playerColliderData.heightScale * 0.5f - num);
			playerColliderData.colliderDir = capsuleCollider.direction;
			switch (playerColliderData.colliderDir)
			{
			case 0:
				playerColliderData.halfVector = Vector3.right * num2;
				break;
			case 1:
				playerColliderData.halfVector = Vector3.up * num2;
				break;
			case 2:
				playerColliderData.halfVector = Vector3.forward * num2;
				break;
			}
			playerColliderData.targetLayerMask = layerMask;
			playerColliderData.colliderCenter = capsuleCollider.center;
		}
		if (playerColliderData != null)
		{
			playerColliderData.transform = transform;
			_playerColliderData = playerColliderData;
		}
	}

	public static void AddPlayerCollider(int viewHandle, int objId, int layerMask, GameObject colliderRoot = null)
	{
		Transform transform = UnitViewFacade.GetTransform(viewHandle);
		if (transform == null)
		{
			return;
		}
		Collider collider = null;
		collider = ((colliderRoot == null) ? UnitViewFacade.GetCollider(viewHandle) : colliderRoot.GetComponent<Collider>());
		if (!(collider == null))
		{
			int unitObjId = UnitViewFacade.GetUnitObjId(viewHandle);
			if (unitObjId == objId)
			{
				AddPlayerColliderImp(transform, collider, unitObjId, layerMask);
			}
		}
	}

	public static void ChangePlayerCollider(int objId, float heightScale)
	{
		if (_playerColliderData != null && _playerColliderData.uid == objId)
		{
			_playerColliderData.heightScale = heightScale;
			float num = Mathf.Max(0f, _playerColliderData.height * _playerColliderData.heightScale * 0.5f - _playerColliderData.radius);
			switch (_playerColliderData.colliderDir)
			{
			case 0:
				_playerColliderData.halfVector = Vector3.right * num;
				break;
			case 1:
				_playerColliderData.halfVector = Vector3.up * num;
				break;
			case 2:
				_playerColliderData.halfVector = Vector3.forward * num;
				break;
			}
		}
	}

	public static void RemovePlayerCollider()
	{
		if (_playerColliderData != null)
		{
			ReleasePlayerColliderData(_playerColliderData);
			_playerColliderData = null;
		}
		_cacheAttackerCollideZMap.Clear();
	}

	public static void InitPlayerColliderResultAccess(LuaArrAccess access)
	{
		_playerColliderResultArrAccess = access;
	}

	public static void UnInitPlayerColliderResultAccess()
	{
		_playerColliderResultArrAccess = null;
		_cacheAttackerCollideZMap.Clear();
		_cacheAttackerCollideDirMap.Clear();
	}

	public static bool PlayerColliderLuaArray()
	{
		if (_playerColliderData == null)
		{
			return false;
		}
		if (_playerColliderResultArrAccess == null)
		{
			return false;
		}
		int num = 1;
		int cap = (int)_playerColliderResultArrAccess.GetArrayCapacity();
		if (_playerColliderData != null)
		{
			bool flag = false;
			_cacheAttackerCollideZMap.Clear();
			int num2 = 0;
			Vector3 position = _playerColliderData.colliderCenter * _playerColliderData.heightScale;
			Vector3 vector = _playerColliderData.transform.TransformPoint(position);
			if (_playerColliderData.colliderType == ColliderType.Capsule)
			{
				if (_playerColliderData.lastCenterPos == Vector3.zero)
				{
					flag = true;
					num2 = Physics.OverlapCapsuleNonAlloc(vector + _playerColliderData.halfVector, vector - _playerColliderData.halfVector, _playerColliderData.radius, _playerColliders, _playerColliderData.targetLayerMask);
				}
				else
				{
					Vector3 lastCenterPos = _playerColliderData.lastCenterPos;
					Vector3 vector2 = vector - lastCenterPos;
					num2 = Physics.CapsuleCastNonAlloc(lastCenterPos + _playerColliderData.halfVector, lastCenterPos - _playerColliderData.halfVector, _playerColliderData.radius, Vector3.Normalize(vector2), _playerHits, Vector3.Magnitude(vector2), _playerColliderData.targetLayerMask);
				}
				_playerColliderData.lastCenterPos = vector;
			}
			if (num2 > 0)
			{
				if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
				{
					return false;
				}
				_playerColliderResultArrAccess.SetInt(num, num2);
				num++;
				if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
				{
					return false;
				}
				for (int i = 0; i < num2; i++)
				{
					Collider collider = null;
					float num3 = 0f;
					if (flag)
					{
						collider = _playerColliders[i];
					}
					else
					{
						RaycastHit raycastHit = _playerHits[i];
						collider = raycastHit.collider;
						num3 = raycastHit.point.z - _playerColliderData.radius;
					}
					CitySpaceManTrigger result;
					if (collider.TryGetComponent<CitySpaceManTrigger>(out var component) && component.ObjectId > 0)
					{
						if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
						{
							return false;
						}
						int num4 = (int)component.ObjectId;
						_playerColliderResultArrAccess.SetInt(num, num4);
						num++;
						if (num3 > 0f)
						{
							_cacheAttackerCollideZMap[num4] = num3;
						}
					}
					else if (collider.TryGetComponentInParent<CitySpaceManTrigger>(out result) && result.ObjectId > 0)
					{
						if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
						{
							return false;
						}
						int num5 = (int)result.ObjectId;
						_playerColliderResultArrAccess.SetInt(num, num5);
						num++;
						if (num3 > 0f)
						{
							_cacheAttackerCollideZMap[num5] = num3;
						}
					}
					else
					{
						if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
						{
							return false;
						}
						_playerColliderResultArrAccess.SetInt(num, 0);
						num++;
					}
				}
			}
		}
		if (num > cap && !ResizePlayerColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_playerColliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	public static void ResetPlayerColliderData()
	{
		if (_playerColliderData != null)
		{
			_playerColliderData.lastCenterPos = Vector3.zero;
		}
	}

	public static float TryGetSurfingPlayerCollideZ(int attackerId)
	{
		if (_cacheAttackerCollideZMap.TryGetValue(attackerId, out var value))
		{
			return value;
		}
		return 0f;
	}

	public static bool TryGetComponentInParent<T>(this Component component, out T result) where T : Component
	{
		result = component.GetComponentInParent<T>();
		return result != null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool ResizePlayerColliderLuaArray(int index, ref int cap)
	{
		if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEPlayerCollider", index))
		{
			Log.Error($"BattleColliderUtils.ResizePlayerColliderLuaArray(): resize array error : {index}");
			return false;
		}
		cap = (int)_playerColliderResultArrAccess.GetArrayCapacity();
		if (index > cap)
		{
			Log.Error($"BattleColliderUtils.ResizePlayerColliderLuaArray(): resize array error : {index}");
			return false;
		}
		return true;
	}

	public static void ClearPlayerColliderData()
	{
		if (_playerColliderData != null)
		{
			ReleasePlayerColliderData(_playerColliderData);
			_playerColliderData = null;
		}
	}

	private static UnitColliderData GetUnitColliderData()
	{
		if (_unitColliderDataPool.Count > 0)
		{
			return _unitColliderDataPool.Pop();
		}
		return new UnitColliderData();
	}

	private static void ReleaseUnitColliderData(UnitColliderData unitColliderData)
	{
		if (unitColliderData != null)
		{
			unitColliderData.Clear();
			_unitColliderDataPool.Push(unitColliderData);
		}
	}

	private static void AddUnitColliderImp(Transform transform, Collider collider, int objId, int layerMask)
	{
		UnitColliderData unitColliderData = null;
		if (collider is BoxCollider boxCollider)
		{
			unitColliderData = GetUnitColliderData();
			unitColliderData.uid = objId;
			unitColliderData.colliderType = ColliderType.Box;
			unitColliderData.halfExtents = boxCollider.size / 2f;
			unitColliderData.targetLayerMask = layerMask;
			unitColliderData.colliderCenter = boxCollider.center;
		}
		if (unitColliderData != null)
		{
			unitColliderData.transform = transform;
			_unitColliderData = unitColliderData;
		}
	}

	public static void AddUnitCollider(int viewHandle, int objId, int layerMask, GameObject colliderRoot = null)
	{
		Transform transform = UnitViewFacade.GetTransform(viewHandle);
		if (transform == null)
		{
			return;
		}
		Collider collider = null;
		collider = ((colliderRoot == null) ? UnitViewFacade.GetCollider(viewHandle) : colliderRoot.GetComponent<Collider>());
		if (!(collider == null))
		{
			int unitObjId = UnitViewFacade.GetUnitObjId(viewHandle);
			if (unitObjId == objId)
			{
				AddUnitColliderImp(transform, collider, unitObjId, layerMask);
			}
		}
	}

	public static void RemoveUnitCollider()
	{
		if (_unitColliderData != null)
		{
			ReleaseUnitColliderData(_unitColliderData);
			_unitColliderData = null;
		}
	}

	public static void InitUnitColliderResultAccess(LuaArrAccess access)
	{
		_unitColliderResultArrAccess = access;
	}

	public static void UnInitUnitColliderResultAccess()
	{
		_unitColliderResultArrAccess = null;
	}

	public static bool UnitColliderLuaArray()
	{
		if (_unitColliderData == null)
		{
			return false;
		}
		if (_unitColliderResultArrAccess == null)
		{
			return false;
		}
		int num = 1;
		int cap = (int)_unitColliderResultArrAccess.GetArrayCapacity();
		if (_unitColliderData != null)
		{
			int num2 = 0;
			Vector3 center = _unitColliderData.transform.TransformPoint(_unitColliderData.colliderCenter);
			if (_unitColliderData.colliderType == ColliderType.Box)
			{
				num2 = Physics.OverlapBoxNonAlloc(center, _unitColliderData.halfExtents, _unitColliders, _unitColliderData.transform.rotation, _unitColliderData.targetLayerMask);
			}
			if (num2 > 0)
			{
				if (num > cap && !ResizeUnitColliderLuaArray(num, ref cap))
				{
					return false;
				}
				_unitColliderResultArrAccess.SetInt(num, num2);
				num++;
				if (num > cap && !ResizeUnitColliderLuaArray(num, ref cap))
				{
					return false;
				}
				for (int i = 0; i < num2; i++)
				{
					if (_unitColliders[i].TryGetComponent<CitySpaceManTrigger>(out var component) && component.ObjectId > 0)
					{
						if (num > cap && !ResizeUnitColliderLuaArray(num, ref cap))
						{
							return false;
						}
						_unitColliderResultArrAccess.SetInt(num, (int)component.ObjectId);
						num++;
					}
					else
					{
						if (num > cap && !ResizeUnitColliderLuaArray(num, ref cap))
						{
							return false;
						}
						_unitColliderResultArrAccess.SetInt(num, 0);
						num++;
					}
				}
			}
		}
		if (num > cap && !ResizeUnitColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_unitColliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool ResizeUnitColliderLuaArray(int index, ref int cap)
	{
		if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEUnitCollider", index))
		{
			Log.Error($"BattleColliderUtils.ResizeUnitColliderLuaArray(): resize array error : {index}");
			return false;
		}
		cap = (int)_unitColliderResultArrAccess.GetArrayCapacity();
		if (index > cap)
		{
			Log.Error($"BattleColliderUtils.ResizeUnitColliderLuaArray(): resize array error : {index}");
			return false;
		}
		return true;
	}

	public static void ClearUnitColliderData()
	{
		if (_unitColliderData != null)
		{
			ReleaseUnitColliderData(_unitColliderData);
			_unitColliderData = null;
		}
	}

	public static void EnableCollider2D(bool enable)
	{
		_useCollider2D = enable;
	}

	public static bool IsCollider2D()
	{
		return _useCollider2D;
	}

	private static BulletData GetBulletData()
	{
		if (_bulletDataPool.Count > 0)
		{
			return _bulletDataPool.Pop();
		}
		return new BulletData();
	}

	private static void ReleaseBulletData(BulletData bulletData)
	{
		if (bulletData != null)
		{
			bulletData.Clear();
			_bulletDataPool.Push(bulletData);
		}
	}

	public static void AddBullet(Transform transform, long uid, float radius, int targetLayerMask)
	{
		if (_bulletDataList == null)
		{
			_bulletDataList = new IndexedList<BulletData>(512);
			_bulletIdMap = new Dictionary<long, int>();
		}
		BulletData bulletData = GetBulletData();
		bulletData.uid = (int)uid;
		bulletData.radius = radius;
		bulletData.targetLayerMask = targetLayerMask;
		bulletData.transform = transform;
		bulletData.lastPos = transform.position;
		bulletData.dotCD = 0f;
		bulletData.dotCDMax = 0f;
		int value = _bulletDataList.Add(bulletData);
		_bulletIdMap[uid] = value;
	}

	public static void AddSphereBullet(int viewHandle, long uid, float radius, int targetLayerMask)
	{
		Transform bulletViewTransform = BulletViewFacade.GetBulletViewTransform(viewHandle);
		if (!(bulletViewTransform == null))
		{
			AddBullet(bulletViewTransform, uid, radius, targetLayerMask);
		}
	}

	public static void AddCapsuleBullet(Transform transform, long uid, float radius, int targetLayerMask, float startOffsetX, float startOffsetY, float startOffsetZ, float endOffsetX, float endOffsetY, float endOffsetZ)
	{
		if (_bulletDataList == null)
		{
			_bulletDataList = new IndexedList<BulletData>(512);
			_bulletIdMap = new Dictionary<long, int>(512);
		}
		BulletData bulletData = GetBulletData();
		bulletData.uid = (int)uid;
		bulletData.radius = radius;
		bulletData.targetLayerMask = targetLayerMask;
		bulletData.transform = transform;
		bulletData.capsule = true;
		bulletData.startOffset = new Vector3(startOffsetX, startOffsetY, startOffsetZ);
		bulletData.endOffset = new Vector3(endOffsetX, endOffsetY, endOffsetZ);
		bulletData.lastPos = transform.TransformPoint(bulletData.startOffset);
		bulletData.dotCD = 0f;
		bulletData.dotCDMax = 0f;
		int value = _bulletDataList.Add(bulletData);
		_bulletIdMap[uid] = value;
	}

	public static void AddCapsuleBullet(int viewHandle, long uid, float radius, int targetLayerMask, float startOffsetX, float startOffsetY, float startOffsetZ, float endOffsetX, float endOffsetY, float endOffsetZ)
	{
		Transform bulletViewTransform = BulletViewFacade.GetBulletViewTransform(viewHandle);
		if (!(bulletViewTransform == null))
		{
			AddCapsuleBullet(bulletViewTransform, uid, radius, targetLayerMask, startOffsetX, startOffsetY, startOffsetZ, endOffsetX, endOffsetY, endOffsetZ);
		}
	}

	public static void SetBulletDotCD(long uid, float dotCD)
	{
		if (_bulletIdMap == null || !_bulletIdMap.TryGetValue(uid, out var value))
		{
			return;
		}
		BulletData bulletData = _bulletDataList[value];
		if (bulletData != null)
		{
			if (bulletData.uid == uid)
			{
				bulletData.dotCDMax = dotCD;
			}
			else
			{
				Log.Error($"Can't change bullet dot CD index : {value}; uid：{bulletData.uid} -> {uid}");
			}
		}
	}

	public static void RemoveBullet(long uid)
	{
		if (_bulletIdMap != null && _bulletIdMap.TryGetValue(uid, out var value))
		{
			BulletData bulletData = _bulletDataList.Remove(value);
			_bulletIdMap.Remove(uid);
			ReleaseBulletData(bulletData);
		}
	}

	public static void InitBulletColliderResultAccess(LuaArrAccess arrAccess)
	{
		_colliderResultArrAccess = arrAccess;
	}

	public static void UnInitBulletColliderResultAccess()
	{
		_colliderResultArrAccess = null;
	}

	public static bool BulletColliderTest()
	{
		if (_colliderResultArrAccess == null)
		{
			return false;
		}
		int arrayCapacity = (int)_colliderResultArrAccess.GetArrayCapacity();
		int i = 0;
		for (int num = arrayCapacity * 3; i < num; i++)
		{
			if (i < arrayCapacity)
			{
				_colliderResultArrAccess.SetInt(i + 1, i);
				continue;
			}
			if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEBulletCollider", arrayCapacity + 1))
			{
				break;
			}
			arrayCapacity = (int)_colliderResultArrAccess.GetArrayCapacity();
			if (i < arrayCapacity)
			{
				_colliderResultArrAccess.SetInt(i + 1, i);
			}
		}
		return true;
	}

	public static LuaTable BulletCollider(float deltaTime)
	{
		if (_bulletDataList == null)
		{
			return null;
		}
		if (_resultLuaTable == null)
		{
			_resultLuaTable = GameEntry.Lua.Env.NewTable();
		}
		if (_useCollider2D)
		{
			Bullet2DColliderLuaTable(deltaTime);
		}
		else
		{
			Bullet3DColliderLuaTable(deltaTime);
		}
		return _resultLuaTable;
	}

	private static void Bullet2DColliderLuaTable(float deltaTime)
	{
		int count = _bulletDataList.Count;
		int num = 1;
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			BulletData bulletData = _bulletDataList[num2];
			if (bulletData != null && bulletData.CheckCD(deltaTime))
			{
				int num3 = 0;
				if (bulletData.capsule)
				{
					Transform transform = bulletData.transform;
					Vector3 lastPos = bulletData.lastPos;
					Vector3 lastPos2 = transform.TransformPoint(bulletData.startOffset);
					Vector3 end = transform.TransformPoint(bulletData.endOffset);
					Debug.DrawLine(lastPos, end, Color.red);
					bulletData.lastPos = lastPos2;
					num3 = Collider2DUtils.OverlapFastCapsule2DCollider(new Vector2(lastPos.x, lastPos.z), new Vector2(end.x, end.z), bulletData.radius, bulletData.targetLayerMask, ref _ColliderList);
				}
				else
				{
					Vector3 lastPos3 = bulletData.lastPos;
					Vector3 localPosition = bulletData.transform.localPosition;
					Debug.DrawLine(lastPos3, localPosition, Color.red);
					bulletData.lastPos = localPosition;
					num3 = Collider2DUtils.OverlapFastCapsule2DCollider(new Vector2(lastPos3.x, lastPos3.z), new Vector2(localPosition.x, localPosition.z), bulletData.radius, bulletData.targetLayerMask, ref _ColliderList);
				}
				if (num3 > 0)
				{
					_resultLuaTable.SetLong(num, bulletData.uid);
					num++;
					_resultLuaTable.SetLong(num, num3);
					num++;
					for (int i = 0; i < num3; i++)
					{
						int num4 = _ColliderList[i];
						if (num4 > 0)
						{
							_resultLuaTable.SetLong(num, num4);
							num++;
						}
						else
						{
							_resultLuaTable.SetLong(num, 0L);
							num++;
						}
					}
				}
			}
		}
		_resultLuaTable.SetLong(num, -1L);
	}

	private static void Bullet3DColliderLuaTable(float deltaTime)
	{
		int count = _bulletDataList.Count;
		int num = 1;
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			BulletData bulletData = _bulletDataList[num2];
			if (bulletData != null && bulletData.CheckCD(deltaTime))
			{
				int num3 = 0;
				if (bulletData.capsule)
				{
					Transform transform = bulletData.transform;
					Vector3 lastPos = bulletData.lastPos;
					Vector3 lastPos2 = transform.TransformPoint(bulletData.startOffset);
					Vector3 vector = transform.TransformPoint(bulletData.endOffset);
					Debug.DrawLine(lastPos, vector, Color.red);
					bulletData.lastPos = lastPos2;
					num3 = Physics.OverlapCapsuleNonAlloc(lastPos, vector, bulletData.radius, _colliders, bulletData.targetLayerMask);
				}
				else
				{
					Vector3 lastPos3 = bulletData.lastPos;
					Vector3 localPosition = bulletData.transform.localPosition;
					Debug.DrawLine(lastPos3, localPosition, Color.red);
					bulletData.lastPos = localPosition;
					num3 = Physics.OverlapCapsuleNonAlloc(lastPos3, localPosition, bulletData.radius, _colliders, bulletData.targetLayerMask);
				}
				if (num3 > 0)
				{
					_resultLuaTable.SetLong(num, bulletData.uid);
					num++;
					_resultLuaTable.SetLong(num, num3);
					num++;
					for (int i = 0; i < num3; i++)
					{
						CitySpaceManTrigger component = _colliders[i].GetComponent<CitySpaceManTrigger>();
						if (component != null && component.ObjectId > 0)
						{
							_resultLuaTable.SetLong(num, component.ObjectId);
							num++;
						}
						else
						{
							_resultLuaTable.SetLong(num, 0L);
							num++;
						}
					}
				}
			}
		}
		_resultLuaTable.SetLong(num, -1L);
	}

	public static void EnterBattle(bool useCollider2D = false)
	{
		_useCollider2D = useCollider2D;
	}

	public static void ExitBattle()
	{
		_useCollider2D = false;
		Collider2DUtils.Clear();
	}

	public static void UpdateCollider()
	{
		if (_useCollider2D)
		{
			Collider2DUtils.UpdateAgents();
			Collider2DUtils.Build(Time.frameCount);
		}
	}

	public static bool BulletColliderLuaArray(float deltaTime)
	{
		if (_bulletDataList == null)
		{
			return false;
		}
		if (_colliderResultArrAccess == null)
		{
			return false;
		}
		if (_useCollider2D)
		{
			return Bullet2DCollider(deltaTime);
		}
		return Bullet3DCollider(deltaTime);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool ResizeColliderLuaArray(int index, ref int cap)
	{
		if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEBulletCollider", index))
		{
			Log.Error($"BattleColliderUtils.BulletColliderLuaArray(): resize array error : {index}");
			return false;
		}
		cap = (int)_colliderResultArrAccess.GetArrayCapacity();
		if (index > cap)
		{
			Log.Error($"BattleColliderUtils.BulletColliderLuaArray(): resize array error : {index}");
			return false;
		}
		return true;
	}

	private static bool Bullet3DCollider(float deltaTime)
	{
		int count = _bulletDataList.Count;
		int num = 1;
		int cap = (int)_colliderResultArrAccess.GetArrayCapacity();
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			BulletData bulletData = _bulletDataList[num2];
			if (bulletData != null && bulletData.CheckCD(deltaTime))
			{
				int num3 = 0;
				if (bulletData.capsule)
				{
					Transform transform = bulletData.transform;
					Vector3 lastPos = bulletData.lastPos;
					Vector3 lastPos2 = transform.TransformPoint(bulletData.startOffset);
					Vector3 point = transform.TransformPoint(bulletData.endOffset);
					bulletData.lastPos = lastPos2;
					num3 = Physics.OverlapCapsuleNonAlloc(lastPos, point, bulletData.radius, _colliders, bulletData.targetLayerMask);
				}
				else
				{
					num3 = Physics.OverlapCapsuleNonAlloc(bulletData.lastPos, bulletData.lastPos = bulletData.transform.localPosition, bulletData.radius, _colliders, bulletData.targetLayerMask);
				}
				if (num3 > 0)
				{
					if (num > cap && !ResizeColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_colliderResultArrAccess.SetInt(num, bulletData.uid);
					num++;
					if (num > cap && !ResizeColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_colliderResultArrAccess.SetInt(num, num3);
					num++;
					for (int i = 0; i < num3; i++)
					{
						if (_colliders[i].TryGetComponent<CitySpaceManTrigger>(out var component) && component.ObjectId > 0)
						{
							if (num > cap && !ResizeColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_colliderResultArrAccess.SetInt(num, (int)component.ObjectId);
							num++;
						}
						else
						{
							if (num > cap && !ResizeColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_colliderResultArrAccess.SetInt(num, 0);
							num++;
						}
					}
				}
			}
		}
		if (num > cap && !ResizeColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_colliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	private static bool Bullet2DCollider(float deltaTime)
	{
		if (_ColliderList == null)
		{
			_ColliderList = new int[100];
		}
		int count = _bulletDataList.Count;
		int num = 1;
		int cap = (int)_colliderResultArrAccess.GetArrayCapacity();
		for (int num2 = count - 1; num2 >= 0; num2--)
		{
			BulletData bulletData = _bulletDataList[num2];
			if (bulletData != null && bulletData.CheckCD(deltaTime))
			{
				int num3 = 0;
				if (bulletData.capsule)
				{
					Transform transform = bulletData.transform;
					Vector3 lastPos = bulletData.lastPos;
					Vector3 lastPos2 = transform.TransformPoint(bulletData.startOffset);
					Vector3 vector = transform.TransformPoint(bulletData.endOffset);
					bulletData.lastPos = lastPos2;
					num3 = Collider2DUtils.OverlapFastCapsule2DCollider(new Vector2(lastPos.x, lastPos.z), new Vector2(vector.x, vector.z), bulletData.radius, bulletData.targetLayerMask, ref _ColliderList);
				}
				else
				{
					Vector3 lastPos3 = bulletData.lastPos;
					Vector3 vector2 = (bulletData.lastPos = bulletData.transform.localPosition);
					num3 = Collider2DUtils.OverlapFastCapsule2DCollider(new Vector2(lastPos3.x, lastPos3.z), new Vector2(vector2.x, vector2.z), bulletData.radius, bulletData.targetLayerMask, ref _ColliderList);
				}
				if (num3 > 0)
				{
					if (num > cap && !ResizeColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_colliderResultArrAccess.SetInt(num, bulletData.uid);
					num++;
					if (num > cap && !ResizeColliderLuaArray(num, ref cap))
					{
						return false;
					}
					_colliderResultArrAccess.SetInt(num, num3);
					num++;
					for (int i = 0; i < num3; i++)
					{
						int num4 = _ColliderList[i];
						if (num4 > 0)
						{
							if (num > cap && !ResizeColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_colliderResultArrAccess.SetInt(num, num4);
							num++;
						}
						else
						{
							if (num > cap && !ResizeColliderLuaArray(num, ref cap))
							{
								return false;
							}
							_colliderResultArrAccess.SetInt(num, 0);
							num++;
						}
					}
				}
			}
		}
		if (num > cap && !ResizeColliderLuaArray(num, ref cap))
		{
			return false;
		}
		_colliderResultArrAccess.SetInt(num, -1);
		return true;
	}

	public static void AddCollider2DAgent(int uid, int colliderId, Collider unityCollider)
	{
		if (_useCollider2D && !(unityCollider == null))
		{
			Collider2DUtils.AddAgent(uid, colliderId, unityCollider);
		}
	}

	public static void RemoveCollider2DAgent(long uid)
	{
		if (_useCollider2D)
		{
			Collider2DUtils.RemoveAgent(uid);
		}
	}

	public static void ClearBulletData()
	{
		if (_bulletDataList == null)
		{
			return;
		}
		for (int num = _bulletDataList.Count - 1; num >= 0; num--)
		{
			BulletData bulletData = _bulletDataList[num];
			if (bulletData != null)
			{
				_bulletIdMap.Remove(bulletData.uid);
				_bulletDataList.Remove(num);
				ReleaseBulletData(bulletData);
			}
		}
		_bulletDataList.Clear();
		if (_resultLuaTable != null)
		{
			_resultLuaTable.Dispose();
			_resultLuaTable = null;
		}
	}

	public static void Dispose()
	{
		ClearBulletData();
		ClearMonsterColliderData();
		Collider2DUtils.Clear();
		UnInitBulletColliderResultAccess();
		UnInitMonsterColliderResultAccess();
		UnInitOverlapSphereNonAllocAccess();
		UnInitUnitColliderResultAccess();
		UnInitPlayerColliderResultAccess();
	}

	public static void GetColliderClosestPoint(Collider collider, float x, float y, float z, out float pointX, out float pointY, out float pointZ)
	{
		Vector3 vector = collider.ClosestPoint(new Vector3(x, y, z));
		pointX = vector.x;
		pointY = vector.y;
		pointZ = vector.z;
	}

	public static Vector3 GetInverseTransformPoint(Transform transform, float x, float y, float z)
	{
		return transform.InverseTransformPoint(new Vector3(x, y, z));
	}

	public static void InitOverlapSphereNonAllocAccess(int arrayCount, LuaArrAccess luaArrAccess, LuaArrAccess resultLuaAccess)
	{
		_overlapSphereNonAllocParamAccess = luaArrAccess;
		_overlapSphereNonAllocResultAccess = resultLuaAccess;
		_overlapSphereNonAllocColliders = new Collider[arrayCount];
	}

	public static void UnInitOverlapSphereNonAllocAccess()
	{
		_overlapSphereNonAllocParamAccess = null;
		_overlapSphereNonAllocResultAccess = null;
	}

	public static void OverlapSphereNonAlloc(bool sortByDistance = false)
	{
		float x = (float)_overlapSphereNonAllocParamAccess.GetDouble(1);
		float y = (float)_overlapSphereNonAllocParamAccess.GetDouble(2);
		float z = (float)_overlapSphereNonAllocParamAccess.GetDouble(3);
		float radius = (float)_overlapSphereNonAllocParamAccess.GetDouble(4);
		int layerMask = _overlapSphereNonAllocParamAccess.GetInt(5);
		float x2 = (float)_overlapSphereNonAllocParamAccess.GetDouble(6);
		float y2 = (float)_overlapSphereNonAllocParamAccess.GetDouble(7);
		float z2 = (float)_overlapSphereNonAllocParamAccess.GetDouble(8);
		Vector3 center = new Vector3(x, y, z);
		Vector3 targetPos = new Vector3(x2, y2, z2);
		if (_useCollider2D)
		{
			OverlapSphereCollider2D(new Vector2(center.x, center.z), new Vector2(targetPos.x, targetPos.z), radius, layerMask);
		}
		else
		{
			OverlapSphereCollider3D(center, targetPos, radius, layerMask, sortByDistance);
		}
	}

	private static void OverlapSphereCollider3D(Vector3 center, Vector3 targetPos, float radius, int layerMask, bool sortByDistance)
	{
		int num = Physics.OverlapSphereNonAlloc(center, radius, _overlapSphereNonAllocColliders, layerMask);
		if (num > 1 && sortByDistance)
		{
			Array.Sort(_overlapSphereNonAllocColliders, 0, num, Comparer<Collider>.Create(delegate(Collider p1, Collider p2)
			{
				Vector3 position = p1.transform.position;
				Vector3 position2 = p2.transform.position;
				return (Mathf.Abs(position.x - targetPos.x) + Mathf.Abs(position.z - targetPos.z)).CompareTo(Mathf.Abs(position2.x - targetPos.x) + Mathf.Abs(position2.z - targetPos.z));
			}));
		}
		_overlapSphereNonAllocResultAccess.SetInt(1, num);
		if (num <= 0)
		{
			return;
		}
		int num2 = 2;
		for (int num3 = 0; num3 < num; num3++)
		{
			if (_overlapSphereNonAllocColliders[num3].TryGetComponent<CitySpaceManTrigger>(out var component))
			{
				int value = (int)component.ObjectId;
				_overlapSphereNonAllocResultAccess.SetInt(num2++, value);
			}
			else
			{
				_overlapSphereNonAllocResultAccess.SetInt(num2++, 0);
			}
		}
	}

	private static void OverlapSphereCollider2D(Vector2 center, Vector2 targetPos, float radius, int layerMask)
	{
		if (_ColliderList == null)
		{
			_ColliderList = new int[100];
		}
		int num = Collider2DUtils.OverlapCircle2DCollider(center, targetPos, radius, layerMask, ref _ColliderList);
		_overlapSphereNonAllocResultAccess.SetInt(1, num);
		if (num <= 0)
		{
			return;
		}
		int num2 = 2;
		for (int i = 0; i < num; i++)
		{
			int num3 = _ColliderList[i];
			if (num3 > 0)
			{
				_overlapSphereNonAllocResultAccess.SetInt(num2++, num3);
			}
			else
			{
				_overlapSphereNonAllocResultAccess.SetInt(num2++, 0);
			}
		}
	}
}
