public class WorldTroopSoundBase
{
	protected const int MAX_LOD = 2;

	protected int curLod;

	protected LoopTimerSound timer;

	protected WorldTroop worldTroop;

	public ETroopSoundType type;

	protected virtual int soundId { get; set; }

	public virtual void Init(WorldTroop worldTroop)
	{
		SetType();
		this.worldTroop = worldTroop;
		CreateTimer();
		GameEntry.Event.Subscribe(EventId.OnSceneCameraDisableRender, OnSceneCameraDisableRender);
	}

	protected virtual void SetType()
	{
		type = ETroopSoundType.None;
	}

	public virtual void UnInit()
	{
		worldTroop = null;
		UpdateSound(WorldTroopState.None);
		DisposeTimer();
		timer = null;
		GameEntry.Event.Unsubscribe(EventId.OnSceneCameraDisableRender, OnSceneCameraDisableRender);
	}

	public virtual void CreateTimer()
	{
		timer = GameEntry.Sound.PlaySoundByIdWithLimit(soundId);
	}

	public virtual void DisposeTimer()
	{
		GameEntry.Sound.StopPlayLoopSoundWithLimit(timer);
	}

	public virtual void UpdateSound(WorldTroopState state)
	{
	}

	protected void Pause()
	{
		timer?.Pause();
	}

	protected void Resume()
	{
		timer?.Resume();
	}

	public void UpdateLOD(int Lod)
	{
		curLod = Lod;
		if (Lod > 2)
		{
			UpdateSound(WorldTroopState.None);
		}
	}

	private void OnSceneCameraDisableRender(object obj)
	{
		if ((bool)obj)
		{
			DisposeTimer();
		}
	}
}
