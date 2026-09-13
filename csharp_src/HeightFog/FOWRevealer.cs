using UnityEngine;

public class FOWRevealer
{
	public FOWSystem.LOSChecks lineOfSightCheck = FOWSystem.LOSChecks.OnlyOnce;

	public bool isActive = true;

	private FOWSystem.Revealer mRevealer;

	public void Init(Vector3 position, Vector2 range)
	{
		mRevealer = new FOWSystem.Revealer();
		mRevealer.pos = position;
		mRevealer.inner = range.x;
		mRevealer.outer = range.y;
		mRevealer.los = lineOfSightCheck;
		mRevealer.isActive = true;
		FOWSystem.AddRevealer(mRevealer);
	}

	public void OnDisable()
	{
		mRevealer.isActive = false;
	}

	public void OnDestroy()
	{
		FOWSystem.DeleteRevealer(mRevealer);
		mRevealer = null;
	}
}
