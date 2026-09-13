public class CityManagerBase
{
	protected CityScene scene;

	public CityManagerBase(CityScene scene)
	{
		this.scene = scene;
	}

	public virtual void Init()
	{
	}

	public virtual void UnInit()
	{
	}

	public virtual void OnUpdate(float deltaTime)
	{
	}
}
