using System.Collections.Generic;
using UnityEngine;
using XLua;

public class DCBuilding : BaseDataContainer
{
	private LuaTable _CSharpCallLuaInterface_;

	private LuaFunction _luafunc_GetBuildingDataParamByUuid;

	private LuaFunction _luafunc_GetBuildingDataParamByBuildId;

	private Vector2Int _cityCenterPos;

	private int _dragonWorldPos = -1;

	private HashSet<long> _myBuildingUUid;

	private void init()
	{
		_cityCenterPos = Vector2Int.zero;
		_CSharpCallLuaInterface_ = GameEntry.Lua.Env.Global.Get<LuaTable>("CSharpCallLuaInterface");
		_luafunc_GetBuildingDataParamByUuid = _CSharpCallLuaInterface_.Get<LuaFunction>("GetBuildingDataParamByUuid");
		_luafunc_GetBuildingDataParamByBuildId = _CSharpCallLuaInterface_.Get<LuaFunction>("GetBuildingDataParamByBuildId");
		_myBuildingUUid = new HashSet<long>();
	}

	public LuaBuildData GetBuildingDataByUuid(long uuid)
	{
		if (_CSharpCallLuaInterface_ == null)
		{
			init();
		}
		LuaBuildData result = null;
		if (_luafunc_GetBuildingDataParamByUuid != null)
		{
			result = _luafunc_GetBuildingDataParamByUuid.CallReturnBuildingData(uuid, 0);
		}
		return result;
	}

	public LuaBuildData GetBuildingDataByBuildId(int buildId)
	{
		if (_CSharpCallLuaInterface_ == null)
		{
			init();
		}
		LuaBuildData result = null;
		if (_luafunc_GetBuildingDataParamByBuildId != null)
		{
			result = _luafunc_GetBuildingDataParamByBuildId.CallReturnBuildingData(0L, buildId);
		}
		return result;
	}

	public int GetMainLv()
	{
		return GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetMainLv");
	}

	public Vector2Int GetMainPos()
	{
		if (_cityCenterPos == Vector2Int.zero)
		{
			_cityCenterPos = GameEntry.Lua.CallWithReturn<Vector2Int>("CSharpCallLuaInterface.GetMainPos");
		}
		return _cityCenterPos;
	}

	public void SetMainPos()
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.SetMainPos");
	}

	public int GetWorldMainPos()
	{
		return GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetWorldMainPos");
	}

	public int GetDragonWorldPos()
	{
		return _dragonWorldPos;
	}

	public void UpdateMyBuilding(long uuid, bool remove)
	{
		if (_myBuildingUUid == null)
		{
			init();
		}
		if (remove)
		{
			_myBuildingUUid.Remove(uuid);
		}
		else
		{
			_myBuildingUUid.Add(uuid);
		}
	}

	public void UpdateDragonWorldPos(int pos)
	{
		_dragonWorldPos = pos;
	}

	public bool CheckIsMyBuilding(long uuid)
	{
		if (_myBuildingUUid == null)
		{
			return false;
		}
		return _myBuildingUUid.Contains(uuid);
	}
}
