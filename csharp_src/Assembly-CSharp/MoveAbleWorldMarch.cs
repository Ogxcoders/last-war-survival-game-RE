using System.Collections.Generic;
using UnityEngine;

public class MoveAbleWorldMarch : ITouchPickable
{
	public long uuid;

	public GameObject gameObject;

	public Transform transform;

	public static float ScreenRangeLeft;

	public static float ScreenRangeRight;

	public static float ScreenRangeTop;

	public static float ScreenRangeDown;

	private float _minX;

	private float _maxX;

	private float _minY;

	private float _maxY;

	private WorldMarch marchInfo;

	private int marchSize = 1;

	private Vector3 worldPosition;

	private Vector2Int tilePos;

	public Vector2Int TilePos
	{
		get
		{
			return tilePos;
		}
		set
		{
			SetTilePos1(value);
		}
	}

	public void Init(long id, GameObject go, Vector3 pos)
	{
		_minX = ScreenRangeLeft;
		_maxX = (float)Screen.width - ScreenRangeRight;
		_minY = ScreenRangeDown;
		_maxY = (float)Screen.height - ScreenRangeTop;
		uuid = id;
		gameObject = go;
		if ((bool)gameObject)
		{
			transform = gameObject.transform;
		}
		marchInfo = SceneManager.World.GetMarch(uuid);
		marchSize = GameEntry.Lua.CallWithReturn<int, string, int, string>("CSharpCallLuaInterface.GetTemplateData", "lw_world_monster", marchInfo.monsterId, "size");
		if (marchInfo != null)
		{
			EnterMoveCityState(pos);
		}
		else
		{
			SceneManager.World.UIDestroyRreCreateMarch();
		}
	}

	public void EnterMoveCityState(Vector3 pos)
	{
		SceneManager.World.SetSelectedPickable(this);
		SetWorldPos(pos);
		SetTouchPickAblePos();
		GameEntry.Lua.UIManager.OpenWindow("UIMoveMarch", uuid, SceneManager.World.TilePosToIndex(tilePos), marchSize, worldPosition);
		if (marchInfo != null && marchInfo.IsDrillBase())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateMoveDrillBase", marchInfo, transform);
		}
	}

	private void SetTilePos1(Vector2Int pos)
	{
		if (pos != tilePos)
		{
			DCPlayer dCPlayer = GameEntry.Data?.Player;
			tilePos = pos;
			transform.position = TileCoord.TileToWorld(tilePos, dCPlayer?.GetCurServerId() ?? 0);
			SetTouchPickAblePos();
		}
	}

	private void SetWorldPos(Vector3 worldPos)
	{
		Vector2Int vector2Int = SceneManager.World.WorldToTile(worldPos);
		if (vector2Int != tilePos)
		{
			tilePos = vector2Int;
			worldPosition = TileCoord.WorldToClosestGridWorld(worldPos);
			transform.position = worldPosition;
			SetTouchPickAblePos();
		}
	}

	private void SetTouchPickAblePos()
	{
		if (SceneManager.World.SelectBuild == this)
		{
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, long, int>("CSharpCallLuaInterface.GetMarchPointsBySizeAndIndex", marchSize, SceneManager.World.TilePosToIndex(tilePos));
			GameEntry.Event.Fire(EventId.UIPlaceMarchChangeWorldPos, worldPosition);
		}
	}

	public void Destroy()
	{
		if (marchInfo != null && marchInfo.IsDrillBase())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveMoveDrillBase", marchInfo, transform);
		}
		gameObject = null;
		transform = null;
		marchInfo = null;
	}

	public bool CanLongTap()
	{
		return false;
	}

	public Transform GetTransform()
	{
		if (transform != null)
		{
			return gameObject.transform;
		}
		return null;
	}

	T ITouchPickable.GetPickComponent<T>()
	{
		if (transform != null)
		{
			return transform.GetComponent<T>();
		}
		return null;
	}

	public bool PointInPick()
	{
		return false;
	}

	public void Drag(Vector3 pos)
	{
		if (SceneManager.World.GetTouchInputControllerEnable() && transform != null)
		{
			SetWorldPos(pos);
		}
	}

	public virtual bool Select()
	{
		return false;
	}

	void ITouchPickable.Click()
	{
	}

	public bool IsOutRange(Vector3 pos)
	{
		Vector3 vector = SceneManager.World.WorldToScreenPoint(pos);
		float x = vector.x;
		float y = vector.y;
		if (!(x < _minX) && !(x > _maxX) && !(y < _minY))
		{
			return y > _maxY;
		}
		return true;
	}

	public void ChangeTouchPos(int index)
	{
	}

	public Vector3 GetClosestPoint(Vector3 pos)
	{
		Vector3 worldPos = SceneManager.World.WorldToScreenPoint(pos);
		float x = worldPos.x;
		float y = worldPos.y;
		if (x < _minX)
		{
			worldPos.x = _minX;
		}
		else if (x > _maxX)
		{
			worldPos.x = _maxX;
		}
		if (y < _minY)
		{
			worldPos.y = _minY;
		}
		else if (y > _maxY)
		{
			worldPos.y = _maxY;
		}
		return SceneManager.World.ScreenPointToWorld(worldPos);
	}
}
