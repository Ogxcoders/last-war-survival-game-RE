public class WorldPlayerTroopSound : WorldTroopSoundBase
{
	protected override int soundId => 10030;

	protected override void SetType()
	{
		type = ETroopSoundType.TroopMarchingSound;
	}

	public override void UpdateSound(WorldTroopState state)
	{
		if (timer != null)
		{
			if (state != WorldTroopState.Move)
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
