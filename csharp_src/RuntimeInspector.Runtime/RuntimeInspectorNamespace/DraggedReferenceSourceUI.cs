using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class DraggedReferenceSourceUI : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler
{
	[SerializeField]
	private Object[] m_references;

	[SerializeField]
	private UISkin draggedReferenceSkin;

	[SerializeField]
	private float holdTime = 0.4f;

	private IEnumerator pointerHeldCoroutine;

	public Object[] References
	{
		get
		{
			return m_references;
		}
		set
		{
			m_references = value;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (pointerHeldCoroutine == null && !m_references.IsEmpty())
		{
			pointerHeldCoroutine = CreateReferenceItemCoroutine(eventData);
			StartCoroutine(pointerHeldCoroutine);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (pointerHeldCoroutine != null)
		{
			StopCoroutine(pointerHeldCoroutine);
			pointerHeldCoroutine = null;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (pointerHeldCoroutine != null)
		{
			StopCoroutine(pointerHeldCoroutine);
			pointerHeldCoroutine = null;
		}
	}

	private IEnumerator CreateReferenceItemCoroutine(PointerEventData eventData)
	{
		Vector2 pressPosition = eventData.pressPosition;
		float dragThreshold = EventSystem.current.pixelDragThreshold;
		yield return new WaitForSecondsRealtime(holdTime);
		if (!m_references.IsEmpty() && (eventData.position - pressPosition).sqrMagnitude < dragThreshold * dragThreshold)
		{
			RuntimeInspectorUtils.CreateDraggedReferenceItem(m_references, eventData, draggedReferenceSkin, GetComponentInParent<Canvas>());
		}
	}
}
