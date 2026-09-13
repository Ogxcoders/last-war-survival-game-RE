using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
public class ControllerColliderHit
{
	internal CharacterController m_Controller;

	internal Collider m_Collider;

	internal Vector3 m_Point;

	internal Vector3 m_Normal;

	internal Vector3 m_MoveDirection;

	internal float m_MoveLength;

	internal int m_Push;

	public CharacterController controller => m_Controller;

	public Collider collider => m_Collider;

	public Rigidbody rigidbody => m_Collider.attachedRigidbody;

	public GameObject gameObject => m_Collider.gameObject;

	public Transform transform => m_Collider.transform;

	public Vector3 point => m_Point;

	public Vector3 normal => m_Normal;

	public Vector3 moveDirection => m_MoveDirection;

	public float moveLength => m_MoveLength;

	private bool push
	{
		get
		{
			return m_Push != 0;
		}
		set
		{
			m_Push = (value ? 1 : 0);
		}
	}
}
