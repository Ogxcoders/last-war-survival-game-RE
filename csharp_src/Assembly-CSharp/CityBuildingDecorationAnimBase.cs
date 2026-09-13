public abstract class CityBuildingDecorationAnimBase
{
	public enum StateType
	{
		Continue = 1,
		Finish
	}

	protected float timer;

	protected int skinId;

	protected CityBuilding building;

	public abstract void Start();

	public abstract StateType Update(float deltaTime);

	protected CityBuildingDecorationAnimBase(CityBuilding building, int skinId)
	{
		this.skinId = skinId;
		this.building = building;
		timer = 0f;
	}

	public virtual void Dispose()
	{
		skinId = 0;
		timer = 0f;
	}
}
