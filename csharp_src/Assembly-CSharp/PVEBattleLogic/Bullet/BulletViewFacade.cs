using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework;
using PVEBattleLogic.Unit;
using UnityEngine;
using XLua;

namespace PVEBattleLogic.Bullet;

public static class BulletViewFacade
{
	public const int INVALID_HANDLE = -1;

	private static Dictionary<string, Stack<BulletGameObject>> _bulletViewPool;

	private static Dictionary<string, Stack<BulletStraightGameObject>> _bulletViewStraightPool;

	private static BattleIndexedList<BulletGameObject> _bulletViewList;

	private static LuaTable _loadedLuaTable;

	private static List<int> _bulletViewStraightHandleList;

	private static Dictionary<string, AnimationCurve> _animationCurves;

	private static LuaTable _toRemoveStraightTable;

	private static Dictionary<int, BulletStraightViewParam> _straightViewParamMap;

	private static Dictionary<string, int> _preloadCache = new Dictionary<string, int>();

	private static HashSet<string> _preloadSet = new HashSet<string>();

	private static HashSet<string> _preloadNormalFlags = new HashSet<string>();

	private static List<string> _toRemovePreload = new List<string>(8);

	private static List<int> _tmpStraightLoadedList;

	private static List<float> _tmpStraightDotCDList;

	private static LuaArrAccess _bulletViewArrAccess;

	private static LuaArrAccess _createStraightArrAccess;

	private static LuaArrAccess _createStraightListArrAccess;

	private static Dictionary<int, string> _bulletViewNameMap;

	private static Dictionary<int, string> _bulletAnimationCurveMap;

	private static BulletStraightGatlingViewParam _straightGatlingViewParam;

	private static LuaArrAccess _createGatlingStraightArrAccess;

	private static int _straightParamCount = 32;

	private static int _gatlingParamCount = 5;

	public static void Init()
	{
		if (_bulletViewPool == null)
		{
			_bulletViewPool = new Dictionary<string, Stack<BulletGameObject>>();
		}
		if (_bulletViewStraightPool == null)
		{
			_bulletViewStraightPool = new Dictionary<string, Stack<BulletStraightGameObject>>();
		}
		if (_bulletViewList == null)
		{
			_bulletViewList = new BattleIndexedList<BulletGameObject>(4096);
		}
		if (_bulletViewStraightHandleList == null)
		{
			_bulletViewStraightHandleList = new List<int>(2048);
		}
		if (_animationCurves == null)
		{
			_animationCurves = new Dictionary<string, AnimationCurve>();
		}
		if (_straightViewParamMap == null)
		{
			_straightViewParamMap = new Dictionary<int, BulletStraightViewParam>(16);
		}
		if (_tmpStraightLoadedList == null)
		{
			_tmpStraightLoadedList = new List<int>(32);
		}
		if (_tmpStraightDotCDList == null)
		{
			_tmpStraightDotCDList = new List<float>(32);
		}
		if (_bulletViewNameMap == null)
		{
			_bulletViewNameMap = new Dictionary<int, string>(32);
		}
		if (_bulletAnimationCurveMap == null)
		{
			_bulletAnimationCurveMap = new Dictionary<int, string>(8);
		}
	}

	public static void SyncNameId(string bulletViewName, int bulletViewNameId)
	{
		_bulletViewNameMap[bulletViewNameId] = bulletViewName;
	}

	public static void SyncAnimationCurve(string animationCurve, int curveId)
	{
		_bulletAnimationCurveMap[curveId] = animationCurve;
	}

	internal static bool TryGetAnimationCurve(int curveId, out string animationCurve)
	{
		return _bulletAnimationCurveMap.TryGetValue(curveId, out animationCurve);
	}

	public static int CreateBulletView(int bulletViewId, out bool loaded)
	{
		loaded = false;
		if (!_bulletViewNameMap.TryGetValue(bulletViewId, out var value))
		{
			Log.Error($"BulletViewFacade.CreateBulletView({bulletViewId}): cannot find bullet view name");
			return -1;
		}
		BulletGameObject bulletGameObject;
		int result;
		if (_bulletViewPool.TryGetValue(value, out var value2) && value2.Count > 0)
		{
			bulletGameObject = value2.Pop();
			bulletGameObject.OutPool();
			result = (bulletGameObject.Handle = _bulletViewList.Add(bulletGameObject));
			loaded = bulletGameObject.IsLoaded;
			return result;
		}
		bulletGameObject = new BulletGameObject(value);
		result = (bulletGameObject.Handle = _bulletViewList.Add(bulletGameObject));
		bulletGameObject.Load();
		loaded = bulletGameObject.IsLoaded;
		return result;
	}

