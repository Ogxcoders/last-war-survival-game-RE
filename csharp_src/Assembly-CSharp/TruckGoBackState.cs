public class TruckGoBackState : ITruckPeopleSate
{
	private bool getPathFinish;

	public void OnEnter(WorldPeopleTruckBase truck)
	{
		truck.TruckAndPeopleMove.GoBack(truck.pathList[0]);
		truck.Anim2.enabled = true;
		getPathFinish = true;
	}

	public void OnUpdate(WorldPeopleTruckBase truck, float deltaTime)
	{
		if (getPathFinish)
		{
			truck.TruckAndPeopleMove.ResumeMove();
			if (truck.TruckAndPeopleMove.UpdateMove(deltaTime) == TruckAndPeopleMove.MoveUpdateResult.FINISH_PATH)
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
