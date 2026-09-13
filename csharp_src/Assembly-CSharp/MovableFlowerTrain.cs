using System.Collections.Generic;
using UnityEngine;

public class MovableFlowerTrain : ITouchPickable
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

	private int size = 3;

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

	public void Init(GameObject go, int pointId, int goodsId)
	{
		_minX = ScreenRangeLeft;
		_maxX = (float)Screen.width - ScreenRangeRight;
		_minY = ScreenRangeDown;
		_maxY = (float)Screen.height - ScreenRangeTop;
		gameObject = go;
		if ((bool)gameObject)
		{
			transform = gameObject.transform;
			transform.localRotation = Quaternion.Euler(0f, -145f, 0f);
		}
		EnterMoveState(pointId, goodsId);
	}

	public void EnterMoveState(int pointId, int goodsId)
	{
		SceneManager.World.SetSelectedPickable(this);
		SetTilePos1(SceneManager.World.IndexToTilePos(pointId));
		SetTouchPickAblePos();
		GameEntry.Lua.UIManager.OpenWindow("UIPlaceFlowerTrain", pointId, goodsId);
	}

	private void SetTilePos1(Vector2Int pos)
	{
		if (pos != tilePos)
		{
			int serverId = GameEntry.Data?.Player?.GetCurServerId() ?? 0;
			tilePos = pos;
			Vector3 vector = new Vector3(-0.2f, 0f, -0.8f);
			transform.position = TileCoord.TileToWorld(tilePos, serverId) + vector;
			SetTouchPickAblePos();
		}
	}

	private void SetTouchPickAblePos()
	{
		if (SceneManager.World.SelectBuild == this)
		{
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, long, int>("CSharpCallLuaInterface.GetMarchPointsBySizeAndIndex", size, SceneManager.World.TilePosToIndex(tilePos));
			GameEntry.Event.Fire(EventId.UIPlaceFlowerTrainChangePos, SceneManager.World.TilePosToIndex(tilePos));
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
