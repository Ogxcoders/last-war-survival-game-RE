using System.Collections.Generic;
using UnityEngine;

public class MovableTrigger : ITouchPickable
{
	public int cfgId;

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

	private Vector3 worldPos;

	private Vector2Int tilePos;

	public Vector2Int TilePos => tilePos;

	public void Init(int theServerId, int cfgId, GameObject go, int pointId, int skillId, Vector3 _worldPos)
	{
		_minX = ScreenRangeLeft;
		_maxX = (float)Screen.width - ScreenRangeRight;
		_minY = ScreenRangeDown;
		_maxY = (float)Screen.height - ScreenRangeTop;
		this.cfgId = cfgId;
		gameObject = go;
		if ((bool)gameObject)
		{
			transform = gameObject.transform;
		}
		EnterMoveCityState(theServerId, pointId, skillId, _worldPos);
	}

	public void EnterMoveCityState(int theServerId, int pointId, int skillId, Vector3 _worldPos)
	{
		SceneManager.World.SetSelectedPickable(this);
		SetTilePos1(theServerId, SceneManager.World.IndexToTilePos(pointId), _worldPos);
		SetTouchPickAblePos();
		GameEntry.Lua.UIManager.OpenWindow("UIPlaceTrigger", cfgId, pointId, size, skillId, theServerId);
	}

	private void SetTilePos1(int theServerId, Vector2Int pos, Vector3? _worldPos = null)
	{
		if (pos != tilePos)
		{
			tilePos = pos;
			if (_worldPos.HasValue)
			{
				worldPos = TileCoord.WorldToClosestGridWorld(_worldPos.Value);
			}
			else
			{
				worldPos = TileCoord.TileToWorld(tilePos, theServerId);
			}
			transform.position = worldPos;
			SetTouchPickAblePos();
		}
	}

	private void SetTouchPickAblePos()
	{
		if (SceneManager.World.SelectBuild == this)
		{
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, long, int>("CSharpCallLuaInterface.GetMarchPointsBySizeAndIndex", size, SceneManager.World.TilePosToIndex(tilePos));
			GameEntry.Event.Fire(EventId.UIPlaceTriggerChangePos, worldPos);
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
			int serverIdFromWorldPos = SeasonDataManager.Instance.GetServerIdFromWorldPos(pos);
			SetTilePos1(serverIdFromWorldPos, SceneManager.World.WorldToTile(pos), pos);
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
		Vector3 vector = SceneManager.World.WorldToScreenPoint(pos);
		float x = vector.x;
		float y = vector.y;
		if (x < _minX)
		{
			vector.x = _minX;
		}
		else if (x > _maxX)
		{
			vector.x = _maxX;
		}
		if (y < _minY)
		{
			vector.y = _minY;
		}
		else if (y > _maxY)
		{
			vector.y = _maxY;
		}
		return SceneManager.World.ScreenPointToWorld(vector);
	}
}