	public static bool IsLoaded(int handle, out float dotMaxCD)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		dotMaxCD = 0f;
		if (bulletView != null)
		{
			dotMaxCD = bulletView.GetDotMaxCD();
			return bulletView.IsLoaded;
		}
		return false;
	}

	public static void SetVisible(int handle, bool visible)
	{
		GetBulletView(handle)?.SetVisible(visible);
	}

	public static void SetParent(int handle, Transform parent)
	{
		GetBulletView(handle)?.SetParent(parent);
	}

	public static void SetLocalPosition(int handle, float x, float y, float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 localPosition = new Vector3(x, y, z);
			bulletView.SetLocalPosition(localPosition);
		}
	}

	public static void ResetLocalPosition(int handle)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 zero = Vector3.zero;
			bulletView.SetLocalPosition(zero);
		}
	}

	public static void SetPosition(int handle, float x, float y, float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 position = new Vector3(x, y, z);
			bulletView.SetPosition(position);
		}
	}

	public static void SetLocalScale(int handle, float x, float y, float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 localScale = new Vector3(x, y, z);
			bulletView.SetLocalScale(localScale);
		}
	}

	public static void SetEulerAngles(int handle, float x, float y, float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 eulerAngles = new Vector3(x, y, z);
			bulletView.SetEulerAngles(eulerAngles);
		}
	}

	public static float GetEulerAnglesY(int handle)
	{
		return GetBulletView(handle)?.EulerAngles.y ?? 0f;
	}

	public static void ResetLocalRotation(int handle)
	{
		GetBulletView(handle)?.ResetLocalRotation();
	}

	public static Vector3 GetPosition(int handle)
	{
		return GetBulletView(handle)?.Position ?? Vector3.zero;
	}

	public static void GetPositionXYZ(int handle, out float x, out float y, out float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 position = bulletView.Position;
			x = position.x;
			y = position.y;
			z = position.z;
		}
		else
		{
			x = 0f;
			z = 0f;
			y = 0f;
		}
	}

	public static void SetForward(int handle, float x, float y, float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 forward = new Vector3(x, y, z);
			bulletView.SetForward(forward);
		}
	}

	public static void GetForward(int handle, out float x, out float y, out float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 forward = bulletView.GetForward();
			x = forward.x;
			y = forward.y;
			z = forward.z;
		}
		else
		{
			x = 0f;
			y = 0f;
			z = 1f;
		}
	}

	public static void GetRight(int handle, out float x, out float y, out float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 right = bulletView.GetRight();
			x = right.x;
			y = right.y;
			z = right.z;
		}
		else
		{
			x = 1f;
			y = 0f;
			z = 0f;
		}
	}

	public static void GetUp(int handle, out float x, out float y, out float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 up = bulletView.GetUp();
			x = up.x;
			y = up.y;
			z = up.z;
		}
		else
		{
			x = 0f;
			y = 1f;
			z = 0f;
		}
	}

	public static void Translate(int handle, float translationZ)
	{
		GetBulletView(handle)?.Translate(translationZ);
	}

	public static void Translate(int handle, float translationX, float translationY, float translationZ)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 translation = new Vector3(translationX, translationY, translationZ);
			bulletView.Translate(translation);
		}
	}

	public static void LookAt(int handle, float x, float y, float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 point = new Vector3(x, y, z);
			bulletView.LookAt(point);
		}
	}

	public static void GetTransformPoint(int handle, float posX, float posY, float posZ, out float x, out float y, out float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 point = new Vector3(posX, posY, posZ);
			Vector3 transformPoint = bulletView.GetTransformPoint(point);
			x = transformPoint.x;
			y = transformPoint.y;
			z = transformPoint.z;
		}
		else
		{
			x = posX;
			y = posY;
			z = posZ;
		}
	}

	public static void GetColliderCenterWorldPos(int handle, out float x, out float y, out float z)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null)
		{
			Vector3 colliderCenterWorldPos = bulletView.GetColliderCenterWorldPos();
			x = colliderCenterWorldPos.x;
			y = colliderCenterWorldPos.y;
			z = colliderCenterWorldPos.z;
		}
		else
		{
			x = 0f;
			y = 0f;
			z = 0f;
		}
	}

	public static LineRenderer[] GetLineRendererArray(int handle)
	{
		return GetBulletView(handle)?.GetLineRendererArray();
	}

	public static bool TryGetSphereCollider(int handle, out SphereCollider sphereCollider)
	{
		sphereCollider = null;
		return GetBulletView(handle)?.TryGetSphereCollider(out sphereCollider) ?? false;
	}

	public static bool TryGetCapsuleCollider(int handle, out CapsuleCollider capsuleCollider)
	{
		capsuleCollider = null;
		return GetBulletView(handle)?.TryGetCapsuleCollider(out capsuleCollider) ?? false;
	}

	public static void ClearTrailRenderer(int handle)
	{
		GetBulletView(handle)?.ClearTrailRenderer();
	}

	public static Transform GetBulletViewTransform(int handle)
	{
		return GetBulletView(handle)?.GetTransform();
	}

	public static void SyncBulletTransform(LuaTable table)
	{
		int num = table.Get<int, int>(1);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				int num2 = 4 * i + 1;
				int handle = table.Get<int, int>(num2 + 1);
				float x = table.Get<int, float>(num2 + 2);
				float y = table.Get<int, float>(num2 + 3);
				float z = table.Get<int, float>(num2 + 4);
				GetBulletView(handle)?.SetLocalPosition(new Vector3(x, y, z));
			}
		}
	}

	public static LuaTable CheckLoaded(LuaTable sourceTable)
	{
		if (_bulletViewList == null)
		{
			Log.Error("CheckLoaded BulletViewList is null");
			return null;
		}
		if (_loadedLuaTable == null)
		{
			_loadedLuaTable = GameEntry.Lua.Env.NewTable();
		}
		int num = sourceTable.Get<int, int>(1);
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				int key = i + 2;
				int num2 = sourceTable.Get<int, int>(key);
				BulletGameObject bulletView = GetBulletView(num2);
				bool value = false;
				if (bulletView != null)
				{
					value = bulletView.IsLoaded;
				}
				_loadedLuaTable.Set(num2, value);
			}
		}
		return _loadedLuaTable;
	}

	public static void DestroyBulletView(ref int handle)
	{
		if (handle == -1)
		{
			return;
		}
		BulletGameObject bulletGameObject = _bulletViewList.Remove(handle);
		if (bulletGameObject != null && CheckHandle(handle, bulletGameObject))
		{
			bulletGameObject.InPool();
			bulletGameObject.Handle = -1;
			string path = bulletGameObject.Path;
			if (!_bulletViewPool.TryGetValue(path, out var value))
			{
				value = new Stack<BulletGameObject>(2048);
				_bulletViewPool.Add(path, value);
			}
			value.Push(bulletGameObject);
			handle = -1;
		}
	}

	private static BulletGameObject GetBulletView(int handle)
	{
		if (handle == -1)
		{
			return null;
		}
		BulletGameObject bulletGameObject = _bulletViewList[handle];
		if (bulletGameObject != null)
		{
			if (!CheckHandle(handle, bulletGameObject))
			{
				return null;
			}
			return bulletGameObject;
		}
		return null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool CheckHandle(int handle, BulletGameObject bulletView)
	{
		if (bulletView.Handle != handle)
		{
			Log.Error("Handle is not the same as the bullet view , path = " + bulletView.Path + " ");
			return false;
		}
		return true;
	}

	public static int CreateBulletViewStraight(string bulletViewName, LuaTable paramTable, out bool loaded, out float dotMaxCD)
	{
		dotMaxCD = 0f;
		loaded = false;
		BulletStraightGameObject bulletStraightGameObject;
		int num;
		if (_bulletViewStraightPool.TryGetValue(bulletViewName, out var value) && value.Count > 0)
		{
			bulletStraightGameObject = value.Pop();
			bulletStraightGameObject.OutPool();
			bulletStraightGameObject.InitParam(paramTable);
			num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
			bulletStraightGameObject.Load();
			dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
			loaded = bulletStraightGameObject.IsLoaded;
			_bulletViewStraightHandleList.Add(num);
			return num;
		}
		bulletStraightGameObject = new BulletStraightGameObject(bulletViewName);
		bulletStraightGameObject.InitParam(paramTable);
		num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
		bulletStraightGameObject.Load();
		dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
		loaded = bulletStraightGameObject.IsLoaded;
		_bulletViewStraightHandleList.Add(num);
		return num;
	}

	public static bool CreateBulletViewStraightListLuaArray(int count)
	{
		for (int i = 0; i < count; i++)
		{
			int num = _straightParamCount * i;
			int key = _createStraightListArrAccess.GetInt(num + 1);
			Stack<BulletStraightGameObject> value2;
			if (!_bulletViewNameMap.TryGetValue(key, out var value))
			{
				_createStraightListArrAccess.SetInt(num + 1, -1);
			}
			else if (_bulletViewStraightPool.TryGetValue(value, out value2) && value2.Count > 0)
			{
				BulletStraightGameObject bulletStraightGameObject = value2.Pop();
				bulletStraightGameObject.OutPool();
				bulletStraightGameObject.InitViewParam(_createStraightListArrAccess, num);
				int num2 = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
				bulletStraightGameObject.Load();
				_bulletViewStraightHandleList.Add(num2);
				_createStraightListArrAccess.SetInt(num + 1, num2);
				_createStraightListArrAccess.SetInt(num + 2, bulletStraightGameObject.IsLoaded ? 1 : 0);
				_createStraightListArrAccess.SetDouble(num + 3, bulletStraightGameObject.GetDotMaxCD());
			}
			else
			{
				BulletStraightGameObject bulletStraightGameObject = new BulletStraightGameObject(value);
				bulletStraightGameObject.InitViewParam(_createStraightListArrAccess, num);
				int num2 = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
				bulletStraightGameObject.Load();
				_bulletViewStraightHandleList.Add(num2);
				_createStraightListArrAccess.SetInt(num + 1, num2);
				_createStraightListArrAccess.SetInt(num + 2, bulletStraightGameObject.IsLoaded ? 1 : 0);
				_createStraightListArrAccess.SetDouble(num + 3, bulletStraightGameObject.GetDotMaxCD());
			}
		}
		return true;
	}

	public static int CreateBulletViewStraightLuaArray(out bool loaded, out float dotMaxCD)
	{
		dotMaxCD = 0f;
		loaded = false;
		int key = _createStraightArrAccess.GetInt(1);
		if (!_bulletViewNameMap.TryGetValue(key, out var value))
		{
			return -1;
		}
		BulletStraightGameObject bulletStraightGameObject;
		int num;
		if (_bulletViewStraightPool.TryGetValue(value, out var value2) && value2.Count > 0)
		{
			bulletStraightGameObject = value2.Pop();
			bulletStraightGameObject.OutPool();
			bulletStraightGameObject.InitViewParam(_createStraightArrAccess);
			num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
			bulletStraightGameObject.Load();
			dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
			loaded = bulletStraightGameObject.IsLoaded;
			_bulletViewStraightHandleList.Add(num);
			return num;
		}
		bulletStraightGameObject = new BulletStraightGameObject(value);
		bulletStraightGameObject.InitViewParam(_createStraightArrAccess);
		num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
		bulletStraightGameObject.Load();
		dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
		loaded = bulletStraightGameObject.IsLoaded;
		_bulletViewStraightHandleList.Add(num);
		return num;
	}

	public static int CreateBulletViewStraightParam(string bulletViewName, long bulletObjId, float startX, float startY, float startZ, float rotY, int targetLayerMask, int metaId, LuaTable paramTable, out bool loaded, out float dotMaxCD)
	{
		dotMaxCD = 0f;
		loaded = false;
		BulletStraightGameObject bulletStraightGameObject;
		int num;
		if (_bulletViewStraightPool.TryGetValue(bulletViewName, out var value) && value.Count > 0)
		{
			bulletStraightGameObject = value.Pop();
			bulletStraightGameObject.OutPool();
			bulletStraightGameObject.InitViewParam(bulletObjId, startX, startY, startZ, rotY, targetLayerMask, metaId, paramTable);
			num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
			bulletStraightGameObject.Load();
			dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
			loaded = bulletStraightGameObject.IsLoaded;
			_bulletViewStraightHandleList.Add(num);
			return num;
		}
		bulletStraightGameObject = new BulletStraightGameObject(bulletViewName);
		bulletStraightGameObject.InitViewParam(bulletObjId, startX, startY, startZ, rotY, targetLayerMask, metaId, paramTable);
		num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
		bulletStraightGameObject.Load();
		dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
		loaded = bulletStraightGameObject.IsLoaded;
		_bulletViewStraightHandleList.Add(num);
		return num;
	}

	public static int CreateBulletViewStraightParamWithInertiaVelocity(string bulletViewName, long bulletObjId, float startX, float startY, float startZ, float rotY, int targetLayerMask, float inertiaX, float inertiaY, float inertiaZ, int metaId, LuaTable paramTable, out bool loaded, out float dotMaxCD)
	{
		dotMaxCD = 0f;
		loaded = false;
		BulletStraightGameObject bulletStraightGameObject;
		int num;
		if (_bulletViewStraightPool.TryGetValue(bulletViewName, out var value) && value.Count > 0)
		{
			bulletStraightGameObject = value.Pop();
			bulletStraightGameObject.OutPool();
			bulletStraightGameObject.InitViewParamWithInertiaVelocity(bulletObjId, startX, startY, startZ, rotY, targetLayerMask, inertiaX, inertiaY, inertiaZ, metaId, paramTable);
			num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
			bulletStraightGameObject.Load();
			dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
			loaded = bulletStraightGameObject.IsLoaded;
			_bulletViewStraightHandleList.Add(num);
			return num;
		}
		bulletStraightGameObject = new BulletStraightGameObject(bulletViewName);
		bulletStraightGameObject.InitViewParamWithInertiaVelocity(bulletObjId, startX, startY, startZ, rotY, targetLayerMask, inertiaX, inertiaY, inertiaZ, metaId, paramTable);
		num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
		bulletStraightGameObject.Load();
		dotMaxCD = bulletStraightGameObject.GetDotMaxCD();
		loaded = bulletStraightGameObject.IsLoaded;
		_bulletViewStraightHandleList.Add(num);
		return num;
	}

	public static void SyncStraightGatlingViewDataLuaArray()
	{
		_straightGatlingViewParam = new BulletStraightGatlingViewParam();
		int num = _createStraightArrAccess.GetInt(1);
		if (!_bulletViewNameMap.TryGetValue(num, out var value))
		{
			Log.Error($"BulletViewFacade.SyncStraightGatlingViewData(): viewId={num} not found.");
			return;
		}
		_straightGatlingViewParam.viewName = value;
		_straightGatlingViewParam.targetLayerMask = _createStraightArrAccess.GetInt(2);
		float x = (float)_createStraightArrAccess.GetDouble(3);
		float y = (float)_createStraightArrAccess.GetDouble(4);
		float z = (float)_createStraightArrAccess.GetDouble(5);
		_straightGatlingViewParam.inertiaVelocity = new Vector3(x, y, z);
		_straightGatlingViewParam.lifeTime = (float)_createStraightArrAccess.GetDouble(6);
		_straightGatlingViewParam.bulletScale = (float)_createStraightArrAccess.GetDouble(7);
		_straightGatlingViewParam.metaColliderRadius = (float)_createStraightArrAccess.GetDouble(8);
		_straightGatlingViewParam.noCollision = _createStraightArrAccess.GetInt(9) == 1;
		_straightGatlingViewParam.duration = (float)_createStraightArrAccess.GetDouble(10);
		_straightGatlingViewParam.spiralLoops = 0f;
		_straightGatlingViewParam.spiralRadius = 0f;
		_straightGatlingViewParam.flySpeed = (float)_createStraightArrAccess.GetDouble(11);
		if (!TryGetAnimationCurve(_createStraightArrAccess.GetInt(12), out var animationCurve) || string.IsNullOrEmpty(animationCurve))
		{
			animationCurve = "0,0,2,2|1,1,0,0";
		}
		_straightGatlingViewParam.animationCurve = GetAnimationCurve(animationCurve);
		_straightGatlingViewParam.colliderType = _createStraightArrAccess.GetInt(13) == 1;
		_straightGatlingViewParam.defaultDotMaxCD = 0f;
	}

	public static bool CreateStraightGatlingViewListLuaArray(int count)
	{
		if (_straightGatlingViewParam == null)
		{
			return false;
		}
		for (int i = 0; i < count; i++)
		{
			bool flag = false;
			int num = _gatlingParamCount * i;
			int viewObjId = _createGatlingStraightArrAccess.GetInt(num + 1);
			Transform firePointById = UnitViewFacade.GetFirePointById(_createGatlingStraightArrAccess.GetInt(num + 2), 1);
			if (firePointById == null)
			{
				_createGatlingStraightArrAccess.SetInt(num + 1, -1);
				continue;
			}
			Vector3 position = firePointById.position;
			float y = firePointById.eulerAngles.y;
			_createGatlingStraightArrAccess.SetDouble(num + 2, position.x);
			_createGatlingStraightArrAccess.SetDouble(num + 3, position.y);
			_createGatlingStraightArrAccess.SetDouble(num + 4, position.z);
			string viewName = _straightGatlingViewParam.viewName;
			if (_bulletViewStraightPool.TryGetValue(viewName, out var value) && value.Count > 0)
			{
				BulletStraightGameObject bulletStraightGameObject = value.Pop();
				bulletStraightGameObject.OutPool();
				bulletStraightGameObject.InitViewParam(viewObjId, _straightGatlingViewParam, position, y);
				int num2 = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
				bulletStraightGameObject.Load();
				flag = bulletStraightGameObject.IsLoaded;
				_createGatlingStraightArrAccess.SetInt(num + 5, flag ? 1 : 0);
				_bulletViewStraightHandleList.Add(num2);
				_createGatlingStraightArrAccess.SetInt(num + 1, num2);
			}
			else
			{
				BulletStraightGameObject bulletStraightGameObject = new BulletStraightGameObject(viewName);
				bulletStraightGameObject.InitViewParam(viewObjId, _straightGatlingViewParam, position, y);
				int num2 = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
				bulletStraightGameObject.Load();
				flag = bulletStraightGameObject.IsLoaded;
				_createGatlingStraightArrAccess.SetInt(num + 5, flag ? 1 : 0);
				_bulletViewStraightHandleList.Add(num2);
				_createGatlingStraightArrAccess.SetInt(num + 1, num2);
			}
		}
		return true;
	}

	public static int CreateStraightGatlingViewLuaArray()
	{
		bool flag = false;
		if (_straightGatlingViewParam == null)
		{
			return -1;
		}
		int viewObjId = _createStraightArrAccess.GetInt(1);
		Transform firePointById = UnitViewFacade.GetFirePointById(_createStraightArrAccess.GetInt(2), 1);
		if (firePointById == null)
		{
			return -1;
		}
		Vector3 position = firePointById.position;
		float y = firePointById.eulerAngles.y;
		_createStraightArrAccess.SetDouble(1, position.x);
		_createStraightArrAccess.SetDouble(2, position.y);
		_createStraightArrAccess.SetDouble(3, position.z);
		string viewName = _straightGatlingViewParam.viewName;
		BulletStraightGameObject bulletStraightGameObject;
		int num;
		if (_bulletViewStraightPool.TryGetValue(viewName, out var value) && value.Count > 0)
		{
			bulletStraightGameObject = value.Pop();
			bulletStraightGameObject.OutPool();
			bulletStraightGameObject.InitViewParam(viewObjId, _straightGatlingViewParam, position, y);
			num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
			bulletStraightGameObject.Load();
			flag = bulletStraightGameObject.IsLoaded;
			_createStraightArrAccess.SetInt(4, flag ? 1 : 0);
			_bulletViewStraightHandleList.Add(num);
			return num;
		}
		bulletStraightGameObject = new BulletStraightGameObject(viewName);
		bulletStraightGameObject.InitViewParam(viewObjId, _straightGatlingViewParam, position, y);
		num = (bulletStraightGameObject.Handle = _bulletViewList.Add(bulletStraightGameObject));
		bulletStraightGameObject.Load();
		flag = bulletStraightGameObject.IsLoaded;
		_createStraightArrAccess.SetInt(4, flag ? 1 : 0);
		_bulletViewStraightHandleList.Add(num);
		return num;
	}

	public static void DestroyBulletViewStraight(ref int handle)
	{
		if (handle == -1)
		{
			return;
		}
		RemoveStraightHandle(handle);
		BulletGameObject bulletGameObject = _bulletViewList.Remove(handle);
		if (bulletGameObject != null && CheckHandle(handle, bulletGameObject) && bulletGameObject is BulletStraightGameObject bulletStraightGameObject)
		{
			bulletStraightGameObject.InPool();
			bulletStraightGameObject.Handle = -1;
			string path = bulletStraightGameObject.Path;
			if (!_bulletViewStraightPool.TryGetValue(path, out var value))
			{
				value = new Stack<BulletStraightGameObject>(2048);
				_bulletViewStraightPool.Add(path, value);
			}
			value.Push(bulletStraightGameObject);
			handle = -1;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void RemoveStraightHandle(int handle)
	{
		if (_bulletViewStraightHandleList != null)
		{
			int num = _bulletViewStraightHandleList.IndexOf(handle);
			if (num >= 0)
			{
				int index = _bulletViewStraightHandleList.Count - 1;
				_bulletViewStraightHandleList[num] = _bulletViewStraightHandleList[index];
				_bulletViewStraightHandleList.RemoveAt(index);
			}
		}
	}

	public static AnimationCurve GetAnimationCurve(string curve)
	{
		if (_animationCurves.TryGetValue(curve, out var value))
		{
			return value;
		}
		value = BulletMotionEditor.StringToCurve(curve);
		_animationCurves.Add(curve, value);
		return value;
	}

	public static bool TryGetStraightViewParam(int metaId, out BulletStraightViewParam bulletStraightViewParam)
	{
		if (_straightViewParamMap.TryGetValue(metaId, out bulletStraightViewParam))
		{
			return true;
		}
		bulletStraightViewParam = new BulletStraightViewParam();
		_straightViewParamMap.Add(metaId, bulletStraightViewParam);
		return false;
	}

	public static LuaTable UpdateStraight(float deltaTime)
	{
		int count = _bulletViewStraightHandleList.Count;
		if (count == 0)
		{
			UpdatePreload();
			return null;
		}
		if (_toRemoveStraightTable == null)
		{
			_toRemoveStraightTable = GameEntry.Lua.Env.NewTable();
		}
		int num = 0;
		int num2 = 2;
		for (int i = 0; i < count; i++)
		{
			if (GetBulletView(_bulletViewStraightHandleList[i]) is BulletStraightGameObject bulletStraightGameObject && bulletStraightGameObject.UpdateTransform(deltaTime, out var lifeEnd))
			{
				_toRemoveStraightTable.SetLong(num2++, bulletStraightGameObject.objId);
				_toRemoveStraightTable.SetInt(num2++, lifeEnd);
				num++;
			}
		}
		if (num > 0)
		{
			_toRemoveStraightTable.SetInt(1, num);
			UpdatePreload();
			return _toRemoveStraightTable;
		}
		UpdatePreload();
		return null;
	}

	public static bool UpdateStraightLuaArray(float deltaTime)
	{
		int count = _bulletViewStraightHandleList.Count;
		if (count == 0)
		{
			UpdatePreload();
			_bulletViewArrAccess.SetInt(1, 0);
			return false;
		}
		int num = 0;
		int num2 = 2;
		int cap = (int)_bulletViewArrAccess.GetArrayCapacity();
		for (int i = 0; i < count; i++)
		{
			if (GetBulletView(_bulletViewStraightHandleList[i]) is BulletStraightGameObject bulletStraightGameObject && bulletStraightGameObject.UpdateTransform(deltaTime, out var lifeEnd))
			{
				if (num2 > cap && !ResizeBulletViewLuaArray(num2, ref cap))
				{
					return false;
				}
				_bulletViewArrAccess.SetInt(num2, (int)bulletStraightGameObject.objId);
				num2++;
				if (num2 > cap && !ResizeBulletViewLuaArray(num2, ref cap))
				{
					return false;
				}
				_bulletViewArrAccess.SetInt(num2, lifeEnd);
				num2++;
				num++;
			}
		}
		if (num > 0)
		{
			_bulletViewArrAccess.SetInt(1, num);
			UpdatePreload();
			return true;
		}
		UpdatePreload();
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool ResizeBulletViewLuaArray(int index, ref int cap)
	{
		if (!GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.ResizePVEBulletView", index))
		{
			Log.Error($"BulletViewFacade.ResizeBulletViewLuaArray(): resize array error : {index}");
			return false;
		}
		cap = (int)_bulletViewArrAccess.GetArrayCapacity();
		if (index > cap)
		{
			Log.Error($"BattleColliderUtils.BulletColliderLuaArray(): resize array error : {index}");
			return false;
		}
		return true;
	}

	private static void UpdatePreload()
	{
		foreach (KeyValuePair<string, int> item in _preloadCache)
		{
			string key = item.Key;
			int value = item.Value;
			if (_preloadNormalFlags.Contains(key))
			{
				BulletGameObject bulletGameObject = new BulletGameObject(key);
				bulletGameObject.InPool();
				bulletGameObject.Handle = -1;
				bulletGameObject.Load();
				if (!_bulletViewPool.TryGetValue(key, out var value2))
				{
					value2 = new Stack<BulletGameObject>(2048);
					_bulletViewPool.Add(key, value2);
				}
				value2.Push(bulletGameObject);
			}
			else
			{
				BulletStraightGameObject bulletStraightGameObject = new BulletStraightGameObject(key);
				bulletStraightGameObject.InPool();
				bulletStraightGameObject.Handle = -1;
				bulletStraightGameObject.Load();
				if (!_bulletViewStraightPool.TryGetValue(key, out var value3))
				{
					value3 = new Stack<BulletStraightGameObject>(2048);
					_bulletViewStraightPool.Add(key, value3);
				}
				value3.Push(bulletStraightGameObject);
			}
			if (value - 1 == 0)
			{
				_toRemovePreload.Add(key);
			}
		}
		if (_toRemovePreload.Count > 0)
		{
			foreach (string item2 in _toRemovePreload)
			{
				_preloadCache.Remove(item2);
				_preloadSet.Remove(item2);
			}
			_toRemovePreload.Clear();
		}
		foreach (string item3 in _preloadSet)
		{
			if (_preloadCache.TryGetValue(item3, out var value4))
			{
				_preloadCache[item3] = value4 - 1;
			}
		}
	}

	public static void PreloadBullet(string bulletView, int count)
	{
		_preloadCache[bulletView] = count;
		_preloadSet.Add(bulletView);
		_preloadNormalFlags.Add(bulletView);
	}

	public static void PreloadStraight(string bulletView, int count)
	{
		_preloadCache[bulletView] = count;
		_preloadSet.Add(bulletView);
	}

	public static void StraightLogicDie(int handle, float delayTime)
	{
		BulletGameObject bulletView = GetBulletView(handle);
		if (bulletView != null && bulletView is BulletStraightGameObject bulletStraightGameObject)
		{
			bulletStraightGameObject.DeadDelay(delayTime);
		}
	}

	internal static void AppendStraightLoaded(int objId, float dotMaxCD)
	{
		_tmpStraightLoadedList.Add(objId);
		_tmpStraightDotCDList.Add(dotMaxCD);
	}

	public static void InitBulletViewArrayAccess(LuaArrAccess luaArrAccess, LuaArrAccess createStraightLuaArrAccess, LuaArrAccess createStraightListArrAccess, LuaArrAccess createGatlingStraightLuaArrAccess)
	{
		_bulletViewArrAccess = luaArrAccess;
		_createStraightArrAccess = createStraightLuaArrAccess;
		_createStraightListArrAccess = createStraightListArrAccess;
		_createGatlingStraightArrAccess = createGatlingStraightLuaArrAccess;
		Init();
	}

	public static void UnInitBulletViewArrayAccess()
	{
		_bulletViewArrAccess = null;
		_createStraightArrAccess = null;
		_createStraightListArrAccess = null;
		_createGatlingStraightArrAccess = null;
	}

	public static void CheckLoaded()
	{
		int count = _tmpStraightLoadedList.Count;
		if (count == 0)
		{
			_bulletViewArrAccess.SetInt(1, 0);
			return;
		}
		int num = (int)(_bulletViewArrAccess.GetArrayCapacity() - 1);
		num /= 2;
		if (num > count)
		{
			_bulletViewArrAccess.SetInt(1, count);
			int num2 = 2;
			for (int i = 0; i < count; i++)
			{
				_bulletViewArrAccess.SetInt(num2, _tmpStraightLoadedList[i]);
				num2++;
				_bulletViewArrAccess.SetDouble(num2, _tmpStraightDotCDList[i]);
				num2++;
			}
			_tmpStraightLoadedList.Clear();
			_tmpStraightDotCDList.Clear();
			return;
		}
		num = Mathf.Min(num, count);
		_bulletViewArrAccess.SetInt(1, num);
		int num3 = 2;
		for (int j = 0; j < num; j++)
		{
			int index = count - j - 1;
			_bulletViewArrAccess.SetInt(num3, _tmpStraightLoadedList[index]);
			num3++;
			_bulletViewArrAccess.SetDouble(num3, _tmpStraightDotCDList[index]);
			num3++;
			_tmpStraightLoadedList.RemoveAt(index);
			_tmpStraightDotCDList.RemoveAt(index);
		}
	}

	internal static void SetHide(Transform transform)
	{
	}

	public static void ClearAll()
	{
		if (_bulletViewPool != null)
		{
			foreach (KeyValuePair<string, Stack<BulletGameObject>> item in _bulletViewPool)
			{
				Stack<BulletGameObject> value = item.Value;
				if (value.Count <= 0)
				{
					continue;
				}
				foreach (BulletGameObject item2 in value)
				{
					item2?.Dispose();
				}
				value.Clear();
			}
			_bulletViewPool.Clear();
		}
		if (_bulletViewStraightPool != null)
		{
			foreach (KeyValuePair<string, Stack<BulletStraightGameObject>> item3 in _bulletViewStraightPool)
			{
				Stack<BulletStraightGameObject> value2 = item3.Value;
				if (value2.Count <= 0)
				{
					continue;
				}
				foreach (BulletStraightGameObject item4 in value2)
				{
					item4?.Dispose();
				}
				value2.Clear();
			}
			_bulletViewStraightPool.Clear();
		}
		if (_bulletViewList != null)
		{
			int count = _bulletViewList.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					_bulletViewList[i]?.Dispose();
				}
				_bulletViewList.Clear();
			}
		}
		if (_bulletViewStraightHandleList != null)
		{
			_bulletViewStraightHandleList.Clear();
		}
		if (_loadedLuaTable != null)
		{
			_loadedLuaTable.Dispose();
			_loadedLuaTable = null;
		}
		if (_toRemoveStraightTable != null)
		{
			_toRemoveStraightTable.Dispose();
			_toRemoveStraightTable = null;
		}
		if (_straightViewParamMap != null)
		{
			_straightViewParamMap.Clear();
		}
		if (_tmpStraightLoadedList != null)
		{
			_tmpStraightLoadedList.Clear();
		}
		if (_tmpStraightDotCDList != null)
		{
			_tmpStraightDotCDList.Clear();
		}
		_preloadCache.Clear();
		_toRemovePreload.Clear();
		_preloadSet.Clear();
		_preloadNormalFlags.Clear();
		_straightGatlingViewParam = null;
	}

	public static void Dispose()
	{
		ClearAll();
		_animationCurves?.Clear();
		UnInitBulletViewArrayAccess();
	}
}
