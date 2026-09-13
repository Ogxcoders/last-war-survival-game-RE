using UnityEngine.Playables;

public static class CustomPlayableExtensions
{
	public static void ResetTime(this Playable playable, float time)
	{
		playable.SetTime(time);
	}
}
