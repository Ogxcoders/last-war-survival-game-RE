public class CityPeopleInitState : ITruckPeopleSate
{
	public void OnEnter(WorldPeopleTruckBase truck)
	{
		truck.gameObject.SetActive(value: false);
		truck.Manager.OnPeopleEnterInit(isEnter: true);
	}

	public void OnUpdate(WorldPeopleTruckBase truck, float deltaTime)
	{
	}

	public void OnLeave(WorldPeopleTruckBase truck)
	{
		truck.gameObject.SetActive(value: true);
		truck.Manager.OnPeopleEnterInit(isEnter: false);
	}
}
