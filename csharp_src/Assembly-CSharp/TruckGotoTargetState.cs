using UnityEngine;

public class TruckGotoTargetState : ITruckPeopleSate
{
	private bool getPathFinish;

	public void OnEnter(WorldPeopleTruckBase truck)
	{
		truck.TruckAndPeopleMove.GoToTarget(truck.GetNowPath());
		truck.ShowObject.SetActive(value: true);
		truck.Anim2.enabled = true;
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
				truck.ChangeState(WorldPeopleTruckBase.States.Init);
			}
			else
			{
				getPathFinish = false;
				truck.ChangeState(WorldPeopleTruckBase.States.WaitTarget);
			}
		}
	}

	public void OnLeave(WorldPeopleTruckBase truck)
	{
	}
}
