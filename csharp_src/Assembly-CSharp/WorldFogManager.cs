using System;
using System.Collections.Generic;
using GameFramework;
using Main.Scripts.Scene.LightAndDark;
using UnityEngine;
using VEngine;

public class WorldFogManager : WorldManagerBase
{
	public bool mUseMeshInstanced = true;

	public bool mIsDebugMode;

	private WorldFogRendererRenderer mFogRenderer;

	private WorldFogInstanceRenderer mFogInstanceRenderer;

	public bool mIsBloodyNight;

	public bool mIsDawn;

	public int FogMaxRange = 512;

	public int FogUpdateRange = 450;

	public Rect mCurrentFogRect;

	public GameObject mFowObj;

	public FOWSystem mFowSystem;

	public bool mIsFogLoaded;

	public Mesh mFogMesh;

	private Asset mMatRequest;

	private Asset mMarchMatRequest;

	private Asset fogPrefabRequest;

	private InstanceRequest fogPrefabInstanceRequest;

	private bool mIsInited;

	private Dictionary<long, FOWSystem.Revealer> mRevealerDic = new Dictionary<long, FOWSystem.Revealer>(64);

	public Dictionary<long, LightDataManager.LightSourceData> mLightSourcesDic = new Dictionary<long, LightDataManager.LightSourceData>(64);

	public bool mLightInstanceDirty = true;

	public WorldFogManager(WorldScene scene)
		: base(scene)
	{
		world = scene;
	}

	public override void Init()
	{
		base.Init();
	}

	public override void UnInit()
	{
		base.UnInit();
	}

	public override void OnUpdate(float deltaTime)
	{
	}

	private void AddListener()
	{
		GameEntry.Event.Subscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
		GameEntry.Event.Subscribe(EventId.OnEnterCrossServer, OnEnterCrossServer);
		GameEntry.Event.Subscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
	}

	private void RemoveListener()
	{
		GameEntry.Event.Unsubscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
		GameEntry.Event.Unsubscribe(EventId.OnEnterCrossServer, OnEnterCrossServer);
		GameEntry.Event.Unsubscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
	}

	private void OnEnterCrossServer(object obj)
	{
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		bool flag = IsDawn(curServerId);
		if (flag)
		{
			OnChangeToDawn(flag);
			return;
		}
		OnChangeToDawn(flag);
		bool isBloodyNight = IsInBloodyNight(curServerId);
		OnChangeToBloodyNight(isBloodyNight);
	}

	private void OnQuitCrossServer(object obj)
	{
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		bool flag = IsDawn(curServerId);
		if (flag)
		{
			OnChangeToDawn(flag);
			return;
		}
		OnChangeToDawn(flag);
		bool isBloodyNight = IsInBloodyNight(curServerId);
		OnChangeToBloodyNight(isBloodyNight);
	}

	public Dictionary<long, LightDataManager.LightSourceData> GetCurrentLightSourcesDic()
	{
		return mLightSourcesDic;
	}

	public void ClearAllRevealer()
	{
		if (mRevealerDic == null || mRevealerDic.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<long, FOWSystem.Revealer> item in mRevealerDic)
		{
			FOWSystem.DeleteRevealer(item.Value);
		}
		mRevealerDic.Clear();
	}

	public int lightSourceAndFogRangeIntersectType(Vector3 cameraCenter, LightDataManager.LightSourceData lightSourceData)
	{
		_ = FogMaxRange;
		float x = cameraCenter.x - (float)FogMaxRange * 0.5f;
		_ = FogMaxRange;
		float y = cameraCenter.z - (float)FogMaxRange * 0.5f;
		Rect rect = new Rect(x, y, FogMaxRange, FogMaxRange);
		if (rect.Contains(lightSourceData.worldPosVector2))
		{
			return 1;
		}
		if (rect.Overlaps(lightSourceData.radiationRect))
		{
			return 2;
		}
		return 0;
	}

