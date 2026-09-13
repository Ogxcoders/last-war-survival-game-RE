using UnityEngine;

public class CityTroopMoveState : BaseCityTroopState
{
	private float _curTime;

	private float _allTime;

	private const float MoveSpeed = 5f;

	private const float StartRotationTime = 0.5f;

	private const float EndRotationTime = 0.5f;

	private const float FogMoveInTime = 1.1f;

	private const float truckSpeed = 1f;

	private float _realEndRotationTime;

	private Vector3 _endPos;

	private Vector3 _startPos;

	private Vector3 _pos;

	private float _guideDisLeft;

	private bool _isInGuide;

	public CityTroopMoveState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		_pos = Vector3.zero;
		_guideDisLeft = 0f;
		_endPos = cityTroop.GetRealEndPos();
		_curTime = 0f;
		_startPos = cityTroop.transform.position;
		cityTroop.ClearTroopUnits();
		cityTroop.OnCreateMoveLine();
		_allTime = cityTroop.EnterWorkMove(5f);
		_allTime += 0.5f;
		if (cityTroop.IsWorkmanPickGarbageTroop())
		{
			_allTime += 0.5f;
			_realEndRotationTime = 0.5f;
		}
		else
		{
			_realEndRotationTime = 0f;
		}
		cityTroop.PlayAnim("run");
		_isInGuide = CheckMoveGuide();
	}

	public override void OnUpdate(float deltaTime)
	{
		_curTime += deltaTime;
		float num = Vector3.Distance(cityTroop.transform.position, _endPos);
		int num2 = SceneManager.World.WorldToTileIndex(_endPos);
		int pointType = SceneManager.World.GetPointType(num2);
		if (num <= _guideDisLeft && _isInGuide)
		{
			GameEntry.Lua.Call("DataCenter.GuideManager:DoNext");
			cityTroop.OnMoveLineStop();
			cityTroop.transform.position = _endPos;
			GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", num2);
			machine.ChangeState(TroopState.Idle);
		}
		if (cityTroop.IsTruckPickGarbageTroop() && num <= cityTroop.GetPickGarbageMoveDistance())
		{
			cityTroop.OnMoveLineStop();
			GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", num2);
			if (GameEntry.Data.Fog.IsUnlock(num2))
			{
				machine.ChangeState(TroopState.CityTruckPickGarbageMovetoGarbage);
			}
		}
		if (cityTroop.IsTruck() && pointType == 2 && num <= cityTroop.GetPickGarbageMoveDistance() + 2f)
		{
			cityTroop.OnMoveLineStop();
			GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", num2);
			machine.ChangeState(TroopState.Fight);
		}
		else if (_curTime >= _allTime)
		{
			cityTroop.OnMoveLineStop();
			cityTroop.transform.position = _endPos;
			GameEntry.Setting.SetPrivateInt("CITY_TROOP_POSITION", num2);
			TroopState state = TroopState.Idle;
			if (!GameEntry.Data.Fog.IsUnlock(num2))
			{
				cityTroop.OpenFogPointIndex = num2;
				state = TroopState.OpenFog;
			}
			else
			{
				switch (pointType)
				{
				case 1:
					state = TroopState.WorkGarbage;
					break;
				case 2:
					state = TroopState.Fight;
					break;
				}
			}
			machine.ChangeState(state);
		}
		else if (_curTime < 0.5f)
		{
			cityTroop.MoveStartRotation(_curTime / 0.5f);
		}
		else if (_curTime < _allTime - _realEndRotationTime)
		{
			Vector3 vector = Vector3.Lerp(_startPos, _endPos, (_curTime - 0.5f) / (_allTime - 0.5f - _realEndRotationTime));
			cityTroop.transform.position = vector;
			cityTroop.Move(_curTime - 0.5f);
			int pointId = SceneManager.World.WorldToTileIndex(vector);
			if (GameEntry.Data.Fog.IsUnlock(pointId))
			{
				Vector3 pos = Vector3.Lerp(_startPos, _endPos, (_curTime - 0.5f + 1.1f) / (_allTime - 0.5f - _realEndRotationTime));
				int pointId2 = SceneManager.World.WorldToTileIndex(pos);
				if (GameEntry.Data.Fog.IsUnlock(pointId2))
				{
					cityTroop.OnMoveLineUpdate();
				}
			}
		}
		else if (cityTroop.IsWorkmanPickGarbageTroop())
		{
			cityTroop.MoveEndRotation((0.5f - (_allTime - _curTime)) / 0.5f);
		}
	}

	public override void OnLeave()
	{
		_isInGuide = false;
		_guideDisLeft = 0f;
	}

	public override void OnBeginDrag()
	{
		if (!_isInGuide)
		{
			SceneManager.World.CanMoving = false;
			cityTroop.OnCreateDragLine();
		}
	}

	public override void OnDrag()
	{
		if (!_isInGuide)
		{
			SceneManager.World.CanMoving = false;
			Vector3 touchPoint = SceneManager.World.GetTouchPoint();
			if (_pos != touchPoint)
			{
				cityTroop.OnDragLineUpdate(touchPoint);
			}
		}
	}

	public override void OnEndDrag()
	{
		if (!_isInGuide)
		{
			SceneManager.World.CanMoving = true;
			cityTroop.OnDragLineStop();
			Vector3 vector = CheckCanMovePosByPos(SceneManager.World.GetTouchPoint());
			if (vector != Vector3.zero && !cityTroop.DealGarbageQueueDataWhenDragEnd(vector))
			{
				cityTroop.EndPos = vector;
				OnEnter();
			}
		}
	}

	public override void CityTroopMoveSignal(Vector3 pos1)
	{
		Vector3 vector = CheckCanMovePosByPos(pos1);
		if (vector != Vector3.zero && !cityTroop.DealGarbageQueueDataWhenDragEnd(vector))
		{
			cityTroop.EndPos = vector;
			OnEnter();
		}
	}

	private bool CheckMoveGuide()
	{
		switch (GameEntry.Lua.CallWithReturn<int>("DataCenter.GuideManager:GetGuideType"))
		{
		case 22:
		{
			string text2 = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para1");
			if (string.IsNullOrEmpty(text2))
			{
				break;
			}
			string[] array2 = text2.Split(new char[1] { ',' });
			if (array2.Length > 1)
			{
				int num2 = SceneManager.World.TilePosToIndex(GameEntry.Data.Building.GetMainPos() + new Vector2Int(array2[0].ToInt(), array2[1].ToInt()));
				string text3 = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para2");
				if (!string.IsNullOrEmpty(text3))
				{
					_guideDisLeft = text3.ToFloat();
				}
				if (SceneManager.World.WorldToTileIndex(_endPos) == num2)
				{
					return true;
				}
			}
			break;
		}
		case 21:
		{
			string text = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para1");
			if (string.IsNullOrEmpty(text))
			{
				break;
			}
			string[] array = text.Split(new char[1] { ',' });
			if (array.Length > 1)
			{
				int num = SceneManager.World.TilePosToIndex(GameEntry.Data.Building.GetMainPos() + new Vector2Int(array[0].ToInt(), array[1].ToInt()));
				if (SceneManager.World.WorldToTileIndex(_endPos) == num)
				{
					return true;
				}
			}
			break;
		}
		}
		return false;
	}
}
