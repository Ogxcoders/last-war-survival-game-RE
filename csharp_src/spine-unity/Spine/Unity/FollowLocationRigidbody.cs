using UnityEngine;

namespace Spine.Unity;

[RequireComponent(typeof(Rigidbody))]
public class FollowLocationRigidbody : MonoBehaviour
{
	public Transform reference;

	private Rigidbody ownRigidbody;

	private void Awake()
	{
		ownRigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		ownRigidbody.rotation = reference.rotation;
		ownRigidbody.position = reference.position;
	}
}
