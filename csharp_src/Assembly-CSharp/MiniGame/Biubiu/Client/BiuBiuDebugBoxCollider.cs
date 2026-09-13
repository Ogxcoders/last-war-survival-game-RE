using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class BiuBiuDebugBoxCollider : MonoBehaviour
{
	private BoxCollider2D boxCollider;

	public Transform Spine3BoneTran;

	private void Info()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		Debug.Log(boxCollider.bounds.min - boxCollider.bounds.center);
		Debug.Log(boxCollider.bounds.max - boxCollider.bounds.center);
		Debug.Log(Spine3BoneTran.position.y);
	}
}
