public class WorldPlayerAttackSound : WorldTroopSoundBase
{
	protected override int soundId => 10031;

	protected override void SetType()
	{
		type = ETroopSoundType.TroopAttackingSound;
	}

	public override void UpdateSound(WorldTroopState state)
	{
		if (timer != null)
		{
			if (state != WorldTroopState.Attack)
			{
				Pause();
			}
			else if (worldTroop.IsPlayerTroop() && curLod <= 2)
			{
				Resume();
			}
		}
	}
}
