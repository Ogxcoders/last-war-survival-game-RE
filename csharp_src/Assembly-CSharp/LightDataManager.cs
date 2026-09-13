using System;
using System.Collections.Generic;
using GameFramework;
using Google.Protobuf.Collections;
using Protobuf;
using UnityEngine;

public class LightDataManager
{
	public delegate void OnLighdataChangeFun(int x1, int y1, int x2, int y2);

	public struct LightSourceData
	{
		public long pointId;

		public int radius;

		public int level;

		public Vector2 worldPosVector2;

		public Vector3 worldPosVector3;

		public Quaternion rotation;

		public int size;

		public float brushSize;

		public float marchLightSize;

		public Rect radiationRect;
	}

	private static LightDataManager instance;

	public static readonly int mEvColorOnId = Shader.PropertyToID("_EvColorOn");

	public static readonly int mEvColorId = Shader.PropertyToID("_EvColor");

	private Dictionary<int, Color> mNightBloodyColorDic;

	private Dictionary<int, Color> mNightColorDic;

	private Dictionary<int, Color> mCityFogColorDic;

	private Dictionary<int, Color> mCityFog_BloodyColorDic;

	public float mMarchSize;

	public float mMarchBrushSize;

	public float mMarchBrushSize_Bloody;

	public float mMarchSizeStation;

	public float mFogBrushSize;

	public float mFogBrushSize_Bloody;

	private const float MarchLightLenght = 0.8f;

	public Dictionary<int, float> mMonsterMarchBrushSizeDic;

	private Dictionary<int, int> mMonsterIdToSpecialId;

	public Dictionary<long, LightSourceData> mLightSourceDataCache = new Dictionary<long, LightSourceData>(64);

	public Dictionary<long, LightSourceData> mLightMarchDict = new Dictionary<long, LightSourceData>();

	public Dictionary<long, LightSourceData> mDiscoLightDataCache = new Dictionary<long, LightSourceData>(32);

	public bool mLightInstanceDirty = true;

	public bool mMarchLightInstanceDirty = true;

	public bool mDiscoLightDataDirty = true;

	public bool mShowMarchLight = true;

	public OnLighdataChangeFun _onLighdataChanged;

	public static LightDataManager GetInstance()
	{
		if (instance == null)
		{
			instance = new LightDataManager();
			instance.Init();
		}
		return instance;
	}

	private void Init()
	{
		string s = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_fade_setting", "k1");
		string s2 = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_fade_setting", "k2");
		string s3 = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_fade_setting", "k3");
		string s4 = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_fade_setting", "k4");
		string s5 = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_fade_setting", "k5");
		string s6 = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_fade_setting", "k6");
		if (!float.TryParse(s, out mMarchSize))
		{
			mMarchSize = 2f;
		}
		if (!float.TryParse(s2, out mMarchBrushSize))
		{
			mMarchBrushSize = 2f;
		}
		if (!float.TryParse(s3, out mMarchBrushSize_Bloody))
		{
			mMarchBrushSize_Bloody = 2f;
		}
		if (!float.TryParse(s4, out mFogBrushSize))
		{
			mFogBrushSize = 2f;
		}
		if (!float.TryParse(s5, out mFogBrushSize_Bloody))
		{
			mFogBrushSize_Bloody = 2f;
		}
		if (!float.TryParse(s6, out mMarchSizeStation))
		{
			mMarchSizeStation = 1.5f;
		}
		mMonsterIdToSpecialId = new Dictionary<int, int>();
		GameEntry.Event.Subscribe(EventId.WorldMarchUpdateDisplayMode, OnWorldMarchUpdateDisplayModeChanged);
		GameEntry.Event.Subscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
	}

	public void Destroy()
	{
		GameEntry.Event.Unsubscribe(EventId.WorldMarchUpdateDisplayMode, OnWorldMarchUpdateDisplayModeChanged);
		GameEntry.Event.Unsubscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
	}

	private void RemoveAllStaticData()
	{
	}

	private void RemoveAllDynamicData()
	{
	}

	private void AddListener()
	{
	}

	private void RemoveListener()
	{
	}

	private void OnEnterCity(object obj)
	{
	}

	private void RefreshCurServerData()
	{
	}

	private void OnEnterCrossServer(object obj)
	{
	}

