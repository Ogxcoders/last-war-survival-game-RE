using BitBenderGames;
using UnityEngine;

public class ModelDragView : MonoBehaviour
{
	public bool allowUserInput = true;

	public float sensitivity = 30f;

	public float dragAcceleration = 40f;

	public float dragDeceleration = 15f;

	public bool reverseControls;

	private TouchInputController touchInput;

	private Transform cachedTransform;

	private Vector2 angularVelocity = Vector2.zero;

	private Quaternion rotation;

	private bool isDragging;

	private Vector3 lastDragPos;

	private Vector3 deltaMove;

	private void Awake()
	{
		cachedTransform = base.transform;
		touchInput = GetComponent<TouchInputController>();
		touchInput.OnDragStart += OnDragStart;
		touchInput.OnDragUpdate += OnDrag;
		touchInput.OnDragStop += OnDragStop;
		rotation = cachedTransform.rotation;
	}

	private void OnDisable()
	{
		ResetRotation();
	}

	private void OnDragStart(Vector3 pos, bool isLongTap)
	{
		lastDragPos = pos;
	}

	private void OnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		isDragging = false;
	}

	private void OnDrag(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
	{
		if (allowUserInput)
		{
			isDragging = true;
			deltaMove = dragPosCurrent - lastDragPos;
			lastDragPos = dragPosCurrent;
		}
		else
		{
			isDragging = false;
		}
	}

	private void Update()
	{
		UpdateDrag();
	}

	private void UpdateDrag()
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		Vector2 b = Vector2.zero;
		float num = dragDeceleration;
		if (isDragging)
		{
			b = sensitivity * new Vector2(deltaMove.x, deltaMove.y);
			num = dragAcceleration;
		}
		angularVelocity = Vector2.Lerp(angularVelocity, b, Time.deltaTime * num);
		Vector2 vector = Time.deltaTime * angularVelocity;
		if (reverseControls)
		{
			vector = -vector;
		}
		localEulerAngles.y -= vector.x;
		base.transform.localEulerAngles = localEulerAngles;
	}

	public void ResetRotation()
	{
		cachedTransform.rotation = rotation;
	}
}
