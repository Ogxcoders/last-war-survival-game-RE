using System;
using Main.Scripts.Scene.LightAndDark;
using UnityEngine;
using VEngine;

public class WorldWeatherManager : WorldManagerBase
{
	private int FogUpdateRange = 450;

	private int FogMaxRange = 512;

	private Rect mCurrentFogRect;

	private WorldFogInstanceRenderer mFogInstanceRenderer;

	private Renderer fogPrefabRender;

	private Asset mMatRequest;

	private GameObject mFowObj;

	private InstanceRequest fogPrefabInstanceRequest;

	private GameObject mFollowObj;

	private InstanceRequest followPrefabInstanceRequest;

	private SeasonWeatherType mCurrentWeatherType = SeasonWeatherType.Unknown;

	private string mCurrentWeatherMatPath = string.Empty;

	private GameObject FowObj
	{
		get
		{
			if (mFowObj == null)
			{
				mFowObj = new GameObject
				{
					name = "WorldWeatherSystem"
				};
			}
			return mFowObj;
		}
	}

	public WorldWeatherManager(WorldScene scene)
		: base(scene)
	{
		world = scene;
	}

	public override void Init()
	{
		AddListener();
		base.Init();
	}

	public override void UnInit()
	{
		base.UnInit();
		RemoveListener();
		ClearAll();
	}

	private void AddListener()
	{
		GameEntry.Event.Subscribe(EventId.LWSeasonWeatherInfoUpdate, SeasonWeatherInfoUpdate);
	}

	private void RemoveListener()
	{
		GameEntry.Event.Unsubscribe(EventId.LWSeasonWeatherInfoUpdate, SeasonWeatherInfoUpdate);
	}

	private void ClearAll()
	{
		RemoveFogPrefab();
		RemoveFollowPrefab();
		if (mFowObj != null)
		{
			UnityEngine.Object.Destroy(mFowObj);
			mFowObj = null;
		}
		if (mCurrentWeatherType == SeasonWeatherType.Rain || mCurrentWeatherType == SeasonWeatherType.AcidRain)
		{
			Shader.SetGlobalInt("_RainStart", 0);
		}
		mCurrentWeatherType = SeasonWeatherType.Unknown;
	}

	private void RemoveFogPrefab()
	{
		if (fogPrefabInstanceRequest != null)
		{
			fogPrefabInstanceRequest.Destroy();
			fogPrefabInstanceRequest = null;
		}
		if (mMatRequest != null)
		{
			mMatRequest.Release();
			mMatRequest = null;
		}
	}

	private void RemoveFollowPrefab()
	{
		if (followPrefabInstanceRequest != null)
		{
			followPrefabInstanceRequest.Destroy();
			followPrefabInstanceRequest = null;
		}
		if (mFollowObj != null)
		{
			UnityEngine.Object.Destroy(mFollowObj);
			mFollowObj = null;
		}
	}

	public bool IsActive()
	{
		if ((bool)mFowObj)
		{
			return mCurrentWeatherType != SeasonWeatherType.Unknown;
		}
		return false;
	}

	private void SeasonWeatherInfoUpdate(object obj)
	{
		SeasonWeatherType seasonWeatherType = (SeasonWeatherType)GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetWeatherType");
		if (object.Equals(mCurrentWeatherType, seasonWeatherType))
		{
			return;
		}
		mCurrentWeatherType = seasonWeatherType;
		if (mCurrentWeatherType == SeasonWeatherType.Unknown)
		{
			ClearAll();
			return;
		}
		if (mCurrentWeatherType == SeasonWeatherType.Rain || mCurrentWeatherType == SeasonWeatherType.AcidRain)
		{
			Shader.SetGlobalInt("_RainStart", 1);
		}
		else
		{
			Shader.SetGlobalInt("_RainStart", 0);
		}
		string text = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetWeatherWorldFollowPath", (int)mCurrentWeatherType);
		if (string.IsNullOrEmpty(text))
		{
			RemoveFollowPrefab();
		}
		else
		{
			RefreshFollowObj(text);
		}
		mCurrentWeatherMatPath = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetWeatherMatPath", (int)mCurrentWeatherType);
		if (string.IsNullOrEmpty(mCurrentWeatherMatPath))
		{
			RemoveFogPrefab();
		}
		else if (fogPrefabInstanceRequest == null)
		{
			InitWorldFog();
		}
		else
		{
			OnChangeFogMat();
		}
		UpdateAllFogOnViewChange(world.Camera);
	}

	public void OnSkinChange(SeasonType seasonType)
	{
		if (seasonType != SeasonType.CityStronghold || !SystemInfo.supportsInstancing || GameEntry.Data.Player.IsInBattleField())
		{
			ClearAll();
		}
		else
		{
			SeasonWeatherInfoUpdate(null);
		}
	}

