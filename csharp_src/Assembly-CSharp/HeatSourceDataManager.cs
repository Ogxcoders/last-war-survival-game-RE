using System;
using System.Collections.Generic;
using FibMatrix;
using GameFramework;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua;

public class HeatSourceDataManager
{
	public delegate void OnHeatSourceChanged(int x1, int y1, int x2, int y2);

	private static HeatSourceDataManager instance;

	private Dictionary<int, CityHeatSource> cityHeatSource = new Dictionary<int, CityHeatSource>();

	private Dictionary<int, ConstHeatSource> constHeatSource = new Dictionary<int, ConstHeatSource>();

	private Dictionary<long, DynamicHeatSource> dynamicHeatSource = new Dictionary<long, DynamicHeatSource>();

	private int myCityId;

	private float constTemperature;

	private MyBaseThermalConductor myBaseConductor = new MyBaseThermalConductor();

	private string mainUuid;

	private ThermalConductor tempConductor = new ThermalConductor();

	private Dictionary<int, int> flagId2Group = new Dictionary<int, int>();

	private Dictionary<int, DynamicHeatSource> group2flagHS = new Dictionary<int, DynamicHeatSource>();

	private ObjectPool<DynamicHeatSource> dhsPool;

	private ObjectPool<CityHeatSource> chsPool;

	private Tuple<float, float, float, int>[] _states;

	private OnHeatSourceChanged _onHeatSourceChanged;

	public bool CurWorldIsSnowSeason;

	private Dictionary<long, DynamicHeatSource> dhsCache = new Dictionary<long, DynamicHeatSource>(64);

	public bool WorldCalSwitch { get; set; }

	public void SetOnHeatSourceChanged(OnHeatSourceChanged func)
	{
		_onHeatSourceChanged = func;
	}

	public static HeatSourceDataManager GetInstance()
	{
		if (instance == null)
		{
			instance = new HeatSourceDataManager();
			instance.Init();
		}
		return instance;
	}

