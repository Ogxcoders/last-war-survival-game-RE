using UnityEngine;

namespace Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowLocationRigidbody2D : MonoBehaviour
{
	public Transform reference;

	public bool followFlippedX;

	private Rigidbody2D ownRigidbody;

	private void Awake()
	{
		ownRigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (followFlippedX)
		{
			ownRigidbody.rotation = (0f - reference.rotation.eulerAngles.z + 270f) % 360f - 90f;
		}
		else
		{
			ownRigidbody.rotation = reference.rotation.eulerAngles.z;
		}
		ownRigidbody.position = reference.position;
	}
}
