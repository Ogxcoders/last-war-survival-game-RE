using System;
using System.Collections.Generic;
using XLua;

public class DCRoad : BaseDataContainer
{
	private Dictionary<int, CityRoadPathParam> allRoad;

	private bool _isInit;

	public CityStreetManager StreetManager = new CityStreetManager();

	public DCRoad()
	{
		_isInit = false;
		allRoad = new Dictionary<int, CityRoadPathParam>();
	}

	public override void Release()
	{
		base.Release();
		_isInit = false;
		GameEntry.Event.Unsubscribe(EventId.RefreshCityRoadArr, RefreshCityRoadArrSignal);
		GameEntry.Event.Unsubscribe(EventId.DeleteCityRoadArr, DeleteCityRoadArrSignal);
	}

	public void Init()
	{
		_isInit = true;
		allRoad.Clear();
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllPathRoadData");
		if (luaTable != null)
		{
			for (int i = 1; i <= luaTable.Length; i++)
			{
				AddOneRoadByLua((LuaTable)luaTable[i]);
			}
		}
		StreetManager.ExpireCache();
		GameEntry.Event.Subscribe(EventId.RefreshCityRoadArr, RefreshCityRoadArrSignal);
		GameEntry.Event.Subscribe(EventId.DeleteCityRoadArr, DeleteCityRoadArrSignal);
	}

	private void AddOneRoadByLua(LuaTable data)
	{
		if (data != null && data.ContainsKey("pointId"))
		{
			int pointId = data.Get<int>("pointId");
			int mainRoadDirection = data.Get<int>("mainRoadDirection");
			int viaductDirection = data.Get<int>("viaductDirection");
			AddOneRoad(pointId, mainRoadDirection, viaductDirection);
		}
	}

	private void AddOneRoad(int pointId, int mainRoadDirection, int viaductDirection)
	{
		CityRoadPathParam cityRoadPathParam = GetRoadByPointId(pointId);
		if (cityRoadPathParam == null)
		{
			cityRoadPathParam = new CityRoadPathParam();
			allRoad.Add(pointId, cityRoadPathParam);
		}
		else
		{
			cityRoadPathParam.Reset();
		}
		if (mainRoadDirection >= 0)
		{
			cityRoadPathParam.addPathType(CityRoadPathParam.PathType.MAIN_ROAD);
			cityRoadPathParam.addDirectType((CityRoadPathParam.DirectType)mainRoadDirection);
		}
		else
		{
			cityRoadPathParam.addPathType(CityRoadPathParam.PathType.NORMAL);
		}
		if (viaductDirection >= 0)
		{
			cityRoadPathParam.addPathType(CityRoadPathParam.PathType.VIADUCT);
			cityRoadPathParam.addDirectType((CityRoadPathParam.DirectType)viaductDirection);
		}
	}

	private void RemoveOneRoad(int pointId)
	{
		if (allRoad.ContainsKey(pointId))
		{
			allRoad.Remove(pointId);
		}
	}

	public CityRoadPathParam GetRoadByPointId(int pointId)
	{
		if (!_isInit)
		{
			Init();
		}
		if (allRoad.ContainsKey(pointId))
		{
			return allRoad[pointId];
		}
		return null;
	}

	public Dictionary<int, CityRoadPathParam> getAllRoads()
	{
		if (!_isInit)
		{
			Init();
		}
		return new Dictionary<int, CityRoadPathParam>(allRoad);
	}

	private void RefreshCityRoadArrSignal(object userData)
	{
		LuaTable luaTable = (LuaTable)userData;
		if (luaTable != null)
		{
			for (int i = 1; i <= luaTable.Length; i++)
			{
				AddOneRoadByLua((LuaTable)luaTable[i]);
			}
			StreetManager.ExpireCache();
		}
	}

	private void DeleteCityRoadArrSignal(object userData)
	{
		LuaTable luaTable = (LuaTable)userData;
		if (luaTable != null)
		{
			for (int i = 1; i <= luaTable.Length; i++)
			{
				RemoveOneRoad(Convert.ToInt32(luaTable[i]));
			}
			StreetManager.ExpireCache();
		}
	}
}