	private void Init()
	{
		dhsPool = new ObjectPool<DynamicHeatSource>(32);
		chsPool = new ObjectPool<CityHeatSource>(200);
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetWarFlagGroup")?.ForEach(delegate(int id, int group)
		{
			flagId2Group[id] = group;
		});
		AddListener();
	}

	public void Destroy()
	{
		myBaseConductor.Destroy();
		RemoveListener();
		RemoveAllStaticData();
		RemoveAllDynamicData();
		dhsPool.Dispose();
		chsPool.Dispose();
	}

	private void RemoveAllStaticData()
	{
		constHeatSource.Clear();
		chsPool.RecycleNoClear(cityHeatSource.Values);
		cityHeatSource.Clear();
	}

	private void RemoveAllDynamicData()
	{
		dhsPool.RecycleNoClear(dynamicHeatSource.Values);
		dynamicHeatSource.Clear();
	}

	private void AddListener()
	{
		GameEntry.Event.Subscribe(EventId.OnEnterCityState, OnEnterCity);
		GameEntry.Event.Subscribe(EventId.OnEnterCrossServer, OnEnterCrossServer);
		GameEntry.Event.Subscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
	}

	private void RemoveListener()
	{
		GameEntry.Event.Unsubscribe(EventId.OnEnterCityState, OnEnterCity);
		GameEntry.Event.Unsubscribe(EventId.OnEnterCrossServer, OnEnterCrossServer);
		GameEntry.Event.Unsubscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
	}

	private void OnEnterCity(object obj)
	{
		HotSpotMayEffectMeMessage.Instance.Send();
	}

	private void RefreshCurServerData()
	{
		HotSpotBaseInfoMessage.Instance.Send(GameEntry.Data.Player.GetCurServerId());
		CurWorldIsSnowSeason = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.CurWorldIsSnowSeason");
	}

	private void OnEnterCrossServer(object obj)
	{
		RefreshCurServerData();
	}

	private void OnQuitCrossServer(object obj)
	{
		RefreshCurServerData();
	}

	public void OnEnterGame()
	{
		RefreshCurServerData();
		HotSpotMayEffectMeMessage.Instance.Send();
		mainUuid = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetMainUuid");
		GetThermalConductorInfo(mainUuid);
		string s = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "season_new_s2_snowyground", "k1");
		string text = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "season_new_s2_snowyground", "k2");
		int num = 1;
		if (int.TryParse(s, out var result) && result > num)
		{
			num = result;
		}
		string[] array = text.Split(new char[1] { ';' });
		if (array.Length <= 1)
		{
			return;
		}
		_states = new Tuple<float, float, float, int>[array.Length];
		int i = 0;
		for (int num2 = array.Length; i < num2; i++)
		{
			string[] array2 = array[i].Split(new char[1] { ',' });
			if (array2.Length >= 3 && float.TryParse(array2[0], out var result2) && float.TryParse(array2[1], out var result3) && float.TryParse(array2[2], out var result4) && result3 > result4)
			{
				float item = ((result2 > (float)num) ? ((float)num) : result2);
				_states[i] = new Tuple<float, float, float, int>(item, result3, result4, 3);
			}
		}
	}

	public void HandleHotSpotBaseInfo(ISFSObject msg)
	{
		RemoveAllStaticData();
		ISFSArray iSFSArray = msg.TryGetArray("list");
		if (iSFSArray != null)
		{
			foreach (object item in iSFSArray)
			{
				CityHeatSource cityHeatSource = chsPool.Allocate();
				cityHeatSource.ParseData(item as ISFSObject);
				this.cityHeatSource[cityHeatSource.GetCityId()] = cityHeatSource;
			}
		}
		constTemperature = 0f;
		ISFSArray iSFSArray2 = msg.TryGetArray("envEvents");
		if (iSFSArray2 == null)
		{
			return;
		}
		foreach (object item2 in iSFSArray2)
		{
			ConstHeatSource constHeatSource = new ConstHeatSource();
			constHeatSource.ParseData(item2 as ISFSObject);
			this.constHeatSource[constHeatSource.cfgId] = constHeatSource;
			constTemperature += constHeatSource.temperature;
		}
	}

	public void HandleWorldGetBlock(ISFSObject msg, int lb, int rt)
	{
		Vector2Int vector2Int = TileCoord.IndexToTilePos(lb, ForceChangeScene.World);
		Vector2Int vector2Int2 = TileCoord.IndexToTilePos(rt, ForceChangeScene.World);
		int x = vector2Int.x;
		int y = vector2Int.y;
		int x2 = vector2Int2.x;
		int y2 = vector2Int2.y;
		dhsCache.Clear();
		foreach (DynamicHeatSource value2 in this.dynamicHeatSource.Values)
		{
			if (x <= value2.x && value2.x <= x2 && y <= value2.y && value2.y <= y2)
			{
				dhsCache[value2.uuid] = value2;
			}
		}
		ISFSArray iSFSArray = msg.TryGetArray("list");
		if (iSFSArray != null)
		{
			foreach (object item in iSFSArray)
			{
				DynamicHeatSource dynamicHeatSource = dhsPool.Allocate();
				dynamicHeatSource.ParseData(item as ISFSObject);
				if (x > dynamicHeatSource.x || dynamicHeatSource.x > x2 || y > dynamicHeatSource.y || dynamicHeatSource.y > y2)
				{
					continue;
				}
				if (dhsCache.TryGetValue(dynamicHeatSource.uuid, out var value))
				{
					if (value.state != dynamicHeatSource.state)
					{
						dhsPool.Recycle(value);
						this.dynamicHeatSource[dynamicHeatSource.uuid] = dynamicHeatSource;
						_onHeatSourceChanged?.Invoke(dynamicHeatSource.minX, dynamicHeatSource.minY, dynamicHeatSource.maxX, dynamicHeatSource.maxY);
					}
					else
					{
						dhsPool.Recycle(dynamicHeatSource);
					}
					dhsCache.Remove(dynamicHeatSource.uuid);
				}
				else
				{
					this.dynamicHeatSource[dynamicHeatSource.uuid] = dynamicHeatSource;
					_onHeatSourceChanged?.Invoke(dynamicHeatSource.minX, dynamicHeatSource.minY, dynamicHeatSource.maxX, dynamicHeatSource.maxY);
				}
			}
		}
		foreach (DynamicHeatSource value3 in dhsCache.Values)
		{
			dhsPool.Recycle(value3);
			this.dynamicHeatSource.Remove(value3.uuid);
			_onHeatSourceChanged?.Invoke(value3.minX, value3.minY, value3.maxX, value3.maxY);
		}
	}

	public void HandleHotSpotMayEffectMe(ISFSObject msg)
	{
		myCityId = msg.TryGetInt("strongholdId");
		if (myCityId <= 0)
		{
			myCityId = msg.TryGetInt("cityId");
		}
		RemoveAllDynamicData();
		ISFSArray iSFSArray = msg.TryGetArray("list");
		if (iSFSArray == null)
		{
			return;
		}
		foreach (object item in iSFSArray)
		{
			DynamicHeatSource dynamicHeatSource = dhsPool.Allocate();
			dynamicHeatSource.ParseData(item as ISFSObject);
			this.dynamicHeatSource[dynamicHeatSource.uuid] = dynamicHeatSource;
		}
	}

	public void HandlePushHotSpotPatch(ISFSObject msg)
	{
		ISFSObject iSFSObject = msg.TryGetObj("update");
		if (iSFSObject != null)
		{
			switch (iSFSObject.TryGetInt("type"))
			{
			case 1:
			case 2:
			{
				int key3 = iSFSObject.TryGetInt("otherCfgId");
				if (!cityHeatSource.TryGetValue(key3, out var value3))
				{
					value3 = chsPool.Allocate();
				}
				value3.ParseData(iSFSObject);
				cityHeatSource[key3] = value3;
				break;
			}
			case 0:
			{
				int key2 = iSFSObject.TryGetInt("cfgId");
				if (constHeatSource.TryGetValue(key2, out var value2))
				{
					constTemperature -= value2.temperature;
				}
				else
				{
					value2 = new ConstHeatSource();
				}
				value2.ParseData(iSFSObject);
				constHeatSource[key2] = value2;
				constTemperature += value2.temperature;
				break;
			}
			default:
			{
				long key = iSFSObject.TryGetLong("uuid");
				if (!dynamicHeatSource.TryGetValue(key, out var value))
				{
					value = dhsPool.Allocate();
				}
				value.ParseData(iSFSObject);
				dynamicHeatSource[key] = value;
				_onHeatSourceChanged?.Invoke(value.minX, value.minY, value.maxX, value.maxY);
				break;
			}
			}
		}
		long num = msg.TryGetLong("delete");
		if (num > 0 && dynamicHeatSource.TryGetValue(num, out var value4))
		{
			dhsPool.Recycle(value4);
			dynamicHeatSource.Remove(num);
			_onHeatSourceChanged?.Invoke(value4.minX, value4.minY, value4.maxX, value4.maxY);
		}
	}

	public void HandlePushConstHotSpotDel(ISFSObject msg)
	{
		int cfgId = msg.TryGetInt("cfgId");
		RemoveConstHeatSource(cfgId);
	}

	public void CreateConstHeatSource(int cfgId, int type, float temperature)
	{
		if (constHeatSource.TryGetValue(cfgId, out var value))
		{
			constTemperature -= value.temperature;
		}
		else
		{
			value = new ConstHeatSource();
		}
		value.CreateByClient(cfgId, type, temperature);
		constHeatSource[cfgId] = value;
		constTemperature += value.temperature;
	}

	public void RemoveConstHeatSource(int cfgId)
	{
		if (constHeatSource.TryGetValue(cfgId, out var value))
		{
			constTemperature -= value.temperature;
			value.Dispose();
			constHeatSource.Remove(cfgId);
		}
	}

	public float GetTemperatureByIndex(int pointIndex)
	{
		Vector2Int vector2Int = TileCoord.IndexToTilePos(pointIndex, ForceChangeScene.World);
		return GetTemperatureByXY(vector2Int.x, vector2Int.y);
	}

	public float GetTemperatureByXY(int x, int y)
	{
		float num = constTemperature;
		int pointId = x + y * 1000 + 1;
		int zoneIdByPosId = SceneManager.World.GetZoneIdByPosId(pointId);
		if (cityHeatSource.TryGetValue(zoneIdByPosId, out var value))
		{
			num += value.temperature;
		}
		group2flagHS.Clear();
		float? num2 = null;
		foreach (DynamicHeatSource value4 in dynamicHeatSource.Values)
		{
			if (!value4.IsInRange(x, y))
			{
				continue;
			}
			if (value4.type == 4)
			{
				if (!num2.HasValue || num2 < value4.temperature)
				{
					num2 = value4.temperature;
				}
			}
			else if (value4.type == 5)
			{
				if (flagId2Group.TryGetValue(value4.otherCfgId, out var value2))
				{
					if (group2flagHS.TryGetValue(value2, out var value3))
					{
						if (value3.otherCfgId < value4.otherCfgId)
						{
							group2flagHS[value2] = value4;
						}
					}
					else
					{
						group2flagHS[value2] = value4;
					}
				}
				else
				{
					Log.Error($"战旗组获取失败！战旗配置id：{value4.otherCfgId},战旗配置数量:{flagId2Group.Count}");
				}
			}
			else
			{
				num += value4.temperature;
			}
		}
		if (num2.HasValue)
		{
			num += num2.Value;
		}
		foreach (DynamicHeatSource value5 in group2flagHS.Values)
		{
			num += value5.temperature;
		}
		return num;
	}

	public float GetCityHeatSourceTemperature(int cityId)
	{
		if (cityHeatSource.TryGetValue(cityId, out var value))
		{
			return value.temperature;
		}
		return 0f;
	}

	public void GetThermalConductorInfo(string uuid)
	{
		ThermalConductorInfoMessage.Instance.Send(uuid);
	}

	public void HandleThermalConductorInfo(ISFSObject msg)
	{
		if (mainUuid == msg.TryGetString("uuid"))
		{
			RefreshMyBaseThermalConductor(msg);
			GameEntry.Event.Fire(EventId.MyBaseTemperatureInit);
		}
		else
		{
			tempConductor.ParseData(msg);
			GameEntry.Lua.Call("CSharpCallLuaInterface.HandleThermalConductorTemperature", (double)tempConductor.GetCurTemperature());
		}
	}

	public void HandlePushThermalConductorInfo(ISFSObject msg)
	{
		string text = msg.TryGetString("uuid");
		if (mainUuid == text)
		{
			RefreshMyBaseThermalConductor(msg);
		}
		if (SceneManager.World == null || !long.TryParse(text, out var result))
		{
			return;
		}
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(result);
		if (pointInfoByUuid != null && pointInfoByUuid.thermalConductor != null)
		{
			pointInfoByUuid.thermalConductor.ParseData(msg);
			if (pointInfoByUuid is BuildPointInfo)
			{
				GameEntry.Event.Fire(EventId.OtherBaseThermalRefresh, text);
			}
			GameEntry.Event.Fire(EventId.PointThermalRefresh, result);
		}
	}

	public void RefreshMyBaseThermalConductor(ISFSObject msg)
	{
		int phase = myBaseConductor.phase;
		float curTemperature = myBaseConductor.GetCurTemperature();
		int phaseChangeType = myBaseConductor.GetPhaseChangeType();
		myBaseConductor.ParseData(msg);
		if (phaseChangeType != myBaseConductor.GetPhaseChangeType())
		{
			GameEntry.Event.Fire(EventId.MyBasePhaseChangeChange);
		}
		if (phase != myBaseConductor.phase)
		{
			GameEntry.Event.Fire(EventId.MyBasePhaseChange);
		}
		float f = myBaseConductor.GetCurTemperature() - curTemperature;
		if (Mathf.Abs(f) > 1f)
		{
			GameEntry.Event.Fire(EventId.MyBaseTemperatureChangeSuddenChange, Mathf.RoundToInt(f));
		}
		GameEntry.Event.Fire(EventId.MyBaseThermalRefresh);
	}

	public float GetMyTileTemperature()
	{
		float num = constTemperature;
		int zoneIdByPosId = myCityId;
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		Vector2Int vector2Int = TileCoord.IndexToTilePos(worldMainPos, ForceChangeScene.World);
		if (SceneManager.IsInWorld())
		{
			zoneIdByPosId = SceneManager.World.GetZoneIdByPosId(worldMainPos);
		}
		if (cityHeatSource.TryGetValue(zoneIdByPosId, out var value))
		{
			num += value.temperature;
		}
		group2flagHS.Clear();
		float? num2 = null;
		foreach (DynamicHeatSource value4 in dynamicHeatSource.Values)
		{
			if (!value4.IsInRange(vector2Int.x, vector2Int.y))
			{
				continue;
			}
			if (value4.type == 4)
			{
				if (!num2.HasValue || num2 < value4.temperature)
				{
					num2 = value4.temperature;
				}
			}
			else if (value4.type == 5)
			{
				if (flagId2Group.TryGetValue(value4.otherCfgId, out var value2))
				{
					if (group2flagHS.TryGetValue(value2, out var value3))
					{
						if (value3.otherCfgId < value4.otherCfgId)
						{
							group2flagHS[value2] = value4;
						}
					}
					else
					{
						group2flagHS[value2] = value4;
					}
				}
				else
				{
					Log.Error($"战旗组获取失败！战旗配置id：{value4.otherCfgId},战旗配置数量:{flagId2Group.Count}");
				}
			}
			else
			{
				num += value4.temperature;
			}
		}
		if (num2.HasValue)
		{
			num += num2.Value;
		}
		foreach (DynamicHeatSource value5 in group2flagHS.Values)
		{
			num += value5.temperature;
		}
		return num;
	}

	public List<HeatSourceBase> GetHeatSourceAffectMe()
	{
		List<HeatSourceBase> list = new List<HeatSourceBase>();
		foreach (ConstHeatSource value4 in constHeatSource.Values)
		{
			list.Add(value4);
		}
		int zoneIdByPosId = myCityId;
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		Vector2Int vector2Int = TileCoord.IndexToTilePos(worldMainPos, ForceChangeScene.World);
		if (SceneManager.IsInWorld())
		{
			zoneIdByPosId = SceneManager.World.GetZoneIdByPosId(worldMainPos);
		}
		if (cityHeatSource.TryGetValue(zoneIdByPosId, out var value))
		{
			list.Add(value);
		}
		group2flagHS.Clear();
		DynamicHeatSource dynamicHeatSource = null;
		foreach (DynamicHeatSource value5 in this.dynamicHeatSource.Values)
		{
			if (!value5.IsInRange(vector2Int.x, vector2Int.y))
			{
				continue;
			}
			if (value5.type == 4)
			{
				if (dynamicHeatSource == null || dynamicHeatSource.temperature < value5.temperature)
				{
					dynamicHeatSource = value5;
				}
			}
			else if (value5.type == 5)
			{
				if (flagId2Group.TryGetValue(value5.otherCfgId, out var value2))
				{
					if (group2flagHS.TryGetValue(value2, out var value3))
					{
						if (value3.otherCfgId < value5.otherCfgId)
						{
							group2flagHS[value2] = value5;
						}
					}
					else
					{
						group2flagHS[value2] = value5;
					}
				}
				else
				{
					Log.Error($"战旗组获取失败！战旗配置id：{value5.otherCfgId},战旗配置数量:{flagId2Group.Count}");
				}
			}
			else
			{
				list.Add(value5);
			}
		}
		if (dynamicHeatSource != null)
		{
			list.Add(dynamicHeatSource);
		}
		foreach (DynamicHeatSource value6 in group2flagHS.Values)
		{
			list.Add(value6);
		}
		return list;
	}

	public bool IsMyBaseFrozen()
	{
		return myBaseConductor.phase == 2;
	}

	public float GetMyBaseTemperature()
	{
		return myBaseConductor.GetCurTemperature();
	}

	public ThermalConductor GetMyBaseConductor()
	{
		return myBaseConductor;
	}

	public float GetTerrainStateByXY(int x, int y)
	{
		if (_states == null)
		{
			return 1f;
		}
		float temperatureByXY = GetTemperatureByXY(x, y);
		int num = _states.Length;
		for (int i = 0; i < num; i++)
		{
			if (_states[i] == null)
			{
				continue;
			}
			switch (_states[i].Item4)
			{
			case 1:
				if (temperatureByXY < _states[i].Item2 && temperatureByXY > _states[i].Item3)
				{
					return _states[i].Item1;
				}
				break;
			case 2:
				if (temperatureByXY < _states[i].Item2 && temperatureByXY >= _states[i].Item3)
				{
					return _states[i].Item1;
				}
				break;
			case 3:
				if (temperatureByXY <= _states[i].Item2 && temperatureByXY > _states[i].Item3)
				{
					return _states[i].Item1;
				}
				break;
			case 4:
				if (temperatureByXY <= _states[i].Item2 && temperatureByXY >= _states[i].Item3)
				{
					return _states[i].Item1;
				}
				break;
			}
		}
		return 1f;
	}
}
