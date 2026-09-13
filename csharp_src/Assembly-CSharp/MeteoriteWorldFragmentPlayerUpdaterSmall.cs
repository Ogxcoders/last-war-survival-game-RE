using System;
using UnityEngine;

[Serializable]
public class MeteoriteWorldFragmentPlayerUpdaterSmall : MeteoriteWorldFragmentPlayerUpdater
{
	public Vector2 lerpToScale;

	public Vector2 randomWidth;

	private float randomScale;

	protected override void OnInit(MeteoriteWorldFragmentPlayer.DisplaySetting settings, float elapsedTimeSec)
	{
		randomScale = UnityEngine.Random.Range(randomWidth.x, randomWidth.y);
	}

	protected override void OnUpdateDrop(float deltaTime, float elapsedTime, float progress)
	{
		Transform nodeTrailScaler = player.nodeTrailScaler;
		if (!(nodeTrailScaler == null))
		{
			Vector3 one = Vector3.one;
			float num = 0.672f;
			if (progress <= num)
			{
				one.z = Mathf.Lerp(lerpToScale.x, lerpToScale.y, (num - progress) / num);
			}
			else
			{
				one.z = Mathf.Lerp(lerpToScale.x, lerpToScale.y, (progress - num) / (1f - num));
			}
			one.y = randomScale;
			nodeTrailScaler.localScale = one;
		}
	}
}
