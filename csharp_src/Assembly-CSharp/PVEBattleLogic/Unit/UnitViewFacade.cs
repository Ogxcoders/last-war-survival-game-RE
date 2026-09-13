using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework;
using UnityEngine;

namespace PVEBattleLogic.Unit;

public static class UnitViewFacade
{
	public const int INVALID_HANDLE = -1;

	private static Dictionary<string, Stack<UnitGameObject>> _unitGameObjectPool;

	private static BattleIndexedList<UnitGameObject> _unitGameObjectList;

	private static UnitGameObjectUpdater _gameObjectUpdater = null;

	private static LuaArrAccess _unitViewLuaArrAccess;

	private static LuaArrAccess _unitViewListLuaArrAccess;

	private static LuaArrAccess _hpBarLuaArrAccess;

	private static LuaArrAccess _hpBarListLuaArrAccess;

	private static List<int> _tmpLoadedList;

	private static Dictionary<string, int> _preloadCache = new Dictionary<string, int>();

	private static HashSet<string> _preloadSet = new HashSet<string>();

	private static List<string> _toRemovePreload = new List<string>(8);

	private static Transform _hpBarCanvasTransform;

	private static Stack<UnitHpBarGameObject> _unitHpBarGameObjectPool;

	private static BattleIndexedList<UnitHpBarGameObject> _unitHpBarGameObjectList;

	private static int _hpBarPreloadCount;

	private static readonly Color _selfColor = new Color(35f / 51f, 83f / 85f, 0.40392157f, 1f);

	private static readonly Color _enemyColor = new Color(0.9254902f, 0.29803923f, 0.21960784f, 1f);

	private static int _unitViewParamCount = 9;

	private static int[] _unitViewReturnHandleList = null;

	private static bool[] _unitViewReturnLoadedList = null;

	private static Material _redMaterial;

	private static bool _redMaterialInit;

	private static Material _whiteMaterial;

	private static bool _whiteMaterialInit;

	private static bool _useGPUMaterial = false;

	private static MaterialPropertyBlock _hitRedMPB;

	private static MaterialPropertyBlock _dieGrayMPB;

	private static MaterialPropertyBlock _modelScaleMPB;

	private static MaterialPropertyBlock _bornEffectMPB;

	private static MaterialPropertyBlock _shieldEffectMPB;

	private static int _hpBarParamCount = 8;

	private static int[] _hpBarReturnHandleList = null;

	public static void Init()
	{
		if (_unitGameObjectPool == null)
		{
			_unitGameObjectPool = new Dictionary<string, Stack<UnitGameObject>>();
		}
		if (_unitGameObjectList == null)
		{
			_unitGameObjectList = new BattleIndexedList<UnitGameObject>(512);
		}
		if (_tmpLoadedList == null)
		{
			_tmpLoadedList = new List<int>(64);
		}
		if (_unitHpBarGameObjectPool == null)
		{
			_unitHpBarGameObjectPool = new Stack<UnitHpBarGameObject>(512);
		}
		if (_unitHpBarGameObjectList == null)
		{
			_unitHpBarGameObjectList = new BattleIndexedList<UnitHpBarGameObject>(512);
		}
	}

	public static void InitUnitView(LuaArrAccess luaArrAccess, LuaArrAccess unitViewListLuaArrAccess, LuaArrAccess hpBarLuaArrAccess, LuaArrAccess hpBarListLuaArrAccess)
	{
		Init();
		_unitViewLuaArrAccess = luaArrAccess;
		_unitViewListLuaArrAccess = unitViewListLuaArrAccess;
		_hpBarLuaArrAccess = hpBarLuaArrAccess;
		_hpBarListLuaArrAccess = hpBarListLuaArrAccess;
		_hpBarCanvasTransform = GameEntry.Lua.CallWithReturn<Transform>("CSharpCallLuaInterface.GetHpBarCanvasTransform");
	}

	public static void UnInitUnitView()
	{
		_unitViewLuaArrAccess = null;
		_unitViewListLuaArrAccess = null;
		_hpBarLuaArrAccess = null;
		_hpBarListLuaArrAccess = null;
		_hpBarCanvasTransform = null;
	}

