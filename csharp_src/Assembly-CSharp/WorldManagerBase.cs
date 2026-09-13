public abstract class WorldManagerBase
{
	protected WorldScene world;

	public WorldManagerBase(WorldScene scene)
	{
		world = scene;
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

	public virtual string Description()
	{
		return string.Empty;
	}
}
