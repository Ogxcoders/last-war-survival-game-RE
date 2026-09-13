using System.Collections.Generic;
using UnityEngine;

public class WorldMovingModel : ITouchPickable
{
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

	private int size = 1;

	private FakeMovingModelFlag flag;

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

	public void Init(GameObject go, FakeMovingModelFlag flag, int pointId, int size)
	{
		_minX = ScreenRangeLeft;
		_maxX = (float)Screen.width - ScreenRangeRight;
		_minY = ScreenRangeDown;
		_maxY = (float)Screen.height - ScreenRangeTop;
		gameObject = go;
		if ((bool)gameObject)
		{
			transform = gameObject.transform;
		}
		this.flag = flag;
		this.size = size;
		EnterMoveModelState(pointId);
	}

	public void EnterMoveModelState(int pointId)
	{
		SceneManager.World.SetSelectedPickable(this);
		SetTilePos1(SceneManager.World.IndexToTilePos(pointId));
		SetTouchPickAblePos();
		GameEntry.Lua.UIManager.OpenWindow("UIMoveModel", (int)flag, pointId, size);
	}

	private void SetTilePos1(Vector2Int pos)
	{
		if (!(pos == tilePos))
		{
			DCPlayer dCPlayer = GameEntry.Data?.Player;
			tilePos = pos;
			transform.position = TileCoord.TileToWorld(tilePos, dCPlayer?.GetCurServerId() ?? 0);
			SetTouchPickAblePos();
		}
	}

	private void SetTouchPickAblePos()
	{
		if (SceneManager.World.SelectBuild == this)
		{
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, long, int>("CSharpCallLuaInterface.GetMarchPointsBySizeAndIndex", size, SceneManager.World.TilePosToIndex(tilePos));
			GameEntry.Event.Fire(EventId.UIPlaceMarchChangePos, SceneManager.World.TilePosToIndex(tilePos));
		}
	}

	public void Destroy()
	{
		gameObject = null;
		transform = null;
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
			SetTilePos1(SceneManager.World.WorldToTile(pos));
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
