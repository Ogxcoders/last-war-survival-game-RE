namespace Joker.Client;

public class GameClient : World, IGameClient, IInitializeSync, IStartupSync, IShutdownSync, IUpdate, ILateUpdate, IFixedUpdate, IDisplay
{
	protected LifeCycleGroup<IGameManager> _managers = new LifeCycleGroup<IGameManager>();

	protected LifeCycleGroup<IGameData> _datas = new LifeCycleGroup<IGameData>();

	protected virtual void InitSystems()
	{
	}

	protected virtual void InitManagers()
	{
		AddManager(new GameTickManager(this));
		AddManager(new GameStateManager());
	}

	protected virtual void InitDatas()
	{
	}

	public void AddManager<M>(M manager) where M : IGameManager
	{
		_managers.Add(manager);
	}

	public M GetManager<M>() where M : IGameManager
	{
		return _managers.Get<M>();
	}

	public void AddData<D>(D data) where D : IGameData, new()
	{
		_datas.Add(data);
	}

	public M GetData<M>() where M : IGameData
	{
		return _datas.Get<M>();
	}

	public virtual void Initialize()
	{
		InitSystems();
		InitManagers();
		InitDatas();
		_managers.Initialize();
		_datas.Initialize();
	}

	public virtual void Startup()
	{
		_managers.Startup();
		_datas.Startup();
	}

	public virtual void Shutdown()
	{
		_managers.Shutdown();
		_datas.Shutdown();
	}

	public virtual void Update()
	{
		_managers.Update();
		_datas.Update();
	}

	public virtual void LateUpdate()
	{
		_managers.LateUpdate();
		_datas.LateUpdate();
	}

	public virtual void FixedUpdate()
	{
		_managers.FixedUpdate();
		_datas.FixedUpdate();
	}

	public virtual void Display()
	{
		_managers.Display();
		_datas.Display();
	}
}