	private void OnMaterialLoadComplete(Asset request)
	{
		if (request.isDone)
		{
			Material material = request.asset as Material;
			mFogInstanceRenderer.SetupBuildingBrushMaterial(material);
		}
	}

	private void OnMarchBrushMaterialLoadComplete(Asset request)
	{
		if (request.isDone)
		{
			Material material = request.asset as Material;
			mFogInstanceRenderer.SetupMarchBrushMaterial(material);
		}
	}

	public void UninitWorldFog()
	{
		mIsInited = false;
		RemoveListener();
		if (fogPrefabRequest != null)
		{
			fogPrefabRequest.Release();
			fogPrefabRequest = null;
		}
		if (mMatRequest != null)
		{
			mMatRequest.Release();
			mMatRequest = null;
		}
		if (mMarchMatRequest != null)
		{
			mMarchMatRequest.Release();
			mMarchMatRequest = null;
		}
		if (fogPrefabInstanceRequest != null)
		{
			fogPrefabInstanceRequest.Destroy();
			fogPrefabInstanceRequest = null;
		}
		if (mFowObj != null)
		{
			mFowObj.Destroy();
		}
		ClearAllRevealer();
		if (mLightSourcesDic != null)
		{
			mLightSourcesDic.Clear();
		}
		if (mMatRequest != null)
		{
			mMatRequest.Release();
		}
		if (mMarchMatRequest != null)
		{
			mMarchMatRequest.Release();
		}
	}

