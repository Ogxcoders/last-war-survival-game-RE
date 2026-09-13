using UnityEngine;

public class CityTroopIdleState : BaseCityTroopState
{
	private Vector3 _pos;

	public CityTroopIdleState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		_pos = Vector3.zero;
		cityTroop.EnterWorkIdle();
		cityTroop.PlayAnim("idle");
		CheckMoveGuide(cityTroop.transform.position);
	}

	public override void OnUpdate(float deltaTime)
	{
	}

	public override void OnLeave()
	{
	}

	public override void OnBeginDrag()
	{
		SceneManager.World.CanMoving = false;
		cityTroop.OnCreateDragLine();
	}

	public override void OnDrag()
	{
		SceneManager.World.CanMoving = false;
		Vector3 touchPoint = SceneManager.World.GetTouchPoint();
		if (_pos != touchPoint)
		{
			_pos = touchPoint;
			cityTroop.OnDragLineUpdate(touchPoint);
		}
	}

	public override void OnEndDrag()
	{
		SceneManager.World.CanMoving = true;
		cityTroop.OnDragLineStop();
		Vector3 vector = CheckCanMovePosByPos(SceneManager.World.GetTouchPoint());
		if (vector != Vector3.zero)
		{
			Vector3 vector2 = CheckDragGuide(vector);
			if (vector2 != Vector3.zero)
			{
				cityTroop.EndPos = vector2;
				cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
				machine.ChangeState(TroopState.Move);
			}
		}
	}

	public override void CityTroopMoveSignal(Vector3 pos1)
	{
		Vector3 vector = CheckCanMovePosByPos(pos1);
		if (vector != Vector3.zero)
		{
			cityTroop.EndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			machine.ChangeState(TroopState.Move);
			CheckSaveEndPos();
		}
	}

	private Vector3 CheckDragGuide(Vector3 pos)
	{
		if (GameEntry.Lua.CallWithReturn<int>("DataCenter.GuideManager:GetGuideType") == 16)
		{
			string text = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para1");
			if (!string.IsNullOrEmpty(text))
			{
				Vector2Int vector2Int = SceneManager.World.WorldToTile(pos);
				if (GameEntry.Lua.CallWithReturn<bool, Vector2Int, string>("GuideManager.CheckCanDragGuide", vector2Int, text))
				{
					GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", SceneManager.World.TilePosToIndex(vector2Int));
					GameEntry.Lua.Call("DataCenter.GuideManager:DoNext");
					return SceneManager.World.TileToWorld(vector2Int);
				}
				return Vector3.zero;
			}
		}
		return pos;
	}

	private bool CheckMoveGuide(Vector3 pos)
	{
		switch (GameEntry.Lua.CallWithReturn<int>("DataCenter.GuideManager:GetGuideType"))
		{
		case 21:
		{
			string text3 = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para1");
			if (string.IsNullOrEmpty(text3))
			{
				break;
			}
			string[] array2 = text3.Split(new char[1] { ',' });
			if (array2.Length > 1)
			{
				int num2 = SceneManager.World.TilePosToIndex(GameEntry.Data.Building.GetMainPos() + new Vector2Int(array2[0].ToInt(), array2[1].ToInt()));
				if (SceneManager.World.WorldToTileIndex(pos) != num2)
				{
					return false;
				}
				int param = GameEntry.Lua.CallWithReturn<int, string>("DataCenter.GuideManager:GetGuideTemplateParam", "nextid");
				GameEntry.Lua.Call("DataCenter.GuideManager:SetCurGuideId", param);
				GameEntry.Lua.Call("DataCenter.GuideManager:DoGuide");
			}
			break;
		}
		case 22:
		{
			string text = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para1");
			if (string.IsNullOrEmpty(text))
			{
				break;
			}
			string[] array = text.Split(new char[1] { ',' });
			if (array.Length > 1)
			{
				Vector3 b = SceneManager.World.TileToWorld(GameEntry.Data.Building.GetMainPos() + new Vector2Int(array[0].ToInt(), array[1].ToInt()));
				float num = 0f;
				string text2 = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para2");
				if (!string.IsNullOrEmpty(text2))
				{
					num = text2.ToFloat();
				}
				if (!(Vector3.Distance(pos, b) <= num))
				{
					return false;
				}
				GameEntry.Lua.Call("DataCenter.GuideManager:DoNext");
			}
			break;
		}
		}
		return true;
	}

	private void CheckSaveEndPos()
	{
		if (GameEntry.Lua.CallWithReturn<int>("DataCenter.GuideManager:GetGuideType") != 21)
		{
			return;
		}
		string text = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para1");
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(new char[1] { ',' });
			if (array.Length > 1)
			{
				int value = SceneManager.World.TilePosToIndex(GameEntry.Data.Building.GetMainPos() + new Vector2Int(array[0].ToInt(), array[1].ToInt()));
				GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", value);
			}
		}
	}
}
