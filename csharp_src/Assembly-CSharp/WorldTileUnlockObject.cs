using System.Collections.Generic;
using UnityEngine;
using XLua;

internal class WorldTileUnlockObject
{
	private int _id;

	private int _lockState;

	private string _prefabPath;

	private Vector2Int _posToBase;

	private List<int> _nextList = new List<int>();

	private InstanceRequest _tileInst;

	private GameObject _gameObject;

	private TouchObjectEventTrigger _touchEvent;

	public List<int> NextList => _nextList;

	public void Create(LuaTable data)
	{
		_id = data.Get<int>("id");
		_lockState = data.Get<int>("state");
		_prefabPath = data.Get<string>("prefabName");
		_nextList.Clear();
		LuaTable luaTable = data.Get<LuaTable>("nextList");
		for (int i = 1; i <= luaTable.Length; i++)
		{
			_nextList.Add(luaTable.Get<int>(i));
		}
		LuaTable luaTable2 = data.Get<LuaTable>("pos");
		_posToBase = new Vector2Int(luaTable2.Get<int>("x"), luaTable2.Get<int>("y"));
		string text = "";
		if (_lockState == 1 || _lockState == 2 || _lockState == 4)
		{
			text = _prefabPath;
			if (string.IsNullOrEmpty(text))
			{
				text = "Assets/Main/Prefabs/World/TileLocked.prefab";
			}
		}
		else if (_lockState == 5)
		{
			text = "Assets/Main/Prefabs/World/TileUnlocked.prefab";
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		Vector3 worldPos = SceneManager.World.TileToWorld(mainPos + _posToBase);
		_tileInst = GameEntry.Resource.InstantiateAsync(text);
		_tileInst.completed += delegate
		{
			_gameObject = _tileInst.gameObject;
			_gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			_gameObject.transform.position = worldPos;
			_touchEvent = _gameObject.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
			if (_touchEvent != null)
			{
				_touchEvent.onPointerClick = OnClick;
			}
		};
	}

	public void Destroy()
	{
		if (_tileInst != null)
		{
			_tileInst.Destroy();
			_tileInst = null;
			_gameObject = null;
		}
	}

	public void RefreshState()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetLandLockDataById", _id);
		int num = luaTable.Get<int>("state");
		if (_lockState != num)
		{
			Destroy();
			Create(luaTable);
		}
	}

	public void OnUnlocked()
	{
		_lockState = 5;
		if (_tileInst != null)
		{
			_gameObject = null;
			_tileInst.Destroy();
			_tileInst = null;
		}
		Vector2Int mainPos = GameEntry.Data.Building.GetMainPos();
		Vector3 worldPos = SceneManager.World.TileToWorld(mainPos + _posToBase);
		_tileInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/TileUnlocked.prefab");
		_tileInst.completed += delegate
		{
			_gameObject = _tileInst.gameObject;
			_gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
			_gameObject.transform.position = worldPos;
			_touchEvent = _gameObject.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
			if (_touchEvent != null)
			{
				_touchEvent.onPointerClick = OnClick;
			}
		};
	}

	public void OnUpdateLod(int lod)
	{
	}

	private void OnClick()
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.ClickLandLockById", _id);
	}
}
