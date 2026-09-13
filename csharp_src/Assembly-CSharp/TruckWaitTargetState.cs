using UnityEngine;

public class TruckWaitTargetState : ITruckPeopleSate
{
	private float _tempTime;

	public void OnEnter(WorldPeopleTruckBase truck)
	{
		_tempTime = 0f;
		truck.Anim2.enabled = false;
		SetTruckRotation(truck);
		truck.TruckAndPeopleMove.PauseMove();
		truck.ShowObject.SetActive(value: false);
	}

	private void SetTruckRotation(WorldPeopleTruckBase truck)
	{
		Vector2Int vector2Int = SceneManager.World.WorldToTile(truck.transform.position);
		Vector2Int vector2Int2 = SceneManager.World.IndexToTilePos(truck.targetPos);
		Vector2Int vector2Int3 = vector2Int - vector2Int2;
		if (vector2Int3 == new Vector2Int(1, 0))
		{
			Vector3 target = SceneManager.World.TileToWorld(vector2Int2 + new Vector2Int(1, 1));
			truck.transform.rotation = VehicleRotation.LookAt(truck.transform, target);
		}
		else if (vector2Int3 == new Vector2Int(0, 1))
		{
			Vector3 target2 = SceneManager.World.TileToWorld(vector2Int2 + new Vector2Int(-1, 1));
			truck.transform.rotation = VehicleRotation.LookAt(truck.transform, target2);
		}
		else if (vector2Int3 == new Vector2Int(-1, 0))
		{
			Vector3 target3 = SceneManager.World.TileToWorld(vector2Int2 + new Vector2Int(-1, -1));
			truck.transform.rotation = VehicleRotation.LookAt(truck.transform, target3);
		}
		else if (vector2Int3 == new Vector2Int(0, -1))
		{
			Vector3 target4 = SceneManager.World.TileToWorld(vector2Int2 + new Vector2Int(1, -1));
			truck.transform.rotation = VehicleRotation.LookAt(truck.transform, target4);
		}
	}

	public void OnUpdate(WorldPeopleTruckBase truck, float deltaTime)
	{
		_tempTime += deltaTime;
		int num = 1;
		if (_tempTime >= (float)num)
		{
			truck.ChangeState(WorldPeopleTruckBase.States.GotoTarget);
		}
	}

	public void OnLeave(WorldPeopleTruckBase truck)
	{
	}
}
