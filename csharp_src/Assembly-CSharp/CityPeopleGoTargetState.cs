using UnityEngine;

public class CityPeopleGoTargetState : ITruckPeopleSate
{
	private static readonly string Run = "run";

	private static readonly string Idle = "idle";

	private static readonly string Walk = "walk";

	private bool getPathFinish;

	public void OnEnter(WorldPeopleTruckBase truck)
	{
		truck.TruckAndPeopleMove.GoToTarget(truck.GetNowPath());
		truck.ShowObject.SetActive(value: true);
		if (truck.Anim2 == null)
		{
			Debug.LogError(truck.gameObject.name + " not find Component:<SimpleAnimation>");
			return;
		}
		truck.Anim2.enabled = true;
		WorldCityPeople worldCityPeople = truck as WorldCityPeople;
		if (worldCityPeople.CurCityPeopleType == WorldCityPeople.CityPeopleType.RoundBase)
		{
			worldCityPeople.PlayAnim(Run);
		}
		else
		{
			worldCityPeople.PlayAnim(Run);
		}
		Transform transform;
		(transform = truck.transform).position = truck.TruckAndPeopleMove.GetPathLastPoint();
		truck.transform.rotation = VehicleRotation.LookAt(transform, truck.TruckAndPeopleMove.GetPathNextPoint());
		getPathFinish = true;
		truck.TruckAndPeopleMove.ResumeMove();
	}

	public void OnUpdate(WorldPeopleTruckBase truck, float deltaTime)
	{
		if (!getPathFinish)
		{
			return;
		}
		TruckAndPeopleMove.MoveUpdateResult moveUpdateResult = truck.TruckAndPeopleMove.UpdateMove(deltaTime);
		if (moveUpdateResult != TruckAndPeopleMove.MoveUpdateResult.MOVING && moveUpdateResult == TruckAndPeopleMove.MoveUpdateResult.FINISH_PATH)
		{
			truck.ReachTarget();
			if (truck.IsPathFinish())
			{
				getPathFinish = false;
				WorldCityPeople worldCityPeople = truck as WorldCityPeople;
				truck.ChangeState(WorldPeopleTruckBase.States.Init);
				GameEntry.Event.Fire(EventId.CitySolderHied, worldCityPeople.index);
			}
			else
			{
				getPathFinish = false;
				truck.ChangeState(WorldPeopleTruckBase.States.Init);
			}
		}
	}

	public void OnLeave(WorldPeopleTruckBase truck)
	{
	}
}
