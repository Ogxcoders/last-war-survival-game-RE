using UnityEngine;

public class MeteoriteWorldFragmentPlayer : MonoBehaviour
{
	public struct DisplaySetting
	{
		public Vector3 target;

		public float height;

		public float degree;

		public float dropTimeSec;

		public float exploreTimeSec;

		public float crackTimeSec;

		public Vector3 scale;

		public float rotateSpeed;
	}

	public Transform nodeDrop;

	[SerializeField]
	public Transform nodeRotator;

	[SerializeField]
	public Transform nodeScaler;

	[SerializeField]
	public Transform nodeTrailScaler;

	[SerializeField]
	public TrailRenderer trailRenderer;

	public Vector3 rotateAxis;

	public Transform nodeExplore;

	public ParticleSystem particleHitEffect;

	public Transform nodeCrack;

	public ParticleSystem particleCrackEffect;

	[SerializeReference]
	private MeteoriteWorldFragmentPlayerUpdater updater;

	public bool CanRotate => nodeRotator != null;

	public void Init(DisplaySetting settings, float elapsedTimeSec)
	{
		updater?.Init(this, settings, elapsedTimeSec);
	}

	public void DeltaUpdate(float deltaTime)
	{
		updater?.DeltaUpdate(deltaTime);
	}
}