	public static int[] CreateUnitViewList(string path, Transform parent, int count, out bool[] loaded)
	{
		if (_unitViewReturnHandleList == null)
		{
			_unitViewReturnHandleList = new int[512];
		}
		if (_unitViewReturnLoadedList == null)
		{
			_unitViewReturnLoadedList = new bool[512];
		}
		int[] unitViewReturnHandleList = _unitViewReturnHandleList;
		loaded = _unitViewReturnLoadedList;
		for (int i = 0; i < count; i++)
		{
			int num = i * _unitViewParamCount;
			int objId = _unitViewListLuaArrAccess.GetInt(num + 1);
			float scale = (float)_unitViewListLuaArrAccess.GetDouble(num + 2);
			float x = (float)_unitViewListLuaArrAccess.GetDouble(num + 3);
			float y = (float)_unitViewListLuaArrAccess.GetDouble(num + 4);
			float z = (float)_unitViewListLuaArrAccess.GetDouble(num + 5);
			float x2 = (float)_unitViewListLuaArrAccess.GetDouble(num + 6);
			float y2 = (float)_unitViewListLuaArrAccess.GetDouble(num + 7);
			float z2 = (float)_unitViewListLuaArrAccess.GetDouble(num + 8);
			int layer = _unitViewListLuaArrAccess.GetInt(num + 9);
			Vector3 position = new Vector3(x, y, z);
			Vector3 rotation = new Vector3(x2, y2, z2);
			loaded[i] = false;
			if (_unitGameObjectPool.TryGetValue(path, out var value) && value.Count > 0)
			{
				UnitGameObject unitGameObject = value.Pop();
				unitGameObject.OutPool();
				int num2 = (unitGameObject.Handle = _unitGameObjectList.Add(unitGameObject));
				unitGameObject.ObjId = objId;
				loaded[i] = unitGameObject.IsLoaded;
				unitGameObject.Show(position, rotation, parent, scale, layer);
				unitViewReturnHandleList[i] = num2;
			}
			else
			{
				UnitGameObject unitGameObject = new UnitGameObject(path);
				int num2 = (unitGameObject.Handle = _unitGameObjectList.Add(unitGameObject));
				unitGameObject.ObjId = objId;
				loaded[i] = unitGameObject.IsLoaded;
				unitGameObject.Show(position, rotation, parent, scale, layer);
				unitViewReturnHandleList[i] = num2;
			}
		}
		return unitViewReturnHandleList;
	}

	public static int CreateUnitView(string path, Transform parent, out bool loaded)
	{
		int objId = _unitViewLuaArrAccess.GetInt(1);
		float scale = (float)_unitViewLuaArrAccess.GetDouble(2);
		float x = (float)_unitViewLuaArrAccess.GetDouble(3);
		float y = (float)_unitViewLuaArrAccess.GetDouble(4);
		float z = (float)_unitViewLuaArrAccess.GetDouble(5);
		float x2 = (float)_unitViewLuaArrAccess.GetDouble(6);
		float y2 = (float)_unitViewLuaArrAccess.GetDouble(7);
		float z2 = (float)_unitViewLuaArrAccess.GetDouble(8);
		int layer = _unitViewLuaArrAccess.GetInt(9);
		Vector3 position = new Vector3(x, y, z);
		Vector3 rotation = new Vector3(x2, y2, z2);
		loaded = false;
		UnitGameObject unitGameObject;
		int result;
		if (_unitGameObjectPool.TryGetValue(path, out var value) && value.Count > 0)
		{
			unitGameObject = value.Pop();
			unitGameObject.OutPool();
			result = (unitGameObject.Handle = _unitGameObjectList.Add(unitGameObject));
			unitGameObject.ObjId = objId;
			loaded = unitGameObject.IsLoaded;
			unitGameObject.Show(position, rotation, parent, scale, layer);
			return result;
		}
		unitGameObject = new UnitGameObject(path);
		result = (unitGameObject.Handle = _unitGameObjectList.Add(unitGameObject));
		unitGameObject.ObjId = objId;
		loaded = unitGameObject.IsLoaded;
		unitGameObject.Show(position, rotation, parent, scale, layer);
		return result;
	}

