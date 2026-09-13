using UnityEngine;

namespace BitBenderGames;

[RequireComponent(typeof(MobileTouchCamera))]
public class SetBoundaryFromCollider : MonoBehaviour
{
	[SerializeField]
	private BoxCollider boxCollider;

	public void Start()
	{
		if (boxCollider == null)
		{
			Debug.LogError("This script requires a box collider to be assigned.");
			return;
		}
		GetComponent<MobileTouchCamera>();
		_ = boxCollider.bounds.min;
		_ = boxCollider.bounds.max;
	}
}
