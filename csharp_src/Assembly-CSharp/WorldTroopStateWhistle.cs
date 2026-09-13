using Unity.Mathematics;

public class WorldTroopStateWhistle : WorldTroopStateBase
{
	private const string WhistleVFX = "Assets/Main/SeasonRes/S4/Prefabs/World/VFX_march_whistle.prefab";

	private int vfx;

	private float duration;

	public WorldTroopStateWhistle(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			base.OnStateEnter();
			vfx = SceneManager.World.CreateVFX("Assets/Main/SeasonRes/S4/Prefabs/World/VFX_march_whistle.prefab", worldTroop.GetPosition(), 3f);
			duration = math.max(worldTroop.DelayApplyTime + 0.5f, 0.5f);
		}
	}

	public override void OnStateLeave()
	{
		duration = 0f;
		SceneManager.World.RemoveVFX(vfx);
		base.OnStateLeave();
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop != null && duration > 0f)
		{
			duration -= deltaTime;
			if (duration <= 0f)
			{
				ChangeState(WorldTroopState.Move);
			}
		}
	}
}