	public static LWBattleRVOAgent AddAgent(LWBattleRVOManager rvoManager)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(_hpBarLuaArrAccess.GetInt(1));
		if (unitGameObject != null)
		{
			float speed = (float)_hpBarLuaArrAccess.GetDouble(2);
			float radius = (float)_hpBarLuaArrAccess.GetDouble(3);
			Transform transform = unitGameObject.GetTransform();
			int num = rvoManager.AddAgent(transform.position, transform.gameObject, speed, radius);
			if (CommonUtils.IsDebug())
			{
				transform.gameObject.name = "Zombie_" + num;
			}
			return transform.GetComponent<LWBattleRVOAgent>();
		}
		return null;
	}

	private static UnitGameObject GetUnitGameObject(int handle)
	{
		if (handle == -1)
		{
			return null;
		}
		UnitGameObject unitGameObject = _unitGameObjectList[handle];
		if (unitGameObject == null)
		{
			return null;
		}
		if (!CheckHandle(handle, unitGameObject))
		{
			return null;
		}
		return unitGameObject;
	}

	internal static void UpdatePreload()
	{
		foreach (KeyValuePair<string, int> item in _preloadCache)
		{
			string key = item.Key;
			int value = item.Value;
			UnitGameObject unitGameObject = new UnitGameObject(key);
			unitGameObject.Handle = -1;
			unitGameObject.Show(Vector3.zero, Vector3.zero, null);
			unitGameObject.InPool();
			if (!_unitGameObjectPool.TryGetValue(key, out var value2))
			{
				value2 = new Stack<UnitGameObject>(32);
				_unitGameObjectPool.Add(key, value2);
			}
			value2.Push(unitGameObject);
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
			if (_preloadCache.TryGetValue(item3, out var value3))
			{
				_preloadCache[item3] = value3 - 1;
			}
		}
		if (_hpBarPreloadCount > 0)
		{
			_hpBarPreloadCount--;
			UnitHpBarGameObject unitHpBarGameObject = new UnitHpBarGameObject();
			unitHpBarGameObject.Handle = -1;
			unitHpBarGameObject.Show(null);
			unitHpBarGameObject.InPool();
			_unitHpBarGameObjectPool.Push(unitHpBarGameObject);
		}
	}

	public static void PreloadUnitView(string path, int count)
	{
		_preloadCache[path] = count;
		_preloadSet.Add(path);
	}

	public static bool InitFirePoints(int handle, int appearanceId, int count)
	{
		return GetUnitGameObject(handle)?.InitFirePoints(appearanceId, count) ?? false;
	}

	public static void AppendFirePoint(int handle, string firePointPath)
	{
		GetUnitGameObject(handle)?.AppendFirePoint(firePointPath);
	}

	public static Transform GetFirePointById(int handle, int id)
	{
		return GetUnitGameObject(handle)?.GetFirePointById(id);
	}

	public static bool InitCompPoints(int handle, int appearanceId, int count)
	{
		return GetUnitGameObject(handle)?.InitCompPoints(appearanceId, count) ?? false;
	}

	public static bool AddCompPoint(int handle, int index, string cmpAttachedPath)
	{
		return GetUnitGameObject(handle)?.AddCompPoint(index, cmpAttachedPath) ?? false;
	}

	public static Transform GetCmpPointByIndex(int handle, int index)
	{
		return GetUnitGameObject(handle)?.GetCmpPointByIndex(index);
	}

	public static void GetTransformPoint(int handle, float posX, float posY, float posZ, out float x, out float y, out float z)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		if (unitGameObject != null)
		{
			Vector3 point = new Vector3(posX, posY, posZ);
			Vector3 transformPoint = unitGameObject.GetTransformPoint(point);
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

	public static void InitBuffPoints(int handle, int count)
	{
		GetUnitGameObject(handle)?.InitBuffPoints(count);
	}

	public static void AppendBuffPoint(int handle, string buffPointPath)
	{
		GetUnitGameObject(handle)?.AppendBuffPoint(buffPointPath);
	}

	public static void InitUIPoint(int handle, string uiPointPath)
	{
		GetUnitGameObject(handle)?.InitUIPoint(uiPointPath);
	}

	public static Transform GetUIPoint(int handle)
	{
		return GetUnitGameObject(handle)?.GetUIPoint();
	}

	public static Transform GetRotateRoot(int handle)
	{
		return GetUnitGameObject(handle)?.GetRotateRoot();
	}

	public static Transform InitCannon(int handle, string cannonPath)
	{
		return GetUnitGameObject(handle)?.InitCannon(cannonPath);
	}

	public static void ResetCannon(int handle)
	{
		GetUnitGameObject(handle)?.ResetCannon();
	}

	public static GameObject GetGameObject(int handle)
	{
		return GetUnitGameObject(handle)?.GetGameObject();
	}

	public static Transform GetTransform(int handle)
	{
		return GetUnitGameObject(handle)?.GetTransform();
	}

	public static Transform InitTxtNumberText(int handle, int number)
	{
		return GetUnitGameObject(handle)?.InitTxtNumberText(number);
	}

	public static Transform InitHpText(int handle, int hp)
	{
		return GetUnitGameObject(handle)?.InitHpText(hp);
	}

	public static void ShowHpTweenScale(int handle)
	{
		GetUnitGameObject(handle)?.ShowHpTweenScale();
	}

	public static void SetNumberHpText(int handle, int number)
	{
		GetUnitGameObject(handle)?.SetNumberHpText(number);
	}

	public static void SetNumberText(int handle, int number)
	{
		GetUnitGameObject(handle)?.SetNumberText(number);
	}

	public static void NumberHpTextActive(int handle, bool active)
	{
		GetUnitGameObject(handle)?.NumberHpTextActive(active);
	}

	public static void SetLocalPosition(int handle, float x, float y, float z)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		if (unitGameObject != null)
		{
			Vector3 localPosition = new Vector3(x, y, z);
			unitGameObject.SetLocalPosition(localPosition);
		}
	}

	public static void SetPosition(int handle, float x, float y, float z)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		if (unitGameObject != null)
		{
			Vector3 position = new Vector3(x, y, z);
			unitGameObject.SetPosition(position);
		}
	}

	public static void MoveToLocalPos(int handle, float x, float y, float z, float time)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		if (unitGameObject != null)
		{
			Vector3 pos = new Vector3(x, y, z);
			unitGameObject.MoveToLocalPos(pos, time);
		}
	}

	public static void GetPositionXYZ(int handle, out float x, out float y, out float z)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		if (unitGameObject != null)
		{
			Vector3 position = unitGameObject.GetPosition();
			x = position.x;
			y = position.y;
			z = position.z;
		}
		else
		{
			x = 0f;
			y = 0f;
			z = 0f;
		}
	}

	public static float GetLocalScaleX(int handle)
	{
		return GetUnitGameObject(handle)?.GetLocalScaleX() ?? 0f;
	}

	public static void SetLocalScaleX(int handle, float newScale)
	{
		GetUnitGameObject(handle)?.SetLocalScaleX(newScale);
	}

	internal static string CheckAnimName(SimpleAnimation animation, string animName)
	{
		if (animName == null || string.IsNullOrEmpty(animName))
		{
			return null;
		}
		if (animation.GetState(animName) != null)
		{
			return animName;
		}
		if (animName == "death")
		{
			if (animation.GetState("dead") != null)
			{
				return "dead";
			}
		}
		else if (animName == "dead" && animation.GetState("death") != null)
		{
			return "death";
		}
		return null;
	}

	public static void PlaySimpleAnim(int handle, string animName, float speed = -1f)
	{
		GetUnitGameObject(handle)?.PlaySimpleAnim(animName, speed);
	}

	public static SimpleAnimation GetSimpleAnimation(int handle)
	{
		return GetUnitGameObject(handle)?.GetSimpleAnimation();
	}

	public static SimpleAnimation.State GetSimpleAnimState(int handle, string animName)
	{
		return GetUnitGameObject(handle)?.GetSimpleAnimState(animName);
	}

	public static void RewindAndPlaySimpleAnim(int handle, string animName, float speed = -1f)
	{
		GetUnitGameObject(handle)?.RewindAndPlaySimpleAnim(animName, speed);
	}

	public static void CrossFadeSimpleAnim(int handle, string animName, float speed = -1f, float fadeTime = 0.2f)
	{
		GetUnitGameObject(handle)?.CrossFadeSimpleAnim(animName, speed, fadeTime);
	}

	public static void RewindSimpleAnim(int handle, string animName)
	{
		GetUnitGameObject(handle)?.RewindSimpleAnim(animName);
	}

	public static string GetCurAnimName(int handle)
	{
		return GetUnitGameObject(handle)?.GetCurAniName();
	}

	public static float GetAnimLength(int handle, string animName)
	{
		return GetUnitGameObject(handle)?.GetAnimLength(animName) ?? 0f;
	}

	public static void DestroyUnitView(ref int handle)
	{
		if (handle == -1)
		{
			return;
		}
		UnitGameObject unitGameObject = _unitGameObjectList.Remove(handle);
		if (unitGameObject != null && CheckHandle(handle, unitGameObject))
		{
			unitGameObject.InPool();
			unitGameObject.Handle = -1;
			string path = unitGameObject.Path;
			if (!_unitGameObjectPool.TryGetValue(path, out var value))
			{
				value = new Stack<UnitGameObject>(32);
				_unitGameObjectPool.Add(path, value);
			}
			value.Push(unitGameObject);
			handle = -1;
		}
	}

	internal static void AppendLoaded(int objId, int objHandle)
	{
		_tmpLoadedList.Add(objId);
		_tmpLoadedList.Add(objHandle);
	}

	public static void UseGPUSkinMaterial(bool use)
	{
		_useGPUMaterial = use;
		_redMaterialInit = false;
		_whiteMaterialInit = false;
	}

	public static Material GetRedMaterial()
	{
		if (!_redMaterialInit)
		{
			_redMaterialInit = true;
			if (!_useGPUMaterial)
			{
				_redMaterial = GameEntry.Resource.LoadAsset("Assets/Main/Material/UnitRed.mat", typeof(Material)).asset as Material;
			}
			else
			{
				_redMaterial = GameEntry.Resource.LoadAsset("Assets/Main/Material/Pve/UnitRed_gpu.mat", typeof(Material)).asset as Material;
			}
		}
		return _redMaterial;
	}

	public static Material GetWhiteMaterial()
	{
		if (!_whiteMaterialInit)
		{
			_whiteMaterialInit = true;
			if (!_useGPUMaterial)
			{
				_whiteMaterial = GameEntry.Resource.LoadAsset("Assets/Main/Material/UnitWhite.mat", typeof(Material)).asset as Material;
			}
			else
			{
				_whiteMaterial = GameEntry.Resource.LoadAsset("Assets/Main/Material/Pve/UnitWhite_gpu.mat", typeof(Material)).asset as Material;
			}
		}
		return _whiteMaterial;
	}

	internal static MaterialPropertyBlock GetHitRedMPB()
	{
		if (_hitRedMPB == null)
		{
			_hitRedMPB = new MaterialPropertyBlock();
			Color value = new Color(1f, 0f, 1f / 51f, 0f);
			_hitRedMPB.SetFloat("_USERIMInstancing", 1f);
			_hitRedMPB.SetFloat("_RimPowInstancing", 0.65f);
			_hitRedMPB.SetFloat("_RimRngInstancing", 0f);
			_hitRedMPB.SetColor("_RimColInstancing", value);
		}
		return _hitRedMPB;
	}

	internal static MaterialPropertyBlock GetDieGrayMPB()
	{
		if (_dieGrayMPB == null)
		{
			_dieGrayMPB = new MaterialPropertyBlock();
			_dieGrayMPB.SetFloat("_BWCInstancing", 1f);
		}
		return _dieGrayMPB;
	}

	internal static MaterialPropertyBlock GetModelScaleMPB()
	{
		if (_modelScaleMPB == null)
		{
			_modelScaleMPB = new MaterialPropertyBlock();
			float p = 1.4169f;
			p = Mathf.Pow(2f, p);
			Color value = new Color(0.7490196f * p, 0.24313726f * p, 2f / 51f * p, 0f);
			_modelScaleMPB.SetFloat("_USERIMInstancing", 1f);
			_modelScaleMPB.SetFloat("_RimPowInstancing", 2f);
			_modelScaleMPB.SetFloat("_RimRngInstancing", 3f);
			_modelScaleMPB.SetColor("_RimColInstancing", value);
		}
		return _modelScaleMPB;
	}

	internal static MaterialPropertyBlock GetBornEffectMPB()
	{
		if (_bornEffectMPB == null)
		{
			_bornEffectMPB = new MaterialPropertyBlock();
			float p = 0f;
			p = Mathf.Pow(2f, p);
			Color value = new Color(27f / 85f * p, 0.6745098f * p, 1f * p, 0f);
			_bornEffectMPB.SetFloat("_USERIMInstancing", 1f);
			_bornEffectMPB.SetFloat("_RimPowInstancing", 4f);
			_bornEffectMPB.SetFloat("_RimRngInstancing", 1.5f);
			_bornEffectMPB.SetColor("_RimColInstancing", value);
		}
		return _bornEffectMPB;
	}

	internal static MaterialPropertyBlock GetShieldEffectMPB()
	{
		if (_shieldEffectMPB == null)
		{
			_shieldEffectMPB = new MaterialPropertyBlock();
			float p = 2.0299f;
			p = Mathf.Pow(2f, p);
			Color value = new Color(0f * p, 0.12156863f * p, 0.7490196f * p, 0f);
			_shieldEffectMPB.SetFloat("_USERIMInstancing", 1f);
			_shieldEffectMPB.SetFloat("_RimPowInstancing", 5f);
			_shieldEffectMPB.SetFloat("_RimRngInstancing", 2.97f);
			_shieldEffectMPB.SetColor("_RimColInstancing", value);
		}
		return _shieldEffectMPB;
	}

	public static void ReplaceMaterialFlashRed(int handle)
	{
		GetUnitGameObject(handle)?.ReplaceMaterialFlashRed();
	}

	public static void ReplaceMaterialFlashWhite(int handle)
	{
		GetUnitGameObject(handle)?.ReplaceMaterialFlashWhite();
	}

	public static void ReplaceMaterialReset(int handle)
	{
		GetUnitGameObject(handle)?.ReplaceMaterialReset();
	}

	public static void ReplaceMaterial(Renderer renderer, Material material)
	{
		AppearenceUtils.ReplaceMaterial(renderer, replace: true, material);
	}

	public static void ReplaceMaterialReset(Renderer renderer, Material material)
	{
		AppearenceUtils.ReplaceMaterial(renderer, replace: false, material);
	}

	public static void MPBFlashRed(int handle)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		unitGameObject?.MPBReset();
		unitGameObject?.MPBFlashRed();
	}

	public static void MPBResetGray(int handle)
	{
		GetUnitGameObject(handle)?.MPBResetGray();
	}

	public static void MPBFlashGray(int handle)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		unitGameObject?.MPBReset();
		unitGameObject?.MPBFlashGray();
	}

	public static void MPBModelScale(int handle)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		unitGameObject?.MPBReset();
		unitGameObject?.MPBModelScale();
	}

	public static void MPBBornEffect(int handle)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		unitGameObject?.MPBReset();
		unitGameObject?.MPBBornEffect();
	}

	public static void MPBShieldEffect(int handle)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		unitGameObject?.MPBReset();
		unitGameObject?.MPBShieldEffect();
	}

	public static void MPBReset(int handle)
	{
		GetUnitGameObject(handle)?.MPBReset();
	}

	public static Collider GetCollider(int handle)
	{
		return GetUnitGameObject(handle)?.GetCollider();
	}

	public static void EnableCollider(int handle, bool enable)
	{
		GetUnitGameObject(handle)?.EnableCollider(enable);
	}

	public static void SetVisible(int handle, bool visible)
	{
		GetUnitGameObject(handle)?.SetVisible(visible);
	}

	internal static int GetUnitObjId(int handle)
	{
		return GetUnitGameObject(handle)?.ObjId ?? 0;
	}

	public static void CheckLoaded()
	{
		int count = _tmpLoadedList.Count;
		if (count == 0)
		{
			_unitViewLuaArrAccess.SetInt(1, 0);
			return;
		}
		int num = (int)(_unitViewLuaArrAccess.GetArrayCapacity() - 1);
		int value = count / 2;
		if (num > count)
		{
			_unitViewLuaArrAccess.SetInt(1, value);
			for (int i = 0; i < count; i++)
			{
				_unitViewLuaArrAccess.SetInt(i + 2, _tmpLoadedList[i]);
			}
			_tmpLoadedList.Clear();
			return;
		}
		num = Mathf.Min(num, count);
		int num2 = num / 2;
		_unitViewLuaArrAccess.SetInt(1, num2);
		int num3 = 2;
		for (int j = 0; j < num2; j++)
		{
			int index;
			int num4 = (index = count - (j + 1) * 2);
			_unitViewLuaArrAccess.SetInt(num3, _tmpLoadedList[index]);
			num3++;
			int index2 = num4 + 1;
			_unitViewLuaArrAccess.SetInt(num3, _tmpLoadedList[index2]);
			num3++;
			_tmpLoadedList.RemoveAt(index2);
			_tmpLoadedList.RemoveAt(index);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool CheckHandle(int handle, UnitGameObject unitGameObject)
	{
		if (unitGameObject.Handle != handle)
		{
			Log.Error("Handle is not the same as the unit gameObject , path = " + unitGameObject.Path + " ");
			return false;
		}
		return true;
	}

	internal static Transform GetHpBarCanvasTransform()
	{
		return _hpBarCanvasTransform;
	}

	public static void RegisterUpdater(int handle, float updateZ)
	{
		UnitGameObject unitGameObject = GetUnitGameObject(handle);
		if (unitGameObject != null)
		{
			if (null == _gameObjectUpdater)
			{
				_gameObjectUpdater = new GameObject("UnitGameObjectUpdater").AddComponent<UnitGameObjectUpdater>();
			}
			_gameObjectUpdater.Register(handle, unitGameObject._transform, updateZ);
		}
	}

	public static void UnregisterUpdater(int handle)
	{
		if (null != _gameObjectUpdater)
		{
			_gameObjectUpdater.Unregister(handle);
		}
	}

	public static void RemoveUpdater()
	{
		if (null != _gameObjectUpdater)
		{
			_gameObjectUpdater.Clear();
			Object.Destroy(_gameObjectUpdater.gameObject);
			_gameObjectUpdater = null;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool CheckHpBarHandle(int handle, UnitHpBarGameObject unitHpBarGameObject)
	{
		if (unitHpBarGameObject.Handle != handle)
		{
			Log.Error("Handle is not the same as the hpBar gameObject");
			return false;
		}
		return true;
	}

	private static UnitHpBarGameObject GetHpBarGameObject(int handle)
	{
		if (handle == -1)
		{
			return null;
		}
		UnitHpBarGameObject unitHpBarGameObject = _unitHpBarGameObjectList[handle];
		if (unitHpBarGameObject == null)
		{
			return null;
		}
		if (!CheckHpBarHandle(handle, unitHpBarGameObject))
		{
			return null;
		}
		return unitHpBarGameObject;
	}

	public static int[] CreateHpBarListWithHandle(int count)
	{
		if (_hpBarReturnHandleList == null)
		{
			_hpBarReturnHandleList = new int[512];
		}
		int[] hpBarReturnHandleList = _hpBarReturnHandleList;
		for (int i = 0; i < count; i++)
		{
			int num = _hpBarParamCount * i;
			float height = (float)_hpBarListLuaArrAccess.GetDouble(num + 1);
			float offsetX = (float)_hpBarListLuaArrAccess.GetDouble(num + 2);
			int curHp = _hpBarListLuaArrAccess.GetInt(num + 3);
			int maxHp = _hpBarListLuaArrAccess.GetInt(num + 4);
			int shieldValue = _hpBarListLuaArrAccess.GetInt(num + 5);
			Transform transform = GetTransform(_hpBarListLuaArrAccess.GetInt(num + 6));
			if (transform == null)
			{
				hpBarReturnHandleList[i] = -1;
				continue;
			}
			int num2 = _hpBarListLuaArrAccess.GetInt(num + 7);
			Color color = ((1 != num2) ? _selfColor : _enemyColor);
			if (_unitHpBarGameObjectPool.Count > 0)
			{
				UnitHpBarGameObject unitHpBarGameObject = _unitHpBarGameObjectPool.Pop();
				unitHpBarGameObject.OutPool();
				int num3 = (unitHpBarGameObject.Handle = _unitHpBarGameObjectList.Add(unitHpBarGameObject));
				unitHpBarGameObject.Color = color;
				unitHpBarGameObject.Show(transform, height, offsetX, curHp, maxHp, shieldValue);
				hpBarReturnHandleList[i] = num3;
			}
			else
			{
				UnitHpBarGameObject unitHpBarGameObject = new UnitHpBarGameObject();
				int num3 = (unitHpBarGameObject.Handle = _unitHpBarGameObjectList.Add(unitHpBarGameObject));
				unitHpBarGameObject.Color = color;
				unitHpBarGameObject.Show(transform, height, offsetX, curHp, maxHp);
				hpBarReturnHandleList[i] = num3;
			}
		}
		return hpBarReturnHandleList;
	}

	internal static int CreateHpBar(Transform parent, Color color, float height = 0f, float offsetX = 0f, int curHp = 0, int maxHp = 1, int shieldValue = 0)
	{
		UnitHpBarGameObject unitHpBarGameObject;
		int result;
		if (_unitHpBarGameObjectPool.Count > 0)
		{
			unitHpBarGameObject = _unitHpBarGameObjectPool.Pop();
			unitHpBarGameObject.OutPool();
			result = (unitHpBarGameObject.Handle = _unitHpBarGameObjectList.Add(unitHpBarGameObject));
			unitHpBarGameObject.Color = color;
			unitHpBarGameObject.Show(parent, height, offsetX, curHp, maxHp, shieldValue);
			return result;
		}
		unitHpBarGameObject = new UnitHpBarGameObject();
		result = (unitHpBarGameObject.Handle = _unitHpBarGameObjectList.Add(unitHpBarGameObject));
		unitHpBarGameObject.Color = color;
		unitHpBarGameObject.Show(parent, height, offsetX, curHp, maxHp);
		return result;
	}

	public static int CreateSelfHpBar(Transform parent)
	{
		float height = (float)_hpBarLuaArrAccess.GetDouble(1);
		float offsetX = (float)_hpBarLuaArrAccess.GetDouble(2);
		int curHp = _hpBarLuaArrAccess.GetInt(3);
		int maxHp = _hpBarLuaArrAccess.GetInt(4);
		int shieldValue = _hpBarLuaArrAccess.GetInt(5);
		return CreateHpBar(parent, _selfColor, height, offsetX, curHp, maxHp, shieldValue);
	}

	public static int CreateSelfHpBarWithHandle()
	{
		Transform transform = GetTransform(_hpBarLuaArrAccess.GetInt(6));
		if (transform == null)
		{
			return -1;
		}
		return CreateSelfHpBar(transform);
	}

	public static int CreateEnemyHpBar(Transform parent)
	{
		float height = (float)_hpBarLuaArrAccess.GetDouble(1);
		float offsetX = (float)_hpBarLuaArrAccess.GetDouble(2);
		int curHp = _hpBarLuaArrAccess.GetInt(3);
		int maxHp = _hpBarLuaArrAccess.GetInt(4);
		int shieldValue = _hpBarLuaArrAccess.GetInt(5);
		return CreateHpBar(parent, _enemyColor, height, offsetX, curHp, maxHp, shieldValue);
	}

	public static int CreateEnemyHpBarWithHandle()
	{
		int handle = _hpBarLuaArrAccess.GetInt(6);
		Transform transform = GetTransform(handle);
		if (transform == null)
		{
			return -1;
		}
		if (_hpBarLuaArrAccess.GetInt(7) == 1)
		{
			Transform uIPoint = GetUIPoint(handle);
			if (uIPoint != null)
			{
				transform = uIPoint;
			}
		}
		return CreateEnemyHpBar(transform);
	}

	public static void SetHpBar()
	{
		UnitHpBarGameObject hpBarGameObject = GetHpBarGameObject(_hpBarLuaArrAccess.GetInt(1));
		if (hpBarGameObject != null)
		{
			int curHp = _hpBarLuaArrAccess.GetInt(2);
			int maxHp = _hpBarLuaArrAccess.GetInt(3);
			int curShieldValue = _hpBarLuaArrAccess.GetInt(4);
			hpBarGameObject.SetHp(curHp, maxHp, curShieldValue);
		}
	}

	public static void SetHpBarOffsetX()
	{
		UnitHpBarGameObject hpBarGameObject = GetHpBarGameObject(_hpBarLuaArrAccess.GetInt(1));
		if (hpBarGameObject != null)
		{
			float offsetX = (float)_hpBarLuaArrAccess.GetDouble(2);
			hpBarGameObject.SetOffsetX(offsetX);
		}
	}

	public static void EnableHpBar()
	{
		UnitHpBarGameObject hpBarGameObject = GetHpBarGameObject(_hpBarLuaArrAccess.GetInt(1));
		if (hpBarGameObject != null)
		{
			int num = _hpBarLuaArrAccess.GetInt(2);
			hpBarGameObject.SetVisible(num == 1);
		}
	}

	public static void ReplaceHpBarTarget(Transform parent)
	{
		GetHpBarGameObject(_hpBarLuaArrAccess.GetInt(1))?.ReplaceTarget(parent);
	}

	public static void ReplaceHpBarTargetWithHandle()
	{
		Transform transform = GetTransform(_hpBarLuaArrAccess.GetInt(2));
		if (!(transform == null))
		{
			ReplaceHpBarTarget(transform);
		}
	}

	public static void PreloadHpBar(int count)
	{
		_hpBarPreloadCount = count;
	}

	public static void DestroyHpBar(ref int handle)
	{
		if (handle != -1)
		{
			UnitHpBarGameObject unitHpBarGameObject = _unitHpBarGameObjectList.Remove(handle);
			if (unitHpBarGameObject != null && CheckHpBarHandle(handle, unitHpBarGameObject))
			{
				unitHpBarGameObject.InPool();
				unitHpBarGameObject.Handle = -1;
				_unitHpBarGameObjectPool.Push(unitHpBarGameObject);
				handle = -1;
			}
		}
	}

	public static void UpdateHpBar()
	{
		if (_unitHpBarGameObjectList != null)
		{
			int count = _unitHpBarGameObjectList.Count;
			Camera main = Camera.main;
			Camera uICamera = GameEntry.UICamera;
			for (int i = 0; i < count; i++)
			{
				_unitHpBarGameObjectList[i]?.UpdatePos(main, uICamera);
			}
		}
	}

	public static void InitGlassAndWaterRender(int handle, string glassRenderName, string waterRenderName)
	{
		GetUnitGameObject(handle)?.InitGlassAndWaterRenderObj(glassRenderName, waterRenderName);
	}

	public static void SetGlassCrackEffect(int handle, int crackValue)
	{
		GetUnitGameObject(handle)?.MPBGlassCrackEffect(crackValue);
	}

	public static void SetWaveIntensityEffect(int handle, float heightValue)
	{
		GetUnitGameObject(handle)?.MPBWaveIntensityEffect(heightValue);
	}

	public static void ClearAll()
	{
		if (_unitGameObjectPool != null)
		{
			foreach (KeyValuePair<string, Stack<UnitGameObject>> item in _unitGameObjectPool)
			{
				Stack<UnitGameObject> value = item.Value;
				if (value.Count <= 0)
				{
					continue;
				}
				foreach (UnitGameObject item2 in value)
				{
					item2?.Dispose();
				}
				value.Clear();
			}
			_unitGameObjectPool.Clear();
		}
		if (_unitGameObjectList != null)
		{
			int count = _unitGameObjectList.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					_unitGameObjectList[i]?.Dispose();
				}
				_unitGameObjectList.Clear();
			}
		}
		_unitViewReturnHandleList = null;
		_unitViewReturnLoadedList = null;
		_preloadCache.Clear();
		_toRemovePreload.Clear();
		_preloadSet.Clear();
		if (_unitHpBarGameObjectPool != null && _unitHpBarGameObjectPool.Count > 0)
		{
			foreach (UnitHpBarGameObject item3 in _unitHpBarGameObjectPool)
			{
				item3?.Dispose();
			}
			_unitHpBarGameObjectPool.Clear();
		}
		if (_unitHpBarGameObjectList != null)
		{
			int count2 = _unitHpBarGameObjectList.Count;
			if (count2 > 0)
			{
				for (int j = 0; j < count2; j++)
				{
					_unitHpBarGameObjectList[j]?.Dispose();
				}
				_unitHpBarGameObjectList.Clear();
			}
		}
		_hpBarPreloadCount = 0;
		RemoveUpdater();
	}

	public static void Dispose()
	{
		ClearAll();
		_hitRedMPB = null;
		_dieGrayMPB = null;
		_redMaterial = null;
		_whiteMaterial = null;
		_redMaterialInit = false;
		_whiteMaterialInit = false;
		_modelScaleMPB = null;
		_bornEffectMPB = null;
		UnInitUnitView();
	}
}
