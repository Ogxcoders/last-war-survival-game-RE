using UnityEngine;

namespace PVEBattleLogic.Bullet;

public class BulletStraightGatlingViewParam
{
	public string viewName;

	public int targetLayerMask;

	public Vector3 inertiaVelocity;

	public float lifeTime;

	public float bulletScale;

	public float metaColliderRadius;

	public bool noCollision;

	public float duration;

	public float spiralLoops;

	public float spiralRadius;

	public float flySpeed;

	public AnimationCurve animationCurve;

	public bool colliderType;

	public float defaultDotMaxCD;
}
