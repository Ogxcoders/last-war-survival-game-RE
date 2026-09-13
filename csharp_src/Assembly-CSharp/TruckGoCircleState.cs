public class TruckGoCircleState : ITruckPeopleSate
{
	public void OnEnter(WorldPeopleTruckBase truck)
	{
		bool isInner = truck.isInner;
		truck.WorldTruckCircleMove.InitCircle(truck.mainPos, truck.radius + (isInner ? (-0.35f) : 0.35f), truck.initAngle, isInner);
		truck.ShowObject.SetActive(value: true);
		truck.Anim2.enabled = true;
	}

	public void OnUpdate(WorldPeopleTruckBase truck, float deltaTime)
	{
		truck.WorldTruckCircleMove.UpdateMove(deltaTime);
	}

	public void OnLeave(WorldPeopleTruckBase truck)
	{
	}
}
