using UnityEngine;

namespace BitBenderGames;

public class MonoBehaviourWrapped : MonoBehaviour
{
	protected Transform cachedTransform;

	protected GameObject cachedGO;

	public Transform Transform
	{
		get
		{
			if (cachedTransform == null)
			{
				cachedTransform = base.transform;
			}
			return cachedTransform;
		}
	}

	public GameObject GameObject
	{
		get
		{
			if (cachedGO == null)
			{
				cachedGO = base.gameObject;
			}
			return cachedGO;
		}
	}
}