	private void OnQuitCrossServer(object obj)
	{
	}

	public void OnEnterGame()
	{
	}

	public bool IsDawn(int targetServerId)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsDawn", targetServerId);
	}

	public void OnBloodyNightActivityRefresh(object serverId)
	{
		if (serverId is long)
		{
			int num = serverId.ToInt();
			int curServerId = GameEntry.Data.Player.GetCurServerId();
			if (curServerId == num && IsDawn(curServerId))
			{
				ClearAllLightData();
			}
		}
		else
		{
			Log.Error("OnBloodyNightActivityRefresh Invalid serverId");
		}
	}

	private void InitMonsterMarchBrushSize()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta == null || !curSkinMeta.IsDarknessMode())
		{
			return;
		}
		mMonsterMarchBrushSizeDic = new Dictionary<int, float>();
		if (string.IsNullOrEmpty(curSkinMeta.light_monster))
		{
			return;
		}
		string[] array = curSkinMeta.light_monster.Split(new char[1] { '|' });
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[1] { ';' });
			if (array2.Length == 2 && int.TryParse(array2[0], out var result) && float.TryParse(array2[1], out var result2))
			{
				mMonsterMarchBrushSizeDic.Add(result, result2);
			}
		}
	}

	private void OnWorldMarchUpdateDisplayModeChanged(object obj)
	{
		if (GameEntry.Lua != null)
		{
			int num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCurrentDisplayLevel");
			bool flag = true;
			flag = num >= -2;
			if (flag != mShowMarchLight)
			{
				mShowMarchLight = flag;
				mMarchLightInstanceDirty = true;
			}
		}
	}

	public QuadCellS3 GetS4LayerStateValue(int x, int y)
	{
		Vector3 vector = SceneManager.World.TileToWorld(new Vector2Int(x, y));
		Vector2 point = new Vector2(vector.x, vector.z);
		int num = -1;
		foreach (LightSourceData value in mLightSourceDataCache.Values)
		{
			Rect radiationRect = value.radiationRect;
			if (radiationRect.Contains(point) && num < value.level)
			{
				num = value.level;
			}
		}
		int num2 = num;
		int num3 = num;
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		return new QuadCellS3(num2, num3, timeSinceLevelLoad);
	}

	public void ClearAllLightData()
	{
		if (mLightSourceDataCache != null)
		{
			mLightSourceDataCache.Clear();
		}
		if (mLightMarchDict != null)
		{
			mLightMarchDict.Clear();
		}
		if (mDiscoLightDataCache != null)
		{
			mDiscoLightDataCache.Clear();
		}
		mLightInstanceDirty = true;
		mMarchLightInstanceDirty = true;
		mDiscoLightDataDirty = true;
		mShowMarchLight = true;
	}

	public void UpdateAllLightDataBrushSize(bool isBloodyNight)
	{
		foreach (LightSourceData value in mLightSourceDataCache.Values)
		{
			LightSourceData current = value;
			if (isBloodyNight)
			{
				current.brushSize = (float)current.size * mFogBrushSize_Bloody;
			}
			else
			{
				current.brushSize = (float)current.size * mFogBrushSize;
			}
		}
		foreach (LightSourceData value2 in mLightMarchDict.Values)
		{
			LightSourceData current2 = value2;
			if (isBloodyNight)
			{
				current2.brushSize = current2.marchLightSize * mMarchBrushSize_Bloody;
			}
			else
			{
				current2.brushSize = current2.marchLightSize * mMarchBrushSize;
			}
		}
	}

	public void TryAddLightMarch(WorldMarch march, WorldScene world)
	{
		if (!(world != null) || world.mWorldFogManager == null)
		{
			return;
		}
		if (mMonsterMarchBrushSizeDic == null)
		{
			InitMonsterMarchBrushSize();
		}
		Vector2 point = new Vector2(march.position.x, march.position.z);
		if (!world.mWorldFogManager.mCurrentFogRect.Contains(point))
		{
			return;
		}
		if (mLightMarchDict.TryGetValue(march.uuid, out var value))
		{
			Vector3 moveDir = march.MoveDir;
			float y = Vector3.SignedAngle(Vector3.forward, moveDir, Vector3.up);
			Vector3 euler = new Vector3(0f, y, 0f);
			value.rotation = Quaternion.Euler(euler);
			mLightMarchDict[march.uuid] = value;
			march.LightLength = value.brushSize * 0.8f;
		}
		else
		{
			value = default(LightSourceData);
			float value2 = 1f;
			if (mMonsterMarchBrushSizeDic != null && mMonsterMarchBrushSizeDic.Count > 0)
			{
				int value3 = march.monsterSpecialType;
				if (value3 == 0)
				{
					int monsterId = march.monsterId;
					if (monsterId > 0 && mMonsterIdToSpecialId != null && !mMonsterIdToSpecialId.TryGetValue(monsterId, out value3))
					{
						value3 = GameEntry.Lua.CallWithReturn<int, string, int, string>("CSharpCallLuaInterface.GetTemplateData", "lw_world_monster", monsterId, "special");
						mMonsterIdToSpecialId.Add(monsterId, value3);
					}
				}
				if (!mMonsterMarchBrushSizeDic.TryGetValue(value3, out value2))
				{
					value2 = mMarchSize;
				}
			}
			else
			{
				value2 = mMarchSize;
			}
			value.marchLightSize = value2;
			if (world.mWorldFogManager.mIsBloodyNight)
			{
				value.brushSize = value2 * mMarchBrushSize_Bloody;
			}
			else
			{
				value.brushSize = value2 * mMarchBrushSize;
			}
			march.LightLength = value.brushSize * 0.8f;
			value.pointId = march.uuid;
			Vector3 moveDir2 = march.MoveDir;
			float y2 = Vector3.SignedAngle(Vector3.forward, moveDir2, Vector3.up);
			Vector3 euler2 = new Vector3(0f, y2, 0f);
			value.rotation = Quaternion.Euler(euler2);
			mLightMarchDict.Add(march.uuid, value);
		}
		mMarchLightInstanceDirty = true;
	}

	public void TryRemoveLightMarch(WorldMarch march)
	{
		if (mLightMarchDict.TryGetValue(march.uuid, out var _))
		{
			mLightMarchDict.Remove(march.uuid);
			mMarchLightInstanceDirty = true;
		}
	}

	public void AddDiscoLight(int pointId)
	{
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (!IsDawn(curServerId))
		{
			Vector2Int tilePos = SceneManager.World.IndexToTilePos(pointId);
			Vector3 worldPosVector = SceneManager.World.TileToWorld(tilePos);
			Vector2 vector = new Vector2(worldPosVector.x, worldPosVector.z);
			int num = 1;
			int num2 = num * 2 + 1;
			float num3 = num2;
			num3 = (float)num2 * mFogBrushSize;
			if (!mDiscoLightDataCache.ContainsKey(pointId))
			{
				LightSourceData value = default(LightSourceData);
				value.pointId = pointId;
				value.radius = num;
				value.worldPosVector2 = vector;
				value.worldPosVector3 = worldPosVector;
				value.level = 1;
				value.size = num2;
				value.brushSize = num3;
				value.radiationRect = new Rect(vector, new Vector2(num2 * 2, num2 * 2));
				value.radiationRect.center = vector;
				mDiscoLightDataCache.Add(value.pointId, value);
			}
			mDiscoLightDataDirty = true;
		}
	}

	public void RemoveDiscoLight(int pointId)
	{
		if (mDiscoLightDataCache.ContainsKey(pointId))
		{
			mDiscoLightDataCache.Remove(pointId);
			mDiscoLightDataDirty = true;
		}
	}

	public void HandleLightDataChange(PushLightChange data, WorldScene world)
	{
		LightData light = data.Light;
		bool state = data.State;
		if (light == null)
		{
			return;
		}
		Vector2Int tilePos = SceneManager.World.IndexToTilePos(light.PointId);
		Vector3 worldPosVector = SceneManager.World.TileToWorld(tilePos);
		Vector2 vector = new Vector2(worldPosVector.x, worldPosVector.z);
		int radius = light.Radius;
		int num = radius * 2 + 1;
		float num2 = num;
		num2 = ((!world.mWorldFogManager.mIsBloodyNight) ? ((float)num * mFogBrushSize) : ((float)num * mFogBrushSize_Bloody));
		if (state)
		{
			if (mLightSourceDataCache.ContainsKey(light.PointId))
			{
				mLightSourceDataCache.TryGetValue(light.PointId, out var value);
				value.pointId = light.PointId;
				value.radius = radius;
				value.worldPosVector2 = vector;
				value.worldPosVector3 = worldPosVector;
				value.level = light.Level;
				value.size = num;
				value.brushSize = num2;
				value.radiationRect = new Rect(vector, new Vector2(num * 2, num * 2));
				value.radiationRect.center = vector;
				mLightSourceDataCache[light.PointId] = value;
			}
			else
			{
				LightSourceData value = default(LightSourceData);
				value.pointId = light.PointId;
				value.radius = radius;
				value.worldPosVector2 = vector;
				value.worldPosVector3 = worldPosVector;
				value.level = light.Level;
				value.size = num;
				value.brushSize = num2;
				value.radiationRect = new Rect(vector, new Vector2(num * 2, num * 2));
				value.radiationRect.center = vector;
				mLightSourceDataCache.Add(value.pointId, value);
			}
			mLightInstanceDirty = true;
		}
		else
		{
			if (!mLightSourceDataCache.ContainsKey(light.PointId))
			{
				return;
			}
			mLightSourceDataCache.TryGetValue(light.PointId, out var _);
			mLightSourceDataCache.Remove(light.PointId);
			mLightInstanceDirty = true;
		}
		if (!world.mWorldFogManager.mIsBloodyNight && !world.mWorldFogManager.mIsDawn)
		{
			GameEntry.Event.Fire(EventId.PushLightChangeInWhiteNight, radius * 10000000 + light.PointId);
		}
	}

	public void HandleWorldGetBlock(RangeLightData msg, WorldScene world)
	{
		RepeatedField<LightData> lightList = msg.LightList;
		mLightSourceDataCache.Clear();
		for (int i = 0; i < lightList.Count; i++)
		{
			LightData lightData = lightList[i];
			if (lightData != null)
			{
				Vector2Int tilePos = SceneManager.World.IndexToTilePos(lightData.PointId);
				Vector3 worldPosVector = SceneManager.World.TileToWorld(tilePos);
				Vector2 vector = new Vector2(worldPosVector.x, worldPosVector.z);
				int radius = lightData.Radius;
				int num = radius * 2 + 1;
				float num2 = num;
				num2 = ((!world.mWorldFogManager.mIsBloodyNight) ? ((float)num * mFogBrushSize) : ((float)num * mFogBrushSize_Bloody));
				LightSourceData value = default(LightSourceData);
				value.pointId = lightData.PointId;
				value.radius = radius;
				value.worldPosVector2 = vector;
				value.worldPosVector3 = worldPosVector;
				value.level = lightData.Level;
				value.size = num;
				value.brushSize = num2;
				value.radiationRect = new Rect(vector, new Vector2(num * 2, num * 2));
				value.radiationRect.center = vector;
				mLightSourceDataCache.Add(value.pointId, value);
			}
		}
		mLightInstanceDirty = true;
	}

	public void UpdateTerrain(Vector2Int centerTile, int tileWidth, int tileHeight)
	{
		int x = centerTile.x - tileWidth;
		int x2 = centerTile.x + tileWidth;
		int y = centerTile.y - tileHeight;
		int y2 = centerTile.y + tileHeight;
		_onLighdataChanged(x, y, x2, y2);
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (worldScene != null && worldScene.StaticManager != null)
		{
			worldScene.StaticManager.SetViewDirty();
		}
	}

	public void GetOnLighdataChange(OnLighdataChangeFun func)
	{
		_onLighdataChanged = func;
	}

	public int GetMaxLightLevelInPointId(int pointId)
	{
		Vector2Int tilePos = SceneManager.World.IndexToTilePos(pointId);
		Vector3 vector = SceneManager.World.TileToWorld(tilePos);
		Vector2 point = new Vector2(vector.x, vector.z);
		int num = -1;
		foreach (LightSourceData value in mLightSourceDataCache.Values)
		{
			Rect radiationRect = value.radiationRect;
			if (radiationRect.Contains(point) && num < value.level)
			{
				num = value.level;
			}
		}
		return num;
	}

	public bool IsLightUpInPointId(int pointId)
	{
		Vector2Int tilePos = SceneManager.World.IndexToTilePos(pointId);
		Vector3 vector = SceneManager.World.TileToWorld(tilePos);
		Vector2 point = new Vector2(vector.x, vector.z);
		foreach (LightSourceData value in mLightSourceDataCache.Values)
		{
			Rect radiationRect = value.radiationRect;
			if (radiationRect.Contains(point) && value.level > 0)
			{
				return true;
			}
		}
		return false;
	}

	public static void CloseEvColorInDarknessSeason()
	{
		Shader.SetGlobalColor(mEvColorId, Color.grey);
		Shader.SetGlobalInt(mEvColorOnId, 0);
	}

	public Color GetEvColorInDarknessSeason(int lightLevel, bool isBloody)
	{
		if (mNightColorDic == null || mNightColorDic == null)
		{
			InitNightColorDic();
		}
		Color value = Color.white;
		if (isBloody)
		{
			mNightBloodyColorDic.TryGetValue(lightLevel, out value);
		}
		else
		{
			mNightColorDic.TryGetValue(lightLevel, out value);
		}
		return value;
	}

	public Color GetCurrentEvColorInDarknessSeason()
	{
		if (mNightColorDic == null || mNightColorDic == null)
		{
			InitNightColorDic();
		}
		int key = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetMainBaseMaxLightLevel");
		Color value = Color.white;
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsBloodyNight", curServerId))
		{
			mNightBloodyColorDic.TryGetValue(key, out value);
		}
		else
		{
			mNightColorDic.TryGetValue(key, out value);
		}
		return value;
	}

	public Color GetCurrentFogColorInDarknessSeason()
	{
		if (mCityFogColorDic == null || mCityFog_BloodyColorDic == null)
		{
			InitNightColorDic();
		}
		int key = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "Season4BrightnessLevel");
		Color value = Color.white;
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsBloodyNight", curServerId))
		{
			mCityFog_BloodyColorDic.TryGetValue(key, out value);
		}
		else
		{
			mCityFogColorDic.TryGetValue(key, out value);
		}
		return value;
	}

	public static Color StringToColor(string data)
	{
		string[] array = data.Split(new char[1] { ';' });
		if (array.Length >= 4)
		{
			return new Color(Convert.ToSingle(array[0]), Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3]));
		}
		return Color.white;
	}

	private void InitNightColorDic()
	{
		mNightBloodyColorDic = new Dictionary<int, Color>();
		mNightColorDic = new Dictionary<int, Color>();
		mCityFogColorDic = new Dictionary<int, Color>();
		mCityFog_BloodyColorDic = new Dictionary<int, Color>();
		Color white = Color.white;
		Color white2 = Color.white;
		float num = 1f;
		for (int i = 0; i < 5; i++)
		{
			white = StringToColor(GameEntry.ConfigCache.GetTemplateData("lw_lights_on", i, "moon_rgba"));
			white2 = new Color(Mathf.Pow(white.r / 255f, 2.2f) * num, Mathf.Pow(white.g / 255f, 2.2f) * num, Mathf.Pow(white.b / 255f, 2.2f) * num);
			mNightBloodyColorDic.Add(i, white2);
			white = StringToColor(GameEntry.ConfigCache.GetTemplateData("lw_lights_on", i, "city_fog_dark"));
			white2 = new Color(Mathf.Pow(white.r / 255f, 2.2f) * num, Mathf.Pow(white.g / 255f, 2.2f) * num, Mathf.Pow(white.b / 255f, 2.2f) * num);
			mCityFogColorDic.Add(i, white2);
			white = StringToColor(GameEntry.ConfigCache.GetTemplateData("lw_lights_on", i, "city_fog_moon"));
			white2 = new Color(Mathf.Pow(white.r / 255f, 2.2f) * num, Mathf.Pow(white.g / 255f, 2.2f) * num, Mathf.Pow(white.b / 255f, 2.2f) * num);
			mCityFog_BloodyColorDic.Add(i, white2);
			white = StringToColor(GameEntry.ConfigCache.GetTemplateData("lw_lights_on", i, "dark_rgba"));
			white2 = new Color(Mathf.Pow(white.r / 255f, 2.2f) * num, Mathf.Pow(white.g / 255f, 2.2f) * num, Mathf.Pow(white.b / 255f, 2.2f) * num);
			mNightColorDic.Add(i, white2);
		}
	}
}
