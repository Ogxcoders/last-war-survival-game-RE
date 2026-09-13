using System;
using UnityEngine;

[Serializable]
public class MeteoriteWorldFragmentPlayerUpdaterMiddle : MeteoriteWorldFragmentPlayerUpdater
{
	public float trailTime;

	public float trailMinTime;

	protected override void OnUpdateDrop(float deltaTime, float elapsedTime, float progress)
	{
		base.OnUpdateDrop(deltaTime, elapsedTime, progress);
		TrailRenderer trailRenderer = player.trailRenderer;
		if (!(trailRenderer == null))
		{
			trailRenderer.time = Mathf.Lerp(trailTime, trailMinTime, progress);
		}
	}
}
