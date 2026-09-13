using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

[RequireComponent(typeof(Camera))]
public class DraggedReferenceSourceCamera : MonoBehaviour
{
	public delegate Object RaycastHitProcesserDelegate(RaycastHit hit);

	private Camera _camera;

	[SerializeField]
	private UISkin draggedReferenceSkin;

	[SerializeField]
	private Canvas draggedReferenceCanvas;

	[SerializeField]
	private float holdTime = 0.4f;

	[SerializeField]
	private LayerMask interactableObjectsMask = -1;

	[SerializeField]
	private float raycastRange = float.MaxValue;

	private bool pointerDown;

	private float pointerDownTime;

	private Vector2 pointerDownPos;

	private Object hitObject;

	private DraggedReferenceItem draggedReference;

	private PointerEventData draggingPointer;

	private readonly List<RaycastResult> hoveredUIElements = new List<RaycastResult>(4);

	public RaycastHitProcesserDelegate ProcessRaycastHit;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void Update()
	{
		if (draggingPointer != null)
		{
			if (!draggedReference || !draggedReference.gameObject.activeSelf)
			{
				draggingPointer = null;
				return;
			}
			if (IsPointerHeld())
			{
				draggingPointer.position = GetPointerPosition();
				ExecuteEvents.Execute(draggedReference.gameObject, draggingPointer, ExecuteEvents.dragHandler);
				return;
			}
			ExecuteEvents.Execute(draggedReference.gameObject, draggingPointer, ExecuteEvents.endDragHandler);
			if (EventSystem.current != null)
			{
				hoveredUIElements.Clear();
				EventSystem.current.RaycastAll(draggingPointer, hoveredUIElements);
				for (int i = 0; i < hoveredUIElements.Count && !ExecuteEvents.ExecuteHierarchy(hoveredUIElements[i].gameObject, draggingPointer, ExecuteEvents.dropHandler); i++)
				{
				}
			}
			draggingPointer = null;
		}
		else if (!pointerDown)
		{
			if (IsPointerDown() && (bool)EventSystem.current && !EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(_camera.ScreenPointToRay(GetPointerPosition()), out var hitInfo, raycastRange, interactableObjectsMask))
			{
				hitObject = ((ProcessRaycastHit != null) ? ProcessRaycastHit(hitInfo) : hitInfo.collider.gameObject);
				if ((bool)hitObject)
				{
					pointerDown = true;
					pointerDownTime = Time.realtimeSinceStartup;
					pointerDownPos = GetPointerPosition();
				}
			}
		}
		else if (IsPointerHeld())
		{
			if ((GetPointerPosition() - pointerDownPos).sqrMagnitude >= 100f)
			{
				pointerDown = false;
			}
			else
			{
				if (!(Time.realtimeSinceStartup - pointerDownTime >= holdTime))
				{
					return;
				}
				pointerDown = false;
				if ((bool)hitObject && (bool)EventSystem.current)
				{
					draggingPointer = new PointerEventData(EventSystem.current)
					{
						pointerId = ((Input.touchCount > 0) ? Input.GetTouch(0).fingerId : (-1)),
						pressPosition = GetPointerPosition(),
						position = GetPointerPosition(),
						button = PointerEventData.InputButton.Left
					};
					draggedReference = RuntimeInspectorUtils.CreateDraggedReferenceItem(hitObject, draggingPointer, draggedReferenceSkin, draggedReferenceCanvas);
					if (!draggedReference)
					{
						pointerDown = false;
						draggingPointer = null;
					}
				}
			}
		}
		else
		{
			pointerDown = false;
		}
	}

	private bool IsPointerDown()
	{
		return Input.GetMouseButtonDown(0);
	}

	private bool IsPointerHeld()
	{
		return Input.GetMouseButton(0);
	}

	private Vector2 GetPointerPosition()
	{
		return Input.mousePosition;
	}
}