	private string GetFogPrefabPath(bool isBloodyNight)
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.IsDarknessMode())
		{
			if (isBloodyNight)
			{
				return curSkinMeta.world_fog_bloody;
			}
			return curSkinMeta.world_fog;
		}
		return string.Empty;
	}

	public void OnChangeToDawn(bool isDawn)
	{
		mIsDawn = isDawn;
		if (isDawn)
		{
			if (mFowObj != null)
			{
				mFowObj.SetActive(value: false);
			}
		}
		else
		{
			mFowObj.SetActive(value: true);
		}
	}

	public void OnChangeToBloodyNight(bool isBloodyNight)
	{
		if (mIsBloodyNight != isBloodyNight)
		{
			mIsBloodyNight = isBloodyNight;
			LightDataManager.GetInstance().UpdateAllLightDataBrushSize(mIsBloodyNight);
			if (mIsInited)
			{
				OnChangeFogMat();
			}
		}
	}

	public void OnChangeFogMat()
	{
		if (!mUseMeshInstanced)
		{
			return;
		}
		string fogPrefabPath = GetFogPrefabPath(mIsBloodyNight);
		fogPrefabInstanceRequest = GameEntry.Resource.InstantiateAsync(fogPrefabPath);
		if (fogPrefabInstanceRequest == null)
		{
			return;
		}
		fogPrefabInstanceRequest.completed += delegate
		{
			if (fogPrefabInstanceRequest != null)
			{
				if (SceneManager.CurrSceneID != 2)
				{
					fogPrefabInstanceRequest.Destroy();
					fogPrefabInstanceRequest = null;
				}
				else
				{
					SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
					if (curSkinMeta != null && curSkinMeta.seasonType != 5)
					{
						fogPrefabInstanceRequest.Destroy();
						fogPrefabInstanceRequest = null;
					}
					else if (GetFogPrefabPath(mIsBloodyNight) != fogPrefabPath)
					{
						fogPrefabInstanceRequest.Destroy();
						fogPrefabInstanceRequest = null;
					}
					else
					{
						if (mFowObj != null)
						{
							mFowObj.Destroy();
						}
						mFowObj = new GameObject();
						mFowObj.name = "WorldFogSystem";
						GameObject gameObject = fogPrefabInstanceRequest.gameObject;
						if (!(gameObject == null))
						{
							gameObject.transform.SetParent(mFowObj.transform);
							gameObject.transform.localPosition = new Vector3(0f, 22f, 0f);
							gameObject.transform.localRotation = Quaternion.identity;
							Material material = gameObject.GetComponent<Renderer>().material;
							mFogInstanceRenderer.SetupFogMaterial(material);
							WorldScene worldScene = SceneManager.World as WorldScene;
							if (worldScene != null)
							{
								UpdateAllFogOnViewChange(worldScene.Camera);
							}
						}
					}
				}
			}
		};
	}

	public void OnBloodyNightActivityRefresh(object serverId)
	{
		if (serverId is long)
		{
			int num = serverId.ToInt();
			int curServerId = GameEntry.Data.Player.GetCurServerId();
			if (curServerId == num)
			{
				bool flag = IsDawn(curServerId);
				if (flag)
				{
					OnChangeToDawn(flag);
					return;
				}
				bool isBloodyNight = IsInBloodyNight(curServerId);
				OnChangeToBloodyNight(isBloodyNight);
			}
		}
		else
		{
			Log.Error("OnBloodyNightActivityRefresh Invalid serverId");
		}
	}

	public bool IsInBloodyNight(int targetServerId)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsBloodyNight", targetServerId);
	}

	public bool IsDawn(int targetServerId)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsDawn", targetServerId);
	}

	public void InitWorldFog(WorldFogRendererRenderer fogRenderer, WorldFogInstanceRenderer fogInstanceRenderer, WorldCamera camera)
	{
		AddListener();
		mFogRenderer = fogRenderer;
		mFogInstanceRenderer = fogInstanceRenderer;
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		mIsDawn = IsDawn(curServerId);
		bool flag = IsInBloodyNight(curServerId);
		mIsBloodyNight = flag;
		mIsInited = true;
		if (mUseMeshInstanced)
		{
			if (mFowObj == null)
			{
				mFowObj = new GameObject();
				mFowObj.name = "WorldFogSystem";
				string fogPrefabPath = GetFogPrefabPath(mIsBloodyNight);
				fogPrefabRequest = GameEntry.Resource.LoadAsset(fogPrefabPath, typeof(GameObject));
				if (fogPrefabRequest == null)
				{
					return;
				}
				Asset asset = fogPrefabRequest;
				asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
				{
					if (fogPrefabRequest != null)
					{
						if (SceneManager.CurrSceneID != 2)
						{
							fogPrefabRequest.Release();
							fogPrefabRequest = null;
						}
						else
						{
							SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
							if (curSkinMeta != null && curSkinMeta.seasonType != 5)
							{
								fogPrefabRequest?.Release();
								fogPrefabRequest = null;
							}
							else if (!(fogPrefabRequest.asset == null))
							{
								if (mFowObj == null)
								{
									fogPrefabRequest?.Release();
									fogPrefabRequest = null;
								}
								else if (GetFogPrefabPath(mIsBloodyNight) != fogPrefabPath)
								{
									fogPrefabRequest?.Release();
									fogPrefabRequest = null;
								}
								else
								{
									GameObject gameObject = UnityEngine.Object.Instantiate(fogPrefabRequest.asset as GameObject);
									gameObject.transform.SetParent(mFowObj.transform);
									gameObject.transform.localPosition = new Vector3(0f, 22f, 0f);
									gameObject.transform.localRotation = Quaternion.identity;
									Material material = gameObject.GetComponent<Renderer>().material;
									mFogInstanceRenderer.SetupFogMaterial(material);
								}
							}
						}
					}
				});
			}
			mMatRequest = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S4/Material/FogBrushMat.mat", typeof(Material));
			Asset asset2 = mMatRequest;
			asset2.completed = (Action<Asset>)Delegate.Combine(asset2.completed, new Action<Asset>(OnMaterialLoadComplete));
			mMarchMatRequest = GameEntry.Resource.LoadAssetAsync("Assets/Main/SeasonRes/S4/Material/MarchLightMat.mat", typeof(Material));
			Asset asset3 = mMarchMatRequest;
			asset3.completed = (Action<Asset>)Delegate.Combine(asset3.completed, new Action<Asset>(OnMarchBrushMaterialLoadComplete));
			fogInstanceRenderer.Setup();
			UpdateAllFogOnViewChange(camera);
		}
		else if (mFowObj == null)
		{
			mFowObj = new GameObject();
			mFowObj.name = "WorldFogSystem";
			FOWSystem fOWSystem = mFowObj.AddComponent<FOWSystem>();
			fOWSystem.worldSize = 512;
			fOWSystem.textureSize = 256;
			fOWSystem.updateFrequency = 0.33f;
			fOWSystem.textureBlendTime = 0f;
			fOWSystem.blurIterations = 1;
			fOWSystem.isRevealRect = true;
			mIsFogLoaded = false;
			fOWSystem.RegisterCompleteAction(delegate
			{
				mIsFogLoaded = true;
			});
			InstanceRequest request = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/FogOfWar/Season/FogVolume_s4.prefab");
			request.completed += delegate
			{
				if (request != null)
				{
					GameObject gameObject = request.gameObject;
					gameObject.transform.SetParent(mFowObj.transform);
					gameObject.transform.localPosition = new Vector3(0f, 5f, 0f);
					gameObject.transform.localRotation = Quaternion.identity;
				}
			};
			mFowSystem = fOWSystem;
		}
		if (mIsDawn)
		{
			HideWorldFogObj(hide: true);
		}
	}

	public void HideWorldFogObj(bool hide)
	{
		if (mFowObj == null)
		{
			return;
		}
		if (hide)
		{
			if (mFowObj.activeSelf)
			{
				mFowObj.SetActive(value: false);
			}
		}
		else if (!mFowObj.activeSelf && !mIsDawn)
		{
			mFowObj.SetActive(value: true);
		}
	}

	public bool NeedUpdateAllFog(WorldCamera camera)
	{
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		if (vector.x >= mCurrentFogRect.xMin && vector2.x <= mCurrentFogRect.xMax && vector.z >= mCurrentFogRect.yMin && vector2.z <= mCurrentFogRect.yMax)
		{
			return false;
		}
		return true;
	}

	public void UpdateCurrentFogRect(Vector3 center)
	{
		float x = center.x - (float)FogUpdateRange * 0.5f;
		float y = center.z - (float)FogUpdateRange * 0.5f;
		Rect rect = new Rect(x, y, FogUpdateRange, FogUpdateRange);
		mCurrentFogRect = rect;
	}

	public void UpdateAllFogOnViewChange(WorldCamera camera)
	{
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		Vector3 vector3 = new Vector3(vector.x + 0.5f * (vector2.x - vector.x), 0f, vector.z + 0.5f * (vector2.z - vector.z));
		Dictionary<long, LightDataManager.LightSourceData> mLightSourceDataCache = LightDataManager.GetInstance().mLightSourceDataCache;
		if (mUseMeshInstanced)
		{
			if (mFowObj != null)
			{
				mFowObj.gameObject.transform.position = vector3;
			}
			UpdateCurrentFogRect(vector3);
		}
		else
		{
			if (mFowSystem == null)
			{
				return;
			}
			ClearAllRevealer();
			mFowSystem.needUpdateCenterPos = true;
			mFowSystem.newCenterPos = vector3;
			mFowSystem.ResetOrigin(vector3);
			UpdateCurrentFogRect(vector3);
		}
		if (mUseMeshInstanced)
		{
			return;
		}
		foreach (KeyValuePair<long, LightDataManager.LightSourceData> item in mLightSourceDataCache)
		{
			LightDataManager.LightSourceData value = item.Value;
			switch (lightSourceAndFogRangeIntersectType(vector3, value))
			{
			case 1:
			{
				FOWSystem.Revealer revealer2 = new FOWSystem.Revealer();
				revealer2.pos = value.worldPosVector3;
				revealer2.inner = 0f;
				revealer2.outer = value.brushSize;
				revealer2.los = FOWSystem.LOSChecks.OnlyOnce;
				revealer2.isActive = true;
				FOWSystem.AddRevealer(revealer2);
				mRevealerDic.Add(value.pointId, revealer2);
				break;
			}
			case 2:
			{
				FOWSystem.Revealer revealer = new FOWSystem.Revealer();
				revealer.pos = value.worldPosVector2;
				revealer.inner = 0f;
				revealer.outer = value.brushSize;
				revealer.los = FOWSystem.LOSChecks.OnlyOnce;
				revealer.isActive = true;
				FOWSystem.AddRevealer(revealer);
				mRevealerDic.Add(value.pointId, revealer);
				break;
			}
			}
		}
	}

	public void OnFogDataChangedAll(Dictionary<long, LightDataManager.LightSourceData> lightSourceDataCache)
	{
		if (mUseMeshInstanced)
		{
			if (!mIsDebugMode)
			{
				mLightSourcesDic.Clear();
			}
			foreach (KeyValuePair<long, LightDataManager.LightSourceData> item in lightSourceDataCache)
			{
				LightDataManager.LightSourceData value = item.Value;
				mLightSourcesDic.Add(item.Key, value);
			}
			mLightInstanceDirty = true;
			return;
		}
		foreach (KeyValuePair<long, LightDataManager.LightSourceData> item2 in lightSourceDataCache)
		{
			LightDataManager.LightSourceData value2 = item2.Value;
			FOWSystem.Revealer value3 = null;
			if (mRevealerDic.TryGetValue(value2.pointId, out value3))
			{
				if (value3.outer != value2.brushSize)
				{
					value3.outer = value2.brushSize;
				}
				continue;
			}
			value3 = new FOWSystem.Revealer();
			value3.pos = value2.worldPosVector3;
			value3.inner = 0f;
			value3.outer = value2.brushSize;
			value3.los = FOWSystem.LOSChecks.OnlyOnce;
			value3.isActive = true;
			FOWSystem.AddRevealer(value3);
			mRevealerDic.Add(value2.pointId, value3);
		}
	}

	public void TestClearFogInPos(Vector3 pos)
	{
		Dictionary<long, LightDataManager.LightSourceData> mLightSourceDataCache = LightDataManager.GetInstance().mLightSourceDataCache;
		int num = SceneManager.World.WorldToTileIndex(pos);
		int num2 = 1;
		int num3 = num2 * 2 + 1;
		float brushSize = num3 * 3;
		Vector2 vector = new Vector2(pos.x, pos.z);
		LightDataManager.LightSourceData value = default(LightDataManager.LightSourceData);
		value.pointId = num;
		value.radius = num2;
		value.worldPosVector2 = new Vector2(pos.x, pos.z);
		value.worldPosVector3 = pos;
		value.level = 2;
		value.brushSize = brushSize;
		value.radiationRect = new Rect(vector, new Vector2(num3 * 2, num3 * 2));
		value.radiationRect.center = vector;
		if (mUseMeshInstanced)
		{
			mLightSourcesDic.Add(value.pointId, value);
			mLightSourceDataCache.Add(num, value);
		}
		else
		{
			FOWSystem.Revealer value2 = null;
			if (mRevealerDic.TryGetValue(value.pointId, out value2))
			{
				return;
			}
			value2 = new FOWSystem.Revealer();
			value2.pos = pos;
			value2.inner = 0f;
			value2.outer = 3f;
			value2.los = FOWSystem.LOSChecks.OnlyOnce;
			value2.isActive = true;
			FOWSystem.AddRevealer(value2);
			mLightSourceDataCache.Add(value.pointId, value);
			mRevealerDic.Add(num, value2);
		}
		Vector2Int vector2Int = SceneManager.World.IndexToTilePos(num);
		int x = vector2Int.x - num3;
		int x2 = vector2Int.x + num3;
		int y = vector2Int.y - num3;
		int y2 = vector2Int.y + num3;
		LightDataManager.GetInstance()._onLighdataChanged(x, y, x2, y2);
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene != null && worldScene.StaticManager != null)
		{
			worldScene.StaticManager.SetViewDirty();
		}
	}
}