	private void OnChangeFogMat()
	{
		mMatRequest?.Release();
		if (!string.IsNullOrEmpty(mCurrentWeatherMatPath))
		{
			mMatRequest = GameEntry.Resource.LoadAssetAsync(mCurrentWeatherMatPath, typeof(Material));
			Asset asset = mMatRequest;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, new Action<Asset>(OnMaterialLoadComplete));
		}
	}

	private void InitWorldFog()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null || curSkinMeta.world_fog.IsNullOrEmpty())
		{
			Debug.LogError("WorldSkinMeta is null or world_fog is empty, please check the skin meta data.");
			return;
		}
		if (mFogInstanceRenderer == null)
		{
			mFogInstanceRenderer = new WorldFogInstanceRenderer();
		}
		if (!(FowObj == null))
		{
			fogPrefabInstanceRequest?.Destroy();
			fogPrefabInstanceRequest = GameEntry.Resource.InstantiateAsync(curSkinMeta.world_fog);
			fogPrefabInstanceRequest.completed += delegate
			{
				GameObject gameObject = fogPrefabInstanceRequest.gameObject;
				gameObject.transform.SetParent(FowObj.transform);
				gameObject.transform.localPosition = new Vector3(0f, 22f, 0f);
				gameObject.transform.localRotation = Quaternion.identity;
				fogPrefabRender = gameObject.GetComponent<Renderer>();
				mFogInstanceRenderer.SetupFogMaterial(fogPrefabRender.material);
				OnChangeFogMat();
			};
			mFogInstanceRenderer.Setup();
		}
	}

	private void RefreshFollowObj(string followPath)
	{
		if (!(FowObj == null))
		{
			if (mFollowObj == null)
			{
				mFollowObj = new GameObject
				{
					name = "WorldFollowObj"
				};
				mFollowObj.transform.SetParent(FowObj.transform);
				mFollowObj.transform.localPosition = Vector3.zero;
			}
			followPrefabInstanceRequest?.Destroy();
			followPrefabInstanceRequest = GameEntry.Resource.InstantiateAsync(followPath);
			followPrefabInstanceRequest.completed += delegate
			{
				GameObject gameObject = followPrefabInstanceRequest.gameObject;
				gameObject.transform.SetParent(mFollowObj.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
			};
		}
	}

	public void UpdateWeather(int currViewLevel, int allianceCityLod, int objLod)
	{
		if (!IsActive() || world.Camera == null)
		{
			return;
		}
		if (currViewLevel >= allianceCityLod)
		{
			HideWorldFogObj(hide: true);
			return;
		}
		if (NeedUpdateAllFog(world.Camera))
		{
			UpdateAllFogOnViewChange(world.Camera);
		}
		HideWorldFollowObj(currViewLevel >= objLod);
		HideWorldFogObj(hide: false);
	}

	private void HideWorldFogObj(bool hide)
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
		else if (!mFowObj.activeSelf)
		{
			mFowObj.SetActive(value: true);
		}
	}

	private void HideWorldFollowObj(bool hide)
	{
		if (mFollowObj == null)
		{
			return;
		}
		if (hide)
		{
			if (mFollowObj.activeSelf)
			{
				mFollowObj.SetActive(value: false);
			}
		}
		else if (!mFollowObj.activeSelf)
		{
			mFollowObj.SetActive(value: true);
		}
	}

	private bool NeedUpdateAllFog(WorldCamera camera)
	{
		Vector3 vector = camera.cameraAnchor[0];
		Vector3 vector2 = camera.cameraAnchor[2];
		if (vector.x >= mCurrentFogRect.xMin && vector2.x <= mCurrentFogRect.xMax && vector.z >= mCurrentFogRect.yMin && vector2.z <= mCurrentFogRect.yMax)
		{
			return false;
		}
		return true;
	}

	private void UpdateAllFogOnViewChange(WorldCamera camera)
	{
		if (!(mFowObj == null))
		{
			Vector3 vector = camera.cameraAnchor[0];
			Vector3 vector2 = camera.cameraAnchor[2];
			Vector3 vector3 = new Vector3(vector.x + 0.5f * (vector2.x - vector.x), 0f, vector.z + 0.5f * (vector2.z - vector.z));
			mFowObj.gameObject.transform.position = vector3;
			UpdateCurrentFogRect(vector3);
		}
	}

	private void UpdateCurrentFogRect(Vector3 center)
	{
		float x = center.x - (float)FogUpdateRange * 0.5f;
		float y = center.z - (float)FogUpdateRange * 0.5f;
		Rect rect = new Rect(x, y, FogUpdateRange, FogUpdateRange);
		mCurrentFogRect = rect;
	}

	private void OnMaterialLoadComplete(Asset request)
	{
		if (request.isDone && fogPrefabRender != null && mFogInstanceRenderer != null && fogPrefabInstanceRequest != null)
		{
			Material material = request.asset as Material;
			fogPrefabRender.material = material;
			mFogInstanceRenderer.SetupFogMaterial(material);
			mFowObj?.SetActive(value: true);
		}
	}
}
